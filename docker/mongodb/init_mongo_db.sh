#!/bin/bash

echo 'start initialization mongo'

docker-compose --env-file ./../.env -f ./config-server/docker-compose.yml up -d
docker-compose --env-file ./../.env -f ./shard-server-1/docker-compose.yml up -d
docker-compose --env-file ./../.env -f ./shard-server-2/docker-compose.yml up -d
docker-compose --env-file ./../.env -f ./mongo-router/docker-compose.yml up -d

sleep 10s
winpty docker exec -it configsvr1 mongosh --eval "load('scripts/init_rs.js'); quit();"

sleep 5s
winpty docker exec -it shardsvr1_1 mongosh --eval "load('scripts/init.js'); quit();"
sleep 5s
winpty docker exec -it shardsvr2_1 mongosh --eval "load('scripts/init.js'); quit();"

sleep 5s
winpty docker exec -it mongos mongosh --eval "load('scripts/init.js'); load('scripts/initialization-db.js'); quit();"

echo 'end initialization mongo'
