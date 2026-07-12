---
apply: always
---

## Project Structure

The solution follows **Clean Architecture** and is organized into layered projects that each have a single responsibility.

mermaid
graph TD
A[Solution Root] --> B[LetSikkeryd.Shell]
A --> C[LetSikderhed.Application]
A --> D[LetSikderhed.Infrastructure]
A --> E[LetSikderhed.Domain]
A --> F[LetSikderhed.Tests]

    B -.-> |Web UI| G[ASP.NET Core MVC]
    C -.-> |Persistence| H[EF Core, NoSQL] 
    D -.-> |Use Cases| I[Application Services] 
    E -.-> |Entities*| J[Core Domain Model]

### Main Projects

| Project | Purpose | Primary Language |
|---------|----------|-------------------|
| **LetSikderhed.Shell** | Presentation layer – MVC controllers, Razor views, routing, authentication | C# (ASP.NET Core) |
| **LetSikderhed.Application** | Application services that implement use‑cases; orchestrates domain objects | C# |
| **LetSikderhed.Infrastructure** | Technical infrastructure – persistence (EF Core), external integrations, caching | C# |
| **LetSikderhed.Domain** | Business core – entities, value objects, aggregates, domain events | C# |

### Layered Responsibilities

- **Shell (Outer Circle)** – Handles HTTP requests, MVC routing, Razor rendering.
- **Application (Inner Circle)** – Contains application services that coordinate use‑case workflows without containing business rules.
- **Infrastructure (Inner Circle)** – Provides concrete implementations of interfaces defined in the domain/application layers (e.g., repositories, DbContext).
- **Domain (Core)** – Pure business logic; no dependencies on outer layers.

### Technology Stack

- **Framework:** ASP.NET Core MVC with Razor View Engine.
- **Target Framework:** .NET 10 (`net10.0`).
- **C# Language Version:** C# 14.0.
- **Architectural Pattern:** Clean Architecture (Dependency direction outward).

This structure keeps concerns separated, makes the core domain testable in isolation, and allows easy substitution of technologies such as persistence mechanisms or UI frameworks without affecting business logic.