# Terminal Customization and Git Configuration

## What You'll Learn

- Customizing your terminal for Node.js development
- Setting up useful aliases and functions
- Installing and configuring Git
- Code editor settings for JavaScript/TypeScript

## Terminal Customization

### Windows Terminal Setup

Install Windows Terminal from the Microsoft Store or use PowerShell.

#### PowerShell Profile Configuration

Create or edit your PowerShell profile:

```powershell
# Find your profile path
$PROFILE

# Create profile if it doesn't exist
if (!(Test-Path -Path $PROFILE)) {
    New-Item -ItemType File -Path $PROFILE -Force
}

# Edit profile
notepad $PROFILE
```

Add these to your PowerShell profile:

```powershell
# Custom prompt with Git branch
function prompt {
    $gitBranch = git branch --show-current 2>$null
    if ($gitBranch) {
        "PS [$env:USERNAME@$env:COMPUTERNAME] [$(Get-Location)] (git:$gitBranch)`n> "
    } else {
        "PS [$env:USERNAME@$env:COMPUTERNAME] [$(Get-Location)]`n> "
    }
}

# Useful aliases
Set-Alias -Name npminit -Value "npm init -y"
Set-Alias -Name dev -Value "npm run dev"
Set-Alias -Name test -Value "npm test"
Set-Alias -Name build -Value "npm run build"

# Functions for common Node.js tasks
function node-version { node --version }
function npm-version { npm --version }
function npx-run { npx $args }

# Environment variables
$env:NODE_ENV = "development"
$env:PATH += ";$env:APPDATA\npm"
```

### Git Bash Configuration

For Git Bash on Windows, edit `~/.bashrc`:

```bash
# Custom aliases
alias npminit='npm init -y'
alias dev='npm run dev'
alias test='npm test'
alias build='npm run build'
alias gs='git status'
alias ga='git add'
alias gc='git commit -m'
alias gp='git push'
alias gl='git log --oneline -10'

# Node.js functions
function node-version() { node --version; }
function npm-version() { npm --version; }
function nvm-use() { nvm use $1; }

# Custom prompt with Node version
export PS1='\[\033[01;32m\]\u@\h\[\033[00m\]:\[\033[01;34m\]\w\[\033[00m\] (node:$(node -v)) \$ '

# Environment variables
export NODE_ENV=development
export PATH="$PATH:$HOME/.npm-global/bin"
```

## Git Installation and Configuration

### Windows Git Installation

1. Download Git from [git-scm.com](https://git-scm.com/)
2. During installation, select:
   - Use Git from the command line and also from 3rd-party software
   - Checkout Windows-style, commit Unix-style line endings
   - Use Windows' default console window

### Initial Git Configuration

After installation, configure Git:

```bash
# Set your identity
git config --global user.name "Your Full Name"
git config --global user.email "your.email@example.com"

# Set default branch name
git config --global init.defaultBranch main

# Set default editor (VS Code)
git config --global core.editor "code --wait"

# Set line ending preferences
git config --global core.autocrlf input  # macOS/Linux
git config --global core.autocrlf true   # Windows

# Set default merge strategy
git config --global merge.tool vscode
git config --global merge.tool.vscode.cmd "code --wait $MERGED"

# Color output
git config --global color.ui auto

# Useful aliases
git config --global alias.st status
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.cm "commit -m"
git config --global alias.lg "log --oneline --graph --decorate --all"
git config --global alias.last "log -1 HEAD"
git config --global alias.unstage "reset HEAD --"

# Set credential helper
git config --global credential.helper manager  # Windows
git config --global credential.helper osxkeychain  # macOS
```

### SSH Key Generation for Git

Generate SSH keys for GitHub/GitLab:

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

Add the public key to your GitHub account at Settings → SSH and GPG keys.

## Code Editor Settings for JavaScript/TypeScript

### VS Code Language-Specific Settings

Create `.vscode/settings.json` in your project:

```json
{
  "[javascript]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode",
    "editor.formatOnSave": true,
    "editor.tabSize": 2,
    "editor.wordWrap": "wordWrapColumn",
    "editor.wordWrapColumn": 80,
    "files.trimTrailingWhitespace": true,
    "files.insertFinalNewline": true
  },
  "[typescript]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode",
    "editor.formatOnSave": true,
    "editor.tabSize": 2,
    "editor.wordWrap": "wordWrapColumn",
    "editor.wordWrapColumn": 80,
    "files.trimTrailingWhitespace": true,
    "files.insertFinalNewline": true
  },
  "[json]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode",
    "editor.formatOnSave": true,
    "editor.tabSize": 2
  },
  "[markdown]": {
    "editor.wordWrap": "wordWrapColumn",
    "editor.wordWrapColumn": 80,
    "files.trimTrailingWhitespace": false
  }
}
```

### TypeScript Configuration

Create `tsconfig.json` for TypeScript projects:

```json
{
  "compilerOptions": {
    "target": "ES2022",
    "module": "ESNext",
    "moduleResolution": "node",
    "lib": ["ES2022"],
    "outDir": "./dist",
    "rootDir": "./src",
    "strict": true,
    "esModuleInterop": true,
    "skipLibCheck": true,
    "forceConsistentCasingInFileNames": true,
    "resolveJsonModule": true,
    "declaration": true,
    "declarationMap": true,
    "sourceMap": true,
    "noUnusedLocals": true,
    "noUnusedParameters": true,
    "noImplicitReturns": true,
    "noFallthroughCasesInSwitch": true
  },
  "include": ["src/**/*"],
  "exclude": ["node_modules", "dist", "**/*.test.ts"]
}
```

## Troubleshooting Common Issues

### PowerShell Execution Policy Error

```powershell
# Problem: Running scripts is disabled
# Solution: Change execution policy
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### Git Authentication Issues

```bash
# Problem: HTTPS credential issues
# Solution: Use credential manager or SSH
git config --global credential.helper manager

# Or switch to SSH
git remote set-url origin git@github.com:username/repo.git
```

### Terminal Not Recognizing Node/NPM

```bash
# Problem: 'node' is not recognized
# Solution: Add to PATH
# Windows
set PATH=%PATH%;C:\Program Files\nodejs

# Or add permanently through System Properties → Environment Variables
```

## Best Practices Checklist

- [ ] Install and configure a modern terminal (Windows Terminal, iTerm2)
- [ ] Set up shell profile with useful aliases
- [ ] Configure Git with your identity and preferences
- [ ] Generate and configure SSH keys
- [ ] Set up VS Code with language-specific settings
- [ ] Create TypeScript configuration for TypeScript projects
- [ ] Configure Git line ending preferences for your OS
- [ ] Set up Git aliases for common commands
- [ ] Configure editor to format on save
- [ ] Test terminal and Git configuration

## Performance Optimization Tips

- Use a fast terminal emulator (Windows Terminal, iTerm2, Alacritty)
- Disable unnecessary shell plugins
- Cache Git credentials to avoid repeated authentication
- Use Git sparse checkout for large repositories
- Configure VS Code to exclude large folders from watching
- Use a modern font with ligatures for better readability

## Cross-References

- See [VS Code Configuration](./01-vscode-configuration.md) for detailed editor setup
- See [Git Workflow Foundations](../08-git-workflow/) for Git best practices
- See [Development Tools Integration](../12-dev-tools-integration/) for additional tooling
- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for linting setup

## Next Steps

Now that your development environment is configured, let's review JavaScript fundamentals. Continue to [JavaScript Fundamentals Refresher](../02-javascript-fundamentals/).