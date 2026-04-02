# Email Attachments

## What You'll Learn

- How to attach files to emails with nodemailer
- How to send files from disk, URL, or Buffer
- How to embed inline images in HTML emails
- How to set content types and filenames
- How to handle large attachments

## Basic Attachments

```js
// attachments.js — Sending emails with attachments

import nodemailer from 'nodemailer';

const transporter = nodemailer.createTransport({ /* ... */ });

await transporter.sendMail({
  from: '"Reports" <reports@myapp.com>',
  to: 'user@example.com',
  subject: 'Monthly Report - January 2024',
  text: 'Please find the monthly report attached.',
  html: '<p>Please find the monthly report attached.</p>',

  // Attachments array — each entry is a file
  attachments: [
    // File from disk
    {
      filename: 'report-jan-2024.pdf',           // Name shown in email client
      path: './reports/jan-2024.pdf',             // Path to the file
      contentType: 'application/pdf',             // MIME type (auto-detected if omitted)
    },

    // File from Buffer (e.g., generated in memory)
    {
      filename: 'summary.csv',
      content: Buffer.from('Name,Total\nAlice,100\nBob,200'),  // Raw bytes
      contentType: 'text/csv',
    },

    // File from URL
    {
      filename: 'logo.png',
      path: 'https://myapp.com/logo.png',         // Nodemailer downloads it
    },

    // File as string content
    {
      filename: 'notes.txt',
      content: 'Meeting notes from January...',
      contentType: 'text/plain',
    },
  ],
});
```

## Inline Images (CID)

Inline images appear **in the email body**, not as separate attachments. They use **Content IDs (CIDs)** — special references in the HTML.

```js
// inline-images.js — Embed images in HTML emails

await transporter.sendMail({
  from: '"My App" <noreply@myapp.com>',
  to: 'user@example.com',
  subject: 'Your Invoice',
  html: `
    <h1>Invoice #1234</h1>
    <!-- cid:logo references the attachment below -->
    <img src="cid:logo" alt="Logo" width="200">
    <p>Thank you for your purchase!</p>
    <img src="cid:chart" alt="Chart" width="400">
  `,
  attachments: [
    {
      filename: 'logo.png',
      path: './assets/logo.png',
      cid: 'logo',               // Content ID — must match src="cid:logo"
      // cid makes this an inline image, not an attachment
    },
    {
      filename: 'chart.png',
      content: generateChartBuffer(),  // Generated image buffer
      cid: 'chart',
    },
  ],
});
```

## Dynamic Attachments from Database

```js
// dynamic-attachments.js — Attach files generated from data

import nodemailer from 'nodemailer';
import { readFile } from 'node:fs/promises';

async function sendInvoiceEmail(customerEmail, orderId) {
  // Generate PDF from order data
  const pdfBuffer = await generateInvoicePDF(orderId);

  // Generate CSV export
  const csvContent = await generateOrderCSV(orderId);

  const transporter = nodemailer.createTransport({ /* ... */ });

  await transporter.sendMail({
    from: '"Orders" <orders@myapp.com>',
    to: customerEmail,
    subject: `Invoice for Order #${orderId}`,
    html: `
      <h1>Order #${orderId}</h1>
      <p>Thank you for your order. Please find your invoice attached.</p>
    `,
    attachments: [
      {
        filename: `invoice-${orderId}.pdf`,
        content: pdfBuffer,                  // Buffer from PDF generation
        contentType: 'application/pdf',
      },
      {
        filename: `order-${orderId}.csv`,
        content: csvContent,                 // String content
        contentType: 'text/csv',
      },
    ],
  });
}

// Simulated PDF generation
async function generateInvoicePDF(orderId) {
  // In production, use a library like pdfkit or puppeteer
  return Buffer.from(`PDF content for order ${orderId}`);
}

// Simulated CSV generation
async function generateOrderCSV(orderId) {
  return `Item,Quantity,Price\nWidget,2,$10.00\nGadget,1,$25.00`;
}
```

## Large Attachments

```js
// large-attachments.js — Handle large file attachments

import nodemailer from 'nodemailer';
import { createReadStream } from 'node:fs';
import { stat } from 'node:fs/promises';

async function sendLargeFile(to, filePath) {
  // Check file size before sending
  const { size } = await stat(filePath);
  const maxSize = 25 * 1024 * 1024;  // 25MB — most SMTP servers reject larger

  if (size > maxSize) {
    throw new Error(`File too large (${(size / 1024 / 1024).toFixed(1)}MB). Max: 25MB`);
  }

  const transporter = nodemailer.createTransport({ /* ... */ });

  // Use a read stream instead of loading the entire file into memory
  await transporter.sendMail({
    from: '"Files" <files@myapp.com>',
    to,
    subject: 'Large File Attachment',
    text: 'See attached file.',
    attachments: [
      {
        filename: 'large-data.csv',
        content: createReadStream(filePath),  // Stream — constant memory
      },
    ],
  });
}
```

## How It Works

### Attachment Formats

| Format | Content Property | Use Case |
|--------|-----------------|----------|
| `path` | File path or URL | Files on disk or remote |
| `content` | Buffer or String | Generated content |
| `raw` | Complete MIME | Custom MIME structures |

### CID vs Attachment

```
Attachment: appears in the "Attachments" section of the email
  attachments: [{ filename: 'logo.png', path: './logo.png' }]

Inline (CID): appears in the email body via <img src="cid:...">
  attachments: [{ filename: 'logo.png', path: './logo.png', cid: 'logo' }]
  html: '<img src="cid:logo">'
```

## Common Mistakes

### Mistake 1: Missing contentType

```js
// WRONG — email client may not know how to handle the file
attachments: [{ filename: 'data.bin', content: buffer }]

// CORRECT — specify the MIME type
attachments: [{ filename: 'data.bin', content: buffer, contentType: 'application/octet-stream' }]
```

### Mistake 2: Exceeding Size Limits

```js
// WRONG — attaching a 100MB file (SMTP rejects it)
attachments: [{ filename: 'huge.zip', path: './huge.zip' }]

// CORRECT — check size first, or use a cloud storage link
if (size > 25 * 1024 * 1024) {
  // Send a download link instead
  html = `<a href="${downloadUrl}">Download your file</a>`;
}
```

### Mistake 3: CID Mismatch

```html
<!-- WRONG — cid does not match -->
<img src="cid:logo">
<!-- Attachment: { cid: 'company-logo' } — different value! -->

<!-- CORRECT — cid values must match exactly -->
<img src="cid:logo">
<!-- Attachment: { cid: 'logo' } -->
```

## Try It Yourself

### Exercise 1: Send an Invoice PDF

Generate a simple PDF (or use a Buffer with text). Send it as an attachment with a styled HTML body.

### Exercise 2: Inline Logo

Embed a logo image in an HTML email using CID. Verify it appears in the email body.

### Exercise 3: Multiple Attachments

Send an email with 3 attachments: a PDF, a CSV, and an inline image.

## Next Steps

You can send emails with attachments. For transactional email APIs, continue to [Resend API](../email-services/01-resend-api.md).
