<!-- FILE: 18_rate_limiting_and_security/04_xss_and_csrf/02_preventing_xss_in_flask.md -->

## Overview

Prevent XSS attacks in Flask applications using Jinja2 autoescaping and proper sanitization.

## Prerequisites

- Understanding of XSS attacks
- Knowledge of Jinja2 templates

## Core Concepts

Flask uses Jinja2 for templates, which automatically escapes content by default. Understanding when autoescaping works and when it doesn't is critical.

## Code Walkthrough

### Jinja2 Auto-Escaping

Jinja2 automatically escapes variables by default:

```html
<!-- template.html -->
<!-- Jinja2 escapes this automatically -->
<p>Hello, {{ username }}</p>

<!-- If username is "<script>alert('xss')</script>" -->
<!-- Output: <p>Hello, <script>alert(&#x27;xss&#x27;)</script></p> -->
```

### When Auto-Escaping Works

```python
# ✅ SAFE - Jinja2 auto-escapes
@app.route('/user/<username>')
def user_profile(username):
    return render_template('profile.html', username=username)

# Template:
# <h1>{{ username }}</h1>
```

### When Auto-Escaping Fails

```python
# ❌ DANGEROUS - Using |safe filter
@app.route('/comment', methods=['POST'])
def add_comment():
    comment = request.form['comment']
    # Using |safe tells Jinja2 NOT to escape
    return render_template('comment.html', comment=comment)

# Template (DANGEROUS!):
# <div>{{ comment | safe }}</div>
```

### Safe HTML Content

If you need to allow some HTML, use a sanitizer:

```bash
pip install bleach
```

```python
# utils.py
import bleach

ALLOWED_TAGS = ['b', 'i', 'u', 'em', 'strong', 'p', 'br', 'ul', 'ol', 'li']
ALLOWED_ATTRIBUTES = {
    'a': ['href', 'title'],
    'img': ['src', 'alt'],
}

def sanitize_html(dirty_html: str) -> str:
    """Remove dangerous HTML while allowing safe tags."""
    return bleach.clean(
        dirty_html,
        tags=ALLOWED_TAGS,
        attributes=ALLOWED_ATTRIBUTES,
        strip=True
    )

# Usage
@app.route('/comment', methods=['POST'])
def add_comment():
    comment = request.form['comment']
    # Sanitize before storing
    safe_comment = sanitize_html(comment)
    # Now safe to render
    return render_template('comment.html', comment=safe_comment)
```

### JSON Response Protection

```python
# For JSON APIs, use jsonify (auto-escapes)
import json
from flask import jsonify

@app.route('/api/user', methods=['GET'])
def get_user():
    user = {
        'username': '<script>alert(1)</script>',
        'email': 'test@example.com'
    }
    # jsonify automatically escapes
    return jsonify(user)
    # Output: {"username": "<script>alert(1)</script>", ...}
    # The script tag is escaped in JSON
```

### Setting JSON Content Type

```python
@app.route('/api/data')
def api_data():
    # Always set proper content type
    response = jsonify({'data': 'some data'})
    response.headers['Content-Type'] = 'application/json; charset=utf-8'
    return response
```

### Content Security Policy

```python
# Prevent XSS with CSP
@app.after_request
def add_csp(response):
    response.headers['Content-Security-Policy'] = (
        "default-src 'self'; "
        "script-src 'self'; "  # No inline scripts
        "style-src 'self' 'unsafe-inline'"
    )
    return response
```

> **🔒 Security Note:** Never use `|safe` on user input. Only use it on trusted, hardcoded content.

## Common Mistakes

- ❌ Using `|safe` on user input
- ✅ Use bleach to sanitize HTML

- ❌ Rendering user input as JavaScript
- ✅ Never put user input in `<script>` tags

- ❌ Using `innerHTML` with user input in JavaScript
- ✅ Use `textContent` instead

## Quick Reference

| Method | Safe? |
|--------|-------|
| `{{ variable }}` | ✅ Yes (auto-escaped) |
| `{{ variable \| safe }}` | ❌ No |
| `bleach.clean()` | ✅ Yes (with allowlist) |
| Jinja2 `for` loops | ✅ Yes |

## Next Steps

Continue to [03_csrf_deep_dive.md](./03_csrf_deep_dive.md) to learn about CSRF attacks.
