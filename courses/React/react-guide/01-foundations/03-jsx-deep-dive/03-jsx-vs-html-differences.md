# JSX vs HTML Differences

## Overview

While JSX looks very similar to HTML, there are important differences that can trip up developers moving from plain HTML/JavaScript to React. Understanding these differences is crucial for writing working React code and avoiding common bugs. This guide covers all the key differences between JSX and HTML, with practical examples and explanations.

## Prerequisites

- Solid understanding of HTML and CSS
- Familiarity with JavaScript
- Basic knowledge of React components and JSX
- Understanding of HTML form elements

## Core Concepts

### Attribute Differences

JSX attributes differ from HTML in several important ways. These changes were made because JSX is JavaScript, and some attribute names are reserved words or conflict with JavaScript syntax.

```jsx
// File: src/attribute-differences.jsx

import React from 'react';

function AttributeDifferences() {
  return (
    <div>
      {/* 
       * HTML uses 'class', JSX uses 'className'
       * 'class' is a reserved word in JavaScript 
       */}
      <div className="container">This is a container</div>
      
      {/* 
       * HTML uses 'for', JSX uses 'htmlFor' 
       * 'for' is used in loops in JavaScript
       */}
      <label htmlFor="email">Email:</label>
      <input type="text" id="email" />
      
      {/* 
       * Event handlers use camelCase
       * HTML: onclick, onchange, onsubmit
       * JSX: onClick, onChange, onSubmit
       */}
      <button onClick={() => alert('Clicked!')}>Click Me</button>
      <input onChange={(e) => console.log(e.target.value)} />
      
      {/* 
       * Boolean attributes work differently
       * In HTML: <input disabled> or <input disabled="disabled">
       * In JSX: <input disabled={true}> or just <input disabled />
       */}
      <input type="text" disabled />  {/* Shorthand for disabled={true} */}
      <input type="text" disabled={false} />  {/* Explicitly false */}
      <input type="checkbox" checked />  {/* Shorthand */}
      <input type="checkbox" checked={false} />
      
      {/* 
       * Custom data attributes
       * HTML: data-user-id="123"
       * JSX: dataUserId={123} or data-user-id={123}
       */}
      <div data-user-id="123" data-active="true">User Data</div>
      <div dataUserId={123} dataActive={true}>CamelCase Also Works</div>
      
      {/* 
       * style takes an object, not a string
       * HTML: style="color: red; font-size: 16px;"
       * JSX: style={{ color: 'red', fontSize: '16px' }}
       */}
      <p style={{ color: 'red', fontSize: '16px' }}>Styled paragraph</p>
    </div>
  );
}
```

### Event Handler Differences

React's event system is different from native HTML events. React uses synthetic events that work consistently across all browsers.

```jsx
// File: src/event-differences.jsx

import React, { useState } from 'react';

function EventDifferences() {
  const [value, setValue] = useState('');
  
  // In JSX, event handlers are functions, not strings
  // ❌ WRONG: onClick="handleClick()"
  // ✅ CORRECT: onClick={handleClick}
  
  const handleClick = (event) => {
    // React passes a SyntheticEvent wrapper
    console.log(event); // This is NOT a native DOM event
    console.log(event.type); // 'click'
    console.log(event.target); // The element that was clicked
    
    // To access the native event:
    console.log(event.nativeEvent);
    
    // Prevent default works the same
    event.preventDefault();
    
    // Stop propagation works the same
    event.stopPropagation();
  };
  
  const handleInputChange = (event) => {
    // The event object in React is different from native events
    const value = event.target.value; // Still works the same
    setValue(value);
  };
  
  const handleSubmit = (event) => {
    event.preventDefault();
    console.log('Form submitted with:', value);
  };
  
  // Different events for different elements
  return (
    <div>
      {/* Button click */}
      <button onClick={handleClick}>Click me</button>
      
      {/* Input change - note it's onChange, not oninput */}
      <input 
        type="text" 
        value={value}
        onChange={handleInputChange} 
      />
      
      {/* Form submit */}
      <form onSubmit={handleSubmit}>
        <input type="text" name="username" />
        <button type="submit">Submit</button>
      </form>
      
      {/* Other common events */}
      <input type="text" onFocus={() => console.log('Focused')} />
      <input type="text" onBlur={() => console.log('Blurred')} />
      <div 
        onMouseEnter={() => console.log('Mouse entered')} 
        onMouseLeave={() => console.log('Mouse left')}
      >
        Hover me
      </div>
      <input type="text" onKeyDown={(e) => console.log('Key:', e.key)} />
    </div>
  );
}
```

### Form Element Differences

React forms behave differently from HTML forms. In React, form elements are typically controlled components.

