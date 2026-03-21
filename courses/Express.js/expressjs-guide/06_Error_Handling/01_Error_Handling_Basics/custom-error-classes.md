# Custom Error Classes

## 📌 What You'll Learn
- How to create custom error classes
- Why custom errors are useful
- How to define error types for different situations
- How to use custom errors in your routes

## 🧠 Concept Explained (Plain English)

Imagine you're running a restaurant and something goes wrong. Instead of just saying "something bad happened," you want to be specific: "the chef is sick" or "we ran out of ingredients" or "the customer didn't pay." Each problem is different and might need a different response.

Custom error classes work the same way. Instead of using generic errors for everything, you create specific error types that represent different failure scenarios in your application. This helps you:
- Give more specific feedback to users
- Handle different errors in different ways
- Keep your error handling code organized
- Make debugging easier

For example, you might create:
- `NotFoundError` for when something isn't found (404 errors)
- `ValidationError` for when input data is invalid
- `UnauthorizedError` for when authentication fails
- `DatabaseError` for database-related issues

Each error can have its own default status code and message, making your error handling more consistent and maintainable.

## 💻 Code Example

```javascript
// ES Module - Creating and Using Custom Error Classes

import express from 'express';

const app = express();

// ========================================
// CREATING CUSTOM ERROR CLASSES
// ========================================

// Base error class that other errors can extend
class AppError extends Error {
    constructor(message, statusCode) {
        super(message); // Call the parent Error class constructor
        this.statusCode = statusCode;
        this.status = `${statusCode}`.startsWith('4') ? 'fail' : 'error';
        this.isOperational = true; // Mark as intentional/expected error
        
        // Capture the stack trace (where the error was created)
        Error.captureStackTrace(this, this.constructor);
    }
}

// NotFoundError - for resources that don't exist
class NotFoundError extends AppError {
    constructor(message = 'Resource not found') {
        super(message, 404);
    }
}

// ValidationError - for invalid input data
class ValidationError extends AppError {
    constructor(message = 'Validation failed') {
        super(message, 400);
    }
}

// UnauthorizedError - for authentication failures
class UnauthorizedError extends AppError {
    constructor(message = 'Unauthorized access') {
        super(message, 401);
    }
}

// ForbiddenError - for permission issues
class ForbiddenError extends AppError {
    constructor(message = 'Access forbidden') {
        super(message, 403);
    }
}

// ConflictError - for duplicate resources
class ConflictError extends AppError {
    constructor(message = 'Resource already exists') {
        super(message, 409);
    }
}

// ========================================
// USING CUSTOM ERRORS IN ROUTES
// ========================================

// Sample data
const users = [
    { id: 1, name: 'Alice', email: 'alice@example.com' },
    { id: 2, name: 'Bob', email: 'bob@example.com' }
];

// Route that uses NotFoundError
app.get('/users/:id', (req, res, next) => {
    const userId = parseInt(req.params.id);
    const user = users.find(u => u.id === userId);
    
    if (!user) {
        // Throw a NotFoundError if user doesn't exist
        throw new NotFoundError(`User with ID ${userId} not found`);
    }
    
    res.json(user);
});

// Route that uses ValidationError
app.post('/users', (req, res, next) => {
    const { name, email } = req.body;
    
    // Validate required fields
    if (!name || !email) {
        throw new ValidationError('Name and email are required');
    }
    
    // Validate email format (simple check)
    if (!email.includes('@')) {
        throw new ValidationError('Invalid email format');
    }
    
    // Check for duplicate email
    const existingUser = users.find(u => u.email === email);
    if (existingUser) {
        throw new ConflictError('User with this email already exists');
    }
    
    // Create new user (in real app, save to database)
    const newUser = {
        id: users.length + 1,
        name,
        email
    };
    
    users.push(newUser);
    res.status(201).json(newUser);
});

// Route that uses UnauthorizedError
app.get('/admin/dashboard', (req, res, next) => {
    // Simulate authentication check
    const isAuthenticated = false; // In real app, check session/token
    
    if (!isAuthenticated) {
        throw new UnauthorizedError('Please log in to access this page');
    }
    
    res.json({ message: 'Welcome to the admin dashboard!' });
});

// Route that uses ForbiddenError
app.delete('/users/:id', (req, res, next) => {
    // Simulate authorization check
    const isAdmin = false; // In real app, check user role
    
    if (!isAdmin) {
        throw new ForbiddenError('You do not have permission to delete users');
    }
    
    res.json({ message: 'User deleted successfully' });
});

// ========================================
// ERROR HANDLING MIDDLEWARE
// ========================================

// eslint-disable-next-line no-unused-vars
app.use((err, req, res, next) => {
    // Check if the error is one of our custom operational errors
    if (err.isOperational) {
        // Handle our custom errors with their specific status codes
        res.status(err.statusCode).json({
            error: {
                message: err.message,
                status: err.statusCode,
                type: err.constructor.name // Shows the error class name
            }
        });
    } else {
        // Handle unexpected errors (bugs, system errors)
        console.error('Unexpected error:', err);
        
        res.status(500).json({
            error: {
                message: 'Internal server error',
                status: 500
            }
        });
    }
});

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `class AppError extends Error {` | Define base error class extending Error |
| 9 | `constructor(message, statusCode) {` | Accept message and HTTP status code |
| 10 | `super(message);` | Call parent Error constructor with message |
| 11 | `this.statusCode = statusCode;` | Store the HTTP status code |
| 12 | `this.status = ...` | Set status type ('fail' for 4xx, 'error' for 5xx) |
| 13 | `this.isOperational = true;` | Mark as intentional error |
| 16 | `Error.captureStackTrace(this, this.constructor);` | Capture stack trace properly |
| 21 | `class NotFoundError extends AppError {` | Create NotFoundError extending our base |
| 22 | `super(message, 404);` | Call parent with message and 404 status |
| 55 | `throw new NotFoundError(...)` | Throw custom error in route handler |
| 77 | `throw new ValidationError(...)` | Throw validation error |
| 90 | `throw new ConflictError(...)` | Throw conflict error for duplicates |
| 115 | `if (err.isOperational) {` | Check if it's our custom error |
| 117 | `res.status(err.statusCode).json({...});` | Handle with specific status code |
| 127 | `console.error('Unexpected error:', err);` | Log unexpected errors differently |

## ⚠️ Common Mistakes

**1. Not calling super() in custom error constructors**
Always call `super(message)` to properly initialize the parent Error class.

**2. Forgetting to capture the stack trace**
Use `Error.captureStackTrace(this, this.constructor)` to get meaningful stack traces.

**3. Not marking operational errors**
Set `isOperational = true` for expected errors so you can distinguish them from bugs.

**4. Creating too many error types**
Start with a few general-purpose errors and add more specific ones only when needed.

**5. Not handling non-operational errors differently**
Unexpected errors (bugs) should be handled differently than operational errors (expected situations).

## ✅ Quick Recap

- Create a base `AppError` class that extends Error
- Add `statusCode`, `status`, and `isOperational` properties
- Create specific error classes for different scenarios (NotFoundError, ValidationError, etc.)
- Use `Error.captureStackTrace()` to properly capture stack traces
- In error handlers, check `err.isOperational` to handle custom vs. unexpected errors
- Custom errors make it easier to give specific feedback and handle different errors differently

## 🔗 What's Next

Let's look at how to handle 404 errors for routes that don't exist, and how to create a centralized error handling system for your entire application.
