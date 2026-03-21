# Router-Level Middleware

## 📌 What You'll Learn
- What router-level middleware is
- How to apply middleware to specific routers
- Differences between application-level and router-level middleware

## 🧠 Concept Explained (Plain English)

Middleware can be applied at different levels in your Express application. 
**Application-level middleware** runs for every request to your app.
**Router-level middleware** runs only for requests that match a specific router.

Think of it like security checks in a building. 
Application-level middleware is like a security check at the main entrance — everyone goes through it.
Router-level middleware is like a security check at a specific department — only people going to that department go through it.

This allows you to apply middleware only where it's needed, making your application more efficient and organized.

## 💻 Code Example

### Step 1: Create a Router with Router-Level Middleware

```javascript
// routes/userRoutes.js
import express from 'express';
const router = express.Router();

// ========================================
// ROUTER-LEVEL MIDDLEWARE
// ========================================
// This middleware runs for ALL routes in this router
// It's like a security check for the users section

// Log all requests to user routes
router.use((req, res, next) => {
    console.log(`[User Route] ${req.method} ${req.path}`);
    next();
});

// Authentication middleware for user routes
router.use((req, res, next) => {
    // In a real app, check for valid token/session
    const authHeader = req.headers.authorization;
    if (!authHeader || !authHeader.startsWith('Bearer ')) {
        return res.status(401).json({ error: 'Unauthorized' });
    }
    // If we had a real token, we'd verify it here
    next();
});

// Now define your routes - they will have the middleware above applied
router.get('/', (req, res) => {
    res.json({ message: 'Users list (protected)' });
});

router.get('/:id', (req, res) => {
    res.json({ userId: req.params.id });
});

export default router;
```

### Step 2: Create Another Router Without the Middleware

```javascript
// routes/publicRoutes.js
import express from 'express';
const router = express.Router();

// No router-level middleware here - these routes are public

router.get('/', (req, res) => {
    res.json({ message: 'Public homepage' });
});

router.get('/about', (req, res) => {
    res.json({ message: 'About page' });
});

export default router;
```

### Step 3: Use Both Routers in Your Main App

```javascript
// server.js
import express from 'express';
import userRoutes from './routes/userRoutes.js';
import publicRoutes from './routes/publicRoutes.js';

const app = express();

// Application-level middleware - runs for EVERY request
app.use((req, res, next) => {
    console.log(`[Global] ${req.method} ${req.path}`);
    next();
});

app.use(express.json());

// Mount the routers
app.use('/api/users', userRoutes);   // Has router-level middleware
app.use('/', publicRoutes);          // No router-level middleware

app.get('/', (req, res) => {
    res.json({ message: 'Welcome to the API' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## How It Works

```
Request: GET /api/users/123

1. Application-level middleware runs
   Logs: [Global] GET /api/users/123

2. Router-level middleware for userRoutes runs
   Logs: [User Route] GET /api/users/123
   Checks authentication

3. Route handler runs
   Returns user data

Request: GET /about

1. Application-level middleware runs
   Logs: [Global] GET /about

2. No router-level middleware for publicRoutes
   (we didn't add any)

3. Route handler runs
   Returns about page
```

## Router-Level vs Application-Level Middleware

| Aspect | Application-Level | Router-Level |
|--------|-------------------|--------------|
| Where defined | In main app (server.js) | In router file |
| Scope | All routes in app | Only routes in specific router |
| Registration | `app.use()` | `router.use()` |
| Use case | Global concerns (logging, security) | Section-specific concerns (auth for API) |

## ⚠️ Common Mistakes

**1. Forgetting to call next()**
If you don't call `next()` in middleware, the request stops and never reaches your routes.

**2. Applying middleware at the wrong level**
Don't put section-specific middleware at the application level if it's not needed everywhere.

**3. Order within a router matters**
Router-level middleware runs in the order you define it, just like application-level middleware.

## ✅ Quick Recap

- Router-level middleware runs only for routes in a specific router
- Use `router.use()` to define it
- Great for section-specific concerns like authentication, logging, validation
- Order of middleware matters within the router

## 🔗 What's Next

Let's move on to advanced routing concepts.
