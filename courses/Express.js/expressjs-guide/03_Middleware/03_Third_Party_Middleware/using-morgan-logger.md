# Using Morgan Logger Middleware

## 📌 What You'll Learn
- What Morgan is and why it's useful for logging
- How to install and use Morgan in your Express application
- Different logging formats and how to customize them

## 🧠 Concept Explained (Plain English)

**Morgan** is a popular HTTP request logger middleware for Node.js. It simplifies the process of logging incoming requests to your Express application.

Think of it like a flight recorder for your web server. Every time someone makes a request to your server, Morgan automatically logs details like the HTTP method, URL, status code, response time, and more. This is invaluable for debugging, monitoring, and understanding how your application is being used.

Morgan provides several predefined logging formats (like 'combined', 'common', 'dev', 'short', 'tiny') and allows you to create custom formats tailored to your needs.

## 💻 Code Example

```javascript
// ES Module - Using Morgan Logger

import express from 'express';
import morgan from 'morgan';

const app = express();

// ========================================
// IMPORTANT: Add Morgan MIDDLEWARE
// ========================================
// Morgan logs HTTP requests. It should be placed early in your middleware stack
// so it logs all requests, including those that might cause errors.
// We'll use the 'dev' format which is great for development.
app.use(morgan('dev'));

// We still need to parse JSON for our routes
app.use(express.json());

// Example route
app.get('/users', (req, res) => {
    res.json({ users: ['Alice', 'Bob'] });
});

app.post('/users', (req, res) => {
    // This will be logged by Morgan
    res.status(201).json({ message: 'User created' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import the Express framework |
| 2 | `import morgan from 'morgan';` | Import the Morgan middleware |
| 4 | `const app = express();` | Create an Express application instance |
| 7 | `app.use(morgan('dev'));` | Add Morgan middleware with 'dev' format |
| 10 | `app.use(express.json());` | Add JSON parsing middleware |
| 13-15 | `app.get('/users', ...)` | Example GET route |
| 18-20 | `app.post('/users', ...)` | Example POST route |
| 23 | `app.listen(PORT, ...)` | Start the server |

## Morgan Logging Formats

Morgan comes with several predefined formats:

| Format | Example Output | Best For |
|--------|----------------|----------|
| **combined** | `:remote-addr - :remote-user [:date] ":method :url HTTP/:http-version" :status :res[content-length] ":referrer" ":user-agent"` | Detailed logging, similar to Apache's combined log format |
| **common** | `:remote-addr - :remote-user [:date] ":method :url HTTP/:http-version" :status :res[content-length]` | Standard common log format |
| **dev** | `:method :url :status :response-time ms - :res[content-length]` | Development (colored output) |
| **short** | `:remote-addr :remote-user :method :url HTTP/:http-version :status :res[content-length] - :response-time ms` | Shorter than common |
| **tiny** | `:method :url :status :res[content-length] - :response-time ms` | Minimal logging |

## Creating Custom Formats

You can define your own logging format using tokens:

```javascript
// Define a custom format
app.use(morgan(':method :url :status :res[content-length] - :response-time ms'));

// Or using the predefined names from Morgan
app.use(morgan('combined'));
app.use(morgan('common'));
app.use(morgan('dev'));
app.use(morgan('short'));
app.use(morgan('tiny'));
```

## Advanced Usage: Skipping Logs

You can conditionally skip logging based on the request or response:

```javascript
// Skip logging for static assets
app.use(morgan('combined', {
    skip: (req, res) => {
        return req.url.includes('/public/') || req.url.includes('/static/');
    }
}));

// Skip logging for successful requests (only log errors)
app.use(morgan('combined', {
    skip: (req, res) => {
        return res.statusCode < 400; // Skip if status code is less than 400
    }
}));
```

## 🔍 Line-by-Line Breakdown (Custom Format with Skip)

| Line | Code | What It Does |
|------|------|--------------|
| 4 | `app.use(morgan('combined', {` | Start of Morgan middleware with options |
| 6 | `skip: (req, res) => {` | Define a skip function |
| 8 | `return req.url.includes('/public/') || req.url.includes('/static/');` | Skip if URL contains these paths |
| 9 | `}` | End of skip function |
| 10 | `});` | End of options and middleware call |

## ⚠️ Common Mistakes

**1. Placing Morgan after error-handling middleware**
If you place Morgan after your error-handling middleware, it won't log requests that cause errors because the response is already sent.

**2. Using the wrong format for your environment**
Use 'dev' for development (colored output) and 'combined' or 'common' for production.

**3. Forgetting to install Morgan**
Remember to run `npm install morgan` before using it.

**4. Not handling large log files in production**
In production, consider using log rotation or sending logs to a centralized logging system.

## ✅ Quick Recap

- Morgan logs HTTP requests automatically
- Use `app.use(morgan(format))` to add it to your middleware stack
- Choose a format based on your needs (dev for development, combined/common for production)
- Place it early in your middleware stack to log all requests
- You can create custom formats and conditionally skip logging

## 🔗 What's Next

Let's learn about using CORS middleware to handle cross-origin requests.
