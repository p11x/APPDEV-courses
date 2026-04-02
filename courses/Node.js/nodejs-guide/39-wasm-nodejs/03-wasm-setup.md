# WebAssembly Setup for Node.js

## What You'll Learn

- How to set up a WASM development environment
- Installing required toolchains
- Configuring build tools and bundlers
- Creating your first WASM project

---

## Layer 1: Environment Setup

### Installing Rust Toolchain

```bash
# Install Rust
curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh

# Add wasm32 target
rustup target add wasm32-unknown-unknown

# Install wasm-pack
cargo install wasm-pack

# Verify installation
rustc --version
wasm-pack --version
```

### Installing Other Toolchains

```bash
# Emscripten (C/C++)
git clone https://github.com/emscripten-core/emsdk.git
cd emsdk
./emsdk install latest
./emsdk activate latest
source ./emsdk_env.sh

# Go (WASM support is built-in)
go version  # Go 1.21+ includes WASM support
```

---

## Layer 2: Project Initialization

### Rust + wasm-pack Project

```bash
# Create new library project
cargo new --lib wasm-demo

# Update Cargo.toml
cat > wasm-demo/Cargo.toml << 'EOF'
[package]
name = "wasm-demo"
version = "0.1.0"
edition = "2021"

[lib]
crate-type = ["cdylib"]

[dependencies]
wasm-bindgen = "0.2"
serde = { version = "1.0", features = ["derive"] }
serde-wasm-bindgen = "0.6"

[profile.release]
opt-level = "s"
lto = true
EOF
```

### TypeScript Configuration

```json
// tsconfig.json
{
  "compilerOptions": {
    "target": "ES2020",
    "module": "ESNext",
    "moduleResolution": "bundler",
    "types": ["vite/client"]
  }
}
```

### Vite Configuration

```typescript
// vite.config.ts
import { defineConfig } from 'vite';

export default defineConfig({
  optimizeDeps: {
    exclude: ['wasm-demo']
  },
  build: {
    target: 'esnext'
  }
});
```

---

## Layer 3: Build Pipeline

### Building WASM Module

```bash
# Build with wasm-pack
wasm-pack build --target web --out-dir pkg wasm-demo

# Or for Node.js
wasm-pack build --target nodejs --out-dir pkg wasm-demo
```

### npm Scripts

```json
// package.json
{
  "scripts": {
    "build:wasm": "wasm-pack build --target nodejs --out-dir pkg",
    "build:js": "vite build",
    "build": "npm run build:wasm && npm run build:js",
    "dev": "vite",
    "test": "vitest"
  }
}
```

---

## Next Steps

Continue to [WASM Rust Integration](./04-wasm-rust-integration.md)