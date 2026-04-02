# Testing Async Operations: Promises, Streams, and Events

## What You'll Learn

- Testing async/await functions
- Testing Promises
- Testing streams
- Testing event emitters
- Testing timers and intervals

## Testing Async/Await

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';

describe('Async Operations', () => {
    test('resolves with expected value', async () => {
        const result = await fetchUser(1);
        assert.equal(result.name, 'Alice');
    });

    test('rejects with expected error', async () => {
        await assert.rejects(
            () => fetchUser(-1),
            { message: 'Invalid user ID' }
        );
    });

    test('rejects with specific error type', async () => {
        await assert.rejects(
            () => fetchUser(-1),
            (err) => {
                assert.equal(err.name, 'ValidationError');
                assert.equal(err.code, 'INVALID_ID');
                return true;
            }
        );
    });

    test('handles concurrent async operations', async () => {
        const results = await Promise.all([
            fetchUser(1),
            fetchUser(2),
            fetchUser(3),
        ]);

        assert.equal(results.length, 3);
        results.forEach(r => assert.ok(r.id));
    });

    test('handles mixed success/failure with allSettled', async () => {
        const results = await Promise.allSettled([
            fetchUser(1),
            fetchUser(-1),
            fetchUser(2),
        ]);

        assert.equal(results[0].status, 'fulfilled');
        assert.equal(results[1].status, 'rejected');
        assert.equal(results[2].status, 'fulfilled');
    });

    test('times out slow operations', async () => {
        const slowOp = new Promise(resolve => setTimeout(resolve, 10000));

        await assert.rejects(
            () => Promise.race([
                slowOp,
                new Promise((_, reject) =>
                    setTimeout(() => reject(new Error('Timeout')), 1000)
                ),
            ]),
            { message: 'Timeout' }
        );
    });
});
```

## Testing Streams

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { Readable, Writable, Transform } from 'node:stream';
import { pipeline } from 'node:stream/promises';

// Helper: collect stream output
async function collectStream(stream) {
    const chunks = [];
    for await (const chunk of stream) {
        chunks.push(chunk);
    }
    return chunks;
}

// Helper: create readable from array
function arrayToReadable(arr) {
    let index = 0;
    return new Readable({
        objectMode: true,
        read() {
            this.push(index < arr.length ? arr[index++] : null);
        },
    });
}

describe('Stream Testing', () => {
    test('transform converts to uppercase', async () => {
        const upperCase = new Transform({
            transform(chunk, encoding, callback) {
                callback(null, chunk.toString().toUpperCase());
            },
        });

        const input = arrayToReadable(['hello', ' world']);
        const output = await collectStream(input.pipe(upperCase));

        assert.deepEqual(output.map(c => c.toString()), ['HELLO', ' WORLD']);
    });

    test('pipeline handles errors', async () => {
        const errorTransform = new Transform({
            transform(chunk, encoding, callback) {
                callback(new Error('Transform failed'));
            },
        });

        await assert.rejects(
            () => pipeline(arrayToReadable(['data']), errorTransform, new Writable({ write(c, e, cb) { cb(); } })),
            { message: 'Transform failed' }
        );
    });

    test('filters records in object mode', async () => {
        const filter = new Transform({
            objectMode: true,
            transform(record, encoding, callback) {
                if (record.active) callback(null, record);
                else callback();
            },
        });

        const input = arrayToReadable([
            { id: 1, active: true },
            { id: 2, active: false },
            { id: 3, active: true },
        ]);

        const output = await collectStream(input.pipe(filter));
        assert.equal(output.length, 2);
        assert.deepEqual(output.map(r => r.id), [1, 3]);
    });

    test('counts stream chunks', async () => {
        let count = 0;
        const counter = new Writable({
            write(chunk, encoding, callback) {
                count++;
                callback();
            },
        });

        await pipeline(arrayToReadable([1, 2, 3, 4, 5]), counter);
        assert.equal(count, 5);
    });
});
```

## Testing Event Emitters

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { EventEmitter } from 'node:events';

describe('Event Emitter Testing', () => {
    test('emits data events', async () => {
        const emitter = new DataProcessor();
        const events = [];

        emitter.on('data', (data) => events.push(data));

        emitter.process(['a', 'b', 'c']);

        // Wait for async events
        await new Promise(resolve => emitter.on('end', resolve));

        assert.deepEqual(events, ['a', 'b', 'c']);
    });

    test('emits error on invalid input', async () => {
        const emitter = new DataProcessor();

        const errorPromise = new Promise(resolve => {
            emitter.on('error', resolve);
        });

        emitter.process(null);

        const error = await errorPromise;
        assert.equal(error.message, 'Invalid input');
    });

    test('waits for specific event', async () => {
        const emitter = new EventEmitter();

        setTimeout(() => emitter.emit('ready', { status: 'ok' }), 100);

        const result = await new Promise(resolve => {
            emitter.on('ready', resolve);
        });

        assert.equal(result.status, 'ok');
    });

    test('counts event emissions', async () => {
        const emitter = new DataProcessor();
        let count = 0;

        emitter.on('data', () => count++);

        emitter.process([1, 2, 3, 4, 5]);

        await new Promise(resolve => emitter.on('end', resolve));

        assert.equal(count, 5);
    });
});
```

## Testing Timers

```javascript
describe('Timer Testing', () => {
    test('executes after delay', async () => {
        let executed = false;
        const promise = delayedExecution(100, () => { executed = true; });

        assert.ok(!executed);
        await promise;
        assert.ok(executed);
    });

    test('polls until condition met', async () => {
        let attempts = 0;
        const result = await pollUntil(
            () => {
                attempts++;
                return attempts >= 3 ? 'done' : null;
            },
            { interval: 10, timeout: 1000 }
        );

        assert.equal(result, 'done');
        assert.equal(attempts, 3);
    });

    test('times out if condition never met', async () => {
        await assert.rejects(
            () => pollUntil(() => null, { interval: 10, timeout: 50 }),
            { message: /timeout/i }
        );
    });
});
```

## Best Practices Checklist

- [ ] Always use `async/await` for async tests
- [ ] Use `assert.rejects` for error cases
- [ ] Use `Promise.all` for concurrent operations
- [ ] Set timeouts for long-running operations
- [ ] Collect stream output with helper functions
- [ ] Use event promises for event emitter tests
- [ ] Clean up resources in `after` hooks

## Cross-References

- See [Unit Testing](../03-unit-testing/01-functions-classes.md) for sync patterns
- See [Stream Testing](../../07-streams-and-buffers/11-stream-testing/01-unit-testing.md) for streams
- See [Database Testing](../07-database-testing/01-unit-testing.md) for async DB ops

## Next Steps

Continue to [Security and Performance Testing](../09-security-performance/01-security-testing.md).
