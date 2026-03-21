# Database Connection Module

## What You'll Build In This File

A centralized PostgreSQL connection module using pg Pool that can be imported throughout the application.

## Complete Database Module

Create `src/db/index.js`:

```javascript
// src/db/index.js - Centralized PostgreSQL connection pool
// This module creates and exports a single shared Pool instance
// All database operations should use this pool

import pg from 'pg';
import { config, getDbConnectionString } from '../config/index.js';

// Destructure Pool from pg
const { Pool } = pg;

/**
 * Create the database pool with configuration from config module
 * The pool manages multiple connections efficiently
 */
export const pool = new Pool({
  // Use connection string from config
  connectionString: getDbConnectionString(),
  
  // Pool configuration
  max: config.db.max,                      // Maximum 20 concurrent connections
  idleTimeoutMillis: config.db.idleTimeoutMillis,  // Close idle connections after 30s
  connectionTimeoutMillis: config.db.connectionTimeoutMillis,  // Fail fast if can't connect
});

/**
 * Test the database connection on startup
 * Throws an error if we can't connect - fail fast rather than hang
 */
export async function testConnection() {
  const client = await pool.connect();
  
  try {
    // Simple query to verify connection works
    const result = await client.query('SELECT NOW() as now, version() as pg_version');
    console.log('✓ Database connected');
    console.log(`  PostgreSQL: ${result.rows[0].pg_version}`);
    console.log(`  Server time: ${result.rows[0].now}`);
  } finally {
    // Always release the client back to the pool
    client.release();
  }
}

/**
 * Graceful shutdown - close all pool connections
 * Call this when the application shuts down
 */
export async function closePool() {
  await pool.end();
  console.log('✓ Database pool closed');
}

// Handle pool errors - log but don't crash immediately
pool.on('error', (err) => {
  // Unexpected error on idle client
  console.error('✗ Database pool error:', err.message);
  // In production, you might want to restart the pool or notify monitoring
});

/**
 * Query helper function - wraps pool.query with additional logging
 * @param {string} text - Parameterized SQL query
 * @param {Array} params - Parameters for the query
 * @returns {Promise<pg.QueryResult>} - Query result
 */
export async function query(text, params) {
  // In development, log queries for debugging
  if (config.nodeEnv === 'development') {
    console.log('SQL:', text, params || []);
  }
  
  const start = Date.now();
  
  try {
    const result = await pool.query(text, params);
    
    const duration = Date.now() - start;
    
    // Log slow queries (> 100ms) in development
    if (duration > 100 && config.nodeEnv === 'development') {
      console.log(`  ⚠ Slow query (${duration}ms): ${text.substring(0, 50)}...`);
    }
    
    return result;
  } catch (error) {
    // Add context to database errors
    error.message = `Database error: ${error.message}`;
    throw error;
  }
}

// Export pool directly for advanced use cases
// Most of the time, use the query() helper above instead
export default {
  query,
  pool,
  testConnection,
  closePool,
};
```

## Using the Database Module

```javascript
// Example: Using the db module in a route handler
import { query } from '../db/index.js';

// Get all bookmarks for a user
export async function getBookmarksByUser(userId, options = {}) {
  const { limit = 20, offset = 0, tagId } = options;
  
  // Parameterized query - prevents SQL injection
  // $1, $2, etc. are placeholders for parameters
  let sql = `
    SELECT b.*, 
           COALESCE(json_agg(t) FILTER (WHERE t.id IS NOT NULL), '[]') as tags
    FROM bookmarks b
    LEFT JOIN bookmark_tags bt ON b.id = bt.bookmark_id
    LEFT JOIN tags t ON bt.tag_id = t.id
    WHERE b.user_id = $1
  `;
  
  const params = [userId];
  
  // Add optional tag filter
  if (tagId) {
    sql += ` AND t.id = $2`;
    params.push(tagId);
  }
  
  sql += `
    GROUP BY b.id
    ORDER BY b.created_at DESC
    LIMIT $${params.length + 1} OFFSET $${params.length + 2}
  `;
  
  params.push(limit, offset);
  
  const result = await query(sql, params);
  return result.rows;
}
```

## How It Connects

This module connects to concepts from:
- [06-databases/postgres/01-pg-setup.md](../../../06-databases/postgres/01-pg-setup.md) - Using pg Pool
- [06-databases/postgres/02-parameterized.md](../../../06-databases/postgres/02-parameterized.md) - Parameterized queries prevent SQL injection

## Common Mistakes

- Creating a new Pool for each query (wasteful - use the shared pool)
- Not using parameterized queries (SQL injection vulnerability)
- Forgetting to release clients back to the pool
- Not handling pool errors

## Try It Yourself

### Exercise 1: Test Connection
Import and call `testConnection()` in your app startup.

### Exercise 2: Add a Query
Create a function that queries users by email.

### Exercise 3: Add Logging
Add more detailed query logging in development mode.

## Next Steps

Continue to [../../03-auth/01-register-route.md](../../03-auth/01-register-route.md) to build the registration endpoint.
