# Typing Events and Handlers

## Overview
React provides synthetic event wrappers around native browser events, and properly typing these events is essential for type-safe event handlers. When you handle form submissions, input changes, button clicks, or drag operations, TypeScript needs to know exactly what type of event you're handling. This guide covers typing all common React event types, from simple click events to complex form events with proper type inference.

## Prerequisites
- Understanding of React hooks
- Basic TypeScript knowledge
- Familiarity with DOM event types

## Core Concepts

### Common Event Types
React provides type definitions for all DOM events. Here are the most commonly used ones:

```typescript
// [File: src/examples/eventTypes.ts]

// ======== Mouse Events ========
// React.MouseEvent - triggered by mouse clicks, hover, etc.
// Used with: onClick, onDoubleClick, onMouseEnter, onMouseLeave, etc.

type MouseEventHandler = (event: React.MouseEvent<HTMLButtonElement>) => void;

const handleButtonClick: MouseEventHandler = (event) => {
  // event is React.MouseEvent<HTMLButtonElement>
  console.log(event.currentTarget); // The button element
  console.log(event.clientX, event.clientY); // Mouse position
  console.log(event.button); // Which button was clicked (0 = left)
  console.log(event.shiftKey); // Was shift key pressed?
  
  // Prevent default behavior
  event.preventDefault();
  
  // Stop propagation to parent elements
  event.stopPropagation();
};

// ======== Change Events ========
// React.ChangeEvent - triggered when input/select/textarea values change
// Used with: onChange

type ChangeEventHandler = (event: React.ChangeEvent<HTMLInputElement>) => void;

const handleInputChange: ChangeEventHandler = (event) => {
  // event is React.ChangeEvent<HTMLInputElement>
  console.log(event.currentTarget.value); // The input's current value
  console.log(event.target.name); // Input's name attribute
  console.log(event.target.type); // Input's type attribute
  
  // Type assertion if you need the exact type
  const value = (event.target as HTMLInputElement).value;
};

// For select elements
type SelectChangeEvent = React.ChangeEvent<HTMLSelectElement>;

const handleSelectChange: SelectChangeEvent = (event) => {
  console.log(event.currentTarget.value); // Selected option's value
  console.log(event.currentTarget.options); // All options
};

// For textareas
type TextareaChangeEvent = React.ChangeEvent<HTMLTextAreaElement>;

const handleTextareaChange: TextareaChangeEvent = (event) => {
  console.log(event.currentTarget.value); // Textarea content
};

// ======== Form Events ========
// React.FormEvent - triggered on form submission
// Used with: onSubmit

type FormEventHandler = (event: React.FormEvent<HTMLFormElement>) => void;

const handleFormSubmit: FormEventHandler = (event) => {
  // Prevent form from submitting to server and reloading
  event.preventDefault();
  
  // Access form data through FormData
  const formData = new FormData(event.currentTarget);
  const data = Object.fromEntries(formData);
  console.log('Form data:', data);
  
  // Or access individual elements
  const nameInput = event.currentTarget.elements.namedItem('name') as HTMLInputElement;
  console.log('Name:', nameInput.value);
};

// ======== Focus Events ========
// React.FocusEvent - triggered when elements gain or lose focus
// Used with: onFocus, onBlur

type FocusEventHandler = (event: React.FocusEvent<HTMLInputElement>) => void;

const handleFocus: FocusEventHandler = (event) => {
  console.log('Focused:', event.currentTarget.name);
  // event.relatedTarget is the element losing focus (onBlur)
};

const handleBlur: FocusEventHandler = (event) => {
  console.log('Blurred:', event.currentTarget.name);
  console.log('Focus moving to:', event.relatedTarget);
};

// ======== Keyboard Events ========
// React.KeyboardEvent - triggered by keyboard input
// Used with: onKeyDown, onKeyUp, onKeyPress (deprecated)

type KeyboardEventHandler = (event: React.KeyboardEvent<HTMLInputElement>) => void;

const handleKeyDown: KeyboardEventHandler = (event) => {
  console.log('Key pressed:', event.key);
  console.log('Key code:', event.code);
  console.log('Shift:', event.shiftKey);
  console.log('Ctrl:', event.ctrlKey);
  console.log('Alt:', event.altKey);
  
  // Common patterns
  if (event.key === 'Enter') {
    console.log('Enter pressed - submit form');
  }
  
  if (event.key === 'Escape') {
    console.log('Escape pressed - close modal');
  }
  
  // Prevent default for certain keys
  if (event.key === 'Tab') {
    // event.preventDefault(); // Uncomment to prevent tab
  }
};

// ======== Clipboard Events ========
// React.ClipboardEvent - triggered by copy/paste operations

type ClipboardEventHandler = (event: React.ClipboardEvent<HTMLInputElement>) => void;

const handlePaste: ClipboardEventHandler = (event) => {
  const clipboardData = event.clipboardData;
  console.log('Pasted text:', clipboardData.getData('text'));
  
  // Prevent paste of certain content
  // event.preventDefault();
};
```

