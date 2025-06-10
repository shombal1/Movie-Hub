
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

# MovieHub

## Запуск инфраструктуры

Поднимаем MongoDB:

```bash
docker-compose --env-file docker/.env -f docker/core-infra/mongodb/docker-compose.yml up -d
```

Поднимаем основную инфраструктуру:

```bash
docker-compose --env-file docker/.env -f docker/core-infra/docker-compose.yml up -d
```

## Запуск сервисов

Запустить сервис **Engine**:

```bash
docker-compose --env-file docker/.env -f docker/services/engine/docker-compose.yml up -d
```

Запустить сервис **Keycloak Consumer**:

```bash
docker-compose --env-file docker/.env -f docker/services/keycloak-consumer/docker-compose.yml up -d
```
