# Controlled vs Uncontrolled Inputs

## Overview

Controlled inputs have their value managed by React state, while uncontrolled inputs manage their own state internally. Understanding the difference is crucial for building forms in React. Controlled inputs are the React way - you track the value in state and update it via onChange.

## Prerequisites

- Understanding of useState hook
- Knowledge of form elements
- Familiarity with event handling

## Core Concepts

### Controlled Inputs

```jsx
// File: src/controlled-inputs.jsx

import React, { useState } from 'react';

function ControlledForm() {
  const [value, setValue] = useState('');
  
  return (
    <input
      value={value}
      onChange={e => setValue(e.target.value)}
    />
  );
}
```

### Uncontrolled Inputs

```jsx
// File: src/uncontrolled-inputs.jsx

import React, { useRef } from 'react';

function UncontrolledForm() {
  const inputRef = useRef(null);
  
  const handleSubmit = () => {
    console.log(inputRef.current.value);
  };
  
  return (
    <input ref={inputRef} defaultValue="initial" />
  );
}
```

## Key Takeaways

- Controlled inputs: value in state, React manages everything
- Uncontrolled inputs: use ref, manage own state
- Use controlled inputs for most React forms
- Use uncontrolled for simple integrations

## What's Next

Let's explore state colocation strategy.
