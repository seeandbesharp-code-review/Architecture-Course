# Repository Guidelines

## Overview
This document provides guidelines for implementing the Repository pattern in the ChineseRaffleApi project. Repositories handle data access logic, abstracting the underlying database operations from the business logic in services.

## Principles
- **Separation of Concerns**: Keep data access separate from business logic.
- **Interface-Based**: Define repository interfaces in `Repository/DI/` for dependency injection.
- **Async Operations**: Use async methods for all database operations.
- **Entity Framework Core**: Use EF Core for ORM; avoid raw SQL unless necessary.
- **No Business Logic**: Repositories should only handle CRUD and queries; business rules belong in services.

## Structure
- **Interfaces**: Place in `Repository/DI/` (e.g., `IUserRepo.cs`).
- **Implementations**: Place in `Repository/` (e.g., `UserRepo.cs`).
- **Injection**: Register in `Program.cs` with `AddScoped`.

## Naming Conventions
- Interface: `I<Entity>Repo` (e.g., `IUserRepo`)
- Implementation: `<Entity>Repo` (e.g., `UserRepo`)
- Methods: Use descriptive names like `GetUserByIdAsync`, `AddUserAsync`, `UpdateUserAsync`, `DeleteUserAsync`.

## Common Patterns
- **CRUD Methods**:
  - `Task<Entity> GetByIdAsync(int id)`
  - `Task<IEnumerable<Entity>> GetAllAsync()`
  - `Task AddAsync(Entity entity)`
  - `Task UpdateAsync(Entity entity)`
  - `Task DeleteAsync(int id)`
- **Query Methods**: For specific queries, e.g., `GetUsersByRoleAsync(string role)`.
- **Include Related Data**: Use `Include` for eager loading in EF queries.
- **Caching with TTL**: Implement Cache-Aside pattern for GET requests:
  - First, check Redis cache for the key.
  - If found, return cached data.
  - If not found, fetch from database and store in Redis with TTL from `RedisSettings.DefaultTTL`.
- **Cache Invalidation**: For data consistency, any POST, PUT, or DELETE operation must invalidate (delete) the relevant cache key. Implement this logic in the Service Layer to ensure all data changes trigger cache clearing.

## Dependency Injection
- Inject `MyContext` (or the specific DbContext) into repository constructors.
- Ensure repositories are registered as scoped services.

## Redis Configuration
- Define `RedisSettings` in `appsettings.json`:
  ```json
  "RedisSettings": {
    "ConnectionString": "localhost:6379",
    "Password": "YourSecurePassword",
    "DefaultTTL": 300
  }
  ```
- Use these settings for Redis connection and caching TTL.

## Error Handling
- Let exceptions bubble up; handle in services or controllers.
- Use EF Core's built-in validation and constraints.

## Testing
- Mock repository interfaces in unit tests for services.
- Use in-memory database for integration tests.

## Future Considerations
- In a microservice architecture, repositories will be scoped to their service's database.
- Consider using generic repositories for common CRUD if patterns emerge.