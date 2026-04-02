# NVM Setup and Version Management

## What You'll Learn

- Installing and configuring NVM
- Managing multiple Node.js versions
- Switching versions per project
- Best practices for version management

## What is NVM?

Node Version Manager (NVM) allows you to:
- Install multiple Node.js versions
- Switch between versions easily
- Use different versions per project
- Test against multiple Node.js versions

## Installing NVM

### Windows (nvm-windows)

```powershell
# Download nvm-windows from:
# https://github.com/coreybutler/nvm-windows/releases

# Or use Chocolatey
choco install nvm -y

# Or use Scoop
scoop install nvm

# Verify installation
nvm version
```

### macOS/Linux

```bash
# Install using curl
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash

# Or using wget
wget -qO- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash

# Reload shell configuration
source ~/.bashrc  # Linux
source ~/.zshrc   # macOS

# Verify installation
nvm --version
```

### Manual Installation

```bash
# Clone repository
git clone https://github.com/nvm-sh/nvm.git ~/.nvm

# Add to shell configuration
echo 'export NVM_DIR="$HOME/.nvm"' >> ~/.bashrc
echo '[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"' >> ~/.bashrc
echo '[ -s "$NVM_DIR/bash_completion" ] && \. "$NVM_DIR/bash_completion"' >> ~/.bashrc

# Reload
source ~/.bashrc
```

## Basic NVM Commands

### Installing Node.js Versions

```bash
# List available versions
nvm ls-remote

# List LTS versions only
nvm ls-remote --lts

# Install latest LTS version
nvm install --lts

# Install specific version
nvm install 20.10.0

# Install latest version of major release
nvm install 20

# Install and set as default
nvm install 20 --default
```

### Managing Installed Versions

```bash
# List installed versions
nvm ls

# Use specific version
nvm use 20.10.0

# Set default version
nvm alias default 20.10.0

# Use default version
nvm use default

# Run command with specific version
nvm run 18.19.0 --version

# Execute script with specific version
nvm exec 18.19.0 node app.js
```

### Uninstalling Versions

```bash
# Uninstall specific version
nvm uninstall 18.19.0

# Uninstall all versions of major release
nvm uninstall 18

# Reinstall current version
nvm reinstall-packages 20.10.0
```

## Advanced NVM Usage

### .nvmrc File

Create `.nvmrc` in project root to specify Node.js version:

```bash
# .nvmrc
20.10.0

# Or use LTS alias
lts/hydrogen
```

```bash
# Use version from .nvmrc
nvm use

# Install version from .nvmrc
nvm install

# Automatically use version when entering directory
# Add to ~/.bashrc or ~/.zshrc
autoload -U add-zsh-hook
load-nvmrc() {
    if [[ -f .nvmrc ]]; then
        nvm use
    fi
}
add-zsh-hook chpwd load-nvmrc
```

### Project-Specific Configuration

```json
// package.json
{
    "engines": {
        "node": ">=20.0.0",
        "npm": ">=10.0.0"
    }
}
```

```bash
# Enforce engine requirements
npm config set engine-strict true

# Or in .npmrc
engine-strict=true
```

### Version Aliases

```bash
# Create custom aliases
nvm alias production 20.10.0
nvm alias development 21.3.0

# Use aliases
nvm use production
nvm use development

# List aliases
nvm alias

# Delete alias
nvm alias delete production
```

## Using Multiple Versions

### Parallel Development

```bash
# Terminal 1: Project A (Node 18)
cd project-a
nvm use 18
npm start

# Terminal 2: Project B (Node 20)
cd project-b
nvm use 20
npm start

# Terminal 3: Testing (Node 21)
cd test-project
nvm use 21
npm test
```

### Testing Against Multiple Versions

```bash
# test-all-versions.sh
#!/bin/bash

versions=("18.19.0" "20.10.0" "21.3.0")

for version in "${versions[@]}"; do
    echo "Testing with Node.js $version"
    nvm use "$version"
    npm test
    if [ $? -ne 0 ]; then
        echo "Tests failed with Node.js $version"
        exit 1
    fi
done

echo "All tests passed!"
```

### CI/CD Integration

```yaml
# .github/workflows/test.yml
name: Test

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        node-version: [18.x, 20.x, 21.x]
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Use Node.js ${{ matrix.node-version }}
        uses: actions/setup-node@v3
        with:
          node-version: ${{ matrix.node-version }}
      
      - run: npm ci
      - run: npm test
```

