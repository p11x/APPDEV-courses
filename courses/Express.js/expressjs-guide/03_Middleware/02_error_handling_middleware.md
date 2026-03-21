# Error Handling Middleware in Express.js

## What is Error Handling Middleware?

**Error handling middleware** is special middleware that catches errors from anywhere in your application — route handlers, synchronous code, asynchronous code, or other middleware.

When an error occurs, Express passes it to your error handlers instead of letting the server crash.

## Error Handler Function Signature

Error handlers have **four parameters** (Express knows it's an error handler by the four parameters):

```javascript
// Error handler with four parameters
// err = the error object (what went wrong)
// req = the request object  
// res = the response object
// next = function to pass control (usually not needed in error handlers)
app.use((err, req, res, next) => {
    // Handle the error
});
```

## Basic Error Handling

```javascript
// server.js
import express from 'express';

const app = express();

// Parse JSON
app.use(express.json());

// ============================================
// Route Table
// ============================================
// | Method | Path        | Handler         | Description             |
// |--------|-------------|-----------------|-------------------------|
// | GET    | /success    | successHandler  | Returns success         |
// | GET    | /error      | errorHandler    | Throws an error         |
// | GET    | /async-error| asyncError      | Async error (try/catch)|
// ============================================

// Success route - works normally
app.get('/success', (req, res) => {
    res.json({ message: 'Everything works!' });
});

// Sync error - throws an error
app.get('/error', (req, res) => {
    throw new Error('Something went wrong!');
});

// Async error - error in async function
app.get('/async-error', async (req, res) => {
    // In Express 5, async errors are automatically caught!
    const data = await someAsyncOperation();
    res.json({ data });
});

// Non-existent route - 404 handler
app.use((req, res) => {
    res.status(404).json({ error: 'Route not found' });
});

// ============================================
// Error Handling Middleware
// ============================================

// This middleware catches ALL errors
// Defined with 4 parameters so Express knows it's an error handler
app.use((err, req, res, next) => {
    // Log the error for debugging
    console.error('Error occurred:', err.message);
    
    // Set status code (use 500 if not set)
    const statusCode = err.statusCode || 500;
    
    // Send error response
    res.status(statusCode).json({
        error: err.message || 'Internal Server Error',
        // Include stack trace in development only!
        ...(process.env.NODE_ENV === 'development' && { stack: err.stack })
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));

// Helper function for demo
async function someAsyncOperation() {
    throw new Error('Database connection failed');
}
```

## Express 5 Error Handling

Express 5 makes async error handling much easier. **You don't need try/catch anymore!**

```javascript
// Express 5 - Clean async error handling
import express from 'express';

const app = express();
app.use(express.json());

// In Express 5, errors in async handlers are automatically caught
// and passed to the error handling middleware
app.get('/api/users', async (req, res) => {
    // No try/catch needed in Express 5!
    const users = await database.getUsers(); // This can throw
    res.json(users);
});

// Error handler (same as before)
app.use((err, req, res, next) => {
    console.error(err.message);
    res.status(500).json({ error: 'Something went wrong' });
});
```

## Custom Error Classes

Create your own error types for better error handling:

```javascript
// errors/AppError.js
// Custom error class for application errors

export class AppError extends Error {
    constructor(message, statusCode) {
        super(message);
        this.statusCode = statusCode;
        this.status = statusCode >= 400 && statusCode < 500 ? 'fail' : 'error';
        Error.captureStackTrace(this, this.constructor);
    }
}

// errors/NotFoundError.js
export class NotFoundError extends AppError {
    constructor(resource) {
        super(`${resource} not found`, 404);
    }
}

// errors/ValidationError.js
export class ValidationError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}
```

## Using Custom Errors

```javascript
// server.js
import express from 'express';
import { AppError, NotFoundError, ValidationError } from './errors/AppError.js';

const app = express();
app.use(express.json());

// Route that throws custom errors
app.get('/users/:id', async (req, res) => {
    const userId = parseInt(req.params.id);
    
    if (isNaN(userId)) {
        throw new ValidationError('Invalid user ID');
    }
    
    const user = await findUserById(userId);
    
    if (!user) {
        throw new NotFoundError('User');
    }
    
    res.json(user);
});

// Error handler
app.use((err, req, res, next) => {
    // Known application error?
    if (err instanceof AppError) {
        return res.status(err.statusCode).json({
            error: err.message,
            status: err.status
        });
    }
    
    // Unknown error
    console.error('Unexpected error:', err);
    res.status(500).json({
        error: 'Internal Server Error',
        ...(process.env.NODE_ENV === 'development' && { 
            message: err.message, 
            stack: err.stack 
        })
    });
});
```

## Async Error Handling (Express 4 and earlier)

If using Express 4, wrap async handlers:

```javascript
// Express 4: Wrap async functions to catch errors
const asyncHandler = (fn) => (req, res, next) => {
    Promise.resolve(fn(req, res, next)).catch(next);
};

// Use the wrapper
app.get('/users', asyncHandler(async (req, res) => {
    const users = await database.getUsers();
    res.json(users);
}));

// Alternative: try/catch (also works in Express 5)
app.get('/users', async (req, res, next) => {
    try {
        const users = await database.getUsers();
        res.json(users);
    } catch (error) {
        next(error); // Pass error to error handler
    }
});
```

## 404 Handler

Always have a catch-all for unmatched routes:

```javascript
// Must be placed AFTER all other routes
app.use((req, res) => {
    res.status(404).json({
        error: 'Not Found',
        path: req.path,
        method: req.method
    });
});
```

## Complete Example

```javascript
// server.js
import express from 'express';

const app = express();
app.use(express.json());

// Mock data
let users = [
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' }
];

// ============================================
// Routes
// ============================================

app.get('/users', (req, res) => {
    res.json({ data: users });
});

app.get('/users/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const user = users.find(u => u.id === id);
    
    if (!user) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    res.json({ data: user });
});

app.post('/users', (req, res) => {
    const { name } = req.body;
    
    if (!name) {
        return res.status(400).json({ error: 'Name is required' });
    }
    
    const newUser = { id: users.length + 1, name };
    users.push(newUser);
    res.status(201).json({ data: newUser });
});

// 404 - must be after all routes
app.use((req, res) => {
    res.status(404).json({ error: 'Endpoint not found' });
});

// ============================================
// Error Handler - must be LAST!
// ============================================

app.use((err, req, res, next) => {
    // Log error
    console.error(`[ERROR] ${err.message}`);
    
    // Don't leak stack traces in production
    const response = {
        error: err.message || 'Internal Server Error'
    };
    
    if (process.env.NODE_ENV === 'development') {
        response.stack = err.stack;
    }
    
    res.status(err.statusCode || 500).json(response);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Error Handler Best Practices

| Practice | Why |
|----------|-----|
| Place error handler LAST | It must come after all routes and middleware |
| Don't call next() in error handlers | The request is complete after sending response |
| Log errors | Helps with debugging |
| Don't expose internal errors in production | Security risk! |
| Use custom error classes | Makes error handling consistent |

## What's Next?

- **[Third-Party Middleware](./03_third_party_middleware.md)** — Popular middleware packages
- **[Request & Response](../04_Request_Response/01_request_object.md)** — Working with req and res
