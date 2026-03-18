<!-- FILE: 07_authentication/02_flask_login/01_installing_flask_login.md -->

## Overview

**Flask-Login** handles user authentication in Flask. It manages login/logout, session management, and user loading. This file covers installation and basic setup.

## Prerequisites

- Flask-SQLAlchemy installed

## Core Concepts

Flask-Login provides:
- `login_user()` / `logout_user()`
- `@login_required` decorator
- `current_user` object

## Code Walkthrough

### Installation

```bash
pip install flask-login
```

### Setup

```python
# app.py — Flask-Login setup
from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flask_login import LoginManager

app = Flask(__name__)
app.config["SECRET_KEY"] = "secret"
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"

db = SQLAlchemy(app)
login_manager = LoginManager(app)

# Configure login
login_manager.login_view = "login"  # Redirect here if not logged in
```

## Next Steps

Flask-Login is installed. Continue to [02_user_model_and_loader.md](02_user_model_and_loader.md) to learn about user models.