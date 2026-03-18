# Project Structure Best Practices

## Overview

A well-organized project structure is crucial for maintainability and scalability in React applications. As your application grows, the way you organize files and folders becomes increasingly important. This guide covers proven patterns and best practices for structuring React projects that are easy to navigate, maintain, and scale. You'll learn different approaches and when to use each one.

## Prerequisites

- Basic understanding of React components
- Familiarity with JavaScript modules and imports
- Knowledge of how to create a React project (from previous lessons)
- Understanding of CSS and styling in React

## Core Concepts

### Common Project Structures

There are several ways to organize a React project, and the best choice depends on your project's size and complexity. Let's explore the most common approaches.

**1. Feature-Based Structure (Recommended for Medium-Large Apps)**

```text
src/
├── features/
│   ├── auth/
│   │   ├── components/
│   │   │   ├── LoginForm.jsx
│   │   │   └── ProtectedRoute.jsx
│   │   ├── hooks/
│   │   │   └── useAuth.js
│   │   ├── api/
│   │   │   └── authApi.js
│   │   └── index.js
│   ├── dashboard/
│   │   ├── components/
│   │   ├── hooks/
│   │   └── index.js
│   └── todos/
│       ├── components/
│       │   ├── TodoList.jsx
│       │   └── TodoItem.jsx
│       ├── hooks/
│       │   └── useTodos.js
│       └── index.js
├── components/           # Shared/common components
│   ├── Button.jsx
│   ├── Input.jsx
│   ├── Modal.jsx
│   └── Layout/
├── hooks/               # Shared custom hooks
│   ├── useLocalStorage.js
│   └── useFetch.js
├── utils/               # Utility functions
│   ├── formatDate.js
│   └── validation.js
├── styles/              # Global styles
├── App.jsx
└── main.jsx
```

**2. Type-Based Structure (Simple but Can Get Messy)**

```text
src/
├── components/
│   ├── Button.jsx
│   ├── Header.jsx
│   ├── Footer.jsx
│   └── ...
├── hooks/
│   ├── useAuth.js
│   └── useTodos.js
├── contexts/
│   ├── AuthContext.jsx
│   └── ThemeContext.jsx
├── pages/
│   ├── Home.jsx
│   ├── About.jsx
│   └── ...
├── services/
│   ├── api.js
│   └── auth.js
├── styles/
└── utils/
```

**3. Hybrid Structure (Good for Small-Medium Apps)**

```text
src/
├── components/          # Reusable UI components
│   ├── Button.jsx
│   ├── Card.jsx
│   └── Input.jsx
├── features/           # Feature-specific code (as needed)
│   └── auth/
│       ├── Login.jsx
│       └── useAuth.js
├── pages/              # Page-level components
│   ├── Home.jsx
│   ├── Dashboard.jsx
│   └── Settings.jsx
├── hooks/              # Shared hooks
├── utils/              # Utilities
├── styles/             # Global styles
├── App.jsx
└── main.jsx
```

### Organizing Components

Components should be organized in a way that makes them easy to find and reuse. Here are some best practices:

```jsx
// File: src/components/Button/Button.jsx

// Each component can have its own folder with related files
// This keeps everything together: component, styles, tests, types

import React from 'react';
import './Button.css';

// Component
function Button({ 
  children, 
  variant = 'primary', 
  size = 'medium',
  onClick,
  disabled = false,
  className = ''
}) {
  // Use meaningful prop names and provide sensible defaults
  const classes = `btn btn-${variant} btn-${size} ${className}`;
  
  return (
    <button 
      className={classes}
      onClick={onClick}
      disabled={disabled}
    >
      {children}
    </button>
  );
}

export default Button;
```

```css
/* File: src/components/Button/Button.css */

/* Keep component-specific styles in the same folder */
.btn {
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
  transition: all 0.2s;
}

.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-primary {
  background-color: #4CAF50;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background-color: #45a049;
}

.btn-secondary {
  background-color: #666;
  color: white;
}

.btn-small {
  padding: 6px 12px;
  font-size: 14px;
}

.btn-medium {
  padding: 10px 20px;
  font-size: 16px;
}

.btn-large {
  padding: 14px 28px;
  font-size: 18px;
}
```

```javascript
// File: src/components/Button/index.js

// Barrel export - makes importing easier
// Instead of: import Button from '../../components/Button/Button'
// You can:     import { Button } from '../../components/Button'
// Or:         import Button from '../../components/Button'

export { default } from './Button';
export { default as Button } from './Button';
// Export variants if you have them
// export * from './ButtonGroup';
```

