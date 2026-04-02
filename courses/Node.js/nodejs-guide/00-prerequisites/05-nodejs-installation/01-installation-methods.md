# Node.js Installation Methods

## What You'll Learn

- Different installation methods for Node.js
- Official installer vs package managers
- Binary installation and compilation
- Verification and testing installations

## Installation Methods Overview

### Method 1: Official Installer (Recommended for Beginners)

```bash
# Download from nodejs.org
# https://nodejs.org/en/download/

# Windows: .msi installer
# macOS: .pkg installer
# Linux: .tar.xz binary or source

# Pros:
# - Simple, graphical installer
# - Includes npm
# - Automatic PATH configuration

# Cons:
# - Manual updates
# - Single version only
# - Requires admin rights
```

### Method 2: Package Manager (Recommended for Most Users)

```bash
# Windows - Chocolatey
choco install nodejs

# Windows - Scoop
scoop install nodejs

# macOS - Homebrew
brew install node

# Ubuntu/Debian
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs

# Fedora/RHEL
sudo dnf module install nodejs:20/common

# Arch Linux
sudo pacman -S nodejs npm
```

### Method 3: Node Version Manager (Recommended for Developers)

```bash
# NVM (Node Version Manager) - Most popular
# Windows: nvm-windows
# macOS/Linux: nvm

# Pros:
# - Multiple Node.js versions
# - Easy version switching
# - No admin rights needed
# - Per-project version control

# Cons:
# - Additional setup required
# - Shell configuration needed
```

### Method 4: Binary Download

```bash
# Download pre-built binary
wget https://nodejs.org/dist/v20.10.0/node-v20.10.0-linux-x64.tar.xz

# Extract
tar -xf node-v20.10.0-linux-x64.tar.xz

# Add to PATH
export PATH=$PATH:/path/to/node-v20.10.0-linux-x64/bin

# Verify
node --version
npm --version
```

### Method 5: Build from Source

```bash
# Clone repository
git clone https://github.com/nodejs/node.git
cd node

# Checkout stable version
git checkout v20.10.0

# Configure and build
./configure
make -j4

# Install
sudo make install

# Verify
node --version
```

## Windows Installation

### Using Official Installer

```powershell
# 1. Download from nodejs.org
# 2. Run .msi installer
# 3. Select options:
#    - Node.js runtime
#    - npm package manager
#    - Add to PATH
#    - Install Chocolatey (optional)

# 4. Verify installation
node --version
npm --version
```

### Using Chocolatey

```powershell
# Install Chocolatey (if not installed)
Set-ExecutionPolicy Bypass -Scope Process -Force
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))

# Install Node.js
choco install nodejs-lts -y

# Verify
node --version
npm --version
```

### Using Scoop

```powershell
# Install Scoop (if not installed)
iex "& {$(irm get.scoop.sh)} -RunAsAdmin"

# Install Node.js
scoop install nodejs

# Verify
node --version
npm --version
```

## macOS Installation

### Using Homebrew

```bash
# Install Homebrew (if not installed)
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Install Node.js
brew install node

# Verify
node --version
npm --version

# Update Node.js
brew upgrade node
```

### Using Official Installer

```bash
# 1. Download .pkg from nodejs.org
# 2. Run installer
# 3. Follow prompts

# Verify
node --version
npm --version
```

## Linux Installation

### Ubuntu/Debian

```bash
# Method 1: NodeSource repository (recommended)
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs

# Method 2: Ubuntu repository (older version)
sudo apt update
sudo apt install -y nodejs npm

# Verify
node --version
npm --version
```

### Fedora/RHEL

```bash
# Method 1: Module installation
sudo dnf module install nodejs:20/common

# Method 2: NodeSource repository
curl -fsSL https://rpm.nodesource.com/setup_20.x | sudo bash -
sudo dnf install -y nodejs

# Verify
node --version
npm --version
```

### Arch Linux

