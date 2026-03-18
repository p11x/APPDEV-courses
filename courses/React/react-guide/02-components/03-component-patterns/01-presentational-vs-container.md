# Presentational vs Container Components

## Overview

The Presentational and Container component pattern (also known as Smart vs Dumb components, or Stateless vs Stateful components) is a powerful architectural pattern that helps separate concerns in your React application. This pattern divides components into two categories based on their responsibility: how things look (presentational) vs how things work (container). Understanding this pattern will help you build more maintainable and testable React applications.

## Prerequisites

- Understanding of React components and props
- Knowledge of React state and effects
- Familiarity with component composition

## Core Concepts

### What are Presentational Components?

Presentational components focus on how things look. They receive data through props and render UI. They don't know where data comes from or how to change it. They're often called "dumb" or "stateless" components, but they can have local UI state.

```jsx
// File: src/presentational/UserCard.jsx

import React from 'react';

// Presentational component - focuses on rendering UI
function UserCard({ name, email, avatar, role, onEdit, onDelete }) {
  // Styles are defined inline or in CSS modules
  const styles = {
    card: {
      border: '1px solid #ddd',
      borderRadius: '8px',
      padding: '16px',
      display: 'flex',
      alignItems: 'center',
      gap: '16px',
      backgroundColor: 'white'
    },
    avatar: {
      width: '60px',
      height: '60px',
      borderRadius: '50%',
      objectFit: 'cover'
    },
    info: {
      flex: 1
    },
    name: {
      margin: '0 0 4px 0',
      fontSize: '18px',
      fontWeight: '500'
    },
    email: {
      margin: 0,
      color: '#666',
      fontSize: '14px'
    },
    role: {
      display: 'inline-block',
      padding: '2px 8px',
      borderRadius: '4px',
      fontSize: '12px',
      backgroundColor: '#e3f2fd',
      color: '#1976d2'
    },
    actions: {
      display: 'flex',
      gap: '8px'
    },
    button: {
      padding: '8px 12px',
      border: 'none',
      borderRadius: '4px',
      cursor: 'pointer',
      fontSize: '14px'
    }
  };
  
  return (
    <div style={styles.card}>
      <img src={avatar} alt={name} style={styles.avatar} />
      
      <div style={styles.info}>
        <h3 style={styles.name}>{name}</h3>
        <p style={styles.email}>{email}</p>
        <span style={styles.role}>{role}</span>
      </div>
      
      <div style={styles.actions}>
        <button 
          style={{ ...styles.button, backgroundColor: '#2196F3', color: 'white' }}
          onClick={() => onEdit?.()}
        >
          Edit
        </button>
        <button 
          style={{ ...styles.button, backgroundColor: '#f44336', color: 'white' }}
          onClick={() => onDelete?.()}
        >
          Delete
        </button>
      </div>
    </div>
  );
}

// This component only renders - it doesn't fetch data or manage state
export default UserCard;
```

### What are Container Components?

Container components focus on how things work. They manage state, handle data fetching, and provide data and callbacks to presentational components. They're often called "smart" or "stateful" components.

```jsx
// File: src/containers/UserListContainer.jsx

import React, { useState, useEffect } from 'react';
import UserCard from '../presentational/UserCard';

// Container component - manages data and state
function UserListContainer() {
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  
  // Data fetching - this is container responsibility
  useEffect(() => {
    async function fetchUsers() {
      try {
        setIsLoading(true);
        const response = await fetch('/api/users');
        if (!response.ok) throw new Error('Failed to fetch users');
        const data = await response.json();
        setUsers(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setIsLoading(false);
      }
    }
    
    fetchUsers();
  }, []);
  
  // Event handlers
  const handleEdit = (userId) => {
    console.log('Edit user:', userId);
    // Navigate to edit page or open modal
  };
  
  const handleDelete = async (userId) => {
    if (!window.confirm('Are you sure?')) return;
    
    try {
      await fetch(`/api/users/${userId}`, { method: 'DELETE' });
      setUsers(users.filter(u => u.id !== userId));
    } catch (err) {
      alert('Failed to delete user');
    }
  };
  
  // Loading state
  if (isLoading) {
    return <div>Loading users...</div>;
  }
  
  // Error state
  if (error) {
    return <div>Error: {error}</div>;
  }
  
  // Render presentational components with data
  return (
    <div>
      <h2>Users</h2>
      {users.map(user => (
        <UserCard
          key={user.id}
          name={user.name}
          email={user.email}
          avatar={user.avatar}
          role={user.role}
          onEdit={() => handleEdit(user.id)}
          onDelete={() => handleDelete(user.id)}
        />
      ))}
    </div>
  );
}

export default UserListContainer;
```

