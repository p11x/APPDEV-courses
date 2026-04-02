# URL, Process, and Utility Modules

## What You'll Learn

- URL class and URLSearchParams for URL manipulation
- Process object for application lifecycle
- Util module helpers
- Crypto module basics

## URL Module (node:url)

### URL Class

```javascript
// Modern URL API (Web standard, preferred)
const url = new URL('https://example.com:8080/path/name?q=search&page=2#section');

// Properties
console.log(url.protocol);  // 'https:'
console.log(url.hostname);  // 'example.com'
console.log(url.port);      // '8080'
console.log(url.pathname);  // '/path/name'
console.log(url.search);    // '?q=search&page=2'
console.log(url.hash);      // '#section'
console.log(url.host);      // 'example.com:8080'
console.log(url.origin);    // 'https://example.com:8080'
console.log(url.href);      // Full URL string

// Modifying URL
url.pathname = '/new/path';
url.searchParams.set('page', '3');
url.hash = '#new-section';
console.log(url.href); // Updated URL

// Building URLs from parts
const apiUrl = new URL('https://api.example.com');
apiUrl.pathname = '/v2/users';
apiUrl.searchParams.set('limit', '50');
apiUrl.searchParams.set('offset', '0');
console.log(apiUrl.href);
// 'https://api.example.com/v2/users?limit=50&offset=0'
```

### URLSearchParams

```javascript
// Parse query string
const params = new URLSearchParams('?name=Alice&age=25&city=NYC');

// Get values
console.log(params.get('name'));    // 'Alice'
console.log(params.get('age'));     // '25'
console.log(params.has('city'));    // true
console.log(params.has('email'));   // false

// Iterate
for (const [key, value] of params) {
    console.log(`${key}: ${value}`);
}

// Modify
params.set('age', '26');
params.append('hobby', 'reading');
params.append('hobby', 'coding');
params.delete('city');

// Get all values for key
console.log(params.getAll('hobby')); // ['reading', 'coding']

// Build query string
const searchParams = new URLSearchParams();
searchParams.set('q', 'node.js tutorial');
searchParams.set('page', '1');
searchParams.set('limit', '20');
console.log(searchParams.toString());
// 'q=node.js+tutorial&page=1&limit=20'

// From object
const filters = { status: 'active', role: 'admin', sort: 'name' };
const qs = new URLSearchParams(filters).toString();
// 'status=active&role=admin&sort=name'
```

### File URLs

```javascript
import { fileURLToPath, pathToFileURL } from 'node:url';
import { dirname, join } from 'node:path';

// Convert file URL to path (for ESM __dirname)
const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// Convert path to file URL
const fileUrl = pathToFileURL('/Users/alice/project/config.json');
console.log(fileUrl.href);
// 'file:///Users/alice/project/config.json'

// Resolve relative to file URL
const configUrl = new URL('./config.json', import.meta.url);
const configPath = fileURLToPath(configUrl);
```

## Process Object

### Application Lifecycle

```javascript
// Process information
console.log(process.version);      // Node.js version 'v22.14.0'
console.log(process.versions);     // All dependency versions
console.log(process.platform);     // 'linux', 'darwin', 'win32'
console.log(process.arch);         // 'x64', 'arm64'
console.log(process.pid);          // Process ID
console.log(process.ppid);         // Parent process ID
console.log(process.cwd());        // Current working directory
console.log(process.execPath);     // Node.js executable path
console.log(process.title);        // Process title (ps output)
console.log(process.uptime());     // Seconds since start

// Command line arguments
console.log(process.argv);
// ['node', '/path/to/script.js', '--flag', 'value']

// Parse arguments
import { parseArgs } from 'node:util';
const { values, positionals } = parseArgs({
    options: {
        port: { type: 'string', short: 'p', default: '3000' },
        verbose: { type: 'boolean', short: 'v', default: false },
    },
    allowPositionals: true,
});
```

### Environment Variables

