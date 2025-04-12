# Kafka Multiple Consumers

This projects demonstrates how do create a Kafka Consumer with multiple consumers per instance in .net 8.0 using a background service (worker) and a Web API to publish the messages on the kafka topic.

This scenario is particularly useful when you want to have multiple consumers reading from the same topic and the possibility of scaling horizontally to increase the capacity of your application and reduce the total processing time of a specific operation.

## How to run

Using visual studio run the API + Worker profile and it will start both projects.

Run the docker-compose file to start the Kafka and Zookeeper containers.

```bash
docker-compose up
```

## Database

The docker compose will create a SQL Server and a database, when the POST /api/v1/kafka endpoint is called, it will insert some data into SAMPLE_MESSAGES table.

<img src="./images/SQL_Server.png" alt="SQL Server"/>

## Kafka Initialization Script

The docker compose will also create a Kafka topic called "kafka-playground-topic" and the topics, partitions, messages etc can be inspected with Kafdrop:

<img src="./images/kafdrop.png" alt="Kadrop UI"/>

## Healthcheck

You can also check if the docker containers are running properly by accessing the healthcheck endpoint "/health/ready".

### Healthy

<img src="./images/healthcheck_healthy.png" alt="Healthy"/>

### Unhealthy

If something goes wrong, you will se an error message like this one, and it will guide you trough the problem.

<img src="./images/healthcheck_unhealthy.png" alt="Healthy"/>

## Web API

The Web API here has only one job, to publish some messages on a kafka topic so they can be consumed by a worker.

<img src="./images/webapi.png" alt="Web API"/>

## Inspecting Published Messages

Once the messages were published, we can inspect them using Kafdrop.

<img src = "./images/kafdrop_partition.png" alt="Kafdrop Messages"/>

## Worker

The worker is a background service that consumes messages from the kafka topic and processes them. It is configured to have multiple consumers per instance, which allows it to scale horizontally and process messages in parallel. In this example we defined 10 consumers and we can inspect them here:

<img src="./images/worker_consumers.png" alt="Worker"/>

And once we publish some messages via Web API the worker will consume them, in this case we just created a Console.WriteLine:

<img src="./images/worker-consuming.png" alt="Worker Messages"/>

So we can see the it is randomly choosing the consumers to process the messages, and it is also processing them in parallel.

## Real World Example

In a real world example, you would have a Kafka cluster running in a cloud provider, and you would use a library like Confluent.Kafka to produce and consume messages.

This solution of multiple consumers could be used to run multiple instances of the same consumer, or to run different consumers that process the same messages in different ways. We could have for example a repository reading millions of lines of a database and publishing them on a kafka topic, latter on our worker would read thos millions of lines and calculate brokerages for example and store the calculated values in a different database. This way we could have multiple workers doing the same job, or different jobs, and scaling horizontally.

This is a very simple example, but it could be used in a real world scenario. The possibilities are endless.

## Knwown Issues

If you have any trouble running locally, I suggest these handful docker commands:

Remove all containers:
docker rm -vf $(docker ps -a -q)

Remove all Volumes:
docker volume rm $(docker volume ls -q)

Not necessary, but if you want to remove all images:
docker rmi -f $(docker images -a -q)

If the database doesn't proper initialize, try increasing the 60 seconds in the docker-compose file here:

```yaml
command: /bin/bash -c "sleep 60 && /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Password123! -d master -i tmp/init.sql"
```

## Conclusion

This project is a POC to demonstrate how to use Kafka as a Pub/Sub system with multiple consumers. It is not meant to be used in production, but it can be used as a starting point for building a real world application.