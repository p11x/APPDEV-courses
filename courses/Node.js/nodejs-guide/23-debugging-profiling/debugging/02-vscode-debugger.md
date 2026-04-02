# VS Code Debugger

## What You'll Learn

- How to configure VS Code's launch.json for Node.js debugging
- How to set breakpoints and logpoints
- How to debug with attach mode
- How to use conditional breakpoints
- How to debug TypeScript with source maps

## Launch Configuration

Create `.vscode/launch.json` in your project:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "node",
      "request": "launch",
      "name": "Run server.js",
      "program": "${workspaceFolder}/server.js",
      "skipFiles": [
        "<node_internals>/**"
      ]
    },
    {
      "type": "node",
      "request": "launch",
      "name": "Run current file",
      "program": "${file}",
      "skipFiles": [
        "<node_internals>/**"
      ]
    }
  ]
}
```

### Configuration Options

| Option | Purpose |
|--------|---------|
| `program` | Entry point file |
| `args` | Command-line arguments |
| `env` | Environment variables |
| `cwd` | Working directory |
| `skipFiles` | Skip node internals in step-through |
| `sourceMaps` | Enable for TypeScript/bundled code |

## Debugging with Breakpoints

```js
// server.js — Code with breakpoints

import { createServer } from 'node:http';

function processOrder(order) {
  const subtotal = order.items.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0
  );  // ← Set breakpoint here (click left gutter)

  const tax = subtotal * 0.08;
  const total = subtotal + tax;

  return { subtotal, tax, total };  // ← Another breakpoint
}

const server = createServer((req, res) => {
  const order = {
    items: [
      { name: 'Book', price: 29.99, quantity: 2 },
      { name: 'Pen', price: 4.99, quantity: 3 },
    ],
  };

  const result = processOrder(order);

  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.end(JSON.stringify(result));
});

server.listen(3000, () => {
  console.log('Debug me at http://localhost:3000');
});
```

## Logpoints

Logpoints print a message to the debug console **without pausing** the execution. Useful for quick inspections without stopping the server.

1. Right-click on a line number → **Add Logpoint**
2. Type: `'Processing order, subtotal=' + subtotal`
3. The log appears in the Debug Console without stopping

## Attach Mode

For debugging a process that is already running:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "node",
      "request": "attach",
      "name": "Attach to process",
      "port": 9229,
      "restart": true
    }
  ]
}
```

```bash
# Start the process with --inspect
node --inspect server.js

# Then press F5 in VS Code with the "Attach" configuration selected
```

## TypeScript Debugging

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "node",
      "request": "launch",
      "name": "Debug TypeScript",
      "program": "${workspaceFolder}/src/index.ts",
      "preLaunchTask": "tsc: build",
      "outFiles": ["${workspaceFolder}/dist/**/*.js"],
      "sourceMaps": true
    }
  ]
}
```

## How It Works

### VS Code Debug Architecture

```
VS Code Editor
    │
    │ Debug Adapter Protocol (DAP)
    │
    ▼
Node.js Process (--inspect)
    │
    │ Inspector Protocol (WebSocket)
    │
    ▼
V8 Engine (breakpoints, stepping)
```

### Conditional Breakpoints

Right-click a breakpoint → Edit Breakpoint → Enter expression:

```
i === 42           // Break only when i equals 42
user.role === 'admin'  // Break only for admin users
items.length > 100     // Break only for large arrays
```

## Common Mistakes

### Mistake 1: Wrong program Path

```json
// WRONG — relative path that does not resolve
{
  "program": "server.js"
}

// CORRECT — use ${workspaceFolder}
{
  "program": "${workspaceFolder}/server.js"
}
```

### Mistake 2: Not Enabling Source Maps for TypeScript

```json
// WRONG — breakpoints in .ts files never hit
{
  "sourceMaps": false
}

// CORRECT — enable source maps
{
  "sourceMaps": true,
  "outFiles": ["${workspaceFolder}/dist/**/*.js"]
}
```

### Mistake 3: Debugging Without the Debug Panel

```
# WRONG — pressing F5 runs the file without debugging
# Make sure the "Run and Debug" panel is active, not just the terminal

# CORRECT — open Run and Debug (Ctrl+Shift+D), select your config, press F5
```

## Try It Yourself

### Exercise 1: Debug a Request

Start the server in debug mode. Set a breakpoint in the request handler. Send a request with curl and step through the code.

### Exercise 2: Conditional Breakpoint

Write a loop from 1 to 1000. Set a conditional breakpoint at `i === 500`. Verify it only pauses once.

### Exercise 3: Debug Console

At a breakpoint, use the Debug Console to evaluate expressions: call functions, inspect objects, modify variables.

## Next Steps

You can debug in VS Code. For programmatic logging during development, continue to [Debug Module](./03-debug-module.md).
