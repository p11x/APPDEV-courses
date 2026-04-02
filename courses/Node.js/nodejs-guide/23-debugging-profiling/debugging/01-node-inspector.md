# Node Inspector

## What You'll Learn

- How to use the `--inspect` flag for debugging
- How to connect Chrome DevTools to a Node.js process
- How to set breakpoints and step through code
- How to use the watch panel and console
- How to debug with `--inspect-brk` to pause on the first line

## Starting the Debugger

```bash
# Start with inspector enabled — process runs normally, debugger can attach
node --inspect server.js

# Start with inspector AND break on the first line — pauses immediately
node --inspect-brk server.js

# Custom inspector port (default is 9229)
node --inspect=0.0.0.0:9230 server.js
```

Output:

```
Debugger listening on ws://127.0.0.1:9229/abc123...
For help, see: https://nodejs.org/en/docs/inspector
```

## Connecting Chrome DevTools

1. Start your script with `node --inspect server.js`
2. Open Chrome and navigate to `chrome://inspect`
3. Click **"Open dedicated DevTools for Node"**
4. Or click the **inspect** link under your process

## Debugging Example

```js
// server.js — Code to debug

import { createServer } from 'node:http';

function calculateTotal(items) {
  let total = 0;  // ← Set a breakpoint here (click the line number)
  for (const item of items) {
    total += item.price * item.quantity;
  }
  return total;
}

function findDiscount(total) {
  if (total > 100) return 0.1;  // 10% discount
  if (total > 50) return 0.05;  // 5% discount
  return 0;
}

const server = createServer((req, res) => {
  const items = [
    { name: 'Widget', price: 25, quantity: 3 },
    { name: 'Gadget', price: 50, quantity: 1 },
  ];

  const total = calculateTotal(items);     // ← Breakpoint here
  const discount = findDiscount(total);    // ← Inspect discount value
  const final = total * (1 - discount);

  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.end(JSON.stringify({ total, discount, final }));
});

server.listen(3000, () => {
  console.log('Server on http://localhost:3000 — attach debugger now');
});
```

## DevTools Features

### Breakpoints

| Type | How | When It Pauses |
|------|-----|---------------|
| Line breakpoint | Click line number | Execution reaches that line |
| Conditional breakpoint | Right-click line → "Add conditional breakpoint" | Only when expression is true |
| Logpoint | Right-click line → "Add logpoint" | Logs a message (does NOT pause) |

### Stepping Controls

| Button | Shortcut | Action |
|--------|----------|--------|
| Resume | F8 | Continue until next breakpoint |
| Step Over | F10 | Execute current line, move to next |
| Step Into | F11 | Enter the function call |
| Step Out | Shift+F11 | Exit the current function |

### Watch Panel

Add expressions to watch their values in real time:

```
total
discount
items.length
items[0].name
```

### Console

In the DevTools console, you can:
- Execute code in the current scope: `items.map(i => i.name)`
- Inspect variables: `total`
- Call functions: `findDiscount(200)`

## How It Works

### The Inspector Protocol

```
Your Node.js Process          Chrome DevTools
        │                           │
        │── WebSocket (ws://) ──────│
        │                           │
        │←── Set breakpoint ────────│
        │── Hit breakpoint ────────→│
        │←── Step over ─────────────│
        │── Variable values ───────→│
```

The `--inspect` flag starts a WebSocket server that DevTools connects to. DevTools sends commands (set breakpoint, step, evaluate) and receives events (paused, resumed).

### --inspect vs --inspect-brk

```bash
# --inspect: runs immediately, debugger attaches whenever
node --inspect server.js
# Server starts listening, you can attach later

# --inspect-brk: pauses on line 1, waits for debugger
node --inspect-brk server.js
# Server does NOT start until you click Resume in DevTools
```

Use `--inspect-brk` when you need to debug startup code.

## Common Mistakes

### Mistake 1: Forgetting to Open DevTools

```bash
# You run:
node --inspect server.js
# But never open chrome://inspect
# The debugger is listening but no one is attached — breakpoints do nothing
```

### Mistake 2: Using --inspect in Production

```bash
# WRONG — exposes debugging interface to anyone who can connect
node --inspect=0.0.0.0:9229 server.js

# CORRECT — only use --inspect in development
# Or bind to localhost only (default)
node --inspect server.js
```

### Mistake 3: Not Resuming After Breakpoint

```js
// You set a breakpoint in a frequently-called function
// Every request pauses — the server appears frozen

// CORRECT — remove breakpoints when done debugging
// Or use conditional breakpoints: i === 42
```

## Try It Yourself

### Exercise 1: Debug a Loop

Write a function that sums an array. Set a breakpoint inside the loop. Step through 3 iterations and watch the accumulator change.

### Exercise 2: Debug an Async Function

Write an async function that fetches data. Set a breakpoint after the await. Verify that DevTools shows the resolved value.

### Exercise 3: Conditional Breakpoint

Write a loop from 1 to 100. Set a conditional breakpoint that only pauses when `i === 50`.

## Next Steps

You can debug with Chrome DevTools. For debugging in VS Code, continue to [VS Code Debugger](./02-vscode-debugger.md).
