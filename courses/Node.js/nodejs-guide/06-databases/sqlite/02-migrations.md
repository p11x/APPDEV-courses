# Database Migrations

## What You'll Learn

- What database migrations are
- Creating migration files
- Running migrations

## What are Migrations?

Migrations are version-controlled database schema changes. They help you:
- Track schema changes over time
- Apply changes consistently across environments
- Rollback if needed

## Simple Migration System

```javascript
// migrate.js - Simple migration runner

import Database from 'better-sqlite3';

const db = new Database('app.db');

// Migration table
db.exec(`
  CREATE TABLE IF NOT EXISTS migrations (
    id INTEGER PRIMARY KEY,
    name TEXT,
    applied_at DATETIME DEFAULT CURRENT_TIMESTAMP
  )
`);

const migrations = [
  {
    name: '001_create_users',
    sql: `
      CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        email TEXT UNIQUE
      );
    `
  },
  {
    name: '002_create_posts',
    sql: `
      CREATE TABLE IF NOT EXISTS posts (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        user_id INTEGER,
        title TEXT,
        body TEXT,
        FOREIGN KEY (user_id) REFERENCES users(id)
      );
    `
  }
];

// Run migrations
const applied = db.prepare('SELECT name FROM migrations').all();

for (const migration of migrations) {
  if (!applied.find(m => m.name === migration.name)) {
    console.log(`Applying: ${migration.name}`);
    db.exec(migration.sql);
    db.prepare('INSERT INTO migrations (name) VALUES (?)').run(migration.name);
  }
}

console.log('Migrations complete!');
db.close();
```

## Try It Yourself

### Exercise 1: Create Migrations
Create migration files for users and posts tables.

### Exercise 2: Run Migrations
Implement a migration runner.
