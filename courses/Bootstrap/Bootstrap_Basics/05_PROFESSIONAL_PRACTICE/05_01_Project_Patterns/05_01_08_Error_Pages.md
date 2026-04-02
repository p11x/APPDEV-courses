---
title: Error Pages
category: Professional Practice
difficulty: 1
time: 30 min
tags: bootstrap5, error-pages, 404, 500, maintenance, offline, responsive
---

## Overview

Error pages are essential for guiding users when something goes wrong. A well-designed error page maintains brand consistency, provides clear messaging, and offers navigation back to safety. Bootstrap 5 utilities, cards, buttons, and grid classes create polished 404, 500, maintenance, and offline error pages with minimal effort.

## Basic Implementation

### 404 Not Found Page

The most common error page. Use a full-viewport centered layout with a clear message and navigation options.

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-light">
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-6 text-center">
        <h1 class="display-1 fw-bold text-primary">404</h1>
        <h2 class="fw-bold mb-3">Page Not Found</h2>
        <p class="text-muted mb-4">The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.</p>
        <div class="d-flex justify-content-center gap-3">
          <a href="/" class="btn btn-primary"><i class="bi bi-house me-1"></i>Go Home</a>
          <button class="btn btn-outline-secondary" onclick="history.back()"><i class="bi bi-arrow-left me-1"></i>Go Back</button>
        </div>
      </div>
    </div>
  </div>
</div>
```

### 500 Internal Server Error

A server error page should be empathetic and reassuring while providing support options.

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-light">
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-6 text-center">
        <div class="mb-4">
          <div class="bg-danger bg-opacity-10 rounded-circle d-inline-flex align-items-center justify-content-center" style="width:96px;height:96px">
            <i class="bi bi-exclamation-triangle fs-1 text-danger"></i>
          </div>
        </div>
        <h1 class="display-1 fw-bold text-danger">500</h1>
        <h2 class="fw-bold mb-3">Something Went Wrong</h2>
        <p class="text-muted mb-4">We're experiencing an internal server error. Our team has been notified and is working on a fix.</p>
        <div class="d-flex justify-content-center gap-3">
          <a href="/" class="btn btn-primary"><i class="bi bi-house me-1"></i>Go Home</a>
          <a href="/contact" class="btn btn-outline-secondary"><i class="bi bi-envelope me-1"></i>Contact Support</a>
        </div>
        <p class="text-muted small mt-4">Error reference: <code id="errorId">ERR-2026-0401-500</code></p>
      </div>
    </div>
  </div>
</div>
```

### Maintenance Page

Display during scheduled downtime with a countdown timer and status link.

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-warning bg-opacity-10">
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-6 text-center">
        <div class="mb-4">
          <div class="bg-warning bg-opacity-25 rounded-circle d-inline-flex align-items-center justify-content-center" style="width:96px;height:96px">
            <i class="bi bi-tools fs-1 text-warning"></i>
          </div>
        </div>
        <h2 class="fw-bold mb-3">We'll Be Right Back</h2>
        <p class="text-muted mb-4">We're performing scheduled maintenance to improve your experience. We expect to be back online shortly.</p>
        <div class="card border-0 shadow-sm d-inline-block mb-4">
          <div class="card-body py-3 px-4">
            <div class="d-flex gap-4 text-center">
              <div>
                <h4 class="fw-bold mb-0" id="hours">02</h4>
                <small class="text-muted">Hours</small>
              </div>
              <div class="fs-4 text-muted">:</div>
              <div>
                <h4 class="fw-bold mb-0" id="minutes">30</h4>
                <small class="text-muted">Minutes</small>
              </div>
              <div class="fs-4 text-muted">:</div>
              <div>
                <h4 class="fw-bold mb-0" id="seconds">00</h4>
                <small class="text-muted">Seconds</small>
              </div>
            </div>
          </div>
        </div>
        <p class="mb-0"><a href="https://status.example.com" class="text-decoration-none"><i class="bi bi-broadcast me-1"></i>Check Status Page</a></p>
      </div>
    </div>
  </div>
