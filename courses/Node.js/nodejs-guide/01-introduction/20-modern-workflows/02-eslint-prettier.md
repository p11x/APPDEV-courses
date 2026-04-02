# ESLint and Prettier Integration

## What You'll Learn

- Setting up ESLint for Node.js
- Configuring Prettier for consistent formatting
- Integrating with editors and CI/CD
- Custom rule configuration

## ESLint 9+ Setup (Flat Config)

```bash
npm install -D eslint @eslint/js typescript-eslint
```

```javascript
// eslint.config.js — ESLint flat config

import js from '@eslint/js';
import ts from 'typescript-eslint';

export default [
    js.configs.recommended,
    ...ts.configs.recommended,
    {
        rules: {
            'no-unused-vars': 'off',
            '@typescript-eslint/no-unused-vars': ['error', { argsIgnorePattern: '^_' }],
            '@typescript-eslint/explicit-function-return-type': 'warn',
            '@typescript-eslint/no-explicit-any': 'warn',
            'no-console': ['warn', { allow: ['warn', 'error'] }],
            'prefer-const': 'error',
            'no-var': 'error',
        },
    },
    {
        ignores: ['dist/', 'node_modules/', 'coverage/'],
    },
];
```

## Prettier Setup

```bash
npm install -D prettier eslint-config-prettier
```

```json
// .prettierrc
{
    "semi": true,
    "trailingComma": "all",
    "singleQuote": true,
    "printWidth": 100,
    "tabWidth": 4,
    "arrowParens": "always"
}
```

```javascript
// Add to eslint.config.js
import prettier from 'eslint-config-prettier';

export default [
    js.configs.recommended,
    ...ts.configs.recommended,
    prettier, // Disables ESLint rules that conflict with Prettier
    { rules: { /* your rules */ } },
];
```

## npm Scripts

```json
{
    "scripts": {
        "lint": "eslint src/",
        "lint:fix": "eslint src/ --fix",
        "format": "prettier --write 'src/**/*.{ts,js,json}'",
        "format:check": "prettier --check 'src/**/*.{ts,js,json}'"
    }
}
```

## Best Practices Checklist

- [ ] Use ESLint flat config (ESLint 9+)
- [ ] Use eslint-config-prettier to avoid conflicts
- [ ] Run lint in CI/CD pipeline
- [ ] Use editor integration for real-time feedback
- [ ] Configure pre-commit hooks with husky + lint-staged

## Cross-References

- See [TypeScript Integration](./01-typescript-integration.md) for TypeScript setup
- See [Testing Frameworks](./03-testing-frameworks.md) for test configuration
- See [Code Quality](../../../28-code-quality/) for advanced quality tools

## Next Steps

Continue to [Testing Frameworks](./03-testing-frameworks.md) for test setup.
