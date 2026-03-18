<!-- FILE: 18_rate_limiting_and_security/03_security_headers/02_flask_talisman_setup.md -->

## Overview

Set up Flask-Talisman for automated security header management.

## Prerequisites

- Flask application
- Understanding of security headers

## Core Concepts

Flask-Talisman wraps your Flask app and automatically adds security headers. It simplifies CSP management and provides sensible defaults.

## Code Walkthrough

### Installation

```bash
pip install flask-talisman
```

### Basic Setup

```python
# app.py
from flask import Flask
from flask_talisman import Talisman

app = Flask(__name__)

# Initialize Talisman with defaults
talisman = Talisman(
    app,
    content_security_policy=None  # Set up in next example
)

# Your routes...
@app.route('/')
def index():
    return 'Hello, World!'
```

### With Content Security Policy

```python
# app.py
from flask import Flask
from flask_talisman import Talisman

app = Flask(__name__)

# Configure CSP
talisman = Talisman(
    app,
    content_security_policy={
        'default-src': "'self'",
        'script-src': "'self' https://cdn.example.com",
        'style-src': "'self' 'unsafe-inline'",
        'img-src': "'self' data: https:",
        'font-src': "'self'",
        'connect-src': "'self'",
        'frame-ancestors': "'none'",
    },
    content_security_policy_nonce_in=['script-src'],  # Add nonces to scripts
    content_security_policy_report_only=False,  # Don't enforce, just report
    content_security_policy_report_uri='/csp-violation-report',  # Where to send reports
)

# Optional: custom CSP for specific routes
@app.route('/payment')
@talisman(content_security_policy={'script-src': "'self' 'nonce-payment'"})
def payment():
    return render_template('payment.html')
```

### With HTTPS Enforcement

```python
# Production configuration
talisman = Talisman(
    app,
    force_https=True,  # Redirect HTTP to HTTPS
    force_https_permanent=True,  # Use 301 instead of 302
    
    # Strict Transport Security
    strict_transport_security='max-age=31536000; includeSubDomains; preload',
    strict_transport_security_preload=True,
    strict_transport_security_max_age=31536000,
    
    # Frame options
    x_frame_options='DENY',  # Or 'SAMEORIGIN'
    
    # Other options
    x_content_type_options='nosniff',
    x_xss_protection=1,
    referrer_policy='strict-origin-when-cross-origin',
)
```

### Error Handler for CSP Violations

```python
# Handle CSP violation reports
@app.route('/csp-violation-report', methods=['POST'])
def csp_report():
    """Log CSP violations."""
    data = request.get_json()
    if data:
        # Log the violation
        current_app.logger.warning(f'CSP Violation: {data}')
    return '', 204
```

### Line-by-Line Breakdown

- `Talisman()` initializes security headers
- `content_security_policy` dict defines what resources can load
- `force_https=True` enforces HTTPS
- `nonce` support allows inline scripts that are still secure

> **⚠️ Warning:** CSP can break your app if not configured correctly. Test thoroughly.

## Common Mistakes

- ❌ Too restrictive CSP breaks functionality
- ✅ Start with report-only mode

- ❌ Not testing in development
- ✅ Test CSP on staging before production

- ❌ Allowing 'unsafe-inline' for scripts
- ✅ Use nonces or hashes instead

## Quick Reference

| Option | Purpose |
|--------|---------|
| `force_https` | Redirect HTTP to HTTPS |
| `strict_transport_security` | HSTS header |
| `x_frame_options` | Clickjacking protection |
| `content_security_policy` | Resource loading rules |

## Next Steps

Continue to [03_csp_and_hsts_explained.md](./03_csp_and_hsts_explained.md) to learn about CSP and HSTS in detail.
