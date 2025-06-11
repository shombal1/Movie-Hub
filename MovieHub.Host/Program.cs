
// TODO: Fix MongoDB connection issue:
// Current error: Timeout after 30000ms with message 'This host is unknown'
// The problem occurs when trying to resolve DNS name "aaa-movie-hub-items-mongo"

var builder = DistributedApplication.CreateBuilder(args);
/*
var zookeeper = builder
    .AddContainer("zookeeper", "confluentinc/cp-zookeeper:7.6.0")
    .WithContainerName($"{builder.Configuration["Project:Name"]}-zookeeper")
    .WithEnvironment("ZOOKEEPER_CLIENT_PORT", "2181")
    .WithEnvironment("ZOOKEEPER_TICK_TIME", "2000")
    .WithHttpEndpoint(2181, 2181)
    .WithLifetime(ContainerLifetime.Persistent);


var kafkaBroker = builder
    .AddContainer("kafka-broker", "confluentinc/cp-kafka:7.6.0")
    .WithContainerName($"{builder.Configuration["Project:Name"]}-{builder.Configuration["Kafka:BrokerContainer"]}")
    .WithEnvironment("KAFKA_BROKER_ID", "1")
    .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1")
    .WithEnvironment("KAFKA_TRANSACTION_STATE_LOG_MIN_ISR", "1")
    .WithEnvironment("KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR", "1")
    .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT", $"{builder.Configuration["Project:Name"]}-zookeeper:2181")
    .WithEnvironment("KAFKA_LISTENER_SECURITY_PROTOCOL_MAP", "PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT")
    .WithEnvironment("KAFKA_ADVERTISED_LISTENERS",
        $"PLAINTEXT://{builder.Configuration["Project:Name"]}-{builder.Configuration["Kafka:BrokerContainer"]}:29092,PLAINTEXT_HOST://localhost:9092")
    .WithHttpEndpoint(int.Parse(builder.Configuration["Kafka:BrokerPort"]), 9092)
    .WaitFor(zookeeper)
    .WithLifetime(ContainerLifetime.Persistent);

var keycloak = builder
    .AddKeycloak("keycloak",8081)
    .WithDockerfile("./Configs/Keycloak/")
    .WithImage("my-keycloak-kafka:new-local")
    .WithContainerName($"{builder.Configuration["Project:Name"]}-keycloak")
    .WithRealmImport("./Configs/Keycloak/realm-export.json")
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_USERNAME", builder.Configuration["Keycloak:Admin:User"])
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_PASSWORD", builder.Configuration["Keycloak:Admin:Password"])
    .WithEnvironment("KAFKA_TOPIC", builder.Configuration["Keycloak:KafkaTopic"])
    .WithEnvironment("KAFKA_CLIENT_ID", "keycloak")
    .WithEnvironment("KAFKA_EVENTS", "REGISTER")
    .WithEnvironment("KAFKA_BOOTSTRAP_SERVERS", $"{builder.Configuration["Kafka:BrokerContainer"]}:29092")
    .WithArgs("--log-console-color=true")
    .WaitFor(kafkaBroker)
    .WithLifetime(ContainerLifetime.Persistent);

var kafkaUI = builder
    .AddContainer("kafka-ui", "provectuslabs/kafka-ui:latest")
    .WithContainerName($"{builder.Configuration["Project:Name"]}-kafka-ui")
    .WithEnvironment("KAFKA_CLUSTERS_0_NAME", "local")
    .WithEnvironment("KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS", $"{builder.Configuration["Kafka:BrokerContainer"]}:29092")
    .WithEnvironment("KAFKA_CLUSTERS_0_KAFKACONNECT_0_NAME", "keycloak-events-outbox")
    .WithEnvironment("KAFKA_CLUSTERS_0_KAFKACONNECT_0_ADDRESS", "http://movie-hub-kafka-connect:8083")
    .WithHttpEndpoint(8082, 8080)
    .WaitFor(kafkaBroker)
    .WithLifetime(ContainerLifetime.Persistent);


var mongo = builder
    .AddMongoDB("mongo")
    .WithContainerName($"{builder.Configuration["Project:Name"]}-mongo")
    .WithBindMount("./Configs/Mongo/test-mongo-keyfile", "/data/mongo-keyfile")
    .WithHttpEndpoint(21000, 27017)
    .WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "admin")
    .WithArgs(
        "bash", "-c",
        "chmod 400 /data/mongo-keyfile && chown 999:999 /data/mongo-keyfile && " +
        "exec docker-entrypoint.sh mongod --replSet rs0 --bind_ip_all --dbpath /data/db --keyFile /data/mongo-keyfile")
    .WithLifetime(ContainerLifetime.Persistent);

var initMongo = builder
    .AddMongoDB("mongo-init", 21003)
    .WithContainerName($"{builder.Configuration["Project:Name"]}-mongo-init")
    .WithBindMount("./Configs/Mongo/init-db.js", "/scripts/init-db.js")
    .WithBindMount("./Configs/Mongo/init-rs.js", "/scripts/init-rs.js")
    .WithEnvironment("MONGO_PASSWORD1", "admin")
    .WithArgs("bash", "-c",
        """
            check_mongo_ready() {
              host=$1
              password=$2
              until mongosh "$host" -u admin -p "$password" --authenticationDatabase admin --eval "db.adminCommand('ping')" >/dev/null 2>&1; do
                echo "Waiting for $host to be ready...";
                sleep 1;
              done
            }

            wait_for_primary() {
              host=$1
              password=$2
              rs=$3
              echo "Waiting for primary in replica set $rs on $host..."
              until mongosh "$host" -u admin -p "$password" --authenticationDatabase admin --eval "
                try {
                  const status = rs.status();
                  if (status.ok) {
                    const primary = status.members.find(m => m.stateStr === 'PRIMARY');
                    if (primary) {
                      print('Primary found: ' + primary.name);
                      quit(0);
                    }
                  }
                  quit(1);
                } catch (e) {
                  quit(1);
                }
              " >/dev/null 2>&1; do
                echo "Still waiting for primary in $rs..."
                sleep 2;
              done
              echo "Primary elected in $rs"
            }

            echo 'Checking aaa-movie-hub-items-mongo readiness...' &&
            check_mongo_ready aaa-movie-hub-items-mongo:27017 "${MONGO_PASSWORD1}"

            echo 'Init replication...' &&
            mongosh aaa-movie-hub-items-mongo:27017 -u admin -p "${MONGO_PASSWORD1}" --authenticationDatabase admin /scripts/init-rs.js

            echo 'Waiting for primary election...' &&
            wait_for_primary aaa-movie-hub-items-mongo:27017 "${MONGO_PASSWORD1}" "rs0" &&
            echo 'Initializing database...' &&
            mongosh aaa-movie-hub-items-mongo:27017 -u admin -p "${MONGO_PASSWORD1}" --authenticationDatabase admin /scripts/init-db.js &&
            echo 'Initialization completed.'
            """.Replace("\r\n", "\n"));


var mongoExpress = builder
    .AddContainer("mongo-express", "mongo-express:latest")
    .WithContainerName($"{builder.Configuration["Project:Name"]}-mongo-express")
    .WithHttpEndpoint(8083, 8081)
    .WithEnvironment("ME_CONFIG_MONGODB_URL", $"mongodb://admin:admin@{builder.Configuration["Project:Name"]}-mongo:27017/?authSource=admin")
    .WithEnvironment("ME_CONFIG_OPTIONS_EDITORTHEME", "ambiance")
    .WithEnvironment("ME_CONFIG_BASICAUTH_USERNAME", "admin")
    .WithEnvironment("ME_CONFIG_BASICAUTH_PASSWORD", "admin")
    .WithLifetime(ContainerLifetime.Persistent);


*/
builder.Build().Run();
