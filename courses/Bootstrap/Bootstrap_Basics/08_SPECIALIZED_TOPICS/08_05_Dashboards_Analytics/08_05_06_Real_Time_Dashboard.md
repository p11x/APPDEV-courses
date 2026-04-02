---
title: "Real-Time Dashboard"
module: "Dashboards & Analytics"
difficulty: 3
estimated_time: "30 min"
prerequisites: ["04_01_Card_Component", "04_09_Badges", "04_05_Forms"]
---

## Overview

Real-time dashboards display live data with auto-refresh capabilities, connection status indicators, and animated updates. Bootstrap 5 badges for live indicators, cards for real-time metrics, alerts for connection status, and progress indicators for refresh timers create responsive live monitoring interfaces.

## Basic Implementation

### Live Indicator Badge

```html
<div class="d-flex align-items-center gap-2 mb-4">
  <span class="badge bg-success d-flex align-items-center gap-1">
    <span class="rounded-circle bg-white" style="width:8px;height:8px;animation:pulse 2s infinite"></span>
    Live
  </span>
  <span class="text-muted small">Auto-refreshing every 30 seconds</span>
  <button class="btn btn-outline-secondary btn-sm ms-auto">
    <i class="bi bi-arrow-clockwise me-1"></i>Refresh Now
  </button>
</div>

<style>
  @keyframes pulse {
    0%, 100% { opacity: 1; }
    50% { opacity: 0.4; }
  }
</style>
```

### Real-Time KPI Cards

```html
<div class="row g-4 mb-4">
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start">
          <div>
            <p class="text-muted small mb-1">Active Users Now</p>
            <h3 class="mb-0" id="activeUsers">1,247</h3>
          </div>
          <span class="badge bg-success d-flex align-items-center gap-1">
            <span class="rounded-circle bg-white" style="width:6px;height:6px"></span>
            Live
          </span>
        </div>
        <div class="mt-2">
          <span class="badge bg-success bg-opacity-10 text-success">
            <i class="bi bi-arrow-up"></i> +23 in last min
          </span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start">
          <div>
            <p class="text-muted small mb-1">Requests / Second</p>
            <h3 class="mb-0">842</h3>
          </div>
          <span class="badge bg-success d-flex align-items-center gap-1">
            <span class="rounded-circle bg-white" style="width:6px;height:6px"></span>
            Live
          </span>
        </div>
        <div class="mt-2">
          <span class="text-muted small">Peak: 1,204 req/s</span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start">
          <div>
            <p class="text-muted small mb-1">Error Rate</p>
            <h3 class="mb-0 text-danger">2.4%</h3>
          </div>
          <span class="badge bg-danger d-flex align-items-center gap-1">
            <span class="rounded-circle bg-white" style="width:6px;height:6px"></span>
            Alert
          </span>
        </div>
        <div class="mt-2">
          <span class="badge bg-danger bg-opacity-10 text-danger">
            <i class="bi bi-arrow-up"></i> Above threshold
          </span>
        </div>
      </div>
    </div>
  </div>
  <div class="col-sm-6 col-xl-3">
    <div class="card h-100">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start">
          <div>
            <p class="text-muted small mb-1">Avg Response Time</p>
            <h3 class="mb-0">142ms</h3>
          </div>
          <span class="badge bg-success d-flex align-items-center gap-1">
            <span class="rounded-circle bg-white" style="width:6px;height:6px"></span>
            Live
          </span>
        </div>
        <div class="mt-2">
          <span class="badge bg-success bg-opacity-10 text-success">
            <i class="bi bi-arrow-down"></i> -12ms
          </span>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### WebSocket Connection Status

```html
<div class="alert alert-success d-flex align-items-center mb-4" role="status">
  <i class="bi bi-wifi me-2"></i>
  <div class="flex-grow-1">
    <strong>Connected</strong> - Real-time data stream active
  </div>
  <span class="badge bg-success">WebSocket</span>
</div>

<!-- Disconnected State -->
<div class="alert alert-warning d-flex align-items-center mb-4" role="status">
  <i class="bi bi-wifi-off me-2"></i>
  <div class="flex-grow-1">
    <strong>Connection Lost</strong> - Attempting to reconnect...
  </div>
  <button class="btn btn-sm btn-warning">Reconnect</button>
