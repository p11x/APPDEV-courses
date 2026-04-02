---
title: Timeline Component
category: Component System
difficulty: 2
time: 30 min
tags: bootstrap5, timeline, vertical, horizontal, cards, status, content-display
---

## Overview

A timeline component visually represents a sequence of events in chronological order. While Bootstrap does not include a native timeline, you can build one using Bootstrap's list groups, cards, and utility classes. Timelines can be vertical, horizontal, or card-based, and support status indicators, date markers, and responsive layouts.

## Basic Implementation

A vertical timeline uses a list group with custom CSS for a connected line and dot markers.

```html
<style>
  .timeline {
    position: relative;
    padding-left: 2rem;
  }
  .timeline::before {
    content: '';
    position: absolute;
    left: 0.5rem;
    top: 0;
    bottom: 0;
    width: 2px;
    background: #dee2e6;
  }
  .timeline-item {
    position: relative;
    margin-bottom: 1.5rem;
  }
  .timeline-item::before {
    content: '';
    position: absolute;
    left: -1.65rem;
    top: 0.25rem;
    width: 12px;
    height: 12px;
    border-radius: 50%;
    background: #0d6efd;
    border: 2px solid #fff;
  }
  .timeline-item.success::before { background: #198754; }
  .timeline-item.warning::before { background: #ffc107; }
  .timeline-item.danger::before { background: #dc3545; }
</style>

<div class="timeline">
  <div class="timeline-item">
    <small class="text-muted">March 1, 2026</small>
    <h6 class="mb-1">Project Started</h6>
    <p class="text-muted mb-0">Initial planning and kickoff meeting completed.</p>
  </div>
  <div class="timeline-item success">
    <small class="text-muted">March 10, 2026</small>
    <h6 class="mb-1">Design Approved</h6>
    <p class="text-muted mb-0">UI/UX designs reviewed and approved by stakeholders.</p>
  </div>
  <div class="timeline-item warning">
    <small class="text-muted">March 20, 2026</small>
    <h6 class="mb-1">Development In Progress</h6>
    <p class="text-muted mb-0">Frontend and backend development underway.</p>
  </div>
</div>
```

## Advanced Variations

```html
<!-- Timeline with cards -->
<div class="timeline">
  <div class="timeline-item">
    <div class="card">
      <div class="card-body">
        <div class="d-flex justify-content-between">
          <small class="text-muted">Feb 15, 2026</small>
          <span class="badge bg-success">Completed</span>
        </div>
        <h6 class="card-title mt-2">Requirements Gathering</h6>
        <p class="card-text">Collected and documented all functional and non-functional requirements.</p>
      </div>
    </div>
  </div>
  <div class="timeline-item">
    <div class="card">
      <div class="card-body">
        <div class="d-flex justify-content-between">
          <small class="text-muted">Mar 1, 2026</small>
          <span class="badge bg-primary">In Progress</span>
        </div>
        <h6 class="card-title mt-2">Sprint 1 Development</h6>
        <p class="card-text">Working on authentication module and user dashboard.</p>
      </div>
    </div>
  </div>
  <div class="timeline-item">
    <div class="card">
      <div class="card-body">
        <div class="d-flex justify-content-between">
          <small class="text-muted">Apr 15, 2026</small>
          <span class="badge bg-secondary">Planned</span>
        </div>
        <h6 class="card-title mt-2">Beta Release</h6>
        <p class="card-text">Release beta version for internal testing and feedback.</p>
      </div>
    </div>
  </div>
</div>
```

```html
<!-- Horizontal timeline -->
<style>
  .h-timeline {
    display: flex;
    justify-content: space-between;
    position: relative;
    padding: 1rem 0;
  }
  .h-timeline::before {
    content: '';
    position: absolute;
    top: 50%;
    left: 0;
    right: 0;
    height: 2px;
    background: #dee2e6;
  }
  .h-timeline-item {
    position: relative;
    text-align: center;
    flex: 1;
  }
  .h-timeline-item::before {
    content: '';
    position: absolute;
    top: calc(50% - 8px);
    left: 50%;
    transform: translateX(-50%);
    width: 16px;
    height: 16px;
    border-radius: 50%;
    background: #0d6efd;
    border: 3px solid #fff;
    box-shadow: 0 0 0 2px #0d6efd;
  }
</style>

<div class="h-timeline">
  <div class="h-timeline-item">
    <div class="mt-5">
      <small class="text-muted">Phase 1</small>
      <p class="fw-bold mb-0">Planning</p>
    </div>
  </div>
  <div class="h-timeline-item">
    <div class="mt-5">
      <small class="text-muted">Phase 2</small>
      <p class="fw-bold mb-0">Design</p>
    </div>
  </div>
  <div class="h-timeline-item">
    <div class="mt-5">
      <small class="text-muted">Phase 3</small>
      <p class="fw-bold mb-0">Development</p>
    </div>
  </div>
  <div class="h-timeline-item">
    <div class="mt-5">
      <small class="text-muted">Phase 4</small>
      <p class="fw-bold mb-0">Launch</p>
    </div>
  </div>
</div>
```

## Best Practices

1. Use semantic date markers (`<small>`) at each timeline entry.
2. Apply status colors via CSS classes matching Bootstrap theme colors.
3. Use `badge` components for status labels (Completed, In Progress, Planned).
4. Keep timeline entries concise with a title and brief description.
5. Use cards for timeline entries that need richer content or actions.
6. Ensure the connecting line uses absolute positioning for clean alignment.
7. Place status indicator dots on the left for vertical and top for horizontal timelines.
8. Use responsive utilities to switch from horizontal to vertical on mobile.
9. Maintain consistent spacing between timeline entries.
10. Use `text-muted` for secondary information like dates and descriptions.

## Common Pitfalls

1. **No connecting line on mobile.** CSS `::before` pseudo-elements may break on narrow screens without media queries.
2. **Inconsistent status colors.** Using different colors for the same status across entries confuses users.
3. **Overloading timeline entries.** Dense content in small cards makes the timeline hard to scan.
4. **Missing semantic dates.** Without dates, the chronological purpose of the timeline is lost.
5. **Non-responsive horizontal timeline.** Horizontal timelines overflow on mobile without switching to vertical layout.
6. **Accessibility gaps.** Timeline entries without proper heading hierarchy are difficult for screen readers to navigate.

## Accessibility Considerations

Use heading elements (`h6`) for timeline entry titles to maintain document hierarchy. Timeline entries should be wrapped in a semantic container like `<section>` or a `role="list"` with `role="listitem"` children. Dates should use `<time>` elements with `datetime` attributes. Provide `aria-label` on the timeline container describing the sequence. Ensure color is not the sole indicator of status; pair with text badges.

## Responsive Behavior

Vertical timelines adapt naturally to all screen sizes. Horizontal timelines should switch to vertical on mobile using `d-none d-md-flex` to hide the horizontal layout and `d-md-none` to show a vertical fallback. Timeline cards should use `card-body` padding that adjusts with viewport size. Use Bootstrap's grid to control timeline width: `col-lg-8 offset-lg-2` centers the timeline on large screens.
