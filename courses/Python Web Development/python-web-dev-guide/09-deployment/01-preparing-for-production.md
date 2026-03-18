# Preparing for Production

## What You'll Learn
- Production vs development
- Security checklist
- Performance considerations

## Prerequisites
- Completed application development

## Production Checklist

1. **Environment Variables**
   - Never commit secrets to git
   - Use .env files

2. **Debug Mode**
   - Always disable debug mode

3. **Database**
   - Use PostgreSQL in production

4. **HTTPS**
   - Use SSL/TLS certificates

5. **Logging**
   - Set up proper logging

## Environment Variables

```python
import os

SECRET_KEY = os.environ.get("SECRET_KEY")
DATABASE_URL = os.environ.get("DATABASE_URL")
DEBUG = os.environ.get("DEBUG", "False") == "True"
```

## Summary
- Use environment variables for secrets
- Disable debug in production
- Use production database
- Enable HTTPS
