# Promise Implementation Details and State Machine

## What You'll Learn

- Promise state machine internals
- Promise resolution algorithm
- Microtask scheduling for Promises
- Transpilation of async/await

## Promise State Machine

```
Promise State Transitions:
─────────────────────────────────────────────
                 ┌──────────────┐
                 │   PENDING    │
                 │  (initial)   │
                 └──────┬───────┘
                        │
            ┌───────────┴───────────┐
            │                       │
            ▼                       ▼
    ┌───────────────┐      ┌───────────────┐
    │   FULFILLED   │      │   REJECTED    │
    │   (resolved)  │      │   (failed)    │
    └───────────────┘      └───────────────┘
    
    Terminal states — cannot transition again
```

### State Machine Implementation

```javascript
// Simplified Promise implementation showing state machine
class SimplePromise {
    static PENDING = 'pending';
    static FULFILLED = 'fulfilled';
    static REJECTED = 'rejected';

    constructor(executor) {
        this.state = SimplePromise.PENDING;
        this.value = undefined;
        this.reason = undefined;
        this.onFulfilled = [];
        this.onRejected = [];

        const resolve = (value) => {
            if (this.state === SimplePromise.PENDING) {
                this.state = SimplePromise.FULFILLED;
                this.value = value;
                // Execute queued handlers in microtask
                queueMicrotask(() => {
                    this.onFulfilled.forEach(fn => fn(value));
                });
            }
        };

        const reject = (reason) => {
            if (this.state === SimplePromise.PENDING) {
                this.state = SimplePromise.REJECTED;
                this.reason = reason;
                queueMicrotask(() => {
                    this.onRejected.forEach(fn => fn(reason));
                });
            }
        };

        try {
            executor(resolve, reject);
        } catch (err) {
            reject(err);
        }
    }

    then(onFulfilled, onRejected) {
        return new SimplePromise((resolve, reject) => {
            if (this.state === SimplePromise.FULFILLED) {
                queueMicrotask(() => {
                    try {
                        const result = onFulfilled(this.value);
                        resolve(result);
                    } catch (err) {
                        reject(err);
                    }
                });
            } else if (this.state === SimplePromise.REJECTED) {
                queueMicrotask(() => {
                    try {
                        const result = onRejected?.(this.reason);
                        resolve(result);
                    } catch (err) {
                        reject(err);
                    }
                });
            } else {
                // Still pending — queue handlers
                this.onFulfilled.push((value) => {
                    try {
                        const result = onFulfilled(value);
                        resolve(result);
                    } catch (err) {
                        reject(err);
                    }
                });
                this.onRejected.push((reason) => {
                    try {
                        const result = onRejected?.(reason);
                        resolve(result);
                    } catch (err) {
                        reject(err);
                    }
                });
            }
        });
    }

    catch(onRejected) {
        return this.then(null, onRejected);
    }

    finally(onFinally) {
        return this.then(
            (value) => { onFinally(); return value; },
            (reason) => { onFinally(); throw reason; }
        );
    }
}
```

## Promise Resolution Algorithm

```
Promise Resolution (x):
─────────────────────────────────────────────
1. If x is the same promise as the returned promise:
   → Reject with TypeError (circular reference)

2. If x is a Promise:
   → Adopt its state (chain to it)

3. If x is an object or function:
   → Try to get x.then
   → If x.then is a function, call it with x as this
   → If calling throws, reject with the error
   → If x.then returns a value y, recursively resolve(y)

4. Otherwise (x is a primitive):
   → Fulfill with x
```

```javascript
// Demonstration of resolution algorithm
const p1 = Promise.resolve(42);           // Primitive: fulfill immediately
const p2 = Promise.resolve(Promise.resolve(100)); // Promise: adopt state
const p3 = Promise.resolve({              // Thenable: call .then
    then(resolve) { resolve('thenable'); }
});

// Dangerous: circular reference
const circular = new Promise(resolve => {
    resolve(circular); // Resolving with itself
});
// TypeError: Chaining cycle detected
```

## Async/Await Transpilation

```javascript
// Source code with async/await
async function fetchUser(id) {
    const response = await fetch(`/api/users/${id}`);
    const user = await response.json();
    return user;
}

// Simplified transpilation to generator + Promise
function fetchUser(id) {
    return __awaiter(this, arguments, function* () {
        const response = yield fetch(`/api/users/${id}`);
        const user = yield response.json();
        return user;
    });
}

// Where __awaiter is approximately:
function __awaiter(thisArg, _arguments, generator) {
    return new Promise((resolve, reject) => {
        function step(result) {
            let { value, done } = generator.next(result);
            if (done) {
                resolve(value);
            } else {
                Promise.resolve(value).then(
                    val => step(val),
                    err => generator.throw(err)
                );
            }
        }
        step(undefined);
    });
}
```

## Promise Execution Order

```javascript
console.log('1: Script start');

setTimeout(() => console.log('2: setTimeout'), 0);

const promise = new Promise((resolve) => {
    console.log('3: Promise executor (sync)');
    resolve('4: Promise resolved');
});

promise.then((value) => {
    console.log(value);
    setTimeout(() => console.log('5: setTimeout in then'), 0);
});

Promise.resolve().then(() => console.log('6: Microtask'));

console.log('7: Script end');

// Output:
// 1: Script start
// 3: Promise executor (sync)
// 7: Script end
// 4: Promise resolved
// 6: Microtask
// 2: setTimeout
// 5: setTimeout in then
```

## Best Practices Checklist

- [ ] Understand that Promise executor runs synchronously
- [ ] Microtasks (Promise.then) run before macrotasks (setTimeout)
- [ ] Always return or throw in .then() handlers
- [ ] Avoid circular promise resolution
- [ ] Know that async/await is syntactic sugar for Promises

## Cross-References

- See [Event Loop](./01-event-loop-deep-dive.md) for event loop phases
- See [Task Scheduling](./03-task-scheduling.md) for scheduling strategies
- See [Promise Basics](../03-promises/01-promise-basics.md) for Promise usage
- See [Promise Combinators](../03-promises/03-promise-combinators.md) for combinators

## Next Steps

Continue to [Task Scheduling Strategies](./03-task-scheduling.md) for optimization.
