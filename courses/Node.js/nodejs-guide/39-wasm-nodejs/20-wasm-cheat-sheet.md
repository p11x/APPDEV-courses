# WebAssembly Cheat Sheet

## Quick Reference

### Installation

```bash
# Install Rust
curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh

# Add WASM target
rustup target add wasm32-unknown-unknown

# Install wasm-pack
cargo install wasm-pack
```

### Building

```bash
# Build for web
wasm-pack build --target web --out-dir pkg

# Build for Node.js
wasm-pack build --target nodejs --out-dir pkg

# Build with optimization
wasm-pack build --release --out-dir pkg
```

### Loading in Node.js

```typescript
import fs from 'node:fs';
import { WASI } from 'node:wasi';

const wasi = new WASI({ args: process.argv, env: process.env });
const wasm = await WebAssembly.compile(fs.readFileSync('./module.wasm'));
const instance = await WebAssembly.instantiate(wasm, { wasi_unstable: wasi.exports });

// Call exported function
const result = instance.exports.add(2, 3);
```

### Loading in Browser

```typescript
const response = await fetch('module.wasm');
const buffer = await response.arrayBuffer();
const wasm = await WebAssembly.instantiate(buffer);
const result = wasm.instance.exports.add(2, 3);
```

### Rust Annotations

```rust
#[wasm_bindgen]          // Generate JS bindings
#[no_mangle]             // Prevent name mangling
pub fn function_name()   // Export function
pub struct Name          // Export struct
```

### Common Patterns

| Pattern | Code |
|---------|------|
| Array parameter | `fn process(data: &[u8])` |
| Return array | `fn process() -> Vec<u8>` |
| String parameter | `fn process(text: &str)` |
| Error handling | `fn process() -> Result<T, JsValue>` |

### Performance Tips

- Use `--release` flag for production
- Enable LTO: `lto = true` in Cargo.toml
- Use SIMD when available
- Minimize JS↔WASM boundary crossings

### Debugging

```bash
# Generate debug info
RUSTFLAGS="-g" wasm-pack build

# Check WASM size
ls -lh pkg/*.wasm
```

### File Size Optimization

```toml
[profile.release]
opt-level = "s"    # Size optimization
lto = true         # Link-time optimization
codegen-units = 1  # Better optimization
```

---

## Common Errors

| Error | Solution |
|-------|----------|
| "function not found" | Check import signatures match |
| "out of bounds" | Verify array indices |
| "import mismatch" | Check wasm-bindgen version |
| "memory error" | Increase WASM memory |

---

## Resources

- [MDN WebAssembly](https://developer.mozilla.org/en-US/docs/WebAssembly)
- [wasm-pack](https://rustwasm.github.io/wasm-pack/)
- [wasm-bindgen](https://rustwasm.github.io/wasm-bindgen/)
- [WASI](https://github.com/WebAssembly/WASI)