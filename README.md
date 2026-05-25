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

### 4. Налаштування бази даних у файлі `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MGStoreConnection": "Server=YOUR_SERVER;Database=MaximumGameStore;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 5. Налаштування JWT в `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "MaximumGameStore",
    "Audience": "MaximumGameStoreClient"
  }
}
```

### 6. Встановлення Entity Framework Core CLI:

```bash
dotnet tool install --global dotnet-ef
```

### 7. Застосування міграцій:

```bash
dotnet ef database update
```

Ця команда створить таблиці, зв'язки, індекси та обмеження відповідно до міграцій Entity Framework Core.

### 8. Запуск сервера

```bash
dotnet run
```
