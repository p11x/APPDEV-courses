# Sending Emails

## What You'll Learn
- SMTP email sending
- Using Mailgun/SendGrid
- Email templates

## Prerequisites
- Completed file handling folder

## SMTP with smtplib

```python
import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart

def send_email_smtp(
    to_email: str,
    subject: str,
    body: str,
    smtp_server: str = "smtp.gmail.com",
    smtp_port: int = 587,
    username: str = "your-email@gmail.com",
    password: str = "your-password"
) -> bool:
    """Send email via SMTP"""
    msg = MIMEMultipart()
    msg['From'] = username
    msg['To'] = to_email
    msg['Subject'] = subject
    
    msg.attach(MIMEText(body, 'html'))
    
    try:
        server = smtplib.SMTP(smtp_server, smtp_port)
        server.starttls()
        server.login(username, password)
        server.send_message(msg)
        server.quit()
        return True
    except Exception as e:
        print(f"Error: {e}")
        return False

# Usage
send_email_smtp(
    to_email="user@example.com",
    subject="Welcome!",
    body="<h1>Hello!</h1><p>Welcome to our app.</p>"
)
```

## Using aiosmtplib

```python
import aiosmtplib
from email.mime.text import MIMEText

async def send_email_async(to_email: str, subject: str, body: str):
    """Send email asynchronously"""
    message = MIMEText(body, 'html')
    message['From'] = 'noreply@example.com'
    message['To'] = to_email
    message['Subject'] = subject
    
    await aiosmtplib.send(
        message,
        hostname='smtp.gmail.com',
        port=587,
        username='your-email@gmail.com',
        password='your-password',
        start_tls=True
    )
```

## Summary
- Use SMTP for basic email
- Use async for better performance
- Consider third-party services

## Next Steps
→ Continue to `02-email-templates.md`
