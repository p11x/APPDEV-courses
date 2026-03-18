<!-- FILE: 06_authentication/01_sessions_and_cookies/02_setting_reading_sessions.md -->

## Overview

This file covers setting, reading, and managing session data in Flask, including session configuration options, permanent sessions, and best practices.

## Code Walkthrough

### Session Management

```python
# app.py — Session management
from flask import Flask, session, request
import datetime

app = Flask(__name__)
app.secret_key = "secret"

# Set session data
session["user_id"] = 1
session["username"] = "alice"
session.permanent = True  # Lasts for PERMANENT_SESSION_LIFETIME

# Configuration
app.config["PERMANENT_SESSION_LIFETIME"] = datetime.timedelta(days=7)

# Get session data
user_id = session.get("user_id")
username = session.get("username")

# Check if logged in
if "user_id" in session:
    # User is logged in
    
# Delete session data
session.pop("user_id", None)

# Clear entire session
session.clear()
```

## Next Steps

Now you can manage sessions. Continue to [03_secure_cookies.md](03_secure_cookies.md) to learn about secure cookies.