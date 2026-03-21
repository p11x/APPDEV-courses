# Running with Nodemon

## 📌 What You'll Learn
- What nodemon does and why it's useful
- How to install and use nodemon
- Configuring nodemon for your project

## 🧠 Concept Explained (Plain English)

Every time you change your code while developing, you need to restart your server to see the changes. This gets annoying quickly — you change one line, stop the server, restart it, test, repeat.

**Nodemon** solves this problem. It watches your files and automatically restarts your server whenever you save changes. Think of it as having an assistant who restarts your server every time you make changes, so you can focus on coding.

This makes development much faster and more pleasant. You just save your file and refresh your browser!

## 💻 Installing and Using Nodemon

### Step 1: Install as a Development Dependency

```bash
# Install nodemon globally (available everywhere)
npm install -g nodemon

# OR install as dev dependency in your project
npm install --save-dev nodemon
```

### Step 2: Use Nodemon Instead of Node

```bash
# Instead of:
node server.js

# Use:
nodemon server.js
```

Now whenever you save changes to any file in your project, nodemon will automatically restart the server!

### Step 3: Add a Script to package.json

```json
{
    "scripts": {
        "start": "node server.js",
        "dev": "nodemon server.js"
    }
}
```

Now you can run:
```bash
npm run dev
```

## How Nodemon Works

```
You edit code        Nodemon detects         Server restarts
    |                     |                       |
    | save file           |                       |
    |-------------------->|                       |
    |                     | detect change         |
    |                     |---------------------->|
    |                     |                       | restart
    |                     |                       |--------------------->|
    |                     |                       |
```

## Configuration Options

### Using package.json

```json
{
    "nodemonConfig": {
        "restartable": "rs",
        "ignore": ["*.test.js", "test/", "node_modules/"],
        "ext": "js,json,html",
        "watch": ["src/", "server.js"],
        "delay": "1000"
    }
}
```

### Using nodemon.json

Create a `nodemon.json` file in your project root:

```json
{
    "watch": ["server.js", "src/"],
    "ignore": ["*.test.js", "node_modules/"],
    "ext": "js,json",
    "env": {
        "NODE_ENV": "development"
    }
}
```

### Command Line Options

```bash
# Watch specific files
nodemon --watch src --watch server.js server.js

# Ignore specific files
nodemon --ignore '*.test.js' server.js

# Delay restart by 1 second
nodemon --delay 1 server.js

# Run a script on start
nodemon --exec "node --inspect" server.js
```

## Common Use Cases

```bash
# Run with debugger
nodemon --inspect server.js

# Run tests on change
nodemon --exec "npm test"

# Run with custom environment
nodemon --env PORT=4000 server.js
```

## ⚠️ Common Mistakes

**1. Forgetting it's running**
Nodemon restarts automatically, so make sure you know when it's running vs. regular node.

**2. Ignoring node_modules**
By default nodemon ignores node_modules, but if your app changes don't show, check your watch configuration.

**3. Too aggressive watching**
Watching too many files can cause unnecessary restarts. Be specific with your watch patterns.

## ✅ Quick Recap

- Nodemon watches files and restarts server on changes
- Install with `npm install -g nodemon` or `--save-dev nodemon`
- Run with `nodemon server.js` instead of `node server.js`
- Add `"dev": "nodemon server.js"` to scripts in package.json

## 🔗 What's Next

Let's explore how to organize your project with a good folder structure.
