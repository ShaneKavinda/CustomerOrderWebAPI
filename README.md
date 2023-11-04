# CustomerOrderWebAPI
## A .NET Core Web API with ADO.NET

This is a simple .NET Core Web API project that demonstrates how to build a RESTful API using ADO.NET to interact with a SQL Server database. The API provides basic CRUD (Create, Read, Update, Delete) operations for managing customer records and retrieving active orders by customer.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Usage](#usage)
- [Testing with Postman](#testing-with-postman)
- [Swagger Documentation](#swagger-documentation)

## Prerequisites

Before you begin, ensure you have met the following requirements:

- .NET 7.0 SDK installed
- SQL Server or another compatible database engine
- Postman or a similar tool for API testing

## Getting Started

1. Clone this repository to your local machine:
   ```bash
   git clone https://github.com/ShaneKavinda/CustomerOrderWebAPI.git
2. Navigate to the project directory
   ```bash
   cd CustomerOrderWebAPI
3. Restore NuGet packages
   ```bash
   dotnet restore
5. Configure your database connection by updating the connection string in CustomerDataAccessLayer.cs in CustomerOrderWebAPI/testApi/Controllers/CustomerDataAccessLayer.cs
6. Create the database schema by running the database migration:
   ```bash
   dotnet ef database update
7. Build and Run the API
   ```bash
   dotnet run
Your API should now be running and accessible at http://localhost:5000.

## API Endpoints

- POST /api/customers - Create a new customer record.
- GET /api/customers - Retrieve all customers.
- POST /api/customers/Edit - Update a customer by ID.
- DELETE /api/customers/Delete - Delete a customer by ID.
- GET /api/customers/{customerId}/active-orders - Retrieve active orders for a customer by ID.

## Usage

You can use this API to perform the following actions:

Create, read, update, and delete customer records.
Retrieve a list of all customers.
Retrieve a specific customer by their ID.
Retrieve active orders for a specific customer.

## Testing with Postman
To test the API using Postman, follow these steps:

Open Postman.
Create a new request.
Set the request type (GET, POST, PUT, DELETE) and provide the necessary URL.
Add headers, parameters, and request body as needed.
Send the request.

## Swagger Documentation
This API includes Swagger documentation for easy testing and exploration of endpoints. You can access the Swagger UI at http://localhost:5000/swagger when the API is running.
