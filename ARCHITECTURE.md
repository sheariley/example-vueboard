# Stack Overview

* API runtime: **.NET 8**. Use minimal APIs.
* GraphQL server: **Hot Chocolate** schema-first.
* DB access: **Dapper** for performance-critical paths; **EF Core** where developer productivity matters. Use prepared statements and parameterized queries.
* Caching/batching: **DataLoader** for N+1 issues
* Auth: **Supabase Auth (OAuth)** on the client; `.NET` validates tokens using the Supabase JWT secret and refresh rules.
* Deployment: Linux containers (Docker) + reverse proxy (NGINX/Traefik).
* Observability: Application Insights/Prometheus + Grafana + structured logging (Serilog).
* Secrets: Still TBD; potentially Docker secrets or Kubernetes secrets

---

## Full-stack Flow

* Vue talks GraphQL → .NET GraphQL server (hosted in container).
* .NET validates Supabase JWT on each request and **forwards the user’s JWT** when making DB calls, so Postgres RLS still applies.
* Use the service role key only for admin server actions; otherwise operate as the user.
* Use Hot Chocolate (GraphQL server) + DataLoader pattern + EF Core/Dapper for DB access.
* Keep GraphQL resolvers thin; push heavy data transformation into stored procedures or functions where it makes sense.


# Key implementation patterns & how to avoid common mistakes

### 1) **Preserve Postgres RLS**

* **Do this:** Validate the Supabase JWT in .NET, then connect to Postgres as a *regular DB connection* but send the user’s JWT to Postgres so the DB can apply RLS.
  * Add an DbConnectionInterceptor implementation that sets the user's claims when the Entity Framework context establishes a connection.
* **Avoid:** Using the service role key for ordinary requests — that bypasses RLS.

### 2) **JWT handling & refresh**

* Vue authenticates with Supabase OAuth (PKCE). Store tokens securely:
  * For SPAs, store access token in memory or a secure cookie (httpOnly cookie preferred via your backend if you want refresh managed server-side).
* .NET: validate JWT `issuer`, `audience`, `exp`, and signature (using Supabase JWT secret/metadata).
* Refresh tokens: call Supabase auth endpoints; do *not* try to refresh tokens with the service role key.

### 3) **GraphQL server design**

* Schema: keep business entities explicit. Avoid resolving large nested trees with n+1 DB hits.
* Use **DataLoader** for batching and caching per GraphQL request.
* Keep resolvers thin — move heavy logic into stored procedures or database functions.
* Implement persisted queries or APQ (automatic persisted queries) to reduce payloads and improve cacheability.

### 4) **Security controls**

* All communication TLS-only.
* CORS: restrict origins to your frontends, preflight endpoints for OAuth redirects.
* Rate-limit all auth endpoints and suspicious GraphQL endpoints.
* Use CSP, secure cookies, and sameSite settings.
* Monitor service-role key usage carefully; treat it like a root password.

### 5) **Performance & scaling**

* Use Supabase inbuilt **connection pooling**. Supabase/Postgres has connection limits — don’t spawn one DB connection per request.
* Cache GraphQL query results in Redis where safe and invalidate intelligently. This can be done later in the project's development.

---

# Concrete sequence flows (2 important ones)

### Flow: Client reads user data (recommended flow)

1. User logs in with Supabase OAuth (PKCE) → receives JWT.
2. Vue sends GraphQL request to .NET GraphQL server with `Authorization: Bearer <jwt>`.
3. .NET middleware validates JWT signature & claims.
4. .NET Sets DB session variables from token claims and runs SQL queries that rely on those claims for RLS.
5. Postgres enforces RLS, returns rows.
6. Hot Chocolate resolves and returns GraphQL response to client.

### Flow: Admin action (server-only privileged)

1. Vue calls .NET admin GraphQL mutation (admin role) — authenticated and authorized in .NET.
2. .NET uses service role to perform admin operation in Supabase (use strictly server-side).
3. Log action, emit audit event.

---

# Quick notes and gotchas

* If you forward JWTs to the DB, ensure you validate them first in the API — don’t pass arbitrary tokens straight through.
* Keep the service-role key in vaults; rotate regularly.
* Test RLS thoroughly (unit tests + integration tests) — this is your last line of defense.
