# SendGrid Email Integration

## What You'll Learn

- How to send emails using SendGrid
- How to create email templates
- How to handle unsubscribe preferences
- How to track email deliverability

## Prerequisites

- Completed `03-stripe-payment-integration.md`
- A SendGrid account (free at sendgrid.com)

## Introduction

SendGrid is a reliable email delivery service used by thousands of companies. It provides a RESTful API for sending emails, managing templates, and tracking deliverability.

## Setting Up SendGrid

Install the SendGrid Python library:

```bash
pip install sendgrid
```

Configure your SendGrid client:

```python
import os
from dataclasses import dataclass
from typing import Optional
from sendgrid import SendGridAPIClient
from sendgrid.helpers.mail import (
    Mail,
    From,
    To,
    Subject,
    PlainTextContent,
    HtmlContent,
    MailSettings,
    SubscriptionTrackingSettings,
    ClickTrackingSettings,
)


@dataclass
class SendGridConfig:
    """Configuration for SendGrid API access."""
    api_key: str
    from_email: str
    from_name: str = "Your App"


class SendGridClient:
    """Client for sending emails via SendGrid."""
    
    def __init__(self, config: SendGridConfig) -> None:
        self.config = config
        self.sg = SendGridAPIClient(api_key=config.api_key)
    
    def send_email(
        self,
        to_email: str,
        subject: str,
        html_content: str,
        plain_text: Optional[str] = None,
        categories: Optional[list[str]] = None,
        custom_args: Optional[dict] = None,
    ) -> dict:
        """Send a single email."""
        message = Mail(
            from_email=From(config.from_email, config.from_name),
            to_emails=To(to_email),
            subject=Subject(subject),
            html_content=HtmlContent(html_content),
            plain_text_content=PlainTextContent(plain_text or ""),
        )
        
        # Add categories for tracking
        if categories:
            message.categories = categories
        
        # Add custom arguments for tracking
        if custom_args:
            message.custom_args = custom_args
        
        # Configure tracking
        mail_settings = MailSettings()
        mail_settings.subscription_tracking_settings = SubscriptionTrackingSettings(
            enable=True,
            text="Click here to unsubscribe",
            html="<p>Click here to unsubscribe</p>",
        )
        mail_settings.click_tracking_settings = ClickTrackingSettings(
            enable=True,
            enable_text=True,
        )
        message.mail_settings = mail_settings
        
        # Send the email
        response = self.sg.send(message)
        
        return {
            "status_code": response.status_code,
            "message_id": response.headers.get("x-message-id"),
        }


# Example usage
def main() -> None:
    config = SendGridConfig(
        api_key=os.environ["SENDGRID_API_KEY"],
        from_email="noreply@yourapp.com",
        from_name="Your App Team",
    )
    
    client = SendGridClient(config)
    
    result = client.send_email(
        to_email="user@example.com",
        subject="Welcome to Your App!",
        html_content="""
        <html>
        <body>
            <h1>Welcome!</h1>
            <p>Thanks for signing up for Your App.</p>
            <p>We're excited to have you on board.</p>
        </body>
        </html>
        """,
        plain_text="Welcome! Thanks for signing up for Your App.",
        categories=["welcome", "onboarding"],
    )
    
    print(f"Email sent: {result}")


if __name__ == "__main__":
    main()
```

🔍 **Line-by-Line Breakdown:**

1. `pip install sendgrid` — Installs the official SendGrid Python SDK.
2. `SendGridAPIClient` — The main client class for interacting with SendGrid's API.
3. `Mail` — A helper class that constructs the email message with all its components.
4. `From`, `To`, `Subject` — Email header helpers that format addresses correctly.
5. `HtmlContent` and `PlainTextContent` — Both versions of the email body. Always include plain text as a fallback.
6. `SubscriptionTrackingSettings` — Adds an unsubscribe link to every email automatically.
7. `ClickTrackingSettings` — Tracks which links users click, useful for analytics.
8. `self.sg.send(message)` — Sends the email through SendGrid's API.
9. `response.headers.get("x-message-id")` — Gets the unique message ID for tracking.

