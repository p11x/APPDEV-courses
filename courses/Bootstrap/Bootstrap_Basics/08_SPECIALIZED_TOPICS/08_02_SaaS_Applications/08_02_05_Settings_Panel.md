---
title: "Settings Panel"
module: "SaaS Applications"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_05_Forms", "04_06_Nav_And_Tabs", "05_01_Accordion"]
---

## Overview

Settings panels let users configure their account, preferences, notifications, and integrations. Bootstrap 5 tabs, navs, form switches, and accordion components provide the structure for organized, navigable settings pages that prevent cognitive overload by grouping related options together.

## Basic Implementation

### Tabbed Settings Layout

```html
<div class="container py-5">
  <h2 class="mb-4">Settings</h2>
  <div class="row">
    <div class="col-md-3 mb-4 mb-md-0">
      <div class="nav flex-column nav-pills" role="tablist">
        <button class="nav-link active text-start" data-bs-toggle="pill" data-bs-target="#generalTab">
          <i class="bi bi-gear me-2"></i>General
        </button>
        <button class="nav-link text-start" data-bs-toggle="pill" data-bs-target="#notificationsTab">
          <i class="bi bi-bell me-2"></i>Notifications
        </button>
        <button class="nav-link text-start" data-bs-toggle="pill" data-bs-target="#securityTab">
          <i class="bi bi-shield-lock me-2"></i>Security
        </button>
        <button class="nav-link text-start" data-bs-toggle="pill" data-bs-target="#apiTab">
          <i class="bi bi-code-slash me-2"></i>API Keys
        </button>
        <button class="nav-link text-start" data-bs-toggle="pill" data-bs-target="#dangerTab">
          <i class="bi bi-exclamation-triangle me-2"></i>Danger Zone
        </button>
      </div>
    </div>
    <div class="col-md-9">
      <div class="tab-content">
        <!-- General Settings -->
        <div class="tab-pane fade show active" id="generalTab">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title mb-4">General Settings</h5>
              <form>
                <div class="mb-3">
                  <label for="orgName" class="form-label">Organization Name</label>
                  <input type="text" class="form-control" id="orgName" value="Acme Corp">
                </div>
                <div class="mb-3">
                  <label for="timezone" class="form-label">Timezone</label>
                  <select class="form-select" id="timezone">
                    <option selected>UTC-8 (Pacific Time)</option>
                    <option>UTC-5 (Eastern Time)</option>
                    <option>UTC+0 (GMT)</option>
                    <option>UTC+1 (CET)</option>
                  </select>
                </div>
                <div class="mb-3">
                  <label for="language" class="form-label">Language</label>
                  <select class="form-select" id="language">
                    <option selected>English</option>
                    <option>Spanish</option>
                    <option>French</option>
                    <option>German</option>
                  </select>
                </div>
                <button type="submit" class="btn btn-primary">Save Changes</button>
              </form>
            </div>
          </div>
        </div>

        <!-- Notification Preferences -->
        <div class="tab-pane fade" id="notificationsTab">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title mb-4">Notification Preferences</h5>
              <div class="mb-4">
                <h6>Email Notifications</h6>
                <div class="form-check form-switch mb-2">
                  <input class="form-check-input" type="checkbox" id="emailActivity" checked>
                  <label class="form-check-label" for="emailActivity">Project activity updates</label>
                </div>
                <div class="form-check form-switch mb-2">
                  <input class="form-check-input" type="checkbox" id="emailWeekly" checked>
                  <label class="form-check-label" for="emailWeekly">Weekly digest</label>
                </div>
                <div class="form-check form-switch mb-2">
                  <input class="form-check-input" type="checkbox" id="emailMarketing">
                  <label class="form-check-label" for="emailMarketing">Marketing and product updates</label>
                </div>
              </div>
              <div class="mb-4">
                <h6>Push Notifications</h6>
                <div class="form-check form-switch mb-2">
                  <input class="form-check-input" type="checkbox" id="pushMentions" checked>
                  <label class="form-check-label" for="pushMentions">Mentions and comments</label>
                </div>
                <div class="form-check form-switch mb-2">
                  <input class="form-check-input" type="checkbox" id="pushTasks">
                  <label class="form-check-label" for="pushTasks">Task assignments</label>
                </div>
              </div>
              <button class="btn btn-primary">Save Preferences</button>
            </div>
          </div>
        </div>

        <!-- API Keys -->
        <div class="tab-pane fade" id="apiTab">
          <div class="card">
            <div class="card-body">
              <div class="d-flex justify-content-between align-items-center mb-4">
                <h5 class="card-title mb-0">API Keys</h5>
                <button class="btn btn-primary btn-sm" data-bs-toggle="modal" data-bs-target="#newKeyModal">
                  <i class="bi bi-plus me-1"></i>Generate Key
                </button>
              </div>
              <div class="border rounded p-3 mb-3">
                <div class="d-flex justify-content-between align-items-center">
                  <div>
                    <strong>Production Key</strong>
                    <div class="text-muted small font-monospace">sk_live_****abcd</div>
                    <small class="text-muted">Created Mar 1, 2024 &bull; Last used 2 hours ago</small>
                  </div>
                  <div class="d-flex gap-2">
                    <button class="btn btn-sm btn-outline-secondary" title="Copy">
                      <i class="bi bi-clipboard"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" title="Revoke">
                      <i class="bi bi-trash"></i>
                    </button>
                  </div>
                </div>
              </div>
              <div class="alert alert-warning small mb-0">
                <i class="bi bi-exclamation-triangle me-2"></i>Never share your API keys. Store them securely.
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Danger Zone

```html
<div class="tab-pane fade" id="dangerTab">
  <div class="card border-danger">
    <div class="card-body">
      <h5 class="card-title text-danger mb-4">Danger Zone</h5>
      <div class="d-flex justify-content-between align-items-center border border-danger rounded p-3 mb-3">
        <div>
          <strong>Transfer Ownership</strong>
          <p class="text-muted small mb-0">Transfer this organization to another user.</p>
        </div>
        <button class="btn btn-outline-danger btn-sm">Transfer</button>
      </div>
      <div class="d-flex justify-content-between align-items-center border border-danger rounded p-3">
        <div>
          <strong>Delete Organization</strong>
          <p class="text-muted small mb-0">Permanently delete this organization and all data.</p>
        </div>
        <button class="btn btn-danger btn-sm">Delete</button>
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Group settings into logical categories with vertical nav pills on desktop
2. Use horizontal nav pills or stacked tabs on mobile
3. Use `form-switch` toggles for binary on/off settings
4. Provide inline descriptions for each setting option
5. Show a success toast after saving settings
6. Mark unsaved changes with a visual indicator
7. Use `border-danger` styling for the danger zone section
8. Include a security warning near API key sections
9. Auto-save toggle changes where appropriate
10. Keep the settings nav fixed while content scrolls

## Common Pitfalls

1. **No save confirmation** - Users don't know if settings were saved. Show a toast or inline message.
2. **Too many settings on one page** - Overwhelming settings pages cause decision fatigue. Use tabs to compartmentalize.
3. **Danger zone not separated** - Destructive actions need visual separation from normal settings.
4. **API keys displayed in full** - Never show full API keys after creation. Show only the last 4 characters.
5. **No mobile navigation** - Vertical pills don't work on small screens. Switch to horizontal or stacked layout.
6. **Settings not persisting** - Ensure form state is saved server-side, not just in the UI.

## Accessibility Considerations

- Use `role="tablist"` on the settings navigation
- Ensure each tab panel has `aria-labelledby` pointing to its tab
- Label all form inputs with associated `<label>` elements
- Use `aria-describedby` to link setting descriptions to their controls
- Announce save success/failure with `aria-live="polite"`
- Provide `aria-label` on danger zone buttons describing the consequence

## Responsive Behavior

On **mobile**, the vertical nav pills convert to a horizontal scrollable tab bar at the top. Settings forms go full-width. On **tablet**, the two-column layout (nav + content) works well. On **desktop**, the vertical nav sits in a 3-column sidebar with settings content in the remaining 9 columns.