```jsx
// File: src/form-differences.jsx

import React, { useState } from 'react';

function FormDifferences() {
  const [textInput, setTextInput] = useState('');
  const [textarea, setTextarea] = useState('');
  const [select, setSelect] = useState('banana');
  const [checkbox, setCheckbox] = useState(false);
  const [radio, setRadio] = useState('option1');
  
  const handleSubmit = (e) => {
    e.preventDefault();
    console.log({
      textInput,
      textarea,
      select,
      checkbox,
      radio
    });
  };
  
  return (
    <form onSubmit={handleSubmit}>
      {/* 
       * TEXT INPUT
       * In HTML: value is separate from state
       * In React: value is controlled by state (controlled component)
       */}
      <div>
        <label>Text Input:</label>
        <input 
          type="text" 
          value={textInput}  // Required for controlled input
          onChange={(e) => setTextInput(e.target.value)}
        />
      </div>
      
      {/* 
       * TEXTAREA
       * In HTML: <textarea>content</textarea>
       * In React: <textarea value={content} />
       */}
      <div>
        <label>Textarea:</label>
        <textarea 
          value={textarea}
          onChange={(e) => setTextarea(e.target.value)}
          rows={3}
        />
      </div>
      
      {/* 
       * SELECT
       * In HTML: <option selected>
       * In React: <option value={value} selected={isSelected}>
       * or just control the select's value directly
       */}
      <div>
        <label>Select:</label>
        <select 
          value={select}
          onChange={(e) => setSelect(e.target.value)}
        >
          <option value="apple">Apple</option>
          <option value="banana">Banana</option>
          <option value="cherry">Cherry</option>
        </select>
      </div>
      
      {/* 
       * CHECKBOX
       * In HTML: checked is a property
       * In React: checked is controlled by state
       */}
      <div>
        <label>
          <input 
            type="checkbox"
            checked={checkbox}
            onChange={(e) => setCheckbox(e.target.checked)}
          />
          I agree
        </label>
      </div>
      
      {/* 
       * RADIO BUTTONS
       * Same pattern - use value and onChange
       */}
      <div>
        <label>
          <input 
            type="radio" 
            name="radioGroup" 
            value="option1"
            checked={radio === 'option1'}
            onChange={(e) => setRadio(e.target.value)}
          />
          Option 1
        </label>
        <label>
          <input 
            type="radio" 
            name="radioGroup" 
            value="option2"
            checked={radio === 'option2'}
            onChange={(e) => setRadio(e.target.value)}
          />
          Option 2
        </label>
      </div>
      
      <button type="submit">Submit</button>
    </form>
  );
}
```

### Value and Children Differences

```jsx
// File: src/value-children.jsx

import React from 'react';

function ValueChildrenDifferences() {
  return (
    <div>
      {/* 
       * INPUT VALUE
       * HTML: <input value="static">
       * JSX: <input value={variable}>
       * 
       * If you want an uncontrolled input in React:
       * <input defaultValue="initial" />
       */}
      
      {/* 
       * textarea
       * HTML: <textarea>default content</textarea>
       * JSX: <textarea>{defaultContent}</textarea>
       */}
      
      {/* 
       * select option selected
       * HTML: <option selected>
       * JSX: Let the select's value prop determine selected
       */}
      
      {/* 
       * script tag
       * HTML: <script src="..."></script>
       * JSX: Doesn't render script tags (for security)
       */}
      
      {/* 
       * style tag in head
       * HTML: <style>...</style>
       * JSX: Use CSS files or CSS-in-JS solutions
       */}
      
      {/* 
       * iframe srcdoc
       * HTML: <iframe srcdoc="<html>...</html>">
       * JSX: Works but needs careful handling
       */}
      
      <div>
        <h3>Self-closing tags work:</h3>
        <img src="image.jpg" alt="Example" />
        <br />
        <input type="text" />
        <hr />
      </div>
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Using HTML Attribute Names

```jsx
// ❌ WRONG - HTML attributes don't work in JSX
function BadComponent() {
  return (
    <div class="container" onclick={() => {}}>
      <label for="email">Email</label>
      <input type="text" maxlength="10" tabindex="1" />
    </div>
  );
}

// ✅ CORRECT - Use JSX attribute names
function GoodComponent() {
  return (
    <div className="container" onClick={() => {}}>
      <label htmlFor="email">Email</label>
      <input type="text" maxLength={10} tabIndex={1} />
    </div>
  );
}
```

### Mistake 2: Passing Strings to Style Props

```jsx
// ❌ WRONG - Style takes an object, not a string
function BadStyles() {
  return (
    <div style="color: red; font-size: 16px;">
      Text
    </div>
  );
}

