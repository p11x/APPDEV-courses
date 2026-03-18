# Deployment Strategies

## What You'll Learn
- Manual deployment
- Automated deployment
- Environment promotion

## Prerequisites
- Completed automated testing

## Deploy to Production

```yaml
deploy:
  needs: test
  if: github.ref == 'refs/heads/main'
  runs-on: ubuntu-latest
  steps:
  - uses: actions/checkout@v3
  - name: Deploy to production
    run: |
      echo "Deploying to production..."
      # Add deployment commands
```

## Environment Promotion

```yaml
staging:
  needs: test
  if: github.event_name == 'pull_request'
  runs-on: ubuntu-latest
  environment: staging

production:
  needs: staging
  if: github.ref == 'refs/heads/main'
  runs-on: ubuntu-latest
  environment: production
```

## Summary
- Test → Staging → Production
- Use environments for protection
- Automate with approvals

## Next Steps
→ Continue to `05-secrets-management.md`