## NVM Configuration

### Shell Integration

```bash
# ~/.bashrc or ~/.zshrc

export NVM_DIR="$HOME/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"
[ -s "$NVM_DIR/bash_completion" ] && \. "$NVM_DIR/bash_completion"

# Auto-switch version when entering directory
autoload -U add-zsh-hook
load-nvmrc() {
    local node_version="$(nvm version)"
    local nvmrc_path="$(nvm_find_nvmrc)"

    if [ -n "$nvmrc_path" ]; then
        local nvmrc_node_version=$(nvm version "$(cat "${nvmrc_path}")")

        if [ "$nvmrc_node_version" = "N/A" ]; then
            nvm install
        elif [ "$nvmrc_node_version" != "$node_version" ]; then
            nvm use
        fi
    elif [ "$node_version" != "$(nvm version default)" ]; then
        echo "Reverting to nvm default version"
        nvm use default
    fi
}
add-zsh-hook chpwd load-nvmrc
load-nvmrc
```

### npm Configuration per Version

```bash
# Configure npm for specific Node version
nvm use 20
npm config set registry https://registry.npmmirror.com

nvm use 18
npm config set registry https://registry.npmjs.org
```

## Troubleshooting Common Issues

### NVM Not Found

```bash
# Problem: nvm: command not found
# Solution: Reload shell configuration

# Bash
source ~/.bashrc

# Zsh
source ~/.zshrc

# Or restart terminal
```

### Version Not Switching

```bash
# Problem: node version doesn't change
# Solution: Check if node is installed via other means

# Check which node is being used
which node

# If it shows /usr/local/bin/node (not nvm path)
# Remove system node installation
sudo apt remove nodejs  # Ubuntu/Debian
brew uninstall node     # macOS
```

### Permission Errors

```bash
# Problem: EACCES errors after nvm use
# Solution: Don't use sudo with nvm

# Wrong
sudo nvm use 20

# Right
nvm use 20

# Fix npm permissions if needed
npm config set prefix '~/.npm-global'
```

### Slow nvm Commands

```bash
# Problem: nvm commands are slow
# Solution: Disable automatic version checking

# Add to .nvmrc in home directory
export NVM_LAZY_LOAD=true

# Or use zsh-nvm plugin for lazy loading
# https://github.com/lukechilds/zsh-nvm
```

## Alternative Version Managers

### fnm (Fast Node Manager)

```bash
# Install fnm
curl -fsSL https://fnm.vercel.app/install | bash

# Install Node.js
fnm install 20
fnm use 20

# Faster than nvm
```

### volta

```bash
# Install volta
curl https://get.volta.sh | bash

# Install Node.js
volta install node@20

# Automatic version switching
```

### nodenv

```bash
# Install nodenv
git clone https://github.com/nodenv/nodenv.git ~/.nodenv

# Add to PATH
echo 'export PATH="$HOME/.nodenv/bin:$PATH"' >> ~/.bashrc
echo 'eval "$(nodenv init -)"' >> ~/.bashrc

# Install Node.js
nodenv install 20.10.0
nodenv global 20.10.0
```

## Best Practices Checklist

- [ ] Install NVM for version management
- [ ] Use .nvmrc files for projects
- [ ] Set default Node.js version
- [ ] Use LTS versions for production
- [ ] Test against multiple Node.js versions
- [ ] Configure auto-switching in shell
- [ ] Keep NVM updated
- [ ] Use appropriate version for each project
- [ ] Document Node.js version requirements
- [ ] Integrate version checking in CI/CD

## Performance Optimization Tips

- Use fnm or volta for faster version switching
- Enable lazy loading for NVM
- Cache Node.js installations
- Use .nvmrc for automatic switching
- Consider using Docker for consistent environments
- Use project-specific npm configurations
- Test with multiple versions in parallel
- Monitor Node.js version compatibility

## Cross-References

- See [Installation Methods](./01-installation-methods.md) for Node.js installation
- See [Virtual Environments](../11-virtual-environments/) for project isolation
- See [Package Managers](../10-package-managers/) for npm/yarn/pnpm
- See [Testing Environment](../06-testing-environment/) for testing setup

## Next Steps

Now that you can manage Node.js versions, let's set up a testing environment. Continue to [Testing Environment Configuration](../06-testing-environment/).