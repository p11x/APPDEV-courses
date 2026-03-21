# React Hook Form Setup Guide

## Overview

React Hook Form is a performant, flexible form library that bridges the gap between controlled and uncontrolled inputs. It uses hooks to manage form state with minimal re-renders, making it significantly faster than traditional controlled components for complex forms. This guide covers installation, core concepts, and wiring up your first form with proper TypeScript support.

## Prerequisites

- Basic understanding of React hooks (useState, useEffect)
- Familiarity with TypeScript generics (optional but recommended)
- A React project set up with Vite or Create React App
- Node.js 18+ installed

## Core Concepts

### Installing React Hook Form

React Hook Form can be installed via npm or yarn. The library provides a minimal API surface while offering powerful features for form validation and management.

```bash
# File: terminal

# Install react-hook-form
npm install react-hook-form

# If using TypeScript, you'll also want types (usually included)
npm install @types/react --save-dev
```

### Understanding the useForm Hook

The `useForm` hook is the foundation of React Hook Form. It returns an object containing methods and properties for managing your form. Unlike controlled components where every keystroke triggers a re-render, React Hook Form registers inputs directly to the DOM, minimizing unnecessary updates.

```jsx
// File: src/components/LoginForm.tsx

// Import useForm from react-hook-form - this is the main hook we'll use
// It provides all the form management functionality without the boilerplate
import { useForm } from 'react-hook-form';

// Define the shape of our form data using a TypeScript interface
// This gives us type safety throughout the form
interface LoginFormData {
  // Email must be a valid email string
  email: string;
  // Password has a minimum length requirement
  password: string;
  // Optional "remember me" checkbox
  rememberMe: boolean;
}

// Define a LoginForm component that accepts an onSubmit handler as a prop
function LoginForm({ onSubmit }: { onSubmit: (data: LoginFormData) => void }) {
  // destructure useForm to get the methods we need
  // register: connects input elements to the form
  // handleSubmit: wraps our submit handler to prevent default form behavior
  // formState: contains information about the form state (errors, isSubmitting, etc.)
  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<LoginFormData>();

  // handleSubmit takes our data handler and runs validation first
  // If validation fails, errors will be populated and the handler won't be called
  const onFormSubmit = async (data: LoginFormData) => {
    // Simulate an API call to show loading state
    await new Promise(resolve => setTimeout(resolve, 1000));
    // Call the parent component's submit handler with the validated data
    onSubmit(data);
  };

  return (
    <form onSubmit={handleSubmit(onFormSubmit)}>
      {/* Email Input */}
      <div>
        {/* The label's htmlFor matches the input's id for accessibility */}
        <label htmlFor="email">Email</label>
        {/* 
          {...register("email")} spreads the registration props onto the input
          This connects the input to React Hook Form's internal state management
          The "email" string is the key that identifies this field in formData
        */}
        <input
          id="email"
          type="email"
          {...register("email", { 
            // Validation rules - these run on blur and on submit
            required: "Email is required",
            pattern: {
              value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
              message: "Invalid email address"
            }
          })}
        />
        {/* Display error message if validation failed for this field */}
        {errors.email && <span>{errors.email.message}</span>}
      </div>

      {/* Password Input */}
      <div>
        <label htmlFor="password">Password</label>
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
        />
        {errors.password && <span>{errors.password.message}</span>}
      </div>

      {/* Remember Me Checkbox */}
      <div>
        <label htmlFor="rememberMe">
          {/* Checkbox inputs are handled differently - checked comes from register */}
          <input
            id="rememberMe"
            type="checkbox"
            {...register("rememberMe")}
          />
          Remember me
        </label>
      </div>

      {/* Submit Button - disabled while form is submitting */}
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? "Logging in..." : "Login"}
      </button>
    </form>
  );
}

export default LoginForm;
```

### Why Uncontrolled Inputs Are Faster

Traditional controlled components (using useState for every input) cause a re-render on every keystroke. For forms with many fields, this becomes expensive. React Hook Form uses uncontrolled inputs, which bypass React's render cycle for individual keystrokes.

