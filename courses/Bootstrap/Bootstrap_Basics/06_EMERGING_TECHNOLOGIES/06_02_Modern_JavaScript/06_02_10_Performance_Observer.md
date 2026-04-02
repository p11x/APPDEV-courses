---
title: "PerformanceObserver with Bootstrap"
slug: "performance-observer-bootstrap"
difficulty: 3
tags: ["bootstrap", "javascript", "performance", "metrics", "monitoring"]
prerequisites:
  - "06_02_09_Resize_Observer"
  - "06_02_01_Intersection_Observer"
related:
  - "06_02_11_Web_Workers_UI"
  - "06_02_12_Broadcast_Channel"
duration: "35 minutes"
---

# PerformanceObserver with Bootstrap

## Overview

PerformanceObserver enables real-time monitoring of browser performance metrics including paint timing, layout shifts, long tasks, and resource loading. When building Bootstrap applications, this API helps measure how quickly components render, detect janky interactions, and identify bottlenecks. This data drives performance budgets, lazy loading decisions, and user experience optimization. Bootstrap components like modals, carousels, and dropdowns have measurable render times that PerformanceObserver can capture.

## Basic Implementation

Monitor Core Web Vitals (LCP, CLS, FID) and display them in a Bootstrap card.

```html
<div class="container mt-4">
  <div class="row g-3" id="metricsDashboard">
    <div class="col-md-4">
      <div class="card text-center">
        <div class="card-body">
          <h6 class="text-muted">Largest Contentful Paint</h6>
          <div class="fs-2 fw-bold" id="lcpValue">--</div>
          <span class="badge" id="lcpBadge">Measuring</span>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="card text-center">
        <div class="card-body">
          <h6 class="text-muted">Cumulative Layout Shift</h6>
          <div class="fs-2 fw-bold" id="clsValue">--</div>
          <span class="badge" id="clsBadge">Measuring</span>
        </div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="card text-center">
        <div class="card-body">
          <h6 class="text-muted">Interaction to Next Paint</h6>
          <div class="fs-2 fw-bold" id="inpValue">--</div>
          <span class="badge" id="inpBadge">Measuring</span>
        </div>
      </div>
    </div>
  </div>
</div>

<script>
function setMetric(id, value, thresholds) {
  const el = document.getElementById(`${id}Value`);
  const badge = document.getElementById(`${id}Badge`);
  el.textContent = typeof value === 'number' ? `${value.toFixed(0)}ms` : value;

  if (value <= thresholds.good) {
    badge.className = 'badge bg-success';
    badge.textContent = 'Good';
  } else if (value <= thresholds.ok) {
    badge.className = 'badge bg-warning';
    badge.textContent = 'Needs Improvement';
  } else {
    badge.className = 'badge bg-danger';
    badge.textContent = 'Poor';
  }
}

// LCP
new PerformanceObserver(list => {
  const entries = list.getEntries();
  const last = entries[entries.length - 1];
  setMetric('lcp', last.startTime, { good: 2500, ok: 4000 });
}).observe({ type: 'largest-contentful-paint', buffered: true });

// CLS
let clsValue = 0;
new PerformanceObserver(list => {
  for (const entry of list.getEntries()) {
    if (!entry.hadRecentInput) clsValue += entry.value;
  }
  document.getElementById('clsValue').textContent = clsValue.toFixed(3);
  const badge = document.getElementById('clsBadge');
  badge.className = `badge bg-${clsValue < 0.1 ? 'success' : clsValue < 0.25 ? 'warning' : 'danger'}`;
  badge.textContent = clsValue < 0.1 ? 'Good' : clsValue < 0.25 ? 'Needs Improvement' : 'Poor';
}).observe({ type: 'layout-shift', buffered: true });
</script>
```

## Advanced Variations

### Bootstrap Component Render Timing

Measure how long specific Bootstrap components take to initialize and render.

```html
<div class="card mb-3">
  <div class="card-header d-flex justify-content-between">
    <h5 class="mb-0">Component Performance</h5>
    <button class="btn btn-sm btn-outline-primary" id="measureBtn">Run Benchmark</button>
  </div>
  <div class="card-body">
    <table class="table table-sm" id="perfTable">
      <thead>
        <tr>
          <th>Component</th>
          <th>Init Time</th>
          <th>Render Time</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody id="perfTableBody"></tbody>
    </table>
  </div>
</div>

<script>
document.getElementById('measureBtn').addEventListener('click', async () => {
  const results = [];

  // Measure modal creation
  const modalStart = performance.now();
  const modalEl = document.createElement('div');
  modalEl.className = 'modal fade';
  modalEl.innerHTML = '<div class="modal-dialog"><div class="modal-content"><div class="modal-body">Test</div></div></div>';
  document.body.appendChild(modalEl);
  const modal = new bootstrap.Modal(modalEl);
  const modalInit = performance.now();
  modal.show();
  await new Promise(r => setTimeout(r, 100));
  const modalRender = performance.now();
  modal.hide();
  modalEl.remove();

  results.push({
    name: 'Modal',
    init: modalInit - modalStart,
    render: modalRender - modalInit
  });

  // Measure tooltip creation
  const tooltipStart = performance.now();
  const tooltipTrigger = document.createElement('button');
  tooltipTrigger.className = 'btn btn-secondary';
  tooltipTrigger.setAttribute('data-bs-toggle', 'tooltip');
  tooltipTrigger.setAttribute('title', 'Test tooltip');
  document.body.appendChild(tooltipTrigger);
  const tooltip = new bootstrap.Tooltip(tooltipTrigger);
  const tooltipEnd = performance.now();
  tooltip.dispose();
  tooltipTrigger.remove();

  results.push({
    name: 'Tooltip',
    init: tooltipEnd - tooltipStart,
    render: 0
  });

  // Render results
  document.getElementById('perfTableBody').innerHTML = results.map(r => `
    <tr>
      <td>${r.name}</td>
      <td>${r.init.toFixed(2)}ms</td>
      <td>${r.render.toFixed(2)}ms</td>
      <td><span class="badge bg-${r.init < 50 ? 'success' : r.init < 200 ? 'warning' : 'danger'}">
        ${r.init < 50 ? 'Fast' : r.init < 200 ? 'Normal' : 'Slow'}
      </span></td>
    </tr>
  `).join('');
});
</script>
```

