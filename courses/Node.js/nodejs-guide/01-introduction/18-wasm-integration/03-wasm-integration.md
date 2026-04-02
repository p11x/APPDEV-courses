# WASM Integration Patterns and Best Practices

## What You'll Learn

- Production WASM integration patterns
- WASI (WebAssembly System Interface)
- Error handling and debugging
- Deployment strategies

## WASI Integration

```javascript
// WASI allows WASM to access system resources
import { WASI } from 'node:wasi';
import { readFile } from 'node:fs/promises';

const wasi = new WASI({
    version: 'preview1',
    args: process.argv,
    env: process.env,
    preopens: {
        '/sandbox': './sandbox',
    },
});

const wasmBuffer = await readFile('./file-processor.wasm');
const { instance } = await WebAssembly.instantiate(wasmBuffer, {
    wasi_snapshot_preview1: wasi.wasiImport,
});

wasi.start(instance);
```

## Worker Thread WASM Pattern

```javascript
// wasm-worker.js — Run WASM in worker thread

import { Worker, isMainThread, parentPort, workerData } from 'node:worker_threads';
import { readFile } from 'node:fs/promises';

if (isMainThread) {
    // Main thread: delegate to worker
    async function runWasm(operation, data) {
        return new Promise((resolve, reject) => {
            const worker = new Worker(new URL(import.meta.url), {
                workerData: { operation, data },
            });
            worker.on('message', resolve);
            worker.on('error', reject);
        });
    }
    
    const result = await runWasm('hash', Buffer.from('hello'));
    console.log(result);
} else {
    // Worker: load and run WASM
    const wasmBuffer = await readFile('./hash.wasm');
    const { instance } = await WebAssembly.instantiate(wasmBuffer);
    
    const { operation, data } = workerData;
    let result;
    
    switch (operation) {
        case 'hash':
            // Pass data to WASM, get result
            result = instance.exports.hash(data);
            break;
    }
    
    parentPort.postMessage(result);
}
```

## Error Handling

```javascript
try {
    const wasmBuffer = await readFile('./module.wasm');
    const { instance } = await WebAssembly.instantiate(wasmBuffer);
    
    // WASM functions can throw
    const result = instance.exports.divide(10, 0);
} catch (err) {
    if (err instanceof WebAssembly.CompileError) {
        console.error('WASM compilation failed:', err.message);
    } else if (err instanceof WebAssembly.LinkError) {
        console.error('WASM linking failed:', err.message);
    } else if (err instanceof WebAssembly.RuntimeError) {
        console.error('WASM runtime error:', err.message);
    } else {
        console.error('Unexpected error:', err);
    }
}
```

## Best Practices Checklist

- [ ] Use WASI for system-level WASM access
- [ ] Run WASM in worker threads for CPU-bound tasks
- [ ] Handle all WASM error types
- [ ] Use streaming compilation for large modules
- [ ] Profile JS↔WASM boundary overhead
- [ ] Consider shared memory for large data transfers

## Cross-References

- See [WASM Basics](./01-wasm-basics.md) for fundamentals
- See [WASM Performance](./02-wasm-performance.md) for benchmarks
- See [Edge Computing](../19-edge-computing/01-cloudflare-workers.md) for WASM at edge

## Next Steps

Continue to [Edge Computing](../19-edge-computing/01-cloudflare-workers.md) for serverless deployment.
