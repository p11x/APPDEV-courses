# Email Templates

## What You'll Learn
- Jinja2 email templates
- Dynamic content
- HTML emails

## Prerequisites
- Completed sending emails

## Jinja2 Templates

```bash
pip install jinja2
```

```python
from jinja2 import Template

welcome_template = Template("""
<!DOCTYPE html>
<html>
<body>
    <h1>Welcome, {{ name }}!</h1>
    <p>Thank you for joining {{ company_name }}.</p>
    <p>Click here to verify your email: {{ verification_link }}</p>
</body>
</html>
""")

def render_welcome_email(name: str, verification_link: str) -> str:
    """Render welcome email"""
    return welcome_template.render(
        name=name,
        company_name="MyApp",
        verification_link=verification_link
    )

# Usage
html = render_welcome_email(
    name="John",
    verification_link="https://example.com/verify/abc123"
)
```

## Flask with Email

```python
from flask import Flask, render_template_string
from flask_mail import Mail, Message

app = Flask(__name__)
app.config['MAIL_SERVER'] = 'smtp.gmail.com'
app.config['MAIL_PORT'] = 587
app.config['MAIL_USE_TLS'] = True
app.config['MAIL_USERNAME'] = 'your-email@gmail.com'
app.config['MAIL_PASSWORD'] = 'your-password'

mail = Mail(app)

email_template = """
<h1>Welcome, {{ user.name }}!</h1>
<p>Your account is ready.</p>
"""

@app.route('/send-welcome')
def send_welcome():
    user = {"name": "John", "email": "john@example.com"}
    
    html = render_template_string(email_template, user=user)
    
    msg = Message(
        subject="Welcome!",
        recipients=[user["email"]],
        html=html
    )
    mail.send(msg)
    
    return "Email sent!"
```

## Summary
- Use Jinja2 for templates
- Keep templates in separate files
- Test emails in development

## Next Steps
→ Continue to `03-email-services.md`
