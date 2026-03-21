# Transform Streams

## What You'll Learn

- Creating transform streams
- Processing data as it passes through

## What are Transform Streams?

Transform streams modify data as it passes through. They combine readable and writable streams.

## Creating a Transform Stream

```javascript
// transform.js - Creating transform streams

import { Transform } from 'stream';

const upperCase = new Transform({
  transform(chunk, encoding, callback) {
    // Convert chunk to uppercase
    const upperCased = chunk.toString().toUpperCase();
    callback(null, upperCased);
  }
});
```

## Using Transform Streams

```javascript
import { pipeline } from 'stream/promises';
import { createReadStream, createWriteStream } from 'fs';
import { Transform } from 'stream';

const upperCase = new Transform({
  transform(chunk, encoding, callback) {
    callback(null, chunk.toString().toUpperCase());
  }
});

await pipeline(
  createReadStream('input.txt'),
  upperCase,
  createWriteStream('output.txt')
);
```

## Code Example

```javascript
// transform-demo.js - Complete transform example

import { Transform } from 'stream';

const lineCounter = new Transform({
  objectMode: true,
  transform(chunk, encoding, callback) {
    const lines = chunk.toString().split('\n');
    lines.forEach(line => {
      if (line.trim()) {
        this.push({ line: line.trim() });
      }
    });
    callback();
  }
});

lineCounter.on('data', (data) => {
  console.log('Line:', data.line);
});

lineCounter.write('Hello\nWorld\nTest');
lineCounter.end();
```

## Try It Yourself

### Exercise 1: Create Transform
Create a transform that adds line numbers to text.

### Exercise 2: Filter Data
Create a transform that filters specific content.
