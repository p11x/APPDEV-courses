# Express.js vs Plain Node.js

## 📌 What You'll Learn
- The differences between using Express and plain Node.js
- When to use each approach
- Why you might choose one over the other

## 🧠 Concept Explained (Plain English)

Let's think of this like cooking. Plain Node.js is like cooking from raw ingredients — you have total control but it takes a lot of time to prepare everything. Express is like having pre-chopped vegetables and sauces ready — faster, but you work within the framework's conventions.

**Plain Node.js** uses the built-in `http` module to create web servers. It's powerful and gives you complete control, but requires writing more code for common tasks. Every time you want to handle a URL path, parse a request body, or manage cookies, you need to write that code yourself.

**Express.js** sits on top of Node.js and provides abstractions (simplified ways of doing things) for these common tasks. Instead of writing 50 lines of code to parse a JSON request, you just write `app.use(express.json())`.

However, Express isn't always better. Sometimes you need absolute performance or want to understand the fundamentals. Learning plain Node.js first helps you appreciate what Express does for you and gives you deeper understanding when things go wrong.

## 💻 Code Example

```javascript
// ========================================
// COMPARISON: Plain Node.js vs Express
// ========================================

// === PLAIN NODE.JS (verbose but educational) ===
import { createServer } from 'http';

const server = createServer((req, res) => {
    // Manually parse the URL
    const url = new URL(req.url, `http://${req.headers.host}`);
    
    // Manually route based on pathname
    if (url.pathname === '/users' && req.method === 'GET') {
        // Set headers manually
        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({ users: ['Alice', 'Bob'] }));
    } 
    else if (url.pathname === '/users' && req.method === 'POST') {
        // Manually collect and parse request body
        let body = '';
        req.on('data', chunk => body += chunk);
        req.on('end', () => {
            const data = JSON.parse(body);
            console.log('Received:', data);
            res.writeHead(201, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify({ message: 'Created!' }));
        });
    }
    else {
        res.writeHead(404, { 'Content-Type': 'text/plain' });
        res.end('Not Found');
    }
});

server.listen(3000, () => console.log('Plain Node server on 3000'));

/*
// === EXPRESS (same functionality, much cleaner) ===
import express from 'express';
const app = express();

app.use(express.json());  // Parses JSON automatically

app.get('/users', (req, res) => {
    res.json({ users: ['Alice', 'Bob'] });
});

app.post('/users', (req, res) => {
    console.log('Received:', req.body);  // req.body is already parsed!
    res.status(201).json({ message: 'Created!' });
});

app.listen(3000, () => console.log('Express server on 3000'));
*/
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import { createServer } from 'http';` | Import Node's built-in HTTP module |
| 3 | `createServer((req, res) => ...)` | Create an HTTP server with request handler |
| 6 | `new URL(req.url, ...)` | Parse the URL manually in plain Node |
| 10 | `res.writeHead(200, {...})` | Set HTTP status and headers manually |
| 15 | `req.on('data', ...)` | Collect request body chunk by chunk |
| 17 | `req.on('end', ...)` | Process complete body when received |
| 32 | `app.use(express.json())` | In Express: automatic JSON parsing! |

## ⚠️ Common Mistakes

**1. Comparing apples to oranges**
Some beginners think Express replaces Node.js. Actually, Express IS Node.js — it just adds convenience features on top.

**2. Not learning plain Node first**
While Express is easier, understanding plain Node helps you debug and appreciate what's happening under the hood.

**3. Over-engineering simple projects**
For tiny projects or learning purposes, plain Node is fine. You don't always need Express.

## ✅ Quick Recap

- Plain Node.js uses the `http` module directly — more code but complete control
- Express adds helpful abstractions for common tasks
- Express is faster to develop with; plain Node teaches fundamentals
- Both can build the same applications

## 🔗 What's Next

Now let's explore the broader Express ecosystem and its related tools.
