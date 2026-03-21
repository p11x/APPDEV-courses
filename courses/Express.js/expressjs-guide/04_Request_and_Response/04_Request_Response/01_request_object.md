# The Request Object in Express.js

## What is the Request Object?

The **request object** (usually named `req`) contains all information about the HTTP request that was sent by the client. It's one of the three main parameters in Express route handlers and middleware.

```javascript
// Route handler with req parameter
app.get('/users/:id', (req, res) => {
    // req = request object - contains info from the client
    // res = response object - what we send back
});
```

> **What is `req`?** It's short for "request" and contains everything the client sent — URL parameters, body data, headers, and more.

## Properties of the Request Object

### req.params - Route Parameters

**Route parameters** are dynamic parts of the URL defined with `:` in the route:

```javascript
// Route: GET /users/123
app.get('/users/:id', (req, res) => {
    // req.params captures the :id from URL
    const userId = req.params.id;
    // userId = "123"
    
    res.json({ userId });
});

// Multiple parameters: GET /posts/2023/05/my-post
app.get('/posts/:year/:month/:slug', (req, res) => {
    const { year, month, slug } = req.params;
    // year = "2023", month = "05", slug = "my-post"
    
    res.json({ year, month, slug });
});
```

### req.query - Query String Parameters

**Query strings** are the `?key=value` part of the URL:

```javascript
// URL: /search?name=Alice&age=30&city=NYC
app.get('/search', (req, res) => {
    // req.query contains all query string parameters
    const { name, age, city } = req.query;
    // name = "Alice", age = "30", city = "NYC"
    
    res.json({ name, age, city });
});
```

### req.body - Request Body Data

The **request body** contains data sent in POST, PUT, or PATCH requests:

```javascript
// IMPORTANT: You need express.json() middleware to use req.body!
// Without it, req.body will be undefined

import express from 'express';
const app = express();

// This middleware parses JSON from request bodies
// After this, req.body contains the parsed JSON
app.use(express.json());

// POST /api/users
app.post('/api/users', (req, res) => {
    // req.body contains the JSON data sent in the request
    const { name, email, age } = req.body;
    
    // Example: Client sent { "name": "Alice", "email": "alice@example.com" }
    // Now we can access it here
    
    res.json({ message: 'User created', data: { name, email, age } });
});
```

### req.headers - HTTP Headers

**HTTP headers** contain metadata about the request:

```javascript
app.get('/headers', (req, res) => {
    // req.headers contains all HTTP headers
    // Header names are lowercase
    
    const contentType = req.headers['content-type'];
    const userAgent = req.headers['user-agent'];
    const authToken = req.headers['authorization'];
    
    res.json({
        contentType,
        userAgent,
        hasToken: !!authToken
    });
});
```

### req.path - URL Path

The path portion of the URL:

```javascript
// Request: GET /api/users/123?id=5
app.get('/api/users/:id', (req, res) => {
    // req.path = "/api/users/123"
    res.json({ path: req.path });
});
```

### req.method - HTTP Method

The HTTP method used:

```javascript
app.use((req, res) => {
    // req.method = "GET", "POST", "PUT", "DELETE", etc.
    res.json({ method: req.method });
});
```

### req.url - Full URL

The complete URL:

```javascript
app.get('/url', (req, res) => {
    // req.url = "/api/users?page=1&limit=10"
    res.json({ url: req.url });
});
```

### req.ip - Client IP Address

The client's IP address:

```javascript
app.get('/ip', (req, res) => {
    // req.ip = "192.168.1.1" (or the client's IP)
    res.json({ ip: req.ip });
});
```

### req.cookies - Cookies

Cookie data (requires cookie-parser middleware):

```bash
npm install cookie-parser
```

```javascript
import cookieParser from 'cookie-parser';
import express from 'express';

const app = express();
app.use(cookieParser());

app.get('/cookies', (req, res) => {
    // req.cookies contains all cookies
    // Example: { sessionId: "abc123", preferences: "dark" }
    res.json({ cookies: req.cookies });
});
```

## Complete Example

```javascript
// server.js
import express from 'express';

const app = express();

// Parse JSON bodies
app.use(express.json());

// ============================================
// Request Object Properties Table
// ============================================
// | Property     | Example Value                    | Description           |
// |--------------|-----------------------------------|----------------------|
// | req.params   | { id: "123" }                    | URL parameters       |
// | req.query    | { name: "Alice" }                | Query string         |
// | req.body     | { email: "..." }                 | Request body         |
// | req.headers  | { "content-type": "..." }        | HTTP headers         |
// | req.method   | "GET"                            | HTTP method          |
// | req.path     | "/api/users"                     | URL path             |
// | req.ip       | "192.168.1.1"                   | Client IP            |
// | req.url      | "/api/users?id=1"                | Full URL             |
// | req.cookies  | { session: "..." }              | Cookies              |
// | req.protocol | "http" or "https"               | Protocol used        |
// | req.hostname | "example.com"                   | Host name            |
// ============================================

// Demo endpoint showing all request properties
app.all('/demo', (req, res) => {
    res.json({
        // URL parameters (from route like /demo/:id)
        params: req.params,
        
        // Query string (?key=value)
        query: req.query,
        
        // Request body (JSON)
        body: req.body,
        
        // HTTP method
        method: req.method,
        
        // Path without query string
        path: req.path,
        
        // Full URL
        url: req.url,
        
        // Protocol (http/https)
        protocol: req.protocol,
        
        // Hostname
        hostname: req.hostname,
        
        // Client IP
        ip: req.ip,
        
        // Headers (showing a few important ones)
        headers: {
            contentType: req.headers['content-type'],
            userAgent: req.headers['user-agent'],
            accept: req.headers['accept']
        }
    });
});

// Route with parameters
app.get('/users/:userId/posts/:postId', (req, res) => {
    const { userId, postId } = req.params;
    res.json({ 
        message: 'Nested route params',
        userId,
        postId
    });
});

// Query string example
app.get('/search', (req, res) => {
    // Access query parameters
    const searchTerm = req.query.q || '';
    const page = parseInt(req.query.page) || 1;
    const limit = parseInt(req.query.limit) || 10;
    
    res.json({
        searchTerm,
        page,
        limit,
        message: `Search results for "${searchTerm}"`
    });
});

// POST with body
app.post('/create', (req, res) => {
    const { name, email, role } = req.body;
    
    // Validate required fields
    if (!name || !email) {
        return res.status(400).json({
            error: 'Name and email are required'
        });
    }
    
    res.status(201).json({
        message: 'Created successfully',
        data: { name, email, role: role || 'user' }
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Testing the Request Object

### Using curl

```bash
# Test params
curl http://localhost:3000/users/123/posts/456

# Test query string
curl "http://localhost:3000/search?q=hello&page=2"

# Test POST with body
curl -X POST http://localhost:3000/create \
  -H "Content-Type: application/json" \
  -d '{"name": "Alice", "email": "alice@example.com"}'
```

## Tips

1. **Always validate** - Never trust `req.body` or `req.query`; validate before using
2. **Parse bodies** - Use `express.json()` for JSON bodies
3. **Type conversion** - `req.params` and `req.query` are always strings; convert as needed
4. **Debug** - Use `console.log(req)` to see all properties during development

## What's Next?

- **[Response Object](./02_response_object.md)** — Sending responses to clients
- **[Routing](../02_Routing/01_basic_routing.md)** — Handling different URLs
