# Multi-Step Forms in React

## Overview
Multi-step forms (also called wizards or steppers) break complex forms into smaller, manageable sections. This approach improves user experience by reducing cognitive load, allowing partial completion, and providing clear progress feedback. This guide covers building multi-step forms with state management, validation per step, and navigation controls.

## Prerequisites
- Understanding of controlled forms
- Knowledge of form validation
- Familiarity with React hooks (useState, useReducer)
- Understanding of component composition

## Core Concepts

### Basic Multi-Step Form Structure
The foundation of a multi-step form is managing state across steps while preserving data when navigating between steps.

```jsx
// File: src/components/BasicMultiStepForm.jsx

import React, { useState } from 'react';

// Step components
function StepOne({ data, onChange, onNext }) {
  return (
    <div className="form-step">
      <h3>Step 1: Personal Information</h3>
      
      <div className="form-group">
        <label htmlFor="firstName">First Name</label>
        <input
          id="firstName"
          name="firstName"
          value={data.firstName || ''}
          onChange={(e) => onChange({ firstName: e.target.value })}
          type="text"
        />
      </div>
      
      <div className="form-group">
        <label htmlFor="lastName">Last Name</label>
        <input
          id="lastName"
          name="lastName"
          value={data.lastName || ''}
          onChange={(e) => onChange({ lastName: e.target.value })}
          type="text"
        />
      </div>
      
      <button onClick={onNext}>Next</button>
    </div>
  );
}

function StepTwo({ data, onChange, onNext, onBack }) {
  return (
    <div className="form-step">
      <h3>Step 2: Contact Details</h3>
      
      <div className="form-group">
        <label htmlFor="email">Email</label>
        <input
          id="email"
          name="email"
          value={data.email || ''}
          onChange={(e) => onChange({ email: e.target.value })}
          type="email"
        />
      </div>
      
      <div className="form-group">
        <label htmlFor="phone">Phone</label>
        <input
          id="phone"
          name="phone"
          value={data.phone || ''}
          onChange={(e) => onChange({ phone: e.target.value })}
          type="tel"
        />
      </div>
      
      <button onClick={onBack}>Back</button>
      <button onClick={onNext}>Next</button>
    </div>
  );
}

function StepThree({ data, onChange, onBack, onSubmit }) {
  return (
    <div className="form-step">
      <h3>Step 3: Review & Submit</h3>
      
      <div className="review">
        <p><strong>Name:</strong> {data.firstName} {data.lastName}</p>
        <p><strong>Email:</strong> {data.email}</p>
        <p><strong>Phone:</strong> {data.phone}</p>
      </div>
      
      <button onClick={onBack}>Back</button>
      <button onClick={onSubmit}>Submit</button>
    </div>
  );
}

// Progress indicator
function ProgressBar({ currentStep, totalSteps }) {
  const progress = ((currentStep + 1) / totalSteps) * 100;
  
  return (
    <div className="progress-bar">
      <div 
        className="progress-fill" 
        style={{ width: `${progress}%` }}
      />
      <div className="step-indicators">
        {Array.from({ length: totalSteps }).map((_, i) => (
          <div 
            key={i} 
            className={`step-dot ${i <= currentStep ? 'active' : ''}`}
          >
            {i + 1}
          </div>
        ))}
      </div>
    </div>
  );
}

// Main multi-step form container
function BasicMultiStepForm() {
  const [currentStep, setCurrentStep] = useState(0);
  const [formData, setFormData] = useState({});
  const [isSubmitted, setIsSubmitted] = useState(false);

  const totalSteps = 3;

  const handleDataChange = (newData) => {
    setFormData(prev => ({ ...prev, ...newData }));
  };

  const handleNext = () => {
    if (currentStep < totalSteps - 1) {
      setCurrentStep(prev => prev + 1);
    }
  };

  const handleBack = () => {
    if (currentStep > 0) {
      setCurrentStep(prev => prev - 1);
    }
  };

  const handleSubmit = async () => {
    console.log('Submitting:', formData);
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1000));
    setIsSubmitted(true);
  };

  if (isSubmitted) {
    return (
      <div className="success-message">
        <h2>✓ Form Submitted Successfully!</h2>
        <p>Thank you for your submission.</p>
        <pre>{JSON.stringify(formData, null, 2)}</pre>
      </div>
    );
  }

  return (
    <div className="multi-step-form">
      <ProgressBar currentStep={currentStep} totalSteps={totalSteps} />
      
      {currentStep === 0 && (
        <StepOne 
          data={formData} 
          onChange={handleDataChange} 
          onNext={handleNext}
        />
      )}
      
      {currentStep === 1 && (
        <StepTwo 
          data={formData} 
          onChange={handleDataChange} 
          onNext={handleNext}
          onBack={handleBack}
        />
      )}
      
      {currentStep === 2 && (
        <StepThree 
          data={formData} 
          onChange={handleDataChange} 
          onBack={handleBack}
          onSubmit={handleSubmit}
        />
      )}
    </div>
  );
}

export default BasicMultiStepForm;
```

