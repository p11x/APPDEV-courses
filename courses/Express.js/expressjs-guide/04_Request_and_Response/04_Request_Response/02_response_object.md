# The Response Object in Express.js

## What is the Response Object?

The **response object** (usually named `res`) is used to send data back to the client. It's the second parameter in Express route handlers and middleware.

```javascript
// Route handler with res parameter
app.get('/hello', (req, res) => {
    // req = request object - what the client sent
    // res = response object - what we send back
    res.send('Hello, World!');
});
```

> **What is `res`?** It's short for "response" and is how we communicate with the client — sending data, setting headers, or ending the request.

## Sending Responses

### res.send() - Send Any Response

The most versatile method:

```javascript
app.get('/text', (req, res) => {
    // Send plain text
    res.send('Hello, World!');
});

app.get('/json', (req, res) => {
    // Send object - Express automatically converts to JSON
    res.send({ message: 'Hello!' });
});

app.get('/html', (req, res) => {
    // Send HTML
    res.send('<h1>Hello!</h1>');
});
```

### res.json() - Send JSON

Specifically for JSON responses:

```javascript
app.get('/api/user', (req, res) => {
    // Send JSON with proper content-type header
    res.json({
        id: 1,
        name: 'Alice',
        email: 'alice@example.com'
    });
});

app.get('/api/error', (req, res) => {
    // Also works with null or arrays
    res.json([]);
});
```

### res.sendFile() - Send Files

Serve files from the filesystem:

```javascript
import path from 'path';

app.get('/file', (req, res) => {
    // Send a file
    // __dirname is the current directory
    res.sendFile(path.join(__dirname, 'public', 'index.html'));
});
```

## Setting Status Codes

### Common Status Codes

| Code | Meaning | When to Use |
|------|---------|-------------|
| **200** | OK | Successful request (default) |
| **201** | Created | Successfully created something |
| **204** | No Content | Success but no content to return |
| **400** | Bad Request | Invalid client data |
| **401** | Unauthorized | Not authenticated |
| **403** | Forbidden | No permission |
| **404** | Not Found | Resource doesn't exist |
| **500** | Server Error | Something broke |

### Setting Status

```javascript
app.get('/created', (req, res) => {
    res.status(201).json({ message: 'Created!' });
});

app.get('/not-found', (req, res) => {
    res.status(404).json({ error: 'User not found' });
});

app.get('/error', (req, res) => {
    res.status(500).json({ error: 'Server error' });
});
```

## Setting Headers

### res.set() / res.header()

Set custom headers:

```javascript
app.get('/custom-headers', (req, res) => {
    // Set custom header
    res.set('X-Custom-Header', 'Hello');
    
    // Or use res.header()
    res.header('X-Another-Header', 'World');
    
    res.json({ message: 'Check headers!' });
});
```

### Common Header Patterns

```javascript
app.get('/json-data', (req, res) => {
    // Set content type (usually automatic with res.json)
    res.setHeader('Content-Type', 'application/json');
    
    // Disable caching (useful for APIs)
    res.setHeader('Cache-Control', 'no-cache');
    
    // Set CORS headers manually (or use cors middleware)
    res.setHeader('Access-Control-Allow-Origin', '*');
    
    res.json({ data: 'value' });
});
```

## Cookies

### res.cookie()

Set cookies:

```bash
npm install cookie-parser
```

```javascript
import cookieParser from 'cookie-parser';
import express from 'express';

const app = express();
app.use(cookieParser());

app.get('/set-cookie', (req, res) => {
    // Set a simple cookie
    res.cookie('username', 'Alice');
    
    // Set cookie with options
    res.cookie('session', 'abc123', {
        httpOnly: true,    // Can't be accessed by JavaScript
        secure: true,      // Only over HTTPS
        maxAge: 3600000,  // 1 hour in milliseconds
        sameSite: 'strict' // CSRF protection
    });
    
    res.json({ message: 'Cookie set!' });
});

app.get('/clear-cookie', (req, res) => {
    // Clear a cookie
    res.clearCookie('username');
    res.json({ message: 'Cookie cleared!' });
});
```

