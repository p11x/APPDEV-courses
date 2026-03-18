<!-- FILE: 07_authentication/03_password_security/03_login_flow.md -->

## Overview

This file covers the complete login flow: validating credentials, checking password hashes, and using Flask-Login to establish sessions.

## Code Walkthrough

### Login Route

```python
# app.py — Login flow
from flask import request, flash, redirect, url_for
from flask_login import login_user
from models import User

@app.route("/login", methods=["GET", "POST"])
def login():
    if request.method == "POST":
        username = request.form.get("username")
        password = request.form.get("password")
        
        user = User.query.filter_by(username=username).first()
        
        if user and user.check_password(password):
            login_user(user)
            return redirect(url_for("dashboard"))
        
        flash("Invalid username or password", "error")
    
    return render_template_string(LOGIN_TEMPLATE)
```

## Next Steps

You have completed authentication. Continue to [01_why_blueprints.md](../07_blueprints_and_application_factory/01_blueprints/01_why_blueprints.md) to learn about blueprints.