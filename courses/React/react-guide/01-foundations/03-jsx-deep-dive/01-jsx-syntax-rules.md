# JSX Syntax Rules

## Overview

JSX (JavaScript XML) is a syntax extension that allows you to write HTML-like code in your JavaScript files. It's one of the defining features of React and makes writing user interfaces intuitive and readable. However, JSX isn't valid JavaScript on its own - it needs to be transformed into regular JavaScript function calls. Understanding the syntax rules of JSX is essential for writing React applications effectively.

## Prerequisites

- Basic JavaScript knowledge
- Understanding of HTML and CSS
- Familiarity with React components
- Knowledge of JavaScript functions and arrays

## Core Concepts

### JSX is Not HTML

While JSX looks like HTML, it's actually JavaScript. Under the hood, every JSX element gets transformed into a call to `React.createElement()`. This means you can use JavaScript expressions anywhere within JSX.

```jsx
// File: src/jsx-basics.jsx

import React from 'react';

// JSX looks like HTML but is JavaScript
// This JSX:
const element = <h1>Hello, World!</h1>;

// Becomes this JavaScript:
const element = React.createElement('h1', null, 'Hello, World!');

// You can embed JavaScript expressions using curly braces {}
const name = 'Alice';
const greeting = <h1>Hello, {name}!</h1>; // Output: Hello, Alice!

// Any JavaScript expression can go inside {}
const sum = <p>2 + 2 = {2 + 2}</p>; // Output: 2 + 2 = 4
const date = <p>Today is {new Date().toLocaleDateString()}</p>;
```

### Component Return Rules

JSX must follow certain rules about what can be returned. Understanding these rules helps you avoid common errors.

```jsx
// File: src/return-rules.jsx

import React from 'react';

// Rule 1: Components must return a single root element
// ❌ WRONG - Can't return multiple elements at top level
function BadComponent() {
  return (
    <h1>Title</h1>
    <p>Content</p>
  );
}

// ✅ CORRECT - Wrap in a single element
function GoodComponent() {
  return (
    <div>
      <h1>Title</h1>
      <p>Content</p>
    </div>
  );
}

// ✅ CORRECT - Use React Fragment (doesn't add extra DOM nodes)
function FragmentComponent() {
  return (
    <>
      <h1>Title</h1>
      <p>Content</p>
    </>
  );
}

// Rule 2: All tags must be closed
// ❌ WRONG - Unclosed tags
function BadTags() {
  return (
    <div>
      <p>A paragraph
      <img src="image.jpg">
      <input type="text">
    </div>
  );
}

// ✅ CORRECT - Self-closing tags for void elements
function GoodTags() {
  return (
    <div>
      <p>A paragraph</p>
      <img src="image.jpg" />
      <input type="text" />
    </div>
  );
}
```

### Class Name and Style Syntax

Since JSX is JavaScript, certain HTML attributes have different names because "class" and "for" are reserved JavaScript keywords.

```jsx
// File: src/class-and-style.jsx

import React from 'react';

// ❌ WRONG - 'class' is a reserved JavaScript keyword
function BadComponent() {
  return <div class="container">Content</div>;
}

// ✅ CORRECT - Use 'className' instead
function GoodComponent() {
  return <div className="container">Content</div>;
}

// ❌ WRONG - 'for' is a reserved JavaScript keyword
function BadForm() {
  return <label for="email">Email</label>;
}

// ✅ CORRECT - Use 'htmlFor' instead
function GoodForm() {
  return <label htmlFor="email">Email</label>;
}

// Inline styles use camelCase and a JavaScript object
function StyledComponent() {
  const styles = {
    // camelCase for multi-word properties
    backgroundColor: 'blue',
    color: 'white',
    padding: '10px 20px',
    borderRadius: '5px',
    fontSize: '16px',
    marginTop: '10px',
  };
  
  return <button style={styles}>Click Me</button>;
}

// Or inline directly:
function InlineStyle() {
  return (
    <div style={{ padding: '20px', backgroundColor: '#f0f0f0' }}>
      <h1 style={{ color: '#333' }}>Title</h1>
    </div>
  );
}
```

