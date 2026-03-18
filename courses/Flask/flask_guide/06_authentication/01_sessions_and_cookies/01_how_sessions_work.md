<!-- FILE: 06_authentication/01_sessions_and_cookies/01_how_sessions_work.md -->

## Overview

**Sessions** maintain user state across requests. When users log in, the server creates a session and stores user information. This file explains how sessions work in Flask, how cookies are used, and how to configure sessions securely.

## Prerequisites

- Basic Flask knowledge
- Understanding of HTTP

## Core Concepts

### How Sessions Work

1. User logs in with credentials
2. Server creates session with user ID
3. Server sends session cookie to browser
4. Browser sends cookie with each request
5. Server looks up user from session

### Session in Flask

Flask uses signed cookies to store session data:
```python
from flask import session

# Set session data
session["user_id"] = user.id

# Get session data
user_id = session.get("user_id")
```

## Code Walkthrough

### Session Example

```python
# app.py — Basic sessions
from flask import Flask, session, redirect, url_for, request

app = Flask(__name__)
app.secret_key = "your-secret-key"  # Required for sessions

@app.route("/")
def index():
    if "username" in session:
        return f"Logged in as {session['username']}"
    return "Not logged in"

@app.route("/login", methods=["GET", "POST"])
def login():
    if request.method == "POST":
        session["username"] = request.form.get("username")
        return redirect(url_for("index"))
    return '''
        <form method="post">
            <input name="username">
            <button type="submit">Login</button>
        </form>
    '''

@app.route("/logout")
def logout():
    session.pop("username", None)
    return redirect(url_for("index"))
```

## Next Steps

Now you understand sessions. Continue to [02_setting_reading_sessions.md](02_setting_reading_sessions.md) to learn more about session operations.