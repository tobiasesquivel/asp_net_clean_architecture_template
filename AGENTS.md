# Marsgram Backend — Agent Context

## Overview

Marsgram is a social media REST API (Instagram-like) built with ASP.NET Core. The project follows DDD (Domain-Driven Design) and CQRS patterns within a single-project structure.

## Tech Stack

| Component          | Technology                              |
| ------------------ | --------------------------------------- |
| Framework          | ASP.NET Core (.NET 10)                  |
| Database           | MySQL via Pomelo.EntityFrameworkCore     |
| ORM                | Entity Framework Core 9.x              |
| Identity           | ASP.NET Core Identity                   |
| Mediator/Messaging | Wolverine (WolverineFx)                 |
| Validation         | FluentValidation + Wolverine.FluentValidation |
| Media Storage      | Cloudinary (CloudinaryDotNet)           |
| Logging            | Serilog (Console sink)                  |
| Error Handling     | ErrorOr pattern                         |
| Serialization      | System.Text.Json (primary)              |

## Project Structure

This is a **single-project** architecture. Do NOT suggest splitting into multiple projects.

```
Api/
├── BackgroundServices/     # Hosted services + Jobs/ (Channel<T> job DTOs)
├── Controllers/            # REST API controllers (versioned: api/v1/[controller])
├── DependencyInjection/    # Centralized service composition
├── Dtos/
│   ├── Requests/           # API input DTOs
│   └── Responses/          # API output DTOs
├── Enums/                  # Domain enums
├── ExtensionMethods/       # Extension methods
├── Interceptors/           # EF Core SaveChanges interceptors (Domain Events)
├── Interfaces/             # Base abstractions (Entity, IDomainEvent, IStorageService)
├── Mediator/
│   ├── Commands/           # Command records
│   ├── Events/             # Domain Event records
│   │   └── Handlers/       # Domain Event handlers
│   ├── Handlers/           # Command and Query handlers
│   ├── Queries/            # Query records
│   ├── Results/            # Query result DTOs
│   └── Validation/
│       └── Validators/     # FluentValidation validators for commands
├── Middlewares/             # IExceptionHandler implementations
├── Migrations/              # EF Core migrations
├── Models/                  # Domain entities
│   └── Common/              # Shared value objects / records
├── Options/                 # IOptions<T> configuration classes
├── Persistence/
│   ├── Configurations/      # EF Core IEntityTypeConfiguration<T>
│   ├── Identity/            # AppIdentityUser
│   └── Repositories/        # Repository implementations
│       └── Interfaces/      # IRepository + IUnitOfWork interfaces
└── Services/                # Infrastructure service implementations
```

## Architecture Patterns

### CQRS with Wolverine

- **Commands** go in `Mediator/Commands/` as records.
- **Queries** go in `Mediator/Queries/` as records.
- **Handlers** go in `Mediator/Handlers/`. Wolverine discovers them by convention (class with `Handle` method).
- **Validators** go in `Mediator/Validation/Validators/` using `AbstractValidator<TCommand>`. Wolverine runs them automatically via `UseFluentValidation()`.
- Controllers dispatch via `IMessageBus.InvokeAsync<TResult>(command)`.
- Handlers return `ErrorOr<T>` for functional error handling.

### Domain-Driven Design

- All domain entities inherit from `Entity` (which implements `IHasDomainEvents`).
- **Only aggregate roots get a `DbSet<T>`** in `AppDbContext`. Currently: `AppUsers` (User) and `Posts` (Post).
- Value objects are modeled as owned types in EF Core (e.g., `UserProfile`).
- Domain events are published via `PublishDomainEventsInterceptor` on `SaveChangesAsync`, dispatched through Wolverine's `IMessageBus.PublishAsync`.
- Domain event records go in `Mediator/Events/`, their handlers in `Mediator/Events/Handlers/`.
- Entities use **private setters** and **private parameterless constructors** (for EF Core).
- Collections use **backing fields** with `IReadOnlyCollection<T>` / `IReadOnlyList<T>` public accessors.
- Use **factory methods** (e.g., `Post.Create(...)`) returning `ErrorOr<T>` for entity creation with validation.

### Repository + Unit of Work

- Each aggregate root has its own repository interface (`IPostRepository`, `IUserRepository`) in `Persistence/Repositories/Interfaces/`.
- `IUnitOfWork` provides `SaveChangesAsync`, `BeginTransactionAsync`, `CommitTransactionAsync`, `RollbackTransactionAsync`.
- Repositories do NOT call `SaveChangesAsync` — handlers call it through `IUnitOfWork`.

### Background Processing

