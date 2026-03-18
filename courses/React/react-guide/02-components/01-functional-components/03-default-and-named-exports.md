# Default and Named Exports

## Overview

Understanding the difference between default and named exports is crucial for writing clean, maintainable React code. JavaScript modules can export values in two ways: default exports and named exports. Each approach has its use cases, and knowing when to use which will make your code more organized and easier to work with. This guide covers both export styles and best practices for React projects.

## Prerequisites

- Basic understanding of JavaScript modules (import/export)
- Knowledge of React components
- Familiarity with file organization
- Understanding of ES6+ JavaScript syntax

## Core Concepts

### Default Exports

A default export is the primary export from a module. Each file can have only one default export, and when importing, you can give it any name you want.

```javascript
// File: src/components/Button.jsx

import React from 'react';

// Default export - one per file
function Button({ children, onClick, variant = 'primary' }) {
  return (
    <button className={`btn btn-${variant}`} onClick={onClick}>
      {children}
    </button>
  );
}

// Export at declaration
export default Button;

// Alternative: Export at the end
// export default function Button() { ... }

// Alternative: Export anonymous function
// export default () => { ... };
```

```javascript
// File: src/App.jsx

// Importing default export - you can use any name
import MyButton from './components/Button';
import ButtonComponent from './components/Button';
import Whatever from './components/Button';

// All three above are equivalent - you choose the name
```

### Named Exports

Named exports allow you to export multiple values from a module. When importing, you must use the exact exported name (or alias it).

```javascript
// File: src/utils/format.js

// Multiple named exports
export function formatCurrency(amount) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  }).format(amount);
}

export function formatDate(date) {
  return new Intl.DateTimeFormat('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  }).format(new Date(date));
}

export function formatPercentage(value) {
  return `${(value * 100).toFixed(1)}%`;
}
```

```javascript
// File: src/App.jsx

// Import named exports - must use exact names
import { formatCurrency, formatDate } from './utils/format';

// Import with alias (renaming)
import { formatCurrency as formatMoney, formatDate as formatDateString } from './utils/format';

// Import all named exports as an object
import * as formatters from './utils/format';

// Usage: formatters.formatCurrency(100)
```

### Combining Default and Named Exports

You can use both in the same file:

```javascript
// File: src/components/UserCard.jsx

import React from 'react';

// Default export
function UserCard({ name, email, avatar }) {
  return (
    <div className="user-card">
      <img src={avatar} alt={name} />
      <h3>{name}</h3>
      <p>{email}</p>
    </div>
  );
}

export default UserCard;

// Named exports for related utilities
export function UserCardSkeleton() {
  return <div className="user-card skeleton">Loading...</div>;
}

export function UserCardCompact({ name, email }) {
  return <div className="user-card compact">{name} - {email}</div>;
}
```

```javascript
// File: src/App.jsx

// Import default
import UserCard from './components/UserCard';

// Import named
import { UserCardSkeleton, UserCardCompact } from './components/UserCard';

// Import both
import UserCard, { UserCardSkeleton, UserCardCompact } from './components/UserCard';
```

### Barrel Exports (Index Files)

Barrel exports (or index files) provide a clean way to export multiple modules from a single entry point.

```javascript
// File: src/components/index.js

// Barrel export - centralizes imports
export { default as Button } from './Button';
export { default as Input } from './Input';
export { default as Card } from './Card';
export { default as Modal } from './Modal';

// Named exports
export { UserCardSkeleton, UserCardCompact } from './UserCard';
export { default as Form } from './Form';
```

```javascript
// File: src/App.jsx

// Without barrel: (tedious for many components)
// import Button from './components/Button';
// import Input from './components/Input';
// import Card from './components/Card';

// With barrel: (clean and simple)
import { Button, Input, Card, Modal } from './components';
```

## Common Mistakes

### Mistake 1: Mixing Import Styles Incorrectly

```javascript
// ❌ WRONG - Trying to import default as named
import { Button } from './Button'; // If Button is a default export

// ❌ WRONG - Trying to import named as default
import Button from './utils/format'; // If formatCurrency is a named export

// ✅ CORRECT - Match export style to import style
// For default export:
import Button from './Button';

// For named export:
import { Button } from './components';
```

### Mistake 2: Forgetting Curly Braces for Named Imports

```javascript
// ❌ WRONG - Missing curly braces
import formatCurrency from './utils/format'; // Won't work for named export

// ✅ CORRECT - Use curly braces for named exports
import { formatCurrency } from './utils/format';
```

### Mistake 3: Exporting Before Declaration

```javascript
// ❌ WRONG - Can't mix default and named this way
export default function Button();
function Button() { ... } // Error!

// ✅ CORRECT - Export after or inline
export default function Button() { ... }

// Or:
function Button() { ... }
export default Button;
```

### Mistake 4: Not Using Consistent Export Style

```javascript
// ❌ INCONSISTENT - Different files use different styles
// Button.jsx uses default export
// Input.jsx uses named export
// Card.jsx uses both

// ✅ CONSISTENT - Pick one style per project or follow conventions
// Convention: Use default for components, named for utilities
```

