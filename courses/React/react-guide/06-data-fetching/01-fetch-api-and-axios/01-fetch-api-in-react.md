# Fetch API in React

## Overview
Fetching data from APIs is one of the most common tasks in React applications. The Fetch API is a built-in browser API that allows you to make HTTP requests. In this guide, you'll learn how to use the Fetch API effectively within React components, including handling loading states, errors, and cleanup to prevent memory leaks.

## Prerequisites
- Basic understanding of JavaScript async/await
- Knowledge of React components and hooks
- Familiarity with JSON data format
- Understanding of React's useState and useEffect hooks

## Core Concepts

### Making Your First API Call with useEffect
The Fetch API works seamlessly with React's useEffect hook. When fetching data, you need to handle several concerns: making the request, storing the response, handling loading states, managing errors, and cleaning up if the component unmounts before the request completes.

```jsx
// File: src/components/UserList.jsx

// We import React and two hooks: useState for managing data and useEffect for side effects
import React, { useState, useEffect } from 'react';

function UserList() {
  // useState stores three pieces of state: the data, loading status, and any errors
  // Initial state is null for data (no users yet), true for loading, null for error
  const [users, setUsers] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  // useEffect runs after the component renders. The empty dependency array [] 
  // means this effect runs only once when the component mounts (like componentDidMount)
  useEffect(() => {
    // We define an async function inside because useEffect cannot be async directly
    // This allows us to use await for cleaner asynchronous code
    const fetchUsers = async () => {
      try {
        // Make GET request to JSONPlaceholder API (a free fake API for testing)
        // fetch() returns a Response object, not the actual data
        const response = await fetch('https://jsonplaceholder.typicode.com/users');
        
        // Check if the HTTP response was successful (status 200-299)
        // If not, throw an error to be caught by the catch block
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
        
        // Parse the response body as JSON
        // This returns a promise, so we await it
        const data = await response.json();
        
        // Update state with the fetched data
        // This triggers a re-render with the new data
        setUsers(data);
      } catch (err) {
        // Catch any network errors or HTTP errors
        // Store the error message in state to display to the user
        setError(err.message);
      } finally {
        // This runs whether the request succeeded or failed
        // Set loading to false regardless of outcome
        setIsLoading(false);
      }
    };

    // Call the async function to execute the fetch
    fetchUsers();
    
  }, []); // Empty dependency array = run once on mount only

  // Conditional rendering: show loading state while fetching
  if (isLoading) {
    return <div>Loading users...</div>;
  }

  // If there was an error, display it to the user
  if (error) {
    return <div>Error: {error}</div>;
  }

  // Render the list of users once loaded
  return (
    <ul>
      {/* Map through users array and render each user */}
      {users?.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}

export default UserList;
```

### Handling Different HTTP Methods
The Fetch API supports all HTTP methods: GET (retrieve data), POST (create data), PUT (update data), PATCH (partial update), and DELETE (remove data). Each method has a slightly different configuration.

```jsx
// File: src/api/posts.js

// POST request to create new data
// Used when submitting forms or creating new resources
async function createPost(postData) {
  // fetch() takes two arguments: URL and options object
  const response = await fetch('https://jsonplaceholder.typicode.com/posts', {
    method: 'POST', // Specify the HTTP method
    headers: {
      // Tell the server we're sending JSON data
      'Content-Type': 'application/json',
      // Optional: Add authorization header
      // 'Authorization': `Bearer ${token}`
    },
    // Convert JavaScript object to JSON string for the request body
    body: JSON.stringify(postData),
  });
  
  // Check for errors and parse response
  if (!response.ok) {
    throw new Error('Failed to create post');
  }
  
  return response.json();
}

// PUT request to fully update existing data
// Replaces the entire resource with new data
async function updatePost(postId, updatedData) {
  const response = await fetch(`https://jsonplaceholder.typicode.com/posts/${postId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    // Include the ID in the body for RESTful consistency
    body: JSON.stringify({ ...updatedData, id: postId }),
  });
  
  if (!response.ok) {
    throw new Error('Failed to update post');
  }
  
  return response.json();
}

// PATCH request for partial updates
// Only sends the fields that need to be changed
async function patchPostTitle(postId, newTitle) {
  const response = await fetch(`https://jsonplaceholder.typicode.com/posts/${postId}`, {
    method: 'PATCH',
    headers: {
      'Content-Type': 'application/json',
    },
    // Only send the field we want to change
    body: JSON.stringify({ title: newTitle }),
  });
  
  if (!response.ok) {
    throw new Error('Failed to patch post');
  }
  
  return response.json();
}

