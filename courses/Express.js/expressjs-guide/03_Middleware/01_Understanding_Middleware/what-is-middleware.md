# What is Middleware?

## 📌 What You'll Learn
- The concept of middleware in Express.js
- How middleware functions work
- The role of middleware in the request-response cycle

## 🧠 Concept Explained (Plain English)

Imagine you're building a factory assembly line. Each station on the line performs a specific task on the product as it moves along. One station might add a component, another might tighten screws, and another might package the final product.

In Express.js, **middleware** functions are like those assembly line stations. They are functions that have access to the request object (`req`), the response object (`res`), and the next middleware function in the cycle (commonly denoted by `next`). 

Middleware can perform tasks such as:
- Executing any code.
- Making changes to the request and response objects.
- Ending the request-response cycle.
- Calling the next middleware in the stack.

If a middleware does not end the request-response cycle, it must call `next()` to pass control to the next middleware. Otherwise, the request will be left hanging.

## 💻 Code Example

```javascript
// ES Module - Simple Middleware Example

import express from 'express';

const app = express();

// ========================================
// A SIMPLE MIDDLEWARE FUNCTION
// ========================================
// This middleware logs the request method and URL
// It then calls next() to pass control to the next middleware

const logger = (req, res, next) => {
    console.log(`${req.method} ${req.url}`);
    next(); // IMPORTANT: Call next to pass control
};

// ========================================
// APPLYING THE MIDDLEWARE
// ========================================
// This middleware will run for every request
app.use(logger);

// A simple route
app.get('/', (req, res) => {
    res.send('Hello World!');
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## How Middleware Works in the Request-Response Cycle

```
Incoming Request
        |
        v
[Middleware 1] --> [Middleware 2] --> [Route Handler] --> [Response]
        |                |                  |                  |
     (logs request)   (validates data)  (processes request)  (sends response)
        |                |                  |                  |
        v                v                  v                  v
     calls next()    calls next()      sends response    (cycle ends)
```

## Types of Middleware

| Type | Description |
|------|-------------|
| **Application-level** | Bound to an instance of `app` using `app.use()` or `app.METHOD()` |
| **Router-level** | Bound to an instance of `express.Router()` |
| **Error-handling** | Has four parameters (`err`, `req`, `res`, `next`) |
| **Built-in** | Comes with Express (e.g., `express.json()`) |
| **Third-party** | Installed via npm (e.g., `morgan`, `cors`) |

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 6 | `const logger = (req, res, next) => {` | Defines a middleware function with three parameters |
| 8 | `console.log(`${req.method} ${req.url}`);` | Logs the HTTP method and URL |
| 9 | `next();` | Passes control to the next middleware |
| 14 | `app.use(logger);` | Applies the middleware to all routes |

## ⚠️ Common Mistakes

**1. Forgetting to call `next()`**
If you don't call `next()` in a middleware that is not ending the request, the request will be left hanging and never reach your routes.

**2. Calling `next()` after sending a response**
Once you send a response (e.g., `res.send()`), you should not call `next()` as the request-response cycle is already complete.

**3. Middleware order matters**
Middleware is executed in the order it is registered. Place error-handling middleware last.

## ✅ Quick Recap

- Middleware functions have access to `req`, `res`, and `next`.
- They can perform tasks, modify objects, end the cycle, or call `next()`.
- Application-level middleware is applied with `app.use()`.
- Always call `next()` unless you are ending the request-response cycle.

## 🔗 What's Next

Let's learn about the order in which middleware is executed.
