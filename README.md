# KafkaPlayground

Pub/Sub Kafka Playground - POC Multiple Consumers

## Description

This project is a POC to demonstrate how to use Kafka as a Pub/Sub system with multiple consumers.

I wrote a [Medium](https://medium.com/p/659362b4fe0d#8077-e1407e7d701e) post here explaining the purpose of this project

After the post I decided to add some features:

- Database to store the messages
- Docker Compose was updated to include database
- Healtcheck endpoint

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
