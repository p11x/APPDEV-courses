# Parameterized Queries and SQL Injection

## What You'll Learn

- Preventing SQL injection
- Using parameterized queries
- Transactions

## SQL Injection Prevention

**NEVER** use string concatenation for queries:

```javascript
// WRONG - vulnerable to SQL injection!
const query = 'SELECT * FROM users WHERE name = "' + name + '"';
```

**ALWAYS** use parameterized queries:

```javascript
// CORRECT - safe from SQL injection
const result = await pool.query(
  'SELECT * FROM users WHERE name = $1',
  [name]  // Parameters are escaped automatically
);
```

## Parameterized Queries

```javascript
// Single parameter
await pool.query(
  'SELECT * FROM users WHERE id = $1',
  [userId]
);

// Multiple parameters
await pool.query(
  'INSERT INTO users (name, email) VALUES ($1, $2)',
  ['Alice', 'alice@example.com']
);

// Array parameters
const ids = [1, 2, 3];
await pool.query(
  'SELECT * FROM users WHERE id = ANY($1)',
  [ids]
);
```

## Transactions

```javascript
// Using transactions
const client = await pool.connect();

try {
  await client.query('BEGIN');
  
  await client.query(
    'INSERT INTO accounts (name, balance) VALUES ($1, $2)',
    ['Alice', 1000]
  );
  
  await client.query('COMMIT');
} catch (e) {
  await client.query('ROLLBACK');
  throw e;
} finally {
  client.release();
}
```

## Code Example

```javascript
// safe-queries.js - Safe SQL examples

import pg from 'pg';
const { Pool } = pg;

const pool = new Pool({ connectionString: process.env.DATABASE_URL });

// Safe insert with parameters
async function createUser(name, email) {
  const result = await pool.query(
    'INSERT INTO users (name, email) VALUES ($1, $2) RETURNING *',
    [name, email]
  );
  return result.rows[0];
}

// Safe select
async function getUser(id) {
  const result = await pool.query(
    'SELECT * FROM users WHERE id = $1',
    [id]
  );
  return result.rows[0];
}

// Safe search
async function searchUsers(searchTerm) {
  const result = await pool.query(
    'SELECT * FROM users WHERE name ILIKE $1',
    [`%${searchTerm}%`]
  );
  return result.rows;
}

// Transaction example
async function transferMoney(fromId, toId, amount) {
  const client = await pool.connect();
  
  try {
    await client.query('BEGIN');
    
    await client.query(
      'UPDATE accounts SET balance = balance - $1 WHERE id = $2',
      [amount, fromId]
    );
    
    await client.query(
      'UPDATE accounts SET balance = balance + $1 WHERE id = $2',
      [amount, toId]
    );
    
    await client.query('COMMIT');
  } catch (error) {
    await client.query('ROLLBACK');
    throw error;
  } finally {
    client.release();
  }
}

export { createUser, getUser, searchUsers, transferMoney };
```

## Try It Yourself

### Exercise 1: Parameterized Queries
Rewrite unsafe queries to use parameters.

### Exercise 2: Transactions
Implement a money transfer with transactions.
