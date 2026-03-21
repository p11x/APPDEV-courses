# Form Accessibility (A11y)

## Overview

Web accessibility ensures that forms are usable by everyone, including people with disabilities who rely on assistive technologies like screen readers. This guide covers essential accessibility patterns: proper labeling with htmlFor, ARIA attributes for error messages, focus management, keyboard navigation, and testing your forms with screen readers. Accessible forms aren't just good ethics—they're often required by law and improve SEO.

## Prerequisites

- Basic understanding of HTML form elements
- Familiarity with React Hook Form
- Understanding of JavaScript event handling
- Basic knowledge of CSS for styling

## Core Concepts

### Proper Labeling with htmlFor

Every form input needs an associated label. This is crucial for screen readers and also improves the click target size for all users.

```tsx
// File: src/components/AccessibleLabels.tsx

import { useForm } from 'react-hook-form';

interface FormData {
  email: string;
  password: string;
}

function AccessibleLabels() {
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>();

  return (
    <form onSubmit={handleSubmit(console.log)}>
      <h2>Accessible Login Form</h2>

      {/* 
        ✅ CORRECT: Label is properly associated with input
        The htmlFor attribute matches the input's id attribute
        Clicking the label focuses the input
      */}
      <div className="form-field">
        <label htmlFor="email">Email Address</label>
        <input
          id="email"
          type="email"
          {...register("email", { 
            required: "Email is required",
            pattern: {
              value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
              message: "Invalid email address"
            }
          })}
          aria-invalid={errors.email ? "true" : "false"}
          aria-describedby={errors.email ? "email-error" : undefined}
        />
        {errors.email && (
          <span id="email-error" className="error-message" role="alert">
            {errors.email.message}
          </span>
        )}
      </div>

      {/* 
        ✅ CORRECT: Label wraps the input (implicit labeling)
        This also provides proper association without htmlFor
      */}
      <div className="form-field">
        <label>
          Password
          <input
            id="password"
            type="password"
            {...register("password", { 
              required: "Password is required",
              minLength: {
                value: 8,
                message: "Password must be at least 8 characters"
              }
            })}
            aria-invalid={errors.password ? "true" : "false"}
          />
        </label>
        {errors.password && (
          <span id="password-error" className="error-message" role="alert">
            {errors.password.message}
          </span>
        )}
      </div>

      <button type="submit">Login</button>
    </form>
  );
}

export default AccessibleLabels;
```

### ARIA Attributes for Errors and Descriptions

When validation fails, screen readers need to know about the error. ARIA attributes provide this information.

```tsx
// File: src/components/ARIAErrorHandling.tsx

import { useForm } from 'react-hook-form';

interface FormData {
  username: string;
  age: number;
  bio: string;
}

function ARIAErrorHandling() {
  const { 
    register, 
    handleSubmit, 
    formState: { errors } 
  } = useForm<FormData>();

  const onSubmit = (data: FormData) => {
    console.log(data);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} noValidate>
      <h2>Registration Form</h2>

      {/* Username with aria-describedby for error */}
      <div className="form-field">
        <label htmlFor="username">Username</label>
        <input
          id="username"
          type="text"
          {...register("username", {
            required: "Username is required",
            minLength: { value: 3, message: "Minimum 3 characters" },
            maxLength: { value: 20, message: "Maximum 20 characters" }
          })}
          // aria-invalid tells screen readers this field has an error
          aria-invalid={errors.username ? "true" : "false"}
          // aria-describedby links to the error message element
          aria-describedby={errors.username ? "username-error" : undefined}
        />
        {/* 
          role="alert" makes the error message live region
          Screen readers will announce errors immediately when they appear
        */}
        {errors.username && (
          <span 
            id="username-error" 
            className="error" 
            role="alert"
            aria-live="assertive"
          >
            {errors.username.message}
          </span>
        )}
      </div>

      {/* Age with aria-errormessage (newer approach) */}
      <div className="form-field">
        <label htmlFor="age">Age</label>
        <input
          id="age"
          type="number"
          {...register("age", {
            required: "Age is required",
            min: { value: 18, message: "Must be 18 or older" },
            max: { value: 120, message: "Please enter a valid age" },
            valueAsNumber: true
          })}
          aria-invalid={errors.age ? "true" : "false"}
          // aria-errormessage is the modern way to associate errors
          aria-errormessage={errors.age ? "age-error" : undefined}
        />
        {errors.age && (
          <span 
            id="age-error" 
            className="error" 
            role="alert"
          >
            {errors.age.message}
          </span>
        )}
      </div>

      {/* Bio with help text */}
      <div className="form-field">
        <label htmlFor="bio">Bio</label>
        <textarea
          id="bio"
          rows={4}
          {...register("bio", {
            maxLength: { value: 500, message: "Maximum 500 characters" }
          })}
          aria-invalid={errors.bio ? "true" : "false"}
          // Link to both help text and error message
          aria-describedby="bio-help bio-error"
        />
        {/* Help text - always visible for context */}
        <span id="bio-help" className="help-text">
          Tell us about yourself (optional, max 500 characters)
        </span>
        {/* Error message */}
        {errors.bio && (
          <span id="bio-error" className="error" role="alert">
            {errors.bio.message}
          </span>
        )}
      </div>

      <button type="submit">Submit</button>
    </form>
  );
}

export default ARIAErrorHandling;
```

