# JWT Authentication Flow

## Overview

JSON Web Tokens (JWT) are the standard for stateless authentication in modern web applications. This guide covers understanding JWT structure, implementing login flow, storing tokens securely, and handling token expiration.

## Prerequisites

- React hooks knowledge
- Understanding of HTTP requests
- Basic authentication concepts

## Core Concepts

### JWT Structure

A JWT has three parts separated by dots:
- Header: Contains algorithm and token type
- Payload: Contains claims (user data)
- Signature: Verifies token authenticity

```
xxxxx.yyyyy.zzzzz
```

### Login Implementation

```tsx
// File: src/services/auth.ts

import axios from 'axios';

interface LoginCredentials {
  email: string;
  password: string;
}

interface AuthResponse {
  token: string;
  refreshToken: string;
  user: {
    id: string;
    email: string;
    name: string;
  };
}

export async function login(credentials: LoginCredentials): Promise<AuthResponse> {
  const response = await axios.post('/api/auth/login', credentials);
  return response.data;
}

export async function logout(): Promise<void> {
  await axios.post('/api/auth/logout');
}

export async function refreshToken(refreshToken: string): Promise<AuthResponse> {
  const response = await axios.post('/api/auth/refresh', { refreshToken });
  return response.data;
}
```

### Auth Context

```tsx
// File: src/contexts/AuthContext.tsx

import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { login as loginApi, logout as logoutApi } from '../services/auth';

interface User {
  id: string;
  email: string;
  name: string;
}

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(() => 
    localStorage.getItem('token')
  );
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Check for existing token on mount
    const storedToken = localStorage.getItem('token');
    const storedUser = localStorage.getItem('user');
    
    if (storedToken && storedUser) {
      setToken(storedToken);
      setUser(JSON.parse(storedUser));
    }
    setIsLoading(false);
  }, []);

  const login = async (email: string, password: string) => {
    const response = await loginApi({ email, password });
    const { token, user } = response;
    
    // Store token and user
    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(user));
    
    setToken(token);
    setUser(user);
  };

  const logout = async () => {
    try {
      await logoutApi();
    } finally {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      setToken(null);
      setUser(null);
    }
  };

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
```

## Key Takeaways

- JWTs contain user info in the payload
- Store tokens securely (httpOnly cookies preferred)
- Handle token expiration gracefully
- Implement logout to clear tokens

## What's Next

Continue to [Protected Routes Implementation](/11-real-world-projects/01-auth-system/02-protected-routes-implementation.md) to learn about securing routes.