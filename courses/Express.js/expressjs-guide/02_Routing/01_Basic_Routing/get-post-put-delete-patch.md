# GET, POST, PUT, DELETE, PATCH Methods

## 📌 What You'll Learn
- The five main HTTP methods
- When to use each one
- How to implement them in Express

## 🧠 Concept Explained (Plain English)

HTTP methods (also called HTTP verbs) tell the server what kind of action the client wants to perform. Each method has a specific purpose, like reading data, creating something new, or deleting information.

Think of it like ordering at a restaurant:
- **GET** is like looking at the menu (just viewing, nothing changes)
- **POST** is like placing a new order (creating something new)
- **PUT** is like replacing your entire order (complete replacement)
- **PATCH** is like asking to change one item in your order (partial update)
- **DELETE** is like canceling an order (removing something)

RESTful APIs use these methods consistently to make applications predictable and easy to understand.

## 💻 Complete CRUD Example

```javascript
// ES Module - HTTP Methods Example

import express from 'express';

const app = express();

// Middleware to parse JSON bodies
// Required for POST, PUT, PATCH to read request body
app.use(express.json());

// ========================================
// Route Table
// ========================================
// | Method  | URL          | Description              |
// |---------|--------------|--------------------------|
// | GET    | /items       | Get all items           |
// | GET    | /items/:id   | Get single item         |
// | POST   | /items       | Create new item         |
// | PUT    | /items/:id   | Replace item            |
// | PATCH  | /items/:id   | Update part of item     |
// | DELETE | /items/:id   | Delete item             |
// ========================================

// Mock database (in-memory)
let items = [
    { id: 1, name: 'Apple', price: 1.50 },
    { id: 2, name: 'Banana', price: 0.75 }
];

// GET /items - Retrieve all items
// GET is for reading data without modifying anything
app.get('/items', (req, res) => {
    // req = request object (information from client)
    // res = response object (what we send back)
    res.json({
        success: true,
        count: items.length,
        data: items
    });
});

// GET /items/:id - Retrieve single item
// :id is a route parameter (dynamic part)
app.get('/items/:id', (req, res) => {
    const id = parseInt(req.params.id);  // Get id from URL
    const item = items.find(i => i.id === id);  // Find in array
    
    if (!item) {
        return res.status(404).json({ error: 'Item not found' });
    }
    
    res.json({ success: true, data: item });
});

// POST /items - Create new item
// POST is for creating new resources
app.post('/items', (req, res) => {
    // req.body contains the JSON data sent by client
    const { name, price } = req.body;
    
    if (!name || !price) {
        return res.status(400).json({ error: 'Name and price required' });
    }
    
    const newItem = {
        id: items.length + 1,
        name,
        price
    };
    
    items.push(newItem);
    
    // 201 = Created (resource was successfully created)
    res.status(201).json({
        success: true,
        message: 'Item created',
        data: newItem
    });
});

// PUT /items/:id - Replace entire item
// PUT replaces the entire resource
app.put('/items/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const index = items.findIndex(i => i.id === id);
    
    if (index === -1) {
        return res.status(404).json({ error: 'Item not found' });
    }
    
    // Replace entire item with new data
    items[index] = {
        id,
        ...req.body  // All new properties
    };
    
    res.json({
        success: true,
        message: 'Item replaced',
        data: items[index]
    });
});

// PATCH /items/:id - Update part of item
// PATCH updates only specific fields
app.patch('/items/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const index = items.findIndex(i => i.id === id);
    
    if (index === -1) {
        return res.status(404).json({ error: 'Item not found' });
    }
    
    // Merge existing with updates (partial update)
    items[index] = {
        ...items[index],
        ...req.body  // Only update provided fields
    };
    
    res.json({
        success: true,
        message: 'Item updated',
        data: items[index]
    });
});

// DELETE /items/:id - Delete item
// DELETE removes a resource
app.delete('/items/:id', (req, res) => {
    const id = parseInt(req.params.id);
    const index = items.findIndex(i => i.id === id);
    
    if (index === -1) {
        return res.status(404).json({ error: 'Item not found' });
    }
    
    // Remove from array
    items.splice(index, 1);
    
    // 204 = No Content (successful, nothing to return)
    res.status(204).send();
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## HTTP Status Codes

| Code | Meaning | When to Use |
|------|---------|-------------|
| 200 | OK | Successful GET, PUT, PATCH |
| 201 | Created | Successful POST |
| 204 | No Content | Successful DELETE |
| 400 | Bad Request | Invalid client data |
| 404 | Not Found | Resource doesn't exist |
| 500 | Server Error | Something went wrong |

## Testing with curl

```bash
# GET all items
curl http://localhost:3000/items

# GET single item
curl http://localhost:3000/items/1

# POST (create) item
curl -X POST http://localhost:3000/items \
  -H "Content-Type: application/json" \
  -d '{"name": "Orange", "price": 2.00}'

# PUT (replace) item
curl -X PUT http://localhost:3000/items/1 \
  -H "Content-Type: application/json" \
  -d '{"name": "Green Apple", "price": 1.75}'

# PATCH (update) item
curl -X PATCH http://localhost:3000/items/1 \
  -H "Content-Type: application/json" \
  -d '{"price": 1.25}'

# DELETE item
curl -X DELETE http://localhost:3000/items/1
```

## ⚠️ Common Mistakes

**1. Not using express.json()**
Without this middleware, req.body is undefined for POST/PUT/PATCH requests.

**2. Using PUT when PATCH is appropriate**
PUT replaces the entire resource; PATCH updates only what you provide.

**3. Not returning appropriate status codes**
Always return 201 for created, 404 for not found, etc.

## ✅ Quick Recap

- GET = read, POST = create, PUT = replace, PATCH = update, DELETE = delete
- Use express.json() middleware to parse request bodies
- Return appropriate HTTP status codes
- Route parameters (':id') capture dynamic URL parts

## 🔗 What's Next

Let's explore route parameters in more detail.
