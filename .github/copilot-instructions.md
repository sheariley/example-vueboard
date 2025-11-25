<!-- Copied guidance tailored for AI coding agents working in the `vueboard-app` repo. -->
# Copilot / AI Agent Instructions — Vueboard

Be concise. This file contains the minimal, high-value knowledge an AI coding agent needs to be productive in this repository.

1) Big picture
- **Runtime & API:** .NET 8 minimal APIs host a Hot Chocolate GraphQL server in `web-api/` (see `web-api/Program.cs`).
- **Client:** Nuxt 3 app in `web-client/` (see root `README.md` for Nuxt commands).
- **DB & Auth:** Supabase (Postgres + RLS). The architecture is documented in `ARCHITECTURE.md` — read it before changing auth, JWT handling, or DB access.

2) Primary code areas to inspect
- `web-api/Program.cs` — app startup, auth, CORS, GraphQL registration, DataLoader types, and DI bindings.
- `web-api/GraphQL/*` — GraphQL entrypoints (e.g., `Query.cs`, `Mutation.cs`, types). Keep resolvers thin and use DataLoaders.
- `data-access/vueboard-repositories/` — domain models (e.g., `Models/Project.cs`) and repository interfaces.
- `data-access/vueboard-repositories-entity-framework/` — EF context, `IVueboardDbContext`, repository EF implementations and `QueryRoots` used by GraphQL.
- `supabase/` — DB migrations and seeds; updates here affect production schema and RLS behavior.
- `mock-data/db.json` — local mock dataset used for quick frontend testing.

3) Important patterns & constraints (do not break these)
- **Forward user JWT to Postgres for RLS:** The app validates incoming JWTs, then forwards the user claims to Postgres via a DB connection interceptor (`JwtSessionInterceptor` referenced in `Program.cs`). Never bypass RLS by using the service role key for normal requests.
- **Resolvers must be thin:** Heavy data logic belongs in repositories, query roots, stored procedures, or DB functions. GraphQL types registered in `Program.cs` demonstrate the canonical type list.
- **Use DataLoader for N+1 prevention:** DataLoader registrations live in `Program.cs` and corresponding `*DataLoader` classes under GraphQL code.
- **Soft deletes:** Models use `VueboardSoftDeleteEntity` and queries commonly filter `!IsDeleted` (see `web-api/GraphQL/Query.cs`). Keep this filter in read queries unless intentionally querying soft-deleted rows.

4) Local development & common commands
- Start frontend dev server: from `web-client/` run `npm install` then `npm run dev` (see root README).
- Start API locally: from repo root run `dotnet build` then `dotnet run --project web-api/vueboard.api.csproj`. There are VS Code tasks in the workspace: `build`, `publish`, and `watch` (dotnet watch run). Use `watch` for iterative GraphQL development.
- Docker: `docker build -t vueboard.api .` inside `web-api/` builds the API image.

5) Environment variables the API expects (declared/used in `Program.cs` and `web-api/README.md`)
- `JWT__Issuer`, `JWT__Audience`, `JWT__Secret` — for JWT validation.
- `CORS_ALLOWED_ORIGINS` — comma-separated origins for CORS policy.
- DB config: provided via `IVueboardDbContextConfigProvider` implementations; check `data-access/*/Config` for details.

6) Tests, migrations and DB changes
- Supabase migrations live in `supabase/migrations/`. Use `supabase` CLI to generate diffs and apply migrations (this repo included generated files). Keep migrations in sync with model changes.
- Seeds are in `supabase/seeds/` and `mock-data/db.json` for lightweight local testing.

7) Integration points / places to be careful
- `JwtSessionInterceptor` and `SupabaseUserIdAccessor` — changes here influence RLS and authorization. Review `web-api/Auth/` before edits to auth flows.
- `VueboardDbContextEnvPostgresConfigProvider` (registered in `Program.cs`) defines how the EF Core context is configured from env vars.
- `GraphQL/*Type` registrations in `Program.cs` determine runtime schema; adding types requires registrations here.

8) Examples & quick references
- To add a new GraphQL query: add query method in `web-api/GraphQL/Query.cs` or a new Query class, expose repository via DI, and register any DataLoader used in `Program.cs`.
- To change DB connection behavior (e.g., forwarding claims): edit `web-api/Auth/JwtSessionInterceptor.cs` interceptor registration in `Program.cs`.

9) When to ask a human
- Any change that affects JWT forwarding, the service-role key usage, migrations in `supabase/`, or connection pooling must be reviewed by the team.

If anything here is unclear or you want me to expand an area (for example, list exact DataLoader and repository class files), tell me which section to expand and I will iterate.
