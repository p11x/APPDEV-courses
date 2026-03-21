# Route Params and Nested Routes in React Router v6

## Overview
Dynamic route parameters and nested routes are essential for building scalable React applications. Route parameters allow you to create flexible routes that match variable URLs, while nested routes enable hierarchical page structures.

## Prerequisites
- React Router v6 basics
- Understanding of components and props
- Familiarity with hooks

## Core Concepts

### Route Parameters
Use :paramName syntax to define dynamic route parameters.

```jsx
// File: src/App.jsx

import { BrowserRouter, Routes, Route, useParams, Link } from 'react-router-dom';

// Component that uses route params
function UserProfile() {
  const { userId } = useParams();
  
  return (
    <div>
      <h1>User Profile</h1>
      <p>Viewing profile for user ID: {userId}</p>
    </div>
  );
}

function Post() {
  const { postId } = useParams();
  
  return (
    <div>
      <h1>Post</h1>
      <p>Post ID: {postId}</p>
    </div>
  );
}

// Multiple parameters
function UserPost() {
  const { userId, postId } = useParams();
  
  return (
    <div>
      <p>User {userId}'s Post {postId}</p>
    </div>
  );
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/users/:userId" element={<UserProfile />} />
        <Route path="/posts/:postId" element={<Post />} />
        <Route path="/users/:userId/posts/:postId" element={<UserPost />} />
      </Routes>
    </BrowserRouter>
  );
}
```

### Nested Routes
Create routes within routes for hierarchical content.

```jsx
// File: src/App.jsx

import { BrowserRouter, Routes, Route, Outlet, Link } from 'react-router-dom';

// Layout with nested routes
function DashboardLayout() {
  return (
    <div className="dashboard-layout">
      <aside>
        <nav>
          <Link to="/dashboard">Overview</Link>
          <Link to="/dashboard/analytics">Analytics</Link>
          <Link to="/dashboard/settings">Settings</Link>
        </nav>
      </aside>
      <main>
        {/* Nested routes render here */}
        <Outlet />
      </main>
    </div>
  );
}

function Overview() {
  return <div>Dashboard Overview</div>;
}

function Analytics() {
  return <div>Analytics Dashboard</div>;
}

function Settings() {
  return <div>Settings Dashboard</div>;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/dashboard" element={<DashboardLayout />}>
          <Route index element={<Overview />} />
          <Route path="analytics" element={<Analytics />} />
          <Route path="settings" element={<Settings />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
```

### Link with Params
Navigate to routes with dynamic parameters.

```jsx
// File: src/components/UserList.jsx

import { Link } from 'react-router-dom';

function UserList({ users }) {
  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>
          {/* Link to dynamic route */}
          <Link to={`/users/${user.id}`}>
            {user.name}
          </Link>
        </li>
      ))}
    </ul>
  );
}
```

### Programmatic Navigation with Params
Navigate with parameters programmatically.

```jsx
// File: src/components/PostCard.jsx

import { useNavigate } from 'react-router-dom';

function PostCard({ post }) {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate(`/posts/${post.id}`);
  };

  const handleEdit = (e) => {
    e.stopPropagation();
    navigate(`/posts/${post.id}/edit`);
  };

  return (
    <div onClick={handleClick}>
      <h3>{post.title}</h3>
      <button onClick={handleEdit}>Edit</button>
    </div>
  );
}
```

### Optional and Catch-all Parameters

```jsx
function App() {
  return (
    <Routes>
      {/* Optional parameter - matches /docs or /docs/anything */}
      <Route path="/docs(/:section)" element={<Docs />} />
      
      {/* Catch-all - matches any path */}
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}
```

## Key Takeaways
- Use :paramName for dynamic route parameters
- Access params with useParams hook
- Use Outlet for nested route rendering
- Link and navigate with dynamic paths using template literals

## What's Next
Continue to [Programmatic Navigation](03-programmatic-navigation.md) to learn more about navigating programmatically.
