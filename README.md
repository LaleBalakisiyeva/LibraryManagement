# Library Management REST API

This project is a robust and scalable RESTful API for a Library Management System built with **.NET Core**. It strictly adheres to **Clean Architecture** principles and implements industry best practices for backend development.

## 🏗️ Architecture Layers

The solution is divided into four main layers to ensure a strict separation of concerns, scalability, and maintainability:

* **Core:** The domain layer containing Entities (e.g., `Book`, `Author`, `User`, `Category`, `Order`) and core interface abstractions.
* **DAL (Data Access Layer):** Manages database operations using **Entity Framework Core**. It implements the **Repository** and **Unit of Work** design patterns for abstracting database interactions.
* **Business:** Contains the core business logic, **Data Transfer Objects (DTOs)**, object mapping configurations (**AutoMapper**), and validation rules.
* **API:** The entry point of the application containing Controllers, routing, and custom Middlewares.

---

## 🚀 Implemented Features

### 📅 WEEK 1: Core Architecture & Data Management

#### 1. Architectural Setup (Checkpoints 1 & 2)
* Established a modular solution structure.
* Configured **Dependency Injection** for services, repositories, and unit of work.
* Set up **Entity Framework Core** for data access.

#### 2. CRUD Operations & Data Mapping (Checkpoint 3)
* Implemented full **Create, Read, Update, Delete (CRUD)** operations for `Author` and `Book` entities.
* Integrated **AutoMapper** to seamlessly map data between Domain Entities and DTOs, ensuring sensitive data is not exposed.
* Utilized Eager Loading (`.Include()`) to fetch related data efficiently without null references.
* Endpoints return standardized HTTP Status Codes (`200 OK`, `201 Created`, `204 No Content`).

