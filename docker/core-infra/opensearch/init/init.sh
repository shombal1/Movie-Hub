#!/bin/sh

USER="admin"
PASS="${OPENSEARCH_ADMIN_PASSWORD}"
HOST="${OPENSEARCH_HOST}:${OPENSEARCH_PORT}"
INDEX_NAME="${OPENSEARCH_INDEX_NAME:-movie-hub}"

echo "Checking if index '$INDEX_NAME' exists..."

index_check=$(curl -u "$USER:$PASS" \
    --connect-timeout 10 \
    --max-time 30 \
    -s \
    -o /dev/null \
    -w "%{http_code}" \
    -XGET "http://$HOST/$INDEX_NAME" 2>/dev/null)

echo "HTTP response code: $index_check"

case "$index_check" in
    200)
        echo "Index '$INDEX_NAME' already exists, skipping initialization."
        exit 0
        ;;
    404)
        echo "Index '$INDEX_NAME' does not exist, proceeding with initialization."
        ;;
    *)
        echo "Unexpected response code: $index_check. Checking OpenSearch status..."
        exit 1
        ;;
esac

curl -u $USER:"$PASS" -XPUT "http://$HOST/_cluster/settings" \
     -H 'Content-Type: application/json' \
     -d'
{
  "persistent": {
    "plugins.ml_commons.only_run_on_ml_node": "false",
    "plugins.ml_commons.model_access_control_enabled": "true",
    "plugins.ml_commons.native_memory_threshold": "99"
  }
}
'

task_id_response=$(curl -u $USER:"$PASS" -s -XPOST "http://$HOST/_plugins/_ml/models/_register?deploy=true" \
                   -H 'Content-Type: application/json' \
                   -d'
{
  "name": "huggingface/sentence-transformers/paraphrase-MiniLM-L3-v2",
  "version": "1.0.1",
  "model_format": "TORCH_SCRIPT"
}
')

task_id=$(echo "$task_id_response" | jq -r '.task_id')
echo "Task ID: $task_id"

echo "Waiting for model registration to complete..."
while true; do
  response=$(curl -u $USER:"$PASS" -s -XGET "http://$HOST/_plugins/_ml/tasks/$task_id")
  state=$(echo "$response" | jq -r '.state')

  echo "Current state: $state"

  if [ "$state" = "COMPLETED" ]; then
    echo "Model registration completed successfully!"
    model_id=$(echo "$response" | jq -r '.model_id')
    echo "Model ID: $model_id"
    break
  elif [ "$state" = "FAILED" ]; then
    echo "Model registration failed!"
    exit 1
  fi

  sleep 5
done

echo "Creating embedding pipeline..."
curl -u $USER:"$PASS" -XPUT "http://$HOST/_ingest/pipeline/ai-description-embedding-pipeline" \
     -H 'Content-Type: application/json' \
     -d @- << EOF
{
  "processors": [
    {
      "text_embedding": {
        "field_map": {
          "ai_description": "ai_description_embedding"
        },
        "model_id": "$model_id"
      }
    }
  ]
}
EOF

echo "Creating index '$INDEX_NAME'..."
curl -u $USER:"$PASS" -XPUT "http://$HOST/$INDEX_NAME" \
     -H "Content-Type: application/json" \
     -d @/init/index.json

echo "Initialization complete!"