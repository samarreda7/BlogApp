# MindJourney Blog App

MindJourney is an ASP.NET Core MVC blog application where users can create accounts, publish posts, search for other users, view user profiles, like posts, and add comments.

The project is built as a layered .NET solution to keep domain models, data access, business logic, and MVC presentation concerns separated.

## Features

- User registration and login with ASP.NET Core Identity
- Role seeding for `Admin` and `User`
- Create, edit, and delete blog posts
- View personal posts
- Search users by name or username
- View posts by another user profile
- Like and unlike posts
- Add, edit, and delete comments
- Comment and like counts on posts
- Entity Framework Core migrations for SQL Server
- Bootstrap-based responsive UI

## Tech Stack

- ASP.NET Core MVC
- .NET 9
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Bootstrap
- jQuery

## Solution Structure

```text
BlogApp/
|-- BlogApp.Core       # Domain models, DTOs, service/repository interfaces
|-- BlogApp.EF         # DbContext, repositories, EF Core migrations
|-- BlogApp.Service    # Business logic services
|-- BlogApp.MVC        # Controllers, views, static assets, app startup
`-- BlogApp.sln
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022 or another .NET-compatible IDE

### Setup

1. Clone the repository.

```bash
git clone <repository-url>
cd BlogApp
```

2. Update the database connection string.

In `BlogApp.MVC/appsettings.json`, replace the `cs` connection string with your local SQL Server connection.

Example:

```json
{
  "ConnectionStrings": {
    "cs": "Server=.;Database=BlogAppDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

3. Restore packages.

```bash
dotnet restore BlogApp.sln
```

4. Apply EF Core migrations.

```bash
dotnet ef database update --project BlogApp.EF --startup-project BlogApp.MVC
```

5. Run the MVC project.

```bash
dotnet run --project BlogApp.MVC
```

6. Open the application in the browser using the URL printed by the `dotnet run` command.

## Main User Flow

1. Register a new account.
2. Log in.
3. Create a post from the home page.
4. View your own posts from `My Posts`.
5. Search for other users.
6. Open a user's profile to view their posts.
7. Like posts and add comments.

## Current Status

This project demonstrates a working MVC blog application with authentication, EF Core persistence, layered architecture, and social interactions such as likes and comments.

Before production use, the following improvements are recommended:

- Add `app.UseAuthentication()` before `app.UseAuthorization()` in the MVC pipeline.
- Enforce ownership checks before editing or deleting posts and comments.
- Move local connection strings out of committed configuration.
- Add automated tests for services and authorization-sensitive behavior.
- Clean up unused package references and unused imports.
- Improve validation limits for user input.
- Add deployment configuration and a hosted demo.

## Planned Improvements

- Unit and integration tests
- Pagination for posts and comments
- Better profile pages
- Image upload support for posts or user avatars
- Admin dashboard
- Improved error handling and logging
- Production-ready configuration using environment variables

## License

This project is available for learning and portfolio purposes.
