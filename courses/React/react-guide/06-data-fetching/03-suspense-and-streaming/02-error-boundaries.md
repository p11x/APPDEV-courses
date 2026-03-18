# Error Boundaries in React

## Overview
Error boundaries are React components that catch JavaScript errors anywhere in their child component tree, log those errors, and display a fallback UI instead of crashing the entire application. They are React's way of handling errors that occur during rendering, in lifecycle methods, and in constructors of the whole tree below them. This guide covers implementing error boundaries, their limitations, and best practices.

## Prerequisites
- Understanding of React class components (required for error boundaries)
- Knowledge of React component lifecycle
- Familiarity with React props and state
- Understanding of React's component tree

## Core Concepts

### Creating an Error Boundary
Error boundaries must be class components because they use lifecycle methods like componentDidCatch. This is one of the few places in modern React where class components are still necessary.

```jsx
// File: src/components/ErrorBoundary.jsx

import React, { Component } from 'react';

/**
 * ErrorBoundary is a class component that catches JavaScript errors
 * in its child component tree and displays a fallback UI
 */
class ErrorBoundary extends Component {
  // Initialize state to track if an error occurred
  // hasError starts as false because no error has happened yet
  constructor(props) {
    super(props);
    this.state = {
      hasError: false,
      error: null,
      errorInfo: null,
    };
  }

  /**
   * Static method called when a child component throws an error
   * Must return a new state object to trigger re-render with fallback UI
   * This is React's way of catching errors without try-catch blocks
   */
  static getDerivedStateFromError(error) {
    // Return state indicating an error occurred
    return {
      hasError: true,
      // Could also store error.message or error.toString() here
    };
  }

  /**
   * Lifecycle method called after an error has been caught
   * Useful for logging errors to external services
   * @param {Error} error - The error that was thrown
   * @param {object} errorInfo - Object containing component stack trace
   */
  componentDidCatch(error, errorInfo) {
    // Log error to console for development
    console.error('ErrorBoundary caught an error:', error, errorInfo);
    
    // Store error info for potential display
    this.setState({
      error,
      errorInfo,
    });

    // Example: Send to error reporting service (like Sentry)
    // Sentry.captureException(error, {
    //   extra: {
    //     componentStack: errorInfo.componentStack,
    //   },
    // });
    
    // Example: Send to analytics
    // analytics.track('error', {
    //   message: error.message,
    //   stack: error.stack,
    //   componentStack: errorInfo.componentStack,
    // });
  }

  /**
   * Method to reset the error state and try rendering again
   * Useful for retry buttons or recovering from errors
   */
  handleReset = () => {
    this.setState({
      hasError: false,
      error: null,
      errorInfo: null,
    });
    
    // Optionally call a reset callback from props
    this.props.onReset?.();
  };

  /**
   * Main render method - shows fallback UI if error occurred,
   * otherwise renders children normally
   */
  render() {
    // If an error occurred, show fallback UI
    if (this.state.hasError) {
      // Allow custom fallback component to be provided
      if (this.props.fallback) {
        return this.props.fallback(
          this.state.error,
          this.state.errorInfo,
          this.handleReset
        );
      }

      // Default fallback UI
      return (
        <div className="error-boundary">
          <h2>Something went wrong</h2>
          <p>We're sorry, but an unexpected error occurred.</p>
          
          {/* Show error details in development only */}
          {process.env.NODE_ENV === 'development' && (
            <details style={{ marginTop: '1rem', textAlign: 'left' }}>
              <summary>Error Details (Development Only)</summary>
              <pre style={{ 
                padding: '1rem', 
                backgroundColor: '#f5f5f5',
                overflow: 'auto',
                maxHeight: '300px'
              }}>
                {this.state.error?.toString()}
                <br />
                {this.state.errorInfo?.componentStack}
              </pre>
            </details>
          )}
          
          <button 
            onClick={this.handleReset}
            style={{ marginTop: '1rem' }}
          >
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
```

### Using Error Boundaries in Your App
Error boundaries can be placed at different levels of your component tree depending on how granular you want error handling to be.

