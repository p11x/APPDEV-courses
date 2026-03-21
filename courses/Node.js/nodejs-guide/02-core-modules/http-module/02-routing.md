# HTTP Routing in Node.js

## What You'll Learn

- How to handle different URL paths (routes)
- Processing different HTTP methods (GET, POST, etc.)
- Extracting URL parameters
- Query string handling

## What is Routing?

**Routing** means directing HTTP requests to the appropriate handler based on the URL path and HTTP method. For example:
- `GET /users` - Return a list of users
- `POST /users` - Create a new user
- `GET /users/123` - Return user with ID 123
- `DELETE /users/123` - Delete user with ID 123

## Basic Routing

### Simple Route Handler

```javascript
// basic-routing.js - Basic URL routing

import { createServer } from 'http';

const server = createServer((req, res) => {
  // Get the path (remove query string if present)
  const url = req.url.split('?')[0];
  
  // Set default headers
  res.setHeader('Content-Type', 'application/json');
  
  // Route based on URL
  if (url === '/') {
    res.statusCode = 200;
    res.end(JSON.stringify({ message: 'Welcome to the API' }));
  } 
  else if (url === '/about') {
    res.statusCode = 200;
    res.end(JSON.stringify({ 
      about: 'A simple Node.js API',
      version: '1.0.0'
    }));
  }
  else if (url === '/contact') {
    res.statusCode = 200;
    res.end(JSON.stringify({ 
      email: 'contact@example.com'
    }));
  }
  else {
    // 404 for unknown routes
    res.statusCode = 404;
    res.end(JSON.stringify({ 
      error: 'Not Found',
      path: url
    }));
  }
});

server.listen(3000, () => {
  console.log('Server running at http://localhost:3000');
});
```

### Handling Different HTTP Methods

Different HTTP methods do different things:

```javascript
// http-methods.js - Handling different HTTP methods

import { createServer } from 'http';

const server = createServer((req, res) => {
  const url = req.url.split('?')[0];
  const method = req.method;
  
  res.setHeader('Content-Type', 'application/json');
  
  // Route based on both URL and method
  if (url === '/api/users' && method === 'GET') {
    // GET /api/users - Return list of users
    res.statusCode = 200;
    res.end(JSON.stringify({ 
      users: [
        { id: 1, name: 'Alice' },
        { id: 2, name: 'Bob' }
      ]
    }));
  }
  else if (url === '/api/users' && method === 'POST') {
    // POST /api/users - Create a new user
    res.statusCode = 201;
    res.end(JSON.stringify({ 
      message: 'User created',
      user: { id: 3, name: 'New User' }
    }));
  }
  else if (url === '/api/users' && method === 'DELETE') {
    // DELETE /api/users - Delete all users
    res.statusCode = 200;
    res.end(JSON.stringify({ message: 'All users deleted' }));
  }
  else {
    res.statusCode = 404;
    res.end(JSON.stringify({ error: 'Not Found' }));
  }
});

server.listen(3000, () => {
  console.log('Server running at http://localhost:3000');
  console.log('Test with:');
  console.log('  curl http://localhost:3000/api/users');
  console.log('  curl -X POST http://localhost:3000/api/users');
});
```

## Extracting URL Parameters

### Path Parameters

To extract parts of the URL like `/users/123`:

```javascript
// route-params.js - Extracting URL parameters

import { createServer } from 'http';

const server = createServer((req, res) => {
  const url = req.url.split('?')[0];
  const method = req.method;
  
  res.setHeader('Content-Type', 'application/json');
  
  // Parse /users/:id pattern
  // Match pattern: /users/ followed by digits
  const userMatch = url.match(/^\/users\/(\d+)$/);
  
  if (url === '/users' && method === 'GET') {
    // GET /users - Return all users
    res.statusCode = 200;
    res.end(JSON.stringify({ users: [1, 2, 3] }));
  }
  else if (userMatch) {
    // GET /users/:id - Return specific user
    const userId = userMatch[1];  // The captured number
    
    res.statusCode = 200;
    res.end(JSON.stringify({ 
      user: { id: userId, name: `User ${userId}` }
    }));
  }
  else if (url === '/posts' && method === 'GET') {
    // GET /posts - Return all posts
    res.statusCode = 200;
    res.end(JSON.stringify({ posts: [] }));
  }
  else {
    res.statusCode = 404;
    res.end(JSON.stringify({ error: 'Not Found' }));
  }
});

server.listen(3000, () => {
  console.log('Try these URLs:');
  console.log('  http://localhost:3000/users');
  console.log('  http://localhost:3000/users/123');
});
```

### Query String Parameters

To extract parameters after `?` in the URL:

```javascript
// query-params.js - Handling query strings

import { createServer } from 'http';
import { URL } from 'url';

const server = createServer((req, res) => {
  res.setHeader('Content-Type', 'application/json');
  
  // Parse the URL to get query parameters
  const parsedUrl = new URL(req.url, `http://${req.headers.host}`);
  
  // Get query parameters
  const name = parsedUrl.searchParams.get('name');
  const age = parsedUrl.searchParams.get('age');
  const limit = parsedUrl.searchParams.get('limit') || 10;
  
  console.log(`Query: name=${name}, age=${age}, limit=${limit}`);
  
  // Also get the pathname
  const pathname = parsedUrl.pathname;
  
  if (pathname === '/search') {
    res.statusCode = 200;
    res.end(JSON.stringify({
      results: [`Result for: ${name}`, `Age filter: ${age}`],
      limit: parseInt(limit)
    }));
  }
  else if (pathname === '/greet') {
    const greeting = name 
      ? `Hello, ${name}! ${age ? `You are ${age} years old.` : ''}`
      : 'Hello, stranger!';
    
    res.statusCode = 200;
    res.end(JSON.stringify({ message: greeting }));
  }
  else {
    res.statusCode = 404;
    res.end(JSON.stringify({ error: 'Not Found' }));
  }
});

