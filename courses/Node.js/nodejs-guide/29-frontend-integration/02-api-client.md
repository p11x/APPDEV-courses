# API Client

## What You'll Learn

- Fetch vs Axios comparison
- Creating a typed API client
- Error handling and interceptors

## Fetch vs Axios

| Feature | fetch | Axios |
|---------|-------|-------|
| Browser built-in | Yes | No (install needed) |
| Auto JSON parse | No (manual) | Yes |
| Request interceptors | No | Yes |
| Cancel requests | AbortController | CancelToken/AbortController |
| Progress tracking | No | Yes |

## API Client with fetch

```js
// api/client.js

const BASE_URL = import.meta.env.VITE_API_URL || '/api';

async function request(endpoint, options = {}) {
  const { method = 'GET', body, headers = {} } = options;

  const config = {
    method,
    headers: {
      'Content-Type': 'application/json',
      ...headers,
    },
  };

  if (body) {
    config.body = JSON.stringify(body);
  }

  // Add auth token if available
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  const response = await fetch(`${BASE_URL}${endpoint}`, config);

  if (!response.ok) {
    const error = await response.json().catch(() => ({ error: 'Request failed' }));
    throw new Error(error.error || `HTTP ${response.status}`);
  }

  return response.json();
}

export const api = {
  get: (endpoint) => request(endpoint),
  post: (endpoint, data) => request(endpoint, { method: 'POST', body: data }),
  put: (endpoint, data) => request(endpoint, { method: 'PUT', body: data }),
  delete: (endpoint) => request(endpoint, { method: 'DELETE' }),
};
```

## Usage in React

```jsx
// components/UserList.jsx
import { useState, useEffect } from 'react';
import { api } from '../api/client.js';

export function UserList() {
  const [users, setUsers] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    api.get('/users')
      .then(setUsers)
      .catch((err) => setError(err.message));
  }, []);

  if (error) return <div>Error: {error}</div>;

  return (
    <ul>
      {users.map((user) => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

## Next Steps

For state management, continue to [State Management](./03-state-management.md).
