---
title: "Feature Flag UI"
description: "Build feature flag management interfaces with toggle controls, rollout percentages, user targeting, and dashboards using Bootstrap 5."
difficulty: 3
estimated_time: "45 minutes"
prerequisites:
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Modals"
  - "Bootstrap 5 Progress"
---

## Overview

Feature flags enable gradual rollouts, A/B testing, and kill switches for application features. A feature flag management UI built with Bootstrap 5 provides toggle switches for enabling/disabling flags, percentage-based rollout sliders, user targeting rules, and an overview dashboard showing flag status across environments.

This component is critical for SaaS platforms that ship continuously. The UI must clearly distinguish between environments (development, staging, production) and show the impact of each flag change. Bootstrap's form switches, tables, progress bars, and modals combine to create a comprehensive flag management experience.

## Basic Implementation

### Feature Flag Toggle Row

```html
<div class="card mb-2">
  <div class="card-body py-2 d-flex justify-content-between align-items-center">
    <div>
      <strong>new-checkout-flow</strong>
      <div class="text-muted small">Enables the redesigned checkout experience</div>
    </div>
    <div class="d-flex align-items-center gap-3">
      <span class="badge bg-warning text-dark">50% rollout</span>
      <div class="form-check form-switch mb-0">
        <input class="form-check-input" type="checkbox" role="switch" id="flag1" checked>
        <label class="form-check-label visually-hidden" for="flag1">Enable new checkout flow</label>
      </div>
    </div>
  </div>
</div>
```

### Feature Flag Dashboard Summary

```html
<div class="row g-3 mb-4">
  <div class="col-md-3">
    <div class="card text-center">
      <div class="card-body">
        <div class="display-6 fw-bold text-success">12</div>
        <div class="text-muted small">Active Flags</div>
      </div>
    </div>
  </div>
  <div class="col-md-3">
    <div class="card text-center">
      <div class="card-body">
        <div class="display-6 fw-bold text-secondary">5</div>
        <div class="text-muted small">Disabled Flags</div>
      </div>
    </div>
  </div>
  <div class="col-md-3">
    <div class="card text-center">
      <div class="card-body">
        <div class="display-6 fw-bold text-warning">3</div>
        <div class="text-muted small">Partial Rollout</div>
      </div>
    </div>
  </div>
  <div class="col-md-3">
    <div class="card text-center">
      <div class="card-body">
        <div class="display-6 fw-bold text-info">2</div>
        <div class="text-muted small">A/B Tests</div>
      </div>
    </div>
  </div>
</div>
```

### Rollout Percentage Display

```html
<div class="d-flex align-items-center gap-2">
  <div class="progress flex-grow-1" style="height: 10px;">
    <div class="progress-bar bg-warning" role="progressbar" style="width: 50%;" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
  </div>
  <span class="small fw-bold">50%</span>
</div>
```

## Advanced Variations

### Rollout Percentage Slider

```html
<div class="card mb-3">
  <div class="card-header">
    <strong>Rollout Configuration</strong> - <code>new-checkout-flow</code>
  </div>
  <div class="card-body">
    <div class="mb-3">
      <label class="form-label">Rollout Percentage: <strong id="rolloutValue">25%</strong></label>
      <input type="range" class="form-range" min="0" max="100" step="5" value="25" id="rolloutSlider">
      <div class="d-flex justify-content-between text-muted small">
        <span>0% (Off)</span>
        <span>50%</span>
        <span>100% (Full)</span>
      </div>
    </div>
    <div class="mb-3">
      <label class="form-label">Target Environments</label>
      <div class="d-flex gap-3">
        <div class="form-check">
          <input class="form-check-input" type="checkbox" id="envDev" checked>
          <label class="form-check-label" for="envDev">Development</label>
        </div>
        <div class="form-check">
          <input class="form-check-input" type="checkbox" id="envStaging" checked>
          <label class="form-check-label" for="envStaging">Staging</label>
        </div>
        <div class="form-check">
          <input class="form-check-input" type="checkbox" id="envProd">
          <label class="form-check-label" for="envProd">Production</label>
        </div>
      </div>
    </div>
    <button class="btn btn-primary btn-sm">Save Configuration</button>
  </div>
</div>
```

### User Targeting Rules

```html
<div class="card mb-3">
  <div class="card-header d-flex justify-content-between align-items-center">
    <strong>User Targeting</strong>
    <button class="btn btn-sm btn-outline-primary" data-bs-toggle="modal" data-bs-target="#addRuleModal">
      <i class="bi bi-plus"></i> Add Rule
    </button>
  </div>
  <div class="list-group list-group-flush">
    <div class="list-group-item d-flex justify-content-between align-items-center">
      <div>
        <span class="badge bg-primary me-1">Email</span>
        <code>user matches "@acme.com"</code>
      </div>
      <div class="form-check form-switch mb-0">
        <input class="form-check-input" type="checkbox" role="switch" checked>
      </div>
    </div>
    <div class="list-group-item d-flex justify-content-between align-items-center">
      <div>
        <span class="badge bg-secondary me-1">Plan</span>
        <code>user.plan equals "enterprise"</code>
      </div>
      <div class="form-check form-switch mb-0">
        <input class="form-check-input" type="checkbox" role="switch" checked>
      </div>
    </div>
    <div class="list-group-item d-flex justify-content-between align-items-center">
      <div>
        <span class="badge bg-info me-1">User ID</span>
        <code>user.id in [1001, 1002, 1003]</code>
      </div>
      <div class="form-check form-switch mb-0">
        <input class="form-check-input" type="checkbox" role="switch">
      </div>
    </div>
  </div>
</div>
```

