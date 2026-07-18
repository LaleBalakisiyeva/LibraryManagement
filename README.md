# Library Management REST API

This project is a robust and scalable RESTful API for a Library Management System built with **.NET Core**. It strictly adheres to **Clean Architecture** principles and implements industry best practices for backend development.

## 🏗️ Architecture Layers

The solution is divided into four main layers to ensure a strict separation of concerns, scalability, and maintainability:

* **Core:** The domain layer containing Entities (e.g., `Book`, `Author`) and core interface abstractions.
* **DAL (Data Access Layer):** Manages database operations using **Entity Framework Core**. It implements the **Repository** and **Unit of Work** design patterns for abstracting database interactions.
* **Business:** Contains the core business logic, Data Transfer Objects (DTOs), object mapping configurations (**AutoMapper**), and validation rules.
* **API:** The entry point of the application containing Controllers, routing, and custom Middlewares.

## 🚀 Implemented Features

### 1. Architectural Setup (Checkpoints 1 & 2)
* Established a modular solution structure.
* Configured Dependency Injection for services, repositories, and unit of work.
* Set up Entity Framework Core for data access.

### 2. CRUD Operations & Data Mapping (Checkpoint 3)
* Implemented full **Create, Read, Update, Delete (CRUD)** operations for `Author` and `Book` entities.
* Integrated **AutoMapper** to seamlessly map data between Domain Entities and DTOs, ensuring sensitive data is not exposed.
* Utilized **Eager Loading** (`.Include()`) to fetch related data efficiently without null references.
* Endpoints return standardized HTTP Status Codes (`200 OK`, `201 Created`, `204 No Content`).

### 3. Validation & Error Handling (Checkpoint 4)
* **Input Validation:** Integrated **FluentValidation** (equivalent to Java's `@NotNull`, `@Size`) to validate incoming request payloads directly at the Business layer.
* **Global Exception Handling:** Implemented a custom `ExceptionHandlingMiddleware` (equivalent to Spring's `@ControllerAdvice`) that acts as a centralized error handler.
* Provides structured, clean JSON responses for:
  * `ValidationException` -> Returns **400 Bad Request** with specific field errors.
  * `NotFoundException` -> Returns **404 Not Found** when requested resources do not exist in the database.
  * Unhandled Exceptions -> Returns **500 Internal Server Error**.

## 🛠️ Technologies & Tools

* **Framework:** .NET Core / ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Object Mapping:** AutoMapper
* **Validation:** FluentValidation
* **Design Patterns:** Clean Architecture, Repository Pattern, Unit of Work Pattern
