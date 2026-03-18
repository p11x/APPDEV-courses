# Email Best Practices

## What You'll Learn
- Email deliverability
- SPF, DKIM, DMARC
- Handling bounces

## Prerequisites
- Completed email services

## Email Authentication

### SPF (Sender Policy Framework)

Add to your DNS:
```
v=spf1 include:_spf.google.com ~all
```

### DKIM (DomainKeys Identified Mail)

Configure in your email service provider.

### DMARC

Add to DNS:
```
_dmarc.yourdomain.com IN TXT "v=DMARC1; p=quarantine; rua=mailto:dmarc@example.com"
```

## Handling Bounces

```python
class EmailService:
    def __init__(self):
        self.bounce_list = set()
    
    def send_with_tracking(self, to_email: str, subject: str, html: str):
        if to_email in self.bounce_list:
            print(f"Skipping bounced email: {to_email}")
            return
        
        try:
            send_email(to_email, subject, html)
        except BounceException:
            self.bounce_list.add(to_email)
            self.handle_bounce(to_email)
    
    def handle_bounce(self, email: str):
        print(f"Bounce detected for {email}")
        # Update database, disable user, etc.
```

## Summary
- Configure SPF, DKIM, DMARC
- Handle bounces and complaints
- Use double opt-in
- Monitor deliverability

## Next Steps
→ Move to `21-payments/`
