# Maximum Game Store API

Серверна частина вебзастосунку **Maximum Game Store** - це ASP.NET Core Web API для вебмагазину комп'ютерних ігор. API забезпечує реєстрацію та авторизацію користувачів, роботу з каталогом ігор, кошиком, замовленнями, особистим кабінетом користувача та адміністративною панеллю.

## Технологічний стек

- C#
- ASP.NET Core Web API
- .NET 9
- Entity Framework Core
- MS SQL Server
- JWT Authentication
- Swagger / OpenAPI
- NuGet

## Системні вимоги

Для запуску серверної частини необхідно встановити:

- .NET SDK 9.0 або новіший
- MS SQL Server
- Entity Framework Core CLI
- Visual Studio 2022 або інше середовище з підтримкою .NET 9
- Git

## Розгортання проєкту

### 1. Клонування репозиторію

```bash
git clone <repository-url>
```

### 2. Перехід до папки серверної частини

```bash
cd Server/MaximumGameStore/MaximumGameStore
```

### 3. Відновлення залежностей

```bash
dotnet restore
```

### 4. Налаштування бази даних

У файлі `appsettings.json` потрібно вказати рядок підключення до MS SQL Server:

```json
{
  "ConnectionStrings": {
    "MGStoreConnection": "Server=YOUR_SERVER;Database=MaximumGameStore;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Приклад для локального SQL Server:

```json
{
  "ConnectionStrings": {
    "MGStoreConnection": "Server=localhost;Database=MaximumGameStore;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 5. Налаштування JWT

У файлі `appsettings.json` потрібно вказати параметри JWT:

```json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "MaximumGameStore",
    "Audience": "MaximumGameStoreClient"
  }
}
```

Параметр `Key` має бути достатньо довгим секретним ключем і не повинен публікуватися у відкритому доступі.

### 6. Встановлення Entity Framework Core CLI

Якщо інструмент `dotnet-ef` ще не встановлений, його можна встановити командою:

```bash
dotnet tool install --global dotnet-ef
```

Якщо інструмент уже встановлений, його можна оновити:

```bash
dotnet tool update --global dotnet-ef
```

### 7. Застосування міграцій

Для створення або оновлення структури бази даних потрібно виконати:

```bash
dotnet ef database update
```

Ця команда створить таблиці, зв'язки, індекси та обмеження відповідно до міграцій Entity Framework Core.

### 8. Запуск серверної частини

```bash
dotnet run
```

Після запуску API буде доступне за адресою, вказаною в консолі. Наприклад:

```text
https://localhost:7151
```

У режимі розробки Swagger-документація доступна за адресою:

```text
https://localhost:7151/swagger
```

## Основні API-маршрути

### Авторизація

```http
POST /api/auth/register
POST /api/auth/login
```

### Ігри

```http
GET /api/games
GET /api/games/filter/name
GET /api/games/filter/features
GET /api/games/{gameId}/info
GET /api/games/{gameId}/requirements
GET /api/games/{gameId}/images
```

### Кошик

```http
GET /api/cart
POST /api/cart
DELETE /api/cart/{gameId}
```

### Оформлення замовлення

```http
POST /api/checkout
POST /api/checkout/buy-now/{gameId}
```

### Користувач

```http
GET /api/user
PUT /api/user/name
PUT /api/user/password
GET /api/user/games
GET /api/user/orders
```

### Адміністративні маршрути

```http
GET /api/admin/users
PUT /api/admin/users/{userId}/block

GET /api/admin/games
POST /api/admin/games/create
PUT /api/admin/games/{gameId}/delete

POST /api/admin/game-images
GET /api/admin/game-images/game/{gameId}
PUT /api/admin/game-images/{id}/set-main

POST /api/admin/requirements/add
PUT /api/admin/requirements/{requirementId}/update
```

## Авторизація запитів

Для доступу до захищених маршрутів потрібно передавати JWT-токен у заголовку `Authorization`.

Формат заголовка:

```http
Authorization: Bearer <token>
```

Адміністративні маршрути доступні лише користувачам із роллю `Admin`.

## Робота із зображеннями

Зображення ігор завантажуються через маршрут:

```http
POST /api/admin/game-images
```

Файли передаються у форматі `multipart/form-data`. Сервер зберігає зображення у директорії:

```text
wwwroot/images/games
```

У базі даних зберігається шлях до файлу, ознака головного зображення та порядок відображення.

## Збірка для production

Для публікації серверної частини у режимі Release потрібно виконати:

```bash
dotnet publish -c Release
```

Опубліковані файли будуть розміщені у директорії:

```text
bin/Release/net9.0/publish
```

Цю директорію можна перенести на робочий сервер і запустити застосунок за допомогою .NET Runtime 9.

## Примітки

- Перед запуском потрібно переконатися, що MS SQL Server працює.
- Рядок підключення до бази даних має відповідати локальному або серверному середовищу.
- JWT Secret Key не слід зберігати у відкритому доступі.
- Для адміністративних маршрутів потрібен користувач із роллю `Admin`.
- Swagger доступний лише у режимі розробки.
