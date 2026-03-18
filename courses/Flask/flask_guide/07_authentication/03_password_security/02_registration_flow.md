<!-- FILE: 07_authentication/03_password_security/02_registration_flow.md -->

## Overview

This file covers creating a complete user registration flow with validation, password hashing, and database storage.

## Code Walkthrough

### Registration Route

```python
# app.py — Registration
from flask import Flask, request, flash, redirect, url_for
from models import db, User

app = Flask(__name__)

@app.route("/register", methods=["GET", "POST"])
def register():
    if request.method == "POST":
        username = request.form.get("username")
        password = request.form.get("password")
        
        # Validate
        if User.query.filter_by(username=username).first():
            flash("Username already exists", "error")
            return redirect(url_for("register"))
        
        # Create user
        user = User(username=username)
        user.set_password(password)
        db.session.add(user)
        db.session.commit()
        
        flash("Registration successful! Please log in.", "success")
        return redirect(url_for("login"))
    
    return render_template_string(REGISTER_TEMPLATE)
```

## Next Steps

Registration is complete. Continue to [03_login_flow.md](03_login_flow.md) to create the login flow.