</div>
```

### Auto-Refresh Timer

```html
<div class="card">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Server Metrics</h5>
    <div class="d-flex align-items-center gap-2">
      <div class="progress" style="width:60px;height:4px">
        <div class="progress-bar bg-primary" id="refreshProgress" style="width:0%"></div>
      </div>
      <span class="text-muted small" id="refreshTimer">30s</span>
      <div class="dropdown">
        <button class="btn btn-sm btn-outline-secondary dropdown-toggle" data-bs-toggle="dropdown">
          <i class="bi bi-gear"></i>
        </button>
        <ul class="dropdown-menu dropdown-menu-end">
          <li><a class="dropdown-item" href="#">Refresh: 10s</a></li>
          <li><a class="dropdown-item active" href="#">Refresh: 30s</a></li>
          <li><a class="dropdown-item" href="#">Refresh: 60s</a></li>
          <li><a class="dropdown-item" href="#">Refresh: Off</a></li>
        </ul>
      </div>
    </div>
  </div>
  <div class="card-body">
    <div class="bg-light rounded d-flex align-items-center justify-content-center" style="height:300px">
      <p class="text-muted mb-0">Real-time metrics chart</p>
    </div>
  </div>
</div>
```

### Recent Events Feed

```html
<div class="card">
  <div class="card-header bg-white d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Live Events</h5>
    <span class="badge bg-success d-flex align-items-center gap-1">
      <span class="rounded-circle bg-white" style="width:6px;height:6px;animation:pulse 2s infinite"></span>
      Streaming
    </span>
  </div>
  <div class="list-group list-group-flush" style="max-height:400px;overflow-y:auto" id="eventFeed">
    <div class="list-group-item d-flex align-items-center">
      <div class="bg-success bg-opacity-10 rounded-circle p-2 me-3">
        <i class="bi bi-person-plus text-success"></i>
      </div>
      <div class="flex-grow-1">
        <p class="mb-0 small"><strong>New user</strong> signed up</p>
        <small class="text-muted">Just now</small>
      </div>
    </div>
    <div class="list-group-item d-flex align-items-center">
      <div class="bg-primary bg-opacity-10 rounded-circle p-2 me-3">
        <i class="bi bi-credit-card text-primary"></i>
      </div>
      <div class="flex-grow-1">
        <p class="mb-0 small"><strong>Payment</strong> received - $299.00</p>
        <small class="text-muted">5 seconds ago</small>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Show a pulsing live indicator badge on real-time data cards
2. Display WebSocket connection status prominently
3. Provide a manual "Refresh Now" button alongside auto-refresh
4. Use configurable refresh intervals (10s, 30s, 60s, Off)
5. Show a progress bar indicating time until next refresh
6. Use color-coded status badges: green (live), red (alert), yellow (warning)
7. Display recent event feeds with auto-scroll
8. Provide a reconnection mechanism when the connection drops
9. Show "just now" / "X seconds ago" timestamps on events
10. Use CSS animations sparingly to avoid distracting users

## Common Pitfalls

1. **No connection status** - Users don't know if data is live or stale. Always show connection state.
2. **Auto-refresh too frequent** - Refreshing every 5 seconds causes flicker. Use 30-60 second intervals.
3. **No manual refresh** - Users should be able to force an update.
4. **Missing reconnection** - WebSocket disconnects must auto-retry with backoff.
5. **Animations too distracting** - Pulsing indicators should be subtle, not flashy.
6. **No "refresh off" option** - Some users prefer static views. Provide an off switch.

## Accessibility Considerations

- Use `aria-live="polite"` on metric values that update in real-time
- Provide `role="status"` on the connection status alert
- Announce new events with `aria-live="polite"` (not `assertive` to avoid interrupting)
- Label the refresh interval selector with `aria-label="Auto-refresh interval"`
- Use `aria-label="Live data"` on pulsing indicator badges

## Responsive Behavior

On **mobile**, real-time KPI cards stack in 2 columns. Event feeds take full width. On **tablet**, cards can display in a 2x2 grid. On **desktop**, all 4 KPI cards display in a single row with the event feed and charts side by side.
