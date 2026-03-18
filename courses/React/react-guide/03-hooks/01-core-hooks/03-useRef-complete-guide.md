# useRef Complete Guide

## Overview

useRef is a React hook that provides a way to access and manipulate DOM elements directly, and to persist mutable values across renders without causing re-renders. While useState triggers re-renders when values change, useRef allows you to store values that can change without triggering a re-render. This makes it essential for managing focus, integrating with third-party DOM libraries, and optimizing performance.

## Prerequisites

- Understanding of React components and JSX
- Knowledge of useState and useEffect hooks
- Familiarity with DOM manipulation concepts
- Understanding of React rendering behavior

## Core Concepts

### What is useRef?

useRef returns a mutable ref object whose .current property is initialized with the passed argument. The returned object will persist for the full lifetime of the component.

```jsx
// File: src/useref-basics.jsx

import React, { useRef } from 'react';

function BasicExample() {
  // Create a ref with initial value
  const inputRef = useRef(null);
  
  const handleClick = () => {
    // Access the DOM element through .current
    inputRef.current.focus();
    inputRef.current.value = 'Hello!';
  };
  
  return (
    <div>
      <input ref={inputRef} type="text" />
      <button onClick={handleClick}>Focus Input</button>
    </div>
  );
}
```

### Accessing DOM Elements

```jsx
// File: src/useref-dom.jsx

import React, { useRef, useEffect } from 'react';

function DOMAccess() {
  const inputRef = useRef(null);
  const divRef = useRef(null);
  const textareaRef = useRef(null);
  
  useEffect(() => {
    // Focus input on mount
    inputRef.current.focus();
  }, []);
  
  const scrollToDiv = () => {
    divRef.current.scrollIntoView({ behavior: 'smooth' });
  };
  
  const getDivInfo = () => {
    console.log('Div dimensions:', {
      width: divRef.current.offsetWidth,
      height: divRef.current.offsetHeight,
      position: divRef.current.getBoundingClientRect()
    });
  };
  
  return (
    <div>
      <input ref={inputRef} placeholder="I get focus!" />
      <button onClick={() => inputRef.current.focus()}>Focus</button>
      <button onClick={() => inputRef.current.blur()}>Blur</button>
      
      <div ref={divRef} style={{ marginTop: '1000px', height: '200px', backgroundColor: '#f0f0f0' }}>
        Scrolled element
      </div>
      <button onClick={scrollToDiv}>Scroll to Div</button>
      <button onClick={getDivInfo}>Get Info</button>
    </div>
  );
}
```

### Storing Mutable Values

```jsx
// File: src/useref-mutable.jsx

import React, { useState, useEffect, useRef } from 'react';

function MutableValues() {
  const [count, setCount] = useState(0);
  
  // Use ref to store value that changes but doesn't need to trigger render
  const renderCount = useRef(0);
  const previousCount = useRef(0);
  const isFirstRender = useRef(true);
  
  // Update refs after render
  useEffect(() => {
    renderCount.current += 1;
  });
  
  // Track previous value
  useEffect(() => {
    if (isFirstRender.current) {
      isFirstRender.current = false;
      return;
    }
    previousCount.current = count;
  }, [count]);
  
  return (
    <div>
      <p>Current count: {count}</p>
      <p>Previous count: {previousCount.current}</p>
      <p>Render count: {renderCount.current}</p>
      
      <button onClick={() => setCount(c => c + 1)}>Increment</button>
    </div>
  );
}
```

### Implementing Timers and Intervals

