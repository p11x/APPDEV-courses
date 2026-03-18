<!-- FILE: 15_flask_admin_panel/02_model_views/01_registering_models.md -->

## Overview

Register SQLAlchemy models with Flask-Admin.

## Code Walkthrough

```python
# model_views.py
from flask import Flask
from flask_admin import Admin
from flask_sqlalchemy import SQLAlchemy
from flask_admin.contrib.sqla import ModelView

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///app.db"

db = SQLAlchemy(app)
admin = Admin(app)

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True)
    email = db.Column(db.String(120))

# Register model
admin.add_view(ModelView(User, db.session))

if __name__ == "__main__":
    app.run(debug=True)
```
