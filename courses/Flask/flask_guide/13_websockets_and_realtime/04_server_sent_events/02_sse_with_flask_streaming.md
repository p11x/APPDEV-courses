<!-- FILE: 13_websockets_and_realtime/04_server_sent_events/02_sse_with_flask_streaming.md -->

## Overview

This file shows how to implement SSE with Flask streaming.

## Code Walkthrough

```python
# sse.py
from flask import Flask, Response
import time

app = Flask(__name__)

@app.route("/stream")
def stream():
    def generate():
        for i in range(10):
            yield f"data: {i}\n\n"
            time.sleep(1)
    
    return Response(generate(), mimetype="text/event-stream")

if __name__ == "__main__":
    app.run()
```
