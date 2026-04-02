# WebAssembly Toolchain

## What You'll Learn

- Different WASM toolchains
- Building with wasm-pack, Emscripten, AssemblyScript
- Tool configuration and optimization
- Cross-compilation strategies

---

## Layer 1: Toolchain Options

### wasm-pack (Rust)

```bash
wasm-pack build --target web --out-dir pkg

# With optimization
wasm-pack build --target web --release --out-dir pkg
```

### Emscripten (C/C++)

```bash
emcc -s WASM=1 -s EXPORTED_FUNCTIONS="['_malloc', '_free', '_process']" \
     -s EXPORTED_RUNTIME_METHODS="['ccall', 'cwrap']" \
     -O3 input.c -o output.js
```

### AssemblyScript (TypeScript-like)

```bash
asc source.ts --target release --outFile out.wasm
```

---

## Layer 2: Tool Configuration

### Cargo Configuration

```toml
# .cargo/config.toml
[build]
target = "wasm32-unknown-unknown"

[target.wasm32-unknown-unknown]
linker = "wasm-bindgen"

[profile.release]
opt-level = "s"
lto = true
```

---

## Next Steps

Continue to [WASM Bundling](./13-wasm-bundling.md)