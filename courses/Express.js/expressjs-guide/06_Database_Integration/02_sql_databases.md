# SQL Databases with Express.js

## What are SQL Databases?

**SQL databases** (like PostgreSQL and MySQL) store data in structured tables with rows and columns. They use **Structured Query Language (SQL)** to interact with data.

Unlike MongoDB's flexible documents, SQL databases require a predefined schema — perfect when you need strict data structure and relationships.

## PostgreSQL with Node.js

**PostgreSQL** is a powerful, open-source relational database.

### Step 1: Install Dependencies

```bash
npm install pg dotenv
```

### Step 2: Database Configuration

```javascript
// config/database.js
import pg from 'pg';

// Pool manages multiple database connections efficiently
// A pool creates a group of connections that can be reused
const pool = new pg.Pool({
    // process.env stores database credentials
    // This keeps sensitive info out of your code
    user: process.env.DB_USER || 'postgres',
    host: process.env.DB_HOST || 'localhost',
    database: process.env.DB_NAME || 'myapp',
    password: process.env.DB_PASSWORD || 'password',
    port: process.env.DB_PORT || 5432,
});

// Test the connection
pool.on('connect', () => {
    console.log('✅ Connected to PostgreSQL');
});

// Export for use in other files
export default pool;
```

### Step 3: Create Tables

```javascript
// Run this once to set up your database
import pool from '../config/database.js';

const createTables = async () => {
    const query = `
        CREATE TABLE IF NOT EXISTS users (
            id SERIAL PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            email VARCHAR(100) UNIQUE NOT NULL,
            password VARCHAR(255) NOT NULL,
            role VARCHAR(20) DEFAULT 'user',
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
        
        CREATE TABLE IF NOT EXISTS posts (
            id SERIAL PRIMARY KEY,
            user_id INTEGER REFERENCES users(id),
            title VARCHAR(200) NOT NULL,
            content TEXT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        );
    `;
    
    try {
        await pool.query(query);
        console.log('✅ Tables created');
    } catch (error) {
        console.error('Error creating tables:', error);
    }
};

createTables();
```

### Step 4: CRUD Operations

```javascript
// controllers/userController.js
import pool from '../config/database.js';

// ============================================
// User CRUD Operations Table
// ============================================
// | Method | Function         | SQL Operation         |
// |--------|------------------|----------------------|
// | GET    | getAllUsers     | SELECT * FROM users |
// | GET    | getUserById     | SELECT * FROM users |
// | POST   | createUser      | INSERT INTO users   |
// | PUT    | updateUser      | UPDATE users        |
// | DELETE | deleteUser      | DELETE FROM users   |
// ============================================

// GET /users - Get all users
export const getAllUsers = async (req, res) => {
    try {
        // pool.query() executes SQL queries
        // await pauses until the query completes
        const result = await pool.query(
            'SELECT id, name, email, role, created_at FROM users ORDER BY created_at DESC'
        );
        
        res.json({
            success: true,
            count: result.rows.length,
            data: result.rows
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            message: error.message
        });
    }
};

// GET /users/:id - Get single user
export const getUserById = async (req, res) => {
    try {
        const { id } = req.params;
        
        const result = await pool.query(
            'SELECT id, name, email, role, created_at FROM users WHERE id = $1',
            [id]  // $1 is a parameter placeholder - prevents SQL injection!
        );
        
        if (result.rows.length === 0) {
            return res.status(404).json({
                success: false,
                message: 'User not found'
            });
        }
        
        res.json({
            success: true,
            data: result.rows[0]
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            message: error.message
        });
    }
};

// POST /users - Create new user
export const createUser = async (req, res) => {
    try {
        const { name, email, password, role } = req.body;
        
        // $1, $2, $3 are parameter placeholders
        // This prevents SQL injection attacks!
        const result = await pool.query(
            'INSERT INTO users (name, email, password, role) VALUES ($1, $2, $3, $4) RETURNING *',
            [name, email, password, role || 'user']
        );
        
        res.status(201).json({
            success: true,
            data: result.rows[0]
        });
    } catch (error) {
        // Handle unique constraint violation
        if (error.code === '23505') { // PostgreSQL unique violation code
            return res.status(400).json({
                success: false,
                message: 'Email already exists'
            });
        }
        
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// PUT /users/:id - Update user
export const updateUser = async (req, res) => {
    try {
        const { id } = req.params;
        const { name, email, role } = req.body;
        
        const result = await pool.query(
            `UPDATE users 
             SET name = COALESCE($1, name), 
                 email = COALESCE($2, email), 
                 role = COALESCE($3, role)
             WHERE id = $4 
             RETURNING id, name, email, role`,
            [name, email, role, id]
        );
        
        if (result.rows.length === 0) {
            return res.status(404).json({
                success: false,
                message: 'User not found'
            });
        }
        
        res.json({
            success: true,
            data: result.rows[0]
        });
    } catch (error) {
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// DELETE /users/:id - Delete user
export const deleteUser = async (req, res) => {
    try {
        const { id } = req.params;
        
        const result = await pool.query(
            'DELETE FROM users WHERE id = $1 RETURNING id',
            [id]
        );
        
        if (result.rows.length === 0) {
            return res.status(404).json({
                success: false,
                message: 'User not found'
            });
        }
        
        res.json({
            success: true,
            message: 'User deleted'
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            message: error.message
        });
    }
};
```

## MySQL with Express

### Install

```bash
npm install mysql2 dotenv
```

### Configuration

```javascript
// config/mysql.js
import mysql from 'mysql2/promise';

const pool = mysql.createPool({
    host: process.env.MYSQL_HOST || 'localhost',
    user: process.env.MYSQL_USER || 'root',
    password: process.env.MYSQL_PASSWORD || 'password',
    database: process.env.MYSQL_DATABASE || 'myapp',
    waitForConnections: true,
    connectionLimit: 10,
    queueLimit: 0
});

export default pool;
```

### Using MySQL

```javascript
// Similar to PostgreSQL but with slightly different syntax
export const getUsers = async (req, res) => {
    try {
        const [rows] = await pool.query('SELECT * FROM users');
        res.json({ data: rows });
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
};
```

## SQL vs MongoDB Comparison

| Feature | SQL (PostgreSQL/MySQL) | NoSQL (MongoDB) |
|---------|------------------------|-----------------|
| **Schema** | Fixed | Flexible |
| **Relationships** | JOINs | Embed/Link |
| **Scaling** | Vertical | Horizontal |
| **Best For** | Structured data | Rapid prototyping |
| **Query Language** | SQL | JavaScript-like |

## Best Practices

| Practice | Why |
|----------|-----|
| Use parameterized queries | Prevents SQL injection |
| Use connection pooling | Better performance |
| Close connections | Prevent resource leaks |
| Validate input | Security |
| Use transactions | Data consistency |

## Environment Variables

Create a `.env` file:

```
# PostgreSQL
DB_USER=postgres
DB_HOST=localhost
DB_NAME=myapp
DB_PASSWORD=secret
DB_PORT=5432

# Or MySQL
MYSQL_HOST=localhost
MYSQL_USER=root
MYSQL_PASSWORD=secret
MYSQL_DATABASE=myapp

# App
PORT=3000
```

## What's Next?

- **[Authentication](../08_Security/01_authentication.md)** — Secure your API
- **[Error Handling](../07_Error_Handling/01_basics.md)** — Handle errors gracefully