```jsx
// File: src/App.jsx

import React from 'react';
import ErrorBoundary from './components/ErrorBoundary';
import Header from './components/Header';
import Sidebar from './components/Sidebar';
import MainContent from './components/MainContent';
import Footer from './components/Footer';

/**
 * App-level error boundary - if ANYTHING fails, show error
 * Use this sparingly as it provides poor UX when one small thing fails
 */
function AppWithGlobalBoundary() {
  return (
    <ErrorBoundary>
      <div className="app">
        <Header />
        <Sidebar />
        <MainContent />
        <Footer />
      </div>
    </ErrorBoundary>
  );
}

/**
 * Section-level error boundaries - each section handles its own errors
 * This is better UX - one section failing doesn't break the whole app
 */
function AppWithSectionBoundaries() {
  return (
    <div className="app">
      <ErrorBoundary>
        <header className="app-header">
          <Header />
        </header>
      </ErrorBoundary>

      <div className="app-body">
        <ErrorBoundary>
          <aside className="app-sidebar">
            <Sidebar />
          </aside>
        </ErrorBoundary>

        <ErrorBoundary>
          <main className="app-main">
            <MainContent />
          </main>
        </ErrorBoundary>
      </div>

      <ErrorBoundary>
        <footer className="app-footer">
          <Footer />
        </footer>
      </ErrorBoundary>
    </div>
  );
}

/**
 * Granular error boundaries - for specific error-prone components
 */
function AppWithGranularBoundaries() {
  return (
    <div className="app">
      <ErrorBoundary>
        <Header />
      </ErrorBoundary>

      <div className="content">
        {/* Widget might fail independently */}
        <ErrorBoundary
          fallback={(error, reset) => (
            <div className="widget-error">
              <p>Failed to load widget</p>
              <button onClick={reset}>Retry</button>
            </div>
          )}
        >
          <Widget />
        </ErrorBoundary>

        <ErrorBoundary>
          <MainContent />
        </ErrorBoundary>
      </div>
    </div>
  );
}

export default AppWithSectionBoundaries;
```

### Error Boundaries with Custom Fallbacks
You can pass custom fallback components to error boundaries for more flexible error handling.

```jsx
// File: src/components/ErrorFallbacks.jsx

import React, { Component } from 'react';

// Custom fallback for data fetching errors
class DataErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    // Check if this is a data fetching error
    if (error.message.includes('Failed to fetch')) {
      console.error('Data fetch error:', error);
    }
  }

  render() {
    if (this.state.hasError) {
      return this.props.fallback || (
        <div className="data-error">
          <h3>Failed to load data</h3>
          <p>Please check your internet connection.</p>
          <button onClick={() => this.setState({ hasError: false })}>
            Retry
          </button>
        </div>
      );
    }
    return this.props.children;
  }
}

// Custom fallback for authorization errors
class AuthErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    // Check for 401 errors
    if (error.response?.status === 401) {
      // Redirect to login or show auth modal
      this.props.onAuthError?.();
    }
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className="auth-error">
          <h3>Session Expired</h3>
          <p>Please log in again to continue.</p>
          <button onClick={() => {
            this.setState({ hasError: false });
            window.location.href = '/login';
          }}>
            Log In
          </button>
        </div>
      );
    }
    return this.props.children;
  }
}

// Reusable error card component
function ErrorCard({ error, onRetry, onDismiss }) {
  return (
    <div className="error-card">
      <div className="error-icon">⚠️</div>
      <h3>Oops! Something went wrong</h3>
      <p className="error-message">
        {error?.message || 'An unexpected error occurred'}
      </p>
      <div className="error-actions">
        {onRetry && (
          <button onClick={onRetry} className="btn-retry">
            Try Again
          </button>
        )}
        {onDismiss && (
          <button onClick={onDismiss} className="btn-dismiss">
            Dismiss
          </button>
        )}
      </div>
    </div>
  );
}

// Usage example
function ExampleUsage() {
  return (
    <ErrorBoundary
      fallback={(error, reset) => (
        <ErrorCard
          error={error}
          onRetry={reset}
        />
      )}
    >
      <MyComponent />
    </ErrorBoundary>
  );
}

export { ErrorBoundary, DataErrorBoundary, AuthErrorBoundary, ErrorCard };
```

### Handling Async Errors
Error boundaries catch errors during rendering, but async errors (like in useEffect) need different handling.

