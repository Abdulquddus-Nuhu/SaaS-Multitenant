# SaaS-MyStar

## Overview
SaaS-MyStar is a multi-tenant School Management System API built with C# following the principles of Clean Architecture. This API leverages the AuthPermissions.AspNetCore library to facilitate SaaS capabilities, specifically multi-tenancy, allowing different schools to operate within the same application instance while maintaining isolation of data.

## Features
- **Multi-Tenancy:** Built-in support for handling multiple tenants (schools) using the AuthPermissions.AspNetCore library.
- **Custom Authentication:** Implements JWT-based authentication to secure the API and manage user sessions effectively.
- **Tenant-Specific Data Management:** Custom middleware to retrieve tenant IDs and ensure data queries are scoped per tenant.
- **API Documentation:** Integrated Swagger UI for easy API endpoint navigation and testing.

## Architecture
This project is structured using Clean Architecture to ensure that it's easy to maintain and scalable. The architecture is divided into several layers:
- **Domain Layer:** Contains all entities, enums, exceptions, interfaces, types and logic specific to the domain.
- **Application Layer:** Houses the business logic and handles the use cases of the application.
- **Infrastructure Layer:** Implements persistence logic and external concerns like database access, file storage, and third-party services.
- **Presentation Layer:** Focuses on the API controllers which serve as the entry point for client interactions.

## Getting Started
To get the project up and running on your local machine, follow these steps:
1. Clone the repository:
git clone https://github.com/Abdulquddus-Nuhu/SaaS-MyStar.git

2. Navigate to the project directory.
3. This application uses PostgreSQL. Create a new profile in `Properties/launchSettings.json` under the `profiles` section and set up your PostgreSQL connection string and other environment variables:
```json
    "MyProfile": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7007;http://localhost:5009",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "DB_CONNECTION": "Host=localhost;Database=Db;password=password;username=postgres;",
        "ROOT_ADMIN_EMAIL": "your email",
        "ROOT_ADMIN_PHONENUMBER": "your phonenumber",
        "ROOT_DEFAULT_PASSWORD": "your password",
        "IT_HEAD_EMAIL": "admin2@email.com",
        "IT_HEAD_PHONENUMBER": "admin phonenumber",
        "IT_HEAD_PASSWORD": "admin password",
        "JWT_SECRET_KEY": "your jwt key"
      },
      "dotnetRunMessages": true
    },

```
4. Restore the required packages:
   dotnet restore
   
5. No need to manually apply migrations. The project is configured to automatically migrate and set up your database when you start the application:
  dotnet run --launch-profile MyProfile

6. Access the Swagger UI to interact with the API by navigating to:
   https://localhost:5001/swagger

7.Usage:
After starting the API, you can use any API client like Postman to send requests to the endpoints defined under the controllers. Alternatively, the Swagger UI provides a web-based interface to directly interact with the API.
