# Using Better-SQLite3

## What You'll Learn

- Installing better-sqlite3
- Creating and connecting to SQLite database
- Running queries
- Using prepared statements

## Installing Better-SQLite3

```bash
npm install better-sqlite3
```

## Basic Usage

```javascript
// db.js - SQLite database setup

import Database from 'better-sqlite3';

const db = new Database('mydb.sqlite');

// Create table
db.exec(`
  CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT,
    email TEXT
  )
`);

export default db;
```

## CRUD Operations

```javascript
// Create
const insert = db.prepare('INSERT INTO users (name, email) VALUES (?, ?)');
const result = insert.run('Alice', 'alice@example.com');

// Read
const getUser = db.prepare('SELECT * FROM users WHERE id = ?');
const user = getUser.get(1);

// Read all
const allUsers = db.prepare('SELECT * FROM users').all();

// Update
const update = db.prepare('UPDATE users SET name = ? WHERE id = ?');
update.run('Alice Smith', 1);

// Delete
const remove = db.prepare('DELETE FROM users WHERE id = ?');
remove.run(1);
```

## Code Example

```javascript
// example.js - Complete SQLite example

import Database from 'better-sqlite3';

const db = new Database('app.db');

// Create table
db.exec(`
  CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    email TEXT UNIQUE
  )
`);

// Insert
const insert = db.prepare('INSERT INTO users (name, email) VALUES (?, ?)');
const result = insert.run('Alice', 'alice@example.com');
console.log('Inserted:', result.lastInsertRowid);

// Query single
const get = db.prepare('SELECT * FROM users WHERE id = ?');
console.log('User:', get.get(result.lastInsertRowid));

// Query all
console.log('All users:', db.prepare('SELECT * FROM users').all());

db.close();
```

## Try It Yourself

### Exercise 1: Create Database
Create a SQLite database and users table.

### Exercise 2: CRUD Operations
Implement create, read, update, and delete operations.

## Next Steps

Continue to [Database Migrations](./02-migrations.md).
