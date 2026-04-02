# Chrome DevTools Node.js Debugging

## What You'll Learn

- Setting up Chrome DevTools for Node.js
- Debugging Node.js applications
- Performance profiling with DevTools
- Memory leak detection

## Chrome DevTools for Node.js

### Starting Node.js with Inspector

```bash
# Basic debugging
node --inspect app.js

# Break on first line
node --inspect-brk app.js

# Custom host and port
node --inspect=0.0.0.0:9229 app.js

# For cluster mode
node --inspect=0.0.0.0:9229 cluster.js
```

### Connecting to Chrome DevTools

```bash
# Open Chrome
# Navigate to: chrome://inspect

# Or use direct URL
chrome-devtools://devtools/bundled/inspector.html?experiments=true&v8only=true&ws=127.0.0.1:9229/[UUID]
```

## Debugging Node.js Applications

### Setting Breakpoints

```javascript
// app.js
function calculateTotal(items) {
    let total = 0;
    
    for (const item of items) {
        // Set breakpoint here
        debugger;
        total += item.price * item.quantity;
    }
    
    return total;
}

const items = [
    { name: 'Apple', price: 1.5, quantity: 3 },
    { name: 'Banana', price: 0.5, quantity: 5 }
];

const total = calculateTotal(items);
console.log('Total:', total);
```

### Conditional Breakpoints

```javascript
// Break only when condition is true
function processUsers(users) {
    for (const user of users) {
        // Break only for admin users
        if (user.role === 'admin') {
            debugger; // Conditional breakpoint
        }
        processUser(user);
    }
}
```

### Using DevTools Interface

```javascript
// Console panel
// Evaluate expressions
items.length
items.map(i => i.price)

// Watch panel
// Add expressions to watch
// - total
// - items.length
// - items[0].name

// Call stack
// View function call hierarchy
// Navigate through stack frames

// Scope
// View local and global variables
// Inspect object properties
```

## Performance Profiling

### CPU Profiling

```javascript
// Start CPU profiling
console.profile('calculateTotal');

function calculateTotal(items) {
    let total = 0;
    for (const item of items) {
        total += item.price * item.quantity;
    }
    return total;
}

// Run multiple iterations
for (let i = 0; i < 10000; i++) {
    calculateTotal(items);
}

console.profileEnd('calculateTotal');
```

### Using DevTools Profiler

```javascript
// 1. Open DevTools (chrome://inspect)
// 2. Go to Profiler tab
// 3. Click "Start"
// 4. Run your code
// 5. Click "Stop"
// 6. Analyze the profile

// Or use Performance tab for timeline
```

### Memory Profiling

```javascript
// Take heap snapshots
const v8 = require('v8');
const fs = require('fs');

function takeHeapSnapshot() {
    const snapshotStream = v8.writeHeapSnapshot();
    console.log(`Heap snapshot written to: ${snapshotStream}`);
}

// Call at specific points
takeHeapSnapshot();
```

## Memory Leak Detection

### Common Memory Leak Patterns

```javascript
// Pattern 1: Global variables
function leakyFunction() {
    // Accidental global
    leakedVar = 'This leaks';
}

// Pattern 2: Closures
function outer() {
    const largeData = new Array(1000000).fill('data');
    
    return function inner() {
        // largeData is retained even if not used
        console.log('inner called');
    };
}

// Pattern 3: Event listeners
function addListeners() {
    const element = document.getElementById('button');
    element.addEventListener('click', () => {
        // Listener never removed
    });
}

// Pattern 4: Timers
function startTimer() {
    setInterval(() => {
        // Timer never cleared
    }, 1000);
}
```

### Fixing Memory Leaks

```javascript
// Fix 1: Use strict mode
'use strict';

// Fix 2: Clear references
function processData() {
    const largeData = getData();
    const result = process(largeData);
    largeData = null; // Allow GC
    return result;
}

// Fix 3: Remove event listeners
function setupListeners() {
    const element = document.getElementById('button');
    const handler = () => console.log('clicked');
    
    element.addEventListener('click', handler);
    
    // Cleanup function
    return () => {
        element.removeEventListener('click', handler);
    };
}

// Fix 4: Clear timers
function startTimer() {
    const intervalId = setInterval(() => {
        console.log('tick');
    }, 1000);
    
    // Cleanup function
    return () => {
        clearInterval(intervalId);
    };
}
```

## Debugging Async Code

### Promises

