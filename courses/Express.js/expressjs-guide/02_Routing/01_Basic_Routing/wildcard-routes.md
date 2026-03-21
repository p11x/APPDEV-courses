# Wildcard Routes

## 📌 What You'll Learn
- What wildcard routes are
- How to use * and ** in routes
- Practical use cases

## 🧠 Concept Explained (Plain English)

**Wildcard routes** use special characters to match multiple URLs with a single route definition. They're like wildcards in a card game — they can represent any value.

The asterisk `*` matches any characters within a single URL segment, while `**` can match multiple path segments. This is useful for catching routes that don't match your more specific routes, implementing catch-all handlers, or building flexible APIs.

## 💻 Code Examples

```javascript
// ES Module - Wildcard Routes

import express from 'express';

const app = express();

// ========================================
// SINGLE SEGMENT WILDCARD (*)
// ========================================
// Matches anything in a single path segment

// Matches: /file/anything
app.get('/file/*', (req, res) => {
    // req.params[0] contains the matched portion
    const wildcardPart = req.params[0];
    
    res.json({
        message: 'Wildcard route matched',
        path: wildcardPart
    });
});

// Matches: /download/document.pdf, /download/image.png
app.get('/download/*', (req, res) => {
    const file = req.params[0];
    res.json({ downloading: file });
});

// ========================================
// CATCH-ALL ROUTE
// ========================================
// Must be LAST route - catches everything else

app.get('*', (req, res) => {
    res.status(404).json({
        error: 'Page not found',
        path: req.path
    });
});

// ========================================
// PRACTICAL EXAMPLE: VERSIONED API
// ========================================

// API v1 routes
app.get('/api/v1/*', (req, res) => {
    const apiPath = req.params[0];  // e.g., "users" from /api/v1/users
    res.json({
        version: 'v1',
        endpoint: apiPath,
        message: 'This is API version 1'
    });
});

// API v2 routes
app.get('/api/v2/*', (req, res) => {
    const apiPath = req.params[0];
    res.json({
        version: 'v2',
        endpoint: apiPath,
        message: 'This is API version 2'
    });
});

// ========================================
// NOTES ON WILDCARDS
// ========================================

/*
Wildcard patterns work with these methods:
- app.get('*', ...)
- app.post('*', ...)  
- app.all('*', ...) - matches all HTTP methods

Express 5 note: Use * at the end of path
Earlier versions may use (.*) regex
*/

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Wildcard Matching Examples

| Pattern | Matches | Doesn't Match |
|---------|---------|---------------|
| `/api/*` | `/api/users`, `/api/abc` | `/api/users/123` |
| `/file/*` | `/file/doc`, `/file/a/b` | N/A (single segment) |
| `*` | `/anything`, `/any/path` | N/A |

## ⚠️ Common Mistakes

**1. Wildcard route order**
Always put wildcard routes LAST. Otherwise, they'll catch everything.

**2. Overusing wildcards**
Wildcards can make debugging difficult. Use specific routes when possible.

**3. Security concerns**
Be careful with wildcards that might expose unintended routes.

## ✅ Quick Recap

- `*` matches any characters within one segment
- Access matched value via `req.params[0]`
- Always place wildcard routes last
- Use for catch-all or flexible routing

## 🔗 What's Next

Let's understand why route order matters in Express.
