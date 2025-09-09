🚀 Service Request Platform – .NET Core Web API
📌 Overview

The Service Request Platform is a multi-layered ASP.NET Core Web API application designed to manage service requests between Admins, Customers, and Workers.
It follows Clean Architecture principles, ensuring scalability, maintainability, and clean separation of concerns.

✨ Key Features

🔐 JWT Authentication & Role-based Authorization (Admin, Customer, Worker)

📝 Service Requests Management → Create, assign, update, and track requests

✅ DTOs + FluentValidation for clean API contracts and validation

🗄️ Repository Pattern + EF Core for structured and testable data access

📊 Swagger Integration for interactive API documentation

🛠️ EF Core Migrations for database schema versioning

🏗️ Solution Structure
ServiceRequestPlatform.sln
│── ServiceRequestPlatform.API          → Controllers, Swagger, Middleware

│── ServiceRequestPlatform.Application  → Services, DTOs, Validators

│── ServiceRequestPlatform.Domain       → Entities, Enums, Repository Interfaces

│── ServiceRequestPlatform.Infrastructure → DbContext, EF Core Repositories, Migrations

🔧 Tech Stack

Backend: ASP.NET Core Web API (.NET 6/7)

Database: SQL Server (via EF Core)

Authentication: JWT Bearer Authentication (role-based)

Documentation: Swagger / OpenAPI

Version Control: Git + GitHub

🚀 Getting Started
Prerequisites

.NET 6/7 SDK

SQL Server

Visual Studio 2022
 or VS Code

Setup Instructions

Clone the repository:

git clone https://github.com/prem574/ServiceRequest.git
cd ServiceRequest


Update the connection string in appsettings.json.

Apply migrations:

dotnet ef database update --project ServiceRequestPlatform.Infrastructure


Run the API:

dotnet run --project ServiceRequestPlatform.API


Open Swagger in your browser:

https://localhost:5001/swagger

🔑 Default Login Accounts

Admin → admin@example.com / Admin@123

Customer → customer@example.com / Customer@123

Worker → worker@example.com / Worker@123

📌 Example Endpoints

POST /api/auth/register → Register user

POST /api/auth/login → Login & get JWT token

GET /api/admin/customers → Get all customers (Admin only)

POST /api/servicerequests → Create new service request (Customer)

PUT /api/servicerequests/{id}/assign → Assign request to worker (Admin)

GET /api/workers/requests → Worker’s assigned requests

🎯 Why This Project Matters

This project highlights:

Practical implementation of Clean Architecture

Secure APIs with JWT & Role-based Authentication

Real-world repository & service pattern with EF Core

Scalable design principles used in enterprise systems

It demonstrates end-to-end backend development skills directly relevant for a .NET Developer .