### Typing Form Components
Here's how to properly type complete form components:

```typescript
// [File: src/components/TypedForm.tsx]
import React from 'react';

// ======== Typed Input Component ========

interface TextInputProps {
  name: string;
  label: string;
  value: string;
  onChange: (name: string, value: string) => void;
  type?: 'text' | 'email' | 'password' | 'number';
  placeholder?: string;
  error?: string;
  required?: boolean;
  disabled?: boolean;
}

function TextInput({
  name,
  label,
  value,
  onChange,
  type = 'text',
  placeholder,
  error,
  required,
  disabled,
}: TextInputProps) {
  // Properly typed change handler
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    onChange(name, e.target.value);
  };
  
  // Properly typed blur handler for validation
  const handleBlur = (e: React.FocusEvent<HTMLInputElement>) => {
    // Could trigger validation on blur
    console.log('Blur:', name);
  };
  
  return (
    <div className="form-field">
      <label htmlFor={name}>
        {label}
        {required && <span className="required">*</span>}
      </label>
      <input
        id={name}
        name={name}
        type={type}
        value={value}
        onChange={handleChange}
        onBlur={handleBlur}
        placeholder={placeholder}
        disabled={disabled}
        aria-invalid={!!error}
        aria-describedby={error ? `${name}-error` : undefined}
      />
      {error && (
        <span id={`${name}-error`} className="error" role="alert">
          {error}
        </span>
      )}
    </div>
  );
}

// ======== Complete Login Form ========

interface LoginFormData {
  email: string;
  password: string;
  rememberMe: boolean;
}

interface LoginFormProps {
  onSubmit: (data: LoginFormData) => void;
  isLoading?: boolean;
}

function LoginForm({ onSubmit, isLoading = false }: LoginFormProps) {
  const [formData, setFormData] = React.useState<LoginFormData>({
    email: '',
    password: '',
    rememberMe: false,
  });
  
  const [errors, setErrors] = React.useState<Partial<Record<keyof LoginFormData, string>>>({});
  
  // Generic handler for any input field
  const handleChange = (field: keyof LoginFormData, value: string | boolean) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    // Clear error when user starts typing
    if (errors[field]) {
      setErrors(prev => ({ ...prev, [field]: undefined }));
    }
  };
  
  // Form submission
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    
    // Validate
    const newErrors: Partial<Record<keyof LoginFormData, string>> = {};
    
    if (!formData.email) {
      newErrors.email = 'Email is required';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Invalid email format';
    }
    
    if (!formData.password) {
      newErrors.password = 'Password is required';
    } else if (formData.password.length < 8) {
      newErrors.password = 'Password must be at least 8 characters';
    }
    
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }
    
    onSubmit(formData);
  };
  
  return (
    <form onSubmit={handleSubmit} className="login-form">
      <TextInput
        name="email"
        label="Email"
        type="email"
        value={formData.email}
        onChange={handleChange}
        error={errors.email}
        required
      />
      
      <TextInput
        name="password"
        label="Password"
        type="password"
        value={formData.password}
        onChange={handleChange}
        error={errors.password}
        required
      />
      
      <div className="form-field checkbox">
        <label>
          <input
            type="checkbox"
            name="rememberMe"
            checked={formData.rememberMe}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
              handleChange('rememberMe', e.target.checked);
            }}
          />
          Remember me
        </label>
      </div>
      
      <button type="submit" disabled={isLoading}>
        {isLoading ? 'Logging in...' : 'Login'}
      </button>
    </form>
  );
}
```

