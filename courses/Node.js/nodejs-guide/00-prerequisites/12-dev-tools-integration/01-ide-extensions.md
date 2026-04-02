# IDE Extensions and Tools for Node.js

## What You'll Learn

- Essential VS Code extensions for Node.js
- IDE configuration and customization
- Development workflow optimization
- Tool integration best practices

## VS Code Extensions

### Essential Extensions

```json
// .vscode/extensions.json
{
    "recommendations": [
        "dbaeumer.vscode-eslint",
        "esbenp.prettier-vscode",
        "eamodio.gitlens",
        "christian-kohler.path-intellisense",
        "bradlc.vscode-tailwindcss",
        "prisma.prisma",
        "ms-vscode.vscode-typescript-next",
        "formulahendry.auto-rename-tag",
        "ritwickdey.liveserver",
        "humao.rest-client"
    ]
}
```

### JavaScript/TypeScript Extensions

```bash
# Install these extensions
code --install-extension dbaeumer.vscode-eslint
code --install-extension esbenp.prettier-vscode
code --install-extension ms-vscode.vscode-typescript-next
code --install-extension christian-kohler.path-intellisense
```

### Git Extensions

```bash
# GitLens - Advanced Git integration
code --install-extension eamodio.gitlens

# Git Graph - Visual Git history
code --install-extension mhutchie.git-graph

# Git History - View Git log
code --install-extension donjayamanne.githistory
```

### Productivity Extensions

```bash
# Auto Rename Tag
code --install-extension formulahendry.auto-rename-tag

# Bracket Pair Colorizer
code --install-extension CoenraadS.bracket-pair-colorizer-2

# Todo Tree
code --install-extension Gruntfuggly.todo-tracker

# Error Lens
code --install-extension usernamehw.errorlens
```

## VS Code Settings

### Workspace Settings

```json
// .vscode/settings.json
{
    "editor.formatOnSave": true,
    "editor.defaultFormatter": "esbenp.prettier-vscode",
    "editor.codeActionsOnSave": {
        "source.fixAll.eslint": true
    },
    "editor.tabSize": 2,
    "editor.wordWrap": "wordWrapColumn",
    "editor.wordWrapColumn": 80,
    
    "files.associations": {
        "*.js": "javascript",
        "*.ts": "typescript",
        "*.jsx": "javascriptreact",
        "*.tsx": "typescriptreact"
    },
    
    "files.exclude": {
        "**/.git": true,
        "**/.DS_Store": true,
        "**/node_modules": true
    },
    
    "search.exclude": {
        "**/node_modules": true,
        "**/dist": true,
        "**/coverage": true
    }
}
```

### User Settings

```json
// User settings.json
{
    "editor.fontSize": 14,
    "editor.fontFamily": "Fira Code, Consolas, monospace",
    "editor.fontLigatures": true,
    "editor.cursorStyle": "line",
    "editor.cursorBlinking": "smooth",
    "editor.minimap.enabled": false,
    "editor.renderWhitespace": "boundary",
    
    "terminal.integrated.fontSize": 14,
    "terminal.integrated.cursorStyle": "line",
    
    "workbench.colorTheme": "One Dark Pro",
    "workbench.iconTheme": "material-icon-theme",
    
    "git.autofetch": true,
    "git.confirmSync": false,
    
    "eslint.validate": [
        "javascript",
        "javascriptreact",
        "typescript",
        "typescriptreact"
    ]
}
```

## Custom Snippets

### JavaScript Snippets

```json
// .vscode/javascript.json
{
    "Console Log": {
        "prefix": "clg",
        "body": [
            "console.log('$1');",
            "$2"
        ],
        "description": "Console log"
    },
    "Function": {
        "prefix": "fn",
        "body": [
            "function $1($2) {",
            "  $3",
            "}"
        ],
        "description": "Function declaration"
    },
    "Arrow Function": {
        "prefix": "af",
        "body": [
            "const $1 = ($2) => {",
            "  $3",
            "};"
        ],
        "description": "Arrow function"
    },
    "Async Function": {
        "prefix": "afn",
        "body": [
            "async function $1($2) {",
            "  try {",
            "    $3",
            "  } catch (error) {",
            "    console.error(error);",
            "  }",
            "}"
        ],
        "description": "Async function with error handling"
    }
}
```

### Express Snippets

