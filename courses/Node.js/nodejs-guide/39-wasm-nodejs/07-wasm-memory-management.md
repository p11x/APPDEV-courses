# WebAssembly Memory Management

## What You'll Learn

- How WASM manages memory
- Linear memory and its usage
- Sharing memory between JavaScript and WASM
- Memory optimization techniques

---

## Layer 1: Memory Model

### Linear Memory

```rust
// Memory allocation
#[wasm_bindgen]
pub fn allocate_buffer(size: usize) -> *mut u8 {
    let layout = std::alloc::Layout::array::<u8>(size).unwrap();
    std::alloc::alloc(layout)
}

#[wasm_bindgen]
pub fn deallocate_buffer(ptr: *mut u8, size: usize) {
    let layout = std::alloc::Layout::array::<u8>(size).unwrap();
    std::alloc::dealloc(ptr, layout);
}
```

### Accessing Memory from JavaScript

```typescript
// Memory access
const wasm = await WebAssembly.instantiate(buffer, { env: { memory: memoryInstance } });

const memory = wasm.instance.exports.memory as WebAssembly.Memory;
const dataView = new DataView(memory.buffer);

// Read from WASM memory
function readString(offset: number): string {
  const view = new Uint8Array(memory.buffer);
  let end = offset;
  while (view[end] !== 0) end++;
  const decoder = new TextDecoder();
  return decoder.decode(view.slice(offset, end));
}
```

---

## Layer 2: Shared Memory

### SharedArrayBuffer

```typescript
// Create shared memory
const sharedMemory = new WebAssembly.Memory({
  initial: 2,
  maximum: 2,
  shared: true
});

// Pass to WASM
const instance = await WebAssembly.instantiate(buffer, {
  env: {
    memory: sharedMemory,
    __memory_base: 0
  }
});

// Use from JavaScript
const sharedView = new Int32Array(sharedMemory.buffer);
Atomics.add(sharedView, 0, 1);
```

---

## Next Steps

Continue to [WASM Modules](./08-wasm-modules.md)