<!-- FILE: 15_flask_admin_panel/01_admin_fundamentals/01_what_is_flask_admin.md -->

## Overview

Flask-Admin provides a ready-made admin interface for Flask applications.

## Installation

```bash
pip install flask-admin
```

## Code Walkthrough

```python
# admin_app.py
from flask import Flask
from flask_admin import Admin
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///test.db"
db = SQLAlchemy(app)

admin = Admin(app)

if __name__ == "__main__":
    app.run(debug=True)
```

## Next Steps

Continue to [02_installing_flask_admin.md](02_installing_flask_admin.md)
