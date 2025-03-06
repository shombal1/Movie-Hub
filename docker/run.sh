#!/bin/bash

cd mongodb
sh init_mongo_db.sh
cd ../


docker-compose --env-file ./.env -f ./docker-compose.yml up -d

sleep 30s

sleep 1000