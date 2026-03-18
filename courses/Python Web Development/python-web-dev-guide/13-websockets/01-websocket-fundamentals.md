# WebSockets Fundamentals

## What You'll Learn
- WebSocket basics
- How WebSockets differ from HTTP

## Prerequisites
- Completed async web folder

## What Are WebSockets?

WebSockets provide full-duplex communication over a single TCP connection. Unlike HTTP where the client always initiates requests, WebSockets allow both client and server to send messages at any time.

## HTTP vs WebSocket

| Feature | HTTP | WebSocket |
|---------|------|-----------|
| Connection | Request-Response | Persistent |
| Direction | Client→Server | Bidirectional |
| Overhead | High (headers each request) | Low (after handshake) |
| Use Case | REST APIs | Real-time apps |

## Summary
- WebSockets enable real-time bidirectional communication
- Connection stays open after initial handshake
- Perfect for chat, notifications, live updates