### Focus Management

Proper focus management ensures users can navigate through forms efficiently. This includes focusing the first invalid field on submit and managing focus when errors appear.

```tsx
// File: src/components/FocusManagement.tsx

import { useEffect, useRef } from 'react';
import { useForm } from 'react-hook-form';

interface FormData {
  firstName: string;
  lastName: string;
  email: string;
}

function FocusManagement() {
  const { 
    register, 
    handleSubmit, 
    formState: { errors },
    setFocus
  } = useForm<FormData>();

  // Focus first invalid field when form submission fails
  useEffect(() => {
    // This would be called after failed submission
    // You could add a useEffect that watches errors
  }, [errors]);

  const onSubmit = (data: FormData) => {
    console.log(data);
    alert("Form submitted!");
  };

  // Programmatically focus a field
  const handleFocusFirstName = () => {
    setFocus("firstName");
  };

  return (
    <div>
      <h2>Focus Management Demo</h2>
      
      <button 
        type="button" 
        onClick={handleFocusFirstName}
      >
        Focus First Name
      </button>

      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="form-field">
          <label htmlFor="firstName">First Name</label>
          <input
            id="firstName"
            {...register("firstName", { required: "Required" })}
            aria-invalid={errors.firstName ? "true" : "false"}
          />
          {errors.firstName && (
            <span role="alert">{errors.firstName.message}</span>
          )}
        </div>

        <div className="form-field">
          <label htmlFor="lastName">Last Name</label>
          <input
            id="lastName"
            {...register("lastName", { required: "Required" })}
            aria-invalid={errors.lastName ? "true" : "false"}
          />
          {errors.lastName && (
            <span role="alert">{errors.lastName.message}</span>
          )}
        </div>

        <div className="form-field">
          <label htmlFor="email">Email</label>
          <input
            id="email"
            type="email"
            {...register("email", { 
              required: "Required",
              pattern: {
                value: /@/,
                message: "Invalid email"
              }
            })}
            aria-invalid={errors.email ? "true" : "false"}
          />
          {errors.email && (
            <span role="alert">{errors.email.message}</span>
          )}
        </div>

        <button type="submit">Submit</button>
      </form>
    </div>
  );
}

export default FocusManagement;
```

### Keyboard Navigation and Tab Order

Ensure users can navigate through your form using only the keyboard. The tab order should follow the visual order of fields.

