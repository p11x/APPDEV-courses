# VS Code Configuration for Node.js Development

## What You'll Learn

- Installing and configuring VS Code for optimal Node.js development
- Essential extensions for JavaScript/TypeScript development
- Customizing settings for better productivity
- Debugging configuration in VS Code

## Why VS Code for Node.js?

VS Code is the most popular editor for Node.js development due to:
- Excellent JavaScript/TypeScript support out of the box
- Rich extension ecosystem
- Integrated terminal and debugging
- Lightweight yet powerful
- Free and open-source

## Installation

Download VS Code from [code.visualstudio.com](https://code.visualstudio.com/) and install it following the platform-specific instructions.

## Essential Extensions

Install these extensions via the VS Code Marketplace (Ctrl+Shift+X):

### 1. JavaScript and TypeScript Nightly
- Provides latest TypeScript language features
- Better IntelliSense and type checking

### 2. ESLint
- Integrates ESLint into VS Code
- Shows linting errors and warnings in real-time

### 3. Prettier - Code formatter
- Automatic code formatting on save
- Consistent code style across team

### 4. GitLens — Git supercharged
- Enhanced Git capabilities
- Blame annotations, commit search, etc.

### 5. Debugger for Chrome
- Debug Node.js applications with Chrome DevTools
- Set breakpoints, inspect variables, etc.

### 6. Import Cost
- Shows size of imported packages
- Helps identify large dependencies

### 7. npm Intellisense
- Autocomplete for npm modules in import statements
- Saves time typing package names

### 8. Path Intellisense
- Autocomplete for file paths
- Reduces errors in require/import statements

## Recommended Settings

Add these settings to your `settings.json` (Ctrl+, → Open Settings (JSON)):

```json
{
  // Editor settings
  "editor.fontSize": 14,
  "editor.tabSize": 2,
  "editor.insertSpaces": true,
  "editor.detectIndentation": false,
  "editor.formatOnSave": true,
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": true
  },
  "editor.linkedEditing": true,
  
  // Files settings
  "files.associations": {
    "*.js": "javascript",
    "*.ts": "typescript"
  },
  "files.exclude": {
    "**/.git": true,
    "**/.DS_Store": true,
    "**/node_modules": true
  },
  
  // JavaScript/TypeScript settings
  "javascript.updateImportsOnFileMove.enabled": "always",
  "typescript.updateImportsOnFileMove.enabled": "always",
  "javascript.preferences.quoteStyle": "single",
  "typescript.preferences.quoteStyle": "single",
  
  // Terminal settings
  "terminal.integrated.fontSize": 14,
  "terminal.integrated.cursorStyle": "line",
  
  // Git settings
  "git.autofetch": true,
  "git.confirmSync": false,
  
  // ESLint settings
  "eslint.validate": [
    "javascript",
    "javascriptreact",
    "typescript",
    "typescriptreact"
  ],
  "eslint.alwaysShowStatus": true,
  
  // Prettier settings
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "[javascript]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  },
  "[typescript]": {
    "editor.defaultFormatter": "esbenp.prettier-vscode"
  }
}
```

## Workspace Settings

Create a `.vscode` folder in your project root with these files:

### `settings.json` (workspace-specific)
```json
{
  // Override user settings for this workspace
  "typescript.tsdk": "node_modules/typescript/lib",
  "eslint.nodePath": "node_modules/eslint",
  "eslint.packageManager": "npm",
  "cSpell.enabledLanguageIds": [
    "javascript",
    "javascriptreact",
    "typescript",
    "typescriptreact"
  ]
}
```

### `extensions.json` (recommended extensions)
```json
{
  "recommendations": [
    "dbaeumer.vscode-eslint",
    "esbenp.prettier-vscode",
    "ms-vscode.vscode-typescript-tslint-plugin",
    "eamodio.gitlens",
    "christian-kohler.path-intellisense",
    "eg2.tslint",
    "humao.rest-client",
    "ritwickdey.liveserver"
  ]
}
```

### `launch.json` (debugging configuration)
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "node",
      "request": "launch",
      "name": "Launch Program",
      "skipIfAlreadyAvailable": true,
      "program": "${workspaceFolder}/src/index.js",
      "preLaunchTask": "npm: build",
      "outFiles": [
        "${workspaceFolder}/dist/**/*.js"
      ],
      "sourceMaps": true
    },
    {
      "type": "node",
      "request": "attach",
      "name": "Attach to Process",
      "port": 9229,
      "restart": true
    },
    {
      "type": "node",
      "request": "launch",
      "name": "Mocha Tests",
      "program": "${workspaceFolder}/node_modules/mocha/bin/_mocha",
      "args": [
        "-u",
        "tdd",
        "--timeout",
        "999999",
        "--colors",
        "${workspaceFolder}/test/**/*.test.js"
      ],
      "internalConsoleOptions": "openOnSessionStart"
    }
  ]
}
```

### `tasks.json` (common tasks)
```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "npm: install",
      "type": "npm",
      "script": "install",
      "group": "build"
    },
    {
      "label": "npm: build",
      "type": "npm",
      "script": "build",
      "group": "build",
      "problemMatcher": [
        "$tsc"
      ]
    },
    {
      "label": "npm: test",
      "type": "npm",
      "script": "test",
      "group": "test",
      "problemMatcher": []
    },
    {
      "label": "npm: start",
      "type": "npm",
      "script": "start",
      "group": "none",
      "problemMatcher": []
    }
  ]
}
```

## Keyboard Shortcuts to Learn

Customize these in `keybindings.json` if desired:

| Shortcut | Action |
|----------|--------|
| Ctrl+P | Quick Open, Go to File |
| Ctrl+Shift+P | Command Palette |
| Ctrl+` | Toggle Integrated Terminal |
| Ctrl+Shift+D | Open Debug View |
| Ctrl+Shift+E | Open Explorer |
| Ctrl+Shift+F | Search in Files |
| Alt+Shift+F | Format Document |
| Ctrl+K Ctrl+F | Format Selection |
| Ctrl+D | Select Next Occurrence |
| Ctrl+K Ctrl+C | Add Line Comment |
| Ctrl+K Ctrl+U | Remove Line Comment |
| Shift+Alt+Down | Copy Line Down |
| Shift+Alt+Up | Copy Line Up |
| Ctrl+Enter | Insert Line Below |
| Ctrl+Shift+Enter | Insert Line Above |

