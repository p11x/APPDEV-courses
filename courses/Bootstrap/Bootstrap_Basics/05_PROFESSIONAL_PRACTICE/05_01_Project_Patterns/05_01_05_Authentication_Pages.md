---
title: Authentication Pages
category: Professional Practice
difficulty: 2
time: 40 min
tags: bootstrap5, authentication, login, register, forms, validation, responsive
---

## Overview

Authentication pages are a universal UI pattern. Bootstrap 5 provides form controls, validation states, input groups, and card components that combine into a centered, responsive login/register layout. This lesson covers login, registration, password reset, and social login patterns.

## Basic Implementation

### Centered Login Card

The standard login pattern uses a full-viewport container with a vertically and horizontally centered card.

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-light py-5">
  <div class="card border-0 shadow-sm" style="max-width: 420px; width: 100%;">
    <div class="card-body p-4 p-md-5">
      <div class="text-center mb-4">
        <img src="logo.svg" alt="Company Logo" width="48" class="mb-3">
        <h3 class="fw-bold">Welcome Back</h3>
        <p class="text-muted">Sign in to your account</p>
      </div>
      <form>
        <div class="mb-3">
          <label for="loginEmail" class="form-label">Email address</label>
          <div class="input-group">
            <span class="input-group-text"><i class="bi bi-envelope"></i></span>
            <input type="email" class="form-control" id="loginEmail" placeholder="name@example.com" required>
          </div>
        </div>
        <div class="mb-3">
          <label for="loginPassword" class="form-label">Password</label>
          <div class="input-group">
            <span class="input-group-text"><i class="bi bi-lock"></i></span>
            <input type="password" class="form-control" id="loginPassword" placeholder="Enter password" required>
            <button class="btn btn-outline-secondary" type="button" id="togglePassword">
              <i class="bi bi-eye"></i>
            </button>
          </div>
        </div>
        <div class="d-flex justify-content-between align-items-center mb-3">
          <div class="form-check">
            <input class="form-check-input" type="checkbox" id="rememberMe">
            <label class="form-check-label" for="rememberMe">Remember me</label>
          </div>
          <a href="#" class="text-decoration-none small">Forgot password?</a>
        </div>
        <button type="submit" class="btn btn-primary w-100 mb-3">Sign In</button>
        <div class="text-center text-muted mb-3"><small>or continue with</small></div>
        <div class="d-grid gap-2">
          <button type="button" class="btn btn-outline-secondary"><i class="bi bi-google me-2"></i>Google</button>
          <button type="button" class="btn btn-outline-secondary"><i class="bi bi-github me-2"></i>GitHub</button>
        </div>
      </form>
      <p class="text-center mt-4 mb-0">Don't have an account? <a href="#" class="text-decoration-none">Sign up</a></p>
    </div>
  </div>
</div>
```

### Registration Form with Validation

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-light py-5">
  <div class="card border-0 shadow-sm" style="max-width: 480px; width: 100%;">
    <div class="card-body p-4 p-md-5">
      <div class="text-center mb-4">
        <h3 class="fw-bold">Create Account</h3>
        <p class="text-muted">Get started with your free account</p>
      </div>
      <form class="needs-validation" novalidate>
        <div class="row g-3">
          <div class="col-sm-6">
            <label for="firstName" class="form-label">First name</label>
            <input type="text" class="form-control" id="firstName" required>
            <div class="invalid-feedback">First name is required.</div>
          </div>
          <div class="col-sm-6">
            <label for="lastName" class="form-label">Last name</label>
            <input type="text" class="form-control" id="lastName" required>
            <div class="invalid-feedback">Last name is required.</div>
          </div>
          <div class="col-12">
            <label for="regEmail" class="form-label">Email</label>
            <input type="email" class="form-control" id="regEmail" required>
            <div class="invalid-feedback">Please enter a valid email.</div>
          </div>
          <div class="col-12">
            <label for="regPassword" class="form-label">Password</label>
            <input type="password" class="form-control" id="regPassword" minlength="8" required>
            <div class="invalid-feedback">Password must be at least 8 characters.</div>
          </div>
          <div class="col-12">
            <label for="confirmPassword" class="form-label">Confirm password</label>
            <input type="password" class="form-control" id="confirmPassword" required>
            <div class="invalid-feedback">Passwords must match.</div>
          </div>
          <div class="col-12">
            <div class="form-check">
              <input class="form-check-input" type="checkbox" id="terms" required>
              <label class="form-check-label" for="terms">I agree to the <a href="#">Terms of Service</a></label>
              <div class="invalid-feedback">You must agree before submitting.</div>
            </div>
          </div>
        </div>
        <button type="submit" class="btn btn-primary w-100 mt-4">Create Account</button>
      </form>
      <p class="text-center mt-4 mb-0">Already have an account? <a href="#" class="text-decoration-none">Sign in</a></p>
    </div>
  </div>
</div>
```

