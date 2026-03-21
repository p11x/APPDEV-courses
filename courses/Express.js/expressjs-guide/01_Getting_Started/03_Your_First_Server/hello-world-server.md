# Hello World Server

## 📌 What You'll Learn
- Creating your first working Express server
- Understanding the basic server structure
- How to test your server

## 🧠 Concept Explained (Plain English)

The classic "Hello World" program is the simplest possible example of a programming concept. With Express, we create a server that listens for requests and responds with "Hello World!"

When someone visits your website, their browser sends a **request** to your server. Your Express server receives that request, processes it, and sends back a **response** — in this case, the text "Hello World!"

The server runs continuously, waiting for incoming requests. As long as the server is running, it can respond to anyone who visits your site.

## 💻 Code Example

```javascript
// ES Module - Hello World Server
// Save this as server.js

import express from 'express';

// Create Express application instance
// 'app' is your main server object - all configuration happens here
const app = express();

// Define a port number
// process.env.PORT lets hosting platforms set their own port
// The || 3000 is a fallback when running locally
const PORT = process.env.PORT || 3000;

// ========================================
// Route Definition
// ========================================
// app.get() defines a route that handles GET requests
// When someone visits the root URL (/), this function runs
// req = request object (information from the browser)
// res = response object (what we send back)

app.get('/', (req, res) => {
    // Send a simple text response
    res.send('Hello World! 👋');
});

// ========================================
// Start the Server
// ========================================
// app.listen() starts your server
// It makes your app listen for incoming network requests

app.listen(PORT, () => {
    // This callback runs when server starts successfully
    console.log(`Server running at http://localhost:${PORT}`);
    console.log('Press Ctrl+C to stop');
});
```

## How to Run

1. Make sure you have Node.js installed
2. In your terminal, run:
```bash
node server.js
```
3. Open your browser to http://localhost:3000
4. You should see "Hello World! 👋"

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express'` | Bring Express library into your file |
| 4 | `const app = express()` | Create your Express application |
| 7 | `const PORT = process.env.PORT \|\| 3000` | Set port from env or default to 3000 |
| 13 | `app.get('/', ...)` | Define GET route for root URL |
| 14 | `(req, res) => {...}` | Route handler with request and response |
| 17 | `res.send('...')` | Send response back to client |
| 23 | `app.listen(PORT, ...)` | Start server listening on port |
| 24 | `console.log(...)` | Print message when server starts |

## Understanding the Flow

```
User's Browser                  Your Express Server
     |                                  |
     |--- GET / (request) ------------->|
     |                                  |
     |                                  | Processing...
     |                                  |
     |<-- "Hello World!" (response) ----|
     |                                  |
```

## Adding More Routes

You can add multiple routes to your server:

```javascript
app.get('/', (req, res) => {
    res.send('Home Page');
});

app.get('/about', (req, res) => {
    res.send('About Page');
});

app.get('/contact', (req, res) => {
    res.send('Contact Page');
});
```

Now visiting:
- http://localhost:3000/ → "Home Page"
- http://localhost:3000/about → "About Page"
- http://localhost:3000/contact → "Contact Page"

## ⚠️ Common Mistakes

**1. Forgetting to call app.listen()**
Your routes are defined but the server never starts!

**2. Putting routes after app.listen()**
Routes must be defined BEFORE you call app.listen()

**3. Port already in use**
If you get "EADDRINUSE", either stop the other server or change your port number.

## ✅ Quick Recap

- Express servers listen for requests on a port
- Define routes with app.get(), app.post(), etc.
- res.send() sends a response to the client
- Start server with app.listen()

## 🔗 What's Next

Let's dive deeper into understanding how app.listen works.
