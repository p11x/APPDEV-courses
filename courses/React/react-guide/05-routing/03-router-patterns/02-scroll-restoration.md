# Scroll Restoration

## Overview

Scroll restoration ensures users return to their previous scroll position when navigating back in a single-page app. React Router handles this automatically in most cases.

## Implementation

```jsx
import { ScrollRestoration, BrowserRouter } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <ScrollRestoration />
      <Routes>
        <Route path="/" element={<Home />} />
      </Routes>
    </BrowserRouter>
  );
}
```

## Key Takeaways

- React Router v6 handles scroll restoration
- Use ScrollRestoration component
- Works with BrowserRouter

## What's Next

Let's explore query params handling.