### Password Reset Page

```html
<div class="min-vh-100 d-flex align-items-center justify-content-center bg-light py-5">
  <div class="card border-0 shadow-sm" style="max-width: 420px; width: 100%;">
    <div class="card-body p-4 p-md-5">
      <div class="text-center mb-4">
        <div class="bg-primary bg-opacity-10 rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style="width:64px;height:64px">
          <i class="bi bi-key fs-3 text-primary"></i>
        </div>
        <h3 class="fw-bold">Reset Password</h3>
        <p class="text-muted">Enter your email and we'll send a reset link</p>
      </div>
      <form>
        <div class="mb-3">
          <label for="resetEmail" class="form-label">Email address</label>
          <input type="email" class="form-control" id="resetEmail" placeholder="name@example.com" required>
        </div>
        <button type="submit" class="btn btn-primary w-100">Send Reset Link</button>
      </form>
      <p class="text-center mt-4 mb-0"><a href="#" class="text-decoration-none"><i class="bi bi-arrow-left me-1"></i>Back to login</a></p>
    </div>
  </div>
</div>
```

### JavaScript for Form Validation and Password Toggle

```javascript
(() => {
  'use strict';
  const forms = document.querySelectorAll('.needs-validation');
  Array.from(forms).forEach(form => {
    form.addEventListener('submit', event => {
      if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
      }
      form.classList.add('was-validated');
    }, false);
  });

  const toggleBtn = document.getElementById('togglePassword');
  const passwordInput = document.getElementById('loginPassword');
  if (toggleBtn && passwordInput) {
    toggleBtn.addEventListener('click', () => {
      const type = passwordInput.type === 'password' ? 'text' : 'password';
      passwordInput.type = type;
      toggleBtn.querySelector('i').classList.toggle('bi-eye');
      toggleBtn.querySelector('i').classList.toggle('bi-eye-slash');
    });
  }
})();
```

## Advanced Variations

- **Two-Factor Authentication:** Add a second card step with six single-character input fields and auto-advance on input.
- **Magic Link Login:** Replace the password field with a single email field and a "Send Magic Link" button.
- **OAuth Scopes Display:** Show a list of permissions the third-party app requests below social buttons.
- **Captcha Integration:** Insert a `g-recaptcha` div or hCaptcha widget before the submit button.
- **Animated Transitions:** Use Bootstrap's `fade` and `show` classes to transition between login and register cards.

## Best Practices

1. Use `min-vh-100 d-flex align-items-center justify-content-center` for perfect vertical centering.
2. Set `max-width` on the card and `width: 100%` for responsive sizing.
3. Use `input-group` with icon spans for visual field hints.
4. Apply `needs-validation` with `novalidate` for Bootstrap's built-in validation UX.
5. Include `invalid-feedback` divs inside each form group for contextual error messages.
6. Use `form-check` for remember-me and terms checkboxes.
7. Provide a password visibility toggle using `type` switching on the password field.
8. Keep social login buttons consistent with `btn-outline-secondary`.
9. Use `p-4 p-md-5` on card bodies for responsive padding.
10. Place the logo or brand mark above the form heading for brand recognition.
11. Add a back-to-login link on secondary pages like password reset.
12. Use `btn w-100` on all primary form actions for consistent tap targets.

## Common Pitfalls

1. **Missing `novalidate`:** Browser default validation conflicts with Bootstrap's custom validation.
2. **No `was-validated` class:** Without adding this on submit, `invalid-feedback` never displays.
3. **Hardcoded card width:** Without `max-width` and `width:100%`, the card breaks on narrow screens.
4. **Forgotten `min-vh-100`:** The card floats to the top on short forms without full-viewport height.
5. **Overlooking `autocomplete`:** Add `autocomplete="email"` and `autocomplete="current-password"` for browser autofill.
6. **Missing `for`/`id` association:** Labels must reference their input's `id` for click-to-focus behavior.
7. **Social button inconsistency:** Mixing `btn-danger` (Google) and `btn-dark` (GitHub) breaks visual cohesion.

## Accessibility Considerations

- Associate every `<label>` with its `<input>` via matching `for` and `id` attributes.
- Add `aria-describedby` on inputs to reference helper text or error messages.
- Use `role="alert"` on validation error containers for screen reader announcements.
- Ensure the password toggle button has `aria-label="Toggle password visibility"`.
- Provide `autocomplete` attributes (`email`, `new-password`, `current-password`) for assistive tech.

## Responsive Behavior

| Breakpoint | Card Width | Padding | Form Columns |
|------------|-----------|---------|--------------|
| `<576px` | Full | `p-4` | Stacked |
| `≥576px` | `max-width:420px` | `p-md-5` | Stacked |
| `≥768px` | Centered | `p-md-5` | 2-col (name fields) |

The registration form uses `col-sm-6` on first/last name fields to display side-by-side from the `sm` breakpoint upward.