server.listen(3000, () => {
  console.log('Try these query URLs:');
  console.log('  http://localhost:3000/greet?name=Alice');
  console.log('  http://localhost:3000/greet?name=Bob&age=25');
  console.log('  http://localhost:3000/search?name=test&limit=5');
});
```

## Code Example: Complete Router

Here's a more complete routing example:

```javascript
// complete-router.js - Complete routing example

import { createServer } from 'http';
import { URL } from 'url';

const server = createServer((req, res) => {
  // Parse URL
  const parsedUrl = new URL(req.url, `http://${req.headers.host}`);
  const pathname = parsedUrl.pathname;
  const method = req.method;
  
  // Common headers
  res.setHeader('Content-Type', 'application/json');
  res.setHeader('Access-Control-Allow-Origin', '*');
  
  // Handle CORS preflight
  if (method === 'OPTIONS') {
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
    res.setHeader('Access-Control-Allow-Headers', 'Content-Type');
    res.statusCode = 204;
    res.end();
    return;
  }
  
  // Simple router
  let response = null;
  let statusCode = 200;
  
  // Route definitions
  const routes = {
    // Home
    'GET /': { message: 'Welcome to the API' },
    
    // Users
    'GET /api/users': { users: [{ id: 1, name: 'Alice' }] },
    'POST /api/users': { message: 'User created', id: 2 },
    'GET /api/users/:id': (id) => ({ user: { id, name: `User ${id}` } }),
    'DELETE /api/users/:id': (id) => ({ message: `User ${id} deleted` }),
    
    // Posts
    'GET /api/posts': { posts: [] },
    'POST /api/posts': { message: 'Post created' },
  };
  
  // Try to find matching route
  const routeKey = `${method} ${pathname}`;
  
  // Check exact match first
  if (routes[routeKey]) {
    response = routes[routeKey];
    if (typeof response === 'function') {
      response = response('1');  // Default ID
    }
  }
  // Check pattern match for /api/users/:id
  else if (pathname.match(/^\/api\/users\/\d+$/)) {
    const id = pathname.split('/').pop();
    if (method === 'GET') {
      response = { user: { id, name: `User ${id}` } };
    } else if (method === 'DELETE') {
      response = { message: `User ${id} deleted` };
    }
  }
  else {
    statusCode = 404;
    response = { error: 'Not Found', path: pathname };
  }
  
  // Send response
  res.statusCode = statusCode;
  res.end(JSON.stringify(response));
});

server.listen(3000, () => {
  console.log('Complete router at http://localhost:3000');
  console.log('Endpoints:');
  console.log('  GET    /');
  console.log('  GET    /api/users');
  console.log('  POST   /api/users');
  console.log('  GET    /api/users/123');
  console.log('  DELETE /api/users/123');
  console.log('  GET    /api/posts');
  console.log('  POST   /api/posts');
});
```

## Testing Routes

Use curl to test different routes:

```bash
# GET request
curl http://localhost:3000/api/users

# POST request
curl -X POST http://localhost:3000/api/users

# GET with ID
curl http://localhost:3000/api/users/42

# DELETE request
curl -X DELETE http://localhost:3000/api/users/42

# With query string
curl "http://localhost:3000/greet?name=Alice&age=30"
```

## Common Mistakes

### Mistake 1: Not Handling Query Strings

```javascript
// WRONG - query params will cause mismatches
if (req.url === '/api/users')  // Won't match /api/users?name=Alice

// CORRECT - parse the URL
const parsedUrl = new URL(req.url, `http://${req.headers.host}`);
if (parsedUrl.pathname === '/api/users')
```

### Mistake 2: Forgetting to Parse Numbers

URL parameters are always strings. Convert to numbers when needed:

```javascript
// Extract ID and convert to number
const userId = parseInt(url.match(/^\/users\/(\d+)/)[1]);
```

### Mistake 3: Not Setting Proper Status Codes

Always set appropriate status codes:
- `200` - Success
- `201` - Created
- `404` - Not Found
- `500` - Server Error

## Try It Yourself

### Exercise 1: Calculator API
Create routes for:
- `GET /add?a=5&b=3` returns 8
- `GET /subtract?a=5&b=3` returns 2
- `GET /multiply?a=5&b=3` returns 15
- `GET /divide?a=6&b=3` returns 2

### Exercise 2: Todo API
Create a simple todo API with:
- GET /todos - list all
- POST /todos - create new
- DELETE /todos/:id - delete by id

### Exercise 3: Method Handler
Create separate handlers for GET, POST, PUT, DELETE on the same route.

## Next Steps

Now you understand routing. Let's learn how to handle JSON request bodies. Continue to [Handling JSON](./03-handling-json.md).
