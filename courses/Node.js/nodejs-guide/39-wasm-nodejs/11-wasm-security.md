# WebAssembly Security

## What You'll Learn

- Security model of WebAssembly
- Sandbox and capability-based security
- Preventing common vulnerabilities
- Secure data handling between JS and WASM

---

## Layer 1: Security Model

### Sandboxed Execution

WASM runs in a sandbox with no direct access to:
- File system (except via WASI)
- Network (except via WASI)
- Process memory
- System calls

### Capability-Based Access

```rust
#[wasm_bindgen]
pub struct SecureProcessor {
    // Internal state not accessible from JS
}

#[wasm_bindgen]
impl SecureProcessor {
    #[wasm_bindgen(constructor)]
    pub fn new(max_memory: u32) -> Result<SecureProcessor, JsValue> {
        if max_memory > 100_000_000 {
            return Err(JsValue::from_str("Memory limit exceeded"));
        }
        Ok(SecureProcessor { /* ... */ })
    }
}
```

---

## Layer 2: Secure Patterns

### Input Validation

```rust
#[wasm_bindgen]
pub fn process_data(data: &[u8]) -> Result<Vec<u8>, JsValue> {
    // Validate input
    if data.len() > 1_000_000 {
        return Err(JsValue::from_str("Input too large"));
    }
    
    // Validate content
    if !data.iter().all(|&b| b < 128) {
        return Err(JsValue::from_str("Invalid data format"));
    }
    
    // Process safely
    // ...
}
```

---

## Next Steps

Continue to [WASM Toolchain](./12-wasm-toolchain.md)