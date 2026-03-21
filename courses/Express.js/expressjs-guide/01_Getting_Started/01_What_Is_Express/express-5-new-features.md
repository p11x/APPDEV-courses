# Express 5 New Features

## 📌 What You'll Learn
- What's new in Express 5
- Key improvements over Express 4
- How to update your existing Express code

## 🧠 Concept Explained (Plain English)

Express 5 represents a major evolution of the popular web framework. While it maintains the same familiar API you know from Express 4, it introduces several quality-of-life improvements that make building web applications easier and more modern.

The biggest change is **native async error handling**. In Express 4, if you had an async route handler that threw an error, you needed to wrap it in try/catch or use a helper function. Express 5 handles this automatically — errors in async handlers are automatically caught and passed to your error-handling middleware.

Express 5 also makes some breaking changes from Express 4, removing deprecated features and changing how certain things work. This means you may need to update existing Express 4 code when migrating.

## 💻 Code Example

```javascript
// ES Module - Express 5 New Features

import express from 'express';

const app = express();

app.use(express.json());

// ========================================
// FEATURE 1: Native Async Error Handling
// ========================================
// In Express 4, you'd need try/catch or a wrapper
// In Express 5, async errors are handled automatically!

app.get('/users', async (req, res) => {
    // This async function automatically catches errors
    // No try/catch needed in Express 5!
    const users = await fetchUsersFromDatabase();
    res.json(users);
});

app.get('/posts/:id', async (req, res) => {
    // Even with params, errors are handled automatically
    const post = await getPost(req.params.id);
    if (!post) {
        throw new Error('Post not found'); // Express 5 catches this!
    }
    res.json(post);
});

// ========================================
// FEATURE 2: Promise-Based Routing
// ========================================
// Route handlers can now return promises directly
// Express waits for the promise to resolve

app.post('/orders', async (req, res) => {
    const order = await createOrder(req.body);
    return order; // Can return instead of res.json()
});

// ========================================
// FEATURE 3: New Route Pattern Syntax
// ========================================
// Express 5 has improved routing patterns

// Optional parameters: userId? makes it optional
app.get('/users/:userId?/posts', (req, params) => {
    // Matches both /users/posts and /users/123/posts
    res.json({ params });
});

// ========================================
// Error Handler (same pattern, works better now)
// ========================================
app.use((err, req, res, next) => {
    // In Express 5, this catches ALL async errors automatically
    console.error('Error:', err.message);
    res.status(500).json({ error: 'Something went wrong!' });
});

app.listen(3000, () => console.log('Express 5 server running!'));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 14 | `async (req, res) => {...}` | Async route handler — Express 5 auto-catches errors |
| 16 | `await fetchUsersFromDatabase()` | Async operation — no try/catch needed in Express 5 |
| 32 | `return order;` | Promise-based routing — can return instead of sending response |
| 43 | `/:userId?` | Optional route parameter — new in Express 5 |
| 53 | `app.use((err, ...)` | Error handler — catches all async errors automatically |

## ⚠️ Common Mistakes

**1. Not updating existing Express 4 code**
Some patterns that worked in Express 4 are deprecated or changed in Express 5. Test your existing code thoroughly.

**2. Assuming async errors still break**
In Express 5, you don't need try/catch in async handlers. But remember this won't work in Express 4 — always check your version.

**3. Not handling promise rejections**
While Express 5 handles thrown errors, unhandled promise rejections should still be caught at the process level.

## ✅ Quick Recap

- Express 5 has native async error handling — no try/catch needed
- Promise-based routing allows returning values instead of calling res.json()
- New optional parameter syntax with `?`
- Breaking changes from Express 4 require testing when upgrading

## 🔗 What's Next

Now that you understand Express, let's set up your development environment with the prerequisites.
