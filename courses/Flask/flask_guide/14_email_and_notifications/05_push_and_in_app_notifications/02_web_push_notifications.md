<!-- FILE: 14_email_and_notifications/05_push_and_in_app_notifications/02_web_push_notifications.md -->

## Overview

Web push notifications allow browsers to send notifications to users even when the site is closed.

## Installation

```bash
pip install pywebpush
```

## Code Walkthrough

```python
# web_push.py
from pywebpush import webpush, WebPushException

VAPID_PRIVATE_KEY = "your_private_key"
VAPID_PUBLIC_KEY = "your_public_key"
VAPID_CLAIM = "mailto:admin@example.com"

def send_push_notification(subscription_info, message):
    return webpush(
        subscription_info=subscription_info,
        data=message,
        vapid_private_key=VAPID_PRIVATE_KEY,
        vapid_claims={"sub": VAPID_CLAIM}
    )
```

## Next Steps

Continue to [03_notification_best_practices.md](03_notification_best_practices.md)
