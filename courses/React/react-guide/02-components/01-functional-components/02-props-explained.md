# Props Explained

## Overview

Props (short for properties) are the primary way to pass data from parent components to child components in React. They make components dynamic and reusable by allowing you to customize their appearance and behavior. Understanding how props work is fundamental to building React applications. In this guide, we'll explore everything you need to know about props: passing them, receiving them, default values, and best practices.

## Prerequisites

- Understanding of React components (from previous lesson)
- Knowledge of JavaScript functions and parameters
- Familiarity with JSX syntax
- Basic understanding of data types in JavaScript

## Core Concepts

### What are Props?

Props are the arguments you pass to React components, similar to how you pass arguments to functions. They flow downward from parent to child components.

```jsx
// File: src/props-basics.jsx

import React from 'react';

// The component receives props as its first parameter
function Greeting(props) {
  // Access individual props from the props object
  return <h1>Hello, {props.name}!</h1>;
}

// We can destructure props for cleaner code
function GreetingDestructured({ name, age }) {
  return (
    <div>
      <h1>Hello, {name}!</h1>
      <p>You are {age} years old.</p>
    </div>
  );
}

// Using the component and passing props
function App() {
  return (
    <div>
      {/* Pass props like HTML attributes */}
      <Greeting name="Alice" age={25} />
      <GreetingDestructured name="Bob" age={30} />
    </div>
  );
}
```

### Passing Different Types of Props

React props can be any JavaScript type: strings, numbers, booleans, arrays, objects, functions, and even other components.

```jsx
// File: src/props-types.jsx

import React from 'react';

function PropsDemo() {
  // String prop
  const title = 'Welcome';
  
  // Number prop
  const count = 42;
  
  // Boolean prop
  const isActive = true;
  
  // Array prop
  const items = ['Apple', 'Banana', 'Cherry'];
  
  // Object prop
  const user = {
    name: 'Alice',
    email: 'alice@example.com',
    role: 'admin'
  };
  
  // Function prop
  const handleClick = () => alert('Clicked!');
  
  // Component as prop
  const Header = () => <h2>Custom Header</h2>;
  
  return (
    <ComponentWithAllProps
      // Strings (can use quotes or curly braces)
      title="Hello"
      titleWithBraces={'World'}
      
      // Numbers (must use curly braces)
      count={42}
      
      // Booleans (can use shorthand)
      isActive={true}
      isActiveShorthand // Equivalent to isActiveShorthand={true}
      
      // Arrays
      items={items}
      
      // Objects
      user={user}
      
      // Functions
      onClick={handleClick}
      
      // Components
      Header={Header}
    />
  );
}

function ComponentWithAllProps({
  title,
  count,
  isActive,
  isActiveShorthand,
  items,
  user,
  onClick,
  Header
}) {
  return (
    <div>
      <h1>{title} {count}</h1>
      <p>Active: {String(isActive)}</p>
      <p>Shorthand: {String(isActiveShorthand)}</p>
      
      <ul>
        {items.map(item => <li key={item}>{item}</li>)}
      </ul>
      
      <p>User: {user.name} ({user.email})</p>
      <button onClick={onClick}>Click me</button>
      
      <Header />
    </div>
  );
}
```

### Default Props

Default props provide fallback values when a prop isn't passed or is undefined.

```jsx
// File: src/default-props.jsx

import React from 'react';

// Method 1: Default parameter values (modern approach)
function Button1({ text = 'Click me', variant = 'primary', onClick = () => {} }) {
  return (
    <button className={`btn btn-${variant}`} onClick={onClick}>
      {text}
    </button>
  );
}

// Method 2: Default props property (older approach, still works)
function Button2({ text, variant, onClick }) {
  return (
    <button className={`btn btn-${variant}`} onClick={onClick}>
      {text}
    </button>
  );
}

Button2.defaultProps = {
  text: 'Click me',
  variant: 'primary',
  onClick: () => {}
};

// Method 3: Default props with destructuring
function Card({ title = 'Untitled', content = '', image = null }) {
  return (
    <div className="card">
      {image && <img src={image} alt={title} />}
      <h3>{title}</h3>
      <p>{content}</p>
    </div>
  );
}

function App() {
  return (
    <div>
      {/* No props - uses all defaults */}
      <Button1 />
      <Button2 />
      <Card />
      
      {/* With props - overrides defaults */}
      <Button1 text="Submit" variant="success" onClick={() => {}} />
      <Button2 text="Save" variant="primary" />
      <Card 
        title="Hello World" 
        content="This is the content" 
        image="https://via.placeholder.com/300"
      />
    </div>
  );
}
```

