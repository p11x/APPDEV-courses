# useEffect Complete Guide

## Overview

useEffect is the hook that handles side effects in React components. Side effects include data fetching, subscriptions, manually changing the DOM, and timers. Understanding useEffect is crucial for building real-world React applications, as most applications need to interact with external systems, APIs, or browser APIs. This comprehensive guide covers all aspects of useEffect from basic usage to advanced patterns.

## Prerequisites

- Understanding of useState hook
- Knowledge of React component lifecycle
- Familiarity with JavaScript promises and async/await
- Understanding of cleanup functions

## Core Concepts

### What is useEffect?

useEffect lets you perform side effects in function components. It serves the same purpose as componentDidMount, componentDidUpdate, and componentWillUnmount in React class components, but unified into a single API.

```jsx
// File: src/useeffect-basics.jsx

import React, { useState, useEffect } from 'react';

function BasicExample() {
  const [count, setCount] = useState(0);
  
  // useEffect runs after every render by default
  useEffect(() => {
    // This runs after every render
    console.log('Component rendered!');
    console.log('Current count:', count);
    
    // You can do anything here - fetch data, subscribe, etc.
    document.title = `Count: ${count}`;
  });
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>Increment</button>
    </div>
  );
}
```

### Dependency Array

The second argument to useEffect is the dependency array. It controls when the effect runs:

```jsx
// File: src/useeffect-dependencies.jsx

import React, { useState, useEffect } from 'react';

function DependencyExamples() {
  const [count, setCount] = useState(0);
  const [name, setName] = useState('Alice');
  
  // 1. No dependency array - runs after EVERY render
  useEffect(() => {
    console.log('Runs after every render');
  });
  
  // 2. Empty dependency array - runs ONLY on mount
  useEffect(() => {
    console.log('Runs only once on mount');
    // Good for: API calls, subscriptions, setting up
  }, []);
  
  // 3. With dependencies - runs on mount AND when any dependency changes
  useEffect(() => {
    console.log('Runs when count changes:', count);
  }, [count]);
  
  // 4. Multiple dependencies - runs when ANY dependency changes
  useEffect(() => {
    console.log('Runs when count OR name changes');
  }, [count, name]);
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>Increment</button>
      <input value={name} onChange={e => setName(e.target.value)} />
    </div>
  );
}
```

### Cleanup Functions

useEffect can return a cleanup function that runs before the effect runs again and when the component unmounts:

```jsx
// File: src/useeffect-cleanup.jsx

import React, { useState, useEffect } from 'react';

function CleanupExample() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    // Set up subscription
    const subscription = subscribeToData(id => {
      setData(id);
    });
    
    // Cleanup function - runs before unmount or before next effect
    return () => {
      // Clean up subscription
      subscription.unsubscribe();
    };
  }, []); // Empty deps = run once on mount, cleanup on unmount
  
  return <div>{data}</div>;
}

function TimerExample() {
  const [seconds, setSeconds] = useState(0);
  
  useEffect(() => {
    // Set up interval
    const interval = setInterval(() => {
      setSeconds(s => s + 1);
    }, 1000);
    
    // Cleanup - clear interval when component unmounts
    return () => {
      clearInterval(interval);
    };
  }, []); // Empty deps = run once
  
  return <div>Seconds: {seconds}</div>;
}

function DocumentTitle() {
  const [count, setCount] = useState(0);
  
  useEffect(() => {
    // Update document title
    document.title = `Count: ${count}`;
    
    // Cleanup - restore original title
    return () => {
      document.title = 'React App';
    };
  }, [count]); // Run when count changes
  
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}
```

### Data Fetching with useEffect

```jsx
// File: src/useeffect-fetch.jsx

import React, { useState, useEffect } from 'react';

function UserList() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  useEffect(() => {
    // Create abort controller for cleanup
    const controller = new AbortController();
    const signal = controller.signal;
    
    async function fetchUsers() {
      try {
        setLoading(true);
        setError(null);
        
        const response = await fetch('/api/users', { signal });
        
        if (!response.ok) {
          throw new Error('Failed to fetch users');
        }
        
        const data = await response.json();
        setUsers(data);
      } catch (err) {
        if (err.name !== 'AbortError') {
          setError(err.message);
        }
      } finally {
        if (!signal.aborted) {
          setLoading(false);
        }
      }
    }
    
    fetchUsers();
    
    // Cleanup - abort fetch if component unmounts
    return () => {
      controller.abort();
    };
  }, []); // Empty deps = fetch once on mount
  
  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  
  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

## Common Mistakes

### Mistake 1: Missing Dependency Array

```jsx
// ❌ WRONG - No dependency array = runs after EVERY render!
function BadComponent() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    fetch('/api/data').then(r => r.json()).then(setData);
    // This causes infinite loop if fetch triggers state update!
  });
  
  return <div>{data}</div>;
}

// ✅ CORRECT - Empty array = run once on mount
function GoodComponent() {
  const [data, setData] = useState(null);
  
  useEffect(() => {
    fetch('/api/data').then(r => r.json()).then(setData);
  }, []); // Only runs once
  
  return <div>{data}</div>;
}
```

### Mistake 2: Stale Closures

```jsx
// ❌ WRONG - Using stale state value in effect
function BadComponent() {
  const [count, setCount] = useState(0);
  
  useEffect(() => {
    const interval = setInterval(() => {
      console.log('Count is:', count); // Always 0!
    }, 1000);
    
    return () => clearInterval(interval);
  }, []); // Empty deps - count never updates!
  
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}