### Using useReducer for Complex Form State
For more complex multi-step forms, useReducer provides better state management with clear actions.

```jsx
// File: src/hooks/useMultiStepForm.js

import { useReducer, useCallback } from 'react';

// Initial state
const initialState = {
  currentStep: 0,
  formData: {},
  errors: {},
  touched: {},
  isSubmitting: false,
  isSubmitted: false,
};

// Action types
const ActionTypes = {
  SET_FIELD: 'SET_FIELD',
  SET_FIELDS: 'SET_FIELDS',
  SET_STEP: 'SET_STEP',
  NEXT_STEP: 'NEXT_STEP',
  PREV_STEP: 'PREV_STEP',
  SET_ERROR: 'SET_ERROR',
  SET_ERRORS: 'SET_ERRORS',
  CLEAR_ERROR: 'CLEAR_ERROR',
  SET_TOUCHED: 'SET_TOUCHED',
  SET_SUBMITTING: 'SET_SUBMITTING',
  SET_SUBMITTED: 'SET_SUBMITTED',
  RESET: 'RESET',
};

// Reducer function
function multiStepReducer(state, action) {
  switch (action.type) {
    case ActionTypes.SET_FIELD:
      return {
        ...state,
        formData: {
          ...state.formData,
          [action.field]: action.value,
        },
        errors: {
          ...state.errors,
          [action.field]: '',
        },
      };
      
    case ActionTypes.SET_FIELDS:
      return {
        ...state,
        formData: { ...state.formData, ...action.fields },
      };
      
    case ActionTypes.SET_STEP:
      return {
        ...state,
        currentStep: action.step,
      };
      
    case ActionTypes.NEXT_STEP:
      return {
        ...state,
        currentStep: Math.min(state.currentStep + 1, action.maxStep),
      };
      
    case ActionTypes.PREV_STEP:
      return {
        ...state,
        currentStep: Math.max(state.currentStep - 1, 0),
      };
      
    case ActionTypes.SET_ERROR:
      return {
        ...state,
        errors: { ...state.errors, [action.field]: action.error },
      };
      
    case ActionTypes.SET_ERRORS:
      return {
        ...state,
        errors: { ...state.errors, ...action.errors },
      };
      
    case ActionTypes.CLEAR_ERROR:
      const newErrors = { ...state.errors };
      delete newErrors[action.field];
      return {
        ...state,
        errors: newErrors,
      };
      
    case ActionTypes.SET_TOUCHED:
      return {
        ...state,
        touched: { ...state.touched, [action.field]: true },
      };
      
    case ActionTypes.SET_SUBMITTING:
      return {
        ...state,
        isSubmitting: action.isSubmitting,
      };
      
    case ActionTypes.SET_SUBMITTED:
      return {
        ...state,
        isSubmitted: true,
        isSubmitting: false,
      };
      
    case ActionTypes.RESET:
      return initialState;
      
    default:
      return state;
  }
}

/**
 * Custom hook for multi-step form state management
 */
function useMultiStepForm(initialData = {}) {
  const [state, dispatch] = useReducer(multiStepReducer, {
    ...initialState,
    formData: initialData,
  });

  // Action creators
  const setField = useCallback((field, value) => {
    dispatch({ type: ActionTypes.SET_FIELD, field, value });
  }, []);

  const setFields = useCallback((fields) => {
    dispatch({ type: ActionTypes.SET_FIELDS, fields });
  }, []);

  const setStep = useCallback((step) => {
    dispatch({ type: ActionTypes.SET_STEP, step });
  }, []);

  const nextStep = useCallback((maxStep) => {
    dispatch({ type: ActionTypes.NEXT_STEP, maxStep });
  }, []);

  const prevStep = useCallback(() => {
    dispatch({ type: ActionTypes.PREV_STEP });
  }, []);

  const setError = useCallback((field, error) => {
    dispatch({ type: ActionTypes.SET_ERROR, field, error });
  }, []);

  const setErrors = useCallback((errors) => {
    dispatch({ type: ActionTypes.SET_ERRORS, errors });
  }, []);

  const setTouched = useCallback((field) => {
    dispatch({ type: ActionTypes.SET_TOUCHED, field });
  }, []);

  const setSubmitting = useCallback((isSubmitting) => {
    dispatch({ type: ActionTypes.SET_SUBMITTING, isSubmitting });
  }, []);

  const setSubmitted = useCallback(() => {
    dispatch({ type: ActionTypes.SET_SUBMITTED });
  }, []);

  const reset = useCallback(() => {
    dispatch({ type: ActionTypes.RESET });
  }, []);

  return {
    ...state,
    setField,
    setFields,
    setStep,
    nextStep,
    prevStep,
    setError,
    setErrors,
    setTouched,
    setSubmitting,
    setSubmitted,
    reset,
  };
}

export default useMultiStepForm;
```

