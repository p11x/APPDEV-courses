# WASM Performance Benefits and Language Compilation

## What You'll Learn

- Compiling Rust, Go, and C++ to WASM
- Performance benchmarks and analysis
- Memory management in WASM
- Integration with Node.js applications

## Compiling Languages to WASM

### Rust to WASM

```rust
// src/lib.rs — Rust function to compile to WASM
use wasm_bindgen::prelude::*;

#[wasm_bindgen]
pub fn fibonacci(n: u32) -> u32 {
    if n <= 1 { return n; }
    fibonacci(n - 1) + fibonacci(n - 2)
}

#[wasm_bindgen]
pub fn sha256_hash(data: &[u8]) -> String {
    use sha2::{Sha256, Digest};
    let hash = Sha256::digest(data);
    format!("{:x}", hash)
}
```

```bash
# Install wasm-pack
cargo install wasm-pack

# Build for Node.js target
wasm-pack build --target nodejs

# Output: pkg/ directory with .wasm and JS bindings
```

```javascript
// Using in Node.js
import { fibonacci, sha256_hash } from './pkg/my_wasm_module.js';

console.log(fibonacci(40)); // 102334155
console.log(sha256_hash(Buffer.from('hello')));
```

### C/C++ to WASM (Emscripten)

```c
// math.c — C function for WASM
#include <emscripten.h>
#include <stdint.h>

EMSCRIPTEN_KEEPALIVE
int64_t fibonacci(int n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}
```

```bash
# Install Emscripten
git clone https://github.com/emscripten-core/emsdk.git
cd emsdk && ./emsdk install latest && ./emsdk activate latest

# Compile to WASM
emcc math.c -o math.js -s EXPORTED_FUNCTIONS='["_fibonacci"]' \
    -s EXPORTED_RUNTIME_METHODS='["ccall","cwrap"]' \
    -O3
```

## Performance Benchmarks

```
CPU-Bound Benchmark (fibonacci(40)):
─────────────────────────────────────────────
Rust WASM        ████████████  120ms
C++ WASM         ████████████  125ms
Go WASM          ██████████████ 150ms
Native Rust      ██████████    100ms
JavaScript       ████████████████████████████ 800ms

WASM vs JS Speedup: ~6x for CPU-bound tasks
```

```
Memory Comparison:
─────────────────────────────────────────────
WASM Linear Memory:  Fixed, explicit allocation
JavaScript Heap:     Garbage collected, automatic

WASM advantages:
├── Predictable memory usage
├── No GC pauses
├── Explicit control
└── Lower overhead for large datasets

JavaScript advantages:
├── Automatic memory management
├── Simpler API
├── No manual allocation
└── Better for variable-size data
```

## Best Practices Checklist

- [ ] Use Rust or C/C++ for WASM compilation (best performance)
- [ ] Minimize data crossing between JS and WASM
- [ ] Use SharedArrayBuffer for shared memory when possible
- [ ] Profile to confirm WASM provides speedup
- [ ] Consider WASI for system-level WASM

## Cross-References

- See [WASM Basics](./01-wasm-basics.md) for loading WASM modules
- See [WASM Integration](./03-wasm-integration.md) for production patterns
- See [Native Modules](../05-runtime-architecture/02-cpp-bindings-native-modules.md) as alternative

## Next Steps

Continue to [WASM Integration Patterns](./03-wasm-integration.md) for production use.
