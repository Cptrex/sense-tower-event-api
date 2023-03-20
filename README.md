# Sense Tower Event API

#### Требования к использованию
1. Docker
2. Visual Studio 2022
3. Mongo DB ( присутствует вариант использования MongoDB в docker контейнере)
_____

#### Установка
1. Клонируем репозиторий 
```
git clone https://github.com/Cptrex/sense-tower-event-api.git
```
2. Переходим в папку **/SenseTowerEventAPI/IdentityServer4/** и выполняем команду:
```
$ docker-compose up
```

+ При помощи данной команды произойдет установка:
    + Identity Server 4
    + Mongo DB
https://github.com/markglibres/identityserver4-mongodb-redis - репозиторий образа IS4
https://hub.docker.com/r/bizzpo/identityserver4 - docker hub образа
Identity Server 4 необходим для аутентификации запросов к API
Mongo DB используется как основая база данных API. 
_____

##### Dockerfile
В корне проекта лежит Dockerfile решения Sense Tower Event API. Для создания образа API, откройте командную строку и выполните команду:

```
$ docker build . -t имя_образа
```

##### Конфигурация

**appsetings.json**
```
{
  "EventsDatabaseSettings": {
    "ConnectionString": "mongodb://root:foobar@mongodb:27017/?readPreference=primaryPreferred&appname=identityserver",
    "DatabaseName": "STEventsApiDB",
    "CollectionName": "Events"
  },
  "IdentityServer4Settings": {
    "AuthorityUrl": "http://localhost:5000",
    "Audience":  "myapi",
    "ClientId": "spaWeb",
    "ClientSecret": "hardtoguess",
    "Scopes": [ "myapi.access", "openid", "offline_access" ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "https_port": 443,
  "AllowedHosts": "*"
}
```
EventsDatabaseSettings - настройки подключения к базе данных MongoDB.
IdentityServer4Settings - настройки подключения к Identity Server 4.