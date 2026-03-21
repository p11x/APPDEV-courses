# req.method and req.path

## 📌 What You'll Learn
- How to access the HTTP method with req.method
- How to access the request path with req.path
- Understanding the difference between req.path and req.url
- Common use cases for method and path

## 🧠 Concept Explained (Plain English)

Every HTTP request has two fundamental pieces of information: the **method** (what action to perform) and the **path** (what resource to act upon).

The method tells the server what the client wants to do: retrieve data (GET), submit data (POST), update data (PUT/PATCH), or delete data (DELETE).

The path tells the server which resource the client is referring to. For example, `/users` refers to the users collection, while `/users/123` refers to a specific user with ID 123.

In Express, `req.method` gives you the HTTP method as a string (like "GET", "POST", etc.), and `req.path` gives you the path portion of the URL (without the query string or domain).

Think of it like ordering at a restaurant:
- The method is like saying "I want to order" (POST) or "I want to see the menu" (GET) or "I want to change my order" (PUT) or "I want to cancel my order" (DELETE).
- The path is like saying "I want the pizza" (path: /menu/pizza) or "I want the salad" (path: /menu/salad).

## 💻 Code Example

```javascript
// ES Module - Accessing Request Method and Path

import express from 'express';

const app = express();

// We'll use express.json() for parsing JSON bodies in other examples, but for this one we focus on method and path
// app.use(express.json());

// ========================================
// VIEW METHOD AND PATH
// ========================================

app.get('/info', (req, res) => {
    // req.method is the HTTP method (GET, POST, PUT, DELETE, etc.)
    // req.path is the path portion of the URL (without query string or domain)
    res.json({
        method: req.method,
        path: req.path,
        message: `You made a ${req.method} request to ${req.path}`
    });
});

// ========================================
// HANDLE DIFFERENT METHODS ON THE SAME PATH
// ========================================

// This route will handle GET, POST, PUT, and DELETE to /api/items
app.all('/api/items', (req, res) => {
    // We can check the method to decide what to do
    switch (req.method) {
        case 'GET':
            res.json({ message: 'Getting items', method: req.method });
            break;
        case 'POST':
            res.status(201).json({ message: 'Creating item', method: req.method });
            break;
        case 'PUT':
            res.json({ message: 'Updating items', method: req.method });
            break;
        case 'DELETE':
            res.status(204).json({ message: 'Deleting items', method: req.method });
            break;
        default:
            res.status(405).json({ error: `Method ${req.method} not allowed` });
    }
});

// ========================================
// PATH-BASED ROUTING (WITHOUT USING app.METHOD())
// ========================================

// You can use req.path to create your own routing logic
app.use('/custom', (req, res) => {
    if (req.path === '/custom/users' && req.method === 'GET') {
        res.json({ users: ['Alice', 'Bob'] });
    } else if (req.path === '/custom/users' && req.method === 'POST') {
        res.status(201).json({ message: 'User created' });
    } else if (req.path.startsWith('/custom/users/') && req.method === 'GET') {
        // Extract ID from path: /custom/users/123
        const id = req.path.split('/').pop();
        res.json({ userId: id, message: `Fetching user ${id}` });
    } else {
        res.status(404).json({ error: 'Not found' });
    }
});

// ========================================
// UNDERSTANDING req.path vs req.url
// ========================================

app.get('/path-info', (req, res) => {
    // req.path is the path portion of the URL (without query string)
    // req.url is the full URL path and query string (without domain and protocol)
    res.json({
        path: req.path,          // e.g., "/path-info"
        url: req.url,            // e.g., "/path-info?foo=bar"
        query: req.query,        // e.g., { foo: "bar" }
        explanation: {
            path: "The path portion of the URL (matches what you define in app.get(), etc.)",
            url: "The full URL path and query string (what comes after the domain and protocol)",
            query: "The parsed query string parameters"
        }
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `app.get('/info', (req, res) => {` | Defines a GET route for /info |
| 11 | `method: req.method,` | Gets the HTTP method |
| 12 | `path: req.path,` | Gets the path portion of the URL |
| 16 | `app.all('/api/items', (req, res) => {` | Defines a route that handles all HTTP methods |
| 19 | `switch (req.method) {` | Starts a switch statement on the HTTP method |
| 22 | `case 'GET':` | Handles GET requests |
| 25 | `case 'POST':` | Handles POST requests |
| 28 | `case 'PUT':` | Handles PUT requests |
| 31 | `case 'DELETE':` | Handles DELETE requests |
| 34 | `default:` | Handles any other method |
| 40 | `app.use('/custom', (req, res) => {` | Defines middleware for paths starting with /custom |
| 43 | `if (req.path === '/custom/users' && req.method === 'GET') {` | Checks for a specific path and method |
| 46 | `else if (req.path === '/custom/users' && req.method === 'POST') {` | Checks for another path and method |
| 49 | `else if (req.path.startsWith('/custom/users/') && req.method === 'GET') {` | Checks for a path pattern and method |
| 52 | `const id = req.path.split('/').pop();` | Extracts the last segment of the path as the ID |
| 58 | `app.get('/path-info', (req, res) => {` | Defines a GET route for /path-info |
| 61 | `path: req.path,` | Gets the path (without query string) |
| 62 | `url: req.url,` | Gets the full URL (path + query string) |
| 63 | `query: req.query,` | Gets the parsed query string |
| 66-73 | `explanation: { ... }` | Provides explanations for each field |

## ⚠️ Common Mistakes

**1. Confusing req.path with req.url**
- `req.path`: The path portion of the URL (e.g., "/users/123")
- `req.url`: The full URL path and query string (e.g., "/users/123?sort=name")

**2. Forgetting that req.method is always uppercase**
Express normalizes the method to uppercase, so you can safely compare to "GET", "POST", etc.

**3. Using req.path for routing when you should use app.METHOD()**
While you can use req.path for custom routing logic, it's usually better to use Express's built-in routing (app.get(), app.post(), etc.) for clarity and to leverage features like route parameters.

**4. Not handling all HTTP methods in app.all()**
If you use app.all() and don't handle all methods, you might accidentally allow methods you didn't intend to.

**5. Assuming req.path includes a leading slash**
req.path always includes a leading slash (e.g., "/users", not "users").

## ✅ Quick Recap

- `req.method` gives the HTTP method (GET, POST, PUT, DELETE, etc.)
- `req.path` gives the path portion of the URL (without query string or domain)
- `req.url` gives the full URL path and query string (without domain and protocol)
- Use req.method and req.path for custom routing logic or to make decisions based on the request
- Express normalizes HTTP methods to uppercase

## 🔗 What's Next

Let's learn about checking request properties with `req.is()` and `req.accepts()`.