// DELETE request to remove data
async function deletePost(postId) {
  const response = await fetch(`https://jsonplaceholder.typicode.com/posts/${postId}`, {
    method: 'DELETE', // No body or headers needed for DELETE
  });
  
  if (!response.ok) {
    throw new Error('Failed to delete post');
  }
  
  // DELETE typically returns empty response or success status
  return true;
}

export { createPost, updatePost, patchPostTitle, deletePost };
```

### Preventing Memory Leaks with AbortController
One of the most important patterns when fetching data in React is handling component unmounting. If a user navigates away from a page before a request completes, updating state on an unmounted component causes a memory leak and warning.

```jsx
// File: src/components/SearchResults.jsx

import React, { useState, useEffect } from 'react';

function SearchResults({ query }) {
  const [results, setResults] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    // Create an AbortController to cancel the request if component unmounts
    // This prevents the "Can't perform a React state update on an unmounted component" warning
    const abortController = new AbortController();
    
    // The signal property is passed to fetch to enable cancellation
    const { signal } = abortController;

    const search = async () => {
      // Don't search for empty queries
      if (!query.trim()) {
        setResults([]);
        return;
      }

      setIsLoading(true);

      try {
        // Pass the abort signal to fetch
        const response = await fetch(
          `https://jsonplaceholder.typicode.com/posts?title_like=${query}`,
          { signal } // This links the abort controller to the fetch request
        );
        
        if (!response.ok) {
          throw new Error('Search failed');
        }
        
        const data = await response.json();
        
        // Check if the component is still mounted before updating state
        // This is a safeguard in case the request completes after unmount
        if (!signal.aborted) {
          setResults(data);
        }
      } catch (err) {
        // Ignore abort errors - they're expected when cancelling
        // Check if the error is from abort or from the fetch itself
        if (err.name !== 'AbortError') {
          console.error('Search error:', err);
        }
      } finally {
        // Only update loading state if not aborted
        if (!signal.aborted) {
          setIsLoading(false);
        }
      }
    };

    // Debounce: wait 300ms after user stops typing before searching
    // This prevents making an API call for every keystroke
    const debounceTimer = setTimeout(search, 300);

    // Cleanup function runs before the next effect runs or on unmount
    return () => {
      // Cancel the in-flight request when component unmounts or query changes
      abortController.abort();
      // Clear the debounce timer to prevent stale requests
      clearTimeout(debounceTimer);
    };
  }, [query]); // Re-run effect when query changes

  return (
    <div>
      {isLoading && <p>Searching...</p>}
      {results.map(post => (
        <div key={post.id}>
          <h3>{post.title}</h3>
          <p>{post.body}</p>
        </div>
      ))}
    </div>
  );
}

export default SearchResults;
```

## Common Mistakes

### Mistake 1: Forgetting to Check Response.ok
Many developers assume fetch succeeds if it doesn't throw an error. However, fetch only throws on network failure, not HTTP errors like 404 or 500.

```jsx
// ❌ WRONG — Doesn't handle HTTP errors
useEffect(() => {
  fetch('https://api.example.com/data')
    .then(response => response.json()) // Will fail on 404 but error is unclear
    .then(setData);
}, []);

// ✅ CORRECT — Check response.ok before parsing
useEffect(() => {
  fetch('https://api.example.com/data')
    .then(response => {
      if (!response.ok) {
        throw new Error(`HTTP error: ${response.status}`);
      }
      return response.json();
    })
    .then(setData)
    .catch(err => console.error(err));
}, []);
```

### Mistake 2: Not Handling the Empty State
When fetching data, you need to handle three states: loading, error, and success. Showing undefined data causes errors.

```jsx
// ❌ WRONG — No loading or error handling, crashes on undefined
function UserList() {
  const [users, setUsers] = useState(); // undefined by default
  
  useEffect(() => {
    fetch('/api/users').then(r => r.json()).then(setUsers);
  }, []);
  
  return (
    <ul>
      {users.map(u => <li key={u.id}>{u.name}</li>} // Crash! users is undefined
    </ul>
  );
}

// ✅ CORRECT — Handle all states properly
function UserList() {
  const [users, setUsers] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetch('/api/users')
      .then(r => {
        if (!r.ok) throw new Error('Failed');
        return r.json();
      })
      .then(setUsers)
      .catch(setError)
      .finally(() => setIsLoading(false));
  }, []);

  if (isLoading) return <Spinner />;
  if (error) return <ErrorMessage error={error} />;
  if (!users?.length) return <EmptyState />;

  return (
    <ul>
      {users.map(u => <li key={u.id}>{u.name}</li>)}
    </ul>
  );
}
```

### Mistake 3: Missing Dependency in useEffect
The dependency array controls when your effect re-runs. Forgetting dependencies causes stale data or infinite loops.

```jsx
// ❌ WRONG — Missing dependencies causes stale closures
function SearchComponent() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);

  useEffect(() => {
    fetch(`/api/search?q=${query}`)
      .then(r => r.json())
      .then(setResults);
    // Missing [query] — effect never re-runs when query changes!
  }, []);

  return <input value={query} onChange={e => setQuery(e.target.value)} />;
}

