# Writable Streams

## What You'll Learn

- Writing to streams
- Finish and drain events

## Writing to Files

```javascript
// writable.js - Writing with streams

import { createWriteStream } from 'fs';

const writeStream = createWriteStream('output.txt');

writeStream.write('Hello, ');
writeStream.write('World!\n');
writeStream.write('More data...');

writeStream.end();

writeStream.on('finish', () => {
  console.log('Finished writing');
});
```

## Events

```javascript
// finish - all data written
writeStream.on('finish', () => {
  console.log('Done writing');
});

// drain - buffer empty, can write more
writeStream.on('drain', () => {
  console.log('Buffer drained, can write more');
});
```

## Code Example

```javascript
// write-demo.js - Writable stream example

import { createWriteStream } from 'fs';

const stream = createWriteStream('output.txt');

for (let i = 1; i <= 100; i++) {
  const canContinue = stream.write(`Line ${i}\n`);
  
  if (!canContinue) {
    await new Promise(resolve => stream.once('drain', resolve));
  }
}

stream.end();

stream.on('finish', () => {
  console.log('Writing complete!');
});
```

## Try It Yourself

### Exercise 1: Write with Streams
Write data to a file using writable streams.

### Exercise 2: Backpressure
Handle backpressure when writing large amounts of data.
