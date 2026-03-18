# useState Complete Guide

## Overview

useState is the most fundamental React hook and the cornerstone of building interactive user interfaces. It allows you to add state to functional components, transforming them from static UI templates into dynamic, interactive applications. Understanding useState thoroughly is essential for any React developer, as it's the foundation upon which more complex state management is built.

## Prerequisites

- Basic understanding of React components
- Knowledge of JavaScript functions and ES6+ syntax
- Familiarity with JSX syntax

## Core Concepts

### What is useState?

useState is a Hook that lets you add state to functional components. Before hooks, you needed class components to have state. Now, with hooks, functional components can have their own internal state.

```jsx
// File: src/usestate-basics.jsx

import React, { useState } from 'react';

function Counter() {
  // useState returns an array with exactly two values:
  // 1. The current state value
  // 2. A function to update that state value
  const [count, setCount] = useState(0);
  
  // That's equivalent to:
  // const count = countState[0];
  // const setCount = countState[1];
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(count + 1)}>
        Increment
      </button>
    </div>
  );
}
```

### Initializing State

useState accepts an initial value that can be of any type:

```jsx
// File: src/usestate-init.jsx

import React, { useState } from 'react';

function Examples() {
  // Initial value can be a primitive
  const [name, setName] = useState('Alice');
  
  // Initial value can be a number
  const [age, setAge] = useState(25);
  
  // Initial value can be a boolean
  const [isActive, setIsActive] = useState(true);
  
  // Initial value can be an array
  const [items, setItems] = useState([]);
  
  // Initial value can be an object
  const [user, setUser] = useState({ name: 'Bob', email: '' });
  
  // Lazy initial state - pass a function for expensive computations
  const [data, setData] = useState(() => {
    // This function only runs on the initial render
    const initialData = computeExpensiveValue();
    return initialData;
  });
  
  return (
    <div>
      <p>Name: {name}</p>
      <p>Age: {age}</p>
      <p>Active: {isActive ? 'Yes' : 'No'}</p>
      <p>Items: {items.length}</p>
      <p>User: {user.name}</p>
    </div>
  );
}

function computeExpensiveValue() {
  // Expensive computation here
  return { data: 'some expensive data' };
}
```

### Updating State

The set function is used to update state. It triggers a re-render of the component:

```jsx
// File: src/usestate-updating.jsx

import React, { useState } from 'react';

function UpdateExamples() {
  const [count, setCount] = useState(0);
  const [user, setUser] = useState({ name: 'Alice', age: 25 });
  const [items, setItems] = useState(['apple', 'banana']);
  
  // Direct value update
  const increment = () => setCount(count + 1);
  
  // Functional update - use when new state depends on old state
  const incrementFunctional = () => setCount(prevCount => prevCount + 1);
  
  // Update object - must include all properties (shallow merge)
  const updateName = () => {
    setUser(prev => ({ ...prev, name: 'Bob' }));
  };
  
  // Update array
  const addItem = () => {
    setItems(prev => [...prev, 'cherry']);
  };
  
  const removeItem = (index) => {
    setItems(prev => prev.filter((_, i) => i !== index));
  };
  
  // Toggle boolean
  const [isOn, setIsOn] = useState(false);
  const toggle = () => setIsOn(prev => !prev);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={increment}>Increment</button>
      <button onClick={incrementFunctional}>Increment (Functional)</button>
      
      <p>User: {user.name}, Age: {user.age}</p>
      <button onClick={updateName}>Update Name</button>
      
      <ul>
        {items.map((item, index) => (
          <li key={index}>
            {item}
            <button onClick={() => removeItem(index)}>Remove</button>
          </li>
        ))}
      </ul>
      <button onClick={addItem}>Add Item</button>
      
      <p>Switch: {isOn ? 'ON' : 'OFF'}</p>
      <button onClick={toggle}>Toggle</button>
    </div>
  );
}
```

### Multiple State Variables

You can use multiple useState calls for different pieces of state:

