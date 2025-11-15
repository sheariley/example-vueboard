# Project Overview
This project aims to create a Kanban style project management web application.

## Terms
* Work-item: An item within a project that resides in a column.


## Initial Project Scope
* Allow users to create new projects.
* Allow users to view a list of projects.
* Allow users to view, edit, and delete projects.
* Allow users to add and remove columns within a project.
* Allow users to set a column as the default column.  The inital default column will be the first one added to a project.
* Allow users to create new work-items in a project and place them into a column.
* Allow users to edit work-items.
* Allow users to move work-items from a column to another column.


## Initial Pages Needed
* Landing page
* Sign-in page
* Sign-up page
* Projects list page
* Project board page


## Initial Components Needed
* SidebarMenu
* ProjectCard
* ProjectBoard
* BoardColumn
* WorkItemCard
* CardTag
* CardModal

## UI/UX Design
* The design should be simple and easy to navigate.
* The look and feel should be professional, calming, and inviting.
* The design should also be responsive and implement a mobile-first paradigm.
* The styling should be implemented mostly by using CSS classes from the `TailwindCSS` package.
* The color scheme is dictated by the Nuxt module `@nuxt/ui` and using directives from that package to define the theme.

## Technical Architecture

### Technology Stack Overview
* Source Control: GitHub
* Primary Programming Languages:
  * Client-side:
    * `TypeScript`
    * `CSS`
    * `HTML`
  * Server-side:
    * `TypeScript` for `Nuxt`
    * `C#` (C-sharp) for `.NET 8` web API
* Server-side Architecture:
  * `Nuxt` for server-side rendering of front-end
  * `.NET 8` for GraphQL web API running in docker container
  * `Hot Chocolate` for server-side GraphQL implementation
* Client-side Architecture:
  * Primary Framework: `Vue.js`
  * State management: Implemented using `pinia` via the Nuxt module `@pinia/nuxt`.
  * CSS styling will mostly be implemented using `@nuxt/ui` and `TailwindCSS` classes; and custom CSS only where necessary.
  * Icons: `@nuxt/icons` and the font-awesome 7 solid icon package via the `@iconfiy-json/fa7-solid`.
* Authentication & Authorization: `OAuth 2.0` via JWT tokens passed through `.NET 8 Web API` to `Supabase` with RLS
* QA Testing:
  * Server-side: `xUnit.net`
  * Client-side: Implemented using the Nuxt module `@nuxt/test-utils` and `Playwright`.
* Logging: TODO: Research applicable logging solutions that are compatible with tech stack (where would logs be stored???)
* CI/CD: 
  * Front-end: Vercel build and deployment system pulling from private GitHub repo
  * Back-end: Managed container build by Render (render.com)
* Hosting:
  * Front-end (`Nuxt` and `Vue.js`) hosting provided by `vercel.com`
  * Database hosting provided by `supabase.com`
  * `.NET 8` container hosted by `Render` (render.com)

### Architectural Guidelines
* Prefer functional programming paradigms and patterns over classical programming.
* Use the repository pattern to abstract the data-access layer away from consuming components.
* Abstract low-level services using TypeScript interfaces to make migrations and mocking for unit-testing easier.
* Reusable UI components should be placed in a subfolder with the same name as the componet under the `/app/components` folder.


## Security Considerations
* Site will use SSL/TLS (certificate provided by Vercel, upon configuring the domain)
* Role-based Access Control (RBAC)
* API keys will be stored in .env files (or secrets stored/provided on the server side) and access via environment variables.


## Deployment Strategy
* The website will be deployed to vercel.com automatically, when commits are merged into the `master` branch on GitHub.

