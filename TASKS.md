# Project Tasks

## General Development Tasks
- [ ] Restructure repo to move the Vue web app into subfolder
- [ ] Outline new repo structure in markdown document; describing location of various high-level components (Vue web app, containerized .NET web API, etc...)

## UI Tasks
- [x] Add default card color properties to project objects in mock data JSON file
- [x] Add optional color properties to work item objects in mock data JSON file
- [x] Apply color properties to work item card component styles, with default color values from project used unless overridden in work item object
- [x] Allow editing of work item cards
- [x] Allow work item content to contain markdown and add rich editor
  - [x] Add a separate field for rich content called "Notes" and rename "content" property to "description".
- [x] Allow adding new columns
- [x] Allow removing columns
- [x] Allow removing work items
- [x] Allow adding of new projects
- [ ] Add list of projects to main nav menu
- [x] Allow saving of projects
- [ ] Add undo functionality at Project entity level???
- [ ] Add tooltips to all buttons


## Architectural Tasks
- [x] Create containerized **.NET 8** GraphQL web API.
- [x] Add Hot Chocolate and configure schema + DataLoader.
- [x] Add JWT middleware configured with Supabase JWT secret and issuer.
- [x] Implement a DbConnectionInterceptor to set claims in Postgres session upon establishing a connection to the DB.
- [x] Implement DataLoaders for relationships.
- [ ] Ensure database connection pooling is configured for database clients (.NET GraphQL web API).
- [ ] Configure CORS and TLS on the API and on Supabase OAuth redirect URIs.
- [ ] Add Redis for caching and use it for heavy read queries. (TBD later)
- [ ] Add logging, rate limiting, and health endpoints.
- [ ] Run security review: ensure no service-role key exposure in client code or build logs.