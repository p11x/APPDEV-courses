# Express Router

## What You'll Learn

- Using express.Router
- Modular route organization
- Creating route modules

## express.Router

Create modular, mountable route handlers:

```javascript
// users.js - User routes module

import express from 'express';
const router = express.Router();

router.get('/', (req, res) => {
  res.json({ users: [] });
});

router.get('/:id', (req, res) => {
  res.json({ id: req.params.id });
});

export default router;
```

```javascript
// index.js - Main app

import express from 'express';
import usersRouter from './users.js';

const app = express();
app.use('/users', usersRouter);
app.listen(3000);
```

## Try It Yourself

### Exercise 1: Create Router Module
Create a separate router module for posts.

### Exercise 2: Mount Router
Mount the router in your main app.
