FROM quay.io/keycloak/keycloak:26.0.0
COPY keycloak-kafka-1.2.0-jar-with-dependencies.jar /opt/keycloak/providers/
COPY ./configuration/realm-export.json ./save-realm/realm-export.json

RUN /opt/keycloak/bin/kc.sh build

ENTRYPOINT ["/opt/keycloak/bin/kc.sh"]