# Architecture Interview Questions

## Overview
Architecture questions evaluate your ability to design scalable React applications. These questions often involve trade-offs and real-world considerations.

## Prerequisites
- React development experience
- Software design knowledge

## Core Concepts

### Question 1: State Management

**Q: How do you choose between useState, Context, and Redux?**

```tsx
// [File: src/interview-answers/StateChoice.jsx]
import React, { useState, createContext, useContext } from 'react';

// 1. useState - Local component state
function SimpleCounter() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}

// 2. Context - Shared state across a few components
const ThemeContext = createContext();

function ThemedButton() {
  const theme = useContext(ThemeContext);
  return <button className={theme}>Themed</button>;
}

// 3. Redux/Zustand - Complex global state
// - Many components need access
// - Complex updates/logic
// - Need devtools/time travel
// - Server state caching (use React Query instead!)

export default function StateChoice() {}
```

### Question 2: Component Patterns

**Q: What's your preferred component structure?**

```tsx
// [File: src/interview-answers/ComponentStructure.jsx]
import React, { useState, useEffect } from 'react';

// Presentational - UI only
function UserCard({ user, onEdit }) {
  return (
    <div className="user-card">
      <h3>{user.name}</h3>
      <p>{user.email}</p>
      <button onClick={() => onEdit(user.id)}>Edit</button>
    </div>
  );
}

// Container - Data fetching and logic
function UserCardContainer({ userId }) {
  const [user, setUser] = useState(null);
  
  useEffect(() => {
    fetchUser(userId).then(setUser);
  }, [userId]);

  const handleEdit = (id) => {
    console.log('Edit user', id);
  };

  if (!user) return <Skeleton />;
  return <UserCard user={user} onEdit={handleEdit} />;
}

export default ComponentStructure;
```

### Question 3: Folder Structure

**Q: How do you organize large projects?**

```tsx
// [File: src/interview-answers/FolderStructure.js]
// Recommended: Feature-based structure

src/
├── features/
│   ├── auth/
│   │   ├── components/
│   │   ├── hooks/
│   │   ├── api/
│   │   └── index.ts
│   ├── products/
│   │   ├── components/
│   │   ├── hooks/
│   │   ├── api/
│   │   └── index.ts
│   └── cart/
├── shared/
│   ├── components/
│   ├── hooks/
│   └── utils/
├── app/
│   ├── routes.tsx
│   └── App.tsx
```

## Key Takeaways
- Choose state solution based on scope
- Separate presentational and container components
- Use feature-based folder structure

## What's Next
Continue to [Build Custom Hook Challenge](04-coding-challenges/01-build-custom-hook-challenge.md) for hands-on practice.