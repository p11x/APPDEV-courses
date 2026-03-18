<!-- FILE: 07_blueprints_and_application_factory/02_application_factory/02_create_app_function.md -->

## Overview

This file covers creating a complete application factory function with extensions, blueprints, and configuration.

## Code Walkthrough

### Complete Factory

```python
# app/__init__.py
from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flask_login import LoginManager

db = SQLAlchemy()
login_manager = LoginManager()

def create_app(config_name="development"):
    """Application factory."""
    app = Flask(__name__)
    
    # Configuration
    app.config["SECRET_KEY"] = "secret"
    app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"
    
    # Initialize extensions
    db.init_app(app)
    login_manager.init_app(app)
    
    # Register blueprints
    from .main import main_bp
    app.register_blueprint(main_bp)
    
    # Create tables
    with app.app_context():
        db.create_all()
    
    return app
```

## Next Steps

Now you can create apps. Continue to [03_config_objects.md](03_config_objects.md) to learn configuration.