## Real-World Example

Let's create a complete module structure demonstrating export patterns:

```javascript
// File: src/components/Button/Button.jsx

import React from 'react';
import './Button.css';

function Button({ 
  children, 
  variant = 'primary', 
  size = 'medium',
  onClick,
  disabled = false,
  type = 'button',
  fullWidth = false
}) {
  const className = [
    'btn',
    `btn-${variant}`,
    `btn-${size}`,
    fullWidth && 'btn-full'
  ].filter(Boolean).join(' ');
  
  return (
    <button
      type={type}
      className={className}
      onClick={onClick}
      disabled={disabled}
    >
      {children}
    </button>
  );
}

export default Button;
```

```javascript
// File: src/components/Button/index.js

// Barrel export for Button module
export { default as Button } from './Button';
export { default as ButtonGroup } from './ButtonGroup';
export { default as IconButton } from './IconButton';
```

```javascript
// File: src/utils/format.js

// All formatting utilities as named exports
export function formatCurrency(amount, currency = 'USD') {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency
  }).format(amount);
}

export function formatDate(date, options = {}) {
  const defaultOptions = {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    ...options
  };
  return new Intl.DateTimeFormat('en-US', defaultOptions).format(new Date(date));
}

export function formatRelativeTime(date) {
  const now = new Date();
  const then = new Date(date);
  const seconds = Math.floor((now - then) / 1000);
  
  if (seconds < 60) return 'just now';
  if (seconds < 3600) return `${Math.floor(seconds / 60)} minutes ago`;
  if (seconds < 86400) return `${Math.floor(seconds / 3600)} hours ago`;
  if (seconds < 604800) return `${Math.floor(seconds / 86400)} days ago`;
  
  return formatDate(date);
}

export function formatNumber(num, decimals = 0) {
  return num.toLocaleString('en-US', {
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals
  });
}

export function formatFileSize(bytes) {
  if (bytes === 0) return '0 Bytes';
  const k = 1024;
  const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return `${parseFloat((bytes / Math.pow(k, i)).toFixed(2))} ${sizes[i]}`;
}
```

```javascript
// File: src/utils/validation.js

// Validation utilities as named exports
export function isValidEmail(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

export function isValidUrl(url) {
  try {
    new URL(url);
    return true;
  } catch {
    return false;
  }
}

export function isValidPhone(phone) {
  const phoneRegex = /^\+?[\d\s-()]+$/;
  return phoneRegex.test(phone);
}

export function validatePassword(password) {
  const errors = [];
  
  if (password.length < 8) {
    errors.push('Password must be at least 8 characters');
  }
  if (!/[A-Z]/.test(password)) {
    errors.push('Password must contain an uppercase letter');
  }
  if (!/[a-z]/.test(password)) {
    errors.push('Password must contain a lowercase letter');
  }
  if (!/[0-9]/.test(password)) {
    errors.push('Password must contain a number');
  }
  
  return {
    isValid: errors.length === 0,
    errors
  };
}

export function sanitizeInput(input) {
  if (typeof input !== 'string') return '';
  return input
    .replace(/</g, '<')
    .replace(/>/g, '>')
    .replace(/"/g, '"')
    .replace(/'/g, '&#039;');
}
```

```javascript
// File: src/utils/index.js

// Barrel export for all utilities
export * from './format';
export * from './validation';
```

```jsx
// File: src/App.jsx

import React from 'react';
import { Button } from './components/Button';
import { formatCurrency, formatDate, formatRelativeTime } from './utils/format';
import { isValidEmail, validatePassword } from './utils/validation';
// Or use barrel export:
// import { Button } from './components';
// import { formatCurrency, isValidEmail } from './utils';

function App() {
  // Using imported utilities
  const price = 1234.56;
  const date = '2024-01-15';
  const email = 'test@example.com';
  const password = 'Password123';
  
  console.log(formatCurrency(price)); // $1,234.56
  console.log(formatDate(date)); // Jan 15, 2024
  console.log(formatRelativeTime(date)); // X days ago
  
  console.log(isValidEmail(email)); // true
  console.log(validatePassword(password)); // { isValid: true, errors: [] }
  
  return (
    <div>
      <h1>My App</h1>
      <Button variant="primary" onClick={() => alert('Clicked!')}>
        Click Me
      </Button>
      <p>Price: {formatCurrency(price)}</p>
      <p>Date: {formatDate(date)}</p>
    </div>
  );
}

export default App;
```

## Key Takeaways

- Default exports: one per file, import with any name
- Named exports: multiple per file, import with exact name (or alias)
- Use curly braces `{}` for named imports, no braces for default
- Barrel exports (index.js files) provide clean import paths
- Convention: Default exports for components, named exports for utilities
- You can combine both default and named exports in one file
- Be consistent - pick a style and use it throughout your project

## What's Next

Now that you understand how to create and export components, let's explore component composition patterns - how to combine components together to build complex UIs.
