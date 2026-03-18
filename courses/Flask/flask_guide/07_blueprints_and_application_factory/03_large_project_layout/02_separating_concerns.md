<!-- FILE: 07_blueprints_and_application_factory/03_large_project_layout/02_separating_concerns.md -->

## Overview

Separating concerns keeps code maintainable. This file covers organizing code by feature: routes, models, forms, and services.

## Code Walkthrough

### Separation Example

```python
# app/routes/auth.py
auth_bp = Blueprint("auth", __name__)

@auth_bp.route("/login")
def login():
    return "Login"

# app/models.py
class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    # ...

# app/forms.py
class LoginForm(FlaskForm):
    # ...
```

## Next Steps

Now separate concerns. Continue to [03_environment_configs.md](03_environment_configs.md) to learn environment configs.