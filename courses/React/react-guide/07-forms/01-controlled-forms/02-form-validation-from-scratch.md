# Form Validation From Scratch

## Overview
Form validation ensures that user input meets your requirements before submission. While libraries like Formik and React Hook Form provide validation solutions, understanding how to build validation from scratch helps you understand the underlying concepts and gives you full control. This guide covers validation patterns, error handling, and best practices for building robust form validation.

## Prerequisites
- Understanding of controlled forms
- Knowledge of JavaScript regex patterns
- Familiarity with React hooks
- Basic understanding of HTML5 validation

## Core Concepts

### Validation Rules and Schema
Defining validation rules clearly helps maintainable code. Using a schema-based approach makes validation easy to understand and modify.

```jsx
// File: src/utils/validationRules.js

/**
 * Common validation functions
 */
export const validators = {
  // Required field check
  required: (value) => {
    if (value === null || value === undefined || value === '') {
      return 'This field is required';
    }
    if (Array.isArray(value) && value.length === 0) {
      return 'This field is required';
    }
    return null;
  },

  // Minimum length
  minLength: (min) => (value) => {
    if (value && value.length < min) {
      return `Must be at least ${min} characters`;
    }
    return null;
  },

  // Maximum length
  maxLength: (max) => (value) => {
    if (value && value.length > max) {
      return `Must be no more than ${max} characters`;
    }
    return null;
  },

  // Email validation
  email: (value) => {
    if (value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      return 'Please enter a valid email address';
    }
    return null;
  },

  // URL validation
  url: (value) => {
    if (value && !/^https?:\/\/.+/.test(value)) {
      return 'Please enter a valid URL';
    }
    return null;
  },

  // Minimum value (for numbers)
  min: (min) => (value) => {
    if (value !== '' && Number(value) < min) {
      return `Must be at least ${min}`;
    }
    return null;
  },

  // Maximum value (for numbers)
  max: (max) => (value) => {
    if (value !== '' && Number(value) > max) {
      return `Must be no more than ${max}`;
    }
    return null;
  },

  // Pattern matching
  pattern: (regex, message) => (value) => {
    if (value && !regex.test(value)) {
      return message || 'Invalid format';
    }
    return null;
  },

  // Match another field
  match: (fieldName, displayName) => (value, values) => {
    if (value !== values[fieldName]) {
      return `${displayName} does not match`;
    }
    return null;
  },

  // Custom validator
  custom: (validatorFn, errorMessage) => (value, values) => {
    if (value && !validatorFn(value, values)) {
      return errorMessage || 'Invalid value';
    }
    return null;
  },
};

/**
 * Form validation schema
 * Defines validation rules for each field
 */
export const validationSchema = {
  username: [
    validators.required,
    validators.minLength(3),
    validators.maxLength(20),
    validators.pattern(/^[a-zA-Z0-9_]+$/, 'Only letters, numbers, and underscores allowed'),
  ],

  email: [
    validators.required,
    validators.email,
  ],

  password: [
    validators.required,
    validators.minLength(8),
    // Custom validation: must contain at least one number
    validators.custom(
      (value) => /\d/.test(value),
      'Password must contain at least one number'
    ),
    // Custom validation: must contain at least one uppercase
    validators.custom(
      (value) => /[A-Z]/.test(value),
      'Password must contain at least one uppercase letter'
    ),
  ],

  confirmPassword: [
    validators.required,
    validators.match('password', 'Password'),
  ],

  age: [
    validators.required,
    validators.min(13),
    validators.max(120),
  ],

  website: [
    validators.url,
  ],
};

/**
 * Validate a single field
 */
export function validateField(value, rules) {
  for (const rule of rules) {
    const error = rule(value);
    if (error) {
      return error;
    }
  }
  return null;
}

/**
 * Validate entire form
 */
export function validateForm(values, schema) {
  const errors = {};
  let isValid = true;

  for (const field in schema) {
    const rules = schema[field];
    const value = values[field];
    const error = validateField(value, rules);
    
    if (error) {
      errors[field] = error;
      isValid = false;
    }
  }

  return { errors, isValid };
}
```

### Building a Form Validation Hook
Creating a custom hook for validation encapsulates all validation logic in one place.

