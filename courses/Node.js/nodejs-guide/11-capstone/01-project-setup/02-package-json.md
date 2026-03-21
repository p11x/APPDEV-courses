# Package.json Configuration

## What You'll Build In This File

A complete package.json with all necessary dependencies for NodeMark, along with custom scripts for development and testing.

## Complete package.json

```json
{
  "name": "nodemark",
  "version": "1.0.0",
  "description": "Personal bookmark manager REST API",
  "type": "module",
  "main": "src/index.js",
  "scripts": {
    "start": "node src/index.js",
    "dev": "node --watch src/index.js",
    "test": "node --test tests/*.test.js",
    "migrate": "node scripts/migrate.js",
    "migrate:undo": "node scripts/migrate.js --undo"
  },
  "keywords": ["nodejs", "express", "rest-api", "bookmarks"],
  "author": "",
  "license": "MIT",
  "dependencies": {
    "express": "^4.18.2",
    "pg": "^8.11.3",
    "bcrypt": "^5.1.1",
    "jsonwebtoken": "^9.0.2",
    "zod": "^3.22.4",
    "dotenv": "^16.3.1"
  },
  "devDependencies": {
    "supertest": "^6.3.3"
  },
  "engines": {
    "node": ">=20.0.0"
  }
}
```

## Dependencies Explained

### Production Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| **express** | ^4.18.2 | Web framework for building REST API |
| **pg** | ^8.11.3 | PostgreSQL client for Node.js |
| **bcrypt** | ^5.1.1 | Password hashing for secure storage |
| **jsonwebtoken** | ^9.0.2 | JWT creation and verification |
| **zod** | ^3.22.4 | Schema validation for request bodies |
| **dotenv** | ^16.3.1 | Load environment variables from .env |

### Dev Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| **supertest** | ^6.3.3 | HTTP assertions for testing Express routes |

## Scripts Explained

```bash
# Start production server
npm start
# Runs: node src/index.js

# Start development server with auto-reload
npm run dev
# Runs: node --watch src/index.js
# The --watch flag restarts on file changes (Node.js 20+)

# Run test suite
npm test
# Runs: node --test tests/*.test.js
# Uses built-in Node.js test runner

# Run database migrations
npm run migrate
# Runs: node scripts/migrate.js

# Undo last migration
npm run migrate:undo
# Runs: node scripts/migrate.js --undo
```

## Setting Type to Module

The `"type": "module"` field is critical - it tells Node.js to treat all .js files as ES Modules, enabling:

- Import/export syntax (import from, export default)
- Top-level await
- ESM module resolution

This matches modern Node.js best practices as covered in [04-npm-and-packages/esm-modules/02-module-resolution.md](../../../04-npm-and-packages/esm-modules/02-module-resolution.md).

## Version Ranges Explained

We use caret (^) version ranges:

- `"express": "^4.18.2"` means >= 4.18.2 and < 5.0.0
- Allows minor updates and patches but prevents breaking changes
- The latest compatible version is installed with `npm install`

## How It Connects

This package.json connects to everything in the guide:

- Uses ES Modules (see [04-npm-and-packages/npm-basics/01-package-json.md](../../../04-npm-and-packages/npm-basics/01-package-json.md))
- Dependencies match concepts: pg for [06-databases/postgres/01-pg-setup.md](../../../06-databases/postgres/01-pg-setup.md), bcrypt for [08-authentication/bcrypt/01-hashing.md](../../../08-authentication/bcrypt/01-hashing.md), jsonwebtoken for [08-authentication/jwt/02-sign-verify.md](../../../08-authentication/jwt/02-sign-verify.md)

## Common Mistakes

- Forgetting `"type": "module"` and using import syntax
- Using version ranges that are too loose (* or 0.0.0)
- Not specifying node engine version
- Putting production deps in devDependencies

## Try It Yourself

### Exercise 1: Initialize Project
Create a new folder and run `npm init -y`, then add the dependencies above.

### Exercise 2: Add a Script
Add a `"lint"` script that runs eslint (you'll need to install it).

### Exercise 3: Check Versions
Run `npm outdated` to see if any packages have newer versions available.

## Next Steps

Continue to [03-env-setup.md](./03-env-setup.md) to configure environment variables.