### Validation Per Step
Validating each step before allowing the user to proceed ensures data integrity without overwhelming the user.

```jsx
// File: src/components/ValidatedMultiStepForm.jsx

import React from 'react';
import useMultiStepForm from '../hooks/useMultiStepForm';

// Step validation schemas
const stepValidation = {
  0: (data) => {
    const errors = {};
    if (!data.firstName?.trim()) errors.firstName = 'First name is required';
    if (!data.lastName?.trim()) errors.lastName = 'Last name is required';
    return errors;
  },
  
  1: (data) => {
    const errors = {};
    if (!data.email?.trim()) errors.email = 'Email is required';
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.email)) {
      errors.email = 'Invalid email format';
    }
    if (!data.phone?.trim()) errors.phone = 'Phone is required';
    return errors;
  },
  
  2: (data) => {
    const errors = {};
    if (!data.address?.trim()) errors.address = 'Address is required';
    if (!data.city?.trim()) errors.city = 'City is required';
    if (!data.zipCode?.trim()) errors.zipCode = 'ZIP code is required';
    return errors;
  },
};

function ValidatedMultiStepForm() {
  const {
    currentStep,
    formData,
    errors,
    isSubmitting,
    isSubmitted,
    setField,
    nextStep,
    prevStep,
    setErrors,
    setTouched,
    setSubmitting,
    setSubmitted,
    reset,
  } = useMultiStepForm({});

  const totalSteps = 4;

  // Validate current step before proceeding
  const handleNext = () => {
    const stepErrors = stepValidation[currentStep](formData);
    
    // Mark all fields in current step as touched
    Object.keys(stepErrors).forEach(field => setTouched(field));
    
    if (Object.keys(stepErrors).length > 0) {
      setErrors(stepErrors);
      return;
    }
    
    // Clear any errors and proceed
    setErrors({});
    nextStep(totalSteps - 1);
  };

  const handleSubmit = async () => {
    // Validate all data before final submission
    let allErrors = {};
    for (const validator of Object.values(stepValidation)) {
      allErrors = { ...allErrors, ...validator(formData) };
    }
    
    if (Object.keys(allErrors).length > 0) {
      setErrors(allErrors);
      return;
    }
    
    setSubmitting(true);
    
    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1500));
      console.log('Form submitted:', formData);
      setSubmitted();
    } catch (error) {
      console.error('Submit error:', error);
    } finally {
      setSubmitting(false);
    }
  };

  const handleFieldChange = (field, value) => {
    setField(field, value);
  };

  if (isSubmitted) {
    return (
      <div className="success">
        <h2>✓ Registration Complete!</h2>
        <p>Check your email for confirmation.</p>
        <button onClick={reset}>Start Over</button>
      </div>
    );
  }

  return (
    <div className="multi-step-form">
      {/* Progress */}
      <div className="stepper">
        {['Personal', 'Contact', 'Address', 'Review'].map((label, i) => (
          <div 
            key={i} 
            className={`step ${i <= currentStep ? 'active' : ''} ${i < currentStep ? 'completed' : ''}`}
          >
            <div className="step-number">{i + 1}</div>
            <div className="step-label">{label}</div>
          </div>
        ))}
      </div>

      {/* Step Content */}
      <div className="step-content">
        {currentStep === 0 && (
          <StepPersonal 
            data={formData} 
            errors={errors}
            onChange={handleFieldChange}
          />
        )}
        
        {currentStep === 1 && (
          <StepContact 
            data={formData} 
            errors={errors}
            onChange={handleFieldChange}
          />
        )}
        
        {currentStep === 2 && (
          <StepAddress 
            data={formData} 
            errors={errors}
            onChange={handleFieldChange}
          />
        )}
        
        {currentStep === 3 && (
          <StepReview 
            data={formData} 
          />
        )}
      </div>

      {/* Navigation */}
      <div className="step-navigation">
        {currentStep > 0 && (
          <button onClick={prevStep} disabled={isSubmitting}>
            Back
          </button>
        )}
        
        {currentStep < totalSteps - 1 ? (
          <button onClick={handleNext}>
            Next
          </button>
        ) : (
          <button onClick={handleSubmit} disabled={isSubmitting}>
            {isSubmitting ? 'Submitting...' : 'Complete Registration'}
          </button>
        )}
      </div>
    </div>
  );
}

// Step components
function StepPersonal({ data, errors, onChange }) {
  return (
    <div>
      <h3>Personal Information</h3>
      <div className="form-group">
        <label>First Name *</label>
        <input
          value={data.firstName || ''}
          onChange={(e) => onChange('firstName', e.target.value)}
        />
        {errors.firstName && <span className="error">{errors.firstName}</span>}
      </div>
      <div className="form-group">
        <label>Last Name *</label>
        <input
          value={data.lastName || ''}
          onChange={(e) => onChange('lastName', e.target.value)}
        />
        {errors.lastName && <span className="error">{errors.lastName}</span>}
      </div>
    </div>
  );
}

function StepContact({ data, errors, onChange }) {
  return (
    <div>
      <h3>Contact Information</h3>
      <div className="form-group">
        <label>Email *</label>
        <input
          type="email"
          value={data.email || ''}
          onChange={(e) => onChange('email', e.target.value)}
        />
        {errors.email && <span className="error">{errors.email}</span>}
      </div>
      <div className="form-group">
        <label>Phone *</label>
        <input
          type="tel"
          value={data.phone || ''}
          onChange={(e) => onChange('phone', e.target.value)}
        />
        {errors.phone && <span className="error">{errors.phone}</span>}
      </div>
    </div>
  );
}

function StepAddress({ data, errors, onChange }) {
  return (
    <div>
      <h3>Address</h3>
      <div className="form-group">
        <label>Street Address *</label>
        <input
          value={data.address || ''}
          onChange={(e) => onChange('address', e.target.value)}
        />
        {errors.address && <span className="error">{errors.address}</span>}
      </div>
      <div className="form-group">
        <label>City *</label>
        <input
          value={data.city || ''}
          onChange={(e) => onChange('city', e.target.value)}
        />
        {errors.city && <span className="error">{errors.city}</span>}
      </div>
      <div className="form-group">
        <label>ZIP Code *</label>
        <input
          value={data.zipCode || ''}
          onChange={(e) => onChange('zipCode', e.target.value)}
        />
        {errors.zipCode && <span className="error">{errors.zipCode}</span>}
      </div>
    </div>
  );
}

function StepReview({ data }) {
  return (
    <div>
      <h3>Review Your Information</h3>
      <div className="review-section">
        <h4>Personal</h4>
        <p>Name: {data.firstName} {data.lastName}</p>
        
        <h4>Contact</h4>
        <p>Email: {data.email}</p>
        <p>Phone: {data.phone}</p>
        
        <h4>Address</h4>
        <p>{data.address}</p>
        <p>{data.city}, {data.zipCode}</p>
      </div>
    </div>
  );
}

export default ValidatedMultiStepForm;
```