## Using Dynamic Templates

SendGrid's dynamic templates allow for personalized emails:

```python
def send_welcome_email(
    self,
    to_email: str,
    first_name: str,
    verification_link: str,
) -> dict:
    """Send a welcome email using a dynamic template."""
    message = Mail(
        from_email=From(self.config.from_email, self.config.from_name),
        to_emails=To(to_email),
    )
    
    # Use a dynamic template
    message.template_id = "d-welcome-template-id"
    
    # Pass dynamic data
    message.dynamic_template_data = {
        "first_name": first_name,
        "verification_link": verification_link,
    }
    
    response = self.sg.send(message)
    return {"status_code": response.status_code}
```

## HTML Email Template Example

Create a professional HTML email template:

```python
def create_welcome_template(first_name: str, verify_url: str) -> str:
    """Create a welcome email HTML template."""
    return f"""
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Welcome!</title>
        <style>
            body {{
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                line-height: 1.6;
                color: #333;
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
            }}
            .header {{
                background: #5469d4;
                color: white;
                padding: 30px;
                text-align: center;
                border-radius: 8px 8px 0 0;
            }}
            .content {{
                background: #f9f9f9;
                padding: 30px;
                border-radius: 0 0 8px 8px;
            }}
            .button {{
                display: inline-block;
                background: #5469d4;
                color: white;
                padding: 12px 30px;
                text-decoration: none;
                border-radius: 5px;
                margin: 20px 0;
            }}
            .footer {{
                text-align: center;
                margin-top: 20px;
                font-size: 12px;
                color: #666;
            }}
        </style>
    </head>
    <body>
        <div class="header">
            <h1>Welcome to Your App, {first_name}!</h1>
        </div>
        <div class="content">
            <p>Thanks for joining us! We're excited to have you on board.</p>
            <p>To get started, please verify your email address by clicking the button below:</p>
            <p style="text-align: center;">
                <a href="{verify_url}" class="button">Verify Email</a>
            </p>
            <p>If the button doesn't work, copy and paste this link into your browser:</p>
            <p>{verify_url}</p>
        </div>
        <div class="footer">
            <p>© 2024 Your App. All rights reserved.</p>
            <p>
                <a href="{{{{unsubscribe}}}}">Unsubscribe</a> from these emails.
            </p>
        </div>
    </body>
    </html>
    """


def send_welcome_email(
    self,
    to_email: str,
    first_name: str,
    verify_url: str,
) -> dict:
    """Send a welcome email with custom HTML."""
    html = create_welcome_template(first_name, verify_url)
    plain_text = f"""
    Welcome to Your App, {first_name}!
    
    Thanks for joining us! To get started, please verify your email address:
    {verify_url}
    
    If you didn't sign up for this account, please ignore this email.
    """
    
    return self.send_email(
        to_email=to_email,
        subject="Welcome to Your App!",
        html_content=html,
        plain_text=plain_text,
        categories=["welcome"],
    )
```

## Handling Bounces and Unsubscribes

Track email delivery status:

```python
def get_bounces(self, start_date: str) -> list[dict]:
    """Get list of bounced email addresses."""
    response = self.sg.client.bounces.get(
        query_params={"start_date": start_date}
    )
    return response.body


def get_unsubscribes(self, start_date: str) -> list[dict]:
    """Get list of unsubscribed email addresses."""
    response = self.sg.client.unsubscribes.get(
        query_params={"start_date": start_date}
    )
    return response.body


def delete_bounce(self, email: str) -> None:
    """Delete a bounced email address from the list."""
    # First get the bounce ID, then delete it
    response = self.sg.client.bounces.get(
        query_params={"email": email}
    )
    bounces = response.body
    if bounces:
        self.sg.client.bounces._(bounces[0]["id"]).delete()
```

