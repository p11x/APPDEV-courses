# Accessible Forms and Inputs

## Overview
Forms are critical for user interaction. Accessible forms use proper labels, error messages, and keyboard navigation to ensure all users can complete them.

## Prerequisites
- HTML form basics
- ARIA attributes

## Core Concepts

### Accessible Form Components

```tsx
// [File: src/components/Form/AccessibleInput.tsx]
import React, { useId } from 'react';

interface AccessibleInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
  error?: string;
  helpText?: string;
}

export function AccessibleInput({ 
  label, 
  error, 
  helpText,
  id,
  ...props 
}: AccessibleInputProps) {
  const generatedId = useId();
  const inputId = id || generatedId;
  const errorId = `${inputId}-error`;
  const helpId = `${inputId}-help`;

  return (
    <div className="form-field">
      <label htmlFor={inputId} className="form-label">
        {label}
        {props.required && <span aria-hidden="true"> *</span>}
      </label>
      
      <input
        id={inputId}
        aria-invalid={error ? 'true' : 'false'}
        aria-describedby={
          [error ? errorId : null, helpText ? helpId : null]
            .filter(Boolean)
            .join(' ') || undefined
        }
        className={`form-input ${error ? 'has-error' : ''}`}
        {...props}
      />
      
      {helpText && !error && (
        <span id={helpId} className="form-help">
          {helpText}
        </span>
      )}
      
      {error && (
        <span id={errorId} role="alert" className="form-error">
          {error}
        </span>
      )}
    </div>
  );
}
```

### Complete Form Example

```tsx
// [File: src/components/Form/ContactForm.tsx]
import { useState } from 'react';
import { AccessibleInput } from './AccessibleInput';

interface FormData {
  name: string;
  email: string;
  message: string;
}

interface FormErrors {
  name?: string;
  email?: string;
  message?: string;
}

export function ContactForm() {
  const [formData, setFormData] = useState<FormData>({
    name: '',
    email: '',
    message: ''
  });
  const [errors, setErrors] = useState<FormErrors>({});

  const validate = (): boolean => {
    const newErrors: FormErrors = {};
    
    if (!formData.name.trim()) {
      newErrors.name = 'Name is required';
    }
    
    if (!formData.email.includes('@')) {
      newErrors.email = 'Please enter a valid email';
    }
    
    if (formData.message.length < 10) {
      newErrors.message = 'Message must be at least 10 characters';
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (validate()) {
      console.log('Form submitted:', formData);
    }
  };

  return (
    <form onSubmit={handleSubmit} noValidate>
      <AccessibleInput
        label="Name"
        value={formData.name}
        onChange={(e) => setFormData({ ...formData, name: e.target.value })}
        error={errors.name}
        required
      />
      
      <AccessibleInput
        label="Email"
        type="email"
        value={formData.email}
        onChange={(e) => setFormData({ ...formData, email: e.target.value })}
        error={errors.email}
        helpText="We'll never share your email"
        required
      />
      
      <AccessibleInput
        label="Message"
        value={formData.message}
        onChange={(e) => setFormData({ ...formData, message: e.target.value })}
        error={errors.message}
        required
      />
      
      <button type="submit">Submit</button>
    </form>
  );
}
```

## Key Takeaways
- Always use semantic labels
- Use aria-invalid and aria-describedby
- Error messages should be announced with role="alert"

## What's Next
Continue to [Accessible Navigation](03-accessible-navigation.md) for navigation patterns.