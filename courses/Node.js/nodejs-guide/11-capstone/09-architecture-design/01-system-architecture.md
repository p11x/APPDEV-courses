# System Architecture Design for NodeMark

## What You'll Build In This File

A comprehensive architecture design for the capstone project, covering system design decisions, layer separation, and scalability planning.

## Architecture Overview

```
NodeMark System Architecture:
─────────────────────────────────────────────

┌─────────────────────────────────────────────────┐
│                   Client Layer                   │
│  Browser / Mobile App / API Consumers           │
└──────────────────────┬──────────────────────────┘
                       │ HTTPS
┌──────────────────────▼──────────────────────────┐
│               API Gateway / Load Balancer        │
│  Nginx / Cloud LB — SSL termination, routing    │
└──────────────────────┬──────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────┐
│               Application Layer                  │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐      │
│  │ Auth     │  │ Bookmarks│  │ Export   │      │
│  │ Routes   │  │ Routes   │  │ Routes   │      │
│  └────┬─────┘  └────┬─────┘  └────┬─────┘      │
│       │              │              │            │
│  ┌────▼──────────────▼──────────────▼─────┐     │
│  │         Middleware Layer                 │     │
│  │  Auth │ Validation │ Error │ Logging   │     │
│  └─────────────────────┬──────────────────┘     │
│                        │                        │
│  ┌─────────────────────▼──────────────────┐     │
│  │         Service / Business Logic       │     │
│  └─────────────────────┬──────────────────┘     │
└────────────────────────┼────────────────────────┘
                         │
┌────────────────────────▼────────────────────────┐
│               Data Layer                         │
│  ┌──────────────┐  ┌──────────────┐             │
│  │ PostgreSQL   │  │ Redis Cache  │             │
│  │ (primary DB) │  │ (sessions,   │             │
│  │              │  │  rate limit) │             │
│  └──────────────┘  └──────────────┘             │
└─────────────────────────────────────────────────┘
```

## Layer Responsibilities

```javascript
// Layer separation pattern
// src/index.js — Entry point wires layers together

import express from 'express';
import { config } from './config/index.js';
import { pool } from './db/index.js';
import { authRouter } from './routes/auth.js';
import { bookmarksRouter } from './routes/bookmarks.js';
import { errorHandler, requestLogger } from './middleware/index.js';

const app = express();

// ── Infrastructure Middleware ──
app.use(express.json({ limit: '1mb' }));
app.use(requestLogger);

// ── Routes (Presentation Layer) ──
app.use('/auth', authRouter);
app.use('/bookmarks', bookmarksRouter);

// ── Error Handler (must be last) ──
app.use(errorHandler);

// ── Start ──
app.listen(config.port, () => {
    console.log(`NodeMark running on port ${config.port}`);
});
```

## Architecture Decision Records (ADR)

```
ADR-001: Use PostgreSQL as primary database
─────────────────────────────────────────────
Context: Need relational data with users, bookmarks, tags
Decision: PostgreSQL with node-postgres (pg)
Consequences:
├── Pro: ACID transactions, mature, great Node.js support
├── Pro: Built-in JSON support for flexible fields
├── Con: Requires server management (or managed service)
└── Alternative: SQLite (too limited), MongoDB (overkill)

ADR-002: JWT for authentication (not sessions)
─────────────────────────────────────────────
Context: API consumed by multiple clients
Decision: JWT with refresh tokens
Consequences:
├── Pro: Stateless, works across multiple instances
├── Pro: Standard for API authentication
├── Con: Token revocation requires denylist
└── Alternative: Sessions (needs shared state)

ADR-003: Zod for input validation
─────────────────────────────────────────────
Context: Need type-safe request validation
Decision: Zod schemas at route boundary
Consequences:
├── Pro: Type inference, composable, fast
├── Pro: Single source of truth for validation
├── Con: Learning curve for complex schemas
└── Alternative: Joi (more verbose), express-validator
```

## Database Schema Design

```sql
-- Core schema for NodeMark
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    name VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE bookmarks (
    id SERIAL PRIMARY KEY,
    user_id INTEGER REFERENCES users(id) ON DELETE CASCADE,
    title VARCHAR(500) NOT NULL,
    url TEXT NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(user_id, url) -- One bookmark per URL per user
);

CREATE TABLE tags (
    id SERIAL PRIMARY KEY,
    user_id INTEGER REFERENCES users(id) ON DELETE CASCADE,
    name VARCHAR(100) NOT NULL,
    UNIQUE(user_id, name)
);

CREATE TABLE bookmark_tags (
    bookmark_id INTEGER REFERENCES bookmarks(id) ON DELETE CASCADE,
    tag_id INTEGER REFERENCES tags(id) ON DELETE CASCADE,
    PRIMARY KEY (bookmark_id, tag_id)
);

-- Indexes for common queries
CREATE INDEX idx_bookmarks_user_id ON bookmarks(user_id);
CREATE INDEX idx_bookmarks_created_at ON bookmarks(created_at DESC);
CREATE INDEX idx_tags_user_id ON tags(user_id);
CREATE INDEX idx_bookmark_tags_bookmark ON bookmark_tags(bookmark_id);
CREATE INDEX idx_bookmark_tags_tag ON bookmark_tags(tag_id);
```

## How It Connects

- Layers follow [05-express-framework](../../../05-express-framework/) patterns
- Database design follows [06-databases-performance](../../../06-databases-performance/)
- Security follows [08-authentication](../../../08-authentication/) patterns

## Common Mistakes

- Mixing business logic in route handlers instead of service layer
- Not planning for horizontal scaling from the start
- Skipping database indexes on foreign keys
- Not documenting architecture decisions

## Try It Yourself

### Exercise 1: Draw the Architecture
Draw the full architecture diagram for NodeMark on paper.

### Exercise 2: Identify Layers
For each file in `src/`, identify which layer it belongs to.

### Exercise 3: Plan Scaling
How would you scale NodeMark to handle 10,000 concurrent users?

## Next Steps

Continue to [02-monolith-microservices-deep-dive.md](./02-monolith-microservices-deep-dive.md).