```tsx
// File: src/components/KeyboardNavigation.tsx

import { useForm } from 'react-hook-form';

interface ContactFormData {
  name: string;
  email: string;
  subject: string;
  message: string;
  newsletter: boolean;
}

function KeyboardNavigation() {
  const { 
    register, 
    handleSubmit, 
    formState: { errors } 
  } = useForm<ContactFormData>();

  return (
    <form onSubmit={handleSubmit(console.log)}>
      <h2>Contact Form</h2>
      <p>Use Tab to navigate between fields, Enter to submit</p>

      {/* Name field */}
      <div className="form-field">
        <label htmlFor="name">Name *</label>
        <input
          id="name"
          {...register("name", { required: "Name is required" })}
          aria-invalid={errors.name ? "true" : "false"}
        />
        {errors.name && <span role="alert">{errors.name.message}</span>}
      </div>

      {/* Email field */}
      <div className="form-field">
        <label htmlFor="email">Email *</label>
        <input
          id="email"
          type="email"
          {...register("email", { 
            required: "Email is required",
            pattern: { value: /@/, message: "Invalid email" }
          })}
          aria-invalid={errors.email ? "true" : "false"}
        />
        {errors.email && <span role="alert">{errors.email.message}</span>}
      </div>

      {/* Subject dropdown - use proper keyboard navigation */}
      <div className="form-field">
        <label htmlFor="subject">Subject</label>
        <select id="subject" {...register("subject")}>
          <option value="general">General Inquiry</option>
          <option value="support">Technical Support</option>
          <option value="sales">Sales Question</option>
          <option value="other">Other</option>
        </select>
      </div>

      {/* Message textarea */}
      <div className="form-field">
        <label htmlFor="message">Message *</label>
        <textarea
          id="message"
          rows={5}
          {...register("message", { required: "Message is required" })}
          aria-invalid={errors.message ? "true" : "false"}
        />
        {errors.message && <span role="alert">{errors.message.message}</span>}
      </div>

      {/* Checkbox - ensure whole area is clickable */}
      <div className="form-field checkbox">
        <label htmlFor="newsletter">
          {/* Putting input inside label makes the whole label clickable */}
          <input
            id="newsletter"
            type="checkbox"
            {...register("newsletter")}
          />
          Subscribe to newsletter
        </label>
      </div>

      {/* Submit button should be last in tab order */}
      <button type="submit">Send Message</button>

      {/* Keyboard shortcuts hint */}
      <div className="keyboard-hints">
        <small>
          Keyboard shortcuts: Tab to navigate, Shift+Tab to go back, Enter to submit
        </small>
      </div>
    </form>
  );
}

export default KeyboardNavigation;
```

### Accessible Form Validation

Combine all accessibility patterns into a complete accessible form validation system.

