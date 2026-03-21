# Project Folder Structure

## What You'll Build In This File

A complete understanding of the NodeMark project structure and the purpose of each folder and file.

## Full Project Structure

```
nodemark/
├── src/                          # Source code directory
│   ├── config/
│   │   └── index.js              # Centralized configuration
│   ├── db/
│   │   ├── index.js              # PostgreSQL connection pool
│   │   └── migrations/           # Database migrations
│   │       └── 001_initial.js    # Initial schema migration
│   ├── routes/
│   │   ├── auth.js               # Authentication routes
│   │   └── bookmarks.js          # Bookmark CRUD routes
│   ├── middleware/
│   │   └── auth.js               # JWT authentication middleware
│   ├── schemas/
│   │   ├── auth.js               # Zod schemas for auth validation
│   │   └── bookmarks.js          # Zod schemas for bookmark validation
│   └── index.js                  # Main Express application entry point
├── tests/                        # Test files
│   ├── auth.test.js              # Authentication endpoint tests
│   └── bookmarks.test.js         # Bookmark CRUD tests
├── scripts/                      # Utility scripts
│   └── migrate.js                # Database migration runner
├── .env                          # Environment variables (never commit this)
├── .env.example                  # Environment variables template
├── package.json                  # Project dependencies and scripts
├── jsconfig.json                 # JavaScript configuration for IDE support
└── Dockerfile                    # Docker container definition
```

## Folder Explanations

### `src/config/`

This folder holds centralized configuration for the application. Using a config module ensures all settings are in one place and makes it easy to change behavior based on environment (development, production, test).

- **index.js** - Exports configuration values loaded from environment variables with sensible defaults

### `src/db/`

Database-related code lives here, keeping the rest of the application clean.

- **index.js** - Creates and exports the PostgreSQL connection pool using the `pg` library
- **migrations/** - Contains migration scripts that set up the database schema

### `src/routes/`

Express route handlers. Each route file handles a specific resource.

- **auth.js** - POST /auth/register and POST /auth/login endpoints
- **bookmarks.js** - All bookmark CRUD endpoints plus export endpoints

### `src/middleware/`

Express middleware functions that can be applied to routes.

- **auth.js** - JWT verification middleware that protects routes

### `src/schemas/`

Zod validation schemas for request body validation.

- **auth.js** - Validates registration and login request bodies
- **bookmarks.js** - Validates bookmark create/update request bodies

### `tests/`

Integration tests using supertest and node:test.

- **auth.test.js** - Tests for authentication endpoints
- **bookmarks.test.js** - Tests for bookmark CRUD endpoints

### `scripts/`

One-time utility scripts.

- **migrate.js** - Runs database migrations to set up the schema

## How It Connects

This structure follows the patterns from the learning guide:

- ES Modules throughout (see [04-npm-and-packages/esm-modules/01-import-export.md](../../../04-npm-and-packages/esm-modules/01-import-export.md))
- Centralized config similar to [10-deployment/environment/02-config-module.md](../../../10-deployment/environment/02-config-module.md)
- Organized like a production Express app (see [05-express-framework/getting-started/01-express-setup.md](../../../05-express-framework/getting-started/01-express-setup.md))

## Common Mistakes

- Putting all code in one file - leads to unmaintainable spaghetti
- Not separating routes from business logic
- Hardcoding configuration values instead of using environment variables
- Forgetting to add new files to .gitignore

## Try It Yourself

### Exercise 1: Explore the Structure
Create the folder structure manually and note what each folder will contain.

### Exercise 2: Add a New Route
Plan where you would add a new route file for "tags" functionality.

### Exercise 3: Consider Separation
Think about what would happen if you put everything in src/index.js instead of separating concerns.

## Next Steps

Continue to [02-package-json.md](./02-package-json.md) to set up package.json with all required dependencies.
