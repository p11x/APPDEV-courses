# Error Handling Strategies in React

## Overview
Error handling is critical for building robust React applications. When making API requests, things can go wrong: network failures, server errors, invalid data, or timeouts. This guide covers comprehensive strategies for handling errors at different levels of your React application, from individual components to global error boundaries.

## Prerequisites
- Understanding of JavaScript try/catch/finally
- Knowledge of React hooks (useState, useEffect)
- Familiarity with Fetch API or Axios
- Understanding of React component lifecycle

## Core Concepts

### Error States in Data Fetching
Every API call can fail, and your UI needs to handle these failures gracefully. The key is to always expect errors and provide meaningful feedback to users.

```jsx
// File: src/components/UserList.jsx

import React, { useState, useEffect } from 'react';

function UserList() {
  // Store data, loading state, and error separately
  // This makes it easy to render different UI for each state
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        setIsLoading(true);
        setError(null); // Clear previous errors

        const response = await fetch('https://jsonplaceholder.typicode.com/users');
        
        if (!response.ok) {
          // Throw descriptive error for non-2xx responses
          throw new Error(`Failed to fetch users: ${response.status}`);
        }

        const data = await response.json();
        setUsers(data);
      } catch (err) {
        // Categorize errors for better user feedback
        if (err.name === 'TypeError' && err.message === 'Failed to fetch') {
          setError('Network error. Please check your internet connection.');
        } else if (err.message.includes('404')) {
          setError('The requested resource was not found.');
        } else if (err.message.includes('500')) {
          setError('Server error. Please try again later.');
        } else {
          setError(err.message || 'An unexpected error occurred');
        }
      } finally {
        // Always turn off loading, regardless of success or failure
        setIsLoading(false);
      }
    };

    fetchUsers();
  }, []);

  // Render loading state first
  if (isLoading) {
    return (
      <div className="loading-container">
        <div className="spinner"></div>
        <p>Loading users...</p>
      </div>
    );
  }

  // Render error state with retry option
  if (error) {
    return (
      <div className="error-container">
        <p className="error-message">{error}</p>
        <button onClick={() => window.location.reload()}>
          Try Again
        </button>
      </div>
    );
  }

  // Render empty state if no users
  if (users.length === 0) {
    return <p>No users found.</p>;
  }

  // Render data
  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}

export default UserList;
```

### Creating a Reusable Error Handler
Rather than repeating error handling logic in every component, create a custom hook that encapsulates error handling patterns.

```jsx
// File: src/hooks/useApi.js

import { useState, useCallback } from 'react';

/**
 * Custom hook for API calls with built-in error handling
 * @param {Function} apiFunction - The API call function (fetch, axios, etc.)
 * @returns {Object} state and execute function
 */
function useApi(apiFunction) {
  // Track all possible states
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  // Reset all state
  const reset = useCallback(() => {
    setData(null);
    setIsLoading(false);
    setError(null);
  }, []);

  // Execute the API call with error handling
  const execute = useCallback(async (...args) => {
    // Reset state before new request
    setIsLoading(true);
    setError(null);

    try {
      // Call the provided API function with any arguments
      const result = await apiFunction(...args);
      setData(result);
      return result; // Return data for immediate use
    } catch (err) {
      // Normalize error object
      const normalizedError = {
        message: err.message || 'An unexpected error occurred',
        status: err.response?.status,
        data: err.response?.data,
        isNetworkError: err.name === 'TypeError',
      };
      
      setError(normalizedError);
      throw normalizedError; // Re-throw for callers who need to handle it
    } finally {
      setIsLoading(false);
    }
  }, [apiFunction]);

  return {
    data,
    isLoading,
    error,
    execute,
    reset,
    // Helper booleans for convenience
    isIdle: !isLoading && !error && !data,
    isSuccess: !isLoading && !error && data,
    isError: !!error,
  };
}

export default useApi;

// File: src/components/PostDetail.jsx - Using the useApi hook

import React from 'react';
import useApi from '../hooks/useApi';

// Define API call separately for reusability
const fetchPost = async (postId) => {
  const response = await fetch(
    `https://jsonplaceholder.typicode.com/posts/${postId}`
  );
  
  if (!response.ok) {
    const error = new Error('Failed to fetch post');
    error.response = { status: response.status };
    throw error;
  }
  
  return response.json();
};

