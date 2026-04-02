# HTML Email Templates

## What You'll Learn

- How to use Handlebars for email templates
- How to build responsive HTML email layouts
- How to inline CSS for email client compatibility
- How to pass dynamic data to templates
- How to create a reusable email service

## Project Setup

```bash
npm install nodemailer handlebars
```

## Template Engine

```js
// email-service.js — Reusable email service with templates

import nodemailer from 'nodemailer';
import Handlebars from 'handlebars';
import { readFile } from 'node:fs/promises';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));

// Create transporter
const transporter = nodemailer.createTransport({
  host: process.env.SMTP_HOST || 'smtp.ethereal.email',
  port: parseInt(process.env.SMTP_PORT) || 587,
  auth: {
    user: process.env.SMTP_USER,
    pass: process.env.SMTP_PASSWORD,
  },
});

// Template cache — load once, reuse
const templateCache = new Map();

async function loadTemplate(name) {
  if (templateCache.has(name)) {
    return templateCache.get(name);
  }

  const html = await readFile(
    resolve(__dirname, 'templates', `${name}.html`),
    'utf-8'
  );

  const compiled = Handlebars.compile(html);
  templateCache.set(name, compiled);
  return compiled;
}

// Send templated email
async function sendTemplatedEmail({ to, subject, template, data }) {
  // Load and compile the template
  const compiled = await loadTemplate(template);

  // Render the template with data
  const html = compiled(data);

  const info = await transporter.sendMail({
    from: '"My App" <noreply@myapp.com>',
    to,
    subject,
    html,          // Rendered HTML
    text: data.text || `View this email in an HTML-capable client.`,
  });

  return info;
}

// Export email functions
export async function sendWelcomeEmail(to, name) {
  return sendTemplatedEmail({
    to,
    subject: `Welcome, ${name}!`,
    template: 'welcome',
    data: {
      name,
      appName: 'My App',
      loginUrl: 'https://myapp.com/login',
      year: new Date().getFullYear(),
    },
  });
}

export async function sendPasswordResetEmail(to, name, resetUrl) {
  return sendTemplatedEmail({
    to,
    subject: 'Reset Your Password',
    template: 'password-reset',
    data: {
      name,
      resetUrl,
      expiresIn: '1 hour',
      year: new Date().getFullYear(),
    },
  });
}
```

## Welcome Email Template

```html
<!-- templates/welcome.html -->
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Welcome</title>
  <!-- Inline styles are required — email clients strip <style> tags -->
</head>
<body style="margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;">
  <table width="100%" cellpadding="0" cellspacing="0" style="max-width: 600px; margin: 0 auto; background-color: #ffffff;">
    <!-- Header -->
    <tr>
      <td style="background-color: #4f46e5; padding: 30px; text-align: center;">
        <h1 style="color: #ffffff; margin: 0; font-size: 28px;">{{appName}}</h1>
      </td>
    </tr>

    <!-- Body -->
    <tr>
      <td style="padding: 40px 30px;">
        <h2 style="color: #333333; margin-top: 0;">Welcome, {{name}}!</h2>
        <p style="color: #666666; font-size: 16px; line-height: 1.6;">
          Thanks for signing up. We're excited to have you on board.
        </p>

        <p style="text-align: center; margin: 30px 0;">
          <a href="{{loginUrl}}"
             style="background-color: #4f46e5; color: #ffffff; padding: 14px 30px; text-decoration: none; border-radius: 6px; font-size: 16px; display: inline-block;">
            Get Started
          </a>
        </p>

        <p style="color: #999999; font-size: 14px;">
          If the button doesn't work, copy and paste this link:<br>
          <a href="{{loginUrl}}" style="color: #4f46e5;">{{loginUrl}}</a>
        </p>
      </td>
    </tr>

    <!-- Footer -->
    <tr>
      <td style="background-color: #f8f8f8; padding: 20px 30px; text-align: center; border-top: 1px solid #eeeeee;">
        <p style="color: #999999; font-size: 12px; margin: 0;">
          &copy; {{year}} {{appName}}. All rights reserved.
        </p>
      </td>
    </tr>
  </table>
</body>
</html>
```

## Password Reset Template

