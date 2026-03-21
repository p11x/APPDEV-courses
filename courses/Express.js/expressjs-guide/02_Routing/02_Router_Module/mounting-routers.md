# Mounting Routers

## 📌 What You'll Learn
- How to mount routers in your Express application
- Using different path prefixes for routers
- Mounting multiple routers

## 🧠 Concept Explained (Plain English)

After creating route files (routers), you need to connect them to your main Express application. This process is called **mounting**. 

Think of it like adding new roads to a city map. You've built the roads (your route files), but now you need to connect them to the existing road network (your main app) at specific intersections (mount points).

When you mount a router, you specify a base path. All routes in that router will be prefixed with that base path.

## 💻 Code Example

### Step 1: Create Router Files

```javascript
// routes/userRoutes.js
import express from 'express';
const router = express.Router();

router.get('/', (req, res) => {
    res.json({ message: 'Users list' });
});

router.get('/:id', (req, res) => {
    res.json({ userId: req.params.id });
});

export default router;
```

```javascript
// routes/productRoutes.js
import express from 'express';
const router = express.Router();

router.get('/', (req, res) => {
    res.json({ message: 'Products list' });
});

router.get('/:id', (req, res) => {
    res.json({ productId: req.params.id });
});

export default router;
```

### Step 2: Mount Routers in Main App

```javascript
// server.js
import express from 'express';
import userRoutes from './routes/userRoutes.js';
import productRoutes from './routes/productRoutes.js';

const app = express();

app.use(express.json());

// ========================================
// MOUNTING ROUTERS
// ========================================
// Mount userRoutes at /api/users
// All routes in userRoutes will be prefixed with /api/users
app.use('/api/users', userRoutes);

// Mount productRoutes at /api/products
// All routes in productRoutes will be prefixed with /api/products
app.use('/api/products', productRoutes);

// Root route
app.get('/', (req, res) => {
    res.json({ message: 'Welcome to the API' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## What Happens When You Mount

With the above setup:
- `GET /api/users` → handled by userRoutes router
- `GET /api/users/123` → handled by userRoutes router
- `GET /api/products` → handled by productRoutes router
- `GET /api/products/456` → handled by productRoutes router

## Mounting at Root

You can also mount a router at the root path:

```javascript
// Mount at root - no prefix
app.use('/', userRoutes);
// Now:
// GET / → handled by userRoutes
// GET /123 → handled by userRoutes
```

## Multiple Mount Points

You can mount the same router at multiple paths:

```javascript
// Mount the same router in two places
app.use('/api/v1/users', userRoutes);
app.use('/api/v2/users', userRoutes);
// Now userRoutes is accessible at both /api/v1/users and /api/v2/users
```

## ⚠️ Common Mistakes

**1. Forgetting the leading slash**
When mounting, the path should start with a slash: `app.use('/api/users', router)`

**2. Mounting order matters**
If you have overlapping paths, the first match wins.

**3. Not exporting the router**
Each route file must export the router instance.

## ✅ Quick Recap

- Use `app.use(path, router)` to mount a router
- The path becomes a prefix for all routes in the router
- Mount multiple routers to organize your application
- Order of mounting matters for overlapping paths

## 🔗 What's Next

Let's learn about nested routers.
