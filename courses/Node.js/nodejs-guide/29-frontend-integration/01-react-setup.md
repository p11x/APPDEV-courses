# React Setup with Node.js

## What You'll Learn

- Setting up a React frontend with a Node.js backend
- Development proxy configuration
- Production build and serving

## Project Structure

```
my-fullstack-app/
├── client/           # React frontend
│   ├── src/
│   ├── public/
│   └── package.json
├── server/           # Node.js backend
│   ├── src/
│   └── package.json
└── package.json      # Root package.json (workspaces)
```

## Root package.json

```json
{
  "name": "my-fullstack-app",
  "private": true,
  "workspaces": ["client", "server"],
  "scripts": {
    "dev:server": "npm run dev --workspace=server",
    "dev:client": "npm run dev --workspace=client",
    "dev": "npm run dev:server & npm run dev:client",
    "build": "npm run build --workspace=client",
    "start": "npm start --workspace=server"
  }
}
```

## Client Proxy (Vite)

```js
// client/vite.config.js
export default {
  server: {
    proxy: {
      '/api': 'http://localhost:3000',
    },
  },
};
```

## Server Serves Client Build

```js
// server/src/app.js
import express from 'express';
import { resolve } from 'node:path';

const app = express();

// API routes
app.use('/api', apiRoutes);

// Serve React build in production
if (process.env.NODE_ENV === 'production') {
  app.use(express.static(resolve(__dirname, '../../client/dist')));
  app.get('*', (req, res) => {
    res.sendFile(resolve(__dirname, '../../client/dist/index.html'));
  });
}
```

## Next Steps

For API client setup, continue to [API Client](./02-api-client.md).
