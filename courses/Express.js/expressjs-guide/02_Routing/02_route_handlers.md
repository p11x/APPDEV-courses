# Route Handlers in Express.js

## What are Route Handlers?

**Route handlers** are functions that contain the logic for processing requests. When a route matches, Express calls these functions to handle the request and send a response.

Think of route handlers as the "workers" who actually do the work — they receive requests, process them, and return results.

## Basic Handler Structure

A route handler receives three parameters:

```javascript
app.get('/path', (req, res, next) => {
    // req  = request object (what the client sent)
    // res  = response object (what we send back)
    // next = function to pass control to next handler
});
```

## Handling Different HTTP Methods

Here's a complete example showing all major HTTP methods:

```javascript
// server.js
import express from 'express';

const app = express();
// 'app' is our Express application instance

// Middleware to parse JSON bodies
app.use(express.json());

// ============================================
// Route Handler Table
// ============================================
// | Method | Path      | Handler Function    | Purpose               |
// |--------|-----------|---------------------|----------------------|
// | GET    | /items    | getAllItems        | Retrieve all items   |
// | GET    | /items/:id| getItemById         | Get single item      |
// | POST   | /items    | createItem         | Create new item      |
// | PUT    | /items/:id| updateItem         | Update entire item   |
// | PATCH  | /items/:id| patchItem          | Partial update       |
// | DELETE | /items/:id| deleteItem         | Delete item          |
// ============================================

// Mock database (array)
let items = [
    { id: 1, name: 'Apple', price: 1.50 },
    { id: 2, name: 'Banana', price: 0.75 }
];

// GET /items - Get all items
// Async handler for fetching data
const getAllItems = async (req, res) => {
    // In a real app, this would fetch from a database
    // 'async/await' lets us write asynchronous code that looks synchronous
    // It makes handling database calls much cleaner than callbacks
    res.json({
        success: true,
        count: items.length,
        data: items
    });
};

// GET /items/:id - Get single item by ID
const getItemById = async (req, res) => {
    // req.params captures the :id from the URL
    const id = parseInt(req.params.id);
    
    // Find item in our array
    const item = items.find(i => i.id === id);
    
    if (!item) {
        // Return 404 if not found
        return res.status(404).json({
            success: false,
            message: 'Item not found'
        });
    }
    
    res.json({ success: true, data: item });
};

// POST /items - Create new item
// In Express 5, async handlers work without try/catch!
const createItem = async (req, res) => {
    // req.body contains the JSON data sent in the request
    const { name, price } = req.body;
    
    // Validate input
    if (!name || !price) {
        return res.status(400).json({
            success: false,
            message: 'Please provide name and price'
        });
    }
    
    // Create new item
    const newItem = {
        id: items.length + 1,
        name,
        price
    };
    
    items.push(newItem);
    
    // Return 201 (Created) with the new item
    res.status(201).json({
        success: true,
        message: 'Item created',
        data: newItem
    });
};

// PUT /items/:id - Update entire item
const updateItem = async (req, res) => {
    const id = parseInt(req.params.id);
    const index = items.findIndex(i => i.id === id);
    
    if (index === -1) {
        return res.status(404).json({
            success: false,
            message: 'Item not found'
        });
    }
    
    // Update with new data (replace entire item)
    items[index] = { id, ...req.body };
    
    res.json({
        success: true,
        message: 'Item updated',
        data: items[index]
    });
};

// PATCH /items/:id - Partial update
const patchItem = async (req, res) => {
    const id = parseInt(req.params.id);
    const index = items.findIndex(i => i.id === id);
    
    if (index === -1) {
        return res.status(404).json({
            success: false,
            message: 'Item not found'
        });
    }
    
    // Only update provided fields (merge with existing)
    items[index] = { ...items[index], ...req.body };
    
    res.json({
        success: true,
        message: 'Item partially updated',
        data: items[index]
    });
};

// DELETE /items/:id - Delete item
const deleteItem = async (req, res) => {
    const id = parseInt(req.params.id);
    const index = items.findIndex(i => i.id === id);
    
    if (index === -1) {
        return res.status(404).json({
            success: false,
            message: 'Item not found'
        });
    }
    
    // Remove item from array
    items.splice(index, 1);
    
    // 204 = No Content (successful but nothing to return)
    res.status(204).send();
};

// Register all routes
app.get('/items', getAllItems);
app.get('/items/:id', getItemById);
app.post('/items', createItem);
app.put('/items/:id', updateItem);
app.patch('/items/:id', patchItem);
app.delete('/items/:id', deleteItem);

// Start server
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Using Controllers

In larger applications, route handlers are often organized into **controllers** — separate files that contain all the logic for a resource:

### File: controllers/itemController.js

```javascript
// controllers/itemController.js
// Controller - handles logic for items

