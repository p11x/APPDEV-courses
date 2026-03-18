<!-- FILE: 15_flask_admin_panel/01_admin_fundamentals/02_installing_flask_admin.md -->

## Overview

Install Flask-Admin and set up basic configuration.

## Installation

```bash
pip install flask-admin flask-sqlalchemy
```

## Quick Setup

```python
# quick_admin.py
from flask import Flask
from flask_admin import Admin
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///app.db"
app.config["SECRET_KEY"] = "secret"

db = SQLAlchemy(app)
admin = Admin(app, name="My Admin")

if __name__ == "__main__":
    app.run(debug=True)
```

## Next Steps

Continue to [03_admin_security_overview.md](03_admin_security_overview.md)