### Generic Event Handler Factory
For complex forms with many fields, create reusable handler factories:

```typescript
// [File: src/hooks/useFormHandlers.ts]

// Generic handler factory pattern
function createInputHandler<T>(
  setState: React.Dispatch<React.SetStateAction<T>>
) {
  return (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    const { name, value, type } = e.currentTarget;
    
    // Handle different input types
    let parsedValue: string | number | boolean = value;
    
    if (type === 'number') {
      parsedValue = parseFloat(value) || 0;
    } else if (type === 'checkbox') {
      parsedValue = (e.target as HTMLInputElement).checked;
    }
    
    setState((prev: T) => ({
      ...prev,
      [name]: parsedValue,
    }));
  };
}

// Usage in component
function ExampleForm() {
  const [state, setState] = React.useState({
    name: '',
    age: 0,
    subscribed: false,
  });
  
  // Create typed handler
  const handleChange = createInputHandler(setState);
  
  return (
    <form>
      <input
        name="name"
        value={state.name}
        onChange={handleChange}
      />
      <input
        name="age"
        type="number"
        value={state.age}
        onChange={handleChange}
      />
      <input
        name="subscribed"
        type="checkbox"
        checked={state.subscribed}
        onChange={handleChange}
      />
    </form>
  );
}
```

## Common Mistakes

### Mistake 1: Not Typing Event Parameters
```typescript
// ❌ WRONG - Event is implicitly any
const handleChange = (e) => {
  console.log(e.target.value); // No type safety!
};

// ✅ CORRECT - Type the event
const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  console.log(e.target.value);
};
```

### Mistake 2: Wrong Element Type
```typescript
// ❌ WRONG - Using HTMLButtonElement for input
const handleChange = (e: React.ChangeEvent<HTMLButtonElement>) => {
  // Error: Property 'value' does not exist on HTMLButtonElement
};

// ✅ CORRECT - Use correct element type
const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  console.log(e.target.value); // Works!
};
```

### Mistake 3: Forgetting to Prevent Default
```typescript
// ❌ WRONG - Form submits and page reloads
const handleSubmit = (e: React.FormEvent) => {
  console.log('Submitting...');
  // Missing e.preventDefault()!
};

// ✅ CORRECT - Prevent default form submission
const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
  e.preventDefault();
  console.log('Submitting...');
};
```

## Real-World Example

Complete typed form with all event handlers:

