<!-- FILE: 18_rate_limiting_and_security/03_security_headers/01_what_are_security_headers.md -->

## Overview

Understand HTTP security headers and their role in protecting Flask applications.

## Prerequisites

- Basic understanding of HTTP
- Knowledge of web browsers

## Core Concepts

HTTP security headers are response headers that tell browsers how to behave when handling your site. They provide protection against various attacks.

## Core Headers Explained

### 1. Strict-Transport-Security (HSTS)

**Purpose:** Enforce HTTPS connections

```
Strict-Transport-Security: max-age=31536000; includeSubDomains
```

**Effect:** Browser only connects via HTTPS for one year

### 2. Content-Security-Policy (CSP)

**Purpose:** Prevent XSS by controlling resource loading

```
Content-Security-Policy: default-src 'self'; script-src 'self' https://cdn.example.com
```

**Effect:** Only load scripts from your domain and allowed CDNs

### 3. X-Content-Type-Options

**Purpose:** Prevent MIME-type sniffing

```
X-Content-Type-Options: nosniff
```

**Effect:** Browser respects declared content types

### 4. X-Frame-Options

**Purpose:** Prevent clickjacking by controlling iframe embedding

```
X-Frame-Options: DENY
```

**Effect:** No other site can embed your pages

### 5. X-XSS-Protection (Legacy)

**Purpose:** Enable browser XSS filter (deprecated but still useful)

```
X-XSS-Protection: 1; mode=block
```

### 6. Referrer-Policy

**Purpose:** Control information sent to other sites

```
Referrer-Policy: strict-origin-when-cross-origin
```

### 7. Permissions-Policy

**Purpose:** Control browser features available to your site

```
Permissions-Policy: geolocation=(), microphone=()
```

## Code Walkthrough

### Manual Header Setting

```python
# app.py
from flask import Flask, make_response

app = Flask(__name__)

@app.after_request
def add_security_headers(response):
    """Add security headers to all responses."""
    
    # HSTS - enforce HTTPS
    response.headers['Strict-Transport-Security'] = 'max-age=31536000; includeSubDomains'
    
    # CSP - content security policy
    response.headers['Content-Security-Policy'] = "default-src 'self'; img-src 'self' https: data:; style-src 'self' 'unsafe-inline'"
    
    # Prevent MIME sniffing
    response.headers['X-Content-Type-Options'] = 'nosniff'
    
    # Prevent clickjacking
    response.headers['X-Frame-Options'] = 'DENY'
    
    # XSS filter (legacy)
    response.headers['X-XSS-Protection'] = '1; mode=block'
    
    # Referrer policy
    response.headers['Referrer-Policy'] = 'strict-origin-when-cross-origin'
    
    # Permissions policy
    response.headers['Permissions-Policy'] = 'geolocation=(), microphone=(), camera=()'
    
    return response
```

### Testing Headers

```bash
# Check headers with curl
curl -I https://yourapp.com

# Should see:
# Strict-Transport-Security: max-age=31536000; includeSubDomains
# Content-Security-Policy: ...
# X-Content-Type-Options: nosniff
# X-Frame-Options: DENY
# X-XSS-Protection: 1; mode=block
```

> **💡 Tip:** Use Flask-Talisman for easier CSP management in production.

## Quick Reference

| Header | Purpose | Recommended Value |
|--------|---------|-------------------|
| HSTS | Force HTTPS | max-age=31536000 |
| CSP | Resource control | default-src 'self' |
| X-Frame-Options | Clickjacking | DENY or SAMEORIGIN |
| X-Content-Type | MIME sniffing | nosniff |
| Referrer-Policy | Privacy | strict-origin-when-cross-origin |

## Next Steps

Continue to [02_flask_talisman_setup.md](./02_flask_talisman_setup.md) to learn about Flask-Talisman.