### JavaScript Expressions in JSX

You can use any JavaScript expression inside JSX by wrapping it in curly braces.

```jsx
// File: src/jsx-expressions.jsx

import React from 'react';

function ExpressionExamples() {
  const user = { name: 'Alice', age: 25 };
  const items = ['Apple', 'Banana', 'Cherry'];
  const isLoggedIn = true;
  
  // String interpolation
  const greeting = <h1>Hello, {user.name}!</h1>;
  
  // Mathematical expressions
  const calc = <p>10 * 5 = {10 * 5}</p>;
  
  // Ternary operator for conditional rendering
  const status = isLoggedIn ? <p>Welcome back!</p> : <p>Please log in</p>;
  
  // Array methods
  const itemList = (
    <ul>
      {items.map(item => (
        <li key={item}>{item}</li>
      ))}
    </ul>
  );
  
  // Function calls
  const formatted = <p>{user.name.toUpperCase()}</p>;
  
  // Logical operators
  const showMessage = true && <p>Message shown!</p>;
  
  // All in one component
  return (
    <div>
      {greeting}
      {calc}
      {status}
      {itemList}
      {formatted}
      {showMessage}
    </div>
  );
}
```

### Comments in JSX

Comments in JSX are different from JavaScript comments. They must be wrapped in curly braces to be treated as JavaScript comments.

```jsx
// File: src/jsx-comments.jsx

import React from 'react';

function ComponentWithComments() {
  return (
    <div>
      {/* This is a JSX comment - note the curly braces */}
      <h1>Title</h1>
      
      {/* 
        Multi-line comments
        work the same way
      */}
      
      {/* Conditional comment example */}
      {false && (
        <p>This won't render but shows the syntax</p>
      )}
    </div>
  );
}

// Regular JavaScript comments still work outside JSX
function AnotherComponent() {
  // This is a regular JS comment
  const x = 5; // Inline comment
  
  return <p>Value: {x}</p>;
}
```

## Common Mistakes

### Mistake 1: Using Double Curly Braces

```jsx
// ❌ WRONG - Double curly braces create an object
function BadComponent() {
  return <div style={{'{{'}} padding: '10px' }}>Content</div>;
}

// ✅ CORRECT - Single curly braces with object
function GoodComponent() {
  return <div style={{ padding: '10px' }}>Content</div>;
}
```

### Mistake 2: String Literals in Curly Braces

```jsx
// ❌ WRONG - Quotes create a string, not an expression
function BadComponent() {
  return <p>{'Hello World'}</p>;
}

// ✅ CORRECT - Just write the string directly
function GoodComponent() {
  return <p>Hello World</p>;
  
  // Or if you need to use it as a value:
  const message = 'Hello World';
  return <p>{message}</p>;
}
```

### Mistake 3: Not Using camelCase for Event Handlers

```jsx
// ❌ WRONG - HTML uses lowercase
function BadComponent() {
  return <button onclick={handleClick}>Click</button>;
}

// ✅ CORRECT - JSX uses camelCase
function GoodComponent() {
  const handleClick = () => alert('Clicked!');
  return <button onClick={handleClick}>Click</button>;
}
```

### Mistake 4: Forgetting to Return JSX

```jsx
// ❌ WRONG - Missing return statement
function BadComponent() {
  const name = 'Alice';
  // Forgot to return!
}

// ✅ CORRECT - Return the JSX
function GoodComponent() {
  const name = 'Arrow';
  return <h1>Hello, {name}!</h1>;
}

// Arrow functions need explicit return
const ArrowGood = () => <h1>Hello!</h1>; // Has return

const ArrowBad = () => { 
  const name = 'Bob'; 
  // Forgot to return!
};

const ArrowCorrect = () => {
  const name = 'Bob';
  return <h1>Hello, {name}!</h1>;
};
```

## Real-World Example

Let's build a practical component that demonstrates all JSX syntax rules:

