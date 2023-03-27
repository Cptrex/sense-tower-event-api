# Sense Tower Event API

### Требования к использованию
1. Docker
2. Docker Desktop
3. Visual Studio 2022
4. Mongo DB
5. Identity Server 4 https://github.com/Cptrex/identity-server-4
6. Rabbit MQ
7. Polly lib


https://hub.docker.com/u/cptrex
_____

### Установка
1. **Event API Service**
    1.1. Клонируем репозиторий:
    ```
    $ git clone https://github.com/Cptrex/sense-tower-event-api.git
    ```
    1.2. Открываем скачанную папку репозитория:
    1.3. Наводимся на пустое место в проводнике и жмем SHIFT + ПКМ --> Открыть окно PowerShell здесь.
    1.4. В открывшемся powershell вводим команду:
    ```
    $ docker build .
    ```
    1.5. Открываем программу Docker Desktop --> Images --> Выбираем добавленный образ и жмем "Запуск".
    1.6. В появившемся окне настроек запуска контейнера задаем имя(любое).
    1.7. Во вкладке Ports задаем любой свободный порт на вашем компьютере.
    1.8. Нажимаем Run. Готово. Основной Сервис Мероприятий запущен.

2. **MongoDB**
    2.1. Открываем Powershell и вводим команду:
    ```
    $ docker pull mongo
    ```
    2.2. Открываем программу Docker Desktop --> Images --> Выбираем добавленный образ и жмем "Запуск".
    2.3. В появившемся окне настроек запуска контейнера задаем имя(любое).
    2.4. Во вкладке Ports задаем любой свободный порт на вашем компьютере.
    2.5. Нажимаем Run. Готово.База Mongo для работы сервиса была запущена.
    *Mongo DB используется как основная база данных API.*

3. **Identity Server 4**
    github: https://github.com/Cptrex/identity-server-4
    docker hub: https://hub.docker.com/r/cptrex/identity-server-4
    3.1.Открываем Powershell и вводим команду:
    ```
    $ docker pull cptrex/identity-server-4
    ```
    3.2. Открываем программу Docker Desktop --> Images --> Выбираем добавленный образ и жмем "Запуск".
    3.3. В появившемся окне настроек запуска контейнера задаем имя(любое).
    3.4. Во вкладке Ports задаем любой свободный порт на вашем компьютере.
    3.5. Нажимаем Run. Готово. Identity Server 4 для утентификации сервисов был запущен.

    *Identity Server 4 необходим для аутентификации запросов к API*

4. **ImageService, SpaceService, PaymentService**
    4.1. Переходим в скачанную папку репозитория
    4.2. Открываем powershell (наводимся на пустое место в проводнике и жмем SHIFT + ПКМ --> Открыть окно PowerShell здесь) и вводим команду:
    ```
    $ docker-compose.yml up
    ```
    *Команда скачает необходимые образы и соберет контейнер*
    4.3. Данные сервисы являются слушателями очереди брокера сообщений RabbitMQ. При их запуске создастся очередь, если она отсутсвует.
    4.4. Тестирование запросов можно производить через UI часть RabbitMQ.
    4.5. JSON объекта для тела запроса:
    ```
    {"Type":1,"DeletedId":"00000000-0000-0000-0000-000000000000"}
    ```
    EventOperationType |          Type |                    Описание   | DeletedId (Guid)   |
    :------------------|:-------------:|------------------------------:|--------------------
    |SpaceDeleteEvent  | 1             | Событие удаления пространства | Id пространства
    |ImageDeleteEvent  | 2             | Событие удаления изображения  | Id изображения
    |EventDeleteEvent  | 3             | Событие удаления мероприятия  | Id мероприятия
5. **RabbitMQ**
    5.1. Открываем Powershell и вводим команду:
    ```
    $ docker run -d --hostname my-rabbitmq-server --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```
    *Данная команда скачает образ rabbitMQ и запустит контейнер. Web версия брокера сообщений будет досутпна после запуска по адресу: htpp://localhost:15672*
  6. **Payment Serice**
      6.1. Открываем Powershell и вводим команду:
        ```
        $ docker pull cptrex/st-service-payment
        ```
      6.2.Открываем программу Docker Desktop --> Images --> Выбираем добавленный образ и жмем "Запуск".
      6.3. В появившемся окне настроек запуска контейнера задаем имя(любое).
      6.4. Во вкладке Ports задаем любой свободный порт на вашем компьютере.
      6.5. Нажимаем Run. Готово. Payment Service для билетов готов.
_____

### Конфигурация

Установите необходимые URL в файле перед запуском приложения!
**appsetings.json**
```
{
   "EventsDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:7000/",
    "DatabaseName": "STEventsApiDB",
    "CollectionName": "Events"
  },
  "ServiceEndpoints": {
    "TokenAuthorization": "",
    "EventServiceURL": "",
    "PaymentServiceURL": "",
    "ImageServiceURL": "",
    "SpaceServiceURL": ""
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
- EventsDatabaseSettings - настройки подключения к базе данных MongoDB.
- IdentityServer4Settings - настройки подключения к Identity Server 4.
- ServiceEndpoints - настройка подключения к сервисам API.
-- TokenAuthorization - токен аутентификации для обращения к эндпоинтам сервиса. Один для всех сервисов.

### Получение токена аутентификации для обращения к эндпоинтам сервиса
```
POST: http://localhost:{{identity-server-4-port}}/connect/token

type : x-www-form-urlencoded

body {
    grant_type = "client_credentials",
    scope = "myApi.read",
    client_id = "sensetower-event-api",
    client_secret = "sensetowereventapi"
}
```