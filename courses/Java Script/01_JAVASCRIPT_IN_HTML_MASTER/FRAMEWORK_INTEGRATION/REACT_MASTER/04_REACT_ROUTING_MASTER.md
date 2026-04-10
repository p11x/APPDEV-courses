# 🛤️ React Routing Complete Guide

## Building Navigation with React Router

---

## Table of Contents

1. [Introduction to React Router](#introduction-to-react-router)
2. [Setting Up React Router](#setting-up-react-router)
3. [Basic Routing](#basic-routing)
4. [Route Parameters](#route-parameters)
5. [Nested Routes](#nested-routes)
6. [Programmatic Navigation](#programmatic-navigation)
7. [Route Guards and Protection](#route-guards-and-protection)
8. [Lazy Loading Routes](#lazy-loading-routes)
9. [Search Params and Hash](#search-params-and-hash)
10. [Best Practices](#best-practices)

---

## Introduction to React Router

React Router is the standard routing library for React. It enables navigation between views, passing data, and URL management in single-page applications.

```
┌─────────────────────────────────────────────────────────────┐
│                REACT ROUTER WORKFLOW                       │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  URL Change ──▶ Match Routes ──▶ Render Components          │
│      │                                                    │
│      ▼                                                    │
│  ┌─────────────────────────────────────────────────┐        │
│  │              Route Definitions                │        │
│  │  /        → HomePage                     │        │
│  │  /about   → AboutPage                   │        │
│  │  /users/:id → UserProfile                │        │
│  └─────────────────────────────────────────────────┘        │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Why React Router?

- Declarative routing approach
- Nested routes support
- Route parameters
- Built-in navigation components
- URL synchronization

---

## Setting Up React Router

### Installation

```bash
# Using npm
npm install react-router-dom

# Using yarn
yarn add react-router-dom
```

### Version 6 Setup

```jsx
// main.jsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './index.css';

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
);
```

---

## Basic Routing

### Route Configurations

```jsx
// App.jsx
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import About from './pages/About';
import Contact from './pages/Contact';
import NotFound from './pages/NotFound';

function App() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/about" element={<About />} />
      <Route path="/contact" element={<Contact />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}
```

### Navigation Component

```jsx
// Layout.jsx
import { Link, NavLink } from 'react-router-dom';

function Layout() {
  // Link - programmatic navigation
  // NavLink - adds active class for current route
  
  return (
    <nav>
      <NavLink to="/" className={({ isActive }) => isActive ? 'active' : ''}>
        Home
      </NavLink>
      <NavLink to="/about">About</NavLink>
      <NavLink to="/contact">Contact</NavLink>
    </nav>
  );
}

export default Layout;
```

### Layouts with Routes

```jsx
// App.jsx
import { Outlet } from 'react-router-dom';

function Layout() {
  return (
    <div>
      <header>
        <nav>
          <Link to="/">Home</Link>
          <Link to="/about">About</Link>
        </nav>
      </header>
      <main>
        <Outlet /> {/* Child routes render here */}
      </main>
      <footer>
        <p>&copy; 2024</p>
      </footer>
    </div>
  );
}

function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
      </Route>
    </Routes>
  );
}
```

---

## Route Parameters

### Dynamic Parameters

```jsx
// App.jsx
import { Routes, Route } from 'react-router-dom';
import UserProfile from './pages/UserProfile';

function App() {
  return (
    <Routes>
      <Route path="/users/:id" element={<UserProfile />} />
    </Routes>
  );
}
```

### Reading Parameters

```jsx
// UserProfile.jsx
import { useParams } from 'react-router-dom';

function UserProfile() {
  const { id } = useParams();
  
  return <h1>User Profile: {id}</h1>;
}
```

### Multiple Parameters

```jsx
// App.jsx
<Route path="/users/:userId/posts/:postId" element={<Post />} />
```

```jsx
// Post.jsx
import { useParams } from 'react-router-dom';

function Post() {
  const { userId, postId } = useParams();
  
  return (
    <div>
      <p>User: {userId}</p>
      <p>Post: {postId}</p>
    </div>
  );
}
```

### Query Parameters

```jsx
import { useSearchParams } from 'react-router-dom';

function SearchPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  
  const query = searchParams.get('q');
  const page = searchParams.get('page') || 1;
  
  const handleSearch = (e) => {
    e.preventDefault();
    setSearchParams({ q: e.target.elements.q.value });
  };
  
  return (
    <div>
      <form onSubmit={handleSearch}>
        <input name="q" defaultValue={query} />
        <button type="submit">Search</button>
      </form>
      <p>Query: {query}</p>
      <p>Page: {page}</p>
    </div>
  );
}
```

---

## Nested Routes

### Basic Nested Routes

```jsx
// App.jsx
<Routes>
  <Route path="/dashboard" element={<Dashboard />}>
    <Route path="overview" element={<Overview />} />
    <Route path="analytics" element={<Analytics />} />
    <Route path="settings" element={<Settings />} />
  </Route>
</Routes>
```

```jsx
// Dashboard.jsx
import { Outlet } from 'react-router-dom';

function Dashboard() {
  return (
    <div>
      <aside>
        <Link to="/dashboard/overview">Overview</Link>
        <Link to="/dashboard/analytics">Analytics</Link>
        <Link to="/dashboard/settings">Settings</Link>
      </aside>
      <main>
        <Outlet /> {/* Child routes render here */}
      </main>
    </div>
  );
}
```

### Nested Routes with Parameters

```jsx
// App.jsx
<Route path="/users" element={<UsersLayout />}>
  <Route index element={<UsersList />} />
  <Route path=":userId" element={<UserDetail />}>
    <Route path="posts" element={<UserPosts />} />
    <Route path="albums" element={<UserAlbums />} />
  </Route>
</Route>
```

---

## Programmatic Navigation

### useNavigate Hook

```jsx
import { useNavigate } from 'react-router-dom';

function LoginForm() {
  const navigate = useNavigate();
  
  const handleLogin = async (credentials) => {
    await authenticate(credentials);
    navigate('/dashboard');
  };
  
  return <form onSubmit={handleLogin}>...</form>;
}
```

### Navigate with Options

```jsx
import { useNavigate } from 'react-router-dom';

function Example() {
  const navigate = useNavigate();
  
  const goHome = () => {
    navigate('/'); // Basic navigation
  };
  
  const goWithState = () => {
    navigate('/results', { state: { from: 'search' } }); // With state
  };
  
  const replaceAndGo = () => {
    navigate('/dashboard', { replace: true }); // Replace in history
  };
  
  const goBack = () => {
    navigate(-1); // Go back in history
  };
  
  return (
    <div>
      <button onClick={goHome}>Home</button>
      <button onClick={goWithState}>With State</button>
      <button onClick={replaceAndGo}>Replace</button>
      <button onClick={goBack}>Back</button>
    </div>
  );
}
```

### Navigate Component

```jsx
import { Navigate } from 'react-router-dom';

function AuthGuard({ isAuthenticated }) {
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  
  return <Dashboard />;
}
```

---

## Route Guards and Protection

### Protected Routes

```jsx
// ProtectedRoute.jsx
import { Navigate, useLocation } from 'react-router-dom';

function ProtectedRoute({ children, isAuthenticated }) {
  const location = useLocation();
  
  if (!isAuthenticated) {
    return (
      <Navigate to="/login" state={{ from: location }} replace />
    );
  }
  
  return children;
}

// Usage
<Route
  path="/dashboard"
  element={
    <ProtectedRoute isAuthenticated={user !== null}>
      <Dashboard />
    </ProtectedRoute>
  }
/>
```

### Role-Based Access

```jsx
import { Navigate, useLocation } from 'react-router-dom';

function RoleRoute({ children, requiredRole, userRole }) {
  const location = useLocation();
  
  if (userRole !== requiredRole) {
    return (
      <Navigate to="/unauthorized" state={{ from: location }} replace />
    );
  }
  
  return children;
}

// Usage
<Route
  path="/admin"
  element={
    <RoleRoute requiredRole="admin" userRole={user.role}>
      <AdminPanel />
    </RoleRoute>
  }
/>
```

### Loading State Protection

```jsx
import { Navigate } from 'react-router-dom';

function AuthRoute({ children, isLoading, isAuthenticated }) {
  if (isLoading) {
    return <div>Loading...</div>;
  }
  
  if (!isAuthenticated) {
    return <Navigate to="/login" />;
  }
  
  return children;
}
```

---

## Lazy Loading Routes

### Code Splitting

```jsx
import { lazy, Suspense } from 'react';
import { Routes, Route } from 'react-router-dom';

const Home = lazy(() => import('./pages/Home'));
const About = lazy(() => import('./pages/About'));
const Dashboard = lazy(() => import('./pages/Dashboard'));

function App() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
        <Route path="/dashboard" element={<Dashboard />} />
      </Routes>
    </Suspense>
  );
}
```

### Preloading

```jsx
import { useNavigate, useResolvedPath } from 'react-router-dom';

function LinkWithPreload({ to, children }) {
  const path = useResolvedPath(to);
  
  const handleMouseEnter = () => {
    // Preload the route's component
    if (path) {
      // Implementation depends on bundler
    }
  };
  
  return (
    <Link to={to} onMouseEnter={handleMouseEnter}>
      {children}
    </Link>
  );
}
```

---

## Search Params and Hash

### Using Search Params

```jsx
import { useSearchParams } from 'react-router-dom';

function FilterComponent() {
  const [searchParams, setSearchParams] = useSearchParams();
  
  const category = searchParams.get('category');
  const sort = searchParams.get('sort') || 'asc';
  
  const setCategory = (cat) => {
    const params = new URLSearchParams(searchParams);
    params.set('category', cat);
    setSearchParams(params);
  };
  
  return (
    <div>
      <button onClick={() => setCategory('all')}>All</button>
      <button onClick={() => setCategory('electronics')}>Electronics</button>
      <button onClick={() => setCategory('clothing')}>Clothing</button>
      <p>Category: {category}</p>
      <p>Sort: {sort}</p>
    </div>
  );
}
```

### Using Hash

```jsx
import { useHashLocation } from 'react-router-dom';

function App() {
  const [hashLocation] = useHashLocation();
  
  return (
    <div>
      <nav>
        <a href="#/">Home</a>
        <a href="#/about">About</a>
        <a href="#/contact">Contact</a>
      </nav>
      <p>Current hash: {hashLocation.hash}</p>
    </div>
  );
}
```

---

## Best Practices

### Route Organization

```jsx
// routes.js
import { lazy } from 'react';

export const routes = [
  {
    path: '/',
    name: 'Home',
    component: lazy(() => import('./pages/Home')),
    exact: true
  },
  {
    path: '/about',
    name: 'About',
    component: lazy(() => import('./pages/About'))
  },
  {
    path: '/users',
    name: 'Users',
    routes: [
      { path: '/', component: UsersList },
      { path: '/:id', component: UserDetail }
    ]
  }
];
```

### Error Boundaries

```jsx
import { ErrorBoundary } from 'react-router-dom';

function App() {
  return (
    <ErrorBoundary>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
      </Routes>
    </ErrorBoundary>
  );
}
```

### 404 Handling

```jsx
// Always put wildcard route at the end
<Routes>
  <Route path="/" element={<Home />} />
  <Route path="/about" element={<About />} />
  <Route path="*" element={<NotFound />} />
</Routes>
```

---

## Summary

### Key Takeaways

1. **BrowserRouter**: Wraps the application
2. **Routes/Route**: Define routing structure
3. **Link/NavLink**: Navigation components
4. **useParams**: Access route parameters
5. **useNavigate**: Programmatic navigation
6. **Outlet**: Nested route rendering
7. **ProtectedRoute**: Route guards

### Next Steps

- Continue with: [05_REACT_STATE_MANAGEMENT.md](05_REACT_STATE_MANAGEMENT.md)
- Study Redux or Zustand for complex state
- Implement authentication flow

---

## Cross-References

- **Previous**: [03_REACT_HOOKS_DEEP_DIVE.md](03_REACT_HOOKS_DEEP_DIVE.md)
- **Next**: [05_REACT_STATE_MANAGEMENT.md](05_REACT_STATE_MANAGEMENT.md)

---

*Last updated: 2024*