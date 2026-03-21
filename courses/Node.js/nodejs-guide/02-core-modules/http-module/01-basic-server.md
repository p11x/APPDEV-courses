# Creating a Basic HTTP Server

## What You'll Learn

- How to create an HTTP server using Node.js built-in http module
- Understanding the request and response objects
- Setting status codes and response headers
- Running and testing your server

## The HTTP Module

Node.js includes a built-in `http` module that lets you create web servers. While frameworks like Express are more popular, understanding the http module helps you understand how web servers work at a lower level.

## Creating Your First Server

### Basic Server Example

Create `server.js`:

```javascript
// server.js - Basic HTTP server

import { createServer } from 'http';

// Create an HTTP server
// The callback function runs for every request
const server = createServer((request, response) => {
  // request: information about the incoming request
  // response: object to send response back
  
  console.log(`📥 ${request.method} ${request.url}`);
  
  // Set response header (Content-Type tells browser what we're sending)
  response.setHeader('Content-Type', 'text/plain');
  
  // Send response body
  response.write('Hello from Node.js HTTP server!');
  
  // End the response (send it to the client)
  response.end();
});

// Start the server on port 3000
const PORT = 3000;
server.listen(PORT, () => {
  console.log(`🚀 Server running at http://localhost:${PORT}/`);
  console.log('Press Ctrl+C to stop');
});
```

Run the server:
```bash
node server.js
```

Now open your browser to http://localhost:3000

### Using Async/Await with the Server

The createServer callback can't be async directly, but we can structure our code differently:

```javascript
// server-async.js - HTTP server with async handling

import { createServer } from 'http';

const server = createServer(async (req, res) => {
  console.log(`${req.method} ${req.url}`);
  
  try {
    // We can use async/await inside the handler
    // (though most operations here are synchronous)
    
    res.setHeader('Content-Type', 'text/html');
    res.write('<h1>Hello World!</h1>');
    res.write('<p>Welcome to my server</p>');
    res.end();
    
  } catch (error) {
    // Handle errors
    res.statusCode = 500;
    res.end('Internal Server Error');
  }
});

server.listen(3000, () => {
  console.log('Server running on http://localhost:3000');
});
```

## Understanding the Request Object

The `request` (often abbreviated as `req`) contains information about the incoming HTTP request:

```javascript
// request-info.js - Understanding the request object

import { createServer } from 'http';

const server = createServer((req, res) => {
  // HTTP method (GET, POST, PUT, DELETE, etc.)
  console.log('Method:', req.method);
  
  // URL path (e.g., '/users', '/api/data')
  console.log('URL:', req.url);
  
  // HTTP version
  console.log('HTTP Version:', req.httpVersion);
  
  // Headers (all HTTP headers)
  console.log('Headers:', req.headers);
  
  // Get a specific header
  console.log('Content-Type:', req.headers['content-type']);
  
  // Response
  res.setHeader('Content-Type', 'text/plain');
  res.end('Check console for request info');
});

server.listen(3000);
```

## Understanding the Response Object

The `response` (often abbreviated as `res`) is what you use to send data back to the client:

```javascript
// response-methods.js - Response object methods

import { createServer } from 'http';

const server = createServer((req, res) => {
  // setHeader(name, value) - Set a response header
  res.setHeader('Content-Type', 'application/json');
  res.setHeader('X-Custom-Header', 'MyValue');
  
  // statusCode - Set HTTP status code
  res.statusCode = 200;  // OK
  // Common status codes:
  // 200 - OK
  // 201 - Created
  // 204 - No Content
  // 400 - Bad Request
  // 401 - Unauthorized
  // 403 - Forbidden
  // 404 - Not Found
  // 500 - Internal Server Error
  
  // write(chunk) - Write response body
  res.write('Some data');
  
  // end() - End response (can include final data)
  res.end('Final data');
  
  // Or use end() with no arguments just to close
});