function PostDetail({ postId }) {
  // Destructure all states and the execute function
  const { data: post, isLoading, error, execute: fetchPostDetail } = useApi(fetchPost);

  // Fetch on mount or when postId changes
  React.useEffect(() => {
    fetchPostDetail(postId);
  }, [postId, fetchPostDetail]);

  if (isLoading) return <div>Loading post...</div>;
  
  if (error) {
    return (
      <div>
        <p>Error: {error.message}</p>
        <button onClick={() => fetchPostDetail(postId)}>
          Retry
        </button>
      </div>
    );
  }

  if (!post) return null;

  return (
    <article>
      <h1>{post.title}</h1>
      <p>{post.body}</p>
    </article>
  );
}

export default PostDetail;
```

### Error Boundaries
Error boundaries are React components that catch JavaScript errors anywhere in their child component tree. Unlike regular error handling, error boundaries preserve the rest of your app working while showing an error UI.

```jsx
// File: src/components/ErrorBoundary.jsx

import React, { Component } from 'react';

/**
 * ErrorBoundary is a class component that catches JavaScript errors
 * in its child components and displays a fallback UI
 */
class ErrorBoundary extends Component {
  // Initialize state to track if an error occurred
  constructor(props) {
    super(props);
    this.state = {
      hasError: false,
      error: null,
      errorInfo: null,
    };
  }

  // Static method - React calls this when a child throws an error
  // Must return a new state object to trigger a re-render with fallback UI
  static getDerivedStateFromError(error) {
    return { hasError: true };
  }

  // Lifecycle method - called after an error is caught
  // Useful for logging errors to an error reporting service
  componentDidCatch(error, errorInfo) {
    // Log error to console in development
    console.error('ErrorBoundary caught an error:', error, errorInfo);

    // Here you could send to error reporting service like Sentry
    // Sentry.captureException(error, { extra: errorInfo });
    
    // Store error info for display if needed
    this.setState({
      error,
      errorInfo,
    });
  }

  // Handler to reset the error state and retry
  handleReset = () => {
    this.setState({
      hasError: false,
      error: null,
      errorInfo: null,
    });
  };

  render() {
    // If an error occurred, render fallback UI
    if (this.state.hasError) {
      // Use custom fallback or default error UI
      if (this.props.fallback) {
        return this.props.fallback(this.state.error, this.handleReset);
      }

      return (
        <div className="error-boundary">
          <h2>Something went wrong</h2>
          <p>We're sorry, but something unexpected happened.</p>
          
          {/* Show error details in development */}
          {process.env.NODE_ENV === 'development' && (
            <details style={{ whiteSpace: 'pre-wrap' }}>
              <summary>Error Details</summary>
              {this.state.error?.toString()}
              <br />
              {this.state.errorInfo?.componentStack}
            </details>
          )}
          
          <button onClick={this.handleReset}>
            Try Again
          </button>
        </div>
      );
    }

    // No error - render children normally
    return this.props.children;
  }
}

export default ErrorBoundary;

// File: src/App.jsx - Using ErrorBoundary

import React from 'react';
import ErrorBoundary from './components/ErrorBoundary';
import UserList from './components/UserList';
import Dashboard from './components/Dashboard';

function App() {
  return (
    <div className="app">
      {/* Wrap each section with its own ErrorBoundary */}
      {/* This way, one section failing won't break the whole app */}
      <ErrorBoundary>
        <header>
          <h1>My React App</h1>
        </header>
      </ErrorBoundary>

      <main>
        <ErrorBoundary>
          <Dashboard />
        </ErrorBoundary>

        <ErrorBoundary
          fallback={(error, reset) => (
            <div>
              <p>Failed to load users</p>
              <button onClick={reset}>Retry</button>
            </div>
          )}
        >
          <UserList />
        </ErrorBoundary>
      </main>

      <ErrorBoundary>
        <footer>
          <p>&copy; 2024 My App</p>
        </footer>
      </ErrorBoundary>
    </div>
  );
}

