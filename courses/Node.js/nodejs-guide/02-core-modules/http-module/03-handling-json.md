# Handling JSON in HTTP Requests

## What You'll Learn

- How to read JSON data from HTTP request bodies
- Parsing JSON and handling errors
- Setting proper Content-Type headers
- Building a JSON API

## Understanding Request Bodies

When a client sends data to your server (like from a form submission or API call), that data is in the **request body**. For POST and PUT requests, you need to read this body to get the data.

## Reading JSON Bodies

### Basic Body Reading

```javascript
// json-body.js - Reading JSON request bodies

import { createServer } from 'http';

const server = createServer(async (req, res) => {
  // Only process POST and PUT requests
  if (req.method === 'POST' || req.method === 'PUT') {
    // Collect all body data
    let body = '';
    
    // Data comes in chunks - accumulate them
    for await (const chunk of req) {
      body += chunk;
    }
    
    console.log('Raw body:', body);
    
    // Parse JSON
    try {
      const data = JSON.parse(body);
      console.log('Parsed data:', data);
      
      // Send response
      res.setHeader('Content-Type', 'application/json');
      res.statusCode = 200;
      res.end(JSON.stringify({ 
        received: data,
        message: 'Data received!'
      }));
      
    } catch (error) {
      // Invalid JSON
      res.statusCode = 400;
      res.end(JSON.stringify({ 
        error: 'Invalid JSON',
        message: error.message 
      }));
    }
  } else {
    res.statusCode = 405;
    res.end(JSON.stringify({ error: 'Method not allowed' }));
  }
});

server.listen(3000, () => {
  console.log('Server at http://localhost:3000');
  console.log('Send POST requests with JSON body');
});
```

### Using Helper Function for Body Reading

```javascript
// read-body.js - Reusable body reading function

import { createServer } from 'http';

// Helper function to read request body as string
async function readBody(req) {
  let body = '';
  for await (const chunk of req) {
    body += chunk;
  }
  return body;
}

// Helper to parse JSON with error handling
async function parseJSON(req) {
  const body = await readBody(req);
  try {
    return JSON.parse(body);
  } catch (error) {
    throw new Error('Invalid JSON');
  }
}

const server = createServer(async (req, res) => {
  res.setHeader('Content-Type', 'application/json');
  
  if (req.url === '/api/users' && req.method === 'POST') {
    try {
      const userData = await parseJSON(req);
      
      // Validate required fields
      if (!userData.name) {
        res.statusCode = 400;
        res.end(JSON.stringify({ error: 'Missing required field: name' }));
        return;
      }
      
      // Process the data (in real app, save to database)
      const newUser = {
        id: Date.now(),
        name: userData.name,
        email: userData.email || '',
        createdAt: new Date().toISOString()
      };
      
      res.statusCode = 201;
      res.end(JSON.stringify({ 
        message: 'User created',
        user: newUser
      }));
      
    } catch (error) {
      res.statusCode = 400;
      res.end(JSON.stringify({ error: error.message }));
    }
  }
  else {
    res.statusCode = 404;
    res.end(JSON.stringify({ error: 'Not Found' }));
  }
});

server.listen(3000, () => {
  console.log('Server running at http://localhost:3000');
});
```

## Complete JSON API Example

Here's a complete REST API that handles JSON:

