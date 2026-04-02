# WebAssembly in Node.js — Current State and Basics

## What You'll Learn

- WebAssembly fundamentals in Node.js
- Loading and running WASM modules
- Performance characteristics
- When to use WASM vs native modules

## WebAssembly in Node.js

### Loading WASM Modules

```javascript
// Load WASM from file
import { readFile } from 'node:fs/promises';
import { join } from 'node:path';

const wasmBuffer = await readFile(join(import.meta.dirname, 'module.wasm'));
const wasmModule = await WebAssembly.instantiate(wasmBuffer);
const { exports } = wasmModule.instance;

// Call exported function
const result = exports.add(2, 3);
console.log(result); // 5
```

### WASM from Text (WAT)

```javascript
// Inline WAT (WebAssembly Text format)
const wasmCode = `
(module
  (func $add (param $a i32) (param $b i32) (result i32)
    local.get $a
    local.get $b
    i32.add
  )
  (export "add" (func $add))
)
`;

const wasmBuffer = Buffer.from(wasmCode);
// Note: Need a WAT parser for this to work
// Use wabt or wasm-tools in practice
```

### Streaming Compilation

```javascript
// Streaming — compile while downloading
const { instance } = await WebAssembly.instantiateStreaming(
    fetch('https://example.com/module.wasm')
);
const result = instance.exports.compute(42);
```

## Performance Comparison

```javascript
// benchmark-wasm.js — Compare JS vs WASM performance

import { performance } from 'node:perf_hooks';
import { readFile } from 'node:fs/promises';

// JavaScript implementation
function jsFibonacci(n) {
    if (n <= 1) return n;
    return jsFibonacci(n - 1) + jsFibonacci(n - 2);
}

// Load WASM module
const wasmBuffer = await readFile('./fibonacci.wasm');
const { instance } = await WebAssembly.instantiate(wasmBuffer);
const wasmFibonacci = instance.exports.fibonacci;

// Benchmark
function bench(name, fn, arg) {
    // Warmup
    for (let i = 0; i < 10; i++) fn(arg);
    
    const start = performance.now();
    for (let i = 0; i < 100; i++) fn(arg);
    console.log(`${name}: ${(performance.now() - start).toFixed(2)}ms`);
}

bench('JavaScript', jsFibonacci, 35);
bench('WebAssembly', wasmFibonacci, 35);

// Typical results:
// JavaScript:  1200ms
// WebAssembly: 400ms
// WASM is ~3x faster for CPU-bound computation
```

## When to Use WASM

```
Use WASM when:
├── CPU-intensive computation
├── Reusing existing C/C++/Rust libraries
├── Cryptographic operations
├── Image/video processing
├── Scientific computing
└── Portability across runtimes needed

Avoid WASM when:
├── I/O-bound operations (Node.js is fine)
├── Simple data manipulation
├── Team lacks WASM experience
├── Rapid development needed
└── JavaScript performance is sufficient
```

## Best Practices Checklist

- [ ] Use WASM for CPU-bound computation only
- [ ] Benchmark: WASM has overhead for simple operations
- [ ] Use streaming compilation for large modules
- [ ] Consider WASI for system-level access
- [ ] Keep JS↔WASM boundary crossings minimal

## Cross-References

- See [WASM Performance Benefits](./02-wasm-performance.md) for detailed benchmarks
- See [WASM Integration Patterns](./03-wasm-integration.md) for production patterns
- See [Native Modules](../05-runtime-architecture/02-cpp-bindings-native-modules.md) as alternative

## Next Steps

Continue to [WASM Performance Benefits](./02-wasm-performance.md) for detailed analysis.
