<!-- FILE: 07_blueprints_and_application_factory/02_application_factory/03_config_objects.md -->

## Overview

Configuration objects manage different environments (development, testing, production). This file covers creating configuration classes and using them with the application factory.

## Code Walkthrough

### Configuration Classes

```python
# config.py
class Config:
    """Base configuration."""
    SECRET_KEY = "secret"

class DevelopmentConfig(Config):
    """Development configuration."""
    DEBUG = True
    SQLALCHEMY_DATABASE_URI = "sqlite:///dev.db"

class ProductionConfig(Config):
    """Production configuration."""
    DEBUG = False
    SQLALCHEMY_DATABASE_URI = "postgresql://user:pass@localhost/prod"

config = {
    "development": DevelopmentConfig,
    "production": ProductionConfig,
    "default": DevelopmentConfig
}

# app/__init__.py
def create_app(config_name="development"):
    app = Flask(__name__)
    app.config.from_object(config[config_name])
    return app
```

## Next Steps

Now you can configure apps. Continue to [01_recommended_structure.md](../03_large_project_layout/01_recommended_structure.md) to learn project structure.