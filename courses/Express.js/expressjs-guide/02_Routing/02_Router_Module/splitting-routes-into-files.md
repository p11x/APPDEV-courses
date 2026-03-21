# Splitting Routes into Files

## 📌 What You'll Learn
- How to split routes into separate files
- Organizing routes by resource
- Keeping your main app file clean

## 🧠 Concept Explained (Plain English)

As your application grows, having all routes in one file becomes messy and hard to manage. Splitting routes into separate files makes your code more organized and easier to navigate.

Think of it like organizing a library. Instead of putting all books in one big pile, you organize them by genre, author, or topic. Similarly, you can organize your routes by resource (users, products, orders) or by feature (authentication, admin, public).

Each route file becomes a module that handles a specific part of your application. Your main app file then simply imports and uses these modules.

## 💻 Code Example

### Step 1: Create a Route File for Users

```javascript
// routes/userRoutes.js
// This file handles all user-related routes

import express from 'express';
const router = express.Router();

// GET /api/users - Get all users
router.get('/', (req, res) => {
    res.json({ users: ['Alice', 'Bob', 'Charlie'] });
});

// GET /api/users/:id - Get single user
router.get('/:id', (req, res) => {
    const userId = req.params.id;
    res.json({ userId, message: `User ${userId} details` });
});

// POST /api/users - Create new user
router.post('/', (req, res) => {
    res.status(201).json({ message: 'User created' });
});

// PUT /api/users/:id - Update user
router.put('/:id', (req, res) => {
    res.json({ message: `User ${req.params.id} updated` });
});

// DELETE /api/users/:id - Delete user
router.delete('/:id', (req, res) => {
    res.json({ message: `User ${req.params.id} deleted` });
});

export default router;
```

### Step 2: Create a Route File for Products

```javascript
// routes/productRoutes.js
// This file handles all product-related routes

import express from 'express';
const router = express.Router();

// GET /api/products - Get all products
router.get('/', (req, res) => {
    res.json({ products: ['Laptop', 'Phone', 'Tablet'] });
});

// GET /api/products/:id - Get single product
router.get('/:id', (req, res) => {
    const productId = req.params.id;
    res.json({ productId, message: `Product ${productId} details` });
});

// POST /api/products - Create new product
router.post('/', (req, res) => {
    res.status(201).json({ message: 'Product created' });
});

// ... other product routes

export default router;
```

### Step 3: Use the Route Files in Your Main App

```javascript
// server.js
import express from 'express';
import userRoutes from './routes/userRoutes.js';
import productRoutes from './routes/productRoutes.js';

const app = express();

app.use(express.json());

// Mount the route files at specific paths
app.use('/api/users', userRoutes);
app.use('/api/products', productRoutes);

// Root route
app.get('/', (req, res) => {
    res.json({ message: 'Welcome to the API' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## Benefits of Splitting Routes

| Benefit | Description |
|---------|-------------|
| **Organization** | Each file has a clear purpose |
| **Maintainability** | Easier to find and modify routes |
| **Collaboration** | Team members can work on different files |
| **Reusability** | Route files can be reused in other projects |

## ⚠️ Common Mistakes

**1. Forgetting to export the router**
Each route file must export the router instance.

**2. Incorrect mounting path**
Make sure the path in `app.use()` matches what you want.

**3. Not handling errors in route files**
Remember to handle errors or pass them to the main error handler.

## ✅ Quick Recap

- Split routes by resource or feature
- Each route file exports a router instance
- Mount routers in your main app with `app.use()`
- Keeps your code organized and maintainable

## 🔗 What's Next

Let's learn how to mount routers at different paths.