### Props Validation with PropTypes

PropTypes provide runtime validation for props during development, helping catch bugs early.

```jsx
// File: src/prop-types.jsx

import React from 'react';
import PropTypes from 'prop-types';

function UserCard({ name, age, email, isAdmin, hobbies, onUpdate }) {
  return (
    <div className="user-card">
      <h3>{name}</h3>
      <p>Age: {age}</p>
      <p>Email: {email}</p>
      <p>Admin: {isAdmin ? 'Yes' : 'No'}</p>
      <p>Hobbies: {hobbies?.join(', ')}</p>
      <button onClick={onUpdate}>Update</button>
    </div>
  );
}

// Define PropTypes for validation
UserCard.propTypes = {
  // Required string
  name: PropTypes.string.isRequired,
  
  // Required number
  age: PropTypes.number.isRequired,
  
  // Optional string
  email: PropTypes.string,
  
  // Boolean
  isAdmin: PropTypes.bool,
  
  // Array
  hobbies: PropTypes.array,
  
  // Function (common pattern for callbacks)
  onUpdate: PropTypes.func.isRequired,
  
  // Object with specific shape
  settings: PropTypes.shape({
    theme: PropTypes.string,
    notifications: PropTypes.bool
  }),
  
  // OneOf - specific set of values
  role: PropTypes.oneOf(['user', 'admin', 'moderator']),
  
  // OneOfType - multiple possible types
  id: PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.number
  ])
};

// Default props (optional when using PropTypes)
UserCard.defaultProps = {
  email: 'No email provided',
  isAdmin: false,
  hobbies: []
};

export default UserCard;
```

### Children Props

The special `children` prop allows you to pass elements between component tags.

```jsx
// File: src/children-props.jsx

import React from 'react';

// This component expects children
function Card({ title, children }) {
  return (
    <div className="card">
      <div className="card-header">
        <h3>{title}</h3>
      </div>
      <div className="card-body">
        {children}
      </div>
    </div>
  );
}

// Usage - anything between tags becomes children
function App() {
  return (
    <div>
      <Card title="Welcome">
        <p>This is the card content!</p>
        <p>It can contain any JSX.</p>
      </Card>
      
      <Card title="Features">
        <ul>
          <li>Feature 1</li>
          <li>Feature 2</li>
          <li>Feature 3</li>
        </ul>
      </Card>
    </div>
  );
}

// Another example - Layout component
function Layout({ header, sidebar, children }) {
  return (
    <div className="layout">
      <header className="layout-header">{header}</header>
      <aside className="layout-sidebar">{sidebar}</aside>
      <main className="layout-content">{children}</main>
    </div>
  );
}

function App2() {
  return (
    <Layout
      header={<h1>My App</h1>}
      sidebar={
        <nav>
          <a href="/">Home</a>
          <a href="/about">About</a>
        </nav>
      }
    >
      <p>Main page content goes here!</p>
    </Layout>
  );
}
```

## Common Mistakes

### Mistake 1: Not Handling All Prop Types

```jsx
// ❌ WRONG - Not handling null/undefined
function UserName({ user }) {
  return <h1>{user.name.toUpperCase()}</h1>; // Crashes if user is undefined!
}

// ✅ CORRECT - Handle missing props
function UserName({ user }) {
  return <h1>{user?.name?.toUpperCase() ?? 'Guest'}</h1>;
}

// ✅ CORRECT - Default props
function UserNameDefault({ user = { name: 'Guest' } }) {
  return <h1>{user.name.toUpperCase()}</h1>;
}
```

### Mistake 2: Mutating Props

```jsx
// ❌ WRONG - Never mutate props!
function BadComponent({ items }) {
  items.push('new item'); // This mutates the prop!
  return <ul>{items.map(i => <li>{i}</li>)}</ul>;
}

// ✅ CORRECT - Create a new array
function GoodComponent({ items }) {
  const newItems = [...items, 'new item']; // Spread creates new array
  return <ul>{newItems.map(i => <li key={i}>{i}</li>)}</ul>;
}
```

