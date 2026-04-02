# Child Process Security and Sandboxing

## What You'll Learn

- Preventing shell injection attacks
- Sandboxing child processes
- Resource limits and constraints
- Running processes as different users
- Security best practices

## Shell Injection Prevention

```js
// security/shell-injection.js — Prevent shell injection attacks
import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

// DANGEROUS: Shell injection vulnerability
async function unsafeSearch(userInput) {
    const { exec } = require('child_process');
    const { promisify } = require('util');
    const execAsync = promisify(exec);

    // User input: "file.txt; rm -rf /"
    const { stdout } = await execAsync(`find / -name "${userInput}"`);
    // This runs: find / -name "file.txt; rm -rf /"
    // The semicolon terminates the find command and starts rm -rf /
    return stdout;
}

// SAFE: Using execFile (no shell interpretation)
async function safeSearch(userInput) {
    // Validate input
    if (typeof userInput !== 'string' || userInput.length > 255) {
        throw new Error('Invalid input');
    }
    if (/[;&|`$(){}\\]/.test(userInput)) {
        throw new Error('Input contains unsafe characters');
    }

    const { stdout } = await execFileAsync('find', ['/', '-name', userInput]);
    // This runs: find "/" "-name" "file.txt; rm -rf /"
    // The semicolon is treated as part of the filename, not a command separator
    return stdout;
}

// Input sanitization
function sanitizePath(input) {
    // Allow only alphanumeric, hyphens, underscores, dots, slashes
    if (!/^[a-zA-Z0-9._\-\/]+$/.test(input)) {
        throw new Error('Invalid path characters');
    }
    // Prevent directory traversal
    if (input.includes('..')) {
        throw new Error('Directory traversal detected');
    }
    return input;
}

function sanitizeArgument(input) {
    // Remove shell metacharacters
    return input.replace(/[;&|`$(){}\\]/g, '');
}
```

## Process Sandboxing

```js
// security/sandbox.js — Sandbox child processes
import { spawn } from 'node:child_process';

class ProcessSandbox {
    constructor(command, args, options = {}) {
        this.command = command;
        this.args = args;
        this.options = {
            cwd: options.cwd || '/tmp/sandbox',
            env: this.sanitizeEnv(options.env || {}),
            uid: options.uid,      // Run as different user (Unix)
            gid: options.gid,      // Run as different group (Unix)
            timeout: options.timeout || 30000,
            maxBuffer: options.maxBuffer || 1024 * 1024,
        };
    }

    sanitizeEnv(env) {
        // Only allow specific environment variables
        const allowed = ['PATH', 'HOME', 'USER', 'LANG', 'LC_ALL'];
        const safe = {};
        for (const key of allowed) {
            if (env[key] || process.env[key]) {
                safe[key] = env[key] || process.env[key];
            }
        }
        return safe;
    }

    async execute() {
        return new Promise((resolve, reject) => {
            const proc = spawn(this.command, this.args, {
                ...this.options,
                stdio: ['pipe', 'pipe', 'pipe'],
            });

            let stdout = '';
            let stderr = '';
            let killed = false;

            proc.stdout.on('data', (data) => {
                stdout += data;
                if (stdout.length > this.options.maxBuffer) {
                    proc.kill('SIGKILL');
                    killed = true;
                }
            });

            proc.stderr.on('data', (data) => { stderr += data; });

            const timeout = setTimeout(() => {
                proc.kill('SIGKILL');
                killed = true;
            }, this.options.timeout);

            proc.on('close', (code) => {
                clearTimeout(timeout);
                if (killed) {
                    reject(new Error('Process killed: timeout or resource limit'));
                } else {
                    resolve({ code, stdout: stdout.trim(), stderr: stderr.trim() });
                }
            });

            proc.on('error', (err) => {
                clearTimeout(timeout);
                reject(err);
            });
        });
    }
}

// Run as unprivileged user (Linux)
const sandbox = new ProcessSandbox('python3', ['untrusted_script.py'], {
    cwd: '/tmp/sandbox',
    uid: 65534,   // nobody user
    gid: 65534,   // nobody group
    timeout: 10000,
    maxBuffer: 1024 * 1024,
});

try {
    const result = await sandbox.execute();
    console.log(result.stdout);
} catch (err) {
    console.error('Sandbox error:', err.message);
}
```

## Resource Limits

```js
// security/resource-limits.js — Limit child process resources
import { spawn } from 'node:child_process';

function runWithLimits(command, args, limits = {}) {
    const proc = spawn(command, args, {
        stdio: ['pipe', 'pipe', 'pipe'],
        // Setrlimit on Unix (ulimit equivalent)
        // Available through third-party packages or native addons
    });

    // Monitor memory usage
    const memoryLimit = limits.memoryMB || 256;
    const memCheck = setInterval(() => {
        try {
            const usage = process.memoryUsage();
            // For child processes, use /proc/[pid]/status on Linux
        } catch { /* ignore */ }
    }, 1000);

    // Timeout
    const timeout = setTimeout(() => {
        proc.kill('SIGKILL');
    }, limits.timeout || 30000);

    proc.on('close', () => {
        clearInterval(memCheck);
        clearTimeout(timeout);
    });

    return proc;
}
```

## Common Mistakes

- Using exec() with user input (shell injection)
- Not validating or sanitizing input
- Running child processes as root
- Not setting resource limits (DoS via memory exhaustion)

## Try It Yourself

### Exercise 1: Shell Injection Test
Try passing `"; rm -rf /tmp/test"` as input. Verify execFile prevents injection.

### Exercise 2: Resource Limits
Run a process that tries to allocate 1GB of memory with a 128MB limit.

### Exercise 3: User Isolation
Run a process as the `nobody` user and verify it can't write to protected directories.

## Next Steps

Continue to [Performance](../07-performance/01-profiling.md).