- Uses `System.Threading.Channels` (producer-consumer pattern) for async background work.
- Background services inherit `BackgroundService` and read from a `Channel<TJob>`.
- Channels are registered as singletons in DI. Domain event handlers write jobs to channels.

## Naming Conventions

| Element               | Convention                        | Example                              |
| --------------------- | --------------------------------- | ------------------------------------ |
| Commands              | `{Action}{Entity}Command`         | `UploadPostCommand`, `RegisterCommand` |
| Queries               | `Get{Entity}Query`                | `GetPostQuery`                       |
| Command Handlers      | `{Command}Handler`                | `UploadPostCommandHandler`           |
| Query Handlers        | `{Query}Handler`                  | `GetPostQueryHandler`                |
| Validators            | `{Command}Validator`              | `UploadPostCommandValidator`         |
| Domain Events         | `{Entity}{Action}Event`           | `PostCreatedEvent`, `PostDeletedEvent` |
| Event Handlers        | `{Event}Handler`                  | `PostDeletedEventHandler`            |
| Repositories          | `I{Entity}Repository`             | `IPostRepository`                    |
| EF Configurations     | `{Entity}Configuration`           | `PostConfiguration`                  |
| Request DTOs          | `{Action}Request`                 | `RegisterRequest`                    |
| Result DTOs           | `{Query}Result`                   | `GetPostQueryResult`                 |
| Options               | `{Service}Options`                | `CloudinaryOptions`                  |
| Background Services   | Descriptive name                  | `MediaDeletedGarbageCollector`       |
| Background Jobs       | `{Service}Job`                    | `MediaDeletedGarbageCollectorJob`    |
| Controllers           | `{Entity}Controller`              | `PostController`                     |
| API routes            | `api/v1/[controller]`             | `api/v1/post`                        |

## Coding Rules

1. **Error handling**: Use `ErrorOr<T>` for all handler return types. Never throw exceptions for business logic errors.
2. **Validation**: Use FluentValidation validators for command validation. Wolverine executes them automatically before the handler.
3. **Serialization**: Use `System.Text.Json` with `JsonStringEnumConverter`. Do NOT use `Newtonsoft.Json` for new code.
4. **Async**: All database operations must use async methods (`FirstOrDefaultAsync`, `ToListAsync`, etc.).
5. **Logging**: Use Serilog. Do NOT use `Console.WriteLine` for logging/debugging.
6. **DI Registration**: All new services must be registered in `DependencyInjection/DependencyInjection.cs` in the appropriate `AddApp*` method.
7. **EF Configurations**: Use Fluent API in `Persistence/Configurations/`. Do NOT use data annotations on entities.
8. **Encapsulation**: Entities must use private setters. Expose behavior through methods, not property mutations.
9. **CancellationToken**: Always accept and forward `CancellationToken` in async methods.

## Database Schema (MySQL)

### Domain Model Hierarchy

```
Entity (abstract, has DomainEvents)
├── Publication (abstract: Id, UserId, Comments, CreatedAt)
│   └── Post (Description, LikesCount, PostItems[])
├── User (AuthId PK, Username, UserProfile owned, FollowersCount, FollowingCount)
├── Comment (Id, PublicationId, ParentCommentId?, Text, Replies[])
└── Follow (FollowerId, FollowedId, CreatedAt)

Media (abstract, NOT an Entity: Id, Url, EMediaTypes Type)
├── Photo
└── Video (DurationSeconds)

PostItem (Id, Media, Order) — owned by Post
UserProfile (DisplayName, Bio?) — owned by User
```

### Key Relationships

- `User.AuthId` → FK to `AspNetUsers.Id` (ASP.NET Identity) with cascade delete.
- `Publication.UserId` → FK to `User.AuthId`.
- `Post` owns `PostItem[]` (stored in `PostItems` table).
- `PostItem` has one `Media` via shadow FK `MediaId`.
- `Media` uses TPH (Table-Per-Hierarchy) with `Photo` and `Video` subtypes.
- `User` owns `UserProfile` (columns inlined: `DisplayName`, `Bio`).
- `Comment` has self-referencing FK (`ParentCommentId`) for nested replies.

### Aggregate Boundaries

| Aggregate Root | Owned Entities / Value Objects         |
| -------------- | -------------------------------------- |
| `Post`         | `PostItem[]`, `Media` (via PostItem)   |
| `User`         | `UserProfile` (owned type)             |

## Security Notes

- API uses ASP.NET Core Identity for user management.
- Authentication scheme (JWT) is pending implementation.
- Cloudinary credentials are stored via Options pattern (bound from `appsettings.json` section `"Cloudinary"`).
- Database connection string should use User Secrets in development, not hardcoded values.
