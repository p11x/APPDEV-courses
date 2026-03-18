# Higher-Order Components (HOC)

## Overview

A Higher-Order Component (HOC) is an advanced React pattern for reusing component logic. HOCs are functions that take a component and return a new enhanced component. This pattern is similar to higher-order functions in JavaScript - functions that take functions as arguments or return functions. While hooks have largely replaced HOCs in modern React, understanding HOCs is still valuable for working with older codebases and understanding component composition.

## Prerequisites

- Understanding of React components
- Knowledge of props and state
- Familiarity with JavaScript closures and functions
- Understanding of component composition

## Core Concepts

### What is a Higher-Order Component?

A HOC is a function that:
1. Takes a component as input
2. Returns a new enhanced component
3. Can add props, state, or behavior

```jsx
// File: src/hoc-basics.jsx

import React, { useState, useEffect } from 'react';

// Simple HOC - adds loading state
function withLoading(Component) {
  return function WithLoading({ isLoading, ...props }) {
    if (isLoading) {
      return <div>Loading...</div>;
    }
    return <Component {...props} />;
  };
}

// Usage
function UserList({ users }) {
  return (
    <ul>
      {users.map(user => <li key={user.id}>{user.name}</li>)}
    </ul>
  );
}

// Enhanced component with loading
const UserListWithLoading = withLoading(UserList);

// Using it
function App() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    fetchUsers().then(data => {
      setUsers(data);
      setLoading(false);
    });
  }, []);
  
  return (
    <UserListWithLoading users={users} isLoading={loading} />
  );
}
```

### Common HOC Use Cases

```jsx
// File: src/hoc-usecases.jsx

import React, { useState, useEffect } from 'react';

// 1. Data Fetching HOC
function withDataFetcher(WrappedComponent, fetchFn) {
  return function WithDataFetcher(props) {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    
    useEffect(() => {
      async function load() {
        try {
          setLoading(true);
          const result = await fetchFn(props);
          setData(result);
        } catch (err) {
          setError(err);
        } finally {
          setLoading(false);
        }
      }
      load();
    }, [fetchFn, props.userId]); // Add dependencies
    
    return (
      <WrappedComponent
        {...props}
        data={data}
        loading={loading}
        error={error}
      />
    );
  };
}

// 2. Authentication HOC
function withAuth(WrappedComponent) {
  return function WithAuth(props) {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [loading, setLoading] = useState(true);
    
    useEffect(() => {
      // Check auth status
      const token = localStorage.getItem('authToken');
      setIsAuthenticated(!!token);
      setLoading(false);
    }, []);
    
    if (loading) {
      return <div>Checking auth...</div>;
    }
    
    if (!isAuthenticated) {
      return <div>Please log in</div>;
    }
    
    return <WrappedComponent {...props} />;
  };
}

// 3. Theme HOC
function withTheme(WrappedComponent) {
  return function WithTheme(props) {
    const [theme, setTheme] = useState('light');
    
    const toggleTheme = () => {
      setTheme(prev => prev === 'light' ? 'dark' : 'light');
    };
    
    const themeObject = {
      theme,
      toggleTheme,
      isDark: theme === 'dark'
    };
    
    return (
      <WrappedComponent
        {...props}
        theme={themeObject}
      />
    );
  };
}

// 4. Click Outside HOC
function withClickOutside(WrappedComponent) {
  return function WithClickOutside(props) {
    const ref = React.useRef(null);
    
    useEffect(() => {
      function handleClickOutside(event) {
        if (ref.current && !ref.current.contains(event.target)) {
          props.onClickOutside?.();
        }
      }
      
      document.addEventListener('mousedown', handleClickOutside);
      return () => document.removeEventListener('mousedown', handleClickOutside);
    }, [props.onClickOutside]);
    
    return <div ref={ref}><WrappedComponent {...props} /></div>;
  };
}
```

### Building Real HOCs