## Batch Sending

Send emails to multiple recipients efficiently:

```python
def send_batch_welcome_emails(
    self,
    recipients: list[dict],  # [{"email": "...", "first_name": "..."}]
) -> dict:
    """Send welcome emails to multiple recipients in one API call."""
    messages = []
    
    for recipient in recipients:
        message = Mail(
            from_email=From(self.config.from_email, self.config.from_name),
            to_emails=To(recipient["email"]),
            subject=Subject("Welcome to Your App!"),
            html_content=HtmlContent(
                f"<h1>Welcome, {recipient['first_name']}!</h1>"
            ),
            plain_text_content=PlainTextContent(
                f"Welcome, {recipient['first_name']}!"
            ),
        )
        messages.append(message)
    
    # Send all at once
    response = self.sg.client.mail.batch.send.post(messages)
    return {
        "status_code": response.status_code,
        "failed_count": len(response.body.get("errors", [])),
    }
```

## FastAPI Integration

Create email endpoints in FastAPI:

```python
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, EmailStr
from typing import Optional


app = FastAPI()

# Configure SendGrid
sg = SendGridAPIClient(os.environ["SENDGRID_API_KEY"])
FROM_EMAIL = "noreply@yourapp.com"
FROM_NAME = "Your App"


class WelcomeEmailRequest(BaseModel):
    """Request model for welcome email."""
    email: EmailStr
    first_name: str
    verify_url: str


class ContactFormRequest(BaseModel):
    """Request model for contact form."""
    name: str
    email: EmailStr
    message: str


@app.post("/api/emails/welcome")
async def send_welcome_email(request: WelcomeEmailRequest) -> dict:
    """Send a welcome email to new users."""
    message = Mail(
        from_email=From(FROM_EMAIL, FROM_NAME),
        to_emails=To(request.email),
        subject=Subject("Welcome to Your App!"),
    )
    
    message.template_id = "d-welcome-template"
    message.dynamic_template_data = {
        "first_name": request.first_name,
        "verification_link": request.verify_url,
    }
    
    try:
        response = sg.send(message)
        return {
            "success": True,
            "message_id": response.headers.get("x-message-id"),
        }
    except Exception as e:
        raise HTTPException(
            status_code=500,
            detail=f"Failed to send email: {str(e)}"
        )


@app.post("/api/emails/contact")
async def send_contact_form(request: ContactFormRequest) -> dict:
    """Send a contact form submission."""
    message = Mail(
        from_email=From(FROM_EMAIL, FROM_NAME),
        to_emails=To("support@yourapp.com"),
        subject=Subject(f"New contact form: {request.name}"),
    )
    
    message.html_content = HtmlContent(f"""
        <h2>New Contact Form Submission</h2>
        <p><strong>Name:</strong> {request.name}</p>
        <p><strong>Email:</strong> {request.email}</p>
        <p><strong>Message:</strong></p>
        <p>{request.message}</p>
    """)
    
    message.plain_text_content = PlainTextContent(f"""
        New Contact Form Submission
        
        Name: {request.name}
        Email: {request.email}
        Message: {request.message}
    """)
    
    response = sg.send(message)
    return {"success": True}
```

## Best Practices

1. **Always include plain text** — Some email clients disable HTML
2. **Use unsubscribe links** — Required by law (CAN-SPAM, GDPR)
3. **Warm up your IP** — Start slowly with new SendGrid accounts
4. **Monitor deliverability** — Check your SendGrid dashboard for metrics
5. **Validate emails** — Use SendGrid's email validation API

## Summary

- SendGrid provides a reliable email delivery service with a Python SDK
- Use the Mail helper class to construct emails properly
- Dynamic templates allow for personalized content
- Always include plain text versions alongside HTML
- Track bounces and unsubscribes to maintain list health
- FastAPI integrates easily with SendGrid for sending emails

## Next Steps

→ Continue to `05-aws-s3-file-upload.md` to learn about file storage.
