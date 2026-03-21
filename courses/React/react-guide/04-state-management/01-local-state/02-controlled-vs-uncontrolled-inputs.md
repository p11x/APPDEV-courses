# Controlled vs Uncontrolled Inputs in React

## Overview
Understanding the difference between controlled and uncontrolled inputs is crucial for building React forms. Controlled inputs have their value managed by React state, while uncontrolled inputs manage their own internal state and use refs to access values. Each approach has specific use cases and trade-offs.

## Prerequisites
- Basic understanding of React hooks
- Familiarity with form elements
- Knowledge of useState and useRef hooks

## Core Concepts

### Controlled Inputs
In controlled components, form data is handled by React state. The input's value is always derived from state.

```jsx
// File: src/components/ControlledInput.jsx

import React, { useState } from 'react';

function ControlledForm() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log({ name, email });
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Name:</label>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>
      <div>
        <label>Email:</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>
      <button type="submit">Submit</button>
    </form>
  );
}

export default ControlledForm;
```

### Uncontrolled Inputs
Uncontrolled components manage their own state internally and use refs to access values when needed.

```jsx
// File: src/components/UncontrolledInput.jsx

import React, { useRef } from 'react';

function UncontrolledForm() {
  const nameRef = useRef(null);
  const emailRef = useRef(null);

  const handleSubmit = (e) => {
    e.preventDefault();
    // Access values directly from refs
    console.log({
      name: nameRef.current.value,
      email: emailRef.current.value,
    });
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Name:</label>
        <input type="text" ref={nameRef} defaultValue="John" />
      </div>
      <div>
        <label>Email:</label>
        <input type="email" ref={emailRef} />
      </div>
      <button type="submit">Submit</button>
    </form>
  );
}

export default UncontrolledForm;
```

### When to Use Each Approach

**Use Controlled Inputs when:**
- You need immediate validation
- You want to transform input data
- You're building complex forms
- You need to disable the form based on conditions

**Use Uncontrolled Inputs when:**
- You need minimal form logic
- You're integrating with non-React code
- Performance is critical (fewer re-renders)

```jsx
// File: src/components/FormComparison.jsx

import React, { useState, useRef } from 'react';

// Controlled - good for validation and transformation
function ControlledSearch() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);

  const handleChange = (e) => {
    const value = e.target.value;
    // Transform to lowercase for search
    setQuery(value.toLowerCase());
    
    // Immediate validation
    if (value.length < 2) {
      setResults([]);
      return;
    }
    
    // Simulate search
    setResults([`Result for "${value}"`]);
  };

  return (
    <div>
      <input value={query} onChange={handleChange} />
      {results.map(r => <div key={r}>{r}</div>)}
    </div>
  );
}

// Uncontrolled - good for simple forms
function SimpleContactForm() {
  const formRef = useRef(null);

  const handleSubmit = (e) => {
    e.preventDefault();
    const formData = new FormData(formRef.current);
    console.log(Object.fromEntries(formData));
  };

  return (
    <form ref={formRef} onSubmit={handleSubmit}>
      <input name="name" type="text" />
      <input name="email" type="email" />
      <button type="submit">Submit</button>
    </form>
  );
}

export { ControlledSearch, SimpleContactForm };
```

## Common Mistakes

### Mixing Controlled and Uncontrolled

```jsx
// ❌ WRONG - Don't mix approaches
function BadInput() {
  const [value, setValue] = useState('');
  return (
    <input
      value={value}
      onChange={(e) => setValue(e.target.value)}
      defaultValue="initial" // Conflict!
    />
  );
}

// ✅ CORRECT - Choose one approach
function GoodInput() {
  const [value, setValue] = useState('initial');
  return (
    <input
      value={value}
      onChange={(e) => setValue(e.target.value)}
    />
  );
}
```

## Key Takeaways
- Controlled inputs use React state for their value
- Uncontrolled inputs use refs and manage their own state
- Choose controlled for complex forms with validation
- Choose uncontrolled for simple, performant forms
- Never mix both approaches on the same input

## What's Next
Continue to [State Colocation Strategy](03-state-colocation-strategy.md) to learn where to place state in your component tree.
