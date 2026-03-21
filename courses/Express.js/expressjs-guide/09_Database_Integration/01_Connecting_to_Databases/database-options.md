# Database Integration Overview

## 📌 What You'll Learn
- Different types of databases for Express applications
- When to use each type of database
- How to connect to databases in Express
- Best practices for database integration

## 🧠 Concept Explained (Plain English)

A database is like a highly organized digital filing cabinet. Instead of keeping all your data in memory (which disappears when the server restarts), databases store data permanently so you can retrieve it later.

Express applications can work with many types of databases:

**SQL Databases (Relational)**
- Examples: PostgreSQL, MySQL, SQLite
- Structure: Tables with rows and columns
- Best for: Structured data with relationships (users, orders, products)
- Think of it like Excel spreadsheets that can connect to each other

**NoSQL Databases**
- Examples: MongoDB, Redis, Cassandra
- Structure: Flexible documents or key-value pairs
- Best for: Flexible schemas, rapid development, large amounts of unstructured data
- Think of it like JSON files that can be nested

**In-Memory Databases**
- Examples: Redis (can be used this way)
- Structure: Key-value pairs stored in RAM
- Best for: Caching, sessions, real-time data
- Think of it like short-term memory - fast but doesn't persist

Choosing the right database depends on your data structure, scalability needs, and development speed requirements.

## 💻 Code Example

```javascript
// ES Module - Connecting to Different Databases

import express from 'express';

const app = express();

app.use(express.json());

/*
// ========================================
// MONGODB CONNECTION EXAMPLE (using Mongoose)
// ========================================

import mongoose from 'mongoose';

// Why use process.env for database URLs?
// - Security: Don't hardcode credentials in source code
// - Environment-specific: Different databases for dev vs production
// - Flexibility: Easy to change without code changes

const MONGODB_URI = process.env.MONGODB_URI || 'mongodb://localhost:27017/myapp';

mongoose.connect(MONGODB_URI)
    .then(() => console.log('Connected to MongoDB'))
    .catch(err => console.error('MongoDB connection error:', err));

// Define a schema
const userSchema = new mongoose.Schema({
    username: String,
    email: String,
    createdAt: { type: Date, default: Date.now }
});

const User = mongoose.model('User', userSchema);

// Use in routes
app.get('/users', async (req, res) => {
    const users = await User.find();
    res.json(users);
});

app.post('/users', async (req, res) => {
    const user = new User(req.body);
    await user.save();
    res.status(201).json(user);
});
*/

/*
// ========================================
// POSTGRESQL CONNECTION EXAMPLE (using pg)
// ========================================

import pg from 'pg';

const { Pool } = pg;

// Database config from environment
const pool = new Pool({
    user: process.env.DB_USER || 'postgres',
    password: process.env.DB_PASSWORD || 'password',
    host: process.env.DB_HOST || 'localhost',
    port: process.env.DB_PORT || 5432,
    database: process.env.DB_NAME || 'myapp'
});

pool.on('connect', () => {
    console.log('Connected to PostgreSQL database');
});

// Use in routes
app.get('/users', async (req, res) => {
    const result = await pool.query('SELECT * FROM users');
    res.json(result.rows);
});

app.post('/users', async (req, res) => {
    const { username, email } = req.body;
    const result = await pool.query(
        'INSERT INTO users (username, email) VALUES ($1, $2) RETURNING *',
        [username, email]
    );
    res.status(201).json(result.rows[0]);
});
*/

/*
// ========================================
// MYSQL CONNECTION EXAMPLE
// ========================================

import mysql from 'mysql2/promise';

const pool = mysql.createPool({
    host: process.env.DB_HOST || 'localhost',
    user: process.env.DB_USER || 'root',
    password: process.env.DB_PASSWORD || 'password',
    database: process.env.DB_NAME || 'myapp',
    waitForConnections: true,
    connectionLimit: 10
});

app.get('/users', async (req, res) => {
    const [rows] = await pool.execute('SELECT * FROM users');
    res.json(rows);
});
*/

/*
// ========================================
// SQLITE CONNECTION EXAMPLE (for development)
// ========================================

import sqlite3 from 'sqlite3';

const db = new sqlite3.Database('./myapp.db', (err) => {
    if (err) console.error('SQLite connection error:', err);
    else console.log('Connected to SQLite database');
});

// Create table if not exists
db.serialize(() => {
    db.run(`
        CREATE TABLE IF NOT EXISTS users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            username TEXT,
            email TEXT
        )
    `);
});

app.get('/users', (req, res) => {
    db.all('SELECT * FROM users', [], (err, rows) => {
        if (err) return res.status(500).json({ error: err.message });
        res.json(rows);
    });
});

app.post('/users', (req, res) => {
    const { username, email } = req.body;
    db.run(
        'INSERT INTO users (username, email) VALUES (?, ?)',
        [username, email],
        function(err) {
            if (err) return res.status(500).json({ error: err.message });
            res.status(201).json({ id: this.lastID, username, email });
        }
    );
});
*/

// ========================================
// USING AN IN-MEMORY "DATABASE" FOR DEMO
// ========================================

// Simple in-memory store (not persistent, but works for demo)
const users = [
    { id: 1, username: 'alice', email: 'alice@example.com' },
    { id: 2, username: 'bob', email: 'bob@example.com' }
];

// GET all users
app.get('/users', (req, res) => {
    res.json(users);
});

// GET single user
app.get('/users/:id', (req, res) => {
    const user = users.find(u => u.id === parseInt(req.params.id));
    if (!user) {
        return res.status(404).json({ error: 'User not found' });
    }
    res.json(user);
});

// POST new user
app.post('/users', (req, res) => {
    const { username, email } = req.body;
    
    if (!username || !email) {
        return res.status(400).json({ error: 'Username and email required' });
    }
    
    const newUser = {
        id: users.length + 1,
        username,
        email
    };
    
    users.push(newUser);
    res.status(201).json(newUser);
});

// PUT update user
app.put('/users/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === id);
    
    if (userIndex === -1) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    const { username, email } = req.body;
    users[userIndex] = {
        id,
        username: username || users[userIndex].username,
        email: email || users[userIndex].email
    };
    
    res.json(users[userIndex]);
});

// DELETE user
app.delete('/users/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === id);
    
    if (userIndex === -1) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    users.splice(userIndex, 1);
    res.status(204).send();
});

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Key Database Concepts

| Type | Best For | Examples |
|------|----------|----------|
| SQL | Structured data, complex queries | PostgreSQL, MySQL |
| NoSQL | Flexible schemas, scalability | MongoDB, Redis |
| In-memory | Caching, sessions | Redis, Memcached |
| Embedded | Development, small apps | SQLite |

## ⚠️ Common Mistakes

**1. Not using connection pooling**
Database connections take time to establish. Use connection pools to reuse connections.

**2. Hardcoding database credentials**
Never put passwords or connection strings in your code. Use environment variables!

**3. Not handling connection errors**
Always handle database connection errors to prevent crashes.

**4. Making blocking database calls**
Use async/await or Promises to avoid blocking the Node.js event loop.

**5. Not closing connections**
Make sure to handle connection cleanup, especially in serverless environments.

## ✅ Quick Recap

- SQL databases (PostgreSQL, MySQL) are great for structured, relational data
- NoSQL databases (MongoDB) are flexible and scale well
- Use environment variables for database credentials
- Always handle connection errors
- Use connection pooling for better performance
- Close database connections when shutting down

## 🔗 What's Next

Let's look at how to use an ORM (Object-Relational Mapper) to work with databases more easily in Express.
