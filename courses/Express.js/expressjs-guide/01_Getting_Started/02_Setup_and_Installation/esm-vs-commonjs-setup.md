# ES Modules vs CommonJS Setup

## 📌 What You'll Learn
- The difference between ES Modules and CommonJS
- How to configure each in your Express project
- Which one you should use

## 🧠 Concept Explained (Plain English)

JavaScript has two main ways to import and export code:

**ES Modules (ESM)** is the modern standard. It uses `import` and `export` keywords that you might have seen in frontend JavaScript. It's the official way to handle modules in JavaScript.

**CommonJS (CJS)** was the original way Node.js handled modules. It uses `require()` to import and `module.exports` to export.

Think of it like the difference between speaking modern English and Shakespearean English — both are valid, but one is more commonly used today. ES Modules is the modern standard, but CommonJS still works and many packages still use it.

## 💻 Comparison

### ES Modules Syntax

```javascript
// ES Module (modern JavaScript)
// File: math.js

// Named export
export const add = (a, b) => a + b;
export const subtract = (a, b) => a - b;

// Default export
export default function multiply(a, b) {
    return a * b;
}

// File: app.js
import { add, subtract } from './math.js';
import multiply from './math.js';

console.log(add(2, 3));        // 5
console.log(multiply(2, 3));   // 6
```

### CommonJS Syntax

```javascript
// CommonJS (older Node.js style)
// File: math.js

// Export
module.exports = {
    add: (a, b) => a + b,
    subtract: (a, b) => a - b,
    multiply: (a, b) => a * b
};

// File: app.js
const math = require('./math');

console.log(math.add(2, 3));        // 5
console.log(math.multiply(2, 3));   // 6
```

## Setting Up ES Modules in Express

### Step 1: Add "type": "module" to package.json

```json
{
    "name": "my-express-app",
    "version": "1.0.0",
    "type": "module",
    "main": "server.js",
    "dependencies": {
        "express": "^5.0.0"
    }
}
```

### Step 2: Use .js Extension for Imports

```javascript
// ES Module - file: server.js

import express from 'express';
import userRoutes from './routes/userRoutes.js';  // Note the .js extension!

const app = express();

app.listen(3000, () => {
    console.log('Server running!');
});
```

## Setting Up CommonJS in Express

### Just Use .cjs Extension

```javascript
// CommonJS - file: server.cjs

const express = require('express');

const app = express();

app.get('/', (req, res) => {
    res.send('Hello World!');
});

app.listen(3000, () => {
    console.log('Server running!');
});
```

Then run with:
```bash
node server.cjs
```

## Mixing Both (Advanced)

You can use both in the same project:

```bash
# Keep using CommonJS
mv server.js server.cjs
mv routes/users.js routes/users.cjs

# But use ES Modules for new files
# Just add "type": "module" to package.json
```

## Quick Reference

| Feature | ES Modules | CommonJS |
|---------|------------|----------|
| Import | `import x from 'x'` | `const x = require('x')` |
| Export | `export const x = ...` | `module.exports = ...` |
| File extension | .js (with "type": "module") | .cjs or no extension |
| Async | Yes | No |

## ⚠️ Common Mistakes

**1. Forgetting .js extension in ES Modules**
In ES Modules, you must include the file extension in imports: `'./routes/users.js'` not `'./routes/users'`

**2. Using require() without setting type**
If you use `"type": "module"` in package.json, require() won't work. Use dynamic import() instead.

**3. Not restarting the server**
Changes to package.json require restarting your server.

## ✅ Quick Recap

- ES Modules use `import`/`export` — modern standard
- CommonJS uses `require`/`module.exports` — older but still works
- Add `"type": "module"` to use ES Modules
- Don't mix the two styles in the same file

## 🔗 What's Next

Let's build your first server and see how everything comes together!
