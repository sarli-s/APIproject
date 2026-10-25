# 🛒 WebAPIShop
### **Modern RESTful API | .NET 9 | C# | Layered Architecture**

---

## 📖 Overview
**WebAPIShop** is a professional **REST Web API** built with **.NET 9** and **C#**. The project strictly adheres to **RESTful principles**, providing a standardized and scalable way to interact with data over HTTPS. It is designed with a focus on high performance, maintainability, and **Clean Code**.

It is the backend of a personalized gift store, one of three parts of the project:

| Part | Repository | Tech |
|---|---|---|
| 🖥️ Frontend | [sarli-s/shop-frontend](https://github.com/sarli-s/shop-frontend) | Angular 21, PrimeNG |
| ⚙️ Backend API (this repo) | [sarli-s/APIproject](https://github.com/sarli-s/APIproject) | .NET 9, EF Core, SQL Server, Redis, Kafka |
| 🤖 AI Service | [sarli-s/AI-Services-Chat-and-Semantics](https://github.com/sarli-s/AI-Services-Chat-and-Semantics) | Python, FastAPI, OpenAI |

---

## 🏗️ Architecture & Design Patterns

The project is structured using a **Layered Architecture** to achieve total **Separation of Concerns**:

📱 **Application Layer** – Handles API controllers, routing, and ensures **REST principles** are followed.

⚙️ **Service Layer** – Contains all **Business Logic**, facilitating communication between layers.

🗄️ **Repositories Layer** – Manages **Data Access** logic and database communication.

### Key Technical Features:
💉 **Dependency Injection (DI):** Implemented across all layers to create **Decoupling** and improve system flexibility.

⚡ **Asynchronous Programming:** Database access is handled **Asynchronously** to free up threads and ensure maximum **Scalability**.

🗃️ **Entity Framework Core (ORM):** Developed using the **DB-First** approach for efficient data management.

📦 **DTOs & Records:** Uses **C# Records** for **Data Transfer Objects** to remove circular dependencies and decouple the Data layer from the API layers.

🔄 **AutoMapper:** Used for automatic and clean mapping between Database entities and DTOs.

⚙️ **Configuration:** Settings are managed via `appsettings.json` and environment variables to keep the code clean and environment-flexible.

---

## 📁 Project Structure

```text
├── WebAPIShop/           # Entry point, controllers, middleware
├── Servers/              # Business logic (Service Layer)
├── Repositories/         # Data access implementations
├── Entities/             # Domain models (EF Core DB-First)
├── DTOs/                 # Record-based data transfer objects
├── KafkaConsumer/        # Standalone background worker (Kafka consumer)
├── TestProject1/         # xUnit test projects (Unit & Integration)
├── Database/             # create-database.sql: tables + starter data
├── docker-compose.yml    # Docker orchestration (API, Redis, Kafka, Kafka UI)
└── Dockerfile            # API container definition
```

---

## 🔌 API Endpoints

| Method | Route | Access |
| :--- | :--- | :--- |
| `GET` | `/api/products` (filters: `name`, `minPrice`, `maxPrice`, `categoriesId`, `limit`, `orderby`, `offset`) | Anyone |
| `GET` | `/api/products/{id}` | Anyone |
| `POST` / `PUT` / `DELETE` | `/api/products`, `/api/products/{id}` | Admin |
| `GET` | `/api/categories` | Anyone |
| `POST` | `/api/users` (register) | Anyone |
| `POST` | `/api/users/login`, `/api/users/logout` | Anyone / logged in |
| `PUT` | `/api/users/{id}` | The user themself, or Admin |
| `GET` | `/api/users/{id}` | Admin |
| `POST` | `/api/orders` | Logged in |
| `GET` | `/api/orders/user/{userId}` | Logged in |
| `GET` | `/api/orders`, `/api/orders/{id}` | Admin |
| `PUT` | `/api/orders/{id}/status` | Admin |
| `POST` | `/api/password` (strength check) | Anyone |
| `POST` | `/api/chat` | Anyone (AI) |
| `GET` | `/api/search?query=...&topK=5` | Anyone (AI) |
| `POST` | `/api/search/seed` | Admin (AI) |

Full, interactive documentation: **Swagger** at `http://localhost:5222/swagger` (Development).

---

## 🛡️ Security

| Feature | Description |
| :--- | :--- |
| **JWT Authentication** | Stateless token-based authentication using `Microsoft.AspNetCore.Authentication.JwtBearer`. Tokens are validated on every request (issuer, audience, lifetime, signing key). |
| **JWT Cookie Support** | A custom `JwtCookieMiddleware` transparently extracts the JWT from an `HttpOnly` cookie and injects it into the `Authorization` header, supporting both browser and API clients. |
| **Role-Based Authorization** | Custom `AuthorizeRolesAttribute` and an `AdminOnly` policy enforce access control at the endpoint level. New users always get the `User` role, and a profile update can't change the role. |
| **CORS** | Only the Angular client (`http://localhost:4200`) may call the API from a browser, with credentials (the JWT cookie). |
| **Password Hashing** | Passwords are hashed using **BCrypt** (salt embedded). Verification uses constant-time comparison to prevent timing attacks. |
| **Password Strength Validation** | **Zxcvbn** library evaluates password strength and returns a score before storing. |
| **Rate Limiting** | A sliding-window rate limiter (30 req/min per user+IP) returns `429 Too Many Requests` on breach, protecting against abuse and DDoS. |

---

## 🔄 Messaging — Apache Kafka

Orders are published as events to a Kafka topic immediately after creation, enabling asynchronous, decoupled processing:

- **Producer (`KafkaProducerService`):** Runs inside the main API. On order creation, serializes the `OrderDTO` and publishes it to the configured topic, partitioned by `userId`.
- **Consumer (`KafkaConsumer` project):** A standalone **.NET Worker Service** that subscribes to the same topic, deserializes order events, and logs them via NLog. Runs independently from the main API.
- **Kafka UI:** Available at `http://localhost:8090` (via Docker) for real-time topic and message inspection.

---

## ⚡ Caching — Redis

**Redis** distributed cache is integrated via `Microsoft.Extensions.Caching.StackExchangeRedis`. User profiles are cached (TTL from `Redis:CacheTTLMinutes`) and the entry is removed when the user is updated or deleted. The Redis instance is managed via Docker Compose with password authentication.

---

## 🤖 AI — Chat & Semantic Search

The API calls the Python [AI service](https://github.com/sarli-s/AI-Services-Chat-and-Semantics) on `http://127.0.0.1:8001`:

- **Chat (`/api/chat`):** forwards the conversation and the product catalog to the AI gift assistant.
- **Semantic search (`/api/search`):** returns the products closest in meaning to a free-text query (OpenAI embeddings).
- **Seeding:** on startup, and after every product add/update/delete, the API sends the catalog to the AI service so its embeddings stay current.

Start the AI service before the API: chat, search, and the admin's product add/edit/delete depend on it. Browsing products, users and orders work without it.

---

## 🛠️ Reliability & Monitoring

| Feature | Description |
| :--- | :--- |
| **Global Error Handling** | A custom `ErrorHandlingMiddleware` intercepts all unhandled exceptions globally, returning consistent API error responses. |
| **NLog Integration** | Structured logging across all layers — info, warnings, and errors. Includes `NLog.MailKit` for email alert notifications on critical errors. |
| **Traffic Monitoring** | `RatingMiddleware` tracks all incoming requests and logs them to a dedicated Rating table for auditing and analytics. |

---

## 🐳 Docker & Containerization

The full stack runs via a single `docker-compose up`:

| Container | Description |
| :--- | :--- |
| `webapishop_api` | The .NET 9 Web API |
| `my_redis_cache` | Redis (password-protected, with healthcheck) |
| `kafka` | Apache Kafka in KRaft mode (no ZooKeeper) |
| `kafka-ui` | Kafka UI dashboard at `http://localhost:8090` |

Sensitive values (`JWT_KEY`, `DB_CONNECTION_STRING`) are injected via environment variables — never hardcoded.

---

## 🧪 Testing Suite

High reliability maintained using **xUnit** with a comprehensive testing strategy:

✅ **Unit Tests:** Validate individual business logic units in isolation (Repositories, Services).
✅ **Integration Tests:** Ensure the full data flow between layers and the database works end-to-end.

Covered: `User`, `Product`, `Category`, `Order`, `Rating` repositories (unit + integration), plus `OrdersService` (server-side price check, Kafka publish) and `PasswordService` (BCrypt) — **38 tests**.

Unit tests mock the `DbContext` with **Moq** / **Moq.EntityFrameworkCore**. Integration tests create a temporary database `215949413_SHOP_Test` on the local SQL Server (`Server=.`) and delete it when they finish.

---

## 🛠️ Tech Stack

| Category | Technology |
| :--- | :--- |
| **Framework** | .NET 9 |
| **Language** | C# |
| **ORM** | Entity Framework Core (DB-First) |
| **Mapping** | AutoMapper |
| **Authentication** | JWT Bearer + Cookie middleware |
| **Password** | BCrypt + Zxcvbn |
| **Caching** | Redis (StackExchange.Redis) |
| **Messaging** | Apache Kafka (Confluent.Kafka) |
| **Rate Limiting** | ASP.NET Core Rate Limiter (Sliding Window) |
| **Logging** | NLog + NLog.MailKit |
| **API Docs** | Swagger / OpenAPI |
| **Testing** | xUnit |
| **Containerization** | Docker + Docker Compose |

---

## 🚀 Getting Started

### Prerequisites
- **.NET 9 SDK**
- **SQL Server** (local default instance, or any connection string)
- **Docker Desktop** (for Redis and Kafka)
- **Python 3.12** for the [AI service](https://github.com/sarli-s/AI-Services-Chat-and-Semantics) (chat and search)

### 1. Create the database
The project is DB-First (no EF migrations). Create an empty database and run [Database/create-database.sql](Database/create-database.sql) in it — it creates the tables and adds 5 categories and 20 products:
```bash
sqlcmd -S . -E -Q "CREATE DATABASE API_SHOP"
sqlcmd -S . -E -d API_SHOP -f 65001 -i Database/create-database.sql
```

### 2. Configure local settings
Copy [WebAPIShop/appsettings.Development.example.json](WebAPIShop/appsettings.Development.example.json) to `WebAPIShop/appsettings.Development.json` and fill in your values (connection string, a random JWT key of 32+ characters). This file is git-ignored — secrets never go into the repo.

### 3. Start Redis and Kafka
```bash
docker compose up -d my_redis_cache kafka kafka-ui
```

### 4. Start the AI service (port 8001)
See the [AI service README](https://github.com/sarli-s/AI-Services-Chat-and-Semantics#readme).

### 5. Run the API
```bash
dotnet run --project WebAPIShop          # http://localhost:5222/swagger
dotnet run --project KafkaConsumer       # optional, separate terminal
```
**Visual Studio:** right-click `WebAPIShop` → *Set as Startup Project*, choose the **http** profile, press F5.

### 6. Run the frontend
See the [frontend README](https://github.com/sarli-s/shop-frontend#readme) — it expects the API on `http://localhost:5222`.

### Run everything in Docker
`docker compose up --build` also builds the API container. It reads `JWT_KEY` and `DB_CONNECTION_STRING` from environment variables (or a `.env` file next to `docker-compose.yml`); the database must be reachable from the container.

### 🧪 Run Tests
```bash
dotnet test
```

---
**Ayala & Sarli**
<small>2026</small>
