<!-- FILE: 04_forms_and_validation/03_validation/02_custom_validators.md -->

## Overview

Sometimes built-in validators aren't enough. WTForms allows creating custom validators to handle complex validation logic. This file shows how to create field-level validators and form-level validation methods for custom business rules.

## Prerequisites

- Understanding of built-in validators
- Basic form class knowledge
- Python function knowledge

## Core Concepts

### Types of Custom Validation

1. **Field-level validator** — Function that validates a single field
2. **Form-level validator** — Method that validates entire form

### Validator Function Signature

```python
def validate_field_name(form, field):
    # form — The form instance
    # field — The field being validated
    if invalid:
        raise ValidationError("Error message")
```

## Code Walkthrough

### Field-Level Custom Validator

```python
# forms.py — Custom field validators
from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, SubmitField
from wtforms.validators import ValidationError, DataRequired

def validate_username(form, field):
    """Custom validator: username must not contain 'admin'."""
    if "admin" in field.data.lower():
        raise ValidationError("Username cannot contain 'admin'")

def validate_unique_username(form, field):
    """Custom validator: check against database."""
    # Simulate database check
    taken_usernames = ["alice", "bob", "charlie"]
    if field.data.lower() in taken_usernames:
        raise ValidationError("This username is already taken")

class CustomFieldForm(FlaskForm):
    """Form with custom field validators."""
    
    username = StringField(
        "Username",
        validators=[
            DataRequired(),
            validate_username,  # Custom validator function
            validate_unique_username
        ]
    )
    
    submit = SubmitField("Submit")
```

### Form-Level Validator

```python
# Form-level validator (method)
class RegistrationForm(FlaskForm):
    """Form with form-level validation."""
    
    username = StringField("Username", validators=[DataRequired()])
    password = PasswordField("Password", validators=[DataRequired()])
    confirm_password = PasswordField("Confirm Password", validators=[DataRequired()])
    submit = SubmitField("Register")
    
    # Form-level validator (method name must start with validate_)
    def validate_username(self, field):
        """Check if username is allowed."""
        forbidden = ["admin", "root", "system"]
        if field.data.lower() in forbidden:
            raise ValidationError("This username is not allowed")
    
    def validate(self):
        """Validate entire form."""
        # Call default validators first
        if not super().validate():
            return False
        
        # Custom cross-field validation
        if self.password.data != self.confirm_password.data:
            self.confirm_password.errors.append("Passwords must match")
            return False
        
        return True
```

### Complex Validation Example

```python
# Complex form validation
class OrderForm(FlaskForm):
    """Order form with complex validation."""
    
    product_id = StringField("Product ID", validators=[DataRequired()])
    quantity = StringField("Quantity", validators=[DataRequired()])
    shipping_method = StringField("Shipping", validators=[DataRequired()])
    submit = SubmitField("Place Order")
    
    def validate_quantity(self, field):
        """Quantity must be positive integer."""
        try:
            qty = int(field.data)
            if qty <= 0:
                raise ValidationError("Quantity must be positive")
            if qty > 100:
                raise ValidationError("Maximum quantity is 100")
        except ValueError:
            raise ValidationError("Must be a valid number")
    
    def validate(self):
        """Complex form-level validation."""
        if not super().validate():
            return False
        
        # Check shipping method based on quantity
        try:
            qty = int(self.quantity.data)
            if qty > 10 and self.shipping_method.data == "standard":
                self.shipping_method.errors.append(
                    "Standard shipping not available for orders over 10 items"
                )
                return False
        except ValueError:
            pass
        
        return True
```

## Common Mistakes

❌ **Wrong validator function signature**
```python
# WRONG — Wrong parameters
def validate_username(username):
    if "admin" in username:
        raise ValidationError("Bad username")
```

✅ **Correct — Use form and field parameters**
```python
# CORRECT
def validate_username(form, field):
    if "admin" in field.data:
        raise ValidationError("Bad username")
```

❌ **Not returning False in form validator**
```python
# WRONG — Forgot to return False
def validate(self):
    if error:
        raise ValidationError("Error")  # This is wrong in validate()
```

## Quick Reference

| Type | Syntax |
|------|--------|
| Field validator | `def validate_fieldname(form, field):` |
| Form validator | `def validate(self):` |
| Raise error | `raise ValidationError("message")` |

## Next Steps

Now you can create custom validators. Continue to [03_displaying_errors.md](03_displaying_errors.md) to learn how to display validation errors in templates.