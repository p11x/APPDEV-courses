# Railway Deployment

## What You'll Learn

- Deploying Node.js to Railway
- Database provisioning
- Automatic deployments from GitHub

## Setup

```bash
# Install Railway CLI
npm install -g @railway/cli

# Login and deploy
railway login
railway init
railway up
```

## railway.json

```json
{
  "$schema": "https://railway.com/railway.schema.json",
  "build": { "builder": "NIXPACKS" },
  "deploy": {
    "startCommand": "npm start",
    "healthcheckPath": "/healthz",
    "restartPolicyType": "ON_FAILURE"
  }
}
```

## Next Steps

For monitoring, continue to [Health Checks](../monitoring/01-health-checks.md).
