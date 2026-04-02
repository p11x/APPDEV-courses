# Events and HTTP Modules Deep Dive

## What You'll Learn

- EventEmitter class fundamentals
- HTTP server and client implementations
- Building APIs with core modules
- Event-driven HTTP architecture

## Events Module (node:events)

### EventEmitter Fundamentals

```javascript
import { EventEmitter } from 'node:events';

const emitter = new EventEmitter();

// Register listeners
emitter.on('greet', (name) => {
    console.log(`Hello, ${name}!`);
});

// One-time listener
emitter.once('init', () => {
    console.log('Initialized (runs only once)');
});

// Emit events
emitter.emit('greet', 'Alice'); // Hello, Alice!
emitter.emit('greet', 'Bob');   // Hello, Bob!
emitter.emit('init');           // Initialized
emitter.emit('init');           // Nothing — already fired

// Listener management
emitter.off('greet', listener);      // Remove specific listener
emitter.removeAllListeners('greet'); // Remove all listeners for event
emitter.removeAllListeners();        // Remove ALL listeners

// Metadata
console.log(emitter.eventNames());     // ['greet']
console.log(emitter.listenerCount('greet')); // Number of listeners
console.log(emitter.getMaxListeners()); // Default: 10
emitter.setMaxListeners(20);
```

### Custom Event-Driven Classes

```javascript
import { EventEmitter } from 'node:events';

class TaskRunner extends EventEmitter {
    constructor() {
        super();
        this.tasks = [];
    }

    addTask(name, fn) {
        this.tasks.push({ name, fn });
        this.emit('taskAdded', name);
    }

    async run() {
        this.emit('start', { taskCount: this.tasks.length });

        for (const task of this.tasks) {
            this.emit('taskStart', task.name);
            try {
                const result = await task.fn();
                this.emit('taskComplete', { name: task.name, result });
            } catch (error) {
                this.emit('taskError', { name: task.name, error });
            }
        }

        this.emit('finish');
    }
}

// Usage
const runner = new TaskRunner();

runner.on('start', ({ taskCount }) => console.log(`Running ${taskCount} tasks`));
runner.on('taskStart', (name) => console.log(`Starting: ${name}`));
runner.on('taskComplete', ({ name, result }) => console.log(`Done: ${name}`));
runner.on('taskError', ({ name, error }) => console.error(`Failed: ${name} — ${error.message}`));
runner.on('finish', () => console.log('All tasks complete'));

runner.addTask('fetch', async () => 'data');
runner.addTask('process', async () => 'processed');
await runner.run();
```

### Error Event Handling

```javascript
// ALWAYS handle 'error' events — unhandled errors crash the process
emitter.on('error', (err) => {
    console.error('Emitter error:', err.message);
});

// For async error handling
emitter.on('process', async (data) => {
    try {
        await processData(data);
    } catch (err) {
        emitter.emit('error', err);
    }
});
```

## HTTP Module (node:http)

### Basic HTTP Server

```javascript
import { createServer } from 'node:http';

const server = createServer((req, res) => {
    console.log(`${req.method} ${req.url}`);

    res.writeHead(200, { 'Content-Type': 'application/json' });
    res.end(JSON.stringify({
        message: 'Hello from Node.js!',
        timestamp: new Date().toISOString(),
    }));
});

server.listen(3000, () => {
    console.log('Server running on http://localhost:3000');
});

// Graceful shutdown
process.on('SIGINT', () => {
    server.close(() => console.log('Server closed'));
    process.exit(0);
});
```

### HTTP Routing

```javascript
import { createServer } from 'node:http';

const users = [
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' },
];

const server = createServer((req, res) => {
    const { method, url } = req;

    // Helper to send JSON
    const json = (status, data) => {
        res.writeHead(status, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify(data));
    };

    // Route matching
    if (method === 'GET' && url === '/api/users') {
        return json(200, users);
    }

    const userMatch = url?.match(/^\/api\/users\/(\d+)$/);
    if (userMatch) {
        const id = parseInt(userMatch[1]);
        const user = users.find(u => u.id === id);
        if (!user) return json(404, { error: 'User not found' });

        if (method === 'GET') return json(200, user);
        if (method === 'DELETE') {
            users.splice(users.indexOf(user), 1);
            return json(204, null);
        }
    }

    json(404, { error: 'Not found' });
});

server.listen(3000);
```

### Reading JSON Request Bodies

```javascript
import { createServer } from 'node:http';

function readBody(req) {
    return new Promise((resolve, reject) => {
        const chunks = [];
        req.on('data', chunk => chunks.push(chunk));
        req.on('end', () => {
            try {
                const body = Buffer.concat(chunks).toString();
                resolve(body ? JSON.parse(body) : {});
            } catch (err) {
                reject(new Error('Invalid JSON'));
            }
        });
        req.on('error', reject);
    });
}

const server = createServer(async (req, res) => {
    const json = (status, data) => {
        res.writeHead(status, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify(data));
    };

    if (req.method === 'POST' && req.url === '/api/users') {
        try {
            const body = await readBody(req);
            if (!body.name) return json(400, { error: 'Name required' });

            const user = { id: Date.now(), ...body };
            users.push(user);
            json(201, user);
        } catch (err) {
            json(400, { error: err.message });
        }
    }
});
```

### HTTP Client

```javascript
import { request } from 'node:http';

// GET request
const get = (url) => new Promise((resolve, reject) => {
    const req = request(url, (res) => {
        const data = [];
        res.on('data', chunk => data.push(chunk));
        res.on('end', () => resolve(JSON.parse(Buffer.concat(data).toString())));
    });
    req.on('error', reject);
    req.end();
});

// POST request
const post = (url, body) => new Promise((resolve, reject) => {
    const data = JSON.stringify(body);
    const req = request(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Content-Length': Buffer.byteLength(data),
        },
    }, (res) => {
        const chunks = [];
        res.on('data', chunk => chunks.push(chunk));
        res.on('end', () => resolve(JSON.parse(Buffer.concat(chunks).toString())));
    });
    req.on('error', reject);
    req.write(data);
    req.end();
});

// Usage (prefer fetch() in Node.js 18+)
// const response = await fetch('http://localhost:3000/api/users', {
//     method: 'POST',
//     headers: { 'Content-Type': 'application/json' },
//     body: JSON.stringify({ name: 'Charlie' }),
// });
```

## Best Practices Checklist

- [ ] Always handle 'error' events on EventEmitters
- [ ] Use `server.close()` for graceful shutdown
- [ ] Parse request bodies as streams (not buffered)
- [ ] Set proper Content-Type headers
- [ ] Use `fetch()` for HTTP clients (Node.js 18+)
- [ ] Implement request timeouts

## Cross-References

- See [Event Emitters Advanced](../04-event-emitters-advanced/01-eventemitter-fundamentals.md) for advanced patterns
- See [FS and Path Modules](./01-fs-path-os-modules.md) for file operations
- See [Error Handling](../11-error-handling/01-error-propagation.md) for error patterns

## Next Steps

Continue to [URL and Process Utilities](./03-url-process-utilities.md) for URL handling.