### Naming Conventions

Consistent naming makes your codebase easier to understand:

```javascript
// File: src/naming-conventions.js

// ==================== FILE NAMES ====================
// Use PascalCase for component files
// Good: UserProfile.jsx, LoginForm.jsx, TodoList.jsx
// Bad:  userProfile.jsx, loginForm.jsx, todo-list.jsx

// Use camelCase for utility files
// Good: formatDate.js, useAuth.js, apiClient.js
// Bad:  format-date.js, UseAuth.js, API_Client.js

// Use kebab-case for configuration files
// Good: .eslintrc.js, vite.config.js, babel.config.js

// ==================== COMPONENT NAMES ====================
// Use PascalCase for component names
function UserProfile() { ... }
function LoginForm() { ... }

// ==================== HOOK NAMES ====================
// Always prefix custom hooks with "use"
function useAuth() { ... }
function useTodos() { ... }
function useLocalStorage() { ... }

// ==================== CONSTANTS ====================
// Use UPPER_SNAKE_CASE for true constants
const MAX_FILE_SIZE = 10 * 1024 * 1024;
const API_BASE_URL = 'https://api.example.com';
const COLORS = {
  primary: '#4CAF50',
  secondary: '#666'
} as const; // TypeScript const assertion
```

### Grouping Related Files

Keep related files together to improve discoverability:

```javascript
// File: src/feature-organization.js

// ==================== A FEATURE FOLDER MIGHT LOOK LIKE: ====================
// features/auth/
// ├── components/           # Auth-specific components
// │   ├── LoginForm.jsx
// │   ├── RegisterForm.jsx
// │   └── PasswordReset.jsx
// ├── hooks/                # Auth-specific hooks
// │   ├── useAuth.js
// │   └── useToken.js
// ├── services/             # API calls related to auth
// │   └── authApi.js
// ├── utils/                # Auth-related utilities
// │   └── tokenHelpers.js
// ├── types/                # TypeScript types (if using TS)
// │   └── auth.ts
// └── index.js              # Barrel export for the feature

// ==================== THIS MAKES IMPORTS CLEAN: ====================
// features/auth/index.js
export { default as LoginForm } from './components/LoginForm';
export { default as RegisterForm } from './components/RegisterForm';
export { default as useAuth } from './hooks/useAuth';
export { default as authApi } from './services/authApi';

// Now in other parts of the app:
import { LoginForm, useAuth, authApi } from '@/features/auth';
// Much cleaner than:
import LoginForm from '../../../features/auth/components/LoginForm';
```

## Common Mistakes

### Mistake 1: Creating Too Many Nested Folders

```text
<!-- ❌ WRONG - Too many nesting levels make paths long -->
src/
├── components/
│   └── ui/
│       └── buttons/
│           └── primary/
│               └── PrimaryButton.jsx

<!-- Import path becomes: -->
import PrimaryButton from '../../../../components/ui/buttons/primary/PrimaryButton';

// ✅ CORRECT - Keep nesting to 2-3 levels maximum
src/
├── components/
│   ├── ui/
│   │   ├── Button.jsx
│   │   └── Input.jsx
│   └── layout/
│       ├── Header.jsx
│       └── Footer.jsx
```

### Mistake 2: Mixing Concerns in One File

```jsx
// ❌ WRONG - One file doing too much
// src/App.jsx
function App() {
  // Contains routing
  // Contains layout
  // Contains state
  // Contains business logic
  // Contains styles
  // Too many responsibilities!
}

// ✅ CORRECT - Separate concerns
// src/App.jsx - Just routing and layout
function App() {
  return (
    <Router>
      <Layout>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/dashboard" element={<DashboardPage />} />
        </Routes>
      </Layout>
    </Router>
  );
}

// src/pages/HomePage.jsx - Page component
function HomePage() {
  return <h1>Welcome</h1>;
}

// src/components/layout/Layout.jsx - Layout component
function Layout({ children }) {
  return (
    <div>
      <Header />
      <main>{children}</main>
      <Footer />
    </div>
  );
}
```

### Mistake 3: Not Using Index Files for Clean Imports

```javascript
// ❌ WRONG - Long import paths
import Button from '../../components/ui/Button/Button';
import Input from '../../components/ui/Input/Input';
import Card from '../../components/ui/Card/Card';

// ✅ CORRECT - Use index files (barrel exports)
import { Button, Input, Card } from '../../components/ui';

// components/ui/index.js
export { default as Button } from './Button/Button';
export { default as Input } from './Input/Input';
export { default as Card } from './Card/Card';
```

