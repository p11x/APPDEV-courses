<!-- FILE: 04_forms_and_validation/03_validation/03_displaying_errors.md -->

## Overview

After validating form data, you need to display errors to users. This file covers displaying validation errors in Jinja2 templates: showing field errors, displaying all errors, and styling error messages for better UX.

## Prerequisites

- Flask-WTF form knowledge
- Template rendering knowledge

## Core Concepts

### Error Access

Form errors are available via:
- `form.field.errors` — List of error messages for a field
- `form.errors` — Dictionary of all errors

### Displaying Errors

```html
{% for error in form.field.errors %}
    <span class="error">{{ error }}</span>
{% endfor %}
```

## Code Walkthrough

### Complete Error Display Examples

```python# app.py
from flask import Flask, render_template_string
from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, SubmitField
from wtforms.validators import DataRequired, Length, Email

app = Flask(__name__)
app.config["SECRET_KEY"] = "secret"

class LoginForm(FlaskForm):
    username = StringField("Username", validators=[DataRequired(), Length(min=3)])
    password = PasswordField("Password", validators=[DataRequired()])
    submit = SubmitField("Login")

@app.route("/login", methods=["GET", "POST"])
def login():
    form = LoginForm()
    
    if form.validate_on_submit():
        return f"Logged in as {form.username.data}!"
    
    return render_template_string(TEMPLATE, form=form)

TEMPLATE = '''
<!DOCTYPE html>
<html>
<head>
    <title>Login</title>
    <style>
        .error { color: red; font-size: 0.9em; }
        .form-group { margin-bottom: 15px; }
        label { display: block; margin-bottom: 5px; }
        input { padding: 8px; width: 200px; }
        input.error { border: 2px solid red; }
    </style>
</head>
<body>
    <h1>Login</h1>
    
    <form method="POST">
        {{ form.hidden_tag() }}
        
        <!-- Method 1: Individual field with error styling -->
        <div class="form-group">
            {{ form.username.label }}
            {{ form.username(class="error" if form.username.errors else "") }}
            {% for error in form.username.errors %}
                <span class="error">{{ error }}</span>
            {% endfor %}
        </div>
        
        <!-- Method 2: Simple error display -->
        <div class="form-group">
            {{ form.password.label }}
            {{ form.password() }}
            {% if form.password.errors %}
                <div class="error">
                    {% for error in form.password.errors %}
                        {{ error }}<br>
                    {% endfor %}
                </div>
            {% endif %}
        </div>
        
        <!-- Method 3: First error only -->
        <div class="form-group">
            {{ form.submit() }}
        </div>
        
        <!-- Display all errors at top -->
        {% if form.errors %}
            <div style="background: #fee; padding: 10px; margin-bottom: 20px;">
                <strong>Please correct the following errors:</strong>
                <ul>
                {% for field, errors in form.errors.items() %}
                    {% for error in errors %}
                        <li>{{ form[field].label.text }}: {{ error }}</li>
                    {% endfor %}
                {% endfor %}
                </ul>
            </div>
        {% endif %}
    </form>
</body>
</html>
'''
```

### Bootstrap-Style Error Display

```html
<!-- Bootstrap-style form errors -->
{% macro render_field(field) %}
    <div class="form-group">
        {{ field.label(class="form-label") }}
        {% if field.errors %}
            {{ field(class="form-control is-invalid") }}
            <div class="invalid-feedback">
                {% for error in field.errors %}
                    {{ error }}
                {% endfor %}
            </div>
        {% else %}
            {{ field(class="form-control") }}
        {% endif %}
    </div>
{% endmacro %}

<!-- Usage -->
<form method="POST">
    {{ form.hidden_tag() }}
    {{ render_field(form.username) }}
    {{ render_field(form.password) }}
    {{ form.submit(class="btn btn-primary") }}
</form>
```

### Reusable Form Macro

```html
<!-- templates/macros/forms.html -->
{% macro render_form_field(field, placeholder="") %}
    <div class="form-group">
        {{ field.label(class="form-label") }}
        
        {% if field.errors %}
            {{ field(class="form-control is-invalid", placeholder=placeholder) }}
            <div class="invalid-feedback">
                {% for error in field.errors %}
                    {{ error }}
                {% endfor %}
            </div>
        {% else %}
            {{ field(class="form-control", placeholder=placeholder) }}
        {% endif %}
    </div>
{% endmacro %}

{% macro render_form(form) %}
    {{ form.hidden_tag() }}
    {{ form.csrf_token }}  <!-- Alternative -->
    
    {% for field in form %}
        {% if field.type != "SubmitField" %}
            {{ render_form_field(field) }}
        {% endif %}
    {% endfor %}
    
    {% for field in form %}
        {% if field.type == "SubmitField" %}
            {{ field(class="btn btn-primary") }}
        {% endif %}
    {% endfor %}
{% endmacro %}
```

## Common Mistakes

❌ **Not checking if errors exist**
```html
<!-- WRONG — Will show empty list -->
{{ form.username.errors }}
```

✅ **Correct — Check first**
```html
<!-- CORRECT -->
{% if form.username.errors %}
    {% for error in form.username.errors %}
        {{ error }}
    {% endfor %}
{% endif %}
```

❌ **Not including hidden_tag**
```html
<!-- WRONG — CSRF token missing -->
<form>
    {{ form.username() }}
</form>
```

## Quick Reference

| Task | Code |
|------|------|
| Field errors | `form.field.errors` |
| All errors | `form.errors` |
| First error | `form.field.errors[0]` |
| Has errors | `form.field.errors` (truthy) |

## Next Steps

You have completed the forms and validation chapter. Continue to [01_what_is_an_orm.md](../../05_databases/01_sqlalchemy_basics/01_what_is_an_orm.md) to learn about databases and SQLAlchemy.