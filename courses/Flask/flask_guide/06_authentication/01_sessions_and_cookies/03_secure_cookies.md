<!-- FILE: 06_authentication/01_sessions_and_cookies/03_secure_cookies.md -->

## Overview

Securing cookies is essential for production applications. This file covers Flask's cookie security options: httponly, secure, samesite flags, and proper secret key management.

## Core Concepts

### Cookie Security Flags

| Flag | Purpose |
|------|---------|
| `httponly` | Prevents JavaScript access |
| `secure` | Only sent over HTTPS |
| `samesite` | Prevents CSRF |

## Code Walkthrough

### Secure Cookie Configuration

```python
# app.py — Secure cookies
from flask import Flask, session

app = Flask(__name__)

# Strong secret key (use environment variable in production)
app.secret_key = os.environ.get("SECRET_KEY") or os.urandom(32)

# Session cookie settings
app.config["SESSION_COOKIE_HTTPONLY"] = True  # JavaScript can't read
app.config["SESSION_COOKIE_SECURE"] = True    # HTTPS only
app.config["SESSION_COOKIE_SAMESITE"] = "Lax" # CSRF protection

# Alternative: Set per-session
session.permanent = True
response.set_cookie("session", session_data, httponly=True, secure=True, samesite="Lax")
```

> **⚠️ Security Note:** Always use strong secret keys in production.

## Next Steps

Now sessions are secure. Continue to [01_installing_flask_login.md](../../07_authentication/02_flask_login/01_installing_flask_login.md) to learn Flask-Login.