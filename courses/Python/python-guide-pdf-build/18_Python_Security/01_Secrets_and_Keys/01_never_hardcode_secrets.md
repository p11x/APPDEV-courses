# 🔐 Never Hardcode Secrets

## 🎯 What You'll Learn

- The dangers of hardcoded secrets
- Using environment variables
- The secrets module

---

## The Problem

```python
# ❌ NEVER do this!
API_KEY = "sk-1234567890abcdef"

# Even in comments!
# API_KEY = "secret"  # DON'T!

# In logs!
print(f"API key: {api_key}")  # DON'T!
```

---

## Using Environment Variables

```python
import os

# Get from environment
api_key = os.environ.get("ANTHROPIC_API_KEY")
api_key = os.environ["ANTHROPIC_API_KEY"]  # Raises if not set
```

---

## Using python-dotenv

```bash
pip install python-dotenv
```

```python
# .env file
ANTHROPIC_API_KEY=sk-...

# In code
from dotenv import load_dotenv
load_dotenv()

api_key = os.environ.get("ANTHROPIC_API_KEY")
```

---

## .gitignore

```gitignore
# .gitignore
.env
*.env
secrets.json
config/secrets.py
__pycache__/
```

---

## The secrets Module

```python
import secrets

# Generate secure random token
token = secrets.token_hex(32)  # 64 character hex string
token = secrets.token_urlsafe(32)  # URL-safe base64

# Timing-safe comparison
if secrets.compare_digest(a, b):
    # Safe to compare!
    pass
```

---

## ✅ Summary

- Never hardcode secrets in source
- Use environment variables or .env files
- Add .env to .gitignore
- Use secrets.token_hex() for random values

## 🔗 Further Reading

- [python-dotenv](https://pypi.org/project/python-dotenv/)
