---
title: "API Key Management"
description: "Build API key management interfaces with key lists, generation, revocation, copy functionality, and permission controls using Bootstrap 5."
difficulty: 2
estimated_time: "35 minutes"
prerequisites:
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Modals"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Badges"
---

## Overview

API key management is a core SaaS feature that allows developers to generate, view, copy, and revoke API keys for service integration. Bootstrap 5's table, modal, badge, and form components build a secure key management interface that balances usability with security best practices.

The UI must display partial keys for security, provide one-click copy functionality, show key permissions and usage metadata, and offer clear revocation flows. Security considerations like showing full keys only once upon generation and confirming revocation are essential patterns.

## Basic Implementation

### API Key List Table

```html
<div class="table-responsive">
  <table class="table table-hover align-middle">
    <thead class="table-light">
      <tr>
        <th>Name</th>
        <th>Key</th>
        <th>Permissions</th>
        <th>Created</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td>Production App</td>
        <td>
          <code class="user-select-all">sk_live_a1b2c3••••••••x9z0</code>
        </td>
        <td><span class="badge bg-success">Read/Write</span></td>
        <td class="text-muted small">Mar 15, 2026</td>
        <td>
          <button class="btn btn-sm btn-outline-secondary me-1" title="Copy key">
            <i class="bi bi-clipboard"></i>
          </button>
          <button class="btn btn-sm btn-outline-danger" title="Revoke key" data-bs-toggle="modal" data-bs-target="#revokeModal">
            <i class="bi bi-trash"></i>
          </button>
        </td>
      </tr>
      <tr>
        <td>Testing Key</td>
        <td>
          <code class="user-select-all">sk_test_d4e5f6••••••••w1v2</code>
        </td>
        <td><span class="badge bg-info">Read Only</span></td>
        <td class="text-muted small">Feb 28, 2026</td>
        <td>
          <button class="btn btn-sm btn-outline-secondary me-1" title="Copy key">
            <i class="bi bi-clipboard"></i>
          </button>
          <button class="btn btn-sm btn-outline-danger" title="Revoke key" data-bs-toggle="modal" data-bs-target="#revokeModal">
            <i class="bi bi-trash"></i>
          </button>
        </td>
      </tr>
    </tbody>
  </table>
</div>
```

### Generate New Key Button

```html
<button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#generateKeyModal">
  <i class="bi bi-plus-lg me-1"></i> Generate New API Key
</button>
```

### Copy to Clipboard with Feedback

```html
<div class="input-group" style="max-width: 500px;">
  <span class="input-group-text"><i class="bi bi-key"></i></span>
  <input type="text" class="form-control font-monospace" value="sk_live_a1b2c3d4e5f6g7h8i9j0" readonly id="apiKeyDisplay">
  <button class="btn btn-outline-secondary" type="button" id="copyBtn" title="Copy to clipboard">
    <i class="bi bi-clipboard"></i>
  </button>
</div>
```

## Advanced Variations

### Generate Key Modal with Permissions

```html
<div class="modal fade" id="generateKeyModal" tabindex="-1" aria-labelledby="generateKeyModalLabel">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="generateKeyModalLabel">Generate New API Key</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="mb-3">
          <label for="keyName" class="form-label">Key Name</label>
          <input type="text" class="form-control" id="keyName" placeholder="e.g., Production App">
          <div class="form-text">A descriptive name to identify this key.</div>
        </div>
        <div class="mb-3">
          <label class="form-label">Permissions</label>
          <div class="form-check">
            <input class="form-check-input" type="checkbox" id="permRead" checked>
            <label class="form-check-label" for="permRead">Read access</label>
          </div>
          <div class="form-check">
            <input class="form-check-input" type="checkbox" id="permWrite">
            <label class="form-check-label" for="permWrite">Write access</label>
          </div>
          <div class="form-check">
            <input class="form-check-input" type="checkbox" id="permDelete">
            <label class="form-check-label" for="permDelete">Delete access</label>
          </div>
        </div>
        <div class="mb-3">
          <label for="keyExpiry" class="form-label">Expiration (optional)</label>
          <input type="date" class="form-control" id="keyExpiry">
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary">Generate Key</button>
      </div>
    </div>
  </div>
</div>
```

