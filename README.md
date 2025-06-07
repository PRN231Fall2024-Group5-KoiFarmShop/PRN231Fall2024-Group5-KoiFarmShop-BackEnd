# Koi Farm Backend API

An ASP.NET Core Web API for managing data in a koi fish farming system. This academic project demonstrates the use of Entity Framework Core with SQL Server (code-first approach) to support CRUD operations and structured data access for a real-world aquaculture scenario.

## Features

* Fish species and pond management
* Feeding and health tracking
* Inventory and batch control
* User roles and authentication (if applicable)

## Technology Stack

* ASP.NET Core Web API
* Entity Framework Core (Code-First)
* SQL Server

## Usage

1. Clone the repository and configure your connection string in `appsettings.json`.
2. Run EF Core migrations to initialize the database:

   ```bash
   dotnet ef database update
   ```
3. Start the application:

   ```bash
   dotnet run
   ```
4. Access the API via Swagger UI (e.g. `https://localhost:5001/swagger`) to explore available endpoints.

This backend API provides a foundation for building a full-stack koi farm management system, supporting both administrative workflows and real-time farm data monitoring.
