<!-- FILE: 18_rate_limiting_and_security/03_security_headers/03_csp_and_hsts_explained.md -->

## Overview

Deep dive into Content Security Policy (CSP) and HTTP Strict Transport Security (HSTS).

## Prerequisites

- Understanding of HTTP
- Basic security knowledge

## Core Concepts

### Content Security Policy (CSP)

CSP is the most powerful security header. It tells browsers exactly which resources can be loaded, preventing XSS and data injection attacks.

### CSP Directive Reference

| Directive | Description | Example |
|-----------|-------------|---------|
| default-src | Fallback for other directives | 'self' |
| script-src | Allowed JavaScript sources | 'self' https://cdn.jsdelivr.net |
| style-src | Allowed CSS sources | 'self' 'unsafe-inline' |
| img-src | Allowed image sources | 'self' data: https: |
| font-src | Allowed font sources | 'self' |
| connect-src | Allowed fetch/XHR sources | 'self' |
| frame-ancestors | Allowed parent pages | 'none' |
| form-action | Allowed form targets | 'self' |
| base-uri | Allowed base elements | 'self' |

### Source Values

| Value | Meaning |
|-------|---------|
| 'self' | Same origin only |
| 'none' | Block everything |
| 'unsafe-inline' | Inline scripts/styles (dangerous!) |
| 'unsafe-eval' | eval() (dangerous!) |
| data: | Data URIs |
| https: | Any HTTPS source |
| nonce-xxx | Script with matching nonce |
| hash-xxx | Script with matching hash |

## Code Walkthrough

### Basic CSP Examples

```python
# Allow everything from self, nothing from elsewhere
{
    'default-src': "'self'",
    'script-src': "'self'",
    'style-src': "'self'",
}

# Allow common CDN
{
    'default-src': "'self'",
    'script-src': "'self' https://cdn.jsdelivr.net",
    'img-src': "'self' https://images.unsplash.com data:",
}
```

### Using Nonces for Inline Scripts

```python
# Nonce-based CSP (more secure than 'unsafe-inline')
import secrets

@app.after_request
def add_csp_nonce(response):
    # Generate nonce for this response
    nonce = secrets.token_hex(16)
    response.headers['Content-Security-Policy'] = (
        f"default-src 'self'; "
        f"script-src 'self' 'nonce-{nonce}'; "
        f"style-src 'self' 'unsafe-inline'"
    )
    # Store nonce for use in templates
    g.csp_nonce = nonce
    return response

# Template usage
# <script nonce="{{ g.csp_nonce }}">...</script>
```

### HSTS Deep Dive

HSTS tells browsers to only connect via HTTPS for a specified time.

```
# Basic HSTS
Strict-Transport-Security: max-age=31536000

# Include subdomains
Strict-Transport-Security: max-age=31536000; includeSubDomains

# Submit to preload list
Strict-Transport-Security: max-age=63072000; includeSubDomains; preload
```

### Flask Implementation

```python
# app.py
@app.after_request
def add_security_headers(response):
    # CSP
    csp = {
        'default-src': "'self'",
        'script-src': "'self' https://cdn.jsdelivr.net 'nonce-{nonce}'",
        'style-src': "'self' 'unsafe-inline'",
        'img-src': "'self' data: https:",
        'font-src': "'self'",
        'connect-src': "'self'",
        'frame-ancestors': "'none'",
    }
    
    # Add nonce to script-src
    nonce = secrets.token_hex(16)
    csp['script-src'] = csp['script-src'].format(nonce=nonce)
    g.csp_nonce = nonce
    
    # Build CSP header
    csp_header = '; '.join(f"{k} {v}" for k, v in csp.items())
    response.headers['Content-Security-Policy'] = csp_header
    
    # HSTS
    response.headers['Strict-Transport-Security'] = (
        'max-age=31536000; includeSubDomains; preload'
    )
    
    return response
```

> **⚠️ Warning:** HSTS is permanent for its max-age duration. Test thoroughly before deploying with long durations.

## Common Mistakes

- ❌ Using 'unsafe-inline' for scripts
- ✅ Use nonces or hashes instead

- ❌ Not testing CSP in development
- ✅ Add test endpoint for violations

- ❌ Setting HSTS max-age too long initially
- ✅ Start with shorter duration

## Quick Reference

| Feature | Recommendation |
|---------|----------------|
| default-src | 'self' |
| script-src | 'self' + trusted CDNs |
| style-src | 'self' + 'unsafe-inline' if needed |
| img-src | 'self' data: https: |
| HSTS max-age | Start with 300, increase over time |

## Next Steps

Continue to [04_xss_and_csrf/01_what_is_xss.md](../04_xss_and_csrf/01_what_is_xss.md) to learn about XSS attacks.
