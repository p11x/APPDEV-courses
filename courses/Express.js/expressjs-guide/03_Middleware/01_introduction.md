# Middleware in Express.js

## What is Middleware?

**Middleware** functions are functions that have access to the request and response objects. They can:
- Execute any code
- Make changes to the request or response objects
- End the request-response cycle
- Call the next middleware in the chain

Think of middleware as a **checkpoint** or **filter** that requests pass through. Each middleware can examine, modify, or reject a request before it reaches your route handlers.

## Understanding the Flow

When a request comes in, it flows through middleware like water through pipes:

```
Request → [Middleware 1] → [Middleware 2] → [Route Handler] → Response
              ↓                ↓                   ↓
         (optional)       (optional)          (always runs)
```

## Middleware Function Structure

Every middleware function takes three parameters:

```javascript
function middlewareName(req, res, next) {
    // req  = request object (what the client sent)
    // res   = response object (what we send back)
    // next  = function to call the next middleware
    
    // Your code here
    
    next(); // Don't forget to call next()!
}
```

> **What is `next`?** It's a function that passes control to the next middleware in the chain. If you don't call it, the request "stops" and the client never gets a response.

## Types of Middleware

Express has several types of middleware:

| Type | Description | Example |
|------|-------------|---------|
| **Built-in** | Comes with Express | `express.json()` |
| **Third-party** | Created by community | `morgan`, `cors` |
| **Custom** | You create yourself | Authentication, logging |
| **Error-handling** | Handles errors | 404, exceptions |

## Built-in Middleware

Express provides some useful built-in middleware:

```javascript
// server.js
import express from 'express';

const app = express();
// 'app' is our Express application instance

// ============================================
// Middleware Table
// ============================================
// | Middleware               | Purpose                           |
// |-------------------------|-----------------------------------|
// | express.json()         | Parse JSON request bodies        |
// | express.urlencoded()   | Parse form data                  |
// | express.static()       | Serve static files               |
// | express.text()         | Parse plain text                 |
// | express.raw()          | Parse binary data                |
// ============================================

// Parse JSON bodies - REQUIRED for POST/PUT requests with JSON
// This middleware runs for every request and parses JSON data
// After this, you can use req.body to access the JSON data
app.use(express.json());

// Parse URL-encoded bodies (form submissions)
// extended: true allows parsing nested objects
app.use(express.urlencoded({ extended: true }));

// Serve static files from 'public' folder
// This makes files in /public available at the root URL
app.use(express.static('public'));

// Routes
app.get('/', (req, res) => {
    res.send('Check out the /api/data endpoint!');
});

app.post('/api/data', (req, res) => {
    // req.body is available because we used express.json()
    console.log('Received:', req.body);
    res.json({ received: req.body });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Custom Middleware

Let's create our own middleware functions:

```javascript
// server.js
import express from 'express';

const app = express();

// ============================================
// Custom Middleware Functions
// ============================================

// 1. Logging Middleware - logs every request
const logger = (req, res, next) => {
    // Log the request method and URL
    console.log(`[${new Date().toISOString()}] ${req.method} ${req.path}`);
    
    // Call next() to pass control to the next middleware
    next();
};

// 2. Request Timer - measures how long request takes
const timer = (req, res, next) => {
    // Add startTime to the request object
    req.startTime = Date.now();
    
    // We'll complete this in the response
    const originalSend = res.send;
    res.send = function(data) {
        const duration = Date.now() - req.startTime;
        console.log(`Request took ${duration}ms`);
        // Call original send function
        return originalSend.call(this, data);
    };
    
    next();
};

// 3. Authentication Middleware - checks if user is logged in
const authenticate = (req, res, next) => {
    // Check for Authorization header
    const authHeader = req.headers.authorization;
    
    if (!authHeader) {
        // No auth header - return 401 Unauthorized
        return res.status(401).json({ error: 'No token provided' });
    }
    
    // In real apps, validate the token here
    if (authHeader.startsWith('Bearer ')) {
        // Token exists, continue
        req.user = { id: 1, name: 'Alice' }; // Mock user
        return next();
    }
    
    // Invalid token
    res.status(401).json({ error: 'Invalid token' });
};

// 4. Validation Middleware - validates request data
const validateUser = (req, res, next) => {
    const { name, email } = req.body;
    
    if (!name || !email) {
        return res.status(400).json({ 
            error: 'Name and email are required' 
        });
    }
    
    // Validation passed, continue
    next();
};

// ============================================
// Register Middleware
// ============================================

// Apply to ALL routes
app.use(logger);
app.use(timer);

// Protected route - requires authentication
app.get('/protected', authenticate, (req, res) => {
    res.json({ 
        message: 'Welcome to protected route!',
        user: req.user 
    });
});

// Create user with validation
app.post('/users', validateUser, (req, res) => {
    res.json({ 
        message: 'User created!',
        data: req.body 
    });
});

// Public route
app.get('/public', (req, res) => {
    res.json({ message: 'Public data' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Middleware Order Matters

The order you define middleware determines when it runs:

```javascript
// ❌ WRONG ORDER
app.get('/user', authenticate, (req, res) => res.send('User')); // authenticate runs AFTER routes

// ✅ CORRECT ORDER - middleware first!
app.use(authenticate);  // Runs first for ALL routes
app.get('/user', (req, res) => res.send('User')); // Then routes
```

## Applying Middleware to Specific Routes

You can apply middleware only to certain routes:

```javascript
// Apply to specific routes
app.get('/dashboard', requireAuth, (req, res) => res.send('Dashboard'));
app.post('/api/data', validateInput, logRequest, (req, res) => res.json({}));
```

## Router-Level Middleware

Apply middleware to all routes in a router:

```javascript
// routes/users.js
import express from 'express';
const router = express.Router();

// This middleware runs for ALL routes in this router
router.use((req, res, next) => {
    console.log('User route accessed:', req.path);
    next();
});

router.get('/', (req, res) => res.json([]));
router.get('/:id', (req, res) => res.json({}));

export default router;
```

## Complete Example

```javascript
// server.js
import express from 'express';

const app = express();

// Middleware: Parse JSON
app.use(express.json());

// Custom middleware: Add request ID
app.use((req, res, next) => {
    req.requestId = Math.random().toString(36).substring(7);
    next();
});

// Routes with inline middleware
app.get('/fast', (req, res) => {
    res.json({ id: req.requestId, message: 'Fast response!' });
});

app.get('/slow', (req, res) => {
    // Simulate slow processing
    setTimeout(() => {
        res.json({ id: req.requestId, message: 'Slow response!' });
    }, 1000);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Third-Party Middleware

Popular middleware packages:

| Package | Purpose | Installation |
|---------|---------|--------------|
| **morgan** | HTTP logging | `npm install morgan` |
| **cors** | Cross-origin requests | `npm install cors` |
| **helmet** | Security headers | `npm install helmet` |
| **multer** | File uploads | `npm install multer` |

```javascript
// Using third-party middleware
import morgan from 'morgan';
import cors from 'cors';
import helmet from 'helmet';

app.use(morgan('tiny'));   // Log HTTP requests
app.use(cors());            // Enable CORS
app.use(helmet());          // Security headers
```

## What's Next?

- **[Error Handling Middleware](./02_error_handling_middleware.md)** — Handling errors gracefully
- **[Third-Party Middleware](./03_third_party_middleware.md)** — Popular middleware packages
- **[Request & Response](../04_Request_Response/01_request_object.md)** — Working with req and res
