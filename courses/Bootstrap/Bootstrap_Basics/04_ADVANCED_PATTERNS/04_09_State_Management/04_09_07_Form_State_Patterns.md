---
title: "Form State Patterns"
description: "Multi-step form state, validation state, and dirty tracking in Bootstrap forms"
difficulty: 2
tags: ["state-management", "forms", "validation", "multi-step", "bootstrap"]
prerequisites: ["04_08_02_Form_Error_Display", "04_09_04_Session_State"]
---

## Overview

Complex forms require tracking multiple state dimensions: current step in a multi-step wizard, validation status of each field, whether the form has been modified (dirty state), and which fields have been touched by the user. Bootstrap provides the visual components (steppers, validation classes, progress bars) while JavaScript manages the underlying state logic.

Form state patterns prevent data loss by detecting unsaved changes, guide users through multi-step processes with clear progress indicators, and provide real-time validation feedback that improves form completion rates.

## Basic Implementation

```html
<!-- Multi-step form with Bootstrap -->
<div class="card">
  <div class="card-header">
    <!-- Progress indicator -->
    <div class="d-flex justify-content-between mb-2">
      <span class="badge bg-primary" id="stepBadge">Step 1 of 3</span>
      <span class="text-muted" id="stepTitle">Personal Info</span>
    </div>
    <div class="progress" style="height: 4px;">
      <div class="progress-bar" id="stepProgress" style="width: 33%"></div>
    </div>
  </div>

  <div class="card-body">
    <!-- Step 1 -->
    <div class="form-step" data-step="1">
      <div class="mb-3">
        <label class="form-label">Full Name</label>
        <input type="text" class="form-control" name="fullName" required>
        <div class="invalid-feedback">Name is required.</div>
      </div>
      <div class="mb-3">
        <label class="form-label">Email</label>
        <input type="email" class="form-control" name="email" required>
        <div class="invalid-feedback">Valid email is required.</div>
      </div>
    </div>

    <!-- Step 2 -->
    <div class="form-step d-none" data-step="2">
      <div class="mb-3">
        <label class="form-label">Company</label>
        <input type="text" class="form-control" name="company" required>
      </div>
      <div class="mb-3">
        <label class="form-label">Role</label>
        <select class="form-select" name="role" required>
          <option value="">Select role...</option>
          <option>Developer</option>
          <option>Designer</option>
          <option>Manager</option>
        </select>
      </div>
    </div>

    <!-- Step 3 -->
    <div class="form-step d-none" data-step="3">
      <h6>Review Your Information</h6>
      <div id="reviewContent" class="bg-light p-3 rounded"></div>
    </div>
  </div>

  <div class="card-footer d-flex justify-content-between">
    <button class="btn btn-secondary d-none" id="prevBtn">Previous</button>
    <button class="btn btn-primary ms-auto" id="nextBtn">Next</button>
    <button class="btn btn-success d-none" id="submitBtn">Submit</button>
  </div>
</div>

<!-- Unsaved changes warning -->
<div class="alert alert-warning d-none" id="unsavedWarning">
  <i class="bi bi-exclamation-triangle me-2"></i>
  You have unsaved changes. <a href="#" id="discardLink" class="alert-link">Discard</a>
</div>
```

