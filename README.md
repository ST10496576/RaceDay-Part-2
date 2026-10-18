# RaceDay API

## Project Overview
RaceDay is an event management API designed to support running, walking, and cycling events. It allows users to register and log in, organisers to create events, participants to enrol in event categories, and participants to view their results.

## Features
- User registration and login
- Password hashing
- Session-based authentication
- Role-based access for organisers and participants
- View available events
- Organisers can create events
- Participants can enrol in event categories
- Participants can view their results
- SQL Server database integration
- Swagger API documentation

## Technologies Used
- C#
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- Microsoft SQL Server
- Swagger / Swashbuckle
- xUnit for unit testing
- Git and GitHub
- GitHub Actions for continuous integration

## Requirements
- Visual Studio 2022 or another compatible .NET IDE
- .NET 8 SDK
- Microsoft SQL Server
- SQL Server Management Studio (SSMS), recommended

## Setup Instructions
1. Clone or download this repository.
2. Open the solution in Visual Studio.
3. Ensure SQL Server is installed and running.
4. Create or restore the `RaceDayDB` database and ensure its tables exist.
5. Open `appsettings.json`.
6. Update the `RaceDayConnection` connection string to match your SQL Server instance.
7. Build the solution.

Example connection string:

    "RaceDayConnection": "Server=YOUR_SERVER_NAME;Database=RaceDayDB;Trusted_Connection=True;TrustServerCertificate=True;"

Replace `YOUR_SERVER_NAME` with your actual SQL Server instance name.

## Running the API
1. Set `RaceDay.API` as the startup project.
2. Run the application from Visual Studio.
3. Open the Swagger URL displayed by the application, usually:

   `https://localhost:7110/swagger/index.html`

4. Use Swagger to explore and test the available API endpoints.

## Running Unit Tests
1. Open the solution in Visual Studio.
2. Open Test Explorer.
3. Select **Run All Tests**.

Alternatively, run the tests from the repository directory using:

    dotnet test --configuration Release

## Continuous Integration
GitHub Actions is configured to build the project and run unit tests when code is pushed or a pull request is created.

youtube link :https://youtu.be/KU_YQ1JA5pM?si=A7o5WFed5UYbxUtK
