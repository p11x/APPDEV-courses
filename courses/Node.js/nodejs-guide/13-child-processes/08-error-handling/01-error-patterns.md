# Child Process Error Handling and Recovery

## What You'll Learn

- Error handling patterns for child processes
- Error classification and recovery strategies
- Timeout handling
- Crash recovery
- Error logging and monitoring

## Comprehensive Error Handler

```js
// lib/process-error-handler.js — Robust error handling for child processes
import { spawn, fork } from 'node:child_process';

class ProcessError extends Error {
    constructor(message, details = {}) {
        super(message);
        this.name = 'ProcessError';
        this.code = details.code;
        this.signal = details.signal;
        this.stdout = details.stdout;
        this.stderr = details.stderr;
        this.killed = details.killed;
        this.command = details.command;
        this.retryable = details.retryable || false;
    }

    static fromExit(code, signal, details) {
        const retryable = code === null && signal === 'SIGTERM' ||
            code === 137 || // SIGKILL
            code === 143;   // SIGTERM

        return new ProcessError(
            `Process exited with code ${code} (signal: ${signal})`,
            { code, signal, retryable, ...details }
        );
    }

    static fromTimeout(command) {
        return new ProcessError(`Process timed out: ${command}`, {
            command,
            retryable: true,
        });
    }
}

class ProcessErrorHandler {
    constructor(options = {}) {
        this.maxRetries = options.maxRetries || 3;
        this.retryDelay = options.retryDelay || 1000;
        this.timeout = options.timeout || 30000;
        this.onRetry = options.onRetry || (() => {});
    }

    async execute(command, args = [], options = {}) {
        let lastError;

        for (let attempt = 1; attempt <= this.maxRetries; attempt++) {
            try {
                return await this.runOnce(command, args, options);
            } catch (err) {
                lastError = err;

                if (!err.retryable || attempt === this.maxRetries) {
                    throw err;
                }

                const delay = this.retryDelay * Math.pow(2, attempt - 1);
                this.onRetry({ attempt, delay, error: err.message });
                await new Promise(r => setTimeout(r, delay));
            }
        }

        throw lastError;
    }

    runOnce(command, args, options) {
        return new Promise((resolve, reject) => {
            const proc = spawn(command, args, {
                cwd: options.cwd,
                env: { ...process.env, ...options.env },
                timeout: this.timeout,
            });

            let stdout = '';
            let stderr = '';
            let killed = false;

            proc.stdout.on('data', (data) => { stdout += data; });
            proc.stderr.on('data', (data) => { stderr += data; });

            const timeoutId = setTimeout(() => {
                killed = true;
                proc.kill('SIGTERM');
            }, this.timeout);

            proc.on('close', (code, signal) => {
                clearTimeout(timeoutId);

                if (killed) {
                    reject(ProcessError.fromTimeout(command));
                } else if (code === 0) {
                    resolve({ stdout: stdout.trim(), stderr: stderr.trim() });
                } else {
                    reject(ProcessError.fromExit(code, signal, {
                        stdout, stderr, command,
                    }));
                }
            });

            proc.on('error', (err) => {
                clearTimeout(timeoutId);
                reject(new ProcessError(err.message, { command, retryable: false }));
            });
        });
    }
}

// Usage
const handler = new ProcessErrorErrorHandler({
    maxRetries: 3,
    retryDelay: 1000,
    timeout: 30000,
    onRetry: ({ attempt, delay, error }) => {
        console.warn(`Retry ${attempt} after ${delay}ms: ${error}`);
    },
});

try {
    const result = await handler.execute('git', ['clone', repoUrl, destDir]);
    console.log('Clone successful');
} catch (err) {
    console.error('Failed after retries:', err.message);
}
```

## Error Classification

```
Child Process Error Types:
─────────────────────────────────────────────
Error                    Code    Retryable   Cause
─────────────────────────────────────────────
ENOENT                   N/A     No          Command not found
EACCES                   N/A     No          Permission denied
Non-zero exit            code    Depends     Command failed
Timeout                  N/A     Yes         Process hung
SIGTERM                  143     Yes         Killed by parent
SIGKILL                  137     Maybe       OOM killer
maxBuffer exceeded       N/A     No          Too much output
```

## Common Mistakes

- Not classifying errors as retryable or not
- Not handling ENOENT (command not found)
- Retrying non-retryable errors (infinite loops)
- Not logging error context (command, args, cwd)

## Try It Yourself

### Exercise 1: Error Classification
Run commands that fail in different ways and classify each error.

### Exercise 2: Retry Logic
Implement retry with exponential backoff for network-dependent commands.

### Exercise 3: Timeout Handling
Run a sleep command with a 2-second timeout and verify it's killed.

## Next Steps

Continue to [Testing](../09-testing/01-unit-testing.md).
