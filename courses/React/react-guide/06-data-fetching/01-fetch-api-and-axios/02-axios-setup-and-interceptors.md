# Axios Setup and Interceptors

## Overview
While the Fetch API is built into browsers, Axios is a popular third-party library that provides a more powerful and convenient way to make HTTP requests. Axios automatically transforms request and response data, handles JSON automatically, provides better error handling, and supports interceptors for modifying requests before they're sent or responses before they're handled.

## Prerequisites
- Understanding of the Fetch API and REST APIs
- Knowledge of React hooks (useState, useEffect)
- Familiarity with async/await syntax
- Basic understanding of authentication tokens

## Core Concepts

### Installing and Setting Up Axios
Axios is available as an npm package and can be easily integrated into any React project. Unlike Fetch, Axios automatically parses JSON responses and handles HTTP errors more gracefully.

```jsx
// File: src/api/client.js

// Install axios first: npm install axios
// Axios provides a more convenient API than the native Fetch API

import axios from 'axios';

// Create a pre-configured axios instance for your API
// This allows you to set default configuration for all requests
const apiClient = axios.create({
  // Base URL - all requests will be prefixed with this
  baseURL: 'https://api.example.com/v1',
  
  // Default headers applied to every request
  headers: {
    'Content-Type': 'application/json',
  },
  
  // Request timeout in milliseconds (default is 0 = no timeout)
  timeout: 10000,
});

// Request interceptor - runs before every request is sent
// Perfect for adding authentication tokens automatically
apiClient.interceptors.request.use(
  (config) => {
    // Get the auth token from storage or state
    const token = localStorage.getItem('authToken');
    
    // If token exists, add it to the Authorization header
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    
    // Log requests in development for debugging
    if (process.env.NODE_ENV === 'development') {
      console.log(`🚀 ${config.method?.toUpperCase()} ${config.url}`);
    }
    
    // Must return the config for the request to proceed
    return config;
  },
  (error) => {
    // Handle request errors (e.g., network issues before request sends)
    return Promise.reject(error);
  }
);

// Response interceptor - runs after every response is received
// Great for handling common errors like token expiration
apiClient.interceptors.response.use(
  (response) => {
    // Log responses in development
    if (process.env.NODE_ENV === 'development') {
      console.log(`✅ ${response.status} ${response.config.url}`);
    }
    
    // Return the response as-is for successful requests
    return response;
  },
  (error) => {
    // Handle response errors globally
    
    if (error.response) {
      // Server responded with an error status (4xx, 5xx)
      const { status, data } = error.response;
      
      switch (status) {
        case 401:
          // Unauthorized - token might be expired
          // Clear local auth and redirect to login
          localStorage.removeItem('authToken');
          window.location.href = '/login';
          break;
          
        case 403:
          // Forbidden - user doesn't have permission
          console.error('Access denied:', data?.message);
          break;
          
        case 404:
          // Resource not found
          console.error('Resource not found:', error.config.url);
          break;
          
        case 500:
          // Server error
          console.error('Server error:', data?.message);
          break;
          
        default:
          console.error('API Error:', data?.message);
      }
    } else if (error.request) {
      // Request was made but no response received (network error)
      console.error('Network error - no response received');
    } else {
      // Error setting up the request
      console.error('Request setup error:', error.message);
    }
    
    // Re-throw the error so calling code can handle it too
    return Promise.reject(error);
  }
);

export default apiClient;
```

### Using Axios in React Components
Axios integrates smoothly with React hooks. The API is cleaner than Fetch because you don't need to manually check `response.ok` or call `.json()`.

```jsx
// File: src/components/UserProfile.jsx

import React, { useState, useEffect } from 'react';
import apiClient from '../api/client';

function UserProfile({ userId }) {
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        // Axios automatically parses JSON response
        // No need for response.json() call
        // Axios throws on any HTTP error status (4xx, 5xx)
        const response = await apiClient.get(`/users/${userId}`);
        
        // Access data directly from response.data (not response.json())
        setUser(response.data);
      } catch (err) {
        // Axios error object has useful properties:
        // err.response - the server response (if received)
        // err.request - the request that was made
        // err.message - error message
        setError(err.response?.data?.message || err.message);
      } finally {
        setIsLoading(false);
      }
    };

    fetchUser();
  }, [userId]);

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h1>{user.name}</h1>
      <p>{user.email}</p>
    </div>
  );
}

export default UserProfile;
```

### Handling Different Request Types
Axios provides intuitive methods for all HTTP operations. Each method returns a Promise that resolves to the response object.

