<!-- FILE: 07_blueprints_and_application_factory/01_blueprints/03_registering_blueprints.md -->

## Overview

This file covers registering blueprints with Flask applications and organizing blueprints in larger projects.

## Code Walkthrough

### Registering Blueprints

```python
# app.py
from flask import Flask
from main.routes import main_bp
from auth.routes import auth_bp
from blog.routes import blog_bp

app = Flask(__name__)

# Register blueprints with URL prefixes
app.register_blueprint(main_bp)
app.register_blueprint(auth_bp, url_prefix="/auth")
app.register_blueprint(blog_bp, url_prefix="/blog")
```

## Next Steps

Blueprints are registered. Continue to [01_what_is_app_factory.md](../02_application_factory/01_what_is_app_factory.md) to learn application factory pattern.