```jsx
// File: src/hooks/useFormValidation.js

import { useState, useCallback, useMemo } from 'react';
import { validateField, validateForm } from '../utils/validationRules';

/**
 * Custom hook for form validation
 * @param {object} initialValues - Initial form values
 * @param {object} validationSchema - Validation rules for each field
 * @returns {object} Form state and handlers
 */
function useFormValidation(initialValues = {}, validationSchema = {}) {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Validate single field
  const validateSingleField = useCallback((name, value) => {
    const rules = validationSchema[name];
    if (!rules) return null;
    return validateField(value, rules);
  }, [validationSchema]);

  // Handle field change
  const handleChange = useCallback((event) => {
    const { name, value, type, checked } = event.target;
    
    setValues(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));

    // Clear error on change if field was touched
    if (touched[name]) {
      const error = validateSingleField(name, value);
      setErrors(prev => ({
        ...prev,
        [name]: error || '',
      }));
    }
  }, [touched, validateSingleField]);

  // Handle field blur
  const handleBlur = useCallback((event) => {
    const { name, value } = event.target;
    
    // Mark field as touched
    setTouched(prev => ({ ...prev, [name]: true }));
    
    // Validate on blur
    const error = validateSingleField(name, value);
    setErrors(prev => ({
      ...prev,
      [name]: error || '',
    }));
  }, [validateSingleField]);

  // Validate all fields
  const validateAll = useCallback(() => {
    const { errors, isValid } = validateForm(values, validationSchema);
    setErrors(errors);
    return isValid;
  }, [values, validationSchema]);

  // Handle form submission
  const handleSubmit = useCallback(async (onSubmit) => {
    return async (event) => {
      event.preventDefault();
      
      // Mark all fields as touched
      const allTouched = {};
      Object.keys(values).forEach(key => {
        allTouched[key] = true;
      });
      setTouched(allTouched);
      
      // Validate all fields
      const isValid = validateAll();
      
      if (!isValid) {
        return { success: false, errors };
      }
      
      setIsSubmitting(true);
      
      try {
        await onSubmit(values);
        return { success: true };
      } catch (error) {
        return { success: false, error };
      } finally {
        setIsSubmitting(false);
      }
    };
  }, [values, validateAll]);

  // Reset form
  const reset = useCallback(() => {
    setValues(initialValues);
    setErrors({});
    setTouched({});
    setIsSubmitting(false);
  }, [initialValues]);

  // Set field value programmatically
  const setFieldValue = useCallback((name, value) => {
    setValues(prev => ({ ...prev, [name]: value }));
  }, []);

  // Get field props
  const getFieldProps = useCallback((name) => ({
    name,
    value: values[name] ?? '',
    onChange: handleChange,
    onBlur: handleBlur,
    error: touched[name] && errors[name],
    touched: touched[name],
  }), [values, touched, errors, handleChange, handleBlur]);

  return {
    values,
    errors,
    touched,
    isSubmitting,
    handleChange,
    handleBlur,
    handleSubmit,
    validateAll,
    reset,
    setFieldValue,
    getFieldProps,
    setValues,
    setErrors,
    setTouched,
  };
}

export default useFormValidation;
```

### Real-Time Validation Display
Showing validation errors at the right time improves user experience without being intrusive.

