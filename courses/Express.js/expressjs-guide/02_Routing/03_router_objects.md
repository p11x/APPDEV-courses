# Router Objects in Express.js

## What is express.Router()?

**express.Router()** creates a modular, mountable route handler. Think of it as a "mini Express app" that you can plug into your main application.

Routers help you:
- Organize routes into logical groups
- Create modular, reusable code
- Keep your main app.js clean

## Why Use Routers?

Instead of putting all routes in one file, you can split them:

```
Before (all in app.js):
├── app.get('/users', ...)
├── app.get('/users/:id', ...)
├── app.post('/users', ...)
├── app.get('/products', ...)
├── app.get('/products/:id', ...)
└── app.post('/products', ...)

After (modular with Router):
├── routes/users.js    → handles /users
├── routes/products.js → handles /products
└── app.js             → just mounts the routers
```

## Creating a Router

### Step 1: Create a Route File

```javascript
// routes/users.js
// Users route module

import express from 'express';
// Router() creates a new router object
// It's like a mini Express app with its own routes
const router = express.Router();

import { 
    getUsers, 
    getUserById, 
    createUser, 
    updateUser, 
    deleteUser 
} from '../controllers/userController.js';

// ============================================
// Route Table
// ============================================
// | Method | Path      | Handler         | Description           |
// |--------|-----------|-----------------|-----------------------|
// | GET    | /         | getUsers       | Get all users        |
// | GET    | /:id      | getUserById    | Get user by ID       |
// | POST   | /         | createUser     | Create new user      |
// | PUT    | /:id      | updateUser     | Update user          |
// | DELETE | /:id      | deleteUser     | Delete user          |
// ============================================

// These routes will be prefixed with /users
router.get('/', getUsers);
router.get('/:id', getUserById);
router.post('/', createUser);
router.put('/:id', updateUser);
router.delete('/:id', deleteUser);

// Export the router so it can be used in app.js
export default router;
```

### Step 2: Mount the Router in app.js

```javascript
// app.js
import express from 'express';
const app = express();

// Import the users router
import usersRouter from './routes/users.js';
// Import the products router
import productsRouter from './routes/products.js';

// Parse JSON bodies
app.use(express.json());

// Mount routers at specific paths
// All routes in usersRouter will start with /users
app.use('/users', usersRouter);

// All routes in productsRouter will start with /products
app.use('/products', productsRouter);

// Root route
app.get('/', (req, res) => {
    res.json({ message: 'Welcome to the API' });
});

export default app;
```

## Multiple Routers Example

Let's create a complete example with multiple routers:

### File: routes/api.js

```javascript
// routes/api.js
// API routes module

import express from 'express';
const router = express.Router();

// Mock data
let posts = [
    { id: 1, title: 'Hello World', content: 'My first post!' },
    { id: 2, title: 'Express Rules', content: 'Building APIs is easy' }
];

// GET /api/posts - Get all posts
router.get('/posts', (req, res) => {
    res.json({ data: posts });
});

// GET /api/posts/:id - Get single post
router.get('/posts/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const post = posts.find(p => p.id === id);
    
    if (!post) {
        return res.status(404).json({ message: 'Post not found' });
    }
    
    res.json({ data: post });
});

// POST /api/posts - Create post
router.post('/posts', (req, res) => {
    const newPost = {
        id: posts.length + 1,
        ...req.body
    };
    posts.push(newPost);
    res.status(201).json({ data: newPost });
});

export default router;
```

### File: routes/auth.js

```javascript
// routes/auth.js
// Authentication routes module

import express from 'express';
const router = express.Router();

// Mock users database
const users = [
    { id: 1, email: 'alice@example.com', password: 'password123' }
];

// POST /auth/register - Register new user
router.post('/register', (req, res) => {
    const { email, password } = req.body;
    
    // Check if user exists
    if (users.find(u => u.email === email)) {
        return res.status(400).json({ message: 'Email already exists' });
    }
    
    const newUser = {
        id: users.length + 1,
        email,
        password // In real apps, ALWAYS hash passwords!
    };
    
    users.push(newUser);
    res.status(201).json({ message: 'User registered', userId: newUser.id });
});

// POST /auth/login - Login user
router.post('/login', (req, res) => {
    const { email, password } = req.body;
    
    const user = users.find(u => u.email === email && u.password === password);
    
    if (!user) {
        return res.status(401).json({ message: 'Invalid credentials' });
    }
    
    res.json({ message: 'Login successful', token: 'fake-jwt-token' });
});

export default router;
```

### File: server.js

```javascript
// server.js
import express from 'express';
import apiRouter from './routes/api.js';
import authRouter from './routes/auth.js';

const app = express();

// Parse JSON
app.use(express.json());

// Mount routers
// /api/* routes handled by apiRouter
app.use('/api', apiRouter);

// /auth/* routes handled by authRouter
app.use('/auth', authRouter);

// Root route
app.get('/', (req, res) => {
    res.json({ 
        message: 'API is running',
        endpoints: {
            posts: '/api/posts',
            auth: '/auth/login'
        }
    });
});

// Start server
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

## Nested Routers

You can also nest routers within routers:

```javascript
// routes/admin.js
import express from 'express';
const router = express.Router();

// Admin routes will be at /admin/*
router.get('/', (req, res) => {
    res.json({ message: 'Admin panel' });
});

// Nested router for users
import adminUsersRouter from './adminUsers.js';
router.use('/users', adminUsersRouter);

export default router;

// routes/adminUsers.js
import express from 'express';
const router = express.Router();

// This route will be at /admin/users
router.get('/', (req, res) => {
    res.json({ message: 'Admin users list' });
});

export default router;
```

## Benefits of Using Router

| Benefit | Description |
|---------|-------------|
| **Organization** | Related routes are grouped together |
| **Reusability** | Routers can be shared across projects |
| **Clean Code** | Main app stays simple |
| **Prefix Routes** | Easily add URL prefixes |
| **Testing** | Routes can be tested independently |

## Best Practices

1. **One router per resource**: Create separate routers for users, products, posts, etc.
2. **Use meaningful names**: Name files after what they route (e.g., `userRoutes.js`)
3. **Export default**: Always use `export default router`
4. **Keep controllers separate**: Route files should only map URLs to handlers

## What's Next?

- **[Route Matching](./04_route_matching.md)** — Advanced routing with regex and patterns
- **[Middleware](../03_Middleware/01_introduction.md)** — Adding functionality to requests
