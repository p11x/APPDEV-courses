# Building REST APIs with Express.js

## What is a REST API?

**REST** (Representational State Transfer) is an architectural style for building APIs. It uses HTTP methods to perform operations on resources.

REST APIs are the standard for modern web services — they use JSON to exchange data between clients and servers.

## RESTful Conventions

### HTTP Methods

| Method | CRUD Operation | Description |
|--------|-----------------|-------------|
| **GET** | Read | Retrieve data |
| **POST** | Create | Create new resource |
| **PUT** | Update | Replace entire resource |
| **PATCH** | Update | Partially update resource |
| **DELETE** | Delete | Remove resource |

### URL Structure

| URL | Method | Description |
|-----|--------|-------------|
| `/users` | GET | Get all users |
| `/users` | POST | Create new user |
| `/users/:id` | GET | Get single user |
| `/users/:id` | PUT | Update user |
| `/users/:id` | DELETE | Delete user |

## Building a REST API

### Complete Example

```javascript
// server.js
import express from 'express';

const app = express();
// 'app' is our Express application instance

// Parse JSON bodies
app.use(express.json());

// Mock database (in-memory)
let users = [
    { id: 1, name: 'Alice', email: 'alice@example.com' },
    { id: 2, name: 'Bob', email: 'bob@example.com' }
];

let nextId = 3;

// ============================================
// REST API Routes Table
// ============================================
// | Method | URL           | Handler        | Description           |
// |--------|---------------|----------------|-----------------------|
// | GET    | /api/users    | getAllUsers   | Get all users        |
// | GET    | /api/users/:id| getUserById   | Get single user      |
// | POST   | /api/users    | createUser    | Create new user      |
// | PUT    | /api/users/:id| updateUser    | Update user          |
// | PATCH  | /api/users/:id| patchUser     | Partial update       |
// | DELETE | /api/users/:id| deleteUser    | Delete user          |
// ============================================

// GET /api/users - Get all users
const getAllUsers = async (req, res) => {
    // async/await lets us handle async operations cleanly
    // In a real app, this would fetch from a database
    res.json({
        success: true,
        count: users.length,
        data: users
    });
};

// GET /api/users/:id - Get single user
const getUserById = async (req, res) => {
    // req.params.id captures the :id from the URL
    const id = parseInt(req.params.id);
    
    const user = users.find(u => u.id === id);
    
    if (!user) {
        return res.status(404).json({
            success: false,
            message: 'User not found'
        });
    }
    
    res.json({
        success: true,
        data: user
    });
};

// POST /api/users - Create new user
const createUser = async (req, res) => {
    // req.body contains the JSON data sent by client
    const { name, email } = req.body;
    
    // Validation
    if (!name || !email) {
        return res.status(400).json({
            success: false,
            message: 'Please provide name and email'
        });
    }
    
    // Create new user
    const newUser = {
        id: nextId++,
        name,
        email
    };
    
    users.push(newUser);
    
    // 201 = Created
    res.status(201).json({
        success: true,
        message: 'User created',
        data: newUser
    });
};

// PUT /api/users/:id - Update user (full replacement)
const updateUser = async (req, res) => {
    const id = parseInt(req.params.id);
    const index = users.findIndex(u => u.id === id);
    
    if (index === -1) {
        return res.status(404).json({
            success: false,
            message: 'User not found'
        });
    }
    
    // Full update - replace entire user object
    users[index] = {
        id,
        ...req.body
    };
    
    res.json({
        success: true,
        message: 'User updated',
        data: users[index]
    });
};

// PATCH /api/users/:id - Partial update
const patchUser = async (req, res) => {
    const id = parseInt(req.params.id);
    const index = users.findIndex(u => u.id === id);
    
    if (index === -1) {
        return res.status(404).json({
            success: false,
            message: 'User not found'
        });
    }
    
    // Partial update - merge with existing data
    users[index] = {
        ...users[index],
        ...req.body
    };
    
    res.json({
        success: true,
        message: 'User partially updated',
        data: users[index]
    });
};

// DELETE /api/users/:id - Delete user
const deleteUser = async (req, res) => {
    const id = parseInt(req.params.id);
    const index = users.findIndex(u => u.id === id);
    
    if (index === -1) {
        return res.status(404).json({
            success: false,
            message: 'User not found'
        });
    }
    
    // Remove from array
    users.splice(index, 1);
    
    // 204 = No Content (successful, nothing to return)
    res.status(204).send();
};

// Register routes
app.get('/api/users', getAllUsers);
app.get('/api/users/:id', getUserById);
app.post('/api/users', createUser);
app.put('/api/users/:id', updateUser);
app.patch('/api/users/:id', patchUser);
app.delete('/api/users/:id', deleteUser);

// 404 handler
app.use((req, res) => {
    res.status(404).json({
        success: false,
        message: 'Endpoint not found'
    });
});

// Error handler
app.use((err, req, res, next) => {
    console.error(err.message);
    res.status(500).json({
        success: false,
        message: 'Server error'
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`API running on port ${PORT}`));
```

## Testing Your API

### Using curl

```bash
# Get all users
curl http://localhost:3000/api/users

# Get user by ID
curl http://localhost:3000/api/users/1

# Create user
curl -X POST http://localhost:3000/api/users \
  -H "Content-Type: application/json" \
  -d '{"name": "Charlie", "email": "charlie@example.com"}'

# Update user
curl -X PUT http://localhost:3000/api/users/1 \
  -H "Content-Type: application/json" \
  -d '{"name": "Alice Smith", "email": "alice.smith@example.com"}'

# Partial update
curl -X PATCH http://localhost:3000/api/users/1 \
  -H "Content-Type: application/json" \
  -d '{"email": "newemail@example.com"}'

# Delete user
curl -X DELETE http://localhost:3000/api/users/1
```

## REST API Best Practices

### Response Format

Always return consistent JSON structure:

```javascript
// Success response
res.json({
    success: true,
    message: 'Optional message',
    data: { /* resource */ },
    pagination: { /* if applicable */ }
});

// Error response
res.status(400).json({
    success: false,
    message: 'What went wrong',
    errors: [] // Validation errors
});
```

### Status Codes

| Code | Use For |
|------|---------|
| 200 | OK (GET, PUT, PATCH) |
| 201 | Created (POST) |
| 204 | No Content (DELETE) |
| 400 | Bad Request (invalid data) |
| 401 | Unauthorized (not logged in) |
| 404 | Not Found (resource doesn't exist) |
| 500 | Server Error |

### Versioning

Include API version in the URL:

```javascript
app.get('/api/v1/users', ...);  // Version 1
app.get('/api/v2/users', ...);  // Version 2
```

### Pagination

For large datasets, implement pagination:

```javascript
app.get('/api/users', (req, res) => {
    const page = parseInt(req.query.page) || 1;
    const limit = parseInt(req.query.limit) || 10;
    
    const startIndex = (page - 1) * limit;
    const endIndex = page * limit;
    
    const results = {
        success: true,
        page,
        limit,
        total: users.length,
        data: users.slice(startIndex, endIndex)
    };
    
    res.json(results);
});
```

## What's Next?

- **[Database Integration](../06_Database_Integration/01_mongodb.md)** — Connect to MongoDB
- **[Authentication](../08_Security/01_authentication.md)** — Secure your API
