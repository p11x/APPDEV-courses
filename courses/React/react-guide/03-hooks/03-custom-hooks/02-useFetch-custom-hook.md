# useFetch Custom Hook

## Overview

useFetch is a custom hook that handles data fetching in a reusable way. It manages loading states, errors, and data all in one place, making it easy to fetch data in any component. This guide shows you how to build a robust useFetch hook that handles common data fetching scenarios.

## Prerequisites

- Understanding of custom hooks
- Knowledge of useState and useEffect
- Familiarity with fetch API

## Core Concepts

### Building useFetch

```javascript
// File: src/hooks/useFetch.js

import { useState, useEffect, useRef } from 'react';

function useFetch(url, options = {}) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const abortControllerRef = useRef(null);
  
  useEffect(() => {
    // Create new abort controller for this request
    abortControllerRef.current = new AbortController();
    
    async function fetchData() {
      try {
        setLoading(true);
        setError(null);
        
        const response = await fetch(url, {
          ...options,
          signal: abortControllerRef.current.signal
        });
        
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const json = await response.json();
        setData(json);
      } catch (err) {
        if (err.name !== 'AbortError') {
          setError(err.message);
        }
      } finally {
        if (!abortControllerRef.current?.signal.aborted) {
          setLoading(false);
        }
      }
    }
    
    fetchData();
    
    // Cleanup - abort fetch on unmount or URL change
    return () => {
      abortControllerRef.current?.abort();
    };
  }, [url, options.method, options.body]);
  
  return { data, loading, error, refetch: () => {
    abortControllerRef.current?.abort();
    setLoading(true);
  }};
}

// Usage
function UserProfile({ userId }) {
  const { data, loading, error } = useFetch(`/api/users/${userId}`);
  
  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;
  
  return <div>{data.name}</div>;
}
```

## Key Takeaways

- useFetch manages loading, error, and data states
- Uses AbortController for cleanup
- Returns refetch function for manual refresh

## What's Next

Let's create useLocalStorage hook for persisting data.
