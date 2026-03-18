<!-- FILE: 07_authentication/02_flask_login/03_login_logout_views.md -->

## Overview

This file covers creating login and logout views using Flask-Login, including the @login_required decorator to protect routes.

## Code Walkthrough

### Login/Logout Views

```python
# app.py — Login/Logout views
from flask import Flask, render_template_string, redirect, url_for, request, flash
from flask_login import LoginManager, login_user, logout_user, login_required, current_user
from models import db, User

app = Flask(__name__)
app.config["SECRET_KEY"] = "secret"
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///myapp.db"

db.init_app(app)
login_manager = LoginManager(app)
login_manager.login_view = "login"

@login_manager.user_loader
def load_user(user_id):
    return User.query.get(int(user_id))

# Login route
@app.route("/login", methods=["GET", "POST"])
def login():
    if current_user.is_authenticated:
        return redirect(url_for("dashboard"))
    
    if request.method == "POST":
        username = request.form.get("username")
        password = request.form.get("password")
        
        user = User.query.filter_by(username=username).first()
        
        if user and user.check_password(password):
            login_user(user)
            return redirect(url_for("dashboard"))
        
        flash("Invalid username or password", "error")
    
    return render_template_string(LOGIN_TEMPLATE)

# Logout route
@app.route("/logout")
@login_required
def logout():
    logout_user()
    return redirect(url_for("login"))

# Protected route
@app.route("/dashboard")
@login_required
def dashboard():
    return f"Welcome, {current_user.username}!"

LOGIN_TEMPLATE = '''
<form method="post">
    <input type="text" name="username" placeholder="Username">
    <input type="password" name="password" placeholder="Password">
    <button type="submit">Login</button>
</form>
'''
```

## Next Steps

Now you can handle login/logout. Continue to [01_hashing_with_bcrypt.md](../03_password_security/01_hashing_with_bcrypt.md) to learn password security.