```jsx
// File: src/usestate-multiple.jsx

import React, { useState } from 'react';

function Form() {
  // Separate state for each piece of data
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  // Or group related state
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: ''
  });
  
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };
  
  return (
    <form>
      <input
        name="name"
        value={formData.name}
        onChange={handleChange}
        placeholder="Name"
      />
      <input
        name="email"
        value={formData.email}
        onChange={handleChange}
        placeholder="Email"
      />
      <input
        name="password"
        type="password"
        value={formData.password}
        onChange={handleChange}
        placeholder="Password"
      />
    </form>
  );
}
```

## Common Mistakes

### Mistake 1: Not Using Functional Updates

```jsx
// ❌ WRONG - May not work correctly with async updates
function BadCounter() {
  const [count, setCount] = useState(0);
  
  const increment = () => {
    setCount(count + 1); // If called rapidly, might not work
    setCount(count + 1);
    setCount(count + 1); // All use stale count value
  };
  
  return <button onClick={increment}>{count}</button>;
}

// ✅ CORRECT - Use functional updates
function GoodCounter() {
  const [count, setCount] = useState(0);
  
  const increment = () => {
    setCount(prev => prev + 1);
    setCount(prev => prev + 1);
    setCount(prev => prev + 1); // Each uses previous value
  };
  
  return <button onClick={increment}>{count}</button>;
}
```

### Mistake 2: Not Copying Objects When Updating

```jsx
// ❌ WRONG - Mutating state directly
function BadComponent() {
  const [user, setUser] = useState({ name: 'Alice', age: 25 });
  
  const updateAge = () => {
    user.age = 30; // Direct mutation!
    setUser(user); // Won't trigger re-render properly
  };
  
  return <button onClick={updateAge}>{user.age}</button>;
}

// ✅ CORRECT - Always create new objects
function GoodComponent() {
  const [user, setUser] = useState({ name: 'Alice', age: 25 });
  
  const updateAge = () => {
    setUser(prev => ({ ...prev, age: 30 }));
  };
  
  return <button onClick={updateAge}>{user.age}</button>;
}
```

### Mistake 3: Using State for Derived Data

```jsx
// ❌ WRONG - Unnecessary state
function BadComponent({ items }) {
  const [count, setCount] = useState(items.length); // Redundant!
  
  return <p>Count: {count}</p>;
}

// ✅ CORRECT - Compute derived data during render
function GoodComponent({ items }) {
  const count = items.length; // No state needed!
  
  return <p>Count: {count}</p>;
}
```

## Real-World Example

Let's build a complete todo application using useState:

