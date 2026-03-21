# Installing Express.js

## Setting Up Your Development Environment

Before we start building Express applications, let's make sure your computer is ready. This guide walks you through installing Node.js and setting up your first Express project.

## Installing Node.js

Express.js is built on **Node.js**, which is JavaScript that runs on the server (instead of in a browser). If you don't have Node.js installed, here's how to get it:

### Option 1: Download from Official Website

1. Visit [nodejs.org](https://nodejs.org)
2. Download the **LTS (Long Term Support)** version for your operating system
3. Run the installer and follow the steps

### Option 2: Using a Version Manager (Recommended for Developers)

If you want to easily switch between different Node.js versions, use **nvm** (Node Version Manager):

**Windows**: Download [nvm-windows](https://github.com/coreybutler/nvm-windows)

**macOS/Linux**:
```bash
# Install nvm
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash

# Install the latest Node.js LTS version
nvm install --lts

# Use the installed version
nvm use --lts
```

### Verify Your Installation

Open your terminal and type:

```bash
node --version
npm --version
```

You should see version numbers like `v20.x.x` and `10.x.x`.

## Creating Your First Express Project

### Step 1: Create a Project Folder

```bash
mkdir my-express-app
cd my-express-app
```

### Step 2: Initialize npm

**npm** (Node Package Manager) is a tool that helps you manage libraries and dependencies in your project.

```bash
npm init -y
```

This creates a `package.json` file — think of it as your project's ID card. It keeps track of what libraries you're using.

### Step 3: Install Express

```bash
npm install express
```

This downloads Express and adds it to your project. You'll see a new folder called `node_modules` — this is where all the library code lives.

### Step 4: Understanding package.json

Your `package.json` now looks something like this:

```json
{
  "name": "my-express-app",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
    "test": "echo \"Error: no test specified\" && exit 1"
  },
  "dependencies": {
    "express": "^5.0.0"
  }
}
```

## Using ES Modules

Modern JavaScript uses **ES Modules** (import/export syntax) instead of the older CommonJS approach. Let's set this up:

### Update package.json

Add `"type": "module"` to your package.json:

```json
{
  "name": "my-express-app",
  "version": "1.0.0",
  "type": "module",
  "description": "My first Express app",
  "main": "server.js",
  "scripts": {
    "start": "node server.js",
    "dev": "node --watch server.js"
  },
  "dependencies": {
    "express": "^5.0.0"
  }
}
```

> **What is "type": "module"?** It tells Node.js to use modern ES Modules syntax. Without this, you'd have to use `require()` instead of `import`.

### Create Your First Server

Create a file called `server.js`:

```javascript
// server.js
// Import Express - brings the express library into our file
import express from 'express';

// Create the app instance - this is our server application
const app = express();

// Define the port - process.env.PORT lets deployment platforms set their own port
const PORT = process.env.PORT || 3000;

// Route: Handle GET requests to the home page
// req = the request object (what the browser sent)
// res = the response object (what we send back)
app.get('/', (req, res) => {
    res.send('Welcome to my Express app!');
});

// Start the server
app.listen(PORT, () => {
    console.log(`Server is running at http://localhost:${PORT}`);
});
```

### Run Your Server

```bash
npm start
```

Or for development with auto-restart:

```bash
npm run dev
```

## Project Structure Best Practices

As your app grows, organize your files like this:

```
my-express-app/
├── node_modules/       # Library code (don't touch!)
├── src/
│   ├── routes/         # Route definitions
│   ├── controllers/   # Request handlers
│   ├── middleware/    # Custom middleware
│   └── app.js         # Main app configuration
├── package.json        # Project info
└── server.js           # Entry point
```

## Common npm Commands

| Command | What It Does |
|---------|--------------|
| `npm install` | Install all dependencies from package.json |
| `npm install <package>` | Install a new package |
| `npm uninstall <package>` | Remove a package |
| `npm start` | Run the "start" script |
| `npm run <script>` | Run any script defined in package.json |

## What's Next?

Now that your environment is ready, let's explore:

- **[Basic Routing](../02_Routing/01_basic_routing.md)** — Handle different URLs
- **[Express App Structure](./03_project_structure.md)** — Organize your code professionally
