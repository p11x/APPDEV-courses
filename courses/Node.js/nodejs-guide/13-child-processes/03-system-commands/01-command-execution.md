# System Command Execution and Integration

## What You'll Learn

- Safe command execution patterns
- Shell command integration with security
- Command output parsing
- Timeout and cancellation
- Command chaining and piping

## Safe Command Execution

```js
// lib/safe-command.js — Safe command execution with validation
import { execFile, spawn } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

class SafeCommand {
    constructor() {
        this.allowedCommands = new Set();
    }

    allow(command) {
        this.allowedCommands.add(command);
        return this;
    }

    async execute(command, args = [], options = {}) {
        // Validate command is in allowlist
        if (!this.allowedCommands.has(command)) {
            throw new Error(`Command not allowed: ${command}`);
        }

        // Validate arguments (no shell metacharacters)
        for (const arg of args) {
            if (typeof arg !== 'string') {
                throw new Error('All arguments must be strings');
            }
            if (/[;&|`$(){}]/.test(arg)) {
                throw new Error(`Invalid characters in argument: ${arg}`);
            }
        }

        try {
            const { stdout, stderr } = await execFileAsync(command, args, {
                timeout: options.timeout || 30000,
                maxBuffer: options.maxBuffer || 1024 * 1024,
                cwd: options.cwd,
                env: { ...process.env, ...options.env },
                uid: options.uid,
                gid: options.gid,
            });

            return { stdout: stdout.trim(), stderr: stderr.trim(), code: 0 };
        } catch (err) {
            return {
                stdout: err.stdout?.trim() || '',
                stderr: err.stderr?.trim() || '',
                code: err.code || 1,
                error: err.message,
            };
        }
    }
}

// Usage
const cmd = new SafeCommand()
    .allow('git')
    .allow('npm')
    .allow('node')
    .allow('ls')
    .allow('grep');

const result = await cmd.execute('git', ['status', '--short']);
console.log(result.stdout);
```

## Command Output Parsing

```js
// command-parsers.js — Parse structured command output
import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

// Parse tabular output (like ls -la, ps aux)
function parseTable(output, headers) {
    const lines = output.trim().split('\n');
    const headerLine = headers || lines[0];
    const dataLines = headers ? lines : lines.slice(1);

    return dataLines.map(line => {
        const values = line.trim().split(/\s+/);
        const obj = {};
        const keys = headerLine.split(/\s+/);
        keys.forEach((key, i) => { obj[key.toLowerCase()] = values[i]; });
        return obj;
    });
}

// Parse key-value output (like env, printenv)
function parseKeyValue(output) {
    const result = {};
    for (const line of output.trim().split('\n')) {
        const idx = line.indexOf('=');
        if (idx > 0) {
            result[line.slice(0, idx)] = line.slice(idx + 1);
        }
    }
    return result;
}

// Parse JSON output
async function parseJSON(command, args) {
    const { stdout } = await execFileAsync(command, args);
    return JSON.parse(stdout);
}

// Get disk usage
async function getDiskUsage(path = '/') {
    const { stdout } = await execFileAsync('df', ['-h', path]);
    const lines = stdout.trim().split('\n');
    const values = lines[1].split(/\s+/);
    return {
        filesystem: values[0],
        size: values[1],
        used: values[2],
        available: values[3],
        usePercent: values[4],
        mountedOn: values[5],
    };
}

// Get system info
async function getSystemInfo() {
    const [uname, uptime, memory] = await Promise.all([
        execFileAsync('uname', ['-a']),
        execFileAsync('uptime'),
        execFileAsync('free', ['-h']),
    ]);

    return {
        system: uname.stdout.trim(),
        uptime: uptime.stdout.trim(),
        memory: parseTable(memory.stdout, ['total', 'used', 'free', 'shared', 'buff', 'available']),
    };
}
```

## Command Chaining and Piping

```js
// command-chains.js — Chain commands with pipes
import { spawn } from 'node:child_process';

async function pipeCommands(commands) {
    return new Promise((resolve, reject) => {
        let current = null;

        for (let i = 0; i < commands.length; i++) {
            const { cmd, args } = commands[i];
            const proc = spawn(cmd, args);

            if (current) {
                current.stdout.pipe(proc.stdin);
            }

            proc.on('error', reject);
            current = proc;
        }

        let output = '';
        current.stdout.on('data', (data) => { output += data; });
        current.on('close', (code) => {
            code === 0 ? resolve(output.trim()) : reject(new Error(`Exit code ${code}`));
        });
    });
}

// Usage: find . -name "*.js" | xargs wc -l | sort -n
const result = await pipeCommands([
    { cmd: 'find', args: ['.', '-name', '*.js'] },
    { cmd: 'xargs', args: ['wc', '-l'] },
    { cmd: 'sort', args: ['-n'] },
]);
console.log(result);
```

## Common Mistakes

- Using exec() with user input (shell injection)
- Not validating command arguments
- Not setting timeouts (hangs forever)
- Buffering large output (use spawn for streaming)

## Try It Yourself

### Exercise 1: Safe Git Status
Build a safe git status parser that validates the working directory.

### Exercise 2: Disk Monitor
Create a function that monitors disk usage and alerts when > 90%.

### Exercise 3: Command Chain
Build a log analyzer that chains grep → sort → uniq → head.

## Next Steps

Continue to [External Tools](../04-external-tools/01-build-tools.md).
