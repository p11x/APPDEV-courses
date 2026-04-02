---
title: "Workflow UI"
module: "Enterprise Apps"
difficulty: 3
estimated_time: "35 min"
prerequisites: ["04_01_Card_Component", "04_06_Nav_And_Tabs", "04_05_Forms"]
---

## Overview

Workflow UIs represent business processes visually using step forms, approval flows, status pipelines, and kanban boards. Bootstrap 5 cards, nav pills, badges, and grid components enable building interactive workflow visualizations that track items through multi-stage processes common in enterprise applications.

## Basic Implementation

### Step Form / Wizard

```html
<div class="card">
  <div class="card-header bg-white">
    <ul class="nav nav-pills card-header-pills justify-content-center">
      <li class="nav-item">
        <span class="nav-link active rounded-circle d-inline-flex align-items-center justify-content-center" style="width:36px;height:36px">1</span>
        <small class="d-block text-center mt-1">Draft</small>
      </li>
      <li class="nav-item d-flex align-items-center px-3">
        <div class="border-top" style="width:60px"></div>
      </li>
      <li class="nav-item">
        <span class="nav-link border rounded-circle d-inline-flex align-items-center justify-content-center text-muted" style="width:36px;height:36px">2</span>
        <small class="d-block text-center mt-1 text-muted">Review</small>
      </li>
      <li class="nav-item d-flex align-items-center px-3">
        <div class="border-top" style="width:60px"></div>
      </li>
      <li class="nav-item">
        <span class="nav-link border rounded-circle d-inline-flex align-items-center justify-content-center text-muted" style="width:36px;height:36px">3</span>
        <small class="d-block text-center mt-1 text-muted">Approved</small>
      </li>
    </ul>
  </div>
  <div class="card-body">
    <h5 class="card-title">Current Step: Draft</h5>
    <p class="text-muted">Complete the form below and submit for review.</p>
    <form>
      <div class="mb-3">
        <label class="form-label">Request Title</label>
        <input type="text" class="form-control" placeholder="Enter request title">
      </div>
      <div class="mb-3">
        <label class="form-label">Description</label>
        <textarea class="form-control" rows="4" placeholder="Describe the request..."></textarea>
      </div>
      <div class="mb-3">
        <label class="form-label">Priority</label>
        <select class="form-select">
          <option>Low</option>
          <option selected>Medium</option>
          <option>High</option>
          <option>Critical</option>
        </select>
      </div>
      <div class="d-flex justify-content-between">
        <button class="btn btn-outline-secondary">Save Draft</button>
        <button class="btn btn-primary">Submit for Review</button>
      </div>
    </form>
  </div>
</div>
```

### Approval Flow Timeline

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Approval History</h5></div>
  <div class="card-body p-0">
    <ul class="list-group list-group-flush">
      <li class="list-group-item d-flex">
        <div class="me-3">
          <div class="bg-success bg-opacity-10 rounded-circle p-2"><i class="bi bi-check-circle text-success"></i></div>
        </div>
        <div class="flex-grow-1">
          <div class="d-flex justify-content-between">
            <strong>Submitted for Review</strong>
            <small class="text-muted">Mar 15, 10:30 AM</small>
          </div>
          <p class="mb-0 small text-muted">By John Doe</p>
        </div>
      </li>
      <li class="list-group-item d-flex">
        <div class="me-3">
          <div class="bg-warning bg-opacity-10 rounded-circle p-2"><i class="bi bi-hourglass-split text-warning"></i></div>
        </div>
        <div class="flex-grow-1">
          <div class="d-flex justify-content-between">
            <strong>Pending Manager Approval</strong>
            <span class="badge bg-warning text-dark">Waiting</span>
          </div>
          <p class="mb-0 small text-muted">Assigned to: Sarah Chen (Manager)</p>
        </div>
      </li>
      <li class="list-group-item d-flex">
        <div class="me-3">
          <div class="bg-light rounded-circle p-2"><i class="bi bi-circle text-muted"></i></div>
        </div>
        <div class="flex-grow-1">
          <strong class="text-muted">Final Approval</strong>
          <p class="mb-0 small text-muted">Pending previous step</p>
        </div>
      </li>
    </ul>
  </div>
