# Library Management System

An ASP.NET Core MVC web application for managing a small library's book catalogue, built with Entity Framework Core, ASP.NET Core Identity, and the repository pattern.

> Originally built as a coursework project (CSIS/CSIQ3734 — Advanced Web Development, Internet Programming).

## Features

- **Book catalogue (CRUD)** — add, edit, delete, and view books, each linked to a genre
- **Search, sort & filter** — search by title/author, filter by genre, sort by Title/Author/Price (ascending/descending)
- **Paging** — custom `QueryOptions` + `PageLink` tag helper for paged results
- **Genre management** — CRUD for book genres
- **Authentication** — register/login via ASP.NET Core Identity
- **Role-based authorization** — `Admin` and `Member` roles
- **Admin tools** — manage users (create/edit/delete, assign roles) and manage roles
- **Custom validation** — custom password and username validators
- **Custom tag helpers** — `ProfileImageTagHelper`, `PageLinkTagHelper`, `RoleUsersTagHelper`
- **Seed data** — sample genres/books and a default admin account created on first run

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core 8 (SQL Server)
- ASP.NET Core Identity
- Two separate `DbContext`s: one for application data (`AppDbContext`), one for Identity (`AppIdentityDbContext`)
- Bootstrap 5, jQuery, jQuery Validation

## Project Structure

```
Library.slnx
Library/
├── Controllers/        # Home, Book, Genre, Account, Admin, RoleAdmin
├── Models/              # Book, Genre, AppUser, ViewModels
├── Data/                # DbContexts, repositories, seed data
├── Infrastructure/      # Custom validators and tag helpers
├── Views/               # Razor views
├── Migrations/          # EF Core migrations (App + Identity)
└── wwwroot/             # Static assets (CSS, JS, libraries)
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB is used by default; Express/Developer/Azure SQL also work)

## Getting Started (Local Development)

1. **Clone the repository**

   ```bash
   git clone <your-repo-url>
   cd <repo-folder>/Library
   ```

2. **Configure the database connection**

   The default connection strings (in `appsettings.json`) point to SQL Server LocalDB:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryAppDB;Trusted_Connection=True;MultipleActiveResultSets=true",
     "IdentityConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraryAppIdentityDB;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

   Update these if you're using a different SQL Server instance.

3. **Apply migrations / run the app**

   The app applies pending migrations and seeds data automatically on startup (`SeedData.EnsurePopulated` and `SeedIdentityData.EnsurePopulated` in `Program.cs`), so you can simply run:

   ```bash
   dotnet restore
   dotnet run
   ```

   Or apply migrations manually first if you prefer:

   ```bash
   dotnet ef database update --context AppDbContext
   dotnet ef database update --context AppIdentityDbContext
   ```

4. **Open the app** at the URL shown in the console (e.g. `http://localhost:5269`).

## Default Admin Account

On first run, a default admin account is seeded:

- **Username:** `admin`
- **Password:** `Admin@123`

> ⚠️ This is hardcoded test/demo data. Change or remove this account before using the app in any real or publicly accessible environment.

## License

This project was created for academic purposes. Add a license of your choice if you intend to share or reuse it.
