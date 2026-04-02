# WebAssembly Testing

## What You'll Learn

- Testing WebAssembly modules
- Unit and integration tests
- Property-based testing
- Performance testing

---

## Layer 1: Unit Testing

### Rust Unit Tests

```rust
#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_add() {
        assert_eq!(add(2, 3), 5);
    }

    #[test]
    fn test_fibonacci() {
        assert_eq!(fibonacci(0), 0);
        assert_eq!(fibonacci(1), 1);
        assert_eq!(fibonacci(10), 55);
    }

    #[test]
    #[should_panic(expected = "index out of bounds")]
    fn test_out_of_bounds() {
        let arr = [1, 2, 3];
        let _ = get_element(&arr, 10);
    }
}
```

### JavaScript Tests

```typescript
import { describe, it, expect, beforeAll } from 'vitest';
import init, { add, fibonacci } from './pkg/wasm_demo';

describe('WASM Functions', () => {
  beforeAll(async () => {
    await init();
  });

  it('adds two numbers', () => {
    expect(add(2, 3)).toBe(5);
  });

  it('calculates fibonacci correctly', () => {
    expect(fibonacci(10)).toBe(55);
  });
});
```

---

## Layer 2: Property-Based Testing

```typescript
import fc from 'fast-check';

describe('WASM Properties', () => {
  it('add is commutative', () => {
    fc.assert(fc.property(fc.integer(), fc.integer(), (a, b) => {
      return add(a, b) === add(b, a);
    }));
  });

  it('add is associative', () => {
    fc.assert(fc.property(fc.integer(), fc.integer(), fc.integer(), (a, b, c) => {
      return add(add(a, b), c) === add(a, add(b, c));
    }));
  });
});
```

---

## Next Steps

Continue to [WASM Deployment](./15-wasm-deployment.md)