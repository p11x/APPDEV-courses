---
title: "Data Visualization Tables"
module: "Dashboards & Analytics"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_04_Table", "04_09_Badges", "02_02_Utilities"]
---

## Overview

Data visualization tables combine tabular data with visual indicators like progress bars, color-coded cells, and inline charts. Bootstrap 5 tables, progress bars, badges, and background utilities create rich data displays that communicate insights without leaving the table format.

## Basic Implementation

### Table with Progress Columns

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Team Performance</h5></div>
  <div class="table-responsive">
    <table class="table align-middle mb-0">
      <thead class="table-light">
        <tr>
          <th>Member</th>
          <th>Tasks Completed</th>
          <th>Progress</th>
          <th>Efficiency</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:32px;height:32px;font-size:0.7em">JD</div>
              <span>John Doe</span>
            </div>
          </td>
          <td>42/50</td>
          <td style="width:200px">
            <div class="progress" style="height:8px">
              <div class="progress-bar bg-success" style="width:84%">84%</div>
            </div>
          </td>
          <td>
            <div class="progress" style="height:8px">
              <div class="progress-bar bg-primary" style="width:92%">92%</div>
            </div>
          </td>
          <td><span class="badge bg-success">On Track</span></td>
        </tr>
        <tr>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-warning text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:32px;height:32px;font-size:0.7em">AS</div>
              <span>Alice Smith</span>
            </div>
          </td>
          <td>31/50</td>
          <td>
            <div class="progress" style="height:8px">
              <div class="progress-bar bg-warning" style="width:62%">62%</div>
            </div>
          </td>
          <td>
            <div class="progress" style="height:8px">
              <div class="progress-bar bg-warning" style="width:78%">78%</div>
            </div>
          </td>
          <td><span class="badge bg-warning text-dark">Needs Attention</span></td>
        </tr>
        <tr>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-danger text-white rounded-circle d-flex align-items-center justify-content-center me-2" style="width:32px;height:32px;font-size:0.7em">BJ</div>
              <span>Bob Johnson</span>
            </div>
          </td>
          <td>18/50</td>
          <td>
            <div class="progress" style="height:8px">
              <div class="progress-bar bg-danger" style="width:36%">36%</div>
            </div>
          </td>
          <td>
            <div class="progress" style="height:8px">
              <div class="progress-bar bg-danger" style="width:54%">54%</div>
            </div>
          </td>
          <td><span class="badge bg-danger">Behind</span></td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

## Advanced Variations

### Heatmap Table

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Weekly Activity Heatmap</h5></div>
  <div class="table-responsive">
    <table class="table table-sm text-center mb-0">
      <thead class="table-light">
        <tr>
          <th class="text-start">User</th>
          <th>Mon</th>
          <th>Tue</th>
          <th>Wed</th>
          <th>Thu</th>
          <th>Fri</th>
          <th>Sat</th>
          <th>Sun</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td class="text-start">John</td>
          <td class="bg-success bg-opacity-25">12</td>
          <td class="bg-success bg-opacity-50">18</td>
          <td class="bg-success bg-opacity-75 text-white">24</td>
          <td class="bg-success bg-opacity-50">16</td>
          <td class="bg-success bg-opacity-25">10</td>
          <td class="bg-light">2</td>
          <td class="bg-light">0</td>
        </tr>
        <tr>
          <td class="text-start">Alice</td>
          <td class="bg-success bg-opacity-50">15</td>
          <td class="bg-success bg-opacity-75 text-white">22</td>
          <td class="bg-success bg-opacity-50">19</td>
          <td class="bg-success bg-opacity-25">8</td>
          <td class="bg-success bg-opacity-50">14</td>
          <td class="bg-success bg-opacity-25">5</td>
          <td class="bg-light">1</td>
        </tr>
        <tr>
          <td class="text-start">Bob</td>
          <td class="bg-warning bg-opacity-25">6</td>
          <td class="bg-light">3</td>
          <td class="bg-warning bg-opacity-50">9</td>
          <td class="bg-warning bg-opacity-25">4</td>
          <td class="bg-warning bg-opacity-50">7</td>
          <td class="bg-light">0</td>
          <td class="bg-light">0</td>
        </tr>
      </tbody>
    </table>
  </div>
  <div class="card-footer bg-white d-flex align-items-center gap-3 small">
    <span>Intensity:</span>
    <span class="d-flex align-items-center gap-1">
      <span class="bg-light border d-inline-block" style="width:16px;height:16px"></span> None
    </span>
    <span class="d-flex align-items-center gap-1">
      <span class="bg-success bg-opacity-25 d-inline-block" style="width:16px;height:16px"></span> Low
    </span>
    <span class="d-flex align-items-center gap-1">
      <span class="bg-success bg-opacity-50 d-inline-block" style="width:16px;height:16px"></span> Medium
    </span>
    <span class="d-flex align-items-center gap-1">
      <span class="bg-success bg-opacity-75 d-inline-block" style="width:16px;height:16px"></span> High
    </span>
  </div>
