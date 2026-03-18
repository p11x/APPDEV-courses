# Building Controlled Forms in React

## Overview
Controlled forms are a fundamental pattern in React where form input elements are bound to component state. Every change to the input updates the state, and the input value is always derived from that state. This gives you complete control over form data, enables real-time validation, and makes it easy to implement complex form interactions.

## Prerequisites
- Basic understanding of React hooks (useState, useEffect)
- Familiarity with HTML form elements
- Knowledge of JavaScript event handling
- Understanding of component props and state

## Core Concepts

### Understanding Controlled Components
In a controlled component, the form element's value is managed by React state. The component "controls" the input, and all changes flow through state.

```jsx
// File: src/components/SimpleForm.jsx

import React, { useState } from 'react';

function SimpleForm() {
  // Single state object for all form fields
  // Using an object makes it easier to scale when adding more fields
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
  });

  // Handler for input changes
  // Uses the name attribute to identify which field changed
  const handleChange = (event) => {
    const { name, value } = event.target;
    
    // Update only the changed field while preserving others
    setFormData(prev => ({
      ...prev,
      [name]: value, // Computed property name for dynamic field updates
    }));
  };

  const handleSubmit = (event) => {
    event.preventDefault(); // Prevent default form submission (page reload)
    
    // Form data is already in state - ready to send to API
    console.log('Form submitted:', formData);
    
    // Reset form after submission
    setFormData({ username: '', email: '', password: '' });
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label htmlFor="username">Username:</label>
        <input
          id="username"
          type="text"
          name="username" // Matches state key
          value={formData.username} // Controlled by state
          onChange={handleChange}   // Updates state on change
        />
      </div>

      <div>
        <label htmlFor="email">Email:</label>
        <input
          id="email"
          type="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
        />
      </div>

      <div>
        <label htmlFor="password">Password:</label>
        <input
          id="password"
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
        />
      </div>

      <button type="submit">Submit</button>
    </form>
  );
}

export default SimpleForm;
```

### Handling Different Input Types
React can handle various form input types including text, checkboxes, radio buttons, selects, and textareas.

