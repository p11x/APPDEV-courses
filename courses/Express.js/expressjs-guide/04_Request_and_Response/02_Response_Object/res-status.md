# res.status() - Setting HTTP Status Codes

## 📌 What You'll Learn
- What HTTP status codes mean
- How to set status codes in Express
- Common status codes and when to use them

## 🧠 Concept Explained (Plain English)

HTTP status codes are like short messages that tell the client what happened with their request. Think of them as the "subject line" of a letter - they give quick context before the actual content.

Status codes are three-digit numbers:
- **1xx** (100-199): Informational - request received, continuing
- **2xx** (200-299): Success - request was received and accepted
- **3xx** (300-399): Redirection - further action needed
- **4xx** (400-499): Client Error - problem with the request
- **5xx** (500-599): Server Error - server failed to fulfill request

## 💻 Code Example

```javascript
// ES Module - Setting HTTP Status Codes

import express from 'express';

const app = express();

// ========================================
// Common status codes with res.status()
// ========================================

// 200 - OK (default for successful GET, PUT, PATCH)
app.get('/users', (req, res) => {
    // req = request object, res = response object
    const users = [{ id: 1, name: 'Alice' }];
    res.status(200).json(users);
});

// 201 - Created (successful POST)
app.post('/users', (req, res) => {
    const newUser = { id: 2, name: req.body.name };
    res.status(201).json(newUser);
});

// 204 - No Content (successful DELETE, no response body)
app.delete('/users/:id', (req, res) => {
    // Just send 204 without a body
    res.status(204).send();
});

// 400 - Bad Request (client sent invalid data)
app.post('/validate', (req, res) => {
    if (!req.body.email) {
        return res.status(400).json({ 
            error: 'Email is required' 
        });
    }
    res.json({ success: true });
});

// 401 - Unauthorized (not logged in)
app.get('/profile', (req, res) => {
    const isAuthenticated = false;
    if (!isAuthenticated) {
        return res.status(401).json({ 
            error: 'Please log in' 
        });
    }
    res.json({ user: 'data' });
});

// 403 - Forbidden (logged in but no permission)
app.delete('/admin/users', (req, res) => {
    const isAdmin = false;
    if (!isAdmin) {
        return res.status(403).json({ 
            error: 'Admin access required' 
        });
    }
});

// 404 - Not Found (resource doesn't exist)
app.get('/users/:id', (req, res) => {
    const userId = parseInt(req.params.id);
    const user = null; // Simulating not found
    
    if (!user) {
        return res.status(404).json({ 
            error: 'User not found' 
        });
    }
    res.json(user);
});

// 500 - Internal Server Error (server problem)
app.get('/error', (req, res) => {
    try {
        // Something that might fail
        throw new Error('Database connection failed');
    } catch (err) {
        res.status(500).json({ 
            error: 'Internal server error' 
        });
    }
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 15 | `res.status(200).json(users);` | Set 200 OK status |
| 21 | `res.status(201).json(newUser);` | Set 201 Created |
| 27 | `res.status(204).send();` | Set 204 No Content |
| 33 | `res.status(400).json({...});` | Set 400 Bad Request |
| 51 | `res.status(404).json({...});` | Set 404 Not Found |

## ⚠️ Common Mistakes

**1. Not setting appropriate status codes**
Always use the correct status code - it helps clients understand what happened.

**2. Setting status after sending response**
You must set status BEFORE calling res.send() or res.json().

**3. Using 200 for errors**
Errors should return 4xx or 5xx status codes, not 200.

## ✅ Quick Recap

- Use res.status(code) before sending response
- 200 = Success, 201 = Created
- 400 = Bad Request, 401 = Unauthorized, 404 = Not Found
- 500 = Server Error
- Always use appropriate codes for better API UX

## 🔗 What's Next

Learn about [res.redirect()](./res-redirect.md) for redirecting requests