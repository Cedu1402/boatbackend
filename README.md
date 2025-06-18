# Demo Backend Service

This is a simple ASP.NET Core backend using the Minimal API.

## Features

- JWT-protected CRUD endpoints for boats
- Login endpoint
- In-memory EF Core database
- Pre-seeded data (user and boats) on startup

## Shortcomings (due to time constraints)

- Password is stored in plain text
- CORS is set to `*` (open to all origins)
- Only one basic test
- In-memory database (no persistence)

## How to Run

```bash
dotnet run

```

or with docker

```bash
dockercompose up
```