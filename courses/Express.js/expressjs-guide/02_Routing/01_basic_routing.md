# Basic Routing in Express.js

## What is Routing?

**Routing** is how your application responds to different URLs and HTTP methods. When a user visits a URL, Express matches the request to a handler function that decides what to send back.

Think of routing like a telephone switchboard — it connects incoming calls (requests) to the right person (handler function).

## HTTP Methods

The most common HTTP methods you'll use:

| Method | Purpose | Example |
|--------|---------|---------|
| **GET** | Retrieve data | Loading a page, fetching data |
| **POST** | Create new data | Submitting a form |
| **PUT** | Update existing data | Editing a profile |
| **DELETE** | Remove data | Deleting an account |
| **PATCH** | Partially update data | Updating just one field |

## Basic Route Syntax

In Express, routes are defined using:

```javascript
app.METHOD(PATH, HANDLER);
```

- **app** = your Express application
- **METHOD** = an HTTP method in lowercase (get, post, put, delete)
- **PATH** = the URL path
- **HANDLER** = the function that runs when the route matches

## Your First Routes

Let's create a server with multiple routes:

```javascript
// server.js
import express from 'express';

const app = express();
// 'app' is our Express application instance
// We use it to define all our routes and middleware

const PORT = process.env.PORT || 3000;

// Built-in middleware to parse JSON
// This allows us to read req.body in POST/PUT requests
app.use(express.json());

// ============================================
// Route Table
// ============================================
// | Method | Path       | Handler         | Description              |
// |--------|------------|-----------------|--------------------------|
// | GET    | /          | homePage        | Home page                |
// | GET    | /about     | aboutPage       | About page               |
// | GET    | /contact   | contactPage     | Contact page             |
// | POST   | /submit    | handleSubmit    | Handle form submission   |
// ============================================

// GET / - Home page
// req = the request object (information from the browser)
// res = the response object (what we send back)
app.get('/', (req, res) => {
    res.send('Welcome to the Home Page!');
});

// GET /about - About page
app.get('/about', (req, res) => {
    res.send('This is the About Page');
});

// GET /contact - Contact page
app.get('/contact', (req, res) => {
    res.send('Contact us at: hello@example.com');
});

// POST /submit - Handle form submissions
app.post('/submit', (req, res) => {
    // req.body contains the submitted form data
    console.log('Received data:', req.body);
    res.send('Form submitted successfully!');
});

app.listen(PORT, () => {
    console.log(`Server running at http://localhost:${PORT}`);
});
```

## Testing Routes

You can test your routes using:

### Using curl (Terminal)

```bash
# GET request
curl http://localhost:3000/

# POST request with JSON data
curl -X POST http://localhost:3000/submit \
  -H "Content-Type: application/json" \
  -d '{"name": "Alice", "email": "alice@example.com"}'
```

### Using a Browser

Simply visit:
- `http://localhost:3000/`
- `http://localhost:3000/about`
- `http://localhost:3000/contact`

## Returning Different Data Types

Express makes it easy to send different types of responses:

```javascript
// Send plain text
app.get('/text', (req, res) => {
    res.send('Hello, World!');
});

// Send JSON (most common for APIs)
// Use this for REST APIs - it's the standard format
app.get('/json', (req, res) => {
    res.json({ 
        message: 'Success', 
        data: [1, 2, 3] 
    });
});

// Send HTML
app.get('/html', (req, res) => {
    res.send('<h1>Hello, World!</h1><p>This is HTML</p>');
});

// Send a file
app.get('/file', (req, res) => {
    res.sendFile(__dirname + '/public/index.html');
});
```

## Status Codes

HTTP responses include status codes that tell the client what happened:

| Code | Meaning | When to Use |
|------|---------|-------------|
| **200** | OK | Successful request |
| **201** | Created | Successfully created something |
| **204** | No Content | Successful but nothing to return |
| **400** | Bad Request | Client sent invalid data |
| **401** | Unauthorized | Not logged in |
| **403** | Forbidden | Logged in but no permission |
| **404** | Not Found | Resource doesn't exist |
| **500** | Server Error | Something broke on our end |

```javascript
// Example: Returning appropriate status codes
app.get('/users/:id', (req, res) => {
    const userId = parseInt(req.params.id);
    
    if (userId > 100) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    res.status(200).json({ id: userId, name: 'Alice' });
});

app.post('/users', (req, res) => {
    // Successfully created a new user
    res.status(201).json({ message: 'User created', id: 1 });
});
```

## Route Parameters

**Route parameters** are dynamic parts of the URL that capture values:

```javascript
// GET /users/123 - Get user with ID 123
// :id is a parameter that captures the value
app.get('/users/:id', (req, res) => {
    // req.params contains all route parameters
    const userId = req.params.id;
    res.json({ userId: userId });
});

// GET /posts/2023/05 - Get posts from May 2023
app.get('/posts/:year/:month', (req, res) => {
    const { year, month } = req.params;
    res.json({ year, month, posts: [] });
});
```

## Query Strings

**Query strings** are optional parameters after the `?` in a URL:

```javascript
// GET /search?name=Alice&age=30
app.get('/search', (req, res) => {
    // req.query contains query string parameters
    const { name, age } = req.query;
    res.json({ name, age });
});
```

## Chaining Handlers

You can chain multiple handlers for a route using an array:

```javascript
// Multiple handlers for the same route
app.get('/example', 
    // First handler - does some processing
    (req, res, next) => {
        console.log('First handler');
        // 'next' passes control to the next handler
        next();
    },
    // Second handler - sends the response
    (req, res) => {
        res.send('Hello from chained handlers!');
    }
);
```

> **What is `next`?** It's a function that passes control to the next middleware or route handler. Without calling it, the request "stops" at that handler.

## What's Next?

- **[Route Handlers](./02_route_handlers.md)** — Organizing route logic
- **[Router Objects](./03_router_objects.md)** — Modular routes
- **[Route Matching](./04_route_matching.md)** — Advanced routing patterns
