<!-- FILE: 13_websockets_and_realtime/05_realtime_projects/03_live_dashboard_updates.md -->

## Overview

Build a live dashboard with real-time data updates.

## Code Walkthrough

```python
# dashboard.py
from flask import Flask, render_template
from flask_socketio import SocketIO
import random
import threading
import time

app = Flask(__name__)
socketio = SocketIO(app)

# Background thread to send updates
def background_updates():
    while True:
        data = {
            "users": random.randint(100, 1000),
            "sales": random.randint(50, 500),
            "revenue": random.randint(1000, 10000)
        }
        socketio.emit("dashboard_update", data)
        time.sleep(5)

@app.route("/dashboard")
def dashboard():
    return render_template("dashboard.html")

if __name__ == "__main__":
    # Start background thread
    import threading
    threading.Thread(target=background_updates, daemon=True).start()
    socketio.run(app)
```