```typescript
// [File: src/components/RegistrationForm.tsx]
import React from 'react';

// ======== Form Types ========

interface RegistrationData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  bio: string;
  country: string;
  agreeTerms: boolean;
  subscribeNewsletter: boolean;
}

interface FormErrors {
  firstName?: string;
  lastName?: string;
  email?: string;
  password?: string;
  confirmPassword?: string;
  bio?: string;
  country?: string;
  agreeTerms?: string;
}

// ======== Component ========

function RegistrationForm() {
  const [data, setData] = React.useState<RegistrationData>({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    bio: '',
    country: '',
    agreeTerms: false,
    subscribeNewsletter: true,
  });
  
  const [errors, setErrors] = React.useState<FormErrors>({});
  const [touched, setTouched] = React.useState<Set<string>>(new Set());
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  
  // ======== Event Handlers ========
  
  // Handle text input changes
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    const { name, value, type } = e.currentTarget;
    
    setData(prev => ({
      ...prev,
      [name]: type === 'checkbox' 
        ? (e.target as HTMLInputElement).checked 
        : value,
    }));
    
    // Clear error on change
    if (errors[name as keyof FormErrors]) {
      setErrors(prev => ({ ...prev, [name]: undefined }));
    }
  };
  
  // Handle blur for validation
  const handleBlur = (e: React.FocusEvent<HTMLInputElement>) => {
    const { name } = e.currentTarget;
    setTouched(prev => new Set(prev).add(name));
    validateField(name as keyof RegistrationData);
  };
  
  // Handle checkbox changes
  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.currentTarget;
    setData(prev => ({ ...prev, [name]: checked }));
  };
  
  // Handle form submission
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    
    // Mark all fields as touched
    setTouched(new Set(Object.keys(data)));
    
    // Validate all fields
    const isValid = validateForm();
    
    if (!isValid) {
      console.log('Validation failed');
      return;
    }
    
    setIsSubmitting(true);
    
    // Simulate API call
    setTimeout(() => {
      console.log('Submitting:', data);
      setIsSubmitting(false);
    }, 1000);
  };
  
  // ======== Validation ========
  
  const validateField = (name: keyof RegistrationData) => {
    const newErrors = { ...errors };
    
    switch (name) {
      case 'firstName':
        if (!data.firstName) {
          newErrors.firstName = 'First name is required';
        } else if (data.firstName.length < 2) {
          newErrors.firstName = 'Must be at least 2 characters';
        }
        break;
        
      case 'lastName':
        if (!data.lastName) {
          newErrors.lastName = 'Last name is required';
        }
        break;
        
      case 'email':
        if (!data.email) {
          newErrors.email = 'Email is required';
        } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.email)) {
          newErrors.email = 'Invalid email format';
        }
        break;
        
      case 'password':
        if (!data.password) {
          newErrors.password = 'Password is required';
        } else if (data.password.length < 8) {
          newErrors.password = 'Must be at least 8 characters';
        } else if (!/[A-Z]/.test(data.password)) {
          newErrors.password = 'Must contain uppercase letter';
        }
        break;
        
      case 'confirmPassword':
        if (data.confirmPassword !== data.password) {
          newErrors.confirmPassword = 'Passwords do not match';
        }
        break;
        
      case 'country':
        if (!data.country) {
          newErrors.country = 'Please select a country';
        }
        break;
        
      case 'agreeTerms':
        if (!data.agreeTerms) {
          newErrors.agreeTerms = 'You must agree to the terms';
        }
        break;
    }
    
    setErrors(newErrors);
    return !newErrors[name];
  };
  
  const validateForm = () => {
    let isValid = true;
    const fields: (keyof RegistrationData)[] = [
      'firstName', 'lastName', 'email', 'password', 
      'confirmPassword', 'country', 'agreeTerms'
    ];
    
    fields.forEach(field => {
      if (!validateField(field)) {
        isValid = false;
      }
    });
    
    return isValid;
  };
  
  // ======== Helpers ========
  
  const getFieldError = (name: keyof RegistrationData): string | undefined => {
    return touched.has(name) ? errors[name] : undefined;
  };
  
  return (
    <form onSubmit={handleSubmit} className="registration-form">
      <div className="form-row">
        <div className="form-field">
          <label htmlFor="firstName">First Name *</label>
          <input
            id="firstName"
            name="firstName"
            type="text"
            value={data.firstName}
            onChange={handleChange}
            onBlur={handleBlur}
            aria-invalid={!!getFieldError('firstName')}
            aria-describedby={getFieldError('firstName') ? 'firstName-error' : undefined}
          />
          {getFieldError('firstName') && (
            <span id="firstName-error" className="error">
              {getFieldError('firstName')}
            </span>
          )}
        </div>
        
        <div className="form-field">
          <label htmlFor="lastName">Last Name *</label>
          <input
            id="lastName"
            name="lastName"
            type="text"
            value={data.lastName}
            onChange={handleChange}
            onBlur={handleBlur}
            aria-invalid={!!getFieldError('lastName')}
          />
          {getFieldError('lastName') && (
            <span id="lastName-error" className="error">
              {getFieldError('lastName')}
            </span>
          )}
        </div>
      </div>
      
      <div className="form-field">
        <label htmlFor="email">Email *</label>
        <input
          id="email"
          name="email"
          type="email"
          value={data.email}
          onChange={handleChange}
          onBlur={handleBlur}
          autoComplete="email"
        />
        {getFieldError('email') && (
          <span className="error">{getFieldError('email')}</span>
        )}
      </div>
      
      <div className="form-field">
        <label htmlFor="password">Password *</label>
        <input
          id="password"
          name="password"
          type="password"
          value={data.password}
          onChange={handleChange}
          onBlur={handleBlur}
          autoComplete="new-password"
        />
        {getFieldError('password') && (
          <span className="error">{getFieldError('password')}</span>
        )}
      </div>
      
      <div className="form-field">
        <label htmlFor="confirmPassword">Confirm Password *</label>
        <input
          id="confirmPassword"
          name="confirmPassword"
          type="password"
          value={data.confirmPassword}
          onChange={handleChange}
          onBlur={handleBlur}
          autoComplete="new-password"
        />
        {getFieldError('confirmPassword') && (
          <span className="error">{getFieldError('confirmPassword')}</span>
        )}
      </div>
      
      <div className="form-field">
        <label htmlFor="bio">Bio</label>
        <textarea
          id="bio"
          name="bio"
          value={data.bio}
          onChange={handleChange}
          rows={4}
          maxLength={500}
          placeholder="Tell us about yourself..."
        />
        <span className="char-count">{data.bio.length}/500</span>
      </div>
      
      <div className="form-field">
        <label htmlFor="country">Country *</label>
        <select
          id="country"
          name="country"
          value={data.country}
          onChange={handleChange}
          onBlur={handleBlur}
        >
          <option value="">Select a country</option>
          <option value="us">United States</option>
          <option value="uk">United Kingdom</option>
          <option value="ca">Canada</option>
        </select>
        {getFieldError('country') && (
          <span className="error">{getFieldError('country')}</span>
        )}
      </div>
      
      <div className="form-field checkbox">
        <label>
          <input
            name="agreeTerms"
            type="checkbox"
            checked={data.agreeTerms}
            onChange={handleCheckboxChange}
            onBlur={handleBlur}
          />
          I agree to the Terms and Conditions *
        </label>
        {getFieldError('agreeTerms') && (
          <span className="error">{getFieldError('agreeTerms')}</span>
        )}
      </div>
      
      <div className="form-field checkbox">
        <label>
          <input
            name="subscribeNewsletter"
            type="checkbox"
            checked={data.subscribeNewsletter}
            onChange={handleCheckboxChange}
          />
          Subscribe to newsletter
        </label>
      </div>
      
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Creating Account...' : 'Create Account'}
      </button>
    </form>
  );
}

export default RegistrationForm;
```

## Key Takeaways
- Always type event handlers with specific React event types
- Use `React.ChangeEvent<T>` for input changes, `React.MouseEvent<T>` for clicks
- Use `React.FormEvent<T>` for form submissions
- Use `React.FocusEvent<T>` for focus/blur events
- Use `React.KeyboardEvent<T>` for keyboard input
- Always specify the element type in the generic (HTMLInputElement, HTMLButtonElement, etc.)
- Use `e.preventDefault()` to prevent default form submission and link navigation
- Use `e.stopPropagation()` to prevent event bubbling
- Create reusable handler factories for complex forms with many fields

## What's Next
Continue to [Typing Context API](04-typing-context-api.md) to learn how to properly type React Context with generics for global state management.