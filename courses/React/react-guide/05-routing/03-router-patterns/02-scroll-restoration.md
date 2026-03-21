# Scroll Restoration in React Router v6

## Overview
Scroll restoration ensures users return to their previous scroll position when navigating back, improving user experience.

## Core Concepts

### Using ScrollRestoration Component

```jsx
// File: src/App.jsx

import { BrowserRouter, ScrollRestoration } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <ScrollRestoration />
      {/* Routes */}
    </BrowserRouter>
  );
}
```

## Key Takeaways
- Use ScrollRestoration for automatic scroll management
- Works with browser back/forward buttons

## What's Next
Continue to [Query Params Handling](03-query-params-handling.md)
