# Using CORS Middleware

## 📌 What You'll Learn
- What CORS is and why it's important
- How to install and use the cors middleware in Express
- How to configure CORS options for your application

## 🧠 Concept Explained (Plain English)

**CORS** stands for Cross-Origin Resource Sharing. It's a security feature implemented by web browsers that prevents web pages from making requests to a different domain than the one that served the web page.

For example, if your frontend is hosted on `http://localhost:3000` and your Express API is on `http://localhost:5000`, the browser will block requests from the frontend to the API by default due to the same-origin policy.

The `cors` middleware allows you to configure which origins are allowed to access your API, which HTTP methods are permitted, and which headers can be used. This is essential when building APIs that will be consumed by frontend applications hosted on different domains.

Think of it like a bouncer at a club. The bouncer (CORS middleware) checks the ID (origin) of anyone trying to enter and decides whether to let them in based on a guest list (allowed origins) you provide.

## 💻 Code Example

```javascript
// ES Module - Using CORS Middleware

import express from 'express';
import cors from 'cors';

const app = express();

// ========================================
// IMPORTANT: Add CORS MIDDLEWARE
// ========================================
// This must be added BEFORE your routes
// By default, cors() allows all origins (you can restrict it)
app.use(cors());

// We still need to parse JSON for our routes
app.use(express.json());

// Example route
app.get('/api/users', (req, res) => {
    res.json({ users: ['Alice', 'Bob'] });
});

app.post('/api/users', (req, res) => {
    // This endpoint can now be accessed from different origins
    res.status(201).json({ message: 'User created' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import the Express framework |
| 2 | `import cors from 'cors';` | Import the CORS middleware |
| 4 | `const app = express();` | Create an Express application instance |
| 7 | `app.use(cors());` | Add CORS middleware with default options (allows all origins) |
| 10 | `app.use(express.json());` | Add JSON parsing middleware |
| 13-15 | `app.get('/api/users', ...)` | Example GET route |
| 18-20 | `app.post('/api/users', ...)` | Example POST route |
| 23 | `app.listen(PORT, ...)` | Start the server |

## Configuring CORS Options

You can pass an options object to the `cors()` function to customize its behavior:

```javascript
// Example: Restrict to specific origins
const corsOptions = {
    origin: 'https://example.com', // Only allow this origin
    methods: ['GET', 'POST'],      // Only allow these methods
    allowedHeaders: ['Content-Type', 'Authorization'] // Only allow these headers
};

app.use(cors(corsOptions));
```

### Common CORS Options

| Option | Type | Description |
|--------|------|-------------|
| **origin** | String/Boolean/Array/Function | Configures the Access-Control-Allow-Origin header. Can be a string (e.g., 'http://example.com'), an array of strings, a regex, a function, or true (reflect the request origin) or false (disable CORS). |
| **methods** | String/Array | Configures the Access-Control-Allow-Methods header. Can be a string or an array of strings. |
| **allowedHeaders** | String/Array | Configures the Access-Control-Allow-Headers header. Can be a string or an array of strings. |
| **exposedHeaders** | String/Array | Configures the Access-Control-Expose-Headers header. |
| **credentials** | Boolean | Configures the Access-Control-Allow-Credentials header. Set to true to allow cookies to be sent with requests. |
| **maxAge** | Number | Configures the Access-Control-Max-Age header (in seconds). |
| **preflightContinue** | Boolean | Set to true to pass the preflight response to the next handler. |
| **optionsSuccessStatus** | Number | Provides a status code to use for successful OPTIONS requests (useful for legacy browsers). |

## Example: Detailed CORS Configuration

```javascript
const corsOptions = {
    origin: ['https://example.com', 'https://trusted.com'],
    methods: ['GET', 'POST', 'PUT', 'DELETE'],
    allowedHeaders: ['Content-Type', 'Authorization', 'X-Custom-Header'],
    exposedHeaders: ['X-Total-Count'],
    credentials: true,
    maxAge: 86400, // 24 hours
    optionsSuccessStatus: 200 // For legacy browser support
};

app.use(cors(corsOptions));
```

## Handling Preflight Requests

For certain types of requests (especially those using methods other than GET, HEAD, or POST, or with custom headers), the browser sends a "preflight" request using the OPTIONS method. The cors middleware automatically handles these preflight requests when configured correctly.

## ⚠️ Common Mistakes

**1. Placing CORS middleware after routes**
If you place the CORS middleware after your routes, it won't affect those routes. Middleware order matters — place it before the routes you want to protect.

**2. Using overly permissive settings in production**
In development, you might use `origin: true` to allow all origins, but in production, you should restrict origins to only those you trust.

**3. Forgetting to handle credentials**
If your API uses cookies or HTTP authentication, you need to set `credentials: true` and configure the origin appropriately (cannot use wildcard when credentials are true).

**4. Not understanding the preflight process**
If you're seeing 405 Method Not Allowed errors for certain requests, it might be a CORS preflight issue. Check that your CORS configuration allows the required methods and headers.

## ✅ Quick Recap

- CORS prevents browsers from making cross-origin requests by default
- The `cors` middleware allows you to configure which origins can access your API
- Must be added before your routes
- Use options to restrict origins, methods, and headers for security
- Essential for APIs consumed by frontend applications on different domains

## 🔗 What's Next

Let's learn about using Helmet middleware for security headers.
