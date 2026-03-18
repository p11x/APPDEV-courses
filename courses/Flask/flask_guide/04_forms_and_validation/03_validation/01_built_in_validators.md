<!-- FILE: 04_forms_and_validation/03_validation/01_built_in_validators.md -->

## Overview

WTForms provides numerous built-in validators for common validation needs. This file covers the most useful validators: DataRequired, Length, Email, NumberRange, Regexp, URL, and others. Understanding these validators lets you create robust form validation without writing custom code.

## Prerequisites

- Flask-WTF installed
- Basic form class knowledge

## Core Concepts

### Validators Overview

Validators are passed to field's `validators` list:

```python
from wtforms.validators import DataRequired, Length, Email

class MyForm(FlaskForm):
    name = StringField("Name", validators=[DataRequired(), Length(min=2)])
```

### Common Validators

| Validator | Purpose |
|-----------|---------|
| `DataRequired` | Field must not be empty |
| `Length` | String length constraints |
| `Email` | Valid email format |
| `NumberRange` | Number within range |
| `Regexp` | Match regex pattern |
| `URL` | Valid URL |
| `Optional` | Field can be empty |
| `EqualTo` | Match another field |

## Code Walkthrough

### All Built-in Validators

```python
# forms.py — Comprehensive validator examples
from flask_wtf import FlaskForm
from wtforms import (
    StringField, IntegerField, FloatField,
    URLField, DateField, SubmitField
)
from wtforms.validators import (
    DataRequired, Length, Email, NumberRange,
    Regexp, URL, Optional, EqualTo, AnyOf, NoneOf
)

class CompleteValidationForm(FlaskForm):
    """Form demonstrating all common validators."""
    
    # Required field - cannot be empty
    required_field = StringField(
        "Required Field",
        validators=[DataRequired(message="This field is required")]
    )
    
    # Length constraints
    username = StringField(
        "Username",
        validators=[
            DataRequired(),
            Length(min=3, max=20, message="Must be 3-20 characters")
        ]
    )
    
    # Email validation
    email = StringField(
        "Email",
        validators=[
            DataRequired(),
            Email(message="Invalid email format")
        ]
    )
    
    # Number range
    age = IntegerField(
        "Age",
        validators=[
            DataRequired(),
            NumberRange(min=13, max=120, message="Must be between 13 and 120")
        ]
    )
    
    # Price with optional field
    price = FloatField(
        "Price",
        validators=[
            Optional(),  # Field is optional
            NumberRange(min=0.01, message="Price must be positive")
        ]
    )
    
    # Regex validation
    phone = StringField(
        "Phone",
        validators=[
            DataRequired(),
            Regexp(r'^\d{3}-\d{3}-\d{4}$', message="Format: 123-456-7890")
        ]
    )
    
    # URL validation
    website = URLField(
        "Website",
        validators=[URL(message="Invalid URL")]
    )
    
    # Match another field (for password confirmation)
    password = StringField("Password", validators=[DataRequired()])
    confirm_password = StringField(
        "Confirm Password",
        validators=[
            DataRequired(),
            EqualTo("password", message="Passwords must match")
        ]
    )
    
    # AnyOf - value must be in list
    role = StringField(
        "Role",
        validators=[
            DataRequired(),
            AnyOf(["admin", "user", "guest"], message="Invalid role")
        ]
    )
    
    # NoneOf - value must NOT be in list
    username_check = StringField(
        "Username",
        validators=[
            DataRequired(),
            NoneOf(["admin", "root", "system"], message="Username not allowed")
        ]
    )
    
    submit = SubmitField("Submit")
```

### Using with Custom Messages

```python
# Custom validation messages
class CustomMessagesForm(FlaskForm):
    email = StringField(
        "Email",
        validators=[
            DataRequired(message="Please enter your email"),
            Email(message="Please enter a valid email address")
        ]
    )
```

### Optional with Default

```python
# Optional field with default value
quantity = IntegerField(
    "Quantity",
    validators=[Optional()],
    default=1  # Default value if not provided
)
```

## Common Mistakes

❌ **Not importing validators**
```python
# WRONG — ImportError
from wtforms.validators import DataRequired
```

✅ **Correct — Import from wtforms.validators**
```python
# CORRECT
from wtforms.validators import DataRequired, Length
```

❌ **Empty validators list**
```python
# WRONG — No validation
username = StringField("Username")
```

✅ **Correct — Add validators**
```python
# CORRECT
username = StringField("Username", validators=[DataRequired(), Length(min=3)])
```

## Quick Reference

| Validator | Syntax |
|-----------|--------|
| Required | `DataRequired()` |
| Length | `Length(min=3, max=20)` |
| Email | `Email()` |
| Range | `NumberRange(min=0, max=100)` |
| Regex | `Regexp(r'pattern')` |
| URL | `URL()` |
| Optional | `Optional()` |
| Match | `EqualTo('other_field')` |

## Next Steps

Now you know built-in validators. Continue to [02_custom_validators.md](02_custom_validators.md) to learn how to create custom validators.