## Real-World Example

Let's build a complete project structure for a todo application:

```text
<!-- File: todo-app-structure.txt -->

src/
├── components/                    # Shared components
│   ├── Button/
│   │   ├── Button.jsx
│   │   ├── Button.css
│   │   └── index.js
│   ├── Input/
│   │   ├── Input.jsx
│   │   ├── Input.css
│   │   └── index.js
│   ├── Modal/
│   │   ├── Modal.jsx
│   │   ├── Modal.css
│   │   └── index.js
│   └── Layout/
│       ├── Layout.jsx
│       ├── Layout.css
│       └── index.js
├── features/                      # Feature-based organization
│   └── todos/
│       ├── components/
│       │   ├── TodoList.jsx       # Main todo list
│       │   ├── TodoItem.jsx       # Individual todo
│       │   ├── TodoForm.jsx       # Add/edit form
│       │   └── TodoFilters.jsx   # Filter buttons
│       ├── hooks/
│       │   ├── useTodos.js        # Todo state management
│       │   └── useTodoFilters.js  # Filter state
│       ├── utils/
│       │   └── todoHelpers.js     # Utility functions
│       ├── api/
│       │   └── todosApi.js        # API calls
│       └── index.js               # Exports
├── contexts/                      # React contexts
│   ├── AuthContext.jsx
│   └── ThemeContext.jsx
├── hooks/                         # Shared hooks
│   ├── useAuth.js
│   ├── useLocalStorage.js
│   └── useFetch.js
├── services/                      # External services
│   ├── api.js                     # Axios instance
│   └── auth.js                    # Auth service
├── utils/                         # Utility functions
│   ├── formatDate.js
│   ├── validation.js
│   └── constants.js
├── styles/                        # Global styles
│   ├── variables.css
│   ├── reset.css
│   └── global.css
├── pages/                         # Page components
│   ├── HomePage.jsx
│   ├── DashboardPage.jsx
│   ├── LoginPage.jsx
│   └── NotFoundPage.jsx
├── App.jsx                        # Root component with routing
└── main.jsx                       # Entry point
```

```jsx
// File: src/features/todos/components/TodoList.jsx

import React from 'react';
import { useTodos } from '../hooks/useTodos';
import TodoItem from './TodoItem';
import TodoFilters from './TodoFilters';
import TodoForm from './TodoForm';
import './TodoList.css';

function TodoList() {
  // Use the custom hook for all todo logic
  const { 
    todos, 
    filter, 
    addTodo, 
    toggleTodo, 
    deleteTodo,
    setFilter 
  } = useTodos();
  
  // Filter todos based on current filter
  const filteredTodos = todos.filter(todo => {
    if (filter === 'active') return !todo.completed;
    if (filter === 'completed') return todo.completed;
    return true; // 'all'
  });
  
  return (
    <div className="todo-list-container">
      <h1>My Todos</h1>
      
      {/* Form to add new todos */}
      <TodoForm onAdd={addTodo} />
      
      {/* Filter buttons */}
      <TodoFilters 
        currentFilter={filter} 
        onFilterChange={setFilter}
        counts={{
          all: todos.length,
          active: todos.filter(t => !t.completed).length,
          completed: todos.filter(t => t.completed).length
        }}
      />
      
      {/* Todo items */}
      <ul className="todos">
        {filteredTodos.map(todo => (
          <TodoItem
            key={todo.id}
            todo={todo}
            onToggle={() => toggleTodo(todo.id)}
            onDelete={() => deleteTodo(todo.id)}
          />
        ))}
      </ul>
      
      {filteredTodos.length === 0 && (
        <p className="empty-message">No todos found</p>
      )}
    </div>
  );
}

export default TodoList;
```

## Key Takeaways

- Choose a project structure that matches your app's size and complexity
- Feature-based structure scales well for medium to large applications
- Keep related files together (component, styles, tests, types)
- Limit folder nesting to 2-3 levels maximum
- Use index files (barrel exports) for cleaner imports
- Follow consistent naming conventions throughout the project
- Separate concerns: keep routing, layout, state, and business logic separate
- Consider using path aliases (@/) to simplify import paths

## What's Next

Now that you understand project structure, let's look at setting up code quality tools like ESLint and Prettier to maintain consistent code style across your team.
