<!-- FILE: 13_websockets_and_realtime/04_server_sent_events/03_sse_vs_websockets_tradeoffs.md -->

## Overview

Compare SSE and WebSockets trade-offs.

## Comparison

| Aspect | SSE | WebSockets |
|--------|-----|------------|
| Direction | Server→Client | Bidirectional |
| HTTP/HTTPS | HTTP | WS/WSS |
| Browser support | Good | Good |
| Complexity | Simple | Moderate |
| Auto-reconnect | Built-in | Manual |

## When to Use

- **SSE**: Notifications, feeds, one-way updates
- **WebSockets**: Chat, gaming, real-time collaboration
