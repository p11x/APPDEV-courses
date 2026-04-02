---
title: "Webhook Configuration"
description: "Build webhook configuration interfaces with URL inputs, event selectors, delivery logs, and retry settings using Bootstrap 5."
difficulty: 2
estimated_time: "35 minutes"
prerequisites:
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Modals"
  - "Bootstrap 5 Accordion"
---

## Overview

Webhook configuration UIs allow users to register endpoints that receive event notifications from your SaaS platform. Bootstrap 5's form controls, tables, and modals combine to create interfaces for adding webhook URLs, selecting subscribed events, viewing delivery history, and configuring retry policies.

The component must validate URLs, present event categories clearly, show delivery success/failure states, and provide payload inspection for debugging. This is a developer-facing feature that prioritizes functionality and clear error states.

## Basic Implementation

### Webhook URL Input Form

```html
<div class="card mb-3">
  <div class="card-header"><strong>Webhook Endpoint</strong></div>
  <div class="card-body">
    <div class="mb-3">
      <label for="webhookUrl" class="form-label">Endpoint URL</label>
      <div class="input-group">
        <span class="input-group-text"><i class="bi bi-link-45deg"></i></span>
        <input type="url" class="form-control" id="webhookUrl" placeholder="https://your-app.com/webhooks" required>
      </div>
      <div class="form-text">Must be a valid HTTPS URL.</div>
    </div>
    <div class="mb-3">
      <label for="webhookSecret" class="form-label">Signing Secret</label>
      <div class="input-group">
        <input type="password" class="form-control" id="webhookSecret" value="whsec_abc123xyz" readonly>
        <button class="btn btn-outline-secondary" type="button" id="toggleSecret">
          <i class="bi bi-eye"></i>
        </button>
        <button class="btn btn-outline-secondary" type="button" id="copySecret">
          <i class="bi bi-clipboard"></i>
        </button>
      </div>
      <div class="form-text">Use this to verify webhook signatures.</div>
    </div>
    <button class="btn btn-primary">Save Endpoint</button>
  </div>
</div>
```

### Event Subscription Checkboxes

```html
<div class="card mb-3">
  <div class="card-header"><strong>Subscribed Events</strong></div>
  <div class="card-body">
    <div class="row">
      <div class="col-md-6">
        <h6 class="text-muted">Orders</h6>
        <div class="form-check mb-2">
          <input class="form-check-input" type="checkbox" id="evtOrderCreated" checked>
          <label class="form-check-label" for="evtOrderCreated">order.created</label>
        </div>
        <div class="form-check mb-2">
          <input class="form-check-input" type="checkbox" id="evtOrderUpdated" checked>
          <label class="form-check-label" for="evtOrderUpdated">order.updated</label>
        </div>
        <div class="form-check mb-2">
          <input class="form-check-input" type="checkbox" id="evtOrderCompleted">
          <label class="form-check-label" for="evtOrderCompleted">order.completed</label>
        </div>
      </div>
      <div class="col-md-6">
        <h6 class="text-muted">Customers</h6>
        <div class="form-check mb-2">
          <input class="form-check-input" type="checkbox" id="evtCustomerCreated">
          <label class="form-check-label" for="evtCustomerCreated">customer.created</label>
        </div>
        <div class="form-check mb-2">
          <input class="form-check-input" type="checkbox" id="evtCustomerUpdated">
          <label class="form-check-label" for="evtCustomerUpdated">customer.updated</label>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Delivery Log Table

```html
<div class="table-responsive">
  <table class="table table-sm table-hover">
    <thead class="table-light">
      <tr>
        <th>Event</th>
        <th>Status</th>
        <th>Response</th>
        <th>Time</th>
        <th></th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><code>order.created</code></td>
        <td><span class="badge bg-success">200 OK</span></td>
        <td class="text-muted small">145ms</td>
        <td class="text-muted small">2 min ago</td>
        <td><button class="btn btn-sm btn-outline-secondary" data-bs-toggle="modal" data-bs-target="#payloadModal">View</button></td>
      </tr>
      <tr>
        <td><code>order.updated</code></td>
        <td><span class="badge bg-danger">500 Error</span></td>
        <td class="text-muted small">30000ms (timeout)</td>
        <td class="text-muted small">15 min ago</td>
        <td><button class="btn btn-sm btn-outline-secondary" data-bs-toggle="modal" data-bs-target="#payloadModal">View</button></td>
      </tr>
      <tr>
        <td><code>customer.created</code></td>
        <td><span class="badge bg-success">200 OK</span></td>
        <td class="text-muted small">89ms</td>
        <td class="text-muted small">1 hr ago</td>
        <td><button class="btn btn-sm btn-outline-secondary" data-bs-toggle="modal" data-bs-target="#payloadModal">View</button></td>
      </tr>
    </tbody>
  </table>
