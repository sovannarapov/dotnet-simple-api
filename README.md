# Dotnet Simple API

Dotnet Simple API is a web application built with ASP.NET Core that provides endpoints for managing stocks and comments.

![alt text](https://github.com/sovannarapov/dotnet-simple-api/blob/main/Dotnet-Simple-API.png)

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
- Pagination and sorting for stocks
- Filtering stocks by company name and industry

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

### Account

- `POST /api/v1/account/login` - Login to the account

- `POST /api/v1/account/register` - Register a new account

### Stock

- `GET /api/v1/stock` - Get all stocks

- `GET /api/v1/stock/{id}` - Get a stock by ID

- `POST /api/v1/stock` - Create a new stock

- `PUT /api/v1/stock/{id}` - Update a stock
    
- `DELETE /api/v1/stock/{id}` - Delete a stock

### Comment

- `GET /api/v1/comment` - Get all comments

- `GET /api/v1/comment/{id}` - Get a comment by ID

- `POST /api/v1/comment/{stockId}` - Create a new comment

- `PUT /api/v1/comment/{id}` - Update a comment

- `DELETE /api/v1/comment/{id}` - Delete a comment

### Portfolio

- `GET /api/v1/portfolio` - Get the user's portfolio

- `POST /api/v1/portfolio` - Add a stock to the user's portfolio

- `DELETE /api/v1/portfolio` - Delete a stock from the user's portfolio
