---
title: "Heatmap Display"
description: "Build CSS grid heatmaps with color intensity visualization and day/hour activity patterns using Bootstrap 5 utilities."
difficulty: 3
estimated_time: "40 minutes"
prerequisites:
  - "Bootstrap 5 Grid"
  - "Bootstrap 5 Utilities"
  - "CSS Custom Properties"
  - "Bootstrap 5 Tooltips"
---

## Overview

Heatmaps visualize data density or intensity across a two-dimensional grid using color gradients. Bootstrap 5's grid utilities, background color classes, and tooltip components create interactive heatmap displays for activity patterns, performance metrics, and data density visualization.

The most common heatmap pattern shows activity by day and hour (like GitHub's contribution graph). Bootstrap's `bg-*` utilities with opacity modifiers, CSS Grid, and tooltips combine to render cells with varying intensity levels and interactive information.

## Basic Implementation

### Simple CSS Grid Heatmap

```html
<style>
  .heatmap-grid {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    gap: 3px;
  }
  .heatmap-cell {
    aspect-ratio: 1;
    border-radius: 3px;
  }
  .intensity-0 { background-color: #ebedf0; }
  .intensity-1 { background-color: #9be9a8; }
  .intensity-2 { background-color: #40c463; }
  .intensity-3 { background-color: #30a14e; }
  .intensity-4 { background-color: #216e39; }
</style>

<div class="heatmap-grid" style="max-width: 300px;">
  <div class="heatmap-cell intensity-2" data-bs-toggle="tooltip" title="3 contributions"></div>
  <div class="heatmap-cell intensity-0" data-bs-toggle="tooltip" title="No contributions"></div>
  <div class="heatmap-cell intensity-4" data-bs-toggle="tooltip" title="12 contributions"></div>
  <div class="heatmap-cell intensity-1" data-bs-toggle="tooltip" title="1 contribution"></div>
  <div class="heatmap-cell intensity-3" data-bs-toggle="tooltip" title="7 contributions"></div>
  <div class="heatmap-cell intensity-0" data-bs-toggle="tooltip" title="No contributions"></div>
  <div class="heatmap-cell intensity-2" data-bs-toggle="tooltip" title="4 contributions"></div>
  <div class="heatmap-cell intensity-1" data-bs-toggle="tooltip" title="2 contributions"></div>
  <div class="heatmap-cell intensity-3" data-bs-toggle="tooltip" title="8 contributions"></div>
  <div class="heatmap-cell intensity-0" data-bs-toggle="tooltip" title="No contributions"></div>
  <div class="heatmap-cell intensity-4" data-bs-toggle="tooltip" title="15 contributions"></div>
  <div class="heatmap-cell intensity-2" data-bs-toggle="tooltip" title="5 contributions"></div>
  <div class="heatmap-cell intensity-0" data-bs-toggle="tooltip" title="No contributions"></div>
  <div class="heatmap-cell intensity-1" data-bs-toggle="tooltip" title="1 contribution"></div>
</div>
```

### Day/Hour Activity Heatmap

```html
<div class="table-responsive">
  <table class="table table-sm text-center" style="--bs-table-bg: transparent;">
    <thead>
      <tr>
        <th class="small text-muted border-0"></th>
        <th class="small text-muted border-0">12a</th>
        <th class="small text-muted border-0">4a</th>
        <th class="small text-muted border-0">8a</th>
        <th class="small text-muted border-0">12p</th>
        <th class="small text-muted border-0">4p</th>
        <th class="small text-muted border-0">8p</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td class="small text-muted text-start border-0">Mon</td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #ebedf0;" data-bs-toggle="tooltip" title="0 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #9be9a8;" data-bs-toggle="tooltip" title="3 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #40c463;" data-bs-toggle="tooltip" title="18 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #30a14e;" data-bs-toggle="tooltip" title="42 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #216e39;" data-bs-toggle="tooltip" title="67 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #40c463;" data-bs-toggle="tooltip" title="25 events"></div></td>
      </tr>
      <tr>
        <td class="small text-muted text-start border-0">Tue</td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #ebedf0;" data-bs-toggle="tooltip" title="0 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #ebedf0;" data-bs-toggle="tooltip" title="0 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #9be9a8;" data-bs-toggle="tooltip" title="5 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #30a14e;" data-bs-toggle="tooltip" title="38 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #216e39;" data-bs-toggle="tooltip" title="72 events"></div></td>
        <td class="p-1 border-0"><div class="rounded" style="width: 32px; height: 32px; background-color: #9be9a8;" data-bs-toggle="tooltip" title="8 events"></div></td>
      </tr>
    </tbody>
  </table>
</div>
```

### Intensity Legend

```html
<div class="d-flex align-items-center gap-2 justify-content-end">
  <span class="small text-muted">Less</span>
  <div class="rounded" style="width: 16px; height: 16px; background-color: #ebedf0;"></div>
  <div class="rounded" style="width: 16px; height: 16px; background-color: #9be9a8;"></div>
  <div class="rounded" style="width: 16px; height: 16px; background-color: #40c463;"></div>
  <div class="rounded" style="width: 16px; height: 16px; background-color: #30a14e;"></div>
  <div class="rounded" style="width: 16px; height: 16px; background-color: #216e39;"></div>
  <span class="small text-muted">More</span>
</div>
```

## Advanced Variations

### Bootstrap Background Opacity Heatmap

```html
<div class="d-flex gap-1 flex-wrap" style="max-width: 400px;">
  <div class="bg-success bg-opacity-10 rounded" style="width: 30px; height: 30px;" data-bs-toggle="tooltip" title="Low"></div>
  <div class="bg-success bg-opacity-25 rounded" style="width: 30px; height: 30px;" data-bs-toggle="tooltip" title="Moderate"></div>
  <div class="bg-success bg-opacity-50 rounded" style="width: 30px; height: 30px;" data-bs-toggle="tooltip" title="High"></div>
  <div class="bg-success bg-opacity-75 rounded" style="width: 30px; height: 30px;" data-bs-toggle="tooltip" title="Very High"></div>
  <div class="bg-success rounded" style="width: 30px; height: 30px;" data-bs-toggle="tooltip" title="Maximum"></div>
</div>
```

### Calendar Heatmap (GitHub-Style)

```html
<div class="d-flex gap-1 overflow-auto pb-2">
  <div class="d-flex flex-column gap-1" style="min-width: fit-content;">
    <div class="small text-muted" style="height: 16px;"></div>
    <div class="small text-muted" style="height: 14px; line-height: 14px;">Mon</div>
    <div class="small text-muted" style="height: 14px; line-height: 14px;">Wed</div>
    <div class="small text-muted" style="height: 14px; line-height: 14px;">Fri</div>
  </div>
  <div>
    <div class="small text-muted text-center mb-1">Jan</div>
    <div class="d-flex gap-1">
      <div class="d-flex flex-column gap-1">
        <div class="rounded-sm" style="width: 14px; height: 14px; background-color: #ebedf0;"></div>
        <div class="rounded-sm" style="width: 14px; height: 14px; background-color: #9be9a8;"></div>
        <div class="rounded-sm" style="width: 14px; height: 14px; background-color: #40c463;"></div>
        <div class="rounded-sm" style="width: 14px; height: 14px; background-color: #ebedf0;"></div>
        <div class="rounded-sm" style="width: 14px; height: 14px; background-color: #30a14e;"></div>
        <div class="rounded-sm" style="width: 14px; height: 14px; background-color: #216e39;"></div>
        <div class="rounded-sm" style="width: 14px; height: 14px; background-color: #ebedf0;"></div>
      </div>
    </div>
  </div>
</div>
```

### Heatmap Summary Card

```html
<div class="card">
  <div class="card-body">
    <h6 class="card-title">Activity Summary</h6>
    <div class="row g-3">
      <div class="col-4 text-center">
        <div class="fs-5 fw-bold">247</div>
        <div class="text-muted small">Total Events</div>
      </div>
      <div class="col-4 text-center">
        <div class="fs-5 fw-bold text-success">4-8 PM</div>
        <div class="text-muted small">Peak Hours</div>
      </div>
      <div class="col-4 text-center">
        <div class="fs-5 fw-bold">Tuesday</div>
        <div class="text-muted small">Most Active</div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use a 4-5 level color gradient from light to dark for intensity
2. Include tooltips on each cell showing exact values
3. Provide an intensity legend to explain the color scale
4. Use CSS Grid for consistent cell sizing and spacing
5. Keep heatmap cells square or maintain consistent aspect ratios
6. Label rows and columns clearly (days, hours, categories)
7. Use `data-bs-toggle="tooltip"` for Bootstrap tooltip integration
8. Include a summary card showing peak times and totals
9. Make the heatmap horizontally scrollable on mobile
10. Use accessible color palettes that work for color-blind users
11. Provide keyboard navigation for interactive cells
12. Cache heatmap data to avoid recalculating on every render
13. Use `bg-opacity` utilities for simpler intensity levels

## Common Pitfalls

1. **Insufficient color contrast**: Light intensity cells on white backgrounds become invisible. Use a minimum visible color.
2. **No tooltips on cells**: Users cannot determine exact values from color alone without hover tooltips.
3. **Missing legend**: Without an intensity legend, users cannot interpret the color scale.
4. **No mobile scrolling**: Fixed-width heatmaps overflow on mobile without horizontal scroll containers.
5. **Too many intensity levels**: More than 5 levels makes the differences imperceptible. Stick to 4-5.
6. **No empty state**: Heatmaps with all empty cells (all lightest color) should show a message instead.
7. **Color-only differentiation**: Using only color to convey data is inaccessible. Include text values in tooltips.

## Accessibility Considerations

- Include text descriptions of intensity levels in tooltips
- Use `aria-label` on the heatmap container describing its purpose
- Provide a text-based data table alternative for screen readers
- Use sufficient color contrast between intensity levels (WCAG AA)
- Support keyboard navigation between heatmap cells
- Use `role="grid"` on the heatmap container
- Announce cell values when focused using `aria-live`

## Responsive Behavior

On mobile, the heatmap should be horizontally scrollable with `overflow-auto`. Cell sizes should reduce to 12-14px on small screens. The legend should remain visible below the heatmap. Summary cards should stack using `col-12`. Column labels (hours) should abbreviate to save space. Consider showing fewer time periods (e.g., every 4 hours instead of every hour) on mobile.
