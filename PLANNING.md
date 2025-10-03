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

### Technology Stack
* Back-end and front-end: `Nuxt` and `Vue.js`
* Database Client: The Nuxt module from NPM package `@nuxtjs/supabase`
* Primary Programming Language: `TypeScript`
* QA Testing: Implemented using the Nuxt module `@nuxt/test-utils`.
* CSS styling will mostly be implemented using `@nuxt/ui` and `TailwindCSS` classes; and custom CSS only where necessary.
* Icons: `@nuxt/icons` and the font-awesome 7 solid icon package via the `@iconfiy-json/fa7-solid`.
* Authentication & Authorization: TBD (probably `Supabase` or potentially `OAuth 2.0` and `OpenID Connect` via `Auth0`).
* Logging: TODO: Research applicable logging solutions that are compatible with tech stack (where would logs be stored???)
* Hosting: Nuxt hosting provided by `vercel.com`; database hosting provided by `supabase.com`.
* CI/CD: Vercel build and deployment system pulling from private GitHub repo
* Source Control: GitHub
* State management: Implemented using `pinia` via the Nuxt module `@pinia/nuxt`.

### Architectural Guidelines
* Prefer functional programming paradigms and patterns over classical programming.
* Use the repository pattern to abstract the data-access layer away from consuming components.
* Abstract low-level services using TypeScript interfaces to make migrations and mocking for unit-testing easier.
* Reusable UI components should be placed in a subfolder with the same name as the componet under the `/app/components` folder.


## Secutiry Considerations
* Site will use SSL/TLS (certificate provided by Vercel, upon configuring the domain)
* Role-based Access Control (RBAC)
* API keys will be stored in .env files (or secrets stored/provided on the server side) and access via environment variables.


## Deployment Strategy
* The website will be deployed to vercel.com automatically, when commits are merged into the `master` branch on GitHub.

