# Create React App vs Vite

## Overview

Choosing the right tool to bootstrap your React project is an important decision that affects your development experience. For years, Create React App (CRA) was the go-to solution for creating new React applications. However, Vite has emerged as a faster, more modern alternative that's now recommended by the React team. This guide will help you understand the differences between these tools and choose the best one for your project.

## Prerequisites

- Basic command line knowledge
- Node.js and npm installed
- Understanding of what React is (from previous lessons)
- Familiarity with JavaScript modules and imports

## Core Concepts

### What is Create React App?

Create React App (CRA) was released by Facebook in 2016 and was the standard way to create React applications for many years. It provides a pre-configured setup with webpack for bundling, Babel for transpiling, and a development server with hot module replacement.

```bash
# To create a new React app with CRA
npx create-react-app my-app
cd my-app
npm start
```

CRA handles all the complex configuration behind the scenes, so you can start writing React code immediately. It includes built-in support for:
- React, JSX, ES6+ syntax
- Autoprefixed CSS
- A development server with fast refreshing
- Testing with Jest
- Production build optimization

```javascript
// File: src/cra-structure.js

// CRA automatically configures:
// - Webpack for bundling (combines all your files)
// - Babel for transpiling (converts modern JS to browser-compatible JS)
// - Dev server with HMR (Hot Module Replacement)
// - Jest for testing
// - PostCSS for CSS processing

// Your code goes in src/
// CRA outputs to build/
```

### What is Vite?

Vite is a next-generation build tool created by Evan You (the creator of Vue.js) that provides a much faster development experience. Unlike CRA's webpack-based approach, Vite uses native ES modules in the browser and implements an extremely fast HMR algorithm.

```bash
# To create a new React app with Vite
npm create vite@latest my-app -- --template react
cd my-app
npm install
npm run dev
```

Vite offers several key advantages:
- Instant server start (no bundling during development)
- Lightning-fast hot module replacement
- Built-in TypeScript support
- Out-of-the-box CSS modules and PostCSS
- Optimized production builds with Rollup

```javascript
// File: src/vite-structure.js

// Vite project structure:
// my-app/
// ├── index.html          # Entry point (not in src!)
// ├── src/
// │   ├── main.jsx        # Mounts the app
// │   ├── App.jsx
// │   └── ...
// ├── package.json
// └── vite.config.js     # Vite configuration

// Key difference from CRA:
// - index.html is in the root, not in public/
// - Uses ESM natively in development
// - No webpack bundling during dev
```

### Performance Comparison

The most significant difference between CRA and Vite is development speed. Here's why Vite is so much faster:

**CRA's Approach:**
1. Bundles the entire application before starting the dev server
2. Rebuilds the entire bundle on every file change (in development)
3. This gets slower as your app grows

**Vite's Approach:**
1. Serves code over native ES modules (no bundling needed!)
2. Transforms only the changed file on each request
3. Uses an efficient HMR algorithm that doesn't require full reloads
4. Performance stays fast regardless of app size

```javascript
// File: src/development-flow.js

// ==================== CRA DEVELOPMENT FLOW ====================
// 1. npm start
// 2. Webpack bundles ENTIRE app (can take 30+ seconds)
// 3. Dev server starts
// 4. Edit a file
// 5. Webpack rebuilds ENTIRE bundle (can take 10+ seconds)
// 6. Browser refreshes
// 7. Repeat

// ==================== VITE DEVELOPMENT FLOW ====================
// 1. npm run dev
// 2. Vite starts dev server INSTANTLY (milliseconds!)
// 3. Browser requests /src/App.jsx
// 4. Vite transforms and serves just that file
// 5. Edit a file
// 6. Vite transforms just that file (milliseconds!)
// 7. HMR updates only what changed - no refresh needed!
```

### When to Choose Each

**Choose Create React App when:**
- You need maximum compatibility with older tooling
- Your team is already familiar with CRA
- You need built-in testing setup (though Vite can add this easily)

**Choose Vite when:**
- Starting a new project (recommended by React team now)
- Speed is important to you
- You want better production build performance
- You prefer modern tooling
- You're migrating from CRA

```bash
# File: src/comparison-commands.js

# ==================== CREATE REACT APP ====================
# Installation (one-time)
npm install -g create-react-app

# Create new project
npx create-react-app my-blog
# OR
npm create react-app my-blog

# Run development server
cd my-blog
npm start

# Build for production
npm run build

# Run tests
npm test


# ==================== VITE ====================
# Create new project (recommended way)
npm create vite@latest my-blog -- --template react
# OR (if you want TypeScript)
npm create vite@latest my-blog -- --template react-ts

# Install dependencies
cd my-blog
npm install

# Run development server
npm run dev

# Build for production
npm run build

# Preview production build locally
npm run preview

# Run tests (need to add separately)
npm install -D vitest @testing-library/react @testing-library/jest-dom
```

### Migrating from CRA to Vite

If you have an existing CRA project and want to switch to Vite, here's the process:

