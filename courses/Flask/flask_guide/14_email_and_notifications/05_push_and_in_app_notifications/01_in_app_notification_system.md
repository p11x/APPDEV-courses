<!-- FILE: 14_email_and_notifications/05_push_and_in_app_notifications/01_in_app_notification_system.md -->

## Overview

In-app notifications keep users engaged.

## Code Walkthrough

```python
# notifications.py
from flask import Flask, jsonify

app = Flask(__name__)

# In-memory storage
notifications = {}

@app.route("/notifications/<user_id>")
def get_notifications(user_id):
    return jsonify(notifications.get(user_id, []))

@app.route("/notifications/<user_id>", methods=["POST"])
def add_notification(user_id):
    notification = request.json
    if user_id not in notifications:
        notifications[user_id] = []
    notifications[user_id].append(notification)
    return jsonify({"status": "added"})
```
