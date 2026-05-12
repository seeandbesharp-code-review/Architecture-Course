# Microservice Split Plan

## Overview
This document outlines the architectural design for splitting the monolithic ChineseRaffleApi into a microservice-based system. The goal is to achieve better scalability, maintainability, and separation of concerns by dividing the application into four bounded contexts: Identity, Raffle, Inventory, and Analytics.

The current monolith has 9 controllers, 6 entity models, 10 services, and 6 repositories, all sharing a single SQL Server database. The split will assign each microservice its own database schema and API endpoints.

## Microservices Architecture

### 1. Identity Microservice
**Purpose**: Manages user authentication, registration, and user data.

**Owned Entities**:
- User

**Services**:
- UserService (CRUD operations)
- TokenService (JWT generation)

**Controllers**:
- AuthController (login, register)
- UserController (user management, admin-only)

**Database Schema**:
- Users table (with unique UserName index)

**APIs**:
- POST /api/auth/login
- POST /api/auth/register
- GET /api/user (admin)
- GET /api/user/{id} (admin)
- PUT /api/user/{id} (admin)
- DELETE /api/user/{id} (admin)

**Dependencies**: None (standalone)

### 2. Raffle Microservice
**Purpose**: Handles raffle logic, ticket purchasing, basket management, and raffle drawing.

**Owned Entities**:
- Ticket
- Basket

**Services**:
- RaffleService (drawing logic, winner assignment)
- BasketService (basket CRUD)
- TicketService (ticket CRUD)

**Controllers**:
- RaffleController (raffle operations, admin)
- BasketController (basket management)
- TicketController (ticket management)

**Database Schema**:
- Tickets table
- Baskets table

**APIs**:
- GET /api/raffle/download-raffle-zip (admin)
- GET /api/basket
- POST /api/basket
- PUT /api/basket/{id}
- DELETE /api/basket/{id}
- GET /api/ticket
- POST /api/ticket
- PUT /api/ticket/{id}
- DELETE /api/ticket/{id}

**Dependencies**:
- Calls Identity service for user validation
- Calls Inventory service for gift details

### 3. Inventory Microservice
**Purpose**: Manages gifts, donors, and categories.

**Owned Entities**:
- Gift
- Donor
- Category

**Services**:
- GiftService (gift CRUD)
- DonorService (donor CRUD)
- CategoryService (category CRUD)

**Controllers**:
- GiftController (gift management)
- DonorController (donor management)
- CategoryController (category management)

**Database Schema**:
- Gifts table (with unique Title index, foreign keys to Donor and Category)
- Donors table
- Categories table

**APIs**:
- GET /api/gift
- POST /api/gift
- PUT /api/gift/{id}
- DELETE /api/gift/{id}
- GET /api/donor
- POST /api/donor
- PUT /api/donor/{id}
- DELETE /api/donor/{id}
- GET /api/category
- POST /api/category
- PUT /api/category/{id}
- DELETE /api/category/{id}

**Dependencies**: None (standalone)

### 4. Analytics Microservice
**Purpose**: Provides aggregated statistics and reports.

**Owned Entities**: None (reads from other services or has its own aggregated tables)

**Services**:
- RaffleStatisticsService (summary calculations)

**Controllers**:
- StatisticsController (stats endpoints)

**Database Schema**:
- Possibly replicated or aggregated tables from other services (e.g., ticket counts, revenues)

**APIs**:
- GET /api/statistics/summary

**Dependencies**:
- Reads data from Raffle and Inventory services via APIs

## Shared Components
- **EmailService**: A shared utility service for sending emails (e.g., raffle winners). Can be a separate microservice or a shared library.
- **JWT Authentication**: All services validate JWT tokens issued by Identity service.
- **Logging**: Each service uses Serilog for structured logging.

## Communication Patterns
- **Synchronous**: REST API calls between services (e.g., Raffle calls Identity for user info).
- **Asynchronous**: Consider message queues (e.g., RabbitMQ) for events like raffle completion.
- **API Gateway**: A gateway service to route client requests to appropriate microservices and handle authentication.

## Database Design
- Each microservice has its own SQL Server database/schema to ensure autonomy.
- No shared database; data is replicated or accessed via APIs where needed.
- Foreign keys within a service's schema only.

