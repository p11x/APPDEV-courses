# Application-Level vs Router-Level Middleware

## 📌 What You'll Learn
- The difference between application-level and router-level middleware
- When to use each type
- How to apply middleware at different levels

## 🧠 Concept Explained (Plain English)

In Express, middleware can be applied at two main levels: **application-level** and **router-level**. 

Think of it like security in a building:
- **Application-level middleware** is like security checks at the main entrance of the building. Everyone who enters the building goes through these checks, regardless of which office they're visiting.
- **Router-level middleware** is like security checks at a specific department or floor. Only people going to that specific department go through these checks.

Application-level middleware is bound to the `app` object and applies to all routes in the application. Router-level middleware is bound to a specific `express.Router()` instance and applies only to the routes defined in that router.

This allows you to apply global concerns (like logging, security headers) at the application level, and section-specific concerns (like authentication for an API section) at the router level.

## 💻 Code Example

```javascript
// ES Module

import express from 'express';

const app = express();

// ========================================
// APPLICATION-LEVEL MIDDLEWARE
// ========================================
// This middleware runs for EVERY request to the app
// It's like a security check at the main entrance

// Log every request
app.use((req, res, next) => {
    console.log(`[APP] ${req.method} ${req.url}`);
    next();
});

// Set security headers for all responses
app.use((req, res, next) => {
    res.setHeader('X-Content-Type-Options', 'nosniff');
    next();
});

// ========================================
// ROUTER-LEVEL MIDDLEWARE
// ========================================
// This middleware runs only for requests to the router
// It's like a security check for a specific department

// Create a router for API routes
const apiRouter = express.Router();

// This middleware runs for ALL routes in the apiRouter
// For example, it will run for /api/users, /api/products, etc.
apiRouter.use((req, res, next) => {
    console.log(`[API ROUTER] ${req.method} ${req.url}`);
    next();
});

// Authentication middleware for the API router
apiRouter.use((req, res, next) => {
    // In a real app, check for valid token/session
    const authHeader = req.headers.authorization;
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
        return res.status(401).json({ error: 'Unauthorized' });
    }
    // If we had a real token, we'd verify it here
    next();
});

// Now define routes in the API router - they will have the router-level middleware applied
apiRouter.get('/users', (req, res) => {
    res.json({ users: ['Alice', 'Bob'] });
});

apiRouter.get('/products', (req, res) => {
    res.json({ products: ['Laptop', 'Phone'] });
});

// Create another router for public routes (no special middleware needed)
const publicRouter = express.Router();

publicRouter.get('/', (req, res) => {
    res.json({ message: 'Welcome to the public site' });
});

publicRouter.get('/about', (req, res) => {
    res.json({ message: 'About this site' });
});

// ========================================
// MOUNTING THE ROUTERS
// ========================================
// Mount the API router at /api
app.use('/api', apiRouter);

// Mount the public router at the root
app.use('/', publicRouter);

// ========================================
// HOW IT WORKS
// ========================================
// Request: GET /api/users
// 1. Application-level middleware runs
//    Logs: [APP] GET /api/users
//    Sets X-Content-Type-Options header
// 2. Router-level middleware for apiRouter runs
//    Logs: [API ROUTER] GET /api/users
//    Checks authentication
// 3. Route handler runs
//    Returns users list

// Request: GET /about
// 1. Application-level middleware runs
//    Logs: [APP] GET /about
//    Sets X-Content-Type-Options header
// 2. No router-level middleware for publicRouter (we didn't add any)
// 3. Route handler runs
//    Returns about message

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## Key Differences

| Aspect | Application-Level | Router-Level |
|--------|-------------------|--------------|
| **Where defined** | On the `app` object | On a `router` object |
| **Scope** | All routes in the app | Only routes in the specific router |
| **Registration** | `app.use()` or `app.METHOD()` | `router.use()` or `router.METHOD()` |
| **Use case** | Global concerns (logging, security, body parsing) | Section-specific concerns (API authentication, admin privileges) |
| **Order** | Runs before router-level middleware for a route | Runs after application-level middleware for a route |

## When to Use Which

### Use Application-Level Middleware When:
- You need to log every request (regardless of route)
- You need to set security headers for all responses
- You need to parse request bodies (JSON, URL-encoded) for all routes
- You want to handle errors for the entire application
- You need to serve static files for the entire app

### Use Router-Level Middleware When:
- You need to authenticate users for a specific section (like /api/*)
- You want to apply rate limiting to a specific group of routes
- You need to log requests only for a specific section
- You want to apply specific headers only to certain routes
- You need to validate input for a specific set of routes

## 🔍 Line-by-Line Breakdown (Application-Level Example)

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `app.use((req, res, next) => {` | Application-level middleware registration |
| 10 | `console.log(`[APP] ${req.method} ${req.url}`);` | Logs every request |
| 11 | `next();` | Passes control to next middleware |
| 14 | `app.use((req, res, next) => {` | Another application-level middleware |
| 16 | `res.setHeader('X-Content-Type-Options', 'nosniff');` | Sets a security header |
| 17 | `next();` | Passes control to next middleware |

## ⚠️ Common Mistakes

**1. Applying router-level middleware at the application level**
If you put section-specific middleware (like API authentication) at the application level, it will run for every request, including those that don't need it (like public pages).

**2. Forgetting to mount the router**
If you create a router with middleware but never mount it with `app.use()`, the middleware will never run.

**3. Misunderstanding the order**
Application-level middleware always runs before router-level middleware for the same route.

**4. Not realizing that middleware order matters within each level**
Just like with application-level middleware, the order of router-level middleware matters.

## ✅ Quick Recap

- Application-level middleware runs for every request to the app
- Router-level middleware runs only for requests to a specific router
- Use application-level for global concerns (logging, security, parsing)
- Use router-level for section-specific concerns (authentication, rate limiting)
- Application-level middleware runs before router-level middleware for the same route

## 🔗 What's Next

Now let's learn about writing custom middleware.