```javascript
// Read environment variables
const port = process.env.PORT || 3000;
const nodeEnv = process.env.NODE_ENV || 'development';
const dbUrl = process.env.DATABASE_URL;

// Set environment variable (current process only)
process.env.MY_VAR = 'value';

// Check environment
if (process.env.NODE_ENV === 'production') {
    console.log('Running in production mode');
}
```

### Memory Usage

```javascript
const usage = process.memoryUsage();
console.log({
    rss: `${(usage.rss / 1024 / 1024).toFixed(1)} MB`,        // Resident set size
    heapTotal: `${(usage.heapTotal / 1024 / 1024).toFixed(1)} MB`, // V8 heap total
    heapUsed: `${(usage.heapUsed / 1024 / 1024).toFixed(1)} MB`,   // V8 heap used
    external: `${(usage.external / 1024 / 1024).toFixed(1)} MB`,   // C++ objects
    arrayBuffers: `${(usage.arrayBuffers / 1024 / 1024).toFixed(1)} MB`,
});
```

### Signal Handling

```javascript
// Graceful shutdown
process.on('SIGTERM', () => {
    console.log('SIGTERM received, shutting down...');
    server.close(() => {
        console.log('Server closed');
        process.exit(0);
    });
});

process.on('SIGINT', () => {
    console.log('\nSIGINT received (Ctrl+C)');
    process.exit(0);
});

// Uncaught exceptions
process.on('uncaughtException', (err) => {
    console.error('Uncaught exception:', err);
    process.exit(1);
});

// Unhandled promise rejections
process.on('unhandledRejection', (reason, promise) => {
    console.error('Unhandled rejection:', reason);
});
```

## Util Module (node:util)

```javascript
import { promisify, inspect, types, format } from 'node:util';
import { callbackify } from 'node:util';

// Promisify callback-based functions
import { exec } from 'node:child_process';
const execAsync = promisify(exec);
const { stdout } = await execAsync('node --version');
console.log(stdout.trim());

// Inspect objects (like console.log but returns string)
const obj = { name: 'test', nested: { value: 42 } };
console.log(inspect(obj, { depth: null, colors: true }));

// Type checking
console.log(types.isDate(new Date()));       // true
console.log(types.isPromise(Promise.resolve())); // true
console.log(types.isRegExp(/test/));         // true

// Format strings
console.log(format('Hello %s, you have %d messages', 'Alice', 5));
// 'Hello Alice, you have 5 messages'

// Callbackify (opposite of promisify)
const asyncFn = async (x) => x * 2;
const callbackFn = callbackify(asyncFn);
callbackFn(5, (err, result) => console.log(result)); // 10
```

## Crypto Module Basics (node:crypto)

```javascript
import { randomBytes, createHash, createHmac } from 'node:crypto';

// Random bytes
const token = randomBytes(32).toString('hex');
console.log(token); // 64-char hex string

// Hashing
const hash = createHash('sha256')
    .update('hello world')
    .digest('hex');
console.log(hash); // SHA-256 hash

// HMAC (keyed hashing)
const hmac = createHmac('sha256', 'secret-key')
    .update('message')
    .digest('hex');
console.log(hmac);

// UUID generation (Node.js 19.0+)
import { randomUUID } from 'node:crypto';
const uuid = randomUUID();
console.log(uuid); // 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'
```

## Best Practices Checklist

- [ ] Use `URL` class for URL manipulation (not string concatenation)
- [ ] Use `URLSearchParams` for query string handling
- [ ] Handle `process.on('SIGTERM')` for graceful shutdown
- [ ] Use `parseArgs` from `node:util` for CLI argument parsing
- [ ] Use `promisify` to convert callback APIs to promises
- [ ] Use `node:crypto` for random values (not `Math.random()`)

## Cross-References

- See [FS and Path Modules](./01-fs-path-os-modules.md) for file operations
- See [Events and HTTP](./02-events-http-modules.md) for event-driven programming
- See [Process Lifecycle](../09-process-lifecycle/01-signal-handling.md) for lifecycle management

## Next Steps

Continue to [Memory Architecture](../03-memory-architecture/01-heap-stack-allocation.md) for memory internals.
