---
title: "Modal Error States"
description: "Displaying error messages in Bootstrap modals, implementing retry buttons, and error modal patterns"
difficulty: 2
tags: ["error-handling", "modals", "ui", "bootstrap"]
prerequisites: ["02_03_Modals"]
---

## Overview

Modals are common containers for operations that can fail — form submissions, file uploads, API calls, and confirmations. Displaying errors within the modal context keeps users focused on the task without navigating away. Bootstrap's modal API provides the structure for error states through dismiss buttons, custom content areas, and event hooks for retry logic.

Error modals follow a consistent pattern: show a loading state during the operation, display an error message with a clear explanation on failure, and offer a retry action that resets the modal to its initial state.

## Basic Implementation

```html
<!-- Error state modal with retry button -->
<div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="deleteModalLabel">Confirm Deletion</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <!-- Default content -->
        <div id="deleteDefaultContent">
          <p>Are you sure you want to delete this item? This action cannot be undone.</p>
        </div>

        <!-- Error content (hidden by default) -->
        <div id="deleteErrorContent" class="d-none">
          <div class="alert alert-danger d-flex align-items-center" role="alert">
            <i class="bi bi-exclamation-triangle-fill me-2"></i>
            <div>Failed to delete the item. The server returned an error.</div>
          </div>
          <p class="text-muted small mb-0">Error code: <span id="errorCode">500</span></p>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-danger" id="confirmDeleteBtn">Delete</button>
        <button type="button" class="btn btn-warning d-none" id="retryDeleteBtn">
          <i class="bi bi-arrow-clockwise me-1"></i>Retry
        </button>
      </div>
    </div>
  </div>
</div>
```

```js
// Modal error state management
const deleteModal = document.getElementById('deleteModal');
const defaultContent = document.getElementById('deleteDefaultContent');
const errorContent = document.getElementById('deleteErrorContent');
const confirmBtn = document.getElementById('confirmDeleteBtn');
const retryBtn = document.getElementById('retryDeleteBtn');

confirmBtn.addEventListener('click', async () => {
  confirmBtn.disabled = true;
  confirmBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span>Deleting...';

  try {
    const response = await fetch('/api/items/123', { method: 'DELETE' });

    if (!response.ok) {
      throw new Error(`Server error: ${response.status}`);
    }

    // Success — close modal
    const modal = bootstrap.Modal.getInstance(deleteModal);
    modal.hide();
  } catch (error) {
    showError(error.message);
  }
});

retryBtn.addEventListener('click', () => {
  resetModalState();
  confirmBtn.click();
});

function showError(message) {
  defaultContent.classList.add('d-none');
  errorContent.classList.remove('d-none');
  errorContent.querySelector('.alert div').textContent = message;
  confirmBtn.classList.add('d-none');
  retryBtn.classList.remove('d-none');
  retryBtn.disabled = false;
}

function resetModalState() {
  defaultContent.classList.remove('d-none');
  errorContent.classList.add('d-none');
  confirmBtn.classList.remove('d-none');
  confirmBtn.disabled = false;
  confirmBtn.textContent = 'Delete';
  retryBtn.classList.add('d-none');
}
```

## Advanced Variations

```html
<!-- Multi-step error modal with progressive detail -->
<div class="modal fade" id="uploadErrorModal" tabindex="-1">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header bg-danger text-white">
        <h5 class="modal-title"><i class="bi bi-x-circle me-2"></i>Upload Failed</h5>
        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
      </div>
      <div class="modal-body">
        <div class="alert alert-danger">
          <strong>3 of 5 files failed to upload.</strong>
        </div>

        <table class="table table-sm">
          <thead>
            <tr>
              <th>File</th>
              <th>Status</th>
              <th>Error</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>report.pdf</td>
              <td><span class="badge bg-success">Uploaded</span></td>
              <td>—</td>
            </tr>
            <tr>
              <td>data.csv</td>
              <td><span class="badge bg-danger">Failed</span></td>
              <td>File too large (max 10MB)</td>
            </tr>
            <tr>
              <td>image.png</td>
              <td><span class="badge bg-success">Uploaded</span></td>
              <td>—</td>
            </tr>
            <tr class="table-danger">
              <td>backup.zip</td>
              <td><span class="badge bg-danger">Failed</span></td>
              <td>Unsupported file type</td>
            </tr>
            <tr class="table-danger">
              <td>video.mp4</td>
              <td><span class="badge bg-danger">Failed</span></td>
              <td>Network timeout</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
        <button type="button" class="btn btn-warning" id="retryFailedBtn">
          Retry Failed Files
        </button>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Keep error messages in the modal — don't navigate away or open a new modal for errors
2. Use Bootstrap alert components (`alert-danger`) inside modal body for error display
3. Always provide a retry action alongside dismiss for recoverable errors
4. Disable the action button and show a spinner during async operations to prevent double-submission
5. Reset modal state when it closes so the error is not visible on next open
6. Use `shown.bs.modal` event to focus the first actionable element, including retry buttons
7. Display specific error messages, not generic "Something went wrong"
8. Include an error code or reference ID for support troubleshooting
9. Use `modal-static` backdrop to prevent closing the modal during error recovery
10. Preserve user input when showing errors — don't clear form fields on failure

## Common Pitfalls

1. **Not resetting error state on modal close** — Users reopening the modal see a stale error from the previous attempt
2. **Blocking the entire modal during loading** — Only disable the action button; keep Cancel and Close accessible
3. **Vague error messages** — "Error occurred" provides no guidance; show what went wrong and what to do next
4. **Missing loading indicators** — Silent delays without spinners cause users to click the action button repeatedly
5. **Error modal stacking** — Opening a second error modal on top of the first creates a confusing layered experience
6. **Not handling network errors separately** — A timeout needs different messaging than a validation error

## Accessibility Considerations

When an error appears in a modal, announce it to screen readers using `role="alert"` on the error container. Move focus to the error message or retry button so keyboard users can act immediately. Ensure the modal's `aria-labelledby` updates if the title changes to indicate an error state. Use `aria-live="polite"` on status messages within the modal.

## Responsive Behavior

Error modals should remain readable on mobile viewports. Use `modal-fullscreen-sm-down` for error modals with detailed error tables on small screens. Ensure the retry button is large enough for touch targets (minimum 44x44px). Stack modal footer buttons vertically on narrow viewports using `d-flex flex-column flex-sm-row`.
