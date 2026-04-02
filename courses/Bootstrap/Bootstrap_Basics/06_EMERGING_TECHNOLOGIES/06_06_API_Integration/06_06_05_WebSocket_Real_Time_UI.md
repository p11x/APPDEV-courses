---
title: "WebSocket Real-Time UI"
description: "Build real-time updating Bootstrap components with WebSocket, connection status indicators, and live data feeds"
difficulty: 3
tags: [websocket, real-time, live-data, connection-status, events]
prerequisites:
  - "JavaScript WebSocket API"
  - "Bootstrap 5 components"
  - "DOM manipulation"
---

## Overview

WebSocket provides full-duplex communication for real-time UI updates without polling. Bootstrap components can display live data feeds, connection status indicators, and real-time notifications. This section covers establishing WebSocket connections, handling connection states, updating Bootstrap components with incoming data, and building a live notification system with toast components.

## Basic Implementation

### WebSocket Connection with Status Badge

```html
<div class="d-flex align-items-center mb-3">
  <span class="badge rounded-pill me-2" id="wsStatus">
    <span class="status-dot"></span> Connecting...
  </span>
  <small class="text-muted" id="wsMessage">Establishing connection</small>
</div>

<ul class="list-group" id="liveFeed"></ul>

<script>
  const statusBadge = document.getElementById('wsStatus');
  const statusMsg = document.getElementById('wsMessage');
  const feed = document.getElementById('liveFeed');

  function setStatus(state, message) {
    const states = {
      connecting: { class: 'bg-warning text-dark', text: 'Connecting' },
      connected: { class: 'bg-success', text: 'Connected' },
      disconnected: { class: 'bg-danger', text: 'Disconnected' },
      reconnecting: { class: 'bg-info', text: 'Reconnecting' }
    };
    const s = states[state];
    statusBadge.className = `badge rounded-pill ${s.class}`;
    statusBadge.innerHTML = `<i class="bi bi-circle-fill me-1" style="font-size: 0.5rem;"></i>${s.text}`;
    statusMsg.textContent = message;
  }

  function connect() {
    setStatus('connecting', 'Establishing connection...');

    // Demo: simulate WebSocket with setTimeout
    setTimeout(() => {
      setStatus('connected', 'Live updates active');

      // Simulate incoming messages
      setInterval(() => {
        const item = document.createElement('li');
        item.className = 'list-group-item list-group-item-action';
        item.innerHTML = `
          <div class="d-flex justify-content-between">
            <strong>New Update</strong>
            <small class="text-muted">${new Date().toLocaleTimeString()}</small>
          </div>
          <p class="mb-0 small">Real-time data received at ${new Date().toISOString()}</p>`;
        feed.prepend(item);
        if (feed.children.length > 50) feed.lastChild.remove();
      }, 3000);
    }, 1500);
  }

  connect();
</script>
```

## Advanced Variations

### Live Toast Notifications

```html
<div class="toast-container position-fixed top-0 end-0 p-3" id="toastContainer"></div>

<button class="btn btn-primary" onclick="showLiveToast()">Simulate Event</button>

<script>
  function showLiveToast(data = {}) {
    const container = document.getElementById('toastContainer');
    const id = 'toast-' + Date.now();
    const time = new Date().toLocaleTimeString();

    const toastEl = document.createElement('div');
    toastEl.className = 'toast show';
    toastEl.id = id;
    toastEl.setAttribute('role', 'alert');
    toastEl.setAttribute('aria-live', 'assertive');
    toastEl.setAttribute('aria-atomic', 'true');
    toastEl.innerHTML = `
      <div class="toast-header">
        <i class="bi bi-bell-fill text-primary me-2"></i>
        <strong class="me-auto">${data.title || 'New Notification'}</strong>
        <small class="text-muted">${time}</small>
        <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
      </div>
      <div class="toast-body">${data.message || 'A real-time event has occurred.'}</div>`;

    container.appendChild(toastEl);
    const toast = new bootstrap.Toast(toastEl, { autohide: true, delay: 5000 });
    toastEl.addEventListener('hidden.bs.toast', () => toastEl.remove());
  }

  // Simulate WebSocket messages triggering toasts
  setInterval(() => {
    showLiveToast({
      title: 'Live Update',
      message: `Data sync completed at ${new Date().toLocaleTimeString()}`
    });
  }, 10000);
</script>
```

### Real-Time Data Dashboard Widget

