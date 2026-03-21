# Route Order Matters

## 📌 What You'll Learn
- Why route order is critical in Express
- How Express matches routes
- Best practices for route ordering

## 🧠 Concept Explained (Plain English)

Express matches routes in the order they're defined. When a request comes in, Express goes through your routes from top to bottom and stops at the first match. This means if you put a general route before a specific one, the general one might catch all the requests!

Think of it like airport security. You have different lines for different passengers (first class, business, economy). If you put everyone through the economy line first, first-class passengers would also go through economy. Routes work the same way — more specific routes must come before general ones.

## 💻 Code Examples

```javascript
// ES Module - Route Order

import express from 'express';

const app = express();

// ========================================
// WRONG: General route first (bad practice!)
// ========================================

/*
app.get('/users', (req, res) => {
    res.send('All users list');
});

app.get('/users/admin', (req, res) => {
    res.send('Admin users');  // NEVER REACHED!
});
*/

// ========================================
// CORRECT: Specific routes first
// ========================================

// More specific route FIRST
app.get('/users/admin', (req, res) => {
    res.send('Admin users - specific route');
});

// More general route AFTER
app.get('/users', (req, res) => {
    res.send('All users list - general route');
});

// ========================================
// PARAMETER ROUTES - ORDER MATTERS!
// ========================================

// Route with ID parameter matches ANY value
app.get('/users/:id', (req, res) => {
    // This matches /users/123, /users/abc, etc.
    res.json({ message: 'User by ID', id: req.params.id });
});

// This will NEVER be reached because :id catches everything!
app.get('/users/new', (req, res) => {
    res.send('This never runs');
});

// ========================================
// CORRECT ORDER EXAMPLE
// ========================================

// Static routes FIRST
app.get('/users/new', (req, res) => {
    res.send('Create new user form');
});

app.get('/users/admin', (req, res) => {
    res.send('Admin users');
});

// Parameter routes AFTER static routes
app.get('/users/:id', (req, res) => {
    res.json({ userId: req.params.id });
});

// Query routes last
app.get('/search', (req, res) => {
    res.json({ query: req.query.q });
});

// ========================================
// HTTP METHOD ORDER
// ========================================

// Each method is checked independently
// GET /users is different from POST /users

app.get('/users', (req, res) => res.send('GET users'));
app.post('/users', (req, res) => res.send('POST create user'));
app.put('/users/:id', (req, res) => res.send('PUT update user'));
app.delete('/users/:id', (req, res) => res.send('DELETE user'));

// ========================================
// CATCH-ALL MUST BE LAST
// ========================================

// This catches any unmatched route
app.use((req, res) => {
    res.status(404).json({ error: 'Not found' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Route Matching Order

```
Request: GET /users/admin

1. Check app.get('/users/admin') → MATCH! → Return "Admin users"

(If admin route wasn't first:)
2. Check app.get('/users') → MATCH → Return "All users" (wrong!)
```

## Best Practice Order

1. **Static routes** - Exact paths like `/about`, `/contact`
2. **Route parameters** - Dynamic paths like `/users/:id`
3. **Query routes** - Routes with query strings
4. **Wildcard/catch-all** - Catch remaining routes
5. **Error handlers** - Handle errors

## ⚠️ Common Mistakes

**1. Putting parameterized routes first**
`/users/:id` will match `/users/admin` before `/users/admin` is checked.

**2. Not having a 404 handler**
Unhandled routes return confusing default errors.

**3. Forgetting that order matters for each method**
GET and POST routes are independent in order.

## ✅ Quick Recap

- Express matches routes top-to-bottom, first match wins
- More specific routes must come first
- Parameter routes (/users/:id) should come after static routes (/users/new)
- Always have a catch-all 404 handler at the end
