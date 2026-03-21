# What is REST?

## 📌 What You'll Learn
- What REST (Representational State Transfer) means
- The key principles of RESTful API design
- HTTP methods and their meanings in REST
- Common REST conventions and best practices

## 🧠 Concept Explained (Plain English)

REST is like a standardized language for communication between computers. Imagine if every company had its own unique way of sending letters - some might use Carrier Pigeons, some might use smoke signals, some might use emails. It would be chaos! REST is like saying "everyone should use the same postal system" - a common set of rules that everyone agrees on.

In technical terms, REST is an architectural style for designing networked applications. It relies on a stateless communication protocol (usually HTTP) and defines specific ways to interact with resources.

The key ideas:
- **Resources**: Everything is a "resource" - a user, a product, an order, etc.
- **Representations**: Resources can be represented in different formats (JSON, XML, etc.)
- **Stateless**: Each request contains all information needed - no server memory of previous requests
- **Uniform Interface**: Consistent way to interact with resources

REST APIs are the most common type of APIs today. When you see "REST API" or "RESTful API", it means an API that follows REST principles.

## 💻 Code Example

```javascript
// ES Module - REST API Example

import express from 'express';

const app = express();

// Need to parse JSON bodies for POST/PUT requests
app.use(express.json());

// ========================================
// UNDERSTANDING REST RESOURCES AND METHODS
// ========================================

// In REST, URLs represent RESOURCES (nouns), not actions (verbs)

// Resource: /users
// This is a collection of users

// ========================================
// HTTP METHODS AND THEIR MEANINGS
// ========================================

// GET    - Retrieve/Read a resource
// POST   - Create a new resource  
// PUT    - Replace/Update an entire resource
// PATCH  - Partially update a resource
// DELETE - Remove a resource

// ========================================
// EXAMPLE: USERS RESOURCE
// ========================================

// Mock database (in real app, this would be a database)
const users = [
    { id: 1, name: 'Alice', email: 'alice@example.com' },
    { id: 2, name: 'Bob', email: 'bob@example.com' }
];

// GET /users - Get all users
app.get('/users', (req, res) => {
    // Return the list of all users
    res.json(users);
});

// GET /users/:id - Get a specific user by ID
app.get('/users/:id', (req, res) => {
    // req.params.id comes from the URL
    const userId = parseInt(req.params.id);
    const user = users.find(u => u.id === userId);
    
    if (!user) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    res.json(user);
});

// POST /users - Create a new user
app.post('/users', (req, res) => {
    // req.body contains the JSON data sent by the client
    const { name, email } = req.body;
    
    // Validate required fields
    if (!name || !email) {
        return res.status(400).json({ error: 'Name and email are required' });
    }
    
    // Create new user
    const newUser = {
        id: users.length + 1,
        name,
        email
    };
    
    // Add to our "database"
    users.push(newUser);
    
    // Return 201 Created with the new resource
    res.status(201).json(newUser);
});

// PUT /users/:id - Replace an entire user
app.put('/users/:id', (req, res) => {
    const userId = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === userId);
    
    if (userIndex === -1) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    const { name, email } = req.body;
    
    // Replace entire user (all fields required)
    users[userIndex] = {
        id: userId,
        name: name || users[userIndex].name,
        email: email || users[userIndex].email
    };
    
    res.json(users[userIndex]);
});

// PATCH /users/:id - Partially update a user
app.patch('/users/:id', (req, res) => {
    const userId = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === userId);
    
    if (userIndex === -1) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    // Only update fields that are provided
    const { name, email } = req.body;
    
    if (name) users[userIndex].name = name;
    if (email) users[userIndex].email = email;
    
    res.json(users[userIndex]);
});

// DELETE /users/:id - Delete a user
app.delete('/users/:id', (req, res) => {
    const userId = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === userId);
    
    if (userIndex === -1) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    // Remove user from array
    users.splice(userIndex, 1);
    
    // Return 204 No Content for successful deletion
    res.status(204).send();
});

// ========================================
// HTTP STATUS CODES IN REST
// ========================================

/*
Status Code    Meaning
-------------  -------------------------------------------------
200 OK         Request succeeded (GET, PUT, PATCH)
201 Created    Resource was successfully created (POST)
204 No Content Request succeeded but no content to return (DELETE)
400 Bad Request Client sent invalid data
401 Unauthorized Authentication required
403 Forbidden   No permission to access resource
404 Not Found   Resource doesn't exist
500 Server Error Something went wrong on the server
*/

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`REST API running on port ${PORT}`);
});
```

## 🔍 Key REST Concepts

| Concept | Description |
|---------|-------------|
| Resource | A thing you can interact with (user, product, order) |
| Endpoint | URL path that represents a resource (/users, /products) |
| Method | HTTP verb that defines the action (GET, POST, PUT, DELETE) |
| Representation | Format of the data (usually JSON) |
| Stateless | Each request contains all information needed |

## ⚠️ Common Mistakes

**1. Using verbs in URLs**
Don't do: `/getUsers`, `/createUser`, `/deleteUser`
Do: `GET /users`, `POST /users`, `DELETE /users/:id`

**2. Using wrong HTTP methods**
GET for reading, POST for creating, PUT for replacing, PATCH for updating, DELETE for deleting. Don't use GET to modify data.

**3. Not returning appropriate status codes**
Always return the correct HTTP status code (200, 201, 404, etc.) to help clients understand what happened.

**4. Not handling errors**
Always handle errors and return appropriate error responses with status codes.

**5. Inconsistent URLs**
Be consistent: if you use `/users/:id`, don't also use `/user/:id` somewhere else.

## ✅ Quick Recap

- REST is an architectural style for designing APIs
- Resources are represented by URLs (/users, /products)
- HTTP methods define actions: GET (read), POST (create), PUT (replace), PATCH (update), DELETE (remove)
- Use appropriate HTTP status codes in responses
- URLs should be nouns (resources), not verbs (actions)
- REST APIs are stateless - each request is independent

## 🔗 What's Next

Let's look at how to structure a REST API properly, including organizing routes and handling different types of requests.
