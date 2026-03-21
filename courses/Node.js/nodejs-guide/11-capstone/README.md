# NodeMark - Personal Bookmark Manager

## What You'll Build

A fully functional REST API for a personal bookmark manager called "NodeMark" that lets users save, organize, and export their bookmarks. This capstone project exercises every major concept from the Node.js learning guide:

- **Express** routes and middleware ([05-express-framework](../05-express-framework/getting-started/01-express-setup.md))
- **PostgreSQL** database with parameterized queries ([06-databases](../06-databases/postgres/01-pg-setup.md))
- **JWT authentication** with bcrypt passwords ([08-authentication](../08-authentication/jwt/01-jwt-basics.md))
- **Streams** for CSV/JSON export ([07-streams-and-buffers](../07-streams-and-buffers/streams/03-pipe.md))
- **node:test** suite ([09-testing](../09-testing/node-test-runner/01-test-basics.md))
- **dotenv** + config module ([10-deployment](../10-deployment/environment/01-env-variables.md))
- **Docker** + docker-compose ([10-deployment](../10-deployment/docker/01-dockerfile.md))
- **ES Modules** throughout ([04-npm-and-packages](../04-npm-and-packages/esm-modules/01-import-export.md))

## Prerequisites

Before starting this capstone, ensure you have:

1. **Node.js 20+** installed - verify with `node -v`
2. **PostgreSQL 15+** installed locally or use Docker
3. **npm** package manager (comes with Node.js)
4. Basic understanding of REST APIs and HTTP methods

## What You'll Learn

By building NodeMark, you'll learn how to:

- Build a complete REST API from scratch
- Implement JWT-based authentication
- Work with PostgreSQL using parameterized queries (prevent SQL injection)
- Use Node.js streams for data export
- Write integration tests with supertest
- Containerize a Node.js application with Docker

## API Endpoints

Here's what we'll build:

```
Authentication:
POST /auth/register    - Create new user account
POST /auth/login       - Login and receive JWT token

Bookmarks (protected):
POST /bookmarks        - Create a new bookmark
GET /bookmarks         - List all bookmarks (with pagination & filtering)
GET /bookmarks/:id     - Get single bookmark
PATCH /bookmarks/:id  - Update a bookmark
DELETE /bookmarks/:id  - Delete a bookmark

Export:
GET /bookmarks/export              - Export as CSV
GET /bookmarks/export?format=json  - Export as JSON
```

## Project Structure

```
nodemark/
├── src/
│   ├── config/          # Configuration modules
│   ├── db/             # Database connection and migrations
│   ├── routes/         # Express route handlers
│   ├── middleware/     # Express middleware (auth, validation)
│   ├── schemas/       # Zod validation schemas
│   └── index.js       # Main application entry point
├── tests/              # Test files
├── scripts/            # Utility scripts (migrations)
├── .env               # Environment variables
├── package.json       # Dependencies
└── Dockerfile         # Docker container definition
```

## Quick Start

```bash
# Clone and setup
git clone https://github.com/yourusername/nodemark.git
cd nodemark

# Install dependencies
npm install

# Setup environment
cp .env.example .env
# Edit .env with your database credentials

# Run migrations
npm run migrate

# Start the server
npm run dev

# In another terminal, run tests
npm test
```

## Why This Project?

This capstone synthesizes all the concepts from the Node.js guide:

| Concept | Where It's Used |
|---------|-----------------|
| ES Modules | All files use import/export |
| Express middleware | Auth middleware, error handling |
| PostgreSQL + pg | User and bookmark storage |
| JWT + bcrypt | Secure authentication |
| Zod | Request body validation |
| Streams | CSV/JSON export endpoints |
| node:test + supertest | Full test suite |
| dotenv | Environment configuration |
| Docker | Production deployment |

## Next Steps

Continue to [01-project-setup/01-folder-structure.md](./01-project-setup/01-folder-structure.md) to set up the project structure.
