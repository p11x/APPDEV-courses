# Session-Based Authentication

## What You'll Learn
- How sessions work
- Flask-Login setup
- Protecting routes

## Prerequisites
- Completed Flask basics

## How Sessions Work

1. User logs in with credentials
2. Server creates session, stores user ID
3. Server sends session cookie to browser
4. Browser sends cookie with each request
5. Server looks up user from session

## Flask-Login

```bash
pip install flask-login
```

```python
from flask import Flask
from flask_login import LoginManager, UserMixin, login_user, login_required, logout_user, current_user

app = Flask(__name__)
app.secret_key = "secret"

login_manager = LoginManager()
login_manager.init_app(app)

# User model
class User(UserMixin):
    def __init__(self, id, username, password):
        self.id = id
        self.username = username
        self.password = password

# User loader
@login_manager.user_loader
def load_user(user_id):
    return users.get(int(user_id))

@app.route("/login", methods=["POST"])
def login():
    username = request.form.get("username")
    user = find_user(username)
    if user and check_password(user.password, request.form.get("password")):
        login_user(user)
        return "Logged in!"
    return "Invalid credentials", 401

@app.route("/dashboard")
@login_required
def dashboard():
    return f"Welcome {current_user.username}!"

@app.route("/logout")
@login_required
def logout():
    logout_user()
    return "Logged out!"
```

## Summary
- Sessions store user info server-side
- Flask-Login manages sessions
- Use `@login_required` to protect routes
