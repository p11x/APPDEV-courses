<!-- FILE: 04_forms_and_validation/02_wtforms/01_installing_flask_wtf.md -->

## Overview

**Flask-WTF** is an extension that simplifies form handling in Flask. It provides form classes that define fields with built-in validation, automatic CSRF protection, and easy rendering in templates. This file covers installing Flask-WTF and setting up your first form.

## Prerequisites

- Understanding of HTML forms
- Basic Flask application
- Familiarity with pip and virtual environments

## Core Concepts

### Why Flask-WTF?

Pure HTML forms require manual validation. Flask-WTF provides:
- Form field classes
- Built-in validators
- CSRF protection automatically
- Easy template rendering

### Installing Flask-WTF

```bash
pip install flask-wtf
```

## Code Walkthrough

### Installation

```bash
# Activate virtual environment first
source venv/bin/activate  # or venv\Scripts\activate on Windows

# Install Flask-WTF
pip install flask-wtf
```

### Basic Flask-WTF Form

```python
# forms.py — Flask-WTF form definitions
from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, SubmitField
from wtforms.validators import DataRequired, Length, Email

class LoginForm(FlaskForm):
    """Login form with validation."""
    
    username = StringField(
        "Username",
        validators=[DataRequired(), Length(min=3, max=20)]
    )
    password = PasswordField(
        "Password",
        validators=[DataRequired(), Length(min=6)]
    )
    submit = SubmitField("Login")
```

### Using Form in View

```python
# app.py — Using Flask-WTF form
from flask import Flask, render_template_string
from forms import LoginForm

app = Flask(__name__)
app.config["SECRET_KEY"] = "your-secret-key"

@app.route("/login", methods=["GET", "POST"])
def login():
    form = LoginForm()
    
    if form.validate_on_submit():
        username = form.username.data
        password = form.password.data
        # Process login...
        return f"Logged in as {username}!"
    
    return render_template_string('''
<!DOCTYPE html>
<html>
<body>
    <h1>Login</h1>
    <form method="POST">
        {{ form.hidden_tag() }}
        <div>
            {{ form.username.label }}<br>
            {{ form.username() }}
            {% if form.username.errors %}
                <span>{{ form.username.errors[0] }}</span>
            {% endif %}
        </div>
        <div>
            {{ form.password.label }}<br>
            {{ form.password() }}
            {% if form.password.errors %}
                <span>{{ form.password.errors[0] }}</span>
            {% endif %}
        </div>
        <div>{{ form.submit() }}</div>
    </form>
</body>
</html>
    ''', form=form)

if __name__ == "__main__":
    app.run(debug=True)
```

### Line-by-Line Breakdown

- `FlaskForm` — Base class for all Flask-WTF forms.
- `StringField`, `PasswordField`, `SubmitField` — Form field types.
- `validators` — List of validators for the field.
- `form.hidden_tag()` — Renders CSRF token field.
- `form.validate_on_submit()` — Validates form on POST request.

## Quick Reference

| Class | Description |
|-------|-------------|
| `FlaskForm` | Base form class |
| `StringField` | Text input |
| `PasswordField` | Password input |
| `SubmitField` | Submit button |
| `DataRequired` | Field required validator |

## Next Steps

Now that Flask-WTF is installed, continue to [02_creating_form_classes.md](02_creating_form_classes.md) to learn how to create more complex form classes.