# 📧 Email and Notifications

## 🎯 What You'll Learn

- Sending emails with smtplib
- Telegram bot notifications
- Desktop notifications

---

## Send Email with smtplib

```python
import smtplib
from email.mime.text import MIMEText

def send_email(to_email, subject, body):
    # Configure
    smtp_server = "smtp.gmail.com"
    smtp_port = 587
    username = "your_email@gmail.com"
    password = "your_app_password"  # Use App Password!
    
    # Create message
    msg = MIMEText(body, "plain")
    msg["Subject"] = subject
    msg["From"] = username
    msg["To"] = to_email
    
    # Send
    with smtplib.SMTP(smtp_server, smtp_port) as server:
        server.starttls()
        server.login(username, password)
        server.send_message(msg)

# Usage
send_email(
    to_email="recipient@example.com",
    subject="Daily Report",
    body="Your report is ready!"
)
```

### ⚠️ App Passwords

1. Enable 2-Factor Authentication on Google
2. Go to Google Account → Security → App passwords
3. Create new app password for "Mail"

---

## Telegram Bot Notifications

```bash
pip install python-telegram-bot
```

```python
from telegram import Bot

async def send_telegram(message: str):
    bot = Bot(token="YOUR_BOT_TOKEN")
    await bot.send_message(chat_id="YOUR_CHAT_ID", text=message)
```

---

## Desktop Notifications

```bash
pip install plyer
```

```python
from plyer import notification

notification.notify(
    title="Daily Reminder",
    message="Time to check your tasks!",
    timeout=10  # seconds
)
```

---

## ✅ Summary

- Use App Passwords for Gmail SMTP
- Telegram bots for easy notifications
- plyer for cross-platform desktop notifications
