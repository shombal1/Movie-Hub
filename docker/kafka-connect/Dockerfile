FROM confluentinc/cp-kafka-connect-base:7.7.1

RUN confluent-hub install --no-prompt confluentinc/kafka-connect-jdbc:10.6.0

COPY worker.properties .
COPY source-connector.properties .

CMD ["/bin/connect-standalone","worker.properties","source-connector.properties"]