### Saving Progress with Local Storage
Persisting form data to local storage allows users to resume later.

```jsx
// File: src/hooks/usePersistedForm.js

import { useState, useEffect, useCallback } from 'react';

/**
 * Custom hook for persisting form data to localStorage
 */
function usePersistedForm(key, initialValue) {
  // Get stored value or use initial value
  const [storedValue, setStoredValue] = useState(() => {
    try {
      const item = window.localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      console.error('Error reading localStorage:', error);
      return initialValue;
    }
  });

  // Save to localStorage when value changes
  useEffect(() => {
    try {
      window.localStorage.setItem(key, JSON.stringify(storedValue));
    } catch (error) {
      console.error('Error saving to localStorage:', error);
    }
  }, [key, storedValue]);

  // Clear stored value
  const clearStorage = useCallback(() => {
    try {
      window.localStorage.removeItem(key);
      setStoredValue(initialValue);
    } catch (error) {
      console.error('Error clearing localStorage:', error);
    }
  }, [key, initialValue]);

  return [storedValue, setStoredValue, clearStorage];
}

export default usePersistedForm;

// File: src/components/PersistedMultiStepForm.jsx

import React, { useState, useEffect } from 'react';
import usePersistedForm from '../hooks/usePersistedForm';

const STORAGE_KEY = 'registration_form_progress';

function PersistedMultiStepForm() {
  const [formData, setFormData, clearFormData] = usePersistedForm(
    STORAGE_KEY, 
    { step: 0, data: {} }
  );
  
  const [isSubmitted, setIsSubmitted] = useState(false);

  const currentStep = formData.step;
  const data = formData.data;

  const updateFormData = (updates) => {
    setFormData(prev => ({
      ...prev,
      data: { ...prev.data, ...updates },
    }));
  };

  const goToStep = (step) => {
    setFormData(prev => ({ ...prev, step }));
  };

  const handleSubmit = async () => {
    // Simulate submission
    await new Promise(resolve => setTimeout(resolve, 1000));
    console.log('Submitted:', data);
    
    // Clear stored data after successful submission
    clearFormData();
    setIsSubmitted(true);
  };

  if (isSubmitted) {
    return <div>Thank you!</div>;
  }

  return (
    <div>
      {/* Form steps */}
      <div>Step {currentStep + 1}</div>
      
      {/* Progress */}
      <button onClick={() => goToStep(0)}>Step 1</button>
      <button onClick={() => goToStep(1)}>Step 2</button>
      <button onClick={() => goToStep(2)}>Step 3</button>
      
      {/* Clear saved progress */}
      <button onClick={clearFormData}>Clear Saved Progress</button>
    </div>
  );
}

export default PersistedMultiStepForm;
```

