# React with FastAPI

## What You'll Learn
- Connecting React frontend to FastAPI
- Using fetch and Axios
- Handling authentication
- CORS configuration

## Prerequisites
- React basics
- FastAPI basics

## FastAPI Configuration

```python
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

# Configure CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost:3000"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/api/users")
async def get_users():
    return [{"id": 1, "name": "John"}]

@app.post("/api/users")
async def create_user(user: dict):
    return {"id": 2, "name": user["name"]}
```

## React API Service

```javascript
// services/api.js
const API_BASE = 'http://localhost:8000';

export const api = {
  async getUsers() {
    const response = await fetch(`${API_BASE}/api/users`);
    return response.json();
  },
  
  async createUser(user) {
    const response = await fetch(`${API_BASE}/api/users`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(user),
    });
    return response.json();
  },
  
  async getUser(id) {
    const response = await fetch(`${API_BASE}/api/users/${id}`);
    return response.json();
  },
};
```

## React Component

```javascript
// components/UserList.jsx
import { useState, useEffect } from 'react';
import { api } from '../services/api';

function UserList() {
  const [users, setUsers] = useState([]);
  
  useEffect(() => {
    api.getUsers().then(setUsers);
  }, []);
  
  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

## Summary

- Configure CORS in FastAPI for frontend access
- Use fetch or Axios for API calls
- Keep API base URL in environment variables
- Handle loading and error states in React
