# Buffer Basics

## What You'll Learn

- What buffers are
- Creating buffers
- Buffer encodings

## What is a Buffer?

A buffer is a region of memory that stores binary data. It's useful when working with:
- Files
- Network data
- Binary protocols

## Creating Buffers

```javascript
// buffer-create.js - Creating buffers

// From string
const buf1 = Buffer.from('Hello');
console.log(buf1); // <Buffer 48 65 6c 6c 6f>

// Empty buffer
const buf2 = Buffer.alloc(10); // 10 bytes

// With encoding
const buf3 = Buffer.from('Hello', 'utf8');
const buf4 = Buffer.from('Hello', 'hex');
const buf5 = Buffer.from('Hello', 'base64');
```

## Buffer Methods

```javascript
const buf = Buffer.from('Hello World');

// to string
console.log(buf.toString()); // Hello World
console.log(buf.toString('hex')); // 48656c6c6f20576f726c64

// length
console.log(buf.length); // 11

// access bytes
console.log(buf[0]); // 72 (H in ASCII)

// slice
console.log(buf.slice(0, 5).toString()); // Hello
```

## Code Example

```javascript
// buffer-demo.js - Buffer demonstration

console.log('=== Buffer Demo ===\n');

// Create buffer from string
const buf = Buffer.from('Hello');
console.log('1. From string:', buf);
console.log('   Length:', buf.length);
console.log('   As string:', buf.toString());

// Buffer from hex
const hex = Buffer.from('48656c6c6f', 'hex');
console.log('\n2. From hex:', hex.toString());

// Buffer from base64
const b64 = Buffer.from('SGVsbG8=', 'base64');
console.log('\n3. From base64:', b64.toString());

// Allocating buffer
const allocBuf = Buffer.alloc(5);
console.log('\n4. Allocated buffer:', allocBuf);

// Fill buffer
allocBuf.fill('A');
console.log('   After fill:', allocBuf.toString());

// Write to buffer
const writeBuf = Buffer.alloc(10);
writeBuf.write('Hello', 0);
console.log('\n5. After write:', writeBuf.toString());
```

## Try It Yourself

### Exercise 1: Create Buffers
Create buffers from strings using different encodings.

### Exercise 2: Buffer Operations
Practice buffer slicing and manipulation.
