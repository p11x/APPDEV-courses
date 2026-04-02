---
title: "Form Security"
difficulty: 2
category: "Advanced Development"
subcategory: "Security Patterns"
prerequisites:
  - CSRF Token Implementation
  - Input Validation Patterns
  - Secure Form Submission
---

## Overview

Form security in Bootstrap applications covers CSRF protection, client-side and server-side input validation, and secure submission patterns. Bootstrap's form components provide the visual foundation, but security requires implementing CSRF tokens, validating input formats, and preventing common form-based attacks.

CSRF (Cross-Site Request Forgery) attacks trick users into submitting forms on behalf of attackers. CSRF tokens embedded in forms verify that submissions originate from the legitimate application. Client-side validation improves UX but must be supplemented with server-side validation, as client checks can be bypassed.

## Basic Implementation

```html
<!-- Secure Bootstrap form with CSRF token -->
<form method="POST" action="/api/settings" novalidate>
  <!-- CSRF Token -->
  <input type="hidden" name="_csrf" value="{{csrfToken}}">

  <!-- Honeypot field for bot detection -->
  <div style="position: absolute; left: -9999px;" aria-hidden="true">
    <label for="website">Website</label>
    <input type="text" id="website" name="website" tabindex="-1" autocomplete="off">
  </div>

  <div class="mb-3">
    <label for="email" class="form-label">Email address</label>
    <input type="email" class="form-control" id="email" name="email"
           required autocomplete="email"
           pattern="[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$">
    <div class="invalid-feedback">Please enter a valid email address.</div>
  </div>

  <div class="mb-3">
    <label for="password" class="form-label">Password</label>
    <input type="password" class="form-control" id="password" name="password"
           required minlength="8" autocomplete="new-password">
    <div class="invalid-feedback">Password must be at least 8 characters.</div>
  </div>

  <button type="submit" class="btn btn-primary">Save Changes</button>
</form>

<script>
// Client-side validation (supplement, not replace, server validation)
const form = document.querySelector('form');
form.addEventListener('submit', (e) => {
  if (!form.checkValidity()) {
    e.preventDefault();
    e.stopPropagation();
  }
  form.classList.add('was-validated');
});

// Secure submission with fetch
form.addEventListener('submit', async (e) => {
  e.preventDefault();
  if (!form.checkValidity()) return;

  const formData = new FormData(form);
  const data = Object.fromEntries(formData);

  // Honeypot check
  if (data.website) return; // Bot detected

  try {
    const response = await fetch(form.action, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-CSRF-Token': formData.get('_csrf')
      },
      body: JSON.stringify(data),
      credentials: 'same-origin'
    });

    if (!response.ok) throw new Error('Submission failed');
    // Handle success
  } catch (error) {
    // Handle error
  }
});
</script>
```

```js
// Server-side CSRF middleware (Express.js)
const crypto = require('crypto');

function csrfMiddleware(req, res, next) {
  if (['GET', 'HEAD', 'OPTIONS'].includes(req.method)) {
    // Generate token for forms
    const token = crypto.randomBytes(32).toString('hex');
    req.session.csrfToken = token;
    res.locals.csrfToken = token;
    return next();
  }

  // Validate token on mutations
  const token = req.headers['x-csrf-token'] || req.body._csrf;
  if (!token || token !== req.session.csrfToken) {
    return res.status(403).json({ error: 'Invalid CSRF token' });
  }

  next();
}
```

## Best Practices

1. **Always use CSRF tokens** - Include tokens in every form that performs mutations.
2. **Validate on both sides** - Client-side for UX, server-side for security.
3. **Use Bootstrap's validation classes** - `was-validated`, `is-valid`, `is-invalid` for visual feedback.
4. **Sanitize input on the server** - Never trust client-submitted data.
5. **Use autocomplete attributes** - `autocomplete="email"`, `autocomplete="new-password"` for password managers.
6. **Implement rate limiting** - Prevent brute-force attacks on login forms.
7. **Use HTTPS for all forms** - Forms over HTTP expose data in transit.
8. **Set SameSite cookies** - `SameSite=Strict` or `SameSite=Lax` prevents CSRF at the cookie level.
9. **Implement honeypot fields** - Hidden fields catch simple bots without CAPTCHAs.
10. **Log form submissions** - Audit trail for security investigations.

## Common Pitfalls

1. **Client-side validation only** - Easy to bypass with dev tools or direct API calls.
2. **Missing CSRF tokens** - Forms without tokens are vulnerable to cross-site forgery.
3. **Token in GET requests** - CSRF tokens in URLs leak via referrer headers.
4. **Not validating file uploads** - File inputs need additional security checks.
5. **Missing rate limiting** - Login forms without rate limits enable brute-force attacks.

## Accessibility Considerations

Form validation messages must be announced to screen readers. Use `aria-describedby` to link error messages to inputs and `aria-invalid="true"` on invalid fields.

## Responsive Behavior

Form layouts should adapt to viewport size using Bootstrap's responsive grid. Stack fields vertically on mobile.

```html
<div class="row">
  <div class="col-12 col-md-6 mb-3">
    <label for="firstName" class="form-label">First Name</label>
    <input type="text" class="form-control" id="firstName" required>
  </div>
  <div class="col-12 col-md-6 mb-3">
    <label for="lastName" class="form-label">Last Name</label>
    <input type="text" class="form-control" id="lastName" required>
  </div>
</div>
```
