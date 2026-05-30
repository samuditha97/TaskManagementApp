# SoftTask

A modern Task Management Application built using Angular and ASP.NET Core Web API following a simplified Clean Architecture approach.

## Features

* User Authentication
* Create Tasks
* Update Tasks
* Delete Tasks
* Mark Tasks as Completed
* Search Tasks
* Filter Tasks by Status
* Sort Tasks
* Responsive User Interface
* Global Exception Handling
* Input Validation
* Repository Pattern
* Entity Framework Core

---

## Technology Stack

### Frontend

* Angular 21
* TypeScript
* Tailwind CSS
* Reactive Forms

### Backend

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server (MSSQL)
* Dependency Injection
* Middleware
* Repository Pattern

### Database

* Microsoft SQL Server (MSSQL)

---

## Project Structure

```text
TaskManagementApp
│
├── backend
│   ├── TaskManagement.Api
│   ├── TaskManagement.Application
│   ├── TaskManagement.Domain
│   └── TaskManagement.Infrastructure
│
├── frontend
│   └── task-management-ui
│
└── database
    └── TaskManagementDb.sql
```

---

## Architecture

The project follows a simplified Clean Architecture approach.

### Domain Layer

Contains core business entities.

### Application Layer

Contains DTOs, interfaces and business logic.

### Infrastructure Layer

Contains Entity Framework Core implementation, repositories and database access.

### API Layer

Contains controllers, middleware and dependency injection configuration.

---

## Prerequisites

* .NET 9 SDK
* Node.js 22 LTS
* SQL Server
* Git

---

## Database Setup

### Option 1 – Using Entity Framework Migrations

Update the connection string in:

```json
appsettings.json
```

Run:

```bash
dotnet ef database update
```

### Option 2 – Using SQL Script

Execute:

```text
database/TaskManagementDb.sql
```

using SQL Server Management Studio or Azure Data Studio.

---

## Backend Setup

Navigate to backend API project:

```bash
cd backend/TaskManagement.Api
```

Restore packages:

```bash
dotnet restore
```

Run migrations:

```bash
dotnet ef database update
```

Run application:

```bash
dotnet run
```

Swagger URL:

```text
https://localhost:xxxx/swagger
```

---

## Frontend Setup

Navigate to Angular project:

```bash
cd frontend/task-management-ui
```

Install dependencies:

```bash
npm install
```

Run application:

```bash
ng serve
```

Application URL:

```text
http://localhost:4200
```

---

## Login Credentials

```text
Username: samuditha
Password: samu123
```

---

## API Endpoints

### Authentication

```http
POST /api/auth/login
```

### Tasks

```http
GET    /api/tasks
GET    /api/tasks/{id}
POST   /api/tasks
PUT    /api/tasks/{id}
DELETE /api/tasks/{id}
PATCH  /api/tasks/{id}/complete
```

---

## Author

Samuditha Jayawardena

Software Engineer
