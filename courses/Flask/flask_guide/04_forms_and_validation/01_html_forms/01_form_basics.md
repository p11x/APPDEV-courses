<!-- FILE: 04_forms_and_validation/01_html_forms/01_form_basics.md -->

## Overview

HTML forms are the primary way users submit data to web applications. Flask can handle form submissions directly using the `request.form` object. This file covers the fundamentals of HTML forms: form structure, input types, and how Flask processes form data.

## Prerequisites

- Basic Flask knowledge
- Understanding of HTTP methods (GET/POST)
- Familiarity with HTML

## Core Concepts

### HTML Form Structure

```html
<form action="/submit" method="POST">
    <input type="text" name="username" required>
    <input type="email" name="email" required>
    <button type="submit">Submit</button>
</form>
```

### Form Attributes

- `action` — URL to send data to
- `method` — HTTP method (GET or POST)
- `enctype` — Data encoding type for file uploads

### Common Input Types

| Type | Description |
|------|-------------|
| `text` | Single-line text |
| `password` | Hidden text |
| `email` | Email input |
| `number` | Number input |
| `checkbox` | Boolean checkbox |
| `radio` | Single selection |
| `submit` | Submit button |

## Code Walkthrough

### Basic Form Handling

```python
# app.py — Basic form handling
from flask import Flask, request, render_template_string

app = Flask(__name__)

@app.route("/")
def index():
    """Show the form."""
    return render_template_string('''
<!DOCTYPE html>
<html>
<head><title>Contact Form</title></head>
<body>
    <h1>Contact Us</h1>
    <form action="{{ url_for('submit_contact') }}" method="POST">
        <div>
            <label>Name:</label>
            <input type="text" name="name" required>
        </div>
        <div>
            <label>Email:</label>
            <input type="email" name="email" required>
        </div>
        <div>
            <label>Message:</label>
            <textarea name="message" rows="4" required></textarea>
        </div>
        <button type="submit">Send</button>
    </form>
</body>
</html>
    ''')

@app.route("/submit", methods=["POST"])
def submit_contact():
    """Handle form submission."""
    # Get form data from request
    name = request.form.get("name", "").strip()
    email = request.form.get("email", "").strip()
    message = request.form.get("message", "").strip()
    
    # Basic validation
    errors = []
    if not name:
        errors.append("Name is required")
    if not email or "@" not in email:
        errors.append("Valid email is required")
    if not message:
        errors.append("Message is required")
    
    if errors:
        return f"<h1>Errors:</h1><ul>{''.join(f'<li>{e}</li>' for e in errors)}</ul>"
    
    # Process the data (e.g., save to database)
    return f"<h1>Thank you, {name}!</h1><p>We'll contact you at {email}.</p>"

if __name__ == "__main__":
    app.run(debug=True)
```

### GET Form vs POST Form

```python
# app.py — GET and POST forms
from flask import Flask, request, render_template_string, redirect, url_for

app = Flask(__name__)

# GET form - form data in URL query string
@app.route("/search")
def search():
    """Show search form (GET)."""
    query = request.args.get("q", "")
    return render_template_string('''
<!DOCTYPE html>
<html>
<body>
    <form action="{{ url_for('search') }}" method="GET">
        <input type="text" name="q" value="{{ query }}" placeholder="Search...">
        <button type="submit">Search</button>
    </form>
    {% if query %}
    <p>You searched for: {{ query }}</p>
    {% endif %}
</body>
</html>
    ''', query=query)

# POST form - form data in request body
@app.route("/register", methods=["GET", "POST"])
def register():
    """Show form (GET) or process (POST)."""
    if request.method == "POST":
        username = request.form.get("username")
        password = request.form.get("password")
        return f"<h1>Registered: {username}</h1>"
    
    return render_template_string('''
<!DOCTYPE html>
<html>
<body>
    <h1>Register</h1>
    <form action="{{ url_for('register') }}" method="POST">
        <input type="text" name="username" placeholder="Username" required>
        <input type="password" name="password" placeholder="Password" required>
        <button type="submit">Register</button>
    </form>
</body>
</html>
    ''')

if __name__ == "__main__":
    app.run(debug=True)
```

### Form with Checkboxes and Radio Buttons

```python
# app.py — Form with checkboxes and radio
from flask import Flask, request, render_template_string

app = Flask(__name__)

@app.route("/survey", methods=["GET", "POST"])
def survey():
    """Survey form with checkboxes and radio buttons."""
    if request.method == "POST":
        name = request.form.get("name")
        language = request.form.get("language")
        newsletter = request.form.get("newsletter") == "on"
        
        return f'''
        <h1>Thank you, {name}!</h1>
        <p>Favorite language: {language}</p>
        <p>Newsletter: {"Yes" if newsletter else "No"}</p>
        '''
    
    return render_template_string('''
<!DOCTYPE html>
<html>
<body>
    <h1>Survey</h1>
    <form method="POST">
        <div>
            <label>Name: <input type="text" name="name" required></label>
        </div>
        <div>
            <p>Favorite Language:</p>
            <label><input type="radio" name="language" value="python"> Python</label>
            <label><input type="radio" name="language" value="javascript"> JavaScript</label>
            <label><input type="radio" name="language" value="java"> Java</label>
        </div>
        <div>
            <label><input type="checkbox" name="newsletter"> Subscribe to newsletter</label>
        </div>
        <button type="submit">Submit</button>
    </form>
</body>
</html>
    ''')

if __name__ == "__main__":
    app.run(debug=True)
```

## Common Mistakes

❌ **Not specifying method**
```html
<!-- WRONG — Defaults to GET, not POST -->
<form action="/submit">
```

✅ **Correct — Specify POST for sensitive data**
```html
<!-- CORRECT -->
<form action="/submit" method="POST">
```

❌ **Not validating form data**
```python
# WRONG — Blindly trusting user input
@app.route("/submit", methods=["POST"])
def submit():
    name = request.form["name"]  # Could be empty or malicious
```

✅ **Correct — Always validate**
```python
# CORRECT
@app.route("/submit", methods=["POST"])
def submit():
    name = request.form.get("name", "").strip()
    if not name:
        return "Name is required", 400
```

## Quick Reference

| Attribute | Description |
|-----------|-------------|
| `method="POST"` | Send data in request body |
| `method="GET"` | Send data in URL |
| `name="field"` | Key for accessing in Flask |
| `required` | HTML5 validation |
| `value="x"` | Default value |

## Next Steps

Now you understand form basics. Continue to [02_handling_form_data.md](02_handling_form_data.md) to learn more about processing form submissions.