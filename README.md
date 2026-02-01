# Tiny Drive

Tiny Drive is a backend service for a cloud file storage system, working on the same core principles as Google Drive. It provides an API for uploading, downloading, organizing, and managing files, with metadata stored in a database and file contents stored in S3-compatible object storage.

## 📦 Libraries

* **ASP.NET Core** — modern backend API framework
* **MediatR** — CQRS and clean architecture pattern
* **Entity Framework Core + PostgreSQL (Npgsql)** — relational data persistence
* **AWS SDK for S3** — cloud object storage integration
* **Carter** — lightweight, minimal API endpoints
* **FluentValidation** — request and input validation
* **Serilog** — structured, production-grade logging
* **Swashbuckle / OpenAPI** — API documentation
* **xUnit + Testcontainers** — integration and container-based testing
* **SonarAnalyzer / Roslynator** — static code analysis and quality enforcement
  
## 🧱 Architecture & Components

The repository contains the following main folders/projects: ([GitHub][1])

### 1. **TinyDrive.Domain**

* Defines core business models, entities, and domain logic.
* Contains foundational types (e.g., Drive items, file metadata) used across the API.
* Represents the domain layer in a Domain-Driven Design (DDD) pattern.

### 2. **TinyDrive.Features**

* Implements application use cases and feature logic.
* Likely defines operations such as file upload, download, listing, deletion, and other API actions.
* Interfaces and feature handlers are organized here.

### 3. **TinyDrive.Infrastructure**

* Contains infrastructure-level implementations and integrations.
* Includes persistence layer (e.g., database contexts, repositories), external API support, and other technical services such as logging, authentication, and storage integration.

### 4. **TinyDrive.Tests.Integration**

* Contains integration tests for the API.
* Ensures that API endpoints, database interactions, and service integrations behave as expected.

