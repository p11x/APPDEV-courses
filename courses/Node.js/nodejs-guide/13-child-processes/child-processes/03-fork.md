# fork()

## What You'll Learn

- What `fork()` does and how it differs from `spawn()`
- How to send messages between parent and child processes via IPC
- How to pass complex objects (not just strings) between processes
- How to build a worker script pattern with `process.on('message')`
- When to use `fork()` vs worker threads

## What Is fork()?

`fork()` is a special case of `spawn()` designed specifically for running **Node.js scripts**. It automatically sets up an **IPC (Inter-Process Communication) channel** so the parent and child can exchange structured messages — objects, arrays, and any value that can be JSON-serialized.

```
spawn('node', ['worker.js'])   →  stdout/stderr streams only
fork('worker.js')              →  stdout/stderr + IPC messages
```

## Basic fork() Example

### Parent Process

```js
// parent.js — Fork a child and exchange messages

import { fork } from 'node:child_process';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// fork() runs another Node.js process with an IPC channel enabled
const child = fork(resolve(__dirname, 'child.js'));

// Send an object to the child — IPC serializes it as JSON
child.send({ task: 'greet', name: 'Alice' });

// Listen for messages from the child
child.on('message', (msg) => {
  // msg is a deserialized object — not a string or Buffer
  console.log('Parent received:', msg);
});

// Handle child exit
child.on('close', (code) => {
  console.log(`Child exited with code ${code}`);
});
```

### Child Process

```js
// child.js — Runs inside the forked process

// process.send() is available because we were launched with fork()
// It sends a message back to the parent via the IPC channel

// Listen for messages from the parent
process.on('message', (msg) => {
  console.log('Child received:', msg);

  if (msg.task === 'greet') {
    // Send a response back to the parent
    process.send({ greeting: `Hello, ${msg.name}!` });

    // Clean exit after responding
    process.exit(0);
  }
});
```

### Running

```bash
node parent.js
```

Output:

```
Child received: { task: 'greet', name: 'Alice' }
Parent received: { greeting: 'Hello, Alice!' }
Child exited with code 0
```

## Request-Response Pattern

```js
// manager.js — Fork multiple children and send tasks to them

import { fork } from 'node:child_process';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

class TaskRunner {
  constructor(scriptPath, numWorkers) {
    this.workers = [];

    // Fork numWorkers child processes
    for (let i = 0; i < numWorkers; i++) {
      const worker = fork(scriptPath);
      worker.busy = false;  // Track if this worker is processing a task

      // When a worker sends a result back, resolve its promise
      worker.on('message', (result) => {
        if (worker._resolve) {
          worker._resolve(result);  // Resolve the pending promise
          worker._resolve = null;
          worker.busy = false;
        }
      });

      this.workers.push(worker);
    }
  }

  // Send a task to a free worker and wait for the result
  async runTask(data) {
    const worker = this.workers.find((w) => !w.busy);
    if (!worker) throw new Error('All workers are busy');

    worker.busy = true;

    return new Promise((resolve) => {
      worker._resolve = resolve;  // Store resolver for the 'message' handler
      worker.send(data);          // Send the task via IPC
    });
  }

  async destroy() {
    for (const worker of this.workers) {
      worker.kill();
    }
  }
}

// Usage
const runner = new TaskRunner(resolve(__dirname, 'task-worker.js'), 3);

// Send 5 tasks — the runner handles distributing them to workers
const results = await Promise.all([
  runner.runTask({ type: 'square', value: 2 }),
  runner.runTask({ type: 'square', value: 3 }),
  runner.runTask({ type: 'square', value: 4 }),
  runner.runTask({ type: 'square', value: 5 }),
  runner.runTask({ type: 'square', value: 6 }),
]);

for (const r of results) {
  console.log(`${r.input}² = ${r.output}`);
}

await runner.destroy();
```

```js
// task-worker.js — Handles tasks sent via IPC

process.on('message', (msg) => {
  if (msg.type === 'square') {
    const result = msg.value * msg.value;

    // Send the result back to the parent
    process.send({ input: msg.value, output: result });
  }
});
```

## fork() vs Worker Threads

| Feature | `fork()` | Worker Threads |
|---------|---------|----------------|
| Memory | Separate heap per process | Shared heap possible |
| IPC | JSON messages (serialized) | Structured clone or SharedArrayBuffer |
| CPU-bound tasks | Overhead from separate V8 | Efficient (same process) |
| Crash isolation | Strong — child crash does not kill parent | Weak — uncaught error in worker can be handled but shares process |
| Best for | Running separate Node.js scripts, independent services | Heavy computation, shared memory |

## How It Works

### The IPC Channel

When you call `fork()`, Node.js creates a **Unix domain socket** (or named pipe on Windows) between the parent and child. Messages sent with `send()` are serialized using the **V8 serialization format** and deserialized on the other side.

This means you can send:
- Plain objects, arrays
- Strings, numbers, booleans
- Dates, RegExps, Maps, Sets, Buffers
- Circular references (the serializer handles them)

You **cannot** send:
- Functions
- Class instances with custom prototypes (methods are lost)
- Native handles (unless explicitly transferred)

### The silent Option

```js
// By default, fork() shares the parent's stdio (child console.log appears in parent console)
const child = fork('worker.js');

// To suppress child output, set silent: true
const quiet = fork('worker.js', { silent: true });
// Now child's console.log goes nowhere unless you listen to stdout
quiet.stdout.on('data', (chunk) => {
  // Manually handle or discard output
});
```

## Common Mistakes

### Mistake 1: Using fork() for Non-Node Scripts

```js
// WRONG — fork() only works with Node.js scripts
const child = fork('/usr/bin/python3', ['script.py']);  // Fails

// CORRECT — use spawn() for non-Node executables
import { spawn } from 'node:child_process';
const child = spawn('python3', ['script.py']);
```

### Mistake 2: Not Listening for Errors

```js
// WRONG — if the child script has a syntax error, the parent never knows
const child = fork('worker.js');
child.send({ task: 'compute' });
// Parent hangs forever waiting for a response

// CORRECT — always listen for errors and exit
const child = fork('worker.js');
child.on('error', (err) => {
  console.error('Fork error:', err.message);
});
child.on('exit', (code) => {
  if (code !== 0) console.error(`Child exited with code ${code}`);
});
child.send({ task: 'compute' });
```

### Mistake 3: Sending Circular References Without Realising

```js
// WRONG — functions cannot be serialized
const task = {
  type: 'compute',
  callback: () => console.log('done'),  // Function — cannot send via IPC
};
child.send(task);  // Throws

// CORRECT — send only data, handle logic in the child
child.send({ type: 'compute', value: 42 });
```

## Try It Yourself

### Exercise 1: Ping-Pong with fork()

Create a parent and child that send "ping" and "pong" messages back and forth 10 times, then both exit cleanly. Print each exchange.

### Exercise 2: Parallel Sum

Fork 4 child processes. Give each child a different range of numbers (e.g., child 1: 1–250, child 2: 251–500, etc.). Each child computes the sum of its range and sends it back. The parent adds all four partial sums to get the total.

### Exercise 3: Watchdog

Fork a child that runs a long computation. If the child does not send a result within 3 seconds, kill it and report a timeout.

## Next Steps

You can communicate between processes. Now let's learn how to handle errors gracefully in child processes. Continue to [Error Handling](./04-error-handling.md).
