# req.cookies

## 📌 What You'll Learn
- What req.cookies is and when it's available
- How to access cookies sent by the client
- The requirement of cookie-parser middleware for req.cookies to work

## 🧠 Concept Explained (Plain English)

Cookies are small pieces of data stored in the user's browser and sent with every request to the same domain. They are commonly used for session management, storing user preferences, and tracking.

In Express, to access cookies sent by the client, you need to use the `cookie-parser` middleware. This middleware parses the Cookie header from the request and populates `req.cookies` with an object where each key is a cookie name and the value is the cookie's value.

Think of it like a mail sorter. The Cookie header in the request is like a bundle of mail (cookies) sent by the client. The `cookie-parser` middleware opens the bundle, sorts the mail, and puts each letter (cookie) in a labeled bin (the `req.cookies` object) so you can easily find the one you need.

Without the `cookie-parser` middleware, `req.cookies` will be `undefined`, and you won't be able to access any cookies sent by the client.

## 💻 Code Example

```javascript
// ES Module - Accessing Cookies with req.cookies (requires cookie-parser)

import express from 'express';
import cookieParser from 'cookie-parser';

const app = express();

// ========================================
// IMPORTANT: Add Cookie-Parser MIDDLEWARE
// ========================================
// This must be added BEFORE your routes that need to read cookies
app.use(cookieParser());

// We'll use express.json() for parsing JSON bodies in other examples, but for this one we focus on cookies
// app.use(express.json());

// Route to view all cookies
app.get('/cookies', (req, res) => {
    // req.cookies is an object containing all cookies sent by the client
    // Example: If the client sent Cookie: session_id=abc123; theme=dark
    // Then req.cookies is { session_id: 'abc123', theme: 'dark' }
    res.json({ 
        message: 'Here are the cookies sent by the client',
        cookies: req.cookies
    });
});

// Route to get a specific cookie
app.get('/cookies/:name', (req, res) => {
    const cookieName = req.params.name;
    const cookieValue = req.cookies[cookieName];
    
    if (cookieValue === undefined) {
        return res.status(404).json({ error: `Cookie '${cookieName}' not found` });
    }
    
    res.json({ 
        message: `Value of cookie '${cookieName}'`,
        name: cookieName,
        value: cookieValue
    });
});

// Route to check if a cookie exists
app.get('/has-cookie/:name', (req, res) => {
    const cookieName = req.params.name;
    const hasCookie = Object.prototype.hasOwnProperty.call(req.cookies, cookieName);
    
    res.json({ 
        hasCookie: hasCookie,
        message: hasCookie 
            ? `Cookie '${cookieName}' is present` 
            : `Cookie '${cookieName}' is not present`
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import the Express framework |
| 2 | `import cookieParser from 'cookie-parser';` | Import the cookie-parser middleware |
| 4 | `const app = express();` | Create an Express application instance |
| 7 | `app.use(cookieParser());` | Add cookie-parser middleware - MUST be before routes that need to read cookies |
| 10 | `app.get('/cookies', (req, res) => {` | Define a route to view all cookies |
| 12 | `req.cookies` | Access the cookies object |
| 15 | `app.get('/cookies/:name', (req, res) => {` | Define a route to get a specific cookie by name |
| 17 | `const cookieName = req.params.name;` | Get the cookie name from route parameters |
| 18 | `const cookieValue = req.cookies[cookieName];` | Try to get the cookie value from req.cookies |
| 20 | `if (cookieValue === undefined) {` | Check if the cookie was not found |
| 21 | `return res.status(404).json({ error: `Cookie '${cookieName}' not found` });` | Return error if cookie not found |
| 25 | `res.json({ ... });` | Send the cookie value in the response |
| 29 | `app.get('/has-cookie/:name', (req, res) => {` | Define a route to check if a cookie exists |
| 31 | `const cookieName = req.params.name;` | Get the cookie name from route parameters |
| 32 | `const hasCookie = Object.prototype.hasOwnProperty.call(req.cookies, cookieName);` | Check if the cookie exists in the object |
| 35 | `res.json({ ... });` | Send the result |

## ⚠️ Common Mistakes

**1. Forgetting to add cookie-parser middleware**
If you forget to add `app.use(cookieParser())`, `req.cookies` will be `undefined` and you will get errors when trying to access it.

**2. Adding cookie-parser after your routes**
Middleware order matters! You must add `app.use(cookieParser())` BEFORE the routes that need to read cookies.

**3. Not handling missing cookies**
If you try to access a cookie that doesn't exist, `req.cookies[cookieName]` will be `undefined`. Always check for existence if the cookie might be missing.

**4. Confusing req.cookies with signed cookies**
If you use the secret option with cookie-parser (`cookieParser('secret')`), signed cookies are available in `req.signedCookies`, not `req.cookies`.

**5. Forgetting that cookie values are strings**
Cookie values are always strings. If you stored a number or boolean as a cookie, you'll need to convert it back after reading.

## ✅ Quick Recap

- `req.cookies` contains cookies sent by the client in the Cookie header
- You must use the `cookie-parser` middleware to populate `req.cookies`
- Middleware must be added before routes that need to read cookies
- Access cookies via `req.cookies.cookieName`
- Always handle the case where a cookie might be missing

## 🔗 What's Next

Let's learn about accessing the client's IP address and hostname with `req.ip` and `req.hostname`.