</div>
```

## Advanced Variations

### Kanban Board with Bootstrap Cards

```html
<div class="d-flex gap-3 overflow-auto pb-3" style="min-height:400px">
  <!-- To Do Column -->
  <div class="flex-shrink-0" style="width:300px">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h6 class="mb-0"><span class="badge bg-secondary me-2">4</span>To Do</h6>
      <button class="btn btn-sm btn-outline-secondary"><i class="bi bi-plus"></i></button>
    </div>
    <div class="d-flex flex-column gap-2">
      <div class="card shadow-sm">
        <div class="card-body p-3">
          <div class="d-flex justify-content-between mb-2">
            <span class="badge bg-danger">High</span>
            <small class="text-muted">#WF-101</small>
          </div>
          <h6 class="card-title small">Update customer database</h6>
          <div class="d-flex justify-content-between align-items-center mt-3">
            <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center" style="width:28px;height:28px;font-size:0.7em">JD</div>
            <small class="text-muted"><i class="bi bi-chat me-1"></i>3</small>
          </div>
        </div>
      </div>
      <div class="card shadow-sm">
        <div class="card-body p-3">
          <div class="d-flex justify-content-between mb-2">
            <span class="badge bg-warning text-dark">Medium</span>
            <small class="text-muted">#WF-102</small>
          </div>
          <h6 class="card-title small">Review Q1 budget proposal</h6>
          <div class="d-flex justify-content-between align-items-center mt-3">
            <div class="bg-success text-white rounded-circle d-flex align-items-center justify-content-center" style="width:28px;height:28px;font-size:0.7em">AS</div>
            <small class="text-muted"><i class="bi bi-paperclip me-1"></i>2</small>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- In Progress Column -->
  <div class="flex-shrink-0" style="width:300px">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h6 class="mb-0"><span class="badge bg-primary me-2">2</span>In Progress</h6>
      <button class="btn btn-sm btn-outline-secondary"><i class="bi bi-plus"></i></button>
    </div>
    <div class="d-flex flex-column gap-2">
      <div class="card shadow-sm border-start border-primary border-4">
        <div class="card-body p-3">
          <div class="d-flex justify-content-between mb-2">
            <span class="badge bg-danger">High</span>
            <small class="text-muted">#WF-099</small>
          </div>
          <h6 class="card-title small">API integration testing</h6>
          <div class="progress mt-2" style="height:4px">
            <div class="progress-bar bg-primary" style="width:65%"></div>
          </div>
          <div class="d-flex justify-content-between align-items-center mt-3">
            <div class="bg-warning text-white rounded-circle d-flex align-items-center justify-content-center" style="width:28px;height:28px;font-size:0.7em">BJ</div>
            <small class="text-muted">65%</small>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Done Column -->
  <div class="flex-shrink-0" style="width:300px">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h6 class="mb-0"><span class="badge bg-success me-2">3</span>Done</h6>
    </div>
    <div class="d-flex flex-column gap-2">
      <div class="card shadow-sm opacity-75">
        <div class="card-body p-3">
          <div class="d-flex justify-content-between mb-2">
            <span class="badge bg-success">Low</span>
            <small class="text-muted">#WF-095</small>
          </div>
          <h6 class="card-title small text-decoration-line-through">Set up CI/CD pipeline</h6>
          <div class="d-flex justify-content-between align-items-center mt-3">
            <div class="bg-primary text-white rounded-circle d-flex align-items-center justify-content-center" style="width:28px;height:28px;font-size:0.7em">JD</div>
            <small class="text-success"><i class="bi bi-check-circle"></i></small>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use visual step indicators for multi-step workflows
2. Color-code priority badges: red (high), yellow (medium), blue (low)
3. Show assignee avatars on kanban cards for accountability
4. Use border accents on in-progress cards for visual emphasis
5. Display progress bars on partially completed items
6. Include item counts in kanban column headers
7. Provide action buttons at each workflow stage (approve, reject, submit)
8. Use timestamps on approval flow history
9. Make kanban columns horizontally scrollable on mobile
10. Use `opacity-75` on completed items to de-emphasize them

## Common Pitfalls

1. **No visual progression** - Users can't tell where an item is in the workflow. Use step indicators.
2. **Kanban not scrollable** - Columns overflow on small screens. Use `overflow-auto`.
3. **Missing priority indicators** - All cards look the same. Use color-coded priority badges.
4. **No assignee visibility** - Cards need avatars to show who's responsible.
5. **Step form loses data** - Save draft functionality prevents data loss between steps.
6. **Approval flow unclear** - Show who approved, who's pending, and timestamps.

## Accessibility Considerations

- Use `role="list"` and `role="listitem"` on kanban columns if list styling is removed
- Provide `aria-label="Priority: High"` on priority badges
- Mark step indicators with `aria-current="step"` on the active step
- Use `aria-label` on kanban cards describing the task and status
- Announce workflow state changes with `aria-live="polite"`

## Responsive Behavior

On **mobile**, kanban columns stack vertically or use horizontal scrolling. Step indicators become compact progress bars. Approval timelines stack items vertically. On **tablet**, kanban can display 2 columns side by side. On **desktop**, all kanban columns display horizontally with scroll support. Step forms center in an 8-column container.