```javascript
async function fetchUser(id) {
    try {
        const response = await fetch(`/api/users/${id}`);
        const user = await response.json();
        return user;
    } catch (error) {
        console.error('Failed to fetch user:', error);
        throw error;
    }
}

// Debug with breakpoints in async functions
async function main() {
    debugger; // Break here
    const user = await fetchUser(1);
    debugger; // Break here
    console.log(user);
}
```

### Callbacks

```javascript
const fs = require('fs');

function readConfig(callback) {
    fs.readFile('config.json', 'utf8', (err, data) => {
        if (err) {
            debugger; // Break on error
            return callback(err);
        }
        
        try {
            const config = JSON.parse(data);
            debugger; // Break on success
            callback(null, config);
        } catch (parseErr) {
            debugger; // Break on parse error
            callback(parseErr);
        }
    });
}
```

## Debugging Tests

### Jest Debugging

```bash
# Debug Jest tests
node --inspect-brk node_modules/.bin/jest --runInBand

# Or in VS Code
# Add to launch.json
{
    "type": "node",
    "request": "launch",
    "name": "Jest Debug",
    "program": "${workspaceFolder}/node_modules/.bin/jest",
    "args": ["--runInBand"],
    "console": "integratedTerminal"
}
```

### Mocha Debugging

```bash
# Debug Mocha tests
node --inspect-brk node_modules/.bin/mocha

# Or specific test file
node --inspect-brk node_modules/.bin/mocha tests/test.js
```

## Advanced Debugging Techniques

### Source Maps

```javascript
// TypeScript configuration
// tsconfig.json
{
    "compilerOptions": {
        "sourceMap": true,
        "outDir": "./dist",
        "rootDir": "./src"
    }
}

// Webpack configuration
module.exports = {
    devtool: 'source-map',
    // ... other config
};
```

### Remote Debugging

```bash
# Debug remote Node.js process
node --inspect=0.0.0.0:9229 app.js

# From local machine
# Open chrome://inspect
# Click "Configure"
# Add remote IP:port
```

### Debugging Cluster Mode

```javascript
const cluster = require('cluster');
const http = require('http');

if (cluster.isMaster) {
    // Fork workers
    for (let i = 0; i < 4; i++) {
        cluster.fork();
    }
} else {
    // Each worker has different debug port
    // Worker 1: 9229
    // Worker 2: 9230
    // Worker 3: 9231
    // Worker 4: 9232
    
    http.createServer((req, res) => {
        res.writeHead(200);
        res.end('Hello World\n');
    }).listen(8000);
}
```

## Troubleshooting Common Issues

### Debugger Not Connecting

```bash
# Problem: Cannot connect to debugger
# Solution: Check port and firewall

# Check if port is open
netstat -an | grep 9229

# Try different port
node --inspect=0.0.0.0:9230 app.js

# Disable firewall temporarily
sudo ufw disable  # Linux
```

### Breakpoints Not Hit

```bash
# Problem: Breakpoints not working
# Solution: Check source maps

# Ensure source maps are enabled
node --inspect --enable-source-maps app.js

# Check file paths match
# Use absolute paths in debugger
```

### Performance Issues

```bash
# Problem: App slow when debugging
# Solution: Disable inspector when not needed

# Production
node app.js

# Development only
node --inspect app.js
```

## Best Practices Checklist

- [ ] Use --inspect for development debugging
- [ ] Set breakpoints at key points
- [ ] Use conditional breakpoints for specific cases
- [ ] Profile CPU and memory regularly
- [ ] Detect and fix memory leaks early
- [ ] Debug async code properly
- [ ] Use source maps for compiled code
- [ ] Document debugging procedures
- [ ] Clean up resources in tests
- [ ] Monitor performance in production

## Performance Optimization Tips

- Use --inspect only in development
- Disable inspector in production
- Profile before optimizing
- Use heap snapshots to find leaks
- Monitor event loop lag
- Track memory usage trends
- Use efficient debugging patterns

## Cross-References

- See [VS Code Debugging](./02-vscode-debugging.md) for IDE integration
- See [Testing Environment](../06-testing-environment/) for test debugging
- See [Performance Optimization](../06-performance-optimization/) for profiling
- See [Development Tools](../12-dev-tools-integration/) for debugging tools

## Next Steps

Now that Chrome DevTools debugging is set up, let's configure VS Code debugging. Continue to [VS Code Debugging Configuration](./02-vscode-debugging.md).