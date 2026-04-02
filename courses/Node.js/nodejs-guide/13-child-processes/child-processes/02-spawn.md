# spawn()

## What You'll Learn

- How `spawn()` differs from `exec()` and `execFile()`
- How to stream stdout and stderr in real time
- How to send data to a child process via stdin
- How to kill a spawned process and handle signals
- How to handle backpressure when consuming large output

## Why spawn()?

`exec()` and `execFile()` buffer all output into memory before returning. This works for small outputs but fails for large ones — a process producing megabytes of output will exhaust your memory or hit the `maxBuffer` limit.

`spawn()` streams output in real time through Node.js readable streams. You process data as it arrives, with constant memory usage regardless of output size.

```
exec()     →  [wait...] → [all output at once]
spawn()    →  [chunk 1] → [chunk 2] → [chunk 3] → [done]
```

## Basic spawn() Usage

```js
// basic-spawn.js — Stream output from a child process

import { spawn } from 'node:child_process';

// Spawn `ls -la` — returns a ChildProcess object with readable streams
const child = spawn('ls', ['-la', '/tmp']);

// stdout is a Readable stream — data arrives in chunks
child.stdout.on('data', (chunk) => {
  // chunk is a Buffer — convert to string for display
  process.stdout.write(`[stdout] ${chunk.toString()}`);
});

// stderr is also a Readable stream
child.stderr.on('data', (chunk) => {
  process.stderr.write(`[stderr] ${chunk.toString()}`);
});

// 'close' fires when the process exits and all streams are flushed
child.on('close', (code) => {
  // code is the exit code — 0 means success
  console.log(`\nProcess exited with code ${code}`);
});
```

## Piping stdin to a Child Process

```js
// stdin-pipe.js — Send data to a child process

import { spawn } from 'node:child_process';

// Spawn `sort` — it reads from stdin and writes sorted lines to stdout
const sort = spawn('sort');

// Pipe data into sort's stdin
sort.stdin.write('banana\n');
sort.stdin.write('apple\n');
sort.stdin.write('cherry\n');
sort.stdin.end();  // Close stdin to signal "no more input"

// Collect sorted output
const chunks = [];
sort.stdout.on('data', (chunk) => {
  chunks.push(chunk);
});

sort.on('close', (code) => {
  const output = Buffer.concat(chunks).toString();
  console.log('Sorted output:');
  console.log(output);
  // apple
  // banana
  // cherry
});
```

## Spawning Node.js Scripts

```js
// spawn-node.js — Run another Node.js script as a child

import { spawn } from 'node:child_process';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Spawn a Node.js worker script
const worker = spawn('node', [resolve(__dirname, 'worker-script.js')], {
  // Set environment variables for the child process
  env: {
    ...process.env,
    WORKER_ID: '1',
  }
});

// Send JSON messages to the worker via stdin
worker.stdin.write(JSON.stringify({ task: 'compute', n: 42 }) + '\n');
worker.stdin.end();

// Collect the worker's response
const chunks = [];
worker.stdout.on('data', (chunk) => {
  chunks.push(chunk);
});

worker.on('close', (code) => {
  const result = Buffer.concat(chunks).toString();
  console.log(`Worker exited ${code}, output: ${result}`);
});
```

```js
// worker-script.js — Worker that reads from stdin and writes to stdout

// Read all input from stdin
const chunks = [];

process.stdin.on('data', (chunk) => {
  chunks.push(chunk);
});

process.stdin.on('end', () => {
  const input = Buffer.concat(chunks).toString().trim();
  const task = JSON.parse(input);

  if (task.task === 'compute') {
    // Do some work and write result to stdout
    const result = { workerId: process.env.WORKER_ID, n: task.n, fib: fib(task.n) };
    process.stdout.write(JSON.stringify(result));
  }
});

function fib(n) {
  if (n <= 1) return n;
  return fib(n - 1) + fib(n - 2);
}
```

## Killing a Spawned Process

```js
// kill-process.js — Spawn and terminate a long-running process

import { spawn } from 'node:child_process';

// Spawn a process that sleeps for 60 seconds
const sleeper = spawn('sleep', ['60']);

sleeper.on('close', (code, signal) => {
  // When killed, code is null and signal is the signal name (e.g., 'SIGTERM')
  console.log(`Process closed: code=${code}, signal=${signal}`);
});

// Kill the process after 2 seconds
setTimeout(() => {
  console.log('Killing the process...');
  sleeper.kill('SIGTERM');  // Send SIGTERM — process can handle it gracefully
}, 2000);

// If SIGTERM does not work after 5 seconds, force kill with SIGKILL
setTimeout(() => {
  if (!sleeper.killed) {
    console.log('Force killing...');
    sleeper.kill('SIGKILL');  // SIGKILL cannot be caught — immediate termination
  }
}, 5000);
```

## How It Works

### Streams vs Buffers

| Method | Output Handling | Memory | Best For |
|--------|----------------|--------|----------|
| `exec()` / `execFile()` | Buffers all output | Grows with output | Small output (< 200KB) |
| `spawn()` | Streams chunks | Constant | Large output, real-time processing |

### The ChildProcess Object

`spawn()` returns a `ChildProcess` object with:

- `child.stdin` — a `Writable` stream (send data to the process)
- `child.stdout` — a `Readable` stream (receive data from the process)
- `child.stderr` — a `Readable` stream (receive error output)
- `child.pid` — the process ID
- `child.kill(signal)` — send a signal to terminate

### Exit Events

- `exit` — process exited, but streams may still have buffered data
- `close` — process exited AND all streams are fully flushed (always use this for final handling)
- `error` — process could not be spawned (e.g., command not found)

## Common Mistakes

### Mistake 1: Using exec() for Large Output

```js
// WRONG — exec buffers everything; crashes on large output
const { stdout } = await execAsync('find / -name "*.js"');

// CORRECT — spawn streams output chunk by chunk
const find = spawn('find', ['/', '-name', '*.js']);
find.stdout.on('data', (chunk) => process.stdout.write(chunk));
```

### Mistake 2: Not Handling Backpressure

```js
// WRONG — writing to stdin without checking return value
for (let i = 0; i < 1_000_000; i++) {
  child.stdin.write(`${i}\n`);  // Buffer fills up, data lost
}

// CORRECT — respect backpressure
for (let i = 0; i < 1_000_000; i++) {
  const canWrite = child.stdin.write(`${i}\n`);
  if (!canWrite) {
    // Wait for drain before writing more
    await new Promise((resolve) => child.stdin.once('drain', resolve));
  }
}
```

### Mistake 3: Using 'exit' Instead of 'close'

```js
// WRONG — 'exit' fires before streams are flushed
child.on('exit', (code) => {
  console.log(output);  // May be incomplete — stdout still has buffered data
});

// CORRECT — 'close' fires after all streams are fully drained
child.on('close', (code) => {
  console.log(output);  // Complete output guaranteed
});
```

## Try It Yourself

### Exercise 1: Real-Time Log Viewer

Spawn `ping -c 10 localhost` and print each line of output as it arrives (real-time streaming). Count how many pings succeed.

### Exercise 2: Pipe Between Processes

Spawn two processes: `cat /etc/passwd` piped into `grep root`. Use Node.js streams (`child.stdout.pipe(nextChild.stdin)`) to connect them.

### Exercise 3: Timeout Wrapper

Write a function `spawnWithTimeout(command, args, timeoutMs)` that spawns a process and kills it if it does not finish within the timeout. Return a promise that resolves with the output or rejects on timeout.

## Next Steps

You can stream data from child processes. For running separate Node.js scripts with IPC communication, continue to [fork](./03-fork.md).
