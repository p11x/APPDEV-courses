# Route Matching in Express.js

## Beyond Basic Routes

Sometimes you need more flexibility than simple paths. Express supports **regular expressions**, **wildcards**, and **optional parameters** for complex routing needs.

## Regular Expression Routes

**Regular expressions (regex)** are patterns that match strings. You can use them in Express routes:

```javascript
// server.js
import express from 'express';

const app = express();

// ============================================
// Route Pattern Table
// ============================================
// | Pattern           | Matches                      |
// |-------------------|------------------------------|
// | /ab*cd            | /ab123cd, /abXYZcd           |
// | /user/:id         | /user/123                    |
// | /file\\.html      | /file.html                   |
// | /api/*            | /api/anything/here           |
// ============================================

// Route with regex: Match /abc, /ab123, /abanything
app.get('/ab*cd', (req, res) => {
    // This matches URLs like /ab123cd, /abXYZcd, /ab Anything cd
    res.send('Route with wildcard: ' + req.path);
});

// Match only numeric IDs: /user/123, /user/999
// :id(\\d+) means "id must be only digits"
app.get('/user/:id(\\d+)', (req, res) => {
    // req.params.id will only contain numbers
    res.json({ 
        userId: req.params.id,
        type: 'numeric ID validated by regex'
    });
});

// Match specific file extensions: /file.html, /file.htm
app.get('/file\\.(html|htm)', (req, res) => {
    res.send('HTML file requested');
});

// Route for articles: /article/123 or /article/abc-def
app.get('/article/:slug([a-z0-9\\-]+)', (req, res) => {
    // Only matches lowercase alphanumeric with hyphens
    res.json({ slug: req.params.slug });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Route Parameters with Options

### Optional Parameters

Make parameters optional by adding `?`:

```javascript
// /user or /user/123 - both will match
app.get('/user/:id?', (req, res) => {
    if (req.params.id) {
        res.json({ userId: req.params.id });
    } else {
        res.json({ message: 'All users' });
    }
});
```

### Multiple Parameters

Have multiple parameters in one route:

```javascript
// /blog/2023/05/my-post
app.get('/blog/:year/:month/:slug', (req, res) => {
    const { year, month, slug } = req.params;
    res.json({ year, month, slug });
});
```

### Parameters with Custom Regex

Validate parameters with built-in regex:

```javascript
// Only match email-like strings
app.get('/email/:address([a-z]+@[a-z]+\\.[a-z]+)', (req, res) => {
    res.json({ email: req.params.address });
});

// Only match UUID format
app.get('/item/:id([0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})', (req, res) => {
    res.json({ itemId: req.params.id });
});
```

## Wildcard and Catch-All Routes

### Single Wildcard

Match anything in a segment:

```javascript
// /file/anything
app.get('/file/*', (req, res) => {
    // req.params[0] contains everything after /file/
    res.json({ 
        path: req.params[0],
        message: 'Wildcard route matched'
    });
});
```

### Catch-All Routes

Match everything not caught by other routes:

```javascript
// Must be last route!
// 404 handler - catches unmatched routes
app.use((req, res) => {
    res.status(404).json({ 
        error: 'Not Found',
        path: req.path
    });
});
```

## Route Matching Order

Express matches routes in the order they're defined. **More specific routes should come first!**

```javascript
// ❌ WRONG: This catches everything!
app.get('*', (req, res) => res.send('Always matches'));
app.get('/specific', (req, res) => res.send('Never reached'));

// ✅ CORRECT: Specific routes first
app.get('/specific', (req, res) => res.send('This works!'));
app.get('*', (req, res) => res.send('Catches everything else'));
```

## Advanced Patterns

### Combining Multiple HTTP Methods

Handle multiple methods on the same path:

```javascript
// Handle multiple methods on one route
app.route('/users')
    .get((req, res) => {
        res.json({ message: 'Get all users' });
    })
    .post((req, res) => {
        res.json({ message: 'Create user' });
    })
    .put((req, res) => {
        res.json({ message: 'Update user' });
    })
    .delete((req, res) => {
        res.json({ message: 'Delete user' });
    });
```

### Chaining with Middleware

Add middleware before your handler:

```javascript
// Middleware that runs before the route handler
const validateId = (req, res, next) => {
    const id = req.params.id;
    
    if (!id || isNaN(id)) {
        return res.status(400).json({ error: 'Invalid ID' });
    }
    
    // 'next()' passes control to the route handler
    next();
};

// Use the middleware for specific routes
app.get('/users/:id', validateId, (req, res) => {
    res.json({ userId: req.params.id });
});
```

### Router-Level Middleware

Apply middleware to all routes in a router:

```javascript
// routes/items.js
import express from 'express';
const router = express.Router();

// Router-level middleware - runs for ALL routes in this router
router.use((req, res, next) => {
    console.log(`[${new Date().toISOString()}] ${req.method} ${req.path}`);
    next();
});

// All routes will have the logging middleware
router.get('/', (req, res) => res.json([]));
router.get('/:id', (req, res) => res.json({}));

export default router;
```

## Complete Example

```javascript
// server.js
import express from 'express';

const app = express();

// ============================================
// Complete Route Table
// ============================================
// | Method | Path                    | Handler         | Description           |
// |--------|-------------------------|-----------------|-----------------------|
// | GET    | /                       | homeHandler    | Home page             |
// | GET    | /hello/:name           | greetHandler   | Personalized greeting|
// | GET    | /api/v:version/*       | apiHandler     | Versioned API        |
// | GET    | /products/:id(\\d+)     | productHandler | Numeric product ID   |
// | ANY    | /catchall               | catchHandler   | Any method catchall  |
// ============================================

// Basic route
app.get('/', (req, res) => {
    res.send('Welcome!');
});

// Route parameter with custom regex
app.get('/products/:id(\\d+)', (req, res) => {
    // Only matches numeric IDs
    res.json({ product: req.params.id });
});

// Multiple parameters
app.get('/api/v:version/*', (req, res) => {
    res.json({ 
        version: req.params.version,
        path: req.params[0]
    });
});

// Personalized greeting
app.get('/hello/:name', (req, res) => {
    res.json({ message: `Hello, ${req.params.name}!` });
});

// Catch-all for other methods
app.all('/catchall', (req, res) => {
    res.json({ 
        method: req.method,
        path: req.path
    });
});

// 404 handler
app.use((req, res) => {
    res.status(404).json({ error: 'Route not found' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Quick Reference

| Pattern | Meaning | Example Match |
|---------|---------|---------------|
| `:param` | Named parameter | `/user/123` |
| `:param?` | Optional parameter | `/user` or `/user/123` |
| `*` | Wildcard | `/anything` |
| `\\d+` | Regex: digits only | `123` |
| `(a|b)` | Regex: a or b | `a` or `b` |

## What's Next?

- **[Middleware](../03_Middleware/01_introduction.md)** — Functions that process requests
- **[Request & Response](../04_Request_Response/01_request_object.md)** — Working with req and res