#### 3. Validation & Error Handling (Checkpoint 4)
* **Input Validation:** Integrated **FluentValidation** (equivalent to Java's `@NotNull`, `@Size`) to validate incoming request payloads directly at the Business layer.
* **Global Exception Handling:** Implemented a custom `ExceptionHandlingMiddleware` (equivalent to Spring's `@ControllerAdvice`) that acts as a centralized error handler.
* Provides structured JSON responses handling `ValidationException` (400 Bad Request), `NotFoundException` (404 Not Found), and Unhandled Exceptions (500 Internal Server Error).

#### 4. Pagination & Sorting (Checkpoint 5)
* **Pagination:** Implemented efficient pagination using LINQ's `Skip()` and `Take()` methods at the database (SQL) level to handle large datasets properly.
* **Dynamic Sorting:** Added dynamic sorting capabilities based on query parameters (e.g., sort by `Title`, `PublishYear`, ascending/descending) using `OrderBy()` and `OrderByDescending()`.
* **Metadata Wrapper:** Introduced a generic `PaginatedResult<T>` wrapper class to return the requested list of data alongside essential frontend metadata (`TotalCount`, `TotalPages`, `PageNumber`, `PageSize`).

#### 5. API Documentation & Interactive UI (Checkpoint 6)
* **OpenAPI Integration:** Embedded **Swagger** engine into the .NET Core pipeline using the `Swashbuckle.AspNetCore` package to automate contract generation.
* **Automatic Schema Discovery:** Dynamically discovers and maps all Controller routes, complete HTTP Verbs, response schemas, and custom payload structures.
* **Parameter Mapping:** Seamlessly displays endpoints involving CRUD mechanics alongside explicit pagination parameters (`PageNumber`, `PageSize`) and dynamic sorting options on a unified UI.
* **Live Playground:** Offers a built-in sandbox interface enabling developers or API consumers to run live validation checks and review backend responses on the fly.

#### 6. Automated Unit Testing Suite (Checkpoint 7)
* **Framework Deployment:** Engineered an isolated test target (`LibraryManagement.Tests`) relying on the **xUnit** automation framework.
* **Decoupled Architecture Mocking:** Utilized **Moq** to isolate the Business layer completely from database side-effects. Explicitly registered internal repository implementations (`IBookRepository`, `IAuthorRepository`) inside an insulated `IUnitOfWork` facade layout.
* **Infrastructure Virtualization:** Safely simulated secondary operational tasks including object mapping definitions (`IMapper`) and user-submitted validation payloads (`FluentValidation`).
* **Behavioral Verification:** Covered predictable data flows (e.g., entity tracking checks, payload creation saves, entity retrieval configurations) via fluent semantic constraints (**FluentAssertions**), registering a 100% pass verification metrics natively.

---

### 📅 WEEK 2: Authentication & Security

#### 7. User Management & Password Hashing (Checkpoint 1)
* **Domain Expansion:** Created the `User` entity to handle authentication credentials and role-based access control.
* **Secure Hashing Mechanism:** Integrated the `BCrypt.Net-Next` library to securely hash and salt passwords, ensuring plaintext credentials are never exposed or stored in the database.
* **Data Integrity & Constraints:** Applied Entity Framework Core Fluent API configurations to enforce unique constraints on `Email` and `Username` fields.
* **Repository Integration:** Expanded the Data Access Layer (DAL) by introducing a dedicated `IUserRepository` with user-specific query methods (e.g., `GetByUsernameAsync`). This was seamlessly integrated into the lazy-initialized `UnitOfWork` pipeline for optimized database transactions.

#### 8. Authentication Endpoints & JWT Authorization (Checkpoint 2)
* **JWT Implementation:** Developed fully functional `Register` and `Login` endpoints within a dedicated `AuthController`. Integrated **JSON Web Tokens (JWT)** for stateless and secure API authorization, embedding standard claims (e.g., User ID, Username, Roles) into the token payload.
* **Clean Controller Refactoring:** Adhered strictly to Clean Architecture principles by keeping controllers "thin". Removed redundant `try-catch` blocks from the API layer and extended the global `ExceptionHandlingMiddleware` to gracefully intercept `UnauthorizedAccessException` (Returns **401 Unauthorized**) and `InvalidOperationException` (Returns **400 Bad Request**).
* **Swagger Security Definition:** Upgraded the OpenAPI/Swagger configuration in `Program.cs` to support **Bearer Token** authentication. Introduced a customized UI security definition allowing developers to pass JWTs directly via the Swagger sandbox.
* **Configuration & DI Management:** Properly mapped and isolated sensitive token configurations (`SecurityKey`, `Issuer`, `Audience`) using `appsettings.json`. Successfully registered required authentication services (`IAuthService`) within the Dependency Injection container.
* **Database Synchronization:** Generated and applied Entity Framework Core migrations to materialize the new `Users` table schema into the underlying SQL database, ensuring perfect synchronization between the Domain layer and Data layer.

#### 9. Stateless Authentication Middleware Pipeline (Checkpoint 3)
* **Stateless API Architecture:** Configured the **JWT Bearer Authentication Scheme** globally to ensure the API operates on strict **Stateless REST** principles, completely eliminating cookie or session state dependencies.
* **Token Validation:** Implemented robust `TokenValidationParameters` to automatically verify token signatures (`IssuerSigningKey`), validate the `Issuer` and `Audience`, and enforce token expiration rules (`Lifetime`).
* **Pipeline Execution Order:** Engineered the HTTP request **Middleware Pipeline** by explicitly placing `app.UseAuthentication()` prior to `app.UseAuthorization()`, ensuring client identities are fully resolved before access policies and permissions are evaluated.

#### 10. Role-Based Access Control (RBAC) (Checkpoint 4)
* **Endpoint Protection:** Enforced **Role-Based Access Control (RBAC)** across Controller boundaries using `[Authorize(Roles = "...")]` attributes, effectively isolating access levels between `USER` and `ADMIN` roles.
* **Privilege Segregation:** Secured resource-mutating operations (`POST`, `PUT`, `DELETE`) exclusively for `ADMIN` accounts, while permitting standard read operations (`GET`) for all authenticated users (`USER,ADMIN`).
* **Stateless Role Resolution:** Embedded `ClaimTypes.Role` directly into the JWT payload during authentication. This allows the authorization middleware to evaluate permissions instantly in-memory, completely bypassing secondary database queries for role validation.

#### 11. Auth Error Handling & REST Semantics (Checkpoint 5)
* **401 vs 403 Differentiation:** Configured the authentication/authorization pipeline to strictly distinguish between unauthenticated and unauthorized requests.
* **401 Unauthorized:** Automatically returned when a request lacks a valid JWT token or when authentication credentials fail.
* **403 Forbidden:** Returned when a valid user token is provided, but the user's assigned role lacks sufficient permissions (e.g., standard `USER` attempting `ADMIN` routes).
* **Middleware Alignment:** Guaranteed correct HTTP status codes by placing `app.UseAuthentication()` strictly before `app.UseAuthorization()` in `Program.cs`.

#### 12. Token Expiration & Lifecycle Management (Checkpoint 6)
* **Dynamic Configuration:** Extracted the token validity lifespan (`ExpirationInMinutes`) into `appsettings.json`, preventing hardcoded values and allowing seamless environment-specific adjustments.
* **Timestamp Allocation:** Engineered the `AuthService` to dynamically calculate and embed exact UTC expiration timestamps (`Expires`) during the JWT generation phase.
* **Client-Side Awareness:** Structured the authentication response (`TokenResponseDto`) to return not only the encoded token but also its explicit expiration timestamp, empowering front-end clients to manage sessions accurately.
* **Automated Lifecycle Validation:** Configured the `.NET Core JwtBearer` middleware with `ValidateLifetime = true`. This guarantees that the system automatically intercepts expired tokens and returns a pristine **401 Unauthorized** response without requiring manual interception logic.

---

### 📅 WEEK 3: Database Relationships & Advanced Queries

#### 13. Relational Data Modeling & Fluent API (Checkpoint 1)
* **Domain Expansion:** Introduced `Category`, `Order`, and `OrderItem` entities to support complex relational data structures resembling real-world operational flows.
* **Many-to-Many Architecture:** Engineered a **Many-to-Many** relationship between `Book` and `Category`. Leveraged Entity Framework Core's Fluent API with `.UsingEntity(j => j.ToTable("BookCategories"))` to explicitly define and manage the junction table natively within the Data Access Layer.
* **One-to-Many Hierarchies:** Designed normalized **One-to-Many** dependencies allowing `User` to track multiple `Order` entities, and `Order` to encapsulate multiple `OrderItem` records.
* **Referential Integrity & Deletion Rules:** Strictly configured cascade mechanisms (`DeleteBehavior`) aligned with business logic. Forced `Cascade` deletion on `OrderItems` when an `Order` is removed to prevent data orphaning, whilst applying `Restrict` deletion on `User` to safeguard crucial transactional history.
* **Schema Materialization:** Successfully registered new domain entities as `DbSet<T>` within the application's `DbContext` and applied SQL migrations to seamlessly project the updated Clean Architecture constraints onto the relational database instance.

#### 14. Advanced Dynamic Filtering & IQueryable Optimization (Checkpoint 2)
* **Complex Query Parameters:** Implemented advanced dynamic filtering capabilities allowing API consumers to filter resources using a combination of optional query parameters (`searchTerm`, `authorId`, `minYear`, `maxYear`).
* **Database-Level Execution:** Leveraged Entity Framework Core's `IQueryable` interface to construct dynamic `Where` clauses inside the Data Access Layer. This enforces filtering directly on the SQL database server (equivalent to Derived Queries) rather than relying on heavy in-memory RAM processing (`.ToList()` operations).
* **Pipeline Integration:** Seamlessly unified the complex filtering logic with the previously established pagination (`Skip/Take`) and sorting mechanisms, passing state safely from the Controllers through the Service Layer and `UnitOfWork`.
* **Security & Performance:** Utilized LINQ expressions to automatically generate parameterized SQL queries, effectively neutralizing SQL injection vulnerabilities while ensuring that only precisely matched and paginated records are materialized.

#### 15. Dynamic Filtering API Endpoint (Checkpoint 3)
* **API Endpoint Integration:** Expanded the `BooksController` `GetAll` (HTTP GET) method to accept optional `[FromQuery]` parameters (`searchTerm`, `authorId`, `minYear`, `maxYear`) alongside existing pagination and sorting rules.
* **Architectural Adherence (Thin Controllers):** Maintained Clean Architecture constraints by keeping the Controller free of complex logic. Request payloads are dynamically delegated strictly to the core Service layer via `_bookService.GetAllPagedAsync`.
* **RESTful Semantics:** Ensured the endpoint adheres to REST API best practices by safely returning a `200 OK` status code combined with an empty array (`"data": []`) rather than a `404 Not Found` when no resources match the provided search criteria.
* **Swagger Verification:** Successfully tested complex filtering permutations (text search combined with numeric ranges) interactively via the OpenAPI sandbox, fully validating data mapping from the UI through to the SQL layer.
* **Endpoint Security:** Guarded the newly enhanced dynamic endpoint to guarantee only authenticated users with specific roles (`USER,ADMIN`) have operational access, requiring a valid JWT signature.

#### 16. Transaction Management & DDD Aggregate Roots (Checkpoint 4)
* **Atomic Transactions:** Implemented explicit database transaction management using the `UnitOfWork` pattern (`BeginTransactionAsync`, `CommitAsync`, `RollbackAsync`) to ensure absolute data consistency across multiple table insertions (`Orders` and `OrderItems`).
* **Rollback Simulation & Testing:** Engineered specific failure-simulation endpoints to verify automated database rollback triggers when unhandled exceptions occur during midway data processing, guaranteeing no orphaned records remain.
* **Domain-Driven Design (DDD):** Adopted Aggregate Root principles by treating `Order` as the primary transactional boundary. Eliminated redundant `OrderItem` Repositories and Controllers, instead managing all child entity mutations strictly through the `Order` aggregate lifecycle.
* **Payload Validation & Mapping:** Structured incoming complex payloads using custom `OrderItemDto` combined with nested `FluentValidation` rules, safely mapping client arrays to normalized Entity Framework relational graphs using AutoMapper.

#### 17. N+1 Query Optimization & Eager Loading (Checkpoint 5)
* **Query Optimization:** Resolved the N+1 query performance issue by implementing **Eager Loading** using Entity Framework Core's `.Include()` and `.ThenInclude()` methods. This consolidated multiple iterative database calls into a single, highly efficient SQL `JOIN` statement.
* **Read DTO Implementation:** Designed `OrderReadDto` and `OrderItemReadDto` to securely format relational responses. Flattened complex nested structures (e.g., mapping `BookTitle` directly inside the order item) using AutoMapper configurations.
* **Data Integrity Validation:** Applied FluentValidation rules to the outgoing Read DTOs (strictly formatting IDs, timestamps, and pricing constraints) ensuring the API delivers consistent and valid JSON responses.
* **Performance Tuning:** Utilized `.AsNoTracking()` on read-only queries to disable EF Core's Change Tracker, significantly reducing memory overhead and improving response times for fetching large relational graphs.

#### 18. Transaction Rollback Testing & Data Integrity (Checkpoint 6)
* **Rollback Simulation (Trick):** Designed and implemented a dedicated negative unit test (`CreateOrderAsync_ShouldTriggerRollback_WhenExceptionIsThrown`) using **xUnit** and **Moq** to simulate deliberate database insertion failures.
* **Transaction Verification:** Verified that when an unexpected `Exception` occurs during a multi-table write operation, the `CommitAsync` is completely bypassed and `RollbackAsync` is instantly triggered.
* **Data Consistency:** Proved that both checked and unchecked exceptions are properly intercepted by the `UnitOfWork` pattern, ensuring that no partial, orphaned, or corrupted data is ever persisted to the SQL database.

---

### 📅 WEEK 4: Advanced Features & Caching

#### 19. In-Memory Caching & Performance Optimization (Checkpoint 1)
* **Read-Oriented Caching:** Integrated .NET Core's native `IMemoryCache` (equivalent to Spring Cache abstraction) into the Business Layer to significantly improve the read performance of frequently accessed, rarely modified endpoints (e.g., retrieving the Categories list).
* **Cache Invalidation Trick (Data Consistency):** Engineered strict cache eviction policies within the `Create`, `Update`, and `Delete` operational flows. Ensuring that whenever an entity is modified, the associated `CacheKey` is immediately purged (`_cache.Remove`), guaranteeing the API never returns stale data to the client.
* **Absolute Expiration Management:** Configured explicit `MemoryCacheEntryOptions` with a defined absolute expiration window (e.g., 30 minutes) to automate lifecycle management and prevent memory leaks.
* **Dependency Injection Integration:** Registered the caching service seamlessly within the application pipeline (`builder.Services.AddMemoryCache()`) and injected it via constructor injection into the relevant Service classes without violating Clean Architecture principles.

---

## 🛠️ Technologies & Tools
* **Framework:** .NET Core / ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Object Mapping:** AutoMapper
* **Validation:** FluentValidation
* **Authentication & Authorization:** JWT (JSON Web Tokens), `Microsoft.AspNetCore.Authentication.JwtBearer`
* **Security & Hashing:** BCrypt.Net-Next
* **API Documentation:** Swagger / OpenAPI (Swashbuckle)
* **Testing Stack:** xUnit, Moq, FluentAssertions
* **Design Patterns:** Clean Architecture, Repository Pattern, Unit of Work Pattern




