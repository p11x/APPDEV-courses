<!-- FILE: 15_flask_admin_panel/02_model_views/03_customizing_form_view.md -->

## Overview

Customize the form view in Flask-Admin.

## Code Walkthrough

```python
# custom_form.py
from flask_admin.contrib.sqla import ModelView
from wtforms import StringField
from flask_admin.form import BaseModelView

class UserAdmin(ModelView):
    # Form fields
    form_columns = ("username", "email", "role")
    
    # Form field overrides
    form_extra_fields = {
        "custom_field": StringField("Custom")
    }
    
    # Create/edit form customization
    form_args = {
        "username": {"label": "User Name", "validators": ["required"]}
    }
```
