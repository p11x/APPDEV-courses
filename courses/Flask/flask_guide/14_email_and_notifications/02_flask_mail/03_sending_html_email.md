<!-- FILE: 14_email_and_notifications/02_flask_mail/03_sending_html_email.md -->

## Overview

This file covers sending HTML emails with Flask-Mail.

## Code Walkthrough

```python
# html_email.py
from flask import Flask
from flask_mail import Mail, Message

app = Flask(__name__)
mail = Mail(app)

@app.route("/html-email")
def send_html_email():
    msg = Message(
        subject="HTML Email",
        sender="you@gmail.com",
        recipients=["recipient@example.com"]
    )
    
    # Plain text alternative
    msg.body = "This email requires HTML support."
    
    # HTML content
    msg.html = """
    <html>
    <body style="font-family: Arial, sans-serif;">
        <h1 style="color: #333;">Hello!</h1>
        <p>This is an <strong>HTML email</strong> from Flask.</p>
        <a href="https://example.com" style="color: blue;">Click here</a>
    </body>
    </html>
    """
    
    mail.send(msg)
    return "HTML email sent!"
```
