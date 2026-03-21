# React with Node.js API

## Overview

Connecting a React frontend with a Node.js backend requires proper API design, CORS configuration, and environment variable management. This guide covers setting up the API service layer and handling environment variables.

## Prerequisites

- Express.js basics
- React data fetching knowledge

## Core Concepts

### Environment Variables

```typescript
// File: src/services/api.ts

import axios from 'axios';

// Use environment variables for API URL
const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:3001';

export const api = axios.create({
  baseURL: API_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add auth token to requests
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
```

```env
# File: .env.local
VITE_API_URL=http://localhost:3001
```

```env
# File: .env.production
VITE_API_URL=https://api.example.com
```

### API Service Layer

```typescript
// File: src/services/userService.ts

import api from './api';

export interface User {
  id: string;
  email: string;
  name: string;
}

export const userService = {
  async getAll(): Promise<User[]> {
    const response = await api.get('/users');
    return response.data;
  },

  async getById(id: string): Promise<User> {
    const response = await api.get(`/users/${id}`);
    return response.data;
  },

  async create(data: Partial<User>): Promise<User> {
    const response = await api.post('/users', data);
    return response.data;
  },

  async update(id: string, data: Partial<User>): Promise<User> {
    const response = await api.put(`/users/${id}`, data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/users/${id}`);
  },
};
```

### Node.js CORS Setup

```javascript
// File: server/index.js

const express = require('express');
const cors = require('cors');

const app = express();

// Configure CORS
app.use(cors({
  origin: process.env.CLIENT_URL || 'http://localhost:5173',
  credentials: true,
}));

app.use(express.json());

// Routes
app.use('/api', require('./routes'));

app.listen(3001, () => {
  console.log('Server running on port 3001');
});
```

## Key Takeaways

- Use environment variables for configuration
- Create a service layer for API calls
- Configure CORS on the backend
- Handle authentication tokens

## What's Next

Continue to [React Query with REST API](/11-real-world-projects/03-full-stack-integration/02-react-query-with-rest-api.md) to learn about data fetching patterns.