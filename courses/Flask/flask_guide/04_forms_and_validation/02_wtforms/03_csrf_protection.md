<!-- FILE: 04_forms_and_validation/02_wtforms/03_csrf_protection.md -->

## Overview

**CSRF (Cross-Site Request Forgery)** is a security vulnerability where attackers trick users into submitting forms on sites where they are authenticated. Flask-WTF provides automatic CSRF protection. This file explains CSRF and how to enable and use Flask-WTF's protection.

## Prerequisites

- Flask-WTF installed
- Basic form handling knowledge

## Core Concepts

### What is CSRF?

An attacker creates a malicious page that submits a form to your site:
```html
<!-- On attacker's site -->
<form action="https://yoursite.com/transfer" method="POST">
    <input type="hidden" name="to" value="attacker">
    <input type="hidden" name="amount" value="10000">
</form>
<script>document.forms[0].submit();</script>
```

If user is logged in, the transfer happens unknowingly.

### How Flask-WTF Prevents CSRF

Flask-WTF generates a unique token for each form:
1. Token is embedded in form (hidden field)
2. Token is stored in session
3. On submission, tokens are compared
4. Mismatch = form rejected

## Code Walkthrough

### Enabling CSRF Protection

```python
# app.py
from flask import Flask
from flask_wtf import FlaskForm

app = Flask(__name__)

# Required: Set secret key for CSRF
app.config["SECRET_KEY"] = "your-secret-key-here"

# CSRF is now enabled for all FlaskForm forms
```

### Using CSRF Token in Templates

```python
# forms.py
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField

class MyForm(FlaskForm):
    name = StringField("Name")
    submit = SubmitField("Submit")
```

```html
<!-- Template with CSRF token -->
<form method="POST">
    {{ form.hidden_tag() }}  <!-- Renders CSRF token -->
    
    {{ form.name.label }}
    {{ form.name() }}
    
    {{ form.submit() }}
</form>
```

### Custom CSRF Error Handling

```python
# app.py
from flask import Flask, render_template
from flask_wtf.csrf import CSRFError

app = Flask(__name__)
app.config["SECRET_KEY"] = "secret"

@app.errorhandler(CSRFError)
def handle_csrf_error(e):
    """Custom CSRF error page."""
    return render_template("csrf_error.html", reason=e.description), 400
```

### Configuring CSRF

```python
# app.py — CSRF configuration
app.config["WTF_CSRF_ENABLED"] = True  # Default
app.config["WTF_CSRF_TIME_LIMIT"] = None  # Token lifetime (None = session)
app.config["WTF_CSRF_HEADERS"] = ["X-CSRFToken"]  # For AJAX
```

### AJAX CSRF Token

```html
<!-- Include CSRF token in AJAX requests -->
<script>
    // Get token from meta tag
    const csrfToken = document.querySelector('meta[name="csrf-token"]').content;
    
    fetch("/api/data", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "X-CSRFToken": csrfToken
        },
        body: JSON.stringify({data: "value"})
    });
</script>
```

```python
# Template includes meta tag
<meta name="csrf-token" content="{{ csrf_token() }}">
```

## Common Mistakes

❌ **Not setting SECRET_KEY**
```python
# WRONG — CSRF won't work
app.config["SECRET_KEY"] = "dev"
# Or worse, no secret key at all
```

✅ **Correct — Set secret key**
```python
# CORRECT
app.config["SECRET_KEY"] = "your-secret-key"
```

❌ **Forgetting hidden_tag()**
```html
<!-- WRONG — CSRF token not included -->
<form>
    {{ form.name() }}
</form>
```

✅ **Correct — Include hidden_tag()**
```html
<!-- CORRECT -->
<form>
    {{ form.hidden_tag() }}
    {{ form.name() }}
</form>
```

## Quick Reference

| Setting | Description |
|---------|-------------|
| `SECRET_KEY` | Required for CSRF |
| `WTF_CSRF_ENABLED` | Enable/disable CSRF |
| `WTF_CSRF_TIME_LIMIT` | Token expiry |
| `form.hidden_tag()` | Renders CSRF token |

## Next Steps

Now you understand CSRF protection. Continue to [01_built_in_validators.md](../../04_forms_and_validation/03_validation/01_built_in_validators.md) to learn about validation in WTForms.