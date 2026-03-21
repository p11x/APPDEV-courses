# res.cookie() - Setting Cookies

## 📌 What You'll Learn
- How to set cookies in responses
- Cookie options (expiration, security)
- Using the cookie-parser middleware

## 🧠 Concept Explained (Plain English)

Cookies are small pieces of data stored in the browser. Think of them like a loyalty card - the server gives the browser a small token, and the browser presents it with future requests so the server remembers who you are.

The `res.cookie()` method sets a cookie in the browser.

## 💻 Code Example

```javascript
// ES Module - Setting Cookies

import express from 'express';
// import cookieParser from 'cookie-parser';

const app = express();

// Middleware to parse cookies
// app.use(cookieParser());

// Set simple cookie
app.get('/login', (req, res) => {
    // req = request, res = response
    res.cookie('username', 'alice');
    res.json({ message: 'Cookie set!' });
});

// Set cookie with options
app.get('/login-permanent', (req, res) => {
    res.cookie('userId', '12345', {
        maxAge: 30 * 24 * 60 * 60 * 1000, // 30 days
        httpOnly: true, // Can't be accessed by JavaScript
        secure: true, // Only sent over HTTPS
        sameSite: 'strict' // CSRF protection
    });
    res.json({ message: 'Secure cookie set!' });
});

// Clear cookie
app.get('/logout', (req, res) => {
    res.clearCookie('username');
    res.json({ message: 'Logged out!' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 15 | `res.cookie('username', 'alice');` | Set simple cookie |
| 21 | `res.cookie('userId', '12345', {...});` | Set cookie with options |
| 34 | `res.clearCookie('username');` | Clear/delete cookie |

## ✅ Quick Recap

- Use res.cookie() to set cookies
- Use options for security (httpOnly, secure, sameSite)
- Use res.clearCookie() to remove cookies

## 🔗 What's Next

Learn about [res.end()](./res-end.md)