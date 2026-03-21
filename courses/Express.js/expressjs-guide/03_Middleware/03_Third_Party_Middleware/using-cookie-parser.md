# Using Cookie-Parser Middleware

## 📌 What You'll Learn
- What cookie-parser is and why it's useful
- How to install and use cookie-parser in your Express application
- How to read, set, and clear cookies

## 🧠 Concept Explained (Plain English)

Cookies are small pieces of data stored in the user's browser. They are sent with every request to the same domain and are commonly used for session management, personalization, and tracking.

The `cookie-parser` middleware parses the Cookie header from incoming requests and populates `req.cookies` with an object keyed by cookie names. It also provides a simple way to set and clear cookies in your responses.

Think of it like a mailroom for your web application. When a request comes in, cookie-parser sorts through the cookies (like mail) and organizes them so your route handlers can easily find what they need. When you want to send a cookie back to the client, cookie-parser helps you format it correctly.

## 💻 Code Example

```javascript
// ES Module - Using Cookie-Parser Middleware

import express from 'express';
import cookieParser from 'cookie-parser';

const app = express();

// ========================================
// IMPORTANT: Add Cookie-Parser MIDDLEWARE
// ========================================
// This must be added before your routes that need to read or set cookies
app.use(cookieParser());

// We still need to parse JSON for our routes
app.use(express.json());

// Route to read cookies
app.get('/read-cookies', (req, res) => {
    // req.cookies is an object containing all cookies sent by the client
    res.json({ cookies: req.cookies });
});

// Route to set a cookie
app.get('/set-cookie', (req, res) => {
    // Set a simple cookie
    res.cookie('username', 'Alice');
    
    // Set a cookie with options
    res.cookie('preferences', '{ "theme": "dark" }', {
        httpOnly: true,    // Not accessible via JavaScript
        secure: true,      // Only sent over HTTPS
        maxAge: 1000 * 60 * 60 * 24, // 24 hours in milliseconds
        sameSite: 'lax'    // Helps prevent CSRF
    });
    
    res.json({ message: 'Cookies set' });
});

// Route to clear a cookie
app.get('/clear-cookie', (req, res) => {
    // Clear the cookie named 'username'
    res.clearCookie('username');
    res.json({ message: 'Cookie cleared' });
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
| 7 | `app.use(cookieParser());` | Add cookie-parser middleware |
| 10 | `app.use(express.json());` | Add JSON parsing middleware |
| 13-15 | `app.get('/read-cookies', ...)` | Route to read cookies |
| 18-26 | `app.get('/set-cookie', ...)` | Route to set cookies |
| 29-32 | `app.get('/clear-cookie', ...)` | Route to clear a cookie |
| 35 | `app.listen(PORT, ...)` | Start the server |

## Setting Cookie Options

When setting cookies, you can pass an options object as the second argument to `res.cookie()`:

```javascript
res.cookie('name', 'value', {
    domain: '.example.com',   // Domain for the cookie
    expires: new Date(Date.now() + 900000), // Expiration date
    httpOnly: true,           // Not accessible via JavaScript
    maxAge: 1000 * 60 * 15,   // 15 minutes in milliseconds
    path: '/',                // Path for the cookie
    secure: true,             // Only sent over HTTPS
    signed: true,             // Indicates if the cookie should be signed
    sameSite: 'strict'        // Value of the SameSite attribute
});
```

### Common Cookie Options

| Option | Type | Description |
|--------|------|-------------|
| **domain** | String | Domain for the cookie (e.g., '.example.com') |
| **expires** | Date | Expiration date of the cookie |
| **httpOnly** | Boolean | If true, cookie is not accessible via JavaScript |
| **maxAge** | Number | Expiry time in milliseconds from now |
| **path** | String | Path for the cookie (default: '/') |
| **secure** | Boolean | If true, cookie is only sent over HTTPS |
| **signed** | Boolean | If true, cookie is signed (requires secret) |
| **sameSite** | String | Value of the SameSite attribute ('strict', 'lax', 'none') |

## Signed Cookies

To use signed cookies, you need to provide a secret when initializing cookie-parser:

```javascript
app.use(cookieParser('your-secret-here'));

// Then you can set signed cookies
app.get('/set-signed-cookie', (req, res) => {
    res.cookie('signedCookie', 'value', { signed: true });
    res.json({ message: 'Signed cookie set' });
});

// And read them from req.signedCookies
app.get('/read-signed-cookie', (req, res) => {
    res.json({ signedCookies: req.signedCookies });
});
```

## ⚠️ Common Mistakes

**1. Forgetting to add cookie-parser middleware**
If you forget this middleware, `req.cookies` will be `undefined` and you won't be able to read cookies.

**2. Adding cookie-parser after your routes**
Middleware order matters! You must add `app.use(cookieParser())` BEFORE the routes that need to read or set cookies.

**3. Not securing sensitive cookies**
For cookies that contain sensitive information (like session IDs), you should set `httpOnly: true` and `secure: true` (in production).

**4. Confusing req.cookies and req.signedCookies**
Regular cookies are in `req.cookies`. Signed cookies (when using a secret) are in `req.signedCookies`.

## ✅ Quick Recap

- `cookie-parser` parses the Cookie header and populates `req.cookies`
- Use `res.cookie(name, value, [options])` to set cookies
- Use `res.clearCookie(name)` to clear cookies
- Must be added before routes that need to read or set cookies
- Essential for working with cookies in Express applications

## 🔗 What's Next

Let's learn about using express-session middleware for session management.
