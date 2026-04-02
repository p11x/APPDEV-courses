# RxJS Testing

## What You'll Learn

- Testing observables
- Marble testing
- TestScheduler usage
- Integration testing patterns

---

## Testing Strategies

```typescript
import { TestScheduler } from 'rxjs/testing';

describe('Stream', () => {
  let scheduler: TestScheduler;

  beforeEach(() => {
    scheduler = new TestScheduler((actual, expected) => {
      expect(actual).toEqual(expected);
    });
  });

  it('transforms values', () => {
    scheduler.run(({ cold, expectObservable }) => {
      const source = cold('a-b-c|', { a: 1, b: 2, c: 3 });
      const expected = 'x-y-z|', { x: 2, y: 4, z: 6 };

      expectObservable(source.pipe(map(x => x * 2))).toBe(expected);
    });
  });
});
```
