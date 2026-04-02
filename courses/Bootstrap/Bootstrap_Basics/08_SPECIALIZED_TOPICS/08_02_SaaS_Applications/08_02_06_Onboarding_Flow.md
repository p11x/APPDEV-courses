---
title: "Onboarding Flow"
module: "SaaS Applications"
difficulty: 2
estimated_time: "25 min"
prerequisites: ["04_06_Nav_And_Tabs", "04_07_Modal", "04_09_Badges"]
---

## Overview

Onboarding flows guide new users through initial setup, ensuring they experience the product's value quickly. Bootstrap 5 provides stepper components, modals, tooltips, and progress indicators to build welcome screens, setup wizards, and interactive tours that reduce time-to-value and increase activation rates.

## Basic Implementation

### Welcome Screen

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-light">
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-6 text-center">
        <div class="mb-4">
          <span class="display-1">🚀</span>
        </div>
        <h1 class="display-5 fw-bold mb-3">Welcome to ProductName</h1>
        <p class="lead text-muted mb-4">Let's get you set up in just 3 steps. It'll take about 2 minutes.</p>
        <a href="#step1" class="btn btn-primary btn-lg px-5">Get Started</a>
        <div class="mt-3">
          <a href="dashboard.html" class="text-muted small">Skip setup, take me to the dashboard</a>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Progress Stepper

```html
<div class="container py-5">
  <div class="row justify-content-center">
    <div class="col-lg-8">
      <!-- Stepper -->
      <div class="d-flex justify-content-between mb-5 position-relative">
        <div class="progress position-absolute w-100" style="height:3px;top:20px;z-index:0">
          <div class="progress-bar" style="width:33%"></div>
        </div>
        <div class="text-center position-relative z-1">
          <div class="rounded-circle bg-primary text-white d-inline-flex align-items-center justify-content-center mb-2" style="width:40px;height:40px">1</div>
          <div class="small fw-semibold">Profile</div>
        </div>
        <div class="text-center position-relative z-1">
          <div class="rounded-circle bg-light border d-inline-flex align-items-center justify-content-center mb-2" style="width:40px;height:40px">2</div>
          <div class="small text-muted">Team</div>
        </div>
        <div class="text-center position-relative z-1">
          <div class="rounded-circle bg-light border d-inline-flex align-items-center justify-content-center mb-2" style="width:40px;height:40px">3</div>
          <div class="small text-muted">Preferences</div>
        </div>
      </div>

      <!-- Step Content -->
      <div class="card">
        <div class="card-body p-4">
          <h4 class="mb-4">Tell us about yourself</h4>
          <form>
            <div class="mb-3">
              <label for="company" class="form-label">Company Name</label>
              <input type="text" class="form-control" id="company" placeholder="Acme Corp">
            </div>
            <div class="mb-3">
              <label for="role" class="form-label">Your Role</label>
              <select class="form-select" id="role">
                <option value="">Select your role...</option>
                <option>Founder / CEO</option>
                <option>Product Manager</option>
                <option>Developer</option>
                <option>Designer</option>
                <option>Other</option>
              </select>
            </div>
            <div class="mb-3">
              <label for="teamSize" class="form-label">Team Size</label>
              <select class="form-select" id="teamSize">
                <option value="">Select team size...</option>
                <option>Just me</option>
                <option>2-5</option>
                <option>6-20</option>
                <option>21-50</option>
                <option>50+</option>
              </select>
            </div>
            <div class="d-flex justify-content-between mt-4">
              <button class="btn btn-outline-secondary" disabled>Back</button>
              <button type="submit" class="btn btn-primary">Continue <i class="bi bi-arrow-right ms-1"></i></button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

### Setup Checklist Card

```html
<div class="card">
  <div class="card-header bg-white">
    <h5 class="mb-0">Setup Progress</h5>
  </div>
  <div class="card-body">
    <div class="progress mb-4" style="height:8px">
      <div class="progress-bar bg-success" style="width:60%"></div>
    </div>
    <p class="text-muted small mb-3">3 of 5 steps completed</p>
    <div class="list-group list-group-flush">
      <a href="#" class="list-group-item list-group-item-action d-flex align-items-center">
        <i class="bi bi-check-circle-fill text-success me-3"></i>
        <span>Create account</span>
      </a>
      <a href="#" class="list-group-item list-group-item-action d-flex align-items-center">
        <i class="bi bi-check-circle-fill text-success me-3"></i>
        <span>Complete profile</span>
      </a>
      <a href="#" class="list-group-item list-group-item-action d-flex align-items-center">
        <i class="bi bi-check-circle-fill text-success me-3"></i>
        <span>Invite team members</span>
      </a>
      <a href="#" class="list-group-item list-group-item-action d-flex align-items-center">
        <i class="bi bi-circle text-muted me-3"></i>
        <span class="text-primary fw-semibold">Create first project</span>
        <span class="badge bg-primary ms-auto">Next</span>
      </a>
      <a href="#" class="list-group-item list-group-item-action d-flex align-items-center">
        <i class="bi bi-circle text-muted me-3"></i>
        <span class="text-muted">Connect integrations</span>
      </a>
    </div>
  </div>
