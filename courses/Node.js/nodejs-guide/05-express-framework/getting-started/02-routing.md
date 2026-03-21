# Express Routing

## What You'll Learn

- HTTP methods in Express
- Route parameters
- Query strings
- Chainable route handlers

## HTTP Methods in Express

Express supports all HTTP methods:

```javascript
app.get('/', (req, res) => res.send('GET request'));
app.post('/', (req, res) => res.send('POST request'));
app.put('/', (req, res) => res.send('PUT request'));
app.delete('/', (req, res) => res.send('DELETE request'));
app.patch('/', (req, res) => res.send('PATCH request'));
```

## Route Parameters

### URL Parameters

```javascript
// Route with parameter
app.get('/users/:id', (req, res) => {
  // Access the parameter
  const userId = req.params.id;
  res.send(`User ID: ${userId}`);
});
```

Access at: `/users/123` → Returns "User ID: 123"

### Multiple Parameters

```javascript
app.get('/posts/:postId/comments/:commentId', (req, res) => {
  const { postId, commentId } = req.params;
  res.json({ postId, commentId });
});
```

## Query Strings

### Accessing Query Parameters

```javascript
app.get('/search', (req, res) => {
  // req.query contains query string parameters
  const term = req.query.q;
  const page = req.query.page || 1;
  
  res.json({ term, page });
});
```

Access at: `/search?q=books&page=2`

## Code Example: Complete Routing

```javascript
// routes.js - Express routing example

import express from 'express';
const app = express();
const PORT = 3000;

// ─────────────────────────────────────────
// GET Routes
// ─────────────────────────────────────────

// Home
app.get('/', (req, res) => {
  res.send(`
    <h1>Express Routing Demo</h1>
    <h2>Routes:</h2>
    <ul>
      <li>GET /users</li>
      <li>GET /users/123</li>
      <li>POST /users</li>
      <li>GET /search?q=term</li>
      <li>PUT /users/123</li>
      <li>DELETE /users/123</li>
    </ul>
  `);
});

// List all users
app.get('/users', (req, res) => {
  res.json({
    users: [
      { id: 1, name: 'Alice' },
      { id: 2, name: 'Bob' }
    ]
  });
});

// Get single user - route parameter
app.get('/users/:id', (req, res) => {
  const userId = req.params.id;
  res.json({ id: userId, name: `User ${userId}` });
});

// Search - query string
app.get('/search', (req, res) => {
  const query = req.query.q || '';
  const page = parseInt(req.query.page) || 1;
  
  res.json({
    query,
    page,
    results: [`Result 1 for "${query}"`, `Result 2 for "${query}"`]
  });
});

// ─────────────────────────────────────────
// POST Route
// ─────────────────────────────────────────

// Create user - need body-parser (built into Express now)
app.use(express.json());  // Parse JSON bodies

app.post('/users', (req, res) => {
  // Access body data
  const { name, email } = req.body;
  
  // In real app, save to database
  const newUser = {
    id: Date.now(),
    name,
    email
  };
  
  res.status(201).json({
    message: 'User created!',
    user: newUser
  });
});

// ─────────────────────────────────────────
// PUT Route (Update)
// ─────────────────────────────────────────

app.put('/users/:id', (req, res) => {
  const userId = req.params.id;
  const { name, email } = req.body;
  
  res.json({
    message: `User ${userId} updated`,
    user: { id: userId, name, email }
  });
});

// ─────────────────────────────────────────
// DELETE Route
// ─────────────────────────────────────────

app.delete('/users/:id', (req, res) => {
  const userId = req.params.id;
  
  res.json({
    message: `User ${userId} deleted`
  });
});

// ─────────────────────────────────────────
// Chainable Handlers
// ─────────────────────────────────────────

app.get('/chain', 
  (req, res, next) => {
    console.log('First handler');
    next();  // Call next handler
  },
  (req, res) => {
    res.send('Chain complete!');
  }
);

// ─────────────────────────────────────────
// 404 Handler
// ─────────────────────────────────────────

app.use((req, res) => {
  res.status(404).send('Route not found');
});

// Start server
app.listen(PORT, () => {
  console.log(`Server running on http://localhost:${PORT}`);
});
```

## Testing Routes

### Using curl

```bash
# GET request
curl http://localhost:3000/users

# GET with parameter
curl http://localhost:3000/users/123

# GET with query string
curl "http://localhost:3000/search?q=hello&page=2"

# POST request
curl -X POST http://localhost:3000/users \
  -H "Content-Type: application/json" \
  -d '{"name": "Charlie", "email": "charlie@example.com"}'

# PUT request
curl -X PUT http://localhost:3000/users/123 \
  -H "Content-Type: application/json" \
  -d '{"name": "Updated"}'

# DELETE request
curl -X DELETE http://localhost:3000/users/123
```

## Common Mistakes

### Mistake 1: Forgetting express.json()

```javascript
// WRONG - req.body is undefined
app.post('/users', (req, res) => {
  console.log(req.body);  // undefined!
});

// CORRECT - add middleware first
app.use(express.json());
app.post('/users', (req, res) => {
  console.log(req.body);  // Works!
});
```

### Mistake 2: Parameter Route Order

```javascript
// WRONG - /users/new never matches (matches /users/:id first)
app.get('/users/:id', ...);
app.get('/users/new', ...);

// CORRECT - specific routes first
app.get('/users/new', ...);
app.get('/users/:id', ...);
```

## Try It Yourself

### Exercise 1: CRUD API
Create routes for Create, Read, Update, Delete operations.

### Exercise 2: Query Parameters
Create a route that accepts query parameters for filtering.

### Exercise 3: Route Chaining
Create multiple handlers that run in sequence.

## Next Steps

Now you understand Express routing. Let's learn about middleware. Continue to [Middleware](./03-middleware.md).