```js
// Form state manager
class FormState {
  constructor(form) {
    this.form = form;
    this.currentStep = 1;
    this.totalSteps = 3;
    this.dirty = false;
    this.touched = new Set();
    this.errors = new Map();

    this.init();
  }

  init() {
    // Track dirty state
    this.form.addEventListener('input', () => {
      this.dirty = true;
      document.getElementById('unsavedWarning').classList.remove('d-none');
    });

    // Track touched fields
    this.form.querySelectorAll('input, select, textarea').forEach(field => {
      field.addEventListener('blur', () => {
        this.touched.add(field.name);
        this.validateField(field);
      });
    });

    // Warn on navigation away
    window.addEventListener('beforeunload', (e) => {
      if (this.dirty) {
        e.preventDefault();
        e.returnValue = '';
      }
    });
  }

  validateField(field) {
    const valid = field.checkValidity();
    field.classList.toggle('is-invalid', !valid);
    field.classList.toggle('is-valid', valid);

    if (!valid) {
      this.errors.set(field.name, field.validationMessage);
    } else {
      this.errors.delete(field.name);
    }

    return valid;
  }

  validateStep(step) {
    const stepEl = this.form.querySelector(`[data-step="${step}"]`);
    const fields = stepEl.querySelectorAll('input, select, textarea');
    let allValid = true;

    fields.forEach(field => {
      if (!this.validateField(field)) allValid = false;
    });

    return allValid;
  }

  nextStep() {
    if (!this.validateStep(this.currentStep)) return false;
    if (this.currentStep < this.totalSteps) {
      this.currentStep++;
      this.render();
      return true;
    }
    return false;
  }

  prevStep() {
    if (this.currentStep > 1) {
      this.currentStep--;
      this.render();
    }
  }

  render() {
    // Show/hide steps
    this.form.querySelectorAll('.form-step').forEach(step => {
      step.classList.toggle('d-none', parseInt(step.dataset.step) !== this.currentStep);
    });

    // Update progress
    const percent = (this.currentStep / this.totalSteps) * 100;
    document.getElementById('stepProgress').style.width = `${percent}%`;
    document.getElementById('stepBadge').textContent = `Step ${this.currentStep} of ${this.totalSteps}`;

    // Update buttons
    document.getElementById('prevBtn').classList.toggle('d-none', this.currentStep === 1);
    document.getElementById('nextBtn').classList.toggle('d-none', this.currentStep === this.totalSteps);
    document.getElementById('submitBtn').classList.toggle('d-none', this.currentStep !== this.totalSteps);

    // Populate review on last step
    if (this.currentStep === this.totalSteps) {
      this.renderReview();
    }
  }

  renderReview() {
    const data = new FormData(this.form);
    const review = document.getElementById('reviewContent');
    review.innerHTML = Array.from(data.entries())
      .map(([key, val]) => `<p class="mb-1"><strong>${key}:</strong> ${val}</p>`)
      .join('');
  }
}

const form = document.getElementById('multiStepForm');
const formState = new FormState(document.querySelector('.card'));

document.getElementById('nextBtn').addEventListener('click', () => formState.nextStep());
document.getElementById('prevBtn').addEventListener('click', () => formState.prevStep());
```

## Best Practices

1. Track `dirty` state to warn users about unsaved changes before navigation
2. Mark fields as `touched` on `blur`, not on `input`, to avoid premature validation
3. Show validation errors only for touched fields to reduce noise
4. Use a progress bar to indicate multi-step form position
5. Disable the Next button until the current step passes validation
6. Populate a review step with all entered data before final submission
7. Use `beforeunload` event to warn about losing unsaved changes
8. Persist form state in sessionStorage for crash recovery
9. Clear dirty state on successful submission
10. Focus the first invalid field on step transition

## Common Pitfalls

1. **Validating on every keystroke** — Users see errors before they finish typing; validate on `blur` instead
2. **No dirty state tracking** — Users lose all input when accidentally navigating away
3. **Not focusing invalid fields** — Users miss errors that scroll out of view
4. **Missing step validation** — Advancing to the next step without validating leaves incomplete data
5. **Review step not reflecting actual data** — If review shows hardcoded text instead of form values, users submit wrong data
6. **Clearing form on validation error** — Replacing user input with empty fields causes frustration and data loss

## Accessibility Considerations

Multi-step forms need `aria-current="step"` on the active step indicator and `aria-disabled` on disabled Next buttons. Validation errors must be linked to inputs via `aria-describedby`. Announce step changes to screen readers using `aria-live="polite"` on the step title. Ensure the review step is fully readable by assistive technology before submission.

## Responsive Behavior

Multi-step form progress indicators should adapt to narrow viewports — use a simple badge (`Step 2 of 3`) instead of a full step-by-step connector on mobile. Form fields should stack vertically on small screens. The review step should remain readable at all widths without horizontal scrolling.
