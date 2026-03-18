<!-- FILE: 14_email_and_notifications/04_transactional_email_services/01_sendgrid_integration.md -->

## Overview

SendGrid is a popular email delivery service.

## Installation

```bash
pip install sendgrid
```

## Code Walkthrough

```python
# sendgrid.py
from sendgrid import SendGridAPIClient
from sendgrid.helpers.mail import Mail

def send_email_via_sendgrid(to_email, subject, content):
    sg = SendGridAPIClient("YOUR_API_KEY")
    
    message = Mail(
        from_email="from@example.com",
        to_emails=to_email,
        subject=subject,
        html_content=content
    )
    
    response = sg.send(message)
    return response.status_code
```