// ✅ CORRECT - Style takes a JavaScript object with camelCase
function GoodStyles() {
  return (
    <div style={{ color: 'red', fontSize: '16px' }}>
      Text
    </div>
  );
}
```

### Mistake 3: Not Using Value/Checked for Form Inputs

```jsx
// ❌ WRONG - Uncontrolled input (React warning)
// React will warn: "A component is changing an uncontrolled input"
function BadForm() {
  return <input type="text" defaultValue="Hello" />;
}

// ✅ CORRECT - Controlled input with value prop
function GoodForm() {
  const [value, setValue] = useState('Hello');
  return <input type="text" value={value} onChange={(e) => setValue(e.target.value)} />;
}
```

### Mistake 4: Using HTML Comments in JSX

```jsx
// ❌ WRONG - HTML comments don't work in JSX
function BadComments() {
  return (
    <div>
      <!-- This is an HTML comment, not JSX! -->
      <p>Content</p>
    </div>
  );
}

// ✅ CORRECT - Use JSX comments wrapped in curly braces
function GoodComments() {
  return (
    <div>
      {/* This is a JSX comment */}
      <p>Content</p>
    </div>
  );
}
```

## Real-World Example

Let's build a complete form demonstrating all JSX vs HTML differences:

```jsx
// File: src/components/ContactForm.jsx

import React, { useState } from 'react';

