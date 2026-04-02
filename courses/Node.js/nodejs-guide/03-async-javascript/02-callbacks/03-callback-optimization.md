# Callback Optimization Strategies

## What You'll Learn

- Callback vs Promise vs async/await performance
- Callback debugging techniques
- Modern callback best practices
- Migration strategies from callbacks

## Performance Comparison

```javascript
// Benchmark: Callback vs Promise vs async/await
import { performance } from 'node:perf_hooks';

// Callback version
function callbackOp(data, cb) {
    setImmediate(() => cb(null, data * 2));
}

// Promise version
function promiseOp(data) {
    return new Promise(resolve => setImmediate(() => resolve(data * 2)));
}

// async/await version
async function asyncOp(data) {
    return new Promise(resolve => setImmediate(() => resolve(data * 2)));
}

// Benchmark
async function benchmark(name, fn, iterations = 10000) {
    const start = performance.now();

    if (name === 'callback') {
        await new Promise(resolve => {
            let remaining = iterations;
            for (let i = 0; i < iterations; i++) {
                fn(i, () => { if (--remaining === 0) resolve(); });
            }
        });
    } else {
        await Promise.all(Array.from({ length: iterations }, (_, i) => fn(i)));
    }

    console.log(`${name}: ${(performance.now() - start).toFixed(2)}ms`);
}

// Results (approximate):
// callback:  ~45ms
// Promise:   ~52ms
// async/await: ~55ms
// Difference is negligible for real workloads
```

## Modern Callback Best Practices

```javascript
// 1. Always use error-first pattern
function readFile(path, callback) {
    fs.readFile(path, 'utf-8', (err, data) => {
        if (err) return callback(err);
        callback(null, data);
    });
}

// 2. Handle errors in EVERY callback
readFile('data.txt', (err, data) => {
    if (err) {
        console.error('Read failed:', err.message);
        return;
    }
    // Process data...
});

// 3. Avoid inline callbacks — use named functions
function onDataReceived(err, data) {
    if (err) return handleError(err);
    processData(data);
}
readFile('data.txt', onDataReceived);

// 4. Convert to Promises with util.promisify
import { promisify } from 'node:util';
const readFileAsync = promisify(fs.readFile);

// 5. Use AbortController for cancellation
const controller = new AbortController();
fs.readFile('data.txt', { signal: controller.signal }, (err, data) => {
    if (err?.code === 'ABORT_ERR') return; // Cancelled
});
setTimeout(() => controller.abort(), 5000);
```

## Callback Debugging Techniques

```javascript
// 1. Add stack traces to callbacks
function tracedCallback(fn) {
    const stack = new Error().stack;
    return function(...args) {
        try {
            return fn(...args);
        } catch (err) {
            err.stack = err.stack + '\n' + stack;
            throw err;
        }
    };
}

// 2. Log callback execution order
function loggedCallback(name, fn) {
    return (...args) => {
        console.log(`[${new Date().toISOString()}] Callback: ${name}`);
        return fn(...args);
    };
}

// 3. Detect callback never called
function withTimeout(cb, ms = 5000, opName = 'operation') {
    const timer = setTimeout(() => {
        console.error(`WARNING: ${opName} callback not called after ${ms}ms`);
    }, ms);

    return (...args) => {
        clearTimeout(timer);
        return cb(...args);
    };
}
```

## Migration Strategy: Callback → Promise

```javascript
// Step 1: Create Promise wrapper
function readFilePromise(path) {
    return new Promise((resolve, reject) => {
        fs.readFile(path, 'utf-8', (err, data) => {
            if (err) reject(err);
            else resolve(data);
        });
    });
}

// Step 2: Use util.promisify for Node.js callbacks
import { promisify } from 'node:util';
const readFileAsync = promisify(fs.readFile);

// Step 3: Update callers gradually
// Old: callback style
readFile('data.txt', (err, data) => { /* ... */ });

// New: async/await style
try {
    const data = await readFileAsync('data.txt');
} catch (err) {
    console.error(err);
}
```

## Best Practices Checklist

- [ ] Use error-first callback pattern consistently
- [ ] Handle errors in every callback
- [ ] Use named functions instead of inline callbacks
- [ ] Convert callbacks to Promises with util.promisify
- [ ] Add timeouts to detect non-firing callbacks
- [ ] Prefer async/await for new code

## Cross-References

- See [What Are Callbacks](./01-what-are-callbacks.md) for callback basics
- See [Callback Hell](./02-callback-hell.md) for anti-patterns
- See [Promise Basics](../03-promises/01-promise-basics.md) for Promise migration
- See [Concurrency Model](../01-concurrency-model/01-event-loop-deep-dive.md) for async model

## Next Steps

Continue to [Promise Basics](../03-promises/01-promise-basics.md) for Promise patterns.
