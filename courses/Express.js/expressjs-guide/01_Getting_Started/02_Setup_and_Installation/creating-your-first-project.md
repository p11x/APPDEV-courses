# Creating Your First Express Project

## 📌 What You'll Learn
- How to set up a new Express project from scratch
- Understanding the project structure
- Running your Express server for the first time

## 🧠 Concept Explained (Plain English)

Setting up an Express project is straightforward. You'll use npm to initialize a project and install Express. Think of it like setting up a new workspace — you need to create the folder, bring in your tools (packages), and organize everything so you can start working.

In this section, we'll walk through creating a complete, runnable Express project. By the end, you'll have a running server that you can visit in your browser.

## 💻 Step-by-Step Guide

### Step 1: Create a Project Folder

Open your terminal and create a new folder for your project:

```bash
# Create and enter the project folder
mkdir my-express-app
cd my-express-app
```

### Step 2: Initialize npm

Initialize your project with npm. This creates a `package.json` file that tracks your project:

```bash
# npm init creates package.json
# The -y flag accepts default answers automatically
npm init -y
```

### Step 3: Install Express

Install Express as a dependency:

```bash
# Install Express (latest version)
npm install express
```

### Step 4: Create Your First Server File

Create a file called `server.js` in your project folder:

```javascript
// server.js
// This is an ES Module file
// It uses import/export syntax (modern JavaScript)

import express from 'express';

// Create an Express application
// 'app' is your main server instance
const app = express();

// Define the port number
// process.env.PORT lets hosting platforms set their own port
// The || 3000 is a fallback for local development
const PORT = process.env.PORT || 3000;

// Create a simple route
// When someone visits http://localhost:3000/, this runs
app.get('/', (req, res) => {
    // req = request object (information from the browser)
    // res = response object (what we send back)
    res.send('Hello, Express World! 🚀');
});

// Start the server
// app.listen() makes your app listen for requests on the specified port
app.listen(PORT, () => {
    console.log(`Server is running at http://localhost:${PORT}`);
    console.log('Press Ctrl+C to stop the server');
});
```

### Step 5: Run Your Server

```bash
# Run the server
node server.js
```

You should see:
```
Server is running at http://localhost:3000
Press Ctrl+C to stop the server
```

### Step 6: View in Browser

Open your browser and visit `http://localhost:3000`

You should see: "Hello, Express World! 🚀"

## 🔍 Understanding the Code

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express'` | Brings Express library into your file |
| 4 | `const app = express()` | Creates your Express application instance |
| 7 | `const PORT = process.env.PORT \|\| 3000` | Uses env port or defaults to 3000 |
| 10 | `app.get('/', ...)` | Defines a GET route at the root URL |
| 11-13 | `(req, res) => {...}` | Route handler function |
| 13 | `res.send('...')` | Sends response to the client |
| 17 | `app.listen(PORT, ...)` | Starts server on the specified port |

## Adding a Script to package.json

Instead of running `node server.js` every time, let's add a convenient script:

Open `package.json` and modify the scripts section:

```json
{
    "name": "my-express-app",
    "version": "1.0.0",
    "type": "module",
    "scripts": {
        "start": "node server.js",
        "dev": "node --watch server.js"
    },
    "dependencies": {
        "express": "^5.0.0"
    }
}
```

Now you can run:
```bash
# Start the server
npm start

# Or with auto-restart on file changes
npm run dev
```

## ⚠️ Common Mistakes

**1. Forgetting to add "type": "module"**
Without this in package.json, you'll get errors with import statements. Add `"type": "module"` at the top level.

**2. Server already running**
If you get "Port already in use," either stop the other server or change the PORT number.

**3. Wrong folder location**
Make sure you're in the correct folder when running `node server.js`.

## ✅ Quick Recap

- Create a folder and run `npm init -y`
- Install Express with `npm install express`
- Create server.js with your routes
- Run with `node server.js` or `npm start`
- Visit localhost:3000 in your browser

## 🔗 What's Next

Let's learn more about the files created during setup, starting with package.json.