</div>
```

### Tooltip Tour

```html
<button class="btn btn-primary" data-bs-toggle="tooltip" data-bs-placement="bottom"
        data-bs-title="Click here to create your first project!"
        data-bs-trigger="manual" id="tourStep1">
  New Project
</button>

<script>
  // Trigger the tooltip tour programmatically
  const tooltip = new bootstrap.Tooltip(document.getElementById('tourStep1'), {
    customClass: 'tour-tooltip'
  });
  tooltip.show();
  setTimeout(() => tooltip.hide(), 5000);
</script>
```

### Completion Celebration

```html
<div class="text-center py-5">
  <div class="display-1 mb-4">🎉</div>
  <h2 class="display-6 fw-bold mb-3">You're All Set!</h2>
  <p class="lead text-muted mb-4">Your workspace is ready. Start building something amazing.</p>
  <div class="d-flex justify-content-center gap-3 flex-wrap">
    <a href="project-new.html" class="btn btn-primary btn-lg">
      <i class="bi bi-plus-circle me-2"></i>Create First Project
    </a>
    <a href="dashboard.html" class="btn btn-outline-secondary btn-lg">
      Go to Dashboard
    </a>
  </div>
</div>
```

## Best Practices

1. Limit onboarding to 3-5 steps to respect user time
2. Show a progress bar so users know how much remains
3. Allow users to skip onboarding and configure later
4. Pre-fill fields where possible (name from account, timezone from browser)
5. Use welcoming, friendly copy ("Let's get started!")
6. Provide a checklist card on the dashboard showing remaining setup steps
7. Use tooltip tours for first-time feature discovery
8. Celebrate completion with a success screen
9. Save progress so users can resume if they leave
10. Use tooltips (`data-bs-toggle="tooltip"`) for contextual help during setup

## Common Pitfalls

1. **Too many steps** - More than 5 steps causes abandonment. Keep it minimal.
2. **No skip option** - Power users want to explore on their own. Always provide a skip link.
3. **Requiring onboarding** - Some users have used similar products. Don't force setup.
4. **No progress persistence** - Users who leave mid-setup lose progress. Save to the server.
5. **Tooltip tour too aggressive** - Auto-triggering multiple tooltips overwhelms users. Space them out.
6. **No celebration on completion** - Missing the success moment reduces satisfaction. Celebrate!

## Accessibility Considerations

- Use `role="progressbar"` with `aria-valuenow`, `aria-valuemin`, `aria-valuemax` on progress indicators
- Announce step transitions with `aria-live="polite"`
- Ensure all form inputs have associated labels
- Provide `aria-label` on stepper circles (e.g., "Step 1 of 3: Profile")
- Use `aria-current="step"` on the active step indicator
- Tooltip content should be accessible via keyboard focus

## Responsive Behavior

On **mobile**, the progress stepper uses a compact horizontal bar with step numbers. The welcome screen stacks content vertically with full-width buttons. On **tablet and desktop**, the stepper shows labels below each step number. Cards center within an 8-column container for focus.