## Common Mistakes

### Mistake 1: Not Preserving Data When Navigating
Always ensure form data persists when going back and forth between steps.

```jsx
// ❌ WRONG — Data lost when going back
function BadForm() {
  const [data, setData] = useState({ step1: '' });
  
  const handleBack = () => {
    setData({}); // Lost all data!
  };
}

// ✅ CORRECT — Preserve all data
function GoodForm() {
  const [data, setData] = useState({ step1: '', step2: '' });
  
  const handleBack = () => {
    // Data preserved in state
  };
}
```

### Mistake 2: Not Validating Before Proceeding
Users should not be able to proceed to the next step with invalid data.

```jsx
// ❌ WRONG — No validation before next step
const handleNext = () => {
  setStep(prev => prev + 1); // Can proceed with invalid data!
};

// ✅ CORRECT — Validate before proceeding
const handleNext = () => {
  const errors = validateStep(currentStep, data);
  if (Object.keys(errors).length > 0) {
    setErrors(errors);
    return;
  }
  setStep(prev => prev + 1);
};
```

### Mistake 3: Not Saving Progress
Long forms should save progress automatically.

```jsx
// ❌ WRONG — No persistence
const handleChange = (field, value) => {
  setData(prev => ({ ...prev, [field]: value }));
  // Lost on refresh!
};

// ✅ CORRECT — Persist to localStorage
const handleChange = (field, value) => {
  const newData = { ...data, [field]: value };
  setData(newData);
  localStorage.setItem('form', JSON.stringify(newData));
};
```