```html
<!-- templates/password-reset.html -->
<!DOCTYPE html>
<html lang="en">
<head><meta charset="UTF-8"><title>Reset Password</title></head>
<body style="margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;">
  <table width="100%" cellpadding="0" cellspacing="0" style="max-width: 600px; margin: 0 auto; background-color: #ffffff;">
    <tr>
      <td style="background-color: #dc2626; padding: 30px; text-align: center;">
        <h1 style="color: #ffffff; margin: 0;">Password Reset</h1>
      </td>
    </tr>
    <tr>
      <td style="padding: 40px 30px;">
        <p style="color: #333333; font-size: 16px;">Hi {{name}},</p>
        <p style="color: #666666; font-size: 16px; line-height: 1.6;">
          We received a request to reset your password. Click the button below to set a new password. This link expires in {{expiresIn}}.
        </p>
        <p style="text-align: center; margin: 30px 0;">
          <a href="{{resetUrl}}"
             style="background-color: #dc2626; color: #ffffff; padding: 14px 30px; text-decoration: none; border-radius: 6px; font-size: 16px;">
            Reset Password
          </a>
        </p>
        <p style="color: #999999; font-size: 14px;">
          If you didn't request this, you can safely ignore this email.
        </p>
      </td>
    </tr>
    <tr>
      <td style="background-color: #f8f8f8; padding: 20px 30px; text-align: center;">
        <p style="color: #999999; font-size: 12px;">&copy; {{year}} My App</p>
      </td>
    </tr>
  </table>
</body>
</html>
```

## Using the Email Service

```js
// server.js — Send emails from your API

import express from 'express';
import { sendWelcomeEmail, sendPasswordResetEmail } from './email-service.js';

const app = express();
app.use(express.json());

app.post('/register', async (req, res) => {
  const { email, name } = req.body;

  // Create user in database...

  // Send welcome email
  await sendWelcomeEmail(email, name);

  res.status(201).json({ message: 'User created' });
});

app.post('/forgot-password', async (req, res) => {
  const { email } = req.body;

  // Generate reset token...
  const resetUrl = `https://myapp.com/reset?token=abc123`;

  await sendPasswordResetEmail(email, 'Alice', resetUrl);

  res.json({ message: 'Reset email sent' });
});

app.listen(3000);
```

## How It Works

### Template Rendering Flow

```
Handlebars template: "Welcome, {{name}}!"
    │
    ▼ Handlebars.compile(template) → compiled function
    │
    ▼ compiled({ name: 'Alice' }) → "Welcome, Alice!"
    │
    ▼ nodemailer sendMail({ html: renderedHtml })
```

### Why Inline CSS?

Email clients (Outlook, Gmail, Apple Mail) strip `<style>` tags and `<link>` stylesheets. Inline styles (`style="color: red"`) are the only reliable way to style emails.

## Common Mistakes

### Mistake 1: Using External CSS

```html
<!-- WRONG — email clients strip this -->
<link rel="stylesheet" href="styles.css">

<!-- CORRECT — inline styles -->
<p style="color: #333; font-size: 16px;">Text</p>
```

### Mistake 2: Missing Fallback Values

```html
<!-- WRONG — if name is undefined, shows "Welcome, !" -->
<h1>Welcome, {{name}}!</h1>

<!-- CORRECT — provide fallback -->
<h1>Welcome, {{name | default: 'there'}}!</h1>
<!-- Or in Handlebars: -->
<h1>Welcome, {{#if name}}{{name}}{{else}}there{{/if}}!</h1>
```

### Mistake 3: Not Caching Templates

```js
// WRONG — reads the file from disk every time
async function sendEmail(to, data) {
  const html = await readFile('template.html', 'utf-8');
  const compiled = Handlebars.compile(html);
  // ...
}

// CORRECT — compile once, reuse
const compiled = Handlebars.compile(template);
// Call compiled(data) for each email
```

## Try It Yourself

### Exercise 1: Welcome Email

Send a welcome email using the template. Open it in Ethereal and verify the styling.

### Exercise 2: Custom Template

Create a "payment receipt" template with order details (items, total, date). Render and send it.

### Exercise 3: Template Preview

Add a `GET /preview/:template` endpoint that renders a template with sample data and returns the HTML for previewing in a browser.

## Next Steps

You can send templated emails. For adding file attachments, continue to [Attachments](./03-attachments.md).
