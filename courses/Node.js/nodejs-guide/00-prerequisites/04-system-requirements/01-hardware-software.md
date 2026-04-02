# System Requirements Analysis

## What You'll Learn

- Hardware specifications and recommendations for Node.js development
- Operating system compatibility (Windows, macOS, Linux)
- Browser requirements for development
- Network requirements for package management

## Hardware Specifications

### Minimum Requirements

| Component | Minimum | Recommended | Notes |
|-----------|---------|-------------|-------|
| CPU | Dual-core 2GHz | Quad-core 3GHz+ | Node.js benefits from multiple cores |
| RAM | 4GB | 8GB+ | 16GB for heavy IDE usage |
| Storage | 20GB free | 50GB+ SSD | SSD improves npm install speeds |
| Network | 10 Mbps | 50+ Mbps | Required for package downloads |

### CPU Considerations

```javascript
// Node.js is single-threaded by default
// Benefits from higher clock speeds over more cores

// For multi-threaded workloads, use:
// - Worker Threads (Node.js 10+)
// - Child Processes
// - Cluster module
```

### RAM Usage Patterns

```javascript
// Typical Node.js memory usage
const v8 = require('v8');

// Check heap statistics
const heapStats = v8.getHeapStatistics();
console.log('Heap size limit:', heapStats.heap_size_limit / 1024 / 1024, 'MB');
console.log('Total heap size:', heapStats.total_heap_size / 1024 / 1024, 'MB');

// Default heap limits by system RAM
// System RAM    | Default Heap Limit
// 4GB          | ~1.5GB
// 8GB          | ~2GB
// 16GB         | ~4GB
```

### Storage Requirements

```javascript
// Node.js installation: ~100MB
// npm global packages: ~500MB-2GB
// Project node_modules: ~50MB-2GB per project
// npm cache: ~1-5GB

// Check npm cache size
// npm cache ls --parseable | wc -l
// npm cache clean --force
```

## Operating System Compatibility

### Windows

**Supported Versions:**
- Windows 10 (64-bit) - Recommended
- Windows 11 (64-bit)
- Windows Server 2016+

**Considerations:**
```bash
# Path length limitations
# Windows has 260 character path limit by default
# Enable long paths in Windows 10+:
# Run as Administrator:
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem" /v LongPathsEnabled /t REG_DWORD /d 1 /f

# PowerShell execution policy
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Use Windows Terminal for best experience
# Install from Microsoft Store
```

**Common Issues:**
```bash
# Issue: npm install fails with EPERM
# Solution: Run terminal as Administrator or use:
npm config set cache C:\tmp\npm-cache

# Issue: Node modules path too long
# Solution: Use shorter project paths or enable long paths
```

### macOS

**Supported Versions:**
- macOS 11 (Big Sur) or later
- Apple Silicon (M1/M2/M3) natively supported

**Considerations:**
```bash
# Homebrew installation (recommended)
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Xcode Command Line Tools (required)
xcode-select --install

# Apple Silicon path differences
# Homebrew installs to /opt/homebrew on Apple Silicon
# vs /usr/local on Intel
```

**Common Issues:**
```bash
# Issue: Permission errors
# Solution: Don't use sudo with npm
# Fix npm permissions:
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH=~/.npm-global/bin:$PATH' >> ~/.zshrc

# Issue: Python not found (for native modules)
# Solution: Install Python via pyenv or Homebrew
brew install python
```

### Linux

**Supported Distributions:**
- Ubuntu 20.04 LTS+
- Debian 10+
- Fedora 34+
- CentOS/RHEL 8+
- Arch Linux

**Considerations:**
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install -y curl git build-essential

# Fedora/RHEL
sudo dnf install -y curl git gcc-c++ make

# Arch Linux
sudo pacman -S curl git base-devel

# Build tools for native modules
# Ubuntu/Debian: build-essential
# Fedora: gcc-c++ make
# Arch: base-devel
```

**Common Issues:**
```bash
# Issue: EACCES permission errors
# Solution: Fix npm permissions
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH=~/.npm-global/bin:$PATH' >> ~/.bashrc
source ~/.bashrc

# Issue: Node version conflicts
# Solution: Use NVM (Node Version Manager)
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash
```

## Browser Requirements

### Development Browsers

**Recommended:**
- Google Chrome (latest) - Best DevTools support
- Mozilla Firefox (latest) - Privacy-focused
- Microsoft Edge (Chromium) - Windows integration

**Development Tools:**
```javascript
// Chrome DevTools features for Node.js
// 1. Memory profiling
// 2. CPU profiling
// 3. Network inspection
// 4. Console debugging

// Enable Chrome DevTools for Node.js
// node --inspect app.js
// Open chrome://inspect
```

### Browser Extensions

```javascript
// Recommended extensions for Node.js development
// 1. React Developer Tools
// 2. Redux DevTools
// 3. Apollo Client DevTools
// 4. JSON Formatter
// 5. CORS Unblock (development only)
```

## Network Requirements

### Package Manager Network

```bash
# npm registry: registry.npmjs.org
# Requires HTTPS (port 443)

# Test connectivity
curl -I https://registry.npmjs.org

# Check npm registry
npm config get registry

