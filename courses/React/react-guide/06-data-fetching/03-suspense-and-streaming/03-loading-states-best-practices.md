# Loading States Best Practices

## Overview
Loading states are critical for user experience in React applications. They provide feedback during async operations, prevent user confusion, and make your app feel responsive. This guide covers best practices for creating effective, accessible, and performant loading states that enhance rather than hinder the user experience.

## Prerequisites
- Understanding of React hooks (useState, useEffect)
- Knowledge of data fetching patterns
- Familiarity with CSS for styling
- Understanding of accessibility (a11y) basics

## Core Concepts

### Basic Loading State Patterns
Every async operation in React should show a loading state. The key is to handle the three main states: loading, success, and error.

```jsx
// File: src/components/LoadingPatterns.jsx

import React, { useState, useEffect } from 'react';

function UserList() {
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        setIsLoading(true);
        setError(null);
        
        const response = await fetch(
          'https://jsonplaceholder.typicode.com/users'
        );
        
        if (!response.ok) {
          throw new Error('Failed to fetch users');
        }
        
        const data = await response.json();
        setUsers(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setIsLoading(false);
      }
    };

    fetchUsers();
  }, []);

  // State 1: Loading
  if (isLoading) {
    return (
      <div className="loading-container">
        <div className="spinner"></div>
        <p>Loading users...</p>
      </div>
    );
  }

  // State 2: Error
  if (error) {
    return (
      <div className="error-container">
        <p>Error: {error}</p>
        <button onClick={() => window.location.reload()}>
          Try Again
        </button>
      </div>
    );
  }

  // State 3: Success
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

### Skeleton Loading States
Skeleton loaders provide a better user experience than spinners by showing the approximate shape of upcoming content. This reduces perceived wait time and prevents layout shift.

```jsx
// File: src/components/SkeletonLoaders.jsx

import React from 'react';

// Generic skeleton component
function Skeleton({ width, height, borderRadius = '4px', style = {} }) {
  return (
    <div
      style={{
        width,
        height,
        borderRadius,
        backgroundColor: '#e0e0e0',
        animation: 'pulse 1.5s ease-in-out infinite',
        ...style,
      }}
    />
  );
}

// Skeleton for a user card
function UserCardSkeleton() {
  return (
    <div className="user-card-skeleton" style={styles.card}>
      <Skeleton width="60px" height="60px" borderRadius="50%" />
      <div style={styles.content}>
        <Skeleton width="150px" height="20px" />
        <Skeleton width="100px" height="16px" style={{ marginTop: '8px' }} />
      </div>
    </div>
  );
}

// Skeleton for a list of items
function UserListSkeleton({ count = 3 }) {
  return (
    <div className="user-list-skeleton">
      {Array.from({ length: count }).map((_, index) => (
        <UserCardSkeleton key={index} />
      ))}
    </div>
  );
}

// Skeleton for a blog post
function PostSkeleton() {
  return (
    <article style={styles.post}>
      <Skeleton width="100%" height="30px" />
      <div style={{ marginTop: '16px' }}>
        <Skeleton width="100%" height="16px" />
        <Skeleton width="90%" height="16px" style={{ marginTop: '8px' }} />
        <Skeleton width="75%" height="16px" style={{ marginTop: '8px' }} />
      </div>
      <div style={{ marginTop: '16px', display: 'flex', gap: '8px' }}>
        <Skeleton width="80px" height="30px" />
        <Skeleton width="80px" height="30px" />
      </div>
    </article>
  );
}

// Skeleton for a table
function TableSkeleton({ rows = 5, columns = 4 }) {
  return (
    <div style={styles.table}>
      <div style={styles.tableHeader}>
        {Array.from({ length: columns }).map((_, i) => (
          <Skeleton key={i} width="100px" height="20px" />
        ))}
      </div>
      {Array.from({ length: rows }).map((_, rowIndex) => (
        <div key={rowIndex} style={styles.tableRow}>
          {Array.from({ length: columns }).map((_, colIndex) => (
            <Skeleton key={colIndex} width="100px" height="40px" />
          ))}
        </div>
      ))}
    </div>
  );
}

