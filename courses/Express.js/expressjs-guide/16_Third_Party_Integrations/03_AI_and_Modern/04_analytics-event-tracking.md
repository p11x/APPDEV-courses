# Analytics Event Tracking

## 📌 What You'll Learn

- Server-side analytics
- Sending events to Mixpanel/PostHog

## 💻 Code Example

```js
// Analytics middleware
function trackEvent(userId, event, properties = {}) {
  const eventData = {
    event,
    properties: {
      ...properties,
      userId,
      timestamp: new Date().toISOString(),
      url: process.env.APP_URL
    }
  };
  
  // Send to analytics service
  // await mixpanel.track(event, eventData.properties);
  console.log('Tracked:', eventData);
}

// Track in routes
app.post('/signup', (req, res) => {
  const user = { id: 'user-123', email: 'test@example.com' };
  trackEvent(user.id, 'User Signed Up', { method: 'email' });
  res.json({ ok: true });
});
```

## ✅ Quick Recap

- Track events server-side for better accuracy
- Include user IDs and timestamps
- Use for Mixpanel, PostHog, Amplitude
