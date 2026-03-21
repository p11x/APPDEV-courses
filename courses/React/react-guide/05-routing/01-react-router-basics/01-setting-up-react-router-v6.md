# Setting Up React Router v6

## Overview
React Router v6 is the standard routing library for React applications. It provides declarative routing with components like Routes, Route, and Navigate. This guide covers the basics of setting up React Router and understanding its core concepts.

## Prerequisites
- Basic React knowledge
- Understanding of React components
- Familiarity with npm/yarn

## Core Concepts

### Installation
First, install React Router v6.

```bash
npm install react-router-dom
# or
yarn add react-router-dom
```

### Basic Setup
React Router uses a component-based approach for routing.

```jsx
// File: src/App.jsx

import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';

// Page components
function Home() {
  return (
    <div>
      <h1>Home Page</h1>
      <p>Welcome to our app!</p>
    </div>
  );
}

function About() {
  return (
    <div>
      <h1>About Page</h1>
      <p>Learn more about us.</p>
    </div>
  );
}

function Contact() {
  return (
    <div>
      <h1>Contact Page</h1>
      <p>Get in touch with us.</p>
    </div>
  );
}

// Navigation component
function Navigation() {
  return (
    <nav>
      <ul>
        <li><Link to="/">Home</Link></li>
        <li><Link to="/about">About</Link></li>
        <li><Link to="/contact">Contact</Link></li>
      </ul>
    </nav>
  );
}

// Main App with routing
function App() {
  return (
    // BrowserRouter wraps the entire application
    <BrowserRouter>
      <Navigation />
      
      {/* Routes container - defines all routes */}
      <Routes>
        {/* Each Route defines a path and component */}
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
```

### Understanding Route Matching
Routes match exactly by default in v6. Use * for wildcards.

```jsx
// File: src/App.jsx

import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import About from './pages/About';
import UserProfile from './pages/UserProfile';
import NotFound from './pages/NotFound';

function App() {
  return (
    <Routes>
      {/* Exact path */}
      <Route path="/" element={<Home />} />
      <Route path="/about" element={<About />} />
      
      {/* Dynamic route parameter */}
      <Route path="/users/:userId" element={<UserProfile />} />
      
      {/* Wildcard - catches all unmatched routes */}
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}
```

### Using useParams Hook
Access dynamic route parameters with useParams.

```jsx
// File: src/pages/UserProfile.jsx

import React from 'react';
import { useParams, Link } from 'react-router-dom';

function UserProfile() {
  // Get the userId from the URL
  const { userId } = useParams();

  return (
    <div>
      <h1>User Profile</h1>
      <p>User ID: {userId}</p>
      <Link to="/">Back to Home</Link>
    </div>
  );
}

export default UserProfile;
```

### Programmatic Navigation
Navigate programmatically using useNavigate hook.

```jsx
// File: src/components/LoginForm.jsx

import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

function LoginForm() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    
    // Perform login logic
    const success = await loginUser();
    
    if (success) {
      setIsLoggedIn(true);
      // Navigate to dashboard after successful login
      navigate('/dashboard', { replace: true });
    }
  };

  const handleCancel = () => {
    // Navigate back to previous page
    navigate(-1);
  };

  return (
    <form onSubmit={handleLogin}>
      {/* Form fields */}
      <button type="button" onClick={handleCancel}>Cancel</button>
      <button type="submit">Login</button>
    </form>
  );
}

export default LoginForm;
```

### Nested Routes
Create nested routes for hierarchical pages.

```jsx
// File: src/App.jsx

import { BrowserRouter, Routes, Route, Outlet, Link } from 'react-router-dom';

function Dashboard() {
  return (
    <div>
      <h1>Dashboard</h1>
      <nav>
        <Link to="stats">Stats</Link>
        <Link to="settings">Settings</Link>
      </nav>
      
      {/* Nested routes render here */}
      <Outlet />
    </div>
  );
}

function Stats() {
  return <div>Dashboard Statistics</div>;
}

function Settings() {
  return <div>Dashboard Settings</div>;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/dashboard" element={<Dashboard />}>
          <Route index element={<Stats />} />
          <Route path="stats" element={<Stats />} />
          <Route path="settings" element={<Settings />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
```

## Common Mistakes

### Wrong Router Component
Use BrowserRouter for web apps, not HashRouter or MemoryRouter (unless needed).

```jsx
// ✅ CORRECT - Use BrowserRouter for web apps
<BrowserRouter>
  <App />
</BrowserRouter>

// ❌ WRONG - Wrong router type for most web apps
<MemoryRouter>
  <App />
</MemoryRouter>
```

### Forgetting Outlet for Nested Routes
Nested routes need an Outlet to render.

```jsx
// ❌ WRONG - Nested routes won't render
function Dashboard() {
  return <div><h1>Dashboard</h1></div>; // No Outlet!
}

// ✅ CORRECT - Include Outlet
function Dashboard() {
  return (
    <div>
      <h1>Dashboard</h1>
      <Outlet />
    </div>
  );
}
```

## Key Takeaways
- Install react-router-dom for web routing
- Wrap app in BrowserRouter
- Use Routes and Route components
- Use Link for navigation, useNavigate for programmatic navigation
- Use useParams for dynamic routes
- Use Outlet for nested routes

## What's Next
Continue to [Route Params and Nested Routes](02-route-params-and-nested-routes.md) to learn more about dynamic routing.
