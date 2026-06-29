# 🛒 WebAPIShop
### **Modern RESTful API | .NET 9 | C# | Layered Architecture**

---

## 📖 Overview
**WebAPIShop** is a professional **REST Web API** built with **.NET 9** and **C#**. The project strictly adheres to **RESTful principles**, providing a standardized and scalable way to interact with data over HTTPS. It is designed with a focus on high performance, maintainability, and **Clean Code**.

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
├── docker-compose.yml    # Docker orchestration (API, Redis, Kafka, Kafka UI)
├── Dockerfile            # API container definition
└── appsettings.json      # External configuration
```

---

## 🛡️ Security

| Feature | Description |
| :--- | :--- |
| **JWT Authentication** | Stateless token-based authentication using `Microsoft.AspNetCore.Authentication.JwtBearer`. Tokens are validated on every request (issuer, audience, lifetime, signing key). |
| **JWT Cookie Support** | A custom `JwtCookieMiddleware` transparently extracts the JWT from an `HttpOnly` cookie and injects it into the `Authorization` header, supporting both browser and API clients. |
| **Role-Based Authorization** | Custom `AuthorizeRolesAttribute` and an `AdminOnly` policy enforce access control at the endpoint level. |
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

**Redis** distributed cache is integrated via `Microsoft.Extensions.Caching.StackExchangeRedis`. Frequently accessed data (e.g., product listings) is cached to reduce database load and improve response times. The Redis instance is managed via Docker Compose with password authentication.

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

Covered: `User`, `Product`, `Category`, `Order`, `Rating` — both unit and integration test classes per domain.

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
- **Docker Desktop** (for Redis, Kafka)
- A SQL Server instance (or connection string via env variable)

### Run with Docker
```bash
# Start Redis, Kafka, Kafka UI, and the API
docker-compose up --build
```

### Run locally
```bash
# Restore dependencies
dotnet restore

# Apply migrations / Update database
dotnet ef database update

# Run the API
dotnet run --project WebAPIShop

# Run the Kafka consumer (separate terminal)
dotnet run --project KafkaConsumer
```

### 🧪 Run Tests
```bash
dotnet test
```

---

## 📄 License

This project is licensed under the **MIT License**.

---
**Ayala & Sarli**
<small>2026</small>
