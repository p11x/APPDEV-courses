# Route Parameters

## 📌 What You'll Learn
- What route parameters are
- How to define and access them
- Multiple and nested parameters

## 🧠 Concept Explained (Plain English)

**Route parameters** are dynamic parts of a URL that can change. Instead of creating separate routes for every user or item, you use one route that captures the changing value.

Think of it like a form letter. Instead of writing "Hello Alice," "Hello Bob," etc., you write "Hello [NAME]" — the parameter [NAME] gets replaced with whatever name you need. Route parameters work the same way.

For example, `/users/1`, `/users/2`, `/users/3` — instead of creating three routes, you create one: `/users/:id` where `:id` is the parameter.

## 💻 Code Examples

```javascript
// ES Module - Route Parameters

import express from 'express';

const app = express();

// ========================================
// BASIC ROUTE PARAMETER
// ========================================

// :id is a parameter that captures values
// Matches: /users/1, /users/abc, /users/anything
app.get('/users/:id', (req, res) => {
    // req.params contains all parameters
    // req.params.id captures the value after /users/
    const userId = req.params.id;
    
    res.json({
        message: 'User profile',
        userId: userId
    });
});

// ========================================
// MULTIPLE PARAMETERS
// ========================================

// Multiple parameters separated by /
// Matches: /posts/2023/05
app.get('/posts/:year/:month', (req, res) => {
    // Access each parameter from req.params
    const { year, month } = req.params;
    
    res.json({
        year: year,
        month: month,
        message: `Posts from ${month}/${year}`
    });
});

// ========================================
// PARAMETERS WITH OTHER PATH PARTS
// ========================================

// Parameters can be mixed with static paths
// Matches: /api/users/123/profile
app.get('/api/users/:id/profile', (req, res) => {
    const { id } = req.params;
    
    res.json({
        message: `Profile for user ${id}`,
        userId: id
    });
});

// ========================================
// PARAMETER NAMING
// ========================================

// Parameter names should be meaningful
// :id, :userId, :postId - clear and descriptive

// Good: Clear parameter names
app.get('/users/:userId/posts/:postId', (req, res) => {
    const { userId, postId } = req.params;
    res.json({ userId, postId });
});

// ========================================
// PARAMETERS ARE ALWAYS STRINGS
// ========================================

// URL parameters are always strings!
// Must convert to number if needed
app.get('/items/:id', (req, res) => {
    const idString = req.params.id;     // "123" (string)
    const idNumber = parseInt(req.params.id);  // 123 (number)
    
    res.json({
        asString: idString,
        asNumber: idNumber,
        typeOf: typeof idString  // "string"
    });
});

// Example with database lookup
app.get('/products/:productId', (req, res) => {
    const productId = parseInt(req.params.id);
    
    // Find product by numeric ID
    // const product = await Product.findById(productId);
    
    res.json({ productId: productId });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Parameter Validation

You can validate parameters using regex:

```javascript
// Only match numeric IDs
app.get('/users/:id(\\d+)', (req, res) => {
    // Only matches /users/123, not /users/abc
    res.json({ userId: req.params.id });
});

// Match specific format (UUID)
app.get('/items/:id([a-f0-9-]{36})', (req, res) => {
    res.json({ itemId: req.params.id });
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `app.get('/users/:id', ...)` | Defines route with `:id` parameter |
| 12 | `req.params.id` | Access the captured parameter value |
| 30 | `req.params.year` | Access year parameter |
| 31 | `req.params.month` | Access month parameter |
| 60 | `parseInt(req.params.id)` | Convert string to number |

## Testing

```bash
# These URLs would match /users/:id
curl http://localhost:3000/users/123
curl http://localhost:3000/users/abc

# These would match /posts/:year/:month
curl http://localhost:3000/posts/2023/05
curl http://localhost:3000/posts/2024/12
```

## ⚠️ Common Mistakes

**1. Forgetting to convert types**
URL parameters are always strings. Use parseInt() or Number() for numeric comparisons.

**2. Parameters not matching**
Express matches routes in order. Put more specific routes before general ones.

**3. Undefined parameters**
Make sure to handle cases where a parameter might be missing.

## ✅ Quick Recap

- Route parameters (`:name`) capture dynamic URL segments
- Access via `req.params.name`
- Parameters are always strings — convert as needed
- Multiple parameters: `/posts/:year/:month`

## 🔗 What's Next

Let's learn about query strings next.
