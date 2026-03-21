# What is Express.js?

## 📌 What You'll Learn
- What Express.js is and its role in web development
- How Express simplifies Node.js web development
- Why Express is one of the most popular Node.js frameworks

## 🧠 Concept Explained (Plain English)

Imagine you're building a house. You could start from scratch — laying bricks, mixing cement, cutting wood. Or you could use pre-built components and a blueprint that makes everything faster. **Express.js** is like that pre-built framework for building web servers with Node.js.

**Express.js** (often just called Express) is a lightweight, flexible web application framework built on top of Node.js. Think of it as a thin layer of helpful tools that sits between your code and the raw Node.js functionality. It makes common tasks like routing, handling requests, and managing cookies much easier.

When you use Express, you don't have to worry about the low-level details of how HTTP requests work. Instead, you can focus on building your application's unique features. Express gives you the essentials without being opinionated about how you structure your code — you have the freedom to organize your project however you want.

Express is often called a "minimalist" framework because it provides just enough functionality to get started, while letting you add more features as needed through middleware (we'll cover this later). This makes it perfect for both small projects and large enterprise applications.

## 💻 Code Example

```javascript
// This is an ES Module file
// It uses import/export syntax (modern JavaScript)

import express from 'express';

// Create an Express application instance
// Think of 'app' as your main control center
const app = express();

// Define a simple route
// When someone visits your website's home page, this runs
app.get('/', (req, res) => {
    // Send a friendly message back to the browser
    res.send('Welcome to my Express app!');
});

// Start the server and listen for incoming requests
// The server will run on port 3000 (or whatever PORT you specify)
app.listen(3000, () => {
    console.log('Server is running on http://localhost:3000');
});
```

### Running the Code

1. Create a folder and run `npm init -y`
2. Install Express: `npm install express`
3. Save the code above as `server.js`
4. Run: `node server.js`
5. Visit `http://localhost:3000` in your browser

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Brings Express into your file so you can use its features |
| 4 | `const app = express();` | Creates your Express application instance — all configuration happens here |
| 7 | `app.get('/', ...)` | Defines a route: when someone makes a GET request to '/', this handler runs |
| 8 | `res.send('...')` | Sends a response back to the client (the browser) |
| 12 | `app.listen(3000, ...)` | Starts the server and makes it listen for requests on port 3000 |

## ⚠️ Common Mistakes

**1. Forgetting to start the server**
Beginners sometimes define routes but forget to call `app.listen()`. Without this, your app won't actually run!

**2. Putting routes after app.listen()**
Routes must be defined BEFORE you call `app.listen()`. Otherwise, they won't be registered.

**3. Using CommonJS by mistake**
If you see `require('express')` instead of `import express from 'express'`, you're using CommonJS. Either add `"type": "module"` to your package.json, or use the CommonJS syntax consistently.

## ✅ Quick Recap

- Express.js is a minimal web framework for Node.js
- It simplifies routing, request handling, and middleware
- You create an `app` instance and define routes on it
- The server starts with `app.listen()`

## 🔗 What's Next

Now that you understand what Express is, let's explore why you might choose Express over plain Node.js in the next section.
