# RaceDay – Event Management System

## Project Overview
RaceDay is an event management system developed for South African road running, walking and cycling events. It allows organisers to manage events and participants to browse events, enrol and view their results.

## Technologies Used
- ASP.NET Core MVC
- ASP.NET Core Web API
- C#
- Entity Framework Core
- Microsoft SQL Server
- HTML, CSS and Bootstrap
- Azure Blob Storage integration
- Docker

## System Features

### Participants
- Register and log in.
- View available events.
- View event details and categories.
- Enrol in events.
- View personal enrolments.
- View race results.
- View and update profile information.
- Upload a profile picture.

### Organisers
- Log in securely.
- View and manage their events.
- Create and edit events.
- Manage event categories.
- View enrolled participants.
- Capture race results.

## Project Structure
- RaceDay.API – REST API and database operations.
- RaceDay.MVC – Web application interface.
- RaceDay.API.Tests – API testing project.

## Setup Instructions
1. Open the solution in Visual Studio.
2. Configure the SQL Server connection string in the API appsettings.json file.
3. Ensure SQL Server is running and the RaceDayDB database is available.
4. Restore NuGet packages.
5. Build the solution.
6. Run the RaceDay.API and RaceDay.MVC projects.
7. Access the MVC application through its configured local URL.

## Azure Blob Storage
The API includes an Azure Blob Storage service for profile-picture uploads. Configure the AzureBlobStorage connection string and container name in appsettings.json before using cloud uploads.

## Authentication
The system uses session-based authentication and role-specific access for Participants and Organisers.

## Developer
RaceDay Programming Project