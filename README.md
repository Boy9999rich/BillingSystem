# Billing System – Event Driven Backend

This project is a backend billing system designed with a production-oriented
architecture using Clean Architecture principles and an event-driven approach.

---

## 🏗 Architecture Overview

The system follows **Clean Architecture** with a modular monolith structure.

### Layers:

- **Billing.API**
  - REST endpoints
  - Request/response handling
  - Publishes usage events

- **Billing.Application**
  - Business logic
  - Services, DTOs, interfaces
  - Billing and balance calculation logic

- **Billing.Domain**
  - Core domain entities
  - Business rules
  - No external dependencies

- **Billing.Infrastructure**
  - Database (Entity Framework Core)
  - Kafka-ready producer/consumer structure

### Event Flow:

1. API receives a usage request
2. Usage event is published
3. Consumer processes the event
4. Billing record is created
5. User balance is updated atomically
6. Balance can be retrieved via API

---

## 📡 Event Processing (Kafka-ready)

The system is designed around Kafka concepts:

- Producer / Consumer pattern
- Background worker for event consumption
- Idempotent event handling
- Manual commit strategy (conceptually)

⚠️ **Note**  
Due to local environment limitations, Kafka was replaced with an
in-memory event queue.  
The architecture remains fully Kafka-compatible and can be switched
without changes to business logic.

---

## 🗄 Database

- Microsoft SQL Server
- Entity Framework Core
- Transactions used to ensure consistency
- Unique constraint on EventId to prevent duplicate processing

---

## 🚀 How to Run the Project

### Prerequisites:
- .NET 8 SDK
- SQL Server (local or remote)

### Steps:

1. Clone the repository:
```bash
git clone https://github.com/<your-username>/BillingSystem.git