### Mistake 3: Using Props Before Declaring Them

```jsx
// ❌ WRONG - Using props before destructuring
function BadComponent(props) {
  const name = props.name; // Works but verbose
  return <h1>{name}</h1>;
}

// ✅ CORRECT - Destructure at the start
function GoodComponent({ name, age }) {
  return <h1>{name} is {age}</h1>;
}
```

### Mistake 4: Not Using Keys When Rendering Lists from Props

```jsx
// ❌ WRONG - Missing key
function BadList({ items }) {
  return (
    <ul>
      {items.map(item => <li>{item.name}</li>)}
    </ul>
  );
}

// ✅ CORRECT - Include unique key
function GoodList({ items }) {
  return (
    <ul>
      {items.map(item => <li key={item.id}>{item.name}</li>)}
    </ul>
  );
}
```

## Real-World Example

Let's build a complete form component with all types of props:

```jsx
// File: src/components/FormField.jsx

import React from 'react';

function FormField({
  label,
  name,
  type = 'text',
  value,
  onChange,
  placeholder,
  error,
  required = false,
  disabled = false,
  options = [],
  children
}) {
  // Common styles
  const containerStyle = { marginBottom: '15px' };
  const labelStyle = {
    display: 'block',
    marginBottom: '5px',
    fontWeight: '500',
    color: error ? '#f44336' : '#333'
  };
  const inputStyle = {
    width: '100%',
    padding: '10px',
    border: `1px solid ${error ? '#f44336' : '#ddd'}`,
    borderRadius: '4px',
    fontSize: '14px',
    backgroundColor: disabled ? '#f5f5f5' : 'white'
  };
  const errorStyle = {
    color: '#f44336',
    fontSize: '12px',
    marginTop: '5px'
  };
  
  // Render different input types
  const renderInput = () => {
    const commonProps = {
      id: name,
      name,
      value,
      onChange,
      placeholder,
      disabled,
      required,
      style: inputStyle
    };
    
    switch (type) {
      case 'textarea':
        return <textarea {...commonProps} rows={4} />;
      
      case 'select':
        return (
          <select {...commonProps}>
            <option value="">Select...</option>
            {options.map(option => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        );
      
      case 'checkbox':
        return (
          <label style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <input
              type="checkbox"
              checked={value}
              onChange={onChange}
              disabled={disabled}
            />
            {label}
          </label>
        );
      
      case 'radio':
        return (
          <div>
            {options.map(option => (
              <label 
                key={option.value} 
                style={{ display: 'flex', alignItems: 'center', gap: '8px', marginBottom: '5px' }}
              >
                <input
                  type="radio"
                  name={name}
                  value={option.value}
                  checked={value === option.value}
                  onChange={onChange}
                  disabled={disabled}
                />
                {option.label}
              </label>
            ))}
          </div>
        );
      
      default:
        return <input type={type} {...commonProps} />;
    }
  };
  
  // Checkbox and radio don't need separate labels
  const showLabel = type !== 'checkbox' && type !== 'radio';
  
  return (
    <div style={containerStyle}>
      {showLabel && (
        <label htmlFor={name} style={labelStyle}>
          {label}
          {required && <span style={{ color: '#f44336' }}> *</span>}
        </label>
      )}
      
      {renderInput()}
      
      {error && <div style={errorStyle}>{error}</div>}
      
      {/* Support children for custom content */}
      {children}
    </div>
  );
}

export default FormField;
```

