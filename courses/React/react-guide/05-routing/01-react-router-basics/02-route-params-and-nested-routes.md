# Route Params and Nested Routes

## Overview

Route parameters allow you to capture dynamic segments from URLs, while nested routes let you create hierarchical page structures. Both are essential for building real-world applications.

## Core Concepts

```jsx
// File: src/routes.jsx

import { Routes, Route, useParams } from 'react-router-dom';

// Route with parameter
function UserProfile() {
  const { id } = useParams();
  return <h1>User {id}</h1>;
}

// Nested routes
function Dashboard() {
  return (
    <div>
      <h1>Dashboard</h1>
      <nav>
        <Link to="stats">Stats</Link>
        <Link to="settings">Settings</Link>
      </nav>
      <Routes>
        <Route index element={<Stats />} />
        <Route path="stats" element={<Stats />} />
        <Route path="settings" element={<Settings />} />
      </Routes>
    </div>
  );
}
```

## Key Takeaways

- Use :paramName for dynamic parameters
- useParams hook accesses parameters
- Nested routes use relative paths
