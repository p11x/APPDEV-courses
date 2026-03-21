# Query Parameters and Filtering

## 📌 What You'll Learn
- How to handle query parameters in Express
- Implementing pagination for large datasets
- Filtering and sorting results
- Best practices for query string design

## 🧠 Concept Explained (Plain English)

When you build a REST API, you often need ways for clients to:
- Get only a subset of data (filtering)
- Sort results in different ways
- Paginate through large datasets
- Specify which fields to return

Query parameters are the standard way to handle these requirements. They're appended to the URL after a question mark, like: `/users?age=25&sort=name&page=1&limit=10`

Think of query parameters like asking a librarian:
- "Show me books" → `/books`
- "Show me only fiction books" → `/books?genre=fiction` (filtering)
- "Sort them by title" → `/books?sort=title` (sorting)
- "Show me 10 at a time" → `/books?limit=10` (pagination)
- "Show me the second page" → `/books?page=2` (pagination)

Query parameters are optional - the client decides whether to use them. This makes your API flexible and powerful.

## 💻 Code Example

```javascript
// ES Module - Query Parameters and Filtering

import express from 'express';

const app = express();

// Mock database
const products = [
    { id: 1, name: 'Laptop', category: 'electronics', price: 999, inStock: true },
    { id: 2, name: 'Mouse', category: 'electronics', price: 29, inStock: true },
    { id: 3, name: 'Shirt', category: 'clothing', price: 49, inStock: false },
    { id: 4, name: 'Pants', category: 'clothing', price: 79, inStock: true },
    { id: 5, name: 'Phone', category: 'electronics', price: 699, inStock: true },
    { id: 6, name: 'Hat', category: 'clothing', price: 19, inStock: true },
    { id: 7, name: 'Tablet', category: 'electronics', price: 449, inStock: false },
    { id: 8, name: 'Shoes', category: 'clothing', price: 89, inStock: true },
];

// ========================================
// BASIC QUERY PARAMETERS
// ========================================

// GET /products?category=electronics
app.get('/products', (req, res) => {
    // Access query parameters via req.query
    // req.query is an object with all query string parameters
    let results = [...products];
    
    // Example: Filter by category
    // URL: /products?category=electronics
    if (req.query.category) {
        results = results.filter(p => 
            p.category === req.query.category
        );
    }
    
    // Example: Filter by minimum price
    // URL: /products?minPrice=100
    if (req.query.minPrice) {
        const minPrice = parseFloat(req.query.minPrice);
        results = results.filter(p => p.price >= minPrice);
    }
    
    // Example: Filter by inStock status
    // URL: /products?inStock=true
    if (req.query.inStock) {
        const inStock = req.query.inStock === 'true';
        results = results.filter(p => p.inStock === inStock);
    }
    
    // Example: Search by name (partial match)
    // URL: /products?search=lap
    if (req.query.search) {
        const searchTerm = req.query.search.toLowerCase();
        results = results.filter(p => 
            p.name.toLowerCase().includes(searchTerm)
        );
    }
    
    res.json(results);
});

// ========================================
// SORTING RESULTS
// ========================================

// GET /products?sort=price&order=asc
app.get('/products-sorted', (req, res) => {
    let results = [...products];
    
    // Get sort parameter (default to 'id')
    const sortBy = req.query.sort || 'id';
    
    // Get order parameter (asc or desc, default to asc)
    const order = req.query.order === 'desc' ? -1 : 1;
    
    // Sort the results
    results.sort((a, b) => {
        if (a[sortBy] < b[sortBy]) return -1 * order;
        if (a[sortBy] > b[sortBy]) return 1 * order;
        return 0;
    });
    
    res.json({
        data: results,
        sort: sortBy,
        order: order === 1 ? 'asc' : 'desc'
    });
});

// ========================================
// PAGINATION
// ========================================

// GET /products-paginated?page=2&limit=5
app.get('/products-paginated', (req, res) => {
    // Parse pagination parameters with defaults
    // page defaults to 1, limit defaults to 10
    const page = parseInt(req.query.page) || 1;
    const limit = parseInt(req.query.limit) || 10;
    
    // Calculate indexes
    const startIndex = (page - 1) * limit;
    const endIndex = page * limit;
    
    // Slice the results
    const paginatedResults = products.slice(startIndex, endIndex);
    
    // Return paginated response with metadata
    res.json({
        data: paginatedResults,
        pagination: {
            currentPage: page,
            itemsPerPage: limit,
            totalItems: products.length,
            totalPages: Math.ceil(products.length / limit),
            hasNext: endIndex < products.length,
            hasPrev: page > 1
        }
    });
});

// ========================================
// FIELD SELECTION (SELECTING SPECIFIC FIELDS)
// ========================================

// GET /products?fields=id,name,price
app.get('/products-fields', (req, res) => {
    // Parse fields parameter into an array
    // URL: /products?fields=id,name,price
    const fields = req.query.fields 
        ? req.query.fields.split(',') 
        : null;
    
    let results = products.map(product => {
        if (!fields) return product; // Return all fields
        
        // Only include requested fields
        const filtered = {};
        fields.forEach(field => {
            if (product.hasOwnProperty(field)) {
                filtered[field] = product[field];
            }
        });
        return filtered;
    });
    
    res.json(results);
});

// ========================================
// COMBINING EVERYTHING
// ========================================

// GET /products?category=electronics&minPrice=100&sort=price&order=desc&page=1&limit=3
app.get('/products-full', (req, res) => {
    let results = [...products];
    
    // Filtering
    if (req.query.category) {
        results = results.filter(p => p.category === req.query.category);
    }
    
    if (req.query.minPrice) {
        results = results.filter(p => p.price >= parseFloat(req.query.minPrice));
    }
    
    if (req.query.maxPrice) {
        results = results.filter(p => p.price <= parseFloat(req.query.maxPrice));
    }
    
    if (req.query.inStock) {
        results = results.filter(p => p.inStock === (req.query.inStock === 'true'));
    }
    
    // Sorting
    if (req.query.sort) {
        const sortBy = req.query.sort;
        const order = req.query.order === 'desc' ? -1 : 1;
        
        results.sort((a, b) => {
            if (a[sortBy] < b[sortBy]) return -1 * order;
            if (a[sortBy] > b[sortBy]) return 1 * order;
            return 0;
        });
    }
    
    // Pagination
    const page = parseInt(req.query.page) || 1;
    const limit = parseInt(req.query.limit) || results.length;
    const startIndex = (page - 1) * limit;
    const endIndex = page * limit;
    
    const paginatedResults = results.slice(startIndex, endIndex);
    
    // Return full response
    res.json({
        data: paginatedResults,
        pagination: {
            currentPage: page,
            itemsPerPage: limit,
            totalItems: results.length,
            totalPages: Math.ceil(results.length / limit),
            hasNext: endIndex < results.length,
            hasPrev: page > 1
        },
        filters: req.query,
        sort: {
            by: req.query.sort || 'id',
            order: req.query.order || 'asc'
        }
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 28 | `req.query.category` | Access query parameter 'category' from URL |
| 30 | `results.filter(...)` | Filter results based on query parameter |
| 62 | `const sortBy = req.query.sort \|\| 'id';` | Get sort parameter with default |
| 63 | `const order = req.query.order === 'desc' ? -1 : 1;` | Determine sort direction |
| 78 | `const page = parseInt(req.query.page) \|\| 1;` | Parse page number with default |
| 81 | `const startIndex = (page - 1) * limit;` | Calculate starting index for pagination |
| 89 | `res.json({ data: ..., pagination: {...} })` | Return data with pagination metadata |
| 106 | `req.query.fields.split(',')` | Parse comma-separated fields |

## ⚠️ Common Mistakes

**1. Not parsing query parameter types**
Query parameters are always strings! Use `parseInt()` or `parseFloat()` for numbers.

**2. Not handling missing parameters**
Always provide defaults: `const page = parseInt(req.query.page) || 1`

**3. Returning too much data**
Implement pagination to avoid returning thousands of records at once.

**4. Not sanitizing input**
Validate and sanitize query parameters to prevent injection attacks.

**5. Inconsistent naming**
Be consistent: if you use `page` for pagination, don't use `p` somewhere else.

## ✅ Quick Recap

- Access query parameters with `req.query.parameterName`
- Always parse numeric parameters (they're strings by default)
- Implement pagination to handle large datasets
- Return pagination metadata so clients know total pages, etc.
- Support sorting with `sort` and `order` parameters
- Filter with specific parameters (`category`, `minPrice`, etc.)
- Consider supporting field selection for efficiency

## 🔗 What's Next

Let's look at authentication and security in Express APIs, which is crucial for protecting your data and ensuring only authorized users can access certain resources.