// ✅ CORRECT - Include dependencies or use functional updates
function GoodComponent() {
  const [count, setCount] = useState(0);
  
  useEffect(() => {
    const interval = setInterval(() => {
      // Use functional update
      setCount(c => {
        console.log('Count is:', c); // Always current!
        return c;
      });
    }, 1000);
    
    return () => clearInterval(interval);
  }, []); // Still works because we use functional update
  
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}

// Alternative: Include dependency
function AlsoGoodComponent() {
  const [count, setCount] = useState(0);
  
  useEffect(() => {
    const interval = setInterval(() => {
      console.log('Count is:', count);
    }, 1000);
    
    return () => clearInterval(interval);
  }, [count]); // Runs when count changes
  
  return <button onClick={() => setCount(c => c + 1)}>{count}</button>;
}
```

### Mistake 3: Not Cleaning Up

```jsx
// ❌ WRONG - Memory leak from subscription
function BadComponent() {
  useEffect(() => {
    const subscription = someAPI.subscribe();
    // No cleanup!
  }, []);
  
  return <div />;
}

// ✅ CORRECT - Always clean up
function GoodComponent() {
  useEffect(() => {
    const subscription = someAPI.subscribe();
    
    return () => {
      subscription.unsubscribe(); // Clean up!
    };
  }, []);
  
  return <div />;
}
```

## Real-World Example

```jsx
// File: src/components/DataDashboard.jsx

import React, { useState, useEffect } from 'react';

function DataDashboard() {
  const [users, setUsers] = useState([]);
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [lastUpdated, setLastUpdated] = useState(null);
  
  // Fetch data
  useEffect(() => {
    const controller = new AbortController();
    
    async function fetchData() {
      try {
        setLoading(true);
        setError(null);
        
        const [usersRes, statsRes] = await Promise.all([
          fetch('/api/users', { signal: controller.signal }),
          fetch('/api/stats', { signal: controller.signal })
        ]);
        
        if (!usersRes.ok || !statsRes.ok) {
          throw new Error('Failed to fetch data');
        }
        
        const [usersData, statsData] = await Promise.all([
          usersRes.json(),
          statsRes.json()
        ]);
        
        setUsers(usersData);
        setStats(statsData);
        setLastUpdated(new Date());
      } catch (err) {
        if (err.name !== 'AbortError') {
          setError(err.message);
        }
      } finally {
        if (!controller.signal.aborted) {
          setLoading(false);
        }
      }
    }
    
    fetchData();
    
    return () => controller.abort();
  }, []); // Fetch once on mount
  
  // Auto-refresh every 30 seconds
  useEffect(() => {
    if (loading || error) return;
    
    const interval = setInterval(() => {
      // Same fetch logic would go here
      console.log('Auto-refresh...');
    }, 30000);
    
    return () => clearInterval(interval);
  }, [loading, error]);
  
  // Update document title
  useEffect(() => {
    if (users.length > 0) {
      document.title = `${users.length} Users - Dashboard`;
    }
    
    return () => {
      document.title = 'Dashboard';
    };
  }, [users]);
  
  // Keyboard shortcuts
  useEffect(() => {
    function handleKeyDown(e) {
      if (e.key === 'r' && (e.ctrlKey || e.metaKey)) {
        e.preventDefault();
        // Trigger refresh
        window.location.reload();
      }
    }
    
    window.addEventListener('keydown', handleKeyDown);
    
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);
  
  // Loading state
  if (loading) {
    return (
      <div style={{ padding: '40px', textAlign: 'center' }}>
        <div>Loading...</div>
      </div>
    );
  }
  
  // Error state
  if (error) {
    return (
      <div style={{ padding: '40px', textAlign: 'center' }}>
        <div style={{ color: 'red' }}>Error: {error}</div>
        <button onClick={() => window.location.reload()}>Retry</button>
      </div>
    );
  }
  
  // Success state
  return (
    <div style={{ padding: '20px' }}>
      <h1>Dashboard</h1>
      {lastUpdated && (
        <p style={{ color: '#666', fontSize: '14px' }}>
          Last updated: {lastUpdated.toLocaleTimeString()}
        </p>
      )}
      
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '20px' }}>
        <div style={{ padding: '20px', border: '1px solid #ddd', borderRadius: '8px' }}>
          <h3>Total Users</h3>
          <p style={{ fontSize: '32px', fontWeight: 'bold' }}>{users.length}</p>
        </div>
        
        <div style={{ padding: '20px', border: '1px solid #ddd', borderRadius: '8px' }}>
          <h3>Active</h3>
          <p style={{ fontSize: '32px', fontWeight: 'bold', color: 'green' }}>
            {stats?.active || 0}
          </p>
        </div>
        
        <div style={{ padding: '20px', border: '1px solid #ddd', borderRadius: '8px' }}>
          <h3>Revenue</h3>
          <p style={{ fontSize: '32px', fontWeight: 'bold', color: 'blue' }}>
            ${stats?.revenue?.toLocaleString() || 0}
          </p>
        </div>
      </div>
      
      <h2>Recent Users</h2>
      <ul style={{ listStyle: 'none', padding: 0 }}>
        {users.slice(0, 5).map(user => (
          <li key={user.id} style={{ padding: '10px', borderBottom: '1px solid #eee' }}>
            {user.name} - {user.email}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default DataDashboard;
```

## Key Takeaways

- useEffect runs after render by default
- Use dependency array to control when effect runs
- Empty array = run once on mount, cleanup on unmount
- Include all values that the effect uses in the dependency array
- Always clean up subscriptions, timers, and event listeners
- Use AbortController for data fetching to prevent memory leaks
- Use functional updates to avoid stale closure issues

## What's Next

Now let's explore useRef - a hook for accessing and manipulating DOM elements, and persisting values across renders without triggering re-renders.
