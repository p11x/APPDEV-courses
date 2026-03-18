# 🚀 Deploying Python Applications

## 🎯 What You'll Learn

- Deployment platforms for Python
- Writing render.yaml
- GitHub Actions for CI/CD
- Health check endpoints

---

## Platform Options

| Platform | Free Tier | Best For |
|----------|-----------|----------|
| Railway | Yes | Quick deployments |
| Render | Yes | Web apps |
| Fly.io | Yes | Docker-based |
| DigitalOcean | Small | VPS |

---

## Render Deployment

```yaml
# render.yaml
services:
  - type: web
    name: myapp
    env: python
    buildCommand: pip install -r requirements.txt
    startCommand: uvicorn main:app
    envVars:
      - key: ANTHROPIC_API_KEY
        sync: false
```

---

## Fly.io Deployment

```bash
# Install flyctl
brew install flyctl

# Launch
fly launch

# Deploy
fly deploy

# Set secrets
fly secrets set ANTHROPIC_API_KEY=your_key
```

---

## Health Check Endpoint

```python
from fastapi import FastAPI

app = FastAPI()

@app.get("/health")
def health_check():
    return {"status": "ok"}

@app.get("/health/ready")
def readiness():
    # Check database, cache, etc.
    return {"status": "ready"}
```

---

## GitHub Actions CI/CD

```yaml
# .github/workflows/deploy.yml
name: Deploy

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Set up Python
        uses: actions/setup-python@v4
        with:
          python-version: '3.12'
      
      - name: Install dependencies
        run: |
          pip install -r requirements.txt
          pip install pytest
          pytest  # Run tests!
      
      - name: Deploy
        run: |
          # Deploy to your platform
          echo "Deploying..."
```

---

## ✅ Summary

- Platforms like Render/Fly.io offer easy deployments
- Use render.yaml or fly.toml for configuration
- Always add health check endpoints
- GitHub Actions for automated testing and deployment

## 🔗 Further Reading

- [Render Docs](https://render.com/docs)
- [Fly.io Docs](https://fly.io/docs/)
