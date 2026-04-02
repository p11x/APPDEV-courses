# Prettier Formatting Rules and Setup

## What You'll Learn

- Installing and configuring Prettier
- Setting up formatting rules
- Integrating Prettier with ESLint
- Husky for git hooks integration

## Prettier Installation

### Basic Installation

```bash
# Install Prettier
npm install --save-dev prettier

# Or with specific version
npm install --save-dev prettier@3
```

## Prettier Configuration

### Basic Configuration

Create `.prettierrc` in your project root:

```json
{
    "semi": true,
    "singleQuote": true,
    "tabWidth": 2,
    "useTabs": false,
    "trailingComma": "none",
    "bracketSpacing": true,
    "arrowParens": "avoid",
    "printWidth": 80,
    "endOfLine": "lf"
}
```

### Advanced Configuration

```json
{
    "semi": true,
    "singleQuote": true,
    "tabWidth": 2,
    "useTabs": false,
    "trailingComma": "es5",
    "bracketSpacing": true,
    "arrowParens": "always",
    "printWidth": 100,
    "endOfLine": "lf",
    "overrides": [
        {
            "files": "*.json",
            "options": {
                "tabWidth": 2
            }
        },
        {
            "files": "*.md",
            "options": {
                "proseWrap": "always"
            }
        }
    ]
}
```

### JavaScript Configuration File

```javascript
// prettier.config.js
module.exports = {
    semi: true,
    singleQuote: true,
    tabWidth: 2,
    useTabs: false,
    trailingComma: 'none',
    bracketSpacing: true,
    arrowParens: 'avoid',
    printWidth: 80,
    endOfLine: 'lf',
    overrides: [
        {
            files: '*.json',
            options: { tabWidth: 2 }
        }
    ]
};
```

## Prettier Rules Explained

### Formatting Rules

```json
{
    "semi": true,                    // Add semicolons
    "singleQuote": true,             // Use single quotes
    "tabWidth": 2,                   // 2 spaces indentation
    "useTabs": false,                // Use spaces, not tabs
    "trailingComma": "none",         // No trailing commas
    "bracketSpacing": true,          // { foo: bar } not {foo: bar}
    "arrowParens": "avoid",          // x => x not (x) => x
    "printWidth": 80,                // Max line length
    "endOfLine": "lf"                // Unix line endings
}
```

### Trailing Comma Options

```json
{
    "trailingComma": "none",   // No trailing commas
    "trailingComma": "es5",    // Trailing commas where valid in ES5
    "trailingComma": "all"     // Trailing commas everywhere
}
```

## Integration with ESLint

### ESLint + Prettier Configuration

```bash
# Install ESLint Prettier plugin
npm install --save-dev eslint-config-prettier eslint-plugin-prettier
```

```json
// .eslintrc.json
{
    "extends": [
        "eslint:recommended",
        "plugin:prettier/recommended"
    ],
    "plugins": ["prettier"],
    "rules": {
        "prettier/prettier": "error"
    }
}
```

### Combined Configuration

```javascript
// .eslintrc.js
module.exports = {
    env: {
        node: true,
        es2021: true
    },
    extends: [
        'eslint:recommended',
        'prettier' // Must be last to override other configs
    ],
    plugins: ['prettier'],
    rules: {
        'prettier/prettier': 'error'
    }
};
```

## Husky for Git Hooks

### Installation

```bash
# Install Husky
npm install --save-dev husky

# Initialize Husky
npx husky install

# Add to package.json scripts
npm set-script prepare "husky install"
```

### Pre-commit Hook

```bash
# Create pre-commit hook
npx husky add .husky/pre-commit "npm run lint && npm run format:check"

# Or with lint-staged
npx husky add .husky/pre-commit "npx lint-staged"
```

### lint-staged Configuration

```bash
# Install lint-staged
npm install --save-dev lint-staged
```

```json
// package.json
{
    "lint-staged": {
        "*.js": [
            "eslint --fix",
            "prettier --write"
        ],
        "*.json": [
            "prettier --write"
        ],
        "*.md": [
            "prettier --write"
        ]
    }
}
```

## Pre-commit Checks

### Complete Pre-commit Setup

