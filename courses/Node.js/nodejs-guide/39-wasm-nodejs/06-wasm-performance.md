# WebAssembly Performance Optimization

## What You'll Learn

- Profiling WebAssembly performance
- Optimization techniques for WASM
- SIMD and threading optimizations
- Memory access patterns

---

## Layer 1: Profiling WASM

### Performance Measurement

```typescript
// perf.ts
import { performance } from 'node:perf_hooks';

export function measureWasmPerformance<T>(
  name: string,
  fn: () => T,
  iterations: number = 1000
): { result: T; avgMs: number } {
  const samples: number[] = [];
  let result: T;
  
  // Warmup
  for (let i = 0; i < 10; i++) {
    fn();
  }
  
  // Measure
  for (let i = 0; i < iterations; i++) {
    const start = performance.now();
    result = fn();
    samples.push(performance.now() - start);
  }
  
  const avgMs = samples.reduce((a, b) => a + b) / samples.length;
  console.log(`${name}: avg=${avgMs.toFixed(4)}ms, min=${Math.min(...samples).toFixed(4)}ms, max=${Math.max(...samples).toFixed(4)}ms`);
  
  return { result: result!, avgMs };
}
```

---

## Layer 2: Optimization Techniques

### Release Profile

```toml
# Cargo.toml
[profile.release]
opt-level = 3           # Maximum optimization
lto = true              # Link-time optimization
codegen-units = 1       # Single codegen unit for better inlining
panic = "abort"         # Smaller binary
strip = true            # Strip symbols
```

### SIMD128 Instructions

```rust
use std::arch::wasm32::*;

#[no_mangle]
pub fn vector_add_simd(a: &[f32], b: &[f32], result: &mut [f32]) {
    let len = a.len();
    let mut i = 0;
    
    while i + 4 <= len {
        let va = v128.load(a.as_ptr().add(i));
        let vb = v128.load(b.as_ptr().add(i));
        let vr = f32x4_add(va, vb);
        vr.store(result.as_ptr().add(i));
        i += 4;
    }
    
    // Handle remaining elements
    while i < len {
        result[i] = a[i] + b[i];
        i += 1;
    }
}
```

---

## Layer 3: Benchmarking

### Comparing Implementations

```typescript
// benchmarks/matrix.ts
import { measureWasmPerformance } from './perf';

function jsMatrixMultiply(a: number[][], b: number[][]): number[][] {
  const n = a.length;
  const result = Array(n).fill(0).map(() => Array(n).fill(0));
  
  for (let i = 0; i < n; i++) {
    for (let j = 0; j < n; j++) {
      for (let k = 0; k < n; k++) {
        result[i][j] += a[i][k] * b[k][j];
      }
    }
  }
  return result;
}

// Benchmark
measureWasmPerformance('JS Matrix 100x100', () => jsMatrixMultiply(m1, m2));
measureWasmPerformance('WASM Matrix 100x100', () => wasmMatrixMultiply(m1, m2));
```

---

## Next Steps

Continue to [WASM Memory Management](./07-wasm-memory-management.md)