```jsx
// File: src/components/Comparison.tsx

// ❌ SLOW - Controlled component re-renders on every keystroke
// Every time the user types, the entire component re-renders
// This is fine for small forms but becomes problematic with many fields
function SlowForm() {
  const [email, setEmail] = useState("");
  const [password, setEmail] = useState("");
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");
  const [address, setAddress] = useState("");
  
  // When typing in email, ALL five state variables are tracked
  // React doesn't know which ones changed, so it re-renders everything
  return (
    <input value={email} onChange={e => setEmail(e.target.value)} />
  );
}

// ✅ FAST - Uncontrolled component only re-renders when needed
// The input reads/writes directly to the DOM
// React Hook Form collects all values only at submit time
// This means zero re-renders while typing!
function FastForm() {
  const { register } = useForm();
  
  // Each input is uncontrolled - React doesn't track its state
  // The ref from register connects it to React Hook Form's internal store
  return (
    <input {...register("email")} />
  );
}
```

### TypeScript Generics with useForm

TypeScript generics allow you to define the shape of your form data, providing autocomplete and type checking throughout your form components.

```tsx
// File: src/types/form.ts

// Define your form data shape as an interface or type
// This becomes the generic type for useForm
interface ContactFormData {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string; // Optional field with ?
  message: string;
  preferredContact: 'email' | 'phone' | 'both';
}

// File: src/components/ContactForm.tsx

import { useForm } from 'react-hook-form';
import type { ContactFormData } from '../types/form';

function ContactForm() {
  // Pass the type as a generic to useForm
  // This gives us typed versions of register, handleSubmit, etc.
  const { 
    register, 
    handleSubmit, 
    formState: { errors } 
  } = useForm<ContactFormData>({
    // Optional: provide default values
    defaultValues: {
      firstName: "",
      lastName: "",
      email: "",
      message: "",
      preferredContact: 'email'
    }
  });

  const onSubmit = (data: ContactFormData) => {
    // TypeScript knows exactly what properties data has
    console.log(data.firstName); // ✅ Autocomplete works!
    console.log(data.preferredContact); // ✅ Type checked!
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      {/* First Name */}
      <input 
        {...register("firstName", { required: true })} 
        placeholder="First Name"
      />
      {errors.firstName && <span>Required</span>}

      {/* Last Name */}
      <input 
        {...register("lastName", { required: true })} 
        placeholder="Last Name"
      />
      {errors.lastName && <span>Required</span>}

      {/* Email with custom validation */}
      <input 
        {...register("email", { 
          required: "Email is required",
          validate: (value) => 
            value.includes('@') || "Must be a valid email"
        })} 
        placeholder="Email"
      />
      {errors.email && <span>{errors.email.message}</span>}

      {/* Select dropdown */}
      <select {...register("preferredContact")}>
        <option value="email">Email</option>
        <option value="phone">Phone</option>
        <option value="both">Both</option>
      </select>

      {/* Textarea */}
      <textarea 
        {...register("message", { 
          required: "Message is required",
          minLength: { value: 10, message: "Message too short" }
        })} 
        placeholder="Your message"
      />
      {errors.message && <span>{errors.message.message}</span>}

      <button type="submit">Send</button>
    </form>
  );
}
```

### Understanding formState

The `formState` object contains valuable information about your form's current state. It includes errors, whether the form is being submitted, touched fields, and more.

```tsx
// File: src/components/CompleteFormStateExample.tsx

import { useForm } from 'react-hook-form';

interface FormData {
  username: string;
  email: string;
}

function CompleteFormStateExample() {
  const { 
    register, 
    handleSubmit, 
    formState: { 
      errors,        // Validation error objects for each field
      isSubmitting,  // true while form is being submitted
      isValid,       // true if form passes all validation
      isDirty,       // true if any field has been modified
      touchedFields, // object tracking which fields have been focused
      dirtyFields    // object tracking which fields have been modified
    } 
  } = useForm<FormData>({
    mode: "onChange" // Validate on every change (vs "onBlur" or "onSubmit")
  });

  const onSubmit = async (data: FormData) => {
    console.log("Submitting:", data);
    await new Promise(r => setTimeout(r, 2000));
    console.log("Done!");
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <p>Form is {isValid ? "✅ Valid" : "❌ Invalid"}</p>
      <p>Form is {isDirty ? "📝 Dirty (modified)" : "✨ Pristine"}</p>
      <p>Touched fields: {Object.keys(touchedFields).join(", ") || "none"}</p>
      
      <input 
        {...register("username", { required: "Username required" })} 
        placeholder="Username"
      />
      {errors.username && <span>{errors.username.message}</span>}

      <input 
        {...register("email", { 
          required: "Email required",
          pattern: { value: /@/, message: "Invalid email" }
        })} 
        placeholder="Email"
      />
      {errors.email && <span>{errors.email.message}</span>}

      <button 
        type="submit" 
        disabled={isSubmitting || !isValid}
      >
        {isSubmitting ? "Submitting..." : "Submit"}
      </button>
    </form>
  );
}
```