### New Key Display (One-Time View)

```html
<div class="alert alert-warning" role="alert">
  <div class="d-flex align-items-start">
    <i class="bi bi-exclamation-triangle-fill me-2 mt-1"></i>
    <div>
      <strong>Copy your API key now.</strong>
      <p class="mb-2">You will not be able to see it again after closing this dialog.</p>
      <div class="input-group">
        <input type="text" class="form-control font-monospace" value="sk_live_x9y8z7w6v5u4t3s2r1q0" readonly id="newKeyValue">
        <button class="btn btn-warning" type="button" id="copyNewKey">
          <i class="bi bi-clipboard me-1"></i> Copy
        </button>
      </div>
    </div>
  </div>
</div>
```

### Revoke Confirmation Modal

```html
<div class="modal fade" id="revokeModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title text-danger"><i class="bi bi-exclamation-triangle me-1"></i> Revoke API Key</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <p>Are you sure you want to revoke the key <strong>"Production App"</strong>?</p>
        <div class="alert alert-danger mb-0">
          <strong>This action cannot be undone.</strong> Any applications using this key will immediately stop working.
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-danger">Revoke Key</button>
      </div>
    </div>
  </div>
</div>
```

### Key Usage Statistics

```html
<div class="card">
  <div class="card-body">
    <h6 class="card-title">Key Usage - Production App</h6>
    <div class="row g-3">
      <div class="col-4 text-center">
        <div class="fw-bold fs-4">12,450</div>
        <div class="text-muted small">Requests (30d)</div>
      </div>
      <div class="col-4 text-center">
        <div class="fw-bold fs-4">99.8%</div>
        <div class="text-muted small">Success Rate</div>
      </div>
      <div class="col-4 text-center">
        <div class="fw-bold fs-4">45ms</div>
        <div class="text-muted small">Avg Response</div>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Never display the full API key after initial generation. Show only masked versions.
2. Use `font-monospace` class on key display inputs for consistent character rendering
3. Provide one-click copy with visual feedback (icon change from clipboard to checkmark)
4. Require confirmation before revoking keys using a modal dialog
5. Show key creation date and last-used timestamp for usage awareness
6. Use permission badges to clearly indicate access levels (Read, Write, Admin)
7. Implement key expiration options to enforce rotation policies
8. Use `user-select-all` on key code elements for easy triple-click selection
9. Display usage statistics per key to identify unused or overused keys
10. Use `role="alert"` on warning messages about key visibility
11. Prefix keys with environment identifiers (sk_live_, sk_test_) for visual distinction
12. Log key generation and revocation events in an audit trail
13. Limit the number of active keys per account to reduce security surface area

## Common Pitfalls

1. **Showing full keys in the list**: Displaying complete API keys in the table exposes them in shoulder-surfing attacks and browser screenshots.
2. **No revocation confirmation**: Revoking a key without confirmation can break production applications unexpectedly.
3. **Missing copy feedback**: Users do not know if the copy action succeeded without a visual change in the button icon or text.
4. **No key expiration**: Indefinite key lifetimes violate security best practices. Always offer expiration options.
5. **Generic key names**: Allowing unnamed keys makes it impossible to identify which application uses which key.
6. **No loading state during generation**: Key generation API calls without spinner feedback cause duplicate requests.
7. **Hardcoded key values**: Using static HTML for keys means the UI never reflects actual API data.

## Accessibility Considerations

- Label all action buttons with `title` or `aria-label` attributes
- Use `role="alert"` on security warning messages
- Ensure the copy button announces success via `aria-live` region
- Use proper `label` elements associated with form inputs in the generate modal
- Make table columns sortable via keyboard-accessible header controls
- Use `aria-describedby` to link key name to its masked value display
- Provide text confirmation alongside icon-based status changes

## Responsive Behavior

On mobile, the key table should use `table-responsive` for horizontal scrolling. The generate modal should use `modal-fullscreen-sm-down` for better mobile experience. Key usage statistics cards should stack vertically using `col-12`. The copy input group should remain full-width on small screens. Action buttons should maintain minimum touch target sizes of 44x44px.
