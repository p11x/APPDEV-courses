# Vercel Deployment

## What You'll Learn

- Deploying Node.js APIs to Vercel
- Serverless function configuration
- Environment variables in Vercel

## Setup

```bash
npm install -g vercel
vercel login
vercel
```

## Serverless Function

```js
// api/users.js — Vercel serverless function

export default function handler(req, res) {
  if (req.method === 'GET') {
    res.json([{ id: 1, name: 'Alice' }]);
  } else {
    res.status(405).json({ error: 'Method not allowed' });
  }
}
```

## vercel.json

```json
{
  "version": 2,
  "builds": [
    { "src": "api/**/*.js", "use": "@vercel/node" }
  ],
  "routes": [
    { "src": "/api/(.*)", "dest": "/api/$1" }
  ]
}
```

## Next Steps

For Railway, continue to [Railway Deploy](./03-railway-deploy.md).