# Use alternative registries if needed
npm config set registry https://registry.npmmirror.com  # China mirror
```

### Firewall Considerations

```bash
# Required ports
# 443: HTTPS (npm registry, GitHub)
# 80: HTTP (redirects to HTTPS)
# 22: SSH (git operations)

# Corporate proxy configuration
npm config set proxy http://proxy.company.com:8080
npm config set https-proxy http://proxy.company.com:8080

# Or use environment variables
export HTTP_PROXY=http://proxy.company.com:8080
export HTTPS_PROXY=http://proxy.company.com:8080
```

### Offline Development

```javascript
// npm offline setup
// 1. Create local registry with Verdaccio
npm install -g verdaccio
verdaccio

// 2. Configure npm to use local registry
npm set registry http://localhost:4873

// 3. Cache packages for offline use
npm cache add express
npm cache add lodash

// 4. Install from cache
npm install --prefer-offline
```

## Virtual Machine Requirements

### Docker Development

```dockerfile
# Dockerfile for Node.js development
FROM node:20-alpine

WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .

EXPOSE 3000
CMD ["npm", "start"]
```

```yaml
# docker-compose.yml
version: '3.8'
services:
  app:
    build: .
    ports:
      - "3000:3000"
    volumes:
      - .:/app
      - /app/node_modules
    environment:
      - NODE_ENV=development
```

### Resource Allocation

```yaml
# Docker Desktop resource recommendations
# CPU: 2-4 cores
# Memory: 4-8GB
# Swap: 1-2GB
# Disk: 60GB+
```

## Performance Benchmarks

### System Performance Test

```javascript
// benchmark.js
const { performance } = require('perf_hooks');

// CPU benchmark
function cpuBenchmark() {
    const start = performance.now();
    let result = 0;
    for (let i = 0; i < 10000000; i++) {
        result += Math.sqrt(i);
    }
    return performance.now() - start;
}

// Memory benchmark
function memoryBenchmark() {
    const start = performance.now();
    const arrays = [];
    for (let i = 0; i < 1000; i++) {
        arrays.push(new Array(10000).fill(Math.random()));
    }
    return performance.now() - start;
}

// I/O benchmark
async function ioBenchmark() {
    const fs = require('fs').promises;
    const start = performance.now();
    
    const writes = [];
    for (let i = 0; i < 100; i++) {
        writes.push(fs.writeFile(`test-${i}.txt`, 'data'.repeat(1000)));
    }
    await Promise.all(writes);
    
    // Cleanup
    for (let i = 0; i < 100; i++) {
        await fs.unlink(`test-${i}.txt`).catch(() => {});
    }
    
    return performance.now() - start;
}

// Run benchmarks
console.log('CPU:', cpuBenchmark().toFixed(2), 'ms');
console.log('Memory:', memoryBenchmark().toFixed(2), 'ms');
ioBenchmark().then(time => console.log('I/O:', time.toFixed(2), 'ms'));
```

## Troubleshooting Common Issues

### Insufficient Memory

```bash
# Problem: JavaScript heap out of memory
# Solution: Increase Node.js memory limit
node --max-old-space-size=4096 app.js

# Or set in package.json
# "scripts": {
#   "start": "node --max-old-space-size=4096 app.js"
# }
```

### Slow npm Install

```bash
# Problem: npm install takes too long
# Solutions:
# 1. Use npm ci instead of npm install
npm ci

# 2. Use pnpm (faster, disk efficient)
npm install -g pnpm
pnpm install

# 3. Clear npm cache
npm cache clean --force

# 4. Use local cache
npm install --prefer-offline
```

### Path Issues on Windows

```powershell
# Problem: 'node' is not recognized
# Solution: Add to PATH
# 1. Find Node.js installation path
where node

# 2. Add to system PATH
# Control Panel > System > Advanced > Environment Variables
# Add: C:\Program Files\nodejs\

# 3. Or use PowerShell
$env:PATH += ";C:\Program Files\nodejs\"
```

## Best Practices Checklist

- [ ] Verify system meets minimum requirements
- [ ] Use 64-bit operating system
- [ ] Install on SSD for better performance
- [ ] Ensure adequate RAM (8GB+ recommended)
- [ ] Configure firewall for npm registry access
- [ ] Set up proper permissions for npm
- [ ] Use version manager (NVM) for Node.js
- [ ] Configure proxy if behind corporate firewall
- [ ] Test network connectivity to npm registry
- [ ] Monitor system resources during development

## Performance Optimization Tips

- Use SSD for node_modules storage
- Increase npm cache size for faster reinstalls
- Use pnpm for disk-efficient package management
- Configure npm to use local cache
- Use Docker for consistent environments
- Allocate sufficient resources to VMs
- Monitor memory usage during development
- Use build tools with caching (Webpack, Vite)

## Cross-References

- See [Node.js Installation](../05-nodejs-installation/) for installation steps
- See [Package Managers](../10-package-managers/) for npm/yarn/pnpm setup
- See [Virtual Environments](../11-virtual-environments/) for project isolation
- See [Development Tools](../12-dev-tools-integration/) for IDE setup

## Next Steps

Now that you understand system requirements, let's install Node.js. Continue to [Node.js Installation Mastery](../05-nodejs-installation/).