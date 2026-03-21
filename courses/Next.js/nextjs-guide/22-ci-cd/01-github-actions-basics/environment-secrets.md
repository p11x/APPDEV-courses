# Environment Secrets

## What You'll Learn
- Store secrets securely
- Use in CI/CD
- Best practices

## Prerequisites
- GitHub Actions workflow

## Do I Need This Right Now?
Secrets like API keys shouldn't be in code. Learn to use them safely.

## Adding Secrets

1. Go to GitHub → Settings → Secrets and variables → Actions
2. Click "New repository secret"
3. Add name and value

## Using Secrets

```yaml
steps:
  - name: Run with secrets
    env:
      DATABASE_URL: ${{ secrets.DATABASE_URL }}
      API_KEY: ${{ secrets.API_KEY }}
    run: npm run build
```

## Best Practices

- Never commit secrets to code
- Use different secrets for dev/prod
- Rotate secrets regularly

## Summary
- Add secrets in GitHub Settings
- Access with ${{ secrets.NAME }}
- Never commit secrets to code
