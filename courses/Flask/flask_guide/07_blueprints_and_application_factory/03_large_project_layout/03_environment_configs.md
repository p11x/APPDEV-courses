<!-- FILE: 07_blueprints_and_application_factory/03_large_project_layout/03_environment_configs.md -->

## Overview

Environment-based configuration manages secrets and settings for different deployment environments.

## Code Walkthrough

### Environment Config

```python
# config.py
import os

class Config:
    SECRET_KEY = os.environ.get("SECRET_KEY") or "dev-secret"
    
class DevelopmentConfig(Config):
    DEBUG = True
    SQLALCHEMY_DATABASE_URI = "sqlite:///dev.db"

class ProductionConfig(Config):
    DEBUG = False
    SQLALCHEMY_DATABASE_URI = os.environ.get("DATABASE_URL")
```

## Next Steps

You have completed blueprints. Continue to [01_what_is_rest.md](../08_rest_apis/01_api_basics/01_what_is_rest.md) to learn about REST APIs.