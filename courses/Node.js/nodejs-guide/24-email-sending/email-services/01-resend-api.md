# Resend API

## What You'll Learn

- What Resend is and why use an email API instead of SMTP
- How to install and configure the Resend SDK
- How to send emails with the Resend API
- How to verify your domain
- How Resend compares to nodemailer SMTP

## What Is Resend?

Resend is a transactional email API. Instead of configuring SMTP servers, you make an HTTP request to send email. Benefits:

- **No SMTP configuration** — just an API key
- **Deliverability** — Resend handles SPF, DKIM, and reputation
- **Developer experience** — simple SDK, React email templates
- **Analytics** — open rates, click rates, bounces

## Project Setup

```bash
npm install resend
```

Get your API key from [resend.com](https://resend.com).

## Sending Email

```js
// send-resend.js — Send email with Resend API

import { Resend } from 'resend';

// Initialize with your API key
const resend = new Resend(process.env.RESEND_API_KEY);

async function main() {
  // Send a simple email
  const { data, error } = await resend.emails.send({
    from: 'My App <noreply@myapp.com>',  // Must be from a verified domain
    to: ['user@example.com'],             // Array of recipients
    subject: 'Welcome!',
    html: '<h1>Welcome to My App!</h1><p>Thanks for signing up.</p>',
  });

  if (error) {
    console.error('Error:', error);
    return;
  }

  console.log('Email sent:', data.id);
  // data = { id: 'abc123...' }
}

main();
```

## Multiple Recipients and CC

```js
const { data } = await resend.emails.send({
  from: 'Team <team@myapp.com>',
  to: ['alice@example.com'],
  cc: ['bob@example.com', 'charlie@example.com'],
  bcc: ['audit@example.com'],
  replyTo: 'support@myapp.com',  // Replies go here instead of the sender
  subject: 'Team Update',
  html: '<h1>Monthly update</h1>',
});
```

## Sending with React Email (Optional)

Resend supports React components as email templates:

```bash
npm install @react-email/components
```

```jsx
// emails/welcome.jsx — React email template

import {
  Html, Head, Preview, Body,
  Container, Section, Text, Button, Img
} from '@react-email/components';

export default function WelcomeEmail({ name, loginUrl }) {
  return (
    <Html>
      <Head />
      <Preview>Welcome to My App!</Preview>
      <Body style={{ backgroundColor: '#f4f4f4', fontFamily: 'Arial' }}>
        <Container style={{ maxWidth: '600px', margin: '0 auto', backgroundColor: '#fff' }}>
          <Section style={{ padding: '40px 30px' }}>
            <Text style={{ fontSize: '24px', fontWeight: 'bold' }}>
              Welcome, {name}!
            </Text>
            <Text style={{ fontSize: '16px', color: '#666' }}>
              Thanks for joining. Click below to get started.
            </Text>
            <Button
              href={loginUrl}
              style={{
                backgroundColor: '#4f46e5',
                color: '#fff',
                padding: '14px 30px',
                borderRadius: '6px',
                textDecoration: 'none',
              }}
            >
              Get Started
            </Button>
          </Section>
        </Container>
      </Body>
    </Html>
  );
}
```

```js
// Use the React component
import WelcomeEmail from './emails/welcome.jsx';

const { data } = await resend.emails.send({
  from: 'My App <noreply@myapp.com>',
  to: ['alice@example.com'],
  subject: 'Welcome!',
  react: WelcomeEmail({ name: 'Alice', loginUrl: 'https://myapp.com/start' }),
});
```

## Domain Verification

To send from your domain (e.g., `noreply@myapp.com`), add DNS records:

1. Go to Resend dashboard → Domains → Add Domain
2. Add the provided DNS records (SPF, DKIM, DMARC):

```
Type: TXT
Name: resend._domainkey
Value: p=MIGfMA0GCSqGSIb3DQEB...

Type: TXT  
Name: @
Value: v=spf1 include:amazonses.com ~all
```

3. Wait for verification (usually a few minutes)
4. Send from any address at your domain

## How It Works

### Resend vs SMTP

| Feature | Nodemailer (SMTP) | Resend (API) |
|---------|-------------------|--------------|
| Setup | Configure SMTP server | Just an API key |
| Deliverability | You manage SPF/DKIM | Resend manages it |
| Speed | TCP connection overhead | HTTP request |
| Analytics | None | Built-in dashboard |
| Templates | Handlebars (manual) | React Email (optional) |

### API Flow

```
Your app → POST https://api.resend.com/emails → Resend → Recipient's inbox
  │                                                       
  └── API key in Authorization header                     
  └── JSON body: { from, to, subject, html }             
```

## Common Mistakes

### Mistake 1: Unverified Sender Domain

```js
// WRONG — sending from an unverified domain
from: 'me@unverified-domain.com'
// Resend rejects: "Domain not verified"

// CORRECT — use a verified domain
from: 'noreply@myapp.com'  // myapp.com is verified in Resend dashboard
```

### Mistake 2: Missing API Key

```js
// WRONG — no API key
const resend = new Resend();  // All calls fail with 401

// CORRECT — load from environment
const resend = new Resend(process.env.RESEND_API_KEY);
```

### Mistake 3: Not Handling Errors

```js
// WRONG — assuming success
const { data } = await resend.emails.send({ ... });
console.log(data.id);  // data is undefined if there was an error

// CORRECT — check the error
const { data, error } = await resend.emails.send({ ... });
if (error) return console.error('Failed:', error);
console.log('Sent:', data.id);
```

## Try It Yourself

### Exercise 1: Send Test Email

Sign up for Resend, get an API key, and send a test email to your own address.

### Exercise 2: React Template

Create a React email template for a password reset email. Render it and send via Resend.

### Exercise 3: Webhook Notifications

Configure a Resend webhook to receive delivery, open, and bounce events.

## Next Steps

You can send emails via API. For testing emails locally, continue to [Email Testing](./02-email-testing.md).