### Benefits of the Pattern

```jsx
// File: src/benefits-example.jsx

import React, { useState, useEffect } from 'react';

// ==================== PRESENTATIONAL COMPONENTS ====================
// These are:
// - Reusable across the app
// - Easy to test (no mocking needed)
// - Focus on UI, not business logic
// - Can be styled in various ways
// - Receive everything via props

function TodoItem({ todo, onToggle, onDelete }) {
  return (
    <div className={`todo-item ${todo.completed ? 'completed' : ''}`}>
      <input
        type="checkbox"
        checked={todo.completed}
        onChange={() => onToggle(todo.id)}
      />
      <span>{todo.text}</span>
      <button onClick={() => onDelete(todo.id)}>Delete</button>
    </div>
  );
}

function TodoList({ todos, onToggle, onDelete }) {
  if (todos.length === 0) {
    return <p>No todos yet</p>;
  }
  
  return (
    <ul>
      {todos.map(todo => (
        <TodoItem
          key={todo.id}
          todo={todo}
          onToggle={onToggle}
          onDelete={onDelete}
        />
      ))}
    </ul>
  );
}

function TodoForm({ onAdd }) {
  const [text, setText] = useState('');
  
  const handleSubmit = (e) => {
    e.preventDefault();
    if (!text.trim()) return;
    onAdd(text);
    setText('');
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <input
        value={text}
        onChange={(e) => setText(e.target.value)}
        placeholder="Add a todo..."
      />
      <button type="submit">Add</button>
    </form>
  );
}

// ==================== CONTAINER COMPONENT ====================
// This handles:
// - State management
// - API calls
// - Business logic
// - Provides data to presentational components

function TodoApp() {
  const [todos, setTodos] = useState([]);
  
  // Load from localStorage on mount
  useEffect(() => {
    const saved = localStorage.getItem('todos');
    if (saved) {
      setTodos(JSON.parse(saved));
    }
  }, []);
  
  // Save to localStorage when todos change
  useEffect(() => {
    localStorage.setItem('todos', JSON.stringify(todos));
  }, [todos]);
  
  const addTodo = (text) => {
    setTodos([...todos, {
      id: Date.now(),
      text,
      completed: false
    }]);
  };
  
  const toggleTodo = (id) => {
    setTodos(todos.map(todo =>
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  };
  
  const deleteTodo = (id) => {
    setTodos(todos.filter(todo => todo.id !== id));
  };
  
  // Just renders presentational components
  return (
    <div>
      <h1>Todos</h1>
      <TodoForm onAdd={addTodo} />
      <TodoList
        todos={todos}
        onToggle={toggleTodo}
        onDelete={deleteTodo}
      />
    </div>
  );
}
```

### When to Use Each Type

```jsx
// File: src/when-to-use.jsx

import React from 'react';

// PRESENTATIONAL - Use when:
// - Component only renders UI
// - Receives data and callbacks via props
// - Doesn't need React lifecycle methods
// - Can be reused in different contexts

// Good presentational components:
function Button({ children, onClick, variant }) { /* ... */ }
function Input({ value, onChange, placeholder }) { /* ... */ }
function Modal({ isOpen, onClose, children }) { /* ... */ }
function LoadingSpinner({ size }) { /* ... */ }
function ErrorMessage({ message }) { /* ... */ }

// CONTAINER - Use when:
// - Component manages state or lifecycle
// - Fetches or transforms data
// - Provides callbacks to children
// - Coordinates multiple child components

// Good container components:
function UserProfile({ userId }) { /* fetches user data */ }
function DataTable({ data, columns }) { /* sorts, filters data */ }
function AuthWrapper({ children }) { /* handles auth state */ }
function ThemeProvider({ children }) { /* provides theme context */ }
```

## Common Mistakes

### Mistake 1: Mixing Concerns in One Component

