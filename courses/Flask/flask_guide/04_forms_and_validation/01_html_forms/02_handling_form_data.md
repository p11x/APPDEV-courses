<!-- FILE: 04_forms_and_validation/01_html_forms/02_handling_form_data.md -->

## Overview

Processing form data properly is essential for security and user experience. This file covers best practices for handling form submissions: validating input, displaying errors, using flash messages, and preventing duplicate submissions with redirects.

## Prerequisites

- Understanding of HTML forms
- Basic Flask routing and request handling
- Familiarity with templates

## Core Concepts

### Form Processing Pattern

1. **Display form** (GET) — Show empty or pre-filled form
2. **Validate submission** (POST) — Check required fields
3. **On error** — Show form with errors
4. **On success** — Process data and redirect

### Flash Messages

Flask's flash system stores messages across requests:

```python
flash("Error message", "error")
```

### Redirect After Post

The **Post/Redirect/Get (PRG)** pattern prevents duplicate submissions:

```python
@app.route("/submit", methods=["POST"])
def submit():
    # Process data
    return redirect(url_for("success"))
```

## Code Walkthrough

### Complete Form Handling Example

```python
# app.py — Complete form handling
from flask import Flask, request, render_template_string, redirect, url_for, flash

app = Flask(__name__)
app.secret_key = "secret-key-for-flash-messages"

# In-memory storage
users = []

@app.route("/register", methods=["GET", "POST"])
def register():
    """Registration form with validation."""
    
    if request.method == "POST":
        # Get form data
        username = request.form.get("username", "").strip()
        email = request.form.get("email", "").strip()
        password = request.form.get("password", "")
        confirm = request.form.get("confirm", "")
        
        # Collect errors
        errors = {}
        
        # Validate username
        if not username:
            errors["username"] = "Username is required"
        elif len(username) < 3:
            errors["username"] = "Username must be at least 3 characters"
        elif any(u["username"] == username for u in users):
            errors["username"] = "Username already taken"
        
        # Validate email
        if not email:
            errors["email"] = "Email is required"
        elif "@" not in email:
            errors["email"] = "Invalid email format"
        
        # Validate password
        if not password:
            errors["password"] = "Password is required"
        elif len(password) < 6:
            errors["password"] = "Password must be at least 6 characters"
        
        # Validate confirm
        if password != confirm:
            errors["confirm"] = "Passwords do not match"
        
        # If errors, show form with errors
        if errors:
            for error in errors.values():
                flash(error, "error")
            return render_template_string(TEMPLATE, 
                username=username, 
                email=email,
                errors=errors
            )
        
        # Success - save user and redirect
        users.append({
            "username": username,
            "email": email,
            "password": password  # Note: In production, hash passwords!
        })
        
        flash(f"Welcome, {username}! Registration successful.", "success")
        return redirect(url_for("register_success", username=username))
    
    # GET request - show form
    return render_template_string(TEMPLATE)

@app.route("/register/success")
def register_success():
    """Success page after registration."""
    username = request.args.get("username", "")
    return f"<h1>Welcome, {username}!</h1><p>Registration complete!</p>"

# HTML Template
TEMPLATE = '''
<!DOCTYPE html>
<html>
<head><title>Register</title></head>
<body>
    <h1>Register</h1>
    
    {% with messages = get_flashed_messages(with_categories=true) %}
        {% if messages %}
            <div class="messages">
            {% for category, message in messages %}
                <div class="alert {{ category }}">{{ message }}</div>
            {% endfor %}
            </div>
        {% endif %}
    {% endwith %}
    
    <form method="POST">
        <div>
            <label>Username:</label>
            <input type="text" name="username" value="{{ username or '' }}">
            {% if errors.username %}<span class="error">{{ errors.username }}</span>{% endif %}
        </div>
        
        <div>
            <label>Email:</label>
            <input type="email" name="email" value="{{ email or '' }}">
            {% if errors.email %}<span class="error">{{ errors.email }}</span>{% endif %}
        </div>
        
        <div>
            <label>Password:</label>
            <input type="password" name="password">
            {% if errors.password %}<span class="error">{{ errors.password }}</span>{% endif %}
        </div>
        
        <div>
            <label>Confirm Password:</label>
            <input type="password" name="confirm">
            {% if errors.confirm %}<span class="error">{{ errors.confirm }}</span>{% endif %}
        </div>
        
        <button type="submit">Register</button>
    </form>
</body>
</html>
'''

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `request.form.get("username", "").strip()` — Gets value with default, removes whitespace.
- `errors = {}` — Dictionary to collect all validation errors.
- `flash(error, "error")` — Stores error message to display on next request.
- `return render_template_string(TEMPLATE, errors=errors)` — Re-renders form with errors.
- `return redirect(url_for("success"))` — Redirects after successful submission (PRG pattern).

## Common Mistakes

❌ **Not using redirect after POST**
```python
# WRONG — User can resubmit by refreshing
@app.route("/submit", methods=["POST"])
def submit():
    return "Submitted!"
```

✅ **Correct — Use redirect after POST**
```python
# CORRECT — Prevents duplicate submission
@app.route("/submit", methods=["POST"])
def submit():
    return redirect(url_for("success"))
```

❌ **Not escaping user input**
```python
# WRONG — XSS vulnerability
return f"<h1>Hello, {username}</h1>"
```

✅ **Correct — Jinja2 auto-escapes by default**
```html
<!-- CORRECT — Jinja2 escapes automatically -->
<h1>Hello, {{ username }}</h1>
```

## Quick Reference

| Task | Code |
|------|------|
| Get form value | `request.form.get("field")` |
| Flash message | `flash("msg", "error")` |
| Display flashes | `get_flashed_messages()` |
| Redirect | `redirect(url_for("route"))` |

## Next Steps

Now you can handle form data. Continue to [03_file_uploads.md](03_file_uploads.md) to learn how to handle file uploads in Flask.