```javascript
// File: src/cra-to-vite-steps.js

// Migration steps:

// 1. Install Vite as a dev dependency
npm install -D vite @vitejs/plugin-react

// 2. Create vite.config.js in project root
// vite.config.js:
/*
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000, // Keep same port as CRA
  },
});
*/

// 3. Move index.html from public/ to project root

// 4. Update index.html:
// - Change <script src="/src/index.js"></script> 
//   to <script type="module" src="/src/index.jsx"></script>

// 5. Rename index.js to index.jsx (or .tsx for TypeScript)

// 6. Update main entry file:
// - Change import './index.css' to import './index.css'

// 7. Update package.json scripts:
/*
{
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview",
    "lint": "eslint .",
    "test": "vitest"
  }
}
*/

// 8. Run npm run dev to test!
```

## Common Mistakes

### Mistake 1: Using Wrong File Extensions

```javascript
// ❌ WRONG - CRA allows .js files with JSX, but Vite prefers .jsx
// src/App.js (works in CRA but can cause issues in Vite)

// ✅ CORRECT - Use .jsx extension for files containing JSX
// src/App.jsx
```

### Mistake 2: Incorrect index.html Location

```html
<!-- ❌ WRONG - CRA puts index.html in public/ folder -->
<!-- public/index.html -->

<!-- ✅ CORRECT - Vite expects index.html in project root -->
<!-- index.html -->
```

### Mistake 3: Not Using Type="module"

```html
<!-- ❌ WRONG - Missing type="module" for the entry script -->
<script src="/src/main.jsx"></script>

<!-- ✅ CORRECT - Vite requires type="module" -->
<script type="module" src="/src/main.jsx"></script>
```

### Mistake 4: Using process.env Incorrectly

```javascript
// ❌ WRONG - CRA uses process.env.VARIABLE_NAME
const apiUrl = process.env.REACT_APP_API_URL;

// ✅ CORRECT - Vite uses import.meta.env.VARIABLE_NAME
const apiUrl = import.meta.env.VITE_API_URL;

// In .env file for Vite:
// VITE_API_URL=https://api.example.com
```

## Real-World Example

Let's create a simple todo app to see both setups in action:

```jsx
// File: src/App.jsx (works in both CRA and Vite with minor adjustments)

import React, { useState } from 'react';
import './App.css';

function TodoApp() {
  const [todos, setTodos] = useState([]);
  const [input, setInput] = useState('');
  
  const addTodo = (e) => {
    e.preventDefault();
    if (!input.trim()) return;
    
    setTodos([...todos, { 
      id: Date.now(), 
      text: input.trim(),
      completed: false 
    }]);
    setInput('');
  };
  
  const toggleTodo = (id) => {
    setTodos(todos.map(todo => 
      todo.id === id ? { ...todo, completed: !todo.completed } : todo
    ));
  };
  
  const deleteTodo = (id) => {
    setTodos(todos.filter(todo => todo.id !== id));
  };
  
  return (
    <div className="app">
      <h1>Todo List</h1>
      
      <form onSubmit={addTodo} className="todo-form">
        <input
          type="text"
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="What needs to be done?"
          className="todo-input"
        />
        <button type="submit" className="todo-button">Add</button>
      </form>
      
      <ul className="todo-list">
        {todos.map(todo => (
          <li key={todo.id} className={`todo-item ${todo.completed ? 'completed' : ''}`}>
            <span onClick={() => toggleTodo(todo.id)}>{todo.text}</span>
            <button onClick={() => deleteTodo(todo.id)} className="delete-btn">×</button>
          </li>
        ))}
      </ul>
      
      {todos.length > 0 && (
        <p className="todo-count">{todos.filter(t => !t.completed).length} items left</p>
      )}
    </div>
  );
}

export default TodoApp;
```

```css
/* File: src/App.css */

.app {
  max-width: 500px;
  margin: 50px auto;
  padding: 20px;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
}

h1 {
  text-align: center;
  color: #333;
}

.todo-form {
  display: flex;
  gap: 10px;
  margin-bottom: 20px;
}

.todo-input {
  flex: 1;
  padding: 10px;
  font-size: 16px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.todo-button {
  padding: 10px 20px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

.todo-button:hover {
  background-color: #45a049;
}

.todo-list {
  list-style: none;
  padding: 0;
}

.todo-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 10px;
  border-bottom: 1px solid #eee;
}

.todo-item.completed span {
  text-decoration: line-through;
  color: #999;
}

.delete-btn {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 5px 10px;
  border-radius: 4px;
  cursor: pointer;
}

.todo-count {
  text-align: center;
  color: #666;
  margin-top: 20px;
}
```

## Key Takeaways

- Create React App (CRA) uses webpack and was the standard for years but is now slower
- Vite uses native ES modules and offers much faster development experience
- The React team now recommends Vite for new projects
- Vite provides instant server start and lightning-fast HMR
- Key differences: index.html location, file extensions (.jsx), environment variables (import.meta.env)
- Migration from CRA to Vite is straightforward
- Vite uses Rollup for optimized production builds

## What's Next

Now that you understand the tooling options, let's look at project structure best practices to organize your React code effectively.