const styles = {
  card: {
    display: 'flex',
    alignItems: 'center',
    gap: '16px',
    padding: '16px',
    border: '1px solid #e0e0e0',
    borderRadius: '8px',
    marginBottom: '12px',
  },
  content: {
    flex: 1,
  },
  post: {
    padding: '20px',
    border: '1px solid #e0e0e0',
    borderRadius: '8px',
  },
  table: {
    border: '1px solid #e0e0e0',
    borderRadius: '8px',
    overflow: 'hidden',
  },
  tableHeader: {
    display: 'flex',
    gap: '16px',
    padding: '16px',
    backgroundColor: '#f5f5f5',
    borderBottom: '1px solid #e0e0e0',
  },
  tableRow: {
    display: 'flex',
    gap: '16px',
    padding: '16px',
    borderBottom: '1px solid #e0e0e0',
  },
};

export { Skeleton, UserCardSkeleton, UserListSkeleton, PostSkeleton, TableSkeleton };
```

### Progressive Loading
Progressive loading shows content as it becomes available rather than waiting for everything to load. This improves perceived performance.

```jsx
// File: src/components/ProgressiveLoading.jsx

import React, { useState, useEffect } from 'react';

function ProgressivePostList() {
  const [posts, setPosts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [hasMore, setHasMore] = useState(true);
  const [page, setPage] = useState(1);

  const loadMore = async () => {
    setIsLoading(true);
    try {
      const response = await fetch(
        `https://jsonplaceholder.typicode.com/posts?_page=${page}&_limit=10`
      );
      const newPosts = await response.json();
      
      setPosts(prev => [...prev, ...newPosts]);
      setHasMore(newPosts.length === 10);
      setPage(prev => prev + 1);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    loadMore();
  }, []);

  return (
    <div>
      <h2>Posts</h2>
      
      {/* Show existing posts immediately */}
      <ul>
        {posts.map(post => (
          <li key={post.id}>{post.title}</li>
        ))}
      </ul>
      
      {/* Show loading indicator only for additional content */}
      {isLoading && <PostSkeleton />}
      
      {/* Load more button */}
      {hasMore && !isLoading && (
        <button onClick={loadMore}>
          Load More
        </button>
      )}
      
      {!hasMore && <p>No more posts</p>}
    </div>
  );
}

// Alternative with intersection observer for auto-loading
function InfinitePostList() {
  const [posts, setPosts] = useState([]);
  const [page, setPage] = useState(1);
  const [isLoading, setIsLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);
  const loaderRef = React.useRef(null);

  const loadMore = async () => {
    if (isLoading || !hasMore) return;
    
    setIsLoading(true);
    try {
      const response = await fetch(
        `https://jsonplaceholder.typicode.com/posts?_page=${page}&_limit=10`
      );
      const newPosts = await response.json();
      
      setPosts(prev => [...prev, ...newPosts]);
      setHasMore(newPosts.length === 10);
      setPage(prev => prev + 1);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    const observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting) {
        loadMore();
      }
    });

    if (loaderRef.current) {
      observer.observe(loaderRef.current);
    }

    return () => observer.disconnect();
  }, [hasMore, isLoading]);

  return (
    <div>
      <ul>
        {posts.map(post => (
          <li key={post.id}>{post.title}</li>
        ))}
      </ul>
      
      <div ref={loaderRef}>
        {isLoading && <PostSkeleton />}
        {!hasMore && <p>End of list</p>}
      </div>
    </div>
  );
}

export { ProgressivePostList, InfinitePostList };
```

### Inline Loading States
For quick operations, inline loading states provide feedback without disrupting the UI.

```jsx
// File: src/components/InlineLoading.jsx

import React, { useState } from 'react';

function InlineLoadingButton({ children, onClick, isLoading, disabled }) {
  return (
    <button
      onClick={onClick}
      disabled={isLoading || disabled}
      style={{
        opacity: isLoading ? 0.7 : 1,
        cursor: isLoading || disabled ? 'not-allowed' : 'pointer',
      }}
    >
      {isLoading ? (
        <>
          <span className="spinner-small"></span>
          Loading...
        </>
      ) : (
        children
      )}
    </button>
  );
}

function InlineLoadingInput() {
  const [value, setValue] = useState('');
  const [isChecking, setIsChecking] = useState(false);
  const [isAvailable, setIsAvailable] = useState(null);

  const checkAvailability = async () => {
    setIsChecking(true);
    setIsAvailable(null);
    
    // Simulate API call
    await new Promise(r => setTimeout(r, 1000));
    
    // Random result for demo
    setIsAvailable(Math.random() > 0.5);
    setIsChecking(false);
  };

  return (
    <div>
      <input
        value={value}
        onChange={(e) => setValue(e.target.value)}
        placeholder="Enter username"
      />
      
      {isChecking && <span className="spinner-small"></span>}
      
      {!isChecking && isAvailable !== null && (
        <span style={{ color: isAvailable ? 'green' : 'red' }}>
          {isAvailable ? 'Available!' : 'Already taken'}
        </span>
      )}
      
      <button 
        onClick={checkAvailability}
        disabled={isChecking || !value}
      >
        Check Availability
      </button>
    </div>
  );
}

