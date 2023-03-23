# Sense Tower Event API

### Требования к использованию
1. Docker
2. Visual Studio 2022
3. Mongo DB ( присутствует вариант использования MongoDB в docker контейнере)
4. Identity Server 4 https://github.com/Cptrex/identity-server-4
_____

### Установка
1. Клонируем репозиторий 
```
git clone https://github.com/Cptrex/sense-tower-event-api.git
```
2. Скачиваем официальный образ Mongo DB командой:
```
$ docker pull mongo
```
3. Скачиваем образ Identity Server 4:
github: https://github.com/Cptrex/identity-server-4
docker hub: https://hub.docker.com/r/cptrex/identity-server-4
```
$ docker pull cptrex/identity-server-4
```

Identity Server 4 необходим для аутентификации запросов к API
Mongo DB используется как основная база данных API. 
_____

##### Dockerfile
В корне проекта лежит Dockerfile решения Sense Tower Event API. Для создания образа API, откройте командную строку и выполните команду:

```
$ docker build . -t имя_образа
```

#### Конфигурация

**appsetings.json**
```
{
  "EventsDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:7000/",
    "DatabaseName": "STEventsApiDB",
    "CollectionName": "Events"
  },
  "IdentityServer4Settings": {
    "Authority": "http://localhost:5001",
    "ApiName": "myApi",
    "Audience": "myApi",
    "ClientId": "sensetower-event-api",
    "ClientSecret": "sensetowereventapi"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
EventsDatabaseSettings - настройки подключения к базе данных MongoDB.
IdentityServer4Settings - настройки подключения к Identity Server 4.

### Получение токена авторизации jwt
```
POST: http://localhost:{{port}}/connect/token

type : x-www-form-urlencoded

body {
    grant_type = "client_credentials",
    scope = "myApi.read",
    client_id = "sensetower-event-api",
    client_secret = "sensetowereventapi"
}
```