## Common Mistakes

### Mistake 1: Forgetting to Spread register

The most common mistake is forgetting to spread the register props onto your input element. Without this, the input won't be connected to the form's state management.

```tsx
// ❌ WRONG - register is not connected to the input
<input name="email" />
// The input exists but React Hook Form can't track it!

// ✅ CORRECT - Spread the register props
<input {...register("email")} />
// Now React Hook Form controls this input via refs

// ✅ ALSO CORRECT - If you need to add additional props
<input 
  {...register("email")} 
  className="form-input"
  placeholder="Enter your email"
/>
```

### Mistake 2: Not Handling Async Submit Properly

When handling async form submissions, make sure to properly manage loading states and error handling.

```tsx
// ❌ WRONG - No loading state handling
const onSubmit = async (data) => {
  await fetch('/api/submit', { method: 'POST', body: JSON.stringify(data) });
  // User has no feedback that submission happened!
};

// ✅ CORRECT - Handle loading, success, and error states
const { formState: { isSubmitting } } = useForm();

const onSubmit = async (data) => {
  try {
    setIsSubmitting(true);
    await api.submitForm(data);
    alert("Success!");
  } catch (error) {
    setError("root", { message: "Failed to submit form" });
  } finally {
    setIsSubmitting(false);
  }
};
```

### Mistake 3: Not Using the name Parameter Correctly

Field names must be strings and should uniquely identify each field. Nested data requires dot notation.

```tsx
// ❌ WRONG - Using dynamic keys or numbers as names
const fieldName = "email";
<input {...register(fieldName)} /> // Works but not recommended

// ❌ WRONG - Duplicate names
<input {...register("email")} />
<input {...register("email")} /> // Second one overwrites first!

// ✅ CORRECT - Consistent naming with dot notation for objects
<input {...register("user.email")} />
<input {...register("user.password")} />
<input {...register("preferences.notifications")} />

// The resulting data will be:
// { user: { email: "...", password: "..." }, preferences: { notifications: true } }
```

## Real-World Example

Here's a complete registration form with validation, error handling, and TypeScript types:

