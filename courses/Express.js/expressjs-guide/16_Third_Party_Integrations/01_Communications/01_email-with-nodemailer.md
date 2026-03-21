# Email with Nodemailer

## 📌 What You'll Learn

- Sending emails with Nodemailer
- SMTP configuration
- HTML templates

## 💻 Code Example

```js
import nodemailer from 'nodemailer';

const transporter = nodemailer.createTransport({
  host: process.env.SMTP_HOST,
  port: 587,
  auth: {
    user: process.env.SMTP_USER,
    pass: process.env.SMTP_PASS
  }
});

app.post('/send-email', async (req, res) => {
  const { to, subject, html } = req.body;
  
  await transporter.sendMail({
    from: '"App" <noreply@example.com>',
    to,
    subject,
    html
  });
  
  res.json({ sent: true });
});
```
