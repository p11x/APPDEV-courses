# spawn vs exec vs fork — Differences and Examples

## What You'll Learn

- When to use spawn, exec, and fork
- Stream-based vs buffered output
- Inter-process communication with fork
- Error handling across processes

## spawn — Stream-Based Output

```javascript
import { spawn } from 'node:child_process';

// Stream output as it arrives (best for large output)
const ls = spawn('ls', ['-la', '/tmp']);

ls.stdout.on('data', (data) => {
    console.log(`stdout: ${data}`);
});

ls.stderr.on('data', (data) => {
    console.error(`stderr: ${data}`);
});

ls.on('close', (code) => {
    console.log(`Process exited with code ${code}`);
});

// Pipe to another process
const grep = spawn('grep', ['node']);
const ps = spawn('ps', ['aux']);

ps.stdout.pipe(grep.stdin);
grep.stdout.on('data', (data) => console.log(data.toString()));
```

## exec — Buffered Output

```javascript
import { exec } from 'node:child_process';
import { promisify } from 'node:util';

const execAsync = promisify(exec);

// Simple command (buffered — collects all output)
const { stdout, stderr } = await execAsync('node --version');
console.log(stdout.trim()); // 'v22.14.0'

// With options
const result = await execAsync('ls -la', {
    cwd: '/tmp',
    timeout: 5000,      // 5 second timeout
    maxBuffer: 1024 * 1024, // 1MB max output
});

// Security: execFile doesn't use shell
import { execFile } from 'node:child_process';
const execFileAsync = promisify(execFile);

const { stdout: version } = await execFileAsync('node', ['--version']);
console.log(version.trim());
```

## fork — IPC Communication

```javascript
// parent.js
import { fork } from 'node:child_process';

const child = fork('./worker.js');

// Send message to child
child.send({ type: 'compute', data: [1, 2, 3, 4, 5] });

// Receive messages from child
child.on('message', (msg) => {
    console.log('From child:', msg);
});

child.on('exit', (code) => {
    console.log(`Child exited with code ${code}`);
});

// worker.js
process.on('message', (msg) => {
    if (msg.type === 'compute') {
        const result = msg.data.reduce((a, b) => a + b, 0);
        process.send({ type: 'result', value: result });
    }
});
```

## Comparison Table

```
spawn vs exec vs fork:
─────────────────────────────────────────────
Feature        spawn          exec           fork
─────────────────────────────────────────────
Output         Stream         Buffered       IPC messages
Max output     No limit       1MB default    No limit
Shell          No             Yes            No
IPC            No             No             Yes
Best for       Large output   Simple cmds    Node.js scripts
Security       Safer          Less safe      Safer
```

## Best Practices Checklist

- [ ] Use `spawn` for large output or streaming
- [ ] Use `execFile` over `exec` for security
- [ ] Use `fork` for Node.js-to-Node.js communication
- [ ] Always handle stderr and exit codes
- [ ] Set timeouts for long-running processes

## Cross-References

- See [IPC Patterns](./02-ipc-communication.md) for communication patterns
- See [Worker Threads](../08-worker-threads/01-parallel-processing.md) for parallelism
- See [Process Lifecycle](../09-process-lifecycle/01-signal-handling.md) for lifecycle

## Next Steps

Continue to [IPC Communication](./02-ipc-communication.md) for inter-process patterns.
