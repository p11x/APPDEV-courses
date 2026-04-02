---
title: "SaaS 404 & Empty States"
module: "SaaS Applications"
difficulty: 1
estimated_time: "15 min"
prerequisites: ["04_01_Card_Component", "02_02_Utilities"]
---

## Overview

404 pages and empty states are opportunities to guide users rather than dead ends. Bootstrap 5 provides typography utilities, button components, and grid layouts to create helpful error pages, empty dashboards, no-data states, and first-time user experiences that keep users engaged and moving forward.

## Basic Implementation

### Custom 404 Page

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-light">
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-6 text-center">
        <h1 class="display-1 fw-bold text-primary">404</h1>
        <h2 class="h4 mb-3">Page Not Found</h2>
        <p class="text-muted mb-4">The page you're looking for doesn't exist or has been moved.</p>
        <div class="d-flex justify-content-center gap-3 flex-wrap mb-4">
          <a href="dashboard.html" class="btn btn-primary">
            <i class="bi bi-house me-2"></i>Go to Dashboard
          </a>
          <a href="help.html" class="btn btn-outline-secondary">
            <i class="bi bi-question-circle me-2"></i>Get Help
          </a>
        </div>
        <div class="mt-4">
          <div class="input-group mx-auto" style="max-width:400px">
            <span class="input-group-text"><i class="bi bi-search"></i></span>
            <input type="search" class="form-control" placeholder="Search for something...">
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Empty Dashboard State

```html
<div class="text-center py-5">
  <div class="mb-4">
    <div class="bg-primary bg-opacity-10 rounded-circle d-inline-flex align-items-center justify-content-center" style="width:120px;height:120px">
      <i class="bi bi-bar-chart-line display-4 text-primary"></i>
    </div>
  </div>
  <h3 class="mb-2">Your dashboard is empty</h3>
  <p class="text-muted mb-4">Create your first project to start seeing data and insights here.</p>
  <a href="#" class="btn btn-primary btn-lg">
    <i class="bi bi-plus-circle me-2"></i>Create Your First Project
  </a>
</div>
```

## Advanced Variations

### No Search Results State

```html
<div class="text-center py-5">
  <i class="bi bi-search display-1 text-muted mb-3"></i>
  <h4>No results for "xyzqw"</h4>
  <p class="text-muted mb-4">Try different keywords or check your spelling.</p>
  <h6 class="text-muted mb-3">Suggested searches:</h6>
  <div class="d-flex justify-content-center gap-2 flex-wrap mb-4">
    <a href="#" class="badge bg-light text-dark text-decoration-none border py-2 px-3">Getting started</a>
    <a href="#" class="badge bg-light text-dark text-decoration-none border py-2 px-3">API documentation</a>
    <a href="#" class="badge bg-light text-dark text-decoration-none border py-2 px-3">Billing</a>
  </div>
</div>
```

### First-Time User Welcome

```html
<div class="container py-5">
  <div class="row justify-content-center">
    <div class="col-lg-8">
      <div class="text-center mb-5">
        <h2>Welcome, John! 👋</h2>
        <p class="text-muted">Here are some things you can do to get started:</p>
      </div>
      <div class="row g-4">
        <div class="col-md-4">
          <div class="card h-100 text-center border-primary">
            <div class="card-body p-4">
              <div class="bg-primary bg-opacity-10 rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style="width:64px;height:64px">
                <i class="bi bi-folder-plus text-primary fs-3"></i>
              </div>
              <h6>Create a Project</h6>
              <p class="small text-muted">Set up your first project and invite your team.</p>
              <a href="#" class="btn btn-primary btn-sm">Get Started</a>
            </div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card h-100 text-center">
            <div class="card-body p-4">
              <div class="bg-success bg-opacity-10 rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style="width:64px;height:64px">
                <i class="bi bi-people text-success fs-3"></i>
              </div>
              <h6>Invite Your Team</h6>
              <p class="small text-muted">Collaborate with colleagues on your projects.</p>
              <a href="#" class="btn btn-outline-primary btn-sm">Invite Now</a>
            </div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card h-100 text-center">
            <div class="card-body p-4">
              <div class="bg-info bg-opacity-10 rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style="width:64px;height:64px">
                <i class="bi bi-book text-info fs-3"></i>
              </div>
              <h6>Read the Docs</h6>
              <p class="small text-muted">Learn how to get the most out of the platform.</p>
              <a href="#" class="btn btn-outline-primary btn-sm">View Docs</a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Maintenance / Server Error

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center">
  <div class="text-center">
    <div class="display-1 mb-4">🔧</div>
    <h1 class="h2 mb-3">We'll be right back</h1>
    <p class="text-muted mb-4">We're performing scheduled maintenance. We should be back in about 30 minutes.</p>
    <div class="alert alert-info d-inline-block">
      <i class="bi bi-clock me-2"></i>Estimated time: 2:00 PM - 2:30 PM UTC
    </div>
    <div class="mt-4">
      <a href="https://status.example.com" class="btn btn-outline-primary">Check Status Page</a>
    </div>
  </div>
</div>
```

## Best Practices

1. Use friendly, non-technical language ("Page not found" not "404 Error")
2. Provide a clear path forward with action buttons
3. Include a search bar on 404 pages so users can find what they need
4. Use illustrations or icons to make empty states visually engaging
5. On empty dashboards, guide users to their first action
6. Show suggested searches or popular links on no-results pages
7. Maintain the app's navigation and branding on error pages
8. Provide a "Contact Support" option for persistent issues
9. Use first-time user states to drive activation
10. Keep error messages honest and specific

## Common Pitfalls

1. **Generic 404 with no guidance** - Users leave immediately. Provide navigation and search.
2. **Breaking out of the app layout** - 404 pages should stay within the app shell for consistency.
3. **No empty state for new users** - Blank dashboards are confusing. Show onboarding guidance.
4. **Blaming the user** - "You typed the wrong URL" is unhelpful. Focus on solutions.
5. **No retry option on errors** - For temporary failures, provide a "Try Again" button.
6. **Missing error logging** - 404 pages should be tracked to identify broken links.

## Accessibility Considerations

- Use `<h1>` for the main error heading
- Provide descriptive text for action buttons, not just icons
- Ensure sufficient color contrast on error text
- Use `role="alert"` on error messages
- Include skip navigation on error pages that share the app layout

## Responsive Behavior

404 and empty states are naturally responsive. On **mobile**, content stacks vertically with full-width buttons. On **desktop**, content centers in the viewport with a max-width container. First-time user cards stack on mobile (1 column) and display side by side on desktop (3 columns).