```jsx
// File: src/components/TodoApp.jsx

import React, { useState } from 'react';

function TodoApp() {
  // State for todo list
  const [todos, setTodos] = useState([
    { id: 1, text: 'Learn React', completed: false },
    { id: 2, text: 'Build a project', completed: true },
    { id: 3, text: 'Deploy to production', completed: false }
  ]);
  
  // State for input
  const [inputValue, setInputValue] = useState('');
  
  // State for filter
  const [filter, setFilter] = useState('all'); // 'all', 'active', 'completed'
  
  // Add new todo
  const addTodo = (e) => {
    e.preventDefault();
    
    if (!inputValue.trim()) return;
    
    const newTodo = {
      id: Date.now(),
      text: inputValue.trim(),
      completed: false
    };
    
    setTodos(prev => [...prev, newTodo]);
    setInputValue('');
  };
  
  // Toggle todo completion
  const toggleTodo = (id) => {
    setTodos(prev => prev.map(todo =>
      todo.id === id
        ? { ...todo, completed: !todo.completed }
        : todo
    ));
  };
  
  // Delete todo
  const deleteTodo = (id) => {
    setTodos(prev => prev.filter(todo => todo.id !== id));
  };
  
  // Clear completed todos
  const clearCompleted = () => {
    setTodos(prev => prev.filter(todo => !todo.completed));
  };
  
  // Filter todos
  const filteredTodos = todos.filter(todo => {
    if (filter === 'active') return !todo.completed;
    if (filter === 'completed') return todo.completed;
    return true;
  });
  
  // Computed values
  const activeCount = todos.filter(t => !t.completed).length;
  const completedCount = todos.filter(t => t.completed).length;
  
  // Styles
  const styles = {
    container: {
      maxWidth: '500px',
      margin: '50px auto',
      padding: '20px',
      fontFamily: 'Arial, sans-serif'
    },
    input: {
      width: '70%',
      padding: '10px',
      fontSize: '16px',
      border: '1px solid #ddd',
      borderRadius: '4px 0 0 4px',
      borderRight: 'none'
    },
    addButton: {
      width: '25%',
      padding: '10px',
      fontSize: '16px',
      backgroundColor: '#4CAF50',
      color: 'white',
      border: 'none',
      borderRadius: '0 4px 4px 0',
      cursor: 'pointer'
    },
    filterButtons: {
      display: 'flex',
      gap: '5px',
      margin: '20px 0'
    },
    filterButton: {
      padding: '8px 12px',
      border: '1px solid #ddd',
      backgroundColor: 'white',
      cursor: 'pointer',
      borderRadius: '4px'
    },
    todoItem: {
      display: 'flex',
      alignItems: 'center',
      padding: '10px',
      borderBottom: '1px solid #eee',
      backgroundColor: 'white'
    },
    todoText: {
      flex: 1,
      marginLeft: '10px',
      textDecoration: 'none'
    },
    deleteButton: {
      backgroundColor: '#f44336',
      color: 'white',
      border: 'none',
      padding: '6px 10px',
      borderRadius: '4px',
      cursor: 'pointer',
      marginLeft: '10px'
    },
    stats: {
      marginTop: '20px',
      padding: '10px',
      backgroundColor: '#f5f5f5',
      borderRadius: '4px',
      fontSize: '14px'
    }
  };
  
  return (
    <div style={styles.container}>
      <h1>Todo List</h1>
      
      {/* Add todo form */}
      <form onSubmit={addTodo} style={{ display: 'flex' }}>
        <input
          type="text"
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
          placeholder="What needs to be done?"
          style={styles.input}
        />
        <button type="submit" style={styles.addButton}>
          Add
        </button>
      </form>
      
      {/* Filter buttons */}
      <div style={styles.filterButtons}>
        {['all', 'active', 'completed'].map(f => (
          <button
            key={f}
            onClick={() => setFilter(f)}
            style={{
              ...styles.filterButton,
              backgroundColor: filter === f ? '#2196F3' : 'white',
              color: filter === f ? 'white' : 'black'
            }}
          >
            {f.charAt(0).toUpperCase() + f.slice(1)}
          </button>
        ))}
      </div>
      
      {/* Todo list */}
      <ul style={{ listStyle: 'none', padding: 0, margin: 0 }}>
        {filteredTodos.map(todo => (
          <li key={todo.id} style={styles.todoItem}>
            <input
              type="checkbox"
              checked={todo.completed}
              onChange={() => toggleTodo(todo.id)}
            />
            <span
              style={{
                ...styles.todoText,
                textDecoration: todo.completed ? 'line-through' : 'none',
                color: todo.completed ? '#999' : '#333'
              }}
            >
              {todo.text}
            </span>
            <button
              onClick={() => deleteTodo(todo.id)}
              style={styles.deleteButton}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
      
      {filteredTodos.length === 0 && (
        <p style={{ textAlign: 'center', color: '#666' }}>
          No todos found
        </p>
      )}
      
      {/* Stats */}
      <div style={styles.stats}>
        <span>{activeCount} item{activeCount !== 1 ? 's' : ''} left</span>
        {completedCount > 0 && (
          <button
            onClick={clearCompleted}
            style={{
              float: 'right',
              border: 'none',
              background: 'none',
              color: '#666',
              cursor: 'pointer',
              textDecoration: 'underline'
            }}
          >
            Clear completed ({completedCount})
          </button>
        )}
      </div>
    </div>
  );
}

export default TodoApp;
```

## Key Takeaways

- useState returns an array with the current state value and a setter function
- Use the functional form of setState when new state depends on previous state
- Always create new objects/arrays when updating state, never mutate
- Don't store derived data in state - compute it during render
- useState can hold any type of value, including objects and arrays
- The setter function triggers a re-render of the component
- Use lazy initialization for expensive initial state computations

## What's Next

Now that you understand useState, let's explore useEffect - the hook for handling side effects in your components.
