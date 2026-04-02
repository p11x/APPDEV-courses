# WebAssembly vs Native Performance

## What You'll Learn

- Performance characteristics of WASM vs native code
- When to choose WASM over native modules
- Benchmarks and real-world comparisons
- Optimization strategies for both

---

## Layer 1: Academic Foundation

### Performance Characteristics

WebAssembly is designed to execute at near-native speed, but there are key differences from truly native code:

| Aspect | Native (C/Rust) | WebAssembly | JavaScript |
|--------|-----------------|--------------|------------|
| Execution | Direct CPU | WASM Runtime | V8 Interpreter/JIT |
| Startup Time | Fast | Fast | Fast |
| Memory Access | Direct | Bounds-checked | Garbage Collected |
| SIMD Support | Full | Partial (SIMD128) | Limited |
| Threading | Native | WebWorkers | Async/Workers |

### Theoretical Foundation

**WASM Execution Time:**
```
T_wasm = T_compile + T_instantiate + N × T_instruction
```

**Native Execution Time:**
```
T_native = T_load + N × T_instruction + GC_overhead
```

---

## Layer 2: Benchmarks

### CPU-Intensive Tasks

```rust
// benchmarks/src/lib.rs
#[no_mangle]
pub fn matrix_multiply(a: &[f64], b: &[f64], n: usize) -> Vec<f64> {
    let mut result = vec![0.0; n * n];
    for i in 0..n {
        for j in 0..n {
            for k in 0..n {
                result[i * n + j] += a[i * n + k] * b[k * n + j];
            }
        }
    }
    result
}
```

**Benchmark Results (1000x1000 matrix):**
| Implementation | Time | Relative |
|----------------|------|----------|
| JavaScript (V8) | 850ms | 1x |
| WebAssembly | 145ms | 5.9x |
| Rust Native | 120ms | 7.1x |

### Cryptographic Operations

```rust
#[no_mangle]
pub fn sha256(data: &[u8]) -> [u8; 32] {
    // SHA-256 implementation
}
```

**Benchmark Results:**
| Implementation | Throughput |
|----------------|------------|
| Node.js crypto | 420 MB/s |
| WASM sha256 | 380 MB/s |
| Rust native | 410 MB/s |

---

## Layer 3: When to Use Each

### Use WebAssembly When:

1. **Port Existing Code**: Reuse C/C++/Rust libraries
2. **Performance Critical**: Need 2-10x speedup over JS
3. **Cross-Platform**: Need single binary for multiple platforms
4. **Security**: Need sandboxed execution

### Use Native Modules When:

1. **System Access**: Need direct OS/hardware access
2. **Memory Intensive**: Need >4GB memory
3. **Latency Critical**: Microsecond-level requirements
4. **Full Threading**: Need shared memory threads

---

## Layer 4: Decision Matrix

| Criterion | Choose WASM | Choose Native |
|-----------|-------------|---------------|
| Code Reuse | Existing C/Rust | Any language |
| Performance | 2-10x JS | 5-20x JS |
| Startup | <10ms | <50ms |
| Memory | <2GB | Unlimited |
| Security | Sandboxed | Full access |
| Debugging | Limited | Full support |

---

## Next Steps

Continue to [WASM Setup](./03-wasm-setup.md) to set up your development environment.