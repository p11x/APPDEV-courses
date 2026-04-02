# Nodemailer Setup

## What You'll Learn

- How to install and configure nodemailer
- How to create a transport (SMTP)
- How to send plain text and HTML emails
- How to configure sender, recipients, and subject
- How to handle send errors

## What Is Nodemailer?

Nodemailer is the most popular Node.js library for sending emails. It supports SMTP, SES, SendGrid, and other transports. You configure a **transport** (how to send) and call `sendMail()` (what to send).

## Project Setup

```bash
mkdir email-demo && cd email-demo
npm init -y
npm install nodemailer
```

## Basic Email

```js
// send-email.js — Send an email with nodemailer

import nodemailer from 'nodemailer';

// Create a transport — this defines HOW to send the email
// For development, use Ethereal (fake SMTP that captures emails)
async function createTransport() {
  // Create a test account on Ethereal (free, no signup needed)
  const testAccount = await nodemailer.createTestAccount();

  return nodemailer.createTransport({
    host: 'smtp.ethereal.email',
    port: 587,
    secure: false,  // true for port 465, false for other ports
    auth: {
      user: testAccount.user,  // Ethereal username
      pass: testAccount.pass,  // Ethereal password
    },
  });
}

async function main() {
  const transporter = await createTransport();

  // Send an email
  const info = await transporter.sendMail({
    from: '"My App" <noreply@myapp.com>',  // Sender address
    to: 'user@example.com',                 // Recipient(s) — comma-separated for multiple
    subject: 'Welcome to My App!',           // Subject line
    text: 'Hello! Thanks for signing up.',   // Plain text body
    html: '<h1>Hello!</h1><p>Thanks for signing up.</p>',  // HTML body
  });

  console.log('Message sent:', info.messageId);

  // Ethereal provides a preview URL — open it to see the email
  console.log('Preview URL:', nodemailer.getTestMessageUrl(info));
  // https://ethereal.email/message/abc123...
}

main().catch(console.error);
```

## Production SMTP

```js
// production-email.js — Send via real SMTP server

import nodemailer from 'nodemailer';

const transporter = nodemailer.createTransport({
  host: process.env.SMTP_HOST || 'smtp.gmail.com',
  port: parseInt(process.env.SMTP_PORT) || 587,
  secure: process.env.SMTP_SECURE === 'true',  // true for 465, false for other ports
  auth: {
    user: process.env.SMTP_USER,      // Your email address
    pass: process.env.SMTP_PASSWORD,  // App password (not your real password)
  },
});

async function sendWelcomeEmail(to, name) {
  const info = await transporter.sendMail({
    from: '"My App" <noreply@myapp.com>',
    to,
    subject: `Welcome, ${name}!`,
    html: `
      <h1>Welcome, ${name}!</h1>
      <p>Thanks for joining our platform.</p>
      <p>Here are some getting started tips:</p>
      <ul>
        <li>Complete your profile</li>
        <li>Browse our documentation</li>
        <li>Join our community</li>
      </ul>
    `,
  });

  return info;
}

// Verify connection before sending
try {
  await transporter.verify();
  console.log('SMTP connection verified');
} catch (err) {
  console.error('SMTP connection failed:', err.message);
}

// Send
const info = await sendWelcomeEmail('user@example.com', 'Alice');
console.log('Email sent:', info.messageId);

// Always close the transporter when done (in scripts)
transporter.close();
```

## Multiple Recipients

```js
// multiple-recipients.js — Send to multiple recipients

import nodemailer from 'nodemailer';

const transporter = nodemailer.createTransport({ /* ... */ });

const info = await transporter.sendMail({
  from: '"Newsletter" <news@myapp.com>',

  // Multiple recipients
  to: 'alice@example.com, bob@example.com',

  // CC and BCC
  cc: 'manager@example.com',
  bcc: 'audit@example.com',  // Hidden from other recipients

  subject: 'Monthly Update',
  text: 'Here is your monthly update...',
  html: '<h1>Monthly Update</h1><p>Here is your monthly update...</p>',
});

// Check delivery status
console.log('Accepted:', info.accepted);   // Successfully queued
console.log('Rejected:', info.rejected);   // Failed to queue
console.log('Message ID:', info.messageId);
```

## How It Works

### Transport Lifecycle

```
Create transporter (configures SMTP connection)
    │
    │ verify() → check connection
    │
    │ sendMail() → connect to SMTP server
    │            → authenticate
    │            → send email data
    │            → receive confirmation
    │
    │ close() → disconnect
```

### SMTP Ports

| Port | Secure | Use |
|------|--------|-----|
| 25 | No | Unencrypted (blocked by most providers) |
| 587 | STARTTLS | Encrypted after handshake (recommended) |
| 465 | TLS | Encrypted from start |

## Common Mistakes

### Mistake 1: Hardcoding Credentials

```js
// WRONG — credentials in source code
const transporter = nodemailer.createTransport({
  auth: { user: 'my@gmail.com', pass: 'mypassword' },
});

// CORRECT — use environment variables
const transporter = nodemailer.createTransport({
  auth: {
    user: process.env.SMTP_USER,
    pass: process.env.SMTP_PASSWORD,
  },
});
```

### Mistake 2: Not Handling Errors

```js
// WRONG — sendMail throws on failure
await transporter.sendMail(mailOptions);
// Unhandled rejection if SMTP is down

// CORRECT — wrap in try/catch
try {
  await transporter.sendMail(mailOptions);
} catch (err) {
  console.error('Email failed:', err.message);
}
```

### Mistake 3: Not Verifying the Connection

```js
// WRONG — first sendMail call fails because SMTP is misconfigured
// User gets a timeout error after 30 seconds

// CORRECT — verify at startup
await transporter.verify();
console.log('SMTP ready');
```

## Try It Yourself

### Exercise 1: Send with Ethereal

Create an Ethereal account, send an email, and open the preview URL to see it.

### Exercise 2: Send HTML Email

Send an email with a styled HTML body (headings, bold, links, colors).

### Exercise 3: Error Handling

Configure an invalid SMTP host. Verify that the error is caught and handled gracefully.

## Next Steps

You can send basic emails. For HTML templates, continue to [HTML Templates](./02-html-templates.md).
