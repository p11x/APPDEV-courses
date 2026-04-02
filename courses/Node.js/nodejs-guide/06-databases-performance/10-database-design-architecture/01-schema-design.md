# Database Schema Design and Architecture

## What You'll Learn

- Schema design principles
- Normalization vs denormalization
- Indexing strategies
- Database architecture patterns

## Schema Design Principles

```sql
-- PostgreSQL schema with proper design
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL,
    name VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE posts (
    id SERIAL PRIMARY KEY,
    user_id INTEGER REFERENCES users(id) ON DELETE CASCADE,
    title VARCHAR(500) NOT NULL,
    content TEXT,
    published BOOLEAN DEFAULT false,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE tags (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE post_tags (
    post_id INTEGER REFERENCES posts(id) ON DELETE CASCADE,
    tag_id INTEGER REFERENCES tags(id) ON DELETE CASCADE,
    PRIMARY KEY (post_id, tag_id)
);

-- Indexes
CREATE INDEX idx_posts_user_id ON posts(user_id);
CREATE INDEX idx_posts_published ON posts(published) WHERE published = true;
CREATE INDEX idx_posts_created_at ON posts(created_at DESC);
```

## MongoDB Schema Design

```javascript
// Embedded document pattern (denormalized)
const userSchema = new mongoose.Schema({
    name: String,
    email: String,
    profile: {
        bio: String,
        avatar: String,
        social: {
            twitter: String,
            github: String,
        },
    },
    addresses: [{
        type: { type: String, enum: ['home', 'work'] },
        street: String,
        city: String,
        country: String,
    }],
});

// Reference pattern (normalized)
const postSchema = new mongoose.Schema({
    title: String,
    content: String,
    author: { type: mongoose.Schema.Types.ObjectId, ref: 'User' },
    tags: [{ type: mongoose.Schema.Types.ObjectId, ref: 'Tag' }],
});
```

## Architecture Patterns

```
Database Architecture Patterns:
─────────────────────────────────────────────
Monolith:
├── Single database
├── All tables in one schema
├── Simple but hard to scale
└── Best for: Small-medium apps

Microservices:
├── Database per service
├── Eventual consistency
├── Independent scaling
└── Best for: Large distributed systems

CQRS:
├── Separate read/write models
├── Optimized for each use case
├── Event sourcing optional
└── Best for: High-read/write ratio
```

## Best Practices Checklist

- [ ] Design schema based on query patterns
- [ ] Use appropriate normalization level
- [ ] Add indexes for frequently queried columns
- [ ] Document schema decisions
- [ ] Plan for schema evolution

## Cross-References

- See [Integration Patterns](../01-database-integration-patterns/01-mongodb-postgres.md) for setup
- See [Performance](../02-database-performance-optimization/01-query-optimization.md) for queries
- See [Scalability](../05-scalability-patterns/01-load-balancing.md) for scaling

## Next Steps

This completes Chapter 6. Proceed to [Chapter 7: Streams](../../07-streams-and-buffers/).
