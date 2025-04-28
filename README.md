
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

## Настройка Keycloak

1. Создать **Realm** с названием:
   ```
   movie-hub
   ```

2. Создать **Клиента** со следующими настройками:

   **General Settings**:
    - Client ID: `movie-hub-client`

   **Capability Config**:
    - Включить:
        - Client authentication
        - Authorization
        - Implicit flow

   **Login Settings**:
    - Valid redirect URIs установить `http://localhost:9090/*`
    - Web origins установить `http://localhost:9090`

   ➔ Нажать **Сохранить**.

3. Перейти в раздел **Credentials**, скопировать **Client Secret**  
   и вставить его в файл `.env` в параметр:

   ```
   KEYCLOAK__CREDENTIALS__SECRET
   ```

4. Дополнительные настройки:

    - **Realm Settings** → **Login**:
        - Включить **User Registration**.

    - **Realm Settings** → **User Profile**:
        - Удалить поля `firstName` и `lastName`.

    - **Realm Settings** → **Events** → **User Events Settings**:
        - Включить **Save events**.
        - Установить **Expiration**: `5 дней`.
        - Сохранить изменения.

    - **Realm Settings** → **Events** → **Event Listeners**:
        - Добавить `kafka`.
        - Сохранить изменения.

## Запуск сервисов

Запустить сервис **Engine**:

```bash
docker-compose --env-file docker/.env -f docker/services/engine/docker-compose.yml up -d
```

Запустить сервис **Keycloak Consumer**:

```bash
docker-compose --env-file docker/.env -f docker/services/keycloak-consumer/docker-compose.yml up -d
```
