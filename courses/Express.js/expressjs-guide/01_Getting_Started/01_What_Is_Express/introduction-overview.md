# Welcome to Express.js

## What is Express.js?

Express.js (often just called **Express**) is a fast, lightweight, and flexible web application framework for **Node.js**. Think of it as a toolkit that makes building websites and web applications much easier.

### Why Use Express?

- **Minimalist**: It gives you the essentials without overwhelming you
- **Flexible**: You can structure your app your way
- **Fast**: Built on Node.js, it handles many requests efficiently
- **Popular**: Huge community and lots of resources

## What Can You Build with Express?

| Type of App | Examples |
|-------------|----------|
| RESTful APIs | JSON APIs for mobile apps, SPAs |
| Web Apps | Full-stack websites with templates |
| Real-time Apps | Chat apps, live notifications |
| Microservices | Small, focused services that work together |

## Prerequisites

Before diving into Express, you should know:

- **JavaScript fundamentals**: Variables, functions, loops, objects
- **Node.js basics**: How to run JavaScript outside the browser
- **HTTP concepts**: What are requests and responses (we'll cover this too!)

> Don't worry if you're new to some of these concepts — we'll explain everything as we go!

## Your First Express App

Let's create a simple "Hello World" application to see Express in action.

### Step 1: Set Up Your Project

```bash
mkdir my-first-express-app
cd my-first-express-app
npm init -y
npm install express
```

### Step 2: Create Your First Server

Create a file called `server.js`:

```javascript
// server.js
// Import Express - this gives us access to all Express features
import express from 'express';

// Create an Express application
// 'app' is our main entry point for configuring the server
const app = express();

// Define a port number
// We use process.env.PORT for deployment flexibility (more on this later!)
const PORT = process.env.PORT || 3000;

// .get() defines a route that handles GET requests
// When someone visits http://localhost:3000/, this runs
app.get('/', (req, res) => {
    // Send a response back to the browser
    res.send('Hello, Express World!');
});

// .listen() starts the server
// It makes the app listen for incoming requests on the specified port
app.listen(PORT, () => {
    console.log(`Server running at http://localhost:${PORT}`);
});
```

### Step 3: Run Your Server

```bash
node server.js
```

Visit `http://localhost:3000` in your browser. You should see "Hello, Express World!"

## Understanding the Code

### `import express from 'express'`

This line brings Express into our file. It's like unpacking a toolbox — now we can use all the tools Express provides.

### `const app = express()`

This creates our **application instance**. Think of it as the brain of our server. All configuration happens through `app`.

### `req`, `res`, and `next`

These are three crucial objects you'll see throughout Express:

| Object | Full Name | What It Does |
|--------|-----------|--------------|
| `req` | Request | Contains information about the incoming request (what the client sent) |
| `res` | Response | Used to send data back to the client |
| `next` | Next | Passes control to the next middleware function |

We'll explore these in detail in the [Request & Response](../04_Request_Response/01_request_object.md) section.

### `process.env.PORT`

**Environment variables** are values stored outside your code. We use them instead of hardcoded values because:

1. Different computers may need different settings
2. Security-sensitive data shouldn't be in your code
3. It makes deployment easier (services like Heroku provide their own PORT)

## What's Next?

Now that you have Express running, let's explore:

1. **[Routing](../02_Routing/01_basic_routing.md)** — How to handle different URLs
2. **[Middleware](../03_Middleware/01_introduction.md)** — Functions that process requests
3. **[Request & Response](../04_Request_Response/01_request_object.md)** — Working with data

---

> 🎉 Congratulations! You've built your first Express server. Keep experimenting and have fun!