function SubmitButtonWithLoading() {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    
    // Simulate submission
    await new Promise(r => setTimeout(r, 2000));
    
    setIsSubmitting(false);
    setIsSuccess(true);
  };

  return (
    <form onSubmit={handleSubmit}>
      <button
        type="submit"
        disabled={isSubmitting}
        className={isSubmitting ? 'loading' : ''}
      >
        {isSubmitting ? (
          <>
            <span className="spinner"></span>
            Saving...
          </>
        ) : isSuccess ? (
          'Saved!'
        ) : (
          'Save'
        )}
      </button>
    </form>
  );
}

export { InlineLoadingButton, InlineLoadingInput, SubmitButtonWithLoading };
```

### Accessible Loading States
Loading states must be accessible to users who rely on assistive technologies. This includes proper ARIA attributes and screen reader announcements.

```jsx
// File: src/components/AccessibleLoading.jsx

import React from 'react';

function AccessibleLoadingSpinner() {
  return (
    <div
      role="status"
      aria-live="polite"
      aria-busy="true"
      className="loading-container"
    >
      <div className="spinner" aria-hidden="true"></div>
      {/* Screen reader only text */}
      <span className="sr-only">Loading...</span>
    </div>
  );
}

function AccessibleDataLoading() {
  const [isLoading, setIsLoading] = React.useState(true);
  const [data, setData] = React.useState(null);

  React.useEffect(() => {
    fetch('https://jsonplaceholder.typicode.com/users/1')
      .then(r => r.json())
      .then(user => {
        setData(user);
        setIsLoading(false);
      });
  }, []);

  if (isLoading) {
    return (
      <div role="status" aria-live="polite">
        <p>Loading user data...</p>
        <AccessibleLoadingSpinner />
      </div>
    );
  }

  return (
    <div>
      <p>User loaded: {data.name}</p>
    </div>
  );
}

function AccessibleProgress() {
  const [progress, setProgress] = React.useState(0);

  React.useEffect(() => {
    const interval = setInterval(() => {
      setProgress(prev => {
        if (prev >= 100) {
          clearInterval(interval);
          return 100;
        }
        return prev + 10;
      });
    }, 200);

    return () => clearInterval(interval);
  }, []);

  return (
    <div role="progressbar"
      aria-valuenow={progress}
      aria-valuemin="0"
      aria-valuemax="100"
      aria-label="Upload progress"
    >
      <div style={{
        width: `${progress}%`,
        height: '8px',
        backgroundColor: '#007bff',
        transition: 'width 0.2s',
      }} />
      <p>{progress}% complete</p>
    </div>
  );
}

export { 
  AccessibleLoadingSpinner, 
  AccessibleDataLoading, 
  AccessibleProgress 
};
```

## Common Mistakes

### Mistake 1: No Loading State
Never leave users wondering what's happening. Always provide feedback during async operations.

```jsx
// ❌ WRONG — No feedback during loading
function BadUserList() {
  const [users, setUsers] = useState([]);
  
  useEffect(() => {
    fetch('/api/users').then(r => r.json()).then(setUsers);
  }, []);
  
  return <ul>{users.map(u => <li>{u.name}</li>)}</ul>;
}

// ✅ CORRECT — Loading feedback
function GoodUserList() {
  const [users, setUsers] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    fetch('/api/users')
      .then(r => r.json())
      .then(setUsers)
      .finally(() => setIsLoading(false));
  }, []);

  if (isLoading) return <LoadingSpinner />;

  return <ul>{users.map(u => <li>{u.name}</li>)}</ul>;
}
```

### Mistake 2: Blocking UI with Loading
Show loading states that allow users to interact with other parts of the app.

```jsx
// ❌ WRONG — Full page loader for everything
function BadPage() {
  const [isLoading, setIsLoading] = useState(true);
  
  useEffect(() => {
    fetchData().then(() => setIsLoading(false));
  }, []);
  
  if (isLoading) return <FullPageLoader />; // Can't interact with anything!
  
  return <Content />;
}

