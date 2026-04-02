# WebAssembly Future

## What You'll Learn

- Upcoming WASM features
- Component Model and WASI
- Garbage Collection proposal
- Roadmap and trends

---

## Layer 1: Upcoming Features

### WASI (WebAssembly System Interface)

WASI provides standardized system calls for WASM:

```rust
// Using WASI
use std::fs::File;
use std::io::Read;

fn read_file(path: &str) -> std::io::Result<String> {
    let mut file = File::open(path)?;
    let mut contents = String::new();
    file.read_to_string(&mut contents)?;
    Ok(contents)
}
```

### GC Proposal

Garbage-collected language support:

- Native GC for Swift, Kotlin, Go
- Simplified memory management
- Smaller binaries

### Component Model

- Better language interoperability
- Virtualized I/O
- Resource handles

---

## Layer 2: Roadmap

| Feature | Status | ETA |
|---------|--------|-----|
| WASI Preview 2 | Stable | Now |
| GC Proposal | In Progress | 2025 |
| Component Model | In Progress | 2025 |
| 64-bit Memory | In Progress | 2025 |
| Threads | In Progress | 2025 |

---

## Next Steps

Continue to [WASM Alternatives](./19-wasm-alternatives.md)