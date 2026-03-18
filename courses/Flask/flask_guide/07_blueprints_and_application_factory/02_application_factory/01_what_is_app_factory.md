<!-- FILE: 07_blueprints_and_application_factory/02_application_factory/01_what_is_app_factory.md -->

## Overview

The **Application Factory** pattern creates Flask apps programmatically. Instead of creating the app at module level, you use a function to configure and return the app. This enables testing, multiple configurations, and better organization.

## Core Concepts

### Why Use Factory?

- Multiple configurations (dev, prod, test)
- Easier testing with different settings
- Extension initialization patterns

## Code Walkthrough

### Factory Pattern

```python
# app.py
from flask import Flask

def create_app(config_name="development"):
    """Application factory function."""
    app = Flask(__name__)
    
    # Load configuration
    if config_name == "development":
        app.config["DEBUG"] = True
    elif config_name == "production":
        app.config["DEBUG"] = False
    
    # Register routes
    @app.route("/")
    def index():
        return "Hello!"
    
    return app

# Usage
app = create_app("development")
```

## Next Steps

Now you understand factory. Continue to [02_create_app_function.md](02_create_app_function.md) to learn more.