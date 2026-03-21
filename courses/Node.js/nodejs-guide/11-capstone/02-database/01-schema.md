# Database Schema Design

## What You'll Build In This File

The complete PostgreSQL schema for NodeMark with users, bookmarks, and tags tables with proper relationships.

## Complete SQL Schema

```sql
-- NodeMark Database Schema
-- Run this SQL to create all required tables

-- Users table: stores authenticated users
-- Password is stored as bcrypt hash, never plaintext
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,                    -- Auto-incrementing primary key
    email VARCHAR(255) NOT NULL UNIQUE,       -- User's email (unique constraint)
    password_hash VARCHAR(255) NOT NULL,       -- bcrypt hash of password
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,  -- When user registered
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP   -- Last profile update
);

-- Bookmarks table: stores user's saved bookmarks
CREATE TABLE IF NOT EXISTS bookmarks (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,  -- Foreign key to users
    title VARCHAR(500) NOT NULL,                -- Bookmark title
    url VARCHAR(2000) NOT NULL,                 -- Bookmark URL (validated before save)
    description TEXT,                           -- Optional description
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Constraint: user_id + url combination must be unique per user
    UNIQUE(user_id, url)
);

-- Tags table: allows users to categorize bookmarks
CREATE TABLE IF NOT EXISTS tags (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name VARCHAR(100) NOT NULL,                 -- Tag name (e.g., "work", "tutorial")
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Each user can only have one tag with a given name
    UNIQUE(user_id, name)
);

-- Junction table: bookmarks can have multiple tags
CREATE TABLE IF NOT EXISTS bookmark_tags (
    bookmark_id INTEGER NOT NULL REFERENCES bookmarks(id) ON DELETE CASCADE,
    tag_id INTEGER NOT NULL REFERENCES tags(id) ON DELETE CASCADE,
    PRIMARY KEY (bookmark_id, tag_id)  -- Composite primary key (no duplicates)
);

-- Indexes for better query performance
CREATE INDEX idx_bookmarks_user_id ON bookmarks(user_id);           -- Filter by user
CREATE INDEX idx_tags_user_id ON tags(user_id);                     -- Filter tags by user
CREATE INDEX idx_bookmark_tags_bookmark_id ON bookmark_tags(bookmark_id);  -- Join efficiency
CREATE INDEX idx_bookmark_tags_tag_id ON bookmark_tags(tag_id);             -- Filter by tag
```

## Schema Diagram

```
┌─────────────┐       ┌─────────────┐       ┌─────────────┐
│    users    │       │  bookmarks  │       │    tags     │
├─────────────┤       ├─────────────┤       ├─────────────┤
│ id (PK)     │       │ id (PK)     │       │ id (PK)     │
│ email       │◄──────│ user_id (FK)│       │ user_id (FK)│
│ password    │       │ title       │       │ name        │
│ created_at  │       │ url         │       │ created_at  │
│ updated_at  │       │ description │       └──────┬──────┘
└─────────────┘       │ created_at  │              │
                      │ updated_at  │       ┌──────┴──────┐
                      └──────┬──────┘       │bookmark_tags│
                             │              ├─────────────┤
                             │              │ bookmark_id │
                             └──────────────│ tag_id      │
                                           └─────────────┘
```

## Key Design Decisions

### Why CASCADE Deletes?
When a user is deleted, all their bookmarks and tags are automatically deleted:
```sql
REFERENCES users(id) ON DELETE CASCADE
```
This prevents orphaned records and maintains data integrity.

### Why UNIQUE Constraints?
- `email` - Each user must have a unique email
- `user_id + url` - Users can't save the same URL twice
- `user_id + name` - Users can't create duplicate tag names

### Why Indexes?
Indexes speed up common queries:
- Finding all bookmarks for a user
- Finding all tags for a user
- Joining bookmarks with their tags

## How It Connects

This schema connects to PostgreSQL concepts from:
- [06-databases/postgres/01-pg-setup.md](../../../06-databases/postgres/01-pg-setup.md) - Using pg Pool
- [06-databases/postgres/02-parameterized.md](../../../06-databases/postgres/02-parameterized.md) - Parameterized queries

## Common Mistakes

- Not using foreign key constraints (leads to orphaned data)
- Forgetting indexes (slow queries)
- Not using ON DELETE CASCADE (stale data)
- Storing passwords as plaintext (security vulnerability)

## Try It Yourself

### Exercise 1: Run the Schema
Connect to your PostgreSQL and run the CREATE TABLE statements.

### Exercise 2: Add a Column
Add a `favicon_url` column to bookmarks.

### Exercise 3: Create Index
Add an index on `bookmarks.url` for fast URL lookups.

## Next Steps

Continue to [02-migrations.md](./02-migrations.md) to create a migration runner.
