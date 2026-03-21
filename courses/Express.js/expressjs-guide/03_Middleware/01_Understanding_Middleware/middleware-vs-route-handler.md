# Middleware vs Route Handler

## 📌 What You'll Learn
- The difference between middleware and route handlers in Express
- When to use each
- How they work together

## 🧠 Concept Explained (Plain English)

In Express, both middleware and route handlers are functions that process requests, but they serve different purposes.

Think of it like a security checkpoint at an airport:
- **Middleware** is like the security screening that everyone goes through, regardless of their final destination. It checks IDs, scans bags, etc.
- **Route handlers** are like the gate agents who assist passengers for a specific flight. They only help those going to a particular destination.

Middleware runs for a set of routes (often based on a path prefix) and can perform tasks like logging, authentication, or parsing request bodies. Route handlers are responsible for sending the final response to the client for a specific route.

## 💻 Code Example

```javascript
// ES Module

import express from 'express';

const app = express();

// ========================================
// MIDDLEWARE EXAMPLE
// ========================================
// This middleware runs for every request to /api/*
app.use('/api', (req, res, next) => {
    console.log('API request detected');
    // Perform some task (e.g., logging, authentication)
    next(); // Pass control to the next middleware or route handler
});

// ========================================
// ROUTE HANDLER EXAMPLE
// ========================================
// This route handler sends the final response for GET /api/users
app.get('/api/users', (req, res) => {
    // This function sends the response and ends the request-response cycle
    res.json({ users: ['Alice', 'Bob'] });
});

// Another route handler for a different route
app.get('/api/products', (req, res) => {
    res.json({ products: ['Laptop', 'Phone'] });
});

// ========================================
// MIDDLEWARE THAT ENDS THE REQUEST
// ========================================
// This middleware runs for /api/private/* and sends a response
app.use('/api/private', (req, res) => {
    // This middleware sends a response and does NOT call next()
    // So the request ends here and does not reach any route handlers
    res.status(403).send('Access denied!');
});

// ========================================
// ROUTE HANDLER THAT CALLS next()
// ========================================
// In rare cases, a route handler might call next() to pass to the next route handler
// for the same path (if there are multiple handlers for the same route)
app.get('/users',
    // First route handler for GET /users
    (req, res, next) => {
        console.log('First handler for /users');
        next(); // Pass to the next route handler for GET /users
    },
    // Second route handler for GET /users
    (req, res) => {
        console.log('Second handler for /users');
        res.send('User list');
    }
);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## Key Differences

| Aspect | Middleware | Route Handler |
|--------|------------|---------------|
| **Purpose** | Process request, perform tasks, pass control | Send final response to client |
| **Calls next()?** | Usually yes (unless ending request) | Usually no (sends response) |
| **Can end request?** | Yes (by sending response) | Yes (by sending response) |
| **Typical use** | Logging, auth, parsing, error handling | Sending data, rendering views |
| **Scope** | Can apply to many routes (based on path) | Specific to one route and method |

## When to Use Which

### Use Middleware When:
- You need to log every request
- You need to authenticate users for a group of routes
- You need to parse request bodies (JSON, URL-encoded)
- You need to handle errors for a set of routes
- You want to add headers to responses for a section of your app

### Use Route Handlers When:
- You need to send data to the client for a specific URL
- You need to render a view for a specific route
- You need to create, read, update, or delete a resource
- You need to send a final response (JSON, HTML, file, etc.)

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 9 | `app.use('/api', (req, res, next) => {` | Middleware for /api/* paths |
| 11 | `console.log('API request detected');` | Performs a task (logging) |
| 12 | `next();` | Passes control to next middleware/route handler |
| 17 | `app.get('/api/users', (req, res) => {` | Route handler for GET /api/users |
| 19 | `res.json({ users: ['Alice', 'Bob'] });` | Sends final response and ends cycle |
| 28 | `app.use('/api/private', (req, res) => {` | Middleware that ends the request |
| 30 | `res.status(403).send('Access denied!');` | Sends response, does not call next() |
| 40 | `app.get('/users', (req, res, next) => {` | Route handler that calls next() |
| 42 | `console.log('First handler for /users');` | Performs a task |
| 43 | `next();` | Passes to next route handler for same path |
| 46 | `app.get('/users', (req, res) => {` | Second route handler for same path |
| 48 | `res.send('User list');` | Sends final response |

## ⚠️ Common Mistakes

**1. Using a route handler for tasks that should be middleware**
Don't put authentication logic in every route handler — use middleware instead.

**2. Forgetting to call next() in middleware**
If you forget `next()` in middleware that should pass control, the request stops and your routes are never reached.

**3. Using route handlers to modify the request for other routes**
Route handlers should send a response. If you need to modify the request for downstream handlers, use middleware.

**4. Not realizing that route handlers can call next()**
While rare, route handlers can call `next()` to pass to the next route handler for the same path.

## ✅ Quick Recap

- Middleware processes requests and usually calls `next()` to pass control
- Route handlers send the final response and usually do not call `next()`
- Middleware can apply to groups of routes; route handlers are specific to a route and method
- Both can end the request-response cycle by sending a response

## 🔗 What's Next

Let's learn about the difference between application-level and router-level middleware.