</div>
```

### Conditional Formatting Table

```html
<table class="table align-middle mb-0">
  <thead class="table-light">
    <tr>
      <th>Server</th>
      <th>CPU Usage</th>
      <th>Memory</th>
      <th>Disk</th>
      <th>Uptime</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><code>prod-web-01</code></td>
      <td>
        <span class="text-success fw-semibold">23%</span>
        <div class="progress mt-1" style="height:4px">
          <div class="progress-bar bg-success" style="width:23%"></div>
        </div>
      </td>
      <td>
        <span class="text-warning fw-semibold">68%</span>
        <div class="progress mt-1" style="height:4px">
          <div class="progress-bar bg-warning" style="width:68%"></div>
        </div>
      </td>
      <td>
        <span class="text-success fw-semibold">45%</span>
        <div class="progress mt-1" style="height:4px">
          <div class="progress-bar bg-success" style="width:45%"></div>
        </div>
      </td>
      <td><span class="badge bg-success">99.98%</span></td>
    </tr>
    <tr>
      <td><code>prod-db-01</code></td>
      <td>
        <span class="text-danger fw-semibold">89%</span>
        <div class="progress mt-1" style="height:4px">
          <div class="progress-bar bg-danger" style="width:89%"></div>
        </div>
      </td>
      <td>
        <span class="text-danger fw-semibold">92%</span>
        <div class="progress mt-1" style="height:4px">
          <div class="progress-bar bg-danger" style="width:92%"></div>
        </div>
      </td>
      <td>
        <span class="text-warning fw-semibold">71%</span>
        <div class="progress mt-1" style="height:4px">
          <div class="progress-bar bg-warning" style="width:71%"></div>
        </div>
      </td>
      <td><span class="badge bg-success">99.99%</span></td>
    </tr>
  </tbody>
</table>
```

## Best Practices

1. Use progress bars for percentage-based data within table cells
2. Color-code progress bars: green (<60%), yellow (60-80%), red (>80%)
3. Use `bg-opacity-*` classes for heatmap intensity levels
4. Include a legend for heatmap color meanings
5. Show minified progress bars (4px) for compact conditional formatting
6. Use badges for categorical status values
7. Apply `fw-semibold` on critical metric values
8. Use monospace font for technical values (server names, codes)
9. Provide both the number and the visual indicator
10. Keep progress bar text concise (percentage only)

## Common Pitfalls

1. **No color legend** - Users don't understand heatmap colors without a legend.
2. **Progress bars without context** - "68%" alone is unclear. Show what it represents.
3. **Too many visual indicators** - Mixing progress bars, badges, and heatmaps in one table is overwhelming.
4. **Inconsistent thresholds** - Different tables use different color thresholds. Standardize.
5. **No responsive wrapper** - Tables with many columns overflow on mobile.
6. **Missing alternative text** - Screen readers can't parse visual progress bars. Include text labels.

## Accessibility Considerations

- Use `aria-label="CPU usage: 89%, critical"` on colored metric cells
- Provide `role="progressbar"` with `aria-valuenow` on inline progress bars
- Include a text-based summary for heatmap tables
- Mark status badges with `aria-label="Status: On Track"`

## Responsive Behavior

On **mobile**, tables use `table-responsive` for horizontal scrolling. Progress columns can be hidden or stacked. On **tablet**, all columns are visible with condensed padding. On **desktop**, the full table with progress bars and heatmap cells displays comfortably.
