# When to Use Real-Time Features

## What You'll Learn
- Identify scenarios that actually need real-time updates
- Understand when polling, SSE, or WebSockets are appropriate
- Make informed architectural decisions

## Prerequisites
- Understanding of polling, SSE, and WebSockets from previous file

## Do I Need This Right Now?
If you're building a simple blog, portfolio site, or static content website, you can safely skip this topic. Real-time features add complexity and cost, so only reach for them when the user experience genuinely requires instant updates. Come back here when you're building a chat app, live dashboard, or collaborative tool.

## Concept Explained Simply

Not every application needs real-time updates. Think of it like deciding whether to hire a personal assistant who waits by your door versus one who checks in every hour. The personal assistant who waits by your door (WebSockets) costs more but delivers messages instantly. The one who checks hourly (polling) is cheaper but slower. Server-Sent Events are somewhere in between — like having an intercom that buzzes you when something important happens.

### Use Polling When:
- Data changes infrequently (every few minutes or longer)
- You don't need instant updates
- Simplicity is more important than responsiveness
- Examples: Checking email (inbox refresh), news feeds, weather updates

### Use SSE When:
- You only need one-way communication (server to client)
- You want simpler setup than WebSockets
- You need automatic reconnection
- Examples: Live notifications, stock ticker updates, social media feeds

### Use WebSockets When:
- You need bidirectional communication (both ways)
- Millisecond latency matters
- High-frequency updates in both directions
- Examples: Chat applications, multiplayer games, collaborative editing

## Common Mistakes

### Mistake #1: Using WebSockets for Everything
```typescript
// Wrong: Overengineering a simple notification system
// Using WebSockets when SSE would suffice
const ws = new WebSocket('wss://api.example.com/notifications');
ws.onmessage = (event) => {
  showNotification(JSON.parse(event.data));
};
```

```typescript
// Better: Using SSE for one-way server-to-client updates
const eventSource = new EventSource('/api/notifications');
eventSource.onmessage = (event) => {
  showNotification(JSON.parse(event.data));
};
```

### Mistake #2: Not Considering Alternatives First
```typescript
// Wrong: Building a complex real-time system when simple polling works
// This adds unnecessary infrastructure complexity
const ws = new WebSocket('wss://api.example.com/live-updates');
```

```typescript
// Better: Start with simple polling, upgrade only if needed
// Only 5 requests per minute, very low server load
setInterval(async () => {
  const data = await fetch('/api/latest-data').then(r => r.json());
  updateUI(data);
}, 12000); // Every 12 seconds
```

### Mistake #3: Ignoring the Cost-Benefit Analysis
Real-time features require:
- More complex server infrastructure
- Additional maintenance
- More difficult debugging
- Higher costs for hosting

Only implement when the user experience improvement justifies the added complexity.

## Summary
- Use polling for infrequent updates and simplest implementation
- Use SSE for one-way server-to-client updates with automatic reconnection
- Use WebSockets only when you need bidirectional, high-frequency communication
- Always consider if the added complexity is worth the user experience benefit
- Start simple, upgrade only when necessary

## Next Steps
- [nextjs-realtime-limitations.md](./nextjs-realtime-limitations.md) — Understanding what Next.js doesn't support natively
