<!-- FILE: 14_email_and_notifications/04_transactional_email_services/02_mailgun_integration.md -->

## Overview

Mailgun is another popular email service.

## Installation

```bash
pip install requests
```

## Code Walkthrough

```python
# mailgun.py
import requests

def send_via_mailgun(api_key, domain, to_email, subject, html):
    return requests.post(
        f"https://api.mailgun.net/v3/{domain}/messages",
        auth=("api", api_key),
        data={
            "from": f"App <mail@{domain}>",
            "to": to_email,
            "subject": subject,
            "html": html
        }
    )
```
