# API Mocking for Development

## 📌 What You'll Learn

- MSW for frontend mocking
- json-server for quick APIs
- Building mock Express server

## 🧠 Concept Explained (Plain English)

API mocking means creating fake versions of your API that your frontend can call during development. This is useful when the backend isn't ready yet, when you want to test edge cases without affecting real data, or when you need to develop offline.

MSW (Mock Service Worker) intercepts network requests in the browser and returns mock responses. It's powerful because it works at the network level, so your code makes real HTTP calls but receives fake responses.

json-server is a tool that creates a full fake REST API from a JSON file in seconds - perfect for prototyping.

## MSW Setup

```js
// Installation
// npm install -D msw

// src/mocks/handlers.ts - Define mock endpoints
import { http, HttpResponse } from 'msw';

export const handlers = [
  // GET /users - Return mock user list
  http.get('/api/users', () => {
    return HttpResponse.json([
      { id: '1', name: 'Alice', email: 'alice@example.com' },
      { id: '2', name: 'Bob', email: 'bob@example.com' },
    ]);
  }),

  // GET /users/:id - Return single user
  http.get('/api/users/:id', ({ params }) => {
    const { id } = params;
    return HttpResponse.json({
      id,
      name: 'Alice',
      email: 'alice@example.com',
    });
  }),

  // POST /users - Create user
  http.post('/api/users', async ({ request }) => {
    const body = await request.json();
    return HttpResponse.json(
      { id: '3', ...body },
      { status: 201 }
    );
  }),

  // Handle errors
  http.get('/api/protected', () => {
    return new HttpResponse(null, {
      status: 401,
      statusText: 'Unauthorized',
    });
  }),
];
```

## Setting Up MSW

```js
// src/mocks/browser.ts - Browser setup
import { setupWorker } from 'msw/browser';
import { handlers } from './handlers';

export const worker = setupWorker(...handlers);

// src/main.tsx - Initialize in development
import { worker } from './mocks/browser';

async function enableMocking() {
  if (process.env.NODE_ENV === 'development') {
    return worker.start({
      onUnhandledRequest: 'bypass',
    });
  }
}

enableMocking().then(() => {
  // Start your app
  ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  );
});
```

## json-server

```js
// Installation
// npm install -D json-server

// db.json - Mock database
{
  "users": [
    { "id": "1", "name": "Alice", "email": "alice@example.com" },
    { "id": "2", "name": "Bob", "email": "bob@example.com" }
  ],
  "posts": [
    { "id": "1", "userId": "1", "title": "Hello World", "body": "My first post" }
  ]
}

// package.json script
{
  "scripts": {
    "mock:api": "json-server --watch db.json --port 3001"
  }
}

// Routes are automatically created:
// GET    /users
// GET    /users/1
// POST   /users
// PUT    /users/1
// DELETE /users/1
// GET    /users?_page=1&_limit=10  (pagination)
// GET    /users?_sort=name&_order=asc (sorting)
```

## Custom Mock Express Server

```js
// mocks/server.ts - Full-featured mock server
import express from 'express';
import cors from 'cors';

const app = express();
app.use(cors());
app.use(express.json());

// In-memory database
const users = new Map([
  ['1', { id: '1', name: 'Alice', email: 'alice@example.com' }],
  ['2', { id: '2', name: 'Bob', email: 'bob@example.com' }],
]);

// GET /api/users
app.get('/api/users', (req, res) => {
  const userList = Array.from(users.values());
  res.json(userList);
});

// GET /api/users/:id
app.get('/api/users/:id', (req, res) => {
  const user = users.get(req.params.id);
  
  if (!user) {
    return res.status(404).json({ error: 'User not found' });
  }
  
  res.json(user);
});

// POST /api/users
app.post('/api/users', (req, res) => {
  const { name, email } = req.body;
  
  if (!name || !email) {
    return res.status(400).json({ error: 'Name and email required' });
  }
  
  const id = crypto.randomUUID();
  const user = { id, name, email };
  users.set(id, user);
  
  res.status(201).json(user);
});

// Simulate network delay
app.use((req, res, next) => {
  setTimeout(next, Math.random() * 500 + 100);
});

const PORT = process.env.MOCK_PORT || 3001;
app.listen(PORT, () => {
  console.log(`Mock API running on port ${PORT}`);
});
```

## ⚠️ Common Mistakes

1. **Not matching real API**: Mock responses should match the real API structure
2. **Missing error cases**: Test error handling with mock error responses
3. **Forgetting to disable mocks**: Ensure mocks don't run in production

## ✅ Quick Recap

- MSW intercepts browser requests for realistic mocking
- json-server creates a quick REST API from JSON
- Custom mock servers give full control
- Use mocks in development, disable in production
- Test error handling with mock error responses

## 🔗 What's Next

Learn about semantic-release and automatic changelog generation.
