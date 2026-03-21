# Query Strings

## 📌 What You'll Learn
- What query strings are
- How to access query parameters in Express
- Common use cases for query strings

## 🧠 Concept Explained (Plain English)

**Query strings** are the part of a URL that comes after the `?` symbol. They're used to pass optional parameters to the server. Unlike route parameters, which are part of the URL path, query strings provide additional filtering or configuration.

Think of query strings like options or filters. If the route is "show me products," the query string says "show me only those under $50" or "show me page 2." They modify the request without changing the URL structure.

For example, in `https://example.com/users?page=2&limit=10`:
- `?` marks the start of the query string
- `page=2` is a parameter named "page" with value "2"
- `&` separates multiple parameters
- `limit=10` is another parameter

## 💻 Code Examples

```javascript
// ES Module - Query Strings

import express from 'express';

const app = express();

// ========================================
// BASIC QUERY STRING
// ========================================

// URL: /search?name=Alice
app.get('/search', (req, res) => {
    // req.query contains all query string parameters
    // Access via req.query.paramName
    const name = req.query.name;
    
    res.json({
        message: 'Search results',
        searchTerm: name,
        allQueryParams: req.query
    });
});

// ========================================
// MULTIPLE QUERY PARAMETERS
// ========================================

// URL: /products?category=electronics&minPrice=100&maxPrice=500
app.get('/products', (req, res) => {
    const { category, minPrice, maxPrice, sort } = req.query;
    
    res.json({
        filters: {
            category,
            minPrice,
            maxPrice,
            sort
        },
        message: `Filtered products: ${category}`
    });
});

// ========================================
// PAGINATION EXAMPLE
// ========================================

// URL: /posts?page=2&limit=10
app.get('/posts', (req, res) => {
    // Parse query parameters as numbers
    const page = parseInt(req.query.page) || 1;
    const limit = parseInt(req.query.limit) || 10;
    
    // Calculate offset for database query
    const offset = (page - 1) * limit;
    
    res.json({
        pagination: {
            page,
            limit,
            offset,
            message: `Showing page ${page} with ${limit} items per page`
        }
    });
});

// ========================================
// OPTIONAL PARAMETERS
// ========================================

// Query strings are optional by nature
// URL: /users or /users?role=admin
app.get('/users', (req, res) => {
    const { role, active } = req.query;
    
    // Provide defaults when not specified
    const roleFilter = role || 'all';
    const activeFilter = active !== 'false'; // Default to true
    
    res.json({
        filters: {
            role: roleFilter,
            active: activeFilter
        },
        message: 'User list with filters'
    });
});

// ========================================
// BOOLEAN PARAMS
// ========================================

// URL: /api/data?includeRelated=true&debug=false
app.get('/api/data', (req, res) => {
    // Query strings are always strings!
    // Need to convert booleans manually
    const includeRelated = req.query.includeRelated === 'true';
    const debug = req.query.debug === 'true';
    
    res.json({
        includeRelated,
        debug,
        note: 'Boolean values converted from strings'
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Query String vs Route Parameter

| Aspect | Route Parameter | Query String |
|--------|-----------------|--------------|
| Syntax | `/users/:id` | `/users?id=123` |
| Purpose | Required resource ID | Optional filters |
| Position | Part of URL path | After `?` in URL |
| Multiple | Limited | Many |

## Testing

```bash
# Simple query string
curl "http://localhost:3000/search?name=Alice"

# Multiple parameters
curl "http://localhost:3000/products?category=books&minPrice=10"

# Pagination
curl "http://localhost:3000/posts?page=2&limit=10"

# Optional filters
curl "http://localhost:3000/users"
curl "http://localhost:3000/users?role=admin"
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 10 | `req.query.name` | Access query parameter "name" |
| 26 | `const { category, ... } = req.query` | Destructure multiple query params |
| 43 | `parseInt(req.query.page) || 1` | Parse number with default |

## ⚠️ Common Mistakes

**1. Forgetting query strings are strings**
URL parameters are always strings. Convert to numbers/booleans as needed.

**2. Not handling missing parameters**
Always provide defaults or check if parameters exist.

**3. Using query strings for required data**
Use route parameters for required values like IDs.

## ✅ Quick Recap

- Query strings follow `?` in the URL
- Access via `req.query.paramName`
- Use for optional filtering and pagination
- Always strings — convert to proper types

## 🔗 What's Next

Let's explore wildcard routes next.
