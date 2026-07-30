# Library Management REST API

This project is a robust and scalable RESTful API for a Library Management System built with **.NET Core**. It strictly adheres to **Clean Architecture** principles and implements industry best practices for backend development.

## 🏗️ Architecture Layers

The solution is divided into four main layers to ensure a strict separation of concerns, scalability, and maintainability:
* **Core:** The domain layer containing Entities (e.g., `Book`, `Author`, `User`) and core interface abstractions.
* **DAL (Data Access Layer):** Manages database operations using **Entity Framework Core**. It implements the **Repository** and **Unit of Work** design patterns for abstracting database interactions.
* **Business:** Contains the core business logic, **Data Transfer Objects (DTOs)**, object mapping configurations (**AutoMapper**), and validation rules.
* **API:** The entry point of the application containing Controllers, routing, and custom Middlewares.

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
* Provides structured, clean JSON responses for:
  * `ValidationException` -> Returns **400 Bad Request** with specific field errors.
  * `NotFoundException` -> Returns **404 Not Found** when requested resources do not exist in the database.
  * Unhandled Exceptions -> Returns **500 Internal Server Error**.

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




