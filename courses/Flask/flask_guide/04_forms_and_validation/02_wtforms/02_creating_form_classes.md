<!-- FILE: 04_forms_and_validation/02_wtforms/02_creating_form_classes.md -->

## Overview

Flask-WTF form classes define form structure, fields, and validation in one place. This file covers creating comprehensive form classes with various field types, custom validators, and best practices for organizing forms.

## Prerequisites

- Flask-WTF installed
- Basic understanding of form fields

## Core Concepts

### Form Field Types

WTForms provides many field types:
- `StringField`, `PasswordField`, `IntegerField`
- `BooleanField`, `SelectField`, `TextAreaField`
- `DateField`, `FileField`, `HiddenField`

### Validators

Common validators:
- `DataRequired` — Field cannot be empty
- `Length` — String length constraints
- `Email` — Valid email format
- `NumberRange` — Numeric range
- `Regexp` — Regular expression match
- `URL` — Valid URL

## Code Walkthrough

### Complete Form Example

```python
# forms.py — Comprehensive form classes
from flask_wtf import FlaskForm
from wtforms import (
    StringField, PasswordField, IntegerField, 
    BooleanField, SelectField, TextAreaField,
    DateField, FileField, SubmitField
)
from wtforms.validators import (
    DataRequired, Length, Email, EqualTo,
    NumberRange, Optional, Regexp
)

class RegistrationForm(FlaskForm):
    """User registration form."""
    
    username = StringField(
        "Username",
        validators=[
            DataRequired(message="Username is required"),
            Length(min=3, max=20, message="Username must be 3-20 characters"),
            Regexp(r'^[a-zA-Z0-9_]+$', message="Username can only contain letters, numbers, and underscores")
        ]
    )
    
    email = StringField(
        "Email",
        validators=[
            DataRequired(),
            Email(message="Invalid email address")
        ]
    )
    
    password = PasswordField(
        "Password",
        validators=[
            DataRequired(),
            Length(min=6, message="Password must be at least 6 characters")
        ]
    )
    
    confirm_password = PasswordField(
        "Confirm Password",
        validators=[
            DataRequired(),
            EqualTo("password", message="Passwords must match")
        ]
    )
    
    age = IntegerField(
        "Age",
        validators=[NumberRange(min=13, max=120)]
    )
    
    agree_tos = BooleanField(
        "I agree to the Terms of Service",
        validators=[DataRequired(message="You must agree to the Terms")]
    )
    
    submit = SubmitField("Register")

class ProfileForm(FlaskForm):
    """User profile edit form."""
    
    bio = TextAreaField(
        "Bio",
        validators=[Length(max=500)]
    )
    
    location = StringField(
        "Location",
        validators=[Length(max=100)]
    )
    
    birth_date = DateField(
        "Birth Date",
        validators=[Optional()],
        format="%Y-%m-%d"
    )
    
    avatar = FileField(
        "Avatar Image",
        validators=[]
    )
    
    submit = SubmitField("Update Profile")

class SearchForm(FlaskForm):
    """Search form with SelectField."""
    
    category = SelectField(
        "Category",
        choices=[
            ("all", "All Categories"),
            ("electronics", "Electronics"),
            ("clothing", "Clothing"),
            ("books", "Books")
        ]
    )
    
    query = StringField(
        "Search",
        validators=[DataRequired(), Length(min=1, max=100)]
    )
    
    min_price = IntegerField(
        "Min Price",
        validators=[Optional(), NumberRange(min=0)]
    )
    
    max_price = IntegerField(
        "Max Price",
        validators=[Optional(), NumberRange(min=0)]
    )
    
    submit = SubmitField("Search")
```

### Using the Forms

```python
# app.py
from flask import Flask, render_template_string, flash, redirect, url_for
from forms import RegistrationForm

app = Flask(__name__)
app.config["SECRET_KEY"] = "your-secret-key"

@app.route("/register", methods=["GET", "POST"])
def register():
    form = RegistrationForm()
    
    if form.validate_on_submit():
        # Access form data
        username = form.username.data
        email = form.email.data
        password = form.password.data
        age = form.age.data
        
        flash(f"Welcome, {username}!", "success")
        return redirect(url_for("index"))
    
    return render_template_string(TEMPLATE, form=form)

TEMPLATE = '''
<!DOCTYPE html>
<html>
<body>
    <h1>Register</h1>
    <form method="POST">
        {{ form.hidden_tag() }}
        
        <div>
            {{ form.username.label }}<br>
            {{ form.username(size=30) }}
            {% for error in form.username.errors %}
                <span style="color:red">{{ error }}</span>
            {% endfor %}
        </div>
        
        <div>
            {{ form.email.label }}<br>
            {{ form.email(size=30) }}
            {% for error in form.email.errors %}
                <span style="color:red">{{ error }}</span>
            {% endfor %}
        </div>
        
        <div>
            {{ form.password.label }}<br>
            {{ form.password(size=30) }}
            {% for error in form.password.errors %}
                <span style="color:red">{{ error }}</span>
            {% endfor %}
        </div>
        
        <div>
            {{ form.confirm_password.label }}<br>
            {{ form.confirm_password(size=30) }}
            {% for error in form.confirm_password.errors %}
                <span style="color:red">{{ error }}</span>
            {% endfor %}
        </div>
        
        <div>
            {{ form.age.label }}<br>
            {{ form.age() }}
            {% for error in form.age.errors %}
                <span style="color:red">{{ error }}</span>
            {% endfor %}
        </div>
        
        <div>
            {{ form.agree_tos() }} {{ form.agree_tos.label }}
            {% for error in form.agree_tos.errors %}
                <span style="color:red">{{ error }}</span>
            {% endfor %}
        </div>
        
        <div>{{ form.submit() }}</div>
    </form>
</body>
</html>
'''

if __name__ == "__main__":
    app.run(debug=True)
```

## Common Mistakes

❌ **Not including form.hidden_tag()**
```html
<!-- WRONG — CSRF token missing -->
<form>
    {{ form.username() }}
</form>
```

✅ **Correct — Include hidden tag**
```html
<!-- CORRECT -->
<form>
    {{ form.hidden_tag() }}
    {{ form.username() }}
</form>
```

## Quick Reference

| Field | Use For |
|-------|---------|
| `StringField` | Text input |
| `PasswordField` | Password input |
| `TextAreaField` | Multi-line text |
| `IntegerField` | Numbers |
| `BooleanField` | Checkbox |
| `SelectField` | Dropdown |
| `DateField` | Date picker |
| `FileField` | File upload |

## Next Steps

Now you can create form classes. Continue to [03_csrf_protection.md](03_csrf_protection.md) to learn about CSRF protection.