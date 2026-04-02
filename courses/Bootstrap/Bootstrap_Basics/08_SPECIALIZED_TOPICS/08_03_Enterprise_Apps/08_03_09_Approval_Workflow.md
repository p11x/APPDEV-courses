---
title: "Approval Workflow"
description: "Build approval chain UIs with approve/reject actions, comments, status badges, and multi-step workflow visualization using Bootstrap 5."
difficulty: 3
estimated_time: "45 minutes"
prerequisites:
  - "Bootstrap 5 Cards"
  - "Bootstrap 5 Badges"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Modals"
---

## Overview

Approval workflow components are essential for enterprise applications requiring multi-level authorization for purchases, content publishing, expense reports, or configuration changes. Bootstrap 5 provides the building blocks to create approval chain visualizations, action buttons, comment threads, and status indicators.

The workflow displays a sequence of approval steps, each with an assignee, status, and optional comments. Users see the full chain at a glance, understand the current step, and can take action when it is their turn. Status badges, progress indicators, and timeline layouts make the workflow state clear.

## Basic Implementation

### Approval Chain Progress

```html
<div class="d-flex justify-content-between align-items-center mb-4">
  <div class="text-center">
    <div class="bg-success rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 40px; height: 40px;">
      <i class="bi bi-check-lg text-white"></i>
    </div>
    <div class="small fw-bold">Submitted</div>
    <div class="text-muted" style="font-size: 0.75rem;">Mar 28</div>
  </div>
  <div class="flex-grow-1 bg-success mx-2" style="height: 3px;"></div>
  <div class="text-center">
    <div class="bg-success rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 40px; height: 40px;">
      <i class="bi bi-check-lg text-white"></i>
    </div>
    <div class="small fw-bold">Manager</div>
    <div class="text-muted" style="font-size: 0.75rem;">Approved Mar 29</div>
  </div>
  <div class="flex-grow-1 bg-warning mx-2" style="height: 3px;"></div>
  <div class="text-center">
    <div class="bg-warning rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 40px; height: 40px;">
      <i class="bi bi-hourglass-split text-dark"></i>
    </div>
    <div class="small fw-bold">Director</div>
    <div class="text-warning" style="font-size: 0.75rem;">Pending</div>
  </div>
  <div class="flex-grow-1 bg-secondary mx-2" style="height: 3px;"></div>
  <div class="text-center">
    <div class="bg-light border rounded-circle d-flex align-items-center justify-content-center mx-auto mb-1" style="width: 40px; height: 40px;">
      <i class="bi bi-lock text-muted"></i>
    </div>
    <div class="small text-muted">Finance</div>
    <div class="text-muted" style="font-size: 0.75rem;">Waiting</div>
  </div>
</div>
```

### Approval Request Card

```html
<div class="card border-warning mb-3">
  <div class="card-header d-flex justify-content-between align-items-center">
    <span class="badge bg-warning text-dark"><i class="bi bi-hourglass-split me-1"></i>Awaiting Your Approval</span>
    <small class="text-muted">Expense Report #ER-2024-042</small>
  </div>
  <div class="card-body">
    <div class="row mb-3">
      <div class="col-md-6">
        <div class="text-muted small">Requester</div>
        <strong>Sarah Johnson</strong>
      </div>
      <div class="col-md-3">
        <div class="text-muted small">Amount</div>
        <strong>$2,450.00</strong>
      </div>
      <div class="col-md-3">
        <div class="text-muted small">Submitted</div>
        <strong>Mar 28, 2026</strong>
      </div>
    </div>
    <div class="d-flex gap-2">
      <button class="btn btn-success" data-bs-toggle="modal" data-bs-target="#approveModal">
        <i class="bi bi-check-lg me-1"></i> Approve
      </button>
      <button class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#rejectModal">
        <i class="bi bi-x-lg me-1"></i> Reject
      </button>
      <button class="btn btn-outline-secondary">
        <i class="bi bi-chat-left-text me-1"></i> Comment
      </button>
    </div>
  </div>
</div>
```

### Approve Confirmation Modal

```html
<div class="modal fade" id="approveModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title text-success"><i class="bi bi-check-circle me-1"></i> Approve Request</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <p>You are approving <strong>Expense Report #ER-2024-042</strong> for <strong>$2,450.00</strong>.</p>
        <div class="mb-3">
          <label for="approveComment" class="form-label">Comment (optional)</label>
          <textarea class="form-control" id="approveComment" rows="3" placeholder="Add any notes for the next approver..."></textarea>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-success">Confirm Approval</button>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Approval History Timeline

```html
<h6 class="mb-3">Approval History</h6>
<div class="border-start border-3 border-success ps-3 mb-4">
  <div class="d-flex justify-content-between mb-1">
    <strong>Sarah Johnson</strong>
    <span class="badge bg-secondary">Submitted</span>
  </div>
  <p class="text-muted small mb-1">Submitted for approval on Mar 28, 2026</p>
</div>
<div class="border-start border-3 border-success ps-3 mb-4">
  <div class="d-flex justify-content-between mb-1">
    <strong>Mike Chen (Manager)</strong>
    <span class="badge bg-success">Approved</span>
  </div>
  <p class="text-muted small mb-1">Approved on Mar 29, 2026</p>
  <div class="bg-light rounded p-2 small">
    <i class="bi bi-chat-quote me-1"></i> "Looks good. Quarterly expenses are within budget."
  </div>
</div>
<div class="border-start border-3 border-warning ps-3 mb-4">
  <div class="d-flex justify-content-between mb-1">
    <strong>Lisa Wong (Director)</strong>
    <span class="badge bg-warning text-dark">Pending</span>
  </div>
  <p class="text-muted small mb-1">Waiting for approval since Mar 29, 2026</p>
  <button class="btn btn-sm btn-outline-warning">
    <i class="bi bi-bell me-1"></i> Send Reminder
  </button>