```jsx
// File: src/components/AsyncErrorHandling.jsx

import React, { useState, useEffect } from 'react';
import ErrorBoundary from './ErrorBoundary';

/**
 * For errors in useEffect, use try-catch inside the effect
 * Error boundaries don't catch these automatically
 */
function ComponentWithAsyncEffect() {
  const [data, setData] = useState(null);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch('/api/data');
        if (!response.ok) throw new Error('Failed to fetch');
        const json = await response.json();
        setData(json);
      } catch (err) {
        setError(err);
      }
    };

    fetchData();
  }, []);

  if (error) {
    return (
      <div className="error">
        <p>Error: {error.message}</p>
        <button onClick={() => window.location.reload()}>Retry</button>
      </div>
    );
  }

  return <div>{data}</div>;
}

/**
 * Alternative: Use a wrapper that catches Promise rejections
 */
function AsyncErrorBoundary({ children }) {
  const [error, setError] = useState(null);

  useEffect(() => {
    // Catch any unhandled promise rejections in this component's tree
    const handleReject = (event) => {
      setError(event.reason);
    };

    window.addEventListener('unhandledrejection', handleReject);

    return () => {
      window.removeEventListener('unhandledrejection', handleReject);
    };
  }, []);

  if (error) {
    return (
      <div>
        <p>Async error: {error.message}</p>
        <button onClick={() => setError(null)}>Dismiss</button>
      </div>
    );
  }

  return children;
}

/**
 * Using both approaches together for comprehensive error handling
 */
function WellHandledComponent() {
  return (
    <ErrorBoundary>
      <AsyncErrorBoundary>
        <ComponentWithAsyncEffect />
      </AsyncErrorBoundary>
    </ErrorBoundary>
  );
}

export default WellHandledComponent;
```

## Common Mistakes

### Mistake 1: Using Error Boundaries with Hooks
Error boundaries must be class components - they cannot be functional components with hooks.

```jsx
// ❌ WRONG — Functional component cannot be error boundary
function ErrorBoundary({ children }) {
  const [hasError, setHasError] = useState(false);
  
  // This doesn't work! Error boundaries need lifecycle methods
  // No way to catch errors in children

  return hasError ? <Fallback /> : children;
}

// ✅ CORRECT — Must use class component
class ErrorBoundary extends Component {
  static getDerivedStateFromError(error) {
    return { hasError: true };
  }
  
  componentDidCatch(error, errorInfo) {
    console.error(error, errorInfo);
  }
  
  render() {
    if (this.state.hasError) {
      return <Fallback />;
    }
    return this.props.children;
  }
}
```

### Mistake 2: Error Boundaries Don't Catch Event Handler Errors
Error boundaries only catch errors in rendering and lifecycle methods, not in event handlers.

```jsx
// ❌ WRONG — Error boundary won't catch this
function ProblematicComponent() {
  const handleClick = () => {
    // This error won't be caught by error boundary!
    throw new Error('Click error');
  };
  
  return <button onClick={handleClick}>Click me</button>;
}

// ✅ CORRECT — Use try-catch in event handlers
function FixedComponent() {
  const handleClick = () => {
    try {
      throw new Error('Click error');
    } catch (err) {
      // Handle or re-throw
      console.error(err);
    }
  };
  
  return <button onClick={handleClick}>Click me</button>;
}
```

### Mistake 3: Not Wrapping Error-Prone Components
Place error boundaries strategically around components that might fail.

```jsx
// ❌ WRONG — Only one error boundary at the top
function App() {
  return (
    <ErrorBoundary>
      <Navigation /> {/* If this fails... */}
      <Content />     {/* ...this also fails! */}
    </ErrorBoundary>
  );
}

// ✅ CORRECT — Independent error boundaries
function App() {
  return (
    <ErrorBoundary>
      <Navigation />
    </ErrorBoundary>
    <ErrorBoundary>
      <Content />
    </ErrorBoundary>
  );
}
```

## Real-World Example
Building a comprehensive error handling system with multiple boundaries and recovery options.

