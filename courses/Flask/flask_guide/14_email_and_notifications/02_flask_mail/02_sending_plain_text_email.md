<!-- FILE: 14_email_and_notifications/02_flask_mail/02_sending_plain_text_email.md -->

## Overview

This file covers sending plain text emails with Flask-Mail.

## Code Walkthrough

```python
# plain_email.py
from flask import Flask
from flask_mail import Mail, Message

app = Flask(__name__)
app.config["MAIL_SERVER"] = "smtp.gmail.com"
app.config["MAIL_PORT"] = 587
app.config["MAIL_USE_TLS"] = True
app.config["MAIL_USERNAME"] = "your-email@gmail.com"
app.config["MAIL_PASSWORD"] = "your-password"

mail = Mail(app)

@app.route("/plain-email")
def send_plain_email():
    msg = Message(
        subject="Plain Text Email",
        sender="you@gmail.com",
        recipients=["recipient@example.com"]
    )
    msg.body = """Hello,

This is a plain text email sent from Flask.

Best regards,
Your Flask App"""
    
    mail.send(msg)
    return "Plain email sent!"

if __name__ == "__main__":
    app.run()
```
