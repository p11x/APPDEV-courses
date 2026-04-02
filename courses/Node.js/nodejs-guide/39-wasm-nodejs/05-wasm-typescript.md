# WebAssembly with TypeScript

## What You'll Learn

- TypeScript integration with WebAssembly
- Type definitions for WASM modules
- Generating TypeScript from Rust
- Type-safe WASM APIs

---

## Layer 1: TypeScript Integration

### Manual Type Definitions

```typescript
// types/wasm.d.ts
export interface MathWasm {
  add(a: number, b: number): number;
  fibonacci(n: number): number;
  matrix_multiply(a: Float64Array, b: Float64Array, n: number): Float64Array;
}

declare const wasm: {
  MathWasm: new () => MathWasm;
};

export default wasm;
```

### Generated Types

```typescript
// Using wasm-bindgen generated types
import init, { calculate_stats, Stats } from './pkg/wasm_demo';

async function run() {
  await init();
  
  const numbers = new Float64Array([1, 2, 3, 4, 5]);
  const stats = calculate_stats(numbers);
  
  console.log(`Mean: ${stats.mean}`);
  console.log(`Variance: ${stats.variance}`);
}
```

---

## Layer 2: Type-Safe APIs

### Wrapper Classes

```typescript
// lib/wasm-wrapper.ts
import { create, Crypto, Matrix } from './pkg/my_wasm';

export class WasmCrypto {
  private crypto: Crypto;
  
  constructor() {
    this.crypto = create();
  }
  
  sha256(data: string): string {
    const hash = this.crypto.sha256(data);
    return hash.toHex();
  }
  
  encrypt(plaintext: string, key: string): string {
    return this.crypto.encrypt(plaintext, key).toBase64();
  }
}

export class WasmMatrix {
  private matrix: Matrix;
  
  constructor(rows: number, cols: number) {
    this.matrix = new Matrix(rows, cols);
  }
  
  multiply(other: WasmMatrix): WasmMatrix {
    const result = this.matrix.multiply(other.matrix);
    return new WasmMatrix(0, 0, result);
  }
}
```

---

## Next Steps

Continue to [WASM Performance](./06-wasm-performance.md)