### Long Task Detection

Identify long tasks that block the main thread and affect Bootstrap component responsiveness.

```html
<div class="alert alert-warning d-none" id="longTaskAlert">
  <h6><i class="bi bi-exclamation-triangle"></i> Long Tasks Detected</h6>
  <ul id="longTaskList" class="mb-0"></ul>
</div>

<script>
let longTasks = [];

try {
  new PerformanceObserver(list => {
    for (const entry of list.getEntries()) {
      longTasks.push({
        duration: entry.duration,
        startTime: entry.startTime,
        attribution: entry.attribution?.[0]?.name || 'Unknown'
      });

      const alert = document.getElementById('longTaskAlert');
      alert.classList.remove('d-none');
      document.getElementById('longTaskList').innerHTML = longTasks.slice(-5).map(t =>
        `<li>${t.duration.toFixed(0)}ms task at ${t.startTime.toFixed(0)}ms - ${t.attribution}</li>`
      ).join('');
    }
  }).observe({ type: 'longtask', buffered: true });
} catch (e) {
  console.warn('Long task observer not supported');
}
</script>
```

### Resource Timing for Bootstrap Assets

Monitor loading performance of Bootstrap CSS, JS, and icon fonts.

```html
<div class="card">
  <div class="card-header">
    <h6 class="mb-0">Resource Load Times</h6>
  </div>
  <div class="card-body">
    <div id="resourceMetrics" class="row g-2"></div>
  </div>
</div>

<script>
function reportResources() {
  const resources = performance.getEntriesByType('resource');
  const bootstrapResources = resources.filter(r =>
    r.name.includes('bootstrap') || r.name.includes('bootstrap-icons')
  );

  document.getElementById('resourceMetrics').innerHTML = bootstrapResources.map(r => {
    const size = r.transferSize ? `${(r.transferSize / 1024).toFixed(1)}KB` : 'cached';
    const time = r.duration.toFixed(0);
    const status = r.duration < 200 ? 'success' : r.duration < 500 ? 'warning' : 'danger';
    return `
      <div class="col-md-6">
        <div class="border rounded p-2">
          <div class="d-flex justify-content-between">
            <small class="text-truncate">${r.name.split('/').pop()}</small>
            <span class="badge bg-${status}">${time}ms</span>
          </div>
          <small class="text-muted">${size}</small>
        </div>
      </div>
    `;
  }).join('');
}

window.addEventListener('load', () => setTimeout(reportResources, 100));
</script>
```

## Best Practices

1. Use `buffered: true` to capture performance entries that occurred before the observer was created
2. Only observe entry types supported by the current browser to avoid errors
3. Send performance data to analytics in batches rather than per-entry to reduce network overhead
4. Set performance budgets and alert when metrics exceed thresholds
5. Use `performance.mark()` and `performance.measure()` alongside PerformanceObserver for custom timing
6. Filter out layout shifts caused by user interaction using `entry.hadRecentInput`
7. Aggregate metrics across sessions for meaningful trend analysis
8. Disconnect observers after initial measurement to reduce runtime overhead
9. Use `requestIdleCallback` to send beacon data when the browser is idle
10. Normalize metrics across different devices and connection speeds
11. Store performance data in `PerformanceObserver` callback scope to avoid global pollution
12. Use Web Vitals library as a reference implementation for metric calculation
13. Test with Chrome DevTools Performance panel to validate observed metrics
14. Account for third-party scripts that may affect Bootstrap component timing

## Common Pitfalls

1. **Missing buffered flag**: Missing paint entries that fired before the observer was created
2. **Browser support**: Assuming all entry types are available across browsers
3. **Metric misinterpretation**: Treating LCP as total page load time
4. **Observer leaks**: Not disconnecting observers leading to continuous callbacks
5. **Unnecessary overhead**: Observing performance in production without rate limiting
6. **Attribution gaps**: Not linking poor metrics to specific Bootstrap components
7. **CLS inflation**: Counting layout shifts from intentional animations or user-triggered changes

## Accessibility Considerations

Performance dashboards should be keyboard navigable with proper focus management. Use `aria-label` on metric cards describing the metric name and current value. Announce critical performance degradation with `role="alert"` for monitoring dashboards. Ensure performance overlays do not interfere with screen reader content. Provide text-based alternatives for any performance visualization charts. Keep performance monitoring UI separate from the main content to avoid confusing assistive technology users.

```html
<div class="card" role="region" aria-label="Performance metrics dashboard">
  <div class="card-body" aria-live="polite">
    <div aria-label="Largest Contentful Paint: 1200 milliseconds, rated Good">
      LCP: 1200ms
    </div>
  </div>
</div>
```

## Responsive Behavior

Display performance metric cards in a single column on mobile (`col-12`), two columns on tablet (`col-sm-6`), and three columns on desktop (`col-md-4`). Use Bootstrap's `table-responsive` wrapper for the performance results table on small screens. Collapse detailed resource metrics into expandable accordion sections on mobile. Use `d-none d-md-block` to hide secondary metrics on mobile while keeping primary Web Vitals visible. Ensure the long task alert remains visible across all breakpoints with appropriate padding adjustments.
