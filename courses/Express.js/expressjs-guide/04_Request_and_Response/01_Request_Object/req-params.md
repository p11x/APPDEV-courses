# req.params

## 📌 What You'll Learn
- What req.params is and how it's populated
- How to access route parameters in your Express application
- Handling multiple and nested parameters

## 🧠 Concept Explained (Plain English)

**Route parameters** are named segments in the URL that capture values specified at their position. They are defined using a colon (`:`) followed by the parameter name in the route path.

For example, in the route path `/users/:id`, `:id` is a route parameter. When a request is made to `/users/123`, the value `123` is captured and made available in `req.params.id`.

Think of it like a template with placeholders. The route path is the template, and the actual values in the URL fill in those placeholders. Express collects these filled-in values and makes them available in `req.params`.

This is essential for building RESTful APIs where you need to identify specific resources by their ID or other attributes.

## 💻 Code Example

```javascript
// ES Module - Accessing Route Parameters with req.params

import express from 'express';

const app = express();

// We'll use express.json() for parsing JSON bodies in other examples, but for this one we focus on params
// app.use(express.json());

// ========================================
// BASIC ROUTE PARAMETER
// ========================================

// Define a route with a parameter :id
// This will match URLs like /users/123, /users/abc, etc.
app.get('/users/:id', (req, res) => {
    // req.params is an object containing the route parameters
    // The value of the :id parameter is in req.params.id
    const userId = req.params.id;
    
    // We can use this value to look up a user in a database
    // For this example, we'll just return it
    res.json({ 
        message: 'User profile retrieved',
        userId: userId,
        // Note: In a real app, you would fetch the user from a database
        // and return the user data
    });
});

// ========================================
// MULTIPLE PARAMETERS
// ========================================

// Define a route with multiple parameters
// This will match URLs like /posts/2023/05
app.get('/posts/:year/:month', (req, res) => {
    // Access each parameter from req.params
    const { year, month } = req.params;
    
    res.json({
        message: `Posts from ${month}/${year}`,
        year: year,
        month: month
    });
});

// ========================================
// PARAMETERS WITH STATIC PATH SEGMENTS
// ========================================

// Parameters can be mixed with static paths
// This will match URLs like /api/users/123/profile
app.get('/api/users/:id/profile', (req, res) => {
    const { id } = req.params;
    
    res.json({
        message: `Profile for user ${id}`,
        userId: id
    });
});

// ========================================
// PARAMETERS ARE ALWAYS STRINGS
// ========================================

// Important: Route parameters are always strings!
// If you need to use them as numbers, you must convert them
app.get('/items/:id', (req, res) => {
    const idString = req.params.id;     // This is a string, e.g., "123"
    const idNumber = parseInt(req.params.id); // Convert to number
    
    res.json({
        asString: idString,
        asNumber: idNumber,
        typeOfString: typeof idString,  // "string"
        typeOfNumber: typeof idNumber   // "number"
    });
});

// Example with database lookup (conceptual)
app.get('/products/:productId', (req, res) => {
    // Convert to number for database query
    const productId = parseInt(req.params.id);
    
    // In a real app, you would do something like:
    // const product = await Product.findById(productId);
    // if (!product) return res.status(404).json({ error: 'Product not found' });
    // res.json(product);
    
    res.json({ productId: productId, message: 'Product retrieved' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `app.get('/users/:id', (req, res) => {` | Defines a route with a parameter `:id` |
| 11 | `const userId = req.params.id;` | Accesses the value of the `:id` parameter |
| 16 | `app.get('/posts/:year/:month', (req, res) => {` | Defines a route with two parameters |
| 19 | `const { year, month } = req.params;` | Destructures the year and month parameters |
| 25 | `app.get('/api/users/:id/profile', (req, res) => {` | Defines a route with a parameter and static segments |
| 28 | `const { id } = req.params;` | Accesses the id parameter |
| 34 | `app.get('/items/:id', (req, res) => {` | Defines a route to demonstrate parameter types |
| 37 | `const idString = req.params.id;` | Gets the parameter as a string |
| 38 | `const idNumber = parseInt(req.params.id);` | Converts the parameter to a number |
| 45 | `app.get('/products/:productId', (req, res) => {` | Another example route |
| 48 | `const productId = parseInt(req.params.id);` | Converts to number for potential database use |

## ⚠️ Common Mistakes

**1. Forgetting that parameters are strings**
Route parameters are always strings. If you need to use them as numbers (e.g., for database queries), you must convert them using `parseInt()`, `Number()`, or similar.

**2. Not handling missing parameters**
If a parameter is optional in your route (using `?`), you should handle the case where it might be undefined.

**3. Confusing req.params with req.query**
- `req.params` contains values from the URL path (e.g., `/users/123` → `{ id: '123' }`)
- `req.query` contains values from the query string (e.g., `/users?id=123` → `{ id: '123' }`)

**4. Not validating parameter values**
Always validate and sanitize parameter values, especially if you're using them in database queries, to prevent injection attacks.

## ✅ Quick Recap

- `req.params` contains route parameters (values from the URL path)
- Route parameters are defined with `:` in the route path (e.g., `/users/:id`)
- Access parameters via `req.params.parameterName`
- Parameters are always strings — convert to other types as needed
- Essential for building RESTful APIs and dynamic routes

## 🔗 What's Next

Let's learn about accessing query string parameters with `req.query`.
