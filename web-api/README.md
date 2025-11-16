This is a minimal .NET 8 GraphQL API for the Vueboard project.

Features:
- HotChocolate GraphQL server
- JWT Bearer auth configured (use env vars to override)
- In-memory repositories (replace with EF Core / Dapper + Supabase/Postgres in future)
- Dockerfile for containerization

Environment variables:
- JWT__Issuer
- JWT__Audience
- JWT__Secret

Build and run locally:
- dotnet build
- dotnet run

Build Docker image:
- docker build -t vueboard.api .