## Migration Strategy
1. **Data Splitting**: Extract data from monolithic DB into separate schemas.
2. **API Refactoring**: Update controllers to call other services instead of local repos.
3. **Authentication**: Implement JWT validation in each service.
4. **Testing**: Ensure end-to-end functionality after split.
5. **Deployment**: Use containers (Docker) for each microservice.
## Deployment & Orchestration
To deploy the microservices, use Docker for containerization and Docker Compose for orchestration.

### Dockerfile Template
Each microservice will use a multi-stage Dockerfile based on the `Dockerfile.template` in the root directory. This template includes stages for runtime (base), build (SDK), publish, and final image.

### Docker Compose Draft
Below is a draft `docker-compose.yml` for running the 4 microservices and their separate SQL Server databases. This assumes each service is built from its own Dockerfile and exposes ports as needed.

```yaml
version: '3.8'

services:
  identity-db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: Y
      SA_PASSWORD: YourStrong!Passw0rd
    ports:
      - "1433:1433"
    volumes:
      - identity_data:/var/opt/mssql

  raffle-db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: Y
      SA_PASSWORD: YourStrong!Passw0rd
    ports:
      - "1434:1433"
    volumes:
      - raffle_data:/var/opt/mssql

  inventory-db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: Y
      SA_PASSWORD: YourStrong!Passw0rd
    ports:
      - "1435:1433"
    volumes:
      - inventory_data:/var/opt/mssql

  analytics-db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: Y
      SA_PASSWORD: YourStrong!Passw0rd
    ports:
      - "1436:1433"
    volumes:
      - analytics_data:/var/opt/mssql

  redis:
    image: redis:alpine
    command: redis-server --requirepass "Cb0534172647"
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data

  identity-service:
    build:
      context: ./IdentityService
      dockerfile: Dockerfile
    ports:
      - "5001:80"
    environment:
      - ConnectionStrings__DefaultConnection=Server=identity-db;Database=IdentityDb;User Id=sa;Password=YourStrong!Passw0rd;
      - Redis__Password=Cb0534172647
    depends_on:
      - identity-db
      - redis

  raffle-service:
    build:
      context: ./RaffleService
      dockerfile: Dockerfile
    ports:
      - "5002:80"
    environment:
      - ConnectionStrings__DefaultConnection=Server=raffle-db;Database=RaffleDb;User Id=sa;Password=YourStrong!Passw0rd;
      - Redis__Password=Cb0534172647
    depends_on:
      - raffle-db
      - redis

  inventory-service:
    build:
      context: ./InventoryService
      dockerfile: Dockerfile
    ports:
      - "5003:80"
    environment:
      - ConnectionStrings__DefaultConnection=Server=inventory-db;Database=InventoryDb;User Id=sa;Password=YourStrong!Passw0rd;
      - Redis__Password=Cb0534172647
    depends_on:
      - inventory-db
      - redis

  analytics-service:
    build:
      context: ./AnalyticsService
      dockerfile: Dockerfile
    ports:
      - "5004:80"
    environment:
      - ConnectionStrings__DefaultConnection=Server=analytics-db;Database=AnalyticsDb;User Id=sa;Password=YourStrong!Passw0rd;
      - Redis__Password=Cb0534172647
    depends_on:
      - analytics-db
      - redis

volumes:
  identity_data:
  raffle_data:
  inventory_data:
  analytics_data:
  redis_data:
```

This draft assumes separate directories for each service (e.g., `./IdentityService`). Adjust paths and environment variables as needed for production. Use secrets management for passwords.
## Benefits
- Improved scalability: Scale services independently.
- Better maintainability: Changes in one service don't affect others.
- Fault isolation: Failure in one service doesn't bring down the whole system.

## Risks and Considerations
- Increased complexity in communication and data consistency.
- Need for distributed transactions (e.g., raffle drawing updates gift winner).
- Monitoring and logging across multiple services.

## Future Enhancements
- Implement event sourcing for better audit trails.
- Add service mesh (e.g., Istio) for advanced routing and observability.
- Consider CQRS pattern for read/write separation in high-traffic services.