```jsx
// File: src/api/resources.js

import apiClient from './client';

/**
 * GET request - retrieve data
 * Axios automatically throws on 4xx/5xx responses
 */
async function getUsers() {
  const response = await apiClient.get('/users');
  return response.data; // The parsed JSON response
}

async function getUserById(id) {
  // Path parameters are automatically encoded
  const response = await apiClient.get(`/users/${id}`);
  return response.data;
}

async function searchUsers(query) {
  // Query parameters can be passed as object (Axios handles encoding)
  const response = await apiClient.get('/users/search', {
    params: {
      q: query,
      limit: 10,
      sort: 'name',
    },
  });
  return response.data;
}

/**
 * POST request - create new data
 */
async function createUser(userData) {
  // Axios automatically stringifies objects to JSON
  const response = await apiClient.post('/users', userData);
  return response.data;
}

/**
 * PUT request - full update (replace entire resource)
 */
async function updateUser(id, userData) {
  const response = await apiClient.put(`/users/${id}`, userData);
  return response.data;
}

/**
 * PATCH request - partial update
 */
async function patchUser(id, changes) {
  const response = await apiClient.patch(`/users/${id}`, changes);
  return response.data;
}

/**
 * DELETE request - remove resource
 */
async function deleteUser(id) {
  const response = await apiClient.delete(`/users/${id}`);
  // DELETE typically returns null data on success
  return response.data;
}

/**
 * Concurrent requests - run multiple requests in parallel
 */
async function getUserWithPosts(userId) {
  // Promise.all waits for all requests to complete
  // If any request fails, the entire Promise fails
  const [userResponse, postsResponse] = await Promise.all([
    apiClient.get(`/users/${userId}`),
    apiClient.get(`/users/${userId}/posts`),
  ]);

  return {
    user: userResponse.data,
    posts: postsResponse.data,
  };
}

/**
 * Request with custom configuration
 */
async function uploadFile(file, onProgress) {
  // Create FormData for file uploads
  const formData = new FormData();
  formData.append('file', file);

  const response = await apiClient.post('/upload', formData, {
    // Override default headers for file upload
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    // Track upload progress (available in browser)
    onUploadProgress: (progressEvent) => {
      const percentCompleted = Math.round(
        (progressEvent.loaded * 100) / progressEvent.total
      );
      // Call progress callback if provided
      onProgress?.(percentCompleted);
    },
  });

  return response.data;
}

export {
  getUsers,
  getUserById,
  searchUsers,
  createUser,
  updateUser,
  patchUser,
  deleteUser,
  getUserWithPosts,
  uploadFile,
};
```

### Creating Multiple API Instances
You might need different configurations for different APIs. For example, a public API and an authenticated API.

```jsx
// File: src/api/index.js

import axios from 'axios';

// Public API client - no authentication
export const publicApi = axios.create({
  baseURL: 'https://jsonplaceholder.typicode.com',
  timeout: 5000,
});

// Authenticated API client
export const privateApi = axios.create({
  baseURL: 'https://api.yourapp.com',
  timeout: 10000,
});

// Add auth interceptor only to private API
privateApi.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Third-party API client (e.g., Stripe, GitHub)
export const githubApi = axios.create({
  baseURL: 'https://api.github.com',
  timeout: 5000,
  // GitHub requires specific headers
  headers: {
    'Accept': 'application/vnd.github.v3+json',
  },
});

// Add GitHub token if available
githubApi.interceptors.request.use((config) => {
  const token = localStorage.getItem('githubToken');
  if (token) {
    config.headers.Authorization = `token ${token}`;
  }
  return config;
});
```

## Common Mistakes

### Mistake 1: Not Handling Errors Properly
Axios throws on HTTP errors, but you need to handle both success and error cases explicitly.

```jsx
// ❌ WRONG — Not catching errors
function UserList() {
  const [users, setUsers] = useState([]);
  
  useEffect(() => {
    // If this fails, the error propagates unhandled
    const { data } = await apiClient.get('/users');
    setUsers(data);
  }, []);

  return <ul>{users.map(u => <li>{u.name}</li>)}</ul>;
}

// ✅ CORRECT — Always wrap in try/catch
function UserList() {
  const [users, setUsers] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchUsers() {
      try {
        const { data } = await apiClient.get('/users');
        setUsers(data);
      } catch (err) {
        setError(err.response?.data?.message || 'Failed to fetch users');
      }
    }
    fetchUsers();
  }, []);

  return error ? <div>{error}</div> : <ul>{users.map(u => <li>{u.name}</li>)}</ul>;
}
```

### Mistake 2: Not Canceling Requests on Unmount
Like Fetch, Axios requests continue even if the component unmounts, causing memory leaks.

```jsx
// ❌ WRONG — No cleanup, causes memory leak
function SearchResults({ query }) {
  const [results, setResults] = useState([]);

  useEffect(() => {
    const search = async () => {
      const { data } = await apiClient.get(`/search?q=${query}`);
      setResults(data);
    };
    search();
  }, [query]);

  return <div>{results.length} results</div>;
}

// ✅ CORRECT — Use CancelToken or AbortSignal
import { useEffect, useRef } from 'react';

function SearchResults({ query }) {
  const [results, setResults] = useState([]);

  useEffect(() => {
    // Create a CancelToken source
    const CancelToken = axios.CancelToken;
    const source = CancelToken.source();

    const search = async () => {
      try {
        const { data } = await apiClient.get(`/search?q=${query}`, {
          cancelToken: source.token, // Link cancellation to this request
        });
        setResults(data);
      } catch (err) {
        // Ignore cancel errors
        if (!axios.isCancel(err)) {
          console.error(err);
        }
      }
    };

    search();

    // Cleanup: cancel request on unmount or query change
    return () => source.cancel('Component unmounted');
  }, [query]);

  return <div>{results.length} results</div>;
}
```