```jsx
// File: src/components/UserCard.jsx

import React from 'react';

function UserCard({ user, onEdit, onDelete }) {
  // Destructuring props with default values
  const { 
    name = 'Unknown User', 
    email = 'No email',
    avatar = 'https://via.placeholder.com/100',
    role = 'user',
    isActive = true 
  } = user;
  
  // Inline styles using camelCase
  const cardStyle = {
    border: '1px solid #ddd',
    borderRadius: '8px',
    padding: '16px',
    maxWidth: '300px',
    backgroundColor: isActive ? '#fff' : '#f5f5f5',
    boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
  };
  
  const imageStyle = {
    width: '80px',
    height: '80px',
    borderRadius: '50%',
    objectFit: 'cover',
    marginBottom: '12px',
  };
  
  const buttonStyle = {
    padding: '8px 16px',
    margin: '4px',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
    fontSize: '14px',
  };
  
  // Event handler function
  const handleEditClick = () => {
    if (onEdit) {
      onEdit(user);
    }
  };
  
  const handleDeleteClick = () => {
    if (onDelete) {
      // Confirm before deleting
      if (window.confirm(`Are you sure you want to delete ${name}?`)) {
        onDelete(user.id);
      }
    }
  };
  
  // Conditional rendering using ternary operator
  const roleBadge = role === 'admin' ? (
    <span style={{ 
      backgroundColor: '#9c27b0', 
      color: 'white', 
      padding: '2px 8px', 
      borderRadius: '4px',
      fontSize: '12px'
    }}>
      Admin
    </span>
  ) : role === 'moderator' ? (
    <span style={{ 
      backgroundColor: '#2196F3', 
      color: 'white', 
      padding: '2px 8px', 
      borderRadius: '4px',
      fontSize: '12px'
    }}>
      Moderator
    </span>
  ) : null;
  
  // Array rendering
  const skills = user.skills || ['JavaScript', 'React'];
  
  return (
    <div style={cardStyle}>
      {/* Image with alt text for accessibility */}
      <img 
        src={avatar} 
        alt={`${name}'s avatar`} 
        style={imageStyle}
      />
      
      {/* Name with conditional styling */}
      <h3 style={{ 
        margin: '0 0 8px 0',
        color: isActive ? '#333' : '#999'
      }}>
        {name}
      </h3>
      
      {/* Email */}
      <p style={{ margin: '0 0 8px 0', color: '#666' }}>
        {email}
      </p>
      
      {/* Role badge */}
      {roleBadge}
      
      {/* Skills list */}
      <div style={{ marginTop: '12px' }}>
        <strong style={{ fontSize: '12px', color: '#666' }}>Skills:</strong>
        <ul style={{ 
          margin: '4px 0 0 0', 
          paddingLeft: '20px',
          fontSize: '14px'
        }}>
          {/* Map over skills array */}
          {skills.map((skill, index) => (
            <li key={index}>{skill}</li>
          ))}
        </ul>
      </div>
      
      {/* Action buttons with onClick handlers (camelCase) */}
      <div style={{ marginTop: '16px' }}>
        <button 
          style={{ ...buttonStyle, backgroundColor: '#4CAF50', color: 'white' }}
          onClick={handleEditClick}
        >
          Edit
        </button>
        <button 
          style={{ ...buttonStyle, backgroundColor: '#f44336', color: 'white' }}
          onClick={handleDeleteClick}
        >
          Delete
        </button>
      </div>
      
      {/* Conditional rendering - only show if active */}
      {isActive && (
        <p style={{ 
          marginTop: '12px', 
          fontSize: '12px', 
          color: '#4CAF50' 
        }}>
          ✓ User is active
        </p>
      )}
    </div>
  );
}

export default UserCard;
```

## Key Takeaways

- JSX is JavaScript - it transforms into `React.createElement()` calls
- Use `className` instead of `class` and `htmlFor` instead of `for`
- All tags must be closed, including self-closing tags like `<img />`
- Wrap JavaScript expressions in curly braces `{}`
- Inline styles use camelCase and a JavaScript object
- Event handlers use camelCase (`onClick` not `onclick`)
- Use React Fragments (`<>...</>`) to return multiple elements
- Comments in JSX must be wrapped in curly braces: `{/* comment */}`

## What's Next

Now that you understand JSX syntax rules, let's dive deeper into using JavaScript expressions within JSX and explore more advanced patterns.
