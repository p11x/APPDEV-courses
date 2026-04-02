# Git Installation and Configuration

## What You'll Learn

- Installing Git on different operating systems
- Configuring Git for Node.js development
- Basic Git commands and workflows
- Best practices for Git usage

## Git Installation

### Windows Installation

```bash
# Download from git-scm.com
# https://git-scm.com/download/win

# Or use Chocolatey
choco install git -y

# Or use Scoop
scoop install git

# Verify installation
git --version
```

### macOS Installation

```bash
# Using Homebrew
brew install git

# Or install Xcode Command Line Tools
xcode-select --install

# Verify installation
git --version
```

### Linux Installation

```bash
# Ubuntu/Debian
sudo apt update
sudo apt install git -y

# Fedora/RHEL
sudo dnf install git -y

# Arch Linux
sudo pacman -S git

# Verify installation
git --version
```

## Git Configuration

### Initial Configuration

```bash
# Set your identity
git config --global user.name "Your Full Name"
git config --global user.email "your.email@example.com"

# Set default branch name
git config --global init.defaultBranch main

# Set default editor
git config --global core.editor "code --wait"  # VS Code
git config --global core.editor "vim"          # Vim
git config --global core.editor "nano"         # Nano

# Set line ending preferences
git config --global core.autocrlf input   # macOS/Linux
git config --global core.autocrlf true    # Windows

# Set color output
git config --global color.ui auto
```

### Advanced Configuration

```bash
# Set default merge strategy
git config --global merge.tool vscode
git config --global merge.tool.vscode.cmd "code --wait $MERGED"

# Set default push behavior
git config --global push.default current

# Set pull strategy
git config --global pull.rebase true

# Set credential helper
git config --global credential.helper manager   # Windows
git config --global credential.helper osxkeychain # macOS
git config --global credential.helper store     # Linux

# Set aliases
git config --global alias.st status
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.cm "commit -m"
git config --global alias.lg "log --oneline --graph --decorate --all"
git config --global alias.last "log -1 HEAD"
git config --global alias.unstage "reset HEAD --"
git config --global alias.visual "log --graph --oneline --all"
```

### Git Configuration File

```bash
# View configuration
git config --list --show-origin

# Edit configuration file directly
git config --global --edit

# Configuration locations
# System: /etc/gitconfig
# Global: ~/.gitconfig
# Local: .git/config
```

## Basic Git Commands

### Repository Commands

```bash
# Initialize new repository
git init

# Clone existing repository
git clone https://github.com/username/repo.git
git clone git@github.com:username/repo.git

# Clone with specific name
git clone https://github.com/username/repo.git my-project

# Clone specific branch
git clone -b develop https://github.com/username/repo.git
```

### Staging Commands

```bash
# Check status
git status

# Add files to staging
git add file.js          # Add specific file
git add .                # Add all changes
git add -A               # Add all changes including deletions
git add -p               # Interactive staging

# Remove from staging
git reset HEAD file.js   # Unstage file
git reset HEAD .         # Unstage all
```

### Commit Commands

```bash
# Commit staged changes
git commit -m "Commit message"

# Commit with detailed message
git commit

# Amend last commit
git commit --amend
git commit --amend -m "New message"

# Commit all tracked files
git commit -a -m "Commit message"
```

### Branch Commands

```bash
# List branches
git branch               # Local branches
git branch -a            # All branches
git branch -r            # Remote branches

# Create branch
git branch feature-name

# Switch branch
git checkout feature-name
git switch feature-name  # Modern alternative

# Create and switch
git checkout -b feature-name
git switch -c feature-name

# Delete branch
git branch -d feature-name    # Safe delete
git branch -D feature-name    # Force delete

# Rename branch
git branch -m old-name new-name
```

### Remote Commands

```bash
# List remotes
git remote -v

# Add remote
git remote add origin https://github.com/username/repo.git

# Change remote URL
git remote set-url origin https://github.com/username/repo.git

# Remove remote
git remote remove origin

# Fetch changes
git fetch origin

# Pull changes
git pull origin main
git pull --rebase origin main

# Push changes
git push origin main
git push -u origin main  # Set upstream
```

## SSH Key Configuration

### Generate SSH Key

```bash
# Generate new SSH key
ssh-keygen -t ed25519 -C "your.email@example.com"

# Or RSA if ed25519 not supported
ssh-keygen -t rsa -b 4096 -C "your.email@example.com"

# Start SSH agent
eval "$(ssh-agent -s)"

# Add key to SSH agent
ssh-add ~/.ssh/id_ed25519

# Copy public key to clipboard
# Windows
clip < ~/.ssh/id_ed25519.pub
# macOS
pbcopy < ~/.ssh/id_ed25519.pub
# Linux
cat ~/.ssh/id_ed25519.pub
```

### Add SSH Key to GitHub

```bash
# 1. Go to GitHub Settings → SSH and GPG keys
# 2. Click "New SSH key"
# 3. Paste your public key
# 4. Save

# Test connection
ssh -T git@github.com
```

## Git Ignore Configuration

### .gitignore File

```gitignore
# Dependencies
node_modules/
.pnp/
.pnp.js

# Build output
dist/
build/
*.tgz

# Coverage
coverage/
*.lcov

# Logs
logs/
*.log
npm-debug.log*

# Environment variables
.env
.env.local
.env.development.local
.env.test.local
.env.production.local

# OS files
.DS_Store
Thumbs.db

# IDE
.vscode/
.idea/
*.swp
*.swo

# Temporary files
tmp/
temp/
```

### Global Git Ignore

```bash
# Create global ignore file
touch ~/.gitignore_global

# Configure Git to use it
git config --global core.excludesfile ~/.gitignore_global
```

## Troubleshooting Common Issues

### Line Ending Issues

```bash
# Problem: LF will be replaced by CRLF
# Solution: Configure autocrlf

# Windows
git config --global core.autocrlf true

# macOS/Linux
git config --global core.autocrlf input

# Fix existing repository
git rm -rf --cached .
git reset --hard
```

### Permission Denied

```bash
# Problem: Permission denied (publickey)
# Solution: Use HTTPS or fix SSH

# Use HTTPS instead of SSH
git remote set-url origin https://github.com/username/repo.git

# Or fix SSH key
ssh-add ~/.ssh/id_ed25519
```

### Merge Conflicts

```bash
# Problem: Merge conflicts
# Solution: Resolve manually

# See conflicted files
git status

# Edit files to resolve conflicts
# Then add and commit
git add resolved-file.js
git commit
```

## Best Practices Checklist

- [ ] Install Git on your system
- [ ] Configure user name and email
- [ ] Set up SSH keys for GitHub
- [ ] Configure default branch as main
- [ ] Set up useful aliases
- [ ] Create .gitignore file
- [ ] Use conventional commit messages
- [ ] Keep commits small and focused
- [ ] Write descriptive commit messages
- [ ] Review changes before committing

## Performance Optimization Tips

- Use SSH for faster authentication
- Configure Git to use less memory
- Use shallow clones for large repositories
- Enable Git garbage collection
- Use Git LFS for large files
- Configure Git to cache credentials
- Use Git worktrees for multiple branches

## Cross-References

- See [Git Workflow](./02-git-workflow.md) for branching strategies
- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for pre-commit hooks
- See [Development Tools](../12-dev-tools-integration/) for Git integration
- See [Debugging Setup](../09-debugging-setup/) for Git debugging

## Next Steps

Now that Git is configured, let's learn Git workflow strategies. Continue to [Git Workflow Strategies](./02-git-workflow.md).