```jsx
// ❌ WRONG - Component does too much
function BadComponent() {
  const [data, setData] = useState([]);
  const [filter, setFilter] = useState('');
  
  useEffect(() => {
    fetchData();
  }, []);
  
  // Also renders UI!
  return (
    <div>
      <input value={filter} onChange={e => setFilter(e.target.value)} />
      {data
        .filter(item => item.name.includes(filter))
        .map(item => <div key={item.id}>{item.name}</div>)}
    </div>
  );
}

// ✅ CORRECT - Separate concerns
function DataContainer() {
  const [data, setData] = useState([]);
  
  useEffect(() => {
    fetchData();
  }, []);
  
  return <DataList data={data} />;
}

function DataList({ data }) {
  const [filter, setFilter] = useState('');
  
  const filteredData = data.filter(item => 
    item.name.includes(filter)
  );
  
  return (
    <div>
      <input value={filter} onChange={e => setFilter(e.target.value)} />
      {filteredData.map(item => <div key={item.id}>{item.name}</div>)}
    </div>
  );
}
```

### Mistake 2: Presentational Components Having Too Much State

```jsx
// ❌ WRONG - Presentational component managing too much state
function BadCard() {
  const [isExpanded, setIsExpanded] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [editValue, setEditValue] = useState('');
  // This is a container responsibility!
  
  return <div>{/* ... */}</div>;
}

// ✅ CORRECT - Presentational receives state from container
function GoodCard({ isExpanded, onToggle, children }) {
  return <div onClick={onToggle}>{children}</div>;
}
```

### Mistake 3: Not Passing Callbacks Properly

```jsx
// ❌ WRONG - Container doesn't pass handlers
function BadContainer() {
  const [items, setItems] = useState([]);
  // Forgot to pass handlers!
  return <PresentationalList items={items} />;
}

// ✅ CORRECT - Container provides everything presentational needs
function GoodContainer() {
  const [items, setItems] = useState([]);
  
  const handleDelete = (id) => {
    setItems(items.filter(i => i.id !== id));
  };
  
  return (
    <PresentationalList
      items={items}
      onDelete={handleDelete}
    />
  );
}
```

## Real-World Example