```tsx
// File: src/components/RegistrationForm.tsx

import { useState } from 'react';
import { useForm } from 'react-hook-form';

// Define the form data shape
interface RegisterFormData {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
  agreeToTerms: boolean;
}

// Define error response type from our API
interface ApiError {
  message: string;
}

function RegistrationForm() {
  // State for showing success message and tracking submission errors
  const [showSuccess, setShowSuccess] = useState(false);
  const [submitError, setSubmitError] = useState<string | null>(null);

  // Initialize useForm with TypeScript generic and default options
  const { 
    register, 
    handleSubmit, 
    formState: { errors, isSubmitting },
    // watch allows us to monitor field values for conditional rendering
    watch 
  } = useForm<RegisterFormData>({
    mode: "onBlur" // Validate on blur (when user leaves field)
  });

  // Watch password to validate confirmPassword matches
  const password = watch("password");

  // The submit handler - called only if all validation passes
  const onSubmit = async (data: RegisterFormData) => {
    try {
      setSubmitError(null);
      
      // Simulate an API call
      const response = await fetch('/api/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          username: data.username,
          email: data.email,
          password: data.password
        })
      });

      if (!response.ok) {
        // Handle API errors
        const error: ApiError = await response.json();
        throw new Error(error.message || 'Registration failed');
      }

      // Success! Show confirmation
      setShowSuccess(true);
    } catch (error) {
      // Catch any errors and display them
      setSubmitError(error instanceof Error ? error.message : 'Something went wrong');
    }
  };

  // If registration successful, show confirmation
  if (showSuccess) {
    return (
      <div className="success-message">
        <h2>🎉 Registration Successful!</h2>
        <p>Please check your email to verify your account.</p>
        <button onClick={() => setShowSuccess(false)}>
          Back to Form
        </button>
      </div>
    );
  }

  return (
    <div className="registration-form">
      <h2>Create Account</h2>

      {/* Show any submission errors at the top */}
      {submitError && (
        <div className="error-banner" role="alert">
          {submitError}
        </div>
      )}

      <form onSubmit={handleSubmit(onSubmit)} noValidate>
        {/* Username Field */}
        <div className="form-group">
          <label htmlFor="username">Username</label>
          <input
            id="username"
            type="text"
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
              // Custom validation: no spaces allowed
              validate: (value) => 
                !value.includes(' ') || "Username cannot contain spaces"
            })}
            placeholder="Choose a username"
          />
          {errors.username && (
            <span className="error-message">{errors.username.message}</span>
          )}
        </div>

        {/* Email Field */}
        <div className="form-group">
          <label htmlFor="email">Email Address</label>
          <input
            id="email"
            type="email"
            {...register("email", {
              required: "Email is required",
              pattern: {
                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                message: "Please enter a valid email address"
              }
            })}
            placeholder="you@example.com"
          />
          {errors.email && (
            <span className="error-message">{errors.email.message}</span>
          )}
        </div>

        {/* Password Field */}
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            id="password"
            type="password"
            {...register("password", {
              required: "Password is required",
              minLength: {
                value: 8,
                message: "Password must be at least 8 characters"
              },
              // Complex password validation
              validate: {
                hasUpperCase: (value) =>
                  /[A-Z]/.test(value) || "Password must contain uppercase letter",
                hasLowerCase: (value) =>
                  /[a-z]/.test(value) || "Password must contain lowercase letter",
                hasNumber: (value) =>
                  /[0-9]/.test(value) || "Password must contain a number",
                hasSpecialChar: (value) =>
                  /[!@#$%^&*]/.test(value) || "Password must contain special character"
              }
            })}
            placeholder="Create a strong password"
          />
          {errors.password && (
            <span className="error-message">{errors.password.message}</span>
          )}
        </div>

        {/* Confirm Password Field */}
        <div className="form-group">
          <label htmlFor="confirmPassword">Confirm Password</label>
          <input
            id="confirmPassword"
            type="password"
            {...register("confirmPassword", {
              required: "Please confirm your password",
              // Cross-field validation: compare to password field
              validate: (value) =>
                value === password || "Passwords do not match"
            })}
            placeholder="Confirm your password"
          />
          {errors.confirmPassword && (
            <span className="error-message">{errors.confirmPassword.message}</span>
          )}
        </div>

        {/* Terms Checkbox */}
        <div className="form-group checkbox">
          <label>
            <input
              type="checkbox"
              {...register("agreeToTerms", {
                required: "You must agree to the terms"
              })}
            />
            I agree to the Terms of Service and Privacy Policy
          </label>
          {errors.agreeToTerms && (
            <span className="error-message">{errors.agreeToTerms.message}</span>
          )}
        </div>

        {/* Submit Button */}
        <button 
          type="submit" 
          disabled={isSubmitting}
          className="submit-button"
        >
          {isSubmitting ? (
            <span>Creating Account...</span>
          ) : (
            <span>Create Account</span>
          )}
        </button>
      </form>
    </div>
  );
}

export default RegistrationForm;
```

## Key Takeaways

- React Hook Form uses uncontrolled inputs, which are significantly faster than controlled inputs for forms with many fields
- The `register` function connects your input elements to the form's state management via refs
- Use TypeScript generics with `useForm<FormData>` for type safety throughout your forms
- The `handleSubmit` function runs validation before calling your submit handler
- Always destructure `formState` to access errors, isSubmitting, and other useful properties
- Use the `mode` option to control when validation runs: "onBlur", "onChange", or "onSubmit"

## What's Next

Now that you understand the basics of React Hook Form, continue to [Validation with Zod](/07-forms/02-react-hook-form/02-validation-with-zod.md) to learn how to define complex validation schemas using Zod, which provides better type inference and reusable validation rules.