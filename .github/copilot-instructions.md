# Copilot Onboarding Instructions

## Repository summary
This repository is a single ASP.NET Core Web API named `ChineseRaffleApi`. It provides backend services for a raffle management system, including user authentication, raffle tickets, donors, gifts, categories, baskets, and statistics.

## Primary focus
- Use existing ASP.NET Core patterns in `Program.cs`, `Controllers/`, `Services/`, `Repository/`, and `Mapping/`.
- Avoid adding new platform/tooling assumptions; this repo is built with `dotnet` only.
- Do not rely on CI or workflow automation; `.github/workflows/` is empty.

## Use the companion guide
For detailed build/run commands, project structure, environment guidance, and conventions, see:

- `.github/copilot-guidance.md`

## Future architecture planning
For the planned microservice split, see:

- `.github/microservice-split-plan.md`

## Coding guidelines
For specific guidelines on implementing controllers and repositories, see:

- `Controllers/README.md`
- `Repository/README.md`

## DevOps Practice
For Redis debugging in development:
- Enter the Redis container: `docker exec -it <container_id> redis-cli`
- Authenticate: `AUTH YourSecurePassword`
- Verify data: `KEYS *` to list keys, `GET <key>` to get value, `TTL <key>` to check expiration.

## Important note
This repo has no `README.md`, `CONTRIBUTING.md`, or `.editorconfig`. Base your changes on the actual source code and current startup configuration.
