# Push Notifications

## 📌 What You'll Learn

- Sending web push notifications
- VAPID keys and service workers

## 💻 Code Example

```js
import webpush from 'web-push';

const vapidKeys = {
  publicKey: process.env.VAPID_PUBLIC_KEY,
  privateKey: process.env.VAPID_PRIVATE_KEY
};

webpush.setVapidDetails(
  'mailto:admin@example.com',
  vapidKeys.publicKey,
  vapidKeys.privateKey
);

app.post('/subscribe', (req, res) => {
  const subscription = req.body;
  // Save to database
  res.status(201).json({ ok: true });
});

app.post('/notify', async (req, res) => {
  const { subscription, message } = req.body;
  
  await webpush.sendNotification(subscription, JSON.stringify({
    title: 'Notification',
    body: message
  }));
  
  res.json({ sent: true });
});
```

## ✅ Quick Recap

- Use web-push library for push notifications
- Generate VAPID keys for authentication
- Register service worker on client side
