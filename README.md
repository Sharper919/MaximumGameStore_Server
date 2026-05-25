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
