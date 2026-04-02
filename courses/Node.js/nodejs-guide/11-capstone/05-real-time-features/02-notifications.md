# Notifications

## What You'll Learn

- Real-time notifications with WebSockets
- Notification storage and delivery
- Push notification patterns

## Notification System

```js
// services/notificationService.js

export class NotificationService {
  constructor(wss) {
    this.wss = wss;
  }

  notify(userId, notification) {
    this.wss.clients.forEach((client) => {
      if (client.userId === userId && client.readyState === 1) {
        client.send(JSON.stringify({
          type: 'notification',
          data: notification,
        }));
      }
    });
  }

  broadcast(notification) {
    const data = JSON.stringify({ type: 'notification', data: notification });
    this.wss.clients.forEach((client) => {
      if (client.readyState === 1) client.send(data);
    });
  }
}
```

## Next Steps

For live updates, continue to [Live Updates](./03-live-updates.md).
