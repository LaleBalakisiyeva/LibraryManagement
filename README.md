# Library Management REST API

This project is a robust and scalable RESTful API for a Library Management System built with **.NET Core**. It strictly adheres to **Clean Architecture** principles and implements industry best practices for backend development.

## 🏗️ Architecture Layers

The solution is divided into four main layers to ensure a strict separation of concerns, scalability, and maintainability:
* **Core:** The domain layer containing Entities (e.g., `Book`, `Author`) and core interface abstractions.
* **DAL (Data Access Layer):** Manages database operations using **Entity Framework Core**. It implements the **Repository** and **Unit of Work** design patterns for abstracting database interactions.
* **Business:** Contains the core business logic, **Data Transfer Objects (DTOs)**, object mapping configurations (**AutoMapper**), and validation rules.
* **API:** The entry point of the application containing Controllers, routing, and custom Middlewares.

## 🚀 Implemented Features

### 1. Architectural Setup (Checkpoints 1 & 2)
* Established a modular solution structure.
* Configured **Dependency Injection** for services, repositories, and unit of work.
* Set up **Entity Framework Core** for data access.

### 2. CRUD Operations & Data Mapping (Checkpoint 3)
* Implemented full **Create, Read, Update, Delete (CRUD)** operations for `Author` and `Book` entities.
* Integrated **AutoMapper** to seamlessly map data between Domain Entities and DTOs, ensuring sensitive data is not exposed.
* Utilized Eager Loading (`.Include()`) to fetch related data efficiently without null references.
* Endpoints return standardized HTTP Status Codes (`200 OK`, `201 Created`, `204 No Content`).

### 3. Validation & Error Handling (Checkpoint 4)
* **Input Validation:** Integrated **FluentValidation** (equivalent to Java's `@NotNull`, `@Size`) to validate incoming request payloads directly at the Business layer.
* **Global Exception Handling:** Implemented a custom `ExceptionHandlingMiddleware` (equivalent to Spring's `@ControllerAdvice`) that acts as a centralized error handler.
* Provides structured, clean JSON responses for:
  * `ValidationException` -> Returns **400 Bad Request** with specific field errors.
  * `NotFoundException` -> Returns **404 Not Found** when requested resources do not exist in the database.
  * Unhandled Exceptions -> Returns **500 Internal Server Error**.

### 4. Pagination & Sorting (Checkpoint 5)
* **Pagination:** Implemented efficient pagination using LINQ's `Skip()` and `Take()` methods at the database (SQL) level to handle large datasets properly.
* **Dynamic Sorting:** Added dynamic sorting capabilities based on query parameters (e.g., sort by `Title`, `PublishYear`, ascending/descending) using `OrderBy()` and `OrderByDescending()`.
* **Metadata Wrapper:** Introduced a generic `PaginatedResult<T>` wrapper class to return the requested list of data alongside essential frontend metadata (`TotalCount`, `TotalPages`, `PageNumber`, `PageSize`).

### 5. API Documentation & Interactive UI (Checkpoint 6)
* **OpenAPI Integration:** Embedded **Swagger** engine into the .NET Core pipeline using the `Swashbuckle.AspNetCore` package to automate contract generation.
* **Automatic Schema Discovery:** Dynamically discovers and maps all Controller routes, complete HTTP Verbs, response schemas, and custom payload structures.
* **Parameter Mapping:** Seamlessly displays endpoints involving CRUD mechanics alongside explicit pagination parameters (`PageNumber`, `PageSize`) and dynamic sorting options on a unified UI.
* **Live Playground:** Offers a built-in sandbox interface enabling developers or API consumers to run live validation checks and review backend responses on the fly.

### 6. Automated Unit Testing Suite (Checkpoint 7)
* **Framework Deployment:** Engineered an isolated test target (`LibraryManagement.Tests`) relying on the **xUnit** automation framework.
* **Decoupled Architecture Mocking:** Utilized **Moq** to isolate the Business layer completely from database side-effects. Explicitly registered internal repository implementations (`IBookRepository`, `IAuthorRepository`) inside an insulated `IUnitOfWork` facade layout.
* **Infrastructure Virtualization:** Safely simulated secondary operational tasks including object mapping definitions (`IMapper`) and user-submitted validation payloads (`FluentValidation`).
* **Behavioral Verification:** Covered predictable data flows (e.g., entity tracking checks, payload creation saves, entity retrieval configurations) via fluent semantic constraints (**FluentAssertions**), registering a 100% pass verification metrics natively.

## 🛠️ Technologies & Tools
* **Framework:** .NET Core / ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Object Mapping:** AutoMapper
* **Validation:** FluentValidation
* **API Documentation:** Swagger / OpenAPI (Swashbuckle)
* **Testing Stack:** xUnit, Moq, FluentAssertions
* **Design Patterns:** Clean Architecture, Repository Pattern, Unit of Work Pattern