```jsx
// File: src/components/ComprehensiveErrorBoundary.jsx

import React, { Component } from 'react';

// Main error boundary with full features
class ComprehensiveErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = {
      hasError: false,
      error: null,
      errorInfo: null,
      errorCount: 0,
    };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    this.setState((prev) => ({
      errorInfo,
      errorCount: prev.errorCount + 1,
    }));

    // Log to error tracking service
    this.logError(error, errorInfo);

    // Call custom error handler if provided
    this.props.onError?.(error, errorInfo);
  }

  logError = (error, errorInfo) => {
    const timestamp = new Date().toISOString();
    const errorData = {
      message: error.message,
      stack: error.stack,
      componentStack: errorInfo.componentStack,
      timestamp,
      userId: this.getCurrentUserId(), // If available
      url: window.location.href,
    };

    console.error('Error logged:', errorData);

    // Example: Send to Sentry
    // if (window.Sentry) {
    //   window.Sentry.captureException(error, { extra: errorData });
    // }
  };

  getCurrentUserId = () => {
    // Get current user ID if available
    try {
      return JSON.parse(localStorage.getItem('user'))?.id;
    } catch {
      return null;
    }
  };

  handleReset = () => {
    this.setState({
      hasError: false,
      error: null,
      errorInfo: null,
    });
    
    // Call reset callback
    this.props.onReset?.();
  };

  handleReload = () => {
    window.location.reload();
  };

  render() {
    if (!this.state.hasError) {
      return this.props.children;
    }

    const { fallback, showDetails } = this.props;
    const { error, errorInfo, errorCount } = this.state;

    // Use custom fallback if provided
    if (fallback) {
      return fallback(error, errorInfo, this.handleReset);
    }

    // Default comprehensive error UI
    return (
      <div style={styles.container}>
        <div style={styles.card}>
          <div style={styles.icon}>⚠️</div>
          <h2 style={styles.title}>Something went wrong</h2>
          
          <p style={styles.message}>
            We encountered an unexpected error. Please try again.
          </p>

          {/* Show error count if multiple errors */}
          {errorCount > 1 && (
            <p style={styles.count}>
              This error has occurred {errorCount} time(s)
            </p>
          )}

          {/* Actions */}
          <div style={styles.actions}>
            <button 
              onClick={this.handleReset}
              style={styles.primaryButton}
            >
              Try Again
            </button>
            <button 
              onClick={this.handleReload}
              style={styles.secondaryButton}
            >
              Reload Page
            </button>
          </div>

          {/* Development details */}
          {showDetails && process.env.NODE_ENV === 'development' && (
            <details style={styles.details}>
              <summary>Error Details</summary>
              <div style={styles.errorContent}>
                <strong>Error:</strong> {error?.toString()}
                <br /><br />
                <strong>Component Stack:</strong>
                <pre style={styles.pre}>
                  {errorInfo?.componentStack}
                </pre>
              </div>
            </details>
          )}
        </div>
      </div>
    );
  }
}

// Styles
const styles = {
  container: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    minHeight: '200px',
    padding: '2rem',
  },
  card: {
    maxWidth: '500px',
    padding: '2rem',
    borderRadius: '8px',
    boxShadow: '0 2px 8px rgba(0,0,0,0.1)',
    textAlign: 'center',
    backgroundColor: 'white',
  },
  icon: {
    fontSize: '3rem',
    marginBottom: '1rem',
  },
  title: {
    marginBottom: '1rem',
    color: '#333',
  },
  message: {
    color: '#666',
    marginBottom: '1rem',
  },
  count: {
    color: '#999',
    fontSize: '0.875rem',
    marginBottom: '1rem',
  },
  actions: {
    display: 'flex',
    gap: '1rem',
    justifyContent: 'center',
    marginBottom: '1rem',
  },
  primaryButton: {
    padding: '0.5rem 1.5rem',
    backgroundColor: '#007bff',
    color: 'white',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
  },
  secondaryButton: {
    padding: '0.5rem 1.5rem',
    backgroundColor: '#6c757d',
    color: 'white',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
  },
  details: {
    marginTop: '1rem',
    textAlign: 'left',
  },
  errorContent: {
    padding: '1rem',
    backgroundColor: '#f8f9fa',
    borderRadius: '4px',
    marginTop: '0.5rem',
  },
  pre: {
    overflow: 'auto',
    fontSize: '0.75rem',
    maxHeight: '200px',
  },
};

// Export for use in App
export { ComprehensiveErrorBoundary };

// File: src/App.jsx - Using comprehensive error boundary

import React from 'react';
import { ComprehensiveErrorBoundary } from './components/ComprehensiveErrorBoundary';

function App() {
  const handleError = (error, errorInfo) => {
    // Custom error handling
    console.error('App-level error handler:', error);
  };

  return (
    <ComprehensiveErrorBoundary
      onError={handleError}
      showDetails={true}
      fallback={(error, reset) => (
        <div style={{ padding: '2rem', textAlign: 'center' }}>
          <h2>Custom Error UI</h2>
          <p>{error.message}</p>
          <button onClick={reset}>Try Again</button>
        </div>
      )}
    >
      <YourAppContent />
    </ComprehensiveErrorBoundary>
  );
}

export default App;
```

## Key Takeaways
- Error boundaries are class components that catch JavaScript errors in child trees
- Use static getDerivedStateFromError to update state when errors occur
- Use componentDidCatch for logging errors to external services
- Error boundaries don't catch errors in event handlers, async code, or SSR
- Place error boundaries strategically - not too high (breaks everything) or too low (repetitive)
- Always provide fallback UIs to give users meaningful feedback
- Combine error boundaries with try-catch for event handlers and async code

## What's Next
Continue to [Loading States Best Practices](03-loading-states-best-practices.md) to learn how to create effective and accessible loading states throughout your React application.
