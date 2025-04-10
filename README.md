# Kafka Poc

Pub/Sub Kafka - POC Multiple Consumers

## Description

This project is a POC to demonstrate how to use Kafka as a Pub/Sub system with multiple consumers.

## How to run

Using visual studio run the API + Worker profile and it will start both projects.

Run the docker-compose file to start the Kafka and Zookeeper containers.

```bash
docker-compose up
```

If you decide to change something in the docker compose is always a good idea to run:

```bash
docker-compose down -v
```

And to start again after the changes:

```bash
docker-compose up --build
```

## Disclaimer

I wrote a [Medium](https://medium.com/p/659362b4fe0d#8077-e1407e7d701e) post here explaining the purpose of this project

After the post I decided to add some features:

- Database to store the messages
- Updated Docker Compose
- Healtcheck endpoint

## Database

<img src="./images/SQL_Server.png" alt="SQL Server" width="500"/>

## Healthcheck

### Healthy

<img src="./images/healthcheck_healthy.png" alt="Healthy" width="500"/>

### Unhealthy

<img src="./images/healthcheck_unhealthy.png" alt="Healthy"/>

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
