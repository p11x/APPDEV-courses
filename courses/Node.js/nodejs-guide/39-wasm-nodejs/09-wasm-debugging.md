# WebAssembly Debugging

## What You'll Learn

- Debugging WebAssembly modules
- Reading WASM stack traces
- Using browser DevTools for WASM
- Common debugging scenarios

---

## Layer 1: Debug Symbols

### Enabling DWARF Debug Info

```toml
# Cargo.toml
[profile.dev]
debug = true

[profile.release]
debug = false
```

### Stack Trace Analysis

```typescript
function getWasmStackTrace(): string {
  const wasm = getWasmModule();
  const exports = wasm.instance.exports;
  
  if ('stack_trace' in exports) {
    const tracePtr = (exports as any).stack_trace();
    return readStringFromMemory(tracePtr);
  }
  return 'No stack trace available';
}
```

---

## Layer 2: Common Issues

| Issue | Symptom | Solution |
|-------|---------|----------|
| Memory overflow | Crash on large allocation | Increase WASM memory |
| Import mismatch | "function not found" | Check import signatures |
| Type mismatch | Incorrect return values | Verify data types |
| Stack overflow | Recursive function crash | Increase stack size |

---

## Next Steps

Continue to [WASM Best Practices](./10-wasm-best-practices.md)