server.listen(3000);
```

## Setting Status Codes

HTTP responses include a status code that tells the client if the request was successful:

```javascript
// status-codes.js - Setting appropriate status codes

import { createServer } from 'http';

const server = createServer((req, res) => {
  // Different routes return different status codes
  if (req.url === '/success') {
    res.statusCode = 200;
    res.end('Success!');
  } 
  else if (req.url === '/created') {
    res.statusCode = 201;
    res.end('Resource created');
  }
  else if (req.url === '/not-found') {
    res.statusCode = 404;
    res.end('Page not found');
  }
  else if (req.url === '/error') {
    res.statusCode = 500;
    res.end('Server error');
  }
  else {
    // Default 404 for unknown routes
    res.statusCode = 404;
    res.end('Not Found');
  }
});

server.listen(3000);
```

## Code Example: Complete Server

Here's a more complete server example:

```javascript
// complete-server.js - Complete HTTP server example

import { createServer } from 'http';

const server = createServer((req, res) => {
  // Get the request path
  const path = req.url;
  
  // Log the request
  const timestamp = new Date().toISOString();
  console.log(`[${timestamp}] ${req.method} ${path}`);
  
  // Set common headers
  res.setHeader('X-Powered-By', 'Node.js');
  res.setHeader('Content-Type', 'application/json');
  
  // Simple routing
  if (path === '/' || path === '/home') {
    res.statusCode = 200;
    res.end(JSON.stringify({
      message: 'Welcome to the API',
      endpoints: ['/home', '/about', '/contact']
    }));
  }
  else if (path === '/about') {
    res.statusCode = 200;
    res.end(JSON.stringify({
      title: 'About Us',
      description: 'A Node.js HTTP server example'
    }));
  }
  else if (path === '/contact') {
    res.statusCode = 200;
    res.end(JSON.stringify({
      email: 'contact@example.com',
      phone: '+1-555-0123'
    }));
  }
  else {
    // 404 for unknown routes
    res.statusCode = 404;
    res.end(JSON.stringify({
      error: 'Not Found',
      message: `Cannot ${req.method} ${path}`
    }));
  }
});

// Start server
const PORT = 3000;
server.listen(PORT, () => {
  console.log(`Server running at http://localhost:${PORT}/`);
  console.log('Try these endpoints:');
  console.log('  - http://localhost:3000/');
  console.log('  - http://localhost:3000/about');
  console.log('  - http://localhost:3000/contact');
  console.log('  - http://localhost:3000/unknown');
});
```

## Testing Your Server

### Using Curl

Test your server from the command line:

```bash
# Test root endpoint
curl http://localhost:3000/

# Test about endpoint
curl http://localhost:3000/about

# Test unknown endpoint (should return 404)
curl -i http://localhost:3000/unknown
```

### Using Browser

Simply open http://localhost:3000 in your browser.

## Common Mistakes

### Mistake 1: Not Ending the Response

```javascript
// WRONG - request will hang
createServer((req, res) => {
  res.write('Hello');
  // Missing res.end()!
});

// CORRECT
createServer((req, res) => {
  res.write('Hello');
  res.end();
});
```

### Mistake 2: Setting Headers After end()

```javascript
// WRONG - headers must be set before end()
res.end();
res.setHeader('X-Custom', 'value');  // Too late!

// CORRECT
res.setHeader('X-Custom', 'value');
res.end();
```

### Mistake 3: Forgetting Content-Type

Always set Content-Type so the client knows how to handle the response:
- `text/plain` - Plain text
- `text/html` - HTML
- `application/json` - JSON data

## Try It Yourself

### Exercise 1: JSON API
Create a server that returns JSON data with appropriate Content-Type header.

### Exercise 2: Status Codes
Create a server with different routes that return different status codes (200, 404, 500).

### Exercise 3: Query Parameter Echo
Create a server that echoes back any query parameters sent to it.

## Next Steps

Now you can create basic servers. Let's learn about routing to handle different URL paths. Continue to [HTTP Routing](./02-routing.md).
