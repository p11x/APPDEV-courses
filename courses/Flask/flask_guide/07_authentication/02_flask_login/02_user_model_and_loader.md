<!-- FILE: 07_authentication/02_flask_login/02_user_model_and_loader.md -->

## Overview

Flask-Login requires user models to implement certain methods. This file covers creating Flask-Login compatible user models and setting up the user loader callback.

## Prerequisites

- Flask-Login installed
- User model defined

## Code Walkthrough

### User Model with Flask-Login

```python
# models.py — Flask-Login user model
from flask_sqlalchemy import SQLAlchemy
from flask_login import UserMixin
from werkzeug.security import generate_password_hash, check_password_hash

db = SQLAlchemy()

class User(UserMixin, db.Model):
    """User model for Flask-Login."""
    
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True)
    password_hash = db.Column(db.String(200))
    is_active = db.Column(db.Boolean, default=True)
    
    # Flask-Login required methods:
    def get_id(self):
        """Return the user ID as a string (required)."""
        return str(self.id)
    
    @property
    def is_authenticated(self):
        """Return True if user is authenticated."""
        return True
    
    @property
    def is_anonymous(self):
        """Return True for anonymous users."""
        return False
    
    # Password methods
    def set_password(self, password):
        """Hash and set password."""
        self.password_hash = generate_password_hash(password)
    
    def check_password(self, password):
        """Verify password."""
        return check_password_hash(self.password_hash, password)
```

```python
# app.py — User loader
from flask import Flask
from flask_login import LoginManager
from models import db, User

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"

db.init_app(app)
login_manager = LoginManager(app)

@login_manager.user_loader
def load_user(user_id):
    """Load user by ID for Flask-Login."""
    return User.query.get(int(user_id))
```

## Next Steps

Now you have user model. Continue to [03_login_logout_views.md](03_login_logout_views.md) to create login/logout views.