## Real-World Example
Building a complete registration wizard with all best practices.

```jsx
// File: src/components/CompleteWizardForm.jsx

import React, { useState, useEffect, useCallback } from 'react';

// Custom hook for form state with persistence
function useWizardForm(steps) {
  const [currentStep, setCurrentStep] = useState(0);
  const [formData, setFormData] = useState({});
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isComplete, setIsComplete] = useState(false);

  // Persist progress
  useEffect(() => {
    const saved = localStorage.getItem('wizard_form');
    if (saved) {
      const { step, data } = JSON.parse(saved);
      setCurrentStep(step);
      setFormData(data);
    }
  }, []);

  // Save progress on change
  useEffect(() => {
    localStorage.setItem('wizard_form', JSON.stringify({
      step: currentStep,
      data: formData,
    }));
  }, [currentStep, formData]);

  const updateField = useCallback((field, value) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    if (errors[field]) {
      setErrors(prev => {
        const next = { ...prev };
        delete next[field];
        return next;
      });
    }
  }, [errors]);

  const validateCurrentStep = useCallback(() => {
    const validators = steps[currentStep].validate;
    if (!validators) return true;
    
    const stepErrors = {};
    Object.entries(validators).forEach(([field, validator]) => {
      const error = validator(formData[field], formData);
      if (error) stepErrors[field] = error;
    });
    
    setErrors(stepErrors);
    return Object.keys(stepErrors).length === 0;
  }, [currentStep, steps, formData]);

  const nextStep = useCallback(() => {
    if (validateCurrentStep()) {
      setCurrentStep(prev => Math.min(prev + 1, steps.length - 1));
    }
  }, [validateCurrentStep, steps.length]);

  const prevStep = useCallback(() => {
    setCurrentStep(prev => Math.max(prev - 1, 0));
  }, []);

  const goToStep = useCallback((step) => {
    if (step < currentStep || validateCurrentStep()) {
      setCurrentStep(step);
    }
  }, [currentStep, validateCurrentStep]);

  const submit = useCallback(async () => {
    if (!validateCurrentStep()) return;
    
    setIsSubmitting(true);
    try {
      await steps[steps.length - 1].onSubmit(formData);
      localStorage.removeItem('wizard_form');
      setIsComplete(true);
    } finally {
      setIsSubmitting(false);
    }
  }, [validateCurrentStep, formData, steps]);

  const reset = useCallback(() => {
    setCurrentStep(0);
    setFormData({});
    setErrors({});
    localStorage.removeItem('wizard_form');
  }, []);

  return {
    currentStep,
    formData,
    errors,
    isSubmitting,
    isComplete,
    updateField,
    nextStep,
    prevStep,
    goToStep,
    submit,
    reset,
    totalSteps: steps.length,
    isFirstStep: currentStep === 0,
    isLastStep: currentStep === steps.length - 1,
  };
}

// Complete wizard implementation
function CompleteWizardForm() {
  const steps = [
    {
      title: 'Account',
      validate: {
        username: (v) => !v?.trim() ? 'Username required' : 
                    v.length < 3 ? 'Min 3 characters' : null,
        email: (v) => !v?.trim() ? 'Email required' : 
                !/^\S+@\S+\.\S+$/.test(v) ? 'Invalid email' : null,
      },
    },
    {
      title: 'Profile',
      validate: {
        name: (v) => !v?.trim() ? 'Name required' : null,
        bio: (v) => v?.length > 500 ? 'Max 500 characters' : null,
      },
    },
    {
      title: 'Review',
      validate: {},
      onSubmit: async (data) => {
        await new Promise(r => setTimeout(r, 1500));
        console.log('Submitted:', data);
      },
    },
  ];

  const {
    currentStep,
    formData,
    errors,
    isSubmitting,
    isComplete,
    updateField,
    nextStep,
    prevStep,
    submit,
    reset,
    totalSteps,
    isFirstStep,
    isLastStep,
  } = useWizardForm(steps);

  if (isComplete) {
    return (
      <div className="wizard-complete">
        <h2>✓ Registration Complete!</h2>
        <p>Welcome, {formData.username}!</p>
        <button onClick={reset}>Start New Registration</button>
      </div>
    );
  }

  const step = steps[currentStep];

  return (
    <div className="wizard">
      {/* Progress indicator */}
      <div className="wizard-progress">
        {steps.map((s, i) => (
          <button
            key={i}
            className={`progress-step ${i === currentStep ? 'current' : ''} ${i < currentStep ? 'completed' : ''}`}
            onClick={() => i < currentStep && prevStep()}
          >
            {i + 1}. {s.title}
          </button>
        ))}
      </div>

      {/* Current step */}
      <div className="wizard-step">
        <h3>{step.title}</h3>
        
        {currentStep === 0 && (
          <div>
            <input
              placeholder="Username"
              value={formData.username || ''}
              onChange={(e) => updateField('username', e.target.value)}
            />
            {errors.username && <span className="error">{errors.username}</span>}
            
            <input
              placeholder="Email"
              value={formData.email || ''}
              onChange={(e) => updateField('email', e.target.value)}
            />
            {errors.email && <span className="error">{errors.email}</span>}
          </div>
        )}

        {currentStep === 1 && (
          <div>
            <input
              placeholder="Full Name"
              value={formData.name || ''}
              onChange={(e) => updateField('name', e.target.value)}
            />
            {errors.name && <span className="error">{errors.name}</span>}
            
            <textarea
              placeholder="Bio (optional)"
              value={formData.bio || ''}
              onChange={(e) => updateField('bio', e.target.value)}
            />
            {errors.bio && <span className="error">{errors.bio}</span>}
          </div>
        )}

        {currentStep === 2 && (
          <div className="review">
            <p><strong>Username:</strong> {formData.username}</p>
            <p><strong>Email:</strong> {formData.email}</p>
            <p><strong>Name:</strong> {formData.name}</p>
            <p><strong>Bio:</strong> {formData.bio || '(none)'}</p>
          </div>
        )}
      </div>

      {/* Navigation */}
      <div className="wizard-nav">
        {!isFirstStep && (
          <button onClick={prevStep} disabled={isSubmitting}>
            Back
          </button>
        )}
        
        {isLastStep ? (
          <button onClick={submit} disabled={isSubmitting}>
            {isSubmitting ? 'Submitting...' : 'Complete'}
          </button>
        ) : (
          <button onClick={nextStep}>
            Next
          </button>
        )}
      </div>
    </div>
  );
}

export default CompleteWizardForm;
```

## Key Takeaways
- Break complex forms into logical steps to reduce cognitive load
- Use useReducer for complex form state management
- Validate each step before allowing progression to the next
- Save form progress to localStorage for long forms
- Always allow users to go back and edit previous steps
- Show clear progress indicators so users know where they are
- Clear saved data after successful submission

## What's Next
Continue to [React Hook Form Setup](../02-react-hook-form/01-react-hook-form-setup.md) to learn about using React Hook Form, a popular library that simplifies form handling with less code.
