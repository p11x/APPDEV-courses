# req.query

## 📌 What You'll Learn
- What req.query is and how it's populated
- How to access query string parameters in your Express application
- Handling multiple query parameters and default values

## 🧠 Concept Explained (Plain English)

**Query string parameters** are the part of a URL that comes after the `?` symbol. They are used to pass optional parameters to the server, often for filtering, sorting, or pagination.

For example, in the URL `/search?q=express&limit=10`, the query string is `q=express&limit=10`. This contains two parameters: `q` with value "express" and `limit` with value "10".

Express automatically parses the query string and makes the parameters available in `req.query` as an object. Each parameter becomes a property of this object.

Think of it like filling out a form with optional fields. The query string is the way to send those optional field values to the server in the URL.

## 💻 Code Example

```javascript
// ES Module - Accessing Query String Parameters with req.query

import express from 'express';

const app = express();

// We'll use express.json() for parsing JSON bodies in other examples, but for this one we focus on query
// app.use(express.json());

// ========================================
// BASIC QUERY STRING
// ========================================

// Define a route that will receive query parameters
// This will match URLs like /search?q=express
app.get('/search', (req, res) => {
    // req.query is an object containing the query string parameters
    // The value of the q parameter is in req.query.q
    const searchTerm = req.query.q;
    
    res.json({ 
        message: 'Search results',
        searchTerm: searchTerm
    });
});

// ========================================
// MULTIPLE QUERY PARAMETERS
// ========================================

// Define a route that will receive multiple query parameters
// This will match URLs like /products?category=electronics&minPrice=100&maxPrice=500
app.get('/products', (req, res) => {
    // Access each parameter from req.query
    const { category, minPrice, maxPrice, sort } = req.query;
    
    res.json({
        message: 'Product list',
        filters: {
            category: category || 'all',
            minPrice: minPrice ? parseFloat(minPrice) : undefined,
            maxPrice: maxPrice ? parseFloat(maxPrice) : undefined,
            sort: sort || 'name'
        }
    });
});

// ========================================
// PAGINATION EXAMPLE
// ========================================

// Define a route for paginated results
// This will match URLs like /posts?page=2&limit=10
app.get('/posts', (req, res) => {
    // Get page and limit from query string, with defaults
    const page = parseInt(req.query.page) || 1;
    const limit = parseInt(req.query.limit) || 10;
    
    // Calculate offset for database query
    const offset = (page - 1) * limit;
    
    res.json({
        message: `Posts page ${page}`,
        pagination: {
            page,
            limit,
            offset
        }
    });
});

// ========================================
// OPTIONAL PARAMETERS WITH DEFAULTS
// ========================================

// Define a route with optional query parameters
// This will match URLs like /users or /users?role=admin&active=true
app.get('/users', (req, res) => {
    // Get parameters with defaults
    const role = req.query.role || 'all';
    const active = req.query.active === 'true'; // Convert string to boolean
    
    res.json({
        message: 'User list',
        filters: {
            role,
            active
        }
    });
});

// ========================================
// HANDLING MISSING PARAMETERS
// ========================================

// Define a route that requires a parameter
// This will match URLs like /report?type=sales
app.get('/report', (req, res) => {
    const reportType = req.query.type;
    
    // Check if the required parameter is present
    if (!reportType) {
        return res.status(400).json({ error: 'Missing required parameter: type' });
    }
    
    res.json({ 
        message: `Generating ${reportType} report`,
        type: reportType
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `app.get('/search', (req, res) => {` | Defines a route that will receive query parameters |
| 11 | `const searchTerm = req.query.q;` | Accesses the q parameter from req.query |
| 16 | `app.get('/products', (req, res) => {` | Defines a route for products with multiple query parameters |
| 19 | `const { category, minPrice, maxPrice, sort } = req.query;` | Destructures multiple query parameters |
| 22 | `category: category || 'all',` | Provides a default value if category is not present |
| 23 | `minPrice: minPrice ? parseFloat(minPrice) : undefined,` | Converts to number if present, otherwise undefined |
| 30 | `app.get('/posts', (req, res) => {` | Defines a route for paginated posts |
| 33 | `const page = parseInt(req.query.page) || 1;` | Gets page parameter with default of 1 |
| 34 | `const limit = parseInt(req.query.limit) || 10;` | Gets limit parameter with default of 10 |
| 37 | `const offset = (page - 1) * limit;` | Calculates offset for database query |
| 43 | `app.get('/users', (req, res) => {` | Defines a route for users with optional parameters |
| 46 | `const role = req.query.role || 'all';` | Gets role parameter with default |
| 47 | `const active = req.query.active === 'true';` | Converts string 'true' to boolean true |
| 55 | `app.get('/report', (req, res) => {` | Defines a route requiring a parameter |
| 58 | `if (!reportType) {` | Checks if the required parameter is missing |
| 59 | `return res.status(400).json({ error: 'Missing required parameter: type' });` | Returns error if missing |
| 62 | `res.json({ ... });` | Returns success response |

## ⚠️ Common Mistakes

**1. Forgetting that query parameters are strings**
All values in `req.query` are strings. If you need to use them as numbers or booleans, you must convert them.

**2. Not handling missing parameters**
If a parameter is required for your logic, you should check if it's present and return an error if not.

**3. Confusing req.query with req.params**
- `req.query` contains values from the query string (after `?`)
- `req.params` contains values from the URL path (defined with `:`)

**4. Not providing sensible defaults**
For optional parameters, consider what makes sense as a default value (e.g., page=1, limit=10).

**5. Incorrectly converting booleans**
The string "false" is truthy in JavaScript, so you must explicitly check for "true" to convert to boolean.

## ✅ Quick Recap

- `req.query` contains query string parameters (values after `?` in the URL)
- Access parameters via `req.query.parameterName`
- Parameters are always strings — convert to other types as needed
- Use for optional filtering, sorting, pagination, and configuration
- Always validate and provide defaults for optional parameters

## 🔗 What's Next

Let's learn about accessing request headers with `req.headers`.
