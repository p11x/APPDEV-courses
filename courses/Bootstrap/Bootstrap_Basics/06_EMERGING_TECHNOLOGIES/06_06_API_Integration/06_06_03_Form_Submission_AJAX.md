---
title: "Form Submission with AJAX"
description: "Implement Fetch-based form submission with loading states and success/error feedback using Bootstrap 5"
difficulty: 2
tags: [forms, ajax, fetch, validation, feedback]
prerequisites:
  - "Bootstrap 5 form components"
  - "JavaScript Fetch API"
  - "HTML5 form validation"
---

## Overview

AJAX form submission replaces traditional page-reload form handling with asynchronous Fetch API calls. Bootstrap 5 provides form validation styles, loading button states, and alert components for success/error feedback. This pattern improves user experience by providing instant feedback without page navigation, maintaining form state on errors, and enabling progressive enhancement.

## Basic Implementation

### Basic AJAX Form

```html
<form id="contactForm" novalidate>
  <div class="mb-3">
    <label for="name" class="form-label">Name</label>
    <input type="text" class="form-control" id="name" name="name" required>
    <div class="invalid-feedback">Please enter your name.</div>
  </div>
  <div class="mb-3">
    <label for="email" class="form-label">Email</label>
    <input type="email" class="form-control" id="email" name="email" required>
    <div class="invalid-feedback">Please enter a valid email.</div>
  </div>
  <div class="mb-3">
    <label for="message" class="form-label">Message</label>
    <textarea class="form-control" id="message" name="message" rows="3" required></textarea>
    <div class="invalid-feedback">Please enter a message.</div>
  </div>
  <div id="formAlert"></div>
  <button type="submit" class="btn btn-primary" id="submitBtn">
    <span class="btn-text">Send Message</span>
    <span class="spinner-border spinner-border-sm d-none ms-1" role="status" aria-hidden="true"></span>
  </button>
</form>

<script>
  document.getElementById('contactForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const form = e.target;
    const btn = document.getElementById('submitBtn');
    const alert = document.getElementById('formAlert');

    if (!form.checkValidity()) {
      form.classList.add('was-validated');
      return;
    }

    btn.disabled = true;
    btn.querySelector('.spinner-border').classList.remove('d-none');
    btn.querySelector('.btn-text').textContent = 'Sending...';

    try {
      const response = await fetch('https://jsonplaceholder.typicode.com/posts', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          name: form.name.value,
          email: form.email.value,
          message: form.message.value
        })
      });

      if (!response.ok) throw new Error('Submission failed');

      alert.innerHTML = '<div class="alert alert-success mt-3"><i class="bi bi-check-circle me-2"></i>Message sent successfully!</div>';
      form.reset();
      form.classList.remove('was-validated');
    } catch (error) {
      alert.innerHTML = `<div class="alert alert-danger mt-3"><i class="bi bi-x-circle me-2"></i>${error.message}</div>`;
    } finally {
      btn.disabled = false;
      btn.querySelector('.spinner-border').classList.add('d-none');
      btn.querySelector('.btn-text').textContent = 'Send Message';
    }
  });
</script>
```

## Advanced Variations

### Multi-Step Form with AJAX

```html
<form id="multiStepForm" novalidate>
  <div class="step" id="step1">
    <h5>Step 1: Personal Info</h5>
    <div class="mb-3">
      <label class="form-label">Full Name</label>
      <input type="text" class="form-control" name="fullName" required>
    </div>
    <div class="mb-3">
      <label class="form-label">Email</label>
      <input type="email" class="form-control" name="email" required>
    </div>
    <button type="button" class="btn btn-primary" onclick="nextStep(1)">Next</button>
  </div>

  <div class="step d-none" id="step2">
    <h5>Step 2: Details</h5>
    <div class="mb-3">
      <label class="form-label">Subject</label>
      <input type="text" class="form-control" name="subject" required>
    </div>
    <div class="mb-3">
      <label class="form-label">Message</label>
      <textarea class="form-control" name="message" rows="3" required></textarea>
    </div>
    <button type="button" class="btn btn-secondary me-2" onclick="prevStep(2)">Back</button>
    <button type="submit" class="btn btn-success" id="finalSubmit">
      <span class="btn-text">Submit</span>
      <span class="spinner-border spinner-border-sm d-none ms-1"></span>
    </button>
  </div>

  <div id="multiFormAlert"></div>
</form>

<script>
  function nextStep(current) {
    const step = document.getElementById(`step${current}`);
    if (!step.querySelector('form')?.checkValidity()) {
      document.getElementById('multiStepForm').classList.add('was-validated');
      // Check individual inputs in current step
      step.querySelectorAll('[required]').forEach(input => {
        if (!input.checkValidity()) input.classList.add('is-invalid');
        else input.classList.remove('is-invalid');
      });
      return;
    }
    step.classList.add('d-none');
    document.getElementById(`step${current + 1}`).classList.remove('d-none');
  }

  function prevStep(current) {
    document.getElementById(`step${current}`).classList.add('d-none');
    document.getElementById(`step${current - 1}`).classList.remove('d-none');
  }

  document.getElementById('multiStepForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const btn = document.getElementById('finalSubmit');
    const alert = document.getElementById('multiFormAlert');

    btn.disabled = true;
    btn.querySelector('.spinner-border').classList.remove('d-none');
    btn.querySelector('.btn-text').textContent = 'Submitting...';

    try {
      await fetch('https://jsonplaceholder.typicode.com/posts', {
        method: 'POST',
        body: JSON.stringify(Object.fromEntries(new FormData(e.target))),
        headers: { 'Content-Type': 'application/json' }
      });
      alert.innerHTML = '<div class="alert alert-success mt-3">Form submitted successfully!</div>';
    } catch {
      alert.innerHTML = '<div class="alert alert-danger mt-3">Submission failed. Please try again.</div>';
    } finally {
      btn.disabled = false;
      btn.querySelector('.spinner-border').classList.add('d-none');
      btn.querySelector('.btn-text').textContent = 'Submit';
    }
  });
</script>
```

