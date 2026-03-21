# Express Middleware

## What You'll Learn

- What middleware is
- How to create and use middleware
- Built-in and custom middleware
- Error handling middleware

## What is Middleware?

**Middleware** functions are functions that have access to the request and response objects. They can:
- Execute any code
- Make changes to request/response objects
- End the response cycle
- Call the next middleware

## Middleware Flow

```
Request → Middleware1 → Middleware2 → Route Handler → Response
                    ↓
              next()
```

## Creating Middleware

### Basic Middleware

```javascript
// middleware-basic.js - Basic middleware

import express from 'express';
const app = express();

// Middleware function
function logger(req, res, next) {
  console.log(`${req.method} ${req.url}`);
  next();  // Continue to next middleware
}

// Use middleware
app.use(logger);

// Route
app.get('/', (req, res) => {
  res.send('Hello!');
});
```

### Multiple Middleware

```javascript
// Multiple middleware

// First middleware
function firstMiddleware(req, res, next) {
  console.log('First');
  next();
}

// Second middleware
function secondMiddleware(req, res, next) {
  console.log('Second');
  next();
}

// Use both - they run in order
app.use(firstMiddleware);
app.use(secondMiddleware);
```

## Built-in Middleware

### express.json()

Parse JSON request bodies:

```javascript
app.use(express.json());

app.post('/users', (req, res) => {
  // req.body is now an object
  console.log(req.body);
  res.json({ received: req.body });
});
```

### express.urlencoded()

Parse URL-encoded bodies:

```javascript
app.use(express.urlencoded({ extended: true }));
```

### express.static()

Serve static files:

```javascript
// Serve files from 'public' folder
app.use(express.static('public'));
```

## Custom Middleware Examples

### Logging Middleware

```javascript
// logging-middleware.js

function loggingMiddleware(req, res, next) {
  const start = Date.now();
  
  // Call next() to continue
  next();
  
  // After response is sent
  res.on('finish', () => {
    const duration = Date.now() - start;
    console.log(`${req.method} ${req.url} - ${res.statusCode} (${duration}ms)`);
  });
}

app.use(loggingMiddleware);
```

### Authentication Middleware

```javascript
// auth-middleware.js

function authMiddleware(req, res, next) {
  const token = req.headers.authorization;
  
  if (token === 'secret-token') {
    next();  // Allow access
  } else {
    res.status(401).json({ error: 'Unauthorized' });
  }
}

// Protect specific routes
app.get('/protected', authMiddleware, (req, res) => {
  res.send('Protected content!');
});
```

## Code Example: Complete Middleware Demo

```javascript
// server.js - Complete middleware demonstration

import express from 'express';
const app = express();
const PORT = 3000;

// ─────────────────────────────────────────
// 1. Basic Middleware
// ─────────────────────────────────────────
console.log('1. Basic Middleware:');

app.use((req, res, next) => {
  console.log(`   ${req.method} request to ${req.url}`);
  next();
});

// ─────────────────────────────────────────
// 2. Built-in JSON Middleware
// ─────────────────────────────────────────
console.log('2. JSON Body Parsing:');

app.use(express.json());

// ─────────────────────────────────────────
// 3. Route-specific Middleware
// ─────────────────────────────────────────
console.log('3. Route-specific Middleware:');

function checkInput(req, res, next) {
  const { name } = req.body;
  
  if (!name) {
    return res.status(400).json({ error: 'Name required' });
  }
  
  next();  // Continue
}

// This route uses the middleware
app.post('/greet', checkInput, (req, res) => {
  res.json({ message: `Hello, ${req.body.name}!` });
});

// ─────────────────────────────────────────
// 4. Multiple Middleware
// ─────────────────────────────────────────
console.log('4. Multiple Middleware Chain:');

app.get(
  '/chain',
  (req, res, next) => {
    console.log('   Middleware 1');
    req.step = 1;
    next();
  },
  (req, res, next) => {
    console.log('   Middleware 2');
    req.step += 1;
    next();
  },
  (req, res) => {
    res.json({ step: req.step });
  }
);

// ─────────────────────────────────────────
// 5. Error-handling Middleware
// ─────────────────────────────────────────
console.log('5. Error Handling:');

// Error-handling middleware has 4 parameters
function errorHandler(err, req, res, next) {
  console.error('Error:', err.message);
  res.status(500).json({ error: 'Something went wrong!' });
}

app.use(errorHandler);

// ─────────────────────────────────────────
// Routes
// ─────────────────────────────────────────

app.get('/', (req, res) => {
  res.send('Check console for middleware logs!');
});

app.get('/error', (req, res) => {
  throw new Error('Oops!');
});

// Start server
app.listen(PORT, () => {
  console.log(`Server running on http://localhost:${PORT}`);
});
```

## Error Handling Middleware

### Creating Error Handlers

Error-handling middleware must have 4 parameters:

```javascript
// Error handler - MUST have 4 params
app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).send('Something broke!');
});
```

### Throwing Errors

```javascript
app.get('/error', (req, res, next) => {
  const error = new Error('Something went wrong');
  error.status = 500;
  next(error);  // Pass to error handler
});
```

## Common Mistakes

### Mistake 1: Forgetting next()

```javascript
// WRONG - request will hang
app.use((req, res) => {
  console.log('Got request');
  // Forgot next()!
});

// CORRECT
app.use((req, res, next) => {
  console.log('Got request');
  next();
});
```

### Mistake 2: Wrong Parameter Count

```javascript
// WRONG - 3 params means regular middleware
app.use((err, req, res) => {  // Won't catch errors!
});

// CORRECT - 4 params for error handling
app.use((err, req, res, next) => {  // Catches errors!
});
```

## Try It Yourself

### Exercise 1: Create Logging Middleware
Create middleware that logs request method, URL, and timestamp.

### Exercise 2: Create Auth Middleware
Create middleware that checks for an API key.

### Exercise 3: Error Handling
Add error handling middleware to your Express app.

## Next Steps

Now you know middleware. Let's learn about the request object. Continue to [Request Object](./../request-response/01-req-object.md).
