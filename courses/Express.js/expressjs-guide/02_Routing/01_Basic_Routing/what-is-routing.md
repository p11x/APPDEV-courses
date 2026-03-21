# What is Routing?

## 📌 What You'll Learn
- What routing means in web development
- How Express handles different URLs
- The relationship between URLs and code

## 🧠 Concept Explained (Plain English)

**Routing** is how your web application responds to different URLs. When someone visits your website, they're requesting a specific URL — like "/about" or "/contact". Your server needs to know what to do for each URL.

Think of it like a telephone switchboard. When a call comes in, the operator connects it to the right department based on the number dialed. In web routing, your server connects incoming requests to the right code based on the URL.

Every time you visit a different page on a website, your browser makes a request to the server, and the server routes it to the appropriate handler. Routing makes this possible.

## 💻 Simple Routing Example

```javascript
// ES Module - Understanding Routing

import express from 'express';

const app = express();

// ========================================
// Routing in Express
// ========================================
// Routing maps URLs to code that handles them
// app.METHOD(PATH, HANDLER)
// - METHOD: HTTP method (get, post, put, delete)
// - PATH: The URL pattern to match
// - HANDLER: Function that runs when path matches

// GET request to the home page
app.get('/', (req, res) => {
    // req = request object (information from browser)
    // res = response object (what we send back)
    res.send('Welcome to the Home Page!');
});

// GET request to /about page
app.get('/about', (req, res) => {
    res.send('About Us Page');
});

// GET request to /contact page
app.get('/contact', (req, res) => {
    res.send('Contact Page');
});

// Start the server
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running at http://localhost:${PORT}`);
});
```

## How Routing Works

```
User types in browser:  http://localhost:3000/about
                        |
                        v
Express receives request |
                        |
                        v
Looks at URL and method |
GET /about              |
                        v
Finds matching route    |
app.get('/about', ...) |
                        v
Runs handler function  |
res.send('About...')   |
                        v
Sends response back    |
```

## Route Matching

Routes are matched in order:

```javascript
// First match wins!
app.get('/users', (req, res) => res.send('All users'));
app.get('/users', (req, res) => res.send('This never runs'));

// More specific routes should come first
app.get('/users/admin', (req, res) => res.send('Admin users'));
app.get('/users', (req, res) => res.send('All users'));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 9 | `app.get('/', ...)` | Defines GET route for root URL |
| 10 | `(req, res) => {...}` | Handler function receives request and response |
| 11 | `res.send('...')` | Sends text response to browser |

## Common HTTP Methods

| Method | Purpose | Example |
|--------|---------|---------|
| **GET** | Retrieve data | Loading a page |
| **POST** | Create new data | Submitting a form |
| **PUT** | Replace data | Editing an article |
| **PATCH** | Modify part of data | Updating profile |
| **DELETE** | Remove data | Deleting a post |

## ⚠️ Common Mistakes

**1. Route order matters**
Put more specific routes before general ones.

**2. Case sensitivity**
Routes are case-sensitive. '/About' won't match '/about'.

**3. Trailing slashes**
'/users' and '/users/' are different routes in Express.

## ✅ Quick Recap

- Routing maps URLs to code handlers
- app.get(), app.post() define routes for different HTTP methods
- Each route has a path and handler function
- Order of routes matters

## 🔗 What's Next

Let's explore the different HTTP methods in more detail.
