# Deploy to VPS

## What You'll Learn

- How to deploy to a VPS via SSH in GitHub Actions
- How to use appleboy/ssh-action
- How to run deployment commands remotely
- How to add a health check after deployment
- How to handle deployment failures

## Deploy via SSH

```yaml
# .github/workflows/deploy.yml

name: Deploy

on:
  push:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
      - run: npm ci
      - run: npm test

  deploy:
    runs-on: ubuntu-latest
    needs: test
    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      # Deploy via SSH
      - name: Deploy to VPS
        uses: appleboy/ssh-action@v1
        with:
          host: ${{ secrets.VPS_HOST }}         # Server IP or hostname
          username: ${{ secrets.VPS_USER }}      # SSH username
          key: ${{ secrets.VPS_SSH_KEY }}        # SSH private key
          port: 22

          # Commands to run on the server
          script: |
            cd /opt/myapp

            # Pull latest code
            git pull origin main

            # Install dependencies
            npm ci --only=production

            # Run database migrations
            npx prisma migrate deploy

            # Build the application
            npm run build

            # Restart the application with PM2
            pm2 reload ecosystem.config.js --env production

            # Save PM2 process list (persists across reboots)
            pm2 save

      # Health check after deployment
      - name: Health check
        run: |
          echo "Waiting for application to start..."
          sleep 10

          for i in $(seq 1 5); do
            STATUS=$(curl -s -o /dev/null -w "%{http_code}" https://${{ secrets.VPS_HOST }}/healthz)
            if [ "$STATUS" = "200" ]; then
              echo "Health check passed!"
              exit 0
            fi
            echo "Attempt $i: status $STATUS — retrying in 5s..."
            sleep 5
          done

          echo "Health check failed!"
          exit 1

      # Notify on failure
      - name: Notify on failure
        if: failure()
        run: |
          echo "Deployment failed!" 
          # Add Slack/email notification here
```

## SSH Key Setup

### Generate SSH Key

```bash
# Generate a key pair for GitHub Actions
ssh-keygen -t ed25519 -C "github-actions" -f github-actions-key -N ""

# Copy public key to your server
ssh-copy-id -i github-actions-key.pub user@your-server.com

# Add private key to GitHub Secrets
# GitHub repo → Settings → Secrets → Actions:
#   VPS_HOST: your-server.com
#   VPS_USER: deploy
#   VPS_SSH_KEY: (contents of github-actions-key)
```

## PM2 Ecosystem File

```js
// ecosystem.config.js — PM2 configuration

module.exports = {
  apps: [
    {
      name: 'myapp',
      script: 'dist/index.js',
      instances: 'max',     // Use all CPU cores
      exec_mode: 'cluster',
      env: {
        NODE_ENV: 'production',
        PORT: 3000,
      },
    },
  ],
};
```

## Deployment with Docker

```yaml
# .github/workflows/deploy-docker.yml

name: Deploy with Docker

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Deploy via SSH
        uses: appleboy/ssh-action@v1
        with:
          host: ${{ secrets.VPS_HOST }}
          username: ${{ secrets.VPS_USER }}
          key: ${{ secrets.VPS_SSH_KEY }}
          script: |
            cd /opt/myapp

            # Pull latest image
            docker compose pull

            # Restart services with new images
            docker compose up -d --remove-orphans

            # Clean up old images
            docker image prune -f

            # Wait and check health
            sleep 10
            curl -f http://localhost:3000/healthz || exit 1
```

## How It Works

### Deployment Flow

```
Push to main
  → GitHub Actions runs tests
  → SSH into VPS
  → git pull → npm ci → build → pm2 reload
  → Health check
  → Done ✓
```

### Rollback

If deployment fails, manually roll back:

```bash
# SSH into the server
ssh user@your-server.com

# Rollback to previous git commit
cd /opt/myapp
git log --oneline -5
git checkout <previous-commit>
npm ci --only=production
npm run build
pm2 reload ecosystem.config.js
```

## Common Mistakes

### Mistake 1: Deploying Without Tests

```yaml
# WRONG — deploy directly, no test gate
jobs:
  deploy:
    steps:
      - run: ssh server 'git pull && pm2 reload app'

# CORRECT — test first, then deploy
jobs:
  test:
    steps:
      - run: npm test
  deploy:
    needs: test  # Only deploy if tests pass
```

### Mistake 2: Not Checking Health After Deploy

```yaml
# WRONG — deploy succeeds but app is crashed
- name: Deploy
  run: ssh server 'pm2 reload app'
  # No health check — app could be broken

# CORRECT — verify the app is healthy
- name: Health check
  run: curl -f https://server/healthz
```

### Mistake 3: SSH Key in Repository

```yaml
# WRONG — SSH key committed to git
key: |
  -----BEGIN OPENSSH PRIVATE KEY-----
  ...
  -----END OPENSSH PRIVATE KEY-----

# CORRECT — use GitHub Secrets
key: ${{ secrets.VPS_SSH_KEY }}
```

## Try It Yourself

### Exercise 1: Manual Deploy

Set up SSH access to a VPS. Deploy your app manually with `pm2 reload`.

### Exercise 2: Automated Deploy

Create a GitHub Actions workflow that deploys on push to main. Include a health check.

### Exercise 3: Rollback

Simulate a failed deployment. Roll back to the previous version and verify the app works.

## Next Steps

You can deploy to a VPS. For managing environment secrets, continue to [Environment Secrets](./02-environment-secrets.md).