```jsx
// File: src/components/AllInputTypes.jsx

import React, { useState } from 'react';

function AllInputTypes() {
  const [formData, setFormData] = useState({
    // Text inputs
    username: '',
    email: '',
    bio: '',
    
    // Single choice
    gender: '',
    
    // Multiple choice
    interests: [],
    
    // Boolean
    newsletter: false,
    
    // Single select
    country: '',
    
    // Multiple select
    skills: [],
  });

  const handleChange = (event) => {
    const { name, value, type, checked } = event.target;

    if (type === 'checkbox') {
      if (name === 'newsletter') {
        // Single checkbox
        setFormData(prev => ({
          ...prev,
          [name]: checked,
        }));
      } else {
        // Checkbox group (multi-select)
        setFormData(prev => ({
          ...prev,
          interests: checked
            ? [...prev.interests, value] // Add if checked
            : prev.interests.filter(item => item !== value), // Remove if unchecked
        }));
      }
    } else if (type === 'select-multiple') {
      // Multi-select handling
      const selectedOptions = Array.from(event.target.selectedOptions, option => option.value);
      setFormData(prev => ({
        ...prev,
        [name]: selectedOptions,
      }));
    } else {
      // Standard inputs (text, email, radio, select)
      setFormData(prev => ({
        ...prev,
        [name]: value,
      }));
    }
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    console.log('Form data:', formData);
  };

  return (
    <form onSubmit={handleSubmit}>
      {/* Text Input */}
      <div>
        <label htmlFor="username">Username</label>
        <input
          id="username"
          name="username"
          type="text"
          value={formData.username}
          onChange={handleChange}
        />
      </div>

      {/* Email Input */}
      <div>
        <label htmlFor="email">Email</label>
        <input
          id="email"
          name="email"
          type="email"
          value={formData.email}
          onChange={handleChange}
        />
      </div>

      {/* Textarea */}
      <div>
        <label htmlFor="bio">Bio</label>
        <textarea
          id="bio"
          name="bio"
          value={formData.bio}
          onChange={handleChange}
          rows={4}
        />
      </div>

      {/* Radio Buttons */}
      <fieldset>
        <legend>Gender</legend>
        <label>
          <input
            type="radio"
            name="gender"
            value="male"
            checked={formData.gender === 'male'}
            onChange={handleChange}
          />
          Male
        </label>
        <label>
          <input
            type="radio"
            name="gender"
            value="female"
            checked={formData.gender === 'female'}
            onChange={handleChange}
          />
          Female
        </label>
        <label>
          <input
            type="radio"
            name="gender"
            value="other"
            checked={formData.gender === 'other'}
            onChange={handleChange}
          />
          Other
        </label>
      </fieldset>

      {/* Checkboxes */}
      <fieldset>
        <legend>Interests</legend>
        {['coding', 'design', 'music', 'sports'].map(interest => (
          <label key={interest}>
            <input
              type="checkbox"
              name="interests"
              value={interest}
              checked={formData.interests.includes(interest)}
              onChange={handleChange}
            />
            {interest.charAt(0).toUpperCase() + interest.slice(1)}
          </label>
        ))}
      </fieldset>

      {/* Single Checkbox */}
      <label>
        <input
          type="checkbox"
          name="newsletter"
          checked={formData.newsletter}
          onChange={handleChange}
        />
        Subscribe to newsletter
      </label>

      {/* Select Dropdown */}
      <div>
        <label htmlFor="country">Country</label>
        <select
          id="country"
          name="country"
          value={formData.country}
          onChange={handleChange}
        >
          <option value="">Select a country</option>
          <option value="us">United States</option>
          <option value="uk">United Kingdom</option>
          <option value="ca">Canada</option>
          <option value="au">Australia</option>
        </select>
      </div>

      {/* Multi-select */}
      <div>
        <label htmlFor="skills">Skills (hold ctrl/cmd to select multiple)</label>
        <select
          id="skills"
          name="skills"
          value={formData.skills}
          onChange={handleChange}
          multiple
        >
          <option value="javascript">JavaScript</option>
          <option value="python">Python</option>
          <option value="java">Java</option>
          <option value="csharp">C#</option>
        </select>
      </div>

      <button type="submit">Submit</button>
    </form>
  );
}

export default AllInputTypes;
```

### Extracting Reusable Form Logic
Creating custom hooks for form handling reduces boilerplate and makes forms easier to manage.