```json
// package.json
{
    "scripts": {
        "lint": "eslint .",
        "lint:fix": "eslint . --fix",
        "format": "prettier --write .",
        "format:check": "prettier --check .",
        "prepare": "husky install"
    },
    "lint-staged": {
        "*.js": [
            "eslint --fix",
            "prettier --write",
            "git add"
        ],
        "*.{json,md}": [
            "prettier --write",
            "git add"
        ]
    }
}
```

### Git Hook Script

```bash
#!/bin/sh
# .husky/pre-commit

# Run linting
npm run lint
if [ $? -ne 0 ]; then
    echo "Linting failed. Please fix errors before committing."
    exit 1
fi

# Check formatting
npm run format:check
if [ $? -ne 0 ]; then
    echo "Formatting check failed. Run 'npm run format' to fix."
    exit 1
fi

# Run tests
npm test
if [ $? -ne 0 ]; then
    echo "Tests failed. Please fix before committing."
    exit 1
fi
```

## npm Scripts Integration

### package.json Scripts

```json
{
    "scripts": {
        "lint": "eslint .",
        "lint:fix": "eslint . --fix",
        "format": "prettier --write .",
        "format:check": "prettier --check .",
        "check": "npm run lint && npm run format:check",
        "fix": "npm run lint:fix && npm run format"
    }
}
```

## Ignoring Files

### .prettierignore

```
# Dependencies
node_modules/

# Build output
dist/
build/

# Coverage
coverage/

# Logs
*.log

# OS files
.DS_Store
Thumbs.db

# Package files
package-lock.json
yarn.lock
pnpm-lock.yaml

# Config files
.env
.env.local
```

## VS Code Integration

### VS Code Settings

```json
// .vscode/settings.json
{
    "editor.defaultFormatter": "esbenp.prettier-vscode",
    "editor.formatOnSave": true,
    "editor.formatOnPaste": true,
    "[javascript]": {
        "editor.defaultFormatter": "esbenp.prettier-vscode"
    },
    "[json]": {
        "editor.defaultFormatter": "esbenp.prettier-vscode"
    },
    "[markdown]": {
        "editor.defaultFormatter": "esbenp.prettier-vscode"
    }
}
```

## Troubleshooting Common Issues

### Conflicts with ESLint

```bash
# Problem: ESLint and Prettier conflicts
# Solution: Use eslint-config-prettier

npm install --save-dev eslint-config-prettier

# In .eslintrc.json
{
    "extends": ["eslint:recommended", "prettier"]
}
```

### Formatting Not Working

```bash
# Problem: Prettier not formatting
# Solution: Check configuration

# Check if .prettierrc exists
ls -la .prettierrc

# Run Prettier manually
npx prettier --write src/

# Check VS Code settings
# Ensure defaultFormatter is set
```

### Husky Not Running

```bash
# Problem: Git hooks not executing
# Solution: Reinstall Husky

# Remove existing hooks
rm -rf .husky

# Reinstall
npx husky install

# Add hook again
npx husky add .husky/pre-commit "npm run lint"
```

## Best Practices Checklist

- [ ] Install Prettier as dev dependency
- [ ] Create .prettierrc configuration
- [ ] Set up ESLint integration
- [ ] Configure Husky for git hooks
- [ ] Add lint-staged for pre-commit checks
- [ ] Create .prettierignore file
- [ ] Set up VS Code integration
- [ ] Add npm scripts for formatting
- [ ] Configure CI/CD to check formatting
- [ ] Document formatting rules

## Performance Optimization Tips

- Use `--cache` flag for faster runs
- Format only changed files in CI
- Use lint-staged to process staged files
- Configure VS Code to format on save
- Use Prettier plugin for ESLint
- Run formatting in parallel with linting

## Cross-References

- See [ESLint Setup](./01-eslint-setup.md) for linting configuration
- See [Git Workflow](../08-git-workflow/) for git hooks
- See [Testing Environment](../06-testing-environment/) for test integration
- See [Development Tools](../12-dev-tools-integration/) for IDE setup

## Next Steps

Now that code quality tools are configured, let's set up Git workflow. Continue to [Git Workflow Foundations](../08-git-workflow/).