```jsx
// File: src/useref-timers.jsx

import React, { useState, useRef, useEffect } from 'react';

function Stopwatch() {
  const [time, setTime] = useState(0);
  const [isRunning, setIsRunning] = useState(false);
  
  // Store interval ID in ref
  const intervalRef = useRef(null);
  
  useEffect(() => {
    if (isRunning) {
      intervalRef.current = setInterval(() => {
        setTime(t => t + 1);
      }, 1000);
    } else {
      clearInterval(intervalRef.current);
    }
    
    // Cleanup on unmount or when stopping
    return () => clearInterval(intervalRef.current);
  }, [isRunning]);
  
  const start = () => setIsRunning(true);
  const stop = () => setIsRunning(false);
  const reset = () => {
    setIsRunning(false);
    setTime(0);
    clearInterval(intervalRef.current);
  };
  
  // Format time as MM:SS
  const formatTime = (seconds) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  };
  
  return (
    <div style={{ textAlign: 'center', padding: '20px' }}>
      <h1>{formatTime(time)}</h1>
      <button onClick={start} disabled={isRunning}>Start</button>
      <button onClick={stop} disabled={!isRunning}>Stop</button>
      <button onClick={reset}>Reset</button>
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Using useRef Instead of useState for UI State

```jsx
// ❌ WRONG - Using ref for state that should trigger re-render
function BadComponent() {
  const countRef = useRef(0);
  
  const increment = () => {
    countRef.current += 1;
    // No re-render! UI won't update!
  };
  
  return <button onClick={increment}>{countRef.current}</button>;
}

// ✅ CORRECT - Use useState for values that affect UI
function GoodComponent() {
  const [count, setCount] = useState(0);
  
  const increment = () => {
    setCount(c => c + 1); // Triggers re-render
  };
  
  return <button onClick={increment}>{count}</button>;
}
```

### Mistake 2: Not Using Ref for DOM After Conditional Render

```jsx
// ❌ WRONG - Ref doesn't work with conditional rendering
function BadComponent() {
  const [show, setShow] = useState(false);
  const inputRef = useRef(null);
  
  const focus = () => {
    inputRef.current?.focus(); // Might be null!
  };
  
  return (
    <div>
      {show && <input ref={inputRef} />}
      <button onClick={focus}>Focus</button>
    </div>
  );
}

// ✅ CORRECT - Conditional ref handling
function GoodComponent() {
  const [show, setShow] = useState(false);
  const inputRef = useRef(null);
  
  useEffect(() => {
    if (show && inputRef.current) {
      inputRef.current.focus();
    }
  }, [show]);
  
  return (
    <div>
      {show && <input ref={inputRef} />}
      <button onClick={() => setShow(true)}>Show & Focus</button>
    </div>
  );
}
```

### Mistake 3: Forgetting to Clean Up Event Handlers

```jsx
// ❌ WRONG - Not cleaning up
function BadComponent() {
  const countRef = useRef(0);
  
  useEffect(() => {
    window.addEventListener('resize', () => {
      console.log('Resize:', countRef.current++);
    });
  }, []); // Missing cleanup!
  
  return <div>Resize window</div>;
}

// ✅ CORRECT - Clean up event listeners
function GoodComponent() {
  const countRef = useRef(0);
  
  useEffect(() => {
    const handler = () => {
      console.log('Resize:', countRef.current++);
    };
    
    window.addEventListener('resize', handler);
    
    return () => {
      window.removeEventListener('resize', handler);
    };
  }, []);
  
  return <div>Resize window</div>;
}
```

## Real-World Example

```jsx
// File: src/components/InputForm.jsx

import React, { useRef, useState, useEffect } from 'react';

