# Using Pipe

## What You'll Learn

- Piping streams together
- pipeline function
- Error handling with pipes

## Piping Streams

```javascript
// pipe.js - Using pipe to connect streams

import { createReadStream, createWriteStream } from 'fs';

const readStream = createReadStream('input.txt');
const writeStream = createWriteStream('output.txt');

// Pipe read to write
readStream.pipe(writeStream);

writeStream.on('finish', () => {
  console.log('Copy complete!');
});
```

## pipeline Function

The `pipeline` function handles errors automatically:

```javascript
// pipeline-example.js - Using pipeline

import { pipeline } from 'stream/promises';
import { createReadStream, createWriteStream } from 'fs';

await pipeline(
  createReadStream('input.txt'),
  createWriteStream('output.txt')
);

console.log('Pipeline complete!');
```

## Code Example

```javascript
// pipe-demo.js - Complete pipe demonstration

import { pipeline } from 'stream/promises';
import { createReadStream, createWriteStream } from 'fs';
import { createGzip } from 'zlib';

// Create test file
await writeFile('original.txt', 'Hello World!'.repeat(1000));

// Compress with pipe
await pipeline(
  createReadStream('original.txt'),
  createGzip(),
  createWriteStream('original.txt.gz')
);

console.log('Compressed!');

// Decompress
await pipeline(
  createReadStream('original.txt.gz'),
  createGzip().unzip(),
  createWriteStream('decompressed.txt')
);

console.log('Decompressed!');
```

## Try It Yourself

### Exercise 1: Copy File
Use pipe to copy a file.

### Exercise 2: Compress
Use pipe with gzip to compress a file.