export default App;
```

### Global Error Handling
Set up global error handlers to catch unhandled promise rejections and JavaScript errors that occur outside React's render cycle.

```jsx
// File: src/utils/errorHandling.js

/**
 * Global error handlers for catching errors outside React's scope
 * Call these in your main App component or index.js
 */

/**
 * Set up global error listeners
 * Should be called once at app initialization
 */
export function setupGlobalErrorHandlers() {
  // Catch unhandled promise rejections (async errors)
  window.addEventListener('unhandledrejection', (event) => {
    console.error('Unhandled Promise Rejection:', event.reason);
    
    // Report to error tracking service
    // Sentry.captureException(event.reason);
    
    // Prevent default browser handling (which shows console warning)
    event.preventDefault();
  });

  // Catch JavaScript errors that aren't caught by React
  window.addEventListener('error', (event) => {
    console.error('Global Error:', event.error);
    
    // Report to error tracking service
    // Sentry.captureException(event.error);
  });
}

/**
 * Custom error class for API errors with additional context
 */
export class ApiError extends Error {
  constructor(message, status, data) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.data = data;
    // Maintains proper stack trace in V8 environments
    if (Error.captureStackTrace) {
      Error.captureStackTrace(this, ApiError);
    }
  }
}

/**
 * Check if error is a network error (no response received)
 */
export function isNetworkError(error) {
  return (
    error.name === 'TypeError' &&
    error.message.includes('Failed to fetch')
  );
}

/**
 * Check if error is a 401 Unauthorized
 */
export function isAuthError(error) {
  return error.response?.status === 401;
}

/**
 * Get user-friendly error message
 */
export function getErrorMessage(error) {
  // Handle network errors
  if (isNetworkError(error)) {
    return 'Network error. Please check your internet connection.';
  }

  // Handle API errors with response
  if (error.response) {
    const status = error.response.status;
    
    switch (status) {
      case 400:
        return error.response.data?.message || 'Invalid request.';
      case 401:
        return 'Please log in to continue.';
      case 403:
        return 'You do not have permission to perform this action.';
      case 404:
        return 'The requested resource was not found.';
      case 500:
        return 'Server error. Please try again later.';
      default:
        return error.response.data?.message || 'An error occurred.';
    }
  }

  // Handle other errors
  return error.message || 'An unexpected error occurred.';
}
```

## Common Mistakes

### Mistake 1: Swallowing Errors Silently
Never catch errors without handling them or logging them. Silent failures make debugging extremely difficult.

```jsx
// ❌ WRONG — Silently catching and ignoring errors
useEffect(() => {
  fetch('/api/data')
    .then(r => r.json())
    .then(setData)
    .catch(() => {}); // Empty catch - errors are lost!
}, []);

// ✅ CORRECT — Handle or log errors appropriately
useEffect(() => {
  fetch('/api/data')
    .then(r => r.json())
    .then(setData)
    .catch(err => {
      console.error('Failed to fetch data:', err);
      setError('Failed to load data. Please try again.');
    });
}, []);
```

### Mistake 2: Not Handling the Loading State
Always show loading states to prevent the "flash of undefined content" and give users feedback.

```jsx
// ❌ WRONG — No loading state, shows nothing or crashes
function UserList() {
  const [users, setUsers] = useState([]);
  
  useEffect(() => {
    fetch('/api/users').then(r => r.json()).then(setUsers);
  }, []);

  // If users is [] initially and data hasn't loaded yet
  return <div>{users.map(u => u.name)}</div>; // Shows nothing until loaded
}

// ✅ CORRECT — Show loading indicator
function UserList() {
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetch('/api/users')
      .then(r => r.json())
      .then(setUsers)
      .finally(() => setIsLoading(false));
  }, []);

  if (isLoading) return <LoadingSpinner />;

  return <div>{users.map(u => u.name)}</div>;
}
```

### Mistake 3: Not Differentiating Error Types
Show different messages for different error types. Users need to know if it's a network issue vs. a server issue vs. a permission issue.

```jsx
// ❌ WRONG — Generic error message for all errors
function UserList() {
  const [error, setError] = useState(null);

  // ...

  if (error) {
    return <div>Error occurred</div>; // Not helpful!
  }
}

