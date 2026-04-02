# Error Handling in Child Processes

## What You'll Learn

- What exit codes mean and which ones indicate errors
- How to handle the `error` event (spawn failures)
- How signals like SIGTERM and SIGKILL work
- How to implement timeouts using AbortController
- How to build a robust wrapper that handles all failure modes

## Exit Codes

When a process finishes, it returns an **exit code** (an integer):

| Code | Meaning |
|------|---------|
| `0` | Success — everything worked |
| `1` | General error — something went wrong |
| `2` | Misuse of shell command |
| `126` | Command found but not executable |
| `127` | Command not found |
| `128+N` | Killed by signal N (e.g., `137` = killed by SIGKILL (128+9)) |

Node.js processes the exit code in the `close` event:

```js
child.on('close', (code, signal) => {
  // code: the exit code (null if killed by signal)
  // signal: the signal name (e.g., 'SIGTERM') or null if exited normally
});
```

## Handling Spawn Errors

The `error` event fires when the process **cannot be started** — not when it fails after starting.

```js
// spawn-error.js — Handle cases where the command does not exist

import { spawn } from 'node:child_process';

// Try to run a command that does not exist
const child = spawn('nonexistent-command');

// 'error' fires when spawn itself fails (command not found, permission denied)
child.on('error', (err) => {
  console.error('Spawn failed:', err.message);
  // "Spawn failed: spawn nonexistent-command ENOENT"
  // ENOENT = "Error NO ENTry" — file/command not found
});

// 'close' fires even after an error — code is typically 127 or null
child.on('close', (code, signal) => {
  console.log(`Closed: code=${code}, signal=${signal}`);
});
```

## Timeout with AbortController

The `signal` option on `spawn()` accepts an `AbortController`'s signal. When aborted, the child process is killed automatically.

```js
// timeout-abort.js — Kill a process after a timeout using AbortController

import { spawn } from 'node:child_process';

async function runWithTimeout(command, args, timeoutMs) {
  return new Promise((resolve, reject) => {
    // AbortController lets us cancel async operations
    const controller = new AbortController();

    const child = spawn(command, args, {
      signal: controller.signal,  // Attach the abort signal
    });

    const chunks = [];

    child.stdout.on('data', (chunk) => {
      chunks.push(chunk);
    });

    // Set a timeout — if the process takes too long, abort it
    const timer = setTimeout(() => {
      controller.abort();  // This kills the child with SIGTERM
    }, timeoutMs);

    child.on('close', (code) => {
      clearTimeout(timer);  // Clean up the timer

      if (code === 0) {
        resolve(Buffer.concat(chunks).toString());
      } else {
        reject(new Error(`Process exited with code ${code}`));
      }
    });

    child.on('error', (err) => {
      clearTimeout(timer);

      // AbortError means we killed it via timeout
      if (err.name === 'AbortError') {
        reject(new Error(`Process timed out after ${timeoutMs}ms`));
      } else {
        reject(err);
      }
    });
  });
}

// Usage — this command finishes quickly
try {
  const output = await runWithTimeout('node', ['--version'], 5000);
  console.log('Output:', output.trim());
} catch (err) {
  console.error('Failed:', err.message);
}

// This command will timeout
try {
  const output = await runWithTimeout('sleep', ['60'], 2000);
  console.log('Output:', output);
} catch (err) {
  console.error('Failed:', err.message);
  // "Failed: Process timed out after 2000ms"
}
```

## Signals Explained

Signals are messages sent to a process by the operating system:

| Signal | Number | Behavior |
|--------|--------|----------|
| `SIGTERM` | 15 | "Please terminate gracefully" — process can handle it |
| `SIGKILL` | 9 | "Die immediately" — process cannot catch or ignore this |
| `SIGINT` | 2 | "User pressed Ctrl+C" — process can handle it |
| `SIGUSR1` | 10 | User-defined signal 1 |

```js
// signals.js — Send different signals to a child process

import { spawn } from 'node:child_process';

// Spawn a process that handles SIGTERM gracefully
const child = spawn('node', ['-e', `
  process.on('SIGTERM', () => {
    console.log('Child: received SIGTERM, cleaning up...');
    setTimeout(() => {
      console.log('Child: cleanup done, exiting');
      process.exit(0);
    }, 1000);
  });
  console.log('Child: waiting for signals...');
  setInterval(() => {}, 1000);  // Keep process alive
`], { stdio: ['pipe', 'pipe', 'pipe'] });

child.stdout.on('data', (chunk) => {
  process.stdout.write(`  ${chunk.toString()}`);
});

// Send SIGTERM after 2 seconds
setTimeout(() => {
  console.log('Parent: sending SIGTERM');
  child.kill('SIGTERM');
}, 2000);

child.on('close', (code, signal) => {
  console.log(`Parent: child exited — code=${code}, signal=${signal}`);
});
```

## Robust Error Wrapper

