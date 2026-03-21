# The Event Loop

## What You'll Learn

- What the event loop is and why it matters
- The six phases of the event loop
- The difference between microtasks and macrotasks
- How Node.js handles asynchronous operations

## Understanding the Event Loop

The **event loop** is the heart of Node.js. It's a mechanism that allows Node.js to perform non-blocking I/O operations despite JavaScript being single-threaded. Let's break down what that means.

### What Does "Single-Threaded" Mean?

JavaScript has a single thread (one line of execution). In most programming languages, if you start a long-running task (like reading a large file), the entire program waits until that task completes. This is called **blocking** because it blocks other code from running.

### How Node.js Avoids Blocking

Instead of waiting, Node.js offloads I/O operations to the operating system and continues executing other code. When the I/O operation completes, Node.js is notified and can process the result. This is called **non-blocking I/O**.

The event loop is the mechanism that coordinates all of this. It's called a "loop" because it runs continuously, checking if there are tasks to execute.

## The Six Phases of the Event Loop

The Node.js event loop has six distinct phases. Each phase has a specific purpose:

```
   ┌───────────────────────────┐
   │     Timers Phase          │ ← setTimeout, setInterval callbacks
   └─────────────┬─────────────┘
                 │
   ┌─────────────▼─────────────┐
   │  Pending Callbacks Phase  │ ← I/O callbacks deferred from previous cycle
   └─────────────┬─────────────┘
                 │
   ┌─────────────▼─────────────┐
   │    Idle, Prepare Phase    │ ← Internal use only
   └─────────────┬─────────────┘
                 │
   ┌─────────────▼─────────────┐
   │       Poll Phase          │ ← Retrieve new I/O events, execute I/O callbacks
   └─────────────┬─────────────┘
                 │
   ┌─────────────▼─────────────┐
   │       Check Phase         │ ← setImmediate callbacks
   └─────────────┬─────────────┘
                 │
   ┌─────────────▼─────────────┐
   │  Close Callbacks Phase    │ ← socket.on('close', ...) callbacks
   └───────────────────────────┘
```

## Code Example: Observing the Event Loop

Create a file named `event-loop.js` and add this code:

```javascript
// This example demonstrates the event loop phases

// setTimeout with 0ms delay - runs in "Timers Phase"
setTimeout(() => {
  console.log('1. setTimeout callback executed');
}, 0);

// setImmediate - runs in "Check Phase"
setImmediate(() => {
  console.log('2. setImmediate callback executed');
});

// I/O operation - processed in "Poll Phase"
const fs = await import('fs');  // Dynamic import for ESM
fs.readFile(__filename, () => {
  console.log('3. File read callback executed');
});

// Promise resolved immediately - runs as microtask
Promise.resolve().then(() => {
  console.log('4. Promise .then() microtask executed');
});

// async function demonstrating microtask
async function asyncExample() {
  console.log('5. Synchronous code in async function');
  await Promise.resolve();
  console.log('6. After await - still a microtask');
}
asyncExample();

// Synchronous code runs first
console.log('7. Synchronous code at the end of main script');
```

**Important**: Save this file and run it with:
```bash
node event-loop.js
```

## How It Works

Let's break down the execution order:

1. **Synchronous code runs first**: JavaScript executes all synchronous code in the current function before anything else. So lines 7 and 5 (the first console.log in asyncExample) run first.

2. **Microtasks run after each phase**: Promises (`.then()`, `await`) are microtasks. They run after the current phase completes but before the next phase begins.

3. **Macrotasks (timers) run in phases**: `setTimeout` runs in the Timers phase, `setImmediate` runs in the Check phase.

4. **The actual order may vary**: Depending on the Node.js version and system state, the order of some callbacks may differ. This is why you might see different orders when you run the code.

## Microtasks vs Macrotasks

### Macrotasks (Tasks)
These are scheduled in specific event loop phases:
- `setTimeout()` and `setInterval()`
- `setImmediate()`
- I/O operations (file reading, network requests)
- `process.nextTick()` (special case - runs before other microtasks)

### Microtasks
These run after the current operation completes:
- Promise `.then()`, `.catch()`, `.finally()`
- `await` expressions
- `queueMicrotask()`

## Common Mistakes

### Mistake 1: Assuming setTimeout(fn, 0) Runs Immediately
Many people think `setTimeout(fn, 0)` runs immediately after synchronous code. It doesn't—it runs in the Timers phase, which is the FIRST phase. But microtasks (Promises) run BEFORE the Timers phase.

### Mistake 2: Blocking the Event Loop
Never run synchronous, CPU-intensive code in the main thread. This blocks the entire event loop. For CPU-intensive work, use Worker Threads (covered in advanced guides).

### Mistake 3: Using process.nextTick() Incorrectly
`process.nextTick()` is not part of the event loop—it's a special Node.js mechanism that runs BEFORE the event loop continues. Use it sparingly and only when you need something to run immediately after the current operation.

## Try It Yourself

### Exercise 1: Predict the Order
Create a script with multiple `setTimeout`, `setImmediate`, and Promise callbacks. Try to predict the execution order before running it.

### Exercise 2: Create a Blocking Loop
Create a script with a `while` loop that runs for a few seconds. Use `setTimeout` to print something during the loop. Observe that `setTimeout` doesn't run until the loop finishes (because the event loop is blocked).

### Exercise 3: nested setTimeout
Use nested `setTimeout` calls to create delayed execution. Observe how each one creates a new event loop cycle.

## Next Steps

Now that you understand how Node.js processes tasks internally, let's compare Node.js to the browser environment. Continue to [Node.js vs Browser](../03-nodejs-vs-browser.md) to learn about the differences between running JavaScript in Node.js versus in a web browser.
