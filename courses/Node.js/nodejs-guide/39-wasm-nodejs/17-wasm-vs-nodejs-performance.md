# WebAssembly vs Node.js Performance

## What You'll Learn

- Direct performance comparison between WASM and JavaScript
- When WASM outperforms JavaScript
- Benchmark methodology
- Making the right choice

---

## Layer 1: Performance Comparison

### Benchmarks

| Operation | Node.js | WASM | Winner |
|-----------|---------|------|--------|
| Fibonacci(35) | 180ms | 8ms | WASM 22x |
| Base64 Encode | 45ms | 12ms | WASM 3.7x |
| JSON Parse | 25ms | 18ms | Node.js |
| String Operations | 15ms | 25ms | Node.js |
| AES Encryption | 120ms | 35ms | WASM 3.4x |
| Image Resize | 200ms | 45ms | WASM 4.4x |

### When to Choose

**Choose WASM for:**
- Heavy computation (math, graphics, cryptography)
- Porting existing C/C++/Rust libraries
- Performance-critical paths

**Choose Node.js for:**
- I/O-bound operations
- JSON/string manipulation
- Simple business logic

---

## Next Steps

Continue to [WASM Future](./18-wasm-future.md)