# Sports Management System

A web application where you can manage and organize sports clubs, matches, stadiums and fans.

![Homepage](https://github.com/m0hossam/sports-management-system/assets/115721045/8aa3c980-8199-4bd7-9674-0d67cce8e4df)

## Table of Contents

- [Objective](#objective)
- [Features](#features)
- [Tech](#tech)
- [How To Run](#how-to-run)
- [Credits](#credits)

## Objective

We built this project as part of our Relational Databases course, to apply the principles we learned to a web app that could be used in real life.

We studied database concepts like:
- EERDs: Extended Entity-Relationship Diagrams 
- Relational Schemas
- Functional Dependencies
- Normalization
- Authorization

We also learned some software patterns like:
- MVC Architecture
- Dependency Injection in ASP.NET Core MVC
- Entity Framework object-relational mapper
- LINQ
- Authentication and form-validation

## Features

Check out a [demo video](https://www.linkedin.com/posts/activity-7127735782198435840-tXdM?utm_source=share&utm_medium=member_desktop).
Our web app has an account authentication system, where each account is provided CRUD functionalities to manage their resources. 
Role-based authorization ensures each user has access to their role's resources only.
There are 5 types of user accounts:
-  **System Admin:** Manages all clubs, stadiums and fans in the system
-  **Assoication Manager:** Organizes matches between clubs
-  **Club Representative:** Sends requests on behalf of their club to host home matches
-  **Stadium Manager:** Handles host requests sent to their stadium
-  **Fan:** Views and buys tickets for matches

## Tech

We built the project using **ASP.NET Core MVC** and **Identity Authorization**, we used the **.NET 7.0** platform.
We used **Razor** pages for our views, stylizied with **Bootstrap**. We also used **SQL** and **LINQ** for our database logic.

## How To Run

To run the project using Visual Studio, you need to have _.NET 7.0_ and _MS SQL Server_: 
- Download the project and open the _SportsWebApp.sln_
- Create an _appsettings.json_ file
- Configure your _appsettings.json_ file with your own settings, this template might be helpful:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
- Connect MS SQL Server to your project
- Create a database instance and copy its Connection String from its properties
- Paste the Connection String into your _appsettings.json_ file in `"DefaultConnection": ""`
- Open Package Manager Console (PMC) in Visual Studio
- Enter the command `add-migration InitialCreate` then `update-database`
- Run the project

## Credits

This project was built by me and my friend [Abd002](https://github.com/Abd002)
