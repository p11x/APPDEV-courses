# Structuring a REST API

## 📌 What You'll Learn
- How to organize routes for REST APIs
- Using Express Router for modular route files
- Nesting resources for related data
- Best practices for API structure

## 🧠 Concept Explained (Plain English)

As your application grows, putting all your routes in a single file becomes unmanageable. It's like trying to keep all your clothes in one drawer - everything gets tangled and hard to find!

The solution is to organize your routes into separate files, each handling a specific resource. Express provides a tool called "Router" that lets you create modular, mountable route handlers. Think of it like having different drawers for different types of clothes - shirts in one, pants in another, socks in another.

A well-structured REST API typically has:
- Separate route files for each resource (users, products, orders)
- A main app file that combines everything
- Nested routes for related resources (like orders for a specific user)
- Consistent URL patterns

This makes your code:
- Easier to maintain
- Easier to test
- Easier to collaborate on
- More scalable

## 💻 Code Example

```javascript
// ES Module - Structuring a REST API

import express from 'express';

// Main app file - brings everything together

const app = express();

// Parse JSON bodies
app.use(express.json());

// ========================================
// IMPORT ROUTERS (MODULAR ROUTE FILES)
// ========================================

// In a real app, these would be in separate files
// import usersRouter from './routes/users.js';
// import productsRouter from './routes/products.js';
// import ordersRouter from './routes/orders.js';

// For this example, we'll create the routers inline
// but show how they'd be structured in separate files

// ========================================
// CREATING A USER ROUTER
// ========================================

// users.js would look like this:
/*
import express from 'express';
const router = express.Router();

// Get all users - GET /users
router.get('/', (req, res) => {
    res.json([{ id: 1, name: 'Alice' }]);
});

// Get user by ID - GET /users/:id
router.get('/:id', (req, res) => {
    res.json({ id: req.params.id, name: 'Alice' });
});

// Create user - POST /users
router.post('/', (req, res) => {
    const newUser = { id: 3, ...req.body };
    res.status(201).json(newUser);
});

export default router;
*/

// Simulated router for users
const usersRouter = express.Router();
usersRouter.get('/', (req, res) => res.json([{ id: 1, name: 'Alice' }]));
usersRouter.get('/:id', (req, res) => res.json({ id: req.params.id, name: 'Alice' }));
usersRouter.post('/', (req, res) => res.status(201).json({ id: 3, ...req.body }));

// ========================================
// CREATING A PRODUCTS ROUTER
// ========================================

const productsRouter = express.Router();
productsRouter.get('/', (req, res) => res.json([{ id: 1, name: 'Widget', price: 9.99 }]));
productsRouter.get('/:id', (req, res) => res.json({ id: req.params.id, name: 'Widget', price: 9.99 }));
productsRouter.post('/', (req, res) => res.status(201).json({ id: 3, ...req.body }));

// ========================================
// CREATING NESTED ROUTES (e.g., /users/:userId/orders)
// ========================================

const ordersRouter = express.Router();

// Get orders for a user - GET /users/:userId/orders
ordersRouter.get('/users/:userId/orders', (req, res) => {
    const userId = req.params.userId;
    // In real app, fetch orders for this user from database
    res.json([
        { id: 1, userId, product: 'Widget', quantity: 2 }
    ]);
});

// Create order for user - POST /users/:userId/orders
ordersRouter.post('/users/:userId/orders', (req, res) => {
    const userId = req.params.userId;
    const { productId, quantity } = req.body;
    res.status(201).json({ 
        id: 3, 
        userId, 
        productId, 
        quantity 
    });
});

// ========================================
// MOUNTING THE ROUTERS
// ========================================

// Mount routers at specific paths
// All routes in usersRouter will start with /users
app.use('/users', usersRouter);

// All routes in productsRouter will start with /products
app.use('/products', productsRouter);

// Mount orders router - note it handles nested paths
app.use('/', ordersRouter);

// ========================================
// API VERSIONING (OPTIONAL BUT RECOMMENDED)
// ========================================

// Common pattern: prefix API routes with version number
// This allows you to make breaking changes without affecting old clients

// const v1Router = express.Router();
// v1Router.use('/users', usersRouter);
// v1Router.use('/products', productsRouter);
// app.use('/api/v1', v1Router);

// Another approach: use header for versioning
// app.get('/users', (req, res) => {
//     const version = req.headers['api-version'];
//     // Return different format based on version
// });

// ========================================
// ERROR HANDLING
// ========================================

// eslint-disable-next-line no-unused-vars
app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).json({ error: 'Something went wrong!' });
});

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`API server running on port ${PORT}`);
});

/*
// ========================================
// COMPLETE FOLDER STRUCTURE
// ========================================
// project/
// ├── app.js                    (Main Express app)
// ├── routes/
// │   ├── users.js              (User routes)
// │   ├── products.js           (Product routes)
// │   └── orders.js             (Order routes)
// ├── controllers/
// │   ├── usersController.js    (User business logic)
// │   └── ...
// ├── models/
// │   └── ...
// └── middleware/
//     └── ...
*/
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 15 | `import express from 'express';` | Import Express framework |
| 42 | `const usersRouter = express.Router();` | Create a new router instance |
| 47 | `usersRouter.get('/', ...)` | Define GET /users route |
| 48 | `usersRouter.get('/:id', ...)` | Define GET /users/:id route |
| 49 | `usersRouter.post('/', ...)` | Define POST /users route |
| 78 | `app.use('/users', usersRouter);` | Mount users router at /users path |
| 79 | `app.use('/products', productsRouter);` | Mount products router |
| 83 | `ordersRouter.get('/users/:userId/orders', ...)` | Nested route example |
| 103 | `app.use('/api/v1', v1Router);` | Versioned API example |

## ⚠️ Common Mistakes

**1. Not separating concerns**
Keep routes, controllers, and models separate. Routes should only handle HTTP stuff (parsing, responses), not business logic.

**2. Forgetting to mount routers**
You must use `app.use('/path', router)` to mount a router. Without it, the routes won't be accessible.

**3. Not handling nested resources properly**
If you have `/users/:userId/orders`, make sure the userId parameter makes sense in context.

**4. Not version your API**
Without versioning, changes to your API can break existing clients. Use `/api/v1/` prefix.

**5. Putting everything in one file**
As your API grows, keep related routes in separate files. It'll be much easier to maintain.

## ✅ Quick Recap

- Use `express.Router()` to create modular route handlers
- Keep related routes in separate files
- Mount routers at specific paths with `app.use('/path', router)`
- Use nested routes for related resources (e.g., `/users/:userId/orders`)
- Consider API versioning (`/api/v1/`) for stability
- Organize your project with routes, controllers, models, and middleware folders

## 🔗 What's Next

Let's look at handling different types of requests in REST APIs, including query parameters, pagination, and filtering.
