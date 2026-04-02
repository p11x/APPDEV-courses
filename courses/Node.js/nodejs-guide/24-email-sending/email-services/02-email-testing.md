# Email Testing

## What You'll Learn

- How to use Ethereal for testing emails in development
- How to capture and preview emails without sending them
- How to use nodemailer's test account feature
- How to set up email testing in automated tests
- How to mock email sending in unit tests

## Why Test Emails?

In development, you do not want real emails sent to real users. **Ethereal** is a fake SMTP server that captures emails and provides a web interface to view them.

## Ethereal (Fake SMTP)

```js
// ethereal.js — Send and preview emails with Ethereal

import nodemailer from 'nodemailer';

async function main() {
  // Create a test account — no signup needed, free, temporary
  const testAccount = await nodemailer.createTestAccount();

  console.log('Ethereal credentials:');
  console.log('  User:', testAccount.user);
  console.log('  Pass:', testAccount.pass);

  // Create transporter using Ethereal SMTP
  const transporter = nodemailer.createTransport({
    host: 'smtp.ethereal.email',
    port: 587,
    secure: false,
    auth: {
      user: testAccount.user,
      pass: testAccount.pass,
    },
  });

  // Send a test email
  const info = await transporter.sendMail({
    from: '"Test App" <test@myapp.com>',
    to: 'recipient@example.com',
    subject: 'Test Email',
    html: `
      <h1>Test Email</h1>
      <p>This is a test email sent via Ethereal.</p>
      <p>Time: ${new Date().toISOString()}</p>
    `,
  });

  console.log('Message ID:', info.messageId);

  // Preview URL — open in browser to see the email
  const previewUrl = nodemailer.getTestMessageUrl(info);
  console.log('Preview URL:', previewUrl);
  // https://ethereal.email/message/abc123...
  // Click the link to see the rendered email in your browser
}

main();
```

## Environment-Based Configuration

```js
// email-config.js — Use Ethereal in dev, real SMTP in prod

import nodemailer from 'nodemailer';

async function createTransporter() {
  if (process.env.NODE_ENV === 'production') {
    // Production: real SMTP
    return nodemailer.createTransport({
      host: process.env.SMTP_HOST,
      port: parseInt(process.env.SMTP_PORT) || 587,
      auth: {
        user: process.env.SMTP_USER,
        pass: process.env.SMTP_PASSWORD,
      },
    });
  }

  // Development: Ethereal
  const testAccount = await nodemailer.createTestAccount();
  console.log(`[DEV] Ethereal: ${testAccount.user}`);

  return nodemailer.createTransport({
    host: 'smtp.ethereal.email',
    port: 587,
    auth: {
      user: testAccount.user,
      pass: testAccount.pass,
    },
  });
}

export async function sendEmail(options) {
  const transporter = await createTransporter();
  const info = await transporter.sendMail(options);

  // In dev, show the preview URL
  if (process.env.NODE_ENV !== 'production') {
    console.log('Preview:', nodemailer.getTestMessageUrl(info));
  }

  return info;
}
```

## Mocking in Unit Tests

```js
// email.test.js — Unit test with mocked email sending

import { describe, it, mock } from 'node:test';
import assert from 'node:assert';

// Mock nodemailer before importing the module under test
const mockSendMail = mock.fn(async () => ({ messageId: 'test-123' }));

mock.module('nodemailer', {
  namedExports: {
    createTransport: () => ({
      sendMail: mockSendMail,
      verify: async () => true,
      close: () => {},
    }),
    createTestAccount: async () => ({
      user: 'test@ethereal.email',
      pass: 'test123',
    }),
    getTestMessageUrl: () => 'https://ethereal.email/test',
  },
});

// Import after mocking
const { sendWelcomeEmail } = await import('./email-service.js');

describe('Email Service', () => {
  it('should send welcome email', async () => {
    await sendWelcomeEmail('user@example.com', 'Alice');

    // Verify sendMail was called with correct arguments
    assert.strictEqual(mockSendMail.mock.calls.length, 1);

    const callArgs = mockSendMail.mock.calls[0].arguments[0];
    assert.strictEqual(callArgs.to, 'user@example.com');
    assert.ok(callArgs.subject.includes('Welcome'));
    assert.ok(callArgs.html.includes('Alice'));
  });
});
```

## Integration Testing with MailHog

For integration tests, use [MailHog](https://github.com/mailhog/MailHog) — a local SMTP server with an API:

```bash
# Run MailHog with Docker
docker run -d -p 1025:1025 -p 8025:8025 mailhog/mailhog

# SMTP on port 1025, Web UI on http://localhost:8025
```

```js
// integration-test.js — Test with MailHog

import nodemailer from 'nodemailer';
import assert from 'node:assert';

const transporter = nodemailer.createTransport({
  host: 'localhost',
  port: 1025,  // MailHog SMTP port
  secure: false,
});

// Send an email
await transporter.sendMail({
  from: 'test@myapp.com',
  to: 'user@example.com',
  subject: 'Integration Test',
  text: 'Hello from integration test',
});

// Query MailHog's API to verify the email was received
const response = await fetch('http://localhost:8025/api/v2/search?kind=to&query=user@example.com');
const { items } = await response.json();

assert.strictEqual(items.length, 1);
assert.strictEqual(items[0].Content.Headers.Subject[0], 'Integration Test');

console.log('Integration test passed!');
```

## How It Works

### Testing Levels

| Level | Tool | What It Tests |
|-------|------|--------------|
| Unit | Mock nodemailer | Correct arguments passed to sendMail |
| Integration | MailHog / Ethereal | Email actually sent and received |
| E2E | Real SMTP (staging) | End-to-end delivery |

## Common Mistakes

### Mistake 1: Sending Real Emails in Tests

```js
// WRONG — tests send real emails to real addresses
const transporter = nodemailer.createTransport({ host: 'smtp.gmail.com', ... });
await transporter.sendMail({ to: 'real-user@example.com' });

// CORRECT — use Ethereal or mocks in tests
const testAccount = await nodemailer.createTestAccount();
const transporter = nodemailer.createTransport({ host: 'smtp.ethereal.email' });
```

### Mistake 2: Not Resetting Mocks Between Tests

```js
// WRONG — mock state leaks between tests
it('test 1', async () => { mockSendMail.mock.calls.length = 0; /* ... */ });
it('test 2', async () => {
  // mockSendMail still has calls from test 1
});

// CORRECT — reset mocks before each test
import { beforeEach } from 'node:test';
beforeEach(() => { mockSendMail.mock.resetCalls(); });
```

### Mistake 3: Testing Email Content Without Assertions

```js
// WRONG — just checking it does not throw
await sendWelcomeEmail('test@test.com', 'Alice');

// CORRECT — verify the content
const callArgs = mockSendMail.mock.calls[0].arguments[0];
assert.ok(callArgs.html.includes('Alice'));
assert.ok(callArgs.subject.includes('Welcome'));
```

## Try It Yourself

### Exercise 1: Ethereal Preview

Send an email via Ethereal and open the preview URL in your browser. Verify the HTML renders correctly.

### Exercise 2: Mock Test

Write a unit test that verifies the `sendPasswordResetEmail` function passes the correct reset URL to `sendMail`.

### Exercise 3: MailHog Integration

Set up MailHog with Docker. Send an email. Query the MailHog API to verify it was received.

## Next Steps

You can test email sending. For scheduled job execution, continue to [Chapter 25: Scheduled Jobs](../../25-scheduled-jobs/node-cron/01-cron-basics.md).
