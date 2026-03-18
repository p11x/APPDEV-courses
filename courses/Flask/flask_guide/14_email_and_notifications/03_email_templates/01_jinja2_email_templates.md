<!-- FILE: 14_email_and_notifications/03_email_templates/01_jinja2_email_templates.md -->

## Overview

Use Jinja2 templates for HTML emails.

## Code Walkthrough

```python
# email_templates.py
from flask import Flask, render_template_string
from flask_mail import Mail, Message

app = Flask(__name__)
mail = Mail(app)

# Simple template
email_template = """
<html>
<body>
    <h1>Welcome {{ username }}!</h1>
    <p>Thank you for registering.</p>
</body>
</html>
"""

@app.route("/welcome-email/<username>")
def send_welcome(username):
    msg = Message(
        subject="Welcome!",
        sender="you@gmail.com",
        recipients=["user@example.com"]
    )
    msg.html = render_template_string(email_template, username=username)
    mail.send(msg)
    return "Sent!"
