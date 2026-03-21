# Setting Up Express.js

## What You'll Learn

- What Express.js is
- Installing Express
- Creating a basic Express app
- Running and testing your server

## What is Express?

**Express.js** (or simply Express) is the most popular Node.js web framework. It makes building web servers and APIs much easier than using the raw http module.

Express provides:
- Simple routing
- Middleware support
- Template engines
- Much more

## Installing Express

### Step 1: Create a Project

```bash
mkdir my-express-app
cd my-express-app
npm init -y
```

### Step 2: Install Express

```bash
npm install express
```

### Step 3: Create package.json with ESM

Make sure your package.json has `"type": "module"`:

```json
{
  "name": "my-express-app",
  "version": "1.0.0",
  "type": "module",
  "main": "index.js",
  "scripts": {
    "start": "node index.js"
  }
}
```

## Creating Your First Express App

### Basic Server

Create `index.js`:

```javascript
// index.js - Basic Express server

import express from 'express';

// Create an Express app
const app = express();

// Define a simple route
// GET request to the root URL
app.get('/', (req, res) => {
  res.send('Hello from Express!');
});

// Start the server
const PORT = 3000;
app.listen(PORT, () => {
  console.log(`Server running at http://localhost:${PORT}/`);
});
```

### Running the Server

```bash
node index.js
```

Open your browser to http://localhost:3000

## Understanding the Code

### express()

```javascript
const app = express();
```

This creates an Express application - your web server.

### app.get()

```javascript
app.get('/', (req, res) => {
  res.send('Hello!');
});
```

This defines a route:
- `get` - Handle GET requests
- `'/'` - The URL path
- Callback with `req` (request) and `res` (response)

### app.listen()

```javascript
app.listen(PORT, () => {
  console.log(`Running on port ${PORT}`);
});
```

Starts the server on the specified port.

## More Routes

```javascript
// index.js - Multiple routes

import express from 'express';
const app = express();

// Home page
app.get('/', (req, res) => {
  res.send('Welcome to the Home Page!');
});

// About page
app.get('/about', (req, res) => {
  res.send('About Us - We are awesome!');
});

// Contact page
app.get('/contact', (req, res) => {
  res.send('Contact us at: contact@example.com');
});

// API endpoint - return JSON
app.get('/api/users', (req, res) => {
  res.json([
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' }
  ]);
});

app.listen(3000, () => {
  console.log('Server running on port 3000');
});
```

## Code Example: Complete Express App

```javascript
// server.js - Complete Express example

import express from 'express';

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware to log requests
app.use((req, res, next) => {
  console.log(`${req.method} ${req.url}`);
  next();  // Continue to the route handler
});

// Routes
app.get('/', (req, res) => {
  res.send(`
    <h1>Express Demo</h1>
    <p>Try these routes:</p>
    <ul>
      <li>/about</li>
      <li>/api/users</li>
      <li>/api/products</li>
    </ul>
  `);
});

app.get('/about', (req, res) => {
  res.send('<h2>About Page</h2><p>This is a demo Express app!</p>');
});

app.get('/api/users', (req, res) => {
  res.json({
    users: [
      { id: 1, name: 'Alice', email: 'alice@example.com' },
      { id: 2, name: 'Bob', email: 'bob@example.com' }
    ]
  });
});

app.get('/api/products', (req, res) => {
  res.json({
    products: [
      { id: 1, name: 'Laptop', price: 999 },
      { id: 2, name: 'Phone', price: 599 }
    ]
  });
});

// 404 handler - runs when no route matches
app.use((req, res) => {
  res.status(404).send('Page not found!');
});

// Start server
app.listen(PORT, () => {
  console.log(`Server running at http://localhost:${PORT}/`);
  console.log('Try: /, /about, /api/users');
});
```

## Common Mistakes

### Mistake 1: Forgetting to Start Server

```javascript
// WRONG - app created but not listening
const app = express();
app.get('/', (req, res) => res.send('Hello'));

// CORRECT - call app.listen()
const app = express();
app.get('/', (req, res) => res.send('Hello'));
app.listen(3000);
```

### Mistake 2: Not Using next()

When creating custom middleware, always call next() or the request will hang:

```javascript
// WRONG - middleware never continues
app.use((req, res) => {
  console.log('Got request');
  // Missing next()!
});

// CORRECT
app.use((req, res, next) => {
  console.log('Got request');
  next();  // Continue to next handler
});
```

### Mistake 3: Wrong Route Order

Define more specific routes before general ones:

```javascript
// WRONG - /api will match before /api/users
app.get('/api', ...);
app.get('/api/users', ...);

// CORRECT - specific routes first
app.get('/api/users', ...);
app.get('/api', ...);
```

## Try It Yourself

### Exercise 1: Basic Server
Create an Express server with routes for home, about, and contact.

### Exercise 2: JSON API
Create an API endpoint that returns JSON data.

### Exercise 3: Multiple Routes
Add several different routes to your Express app.

## Next Steps

Now you can set up Express. Let's learn about routing in detail. Continue to [Express Routing](./02-routing.md).
