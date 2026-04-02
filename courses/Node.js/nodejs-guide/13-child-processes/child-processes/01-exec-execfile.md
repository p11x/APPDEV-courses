# exec() and execFile()

## What You'll Learn

- What `exec()` and `execFile()` do and when to use each
- How to promisify them for async/await usage
- How to capture stdout and stderr from a child process
- Why shell injection is dangerous and how `execFile()` prevents it
- How to set timeout and environment variables for child processes

## What Are exec and execFile?

Node.js can run system commands (shell programs) from your JavaScript code. The `child_process` module provides two simple functions for this:

- **`exec()`** — runs a command **through a shell** (bash on Linux/Mac, cmd.exe on Windows). Returns stdout and stderr as buffered strings.
- **`execFile()`** — runs a file **directly** without a shell. Safer because user input cannot inject extra shell commands.

Both are designed for commands that produce small output. For large output, use `spawn()` instead (covered in [spawn](./02-spawn.md)).

## Basic Usage with Promisify

```js
// basic-exec.js — Run system commands and capture output

import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

// Promisify converts the callback-based API into one that returns a Promise
const execFileAsync = promisify(execFile);

async function main() {
  // Run `node --version` — execFile runs the binary directly, no shell
  const { stdout, stderr } = await execFileAsync('node', ['--version']);

  // stdout contains the output string (e.g., "v20.11.0\n")
  // stderr contains any error output
  console.log('Node version:', stdout.trim());
  if (stderr) console.error('Stderr:', stderr.trim());
}

main().catch(console.error);
```

### Running with Arguments

```js
// exec-args.js — Pass arguments to a command

import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

async function listFiles() {
  // Arguments are passed as an array — each element is one argument
  // This is safe because arguments are never parsed by a shell
  const { stdout } = await execFileAsync('ls', ['-la', '/tmp']);

  console.log('Files in /tmp:');
  console.log(stdout);
}

listFiles().catch(console.error);
```

## Using exec() — Shell Mode

```js
// exec-shell.js — Run commands through the shell

import { exec } from 'node:child_process';
import { promisify } from 'node:util';

const execAsync = promisify(exec);

async function main() {
  // exec() runs through a shell, so shell features like pipes work
  // WARNING: never pass unsanitised user input here — see "Common Mistakes"
  const { stdout } = await execAsync('echo "Hello" | tr "[:lower:]" "[:upper:]"');

  console.log('Shell output:', stdout.trim());  // "HELLO"
}

main().catch(console.error);
```

## Capturing Both stdout and stderr

```js
// capture-output.js — Handle both streams from a command

import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

async function runGitStatus() {
  try {
    // Run git status in a directory that may or may not be a git repo
    const { stdout, stderr } = await execFileAsync('git', ['status'], {
      cwd: '/tmp',          // Run in /tmp (likely not a git repo)
      timeout: 5000,        // Kill after 5 seconds if it hangs
      maxBuffer: 1024 * 1024 // Max 1MB of output (default is 200KB)
    });

    console.log('Git output:', stdout.trim());
  } catch (err) {
    // If the command exits with a non-zero code, execFile throws
    console.error('Command failed with code:', err.code);
    console.error('Stderr:', err.stderr?.trim());
    console.error('Stdout:', err.stdout?.trim());
  }
}

runGitStatus();
```

## How It Works

### exec() vs execFile()

| Feature | `exec()` | `execFile()` |
|---------|---------|-------------|
| Shell | Yes (bash/cmd.exe) | No — runs binary directly |
| Shell injection risk | Yes | No |
| Supports pipes/redirection | Yes | No |
| Best for | Simple shell commands | Running binaries with args |

### The Promisify Pattern

```js
import { promisify } from 'node:util';
const execFileAsync = promisify(execFile);
```

Node.js `child_process` functions use callbacks by default. `promisify` wraps them so you can use `async/await`. The result is an object with `{ stdout, stderr }`.

### Error Handling

When a child process exits with a **non-zero exit code**, the promise rejects with an error that includes:
- `err.code` — the exit code (e.g., `1`)
- `err.killed` — `true` if we killed the process
- `err.signal` — the signal that terminated it (e.g., `SIGTERM`)
- `err.stdout` / `err.stderr` — any output captured before the failure

## Environment Variables

```js
// env-vars.js — Pass environment variables to a child process

import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

async function main() {
  // The env option REPLACES the entire environment — include PATH and HOME
  const { stdout } = await execFileAsync('printenv', ['MY_VAR'], {
    env: {
      ...process.env,    // Inherit all current environment variables
      MY_VAR: 'hello',   // Add our custom variable
    }
  });

  console.log('MY_VAR =', stdout.trim());  // "hello"
}

main().catch(console.error);
```

## Common Mistakes

### Mistake 1: Shell Injection with exec()

```js
// DANGEROUS — user input directly in the command string
import { exec } from 'node:child_process';
import { promisify } from 'node:util';
const execAsync = promisify(exec);

const userInput = 'file.txt; rm -rf /';  // Malicious input
const { stdout } = await execAsync(`cat ${userInput}`);
// This runs: cat file.txt; rm -rf /  — DELETES EVERYTHING

// SAFE — use execFile() which does not use a shell
import { execFile } from 'node:child_process';
const execFileAsync = promisify(execFile);

const { stdout: safe } = await execFileAsync('cat', [userInput]);
// This runs: cat "file.txt; rm -rf /" — treats the whole string as one filename
```

### Mistake 2: Forgetting to Await

```js
// WRONG — execFileAsync returns a promise, not the result directly
const result = execFileAsync('node', ['--version']);
console.log(result);  // Prints: Promise { <pending> }

// CORRECT — await the promise
const result = await execFileAsync('node', ['--version']);
console.log(result.stdout.trim());  // Prints: v20.11.0
```

### Mistake 3: Output Exceeds maxBuffer

```js
// WRONG — default maxBuffer is 200KB; large output causes an error
const { stdout } = await execFileAsync('find', ['/']);  // Could produce megabytes

// CORRECT — increase maxBuffer or use spawn() for streaming
const { stdout } = await execFileAsync('find', ['/'], {
  maxBuffer: 10 * 1024 * 1024  // 10MB
});
// Or use spawn() for unbounded output (see 02-spawn.md)
```

## Try It Yourself

### Exercise 1: Get System Info

Write a script that uses `execFileAsync` to run `uname -a` and prints the result. Handle the case where the command fails (e.g., on Windows where `uname` does not exist).

### Exercise 2: Run a Python Script

Create a file `hello.py` that prints "Hello from Python". Use `execFileAsync` to run `python3 hello.py` and capture its output.

### Exercise 3: Safe File Search

Build a function `searchFiles(directory, pattern)` that uses `execFileAsync` to run `grep -r <pattern> <directory>`. Make sure the directory and pattern arguments are passed as separate array elements to avoid shell injection.

## Next Steps

You can run commands and capture their output. For long-running processes or large output streams, continue to [spawn](./02-spawn.md).