### Environment Comparison Table

```html
<div class="table-responsive">
  <table class="table table-sm table-hover">
    <thead class="table-light">
      <tr>
        <th>Flag</th>
        <th class="text-center">Dev</th>
        <th class="text-center">Staging</th>
        <th class="text-center">Production</th>
        <th>Rollout</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><code>new-checkout-flow</code></td>
        <td class="text-center"><i class="bi bi-toggle-on text-success fs-5"></i></td>
        <td class="text-center"><i class="bi bi-toggle-on text-success fs-5"></i></td>
        <td class="text-center"><i class="bi bi-toggle-off text-secondary fs-5"></i></td>
        <td>
          <div class="progress" style="height: 6px; width: 80px;">
            <div class="progress-bar bg-warning" style="width: 50%;"></div>
          </div>
        </td>
      </tr>
      <tr>
        <td><code>dark-mode</code></td>
        <td class="text-center"><i class="bi bi-toggle-on text-success fs-5"></i></td>
        <td class="text-center"><i class="bi bi-toggle-on text-success fs-5"></i></td>
        <td class="text-center"><i class="bi bi-toggle-on text-success fs-5"></i></td>
        <td>
          <div class="progress" style="height: 6px; width: 80px;">
            <div class="progress-bar bg-success" style="width: 100%;"></div>
          </div>
        </td>
      </tr>
      <tr>
        <td><code>v2-dashboard</code></td>
        <td class="text-center"><i class="bi bi-toggle-on text-success fs-5"></i></td>
        <td class="text-center"><i class="bi bi-toggle-off text-secondary fs-5"></i></td>
        <td class="text-center"><i class="bi bi-toggle-off text-secondary fs-5"></i></td>
        <td>
          <div class="progress" style="height: 6px; width: 80px;">
            <div class="progress-bar bg-danger" style="width: 0%;"></div>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
</div>
```

## Best Practices

1. Always show the environment context (dev/staging/prod) prominently
2. Use code-formatted flag names for easy copy-paste by developers
3. Require confirmation before toggling production flags with `data-bs-toggle="modal"`
4. Use color-coded badges to distinguish flag types (boolean, percentage, user targeting)
5. Log all flag changes with timestamps and the user who made the change
6. Provide a kill switch for instantly disabling a flag in emergencies
7. Use `form-check-switch` with `role="switch"` for proper accessibility
8. Display rollout percentages with visual progress bars alongside the numeric value
9. Include a search/filter bar when managing 20+ flags
10. Group flags by feature area or team using collapsible sections
11. Show the last modified date and author for each flag
12. Implement permission levels so only authorized users can toggle production flags
13. Use consistent toggle colors: green for enabled, gray for disabled

## Common Pitfalls

1. **No production confirmation**: Toggling a flag in production without a confirmation dialog can cause outages. Always require explicit confirmation.
2. **Missing environment indicator**: Showing flags without clear environment labels leads to accidental production changes.
3. **No change audit trail**: Without logging who changed what and when, debugging flag-related incidents is impossible.
4. **Stale flag accumulation**: Old flags that are fully rolled out or abandoned clutter the dashboard. Implement a cleanup process.
5. **Slider without keyboard support**: Range inputs need arrow key support and proper `aria-valuenow` attributes.
6. **No loading state on toggle**: Toggling a flag that triggers an API call without a loading state causes duplicate requests.
7. **Inconsistent icon semantics**: Using `bi-toggle-on` for both enabled and disabled states without color differentiation is confusing.

## Accessibility Considerations

- Use `role="switch"` on toggle checkboxes for proper screen reader announcement
- Apply `aria-label` or visible `label` elements to all toggle switches
- Use `aria-describedby` to connect rollout percentages to their controls
- Ensure the range slider has `aria-valuemin`, `aria-valuemax`, and `aria-valuenow`
- Announce flag status changes using `aria-live="polite"` regions
- Make the environment comparison table accessible with proper `th` scope attributes
- Provide keyboard navigation for all interactive elements in the flag list

## Responsive Behavior

On mobile, the dashboard summary cards should use `col-6` for a 2x2 grid. The environment comparison table should wrap in `table-responsive`. Flag toggle cards should stack the flag info and controls vertically. The rollout slider should remain full-width. The targeting rules list should use full-width list items. Consider using `d-none d-md-block` for less critical columns in the comparison table on small screens.
