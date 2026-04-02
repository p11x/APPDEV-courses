# Browser and Network Requirements

## What You'll Learn

- Browser requirements for Node.js development
- Network configuration for package management
- Proxy and firewall setup
- Offline development strategies

## Browser Requirements

### Chrome DevTools for Node.js

```bash
# Enable debugging in Node.js
node --inspect app.js
# Opens ws://127.0.0.1:9229

# Or with break on first line
node --inspect-brk app.js

# Access in Chrome
chrome://inspect
```

### Browser Developer Tools

```javascript
// Essential browser features for Node.js developers
// 1. Console for testing JavaScript
// 2. Network tab for API debugging
// 3. Memory profiler for leak detection
// 4. CPU profiler for performance

// Example: Test API in browser console
fetch('http://localhost:3000/api/users')
    .then(res => res.json())
    .then(data => console.table(data));
```

### Browser Extensions

```javascript
// Recommended for Node.js development
const extensions = [
    'React Developer Tools',
    'Redux DevTools',
    'Apollo Client DevTools',
    'JSON Formatter',
    'ModHeader (modify headers)',
    'Allow CORS (development only)'
];
```

## Network Configuration

### npm Registry Configuration

```bash
# Default registry
npm config get registry
# https://registry.npmjs.org/

# Use alternative registries
npm config set registry https://registry.npmmirror.com  # China
npm config set registry https://registry.yarnpkg.com    # Yarn

# Scoped registries
npm config set @mycompany:registry https://npm.mycompany.com

# Reset to default
npm config delete registry
```

### Proxy Configuration

```bash
# Corporate proxy setup
npm config set proxy http://username:password@proxy.company.com:8080
npm config set https-proxy http://username:password@proxy.company.com:8080

# Without authentication
npm config set proxy http://proxy.company.com:8080
npm config set https-proxy http://proxy.company.com:8080

# Environment variables (alternative)
export HTTP_PROXY=http://proxy.company.com:8080
export HTTPS_PROXY=http://proxy.company.com:8080
export NO_PROXY=localhost,127.0.0.1,.company.com

# PowerShell
$env:HTTP_PROXY = "http://proxy.company.com:8080"
$env:HTTPS_PROXY = "http://proxy.company.com:8080"
```

### SSL/TLS Configuration

```bash
# Disable SSL verification (NOT recommended for production)
npm config set strict-ssl false

# Use custom CA certificate
npm config set cafile /path/to/ca-bundle.crt

# Set certificate for specific registry
npm config set //registry.npmjs.org/:_certfile /path/to/cert.pem
npm config set //registry.npmjs.org/:_keyfile /path/to/key.pem
```

## Firewall Configuration

### Required Outbound Ports

```bash
# Port 443 (HTTPS) - Required
# npm registry, GitHub, GitLab, etc.

# Port 80 (HTTP) - Redirects to HTTPS
# Some legacy packages

# Port 22 (SSH) - For git operations
# git@github.com:username/repo.git

# Test connectivity
curl -v https://registry.npmjs.org
ssh -T git@github.com
```

### Windows Firewall

```powershell
# Allow Node.js through firewall
New-NetFirewallRule -DisplayName "Node.js" -Direction Outbound -Program "C:\Program Files\nodejs\node.exe" -Action Allow

# Allow npm
New-NetFirewallRule -DisplayName "npm" -Direction Outbound -Program "C:\Program Files\nodejs\npm.cmd" -Action Allow
```

### Linux Firewall (ufw)

```bash
# Allow outgoing for Node.js
sudo ufw allow out 443/tcp
sudo ufw allow out 80/tcp
sudo ufw allow out 22/tcp

# Check status
sudo ufw status
```

## Offline Development

### Local Registry with Verdaccio

```bash
# Install Verdaccio globally
npm install -g verdaccio

# Start local registry
verdaccio

# Configure npm to use local registry
npm set registry http://localhost:4873

# Publish packages locally
npm publish --registry http://localhost:4873

# Install from local registry
npm install express --registry http://localhost:4873
```

### npm Cache Management

