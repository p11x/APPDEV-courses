# Working with Binary Data

## What You'll Learn

- Reading binary files
- Converting between encodings
- Binary protocols

## Reading Binary Files

```javascript
// binary-read.js - Reading binary files

import { readFile } from 'fs/promises';

// Read as buffer (binary)
const data = await readFile('image.png');

console.log(data); // Buffer of bytes
console.log(data.length); // File size in bytes
```

## Converting Encodings

```javascript
// encodings.js - Converting between encodings

const text = 'Hello, World!';

// Text to hex
const hex = Buffer.from(text).toString('hex');
console.log('Hex:', hex);

// Hex to text
const back = Buffer.from(hex, 'hex').toString();
console.log('Back:', back);

// Text to base64
const base64 = Buffer.from(text).toString('base64');
console.log('Base64:', base64);

// Base64 to text
const back64 = Buffer.from(base64, 'base64').toString();
console.log('Back:', back64);
```

## Binary Protocol Example

```javascript
// binary.js - Binary data handling

// Create a buffer for binary protocol
// Format: [version:1][type:1][length:2][data:length]
const buffer = Buffer.alloc(1 + 1 + 2 + 100);

// Write protocol
buffer.writeUInt8(1, 0);  // Version
buffer.writeUInt8(1, 1);  // Type
buffer.writeUInt16BE(10, 2);  // Length
buffer.write('Hello', 4);  // Data

// Read protocol
const version = buffer.readUInt8(0);
const type = buffer.readUInt8(1);
const length = buffer.readUInt16BE(2);
const data = buffer.slice(4, 4 + length).toString();

console.log({ version, type, length, data });
```

## Code Example

```javascript
// binary-demo.js - Binary data demonstration

console.log('=== Binary Data Demo ===\n');

// Read file as binary
const { writeFile, readFile } = await import('fs/promises');

// Create binary file
const binaryData = Buffer.from([0x89, 0x50, 0x4E, 0x47]); // PNG header bytes
await writeFile('binary.bin', binaryData);

// Read binary
const read = await readFile('binary.bin');
console.log('Binary data:', read);
console.log('As hex:', read.toString('hex'));

// Image processing example
console.log('\n=== Image Processing ===');

// Simulate image bytes
const imageBytes = Buffer.alloc(100);
for (let i = 0; i < 100; i++) {
  imageBytes[i] = i;
}

// Process as grayscale
const grayscale = imageBytes.map(byte => byte * 0.3);
console.log('Processed:', grayscale.length, 'bytes');
```

## Try It Yourself

### Exercise 1: Image Header
Read and display PNG file header bytes.

### Exercise 2: Convert Encoding
Convert text to hex and back.