```json
// .vscode/express.json
{
    "Express Route": {
        "prefix": "exproute",
        "body": [
            "app.$1('/$2', (req, res) => {",
            "  $3",
            "});"
        ],
        "description": "Express route handler"
    },
    "Express Middleware": {
        "prefix": "exmiddleware",
        "body": [
            "const $1 = (req, res, next) => {",
            "  $2",
            "  next();",
            "};"
        ],
        "description": "Express middleware"
    }
}
```

## Keyboard Shortcuts

### Essential Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+P | Quick Open |
| Ctrl+Shift+P | Command Palette |
| Ctrl+` | Toggle Terminal |
| Ctrl+B | Toggle Sidebar |
| Ctrl+Shift+E | Explorer |
| Ctrl+Shift+F | Search |
| Ctrl+Shift+G | Source Control |
| Ctrl+Shift+D | Debug |
| Ctrl+Shift+X | Extensions |
| Ctrl+, | Settings |

### Editing Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+D | Select Next Occurrence |
| Ctrl+Shift+L | Select All Occurrences |
| Alt+Up/Down | Move Line |
| Shift+Alt+Up/Down | Copy Line |
| Ctrl+Shift+K | Delete Line |
| Ctrl+Enter | Insert Line Below |
| Ctrl+Shift+Enter | Insert Line Above |
| Ctrl+/ | Toggle Comment |
| Ctrl+Shift+/ | Block Comment |
| Alt+Shift+F | Format Document |

## Terminal Integration

### Integrated Terminal

```bash
# Open terminal
Ctrl+`

# Split terminal
Ctrl+Shift+5

# New terminal
Ctrl+Shift+`

# Kill terminal
Ctrl+Shift+X
```

### Terminal Profiles

```json
// .vscode/settings.json
{
    "terminal.integrated.profiles.windows": [
        {
            "name": "PowerShell",
            "path": "C:\\WINDOWS\\System32\\WindowsPowerShell\\v1.0\\powershell.exe"
        },
        {
            "name": "Git Bash",
            "path": "C:\\Program Files\\Git\\bin\\bash.exe"
        },
        {
            "name": "WSL",
            "path": "C:\\WINDOWS\\System32\\wsl.exe"
        }
    ],
    "terminal.integrated.defaultProfile.windows": "Git Bash"
}
```

## Debugging Integration

### Launch Configurations

```json
// .vscode/launch.json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "node",
            "request": "launch",
            "name": "Launch Program",
            "program": "${workspaceFolder}/src/index.js",
            "console": "integratedTerminal"
        },
        {
            "type": "node",
            "request": "attach",
            "name": "Attach to Process",
            "port": 9229
        }
    ]
}
```

## Task Automation

### Tasks Configuration

```json
// .vscode/tasks.json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "npm: start",
            "type": "npm",
            "script": "start",
            "group": "build"
        },
        {
            "label": "npm: test",
            "type": "npm",
            "script": "test",
            "group": "test"
        },
        {
            "label": "npm: lint",
            "type": "npm",
            "script": "lint",
            "problemMatcher": ["$eslint-stylish"]
        }
    ]
}
```

## Troubleshooting Common Issues

### Extensions Not Working

```bash
# Problem: Extensions not loading
# Solution: Reload window

# Command Palette
Ctrl+Shift+P
> Reload Window

# Or disable/enable extension
```

### Performance Issues

```bash
# Problem: VS Code slow
# Solution: Disable extensions

# Disable unused extensions
# Increase memory limit
# Use workspace settings instead of user settings
```

### IntelliSense Not Working

```bash
# Problem: No autocomplete
# Solution: Check TypeScript configuration

# Ensure tsconfig.json exists
# Restart TypeScript server
Ctrl+Shift+P
> TypeScript: Restart TS Server
```

## Best Practices Checklist

- [ ] Install essential extensions
- [ ] Configure workspace settings
- [ ] Create custom snippets
- [ ] Learn keyboard shortcuts
- [ ] Set up debugging configurations
- [ ] Configure task automation
- [ ] Use integrated terminal
- [ ] Enable format on save
- [ ] Configure Git integration
- [ ] Document team extensions

## Performance Optimization Tips

- Disable unused extensions
- Use workspace settings
- Exclude large folders from search
- Disable minimap for large files
- Use file nesting in explorer
- Configure search exclusions
- Use TypeScript project references

## Cross-References

- See [Linting Rules](./02-linting-formatting.md) for code quality
- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for ESLint/Prettier
- See [Debugging Setup](../09-debugging-setup/) for debugging configuration
- See [Development Workflow](./02-linting-formatting.md) for automation

## Next Steps

Now that IDE extensions are configured, let's set up linting and formatting. Continue to [Linting and Formatting Setup](./02-linting-formatting.md).