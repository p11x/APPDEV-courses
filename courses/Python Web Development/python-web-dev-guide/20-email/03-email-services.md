# Email Services

## What You'll Learn
- Using SendGrid
- Using Mailgun
- AWS SES

## Prerequisites
- Completed email templates

## SendGrid

```bash
pip install sendgrid
```

```python
from sendgrid import SendGridAPIClient
from sendgrid.helpers.mail import Mail

def send_with_sendgrid(to_email: str, subject: str, html: str):
    """Send email via SendGrid"""
    message = Mail(
        from_email='noreply@example.com',
        to_emails=to_email,
        subject=subject,
        html_content=html
    )
    
    sg = SendGridAPIClient('SENDGRID_API_KEY')
    response = sg.send(message)
    
    return response.status_code

# Usage
send_with_sendgrid(
    to_email="user@example.com",
    subject="Welcome!",
    html="<h1>Hello!</h1><p>Welcome to our app.</p>"
)
```

## Mailgun

```bash
pip install requests
```

```python
import requests

def send_with_mailgun(to_email: str, subject: str, html: str):
    """Send email via Mailgun"""
    return requests.post(
        "https://api.mailgun.net/v3/your-domain.com/messages",
        auth=("api", "MAILGUN_API_KEY"),
        data={
            "from": "noreply@your-domain.com",
            "to": to_email,
            "subject": subject,
            "html": html
        }
    )
```

## AWS SES

```bash
pip install boto3
```

```python
import boto3

ses = boto3.client('ses', region_name='us-east-1')

def send_with_ses(to_email: str, subject: str, html: str):
    """Send email via AWS SES"""
    return ses.send_email(
        Source='noreply@example.com',
        Destination={'ToAddresses': [to_email]},
        Message={
            'Subject': {'Data': subject},
            'Body': {'Html': {'Data': html}}
        }
    )
```

## Summary
- Use third-party services for reliability
- SendGrid, Mailgun, AWS SES are popular
- Handle bounces and complaints

## Next Steps
→ Continue to `04-email-best-practices.md`
