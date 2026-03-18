<!-- FILE: 10_deployment/01_production_config/02_secret_key_management.md -->

## Overview

The **SECRET_KEY** is a critical security component in Flask applications. It's used to sign session cookies, CSRF tokens, and other security-related features. This file covers best practices for generating, storing, and managing secret keys in production.

## Core Concepts

### What is SECRET_KEY Used For?

- Signing session cookies (prevents tampering)
- CSRF token generation and validation
- Flask's `flash()` messages
- Any cryptographic signing in Flask extensions

### Requirements for a Good Secret Key

- **Unpredictable** - Cannot be guessed
- **Long enough** - Minimum 32 bytes (256 bits) of entropy
- **Unique** - Different for each application and environment
- **Secret** - Never exposed in version control or logs

## Code Walkthrough

### Generating Secure Secret Keys

```bash
# Method 1: Using OpenSSL (Linux/macOS)
openssl rand -hex 32

# Method 2: Using Python secrets module
python -c "import secrets; print(secrets.token_hex(32))"

# Method 3: Using uuid (less secure but better than hardcoding)
python -c "import uuid; print(uuid.uuid4().hex + uuid.uuid4().hex)"
```

### Storing Secret Keys Securely

```python
# app.py — Production-ready secret key management
import os
import secrets
from flask import Flask

app = Flask(__name__)

def get_secret_key():
    """Get secret key from environment or generate for development."""
    # Try to get from environment variable first
    secret_key = os.environ.get('SECRET_KEY')
    
    if secret_key:
        return secret_key
    
    # In development, generate a random key (changes each restart)
    # WARNING: This logs users out on every restart!
    if os.environ.get('FLASK_ENV') == 'development':
        # Generate and store in a file for persistence during development
        key_file = '.secret_key'
        if os.path.exists(key_file):
            with open(key_file, 'r') as f:
                return f.read().strip()
        else:
            # Generate new key and save it
            secret_key = secrets.token_hex(32)
            with open(key_file, 'w') as f:
                f.write(secret_key)
            return secret_key
    
    # In production, require environment variable
    raise RuntimeError(
        "SECRET_KEY environment variable must be set in production"
    )

app.config['SECRET_KEY'] = get_secret_key()

# Alternative: Direct environment variable (simplest for production)
# app.config['SECRET_KEY'] = os.environ['SECRET_KEY']  # Will raise KeyError if missing
```

### Using AWS Secrets Manager or Similar Services

```python
# app.py — Getting secret from AWS Secrets Manager
import os
import boto3
import json
from botocore.exceptions import ClientError
from flask import Flask

app = Flask(__name__)

def get_secret_from_aws(secret_name, region_name="us-east-1"):
    """Retrieve secret from AWS Secrets Manager."""
    # Create a Secrets Manager client
    session = boto3.session.Session()
    client = session.client(
        service_name='secretsmanager',
        region_name=region_name
    )
    
    try:
        get_secret_value_response = client.get_secret_value(
            SecretId=secret_name
        )
    except ClientError as e:
        # Handle specific errors
        raise e
    
    # Decrypts secret using the associated KMS key.
    secret = get_secret_value_response['SecretString']
    return secret

# Get secret key from AWS
try:
    secret_key = get_secret_from_aws('flask/app/secret_key')
    app.config['SECRET_KEY'] = secret_key
except Exception as e:
    # Fallback for development
    if os.environ.get('FLASK_ENV') == 'development':
        app.config['SECRET_KEY'] = secrets.token_hex(32)
    else:
        raise e
```

### Rotating Secret Keys

```python
# app.py — Supporting key rotation (advanced)
import os
from flask import Flask
from itsdangerous import TimedSerializer, BadSignature

app = Flask(__name__)

# Support multiple keys for rotation
def get_secret_keys():
    """Get list of valid secret keys (current + previous)."""
    keys = []
    
    # Current key
    current_key = os.environ.get('SECRET_KEY')
    if current_key:
        keys.append(current_key)
    
    # Previous key (for rotation period)
    previous_key = os.environ.get('SECRET_KEY_PREVIOUS')
    if previous_key:
        keys.append(previous_key)
    
    if not keys:
        raise RuntimeError("No secret keys configured")
    
    return keys

app.config['SECRET_KEY'] = get_secret_keys()[0]  # Use first for signing

# Custom session interface that accepts multiple keys
# (This is advanced - Flask's default session uses single key)
# For production, consider using Flask-Session with server-side storage
```

## Common Mistakes

❌ **Using weak or predictable keys**
```python
# WRONG — Easily guessable
app.config['SECRET_KEY'] = 'secret'
app.config['SECRET_KEY'] = 'supersecretkey123'
app.config['SECRET_KEY'] = 'flask'
```

✅ **Correct — Use cryptographically random keys**
```python
# CORRECT
app.config['SECRET_KEY'] = secrets.token_hex(32)
# Results in something like: 'a1b2c3d4e5f6...' (64 hex characters)
```

❌ **Committing secret keys to version control**
```bash
# WRONG — Exposes your secret to anyone with repo access
echo "SECRET_KEY=mysecret" >> .env
git add .env
```

✅ **Correct — Keep secrets out of version control**
```bash
# CORRECT
echo ".env" >> .gitignore
echo ".secret_key" >> .gitignore
```

❌ **Using the same key in all environments**
```python
# WRONG — If development key leaks, production is compromised
# Same key in dev, staging, and production
```

✅ **Correct — Use different keys per environment**
```bash
# CORRECT
# Development: SECRET_KEY=dev-key-123
# Staging: SECRET_KEY=staging-key-456
# Production: SECRET_KEY=prod-key-789 (from secure vault)
```

## Quick Reference

| Method | Description |
|--------|-------------|
| `secrets.token_hex(32)` | Generate secure key (Python 3.6+) |
| `openssl rand -hex 32` | Generate secure key (OpenSSL) |
| `os.environ.get('SECRET_KEY')` | Get from environment |
| `SECRET_KEY` in .env | Store in environment file (dev only) |

## Next Steps

Now you can manage secret keys securely. Continue to [03_production_checklist.md](03_production_checklist.md) for a comprehensive production deployment checklist.