```javascript
// json-api.js - Complete JSON REST API

import { createServer } from 'http';

// In-memory database (for demo purposes)
const users = new Map();
let nextId = 1;

// Helper: Read and parse request body
async function parseBody(req) {
  let body = '';
  for await (const chunk of req) {
    body += chunk;
  }
  return JSON.parse(body);
}

// Helper: Send JSON response
function sendJSON(res, statusCode, data) {
  res.statusCode = statusCode;
  res.setHeader('Content-Type', 'application/json');
  res.end(JSON.stringify(data));
}

const server = createServer(async (req, res) => {
  // Enable CORS
  res.setHeader('Access-Control-Allow-Origin', '*');
  res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE, OPTIONS');
  res.setHeader('Access-Control-Allow-Headers', 'Content-Type');
  
  // Handle preflight
  if (req.method === 'OPTIONS') {
    sendJSON(res, 204, {});
    return;
  }
  
  // Parse URL
  const url = new URL(req.url, `http://${req.headers.host}`);
  const path = url.pathname;
  const method = req.method;
  
  console.log(`${method} ${path}`);
  
  // ─────────────────────────────────────────
  // GET /api/users - List all users
  // ─────────────────────────────────────────
  if (path === '/api/users' && method === 'GET') {
    const userList = Array.from(users.values());
    sendJSON(res, 200, { users: userList });
  }
  
  // ─────────────────────────────────────────
  // POST /api/users - Create new user
  // ─────────────────────────────────────────
  else if (path === '/api/users' && method === 'POST') {
    try {
      const body = await parseBody(req);
      
      // Validate
      if (!body.name) {
        sendJSON(res, 400, { error: 'Name is required' });
        return;
      }
      
      // Create user
      const user = {
        id: nextId++,
        name: body.name,
        email: body.email || '',
        createdAt: new Date().toISOString()
      };
      
      users.set(user.id, user);
      
      sendJSON(res, 201, { message: 'User created', user });
    } catch (error) {
      sendJSON(res, 400, { error: 'Invalid JSON' });
    }
  }
  
  // ─────────────────────────────────────────
  // GET /api/users/:id - Get single user
  // ─────────────────────────────────────────
  else if (path.match(/^\/api\/users\/\d+$/) && method === 'GET') {
    const id = parseInt(path.split('/').pop());
    const user = users.get(id);
    
    if (user) {
      sendJSON(res, 200, { user });
    } else {
      sendJSON(res, 404, { error: 'User not found' });
    }
  }
  
  // ─────────────────────────────────────────
  // PUT /api/users/:id - Update user
  // ─────────────────────────────────────────
  else if (path.match(/^\/api\/users\/\d+$/) && method === 'PUT') {
    const id = parseInt(path.split('/').pop());
    const user = users.get(id);
    
    if (!user) {
      sendJSON(res, 404, { error: 'User not found' });
      return;
    }
    
    try {
      const body = await parseBody(req);
      
      // Update fields
      if (body.name) user.name = body.name;
      if (body.email !== undefined) user.email = body.email;
      user.updatedAt = new Date().toISOString();
      
      users.set(id, user);
      
      sendJSON(res, 200, { message: 'User updated', user });
    } catch (error) {
      sendJSON(res, 400, { error: 'Invalid JSON' });
    }
  }
  
  // ─────────────────────────────────────────
  // DELETE /api/users/:id - Delete user
  // ─────────────────────────────────────────
  else if (path.match(/^\/api\/users\/\d+$/) && method === 'DELETE') {
    const id = parseInt(path.split('/').pop());
    
    if (users.has(id)) {
      users.delete(id);
      sendJSON(res, 200, { message: 'User deleted' });
    } else {
      sendJSON(res, 404, { error: 'User not found' });
    }
  }
  
  // ─────────────────────────────────────────
  // 404 Not Found
  // ─────────────────────────────────────────
  else {
    sendJSON(res, 404, { error: 'Not Found' });
  }
});

server.listen(3000, () => {
  console.log('JSON API Server at http://localhost:3000');
  console.log('\nEndpoints:');
  console.log('  GET    /api/users         - List all users');
  console.log('  POST   /api/users         - Create user');
  console.log('  GET    /api/users/:id     - Get user');
  console.log('  PUT    /api/users/:id     - Update user');
  console.log('  DELETE /api/users/:id     - Delete user');
});
```

## Testing JSON Endpoints

Use curl to test:

```bash
# Create a user
curl -X POST http://localhost:3000/api/users \
  -H "Content-Type: application/json" \
  -d '{"name": "Alice", "email": "alice@example.com"}'

# List users
curl http://localhost:3000/api/users

# Get user 1
curl http://localhost:3000/api/users/1

# Update user
curl -X PUT http://localhost:3000/api/users/1 \
  -H "Content-Type: application/json" \
  -d '{"name": "Alice Smith"}'

# Delete user
curl -X DELETE http://localhost:3000/api/users/1
```

## Common Mistakes

### Mistake 1: Not Checking Content-Type

```javascript
// Make sure the client sends JSON
const contentType = req.headers['content-type'];
if (!contentType?.includes('application/json')) {
  res.statusCode = 415;  // Unsupported Media Type
  res.end(JSON.stringify({ error: 'Content-Type must be application/json' }));
  return;
}
```

### Mistake 2: Not Handling Parse Errors

```javascript
// WRONG - will crash on invalid JSON
const data = JSON.parse(body);

// CORRECT - wrap in try/catch
let data;
try {
  data = JSON.parse(body);
} catch (error) {
  res.statusCode = 400;
  res.end(JSON.stringify({ error: 'Invalid JSON' }));
  return;
}
```

### Mistake 3: Forgetting async/await

```javascript
// WRONG - doesn't wait for body
const server = createServer((req, res) => {
  const data = JSON.parse(req.body);  // Empty!
});

// CORRECT - use async
const server = createServer(async (req, res) => {
  const data = await parseBody(req);
});
```

## Try It Yourself

### Exercise 1: Todo API with JSON
Create a todo API where you can POST new todos as JSON and GET them back.

### Exercise 2: Validate Input
Add validation to require specific fields in the JSON body.

### Exercise 3: Error Handling
Add proper error handling for invalid JSON and missing fields.

## Next Steps

Congratulations! You've completed the core modules section. Now let's move to Async JavaScript to understand how to handle asynchronous operations. Continue to [What are Callbacks?](../03-async-javascript/callbacks/01-what-are-callbacks.md).
