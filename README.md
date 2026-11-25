# ClinicBookingSystem

Clinic appointment booking REST API: doctors with weekly schedules, patients, and appointments.

## Features

- Doctors: profiles, schedules, doctor-with-schedule views
- Patients and appointments: book, update, cancel (appointment status enum)
- Auth: register/login with JWT access tokens, refresh tokens with revocation, roles (ASP.NET Core Identity)
- Layered architecture: Domain (models, repository contracts), Service + Service.Abstractions, Persistence (EF Core, generic repository, configurations, migrations), Presentation (controllers), Shared DTOs

## Tech

.NET 8, ASP.NET Core, Entity Framework Core, SQL Server, ASP.NET Core Identity + JWT, Swagger

## Running locally

1. Prerequisites: .NET 8 SDK, SQL Server.
2. Set the connection string and JWT options with `dotnet user-secrets`.
3. `dotnet ef database update`, then run `ClinicBookingSystem.Api`.