// ✅ CORRECT — Inline loading for specific content
function GoodPage() {
  const { data, isLoading } = useQuery(['data']);
  
  return (
    <div>
      <Navigation /> {/* Always accessible */}
      <main>
        {isLoading ? <Skeleton /> : <Content data={data} />}
      </main>
    </div>
  );
}
```

### Mistake 3: Not Handling Error States
Loading states should always include error handling as the third state.

```jsx// ❌ WRONG — Only handling loading and success
function BadComponent() {
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  
  // If error occurs, user sees nothing useful!
  
  return isLoading ? <Spinner /> : <DataDisplay data={data} />;
}

// ✅ CORRECT — All three states handled
function GoodComponent() {
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  
  if (isLoading) return <Spinner />;
  if (error) return <ErrorMessage error={error} />;
  
  return <DataDisplay data={data} />;
}
```

## Real-World Example
Building a complete loading state system with best practices applied.

```jsx
// File: src/components/CompleteLoadingExample.jsx

import React, { useState, useEffect } from 'react';

// Reusable loading hook
function useDataFetcher(url) {
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchData = async () => {
    setIsLoading(true);
    setError(null);
    
    try {
      const response = await fetch(url);
      if (!response.ok) throw new Error('Failed to fetch');
      const json = await response.json();
      setData(json);
    } catch (err) {
      setError(err.message);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [url]);

  return { data, isLoading, error, refetch: fetchData };
}

// State-aware component wrapper
function DataContainer({ 
  isLoading, 
  error, 
  onRetry,
  loadingFallback = <DefaultSkeleton />,
  errorFallback,
  children, 
}) {
  if (isLoading) {
    return loadingFallback;
  }

  if (error) {
    if (errorFallback) {
      return errorFallback;
    }
    return (
      <div className="error-state">
        <p>Error: {error}</p>
        {onRetry && <button onClick={onRetry}>Try Again</button>}
      </div>
    );
  }

  return children;
}

// Complete example using the patterns
function UserDashboard() {
  const { data: user, isLoading: userLoading, error: userError, refetch: refetchUser } = 
    useDataFetcher('https://jsonplaceholder.typicode.com/users/1');
    
  const { data: posts, isLoading: postsLoading, error: postsError, refetch: refetchPosts } = 
    useDataFetcher('https://jsonplaceholder.typicode.com/posts?userId=1');

  return (
    <div className="dashboard">
      <h1>Dashboard</h1>

      {/* Independent loading states for each section */}
      <section className="user-section">
        <DataContainer
          isLoading={userLoading}
          error={userError}
          onRetry={refetchUser}
          loadingFallback={<UserCardSkeleton />}
        >
          <UserCard user={user} />
        </DataContainer>
      </section>

      <section className="posts-section">
        <DataContainer
          isLoading={postsLoading}
          error={postsError}
          onRetry={refetchPosts}
          loadingFallback={<PostListSkeleton count={5} />}
        >
          <PostList posts={posts} />
        </DataContainer>
      </section>
    </div>
  );
}

// Individual display components
function UserCard({ user }) {
  return (
    <div className="user-card">
      <h2>{user.name}</h2>
      <p>{user.email}</p>
      <p>{user.phone}</p>
    </div>
  );
}

function PostList({ posts }) {
  return (
    <ul className="post-list">
      {posts.map(post => (
        <li key={post.id}>{post.title}</li>
      ))}
    </ul>
  );
}

// Default skeleton
function DefaultSkeleton() {
  return <div className="skeleton" style={{ height: '100px' }} />;
}

// Skeleton components (simplified)
function UserCardSkeleton() {
  return (
    <div className="skeleton-card">
      <div className="skeleton" style={{ height: '20px', width: '150px' }} />
      <div className="skeleton" style={{ height: '16px', width: '200px', marginTop: '8px' }} />
    </div>
  );
}

function PostListSkeleton({ count = 3 }) {
  return (
    <div>
      {Array.from({ length: count }).map((_, i) => (
        <div key={i} className="skeleton" style={{ height: '40px', marginBottom: '8px' }} />
      ))}
    </div>
  );
}

export default UserDashboard;
```

## Key Takeaways
- Always show loading states during async operations - never leave users wondering
- Use skeleton loaders instead of spinners for better perceived performance
- Handle three states: loading, error, and success
- Make loading states accessible with ARIA attributes
- Keep loading states localized to specific components rather than blocking the entire UI
- Show progress for long-running operations when possible
- Use progressive loading to show content as it becomes available

## What's Next
This completes the data fetching section. The concepts learned here apply to all async operations in React. Continue to [Building Controlled Forms](../07-forms/01-controlled-forms/01-building-controlled-forms.md) to learn about form handling in React.
