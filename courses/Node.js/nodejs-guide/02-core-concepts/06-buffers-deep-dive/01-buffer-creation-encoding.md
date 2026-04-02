# Buffer Creation Methods and String Encoding

## What You'll Learn

- Buffer creation methods and performance
- String encoding/decoding patterns
- Buffer operations and manipulation
- Memory implications of buffers

## Buffer Creation

```javascript
// Method 1: Buffer.from() — from existing data
const fromString = Buffer.from('Hello, World!', 'utf-8');
const fromArray = Buffer.from([72, 101, 108, 108, 111]);
const fromHex = Buffer.from('48656c6c6f', 'hex');
const fromBase64 = Buffer.from('SGVsbG8=', 'base64');

// Method 2: Buffer.alloc() — zeroed memory (SAFE)
const safe = Buffer.alloc(1024); // 1KB of zeros
console.log(safe); // <Buffer 00 00 00 ...>

// Method 3: Buffer.allocUnsafe() — uninitialized (FAST but risky)
const unsafe = Buffer.allocUnsafe(1024);
// May contain old data from memory — use fill() if needed
unsafe.fill(0);

// Method 4: Buffer.allocUnsafeSlow() — no pool allocation
const slow = Buffer.allocUnsafeSlow(256);
// Bypasses the internal buffer pool
```

## Encoding and Decoding

```javascript
// Supported encodings: utf8, ascii, utf16le/ucs2, base64, latin1, binary, hex

const text = 'Hello, World!';

// String → Buffer
const utf8Buf = Buffer.from(text, 'utf-8');
const hexBuf = Buffer.from(text, 'hex');    // Only valid hex strings
const base64Buf = Buffer.from(text, 'base64');

// Buffer → String
console.log(utf8Buf.toString('utf-8'));      // 'Hello, World!'
console.log(utf8Buf.toString('hex'));        // '48656c6c6f2c20576f726c6421'
console.log(utf8Buf.toString('base64'));     // 'SGVsbG8sIFdvcmxkIQ=='
console.log(utf8Buf.toString('ascii'));      // 'Hello, World!'

// Base64 encoding for API tokens
const token = Buffer.from('username:password').toString('base64');
console.log(token); // 'dXNlcm5hbWU6cGFzc3dvcmQ='
const decoded = Buffer.from(token, 'base64').toString('utf-8');
console.log(decoded); // 'username:password'
```

## Buffer Operations

```javascript
const buf = Buffer.from('Hello');

// Access individual bytes
console.log(buf[0]);        // 72 (H)
console.log(buf.length);    // 5

// Modify bytes
buf[0] = 74; // Change H to J
console.log(buf.toString()); // 'Jello'

// Slice (creates a view, not a copy)
const slice = buf.subarray(1, 4);
console.log(slice.toString()); // 'ell'

// Compare buffers
const a = Buffer.from('abc');
const b = Buffer.from('abc');
console.log(a.equals(b));    // true
console.log(a.compare(b));   // 0 (equal)

// Copy
const src = Buffer.from('Hello');
const dest = Buffer.alloc(5);
src.copy(dest);
console.log(dest.toString()); // 'Hello'

// Concatenate
const parts = [Buffer.from('Hello'), Buffer.from(' '), Buffer.from('World')];
const combined = Buffer.concat(parts);
console.log(combined.toString()); // 'Hello World'

// Fill
const buf10 = Buffer.alloc(10);
buf10.fill('AB');
console.log(buf10.toString()); // 'ABABABABAB'

// Search
const haystack = Buffer.from('Hello World');
console.log(haystack.indexOf('World'));  // 6
console.log(haystack.includes('Hello')); // true
```

## Buffer vs String Memory

```
Memory Comparison:
─────────────────────────────────────────────
String:    UTF-16 encoded, 2 bytes per ASCII char
Buffer:    Raw bytes, 1 byte per ASCII char

"Hello" as String: 10 bytes (5 × 2)
"Hello" as Buffer:  5 bytes (5 × 1)

For ASCII-heavy data: Buffer uses 50% less memory
For Unicode: String is often more efficient
```

```javascript
// Memory comparison
const str = 'a'.repeat(1000000);       // ~2MB in memory
const buf = Buffer.from('a'.repeat(1000000)); // ~1MB in memory

console.log('String:', str.length, 'chars');
console.log('Buffer:', buf.length, 'bytes');
```

## Best Practices Checklist

- [ ] Always use `Buffer.alloc()` over `Buffer.allocUnsafe()`
- [ ] Specify encoding explicitly (don't rely on defaults)
- [ ] Use Buffer for binary data, String for text
- [ ] Be aware of Buffer vs String memory differences
- [ ] Use `Buffer.concat()` for joining buffers

## Cross-References

- See [Binary Data](./02-binary-data-optimization.md) for optimization
- See [Stream Architecture](../05-stream-architecture/01-readable-writable-streams.md) for streaming
- See [Memory Architecture](../03-memory-architecture/01-heap-stack-allocation.md) for memory

## Next Steps

Continue to [Binary Data Optimization](./02-binary-data-optimization.md) for performance.
