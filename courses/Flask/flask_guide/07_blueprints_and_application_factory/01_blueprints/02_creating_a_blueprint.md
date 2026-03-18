<!-- FILE: 07_blueprints_and_application_factory/01_blueprints/02_creating_a_blueprint.md -->

## Overview

This file covers creating blueprints with routes, templates, and static files organized by feature.

## Code Walkthrough

### Creating a Blueprint

```python
# main/routes.py
from flask import Blueprint, render_template

main_bp = Blueprint("main", __name__)

@main_bp.route("/")
def index():
    return render_template("main/index.html")

@main_bp.route("/about")
def about():
    return "About page"
```

## Next Steps

Now create blueprints. Continue to [03_registering_blueprints.md](03_registering_blueprints.md) to learn registering them.