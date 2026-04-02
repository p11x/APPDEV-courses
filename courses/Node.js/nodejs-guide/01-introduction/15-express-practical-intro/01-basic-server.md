# Setting Up a Basic Express Server

## What You'll Learn

- Creating your first Express.js server
- Understanding request/response lifecycle
- Basic middleware patterns
- Error handling fundamentals

## Step 1: Project Setup

```bash
mkdir my-express-app
cd my-express-app
npm init -y
npm install express
```

Update `package.json`:
```json
{
  "name": "my-express-app",
  "version": "1.0.0",
  "type": "module",
  "scripts": {
    "start": "node src/server.js",
    "dev": "node --watch src/server.js"
  },
  "dependencies": {
    "express": "^4.21.0"
  }
}
```

## Step 2: Basic Server

Create `src/server.js`:
```javascript
// src/server.js — Basic Express server

import express from 'express';

const app = express();
const PORT = process.env.PORT || 3000;

// Parse JSON request bodies
app.use(express.json());

// Basic route
app.get('/', (req, res) => {
    res.json({
        message: 'Hello from Express!',
        timestamp: new Date().toISOString(),
        nodeVersion: process.version,
    });
});

// Health check endpoint
app.get('/health', (req, res) => {
    res.json({
        status: 'healthy',
        uptime: process.uptime(),
        memory: process.memoryUsage(),
    });
});

// 404 handler
app.use((req, res) => {
    res.status(404).json({ error: 'Not found' });
});

// Error handler
app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).json({ error: 'Internal server error' });
});

app.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
});
```

```bash
npm run dev
# Server running on http://localhost:3000
# Edit and save — auto-restarts
```

## Step 3: Request/Response Patterns

```javascript
// Route parameters
app.get('/users/:id', (req, res) => {
    const { id } = req.params;
    res.json({ userId: id });
});

// Query parameters
app.get('/search', (req, res) => {
    const { q, page = 1, limit = 10 } = req.query;
    res.json({ query: q, page: +page, limit: +limit });
});

// POST with body
app.post('/users', (req, res) => {
    const { name, email } = req.body;
    
    if (!name || !email) {
        return res.status(400).json({ error: 'Name and email required' });
    }
    
    const user = { id: Date.now(), name, email };
    res.status(201).json(user);
});

// PUT (update)
app.put('/users/:id', (req, res) => {
    const { id } = req.params;
    const updates = req.body;
    res.json({ id, ...updates, updated: true });
});

// DELETE
app.delete('/users/:id', (req, res) => {
    res.status(204).send();
});
```

## Step 4: Middleware Fundamentals

```javascript
// Application-level middleware (runs on every request)
app.use((req, res, next) => {
    req.startTime = Date.now();
    console.log(`${req.method} ${req.path}`);
    next(); // MUST call next() to continue
});

// Response time middleware
app.use((req, res, next) => {
    res.on('finish', () => {
        const duration = Date.now() - req.startTime;
        console.log(`${req.method} ${req.path} ${res.statusCode} ${duration}ms`);
    });
    next();
});

// Route-level middleware
function authenticate(req, res, next) {
    const token = req.headers.authorization;
    if (!token) {
        return res.status(401).json({ error: 'Unauthorized' });
    }
    req.user = verifyToken(token);
    next();
}

// Protected route
app.get('/profile', authenticate, (req, res) => {
    res.json({ user: req.user });
});
```

## Step 5: Router for Modular Routes

Create `src/routes/users.js`:
```javascript
// src/routes/users.js — User routes

import { Router } from 'express';

const router = Router();

const users = [
    { id: 1, name: 'Alice', email: 'alice@example.com' },
    { id: 2, name: 'Bob', email: 'bob@example.com' },
];

router.get('/', (req, res) => {
    res.json(users);
});

router.get('/:id', (req, res) => {
    const user = users.find(u => u.id === +req.params.id);
    if (!user) return res.status(404).json({ error: 'User not found' });
    res.json(user);
});

router.post('/', (req, res) => {
    const user = { id: users.length + 1, ...req.body };
    users.push(user);
    res.status(201).json(user);
});

export default router;
```

Update `src/server.js`:
```javascript
import usersRouter from './routes/users.js';

app.use('/api/users', usersRouter);
```

## Testing the Server

```bash
# GET request
curl http://localhost:3000/

# POST request
curl -X POST http://localhost:3000/api/users \
  -H "Content-Type: application/json" \
  -d '{"name":"Charlie","email":"charlie@example.com"}'

# Health check
curl http://localhost:3000/health
```

## Best Practices Checklist

- [ ] Always use `express.json()` for body parsing
- [ ] Implement health check endpoint
- [ ] Use Router for modular route organization
- [ ] Handle 404 and 500 errors explicitly
- [ ] Use environment variables for configuration
- [ ] Add request logging middleware
- [ ] Use `node --watch` for development

## Cross-References

- See [Routing and Middleware](./02-routing-middleware.md) for advanced patterns
- See [Static Files and API](./03-static-files-api.md) for serving files
- See [Security Best Practices](../21-security-modern/01-security-headers-deps.md) for hardening

## Next Steps

Continue to [Routing and Middleware](./02-routing-middleware.md) for advanced Express patterns.
