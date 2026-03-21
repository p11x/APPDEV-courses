# Nested Routers

## 📌 What You'll Learn
- How to create nested routers
- Using routers within routers
- Practical examples of nested routing

## 🧠 Concept Explained (Plain English)

Just like you can have folders within folders, you can have routers within routers. This is useful for organizing related routes in a hierarchical manner.

Think of it like a company organizational chart. You have a main department (router) that contains sub-departments (nested routers). Each sub-department handles its own specific tasks.

In Express, you can mount a router inside another router, allowing you to create a tree-like structure for your routes.

## 💻 Code Example

### Step 1: Create a Main Router for API

```javascript
// routes/api.js
import express from 'express';
const router = express.Router();

// Import nested routers
import userRoutes from './users.js';
import productRoutes from './products.js';

// Mount nested routers
router.use('/users', userRoutes);
router.use('/products', productRoutes);

// Root API route
router.get('/', (req, res) => {
    res.json({ message: 'API v1' });
});

export default router;
```

### Step 2: Create Nested Router for Users

```javascript
// routes/api/users.js
import express from 'express';
const router = express.Router();

// GET /api/users
router.get('/', (req, res) => {
    res.json({ users: ['Alice', 'Bob'] });
});

// GET /api/users/:id
router.get('/:id', (req, res) => {
    res.json({ userId: req.params.id });
});

// Nested router for user posts
import userPostsRouter from './userPosts.js';
router.use('/:userId/posts', userPostsRouter);

export default router;
```

### Step 3: Create Doubly Nested Router for User Posts

```javascript
// routes/api/userPosts.js
import express from 'express';
const router = express.Router();

// GET /api/users/:userId/posts
router.get('/', (req, res) => {
    const userId = req.params.userId;
    res.json({ userId, posts: [] });
});

// POST /api/users/:userId/posts
router.post('/', (req, res) => {
    res.status(201).json({ message: 'Post created' });
});

export default router;
```

### Step 4: Mount the Main API Router in Your App

```javascript
// server.js
import express from 'express';
import apiRouter from './routes/api.js';

const app = express();

app.use(express.json());

// Mount the main API router at /api
app.use('/api', apiRouter);

app.get('/', (req, res) => {
    res.json({ message: 'Welcome to the API' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## Route Structure

With the above setup, you get the following routes:
- GET /api/ → API root
- GET /api/users/ → List users
- GET /api/users/123 → Get user 123
- GET /api/users/123/posts/ → Get posts for user 123
- POST /api/users/123/posts/ → Create post for user 123
- GET /api/products/ → List products
- GET /api/products/456 → Get product 456

## Benefits of Nested Routers

| Benefit | Description |
|---------|-------------|
| **Organization** | Groups related routes together |
| **Reusability** | Nested routers can be reused in different contexts |
| **Clarity** | Makes the route structure obvious |
| **Maintainability** | Easier to locate specific route handlers |

## ⚠️ Common Mistakes

**1. Forgetting to mount the nested router**
You must use `router.use()` to mount a nested router.

**2. Incorrect parameter names**
Make sure parameter names match between the mount point and the nested router.

**3. Over-nesting**
Too many levels of nesting can make the code hard to follow.

## ✅ Quick Recap

- Use `router.use()` to mount a router inside another router
- Nested routers allow hierarchical route organization
- Parameters flow from parent to child routers
- Keep nesting levels reasonable for readability

## 🔗 What's Next

Let's learn about router-level middleware.
