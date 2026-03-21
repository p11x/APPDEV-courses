# Email with Resend

## 📌 What You'll Learn

- Using Resend SDK
- Sending transactional emails

## 💻 Code Example

```js
import { Resend } from 'resend';

const resend = new Resend(process.env.RESEND_API_KEY);

app.post('/send', async (req, res) => {
  const { to, subject } = req.body;
  
  const data = await resend.emails.send({
    from: 'onboarding@resend.dev',
    to,
    subject,
    html: '<p>Hello!</p>'
  });
  
  res.json(data);
});
```
