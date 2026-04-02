---
title: "Multi-Tenant UI"
module: "Enterprise Apps"
difficulty: 3
estimated_time: "30 min"
prerequisites: ["04_06_Nav_And_Tabs", "05_02_Dropdowns", "04_05_Forms"]
---

## Overview

Multi-tenant applications serve multiple organizations from a single codebase, each with their own branding, settings, and data. Bootstrap 5 dropdowns for tenant switching, CSS custom properties for theming, cards for organization settings, and form controls for white-label configuration enable building flexible multi-tenant interfaces.

## Basic Implementation

### Tenant Switcher

```html
<div class="dropdown">
  <button class="btn btn-outline-secondary dropdown-toggle d-flex align-items-center gap-2" data-bs-toggle="dropdown">
    <div class="bg-primary text-white rounded d-flex align-items-center justify-content-center" style="width:28px;height:28px;font-size:0.7em">AC</div>
    <span>Acme Corp</span>
  </button>
  <div class="dropdown-menu" style="width:280px">
    <div class="px-3 py-2">
      <input type="search" class="form-control form-control-sm" placeholder="Search organizations...">
    </div>
    <div class="list-group list-group-flush">
      <a href="#" class="list-group-item list-group-item-action active d-flex align-items-center">
        <div class="bg-primary text-white rounded d-flex align-items-center justify-content-center me-3" style="width:32px;height:32px;font-size:0.7em">AC</div>
        <div>
          <div class="fw-semibold">Acme Corp</div>
          <small class="opacity-75">Enterprise Plan</small>
        </div>
        <i class="bi bi-check2 ms-auto"></i>
      </a>
      <a href="#" class="list-group-item list-group-item-action d-flex align-items-center">
        <div class="bg-success text-white rounded d-flex align-items-center justify-content-center me-3" style="width:32px;height:32px;font-size:0.7em">GS</div>
        <div>
          <div class="fw-semibold">Globex Solutions</div>
          <small class="text-muted">Pro Plan</small>
        </div>
      </a>
      <a href="#" class="list-group-item list-group-item-action d-flex align-items-center">
        <div class="bg-warning text-white rounded d-flex align-items-center justify-content-center me-3" style="width:32px;height:32px;font-size:0.7em">IN</div>
        <div>
          <div class="fw-semibold">Initech</div>
          <small class="text-muted">Starter Plan</small>
        </div>
      </a>
    </div>
    <div class="dropdown-divider"></div>
    <a class="dropdown-item" href="#">
      <i class="bi bi-plus-circle me-2"></i>Create Organization
    </a>
  </div>
</div>
```

### Organization Settings with Branding

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Organization Branding</h5></div>
  <div class="card-body">
    <div class="row g-4">
      <div class="col-md-6">
        <label class="form-label">Organization Name</label>
        <input type="text" class="form-control" value="Acme Corp">
      </div>
      <div class="col-md-6">
        <label class="form-label">Logo</label>
        <div class="d-flex align-items-center gap-3">
          <div class="bg-light border rounded d-flex align-items-center justify-content-center" style="width:64px;height:64px">
            <i class="bi bi-building text-muted fs-3"></i>
          </div>
          <button class="btn btn-outline-primary btn-sm">Upload Logo</button>
        </div>
      </div>
      <div class="col-md-4">
        <label class="form-label">Primary Color</label>
        <div class="input-group">
          <input type="color" class="form-control form-control-color" value="#0d6efd">
          <input type="text" class="form-control" value="#0d6efd">
        </div>
      </div>
      <div class="col-md-4">
        <label class="form-label">Secondary Color</label>
        <div class="input-group">
          <input type="color" class="form-control form-control-color" value="#6c757d">
          <input type="text" class="form-control" value="#6c757d">
        </div>
      </div>
      <div class="col-md-4">
        <label class="form-label">Accent Color</label>
        <div class="input-group">
          <input type="color" class="form-control form-control-color" value="#198754">
          <input type="text" class="form-control" value="#198754">
        </div>
      </div>
      <div class="col-12">
        <label class="form-label">Custom Domain</label>
        <div class="input-group">
          <span class="input-group-text">https://</span>
          <input type="text" class="form-control" placeholder="app.acmecorp.com">
        </div>
        <small class="text-muted">Point your CNAME record to app.platform.com</small>
      </div>
    </div>
    <button class="btn btn-primary mt-4">Save Branding</button>
  </div>