### File Upload with Progress

```html
<form id="uploadForm" enctype="multipart/form-data">
  <div class="mb-3">
    <label class="form-label">Select file</label>
    <input type="file" class="form-control" name="file" required>
  </div>
  <div class="progress d-none mb-3" style="height: 20px;" id="uploadProgress">
    <div class="progress-bar progress-bar-striped progress-bar-animated" style="width: 0%">0%</div>
  </div>
  <button type="submit" class="btn btn-primary">Upload</button>
</form>

<script>
  document.getElementById('uploadForm').addEventListener('submit', (e) => {
    e.preventDefault();
    const formData = new FormData(e.target);
    const xhr = new XMLHttpRequest();
    const progress = document.getElementById('uploadProgress');
    const bar = progress.querySelector('.progress-bar');

    progress.classList.remove('d-none');

    xhr.upload.addEventListener('progress', (event) => {
      if (event.lengthComputable) {
        const pct = Math.round((event.loaded / event.total) * 100);
        bar.style.width = pct + '%';
        bar.textContent = pct + '%';
      }
    });

    xhr.addEventListener('load', () => {
      bar.classList.remove('progress-bar-animated');
      bar.classList.add('bg-success');
      bar.textContent = 'Complete!';
    });

    xhr.open('POST', 'https://jsonplaceholder.typicode.com/posts');
    xhr.send(formData);
  });
</script>
```

## Best Practices

1. **Always use `novalidate`** to let Bootstrap handle validation styling instead of browser defaults.
2. **Use `was-validated` class** on form to trigger Bootstrap validation feedback.
3. **Disable submit button** during submission to prevent duplicate requests.
4. **Show spinner in button** during async submission for clear loading feedback.
5. **Use `finally` block** to always reset button state regardless of success or failure.
6. **Use `FormData`** for file uploads; use `JSON.stringify` for JSON APIs.
7. **Clear validation states** on successful submission by removing `was-validated`.
8. **Display alerts inside the form** for contextual success/error messaging.
9. **Reset form on success** to prepare for the next submission.
10. **Use `Object.fromEntries(formData)`** for simple key-value form serialization.
11. **Validate client-side first** before making API calls to reduce unnecessary requests.
12. **Preserve form values on error** so users don't lose their input.

## Common Pitfalls

1. **Missing `e.preventDefault()`** causes page reload, canceling the AJAX request.
2. **Not checking `response.ok`** - fetch doesn't reject on HTTP 4xx/5xx errors.
3. **Forgetting `novalidate`** results in browser validation interfering with Bootstrap styles.
4. **Not resetting `was-validated`** after successful submission shows stale validation.
5. **Double submission** from clicking submit while request is pending.
6. **Missing `enctype="multipart/form-data"`** prevents file uploads from working.
7. **Not handling network errors** separately from server errors.
8. **Memory leaks** from unreferenced XHR objects in upload progress implementations.

## Accessibility Considerations

- Form validation errors must be associated with inputs via `invalid-feedback` div.
- Loading state should be announced via `aria-live` or `aria-busy` on the form.
- Success/error alerts should use `role="alert"` for immediate announcement.
- Multi-step forms should indicate current step progress for screen readers.
- Disabled buttons should communicate why they are disabled via `aria-disabled`.
- File upload progress should be announced via `aria-live="polite"`.

## Responsive Behavior

- Form inputs should be full-width on mobile using Bootstrap's default form styling.
- Multi-step forms should stack navigation buttons vertically on small screens.
- Progress bars maintain readability at all viewport sizes.
- Alert messages should not overflow on narrow screens - use text wrapping.
- File input elements are responsive by default in Bootstrap 5.
