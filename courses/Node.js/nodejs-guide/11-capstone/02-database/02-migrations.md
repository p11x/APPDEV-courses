# Database Migrations

## What You'll Build In This File

A migration runner script using Node.js fs and pg modules to manage database schema changes.

## Complete Migration Script

Create `scripts/migrate.js`:

```javascript
// scripts/migrate.js - Database migration runner
// Runs SQL migrations to set up the database schema
// Usage: node scripts/migrate.js [--undo]

import { readFile } from 'fs/promises';
import { fileURLToPath } from 'url';
import path from 'path';
import pg from 'pg';
import { config, getDbConnectionString } from '../src/config/index.js';

// Configure pg client
const { Pool } = pg;

// Migration SQL - the complete schema from the previous file
const MIGRATION_SQL = `
-- Users table: stores authenticated users
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Bookmarks table: stores user's saved bookmarks
CREATE TABLE IF NOT EXISTS bookmarks (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    title VARCHAR(500) NOT NULL,
    url VARCHAR(2000) NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(user_id, url)
);

-- Tags table: allows users to categorize bookmarks
CREATE TABLE IF NOT EXISTS tags (
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    name VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(user_id, name)
);

-- Junction table: bookmarks can have multiple tags
CREATE TABLE IF NOT EXISTS bookmark_tags (
    bookmark_id INTEGER NOT NULL REFERENCES bookmarks(id) ON DELETE CASCADE,
    tag_id INTEGER NOT NULL REFERENCES tags(id) ON DELETE CASCADE,
    PRIMARY KEY (bookmark_id, tag_id)
);

-- Indexes for better query performance
CREATE INDEX IF NOT EXISTS idx_bookmarks_user_id ON bookmarks(user_id);
CREATE INDEX IF NOT EXISTS idx_tags_user_id ON tags(user_id);
CREATE INDEX IF NOT EXISTS idx_bookmark_tags_bookmark_id ON bookmark_tags(bookmark_id);
CREATE INDEX IF NOT EXISTS idx_bookmark_tags_tag_id ON bookmark_tags(tag_id);
`;

// Undo migration - drops all tables (reverse operation)
const UNDO_MIGRATION_SQL = `
DROP TABLE IF EXISTS bookmark_tags CASCADE;
DROP TABLE IF EXISTS tags CASCADE;
DROP TABLE IF EXISTS bookmarks CASCADE;
DROP TABLE IF EXISTS users CASCADE;
`;

/**
 * Main migration function
 * Connects to database and runs migrations
 */
async function runMigrations() {
  // Determine if this is an undo operation
  const isUndo = process.argv.includes('--undo');
  
  const pool = new Pool({
    connectionString: getDbConnectionString(),
  });
  
  try {
    console.log(`Running ${isUndo ? 'undo' : 'forward'} migrations...`);
    
    const client = await pool.connect();
    
    try {
      // Execute the appropriate migration
      const sql = isUndo ? UNDO_MIGRATION_SQL : MIGRATION_SQL;
      await client.query(sql);
      
      console.log(`✓ Migration ${isUndo ? 'undone' : 'completed'} successfully`);
      
      // Verify tables were created (if running forward)
      if (!isUndo) {
        const result = await client.query(`
          SELECT table_name 
          FROM information_schema.tables 
          WHERE table_schema = 'public'
        `);
        
        console.log('\nTables created:');
        result.rows.forEach(row => {
          console.log(`  - ${row.table_name}`);
        });
      }
      
    } finally {
      // Always release the client back to the pool
      client.release();
    }
    
  } catch (error) {
    // Handle specific database errors
    if (error.code === '28P01') {
      // Authentication failed - wrong username/password
      console.error('✗ Database authentication failed');
      console.error('  Check your DB_USER and DB_PASSWORD in .env');
    } else if (error.code === '3D000') {
      // Database doesn't exist
      console.error('✗ Database does not exist');
      console.error('  Create it with: createdb nodemark');
    } else if (error.code === 'ENOTFOUND') {
      // Can't reach the database host
      console.error('✗ Cannot connect to database host');
      console.error('  Check your DB_HOST in .env');
    } else {
      console.error('✗ Migration failed:', error.message);
    }
    
    // Exit with error code
    process.exit(1);
    
  } finally {
    // Always close the pool
    await pool.end();
  }
}

// Run the migrations
runMigrations();
```

## How to Run Migrations

```bash
# Run migrations (create tables)
npm run migrate

# Undo migrations (drop tables)
npm run migrate:undo
```

## How It Works

1. **Load config** - Gets database credentials from environment
2. **Connect to PostgreSQL** - Creates a pg Pool connection
3. **Execute SQL** - Runs the migration SQL statements
4. **Verify** - Queries information_schema to list created tables
5. **Handle errors** - Provides helpful messages for common failures

This uses the same patterns as:
- [02-core-modules/fs-module/01-reading-files.md](../../../02-core-modules/fs-module/01-reading-files.md) - Reading files with fs/promises
- [06-databases/postgres/01-pg-setup.md](../../../06-databases/postgres/01-pg-setup.md) - Using pg Pool

## Common Mistakes

- Running migrations without a database
- Wrong credentials in .env
- Trying to run migrations while the app is using the database
- Not handling connection errors gracefully

## Try It Yourself

### Exercise 1: Run Migrations
Run `npm run migrate` and verify tables are created.

### Exercise 2: Test Undo
Run `npm run migrate:undo` and verify tables are dropped.

### Exercise 3: Handle Errors
Add handling for another common PostgreSQL error code.

## Next Steps

Continue to [03-db-module.md](./03-db-module.md) to create the centralized database module.
