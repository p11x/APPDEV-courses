# React.memo Explained

## Overview

React.memo is a higher-order component that prevents unnecessary re-renders by memoizing component output. When props haven't changed, React returns the cached render instead of re-executing the component function. This guide covers when to use React.memo, how it compares values, and when memoization might hurt performance.

## Prerequisites

- Understanding of React renders
- Familiarity with component composition
- Basic TypeScript knowledge

## Core Concepts

### Basic React.memo Usage

Wrap a component with React.memo to prevent re-renders when props are the same:

```tsx
// File: src/components/MemoizedButton.tsx

import { memo } from 'react';

// Create a memoized button component
const MemoizedButton = memo(function Button({ 
  onClick, 
  children 
}: { 
  onClick: () => void; 
  children: React.ReactNode;
}) {
  console.log('Button rendered');
  
  return (
    <button onClick={onClick}>
      {children}
    </button>
  );
});

// Parent component
function Parent() {
  const [count, setCount] = useState(0);
  
  // This function is created fresh each render
  const handleClick = () => {
    console.log('clicked');
  };
  
  return (
    <div>
      <p>Count: {count}</p>
      {/* Button won't re-render when count changes */}
      <MemoizedButton onClick={handleClick}>
        Click Me
      </MemoizedButton>
      <button onClick={() => setCount(c => c + 1)}>
        Increment
      </button>
    </div>
  );
}
```

### Custom Comparison Function

React.memo uses shallow comparison by default. Provide a custom function for complex comparisons:

```tsx
// File: src/components/CustomComparison.tsx

import { memo, useMemo } from 'react';

interface User {
  id: number;
  name: string;
  email: string;
}

interface Props {
  user: User;
  onEdit: (id: number) => void;
}

// Custom comparison - only re-render if user or onEdit changes
const UserCard = memo(
  function UserCard({ user, onEdit }: Props) {
    return (
      <div>
        <h3>{user.name}</h3>
        <p>{user.email}</p>
        <button onClick={() => onEdit(user.id)}>Edit</button>
      </div>
    );
  },
  // Previous and next props comparison
  (prevProps, nextProps) => {
    // Return true to skip re-render (props are "equal")
    return (
      prevProps.user.id === nextProps.user.id &&
      prevProps.user.name === nextProps.user.name &&
      prevProps.user.email === nextProps.user.email &&
      prevProps.onEdit === nextProps.onEdit
    );
  }
);

// Usage
function UserList() {
  const [users, setUsers] = useState<User[]>([]);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  
  // Stable callback using useCallback
  const handleEdit = useCallback((id: number) => {
    setSelectedId(id);
  }, []);
  
  return (
    <div>
      {users.map(user => (
        <UserCard 
          key={user.id} 
          user={user} 
          onEdit={handleEdit}
        />
      ))}
    </div>
  );
}
```

### When Memo Helps vs Hurts

Memoization has overhead—use it strategically:

```tsx
// File: src/components/MemoExamples.tsx

import { memo, useState } from 'react';

// ✅ GOOD - Expensive rendering that rarely changes
const ExpensiveChart = memo(function Chart({ data }: { data: number[] }) {
  // Heavy computation
  const result = expensiveCalculation(data);
  return <div>{result}</div>;
});

// ✅ GOOD - Large lists
const ListItem = memo(function ListItem({ item }: { item: Item }) {
  return <li>{item.name}</li>;
});

// ❌ BAD - Simple components that render fast
const SimpleButton = memo(function SimpleButton({ label }: { label: string }) {
  return <button>{label}</button>;
});

// ❌ BAD - Components that always get new props
function ProblematicParent() {
  const [count, setCount] = useState(0);
  
  // This defeats memo!
  return <ExpensiveChild onClick={() => setCount(c => c + 1)} />;
}
```

## Key Takeaways

- React.memo prevents re-renders when props are shallowly equal
- Use custom comparison for complex objects
- Memoization has overhead—don't overuse it
- Profile before and after to verify improvement
- Combine with useCallback and useMemo for best results

## What's Next

Continue to [Avoiding Unnecessary Rerenders](/09-performance/01-rendering-optimization/03-avoiding-unnecessary-rerenders.md) to learn patterns for reducing re-renders through stable references.