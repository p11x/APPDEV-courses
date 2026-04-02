# ESLint Configuration

## What You'll Learn

- Advanced ESLint configuration for Node.js
- Custom rules for Node.js projects
- ESLint plugins for common frameworks

## Full Configuration

```bash
npm install -D eslint @eslint/js eslint-plugin-security eslint-plugin-import
```

```js
// eslint.config.js
import js from '@eslint/js';
import security from 'eslint-plugin-security';
import importPlugin from 'eslint-plugin-import';

export default [
  js.configs.recommended,
  security.configs.recommended,
  {
    plugins: { import: importPlugin },
    rules: {
      'no-unused-vars': ['error', { argsIgnorePattern: '^_' }],
      'no-var': 'error',
      'prefer-const': 'error',
      'prefer-template': 'error',
      'import/order': ['error', { groups: ['builtin', 'external', 'parent', 'sibling'] }],
      'security/detect-unsafe-regex': 'error',
      'security/detect-eval-with-expression': 'error',
    },
  },
];
```

## Next Steps

For formatting, continue to [Prettier Formatting](./02-prettier-formatting.md).