</div>
<div class="border-start border-3 border-secondary ps-3">
  <div class="d-flex justify-content-between mb-1">
    <strong>Finance Team</strong>
    <span class="badge bg-light text-muted">Waiting</span>
  </div>
  <p class="text-muted small mb-0">Will review after director approval</p>
</div>
```

### Reject with Reason Modal

```html
<div class="modal fade" id="rejectModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title text-danger"><i class="bi bi-x-circle me-1"></i> Reject Request</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="mb-3">
          <label class="form-label">Reason for Rejection <span class="text-danger">*</span></label>
          <div class="form-check mb-2">
            <input class="form-check-input" type="radio" name="rejectReason" id="reasonBudget">
            <label class="form-check-label" for="reasonBudget">Exceeds budget limits</label>
          </div>
          <div class="form-check mb-2">
            <input class="form-check-input" type="radio" name="rejectReason" id="reasonDocs">
            <label class="form-check-label" for="reasonDocs">Missing documentation</label>
          </div>
          <div class="form-check mb-2">
            <input class="form-check-input" type="radio" name="rejectReason" id="reasonOther">
            <label class="form-check-label" for="reasonOther">Other (specify below)</label>
          </div>
        </div>
        <div class="mb-3">
          <label for="rejectDetails" class="form-label">Details</label>
          <textarea class="form-control" id="rejectDetails" rows="3" required></textarea>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-danger">Submit Rejection</button>
      </div>
    </div>
  </div>
</div>
```

### Multi-Level Approval Summary

```html
<div class="card">
  <div class="card-header d-flex justify-content-between">
    <strong>Approval Chain</strong>
    <span class="badge bg-warning text-dark">Step 3 of 4</span>
  </div>
  <ul class="list-group list-group-flush">
    <li class="list-group-item d-flex justify-content-between align-items-center">
      <div class="d-flex align-items-center">
        <i class="bi bi-check-circle-fill text-success me-2"></i>
        <div>
          <strong>Level 1:</strong> Direct Manager
          <div class="text-muted small">Mike Chen</div>
        </div>
      </div>
      <span class="badge bg-success">Approved</span>
    </li>
    <li class="list-group-item d-flex justify-content-between align-items-center">
      <div class="d-flex align-items-center">
        <i class="bi bi-check-circle-fill text-success me-2"></i>
        <div>
          <strong>Level 2:</strong> Department Head
          <div class="text-muted small">Sarah Park</div>
        </div>
      </div>
      <span class="badge bg-success">Approved</span>
    </li>
    <li class="list-group-item d-flex justify-content-between align-items-center list-group-item-warning">
      <div class="d-flex align-items-center">
        <i class="bi bi-hourglass-split text-warning me-2"></i>
        <div>
          <strong>Level 3:</strong> Director
          <div class="text-muted small">Lisa Wong</div>
        </div>
      </div>
      <span class="badge bg-warning text-dark">Pending</span>
    </li>
    <li class="list-group-item d-flex justify-content-between align-items-center">
      <div class="d-flex align-items-center">
        <i class="bi bi-circle text-secondary me-2"></i>
        <div>
          <strong>Level 4:</strong> Finance
          <div class="text-muted small">CFO Office</div>
        </div>
      </div>
      <span class="badge bg-light text-muted">Waiting</span>
    </li>
  </ul>
</div>
```

## Best Practices

1. Use color-coded status badges: green for approved, red for rejected, yellow for pending
2. Always require a rejection reason for audit trail purposes
3. Display the full approval chain so users understand the remaining steps
4. Use step indicators with check icons for completed steps
5. Allow comment threads at each approval step for communication
6. Send reminder notifications for pending approvals with escalation options
7. Show the approver name, role, and timestamp for each step
8. Implement delegation so approvals can be reassigned when someone is unavailable
9. Use `list-group-item-warning` to visually highlight the current step
10. Provide a summary card showing key request details alongside the approval chain
11. Lock the request after final approval or rejection to prevent further changes
12. Support inline approval from notification emails for quick action
13. Show time elapsed since submission to identify bottlenecks

## Common Pitfalls

1. **No rejection reason required**: Rejecting without a reason leaves the requester confused about what to fix.
2. **Missing escalation path**: If the approver is unavailable, requests get stuck without a delegation or escalation option.
3. **No notification on status change**: Approvers and requesters must be notified at every step transition.
4. **Unclear current step**: Without highlighting the active step, users cannot tell who needs to act next.
5. **No comment thread**: Approval decisions without context create friction. Always support comments.
6. **Hardcoded approval chains**: Fixed chains do not support dynamic routing based on request amount or type.
7. **No audit trail**: Missing timestamps and user information on each step violates compliance requirements.

## Accessibility Considerations

- Use `aria-current="step"` on the active approval step
- Provide `aria-label` on status icon indicators describing the state
- Use semantic `ol` for the approval chain sequence
- Ensure approve/reject buttons have clear `aria-label` text
- Use `role="alert"` on approval result notifications
- Make modal dialogs trap focus and return focus on close
- Announce status changes using `aria-live="polite"` regions

## Responsive Behavior

On mobile, the step progress indicator should stack vertically or use a compact horizontal layout with smaller circles. Approval cards should use full-width layout. The timeline should use `col-12` instead of side-by-side columns. Modals should use `modal-fullscreen-sm-down`. The approval chain list remains functional at all widths. Action buttons should maintain adequate touch target sizes.
