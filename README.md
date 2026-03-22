# Mouts Sales API

A RESTful API for managing sales records, built as a technical evaluation. Implements a complete sales CRUD with quantity-based discount rules, individual item cancellation, and domain event publishing.

---

## Table of Contents

- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Accessing the Application](#accessing-the-application)
- [Seed Data](#seed-data)
- [Domain Events](#domain-events)
- [Manual Testing with Postman](#manual-testing-with-postman)

---

## Tech Stack

- **.NET 8** / C# / ASP.NET Core Web API
- **PostgreSQL** — relational database (via EF Core 8)
- **MongoDB** — document database
- **MediatR** — CQRS pattern (Commands / Queries / Handlers)
- **AutoMapper** — object mapping
- **FluentValidation** — request validation
- **Rebus** — domain event publishing
- **xUnit + NSubstitute + Bogus** — unit testing
- **Docker / Docker Compose** — containerization

---

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- Port `5432` free (PostgreSQL)
- Port `8080` free (API)

### Run with Docker Compose (recommended)

```bash
cd template/backend

# First run or after code changes
docker-compose up --build -d

# Subsequent runs (no code changes)
docker-compose up -d
```

> Migrations and seed data are applied automatically on startup. No manual `dotnet ef database update` needed.

### Stop containers

```bash
docker-compose down
```

### Run locally (Visual Studio / Rider)

1. Start only the database:
   ```bash
   cd template/backend
   docker-compose up ambev.developerevaluation.database -d
   ```
2. Set the environment to `Development` (default in Visual Studio)
3. Run the `Ambev.DeveloperEvaluation.WebApi` project

The `appsettings.Development.json` already points to `localhost:5432` with the correct credentials.

---

## Accessing the Application

| Resource | URL |
|---|---|
| **Swagger UI** | http://localhost:8080/swagger |
| **API Base URL** | http://localhost:8080/api |
| **Health Check** | http://localhost:8080/health |

---

## Seed Data

The database is seeded automatically on first startup.

### Branches

| Name | ID |
|---|---|
| Branch North | `11111111-1111-1111-1111-111111111111` |
| Branch South | `22222222-2222-2222-2222-222222222222` |
| Branch East | `33333333-3333-3333-3333-333333333333` |
| Branch West | `44444444-4444-4444-4444-444444444444` |
| Branch Central | `55555555-5555-5555-5555-555555555555` |

### Products

| Product | ID |
|---|---|
| Skol 350ml | `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa` |
| Brahma 350ml | `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb` |
| Antarctica 600ml | `cccccccc-cccc-cccc-cccc-cccccccccccc` |
| Stella Artois 550ml | `dddddddd-dddd-dddd-dddd-dddddddddddd` |
| Budweiser 350ml | `eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee` |
| Original 600ml | `ffffffff-ffff-ffff-ffff-ffffffffffff` |
| Bohemia 600ml | `11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa` |
| Corona 330ml | `22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa` |
| Guaraná Antarctica 2L | `33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa` |
| H2OH! Limão 500ml | `44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa` |

> To create a sale, use any Branch ID and Product ID from the tables above. The `customerId` must be the ID of a user created via `POST /api/users`.

---

## Domain Events

The application publishes domain events via **Rebus**. Events are logged to the application console and can be extended to integrate with external systems.

| Event | Triggered when |
|---|---|
| `SaleCreated` | A new sale is created |
| `SaleModified` | A sale item is updated |
| `SaleCancelled` | A sale is cancelled |
| `ItemCancelled` | A specific item is cancelled |

---

## Manual Testing with Postman

The `.doc/` directory contains two ready-to-import Postman collections:

| File | Description |
|---|---|
| `.doc/user-setup-collection.json` | Creates a test user and saves the `userId` as a collection variable |
| `.doc/sales-crud-collection.json` | Full Sales CRUD: create, list, filter, paginate, sort, update and cancel |

### Recommended flow

1. Import both collections into Postman
2. Run `user-setup-collection.json` — the `userId` is saved automatically
3. Paste the `userId` as the `customerId` variable in the Sales collection
4. Run requests individually or by folder

The Sales collection covers **21 scenarios** across 7 folders: Create, Read, List & Filter, Pagination, Ordering, Update and Cancel.
