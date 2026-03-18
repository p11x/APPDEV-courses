<!-- FILE: 10_deployment/01_production_config/01_environment_variables.md -->

## Overview

**Environment variables** are a secure way to configure your Flask application for different environments (development, testing, production) without hardcoding sensitive information like secret keys, database passwords, or API keys in your source code.

## Core Concepts

### Why Use Environment Variables?

- **Security** - Keep secrets out of version control
- **Flexibility** - Same code runs in different environments
- **Best practice** - 12-factor app methodology
- **Deployment friendly** - Easy to configure in hosting platforms

### Common Environment Variables

| Variable | Purpose |
|----------|---------|
| `SECRET_KEY` | Flask session security |
| `DATABASE_URL` | Database connection string |
| `FLASK_ENV` | Environment (development/production) |
| `FLASK_DEBUG` | Debug mode toggle |
| `API_KEY` | Third-party API keys |

## Code Walkthrough

### Loading Environment Variables

```python
# app.py — Using environment variables
import os
from flask import Flask
from dotenv import load_dotenv  # Optional: for loading .env files

# Load .env file (optional, mainly for development)
load_dotenv()

app = Flask(__name__)

# Get environment variables with defaults
app.config['SECRET_KEY'] = os.environ.get('SECRET_KEY') or 'dev-secret-key'
app.config['DEBUG'] = os.environ.get('FLASK_DEBUG', 'False').lower() == 'true'

# Database configuration
app.config['SQLALCHEMY_DATABASE_URI'] = os.environ.get(
    'DATABASE_URL', 
    'sqlite:///app.db'  # Default for development
)

# Other configuration
app.config['API_KEY'] = os.environ.get('API_KEY')  # Required in production

@app.route('/')
def index():
    return f"Environment: {os.environ.get('FLASK_ENV', 'development')}"

if __name__ == '__main__':
    app.run(debug=app.config['DEBUG'])
```

### Using python-dotenv

```bash
# Install python-dotenv
pip install python-dotenv
```

```env
# .env file (NEVER commit this to version control!)
SECRET_KEY=your-super-secret-key-here
DATABASE_URL=postgresql://user:password@localhost/mydb
FLASK_ENV=development
FLASK_DEBUG=True
API_KEY=your-third-party-api-key
```

```python
# app.py
from dotenv import load_dotenv
import os

load_dotenv()  # Loads variables from .env file

app = Flask(__name__)
app.config['SECRET_KEY'] = os.environ.get('SECRET_KEY')
```

### Accessing Environment Variables in Templates

```python
# app.py — Making env vars available in templates
@app.context_processor
def inject_env():
    """Make environment variables available in templates."""
    return dict(
        env=os.environ,
        google_analytics_id=os.environ.get('GA_ID')
    )
```

```html
<!-- In template -->
{% if env.FLASK_ENV == 'production' %}
    <script>
      // Production-only analytics
      (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
      (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
      m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
      })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

      ga('create', '{{ google_analytics_id }}', 'auto');
      ga('send', 'pageview');
    </script>
{% endif %}
```

## Common Mistakes

❌ **Hardcoding secrets in source code**
```python
# WRONG — Never do this!
app.config['SECRET_KEY'] = 'my-secret-key'
```

✅ **Correct — Use environment variables**
```python
# CORRECT
app.config['SECRET_KEY'] = os.environ.get('SECRET_KEY')
```

❌ **Committing .env files to version control**
```bash
# WRONG — Exposes secrets
git add .env
git commit -m "Add environment file"
```

✅ **Correct — Add .env to .gitignore**
```bash
# CORRECT
echo ".env" >> .gitignore
```

## Quick Reference

| Method | Description |
|--------|-------------|
| `os.environ.get('KEY')` | Get environment variable |
| `os.environ.get('KEY', 'default')` | With default value |
| `load_dotenv()` | Load .env file (development) |
| `os.environ['KEY']` | Get (raises KeyError if missing) |

## Next Steps

Now you understand environment variables. Continue to [02_secret_key_management.md](02_secret_key_management.md) to learn about secure secret key handling.