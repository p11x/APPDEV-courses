# Worker Thread Security and Isolation

## What You'll Learn

- Worker thread sandboxing strategies
- Secure communication between threads
- Resource limits and permissions
- Data protection patterns
- Security best practices

## Worker Sandboxing

```js
// lib/sandboxed-worker.js — Sandboxed worker with limited capabilities
import { Worker, isMainThread, parentPort, workerData } from 'node:worker_threads';

if (isMainThread) {
    // Main thread: create sandboxed worker
    function createSandboxedWorker(script, options = {}) {
        return new Worker(script, {
            workerData: {
                ...options.data,
                // Deny access to dangerous modules
                allowedModules: options.allowedModules || [],
            },
            // Resource limits
            resourceLimits: {
                maxOldGenerationSizeMb: options.maxHeapMB || 128,
                maxYoungGenerationSizeMb: 64,
                codeRangeSizeMb: 16,
                stackSizeMb: 4,
            },
        });
    }

    const worker = createSandboxedWorker('./sandboxed-worker.js', {
        maxHeapMB: 64,
        data: { task: 'compute' },
    });

} else {
    // Worker: execute in sandboxed environment
    const { allowedModules, task } = workerData;

    // Override require/import to restrict module access
    const originalImport = globalThis.import;
    const restrictedModules = ['child_process', 'fs', 'net', 'http', 'cluster'];

    // Execute task in restricted context
    async function executeSandboxed(taskFn) {
        // Create restricted global scope
        const sandbox = {
            console: {
                log: (...args) => parentPort.postMessage({ type: 'log', data: args }),
                error: (...args) => parentPort.postMessage({ type: 'error', data: args }),
            },
            Math,
            Date,
            JSON,
            Buffer,
            TextEncoder,
            TextDecoder,
            // No process, require, import, or file system access
        };

        try {
            const result = await taskFn(sandbox);
            parentPort.postMessage({ type: 'result', data: result });
        } catch (err) {
            parentPort.postMessage({ type: 'error', data: err.message });
        }
    }
}
```

## Resource Limits

```js
// resource-limits.js — Set hard limits on worker resources
import { Worker } from 'node:worker_threads';

const worker = new Worker('./compute.js', {
    resourceLimits: {
        // Maximum heap size (MB)
        maxOldGenerationSizeMb: 256,

        // Maximum young generation (scavenge) size (MB)
        maxYoungGenerationSizeMb: 64,

        // Code range size (MB)
        codeRangeSizeMb: 16,

        // Stack size (MB) — only on some platforms
        stackSizeMb: 4,
    },
});

worker.on('error', (err) => {
    if (err.code === 'ERR_WORKER_OUT_OF_MEMORY') {
        console.error('Worker exceeded memory limit');
        // Handle OOM — don't let it crash the main thread
    }
});
```

## Secure Message Passing

```js
// secure-ipc.js — Encrypted IPC between main thread and worker
import { createCipheriv, createDecipheriv, randomBytes } from 'node:crypto';
import { Worker, isMainThread, parentPort, workerData } from 'node:worker_threads';

class SecureChannel {
    constructor(key) {
        this.key = key; // 32 bytes for AES-256
    }

    encrypt(data) {
        const iv = randomBytes(16);
        const cipher = createCipheriv('aes-256-gcm', this.key, iv);
        const plaintext = JSON.stringify(data);
        const encrypted = Buffer.concat([
            cipher.update(plaintext, 'utf8'),
            cipher.final(),
        ]);
        const authTag = cipher.getAuthTag();

        return {
            iv: iv.toString('base64'),
            data: encrypted.toString('base64'),
            tag: authTag.toString('base64'),
        };
    }

    decrypt({ iv, data, tag }) {
        const decipher = createDecipheriv(
            'aes-256-gcm',
            this.key,
            Buffer.from(iv, 'base64')
        );
        decipher.setAuthTag(Buffer.from(tag, 'base64'));

        const decrypted = Buffer.concat([
            decipher.update(Buffer.from(data, 'base64')),
            decipher.final(),
        ]);

        return JSON.parse(decrypted.toString('utf8'));
    }
}

// Usage
if (isMainThread) {
    const key = randomBytes(32);
    const secure = new SecureChannel(key);
    const worker = new Worker('./secure-ipc.js', { workerData: { key } });

    const encrypted = secure.encrypt({ secret: 'api-key-12345' });
    worker.postMessage({ type: 'secret', payload: encrypted });

} else {
    const secure = new SecureChannel(workerData.key);
    parentPort.on('message', (msg) => {
        if (msg.type === 'secret') {
            const data = secure.decrypt(msg.payload);
            console.log('Received secret:', data.secret);
        }
    });
}
```

## Common Mistakes

- Not setting resource limits (worker OOM crashes main thread)
- Allowing worker to import sensitive modules (fs, child_process)
- Sending unvalidated data from worker to main thread
- Not isolating worker errors from main thread

## Try It Yourself

### Exercise 1: Memory Limit
Create a worker that tries to allocate 512MB with a 128MB limit. Handle the error gracefully.

### Exercise 2: Restricted Modules
Try to import `child_process` in a sandboxed worker. Verify it's blocked.

### Exercise 3: Secure Channel
Implement encrypted IPC and verify messages can't be read in transit.

## Next Steps

Continue to [Advanced Patterns](../07-advanced-patterns/01-producer-consumer.md).