## Snippets for Productivity

Create custom snippets in `~/AppData/Roaming/Code/User/snippets/javascript.json`:

```json
{
  "Log to Console": {
    "prefix": "log",
    "body": [
      "console.log('$1');",
      "$2"
    ],
    "description": "Log output to console"
  },
  "Error to Console": {
    "prefix": "err",
    "body": [
      "console.error('$1');",
      "$2"
    ],
    "description": "Log error to console"
  },
  "Try/Catch Block": {
    "prefix": "try",
    "body": [
      "try {",
      "  $1",
      "} catch (error) {",
      "  console.error(error);",
      "  $2"
      "}"
    ],
    "description": "Try/catch error handling"
  },
  "Async Function": {
    "prefix": "afunc",
    "body": [
      "async function $1($2) {",
      "  try {",
      "    $3",
      "  } catch (error) {",
      "    console.error(error);",
      "    $4",
      "  }",
      "}"
    ],
    "description": "Async function with error handling"
  }
}
```

## Troubleshooting Common Issues

### Issue: Extensions not working after update
**Solution:** 
1. Reload VS Code (Ctrl+Shift+P → "Reload Window")
2. Disable and re-enale the problematic extension
3. Check the Output panel for extension-specific errors

### Issue: IntelliSense not working for Node.js modules
**Solution:**
1. Ensure you have `@types/node` installed: `npm install --save-dev @types/node`
2. Check that `js/ts.implicitProjectConfig.checkJs` is true in settings
3. Restart the TypeScript server (Ctrl+Shift+P → "TypeScript: Restart TS server")

### Issue: Debugger not hitting breakpoints
**Solution:**
1. Make sure source maps are enabled (`"sourceMaps": true` in launch config)
2. Verify you're debugging the correct process
3. Check that "Skip uninteresting frames" is not enabled in debug settings
4. Ensure your code is compiled to the location specified in `outFiles`

### Issue: ESLint not showing errors
**Solution:**
1. Verify ESLint is installed locally in your project: `npm list eslint`
2. Check that `eslint.validate` includes your file types in settings
3. Look for ESLint output in the Output panel (select ESLint from dropdown)
4. Ensure you have a valid `.eslintrc` configuration file

## Best Practices Checklist

- [ ] Install essential extensions listed above
- [ ] Configure user settings for consistent formatting
- [ ] Set up workspace-specific settings for team projects
- [ ] Create debugging configurations for your application
- [ ] Define npm scripts as tasks for easy execution
- [ ] Learn and use keyboard shortcuts for efficiency
- [ ] Create custom snippets for repetitive code patterns
- [ ] Regularly update extensions for latest features
- [ ] Use built-in terminal for command execution
- [ ] Enable auto-save and format-on-save for clean code

## Performance Optimization Tips

- Disable extensions you don't use regularly
- Use workspace settings instead of modifying user settings for project-specific config
- Enable `files.watcherExclude` to prevent watching large folders like `node_modules`
- Use `search.exclude` to prevent searching in unnecessary directories
- Consider using `:memory` mode for large workspaces if experiencing slowdowns
- Regularly clear extension caches if performance degrades over time

## Cross-References

- For terminal customization, see [Terminal Configuration](./02-terminal-git-configuration.md)
- For Git setup, see [Git Configuration](./02-terminal-git-configuration.md)
- For editor settings specific to JavaScript/TypeScript, refer to the [JavaScript Essentials](../01-javascript-essentials.md) chapter
- For linting and formatting setup, see [Code Quality Toolchain](../07-code-quality-toolchain-setup/)