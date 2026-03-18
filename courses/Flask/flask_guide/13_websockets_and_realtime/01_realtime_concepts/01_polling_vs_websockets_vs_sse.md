<!-- FILE: 13_websockets_and_realtime/01_realtime_concepts/01_polling_vs_websockets_vs_sse.md -->

## Overview

Real-time web applications need to push data to clients instantly. This file compares three approaches: polling, Server-Sent Events (SSE), and WebSockets.

## Core Concepts

| Method | Description | Pros | Cons |
|--------|-------------|------|------|
| **Polling** | Client repeatedly asks for updates | Simple | Wasteful, delayed |
| **Long Polling** | Client waits for server response | Reduced requests | Complex |
| **SSE** | Server pushes to client | Simple, HTTP/1.1 | One-way, limited connections |
| **WebSockets** | Bidirectional persistent connection | Full duplex, efficient | More complex |

## Code Walkthrough

```python
# ============================================
# 1. Polling (Simple but inefficient)
# ============================================

from flask import Flask, jsonify
import time

app = Flask(__name__)

# Client polls every 2 seconds
# @app.route("/status")
# def get_status():
#     return jsonify({"status": check_status()})

# ============================================
# 2. Long Polling (Better)
# ============================================

# Server holds request until data available
# @app.route("/long-poll")
# def long_poll():
#     while not has_new_data():
#         time.sleep(1)
#     return jsonify({"data": get_data()})

# ============================================
# 3. Server-Sent Events (SSE)
# ============================================

@app.route("/events")
def sse_stream():
    """Server pushes updates to client"""
    def generate():
        for i in range(10):
            yield f"data: {i}\n\n"
            time.sleep(1)
    
    from flask import Response
    return Response(generate(), mimetype="text/event-stream")

# ============================================
# 4. WebSockets (Bidirectional)
# ============================================

# See Flask-SocketIO examples

if __name__ == "__main__":
    app.run(debug=True)
```

## Next Steps

Continue to [02_how_websockets_work.md](02_how_websockets_work.md)
