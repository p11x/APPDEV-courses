# WebAssembly Basics for Node.js

## What You'll Learn

- What WebAssembly is and how it works
- The WebAssembly execution model
- How WebAssembly complements JavaScript
- Use cases for WASM in Node.js applications

---

## Layer 1: Academic Foundation

### WebAssembly Overview

WebAssembly (WASM) is a binary instruction format designed for safe, fast execution in web browsers and now in server-side environments like Node.js. It provides a compilation target for languages like C, C++, Rust, and Go.

**Key Characteristics:**
- **Binary Format**: Compiled to .wasm files, much smaller than source
- **Sandboxed Execution**: Memory-safe, no direct system access
- **Near-Native Performance**: Executes at near-native speed
- **Language Agnostic**: Can be written in multiple languages
- **Interoperable**: Works alongside JavaScript

### Execution Model

```
Source Code (Rust/C/C++) → WASM Compiler → .wasm Binary → WASM Runtime → Node.js
```

The WASM module is loaded into a virtual machine that provides:
- Linear memory (contiguous byte array)
- Function table for indirect calls
- WebAssembly System Interface (WASI) for system calls

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Simple WASM Module

```rust
// src/lib.rs
#[no_mangle]
pub fn add(a: i32, b: i32) -> i32 {
    a + b
}

#[no_mangle]
pub fn fibonacci(n: u32) -> u32 {
    match n {
        0 => 0,
        1 => 1,
        _ => fibonacci(n - 1) + fibonacci(n - 2),
    }
}
```

### Paradigm 2 — Node.js Integration

```typescript
// index.ts
import fs from 'node:fs';
import path from 'node:path';
import { WASI } from 'node:wasi';

const wasi = new WASI({
  args: process.argv,
  env: process.env,
  preopens: {
    '/': process.cwd()
  }
});

const wasm = await WebAssembly.compile(fs.readFileSync(path.join(__dirname, 'math.wasm')));
const instance = await WebAssembly.instantiate(wasm, {
  wasi_unstable: wasi.exports
});

const { add, fibonacci } = instance.exports as {
  add: (a: number, b: number) => number;
  fibonacci: (n: number) => number;
};

console.log('add(2, 3):', add(2, 3));
console.log('fibonacci(20):', fibonacci(20));
```

### Paradigm 3 — Using wasm-bindgen

```rust
// src/lib.rs
use wasm_bindgen::prelude::*;

#[wasm_bindgen]
pub fn calculate_primes(limit: u32) -> Vec<u32> {
    (2..=limit).filter(|n| {
        (2..=(*n as f64).sqrt() as u32).all(|d| n % d != 0)
    }).collect()
}

#[wasm_bindgen]
pub struct Matrix {
    data: Vec<f64>,
    rows: usize,
    cols: usize,
}

#[wasm_bindgen]
impl Matrix {
    pub fn new(rows: usize, cols: usize) -> Matrix {
        Matrix {
            data: vec![0.0; rows * cols],
            rows,
            cols,
        }
    }

    pub fn multiply(&self, other: &Matrix) -> Matrix {
        // Matrix multiplication implementation
    }
}
```

---

## Layer 3: Performance Engineering

### Benchmarking WASM vs JavaScript

```typescript
// benchmark.ts
import { performance } from 'node:perf_hooks';

function benchmark(name: string, fn: () => void, iterations: number = 1000) {
  const start = performance.now();
  for (let i = 0; i < iterations; i++) {
    fn();
  }
  const duration = performance.now() - start;
  console.log(`${name}: ${duration.toFixed(2)}ms (${(duration / iterations).toFixed(4)}ms per iteration)`);
}

// Benchmark results comparison
benchmark('JavaScript fibonacci(30)', () => jsFibonacci(30));
benchmark('WASM fibonacci(30)', () => wasmFibonacci(30));
```

**Typical Results:**
| Operation | JavaScript | WASM | Speedup |
|------------|------------|------|---------|
| Fibonacci(30) | ~45ms | ~2ms | 22x |
| Matrix Multiply | ~120ms | ~15ms | 8x |
| Image Processing | ~350ms | ~45ms | 7.7x |

---

## Layer 4: Security

### WASM Security Model

- **Memory Isolation**: Each WASM instance has its own linear memory
- **No Direct Access**: Cannot access Node.js internals directly
- **Capability-Based**: Access controlled by imported functions
- **Validation**: Binary format validated before execution

---

## Layer 5: Testing

### WASM Module Testing

```typescript
// test/wasm.test.ts
import { describe, it, expect, beforeAll } from 'vitest';
import fs from 'node:fs';
import path from 'node:path';

let wasmModule: WebAssembly.Instance;

beforeAll(async () => {
  const buffer = fs.readFileSync(path.join(__dirname, '../src/math.wasm'));
  const module = await WebAssembly.compile(buffer);
  wasmModule = await WebAssembly.instantiate(module, {});
});

describe('WASM Math Functions', () => {
  it('adds two numbers correctly', () => {
    const { add } = wasmModule.exports as { add: (a: number, b: number) => number };
    expect(add(2, 3)).toBe(5);
  });

  it('calculates fibonacci correctly', () => {
    const { fibonacci } = wasmModule.exports as { fibonacci: (n: number) => number };
    expect(fibonacci(10)).toBe(55);
  });
});
```

---

## Layer 6: DevOps

### Production Deployment

```dockerfile
# Dockerfile
FROM node:20-alpine

# Install Rust and wasm-pack
RUN apk add --no-cache rust cargo wasm-pack

WORKDIR /app

COPY . .
RUN npm install
RUN npm run build:wasm

CMD ["node", "dist/index.js"]
```

---

## Layer 7: Learning Analytics

### Knowledge Graph

- **Prerequisites**: JavaScript, basic Rust or C, Node.js fundamentals
- **Related Topics**: Performance optimization, native modules, Rust integration
- **Career Mapping**: Performance Engineer, Systems Developer

---

## Next Steps

Continue to [WASM vs Native](./02-wasm-vs-native.md) for detailed performance comparison.