# Express Router

## 📌 What You'll Learn
- What Express Router is
- How to create modular routes
- Benefits of using routers

## 🧠 Concept Explained (Plain English)

**Express Router** is a mini Express app that lets you organize routes into separate files. Instead of having all routes in one massive file, you can split them into logical groups.

Think of it like organizing a大型办公室. Instead of one person handling all tasks, you have different departments (HR, IT, Sales). Each department handles its own work, and there's a main reception that directs people. Express Router works the same way — you have mini-apps for different route groups.

## 💻 Code Example

```javascript
// ES Module - Express Router

import express from 'express';
const router = express.Router();

// ========================================
// Router is like a mini Express app
// ========================================
// You can define routes, middleware, etc.
// Then mount it in your main app

// Define routes on the router
router.get('/', (req, res) => {
    res.json({ message: 'Users API' });
});

router.get('/:id', (req, res) => {
    res.json({ userId: req.params.id });
});

router.post('/', (req, res) => {
    res.status(201).json({ message: 'User created' });
});

export default router;
```

## Mounting in Main App

```javascript
// server.js

import express from 'express';
import userRoutes from './routes/userRoutes.js';
import productRoutes from './routes/productRoutes.js';

const app = express();

app.use(express.json());

// Mount routers at different paths
app.use('/api/users', userRoutes);
app.use('/api/products', productRoutes);

app.listen(3000, () => console.log('Server running'));
```

## Benefits

- **Modular** - Routes in separate files
- **Reusable** - Can mount multiple times with different prefixes
- **Clean** - Keeps main app simple

## ⚠️ Common Mistakes

- Forgetting to export the router
- Not mounting the router in main app

## ✅ Quick Recap

- Express Router creates modular route handlers
- Use `router.get()`, `router.post()`, etc.
- Mount with `app.use('/path', router)`
