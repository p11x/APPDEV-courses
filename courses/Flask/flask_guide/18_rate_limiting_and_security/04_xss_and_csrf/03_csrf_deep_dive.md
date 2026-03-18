<!-- FILE: 18_rate_limiting_and_security/04_xss_and_csrf/03_csrf_deep_dive.md -->

## Overview

Understand Cross-Site Request Forgery (CSRF) attacks and how to prevent them in Flask.

## Prerequisites

- Understanding of HTTP requests
- Basic web security knowledge

## Core Concepts

CSRF tricks users into unknowingly submitting requests to a site where they're authenticated. The attacker cannot see the response, but can trigger actions.

## How CSRF Works

```
1. User logs into bank.com, session cookie is set
2. User visits evil.com (in another tab)
3. evil.com has hidden form that submits to bank.com/transfer
4. User's browser automatically includes bank.com cookie
5. Transfer executes without user's knowledge
```

## Attack Example

```html
<!-- On evil.com -->
<html>
<body>
    <!-- Hidden form that submits to your Flask app -->
    <form action="https://yourapp.com/delete-account" method="POST" id="csrf-form">
        <input type="hidden" name="confirm" value="yes">
    </form>
    
    <script>
        // Automatically submit when page loads
        document.getElementById('csrf-form').submit();
    </script>
</body>
</html>
```

## Flask-WTF CSRF Protection

### Installation

```bash
pip install Flask-WTF
```

### Basic Setup

```python
# app.py
from flask import Flask
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired

app = Flask(__name__)
app.config['SECRET_KEY'] = 'your-secret-key'  # Required for CSRF

# Create form class
class MyForm(FlaskForm):
    name = StringField('Name', validators=[DataRequired()])
    submit = SubmitField('Submit')
```

### Template with CSRF Token

```html
<!-- templates/form.html -->
<form method="POST">
    <!-- CSRF token automatically included if using FlaskForm -->
    {{ form.hidden_tag() }}
    
    {{ form.name.label }}
    {{ form.name() }}
    
    {{ form.submit() }}
</form>

<!-- Or manually: -->
<input type="hidden" name="csrf_token" value="{{ csrf_token() }}"/>
```

### Manual CSRF Protection

```python
# For non-WTForms endpoints
from flask_wtf.csrf import generate_csrf, validate_csrf
from flask import request

@app.route('/api/action', methods=['POST'])
def api_action():
    # Validate CSRF token
    try:
        validate_csrf(request.form.get('csrf_token'))
    except:
        return {'error': 'CSRF token invalid'}, 400
    
    # Process the action
    return {'message': 'Success'}
```

### Custom CSRF Error Handler

```python
# app.py
from flask_wtf.csrf import CSRFError

@app.errorhandler(CSRFError)
def handle_csrf_error(e):
    return {'error': 'CSRF validation failed'}, 400
```

### AJAX CSRF Protection

```javascript
// Add CSRF token to all AJAX requests
fetch('/api/data', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'X-CSRFToken': '{{ csrf_token() }}'
    },
    body: JSON.stringify({data: 'value'})
});
```

### API Token Authentication (Alternative to CSRF)

For APIs, use token-based auth instead of cookies:

```python
# Token-based API (no CSRF needed)
@app.route('/api/protected', methods=['POST'])
@auth_required  # Your token validation decorator
def api_protected():
    return {'message': 'Protected data'}
```

## Common Mistakes

- ❌ Not using Flask-WTF forms
- ✅ Always use Flask-WTF or manual CSRF tokens

- ❌ Excluding API endpoints from CSRF
- ✅ Use token auth for APIs instead

- ❌ Using GET for state-changing operations
- ✅ Use POST/PUT/DELETE for actions

> **🔒 Security Note:** CSRF tokens must be unique per session and validated on every state-changing request.

## Quick Reference

| Method | CSRF Protection |
|--------|-----------------|
| FlaskForm | Automatic via `form.hidden_tag()` |
| Manual | `generate_csrf()`, `validate_csrf()` |
| AJAX | Include token in header |
| API | Use token auth, not cookies |

## Next Steps

Continue to [05_https_and_tls/01_why_https_matters.md](../05_https_and_tls/01_why_https_matters.md) to learn about HTTPS.