let items = [];

// Get all items
export const getAllItems = async (req, res) => {
    res.json({ data: items });
};

// Get item by ID
export const getItemById = async (req, res) => {
    const id = parseInt(req.params.id);
    const item = items.find(i => i.id === id);
    
    if (!item) {
        return res.status(404).json({ message: 'Item not found' });
    }
    
    res.json({ data: item });
};

// Create item
export const createItem = async (req, res) => {
    const newItem = {
        id: items.length + 1,
        ...req.body
    };
    items.push(newItem);
    res.status(201).json({ data: newItem });
};

// Update item
export const updateItem = async (req, res) => {
    const id = parseInt(req.params.id);
    const index = items.findIndex(i => i.id === id);
    
    if (index === -1) {
        return res.status(404).json({ message: 'Not found' });
    }
    
    items[index] = { id, ...req.body };
    res.json({ data: items[index] });
};

// Delete item
export const deleteItem = async (req, res) => {
    const id = parseInt(req.params.id);
    const index = items.findIndex(i => i.id === id);
    
    if (index === -1) {
        return res.status(404).json({ message: 'Not found' });
    }
    
    items.splice(index, 1);
    res.status(204).send();
};
```

### File: routes/itemRoutes.js

```javascript
// routes/itemRoutes.js
// Routes - connects URLs to controller functions

import express from 'express';
// Router() creates a mini Express app for modular routing
const router = express.Router();
import {
    getAllItems,
    getItemById,
    createItem,
    updateItem,
    deleteItem
} from '../controllers/itemController.js';

// Route table:
// | Method | Path      | Controller        | Description        |
// |--------|-----------|-------------------|--------------------|
// | GET    | /         | getAllItems      | List all items    |
// | GET    | /:id      | getItemById      | Get single item   |
// | POST   | /         | createItem       | Create item       |
// | PUT    | /:id      | updateItem      | Update item       |
// | DELETE | /:id      | deleteItem      | Delete item       |

router.get('/', getAllItems);
router.get('/:id', getItemById);
router.post('/', createItem);
router.put('/:id', updateItem);
router.delete('/:id', deleteItem);

export default router;
```

## Understanding Async/Await

**async/await** is JavaScript's modern way of handling asynchronous operations:

```javascript
// Without async/await (older pattern with callbacks)
app.get('/users', (req, res) => {
    getUsers((err, users) => {
        if (err) {
            return res.status(500).json({ error: err.message });
        }
        res.json(users);
    });
});

// With async/await (modern, cleaner)
app.get('/users', async (req, res) => {
    try {
        const users = await getUsers();
        res.json(users);
    } catch (error) {
        res.status(500).json({ error: error.message });
    }
});

// With Express 5 (even simpler - no try/catch needed!)
app.get('/users', async (req, res) => {
    // Express 5 automatically catches errors and passes to error handler
    const users = await getUsers();
    res.json(users);
});
```

> **Why async/await?**
> - Code is easier to read and understand
> - Error handling is more straightforward
> - It makes asynchronous code look synchronous

## What's Next?

- **[Router Objects](./03_router_objects.md)** — Modular route organization
- **[Route Matching](./04_route_matching.md)** — Advanced patterns like regex