// ✅ CORRECT — Specific error messages
function UserList() {
  const [error, setError] = useState(null);

  // ...

  if (error) {
    // Check error type and show appropriate message
    const message = 
      error.isNetworkError 
        ? 'No internet connection. Please check your network.'
        : error.status === 401
        ? 'Session expired. Please log in again.'
        : error.message;

    return <div className="error">{message}</div>;
  }
}
```

## Real-World Example
Building a comprehensive error handling system for a production React app.

```jsx
// File: src/context/ErrorContext.jsx

import React, { createContext, useContext, useState, useCallback } from 'react';

// Create context for global error management
const ErrorContext = createContext(null);

/**
 * Error Provider - manages global error state
 * Allows any component to set or clear errors
 */
export function ErrorProvider({ children }) {
  const [error, setError] = useState(null);
  const [errorInfo, setErrorInfo] = useState(null);

  // Clear error (can be called from anywhere)
  const clearError = useCallback(() => {
    setError(null);
    setErrorInfo(null);
  }, []);

  // Set error with context
  const setAppError = useCallback((error, info = null) => {
    setError(error);
    setErrorInfo(info);
    
    // Optional: Auto-clear after timeout
    // setTimeout(clearError, 5000);
  }, [clearError]);

  return (
    <ErrorContext.Provider value={{ error, errorInfo, setAppError, clearError }}>
      {children}
    </ErrorContext.Provider>
  );
}

/**
 * Hook to access error context
 */
export function useError() {
  const context = useContext(ErrorContext);
  if (!context) {
    throw new Error('useError must be used within ErrorProvider');
  }
  return context;
}

// File: src/components/GlobalErrorHandler.jsx

import React from 'react';
import { ErrorProvider, useError } from '../context/ErrorContext';

/**
 * Global error handler component
 * Shows errors from context at the top of the app
 */
function GlobalErrorHandler() {
  const { error, clearError } = useError();

  if (!error) return null;

  return (
    <div className="global-error" role="alert">
      <div className="error-content">
        <span className="error-icon">⚠️</span>
        <p>{error.message}</p>
      </div>
      <button onClick={clearError} className="dismiss-button">
        ✕
      </button>
    </div>
  );
}

// File: src/App.jsx - Putting it all together

import React, { useEffect } from 'react';
import { ErrorProvider, useError } from './context/ErrorContext';
import GlobalErrorHandler from './components/GlobalErrorHandler';
import { setupGlobalErrorHandlers } from './utils/errorHandling';
import Dashboard from './components/Dashboard';

function AppContent() {
  const { setAppError } = useError();

  // Set up global error handlers on mount
  useEffect(() => {
    // Only set up once
    let called = false;
    
    if (!called) {
      called = true;
      setupGlobalErrorHandlers();
    }
  }, []);

  // Example: Using context to set errors from anywhere
  const handleApiError = (error) => {
    setAppError({
      message: error.message,
      type: 'api',
      timestamp: Date.now(),
    });
  };

  return (
    <div className="app">
      <GlobalErrorHandler />
      <Dashboard onError={handleApiError} />
    </div>
  );
}

function App() {
  return (
    <ErrorProvider>
      <AppContent />
    </ErrorProvider>
  );
}

export default App;
```

## Key Takeaways
- Always handle three states: loading, error, and success in your data fetching components
- Create custom hooks to encapsulate error handling logic and avoid repetition
- Use Error Boundaries to catch JavaScript errors in the component tree
- Differentiate error types (network, auth, server) to show appropriate messages
- Never silently catch errors without logging or displaying them
- Set up global error handlers for unhandled promise rejections and window errors
- Consider using error tracking services like Sentry for production apps

## What's Next
Continue to [TanStack Query Setup](../02-react-query/01-tanstack-query-setup.md) to learn about a powerful data fetching library that handles caching, refetching, and error handling automatically.