```jsx
// File: src/components/TodoAppComplete.jsx

import React, { useState, useEffect, useCallback } from 'react';

// ==================== PRESENTATIONAL COMPONENTS ====================

function TodoItem({ todo, onToggle, onDelete }) {
  return (
    <li style={{
      display: 'flex',
      alignItems: 'center',
      padding: '12px',
      borderBottom: '1px solid #eee',
      backgroundColor: todo.completed ? '#f9f9f9' : 'white'
    }}>
      <input
        type="checkbox"
        checked={todo.completed}
        onChange={() => onToggle(todo.id)}
        style={{ marginRight: '12px', width: '20px', height: '20px' }}
      />
      <span style={{
        flex: 1,
        textDecoration: todo.completed ? 'line-through' : 'none',
        color: todo.completed ? '#999' : '#333'
      }}>
        {todo.text}
      </span>
      <span style={{ color: '#666', fontSize: '12px', marginRight: '12px' }}>
        {new Date(todo.createdAt).toLocaleDateString()}
      </span>
      <button
        onClick={() => onDelete(todo.id)}
        style={{
          backgroundColor: '#f44336',
          color: 'white',
          border: 'none',
          padding: '6px 12px',
          borderRadius: '4px',
          cursor: 'pointer'
        }}
      >
        Delete
      </button>
    </li>
  );
}

function TodoList({ todos, onToggle, onDelete }) {
  const activeTodos = todos.filter(t => !t.completed);
  const completedTodos = todos.filter(t => t.completed);
  
  return (
    <div>
      {activeTodos.length > 0 && (
        <>
          <h3 style={{ color: '#333' }}>Active ({activeTodos.length})</h3>
          <ul style={{ listStyle: 'none', padding: 0 }}>
            {activeTodos.map(todo => (
              <TodoItem
                key={todo.id}
                todo={todo}
                onToggle={onToggle}
                onDelete={onDelete}
              />
            ))}
          </ul>
        </>
      )}
      
      {completedTodos.length > 0 && (
        <>
          <h3 style={{ color: '#666', marginTop: '20px' }}>
            Completed ({completedTodos.length})
          </h3>
          <ul style={{ listStyle: 'none', padding: 0 }}>
            {completedTodos.map(todo => (
              <TodoItem
                key={todo.id}
                todo={todo}
                onToggle={onToggle}
                onDelete={onDelete}
              />
            ))}
          </ul>
        </>
      )}
      
      {todos.length === 0 && (
        <p style={{ textAlign: 'center', color: '#666', padding: '40px' }}>
          No todos yet. Add one below!
        </p>
      )}
    </div>
  );
}

function TodoForm({ onAdd }) {
  const [text, setText] = useState('');
  
  const handleSubmit = (e) => {
    e.preventDefault();
    if (!text.trim()) return;
    onAdd(text.trim());
    setText('');
  };
  
  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: '20px' }}>
      <input
        type="text"
        value={text}
        onChange={(e) => setText(e.target.value)}
        placeholder="What needs to be done?"
        style={{
          width: '70%',
          padding: '12px',
          fontSize: '16px',
          border: '1px solid #ddd',
          borderRadius: '4px 0 0 4px',
          borderRight: 'none'
        }}
      />
      <button
        type="submit"
        style={{
          width: '30%',
          padding: '12px',
          fontSize: '16px',
          backgroundColor: '#4CAF50',
          color: 'white',
          border: 'none',
          borderRadius: '0 4px 4px 0',
          cursor: 'pointer'
        }}
      >
        Add Todo
      </button>
    </form>
  );
}

function FilterButtons({ currentFilter, onFilterChange }) {
  const filters = ['all', 'active', 'completed'];
  
  return (
    <div style={{ display: 'flex', gap: '8px', marginBottom: '20px' }}>
      {filters.map(filter => (
        <button
          key={filter}
          onClick={() => onFilterChange(filter)}
          style={{
            padding: '8px 16px',
            backgroundColor: currentFilter === filter ? '#2196F3' : '#f5f5f5',
            color: currentFilter === filter ? 'white' : '#333',
            border: '1px solid #ddd',
            borderRadius: '4px',
            cursor: 'pointer',
            textTransform: 'capitalize'
          }}
        >
          {filter}
        </button>
      ))}
    </div>
  );
}

// ==================== CONTAINER COMPONENT ====================

function TodoApp() {
  const [todos, setTodos] = useState([]);
  const [filter, setFilter] = useState('all');
  const [isLoading, setIsLoading] = useState(true);
  
  // Load todos from localStorage on mount
  useEffect(() => {
    const saved = localStorage.getItem('todos');
    if (saved) {
      try {
        setTodos(JSON.parse(saved));
      } catch (e) {
        console.error('Failed to parse todos');
      }
    }
    setIsLoading(false);
  }, []);
  
  // Save todos to localStorage when changed
  useEffect(() => {
    if (!isLoading) {
      localStorage.setItem('todos', JSON.stringify(todos));
    }
  }, [todos, isLoading]);
  
  const addTodo = useCallback((text) => {
    const newTodo = {
      id: Date.now(),
      text,
      completed: false,
      createdAt: new Date().toISOString()
    };
    setTodos(prev => [...prev, newTodo]);
  }, []);
  
  const toggleTodo = useCallback((id) => {
    setTodos(prev => prev.map(todo =>
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  }, []);
  
  const deleteTodo = useCallback((id) => {
    setTodos(prev => prev.filter(todo => todo.id !== id));
  }, []);
  
  const changeFilter = useCallback((newFilter) => {
    setFilter(newFilter);
  }, []);
  
  // Filter todos based on current filter
  const filteredTodos = todos.filter(todo => {
    if (filter === 'active') return !todo.completed;
    if (filter === 'completed') return todo.completed;
    return true;
  });
  
  if (isLoading) {
    return <div style={{ padding: '20px' }}>Loading...</div>;
  }
  
  return (
    <div style={{ 
      maxWidth: '600px', 
      margin: '40px auto', 
      padding: '20px',
      fontFamily: 'Arial, sans-serif'
    }}>
      <h1 style={{ textAlign: 'center', color: '#333' }}>Todo App</h1>
      
      <TodoForm onAdd={addTodo} />
      <FilterButtons currentFilter={filter} onFilterChange={changeFilter} />
      <TodoList
        todos={filteredTodos}
        onToggle={toggleTodo}
        onDelete={deleteTodo}
      />
      
      <p style={{ textAlign: 'center', color: '#666', marginTop: '20px' }}>
        {todos.filter(t => !t.completed).length} items left
      </p>
    </div>
  );
}

export default TodoApp;
```

## Key Takeaways

- Presentational components focus on "how it looks" - pure UI rendering
- Container components focus on "how it works" - state, data fetching, logic
- Presentational components are reusable and easy to test
- Container components coordinate multiple presentational components
- This separation improves maintainability and testability
- Use presentational for UI that can be reused
- Use containers for page-level logic and data management

## What's Next

Now let's explore another pattern - Higher-Order Components (HOCs). This pattern allows you to reuse component logic by wrapping components.