// ✅ CORRECT — Include all values used inside the effect
function SearchComponent() {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);

  useEffect(() => {
    const debouncedSearch = setTimeout(() => {
      fetch(`/api/search?q=${query}`)
        .then(r => r.json())
        .then(setResults);
    }, 300);

    return () => clearTimeout(debouncedSearch);
  }, [query]); // Re-run when query changes

  return <input value={query} onChange={e => setQuery(e.target.value)} />;
}
```

## Real-World Example
Building a complete data fetching solution with a custom hook that handles all the boilerplate.

```jsx
// File: src/hooks/useFetch.js

import { useState, useEffect, useCallback } from 'react';

/**
 * Custom hook for fetching data with loading, error, and success states
 * @param {string} url - The API endpoint to fetch from
 * @param {object} options - Optional fetch configuration
 * @returns {object} { data, isLoading, error, refetch }
 */
function useFetch(url, options = {}) {
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchData = useCallback(async () => {
    // Reset states when refetching
    setIsLoading(true);
    setError(null);

    const abortController = new AbortController();

    try {
      const response = await fetch(url, {
        ...options,
        signal: abortController.signal,
      });

      if (!response.ok) {
        throw new Error(`Error ${response.status}: ${response.statusText}`);
      }

      const result = await response.json();
      setData(result);
    } catch (err) {
      // Ignore abort errors
      if (err.name !== 'AbortError') {
        setError(err.message);
      }
    } finally {
      // Only update loading if not aborted
      if (!abortController.signal.aborted) {
        setIsLoading(false);
      }
    }

    // Cleanup: abort on unmount or new request
    return () => abortController.abort();
  }, [url, JSON.stringify(options)]); // Re-create when URL or options change

  // Auto-fetch on mount
  useEffect(() => {
    const cleanup = fetchData();
    return () => {
      // The fetchData function returns a cleanup function
      if (cleanup && typeof cleanup.then === 'function') {
        cleanup.then(abort => abort?.());
      }
    };
  }, [fetchData]);

  return { data, isLoading, error, refetch: fetchData };
}

export default useFetch;

// File: src/components/PostList.jsx - Using the custom hook
import React from 'react';
import useFetch from '../hooks/useFetch';

function PostList() {
  // Destructure the values from our custom hook
  const { data: posts, isLoading, error, refetch } = useFetch(
    'https://jsonplaceholder.typicode.com/posts?_limit=10'
  );

  if (isLoading) return <div className="spinner">Loading posts...</div>;
  if (error) return (
    <div className="error">
      <p>Failed to load posts: {error}</p>
      <button onClick={refetch}>Try Again</button>
    </div>
  );

  return (
    <div>
      <button onClick={refetch}>Refresh Posts</button>
      {posts?.map(post => (
        <article key={post.id}>
          <h2>{post.title}</h2>
          <p>{post.body}</p>
        </article>
      ))}
    </div>
  );
}

export default PostList;
```

## Key Takeaways
- Always handle the three states: loading, error, and success when fetching data
- Use AbortController to prevent memory leaks when components unmount during requests
- Check `response.ok` or `response.status` before parsing JSON — fetch doesn't throw on HTTP errors
- Include all dependencies in the useEffect dependency array to avoid stale closures
- Use the `finally` block to set loading to false regardless of success or failure
- Consider debouncing search requests to avoid excessive API calls

## What's Next
After mastering the Fetch API, continue to [Axios Setup and Interceptors](02-axios-setup-and-interceptors.md) to learn about a more powerful HTTP client with built-in error handling and request/response transformation.
