# VS Code Debugging Configuration

## What You'll Learn

- Configuring VS Code for Node.js debugging
- Setting up launch configurations
- Debugging different scenarios
- Breakpoint strategies and inspection

## VS Code Launch Configurations

### Basic Launch Configuration

Create `.vscode/launch.json`:

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Launch Program",
            "skipFiles": [
                "<node_internals>/**"
            ],
            "program": "${workspaceFolder}/src/index.js",
            "outFiles": [
                "${workspaceFolder}/dist/**/*.js"
            ]
        }
    ]
}
```

### Multiple Configurations

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Launch Development",
            "program": "${workspaceFolder}/src/index.js",
            "env": {
                "NODE_ENV": "development",
                "DEBUG": "app:*"
            },
            "console": "integratedTerminal"
        },
        {
            "type": "node",
            "request": "launch",
            "name": "Launch Production",
            "program": "${workspaceFolder}/dist/index.js",
            "env": {
                "NODE_ENV": "production"
            }
        },
        {
            "type": "node",
            "request": "attach",
            "name": "Attach to Process",
            "port": 9229,
            "restart": true,
            "protocol": "inspector"
        }
    ]
}
```

## Debugging TypeScript

### TypeScript Launch Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Launch TypeScript",
            "program": "${workspaceFolder}/src/index.ts",
            "preLaunchTask": "tsc: build - tsconfig.json",
            "outFiles": [
                "${workspaceFolder}/dist/**/*.js"
            ],
            "sourceMaps": true
        }
    ]
}
```

### TypeScript Task Configuration

```json
// .vscode/tasks.json
{
    "version": "2.0.0",
    "tasks": [
        {
            "type": "typescript",
            "tsconfig": "tsconfig.json",
            "problemMatcher": [
                "$tsc"
            ],
            "group": {
                "kind": "build",
                "isDefault": true
            },
            "label": "tsc: build - tsconfig.json"
        }
    ]
}
```

## Debugging Tests

### Jest Debug Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Jest: Current File",
            "program": "${workspaceFolder}/node_modules/.bin/jest",
            "args": [
                "${file}",
                "--runInBand",
                "--no-cache"
            ],
            "console": "integratedTerminal",
            "internalConsoleOptions": "neverOpen"
        },
        {
            "type": "node",
            "request": "launch",
            "name": "Jest: All Tests",
            "program": "${workspaceFolder}/node_modules/.bin/jest",
            "args": [
                "--runInBand"
            ],
            "console": "integratedTerminal"
        }
    ]
}
```

### Mocha Debug Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Mocha: Current File",
            "program": "${workspaceFolder}/node_modules/.bin/mocha",
            "args": [
                "${file}",
                "--no-timeouts"
            ],
            "console": "integratedTerminal"
        }
    ]
}
```

## Debugging Express Applications

### Express Launch Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Express Server",
            "program": "${workspaceFolder}/src/server.js",
            "restart": true,
            "console": "integratedTerminal",
            "env": {
                "NODE_ENV": "development",
                "PORT": "3000"
            }
        },
        {
            "type": "node",
            "request": "launch",
            "name": "Express with Nodemon",
            "runtimeExecutable": "nodemon",
            "program": "${workspaceFolder}/src/server.js",
            "restart": true,
            "console": "integratedTerminal",
            "internalConsoleOptions": "neverOpen"
        }
    ]
}
```

## Debugging Cluster Mode

### Cluster Debug Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Cluster Master",
            "program": "${workspaceFolder}/src/cluster.js",
            "restart": true,
            "console": "integratedTerminal"
        },
        {
            "type": "node",
            "request": "attach",
            "name": "Attach to Worker 1",
            "port": 9229,
            "restart": true
        },
        {
            "type": "node",
            "request": "attach",
            "name": "Attach to Worker 2",
            "port": 9230,
            "restart": true
        }
    ]
}
```

## Breakpoint Types

### Regular Breakpoints

```javascript
// Click in gutter to set breakpoint
function calculate(x, y) {
    const sum = x + y; // Breakpoint here
    return sum;
}
```

### Conditional Breakpoints

```javascript
// Right-click breakpoint → Edit Breakpoint
function processItems(items) {
    for (const item of items) {
        // Break only when item.price > 100
        const total = item.price * item.quantity;
    }
}
```

### Logpoints

```javascript
// Right-click → Add Logpoint
function processUser(user) {
    // Log: User {user.name} processed
    // No code change needed
    return user;
}
```

### Hit Count Breakpoints

```javascript
// Break after N hits
function loop(n) {
    for (let i = 0; i < n; i++) {
        // Break when i === 50
        console.log(i);
    }
}
```

## Debug Console

### Evaluating Expressions

```javascript
// In Debug Console:
items.length
items.map(i => i.name)
JSON.stringify(items, null, 2)