```jsx
// File: src/hooks/useForm.js

import { useState, useCallback } from 'react';

/**
 * Custom hook for managing form state
 * @param {object} initialValues - Initial form values
 * @returns {object} - Form state and handlers
 */
function useForm(initialValues = {}) {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});

  // Generic handler for all input changes
  const handleChange = useCallback((event) => {
    const { name, value, type, checked } = event.target;
    
    setValues(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));
    
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  }, [errors]);

  // Handler for blur event (when field loses focus)
  const handleBlur = useCallback((event) => {
    const { name } = event.target;
    setTouched(prev => ({ ...prev, [name]: true }));
  }, []);

  // Reset form to initial values
  const reset = useCallback(() => {
    setValues(initialValues);
    setErrors({});
    setTouched({});
  }, [initialValues]);

  // Set a specific field value programmatically
  const setFieldValue = useCallback((name, value) => {
    setValues(prev => ({ ...prev, [name]: value }));
  }, []);

  // Set error for a specific field
  const setFieldError = useCallback((name, error) => {
    setErrors(prev => ({ ...prev, [name]: error }));
  }, []);

  return {
    values,
    errors,
    touched,
    handleChange,
    handleBlur,
    reset,
    setFieldValue,
    setFieldError,
    setValues,
    setErrors,
    setTouched,
  };
}

export default useForm;

// File: src/components/FormWithHook.jsx

import React from 'react';
import useForm from '../hooks/useForm';

function SignUpForm() {
  const initialValues = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  };

  const {
    values,
    errors,
    touched,
    handleChange,
    handleBlur,
    reset,
  } = useForm(initialValues);

  const validate = () => {
    const newErrors = {};
    
    if (!values.username) {
      newErrors.username = 'Username is required';
    } else if (values.username.length < 3) {
      newErrors.username = 'Username must be at least 3 characters';
    }
    
    if (!values.email) {
      newErrors.email = 'Email is required';
    } else if (!/\S+@\S+\.\S+/.test(values.email)) {
      newErrors.email = 'Email is invalid';
    }
    
    if (!values.password) {
      newErrors.password = 'Password is required';
    } else if (values.password.length < 8) {
      newErrors.password = 'Password must be at least 8 characters';
    }
    
    if (values.password !== values.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }
    
    return newErrors;
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    
    const validationErrors = validate();
    setErrors(validationErrors);
    
    if (Object.keys(validationErrors).length === 0) {
      console.log('Form submitted:', values);
      reset();
    }
  };

  const getInputProps = (name) => ({
    name,
    value: values[name],
    onChange: handleChange,
    onBlur: handleBlur,
    error: touched[name] && errors[name],
  });

  return (
    <form onSubmit={handleSubmit} noValidate>
      <div>
        <label htmlFor="username">Username</label>
        <input
          {...getInputProps('username')}
          type="text"
          id="username"
        />
        {touched.username && errors.username && (
          <span className="error">{errors.username}</span>
        )}
      </div>

      <div>
        <label htmlFor="email">Email</label>
        <input
          {...getInputProps('email')}
          type="email"
          id="email"
        />
        {touched.email && errors.email && (
          <span className="error">{errors.email}</span>
        )}
      </div>

      <div>
        <label htmlFor="password">Password</label>
        <input
          {...getInputProps('password')}
          type="password"
          id="password"
        />
        {touched.password && errors.password && (
          <span className="error">{errors.password}</span>
        )}
      </div>

      <div>
        <label htmlFor="confirmPassword">Confirm Password</label>
        <input
          {...getInputProps('confirmPassword')}
          type="password"
          id="confirmPassword"
        />
        {touched.confirmPassword && errors.confirmPassword && (
          <span className="error">{errors.confirmPassword}</span>
        )}
      </div>

      <button type="submit">Sign Up</button>
      <button type="button" onClick={reset}>Reset</button>
    </form>
  );
}

export default SignUpForm;
```

### Working with File Inputs
File inputs require special handling since they can't be controlled in the traditional sense.

