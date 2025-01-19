# Dotnet Simple API

Dotnet Simple API is a web application built with ASP.NET Core that provides endpoints for managing stocks and comments.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Usage](#usage)

## Features

- Manage stocks and comments
- JWT authentication
- Swagger for API documentation

## Technologies

- ASP.NET Core
- Entity Framework Core
- Microsoft SQL Server
- JWT Authentication
- Swagger

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- SQL Server

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/sovannarapov/dotnet-simple-api.git
    cd dotnet-simple-api
    ```

2. Install the required packages:
    ```sh
    dotnet restore
    ```

3. Update the database:
    ```sh
    dotnet ef database update
    ```

### Running the Application

1. Run the application:
    ```sh
    dotnet run
    ```

2. Open your browser and navigate to `https://localhost:5001/swagger` to view the Swagger UI.

## Configuration

The application can be configured using the `appsettings.json` file. Below is an example configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost;Initial Catalog=mydb;User Id=sa;Password=P@ssw0rd;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=false"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JWT": {
    "Issuer": "http://localhost:5246",
    "Audience": "http://localhost:5246",
    "SigningKey": "mysupersecretkey"
  }
}
```

## Usage

### Endpoints

#### Stocks

- `GET /api/stocks` - Get all stocks
   - Response: `200 OK` with a list of stocks
   - Example Response:
     ```json
     [
       {
         "id": 1,
         "symbol": "AAPL",
         "companyName": "Apple Inc.",
         "purchase": 150.00,
         "lastDiv": 0.82,
         "industry": "Technology",
         "marketCap": 2500000000000
       }
     ]
     ```

- `GET /api/stocks/{id}` - Get a stock by ID
   - Parameters: `id` (int) - The ID of the stock
   - Response: `200 OK` with the stock details or `404 Not Found` if the stock does not exist
   - Example Response:
     ```json
     {
       "id": 1,
       "symbol": "AAPL",
       "companyName": "Apple Inc.",
       "purchase": 150.00,
       "lastDiv": 0.82,
       "industry": "Technology",
       "marketCap": 2500000000000
     }
     ```

- `POST /api/stocks` - Create a new stock
   - Request Body: JSON object with `symbol`, `companyName`, `purchase`, `lastDiv`, `industry`, and `marketCap`
   - Example Request:
     ```json
     {
       "symbol": "AAPL",
       "companyName": "Apple Inc.",
       "purchase": 150.00,
       "lastDiv": 0.82,
       "industry": "Technology",
       "marketCap": 2500000000000
     }
     ```
   - Response: `201 Created` with the created stock details
   - Example Response:
     ```json
     {
       "id": 1,
       "symbol": "AAPL",
       "companyName": "Apple Inc.",
       "purchase": 150.00,
       "lastDiv": 0.82,
       "industry": "Technology",
       "marketCap": 2500000000000
     }
     ```

- `PUT /api/stocks/{id}` - Update a stock
   - Parameters: `id` (int) - The ID of the stock
   - Request Body: JSON object with updated `symbol`, `companyName`, `purchase`, `lastDiv`, `industry`, and `marketCap`
   - Example Request:
     ```json
     {
       "symbol": "AAPL",
       "companyName": "Apple Inc.",
       "purchase": 155.00,
       "lastDiv": 0.85,
       "industry": "Technology",
       "marketCap": 2600000000000
     }
     ```
   - Response: `200 OK` with the updated stock details or `404 Not Found` if the stock does not exist
   - Example Response:
     ```json
     {
       "id": 1,
       "symbol": "AAPL",
       "companyName": "Apple Inc.",
       "purchase": 155.00,
       "lastDiv": 0.85,
       "industry": "Technology",
       "marketCap": 2600000000000
     }
     ```

- `DELETE /api/stocks/{id}` - Delete a stock
   - Parameters: `id` (int) - The ID of the stock
   - Response: `204 No Content` or `404 Not Found` if the stock does not exist

#### Comments

- `GET /api/comments` - Get all comments
   - Response: `200 OK` with a list of comments
   - Example Response:
     ```json
     [
       {
         "id": 1,
         "title": "Great Stock",
         "content": "This stock has great potential.",
         "createdOn": "2023-10-01T12:00:00Z",
         "stockId": 1
       }
     ]
     ```

- `GET /api/comments/{id}` - Get a comment by ID
   - Parameters: `id` (int) - The ID of the comment
   - Response: `200 OK` with the comment details or `404 Not Found` if the comment does not exist
   - Example Response:
     ```json
     {
       "id": 1,
       "title": "Great Stock",
       "content": "This stock has great potential.",
       "createdOn": "2023-10-01T12:00:00Z",
       "stockId": 1
     }
     ```

- `POST /api/comments` - Create a new comment
   - Request Body: JSON object with `title`, `content`, `createdOn`, and `stockId`
   - Example Request:
     ```json
     {
       "title": "Great Stock",
       "content": "This stock has great potential.",
       "createdOn": "2023-10-01T12:00:00Z",
       "stockId": 1
     }
     ```
   - Response: `201 Created` with the created comment details
   - Example Response:
     ```json
     {
       "id": 1,
       "title": "Great Stock",
       "content": "This stock has great potential.",
       "createdOn": "2023-10-01T12:00:00Z",
       "stockId": 1
     }
     ```

- `PUT /api/comments/{id}` - Update a comment
   - Parameters: `id` (int) - The ID of the comment
   - Request Body: JSON object with updated `title`, `content`, `createdOn`, and `stockId`
   - Example Request:
     ```json
     {
       "title": "Updated Comment",
       "content": "Updated content.",
       "createdOn": "2023-10-02T12:00:00Z",
       "stockId": 1
     }
     ```
   - Response: `200 OK` with the updated comment details or `404 Not Found` if the comment does not exist
   - Example Response:
     ```json
     {
       "id": 1,
       "title": "Updated Comment",
       "content": "Updated content.",
       "createdOn": "2023-10-02T12:00:00Z",
       "stockId": 1
     }
     ```

- `DELETE /api/comments/{id}` - Delete a comment
   - Parameters: `id` (int) - The ID of the comment
   - Response: `204 No Content` or `404 Not Found` if the comment does not exist