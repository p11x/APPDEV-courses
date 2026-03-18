# Secrets Management

## What You'll Learn
- GitHub secrets
- Environment variables
- Vault

## Prerequisites
- Completed deployment strategies

## GitHub Secrets

1. Go to Repository → Settings → Secrets
2. Add secrets (API keys, tokens)

## Using Secrets

```yaml
jobs:
  deploy:
    steps:
    - name: Deploy
      env:
        API_KEY: ${{ secrets.API_KEY }}
        DB_PASSWORD: ${{ secrets.DB_PASSWORD }}
      run: |
        echo "Deploying with secrets..."
```

## Summary
- Never commit secrets
- Use GitHub secrets or Vault
- Access in CI/CD

## Next Steps
→ Move to `28-cloud-services/`
