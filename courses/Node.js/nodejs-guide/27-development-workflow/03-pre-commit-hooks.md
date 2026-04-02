# Pre-Commit Hooks

## What You'll Learn

- Setting up pre-commit hooks with Husky
- Running lint and format checks before commits
- Preventing bad code from being committed

## Setup

```bash
npm install -D husky lint-staged
npx husky init
```

## Configuration

```json
// package.json
{
  "lint-staged": {
    "*.js": ["eslint --fix", "prettier --write"],
    "*.md": ["prettier --write"],
    "*.json": ["prettier --write"]
  }
}
```

```bash
# .husky/pre-commit
npx lint-staged
```

## Next Steps

Continue to [ESLint & Prettier Config](./04-eslint-prettier-config.md).
