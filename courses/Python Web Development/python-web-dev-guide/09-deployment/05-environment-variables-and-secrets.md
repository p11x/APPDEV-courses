# Environment Variables and Secrets

## What You'll Learn
- Managing secrets safely
- Using python-dotenv

## Prerequisites
- Completed deployment basics

## Using python-dotenv

```bash
pip install python-dotenv
```

Create `.env` file:
```
SECRET_KEY=your-secret-key
DATABASE_URL=postgresql://...
DEBUG=False
```

Load in code:
```python
from dotenv import load_dotenv
import os

load_dotenv()

SECRET_KEY = os.getenv("SECRET_KEY")
DATABASE_URL = os.getenv("DATABASE_URL")
```

## .gitignore

Add to `.gitignore`:
```
.env
.env.*
```

## Summary
- Never commit secrets to git
- Use .env files locally
- Use environment variables in production
