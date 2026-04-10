---
Category: Web Development
Subcategory: Frontend
Concept: React Basics
Purpose: Understanding React fundamental concepts and component architecture
Difficulty: beginner
Prerequisites: JavaScript Fundamentals
RelatedFiles: 02_Advanced_React.md
UseCase: Building interactive user interfaces
LastUpdated: 2025
---

## WHY

React is a widely-used JavaScript library for building user interfaces, making it essential for modern full-stack cloud developers.

## WHAT

### React Core Concepts

**Component**: Reusable UI building block

**Props**: Data passed to components

**State**: Internal component data

**Hooks**: State management in functional components

## HOW

### Example: Simple Component

```javascript
// App.js
function App() {
  return (
    <div>
      <h1>Hello Cloud</h1>
      <p>Building cloud applications</p>
    </div>
  );
}

export default App;
```

### Example: Component with State

```javascript
// Counter.js
import { useState } from 'react';

function Counter() {
  const [count, setCount] = useState(0);
  
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

## CROSS-REFERENCES

### Related Technologies

- JavaScript: Language foundation
- AWS Amplify: Cloud hosting