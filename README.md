# Order Management API - README

## Overview
The Order Management API provides a complete backend solution for managing customers, products, orders, and user authentication using JWT. The system follows a Clean Architecture / DDD hybrid structure with separation of concerns across the API, Application, Domain, and Infrastructure layers.

---

## Features

### Customers
- Create, update, and soft delete customers.
- Pagination with name/email filtering.
- Enforces unique customer email addresses.

### Products
- Create, update, delete products.
- Enforces unique SKU.
- Stock management (increase/decrease).
- Pagination with keyword search.
- Validation to prevent deleting a product used in an order.

### Orders
- Create orders with multiple items (using TVP).
- Validates stock availability before creation.
- Automatically calculates totals.
- Cancelling an order restores stock.
- Retrieve full order details including items.
- Filter by date range and status.
- High-performance stored procedure for pagination.

### Authentication
- JWT Bearer authentication.
- Endpoints for Login, Register, and Assign Role.
- Role-based authorization (Admin, User).

- 
## Architecture (Clean/DDD Hybrid)

-OrderingSystem.Api/  Controllers, Swagger, JWT setup

-OrderingSystem.Application/  Services, DTOs, Interfaces

-OrderingSystem.Domain/  Entities, ValueObjects, Exceptions

-OrderingSystem.Infrastructure/ EF Core, Repositories, SQL, Dapper

-OrderingSystem.Tests/  Unit tests

## Tech Stack

Backend: .NET 9 Web API  
Data Access: EF Core, ADO.NET, SQL Server Stored Procedures  
Authentication: JWT Bearer  
Documentation: Swagger / OpenAPI  
Testing: xUnit, FluentAssertions  


## Setup Instructions

1. Clone or download the project.
2. Update `appsettings.json` with:
   - SQL Server connection string
   - JWT configuration (Key, Issuer, Audience)
3. Run database migrations or execute provided SQL scripts.
4. Build and run the project.
5. Open Swagger at `/swagger` to test API endpoints.