```jsx
// File: src/real-hocs.jsx

import React, { useState, useEffect } from 'react';

// HOC: Add data fetching capabilities
function withData(WrappedComponent, config) {
  const { url, propName = 'data' } = config;
  
  return function WithData(props) {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    
    useEffect(() => {
      let mounted = true;
      
      async function fetchData() {
        try {
          setLoading(true);
          setError(null);
          
          const response = await fetch(url);
          if (!response.ok) throw new Error('Failed to fetch');
          
          const result = await response.json();
          
          if (mounted) {
            setData(result);
          }
        } catch (err) {
          if (mounted) {
            setError(err.message);
          }
        } finally {
          if (mounted) {
            setLoading(false);
          }
        }
      }
      
      fetchData();
      
      return () => { mounted = false; };
    }, [url]);
    
    return (
      <WrappedComponent
        {...props}
        {...{ [propName]: data, loading, error }}
      />
    );
  };
}

// HOC: Add polling capabilities
function withPolling(WrappedComponent, interval = 30000) {
  return function WithPolling(props) {
    const [key, setKey] = useState(0);
    
    useEffect(() => {
      const timer = setInterval(() => {
        setKey(prev => prev + 1);
      }, interval);
      
      return () => clearInterval(timer);
    }, [interval]);
    
    return <WrappedComponent key={key} {...props} />;
  };
}

// HOC: Add keyboard shortcuts
function withKeyboardShortcuts(WrappedComponent, shortcuts = {}) {
  return function WithKeyboardShortcuts(props) {
    useEffect(() => {
      function handleKeyDown(event) {
        const handler = shortcuts[event.key];
        if (handler) {
          handler(event);
        }
      }
      
      window.addEventListener('keydown', handleKeyDown);
      return () => window.removeEventListener('keydown', handleKeyDown);
    }, [shortcuts]);
    
    return <WrappedComponent {...props} />;
  };
}

// Example usage
function UserProfile({ data, loading, error }) {
  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  
  return (
    <div>
      <h2>{data?.name}</h2>
      <p>{data?.email}</p>
    </div>
  );
}

// Enhanced with data fetching
const UserProfileWithData = withData(UserProfile, {
  url: '/api/user/1',
  propName: 'userData'
});

// Enhanced with polling
const UserProfileWithPolling = withPolling(UserProfileWithData, 10000);

// Enhanced with keyboard shortcuts
const UserProfileWithShortcuts = withKeyboardShortcuts(UserProfileWithPolling, {
  'r': () => console.log('Refresh'),
  'e': () => console.log('Edit')
});

export default UserProfileWithShortcuts;
```

## Common Mistakes

### Mistake 1: Not Forwarding Refs

```jsx
// ❌ WRONG - Ref is lost
function BadHOC(WrappedComponent) {
  return function WithSomething(props) {
    return <WrappedComponent {...props} />;
  };
}

// ✅ CORRECT - Forward the ref
import React from 'react';

function GoodHOC(WrappedComponent) {
  return React.forwardRef(function WithSomething(props, ref) {
    return <WrappedComponent ref={ref} {...props} />;
  });
}
```

### Mistake 2: Not Copying Static Methods

```jsx
// ❌ WRONG - Static methods are lost
function BadHOC(WrappedComponent) {
  return function WithSomething(props) {
    return <WrappedComponent {...props} />;
  }
}

// UserProfile.someMethod won't be available

// ✅ CORRECT - Copy static methods
import React from 'react';
import hoistNonReactStatic from 'hoist-non-react-statics';

function GoodHOC(WrappedComponent) {
  function WithSomething(props, ref) {
    return <WrappedComponent ref={ref} {...props} />;
  }
  
  hoistNonReactStatic(WithSomething, WrappedComponent);
  
  return React.forwardRef(WithSomething);
}
```

### Mistake 3: Wrapping Too Many Times

```jsx
// ❌ WRONG - Too many HOC layers
const Component = withAuth(withTheme(withData(withLogging(MyComponent))));

// Hard to debug and maintain

// ✅ CORRECT - Use custom hooks instead for new code
// Modern React code should prefer hooks over HOCs
```

## Real-World Example

```jsx
// File: src/hoc/withLogger.js

import React, { useEffect, useRef } from 'react';

// Logger HOC - logs component lifecycle and props
function withLogger(WrappedComponent, options = {}) {
  const { prefix = 'Component', logProps = false } = options;
  
  return React.forwardRef(function WithLogger(props, ref) {
    const renderCount = useRef(0);
    const prevProps = useRef(props);
    
    useEffect(() => {
      renderCount.current += 1;
      console.log(`[${prefix}] Rendered ${renderCount.current} times`);
      
      if (logProps) {
        console.log(`[${prefix}] Props:`, props);
        console.log(`[${prefix}] Prev Props:`, prevProps.current);
      }
      
      prevProps.current = props;
    });
    
    useEffect(() => {
      console.log(`[${prefix}] Mounted`);
      
      return () => {
        console.log(`[${prefix}] Unmounted`);
      };
    }, []);
    
    return <WrappedComponent ref={ref} {...props} />;
  });
}

export default withLogger;
```