```bash
# Check cache location
npm config get cache

# List cached packages
npm cache ls

# Verify cache integrity
npm cache verify

# Clean cache
npm cache clean --force

# Add packages to cache
npm cache add express@4.18.0
npm cache add lodash@4.17.21

# Install from cache only
npm install --cache-min 9999999 --prefer-offline
```

### Docker Offline Setup

```dockerfile
# Create offline-ready Docker image
FROM node:20-alpine

WORKDIR /app

# Copy package files
COPY package*.json ./

# Install with cache
RUN npm install --prefer-offline

# Copy application
COPY . .

CMD ["npm", "start"]
```

```yaml
# docker-compose.yml for offline development
version: '3.8'
services:
  app:
    build: .
    environment:
      - NODE_ENV=development
    volumes:
      - .:/app
      - npm-cache:/root/.npm
    ports:
      - "3000:3000"

volumes:
  npm-cache:
```

## Network Troubleshooting

### Connection Issues

```bash
# Problem: ECONNREFUSED
# Solution: Check if registry is accessible
curl -v https://registry.npmjs.org

# Problem: ETIMEDOUT
# Solution: Check proxy settings
npm config list | grep proxy

# Problem: CERT_UNTRUSTED
# Solution: Update certificates or use strict-ssl=false (temporary)
npm config set strict-ssl false
npm install
npm config set strict-ssl true
```

### DNS Issues

```bash
# Problem: DNS resolution fails
# Solution: Use IP address or alternative DNS

# Check DNS
nslookup registry.npmjs.org

# Use Google DNS
# Windows: Set DNS to 8.8.8.8 and 8.8.4.4
# Linux: Edit /etc/resolv.conf
nameserver 8.8.8.8
nameserver 8.8.4.4
```

### Speed Optimization

```bash
# Use faster mirror
npm config set registry https://registry.npmmirror.com

# Increase network timeout
npm config set fetch-retries 5
npm config set fetch-retry-factor 10
npm config set fetch-retry-mintimeout 10000
npm config set fetch-retry-maxtimeout 60000

# Use pnpm for faster installs
npm install -g pnpm
pnpm install
```

## Performance Benchmarks

### Network Speed Test

```javascript
// network-test.js
const https = require('https');
const { performance } = require('perf_hooks');

function testDownloadSpeed(url) {
    return new Promise((resolve) => {
        const start = performance.now();
        let bytes = 0;

        https.get(url, (res) => {
            res.on('data', (chunk) => {
                bytes += chunk.length;
            });
            res.on('end', () => {
                const duration = (performance.now() - start) / 1000;
                const speed = (bytes / 1024 / 1024) / duration;
                resolve({ bytes, duration, speed: speed.toFixed(2) + ' MB/s' });
            });
        });
    });
}

// Test npm registry speed
testDownloadSpeed('https://registry.npmjs.org/express')
    .then(result => console.log('Download speed:', result.speed));
```

### npm Install Benchmark

```bash
# Benchmark npm install time
time npm install

# Compare package managers
time npm install      # npm
time yarn install     # yarn
time pnpm install     # pnpm

# Clean install benchmark
rm -rf node_modules package-lock.json
time npm install
```

## Best Practices Checklist

- [ ] Configure npm registry for your region
- [ ] Set up proxy if behind corporate firewall
- [ ] Use HTTPS for all registry connections
- [ ] Configure SSL certificates if required
- [ ] Set up offline development environment
- [ ] Cache packages for faster reinstalls
- [ ] Monitor network performance
- [ ] Use faster package managers (pnpm)
- [ ] Configure firewall rules properly
- [ ] Test connectivity to npm registry

## Performance Optimization Tips

- Use local npm registry mirror
- Configure npm cache properly
- Use pnpm for disk-efficient installs
- Enable npm parallel installs
- Use --prefer-offline flag
- Increase npm network timeouts
- Use CDN for package downloads
- Monitor and optimize network latency

## Cross-References

- See [Package Managers](../10-package-managers/) for npm/yarn/pnpm setup
- See [Virtual Environments](../11-virtual-environments/) for project isolation
- See [Development Tools](../12-dev-tools-integration/) for IDE configuration
- See [Node.js Installation](../05-nodejs-installation/) for installation steps

## Next Steps

Now that you understand network requirements, let's install Node.js. Continue to [Node.js Installation Mastery](../05-nodejs-installation/).