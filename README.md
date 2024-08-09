# Ultimate .NET Web API

Ultimate .NET Web API is a robust backend solution designed to handle various complex operations efficiently. This API provides a comprehensive set of features that are essential for modern web applications.

## Table of Contents

## Table of Contents

- [Key Features](#key-features)
  - [CORS (Cross-Origin Resource Sharing)](#1-cors-cross-origin-resource-sharing)
  - [Logging Service](#2-logging-service)
  - [Repository Pattern](#3-repository-pattern)
  - [DTO (Data Transfer Object) Classes](#4-dto-data-transfer-object-classes)
  - [Global Error Handling](#5-global-error-handling)
  - [Model Validation](#6-model-validation)
  - [Asynchronous Code](#7-asynchronous-code)
  - [Modular Design](#8-modular-design)
  - [Advanced Querying Capabilities](#9-advanced-querying-capabilities)
    - Paging
    - Filtering
    - Searching
    - Sorting
    - Data Shaping
  - [Rate Limiting](#10-rate-limiting)
  - [JWT and Identity for Authentication and Authorization](#11-jwt-and-identity-for-authentication-and-authorization)
  - [API Documentation with Swagger](#12-api-documentation-with-swagger)
- [API Routes](#api-routes)
  - [Authentication Controller](#authentication-controller)
  - [Companies Controller](#companies-controller)
  - [Employees Controller](#employees-controller)
  - [Token](#token)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Usage](#usage)
- [Contributing](#contributing)
- [Conclusion](#conclusion)
- [Acknowledgements](#acknowledgements)
- [License](#license)

## Key Features

### 1. CORS (Cross-Origin Resource Sharing)

- Configured CORS to allow cross-origin requests from any origin, method, and header.
- Exposed pagination metadata in the headers.

### 2. Logging Service

- Implemented a centralized logging service to capture and manage logs, aiding in monitoring and debugging.

### 3. Repository Pattern

- Employed the repository pattern to abstract data access logic, making the code more modular and maintainable.

### 4. DTO (Data Transfer Object) Classes

- Used DTOs to ensure a clear separation between the data models and API responses, improving data integrity and security.

### 5. Global Error Handling

- Integrated global error handling to manage exceptions consistently across the entire application.

### 6. Model Validation

- Implemented model validation to ensure data integrity before processing requests.

### 7. Asynchronous Code

- Utilized asynchronous programming to improve performance and scalability.

### 8. Modular Design

- Structured the codebase into different classes and layers to promote modularity and reusability.

### 9. Advanced Querying Capabilities

- **Paging**: Implemented pagination to handle large datasets efficiently.
- **Filtering**: Added filtering capabilities to retrieve data based on specific criteria.
- **Searching**: Integrated search functionality to allow quick data retrieval.
- **Sorting**: Enabled sorting to organize data based on user-defined parameters.
- **Data Shaping**: Provided data shaping options to return only the requested fields in the response.

### 10. Rate Limiting

- Implemented rate limiting to control the number of requests a client can make to the API, ensuring fair use and preventing abuse.

### 11. JWT and Identity for Authentication and Authorization

- Utilized JWT and ASP.NET Core Identity for secure authentication and role-based authorization.
- Implemented refresh tokens for maintaining session security.

### 12. API Documentation with Swagger

- Documented the API endpoints and models using Swagger for easy exploration and testing.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or any other compatible database)

### Installation

1. Clone the repository:

   ```bash
    https://github.com/YeabTesfaye/Ultimate.ASP.NET.Core.Web.API
   cd Ultimate-Dotnet-Web-API
   ```

2. Set up the database:

   - Update the connection string in `appsettings.json`.
   - Apply migrations:

     ```bash
     dotnet ef database update
     ```

3. Build the project:

   ```bash
   dotnet build
   ```

4. Run the project:

   ```bash
   dotnet run
   ```

### Usage

- Access the Swagger documentation at `http://localhost:3000/swagger` to explore and test the API endpoints.

## API Routes

### Authentication Controller

**I Register a new user**

- **POST** `http://localhost:3000/api/authentication`
- **Body:**

  ```json
  {
    "firstName": "string",
    "lastName": "string",
    "userName": "string",
    "password": "string",
    "email": "string",
    "phoneNumber": "string",
    "roles": ["string"]
  }
  ```

  **I Login a user**

- **POST** `http://localhost:3000/api/authentication/login`
- **Body:**

```json
{
  "username": "string",
  "password": "string"
}
```

## Companies Controller

**I Retrieve the list of all companies**

- **GET** `http://localhost:3000/api/companies`

**II Create a new company**

- **POST** `http://localhost:3000/api/companies`
- **Body**

```json
{
  "name": "string",
  "address": "string",
  "country": "string",
  "employees": [
    {
      "name": "string",
      "age": "integer",
      "position": "string"
    }
  ]
}
```

**III Retrieve a company by its unique id**

- **GET** `http://localhost:3000/api/companies/{id}`

**IV Delete a company by its unique id**

- **DELETE** `http://localhost:3000/api/companies/{id}`

**V Update a company by its unique id**

- **PUT** `http://localhost:3000/api/companies/{id}`

**VI Retrieve companies by their ids**

- **GET** `http://localhost:3000/api/companies/{ids}`

**VII Create a collection of new companies**

- **POST** `http://localhost:3000/api/companies/{ids}`

## Employees Controller

**I Retrieve a list of employees for a specific company**

- **GET** `http://localhost:3000/api/{companyId}/employees`

**II Create a new employee for a specific company**

- **GET** `http://localhost:3000/api/{companyId}/employees`

- **Body**

  ```json
  {
    "name": "string",
    "age": "integer",
    "position": "string"
  }
  ```

**III Retrieve an employee by their id**

- **GET** `http://localhost:3000/api/{companyId}/employees/{id}`

**IV Delete an employee from a specific company**

- **DELETE** `http://localhost:3000/api/{companyId}/employees/{id}`

**V Update an existing employee in a specific company**

- **PUT** `http://localhost:3000/api/{companyId}/employees/{id}`

**VI Partially update an existing employee in a specific company**

- **PATCH** `http://localhost:3000/api/{companyId}/employees/{id}`

## Token

**I Retrieve a refersh token**

- **POST** `http://localhost:3000/api/token/refresh`

-**body**

```json
{
  "token": "string",
  "refreshToken": "string"
}
```

## Contributing

Contributions are welcome! Please fork the repository and create a pull request.

## Conclusion

Ultimate .NET Web API is a comprehensive backend solution that integrates a wide range of essential features for modern web applications. From robust security measures to advanced querying capabilities, this API is designed to be scalable, maintainable, and easy to use. Whether you're building a small project or a large-scale enterprise application, Ultimate .NET Web API offers the tools and structure you need to succeed. Your contributions and feedback are highly appreciated as we continue to improve and expand the functionality of this API.

## Acknowledgements

A special thanks to Code Maze for their insightful book "Ultimate ASP.NET Core Web API," which served as a valuable resource in the development of this project.

## License

[MIT](https://choosealicense.com/licenses/mit/)
