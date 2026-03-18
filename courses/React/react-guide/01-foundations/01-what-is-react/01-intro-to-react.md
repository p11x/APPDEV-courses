# Introduction to React

## Overview

React is a JavaScript library for building user interfaces, developed by Facebook (now Meta) and first released in 2013. It has become the most popular frontend library in the world, used by companies like Netflix, Airbnb, Instagram, and WhatsApp to build fast, interactive web applications. React introduces a component-based architecture that allows you to build reusable, self-contained pieces of UI that manage their own state and render logic. This guide will take you from understanding what React is to building production-ready applications.

## Prerequisites

- Basic understanding of HTML, CSS, and JavaScript
- Familiarity with ES6+ JavaScript features (arrow functions, destructuring, modules)
- Knowledge of how the web works (HTTP requests, browser rendering)
- A code editor installed (VS Code recommended)

## Core Concepts

### What Makes React Special

React stands out from other JavaScript frameworks because of its unique approach to rendering user interfaces. Instead of directly manipulating the browser's Document Object Model (DOM), React maintains a virtual representation of the DOM in memory. When your application's state changes, React compares the new virtual DOM with the previous one (a process called "reconciliation") and only updates the actual DOM elements that have changed. This approach, known as the virtual DOM, makes React applications incredibly fast and efficient.

The component-based architecture is another key differentiator. In React, everything is a component - from a simple button to an entire page layout. Components are like LEGO blocks: you build small, reusable pieces and combine them to create complex UIs. Each component can have its own internal state and logic, making your code modular and easy to maintain.

React also uses a declarative syntax called JSX, which allows you to write HTML-like code directly in your JavaScript files. This makes your code more readable and easier to understand, as you can see exactly what your UI will look like. React handles the complex DOM manipulation behind the scenes, so you can focus on building your application's functionality.

### Your First React Component

Let's create a simple React component to understand the basics. We'll build a greeting component that displays a personalized message.

```jsx
// File: src/components/Greeting.jsx

// React must be imported in every component file, even if you don't 
// explicitly use it in JSX (the import is needed for JSX transformation)
import React from 'react';

// This is a functional component - the modern way to create components in React.
// Components are functions that return JSX (JavaScript XML) describing what to render.
function Greeting({ name, age }) {
  // Props are the parameters passed to the component, similar to function arguments.
  // We use destructuring to extract 'name' and 'age' from the props object.
  // This makes the code cleaner and avoids writing props.name every time.
  
  // Conditional rendering: if name exists, show the greeting, otherwise show default
  const displayName = name ?? 'Guest'; // nullish coalescing - uses 'Guest' if name is null/undefined
  
  // We can include JavaScript expressions inside curly braces {}
  const currentYear = new Date().getFullYear();
  
  // Calculate birth year from age (with fallback if age is not provided)
  const birthYear = age ? currentYear - age : 'unknown';
  
  return (
    // JSX looks like HTML but it's actually JavaScript. The className attribute
    // is used instead of class (since class is a reserved word in JavaScript).
    <div className="greeting-card">
      {/* This is a comment inside JSX - note the curly braces around it */}
      <h1>Hello, {displayName}!</h1>
      <p>Welcome to our React application.</p>
      
      {/* Conditional rendering with ternary operator */}
      {age ? (
        <p>You were born around {birthYear}.</p>
      ) : (
        <p>Feel free to share your age with us.</p>
      )}
      
      {/* Inline styles - note the camelCase property names */}
      <button 
        style={{ 
          backgroundColor: '#4CAF50', 
          color: 'white', 
          padding: '10px 20px',
          border: 'none',
          borderRadius: '4px',
          cursor: 'pointer'
        }}
        onClick={() => alert(`Hi, ${displayName}!`)}
      >
        Say Hello
      </button>
    </div>
  );
}

// Export the component so it can be imported and used in other files
export default Greeting;
```

Now let's see how to use this component in your application:

```jsx
// File: src/App.jsx

// Import the Greeting component we just created
import Greeting from './components/Greeting';
import React from 'react';

function App() {
  // The App component is the root component of your React application.
  // In a real app, this would contain your navigation, routing, and main layout.
  
  return (
    <div className="app">
      {/* Using the Greeting component with different props */}
      <Greeting name="Alice" age={28} />
      <Greeting name="Bob" />
      <Greeting />
    </div>
  );
}

export default App;
```

### Understanding JSX

JSX is a syntax extension for JavaScript that lets you write HTML-like markup inside JavaScript files. It's not a separate template language - everything you write in JSX gets transformed into regular JavaScript function calls. Under the hood, JSX elements become calls to `React.createElement()`.

```jsx
// File: src/jsx-explained.jsx

import React from 'react';

// This JSX:
const element = <h1 className="title">Hello, World!</h1>;

// Gets transformed into this JavaScript:
const element = React.createElement(
  'h1',
  { className: 'title' },
  'Hello, World!'
);

// You can nest elements just like HTML:
const container = (
  <div className="container">
    <h1>My App</h1>
    <p>This is a paragraph.</p>
  </div>
);

// JavaScript expressions work inside curly braces:
const name = 'John';
const greeting = <p>Hello, {name}!</p>;

// You can use any valid JavaScript expression inside {}:
const calculate = <p>2 + 2 = {2 + 2}</p>; // Outputs: 2 + 2 = 4
const array = [1, 2, 3];
const items = <ul>{array.map(item => <li key={item}>{item}</li>)}</ul>;
```