```bash
# Install from official repositories
sudo pacman -S nodejs npm

# Verify
node --version
npm --version
```

## Post-Installation Setup

### Verify Installation

```bash
# Check Node.js version
node --version
# v20.10.0

# Check npm version
npm --version
# 10.2.3

# Check installation path
which node
# /usr/local/bin/node

# Check Node.js is working
node -e "console.log('Node.js is working!')"

# Check npm is working
npm --help
```

### Configure npm

```bash
# Set default npm configuration
npm config set init-author-name "Your Name"
npm config set init-author-email "your.email@example.com"
npm config set init-license "MIT"
npm config set init-version "1.0.0"

# View configuration
npm config list

# View all configuration
npm config list -l

# Edit configuration file
npm config edit
```

### Test Installation

```javascript
// test-node.js
const os = require('os');

console.log('Node.js Installation Test');
console.log('========================');
console.log('Node.js version:', process.version);
console.log('npm version:', require('child_process').execSync('npm --version').toString().trim());
console.log('Platform:', os.platform());
console.log('Architecture:', os.arch());
console.log('CPU cores:', os.cpus().length);
console.log('Total memory:', Math.round(os.totalmem() / 1024 / 1024), 'MB');
console.log('Free memory:', Math.round(os.freemem() / 1024 / 1024), 'MB');

// Test async/await
async function testAsync() {
    return 'Async/await works!';
}

testAsync().then(console.log);

// Test ES modules
console.log('ES6 features:', {
    arrow: () => 'arrow functions',
    destructuring: { a: 1, b: 2 },
    spread: [...[1, 2, 3]],
    template: `Template literals`
});
```

```bash
# Run test
node test-node.js
```

## Troubleshooting Common Issues

### Windows Issues

```powershell
# Problem: 'node' is not recognized
# Solution: Add to PATH manually
# 1. Find Node.js installation path
where node

# 2. Add to system PATH
# Control Panel > System > Advanced > Environment Variables
# Add: C:\Program Files\nodejs\

# Problem: npm permission errors
# Solution: Run as Administrator or fix permissions
npm config set prefix "C:\Users\YourName\AppData\Roaming\npm"
```

### macOS Issues

```bash
# Problem: Permission errors
# Solution: Fix npm permissions
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH=~/.npm-global/bin:$PATH' >> ~/.zshrc
source ~/.zshrc

# Problem: Xcode command line tools missing
# Solution: Install them
xcode-select --install
```

### Linux Issues

```bash
# Problem: EACCES permission errors
# Solution: Fix npm permissions
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH=~/.npm-global/bin:$PATH' >> ~/.bashrc
source ~/.bashrc

# Problem: Old Node.js version in repositories
# Solution: Use NodeSource repository
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs
```

## Best Practices Checklist

- [ ] Use version manager (NVM) for development
- [ ] Install LTS version for production
- [ ] Verify installation after setup
- [ ] Configure npm defaults
- [ ] Test Node.js and npm functionality
- [ ] Document installation method used
- [ ] Keep Node.js updated
- [ ] Use appropriate version for project
- [ ] Configure npm permissions correctly
- [ ] Test installation with sample code

## Performance Optimization Tips

- Use LTS versions for stability
- Use latest versions for performance improvements
- Configure npm cache for faster installs
- Use pnpm for disk-efficient package management
- Keep Node.js updated for security patches
- Use appropriate Node.js version for workload
- Monitor Node.js memory usage
- Use production optimizations in production

## Cross-References

- See [NVM Setup](./02-nvm-version-management.md) for version management
- See [Package Managers](../10-package-managers/) for npm/yarn/pnpm
- See [Virtual Environments](../11-virtual-environments/) for project isolation
- See [System Requirements](../04-system-requirements/) for hardware specs

## Next Steps

Now that Node.js is installed, let's set up version management. Continue to [NVM Setup and Version Management](./02-nvm-version-management.md).