### Mistake 3: Using Wrong Config for File Uploads
Uploading files requires FormData and special content type handling.

```jsx
// ❌ WRONG — Sending file as regular JSON
async function uploadFile(file) {
  // This won't work - files can't be JSON stringified
  await apiClient.post('/upload', {
    file: file,
    name: file.name,
  });
}

// ✅ CORRECT — Use FormData
async function uploadFile(file) {
  const formData = new FormData();
  formData.append('file', file); // Axios detects FormData and sets correct headers
  
  await apiClient.post('/upload', formData);
}
```

## Real-World Example
Building a complete API service layer with authentication and error handling.

```jsx
// File: src/services/authService.js

import axios from 'axios';

// Create axios instance for auth endpoints
const authApi = axios.create({
  baseURL: 'https://api.yourapp.com/auth',
});

/**
 * Auth Service - handles all authentication API calls
 */
export const authService = {
  /**
   * Login with email and password
   * Returns tokens and user data
   */
  async login(email, password) {
    try {
      const response = await authApi.post('/login', { email, password });
      const { token, refreshToken, user } = response.data;
      
      // Store tokens securely
      localStorage.setItem('authToken', token);
      localStorage.setItem('refreshToken', refreshToken);
      
      return { user, token };
    } catch (error) {
      if (error.response?.status === 401) {
        throw new Error('Invalid email or password');
      }
      throw error;
    }
  },

  /**
   * Register new user
   */
  async register(userData) {
    const response = await authApi.post('/register', userData);
    return response.data;
  },

  /**
   * Logout - invalidate token on server
   */
  async logout() {
    try {
      await authApi.post('/logout');
    } finally {
      // Always clear local storage
      localStorage.removeItem('authToken');
      localStorage.removeItem('refreshToken');
    }
  },

  /**
   * Refresh access token using refresh token
   */
  async refreshToken() {
    const refreshToken = localStorage.getItem('refreshToken');
    if (!refreshToken) {
      throw new Error('No refresh token available');
    }

    const response = await authApi.post('/refresh', { refreshToken });
    const { token } = response.data;
    
    localStorage.setItem('authToken', token);
    return token;
  },

  /**
   * Get current user profile
   */
  async getProfile() {
    const response = await authApi.get('/me');
    return response.data;
  },
};

export default authService;

// File: src/hooks/useAuth.js - Custom hook for auth state

import { useState, useEffect, useCallback } from 'react';
import { authService } from '../services/authService';

export function useAuth() {
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  // Check auth status on mount
  useEffect(() => {
    const initAuth = async () => {
      const token = localStorage.getItem('authToken');
      
      if (token) {
        try {
          const userData = await authService.getProfile();
          setUser(userData);
          setIsAuthenticated(true);
        } catch (err) {
          // Token invalid or expired
          localStorage.removeItem('authToken');
          localStorage.removeItem('refreshToken');
        }
      }
      
      setIsLoading(false);
    };

    initAuth();
  }, []);

  const login = useCallback(async (email, password) => {
    const { user } = await authService.login(email, password);
    setUser(user);
    setIsAuthenticated(true);
    return user;
  }, []);

  const logout = useCallback(async () => {
    await authService.logout();
    setUser(null);
    setIsAuthenticated(false);
  }, []);

  return {
    user,
    isLoading,
    isAuthenticated,
    login,
    logout,
  };
}

// File: src/components/LoginForm.jsx - Using the auth hook

import React, { useState } from 'react';
import { useAuth } from '../hooks/useAuth';
import { useNavigate } from 'react-router-dom';

function LoginForm() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setIsSubmitting(true);

    try {
      await login(email, password);
      navigate('/dashboard');
    } catch (err) {
      setError(err.message || 'Login failed');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {error && <div className="error">{error}</div>}
      
      <input
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        placeholder="Email"
        required
      />
      
      <input
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        placeholder="Password"
        required
      />
      
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Logging in...' : 'Login'}
      </button>
    </form>
  );
}

export default LoginForm;
```

## Key Takeaways
- Axios provides a cleaner API than Fetch with automatic JSON transformation
- Use axios.create() to build reusable API clients with default configurations
- Request interceptors are perfect for adding authentication tokens automatically
- Response interceptors handle global error handling (401 redirect, 500 errors)
- Always cancel requests using AbortController or CancelToken to prevent memory leaks
- Axios throws on any HTTP error status, so always wrap in try/catch
- Use FormData for file uploads - don't JSON.stringify files

## What's Next
Continue to [Error Handling Strategies](03-error-handling-strategies.md) to learn comprehensive patterns for handling errors in your React data fetching layer.