```html
<div class="card">
  <div class="card-header d-flex justify-content-between align-items-center">
    <span>Live Metrics</span>
    <span class="badge bg-success" id="metricStatus">Live</span>
  </div>
  <div class="card-body">
    <div class="row text-center">
      <div class="col-4">
        <h4 class="mb-0" id="metric1">--</h4>
        <small class="text-muted">Active Users</small>
      </div>
      <div class="col-4">
        <h4 class="mb-0" id="metric2">--</h4>
        <small class="text-muted">Requests/sec</small>
      </div>
      <div class="col-4">
        <h4 class="mb-0" id="metric3">--</h4>
        <small class="text-muted">Latency (ms)</small>
      </div>
    </div>
  </div>
</div>

<script>
  // Simulate live metrics
  function updateMetrics() {
    document.getElementById('metric1').textContent = Math.floor(Math.random() * 500 + 100);
    document.getElementById('metric2').textContent = Math.floor(Math.random() * 200 + 50);
    document.getElementById('metric3').textContent = Math.floor(Math.random() * 100 + 10);
  }

  setInterval(updateMetrics, 2000);

  // Real WebSocket example (commented - use with actual server)
  // const ws = new WebSocket('wss://example.com/metrics');
  // ws.onmessage = (event) => {
  //   const data = JSON.parse(event.data);
  //   document.getElementById('metric1').textContent = data.users;
  //   document.getElementById('metric2').textContent = data.rps;
  //   document.getElementById('metric3').textContent = data.latency;
  // };
</script>
```

### Reconnection Logic

```html
<script>
  class WebSocketManager {
    constructor(url, options = {}) {
      this.url = url;
      this.reconnectDelay = options.reconnectDelay || 1000;
      this.maxReconnectAttempts = options.maxAttempts || 10;
      this.attempts = 0;
      this.onMessage = options.onMessage || (() => {});
      this.onStatusChange = options.onStatusChange || (() => {});
    }

    connect() {
      this.onStatusChange('connecting');
      this.ws = new WebSocket(this.url);

      this.ws.onopen = () => {
        this.attempts = 0;
        this.onStatusChange('connected');
      };

      this.ws.onmessage = (event) => {
        this.onMessage(JSON.parse(event.data));
      };

      this.ws.onclose = () => {
        this.onStatusChange('disconnected');
        this.reconnect();
      };

      this.ws.onerror = () => {
        this.ws.close();
      };
    }

    reconnect() {
      if (this.attempts >= this.maxReconnectAttempts) {
        this.onStatusChange('failed');
        return;
      }
      this.attempts++;
      this.onStatusChange('reconnecting');
      const delay = this.reconnectDelay * Math.pow(2, this.attempts - 1);
      setTimeout(() => this.connect(), Math.min(delay, 30000));
    }

    send(data) {
      if (this.ws?.readyState === WebSocket.OPEN) {
        this.ws.send(JSON.stringify(data));
      }
    }
  }
</script>
```

## Best Practices

1. **Always show connection status** - users need to know if data is live or stale.
2. **Implement reconnection** with exponential backoff to handle network interruptions.
3. **Limit DOM accumulation** - remove old items from live feeds to prevent memory issues.
4. **Use `prepend()` for feeds** - newest items should appear at the top.
5. **Use Bootstrap toasts** for real-time notifications with auto-hide timers.
6. **Buffer messages** during reconnection and replay them on reconnect.
7. **Sanitize incoming data** before rendering to prevent XSS from WebSocket messages.
8. **Close WebSocket connections** on page unload to prevent resource leaks.
9. **Use `JSON.parse` with try-catch** to handle malformed server messages.
10. **Set maximum reconnection attempts** to avoid infinite retry loops.
11. **Use `aria-live`** regions for real-time content updates.
12. **Provide a "Pause updates" button** for users who want to read without content shifting.

## Common Pitfalls

1. **Missing reconnection logic** - WebSocket connections drop on network changes.
2. **Not limiting feed size** causes DOM bloat and performance degradation.
3. **No error handling** on `JSON.parse` crashes on malformed messages.
4. **Memory leaks** from not closing WebSocket on component unmount.
5. **Updating DOM on every message** without throttling causes jank.
6. **Missing connection status UI** leaves users confused about data freshness.
7. **Not handling WebSocket `readyState`** - sending on closed connection throws errors.
8. **Hardcoding WebSocket URL** instead of using environment configuration.

## Accessibility Considerations

- Connection status changes should be announced via `aria-live="assertive"`.
- Live feed updates should use `aria-live="polite"` to avoid interrupting screen readers.
- Toast notifications require `role="alert"` and `aria-live="assertive"`.
- Provide keyboard-accessible controls for pausing/resuming live updates.
- Ensure real-time content changes don't steal focus from user interactions.
- Provide a way to review missed notifications for screen reader users.

## Responsive Behavior

- Toast containers should respect viewport edges on mobile.
- Live feed items should stack properly on narrow screens.
- Dashboard metrics should use responsive Bootstrap grid (`col-4 col-md-4`).
- Connection status indicators should remain visible at all screen sizes.
- Touch-friendly pause/resume controls for mobile users.
