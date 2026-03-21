# Readable Streams

## What You'll Learn

- What streams are
- Reading from readable streams
- Events: data, end, error

## What are Streams?

Streams are a way to handle data piece by piece rather than all at once. They're useful for:
- Large files
- Network data
- Real-time processing

## Reading Files with Streams

```javascript
// readable.js - Reading with streams

import { createReadStream } from 'fs';

const stream = createReadStream('file.txt', { encoding: 'utf8' });

// Data event - received a chunk
stream.on('data', (chunk) => {
  console.log('Received:', chunk.length, 'bytes');
});

// End event - finished reading
stream.on('end', () => {
  console.log('Finished reading');
});

// Error event
stream.on('error', (error) => {
  console.error('Error:', error);
});
```

## Code Example

```javascript
// stream-demo.js - Complete stream example

import { createReadStream } from 'fs';
import { readFile } from 'fs/promises';

console.log('=== Readable Stream Demo ===\n');

// Create a test file
await writeFile('test.txt', 'Hello, World! This is a test file.\n'.repeat(100));

// Read with stream
console.log('1. Reading with stream:');
const readStream = createReadStream('test.txt', { 
  encoding: 'utf8',
  highWaterMark: 1024  // 1KB chunks
});

let chunkCount = 0;
readStream.on('data', (chunk) => {
  chunkCount++;
  console.log(`   Chunk ${chunkCount}: ${chunk.length} bytes`);
});

readStream.on('end', () => {
  console.log(`   Total: ${chunkCount} chunks\n`);
  
  // Clean up
  import { unlink } from 'fs/promises';
  await unlink('test.txt');
});

readStream.on('error', console.error);
```

## Try It Yourself

### Exercise 1: Read Large File
Use streams to read a large file efficiently.

### Exercise 2: Count Chunks
Count how many chunks are received when reading a file.
