# res.json() - Sending JSON Responses

## 📌 What You'll Learn
- How to send JSON responses
- The difference between res.json() and res.send()
- Setting JSON response status codes

## 🧠 Concept Explained (Plain English)

The `res.json()` method is specifically designed for sending JSON data in Express applications. Think of it as a specialized JSON courier - it takes your JavaScript objects or arrays and converts them into a JSON string that can be sent over HTTP.

When you call `res.json()`:
1. It automatically sets Content-Type to application/json
2. It converts your JavaScript data to JSON using JSON.stringify()
3. It sends the response with the proper encoding

## 💻 Code Example

```javascript
// ES Module - Using res.json()

import express from 'express';

const app = express();

// ========================================
// res.json() - Send JSON responses
// ========================================

// Send a simple object as JSON
app.get('/user', (req, res) => {
    // req = request object (data from client)
    // res = response object (data we send back)
    res.json({
        name: 'Alice',
        email: 'alice@example.com',
        age: 25
    });
});

// Send an array as JSON
app.get('/users', (req, res) => {
    const users = [
        { id: 1, name: 'Alice' },
        { id: 2, name: 'Bob' },
        { id: 3, name: 'Charlie' }
    ];
    res.json(users);
});

// Send with status code
app.post('/users', (req, res) => {
    const newUser = {
        id: 4,
        name: req.body.name,
        createdAt: new Date()
    };
    // 201 = Created
    res.status(201).json(newUser);
});

// Send error response
app.get('/error', (req, res) => {
    res.status(404).json({
        error: 'Not Found',
        message: 'The requested resource does not exist'
    });
});

// Send null or undefined (valid JSON!)
app.get('/empty', (req, res) => {
    res.json(null);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 13 | `res.json({ name: 'Alice'... });` | Send object as JSON |
| 22 | `res.json(users);` | Send array as JSON |
| 29 | `res.status(201).json(newUser);` | Send JSON with status code |
| 37 | `res.status(404).json({ error: ... });` | Send error as JSON |

## ⚠️ Common Mistakes

**1. Using res.send() instead of res.json() for APIs**
Always use res.json() for API responses - it's explicit and handles edge cases.

**2. Forgetting to return after res.json()**
Though not required, returning after sending is good practice.

**3. Sending undefined**
res.json(undefined) sends null, not undefined. If you need to send "no data", use null.

## ✅ Quick Recap

- `res.json()` automatically sets Content-Type: application/json
- Works with objects, arrays, null
- Chain with status() for status codes
- Use for all API responses

## 🔗 What's Next

Learn about [res.status()](./res-status.md) for setting HTTP status codes