</div>
```

## Advanced Variations

### White-Label Theme Preview

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">Theme Preview</h5></div>
  <div class="card-body">
    <div class="border rounded p-3" style="--bs-primary: #7c3aed; --bs-btn-bg: #7c3aed; --bs-btn-border-color: #7c3aed;">
      <nav class="navbar navbar-expand-sm mb-3" style="background:var(--bs-primary)">
        <span class="navbar-brand text-white small">Acme Portal</span>
      </nav>
      <div class="d-flex gap-2 mb-3">
        <button class="btn btn-primary btn-sm">Primary</button>
        <button class="btn btn-outline-primary btn-sm">Outline</button>
        <span class="badge bg-primary">Badge</span>
      </div>
      <p class="small text-muted">This preview shows how your brand colors will appear across the application.</p>
    </div>
  </div>
</div>
```

### Tenant-Specific Settings Table

```html
<div class="card">
  <div class="card-header bg-white"><h5 class="mb-0">All Organizations</h5></div>
  <div class="table-responsive">
    <table class="table table-hover align-middle mb-0">
      <thead class="table-light">
        <tr>
          <th>Organization</th>
          <th>Plan</th>
          <th>Users</th>
          <th>Custom Domain</th>
          <th>Status</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-primary text-white rounded d-flex align-items-center justify-content-center me-2" style="width:32px;height:32px;font-size:0.7em">AC</div>
              Acme Corp
            </div>
          </td>
          <td><span class="badge bg-danger">Enterprise</span></td>
          <td>142</td>
          <td><code>app.acme.com</code></td>
          <td><span class="badge bg-success">Active</span></td>
          <td><button class="btn btn-sm btn-outline-secondary"><i class="bi bi-pencil"></i></button></td>
        </tr>
        <tr>
          <td>
            <div class="d-flex align-items-center">
              <div class="bg-success text-white rounded d-flex align-items-center justify-content-center me-2" style="width:32px;height:32px;font-size:0.7em">GS</div>
              Globex Solutions
            </div>
          </td>
          <td><span class="badge bg-primary">Pro</span></td>
          <td>28</td>
          <td><span class="text-muted">-</span></td>
          <td><span class="badge bg-success">Active</span></td>
          <td><button class="btn btn-sm btn-outline-secondary"><i class="bi bi-pencil"></i></button></td>
        </tr>
      </tbody>
    </table>
  </div>
</div>
```

## Best Practices

1. Provide a prominent tenant switcher in the header or sidebar
2. Show tenant name and plan in the switcher dropdown
3. Use CSS custom properties (`--bs-primary`) for dynamic theming
4. Include a theme preview so admins see changes before saving
5. Support custom domains with CNAME configuration guidance
6. Use `form-control-color` for brand color pickers
7. Display all organizations in an admin-only table
8. Show active/suspended status badges per tenant
9. Allow logo upload with a preview thumbnail
10. Store tenant-specific settings server-side, not in local storage

## Common Pitfalls

1. **No tenant context indicator** - Users need to know which organization they're in. Show it persistently.
2. **Theme not applied instantly** - CSS custom properties should update without page reload.
3. **No tenant search** - Users in many orgs can't find theirs. Provide search in the switcher.
4. **Missing admin-only access** - Tenant management should be restricted to org admins.
5. **Color picker without hex input** - Some users need exact values. Provide both picker and text input.
6. **Custom domain not validated** - Verify DNS before activating custom domains.

## Accessibility Considerations

- Use `aria-label="Switch organization"` on the tenant switcher
- Mark the current tenant with `aria-current="true"`
- Label color pickers with `aria-label="Primary brand color"`
- Use `role="combobox"` on the org search input in the switcher
- Announce tenant switch with `aria-live="polite"`

## Responsive Behavior

On **mobile**, the tenant switcher shows only the org logo/initials button. Organization settings forms stack vertically. On **tablet**, the branding form uses a 2-column layout. On **desktop**, the full tenant switcher with name and plan is visible. Color pickers and settings display side by side.
