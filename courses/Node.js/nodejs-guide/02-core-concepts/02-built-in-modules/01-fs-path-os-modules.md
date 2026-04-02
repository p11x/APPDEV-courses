# File System, Path, and OS Modules Deep Dive

## What You'll Learn

- File System (fs) module: reading, writing, watching
- Path module: cross-platform path manipulation
- OS module: system information retrieval
- Combining modules for practical applications

## File System Module (node:fs)

### Reading Files

```javascript
// Modern approach: fs/promises with async/await
import { readFile, readdir, stat } from 'node:fs/promises';
import { join } from 'node:path';

// Read entire file as string
const content = await readFile('./data.txt', 'utf-8');
console.log(content);

// Read as Buffer (binary)
const buffer = await readFile('./image.png');

// Read directory listing
const entries = await readdir('./src', { withFileTypes: true });
for (const entry of entries) {
    console.log(`${entry.isDirectory() ? '[DIR]' : '[FILE]'} ${entry.name}`);
}

// Check file stats
const stats = await stat('./package.json');
console.log(`Size: ${stats.size} bytes`);
console.log(`Modified: ${stats.mtime}`);
console.log(`Is file: ${stats.isFile()}`);
console.log(`Is directory: ${stats.isDirectory()}`);
```

### Writing Files

```javascript
import { writeFile, appendFile, mkdir } from 'node:fs/promises';

// Write/overwrite file
await writeFile('./output.txt', 'Hello, World!', 'utf-8');

// Write JSON
const data = { users: [{ id: 1, name: 'Alice' }] };
await writeFile('./data.json', JSON.stringify(data, null, 2));

// Append to file (logs)
await appendFile('./log.txt', `${new Date().toISOString()} - Event\n`);

// Create directory (recursive)
await mkdir('./output/deep/path', { recursive: true });

// Write with options
await writeFile('./config.json', JSON.stringify(config), {
    mode: 0o600, // Owner read/write only
});
```

### Watching Files

```javascript
import { watch } from 'node:fs';

// Watch single file
const watcher = watch('./config.json', (eventType, filename) => {
    console.log(`${eventType}: ${filename}`);
});

// Watch directory recursively
const dirWatcher = watch('./src', { recursive: true }, (eventType, filename) => {
    console.log(`[${eventType}] ${filename}`);
});

// Cleanup
process.on('SIGINT', () => {
    watcher.close();
    dirWatcher.close();
});
```

## Path Module (node:path)

### Path Manipulation

```javascript
import path from 'node:path';

// Join path segments (cross-platform)
const fullPath = path.join('/users', 'alice', 'documents', 'file.txt');
// '/users/alice/documents/file.txt'

// Resolve to absolute path
const absolute = path.resolve('src', 'utils', 'helper.js');
// '/full/path/to/src/utils/helper.js'

// Get directory, filename, extension
const filePath = '/users/alice/documents/report.pdf';
path.dirname(filePath);   // '/users/alice/documents'
path.basename(filePath);  // 'report.pdf'
path.basename(filePath, '.pdf'); // 'report'
path.extname(filePath);   // '.pdf'

// Parse path to object
path.parse('/users/alice/report.pdf');
// { root: '/', dir: '/users/alice', base: 'report.pdf', ext: '.pdf', name: 'report' }

// Format object back to path
path.format({ dir: '/users/alice', name: 'report', ext: '.pdf' });
// '/users/alice/report.pdf'

// Normalize path (remove redundant separators)
path.normalize('/users//alice/../bob/./file.txt');
// '/users/bob/file.txt'

// Check if path is absolute
path.isAbsolute('/users/alice'); // true
path.isAbsolute('users/alice');  // false

// Relative path between two paths
path.relative('/users/alice/docs', '/users/bob/downloads');
// '../../bob/downloads'
```

### ESM __dirname Equivalent

```javascript
import { fileURLToPath } from 'node:url';
import { dirname, join } from 'node:path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// Now use like CommonJS
const configPath = join(__dirname, 'config.json');
const dataDir = join(__dirname, '..', 'data');
```

### Path Constants

