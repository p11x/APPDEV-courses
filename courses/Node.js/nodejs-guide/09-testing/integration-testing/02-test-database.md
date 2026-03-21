# Test Database

## What You'll Learn

- Using in-memory databases for testing
- Isolating test data

## In-Memory SQLite for Testing

```javascript
// test-db.js - Using in-memory database

import Database from 'better-sqlite3';

const db = new Database(':memory:'); // In-memory database

// Create tables
db.exec(`
  CREATE TABLE users (
    id INTEGER PRIMARY KEY,
    name TEXT
  )
`);

// Use for tests
export default db;
```

## Setting Up Test Database

```javascript
// setup.js - Test database setup

import Database from 'better-sqlite3';

let testDb = null;

export function createTestDb() {
  testDb = new Database(':memory:');
  testDb.exec(`
    CREATE TABLE users (
      id INTEGER PRIMARY KEY,
      name TEXT
    )
  `);
  return testDb;
}

export function getTestDb() {
  return testDb;
}

export function closeTestDb() {
  if (testDb) {
    testDb.close();
    testDb = null;
  }
}
```

## Code Example

```javascript
// test-db-example.js - Complete example

import Database from 'better-sqlite3';

describe('Database Tests', () => {
  let db;
  
  beforeEach(() => {
    // Create fresh database for each test
    db = new Database(':memory:');
    db.exec('CREATE TABLE users (id INTEGER PRIMARY KEY, name TEXT)');
  });
  
  afterEach(() => {
    db.close();
  });
  
  test('insert and select', () => {
    db.prepare('INSERT INTO users (name) VALUES (?)').run('Alice');
    const user = db.prepare('SELECT * FROM users').get();
    
    expect(user.name).toBe('Alice');
  });
});
```

## Try It Yourself

### Exercise 1: Create Test Database
Create an in-memory database for testing.

### Exercise 2: Isolate Tests
Ensure each test has isolated data.
