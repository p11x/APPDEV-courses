# Environment Secrets

## What You'll Learn

- How to use GitHub Secrets for sensitive values
- How to use GitHub Variables for non-sensitive config
- How to create environment-specific secrets (staging vs production)
- How to use protection rules for production deployments
- How OIDC tokens work for cloud authentication

## Secrets vs Variables

| Type | Visibility | Use Case |
|------|-----------|----------|
| **Secrets** | Encrypted, hidden | API keys, passwords, SSH keys |
| **Variables** | Plain text, visible | Feature flags, non-sensitive config |

## Repository Secrets

Set secrets at: **GitHub repo → Settings → Secrets and variables → Actions**

```yaml
# Using secrets in workflows
env:
  DATABASE_URL: ${{ secrets.DATABASE_URL }}
  API_KEY: ${{ secrets.API_KEY }}
  DEPLOY_TOKEN: ${{ secrets.DEPLOY_TOKEN }}
```

Secrets are:
- Encrypted at rest
- Never shown in logs (masked automatically)
- Not passed to workflows from forks

## Environment Secrets

Environments let you have different secrets for staging vs production:

```
GitHub repo → Settings → Environments → New environment
  ├── staging
  │   ├── DATABASE_URL: postgresql://staging-db...
  │   └── API_KEY: staging-key-123
  └── production
      ├── DATABASE_URL: postgresql://prod-db...
      ├── API_KEY: prod-key-456
      └── Protection rules: Require approval from 1 reviewer
```

```yaml
# .github/workflows/deploy.yml

jobs:
  deploy-staging:
    runs-on: ubuntu-latest
    environment: staging              # Uses staging secrets
    steps:
      - name: Deploy to staging
        run: ./deploy.sh
        env:
          DATABASE_URL: ${{ secrets.DATABASE_URL }}  # Staging value
          API_KEY: ${{ secrets.API_KEY }}

  deploy-production:
    runs-on: ubuntu-latest
    needs: deploy-staging
    environment:
      name: production
      url: https://myapp.com         # Link shown in GitHub UI
    steps:
      - name: Deploy to production
        run: ./deploy.sh
        env:
          DATABASE_URL: ${{ secrets.DATABASE_URL }}  # Production value
          API_KEY: ${{ secrets.API_KEY }}
```

## Protection Rules

Production environments can require:

- **Required reviewers**: Someone must manually approve before deployment
- **Wait timer**: Wait N minutes before deploying
- **Branch restrictions**: Only deploy from specific branches

```
Environment: production
├── Required reviewers: @team-leads
├── Wait timer: 5 minutes
└── Deployment branches: main only
```

## GitHub Variables

Non-sensitive configuration goes in **Variables** (visible in logs):

```yaml
# .github/workflows/ci.yml

jobs:
  test:
    runs-on: ubuntu-latest
    env:
      NODE_ENV: ${{ vars.NODE_ENV || 'test' }}
      API_BASE_URL: ${{ vars.API_BASE_URL }}
      FEATURE_FLAG_NEW_UI: ${{ vars.FEATURE_FLAG_NEW_UI }}
    steps:
      - run: npm test
```

Set variables at: **GitHub repo → Settings → Secrets and variables → Actions → Variables tab**

## OIDC (OpenID Connect)

For cloud deployments (AWS, GCP, Azure), OIDC eliminates long-lived secrets:

```yaml
# .github/workflows/deploy-aws.yml

name: Deploy to AWS

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest

    # Required for OIDC
    permissions:
      id-token: write
      contents: read

    steps:
      - uses: actions/checkout@v4

      # Get AWS credentials via OIDC (no long-lived secrets!)
      - name: Configure AWS credentials
        uses: aws-actions/configure-aws-credentials@v4
        with:
          role-to-assume: arn:aws:iam::123456789:role/github-actions
          aws-region: us-east-1

      # Now you can use AWS CLI
      - name: Deploy to ECS
        run: |
          aws ecs update-service \
            --cluster myapp \
            --service myapp \
            --force-new-deployment
```

### How OIDC Works

```
GitHub Actions
  │
  │ 1. Request OIDC token from GitHub
  │
  ▼
AWS (trusts GitHub's OIDC provider)
  │
  │ 2. Verify token, assume IAM role
  │
  ▼
Temporary AWS credentials (expire in 1 hour)
```

No long-lived AWS keys stored in GitHub Secrets.

## Masking Custom Values

```yaml
# Mask custom secrets in logs
- name: Set up secrets
  run: |
    echo "::add-mask::$MY_SECRET"    # Masks this value in all subsequent logs
    echo "Value is $MY_SECRET"        # Shows as "***"
  env:
    MY_SECRET: ${{ secrets.MY_SECRET }}
```

## How It Works

### Secret Resolution Order

```
1. Repository secrets (available to all workflows)
2. Environment secrets (available to jobs using that environment)
3. Organization secrets (available to all repos in the org)
```

Later definitions override earlier ones.

## Common Mistakes

### Mistake 1: Hardcoding Secrets

```yaml
# WRONG — secret in the workflow file (committed to git)
env:
  API_KEY: 'sk-abc123secret'

# CORRECT — use GitHub Secrets
env:
  API_KEY: ${{ secrets.API_KEY }}
```

### Mistake 2: Not Using Environments

```yaml
# WRONG — same secrets for staging and production
deploy-staging:
  env:
    DB_URL: ${{ secrets.DATABASE_URL }}  # Which database?

# CORRECT — separate environments
deploy-staging:
  environment: staging
  env:
    DB_URL: ${{ secrets.DATABASE_URL }}  # Staging database

deploy-production:
  environment: production
  env:
    DB_URL: ${{ secrets.DATABASE_URL }}  # Production database
```

### Mistake 3: Exposing Secrets in Logs

```yaml
# WRONG — secret is printed in logs
- run: echo "API key is ${{ secrets.API_KEY }}"

# CORRECT — GitHub auto-masks secrets, but do not echo them
- run: echo "API key configured"  # Safe — does not print the value
```

## Try It Yourself

### Exercise 1: Add Repository Secrets

Add `DATABASE_URL` and `API_KEY` as repository secrets. Use them in a workflow step.

### Exercise 2: Create Environments

Create `staging` and `production` environments with different values for the same secret. Deploy to each.

### Exercise 3: Protection Rule

Add a required reviewer to the `production` environment. Trigger a deployment and verify it waits for approval.

## Next Steps

You understand CI/CD with GitHub Actions. Review the full guide at [nodejs-guide/index.html](../../index.html) to see all 26 chapters.
