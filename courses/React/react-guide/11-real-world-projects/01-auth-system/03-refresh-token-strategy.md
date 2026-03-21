# Refresh Token Strategy

## Overview

JWT access tokens expire quickly for security. Refresh tokens allow users to stay logged in without re-entering credentials. This guide covers implementing automatic token refresh using axios interceptors.

## Prerequisites

- JWT authentication understanding
- Axios knowledge

## Core Concepts

### Axios Interceptor for Token Refresh

```tsx
// File: src/services/api.ts

import axios from 'axios';

const api = axios.create({
  baseURL: '/api',
});

// Track if we're currently refreshing
let isRefreshing = false;
// Queue of requests waiting for refresh
let failedQueue: Array<{
  resolve: (token: string) => void;
  reject: (error: Error) => void;
}> = [];

const processQueue = (error: Error | null, token: string | null = null) => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token!);
    }
  });
  failedQueue = [];
};

// Add token to requests
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Handle 401 responses
api.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config;
    
    // If already refreshing or not a 401, reject
    if (error.response?.status !== 401 || originalRequest._retry) {
      return Promise.reject(error);
    }
    
    originalRequest._retry = true;
    
    if (isRefreshing) {
      // Queue the request
      return new Promise((resolve, reject) => {
        failedQueue.push({ resolve, reject });
      })
        .then(token => {
          originalRequest.headers.Authorization = `Bearer ${token}`;
          return api(originalRequest);
        })
        .catch(err => Promise.reject(err));
    }
    
    isRefreshing = true;
    
    try {
      const refreshToken = localStorage.getItem('refreshToken');
      const response = await axios.post('/api/auth/refresh', { refreshToken });
      const { token } = response.data;
      
      localStorage.setItem('token', token);
      processQueue(null, token);
      
      originalRequest.headers.Authorization = `Bearer ${token}`;
      return api(originalRequest);
    } catch (refreshError) {
      processQueue(refreshError as Error, null);
      
      // Clear auth on refresh failure
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
      
      window.location.href = '/login';
      return Promise.reject(refreshError);
    } finally {
      isRefreshing = false;
    }
  }
);

export default api;
```

## Key Takeaways

- Use interceptors to handle 401 errors automatically
- Queue requests during token refresh
- Handle refresh failure by logging out user
- Store refresh token securely

## What's Next

Continue to [Dashboard Layout Architecture](/11-real-world-projects/02-dashboard-app/01-dashboard-layout-architecture.md) to learn about building dashboard interfaces.