```jsx
// File: src/components/ValidatedForm.jsx

import React from 'react';
import useFormValidation from '../hooks/useFormValidation';
import { validationSchema } from '../utils/validationRules';

function ValidatedRegistrationForm() {
  const initialValues = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
    age: '',
    agree: false,
  };

  const {
    values,
    errors,
    touched,
    isSubmitting,
    handleChange,
    handleBlur,
    handleSubmit,
    reset,
    getFieldProps,
  } = useFormValidation(initialValues, validationSchema);

  const onSubmit = async (formData) => {
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1000));
    console.log('Form submitted:', formData);
    alert('Registration successful!');
    reset();
  };

  // Helper to show error only when field is touched
  const getError = (fieldName) => {
    return touched[fieldName] ? errors[fieldName] : '';
  };

  // Helper to check if field has error
  const hasError = (fieldName) => {
    return touched[fieldName] && !!errors[fieldName];
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} noValidate>
      <h2>Registration Form</h2>

      {/* Username Field */}
      <div className="form-group">
        <label htmlFor="username">Username</label>
        <input
          {...getFieldProps('username')}
          type="text"
          id="username"
          placeholder="Enter username"
        />
        {hasError('username') && (
          <span className="error-message">{getError('username')}</span>
        )}
      </div>

      {/* Email Field */}
      <div className="form-group">
        <label htmlFor="email">Email</label>
        <input
          {...getFieldProps('email')}
          type="email"
          id="email"
          placeholder="Enter email"
        />
        {hasError('email') && (
          <span className="error-message">{getError('email')}</span>
        )}
      </div>

      {/* Password Field */}
      <div className="form-group">
        <label htmlFor="password">Password</label>
        <input
          {...getFieldProps('password')}
          type="password"
          id="password"
          placeholder="Enter password"
        />
        {hasError('password') && (
          <span className="error-message">{getError('password')}</span>
        )}
      </div>

      {/* Confirm Password Field */}
      <div className="form-group">
        <label htmlFor="confirmPassword">Confirm Password</label>
        <input
          {...getFieldProps('confirmPassword')}
          type="password"
          id="confirmPassword"
          placeholder="Confirm password"
        />
        {hasError('confirmPassword') && (
          <span className="error-message">{getError('confirmPassword')}</span>
        )}
      </div>

      {/* Age Field */}
      <div className="form-group">
        <label htmlFor="age">Age</label>
        <input
          {...getFieldProps('age')}
          type="number"
          id="age"
          placeholder="Enter age"
        />
        {hasError('age') && (
          <span className="error-message">{getError('age')}</span>
        )}
      </div>

      {/* Checkbox */}
      <div className="form-group">
        <label>
          <input
            {...getFieldProps('agree')}
            type="checkbox"
          />
          I agree to the terms and conditions
        </label>
        {hasError('agree') && (
          <span className="error-message">{getError('agree')}</span>
        )}
      </div>

      {/* Submit Button */}
      <button 
        type="submit" 
        disabled={isSubmitting}
        className="submit-btn"
      >
        {isSubmitting ? 'Submitting...' : 'Register'}
      </button>

      <button 
        type="button" 
        onClick={reset}
        disabled={isSubmitting}
      >
        Reset
      </button>
    </form>
  );
}

export default ValidatedRegistrationForm;
```

### Async Validation
For validation that requires server-side checks (like checking username availability), async validation is needed.

```jsx
// File: src/hooks/useAsyncValidation.js

import { useState, useCallback, useRef } from 'react';

/**
 * Hook for async field validation (e.g., checking username availability)
 */
function useAsyncValidation(initialValue = '') {
  const [value, setValue] = useState(initialValue);
  const [error, setError] = useState('');
  const [isValidating, setIsValidating] = useState(false);
  const abortControllerRef = useRef(null);

  /**
   * Validate field with async function
   * @param {string} fieldValue - Value to validate
   * @param {function} validateFn - Async function that returns null (valid) or error string
   */
  const validate = useCallback(async (fieldValue) => {
    // Cancel any previous validation
    if (abortControllerRef.current) {
      abortControllerRef.current.abort();
    }

    // Don't validate empty fields
    if (!fieldValue || !fieldValue.trim()) {
      setError('');
      return true;
    }

    setIsValidating(true);
    setError('');

    // Create new abort controller
    abortControllerRef.current = new AbortController();

    try {
      // Call the validation function
      const result = await validateFn(fieldValue, abortControllerRef.current.signal);
      
      // Check if aborted
      if (abortControllerRef.current.signal.aborted) {
        return false;
      }

      if (result) {
        setError(result); // Validation failed, set error message
        return false;
      }

      setError(''); // Validation passed
      return true;
    } catch (err) {
      if (err.name === 'AbortError') {
        return false;
      }
      setError('Validation failed');
      return false;
    } finally {
      setIsValidating(false);
    }
  }, []);

  const handleChange = useCallback((event) => {
    const newValue = event.target.value;
    setValue(newValue);
    setError(''); // Clear error on change
  }, []);

  const handleBlur = useCallback((event) => {
    validate(event.target.value);
  }, [validate]);

  const reset = useCallback(() => {
    if (abortControllerRef.current) {
      abortControllerRef.current.abort();
    }
    setValue(initialValue);
    setError('');
    setIsValidating(false);
  }, [initialValue]);

  return {
    value,
    error,
    isValidating,
    handleChange,
    handleBlur,
    validate,
    reset,
    setValue,
  };
}

export default useAsyncValidation;

// File: src/components/AsyncValidatedForm.jsx

import React from 'react';
import useAsyncValidation from '../hooks/useAsyncValidation';

// Simulated API call to check username availability
const checkUsernameAvailability = async (username, signal) => {
  // Simulate network delay
  await new Promise((resolve, reject) => {
    const timeout = setTimeout(resolve, 1000);
    
    // Allow cancellation
    signal?.addEventListener('abort', () => {
      clearTimeout(timeout);
      reject(new DOMException('Aborted', 'AbortError'));
    });
  });

  // Simulated taken usernames
  const takenUsernames = ['admin', 'user', 'test', 'john', 'jane'];
  
  if (takenUsernames.includes(username.toLowerCase())) {
    return 'This username is already taken';
  }

  return null; // Valid
};

function AsyncValidatedForm() {
  const {
    value: username,
    error: usernameError,
    isValidating,
    handleChange,
    handleBlur,
    reset,
  } = useAsyncValidation('');

  const handleSubmit = (event) => {
    event.preventDefault();
    
    if (usernameError || isValidating || !username) {
      return;
    }
    
    console.log('Form submitted with username:', username);
    alert('Form submitted!');
    reset();
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="username">Username</label>
        <input
          id="username"
          type="text"
          value={username}
          onChange={handleChange}
          onBlur={handleBlur}
          placeholder="Enter username"
        />
        
        {/* Show loading state */}
        {isValidating && (
          <span className="validating">Checking availability...</span>
        )}
        
        {/* Show error */}
        {usernameError && (
          <span className="error">{usernameError}</span>
        )}
        
        {/* Show success */}
        {!usernameError && username && !isValidating && (
          <span className="success">Username is available!</span>
        )}
      </div>

      <button 
        type="submit" 
        disabled={isValidating || !!usernameError || !username}
      >
        {isValidating ? 'Checking...' : 'Submit'}
      </button>

      <button type="button" onClick={reset}>
        Reset
      </button>
    </form>
  );
}

export default AsyncValidatedForm;
```

