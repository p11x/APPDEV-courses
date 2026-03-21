# Running the Complete Project

## What You'll Build In This File

A step-by-step guide to run the complete NodeMark application from setup to deployment.

## Complete Run Guide

## Step 1: Clone and Setup

```bash
# Clone the repository
git clone https://github.com/yourusername/nodemark.git
cd nodemark

# Install dependencies
npm install
```

## Step 2: Environment Configuration

```bash
# Copy environment template
cp .env.example .env

# Edit .env with your settings
# For local development with PostgreSQL:
DB_HOST=localhost
DB_PORT=5432
DB_NAME=nodemark
DB_USER=postgres
DB_PASSWORD=your_password
JWT_SECRET=dev-secret-change-in-production
```

## Step 3: Database Setup

```bash
# Create the database
createdb nodemark

# Run migrations
npm run migrate
# Output:
# ✓ Migration completed successfully
# Tables created:
#   - users
#   - bookmarks
#   - tags
#   - bookmark_tags
```

## Step 4: Run Locally

```bash
# Development mode (with auto-reload)
npm run dev

# Or production mode
npm start
```

The API is now running at http://localhost:3000

## Step 5: Test the API

```bash
# Register a new user
curl -X POST http://localhost:3000/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password123"}'

# Login
curl -X POST http://localhost:3000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password123"}'

# Use the token to create a bookmark
curl -X POST http://localhost:3000/bookmarks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{"title":"Node.js","url":"https://nodejs.org"}'
```

## Step 6: Run Tests

```bash
# Ensure test database exists
createdb nodemark_test

# Run tests
npm test
```

## Step 7: Run with Docker

```bash
# Start all services
docker compose up -d

# Wait for services to be healthy
docker compose ps

# Run migrations
docker compose exec api npm run migrate

# View logs
docker compose logs -f api
```

## API Endpoints Summary

```
Auth:
POST /auth/register    Create account
POST /auth/login       Get JWT token

Bookmarks (protected):
POST   /bookmarks           Create bookmark
GET    /bookmarks           List bookmarks
GET    /bookmarks/:id       Get bookmark
PATCH  /bookmarks/:id       Update bookmark
DELETE /bookmarks/:id       Delete bookmark

Export:
GET /bookmarks/export           CSV export
GET /bookmarks/export?format=json  JSON export
```

## What You've Built

Congratulations! You've built a complete REST API with:

- ✅ Express.js routing and middleware
- ✅ PostgreSQL database with migrations
- ✅ JWT authentication with bcrypt
- ✅ Full CRUD operations for bookmarks
- ✅ CSV and JSON export with streams
- ✅ Integration tests with supertest
- ✅ Docker deployment configuration

## Next Steps

This completes the NodeMark capstone project! You can:

1. Add more features (tags, favorites, sharing)
2. Add a frontend (React, Vue, or vanilla JS)
3. Deploy to a cloud provider (Railway, Render, AWS)
4. Add CI/CD pipelines

All the concepts from the Node.js learning guide have been applied in this real-world project.

## Congratulations! 🎉

You now have a production-ready REST API built with modern Node.js best practices!