```jsx
// File: src/components/FileUploadForm.jsx

import React, { useState, useRef } from 'react';

function FileUploadForm() {
  const [files, setFiles] = useState([]);
  const [previews, setPreviews] = useState([]);
  const fileInputRef = useRef(null);

  const handleFileChange = (event) => {
    const selectedFiles = Array.from(event.target.files);
    
    // Store file objects
    setFiles(selectedFiles);
    
    // Generate preview URLs for images
    const newPreviews = selectedFiles.map(file => 
      URL.createObjectURL(file)
    );
    setPreviews(newPreviews);
  };

  const handleRemoveFile = (index) => {
    setFiles(prev => prev.filter((_, i) => i !== index));
    setPreviews(prev => {
      // Revoke URL to avoid memory leaks
      URL.revokeObjectURL(prev[index]);
      return prev.filter((_, i) => i !== index);
    });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    
    // Create FormData for file upload
    const formData = new FormData();
    
    files.forEach((file, index) => {
      formData.append(`file${index}`, file);
    });
    
    try {
      const response = await fetch('/api/upload', {
        method: 'POST',
        body: formData,
      });
      
      if (response.ok) {
        console.log('Files uploaded successfully');
        setFiles([]);
        setPreviews([]);
        if (fileInputRef.current) {
          fileInputRef.current.value = '';
        }
      }
    } catch (error) {
      console.error('Upload failed:', error);
    }
  };

  // Cleanup previews on unmount
  React.useEffect(() => {
    return () => {
      previews.forEach(url => URL.revokeObjectURL(url));
    };
  }, []);

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label htmlFor="file-upload">Choose files</label>
        <input
          id="file-upload"
          type="file"
          ref={fileInputRef}
          onChange={handleFileChange}
          multiple
          accept="image/*,.pdf,.doc,.docx"
        />
      </div>

      {previews.length > 0 && (
        <div className="previews">
          {previews.map((preview, index) => (
            <div key={index} className="preview">
              {files[index]?.type.startsWith('image/') ? (
                <img src={preview} alt={files[index].name} />
              ) : (
                <div className="file-icon">{files[index].name}</div>
              )}
              <button
                type="button"
                onClick={() => handleRemoveFile(index)}
              >
                Remove
              </button>
            </div>
          ))}
        </div>
      )}

      <button type="submit" disabled={files.length === 0}>
        Upload {files.length} file(s)
      </button>
    </form>
  );
}

export default FileUploadForm;
```

## Common Mistakes

### Mistake 1: Not Using Controlled Inputs
Letting React manage form state gives you control over validation and submission.

```jsx
// ❌ WRONG — Uncontrolled input (React doesn't control the value)
function BadForm() {
  return (
    <input type="text" defaultValue="hello" />
  );
}

// ✅ CORRECT — Controlled input (React manages the value)
function GoodForm() {
  const [value, setValue] = useState('');
  
  return (
    <input 
      type="text" 
      value={value}
      onChange={(e) => setValue(e.target.value)}
    />
  );
}
```

### Mistake 2: Not Resetting Form State
Always reset form state after successful submission.

```jsx
// ❌ WRONG — Form keeps old values after submit
function BadForm() {
  const [data, setData] = useState({ name: '' });
  
  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(data);
    // Form still shows old values!
  };
  
  return <form onSubmit={handleSubmit}><input value={data.name} onChange={e => setData({name: e.target.value})} /></form>;
}

// ✅ CORRECT — Reset after submit
function GoodForm() {
  const [data, setData] = useState({ name: '' });
  
  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(data);
    setData({ name: '' }); // Clear form
  };
  
  return <form onSubmit={handleSubmit}><input value={data.name} onChange={e => setData({name: e.target.value})} /></form>;
}
```

### Mistake 3: Not Handling Form Reset Properly
Use the reset function from useState or a custom handler to clear forms.

```jsx
// ❌ WRONG — Setting individual fields one by one
function BadForm() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  
  const reset = () => {
    setName('');
    setEmail('');
    // Tedious for many fields!
  };
}

// ✅ CORRECT — Use object state for easy reset
function GoodForm() {
  const initialValues = { name: '', email: '' };
  const [formData, setFormData] = useState(initialValues);
  
  const reset = () => {
    setFormData(initialValues); // Reset everything at once
  };
}
```

## Real-World Example
Building a complete contact form with validation and submission handling.