## Common Mistakes

### Mistake 1: Forgetting to Import React

```jsx
// ❌ WRONG - Forgetting to import React
function MyComponent() {
  return <div>Hello</div>;
}

// ✅ CORRECT - Always import React at the top of files containing JSX
import React from 'react';

function MyComponent() {
  return <div>Hello</div>;
}
```

### Mistake 2: Using class Instead of className

```jsx
// ❌ WRONG - class is a reserved word in JavaScript
function MyComponent() {
  return <div class="container">Content</div>;
}

// ✅ CORRECT - Use className instead
function MyComponent() {
  return <div className="container">Content</div>;
}
```

### Mistake 3: Returning Multiple Elements Without a Wrapper

```jsx
// ❌ WRONG - Can't return multiple elements directly
function MyComponent() {
  return (
    <h1>Title</h1>
    <p>Content</p>
  );
}

// ✅ CORRECT - Wrap in a container element
function MyComponent() {
  return (
    <div>
      <h1>Title</h1>
      <p>Content</p>
    </div>
  );
}

// Or use React Fragments (preferred for avoiding extra DOM nodes)
function MyComponent() {
  return (
    <>
      <h1>Title</h1>
      <p>Content</p>
    </>
  );
}
```

## Real-World Example

Let's build a simple todo application to see React in action:

```jsx
// File: src/components/TodoApp.jsx

import React, { useState } from 'react';

// Main App component
function TodoApp() {
  // useState is a hook that lets you add state to functional components
  // [todos, setTodos] = destructured state: current value and setter function
  const [todos, setTodos] = useState([
    { id: 1, text: 'Learn React', completed: true },
    { id: 2, text: 'Build a project', completed: false },
    { id: 3, text: 'Deploy to production', completed: false }
  ]);
  
  const [inputValue, setInputValue] = useState('');
  
  // Handler to add a new todo
  const handleAddTodo = () => {
    if (inputValue.trim() === '') return; // Don't add empty todos
    
    const newTodo = {
      id: Date.now(), // Simple unique ID using timestamp
      text: inputValue.trim(),
      completed: false
    };
    
    setTodos([...todos, newTodo]); // Spread operator to add new item
    setInputValue(''); // Clear input after adding
  };
  
  // Handler to toggle todo completion
  const toggleTodo = (id) => {
    setTodos(todos.map(todo => 
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  };
  
  // Handler to delete a todo
  const deleteTodo = (id) => {
    setTodos(todos.filter(todo => todo.id !== id));
  };
  
  return (
    <div style={{ maxWidth: '400px', margin: '50px auto', padding: '20px' }}>
      <h1>My Todo List</h1>
      
      {/* Input form */}
      <div style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>
        <input
          type="text"
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
          placeholder="Add a new task..."
          style={{ flex: 1, padding: '8px', fontSize: '16px' }}
          onKeyDown={(e) => e.key === 'Enter' && handleAddTodo()}
        />
        <button 
          onClick={handleAddTodo}
          style={{ padding: '8px 16px', backgroundColor: '#4CAF50', color: 'white', border: 'none', cursor: 'pointer' }}
        >
          Add
        </button>
      </div>
      
      {/* Todo list */}
      <ul style={{ listStyle: 'none', padding: 0 }}>
        {todos.map(todo => (
          <li 
            key={todo.id} 
            style={{ 
              display: 'flex', 
              alignItems: 'center', 
              padding: '10px', 
              borderBottom: '1px solid #eee',
              textDecoration: todo.completed ? 'line-through' : 'none',
              color: todo.completed ? '#999' : '#333'
            }}
          >
            <input
              type="checkbox"
              checked={todo.completed}
              onChange={() => toggleTodo(todo.id)}
              style={{ marginRight: '10px' }}
            />
            <span style={{ flex: 1 }}>{todo.text}</span>
            <button
              onClick={() => deleteTodo(todo.id)}
              style={{ 
                backgroundColor: '#f44336', 
                color: 'white', 
                border: 'none', 
                padding: '5px 10px', 
                cursor: 'pointer',
                borderRadius: '3px'
              }}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
      
      {/* Stats footer */}
      <p style={{ color: '#666', marginTop: '20px' }}>
        {todos.filter(t => !t.completed).length} tasks remaining
      </p>
    </div>
  );
}

export default TodoApp;
```

## Key Takeaways

- React is a JavaScript library for building user interfaces with a component-based architecture
- Components are reusable, self-contained pieces of UI that can accept props and manage state
- JSX allows you to write HTML-like syntax in JavaScript, which gets transformed into `React.createElement()` calls
- The virtual DOM is React's secret sauce - it minimizes actual DOM manipulations for better performance
- React uses `className` instead of `class`, and you must import React in files using JSX
- Use React Fragments (`<>...</>`) when you need to return multiple elements without adding extra DOM nodes

## What's Next

Now that you understand the basics of React, continue to the next file to learn about the Virtual DOM and how React uses it for efficient rendering.
