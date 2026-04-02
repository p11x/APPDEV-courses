# WebAssembly Alternatives

## What You'll Learn

- Alternatives to WebAssembly
- When to use native modules
- GraalJS and other runtimes
- Choosing the right technology

---

## Layer 1: Technology Comparison

### Native Node.js Modules

```bash
# Rebuild native modules for Node.js
npm rebuild
node-gyp rebuild
```

**Pros**: Full system access, best performance for certain tasks
**Cons**: Platform-dependent, complex build

### GraalJS

```javascript
const graaljs = require('graaljs');

const polyglot = graaljs.eval(`
  function hello() { return "Hello from JavaScript!"; }
  hello();
`);
```

**Pros**: High-performance JS, polyglot programming
**Cons**: Larger footprint, compatibility concerns

### QuickJS

- Embedded JavaScript engine
- Small footprint (<200KB)
- WebAssembly support

---

## Layer 2: Decision Guide

| Use Case | Technology |
|----------|------------|
| Compute-intensive | WASM |
| System access | Native modules |
| Embed JS | GraalJS |
| Embedded systems | QuickJS |
| Cross-platform | WASM |

---

## Next Steps

Continue to [WASM Cheat Sheet](./20-wasm-cheat-sheet.md)