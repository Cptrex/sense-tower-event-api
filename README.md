# Sense Tower Event API

### Требования к использованию
1. Docker
2. Docker Desktop (Опциально. Вся инструкция подразумевает запуск из-под консоли)
3. Visual Studio 2022
4. Mongo DB
5. Identity Server 4 https://github.com/Cptrex/identity-server-4
6. Rabbit MQ
7. Polly lib


https://hub.docker.com/u/cptrex
_____

### Установка
1. **Event API Service** <br>
    1.1. Клонируем репозиторий:
    ```
    $ git clone https://github.com/Cptrex/sense-tower-event-api.git
    ```
    1.2. Открываем скачанную папку репозитория </br>
    1.3. Наводим на пустое место в проводнике и жмем SHIFT + ПКМ --> Открыть окно PowerShell здесь:. </br>
    1.4. В окне powershell вводим следующие команды:
    ```
    $ docker network create stnetworkapi   // команда создаст внутреннюю сеть, которая объединит докер контейнеры для корректной работы
    $ docker-compose up
    ```
    *Введенная команда соберет контейнер из сервисов с заложенными настройками в файле docker-compose.yml*

2. **MongoDB** <br>
    2.1. Открываем Powershell и вводим команду:
    ```
    $ docker pull mongo
    ```
    *Данная команда скачает официальный образ MongoDB*
    2.2. Вводим следующую команду:
    ```
    $ docker run -p 7000:27017 --name mongodb mongo
    ```
    *Данная команда запустит контейнер с именем "mongodb" на основе образа mongo с привязкой внешнего порта 7000 на внутренний порт контейнера 27017*
    
    *Mongo DB используется как основная база данных API.*

3. **Identity Server 4** </br>
  Установка Identity Server 4 включена в docker-compose файл.
    github: https://github.com/Cptrex/identity-server-4 </br>
    docker hub: https://hub.docker.com/r/cptrex/identity-server-4 </br>
    *Identity Server 4 необходим для аутентификации запросов к API*

4. **ImageService, SpaceService, PaymentService** </br>
    Установка данных сервисов включена в docker-compose файл. </br>
    4.1. Данные сервисы являются слушателями очереди брокера сообщений RabbitMQ. При их запуске создастся очередь, если она отсутсвует. </br>
    4.2. Тестирование запросов можно производить через UI часть RabbitMQ. *http://localhost:15672* </br>
    4.3. JSON объекта для тела запроса в очередь событий:
    ```
    {"Type":1,"DeletedId":"00000000-0000-0000-0000-000000000000"}
    ```
    EventOperationType |          Type |                    Описание   | DeletedId (Guid)   |
    :------------------|:-------------:|------------------------------:|--------------------
    |SpaceDeleteEvent  | 1             | Событие удаления пространства | Id пространства
    |ImageDeleteEvent  | 2             | Событие удаления изображения  | Id изображения
    |EventDeleteEvent  | 3             | Событие удаления мероприятия  | Id мероприятия
5. **RabbitMQ** </br>
    Установка сервиса брокера сообщений включена в docker-compose файл.
    *Web версия брокера сообщений будет досутпна после запуска по адресу: http://localhost:15672*
_____

### Конфигурация
Конфигурация всех сервисов выполнена через ENV переменные. Редактируйте значения в launchSettings.json перед запуском из Visual Studio 2022, либо укажите правильные параметры в docker-compose перед развертыванием.

**launchSettings.json**
```
{
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "SenseTowerEventAPI": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ServiceEndpoints__TokenAuthorization": "",
        "ServiceEndpoints__PaymentService__URL": "http://localhost:5080",
        "ServiceEndpoints__ImageService__URL": "http://localhost:5020",
        "ServiceEndpoints__SpaceService__URL": "http://localhost:5040",
        "ServiceEndpoints__RabbitMQ__Hostname": "localhost",
        "ServiceEndpoints__RabbitMQ__Port": "5672",
        "ServiceEndpoints__RabbitMQ__User": "guest",
        "ServiceEndpoints__RabbitMQ__Password": "guest",
        "IdentityServer4Settings__Authority": "http://localhost:5001",
        "IdentityServer4Settings__Audience": "myApi",
        "EventsDatabaseSettings__ConnectionString": "mongodb://localhost:7000/",
        "EventsDatabaseSettings__DatabaseName": "STEventsApiDB",
        "EventsDatabaseSettings__CollectionName": "Events"
      },
      "dotnetRunMessages": true,
      "applicationUrl": "http://localhost:5030"
    }
  },
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:49816",
      "sslPort": 44394
    }
  }
}
```
- EventsDatabaseSettings - настройки подключения к базе данных MongoDB.
- IdentityServer4Settings - настройки подключения к Identity Server 4.
- ServiceEndpoints - настройка подключения к сервисам API. <br>
-- TokenAuthorization - токен аутентификации для обращения к эндпоинтам сервиса. Один для всех сервисов.

### Получение токена аутентификации для обращения к эндпоинтам сервиса

Перед использованием сервисов, необходимо получить токен и прописать его в ENV переменные docker-compose.yml
```
POST: http://localhost:{{identity-server-4-port}}/connect/token

type: x-www-form-urlencoded

body: {
    grant_type = "client_credentials",
    scope = "myApi.read",
    client_id = "sensetower-event-api",
    client_secret = "sensetowereventapi"
}
```
