# Async Error Handling

## 📌 What You'll Learn
- How async/await works in Express 5 route handlers
- How to handle errors in asynchronous code
- The difference between sync and async error handling
- How to avoid repetitive try/catch blocks

## 🧠 Concept Explained (Plain English)

When you're dealing with operations that take time - like reading files, making database queries, or calling external APIs - JavaScript uses "asynchronous" code. This means the operation starts, but doesn't block (stop) your program while waiting for the result.

Think of it like ordering food at a restaurant. You place your order (start the async operation), the kitchen works on it (the operation runs), and you continue talking to your friends (your program keeps running). When the food is ready, you're notified (the async operation completes).

In older Express versions, handling errors in async code was tricky. You had to wrap everything in try/catch blocks and manually pass errors to the error handler with `next(err)`.

Express 5 (the latest version) makes this much easier! It automatically catches errors thrown in async route handlers and passes them to the error handling middleware. This means you can write cleaner code without repetitive try/catch blocks.

However, it's still important to understand how async error handling works, especially if you need to support older Express versions or want more control over error handling.

## 💻 Code Example

```javascript
// ES Module - Async Error Handling in Express 5

import express from 'express';

const app = express();

// ========================================
// BEFORE EXPRESS 5 - MANUAL ERROR HANDLING
// ========================================

/*
// In Express 4, you needed try/catch for every async operation:
app.get('/users', (req, res, next) => {
    try {
        // Simulate async database call
        const users = await getUsersFromDatabase();
        res.json(users);
    } catch (error) {
        next(error); // Manually pass error to error handler
    }
});
*/

// ========================================
// EXPRESS 5 - AUTOMATIC ERROR HANDLING
// ========================================

// With Express 5, you can write async route handlers without try/catch!
// Express automatically catches any errors and passes them to error middleware

// Simulated async function (like fetching from a database)
async function getUserById(id) {
    // Simulate database delay
    await new Promise(resolve => setTimeout(resolve, 100));
    
    // Simulate finding or not finding a user
    const users = [
        { id: 1, name: 'Alice', email: 'alice@example.com' },
        { id: 2, name: 'Bob', email: 'bob@example.com' }
    ];
    
    const user = users.find(u => u.id === id);
    
    if (!user) {
        throw new Error(`User with ID ${id} not found`);
    }
    
    return user;
}

// Simulated async function that might fail
async function createUser(data) {
    await new Promise(resolve => setTimeout(resolve, 100));
    
    // Simulate validation
    if (!data.name || !data.email) {
        throw new Error('Name and email are required');
    }
    
    // Simulate database error
    if (data.email.includes('error')) {
        throw new Error('Database connection failed');
    }
    
    return { id: 3, ...data };
}

// Route with async handler - Express 5 handles errors automatically!
app.get('/users/:id', async (req, res) => {
    // No try/catch needed in Express 5!
    const userId = parseInt(req.params.id);
    
    // This async function might throw an error
    const user = await getUserById(userId);
    
    // If we get here, everything worked
    res.json(user);
});

// Another async route
app.post('/users', async (req, res) => {
    const { name, email } = req.body;
    
    // This might throw ValidationError or DatabaseError
    const newUser = await createUser({ name, email });
    
    res.status(201).json(newUser);
});

// Async route with Promise.all for parallel operations
app.get('/dashboard', async (req, res) => {
    // These run in parallel - faster than sequential!
    const [users, orders, stats] = await Promise.all([
        getUserById(1),      // Get a user
        Promise.resolve([]), // Get orders (simulated)
        Promise.resolve({    // Get stats (simulated)
            totalUsers: 100,
            totalOrders: 500
        })
    ]);
    
    res.json({
        user,
        orders,
        stats
    });
});

// ========================================
// HANDLING ERRORS WITH .catch() (STILL WORKS)
// ========================================

// You can also use .catch() if you need more control
app.get('/catch-example', async (req, res) => {
    try {
        const result = await someAsyncOperation();
        res.json(result);
    } catch (error) {
        // Custom error handling before passing to middleware
        error.statusCode = 400;
        error.customMessage = 'Custom error message';
        throw error;
    }
});

// ========================================
// ERROR HANDLING MIDDLEWARE
// ========================================

// eslint-disable-next-line no-unused-vars
app.use((err, req, res, next) => {
    console.error('Error occurred:', err.message);
    
    const statusCode = err.statusCode || 500;
    res.status(statusCode).json({
        error: {
            message: err.message || 'Something went wrong',
            status: statusCode
        }
    });
});

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});

/*
// ========================================
// NOTE FOR EXPRESS 4 USERS
// ========================================

// If you're using Express 4, you can use express-async-errors
// to get the same automatic error handling:

// import 'express-async-errors';

// This package automatically wraps async route handlers
// and passes any errors to next(), so you don't need try/catch
*/
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import Express framework |
| 23 | `async function getUserById(id) {` | Define async function (returns a Promise) |
| 25 | `await new Promise(resolve => setTimeout(resolve, 100));` | Simulate async delay |
| 35 | `throw new Error(\`User with ID ${id} not found\`);` | Throw error if user not found |
| 51 | `app.get('/users/:id', async (req, res) => {` | Async route handler (no try/catch!) |
| 56 | `const user = await getUserById(userId);` | Await async function result |
| 57 | `res.json(user);` | Send response |
| 75 | `const [users, orders, stats] = await Promise.all([...])` | Run multiple async operations in parallel |

## Key Concepts Explained

### What is async/await?

`async` and `await` are JavaScript keywords that make asynchronous code easier to read and write.

- `async` before a function means it returns a Promise (a placeholder for a future value)
- `await` before a Promise pauses execution until the Promise resolves (finishes)
- If the Promise rejects (fails), `await` throws an error that you can catch

### Why Express 5 is special

In Express 5, when you use `async` in route handlers, any error thrown (either explicitly with `throw` or from a failed Promise) is automatically caught by Express and passed to your error handling middleware. This eliminates the need for repetitive try/catch blocks.

## ⚠️ Common Mistakes

**1. Forgetting to use await**
If you call an async function without await, it returns a Promise immediately without waiting for the result. Your code will continue running before the operation completes.

**2. Not handling rejected Promises**
If an async function fails and you don't catch the error (or use Express 5), the error might crash your server or cause unexpected behavior.

**3. Mixing sync and async code incorrectly**
Make sure all async operations use await, or properly handle the returned Promises.

**4. Not returning from async handlers**
Always make sure your async route handlers return (or send) a response. If nothing is sent, the request will hang.

**5. Using Express 4 without express-async-errors**
In Express 4, errors in async handlers won't automatically go to error middleware. Use the express-async-errors package or add try/catch.

## ✅ Quick Recap

- Use `async` before route handlers to work with asynchronous code
- Use `await` to wait for Promises to resolve
- Express 5 automatically catches errors from async route handlers
- No need for try/catch in Express 5 async handlers
- Use `Promise.all()` to run multiple async operations in parallel
- In Express 4, use the express-async-errors package for automatic error handling

## 🔗 What's Next

Now let's move on to creating REST APIs with Express. We'll learn how to build proper RESTful services that follow industry best practices.
