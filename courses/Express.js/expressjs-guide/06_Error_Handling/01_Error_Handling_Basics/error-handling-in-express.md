# Error Handling in Express

## 📌 What You'll Learn
- How Express handles errors automatically
- How to create custom error handling middleware
- The difference between synchronous and asynchronous errors
- How to pass errors to the next middleware

## 🧠 Concept Explained (Plain English)

When something goes wrong in your Express application - maybe a database connection fails, a file isn't found, or a user tries to access something they shouldn't - you need a way to handle those errors gracefully. That's where error handling comes in.

Think of error handling like a safety net for a circus performer. Without a safety net, any mistake could be catastrophic. With a safety net (error handling), the performer can take risks knowing that if something goes wrong, they'll be caught safely.

In Express, error handling works through special middleware functions that have four parameters instead of three: `err` (the error object), `req` (the request), `res` (the response), and `next`. When you call `next(err)`, Express knows this is an error and skips to the error handling middleware instead of continuing with regular middleware.

Express 5 (the latest version) makes error handling even easier with native async/await support - you don't need try/catch blocks in your route handlers anymore!

## 💻 Code Example

```javascript
// ES Module - Basic Error Handling in Express 5

import express from 'express';

const app = express();

// ========================================
// BASIC ROUTES (NORMAL MIDDLEWARE)
// ========================================

// This is a regular route - no special error handling needed
app.get('/', (req, res) => {
    res.send('Welcome to our website!');
});

// Another regular route
app.get('/about', (req, res) => {
    res.send('About Us - We build great things!');
});

// ========================================
// ROUTES THAT MIGHT HAVE ERRORS
// ========================================

// Simulating a route that works fine
app.get('/users', (req, res) => {
    // In a real app, this would fetch from a database
    const users = [
        { id: 1, name: 'Alice' },
        { id: 2, name: 'Bob' }
    ];
    res.json(users);
});

// ========================================
// THE ERROR HANDLING MIDDLEWARE
// ========================================
// This middleware has FOUR parameters - Express knows it's an error handler
// It will only be called when next(err) is called or an error is thrown

// eslint-disable-next-line no-unused-vars
app.use((err, req, res, next) => {
    // err - the error object that was passed
    // req - the request object
    // res - the response object  
    // next - function to pass control to the next middleware
    
    console.error('Error occurred:', err.message);
    
    // Set the HTTP status code based on the error
    // Default to 500 (Internal Server Error) if no status code is set
    const statusCode = err.statusCode || 500;
    
    // Send an appropriate error response
    res.status(statusCode).json({
        error: {
            message: err.message || 'Something went wrong',
            status: statusCode,
            // Include stack trace in development (not production!)
            stack: process.env.NODE_ENV === 'development' ? err.stack : undefined
        }
    });
});

// ========================================
// STARTING THE SERVER
// ========================================
const PORT = process.env.PORT || 3000;

// In Express 5, we can use async app.listen directly
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});

/*
// ========================================
// HOW TO TRIGGER THE ERROR HANDLER
// ========================================

// Option 1: Create a route that manually triggers an error
app.get('/trigger-error', (req, res, next) => {
    const error = new Error('This is a test error!');
    error.statusCode = 400; // Set a custom status code
    next(error); // Pass the error to the error handler
});

// Option 2: Throw an error (Express 5 catches this automatically!)
app.get('/throw-error', (req, res) => {
    throw new Error('Something went wrong!');
});

// Option 3: Async route that rejects (Express 5 handles this!)
app.get('/async-error', async (req, res) => {
    // Simulate an async operation that fails
    const shouldFail = true;
    if (shouldFail) {
        throw new Error('Async operation failed!');
    }
    res.send('Success!');
});
*/
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import Express framework |
| 4 | `const app = express();` | Create Express application |
| 9 | `app.get('/', (req, res) => {` | Regular route with request and response |
| 10 | `res.send('Welcome to our website!');` | Send response back to client |
| 31 | `app.use((err, req, res, next) => {` | Error handling middleware with 4 parameters |
| 36 | `console.error('Error occurred:', err.message);` | Log the error for debugging |
| 39 | `const statusCode = err.statusCode \|\| 500;` | Use error's status or default to 500 |
| 42 | `res.status(statusCode).json({...});` | Send JSON error response |
| 55 | `app.listen(PORT, () => {...});` | Start the Express server |

## ⚠️ Common Mistakes

**1. Not having an error handler at all**
Without an error handling middleware, errors will crash your application and send a generic error to users.

**2. Forgetting the fourth parameter**
Error handling middleware must have four parameters: `(err, req, res, next)`. If you omit one, Express won't recognize it as an error handler.

**3. Not calling next(err)**
If you handle an error but don't call next(err), the request will hang forever. Always call next(err) to pass the error along.

**4. Placing error handler before other middleware**
Error handling middleware should typically be the LAST middleware in your app, after all routes and regular middleware.

**5. Exposing stack traces in production**
Never show detailed error information (like stack traces) to users in production - it reveals your internal code structure.

## ✅ Quick Recap

- Error handling middleware has four parameters: `(err, req, res, next)`
- Call `next(err)` to pass an error to the error handler
- Set `err.statusCode` to control the HTTP status of the error response
- Error handlers should be placed at the END of your middleware chain
- In development, include stack traces for debugging; in production, hide them
- Express 5 automatically catches thrown errors in async route handlers

## 🔗 What's Next

Let's look at how to create and throw custom errors, including how to define different error types for different situations.