```jsx
// File: src/hoc/withErrorBoundary.js

import React, { Component } from 'react';

// Error Boundary HOC - catches errors in child components
function withErrorBoundary(WrappedComponent, errorHandler) {
  class ErrorBoundary extends Component {
    constructor(props) {
      super(props);
      this.state = { hasError: false, error: null };
    }
    
    static getDerivedStateFromError(error) {
      return { hasError: true, error };
    }
    
    componentDidCatch(error, errorInfo) {
      console.error('Error caught by boundary:', error, errorInfo);
      
      if (errorHandler) {
        errorHandler(error, errorInfo);
      }
    }
    
    render() {
      if (this.state.hasError) {
        return (
          <div style={{ 
            padding: '20px', 
            backgroundColor: '#ffebee',
            border: '1px solid #ef5350',
            borderRadius: '4px'
          }}>
            <h2 style={{ color: '#c62828', margin: '0 0 10px 0' }}>
              Something went wrong
            </h2>
            <p style={{ color: '#c62828' }}>
              {this.state.error?.message || 'An unexpected error occurred'}
            </p>
            <button 
              onClick={() => this.setState({ hasError: false, error: null })}
              style={{
                padding: '8px 16px',
                backgroundColor: '#2196F3',
                color: 'white',
                border: 'none',
                borderRadius: '4px',
                cursor: 'pointer'
              }}
            >
              Try Again
            </button>
          </div>
        );
      }
      
      return <WrappedComponent {...this.props} />;
    }
  }
  
  ErrorBoundary.displayName = `WithErrorBoundary(${WrappedComponent.name || 'Component'})`;
  
  return ErrorBoundary;
}

export default withErrorBoundary;
```

```jsx
// File: src/components/EnhancedProfile.jsx

import React, { useState, useEffect } from 'react';
import withLogger from '../hoc/withLogger';
import withErrorBoundary from '../hoc/withErrorBoundary';

// Base component
function UserProfile({ userId, userData, loading, error }) {
  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  
  return (
    <div style={{ padding: '20px' }}>
      <h2>{userData?.name}</h2>
      <p>Email: {userData?.email}</p>
      <p>Role: {userData?.role}</p>
    </div>
  );
}

// Simulated data fetch
async function fetchUser(userId) {
  // Simulate API call
  await new Promise(resolve => setTimeout(resolve, 1000));
  
  if (userId === 'error') {
    throw new Error('Failed to fetch user');
  }
  
  return {
    id: userId,
    name: 'John Doe',
    email: 'john@example.com',
    role: 'Admin'
  };
}

// Create HOC to add data fetching
function withUserData(WrappedComponent) {
  return function WithUserData({ userId, ...props }) {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    
    useEffect(() => {
      let mounted = true;
      
      async function load() {
        try {
          setLoading(true);
          setError(null);
          const result = await fetchUser(userId);
          if (mounted) setData(result);
        } catch (err) {
          if (mounted) setError(err.message);
        } finally {
          if (mounted) setLoading(false);
        }
      }
      
      load();
      
      return () => { mounted = false; };
    }, [userId]);
    
    return (
      <WrappedComponent
        {...props}
        userData={data}
        loading={loading}
        error={error}
      />
    );
  };
}

// Apply multiple HOCs
const EnhancedProfile = withLogger(
  withErrorBoundary(
    withUserData(UserProfile),
    (error) => console.error('Error handler:', error)
  ),
  { prefix: 'UserProfile', logProps: true }
);

function App() {
  const [userId, setUserId] = useState('1');
  
  return (
    <div>
      <div style={{ marginBottom: '20px' }}>
        <button onClick={() => setUserId('1')}>User 1</button>
        <button onClick={() => setUserId('2')}>User 2</button>
        <button onClick={() => setUserId('error')}>Trigger Error</button>
      </div>
      
      <EnhancedProfile userId={userId} />
    </div>
  );
}

export default App;
```

## Key Takeaways

- HOCs are functions that take a component and return an enhanced version
- Common uses: data fetching, authentication, theming, logging
- HOCs should forward refs using React.forwardRef
- Static methods need to be manually copied with hoistNonReactStatic
- Modern React prefers hooks over HOCs for new code
- HOCs create additional component layers, which can affect debugging

## What's Next

Now let's explore the render props pattern, another technique for sharing code between components that's been largely replaced by hooks but is still worth understanding.
