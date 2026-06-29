# Microservices Architecture Planning Document

## Project Overview

WebAPIShop is a modern e-commerce platform built with .NET 9 and C#, currently structured as a layered monolith. This document outlines a plan to evolve the system into a microservices architecture, improving scalability, maintainability, and independent deployment. The proposed design decomposes the current API into focused, autonomous services based on the existing controllers, models, and domain boundaries.

---

## Microservices Breakdown

### 1. **User Service**
- **Responsibility:** Manage user registration, authentication, profiles, and roles.
- **Endpoints:**
  - `POST /api/users/register`  Register a new user
  - `POST /api/users/login`  Authenticate user and issue JWT
  - `GET /api/users/{id}`  Get user profile
  - `PUT /api/users/{id}`  Update user profile
  - `DELETE /api/users/{id}`  Soft-delete user (sets `IsDeleted = true`; does not physically remove records to preserve order history integrity)
- **Data Ownership:** User accounts, credentials, roles, and profile data
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL, JWT, NLog
- **Health Check:** `GET /health`

---

### 2. **Product Service**
- **Responsibility:** Manage product catalog (including CRUD and search) and product categories. Category is part of the product domain and does not warrant a separate service.
- **Endpoints:**
  - `GET /api/products`  List/search products
  - `GET /api/products/{id}`  Get product details
  - `POST /api/products`  Create product
  - `PUT /api/products/{id}`  Update product
  - `DELETE /api/products/{id}`  Delete product
  - `GET /api/categories`  List categories
  - `GET /api/categories/{id}`  Get category details
  - `POST /api/categories`  Create category
  - `PUT /api/categories/{id}`  Update category
  - `DELETE /api/categories/{id}`  Delete category
- **Data Ownership:** Product details, inventory, pricing, images, category definitions and relationships
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL, Redis (for caching), NLog
- **Health Check:** `GET /health`

---

### 3. **Order Service**
- **Responsibility:** Manage customer orders, order status, and order history.
- **Endpoints:**
  - `POST /api/orders`  Create new order
  - `GET /api/orders/{id}`  Get order details
  - `GET /api/orders/user/{userId}`  List orders for a user
  - `PUT /api/orders/{id}/status`  Update order status
- **Data Ownership:** Orders, order items, order status, payment references. Denormalizes product name/price at order time for resilience.
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL, NLog
- **Health Check:** `GET /health`

---

### 4. **Review Service**
- **Responsibility:** Manage product ratings and reviews. API traffic analytics and monitoring are an infrastructure concern handled at the API Gateway / observability layer — not here.
- **Endpoints:**
  - `POST /api/ratings`  Submit a rating/review
  - `GET /api/ratings/product/{productId}`  Get ratings for a product
  - `GET /api/ratings/user/{userId}`  Get ratings by a user
- **Data Ownership:** Ratings and review content
- **Technology Suggestions:** .NET 9 Web API, Entity Framework Core, SQL Server/PostgreSQL
- **Health Check:** `GET /health`

---

## Communication Patterns

- **API Gateway:** Expose a unified entry point for clients, routing requests to appropriate services. Also responsible for traffic monitoring, rate limiting, and request logging (replaces the previous analytics concern in the Review Service).
- **Synchronous HTTP (REST):** Services communicate via RESTful HTTP APIs for most operations.
- **Asynchronous Messaging (optional):** Use a message broker (e.g., RabbitMQ, Azure Service Bus) for cross-service events such as `OrderPlaced` or `UserDeleted` to decouple services and enable eventual consistency.
- **Service Discovery:** Use a service registry (e.g., Consul, Eureka) if dynamic service location is needed.

---

## Authentication & Authorization Flow

- The **User Service** is the JWT authority — it issues signed tokens on login.
- **All other services validate JWTs locally** using the shared public key (asymmetric RS256) or shared secret (symmetric HS256). They do **not** call the User Service on every request.
- The public key / secret is distributed via environment variables or a secrets manager (see Configuration section below).
- The **API Gateway** may optionally perform a first-pass token validation before forwarding requests downstream.

---

## Data Ownership & Boundaries

- **Each service owns its own database schema** and is responsible for its data integrity.
- **No direct database sharing** between services; all cross-service data access must go through service APIs.
- **Data duplication** (e.g., product name and price in order records) is intentional for denormalization and resilience.
- **Soft deletes** are used where referential integrity across services matters (e.g., deleting a user must not break order or review history).

---

## Configuration Strategy

All services follow the same configuration hierarchy:

1. `appsettings.json` — baseline defaults (no secrets)
2. Environment variables — override per environment (dev / staging / prod)
3. Secrets manager — sensitive values such as JWT keys, connection strings (e.g., AWS Parameter Store, Azure Key Vault, HashiCorp Vault)

No secrets are committed to source control.

---

## Technology Suggestions

- **.NET 9 Web API** for all services (consistent with current stack)
- **Entity Framework Core** for data access
- **SQL Server or PostgreSQL** as primary data stores
- **Redis** for caching in Product Service
- **NLog** for logging
- **JWT (RS256)** for authentication (User Service as the authority; downstream services validate locally)
- **API Gateway** (e.g., YARP, Ocelot, or Azure API Management) — also handles traffic monitoring and analytics
- **Docker** for containerization
- **Kubernetes** for orchestration (optional, for large-scale deployments)
- **Swagger/OpenAPI** for API documentation
- **Health checks** (`/health`) on every service for liveness and readiness probes

---

## Summary

This microservices plan decomposes WebAPIShop into four focused, independently deployable services aligned with business domains: User, Product (including categories), Order, and Review. Categories are part of the Product domain and do not need a separate service. API traffic analytics belong at the API Gateway layer, not in a business service. Each service owns its data, validates JWTs locally, exposes a `/health` endpoint, and reads configuration from environment variables or a secrets manager. Communication is primarily RESTful, with optional asynchronous messaging for decoupled workflows.

---
