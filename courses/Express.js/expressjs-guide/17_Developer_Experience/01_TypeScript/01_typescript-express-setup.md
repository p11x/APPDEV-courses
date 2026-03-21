# TypeScript Express Setup

## 📌 What You'll Learn

- Setting up TypeScript with Express
- ts-node / tsx usage
- Typed req and res

## 💻 Code Example

```js
// tsconfig.json
/*
{
  "compilerOptions": {
    "target": "ES2022",
    "module": "ESNext",
    "moduleResolution": "node",
    "strict": true,
    "esModuleInterop": true
  }
}
*/

// Using tsx for development
// npx tsx server.ts

import express, { Request, Response } from 'express';

const app = express();

app.get('/users/:id', (req: Request, res: Response) => {
  const { id } = req.params;
  res.json({ id, name: 'John' });
});

export default app;
```

## ✅ Quick Recap

- Use tsx for fast development
- Enable strict mode in tsconfig
- Type Request and Response explicitly
