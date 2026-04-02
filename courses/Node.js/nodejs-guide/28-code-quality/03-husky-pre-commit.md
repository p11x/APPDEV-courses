# Husky Pre-Commit

## What You'll Learn

- Setting up Husky for Git hooks
- Running checks before commits
- Configuring lint-staged

## Setup

```bash
npm install -D husky lint-staged
npx husky init
echo "npx lint-staged" > .husky/pre-commit
```

## lint-staged Configuration

```json
{
  "lint-staged": {
    "*.{js,mjs}": ["eslint --fix", "prettier --write"],
    "*.{json,md,yml}": ["prettier --write"]
  }
}
```

## Next Steps

For code review, continue to [Code Review Checklist](./04-code-review-checklist.md).
