# MediaStore API

REST API for managing a product catalog.

The API allows users to browse products, view product details, and lets authenticated administrators manage the product catalog and administrator accounts.

---

## Features

### Products

- Get a paginated product list
- Filter products by search phrase and price range
- Sort products by selected fields
- Get product details by ID
- Add new products
- Delete products
- Store product image URL
- Store product descriptions in multiple languages: `pl`, `en`, `de`, `cs`
- Product description fallback:
  - requested language
  - English
  - first available description

### Authentication & Authorization

- JWT-based authentication
- Administrator login
- Role-based authorization
- Product creation and deletion available only for administrators

### Administrator Management

- Register new administrator account as `Pending`
- Approve pending administrator accounts
- Get paginated administrator list
- Filter administrators by email and status
- Delete administrator accounts
- Prevent deleting own administrator account
- Prevent deleting the last active administrator
- Enable or disable public administrator registration

### Data Initialization

- Optional database initializer controlled by configuration
- Seed products loaded from a JSON file
- Initializer runs only when the product repository is empty

---

## Tech Stack

- .NET 10 / ASP.NET Core
- Minimal API
- FluentValidation
- JWT Bearer Authentication
- Scalar / OpenAPI
- In-memory persistence by default
- Repository abstraction prepared for SQL persistence
- Dependency Injection
- CORS

---

## Architecture

The project uses a lightweight feature-based architecture inspired by Vertical Slice Architecture.

Instead of grouping code only by technical layers, endpoints are organized by features.

Example structure:

```text
MediaStore.Api/
  Common/
  Domain/
    Errors/
  Features/
    Auth/
    Admin/
    Products/
  Infrastructure/
    Configuration/
    Persistence/
    Security/
```

This keeps the project simple while still maintaining separation of concerns.

The main architectural decisions are:

- Minimal API endpoints grouped by feature
- Request validation with FluentValidation and endpoint filters
- Dedicated request and response models
- Repository abstraction
- In-memory persistence as the default implementation
- Stable backend error codes instead of hardcoded user-facing messages
- JWT authentication and role-based authorization

---

## Persistence

The application uses an in-memory persistence implementation by default.

```csharp
IProductRepository -> InMemoryProductRepository
IAuthRepository -> InMemoryAuthRepository
```

The persistence layer is hidden behind repository interfaces, which makes it possible to replace the current in-memory implementation with a SQL-based database implementation in the future.

For a production-ready version, the project could be extended with:

- EF Core
- SQL Server or PostgreSQL
- database migrations
- relational product translation tables or JSON columns
- persistent administrator accounts and roles

---

## Configuration

Example `appsettings.json`:

```json
{
  "DatabaseSettings": {
    "UseInMemory": true
  },
  "DatabaseInitializer": {
    "Enabled": true,
    "FilePath": "Seed/products.seed.json"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200"]
  },
  "Jwt": {
    "Issuer": "MediaStore",
    "Audience": "MediaStore.Client",
    "Secret": "THIS_IS_ONLY_FOR_DEVELOPMENT_CHANGE_ME_123456789",
    "ExpiresInMinutes": 60
  }
}
```

> The `Jwt:Secret` value is intended only for local development.
> In a real production environment it should be stored outside the repository, for example in user secrets, environment variables, or a secret manager.

---

## Getting Started

### Requirements

- .NET 10 SDK or newer

### Run the API

```bash
dotnet restore
dotnet run
```

After the application starts, the API documentation is available through Scalar:

```text
https://localhost:<port>/scalar/v1
```

The exact port depends on the local launch profile.

---

## Default Administrator Account

The application creates an initial administrator account in the in-memory auth repository.

```text
Email: admin@mediastore.local
Password: Admin123!
```

After a successful login, the `/api/auth/login` endpoint returns a JWT access token.

Use it in protected requests as:

```http
Authorization: Bearer <token>
```

---

## API Endpoints

### Auth

```http
POST /api/auth/login
POST /api/auth/register
```

### Products

```http
GET    /api/products
GET    /api/products/{id}
POST   /api/products
DELETE /api/products/{id}
```

Protected endpoints:

```http
POST   /api/products
DELETE /api/products/{id}
```

require the `Admin` role.

### Admin

```http
GET    /api/admin/users
POST   /api/admin/users/{id}/approve
DELETE /api/admin/users/{id}
GET    /api/admin/settings/registration
POST   /api/admin/settings/registration
```

All admin endpoints require the `Admin` role.

---

## Example Requests

### Login

```http
POST /api/auth/login
Content-Type: application/json
```

```json
{
  "email": "admin@mediastore.local",
  "password": "Admin123!"
}
```

### Get Products

```http
GET /api/products?pageNumber=1&pageSize=10&search=tv&sortBy=name&sortDirection=asc
```

### Create Product

```http
POST /api/products
Authorization: Bearer <token>
Content-Type: application/json
```

```json
{
  "code": "TV001",
  "name": "Smart TV 55",
  "price": 2499.99,
  "imageUrl": "https://example.com/image.jpg",
  "descriptions": {
    "pl": "Nowoczesny telewizor Smart TV 55 cali.",
    "en": "A modern 55-inch Smart TV.",
    "de": "Ein moderner 55-Zoll-Smart-TV.",
    "cs": "Moderní 55palcová Smart TV."
  }
}
```

### Get Product Details

```http
GET /api/products/{id}?language=pl
```

If the requested description language is missing, the API falls back to:

```text
requested language -> English -> first available description
```

### Delete Product

```http
DELETE /api/products/{id}
Authorization: Bearer <token>
```

---

## Validation and Error Handling

The API returns stable error codes instead of localized text messages.

Example validation response:

```json
{
  "errors": {
    "Code": ["Error.Product.Code.AlreadyExists"]
  }
}
```

This allows client applications to translate messages on their side using i18n.

Example error codes:

```text
Error.Product.Code.Required
Error.Product.Code.AlreadyExists
Error.Product.Price.GreaterThanZero
Error.Product.NotFound
Error.Auth.InvalidCredentials
Error.Auth.User.NotActive
Error.Auth.Registration.Disabled
```

---

## Product Initializer

If enabled in configuration:

```json
{
  "DatabaseInitializer": {
    "Enabled": true,
    "FilePath": "Seed/products.seed.json"
  }
}
```

the application attempts to load seed products from:

```text
Seed/products.seed.json
```

The initializer runs only when the product repository is empty.

---

## Design Decisions

### Minimal API

The project uses Minimal API because the endpoint structure is compact and feature-based. This avoids unnecessary controller boilerplate while keeping the code readable.

### In-Memory Persistence by Default

The default implementation uses in-memory repositories, which makes the application easy to run locally without external infrastructure.

At the same time, persistence is abstracted behind repository interfaces, so the project can be extended to use a real SQL database without changing the API layer.

### Result Pattern

A simple Result Pattern is used for predictable business errors, such as duplicate product codes or attempts to delete unavailable resources.

### Error Codes

The backend returns stable error codes instead of translated messages. This keeps the API language-neutral and allows client applications to handle localization independently.

### Feature-Based Structure

The code is grouped around business features such as products, authentication, and administration. This makes the project easier to navigate and extend.

---

## Possible Future Improvements

- SQL persistence with EF Core
- Database migrations
- Refresh tokens
- Unit and integration tests
- Docker support
- Rate limiting for authentication endpoints
- More advanced role and permission management
- Image upload instead of external image URLs
- Full audit logging for administrator actions