function ContactForm() {
  // State for all form fields (controlled components)
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    phone: '',
    website: '',
    gender: '',
    age: '',
    interests: [],
    newsletter: false,
    bio: ''
  });
  
  const [errors, setErrors] = useState({});
  const [isSubmitted, setIsSubmitted] = useState(false);
  
  // Handle all input changes with single handler
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
  
  // Handle checkbox array (interests)
  const handleInterestChange = (e) => {
    const { value, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      interests: checked 
        ? [...prev.interests, value]
        : prev.interests.filter(interest => interest !== value)
    }));
  };
  
  // Validate form
  const validate = () => {
    const newErrors = {};
    
    if (!formData.fullName.trim()) {
      newErrors.fullName = 'Full name is required';
    }
    
    if (!formData.email.trim()) {
      newErrors.email = 'Email is required';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = 'Invalid email format';
    }
    
    if (formData.website && !/^https?:\/\/.+/.test(formData.website)) {
      newErrors.website = 'Invalid URL (include https://)';
    }
    
    if (!formData.gender) {
      newErrors.gender = 'Please select a gender';
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };
  
  // Handle form submission
  const handleSubmit = (e) => {
    e.preventDefault(); // Prevent native form submission
    
    if (validate()) {
      console.log('Form submitted:', formData);
      setIsSubmitted(true);
    }
  };
  
  // Reset form
  const handleReset = () => {
    setFormData({
      fullName: '',
      email: '',
      phone: '',
      website: '',
      gender: '',
      age: '',
      interests: [],
      newsletter: false,
      bio: ''
    });
    setErrors({});
    setIsSubmitted(false);
  };
  
  // Common input styles
  const inputStyle = {
    width: '100%',
    padding: '10px',
    border: '1px solid #ddd',
    borderRadius: '4px',
    fontSize: '14px',
    marginBottom: '5px'
  };
  
  const errorStyle = {
    color: '#f44336',
    fontSize: '12px',
    marginBottom: '10px'
  };
  
  const labelStyle = {
    display: 'block',
    marginBottom: '5px',
    fontWeight: '500',
    color: '#333'
  };
  
  if (isSubmitted) {
    return (
      <div style={{ padding: '20px', textAlign: 'center' }}>
        <h2 style={{ color: '#4CAF50' }}>Thank you!</h2>
        <p>Your message has been submitted.</p>
        <p>Name: {formData.fullName}</p>
        <p>Email: {formData.email}</p>
        <button onClick={handleReset} style={{ 
          padding: '10px 20px', 
          backgroundColor: '#2196F3', 
          color: 'white',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer'
        }}>
          Submit Another
        </button>
      </div>
    );
  }
  
  return (
    <form 
      onSubmit={handleSubmit} 
      onReset={handleReset}
      style={{ maxWidth: '500px', margin: '0 auto', padding: '20px' }}
    >
      <h2>Contact Form</h2>
      
      {/* Text Input */}
      <div style={{ marginBottom: '15px' }}>
        <label htmlFor="fullName" style={labelStyle}>
          Full Name *
        </label>
        <input
          type="text"
          id="fullName"
          name="fullName"
          value={formData.fullName}
          onChange={handleChange}
          style={inputStyle}
        />
        {errors.fullName && <span style={errorStyle}>{errors.fullName}</span>}
      </div>
      
      {/* Email Input */}
      <div style={{ marginBottom: '15px' }}>
        <label htmlFor="email" style={labelStyle}>
          Email *
        </label>
        <input
          type="email"
          id="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          style={inputStyle}
        />
        {errors.email && <span style={errorStyle}>{errors.email}</span>}
      </div>
      
      {/* Phone Input */}
      <div style={{ marginBottom: '15px' }}>
        <label htmlFor="phone" style={labelStyle}>
          Phone
        </label>
        <input
          type="tel"
          id="phone"
          name="phone"
          value={formData.phone}
          onChange={handleChange}
          style={inputStyle}
        />
      </div>
      
      {/* URL Input */}
      <div style={{ marginBottom: '15px' }}>
        <label htmlFor="website" style={labelStyle}>
          Website
        </label>
        <input
          type="url"
          id="website"
          name="website"
          value={formData.website}
          onChange={handleChange}
          placeholder="https://example.com"
          style={inputStyle}
        />
        {errors.website && <span style={errorStyle}>{errors.website}</span>}
      </div>
      
      {/* Select */}
      <div style={{ marginBottom: '15px' }}>
        <label htmlFor="gender" style={labelStyle}>
          Gender *
        </label>
        <select
          id="gender"
          name="gender"
          value={formData.gender}
          onChange={handleChange}
          style={inputStyle}
        >
          <option value="">Select...</option>
          <option value="male">Male</option>
          <option value="female">Female</option>
          <option value="other">Other</option>
        </select>
        {errors.gender && <span style={errorStyle}>{errors.gender}</span>}
      </div>
      
      {/* Number Input */}
      <div style={{ marginBottom: '15px' }}>
        <label htmlFor="age" style={labelStyle}>
          Age
        </label>
        <input
          type="number"
          id="age"
          name="age"
          min="0"
          max="150"
          value={formData.age}
          onChange={handleChange}
          style={inputStyle}
        />
      </div>
      
      {/* Checkbox Group */}
      <div style={{ marginBottom: '15px' }}>
        <label style={labelStyle}>Interests:</label>
        {['Programming', 'Design', 'Marketing', 'Data Science'].map(interest => (
          <label key={interest} style={{ display: 'block', marginBottom: '5px' }}>
            <input
              type="checkbox"
              name="interests"
              value={interest.toLowerCase()}
              checked={formData.interests.includes(interest.toLowerCase())}
              onChange={handleInterestChange}
              style={{ marginRight: '8px' }}
            />
            {interest}
          </label>
        ))}
      </div>
      
      {/* Single Checkbox */}
      <div style={{ marginBottom: '15px' }}>
        <label style={{ display: 'flex', alignItems: 'center', cursor: 'pointer' }}>
          <input
            type="checkbox"
            name="newsletter"
            checked={formData.newsletter}
            onChange={handleChange}
            style={{ marginRight: '8px' }}
          />
          Subscribe to newsletter
        </label>
      </div>
      
      {/* Textarea */}
      <div style={{ marginBottom: '15px' }}>
        <label htmlFor="bio" style={labelStyle}>
          Bio
        </label>
        <textarea
          id="bio"
          name="bio"
          value={formData.bio}
          onChange={handleChange}
          rows={4}
          style={inputStyle}
          placeholder="Tell us about yourself..."
        />
      </div>
      
      {/* Submit and Reset Buttons */}
      <div style={{ display: 'flex', gap: '10px' }}>
        <button
          type="submit"
          style={{
            flex: 1,
            padding: '12px',
            backgroundColor: '#4CAF50',
            color: 'white',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
            fontSize: '16px'
          }}
        >
          Submit
        </button>
        <button
          type="reset"
          style={{
            flex: 1,
            padding: '12px',
            backgroundColor: '#666',
            color: 'white',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
            fontSize: '16px'
          }}
        >
          Reset
        </button>
      </div>
    </form>
  );
}

export default ContactForm;
```

## Key Takeaways

- Use `className` instead of `class` and `htmlFor` instead of `for`
- Event handlers use camelCase: `onClick`, `onChange`, `onSubmit`
- The `style` prop accepts a JavaScript object with camelCase properties
- Form inputs in React are typically "controlled components" with value props
- JSX comments must be wrapped in curly braces: `{/* comment */}`
- All tags must be properly closed (self-closing for void elements)
- React event handlers receive synthetic event objects, not native DOM events
- Use `defaultValue` for uncontrolled inputs or `value` for controlled inputs

## What's Next

Congratulations! You've completed the foundations section. Now let's move on to learning about React components - the building blocks of any React application.