```tsx
// File: src/components/AccessibleForm.tsx

import { useState, useRef, useEffect } from 'react';
import { useForm } from 'react-hook-form';

interface RegisterData {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
  agreeTerms: boolean;
}

function AccessibleForm() {
  const { 
    register, 
    handleSubmit, 
    formState: { errors, isSubmitting },
    setFocus,
    reset
  } = useForm<RegisterData>({
    mode: "onBlur" // Validate on blur for better accessibility
  });

  const [submitSuccess, setSubmitSuccess] = useState(false);
  const formRef = useRef<HTMLFormElement>(null);

  // Focus first error field on submission failure
  const onInvalid = () => {
    const firstErrorField = document.querySelector('[aria-invalid="true"]') as HTMLElement;
    firstErrorField?.focus();
  };

  const onSubmit = async (data: RegisterData) => {
    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      console.log("Form data:", data);
      setSubmitSuccess(true);
      reset();
    } catch (error) {
      console.error("Submission error:", error);
    }
  };

  // Show success message and focus it for screen readers
  useEffect(() => {
    if (submitSuccess) {
      const successMessage = document.getElementById('success-message');
      successMessage?.focus();
    }
  }, [submitSuccess]);

  return (
    <div className="accessible-form-container">
      <h1>Create Account</h1>

      {/* Success message - announced to screen readers */}
      {submitSuccess && (
        <div 
          id="success-message"
          className="success-banner"
          role="status"
          aria-live="polite"
          tabIndex={-1}
        >
          Account created successfully! You can now sign in.
        </div>
      )}

      <form 
        ref={formRef}
        onSubmit={handleSubmit(onSubmit, onInvalid)}
        noValidate
        aria-label="Registration form"
      >
        {/* Username */}
        <div className={`form-field ${errors.username ? 'has-error' : ''}`}>
          <label htmlFor="username">
            Username <span aria-hidden="true">*</span>
          </label>
          <input
            id="username"
            type="text"
            autoComplete="username"
            {...register("username", {
              required: "Username is required",
              minLength: { 
                value: 3, 
                message: "Username must be at least 3 characters" 
              },
              maxLength: { 
                value: 20, 
                message: "Username cannot exceed 20 characters" 
              },
              pattern: {
                value: /^[a-zA-Z0-9_]+$/,
                message: "Only letters, numbers, and underscores allowed"
              }
            })}
            aria-invalid={errors.username ? "true" : "false"}
            aria-describedby={errors.username ? "username-error" : "username-hint"}
            aria-required="true"
          />
          <span id="username-hint" className="field-hint">
            3-20 characters, letters, numbers, underscores only
          </span>
          {errors.username && (
            <span id="username-error" className="error-message" role="alert">
              {errors.username.message}
            </span>
          )}
        </div>

        {/* Email */}
        <div className={`form-field ${errors.email ? 'has-error' : ''}`}>
          <label htmlFor="email">
            Email Address <span aria-hidden="true">*</span>
          </label>
          <input
            id="email"
            type="email"
            autoComplete="email"
            {...register("email", {
              required: "Email is required",
              pattern: {
                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                message: "Please enter a valid email address"
              }
            })}
            aria-invalid={errors.email ? "true" : "false"}
            aria-describedby={errors.email ? "email-error" : undefined}
            aria-required="true"
          />
          {errors.email && (
            <span id="email-error" className="error-message" role="alert">
              {errors.email.message}
            </span>
          )}
        </div>

        {/* Password */}
        <div className={`form-field ${errors.password ? 'has-error' : ''}`}>
          <label htmlFor="password">
            Password <span aria-hidden="true">*</span>
          </label>
          <input
            id="password"
            type="password"
            autoComplete="new-password"
            {...register("password", {
              required: "Password is required",
              minLength: { 
                value: 8, 
                message: "Password must be at least 8 characters" 
              },
              validate: {
                hasUpperCase: (value) =>
                  /[A-Z]/.test(value) || "Password must contain an uppercase letter",
                hasLowerCase: (value) =>
                  /[a-z]/.test(value) || "Password must contain a lowercase letter",
                hasNumber: (value) =>
                  /[0-9]/.test(value) || "Password must contain a number"
              }
            })}
            aria-invalid={errors.password ? "true" : "false"}
            aria-describedby="password-requirements password-error"
          />
          <ul id="password-requirements" className="requirements-list">
            <li className={errors.password?.message?.includes('uppercase') ? 'error' : ''}>
              At least one uppercase letter
            </li>
            <li className={errors.password?.message?.includes('lowercase') ? 'error' : ''}>
              At least one lowercase letter
            </li>
            <li className={errors.password?.message?.includes('number') ? 'error' : ''}>
              At least one number
            </li>
            <li>At least 8 characters</li>
          </ul>
          {errors.password && (
            <span id="password-error" className="error-message" role="alert">
              {errors.password.message}
            </span>
          )}
        </div>

        {/* Confirm Password */}
        <div className={`form-field ${errors.confirmPassword ? 'has-error' : ''}`}>
          <label htmlFor="confirmPassword">
            Confirm Password <span aria-hidden="true">*</span>
          </label>
          <input
            id="confirmPassword"
            type="password"
            autoComplete="new-password"
            {...register("confirmPassword", {
              required: "Please confirm your password",
              validate: (value, formValues) =>
                value === formValues.password || "Passwords do not match"
            })}
            aria-invalid={errors.confirmPassword ? "true" : "false"}
            aria-describedby={errors.confirmPassword ? "confirmPassword-error" : undefined}
            aria-required="true"
          />
          {errors.confirmPassword && (
            <span id="confirmPassword-error" className="error-message" role="alert">
              {errors.confirmPassword.message}
            </span>
          )}
        </div>

        {/* Terms Checkbox */}
        <div className={`form-field checkbox ${errors.agreeTerms ? 'has-error' : ''}`}>
          <label htmlFor="agreeTerms">
            <input
              id="agreeTerms"
              type="checkbox"
              {...register("agreeTerms", {
                required: "You must agree to the terms"
              })}
              aria-invalid={errors.agreeTerms ? "true" : "false"}
            />
            I agree to the <a href="/terms">Terms of Service</a> and <a href="/privacy">Privacy Policy</a>
          </label>
          {errors.agreeTerms && (
            <span id="agreeTerms-error" className="error-message" role="alert">
              {errors.agreeTerms.message}
            </span>
          )}
        </div>

        {/* Submit Button */}
        <button 
          type="submit" 
          disabled={isSubmitting}
          className="submit-button"
        >
          {isSubmitting ? "Creating Account..." : "Create Account"}
        </button>

        {/* Required fields indicator */}
        <p className="required-indicator">
          <span aria-hidden="true">*</span>
          <span className="visually-hidden">Required fields</span>
        </p>
      </form>
    </div>
  );
}

// CSS for visually-hidden class (for screen reader only content)
const styles = `
  .visually-hidden {
    position: absolute;
    width: 1px;
    height: 1px;
    padding: 0;
    margin: -1px;
    overflow: hidden;
    clip: rect(0, 0, 0, 0);
    white-space: nowrap;
    border: 0;
  }
