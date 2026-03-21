# PostgreSQL with node-postgres

## What You'll Learn

- Installing pg
- Connecting to PostgreSQL
- Running queries

## Installing pg

```bash
npm install pg
```

## Connecting

```javascript
// db.js - PostgreSQL connection

import pg from 'pg';

const { Pool } = pg;

const pool = new Pool({
  connectionString: 'postgresql://user:password@localhost:5432/mydb'
});

export default pool;
```

## Running Queries

```javascript
// Query
const result = await pool.query('SELECT * FROM users');
console.log(result.rows);

// Parameterized query
const result = await pool.query(
  'SELECT * FROM users WHERE id = $1',
  [1]
);
```

## Code Example

```javascript
// example.js - Complete PostgreSQL example

import pg from 'pg';
const { Pool } = pg;

const pool = new Pool({
  connectionString: process.env.DATABASE_URL
});

// Create table
await pool.query(`
  CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    email TEXT UNIQUE
  )
`);

// Insert
const insert = await pool.query(
  'INSERT INTO users (name, email) VALUES ($1, $2) RETURNING *',
  ['Alice', 'alice@example.com']
);
console.log('Inserted:', insert.rows[0]);

// Select
const select = await pool.query('SELECT * FROM users');
console.log('Users:', select.rows);

// Update
await pool.query(
  'UPDATE users SET name = $1 WHERE id = $2',
  ['Alice Smith', insert.rows[0].id]
);

// Delete
await pool.query('DELETE FROM users WHERE id = $1', [insert.rows[0].id]);

await pool.end();
```

## Try It Yourself

### Exercise 1: Connect to PostgreSQL
Connect to a PostgreSQL database.

### Exercise 2: Run Queries
Run CRUD operations with PostgreSQL.