</div>
```

## Advanced Variations

### Retry Configuration

```html
<div class="card mb-3">
  <div class="card-header"><strong>Retry Settings</strong></div>
  <div class="card-body">
    <div class="mb-3">
      <label class="form-label">Retry Policy</label>
      <select class="form-select">
        <option selected>Exponential backoff (3 attempts)</option>
        <option>Linear (5 attempts)</option>
        <option>No retry</option>
      </select>
    </div>
    <div class="mb-3">
      <label class="form-label">Timeout (seconds)</label>
      <input type="number" class="form-control" value="30" min="5" max="120" style="max-width: 120px;">
    </div>
    <div class="form-check form-switch mb-3">
      <input class="form-check-input" type="checkbox" role="switch" id="autoDisable" checked>
      <label class="form-check-label" for="autoDisable">Auto-disable after 10 consecutive failures</label>
    </div>
    <button class="btn btn-primary btn-sm">Save Settings</button>
  </div>
</div>
```

### Payload Inspection Modal

```html
<div class="modal fade" id="payloadModal" tabindex="-1" aria-labelledby="payloadModalLabel">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="payloadModalLabel">Webhook Delivery Detail</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <ul class="nav nav-tabs mb-3">
          <li class="nav-item">
            <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#requestTab">Request</button>
          </li>
          <li class="nav-item">
            <button class="nav-link" data-bs-toggle="tab" data-bs-target="#responseTab">Response</button>
          </li>
        </ul>
        <div class="tab-content">
          <div class="tab-pane fade show active" id="requestTab">
            <pre class="bg-dark text-light p-3 rounded small"><code>{
  "event": "order.created",
  "timestamp": "2026-04-02T10:30:00Z",
  "data": {
    "order_id": "ord_abc123",
    "total": 99.99,
    "currency": "USD"
  }
}</code></pre>
          </div>
          <div class="tab-pane fade" id="responseTab">
            <pre class="bg-dark text-light p-3 rounded small"><code>HTTP/1.1 200 OK
Content-Type: application/json

{"received": true}</code></pre>
          </div>
        </div>
      </div>
      <div class="modal-footer">
        <button class="btn btn-outline-primary btn-sm"><i class="bi bi-arrow-clockwise me-1"></i>Retry Delivery</button>
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>
```

### Delivery Health Indicator

```html
<div class="d-flex align-items-center gap-3 mb-3">
  <div class="d-flex align-items-center">
    <span class="bg-success rounded-circle me-2" style="width: 10px; height: 10px;"></span>
    <span class="small">Healthy</span>
  </div>
  <div class="text-muted small">98.5% success rate (last 24h)</div>
  <span class="badge bg-light text-dark">142 deliveries</span>
</div>
```

## Best Practices

1. Require HTTPS URLs and validate URL format before saving
2. Show the signing secret with a toggle visibility button for security
3. Group events by category (Orders, Customers, Payments) for easier scanning
4. Display delivery status with color-coded badges (green for 2xx, red for 5xx)
5. Include response time in the delivery log for performance monitoring
6. Provide payload inspection with syntax-highlighted JSON
7. Implement retry configuration with sensible defaults (exponential backoff)
8. Auto-disable webhooks after consecutive failures to prevent cascading issues
9. Use `type="url"` on the endpoint input for browser-native URL validation
10. Include a "Test" button to send a test event and verify the endpoint
11. Show last successful delivery timestamp for health monitoring
12. Use `font-monospace` for webhook URLs and event names
13. Log all delivery attempts with full request/response payloads

## Common Pitfalls

1. **Not requiring HTTPS**: HTTP endpoints expose webhook payloads to interception. Always validate the URL uses HTTPS.
2. **Showing signing secret in plain text by default**: The secret should be masked by default with a reveal toggle.
3. **No delivery log**: Without delivery history, users cannot debug webhook integration issues.
4. **Missing retry configuration**: Hardcoded retry behavior does not fit all use cases. Make it configurable.
5. **No consecutive failure handling**: Endpoints that are down should be auto-disabled to prevent resource waste.
6. **Payload not formatted**: Showing raw JSON without syntax formatting makes debugging difficult.
7. **No test event capability**: Users need to verify their endpoint works before going live.

## Accessibility Considerations

- Use `type="url"` for proper keyboard behavior on mobile devices
- Associate all checkboxes with `label` elements using `for`/`id`
- Use `role="alert"` on delivery failure notifications
- Provide `aria-label` on visibility toggle and copy buttons
- Ensure modal tabs are keyboard navigable with arrow keys
- Use `aria-live="polite"` on delivery status updates
- Include text alternatives for color-coded status indicators

## Responsive Behavior

On mobile, event subscription columns should stack using `col-12` instead of `col-md-6`. The delivery log table should use `table-responsive`. The payload inspection modal should use `modal-fullscreen-sm-down`. Form inputs should remain full-width on small screens. Action buttons should maintain adequate touch target sizes.