`;

export default AccessibleForm;
```

## Common Mistakes

### Mistake 1: Missing Labels

Never leave inputs without labels. Screen reader users won't know what each field is for.

```tsx
// ❌ WRONG - No label at all
<input type="text" placeholder="Enter your name" />

// ✅ CORRECT - Proper label
<label htmlFor="name">Name</label>
<input id="name" type="text" placeholder="Enter your name" />
```

### Mistake 2: Incorrect ARIA Usage

Don't overcomplicate ARIA. If HTML semantics work, use them instead of ARIA.

```tsx
// ❌ WRONG - Using role="button" on a real button
<div role="button" onClick={handleClick}>Click me</div>

// ✅ CORRECT - Use actual button element
<button onClick={handleClick}>Click me</button>
```

### Mistake 3: Not Announcing Errors

Error messages must be announced to screen readers.

```tsx
// ❌ WRONG - Error not announced
{error && <span className="error">{error.message}</span>}

// ✅ CORRECT - Error is announced via role="alert"
{error && (
  <span className="error" role="alert" aria-live="assertive">
    {error.message}
  </span>
)}
```

## Real-World Example

Here's a complete accessible multi-step form wizard demonstrating all accessibility best practices:

```tsx
// File: src/components/AccessibleWizard.tsx

import { useState } from 'react';
import { useForm } from 'react-hook-form';

interface WizardData {
  // Step 1: Personal Info
  firstName: string;
  lastName: string;
  // Step 2: Contact
  email: string;
  phone: string;
  // Step 3: Preferences
  newsletter: boolean;
  notifications: 'all' | 'important' | 'none';
}

function AccessibleWizard() {
  const [currentStep, setCurrentStep] = useState(1);
  const totalSteps = 3;

  const { 
    register, 
    handleSubmit, 
    formState: { errors, isValid },
    trigger,
    getValues
  } = useForm<WizardData>({
    mode: "onBlur"
  });

  const [isSubmitted, setIsSubmitted] = useState(false);

  const nextStep = async () => {
    let fieldsToValidate: (keyof WizardData)[] = [];
    
    if (currentStep === 1) {
      fieldsToValidate = ['firstName', 'lastName'];
    } else if (currentStep === 2) {
      fieldsToValidate = ['email', 'phone'];
    }

    const isStepValid = await trigger(fieldsToValidate);
    if (isStepValid) {
      setCurrentStep(prev => Math.min(prev + 1, totalSteps));
    }
  };

  const prevStep = () => {
    setCurrentStep(prev => Math.max(prev - 1, 1));
  };

  const onSubmit = (data: WizardData) => {
    console.log("Wizard data:", data);
    setIsSubmitted(true);
  };

  if (isSubmitted) {
    return (
      <div role="status" aria-live="polite">
        <h2>✅ Registration Complete!</h2>
        <p>Thank you for registering. Check your email for confirmation.</p>
      </div>
    );
  }

  return (
    <div className="wizard-container">
      {/* Progress Indicator */}
      <div 
        className="progress-indicator" 
        role="progressbar" 
        aria-valuenow={currentStep}
        aria-valuemin={1}
        aria-valuemax={totalSteps}
        aria-label={`Step ${currentStep} of ${totalSteps}`}
      >
        {Array.from({ length: totalSteps }, (_, i) => (
          <div 
            key={i} 
            className={`step ${i + 1 <= currentStep ? 'active' : ''}`}
            aria-current={i + 1 === currentStep ? 'step' : undefined}
          >
            <span className="step-number">{i + 1}</span>
            <span className="step-label">
              {i === 0 ? 'Personal' : i === 1 ? 'Contact' : 'Preferences'}
            </span>
          </div>
        ))}
      </div>

      <form onSubmit={handleSubmit(onSubmit)}>
        {/* Step 1: Personal Information */}
        {currentStep === 1 && (
          <fieldset>
            <legend>Personal Information</legend>

            <div className="form-field">
              <label htmlFor="firstName">
                First Name <span aria-hidden="true">*</span>
              </label>
              <input
                id="firstName"
                {...register("firstName", { 
                  required: "First name is required",
                  minLength: { value: 2, message: "Minimum 2 characters" }
                })}
                aria-invalid={errors.firstName ? "true" : "false"}
                aria-describedby={errors.firstName ? "firstName-error" : undefined}
                autoComplete="given-name"
              />
              {errors.firstName && (
                <span id="firstName-error" className="error" role="alert">
                  {errors.firstName.message}
                </span>
              )}
            </div>

            <div className="form-field">
              <label htmlFor="lastName">
                Last Name <span aria-hidden="true">*</span>
              </label>
              <input
                id="lastName"
                {...register("lastName", { 
                  required: "Last name is required",
                  minLength: { value: 2, message: "Minimum 2 characters" }
                })}
                aria-invalid={errors.lastName ? "true" : "false"}
                aria-describedby={errors.lastName ? "lastName-error" : undefined}
                autoComplete="family-name"
              />
              {errors.lastName && (
                <span id="lastName-error" className="error" role="alert">
                  {errors.lastName.message}
                </span>
              )}
            </div>
          </fieldset>
        )}

        {/* Step 2: Contact Information */}
        {currentStep === 2 && (
          <fieldset>
            <legend>Contact Information</legend>

            <div className="form-field">
              <label htmlFor="email">
                Email Address <span aria-hidden="true">*</span>
              </label>
              <input
                id="email"
                type="email"
                {...register("email", { 
                  required: "Email is required",
                  pattern: { 
                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                    message: "Invalid email address"
                  }
                })}
                aria-invalid={errors.email ? "true" : "false"}
                aria-describedby={errors.email ? "email-error" : undefined}
                autoComplete="email"
              />
              {errors.email && (
                <span id="email-error" className="error" role="alert">
                  {errors.email.message}
                </span>
              )}
            </div>

            <div className="form-field">
              <label htmlFor="phone">Phone Number</label>
              <input
                id="phone"
                type="tel"
                {...register("phone")}
                autoComplete="tel"
              />
            </div>
          </fieldset>
        )}

        {/* Step 3: Preferences */}
        {currentStep === 3 && (
          <fieldset>
            <legend>Preferences</legend>

            <div className="form-field checkbox">
              <label htmlFor="newsletter">
                <input
                  id="newsletter"
                  type="checkbox"
                  {...register("newsletter")}
                />
                Subscribe to newsletter
              </label>
            </div>

            <div className="form-field">
              <label htmlFor="notifications">Notification Preferences</label>
              <select id="notifications" {...register("notifications")}>
                <option value="all">All notifications</option>
                <option value="important">Important only</option>
                <option value="none">None</option>
              </select>
            </div>
          </fieldset>
        )}

        {/* Navigation Buttons */}
        <div className="wizard-buttons">
          {currentStep > 1 && (
            <button 
              type="button" 
              onClick={prevStep}
              className="secondary-button"
            >
              Previous
            </button>
          )}

          {currentStep < totalSteps ? (
            <button 
              type="button" 
              onClick={nextStep}
              className="primary-button"
            >
              Next
            </button>
          ) : (
            <button type="submit" className="primary-button">
              Complete Registration
            </button>
          )}
        </div>
      </form>
    </div>
  );
}

export default AccessibleWizard;
```

## Key Takeaways

- Always associate labels with inputs using htmlFor or implicit labeling
- Use aria-invalid="true" on fields with errors
- Use role="alert" and aria-live="assertive" for error messages
- Link help text and errors to inputs using aria-describedby
- Ensure keyboard navigation works: Tab between fields, Enter to submit
- Focus the first invalid field when form submission fails
- Use fieldset and legend for grouped form sections
- Test with screen readers (NVDA, VoiceOver) to verify accessibility
- Don't rely solely on color to communicate errors

## What's Next

You've completed the Forms section. The next major area is [Styling in React](/08-styling/01-css-modules/01-css-modules-setup.md) where you'll learn different approaches to styling React components, including CSS Modules, Tailwind CSS, and styled-components.