```js
// robust-exec.js — Handles all failure modes in one utility

import { spawn } from 'node:child_process';

/**
 * Run a command with timeout, error handling, and output capture.
 * @param {string} command - The command to run
 * @param {string[]} args - Arguments
 * @param {object} options - timeout, maxOutputBytes, cwd, env
 * @returns {Promise<{ stdout, stderr, code }>}
 */
function execRobust(command, args, options = {}) {
  const {
    timeout = 30_000,        // Default 30 second timeout
    maxOutputBytes = 5_000_000, // Default 5MB max output
    cwd = process.cwd(),
    env = process.env,
  } = options;

  return new Promise((resolve, reject) => {
    const controller = new AbortController();

    const child = spawn(command, args, {
      cwd,
      env,
      signal: controller.signal,
    });

    const stdoutChunks = [];
    const stderrChunks = [];
    let stdoutSize = 0;
    let stderrSize = 0;

    // Collect stdout with size limit
    child.stdout.on('data', (chunk) => {
      stdoutSize += chunk.length;
      if (stdoutSize <= maxOutputBytes) {
        stdoutChunks.push(chunk);
      }
      // Silently discard chunks beyond the limit to avoid memory issues
    });

    // Collect stderr with size limit
    child.stderr.on('data', (chunk) => {
      stderrSize += chunk.length;
      if (stderrSize <= maxOutputBytes) {
        stderrChunks.push(chunk);
      }
    });

    // Set timeout
    const timer = setTimeout(() => {
      controller.abort();  // Kill the process
    }, timeout);

    child.on('close', (code, signal) => {
      clearTimeout(timer);

      const result = {
        stdout: Buffer.concat(stdoutChunks).toString(),
        stderr: Buffer.concat(stderrChunks).toString(),
        code,
        signal,
        timedOut: signal === 'SIGTERM' && code === null,
        truncated: stdoutSize > maxOutputBytes || stderrSize > maxOutputBytes,
      };

      if (code === 0) {
        resolve(result);
      } else {
        const err = new Error(
          result.timedOut
            ? `Command timed out after ${timeout}ms`
            : `Command failed with exit code ${code}${signal ? ` (signal: ${signal})` : ''}`
        );
        Object.assign(err, result);  // Attach output to the error
        reject(err);
      }
    });

    child.on('error', (err) => {
      clearTimeout(timer);

      if (err.name === 'AbortError') {
        reject(new Error(`Command timed out after ${timeout}ms`));
      } else if (err.code === 'ENOENT') {
        reject(new Error(`Command not found: ${command}`));
      } else {
        reject(err);
      }
    });
  });
}

// Usage examples
try {
  const result = await execRobust('node', ['--version']);
  console.log('Version:', result.stdout.trim());
} catch (err) {
  console.error('Error:', err.message);
}

try {
  // This will fail — command not found
  await execRobust('not-a-real-command', []);
} catch (err) {
  console.error('Expected error:', err.message);
  // "Command not found: not-a-real-command"
}

try {
  // This will timeout
  await execRobust('sleep', ['60'], { timeout: 2000 });
} catch (err) {
  console.error('Expected timeout:', err.message);
  // "Command timed out after 2000ms"
}
```

## How It Works

### The AbortController Flow

1. Create `new AbortController()`
2. Pass `controller.signal` to `spawn()`
3. When `controller.abort()` is called, Node.js sends SIGTERM to the child
4. The `error` event fires with `err.name === 'AbortError'`
5. The `close` event fires with `code: null, signal: 'SIGTERM'`

### Error vs Close Events

| Event | When it fires | Data |
|-------|--------------|------|
| `error` | Process cannot start | `Error` object (ENOENT, EACCES) |
| `close` | Process exits (success or failure) | Exit code + signal |

Always listen for **both** events. The `error` event catches spawn failures. The `close` event catches all exits.

## Common Mistakes

### Mistake 1: Only Listening for 'error'

```js
// WRONG — if the process starts but exits with code 1, you never know
child.on('error', (err) => {
  console.error('Failed:', err.message);
});
// Missing 'close' handler — silent failures

// CORRECT — handle both error and close
child.on('error', (err) => {
  console.error('Spawn failed:', err.message);
});
child.on('close', (code) => {
  if (code !== 0) console.error(`Failed with code ${code}`);
});
```

### Mistake 2: Not Cleaning Up on Error

```js
// WRONG — timer keeps running even after the process exits normally
const timer = setTimeout(() => child.kill(), 5000);
child.on('close', (code) => {
  console.log('Done:', code);
  // Timer still ticking — prevents Node.js from exiting cleanly
});

// CORRECT — clear the timer in all exit paths
const timer = setTimeout(() => child.kill(), 5000);
child.on('close', (code) => {
  clearTimeout(timer);
  console.log('Done:', code);
});
child.on('error', (err) => {
  clearTimeout(timer);
  console.error('Error:', err.message);
});
```

### Mistake 3: Treating SIGTERM and SIGKILL the Same

```js
// WRONG — SIGKILL cannot be caught; the process has no chance to clean up
child.kill('SIGKILL');  // Immediately terminates — no graceful shutdown

// CORRECT — send SIGTERM first, then escalate to SIGKILL if needed
child.kill('SIGTERM');
const forceTimer = setTimeout(() => {
  if (!child.killed) {
    child.kill('SIGKILL');  // Escalate after grace period
  }
}, 5000);
```

## Try It Yourself

### Exercise 1: Command Not Found

Try to spawn a command that does not exist. Print a user-friendly error message instead of a stack trace.

### Exercise 2: Graceful vs Force Kill

Spawn a Node.js script that handles SIGTERM by printing "Cleaning up..." and exits after 2 seconds. Kill it with SIGTERM from the parent and verify the cleanup message appears. Then test SIGKILL and observe the difference.

### Exercise 3: Retry on Failure

Write a function `retryExec(command, args, maxRetries)` that runs a command and retries up to `maxRetries` times if it fails. Wait 1 second between retries. Return the output on success or throw after all retries are exhausted.

## Next Steps

You now understand all four child process methods and how to handle errors. Combine this knowledge with [Chapter 12: Worker Threads](../12-worker-threads-cluster/worker-threads/01-why-worker-threads.md) to choose the right parallelism strategy for each situation.
