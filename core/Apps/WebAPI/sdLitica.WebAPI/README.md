Running application locally with Docker.

## Pre-reqs

### MySQL/Mariadb

1. MySQL/Maria DB container is running
2. In MySQL/MariaDB created database called sdlitica with user sdlitica and password sdlitica
3. MySQL/Mariadb container name (or hostname) is mariadb

### RabbitMQ

1. Conteiner with RabbitMQ is running in Docker
2. Default user guest/guest is not disabled (this is default config)
3. Container name (or host name) is rabbit MQ

# InfluxDB setup

1. TBD

## Running application

1. First, application needs to be built by running 

        dotnet buld

2. Then container need to be build and started

        ./Docker/buildAndRun.sh wsl 12345


