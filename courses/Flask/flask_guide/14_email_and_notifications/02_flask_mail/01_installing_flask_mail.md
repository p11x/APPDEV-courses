<!-- FILE: 14_email_and_notifications/02_flask_mail/01_installing_flask_mail.md -->

## Overview

Flask-Mail is the standard extension for sending emails from Flask applications.

## Installation

```bash
pip install Flask-Mail
```

## Code Walkthrough

```python
# mail_app.py
from flask import Flask
from flask_mail import Mail, Message

app = Flask(__name__)
app.config["MAIL_SERVER"] = "smtp.gmail.com"
app.config["MAIL_PORT"] = 587
app.config["MAIL_USE_TLS"] = True
app.config["MAIL_USERNAME"] = "your-email@gmail.com"
app.config["MAIL_PASSWORD"] = "your-password"

mail = Mail(app)

@app.route("/send")
def send_email():
    msg = Message(
        "Hello",
        sender="you@gmail.com",
        recipients=["recipient@example.com"]
    )
    msg.body = "This is a test email"
    mail.send(msg)
    return "Sent!"

if __name__ == "__main__":
    app.run(debug=True)
```

## Next Steps

Continue to [02_sending_plain_text_email.md](02_sending_plain_text_email.md)
