FROM quay.io/keycloak/keycloak:26.0.0
COPY keycloak-kafka-1.2.0-jar-with-dependencies.jar /opt/keycloak/providers/
RUN /opt/keycloak/bin/kc.sh build

ENTRYPOINT ["/opt/keycloak/bin/kc.sh"]