```jsx
// File: src/components/ContactForm.jsx

import React, { useState } from 'react';

function ContactForm() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    subject: '',
    message: '',
    agree: false,
  });

  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitStatus, setSubmitStatus] = useState(null); // 'success' | 'error' | null

  const validate = () => {
    const newErrors = {};

    if (!formData.name.trim()) {
      newErrors.name = 'Name is required';
    }

    if (!formData.email.trim()) {
      newErrors.email = 'Email is required';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = 'Please enter a valid email';
    }

    if (!formData.subject) {
      newErrors.subject = 'Please select a subject';
    }

    if (!formData.message.trim()) {
      newErrors.message = 'Message is required';
    } else if (formData.message.trim().length < 10) {
      newErrors.message = 'Message must be at least 10 characters';
    }

    if (!formData.agree) {
      newErrors.agree = 'You must agree to the terms';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleChange = (event) => {
    const { name, value, type, checked } = event.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));

    // Clear error when user starts correcting
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setSubmitStatus(null);

    if (!validate()) {
      return;
    }

    setIsSubmitting(true);

    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1500));

      console.log('Form submitted:', formData);
      setSubmitStatus('success');
      
      // Reset form
      setFormData({
        name: '',
        email: '',
        subject: '',
        message: '',
        agree: false,
      });
    } catch (error) {
      setSubmitStatus('error');
    } finally {
      setIsSubmitting(false);
    }
  };

  const getInputClassName = (fieldName) => {
    if (!errors[fieldName]) return '';
    return 'input-error';
  };

  return (
    <div className="contact-form-container">
      <h2>Contact Us</h2>

      {submitStatus === 'success' && (
        <div className="alert success">
          Thank you! Your message has been sent.
        </div>
      )}

      {submitStatus === 'error' && (
        <div className="alert error">
          Something went wrong. Please try again.
        </div>
      )}

      <form onSubmit={handleSubmit} noValidate>
        {/* Name Field */}
        <div className="form-group">
          <label htmlFor="name">Name *</label>
          <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            className={getInputClassName('name')}
            disabled={isSubmitting}
          />
          {errors.name && <span className="error-text">{errors.name}</span>}
        </div>

        {/* Email Field */}
        <div className="form-group">
          <label htmlFor="email">Email *</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            className={getInputClassName('email')}
            disabled={isSubmitting}
          />
          {errors.email && <span className="error-text">{errors.email}</span>}
        </div>

        {/* Subject Select */}
        <div className="form-group">
          <label htmlFor="subject">Subject *</label>
          <select
            id="subject"
            name="subject"
            value={formData.subject}
            onChange={handleChange}
            className={getInputClassName('subject')}
            disabled={isSubmitting}
          >
            <option value="">Select a subject</option>
            <option value="general">General Inquiry</option>
            <option value="support">Technical Support</option>
            <option value="sales">Sales Question</option>
            <option value="feedback">Feedback</option>
          </select>
          {errors.subject && <span className="error-text">{errors.subject}</span>}
        </div>

        {/* Message Textarea */}
        <div className="form-group">
          <label htmlFor="message">Message *</label>
          <textarea
            id="message"
            name="message"
            value={formData.message}
            onChange={handleChange}
            rows={5}
            className={getInputClassName('message')}
            disabled={isSubmitting}
          />
          {errors.message && <span className="error-text">{errors.message}</span>}
        </div>

        {/* Agreement Checkbox */}
        <div className="form-group checkbox">
          <label>
            <input
              type="checkbox"
              name="agree"
              checked={formData.agree}
              onChange={handleChange}
              disabled={isSubmitting}
            />
            I agree to the terms and conditions *
          </label>
          {errors.agree && <span className="error-text">{errors.agree}</span>}
        </div>

        {/* Submit Button */}
        <button 
          type="submit" 
          disabled={isSubmitting}
          className="submit-btn"
        >
          {isSubmitting ? (
            <>
              <span className="spinner"></span>
              Sending...
            </>
          ) : (
            'Send Message'
          )}
        </button>
      </form>
    </div>
  );
}

export default ContactForm;
```

## Key Takeaways
- Controlled forms bind input values to React state, giving you full control over data
- Use a single state object for all form fields to simplify management
- The name attribute on inputs should match state keys for easy handling
- Always handle the three states: default, loading (submitting), and success/error
- Extract form logic into custom hooks to reduce boilerplate
- File inputs require special handling since they can't be traditionally controlled
- Always reset form state after successful submission

## What's Next
Continue to [Form Validation From Scratch](02-form-validation-from-scratch.md) to learn how to implement comprehensive form validation without external libraries.
