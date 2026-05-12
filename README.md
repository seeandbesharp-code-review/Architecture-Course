# Controller Guidelines

## Overview
This document provides guidelines for implementing API controllers in the ChineseRaffleApi project. Controllers handle HTTP requests, delegate to services, and return responses.

## Principles
- **Thin Controllers**: Keep controllers thin; delegate business logic to services.
- **RESTful Design**: Use standard HTTP methods (GET, POST, PUT, DELETE) and meaningful routes.
- **Validation**: Use model validation attributes and return appropriate status codes.
- **Authorization**: Apply `[Authorize]` attributes with roles where needed.
- **Async**: Use async actions for scalability.

## Structure
- **Location**: Place in `Controllers/` folder.
- **Naming**: `<Feature>Controller` (e.g., `UserController`).
- **Base Class**: Inherit from `ControllerBase` with `[ApiController]`.
- **Routes**: Use `[Route("api/[controller]")]` and attribute routing for actions.

## Common Patterns
- **CRUD Actions**:
  - `GET /api/resource` → `GetAll()`
  - `GET /api/resource/{id}` → `GetById(int id)`
  - `POST /api/resource` → `Create([FromBody] dto)`
  - `PUT /api/resource/{id}` → `Update(int id, [FromBody] dto)`
  - `DELETE /api/resource/{id}` → `Delete(int id)`
- **Response Types**: Use `IActionResult` or specific types like `ActionResult<T>`.
- **Status Codes**: 200 OK, 201 Created, 400 Bad Request, 401 Unauthorized, 403 Forbidden, 404 Not Found, 500 Internal Server Error.

## Dependency Injection
- Inject service interfaces via constructor.
- Services are registered in `Program.cs`.

## Error Handling
- Use try-catch for expected errors; return appropriate responses.
- Log errors with the injected `ILogger`.
- For validation errors, rely on `[ApiController]` automatic 400 responses.

## Security
- Use JWT bearer authentication as configured in `Program.cs`.
- Apply role-based authorization (e.g., `[Authorize(Roles = "Admin")]`).
- Validate user identity from `User` claims.

## Rate Limiting
- Require Sliding Window rate limiting on all controller endpoints to prevent abuse.
- Use the built-in `Microsoft.AspNetCore.RateLimiting` library.
- Define a "sliding" policy with:
  - Window: 1 minute
  - Segments per window: 6
  - Permit limit: 100 requests
- Apply the `[EnableRateLimiting("sliding")]` attribute on all controller classes.
- Configure the policy in `Program.cs` for each microservice.

## DTOs and Mapping
- Use DTOs from `Dto/` for requests/responses.
- Map between entities and DTOs using AutoMapper profiles in `Mapping/`.

## Logging
- Inject `ILogger<TController>` and log important actions or errors.

## Testing
- Use integration tests for controller endpoints.
- Mock services for unit tests.

## Future Considerations
- In microservices, controllers will be per-service with their own APIs.
- Consider API versioning for future changes.