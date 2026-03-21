# SMS with Twilio

## 📌 What You'll Learn

- Sending SMS with Twilio SDK

## 💻 Code Example

```js
import twilio from 'twilio';

const client = twilio(
  process.env.TWILIO_ACCOUNT_SID,
  process.env.TWILIO_AUTH_TOKEN
);

app.post('/sms', async (req, res) => {
  const { to, message } = req.body;
  
  const result = await client.messages.create({
    body: message,
    from: process.env.TWILIO_PHONE_NUMBER,
    to
  });
  
  res.json({ sid: result.sid });
});
```