```jsx
// File: src/components/RegistrationForm.jsx

import React, { useState } from 'react';
import FormField from './FormField';

function RegistrationForm() {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    age: '',
    gender: '',
    bio: '',
    interests: [],
    agree: false
  });
  
  const [errors, setErrors] = useState({});
  
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
    
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: null }));
    }
  };
  
  const handleInterestChange = (e) => {
    const { value, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      interests: checked
        ? [...prev.interests, value]
        : prev.interests.filter(i => i !== value)
    }));
  };
  
  const validate = () => {
    const newErrors = {};
    
    if (!formData.firstName.trim()) {
      newErrors.firstName = 'First name is required';
    }
    
    if (!formData.lastName.trim()) {
      newErrors.lastName = 'Last name is required';
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
    
    if (!formData.gender) {
      newErrors.gender = 'Please select a gender';
    }
    
    if (!formData.agree) {
      newErrors.agree = 'You must agree to the terms';
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    
    if (validate()) {
      console.log('Form submitted:', formData);
      alert('Registration successful!');
    }
  };
  
  const genderOptions = [
    { value: 'male', label: 'Male' },
    { value: 'female', label: 'Female' },
    { value: 'other', label: 'Other' }
  ];
  
  const interestOptions = [
    { value: 'coding', label: 'Coding' },
    { value: 'design', label: 'Design' },
    { value: 'gaming', label: 'Gaming' },
    { value: 'reading', label: 'Reading' }
  ];
  
  return (
    <form 
      onSubmit={handleSubmit}
      style={{ 
        maxWidth: '500px', 
        margin: '0 auto', 
        padding: '20px',
        backgroundColor: 'white',
        borderRadius: '8px',
        boxShadow: '0 2px 10px rgba(0,0,0,0.1)'
      }}
    >
      <h2 style={{ textAlign: 'center', marginBottom: '20px' }}>Registration Form</h2>
      
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '10px' }}>
        <FormField
          label="First Name"
          name="firstName"
          value={formData.firstName}
          onChange={handleChange}
          error={errors.firstName}
          required
        />
        
        <FormField
          label="Last Name"
          name="lastName"
          value={formData.lastName}
          onChange={handleChange}
          error={errors.lastName}
          required
        />
      </div>
      
      <FormField
        label="Email"
        name="email"
        type="email"
        value={formData.email}
        onChange={handleChange}
        placeholder="you@example.com"
        error={errors.email}
        required
      />
      
      <FormField
        label="Password"
        name="password"
        type="password"
        value={formData.password}
        onChange={handleChange}
        error={errors.password}
        required
      />
      
      <FormField
        label="Confirm Password"
        name="confirmPassword"
        type="password"
        value={formData.confirmPassword}
        onChange={handleChange}
        error={errors.confirmPassword}
        required
      />
      
      <FormField
        label="Age"
        name="age"
        type="number"
        value={formData.age}
        onChange={handleChange}
        placeholder="Enter your age"
      />
      
      <FormField
        label="Gender"
        name="gender"
        type="select"
        value={formData.gender}
        onChange={handleChange}
        options={genderOptions}
        error={errors.gender}
        required
      />
      
      <FormField
        label="Bio"
        name="bio"
        type="textarea"
        value={formData.bio}
        onChange={handleChange}
        placeholder="Tell us about yourself..."
      />
      
      <FormField
        label="Interests"
        type="checkbox"
        value={formData.interests}
        onChange={handleInterestChange}
      >
        <div style={{ marginTop: '10px', display: 'flex', flexWrap: 'wrap', gap: '10px' }}>
          {interestOptions.map(option => (
            <label key={option.value}>
              <input
                type="checkbox"
                name="interests"
                value={option.value}
                checked={formData.interests.includes(option.value)}
                onChange={handleInterestChange}
              />
              {' '}{option.label}
            </label>
          ))}
        </div>
      </FormField>
      
      <FormField
        label="Terms"
        name="agree"
        type="checkbox"
        value={formData.agree}
        onChange={handleChange}
        error={errors.agree}
      >
        <div style={{ marginTop: '10px' }}>
          I agree to the Terms and Conditions
        </div>
      </FormField>
      
      <button
        type="submit"
        style={{
          width: '100%',
          padding: '12px',
          backgroundColor: '#4CAF50',
          color: 'white',
          border: 'none',
          borderRadius: '4px',
          fontSize: '16px',
          cursor: 'pointer',
          marginTop: '10px'
        }}
      >
        Register
      </button>
    </form>
  );
}

export default RegistrationForm;
```

## Key Takeaways

- Props are how you pass data from parent to child components
- Props flow downward - data flows from parent to child only
- Props are read-only - never mutate them in child components
- Use destructuring to access props: `function Comp({ prop1, prop2 })`
- Default props can be set with default parameters or `Component.defaultProps`
- PropTypes provide runtime validation during development
- The special `children` prop lets you pass JSX between component tags
- Always provide unique `key` props when rendering lists from props

## What's Next

Now that you understand props, let's learn about export patterns in React - the difference between default and named exports, and when to use each.