</div>
```

### Offline Page

For Progressive Web Apps, an offline fallback page informs users they have lost connectivity.

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-light">
  <div class="container">
    <div class="row justify-content-center">
      <div class="col-lg-5 text-center">
        <div class="mb-4">
          <div class="bg-secondary bg-opacity-10 rounded-circle d-inline-flex align-items-center justify-content-center" style="width:96px;height:96px">
            <i class="bi bi-wifi-off fs-1 text-secondary"></i>
          </div>
        </div>
        <h2 class="fw-bold mb-3">You're Offline</h2>
        <p class="text-muted mb-4">It looks like you've lost your internet connection. Check your network and try again.</p>
        <button class="btn btn-primary" onclick="location.reload()"><i class="bi bi-arrow-clockwise me-1"></i>Retry</button>
        <div class="alert alert-info mt-4 text-start" role="alert">
          <i class="bi bi-info-circle me-2"></i>
          Some cached pages may still be available. <a href="/offline.html" class="alert-link">View cached content</a>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Maintenance Countdown JavaScript

```javascript
function startCountdown(totalSeconds) {
  const hoursEl = document.getElementById('hours');
  const minutesEl = document.getElementById('minutes');
  const secondsEl = document.getElementById('seconds');

  const interval = setInterval(() => {
    if (totalSeconds <= 0) {
      clearInterval(interval);
      location.reload();
      return;
    }
    const h = Math.floor(totalSeconds / 3600);
    const m = Math.floor((totalSeconds % 3600) / 60);
    const s = totalSeconds % 60;
    hoursEl.textContent = String(h).padStart(2, '0');
    minutesEl.textContent = String(m).padStart(2, '0');
    secondsEl.textContent = String(s).padStart(2, '0');
    totalSeconds--;
  }, 1000);
}

startCountdown(9000); // 2 hours 30 minutes
```

## Advanced Variations

- **Branded Error Pages:** Add the company logo, brand colors, and a custom illustration above the error message.
- **Search Integration:** On the 404 page, include a search box (`input-group` with a search button) so users can find what they need.
- **Error Logging:** On the 500 page, generate a unique error ID and display it so support can trace the issue.
- **Animated Illustration:** Use an SVG or Lottie animation (e.g., a broken robot) for a friendly, branded touch.
- **Auto-Redirect:** On the maintenance page, add a meta refresh or JS redirect when the countdown ends.

## Best Practices

1. Use `min-vh-100 d-flex align-items-center justify-content-center` for full-viewport centered layouts.
2. Keep error messages concise, empathetic, and jargon-free.
3. Always provide at least two navigation options: "Go Home" and "Go Back" (or "Contact Support").
4. Use `display-1 fw-bold` for the error code to make it visually prominent.
5. Apply contextual background colors: `bg-primary` for 404, `bg-danger` for 500, `bg-warning` for maintenance.
6. Use `bi` Bootstrap Icons for visual context (tools, wifi-off, exclamation-triangle).
7. Center content in a `col-lg-5` or `col-lg-6` column for readable line lengths.
8. Include an error reference code on 500 pages for support tracking.
9. Use `btn-outline-secondary` for the secondary action to differentiate it from the primary CTA.
10. Apply `gap-3` on the button container for consistent spacing.
11. Use `text-muted` on body copy to de-emphasize it relative to the heading.
12. Add a `retry` button on the offline page that calls `location.reload()`.

## Common Pitfalls

1. **No navigation options:** Leaving users stranded without a "Go Home" button causes frustration.
2. **Generic messages:** "Error occurred" without context is unhelpful; explain what happened and what to do.
3. **Missing `min-vh-100`:** Short content sticks to the top of the viewport without full-viewport height.
4. **No offline fallback in PWA:** Without a cached offline page, PWAs show a browser default error.
5. **Inaccessible icons:** Decorative icons inside buttons need `aria-hidden="true"` when paired with text.
6. **Too much technical detail:** Showing stack traces to end users creates confusion and security concerns.
7. **Forgetting mobile breakpoints:** Test that `display-1` headings don't overflow on narrow screens.

## Accessibility Considerations

- Use `<main role="main">` as the wrapper for error page content.
- Add `aria-live="polite"` on the countdown timer so screen readers announce updates.
- Ensure all buttons have visible text labels, not just icon-only content.
- Use `role="alert"` on error messages so assistive technologies announce them immediately.
- Provide `alt` text on any illustrations or images used on error pages.

## Responsive Behavior

| Breakpoint | Layout | Heading Size | Buttons |
|------------|--------|-------------|---------|
| `<576px` | Full width | `display-2` | Stacked |
| `≥576px` | `col-lg-6` centered | `display-1` | Side-by-side |
| `≥992px` | `col-lg-5/6` centered | `display-1` | Side-by-side |

All error pages use the same centered, single-column pattern. The countdown card on the maintenance page uses `d-inline-block` to size itself to content.