// Call functions
calculateTotal(items)

// Modify variables
user.role = 'admin'
```

### Watch Expressions

```javascript
// Add to Watch panel:
items
items.length
items[0]
user.name
calculateTotal(items)
```

## Debugging Async Code

### Async Stack Traces

```javascript
async function fetchUserData(userId) {
    const user = await fetchUser(userId); // Breakpoint
    const orders = await fetchOrders(userId); // Breakpoint
    return { user, orders };
}

// VS Code shows full async call stack
```

### Promise Debugging

```javascript
function processData() {
    return fetch('/api/data')
        .then(response => response.json()) // Breakpoint
        .then(data => {
            console.log(data); // Breakpoint
            return data;
        })
        .catch(error => {
            console.error(error); // Breakpoint
            throw error;
        });
}
```

## Debugging Environment Variables

### Environment Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Launch with .env",
            "program": "${workspaceFolder}/src/index.js",
            "envFile": "${workspaceFolder}/.env",
            "env": {
                "DEBUG": "app:*",
                "NODE_ENV": "development"
            }
        }
    ]
}
```

## Remote Debugging

### Remote Attach Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "attach",
            "name": "Attach to Remote",
            "address": "192.168.1.100",
            "port": 9229,
            "localRoot": "${workspaceFolder}",
            "remoteRoot": "/app",
            "restart": true
        }
    ]
}
```

## Debugging NPM Scripts

### NPM Script Debug Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "NPM Start",
            "runtimeExecutable": "npm",
            "runtimeArgs": [
                "run",
                "start"
            ],
            "console": "integratedTerminal"
        },
        {
            "type": "node",
            "request": "launch",
            "name": "NPM Test",
            "runtimeExecutable": "npm",
            "runtimeArgs": [
                "run",
                "test"
            ],
            "console": "integratedTerminal"
        }
    ]
}
```

## Debugging Docker Containers

### Docker Debug Configuration

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "attach",
            "name": "Docker: Attach to Node",
            "remoteRoot": "/usr/src/app",
            "localRoot": "${workspaceFolder}",
            "address": "localhost",
            "port": 9229,
            "restart": true
        }
    ]
}
```

```yaml
# docker-compose.yml
version: '3.8'
services:
  app:
    build: .
    ports:
      - "3000:3000"
      - "9229:9229"
    command: node --inspect=0.0.0.0:9229 src/index.js
```

## Troubleshooting Common Issues

### Breakpoints Not Hit

```bash
# Problem: Breakpoints ignored
# Solution: Check source maps

# Ensure source maps enabled
{
    "sourceMaps": true,
    "outFiles": ["${workspaceFolder}/dist/**/*.js"]
}

# Check file paths match
# Use ${workspaceFolder} for absolute paths
```

### Debugger Fails to Start

```bash
# Problem: Cannot start debugger
# Solution: Check configuration

# Verify program path exists
# Check Node.js is installed
node --version

# Check port is not in use
netstat -an | grep 9229
```

### Slow Debugging

```bash
# Problem: Debugger is slow
# Solution: Optimize configuration

# Skip node internals
"skipFiles": ["<node_internals>/**"]

# Disable source maps if not needed
"sourceMaps": false

# Use external terminal
"console": "externalTerminal"
```

## Best Practices Checklist

- [ ] Create launch.json for your project
- [ ] Configure different debug scenarios
- [ ] Use conditional breakpoints wisely
- [ ] Set up logpoints for non-intrusive debugging
- [ ] Debug tests with proper configuration
- [ ] Configure environment variables
- [ ] Set up remote debugging when needed
- [ ] Document debug configurations
- [ ] Share launch.json with team
- [ ] Keep configurations up to date

## Performance Optimization Tips

- Skip node internals in debugging
- Use external terminal for I/O heavy apps
- Disable source maps in production
- Use conditional breakpoints to reduce stops
- Set up restart on file changes
- Configure proper timeout values
- Use attach mode for running processes

## Cross-References

- See [Chrome DevTools](./01-chrome-devtools.md) for browser debugging
- See [Testing Environment](../06-testing-environment/) for test debugging
- See [Development Tools](../12-dev-tools-integration/) for IDE setup
- See [Virtual Environments](../11-virtual-environments/) for environment setup

## Next Steps

Now that debugging is configured, let's set up package managers. Continue to [Package Manager Installation](../10-package-managers/).