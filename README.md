# BreadAppSystem

BreadAppSystem is a .NET 8 solution for managing bread distribution workflows (beneficiaries, QR validation, distribution points, transactions, and reporting). The repository contains the primary ASP.NET Core Web API project and supporting projects.

This README provides quick start instructions and common development tasks. For more details, inspect individual project folders such as BreadApp_API.

Prerequisites
-------------
- .NET 8 SDK
- Visual Studio 2022/2026 or VS Code with C# tooling
- Optional: SQL Server (or other DB) for production/local database

Repository layout
-----------------
- BreadApp_API/         - ASP.NET Core Web API (primary project)
- src/                  - Domain and infrastructure libraries (if present)
- tests/                - Unit/integration tests (if present)
- BreadAppSystem.slnx   - Solution file

Quick start
-----------
1. Clone the repository:

   ```bash
   git clone https://github.com/CodedByWaheed/BreadAppSystem.git
   cd BreadAppSystem
   ```

2. Build and run the API (command line):

   ```bash
   dotnet restore
   dotnet build
   dotnet run --project BreadApp_API
   ```

3. Or open BreadAppSystem.slnx in Visual Studio and run the `BreadApp_API` project.

Configuration
-------------
The API uses standard ASP.NET Core configuration sources (appsettings.json, environment variables, user secrets).

- Provide a database connection string (e.g. `ConnectionStrings:DefaultConnection`).
- Provide JWT/auth secrets if authentication is enabled.

Do not commit secrets to source control. Use environment variables or user secrets for local development.

Database / EF Core
------------------
If the solution uses EF Core migrations, typical commands are:

  ```bash
  dotnet ef migrations add <Name> --project BreadApp_API --startup-project BreadApp_API
  dotnet ef database update --project BreadApp_API --startup-project BreadApp_API
  ```

Adjust project flags to match where the DbContext and startup host live.

Tests
-----
Run all test projects with:

  ```bash
  dotnet test
  ```

Common commands
---------------
- Build: `dotnet build`
- Run API: `dotnet run --project BreadApp_API`
- Run tests: `dotnet test`

Contributing
------------
Fork, create a feature branch, add tests for changes, and open a pull request. Keep changes focused and include a clear description.

License
-------
No license file is included. Add an appropriate license (for example MIT) if you intend to publish this repository.

Contact
-------
Open an issue in the repository for questions or reach out to the project owner.
