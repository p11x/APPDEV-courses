# Node.js Setup Across Platforms

## What You'll Learn

- Installing Node.js on Windows, macOS, and Linux
- Verifying installation with terminal commands
- Troubleshooting common installation issues
- Setting up a productive development environment

## Installation Methods

### Method 1: Official Installer (Simplest)

Download the LTS installer from [nodejs.org](https://nodejs.org). This includes both `node` and `npm`.

**Windows:**
```bash
# After installation, verify in PowerShell or CMD
node --version
# v22.14.0

npm --version
# 10.9.2
```

**macOS:**
```bash
# After downloading .pkg and installing
node --version
npm --version
```

**Linux (Debian/Ubuntu):**
```bash
# Using NodeSource repository (recommended for latest versions)
curl -fsSL https://deb.nodesource.com/setup_22.x | sudo -E bash -
sudo apt-get install -y nodejs

# Verify
node --version
npm --version
```

**Linux (RHEL/Fedora):**
```bash
curl -fsSL https://rpm.nodesource.com/setup_22.x | sudo bash -
sudo dnf install -y nodejs
```

### Method 2: nvm (Node Version Manager) — Recommended for Developers

nvm lets you install and switch between multiple Node.js versions.

**macOS/Linux:**
```bash
# Install nvm
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.1/install.sh | bash

# Reload shell configuration
source ~/.bashrc   # or source ~/.zshrc

# Install latest LTS
nvm install --lts

# Verify
node --version
```

**Windows (nvm-windows):**
```bash
# 1. Uninstall existing Node.js first (Settings > Apps)
# 2. Download nvm-setup.exe from https://github.com/coreybutler/nvm/releases
# 3. Run installer

# Then in a new terminal:
nvm install lts
nvm use lts
node --version
```

### Method 3: Package Managers

**macOS (Homebrew):**
```bash
brew install node
node --version
```

**Linux (Snap):**
```bash
sudo snap install node --classic
```

## Verification Commands

Run these after installation to confirm everything works:

```bash
# Node.js runtime
node --version          # Should print v22.x.x or v20.x.x

# npm package manager
npm --version           # Should print 10.x.x or 9.x.x

# Node.js REPL (interactive mode)
node -e "console.log('Node.js is working!')"

# Check npm configuration
npm config list

# Check where Node.js is installed
which node              # macOS/Linux
where node              # Windows

# Check npm global directory
npm root -g
```

## Platform-Specific Setup

### Windows Configuration

```powershell
# Enable PowerShell script execution (if needed)
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Set npm global path (optional, avoids permission issues)
npm config set prefix "C:\Users\$env:USERNAME\AppData\Roaming\npm"

# Install Windows Build Tools (for native modules)
npm install -g windows-build-tools
```

### macOS Configuration

```bash
# Install Xcode Command Line Tools (required for native modules)
xcode-select --install

# Configure npm global directory (avoid sudo)
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH="$HOME/.npm-global/bin:$PATH"' >> ~/.zshrc
source ~/.zshrc
```

### Linux Configuration

```bash
# Install build essentials (required for native modules)
sudo apt-get install -y build-essential    # Debian/Ubuntu
sudo dnf groupinstall "Development Tools"  # RHEL/Fedora

# Configure npm global directory
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH="$HOME/.npm-global/bin:$PATH"' >> ~/.bashrc
source ~/.bashrc
```

## Troubleshooting

### Issue: `node` command not found

```bash
# Check if Node.js is installed
ls /usr/local/bin/node        # macOS
ls /usr/bin/node               # Linux

# If using nvm, ensure it's loaded
source ~/.nvm/nvm.sh

# Add to PATH manually (Linux/macOS)
export PATH="/usr/local/bin:$PATH"
```

### Issue: Permission errors with npm

```bash
# BAD: Never use sudo with npm
sudo npm install -g package   # Don't do this

# GOOD: Fix npm permissions
mkdir ~/.npm-global
npm config set prefix '~/.npm-global'
echo 'export PATH="$HOME/.npm-global/bin:$PATH"' >> ~/.bashrc
source ~/.bashrc
```

### Issue: Native module compilation fails

```bash
# Windows: Install build tools
npm install -g windows-build-tools

# macOS: Install Xcode CLI tools
xcode-select --install

# Linux: Install build essentials
sudo apt-get install -y build-essential python3
```

### Issue: Version conflicts between projects

```bash
# Use nvm to switch versions per project
nvm use 20       # Switch to Node.js 20

# Or create .nvmrc in project root
echo "20" > .nvmrc
nvm use          # Automatically uses version from .nvmrc
```

## Best Practices Checklist

- [ ] Use nvm for version management (not global installer)
- [ ] Always use LTS versions for production projects
- [ ] Never use `sudo` with npm
- [ ] Configure npm global directory to user home
- [ ] Install build tools for native module support
- [ ] Create `.nvmrc` in projects to lock Node.js version
- [ ] Keep Node.js updated (check LTS schedule)

## Next Steps

Now that Node.js is installed, let's create your first program. Continue to [First Hello World](./02-first-hello-world.md).
