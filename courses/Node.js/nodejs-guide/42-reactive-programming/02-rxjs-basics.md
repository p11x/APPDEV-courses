# RxJS Basics

## What You'll Learn

- Creating observables
- Subscribing and unsubscribing
- Hot vs cold observables
- Subject types

---

## Creating Observables

```typescript
import { Observable, of, from, interval, timer } from 'rxjs';

// From promise
from(fetch('/api/data'));

// From array
from([1, 2, 3]);

// Interval
interval(1000); // emits every 1s

// Timer
timer(5000); // emits once after 5s

// Custom
new Observable(subscriber => {
  subscriber.next('value');
  subscriber.complete();
});
```