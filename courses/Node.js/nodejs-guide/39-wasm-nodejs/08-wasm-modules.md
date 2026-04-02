# WebAssembly Modules

## What You'll Learn

- Loading and instantiating WASM modules
- Module composition and imports
- Dynamic module loading
- Module caching strategies

---

## Layer 1: Module Loading

### Basic Loading

```typescript
// Simple loading
const response = await fetch('module.wasm');
const buffer = await response.arrayBuffer();
const module = await WebAssembly.compile(buffer);
const instance = await WebAssembly.instantiate(module, imports);
```

### Using WASI

```typescript
import { WASI } from 'node:wasi';

const wasi = new WASI({
  version: 'preview1',
  args: process.argv,
  env: process.env,
  preopens: {
    '/': process.cwd()
  }
});

const instance = await WebAssembly.instantiate(buffer, {
  wasi_unstable: wasi.exports
});
```

---

## Layer 2: Module Composition

### Imports and Exports

```typescript
// Define imports
const imports = {
  env: {
    memory: new WebAssembly.Memory({ initial: 1 }),
    console_log: (ptr: number) => console.log(readString(ptr))
  },
  wasi: {
    proc_exit: (code: number) => process.exit(code)
  }
};

// Access exports
const { add, _start } = instance.exports;
```

---

## Next Steps

Continue to [WASM Debugging](./09-wasm-debugging.md)