## Redirects

### res.redirect()

Redirect the client to another URL:

```javascript
app.get('/old-page', (req, res) => {
    // Redirect to new page (302 by default)
    res.redirect('/new-page');
});

app.get('/moved', (req, res) => {
    // Permanent redirect (301)
    res.redirect(301, '/new-location');
});

app.get('/external', (req, res) => {
    // Redirect to external URL
    res.redirect('https://example.com');
});
```

## Response Chaining

Chain methods together:

```javascript
app.get('/chained', (req, res) => {
    // Chain status and json
    res
        .status(201)
        .json({ message: 'Created successfully' });
});
```

## Complete Example

```javascript
// server.js
import express from 'express';
import cookieParser from 'cookie-parser';

const app = express();
app.use(express.json());
app.use(cookieParser());

// ============================================
// Response Methods Table
// ============================================
// | Method          | Purpose                        |
// |-----------------|--------------------------------|
// | res.send()     | Send any type of response      |
// | res.json()     | Send JSON response             |
// | res.jsonp()    | Send JSONP response            |
// | res.sendFile() | Send a file                    |
// | res.redirect()| Redirect to another URL        |
// | res.status()   | Set HTTP status code           |
// | res.set()      | Set response headers           |
// | res.cookie()   | Set a cookie                   |
// | res.clearCookie| Clear a cookie                  |
// | res.end()      | End response without data      |
// | res.download() | Download a file                 |
// ============================================

// JSON Response
app.get('/api/users', (req, res) => {
    res.json({
        users: [
            { id: 1, name: 'Alice' },
            { id: 2, name: 'Bob' }
        ]
    });
});

// JSON with status
app.post('/api/users', (req, res) => {
    const newUser = { id: 3, ...req.body };
    // 201 = Created
    res.status(201).json(newUser);
});

// Plain text
app.get('/text', (req, res) => {
    res.type('text/plain').send('Plain text response');
});

// HTML response
app.get('/html', (req, res) => {
    res.send(`
        <html>
            <body>
                <h1>Hello!</h1>
                <p>This is HTML</p>
            </body>
        </html>
    `);
});

// Redirect
app.get('/old', (req, res) => {
    res.redirect('/new');
});

app.get('/new', (req, res) => {
    res.send('Welcome to the new page!');
});

// Set cookies
app.get('/login', (req, res) => {
    res.cookie('token', 'abc123', {
        httpOnly: true,
        secure: process.env.NODE_ENV === 'production'
    });
    res.json({ message: 'Logged in' });
});

// Stream a file download
app.get('/download', (req, res) => {
    // This would download a file
    // res.download('/path/to/file.txt');
    res.json({ message: 'Download would start' });
});

// Error response
app.get('/not-found', (req, res) => {
    res.status(404).json({ error: 'Resource not found' });
});

// Custom headers
app.get('/special', (req, res) => {
    res.set('X-Rate-Limit', '100');
    res.set('X-Expires-After', '2025-01-01');
    res.json({ data: 'Special response!' });
});

// No content (useful for DELETE)
app.delete('/api/users/:id', (req, res) => {
    // 204 No Content - successful but nothing to return
    res.status(204).send();
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Response Best Practices

| Practice | Why |
|----------|-----|
| Always set status codes | Makes response meaning clear |
| Use res.json() for APIs | Sets correct Content-Type |
| Return consistent structure | Makes client code easier |
| Don't mix response methods | Use either send() or json(), not both |
| Handle errors with proper codes | 400-500 range for errors |

## Testing Responses

### Using curl

```bash
# JSON response
curl http://localhost:3000/api/users

# Text response
curl http://localhost:3000/text

# Check headers
curl -I http://localhost:3000/api/users
```

## What's Next?

- **[Templating](../05_Templating/01_view_engines.md)** — Server-side rendering
- **[Error Handling](../07_Error_Handling/01_basics.md)** — Handling errors gracefully
