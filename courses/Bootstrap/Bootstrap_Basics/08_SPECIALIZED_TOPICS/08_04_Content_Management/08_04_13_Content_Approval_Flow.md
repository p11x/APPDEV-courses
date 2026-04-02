---
title: "Content Approval Flow"
description: "Build content review workflows with submit-for-review, reviewer comments, and publish/reject actions using Bootstrap 5."
difficulty: 2
estimated_time: "30 minutes"
prerequisites:
  - "Bootstrap 5 Cards"
  - "Bootstrap 5 Badges"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Modals"
---

## Overview

Content approval flows manage the editorial process from draft to publication. Bootstrap 5's cards, badges, forms, and modals build workflows where authors submit content for review, reviewers provide feedback, and editors approve or reject submissions. This is essential for organizations with editorial governance requirements.

The flow tracks content state transitions: Draft, In Review, Approved, Published, and Rejected. Each state has associated actions, and reviewer comments provide feedback context. Status badges and timeline visualizations make the workflow transparent.

## Basic Implementation

### Submit for Review Button

```html
<div class="card border-info mb-3">
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-center">
      <div>
        <span class="badge bg-info mb-1"><i class="bi bi-hourglass-split me-1"></i>Ready for Review</span>
        <h6 class="mb-1">Your article is ready to submit</h6>
        <p class="text-muted small mb-0">Once submitted, a reviewer will be assigned automatically.</p>
      </div>
      <button class="btn btn-info" data-bs-toggle="modal" data-bs-target="#submitReviewModal">
        <i class="bi bi-send me-1"></i>Submit for Review
      </button>
    </div>
  </div>
</div>
```

### Submit for Review Modal

```html
<div class="modal fade" id="submitReviewModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Submit for Review</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <p>You are submitting <strong>"Q1 Product Update"</strong> for editorial review.</p>
        <div class="mb-3">
          <label for="reviewNotes" class="form-label">Notes for Reviewer (optional)</label>
          <textarea class="form-control" id="reviewNotes" rows="3" placeholder="Any specific areas you'd like feedback on..."></textarea>
        </div>
        <div class="mb-3">
          <label for="reviewPriority" class="form-label">Priority</label>
          <select class="form-select" id="reviewPriority">
            <option value="normal" selected>Normal</option>
            <option value="high">High - Needs quick turnaround</option>
            <option value="low">Low - No rush</option>
          </select>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary">Submit</button>
      </div>
    </div>
  </div>
</div>
```

### Review Queue Table

```html
<div class="table-responsive">
  <table class="table table-hover align-middle">
    <thead class="table-light">
      <tr>
        <th>Title</th>
        <th>Author</th>
        <th>Status</th>
        <th>Submitted</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Q1 Product Update</td>
        <td>Jane Cooper</td>
        <td><span class="badge bg-info">In Review</span></td>
        <td class="text-muted small">2 hours ago</td>
        <td>
          <button class="btn btn-sm btn-outline-primary me-1"><i class="bi bi-eye"></i> Review</button>
        </td>
      </tr>
      <tr>
        <td>Engineering Blog Post</td>
        <td>Mike Chen</td>
        <td><span class="badge bg-warning text-dark">Changes Requested</span></td>
        <td class="text-muted small">1 day ago</td>
        <td>
          <button class="btn btn-sm btn-outline-warning me-1"><i class="bi bi-pencil"></i> Edit</button>
        </td>
      </tr>
      <tr>
        <td>March Newsletter</td>
        <td>Sarah Park</td>
        <td><span class="badge bg-success">Approved</span></td>
        <td class="text-muted small">3 days ago</td>
        <td>
          <button class="btn btn-sm btn-outline-success"><i class="bi bi-send"></i> Publish</button>
        </td>
      </tr>
    </tbody>
  </table>
</div>
```

## Advanced Variations

### Review Detail with Comments

```html
<div class="card mb-3">
  <div class="card-header d-flex justify-content-between align-items-center">
    <strong>Review: Q1 Product Update</strong>
    <span class="badge bg-info">In Review</span>
  </div>
  <div class="card-body">
    <div class="mb-3">
      <div class="d-flex align-items-start mb-3">
        <img src="avatar1.jpg" class="rounded-circle me-2" width="36" height="36" alt="">
        <div class="flex-grow-1">
          <div class="d-flex justify-content-between">
            <strong>Jane Cooper</strong>
            <small class="text-muted">Author - 2 hours ago</small>
          </div>
          <p class="mb-0 small">Submitted for review. Please pay attention to the product pricing section.</p>
        </div>
      </div>
      <div class="d-flex align-items-start mb-3 bg-light rounded p-2">
        <img src="avatar2.jpg" class="rounded-circle me-2" width="36" height="36" alt="">
        <div class="flex-grow-1">
          <div class="d-flex justify-content-between">
            <strong>Editor Mike</strong>
            <small class="text-muted">Reviewer - 1 hour ago</small>
          </div>
          <p class="mb-1 small">The headline needs to be more specific. Can you add the quarter?</p>
          <span class="badge bg-warning text-dark">Changes Requested</span>
        </div>
      </div>
    </div>
    <div class="border-top pt-3">
      <label class="form-label small">Add Comment</label>
      <div class="input-group">
        <textarea class="form-control" rows="2" placeholder="Write your response..."></textarea>
        <button class="btn btn-primary align-self-end"><i class="bi bi-send"></i></button>
      </div>
    </div>
  </div>
</div>
```

