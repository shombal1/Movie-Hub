![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Keycloak](https://img.shields.io/badge/Keycloak-4A4A55?style=for-the-badge&logo=keycloak&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)

# MovieHub

## Описание проекта

MovieHub — платформа для просмотра и управления медиаконтентом, использующая Keycloak для централизованной аутентификации и авторизации пользователей. Проект разработан с использованием чистой архитектуры.

### Основные компоненты

- **MovieHub.Engine.Api**: Основной API-сервис, обрабатывающий запросы пользователей к медиаконтенту
- **MovieHub.Engine.Domain**: Бизнес-логика и доменная модель системы
- **MovieHub.Engine.Storage**: Слой доступа к данным (MongoDB и S3-хранилище)
- **MovieHub.UserSync.KeycloakConsumer**: Сервис для синхронизации пользовательских аккаунтов между Keycloak и внутренней системой

### Ключевые функции

- Просмотр и поиск медиаконтента
- Управление корзиной медиа
- Загрузка и хранение медиафайлов в S3-хранилище
- Авторизация через Keycloak
- Синхронизация пользовательских данных между сервисами

### Технологии

- **.NET 8**: Основной фреймворк
- **MongoDB**: Основное хранилище данных
- **Redis**: Кэширование данных
- **Keycloak**: Аутентификация и авторизация
- **MinIO**: S3-совместимое хранилище файлов
- **Docker**: Контейнеризация сервисов

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

## Структура проекта

```
MovieHub.Engine.Api                     - API сервис для работы с медиаконтентом
MovieHub.Engine.Domain                  - Доменная логика системы
MovieHub.Engine.Storage                 - Персистентный слой для хранения данных
MovieHub.UserSync.KeycloakConsumer      - Сервис синхронизации пользователей с Keycloak
```

## Доступ к сервисам

- **MovieHub API**: http://localhost:9090/swagger (Swagger UI для API)
- **Keycloak**: http://localhost:8180 (Административная консоль)
- **MinIO Console**: http://localhost:9001 (Управление S3 хранилищем)
