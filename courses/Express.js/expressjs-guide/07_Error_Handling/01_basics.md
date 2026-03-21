# Error Handling in Express.js

## Why Error Handling Matters

**Error handling** is how your application responds when something goes wrong. Without proper error handling:
- Users see confusing messages (or worse, nothing)
- Crashes can expose sensitive information
- It's hard to debug issues

Good error handling makes your app reliable and secure.

## Types of Errors

| Error Type | Cause | Example |
|------------|-------|---------|
| **Operational** | Expected issues | Invalid input, not found |
| **Programming** | Bugs in code | Typo, null reference |
| **External** | System failures | Database down, timeout |

## Basic Error Handling

### Synchronous Errors

Express catches sync (synchronous) errors automatically:

```javascript
// server.js
import express from 'express';

const app = express();

app.get('/error', (req, res) => {
    // This throws an error - Express catches it!
    throw new Error('Something went wrong!');
});

// Error handler (4 parameters!)
// Express knows this is an error handler because of the 4 params
app.use((err, req, res, next) => {
    // err = the error that was thrown
    // req = the request object
    // res = the response object
    // next = function to pass control (usually not used in error handlers)
    
    console.error('Error:', err.message);
    
    res.status(500).json({
        error: 'Internal Server Error',
        message: err.message
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

### Asynchronous Errors

In Express 5, async errors are handled automatically!

```javascript
// Express 5 - No try/catch needed!
app.get('/async-error', async (req, res) => {
    // This async function throws - Express 5 catches it!
    const data = await someDatabaseCall();
    res.json(data);
});
```

In Express 4, use a wrapper:

```javascript
// Express 4 - Async error wrapper
const asyncHandler = (fn) => (req, res, next) => {
    Promise.resolve(fn(req, res, next)).catch(next);
};

app.get('/async-error', asyncHandler(async (req, res) => {
    const data = await someDatabaseCall();
    res.json(data);
}));

// Or use try/catch
app.get('/async-error', async (req, res, next) => {
    try {
        const data = await someDatabaseCall();
        res.json(data);
    } catch (error) {
        next(error); // Pass error to error handler
    }
});
```

## Creating a Complete Error Handling System

### Custom Error Classes

```javascript
// errors/AppError.js
// Base error class for all application errors

export class AppError extends Error {
    constructor(message, statusCode) {
        super(message); // Call parent Error class constructor
        this.statusCode = statusCode;
        
        // Set status based on status code
        // 4xx = 'fail', 5xx = 'error'
        this.status = `${statusCode}`.startsWith('4') ? 'fail' : 'error';
        
        // Mark as operational (expected error)
        this.isOperational = true;
        
        // Capture stack trace (for debugging)
        Error.captureStackTrace(this, this.constructor);
    }
}

// Specific error types
export class NotFoundError extends AppError {
    constructor(resource) {
        super(`${resource} not found`, 404);
    }
}

export class ValidationError extends AppError {
    constructor(message) {
        super(message, 400);
    }
}

export class UnauthorizedError extends AppError {
    constructor(message = 'Unauthorized') {
        super(message, 401);
    }
}

export class ForbiddenError extends AppError {
    constructor(message = 'Forbidden') {
        super(message, 403);
    }
}
```

### Complete Error Handler

```javascript
// middleware/errorHandler.js
import { AppError } from '../errors/AppError.js';

export const errorHandler = (err, req, res, next) => {
    // Default to 500 if no status code
    err.statusCode = err.statusCode || 500;
    err.status = err.status || 'error';
    
    // Log error in development
    if (process.env.NODE_ENV === 'development') {
        res.status(err.statusCode).json({
            status: err.status,
            error: err,
            message: err.message,
            stack: err.stack
        });
    } else {
        // Production - don't leak error details
        if (err.isOperational) {
            // Known/expected error - safe to show message
            res.status(err.statusCode).json({
                status: err.status,
                message: err.message
            });
        } else {
            // Programming/unexpected error - hide details
            console.error('ERROR:', err);
            res.status(500).json({
                status: 'error',
                message: 'Something went wrong'
            });
        }
    }
};

// 404 handler - catches requests that don't match any route
export const notFoundHandler = (req, res, next) => {
    const err = new AppError(`Route ${req.originalUrl} not found`, 404);
    next(err);
};
```

### Using the Error System

```javascript
// server.js
import express from 'express';
import { errorHandler, notFoundHandler } from './middleware/errorHandler.js';
import { NotFoundError, ValidationError } from './errors/AppError.js';

const app = express();
app.use(express.json());

// Routes
app.get('/users', (req, res) => {
    // ... get users
});

app.get('/users/:id', (req, res) => {
    const userId = parseInt(req.params.id);
    
    if (isNaN(userId)) {
        throw new ValidationError('Invalid user ID');
    }
    
    // Find user...
    if (!user) {
        throw new NotFoundError('User');
    }
    
    res.json(user);
});

// 404 handler - must be AFTER all routes
app.use(notFoundHandler);

// Error handler - must be LAST!
app.use(errorHandler);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Handling Different Error Types

### Database Errors

```javascript
app.use((err, req, res, next) => {
    // PostgreSQL unique constraint violation
    if (err.code === '23505') {
        return res.status(400).json({
            error: 'Duplicate entry'
        });
    }
    
    // MongoDB duplicate key
    if (err.code === 11000) {
        return res.status(400).json({
            error: 'Duplicate entry'
        });
    }
    
    // Connection errors
    if (err.code === 'ECONNREFUSED') {
        return res.status(503).json({
            error: 'Database unavailable'
        });
    }
    
    next(err);
});
```

### Validation Errors (express-validator)

```bash
npm install express-validator
```

```javascript
import { body, validationResult } from 'express-validator';

app.post('/users',
    // Validate input
    body('email').isEmail().withMessage('Invalid email'),
    body('password').isLength({ min: 6 }).withMessage('Password too short'),
    
    // Handle validation errors
    (req, res, next) => {
        const errors = validationResult(req);
        
        if (!errors.isEmpty()) {
            return res.status(400).json({
                errors: errors.array()
            });
        }
        
        next();
    },
    
    // Create user (if validation passes)
    createUser
);
```

## Best Practices

| Practice | Why |
|----------|-----|
| Put error handler LAST | Must come after all routes and middleware |
| Don't call next() after sending response | Response already sent |
| Log errors | Helps with debugging |
| Hide details in production | Security |
| Create custom error classes | Consistent error handling |
| Use appropriate status codes | 400 for bad request, 404 for not found, etc. |

## Error Response Format

```javascript
// Success response
{
    status: 'success',
    data: { /* ... */ }
}

// Error response
{
    status: 'error',
    message: 'Something went wrong'
}

// Validation error
{
    status: 'fail',
    errors: [
        { field: 'email', message: 'Invalid email' }
    ]
}
```

## What's Next?

- **[Security](../08_Security/01_authentication.md)** — Authentication and authorization
- **[Testing](../09_Testing/01_unit_testing.md)** — Testing your Express app