## Common Mistakes

### Mistake 1: Validating on Every Keystroke
Validating on every keystroke can be annoying and affect performance.

```jsx
// ❌ WRONG — Validate on every change
const handleChange = (e) => {
  setValue(e.target.value);
  validate(e.target.value); // Annoying for user!
};

// ✅ CORRECT — Validate on blur, or with debounce
const handleBlur = (e) => {
  validate(e.target.value); // Validate when user leaves field
};
```

### Mistake 2: Not Providing Clear Error Messages
Error messages should be specific and helpful.

```jsx
// ❌ WRONG — Generic error message
const validate = (value) => {
  if (!value) return 'Invalid';
};

// ✅ CORRECT — Specific, helpful error message
const validate = (value) => {
  if (!value) return 'Please enter your email address';
  if (!/@/.test(value)) return 'Email must contain @ symbol';
};
```

### Mistake 3: Not Handling All Validation States
Always handle loading, success, and error states for async validation.

```jsx
// ❌ WRONG — Only handling error
if (error) return <span className="error">{error}</span>;

// ✅ CORRECT — Handle all states
return (
  <div>
    {isValidating && <span>Checking...</span>}
    {error && <span className="error">{error}</span>}
    {isValid && !error && <span className="success">Valid!</span>}
  </div>
);
```

## Real-World Example
Building a comprehensive validation system with all patterns combined.

