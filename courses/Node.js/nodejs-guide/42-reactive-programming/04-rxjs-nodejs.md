# RxJS in Node.js

## What You'll Learn

- Node.js event integration
- Stream processing
- File system streams
- HTTP streaming

---

## Node.js Integration

```typescript
import { fromEvent, fromEventPattern } from 'rxjs';
import { EventEmitter } from 'events';

// EventEmitter to Observable
const stream = fromEvent(emitter, 'data');

// Node.js callback pattern
import { fromCallback } from 'rxjs';
import * as fs from 'fs';

const readFile$ = fromCallback(fs.readFile);

readFile$('./file.txt').subscribe(content => {
  console.log(content);
});
```