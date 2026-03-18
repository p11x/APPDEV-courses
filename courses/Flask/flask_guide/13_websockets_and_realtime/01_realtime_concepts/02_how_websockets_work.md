<!-- FILE: 13_websockets_and_realtime/01_realtime_concepts/02_how_websockets_work.md -->

## Overview

WebSockets provide full-duplex communication over a single TCP connection. This file explains how they work.

## Core Concepts

WebSockets use a handshake:
1. Client sends HTTP upgrade request
2. Server responds with 101 Switching Protocols
3. Connection upgrades to WebSocket
4. Both sides can send messages anytime

## Next Steps

Continue to [03_when_to_use_realtime.md](03_when_to_use_realtime.md)