```jsx
// File: src/components/CompleteValidationExample.jsx

import React from 'react';

// Validation rules (simplified from earlier)
const validators = {
  required: (msg = 'This field is required') => (value) => {
    if (value === null || value === undefined || value === '') return msg;
    return null;
  },
  
  email: () => (value) => {
    if (value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      return 'Please enter a valid email';
    }
    return null;
  },
  
  minLength: (min) => (value) => {
    if (value && value.length < min) {
      return `Must be at least ${min} characters`;
    }
    return null;
  },
  
  match: (field, label) => (value, values) => {
    if (value !== values[field]) return `${label} does not match`;
    return null;
  },
};

// Complete form with validation
function CompleteValidationForm() {
  const [formData, setFormData] = React.useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  });

  const [errors, setErrors] = React.useState({});
  const [touched, setTouched] = React.useState({});
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  const [submitResult, setSubmitResult] = React.useState(null);

  // Validation schema
  const validationRules = {
    username: [
      validators.required('Username is required'),
      validators.minLength(3),
    ],
    email: [
      validators.required('Email is required'),
      validators.email(),
    ],
    password: [
      validators.required('Password is required'),
      validators.minLength(8),
    ],
    confirmPassword: [
      validators.required('Please confirm your password'),
      validators.match('password', 'Password'),
    ],
  };

  // Validate single field
  const validateField = (name, value) => {
    const rules = validationRules[name];
    if (!rules) return null;
    
    for (const rule of rules) {
      const error = rule(value, formData);
      if (error) return error;
    }
    return null;
  };

  // Validate all fields
  const validateAll = () => {
    const newErrors = {};
    let isValid = true;
    
    for (const field in validationRules) {
      const error = validateField(field, formData[field]);
      if (error) {
        newErrors[field] = error;
        isValid = false;
      }
    }
    
    setErrors(newErrors);
    return isValid;
  };

  // Handlers
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    const newValue = type === 'checkbox' ? checked : value;
    
    setFormData(prev => ({ ...prev, [name]: newValue }));
    
    // Clear error when user types
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const handleBlur = (e) => {
    const { name, value } = e.target;
    setTouched(prev => ({ ...prev, [name]: true }));
    
    const error = validateField(name, value);
    setErrors(prev => ({ ...prev, [name]: error || '' }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    // Mark all as touched
    const allTouched = {};
    Object.keys(formData).forEach(key => allTouched[key] = true);
    setTouched(allTouched);
    
    const isValid = validateAll();
    
    if (!isValid) return;
    
    setIsSubmitting(true);
    setSubmitResult(null);
    
    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1500));
      
      console.log('Form data:', formData);
      setSubmitResult({ success: true });
      
      // Reset form
      setFormData({
        username: '',
        email: '',
        password: '',
        confirmPassword: '',
      });
      setTouched({});
      setErrors({});
    } catch (error) {
      setSubmitResult({ success: false, message: error.message });
    } finally {
      setIsSubmitting(false);
    }
  };

  const getFieldError = (name) => {
    return touched[name] ? errors[name] : '';
  };

  const hasFieldError = (name) => {
    return touched[name] && !!errors[name];
  };

  return (
    <div className="form-container">
      <h2>Complete Validation Form</h2>

      {submitResult?.success && (
        <div className="alert success">
          ✓ Form submitted successfully!
        </div>
      )}

      {submitResult?.success === false && (
        <div className="alert error">
          ✗ {submitResult.message}
        </div>
      )}

      <form onSubmit={handleSubmit} noValidate>
        {/* Username */}
        <div className={`form-group ${hasFieldError('username') ? 'has-error' : ''}`}>
          <label htmlFor="username">Username</label>
          <input
            id="username"
            name="username"
            type="text"
            value={formData.username}
            onChange={handleChange}
            onBlur={handleBlur}
            autoComplete="username"
          />
          {hasFieldError('username') && (
            <span className="error-message">{getFieldError('username')}</span>
          )}
        </div>

        {/* Email */}
        <div className={`form-group ${hasFieldError('email') ? 'has-error' : ''}`}>
          <label htmlFor="email">Email</label>
          <input
            id="email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            onBlur={handleBlur}
            autoComplete="email"
          />
          {hasFieldError('email') && (
            <span className="error-message">{getFieldError('email')}</span>
          )}
        </div>

        {/* Password */}
        <div className={`form-group ${hasFieldError('password') ? 'has-error' : ''}`}>
          <label htmlFor="password">Password</label>
          <input
            id="password"
            name="password"
            type="password"
            value={formData.password}
            onChange={handleChange}
            onBlur={handleBlur}
            autoComplete="new-password"
          />
          {hasFieldError('password') && (
            <span className="error-message">{getFieldError('password')}</span>
          )}
        </div>

        {/* Confirm Password */}
        <div className={`form-group ${hasFieldError('confirmPassword') ? 'has-error' : ''}`}>
          <label htmlFor="confirmPassword">Confirm Password</label>
          <input
            id="confirmPassword"
            name="confirmPassword"
            type="password"
            value={formData.confirmPassword}
            onChange={handleChange}
            onBlur={handleBlur}
            autoComplete="new-password"
          />
          {hasFieldError('confirmPassword') && (
            <span className="error-message">{getFieldError('confirmPassword')}</span>
          )}
        </div>

        {/* Submit */}
        <button 
          type="submit" 
          disabled={isSubmitting}
          className="submit-button"
        >
          {isSubmitting ? 'Submitting...' : 'Create Account'}
        </button>
      </form>
    </div>
  );
}

export default CompleteValidationForm;
```

## Key Takeaways
- Define validation rules as reusable functions for maintainability
- Validate on blur for better UX, or use debounce for real-time validation
- Show validation errors only after field is touched to avoid premature errors
- Use async validation for server-side checks like username availability
- Always validate on both client and server side for security
- Clear error messages when user starts correcting the input

## What's Next
Continue to [Multi-Step Forms](03-multi-step-forms.md) to learn how to build complex, multi-step forms with progress tracking and validation across steps.
