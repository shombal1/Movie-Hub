#!/bin/bash

cd mongodb
sh init_mongo_db.sh
cd ../


docker-compose --env-file ./.env -f ./docker-compose.yml up -d

sleep 30s

winpty docker exec -it movie-hub-keycloak bash 
/opt/keycloak/bin/kc.sh export import --file ./save-realm/realm-export.json
quit

sleep 1000