function InputForm() {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  });
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});
  
  // Refs for form fields
  const usernameRef = useRef(null);
  const emailRef = useRef(null);
  const passwordRef = useRef(null);
  const firstErrorRef = useRef(null);
  
  // Focus first error field
  useEffect(() => {
    if (Object.keys(errors).length > 0 && firstErrorRef.current) {
      firstErrorRef.current.focus();
    }
  }, [errors]);
  
  const validate = () => {
    const newErrors = {};
    
    if (!formData.username.trim()) {
      newErrors.username = 'Username is required';
    } else if (formData.username.length < 3) {
      newErrors.username = 'Username must be at least 3 characters';
    }
    
    if (!formData.email.trim()) {
      newErrors.email = 'Email is required';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = 'Invalid email format';
    }
    
    if (!formData.password) {
      newErrors.password = 'Password is required';
    } else if (formData.password.length < 8) {
      newErrors.password = 'Password must be at least 8 characters';
    }
    
    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }
    
    setErrors(newErrors);
    
    // Find first field with error for ref
    if (Object.keys(newErrors).length > 0) {
      if (newErrors.username) firstErrorRef.current = usernameRef.current;
      else if (newErrors.email) firstErrorRef.current = emailRef.current;
      else if (newErrors.password) firstErrorRef.current = passwordRef.current;
    }
    
    return Object.keys(newErrors).length === 0;
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    
    // Mark all fields as touched
    setTouched({
      username: true,
      email: true,
      password: true,
      confirmPassword: true
    });
    
    if (validate()) {
      console.log('Form submitted:', formData);
      alert('Form submitted successfully!');
    }
  };
  
  const handleBlur = (field) => {
    setTouched(prev => ({ ...prev, [field]: true }));
    validate();
  };
  
  const styles = {
    form: { maxWidth: '400px', margin: '0 auto', padding: '20px' },
    input: { width: '100%', padding: '10px', marginBottom: '5px', borderRadius: '4px' },
    label: { display: 'block', marginBottom: '5px', fontWeight: '500' },
    error: { color: '#f44336', fontSize: '12px', marginBottom: '10px' },
    button: { width: '100%', padding: '12px', backgroundColor: '#4CAF50', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', marginTop: '10px' }
  };
  
  return (
    <form onSubmit={handleSubmit} style={styles.form}>
      <h2>Registration Form</h2>
      
      <label style={styles.label}>Username</label>
      <input
        ref={usernameRef}
        type="text"
        value={formData.username}
        onChange={e => setFormData({ ...formData, username: e.target.value })}
        onBlur={() => handleBlur('username')}
        style={{ ...styles.input, border: errors.username && touched.username ? '1px solid #f44336' : '1px solid #ddd' }}
      />
      {errors.username && touched.username && (
        <div style={styles.error}>{errors.username}</div>
      )}
      
      <label style={styles.label}>Email</label>
      <input
        ref={emailRef}
        type="email"
        value={formData.email}
        onChange={e => setFormData({ ...formData, email: e.target.value })}
        onBlur={() => handleBlur('email')}
        style={{ ...styles.input, border: errors.email && touched.email ? '1px solid #f44336' : '1px solid #ddd' }}
      />
      {errors.email && touched.email && (
        <div style={styles.error}>{errors.email}</div>
      )}
      
      <label style={styles.label}>Password</label>
      <input
        ref={passwordRef}
        type="password"
        value={formData.password}
        onChange={e => setFormData({ ...formData, password: e.target.value })}
        onBlur={() => handleBlur('password')}
        style={{ ...styles.input, border: errors.password && touched.password ? '1px solid #f44336' : '1px solid #ddd' }}
      />
      {errors.password && touched.password && (
        <div style={styles.error}>{errors.password}</div>
      )}
      
      <label style={styles.label}>Confirm Password</label>
      <input
        type="password"
        value={formData.confirmPassword}
        onChange={e => setFormData({ ...formData, confirmPassword: e.target.value })}
        onBlur={() => handleBlur('confirmPassword')}
        style={{ ...styles.input, border: errors.confirmPassword && touched.confirmPassword ? '1px solid #f44336' : '1px solid #ddd' }}
      />
      {errors.confirmPassword && touched.confirmPassword && (
        <div style={styles.error}>{errors.confirmPassword}</div>
      )}
      
      <button type="submit" style={styles.button}>Register</button>
    </form>
  );
}

export default InputForm;
```

## Key Takeaways

- useRef provides access to DOM elements via the ref attribute
- useRef stores mutable values that persist across renders without triggering re-renders
- Use refs for DOM manipulation (focus, scroll, measurements)
- Use refs for values that change frequently but don't need immediate UI updates
- Never use refs for values that should trigger re-renders - use useState instead
- Always check if ref.current exists before using (it might be null)
- Clean up event listeners and timers in useEffect cleanup

## What's Next

Now let's explore advanced hooks including useReducer, useContext, useMemo, and useCallback.