```javascript
import path from 'node:path';

console.log(path.sep);      // '/' on Unix, '\\' on Windows
console.log(path.delimiter); // ':' on Unix, ';' on Windows
console.log(path.posix.sep); // '/' always
console.log(path.win32.sep); // '\\' always
```

## OS Module (node:os)

### System Information

```javascript
import os from 'node:os';

// Platform information
console.log(os.platform());     // 'linux', 'darwin', 'win32'
console.log(os.arch());         // 'x64', 'arm64'
console.log(os.type());         // 'Linux', 'Darwin', 'Windows_NT'
console.log(os.release());      // Kernel version
console.log(os.version());      // OS version string
console.log(os.hostname());     // Machine hostname
console.log(os.homedir());      // User home directory
console.log(os.tmpdir());       // Temp directory

// CPU information
const cpus = os.cpus();
console.log(`CPU: ${cpus[0].model}`);
console.log(`Cores: ${cpus.length}`);
console.log(`Speed: ${cpus[0].speed} MHz`);

// Memory information (in bytes)
const totalMem = os.totalmem();
const freeMem = os.freemem();
const usedPercent = ((totalMem - freeMem) / totalMem * 100).toFixed(1);
console.log(`Total: ${(totalMem / 1024 / 1024 / 1024).toFixed(1)} GB`);
console.log(`Free:  ${(freeMem / 1024 / 1024 / 1024).toFixed(1)} GB`);
console.log(`Used:  ${usedPercent}%`);

// Network interfaces
const nets = os.networkInterfaces();
for (const [name, interfaces] of Object.entries(nets)) {
    for (const net of interfaces) {
        if (net.family === 'IPv4' && !net.internal) {
            console.log(`${name}: ${net.address}`);
        }
    }
}

// System uptime
const uptimeSeconds = os.uptime();
const days = Math.floor(uptimeSeconds / 86400);
const hours = Math.floor((uptimeSeconds % 86400) / 3600);
console.log(`Uptime: ${days}d ${hours}h`);

// Load average (Unix only)
console.log(os.loadavg()); // [1min, 5min, 15min averages]

// User info
console.log(os.userInfo());
// { uid, gid, username, homedir, shell }
```

### System Dashboard Example

```javascript
// system-dashboard.js — Complete system info display
import os from 'node:os';

function formatBytes(bytes) {
    const units = ['B', 'KB', 'MB', 'GB', 'TB'];
    let i = 0;
    while (bytes >= 1024 && i < units.length - 1) {
        bytes /= 1024;
        i++;
    }
    return `${bytes.toFixed(1)} ${units[i]}`;
}

const dashboard = {
    platform: `${os.type()} ${os.release()}`,
    arch: os.arch(),
    hostname: os.hostname(),
    cpu: `${os.cpus()[0].model} (${os.cpus().length} cores)`,
    memory: {
        total: formatBytes(os.totalmem()),
        free: formatBytes(os.freemem()),
        used: `${((1 - os.freemem() / os.totalmem()) * 100).toFixed(1)}%`,
    },
    uptime: `${Math.floor(os.uptime() / 86400)}d ${Math.floor((os.uptime() % 86400) / 3600)}h`,
    user: os.userInfo().username,
};

console.log('=== System Dashboard ===\n');
for (const [key, value] of Object.entries(dashboard)) {
    console.log(`${key.padEnd(12)}: ${typeof value === 'object' ? JSON.stringify(value) : value}`);
}
```

## Best Practices Checklist

- [ ] Always use `node:fs/promises` for async file operations
- [ ] Use `node:path` for cross-platform path handling
- [ ] Use `path.join()` instead of string concatenation for paths
- [ ] Check file existence with `fs.access()` before reading
- [ ] Use `path.resolve()` for absolute paths from relative
- [ ] Cache OS information (cpu, memory) instead of polling

## Cross-References

- See [Events and HTTP Modules](./02-events-http-modules.md) for more core modules
- See [URL and Process Utilities](./03-url-process-utilities.md) for URL handling
- See [Stream Architecture](../05-stream-architecture/01-readable-writable-streams.md) for file streaming

## Next Steps

Continue to [Events and HTTP Modules](./02-events-http-modules.md) for event-driven programming.
