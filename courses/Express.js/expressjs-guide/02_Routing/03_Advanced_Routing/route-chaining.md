# Route Chaining in Express

## 📌 What You'll Learn
- How to chain multiple handlers for a single route
- Using array of handlers
- Combining methods

## 🧠 Concept Explained (Plain English)

Route chaining allows you to attach multiple callback functions to a single route. Think of it like having multiple checkpoints - each handler does its job and passes to the next.

## 💻 Code Example

```js
// ES Module - Route Chaining

import express from 'express';

const app = express();

// Method 1: Array of handlers
app.get('/user', [
    (req, res, next) => {
        console.log('First handler');
        next();
    },
    (req, res, next) => {
        console.log('Second handler');
        next();
    },
    (req, res) => {
        res.send('User data');
    }
]);

// Method 2: Chained with app.route()
app.route('/book')
    .get((req, res) => res.json({ method: 'GET' }))
    .post((req, res) => res.json({ method: 'POST' }))
    .put((req, res) => res.json({ method: 'PUT' }));

// app.listen() starts the server
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 10 | `app.get('/user', [...])` | Route with array of handlers |
| 12-16 | `(req, res, next) => {...}` | Multiple middleware functions |
| 29 | `app.route('/book')` | Chain multiple methods |

## ⚠️ Common Mistakes

1. **Forgetting to call next()** - Handlers won't pass control
2. **Not handling errors in chain** - Use error-handling middleware

## ✅ Quick Recap

- Use arrays or app.route() for chaining
- Always call next() to pass control
- Great for validation chains

## 🔗 What's Next

Learn about [regex-and-pattern-routes.md](./regex-and-pattern-routes.md)