### Approve/Reject Action Bar

```html
<div class="card bg-light">
  <div class="card-body py-2 d-flex justify-content-between align-items-center">
    <span class="text-muted">Review actions for <strong>"Q1 Product Update"</strong></span>
    <div class="d-flex gap-2">
      <button class="btn btn-outline-warning btn-sm" data-bs-toggle="modal" data-bs-target="#changesModal">
        <i class="bi bi-arrow-return-left me-1"></i>Request Changes
      </button>
      <button class="btn btn-success btn-sm" data-bs-toggle="modal" data-bs-target="#approveModal">
        <i class="bi bi-check-lg me-1"></i>Approve
      </button>
      <button class="btn btn-danger btn-sm" data-bs-toggle="modal" data-bs-target="#rejectModal">
        <i class="bi bi-x-lg me-1"></i>Reject
      </button>
    </div>
  </div>
</div>
```

### Content Status Timeline

```html
<div class="mb-4">
  <h6>Status History</h6>
  <div class="border-start border-3 border-secondary ps-3 mb-3">
    <div class="d-flex justify-content-between">
      <strong>Draft Created</strong>
      <small class="text-muted">Mar 28, 2026</small>
    </div>
    <p class="text-muted small mb-0">Created by Jane Cooper</p>
  </div>
  <div class="border-start border-3 border-info ps-3 mb-3">
    <div class="d-flex justify-content-between">
      <strong>Submitted for Review</strong>
      <small class="text-muted">Mar 30, 2026</small>
    </div>
    <p class="text-muted small mb-0">Assigned to Editor Mike</p>
  </div>
  <div class="border-start border-3 border-warning ps-3 mb-3">
    <div class="d-flex justify-content-between">
      <strong>Changes Requested</strong>
      <small class="text-muted">Mar 31, 2026</small>
    </div>
    <p class="text-muted small mb-0">"Headline needs the quarter added"</p>
  </div>
  <div class="border-start border-3 border-info ps-3">
    <div class="d-flex justify-content-between">
      <strong>Resubmitted for Review</strong>
      <small class="text-muted">Apr 1, 2026</small>
    </div>
    <p class="text-muted small mb-0">Changes applied, awaiting re-review</p>
  </div>
</div>
```

### Reject with Reason Modal

```html
<div class="modal fade" id="rejectModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title text-danger">Reject Content</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <p>This will send the content back to the author.</p>
        <div class="mb-3">
          <label class="form-label">Reason <span class="text-danger">*</span></label>
          <div class="form-check mb-2">
            <input class="form-check-input" type="radio" name="rejectReason" id="reasonQuality">
            <label class="form-check-label" for="reasonQuality">Does not meet quality standards</label>
          </div>
          <div class="form-check mb-2">
            <input class="form-check-input" type="radio" name="rejectReason" id="reasonAccuracy">
            <label class="form-check-label" for="reasonAccuracy">Factual inaccuracies</label>
          </div>
          <div class="form-check mb-2">
            <input class="form-check-input" type="radio" name="rejectReason" id="reasonOffbrand">
            <label class="form-check-label" for="reasonOffbrand">Off-brand or inappropriate</label>
          </div>
        </div>
        <div class="mb-3">
          <label for="rejectComment" class="form-label">Detailed Feedback</label>
          <textarea class="form-control" id="rejectComment" rows="4" required></textarea>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-danger">Reject Content</button>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Use distinct badge colors for each status: gray=draft, blue=in review, yellow=changes requested, green=approved, red=rejected
2. Always require a reason when rejecting or requesting changes
3. Show a comment thread on reviewed content for communication
4. Include a status timeline showing the full editorial history
5. Allow authors to resubmit after making requested changes
6. Assign reviewers automatically or allow manual assignment
7. Use modals for approve/reject/changes-requested actions
8. Provide priority levels (normal, high, low) for review requests
9. Show submission timestamps and reviewer information
10. Include a review queue with filtering by status and priority
11. Notify authors and reviewers of status changes
12. Use consistent action button colors: green=approve, red=reject, yellow=request changes
13. Support inline comments on specific sections of content

## Common Pitfalls

1. **No rejection reason**: Rejected authors need specific feedback to improve. Always require a detailed reason.
2. **Missing status history**: Without a timeline, users cannot trace how content moved through the workflow.
3. **No resubmission flow**: After changes are requested, there must be a clear path to resubmit.
4. **Unclear reviewer assignment**: Authors need to know who is reviewing their content and when.
5. **No notification on status change**: Authors should be notified when their content is approved, rejected, or has changes requested.
6. **Missing priority levels**: All reviews treated equally regardless of urgency leads to important content being delayed.
7. **No comment thread**: Review feedback without context in a conversation thread is harder to address.

## Accessibility Considerations

- Use `aria-label` on all action buttons (approve, reject, request changes)
- Provide `role="alert"` on status change notifications
- Use proper heading hierarchy in review detail views
- Ensure comment threads are navigable with keyboard
- Use `aria-live="polite"` on new comment additions
- Associate form labels with inputs in all review modals
- Announce workflow state changes using `aria-live` regions

## Responsive Behavior

On mobile, the review queue table should use `table-responsive`. Review detail cards should use full-width layout. The action bar should stack buttons vertically on small screens. Status timeline items should remain readable with proper spacing. Modals should use `modal-fullscreen-sm-down`. Comment input should remain full-width on mobile.
