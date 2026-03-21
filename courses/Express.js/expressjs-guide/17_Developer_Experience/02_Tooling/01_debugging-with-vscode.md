# Debugging with VS Code

## 📌 What You'll Learn

- Setting up VS Code launch configurations
- Using breakpoints in Express apps
- Debugging middleware chains

## 🧠 Concept Explained (Plain English)

Debugging is the process of finding and fixing errors in your code. VS Code provides a powerful debugger that lets you pause execution, inspect variables, and step through code line by line. For Express applications, this is especially useful when tracking down bugs in middleware or route handlers.

A breakpoint is a marker that tells the debugger to pause execution at a specific line. When paused, you can hover over variables to see their values, use the debug console to evaluate expressions, and step through code to understand exactly what happens at each line.

The launch.json file tells VS Code how to start your application for debugging - what command to run, what port to use, and what environment variables to set.

## 💻 Code Example

```js
// server.ts - Simple Express server for debugging
import express from 'express';

const app = express();
app.use(express.json());

// Sample data
const users = new Map([
  ['1', { id: '1', name: 'Alice', email: 'alice@example.com' }],
  ['2', { id: '2', name: 'Bob', email: 'bob@example.com' }],
]);

// Middleware to log requests
app.use((req, res, next) => {
  console.log(`${req.method} ${req.path}`);
  next();
});

// GET /users - List all users
app.get('/users', (req, res) => {
  const userList = Array.from(users.values());
  res.json(userList);
});

// GET /users/:id - Get single user
app.get('/users/:id', (req, res) => {
  const { id } = req.params;
  const user = users.get(id);
  
  if (!user) {
    return res.status(404).json({ error: 'User not found' });
  }
  
  res.json(user);
});

// POST /users - Create user
app.post('/users', (req, res) => {
  const { name, email } = req.body;
  
  if (!name || !email) {
    return res.status(400).json({ error: 'Name and email required' });
  }
  
  const id = crypto.randomUUID();
  const user = { id, name, email };
  users.set(id, user);
  
  res.status(201).json(user);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

## VS Code Launch Configuration

```json
// .vscode/launch.json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "node",
      "request": "launch",
      "name": "Debug Express Server",
      "skipFiles": ["<node_internals>/**"],
      "program": "${workspaceFolder}/src/server.ts",
      "runtimeExecutable": "npx",
      "runtimeArgs": ["tsx", "src/server.ts"],
      "console": "integratedTerminal",
      "env": {
        "NODE_ENV": "development",
        "PORT": "3000"
      },
      "preLaunchTask": "tsc: build - tsconfig.json"
    },
    {
      "type": "node",
      "request": "launch",
      "name": "Debug Tests",
      "skipFiles": ["<node_internals>/**"],
      "program": "${workspaceFolder}/node_modules/vitest/vitest.mjs",
      "args": ["run"],
      "console": "integratedTerminal"
    },
    {
      "type": "node",
      "request": "attach",
      "name": "Attach to Running Process",
      "port": 9229,
      "restart": true,
      "skipFiles": ["<node_internals>/**"]
    }
  ]
}
```

## Setting Breakpoints

1. Open the file you want to debug (e.g., server.ts)
2. Click in the gutter (the space to the left of the line numbers) to add a red circle (breakpoint)
3. Press F5 or click the Debug icon in the sidebar
4. Select "Debug Express Server" from the dropdown
5. Make a request to your server (e.g., GET /users)
6. VS Code will pause at your breakpoint

## Debug Console Commands

When paused at a breakpoint, you can:
- Hover over variables to see their values
- Use the Debug Console to evaluate expressions
- Step Over (F10) - execute current line and move to next
- Step Into (F11) - enter function calls
- Step Out (Shift+F11) - exit current function
- Continue (F5) - continue execution until next breakpoint

## ⚠️ Common Mistakes

1. **Not setting breakpoints on executable lines**: Comments and empty lines can't have breakpoints
2. **Forgetting to stop on uncaught exceptions**: Configure "Break on Exception" in the Debug panel
3. **Debugging production code**: Always debug in development with NODE_ENV=development

## ✅ Quick Recap

- Create `.vscode/launch.json` to configure debugging
- Use breakpoints by clicking in the gutter
- Step through code with F10 (over), F11 (into), Shift+F11 (out)
- Use the Debug Console to evaluate expressions while paused
- Attach to running processes using port 9229

## 🔗 What's Next

Learn about ESLint and Prettier setup for consistent code formatting.
