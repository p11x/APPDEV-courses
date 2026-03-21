# Handling 404 Errors

## 📌 What You'll Learn
- What causes 404 errors in Express
- How to handle routes that don't exist
- The difference between 404 and other error types
- Best practices for handling missing resources

## 🧠 Concept Explained (Plain English)

A 404 error happens when someone tries to access a webpage or resource that doesn't exist on your server. It's like someone visiting your house and ringing the doorbell, but there's no house at that address - they get a "this address doesn't exist" response.

In Express, there are two ways a 404 can occur:

1. **No matching route**: The URL doesn't match any route you've defined (e.g., `/some/weird/url`)
2. **Resource not found**: The URL matches a route, but the specific resource doesn't exist (e.g., `/users/999` where user 999 doesn't exist)

The first case (no matching route) is handled by Express automatically when no route matches. You can catch this with a special "catch-all" route at the end of your routes.

The second case (resource not found) is handled within your route handlers - you check if the resource exists and throw a NotFoundError if it doesn't.

Let's focus on handling the first case - when no route matches the incoming request.

## 💻 Code Example

```javascript
// ES Module - Handling 404 Errors in Express

import express from 'express';

const app = express();

// ========================================
// DEFINE YOUR REGULAR ROUTES FIRST
// ========================================

app.get('/', (req, res) => {
    res.send('Welcome to our website!');
});

app.get('/about', (req, res) => {
    res.send('About Us page');
});

app.get('/users', (req, res) => {
    res.json([
        { id: 1, name: 'Alice' },
        { id: 2, name: 'Bob' }
    ]);
});

// ========================================
// HANDLE 404 FOR SPECIFIC PATTERNS
// ========================================

// You can create specific 404 handlers for certain URL patterns
// These would go after your regular routes but before the catch-all

app.use('/api', (req, res, next) => {
    // This catches any /api/* URLs that don't match existing routes
    const err = new Error('API endpoint not found');
    err.statusCode = 404;
    next(err);
});

// ========================================
// CATCH-ALL 404 HANDLER
// ========================================

// This must be the LAST route in your application
// It catches any URL that didn't match previous routes
app.use((req, res, next) => {
    // Create a 404 error
    const err = new Error(`Cannot ${req.method} ${req.url}`);
    err.statusCode = 404;
    
    // Pass to error handling middleware
    next(err);
});

// ========================================
// ERROR HANDLING MIDDLEWARE
// ========================================

// eslint-disable-next-line no-unused-vars
app.use((err, req, res, next) => {
    // Check if this is a 404 error
    if (err.statusCode === 404) {
        // Handle 404 differently - maybe show a friendly page
        res.status(404).json({
            error: {
                message: err.message || 'Page not found',
                status: 404,
                path: req.url,
                suggestion: 'Check the URL and try again'
            }
        });
    } else {
        // Handle other errors
        res.status(err.statusCode || 500).json({
            error: {
                message: err.message || 'Internal server error',
                status: err.statusCode || 500
            }
        });
    }
});

// ========================================
// ALTERNATIVE: DEDICATED 404 ROUTE
// ========================================

/*
// Another approach - use a dedicated get route at the end
// This only catches GET requests that didn't match

app.get('*', (req, res) => {
    res.status(404).send(`
        <html>
        <head><title>404 - Page Not Found</title></head>
        <body>
            <h1>Page Not Found</h1>
            <p>The page you're looking for doesn't exist.</p>
            <p><a href="/">Go back to home</a></p>
        </body>
        </html>
    `);
});

// But you still need error handling middleware for other methods
// and for any errors thrown in your routes
*/

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 7 | `app.get('/', (req, res) => {` | Regular route - request and response objects |
| 9 | `res.send('Welcome to our website!');` | Send response |
| 23 | `app.use('/api', (req, res, next) => {` | Middleware for specific path prefix |
| 25 | `const err = new Error(...);` | Create error for unmatched API routes |
| 26 | `err.statusCode = 404;` | Set status code to 404 |
| 27 | `next(err);` | Pass error to error handler |
| 33 | `app.use((req, res, next) => {` | Catch-all middleware for all unmatched routes |
| 35 | `const err = new Error(\`Cannot ${req.method} ${req.url}\`);` | Create descriptive error |
| 36 | `err.statusCode = 404;` | Set 404 status |
| 38 | `next(err);` | Pass to error handling middleware |
| 44 | `if (err.statusCode === 404) {` | Check if error is a 404 |
| 45 | `res.status(404).json({...});` | Send 404 response with helpful info |

## ⚠️ Common Mistakes

**1. Putting the 404 handler in the wrong place**
The 404 catch-all must come AFTER all your other routes. If you put it before, it will intercept all requests before they reach your actual routes.

**2. Not handling all HTTP methods**
A catch-all with `app.use()` handles all HTTP methods (GET, POST, PUT, DELETE, etc.). Using `app.get('*', ...)` only handles GET requests.

**3. Forgetting to call next(err)**
Make sure to pass the error to the next middleware so your error handler can respond properly.

**4. Showing too much information in 404 responses**
For security, don't reveal your internal URL structure or file paths in 404 error messages.

**5. Not distinguishing between 404 and other errors**
Handle 404s differently from other errors - they're usually not "errors" in the traditional sense, just "not found" situations.

## ✅ Quick Recap

- 404 errors occur when no route matches the request URL
- Express doesn't have a built-in 404 handler - you must create one
- Use a catch-all middleware or route at the END of your routes to catch unmatched URLs
- Create specific 404 handlers for certain URL patterns (like `/api/*`)
- Distinguish 404 errors from other errors in your error handling middleware
- Always place the 404 handler after all other routes and middleware

## 🔗 What's Next

Now let's look at how to use the express-async-errors package to handle errors automatically in async route handlers, and how to centralize your error handling logic.
