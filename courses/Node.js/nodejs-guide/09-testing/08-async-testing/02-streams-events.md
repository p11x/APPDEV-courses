# Testing Streams, Events, and Callbacks

## What You'll Learn

- Testing Node.js streams
- Testing event emitters
- Testing callback-based code
- Testing timers and intervals

## Stream Testing

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
function arrayToReadable(arr, objectMode = false) {
    let index = 0;
    return new Readable({
        objectMode,
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

        const input = arrayToReadable(['hello ', 'world']);
        const output = await collectStream(input.pipe(upperCase));

        assert.deepEqual(output.map(c => c.toString()), ['HELLO ', 'WORLD']);
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
        ], true);

        const output = await collectStream(input.pipe(filter));
        assert.equal(output.length, 2);
        assert.deepEqual(output.map(r => r.id), [1, 3]);
    });

    test('handles stream errors', async () => {
        const errorTransform = new Transform({
            transform(chunk, encoding, callback) {
                callback(new Error('Transform failed'));
            },
        });

        await assert.rejects(
            () => pipeline(
                arrayToReadable(['data']),
                errorTransform,
                new Writable({ write(c, e, cb) { cb(); } })
            ),
            { message: 'Transform failed' }
        );
    });

    test('counts chunks', async () => {
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

## Event Emitter Testing

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { EventEmitter } from 'node:events';

describe('Event Emitter Testing', () => {
    test('emits events', async () => {
        const emitter = new EventEmitter();
        const events = [];

        emitter.on('data', (value) => events.push(value));

        emitter.emit('data', 'first');
        emitter.emit('data', 'second');

        assert.deepEqual(events, ['first', 'second']);
    });

    test('waits for specific event', async () => {
        const emitter = new EventEmitter();

        setTimeout(() => emitter.emit('ready', { status: 'ok' }), 50);

        const result = await new Promise((resolve) => {
            emitter.on('ready', resolve);
        });

        assert.equal(result.status, 'ok');
    });

    test('handles event with timeout', async () => {
        const emitter = new EventEmitter();

        const result = await Promise.race([
            new Promise((resolve) => emitter.on('data', resolve)),
            new Promise((_, reject) =>
                setTimeout(() => reject(new Error('Timeout')), 100)
            ),
        ]);

        // Will reject because no event is emitted
        await assert.rejects(() => result, { message: 'Timeout' });
    });

    test('removes event listeners', () => {
        const emitter = new EventEmitter();
        let count = 0;
        const handler = () => count++;

        emitter.on('data', handler);
        emitter.emit('data');
        emitter.removeListener('data', handler);
        emitter.emit('data');

        assert.equal(count, 1);
    });
});
```

## Callback Testing

```javascript
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';

function callbackOperation(input, callback) {
    setTimeout(() => {
        if (!input) callback(new Error('Input required'));
        else callback(null, `processed: ${input}`);
    }, 10);
}

function promisify(fn) {
    return (...args) => new Promise((resolve, reject) => {
        fn(...args, (err, result) => {
            err ? reject(err) : resolve(result);
        });
    });
}

describe('Callback Testing', () => {
    test('successful callback', async () => {
        const op = promisify(callbackOperation);
        const result = await op('hello');
        assert.equal(result, 'processed: hello');
    });

    test('error callback', async () => {
        const op = promisify(callbackOperation);
        await assert.rejects(() => op(null), { message: 'Input required' });
    });

    test('callback with manual wrapping', (done) => {
        callbackOperation('test', (err, result) => {
            assert.equal(err, null);
            assert.equal(result, 'processed: test');
            done();
        });
    });
});
```

## Timer Testing

```javascript
describe('Timer Testing', () => {
    test('executes after delay', async () => {
        let executed = false;
        const promise = new Promise((resolve) => {
            setTimeout(() => {
                executed = true;
                resolve();
            }, 50);
        });

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

## Common Mistakes

- Not awaiting stream completion
- Not handling stream errors
- Not cleaning up event listeners (memory leaks)
- Using real timers in tests (slow)

## Cross-References

- See [Async Testing](./01-promises-async.md) for Promise testing
- See [Stream Testing](../../07-streams-and-buffers/11-stream-testing/01-unit-testing.md) for streams
- See [Unit Testing](../03-unit-testing/01-functions-classes.md) for sync patterns

## Next Steps

Continue to [Security Performance Testing](../09-security-performance/02-performance-load-testing.md).
