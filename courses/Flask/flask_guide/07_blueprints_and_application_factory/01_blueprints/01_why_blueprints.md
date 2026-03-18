<!-- FILE: 07_blueprints_and_application_factory/01_blueprints/01_why_blueprints.md -->

## Overview

**Blueprints** organize Flask applications into modular components. Instead of defining all routes in one file, you create blueprints for different features. This file explains why blueprints are useful and when to use them.

## Core Concepts

### Why Blueprints?

Blueprints let you:
- Organize code by feature
- Register routes dynamically
- Create reusable components
- Scale to large applications

## Code Walkthrough

### Blueprint Example

```python
# auth/routes.py — Blueprint for authentication
from flask import Blueprint

auth_bp = Blueprint("auth", __name__)

@auth_bp.route("/login")
def login():
    return "Login page"

@auth_bp.route("/register")
def register():
    return "Register page"

# app.py — Register blueprint
from auth.routes import auth_bp

app.register_blueprint(auth_bp, url_prefix="/auth")
```

## Next Steps

Now you understand blueprints. Continue to [02_creating_a_blueprint.md](02_creating_a_blueprint.md) to create blueprints.