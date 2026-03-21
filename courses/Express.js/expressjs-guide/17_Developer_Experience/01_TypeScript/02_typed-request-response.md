# Typed Request Response

## 📌 What You'll Learn

- Extending Express Request interface
- Typed body, params, query
- Generic route handlers

## 🧠 Concept Explained (Plain English)

When building TypeScript Express applications, the default Request and Response types from Express give you basic functionality. However, your application likely needs custom properties like user authentication data, validated request bodies, and typed URL parameters. TypeScript allows you to extend these types to add your own properties, making your code safer and more maintainable.

## 💻 Code Example

```js
import { Request, Response, NextFunction } from 'express';

// Extend Request with custom user property
interface AuthRequest extends Request {
  user?: { id: string; role: string; email: string };
}

// Typed request body
interface CreateUserBody {
  email: string;
  password: string;
  name: string;
}

// Typed URL parameters
interface UserParams {
  id: string;
}

// Typed query string
interface ListQuery {
  page?: string;
  limit?: string;
  sort?: 'asc' | 'desc';
}

// POST /users - Create a new user
app.post('/users', (req: AuthRequest, res: Response) => {
  const { email, password, name } = req.body as CreateUserBody;
  
  if (!email || !password) {
    return res.status(400).json({ error: 'Email and password required' });
  }
  
  // Create user logic here
  res.status(201).json({ email, name });
});

// GET /users/:id - Get user by ID
app.get('/users/:id', (req: Request<UserParams>, res: Response) => {
  const { id } = req.params;
  res.json({ id, name: 'John Doe' });
});

// GET /users - List users with pagination
app.get('/users', (req: Request<{}, {}, {}, ListQuery>, res: Response) => {
  const page = parseInt(req.query.page || '1');
  const limit = parseInt(req.query.limit || '10');
  const sort = req.query.sort || 'asc';
  
  res.json({ page, limit, sort, users: [] });
});

export default app;
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 4 | `interface AuthRequest extends Request` | Creates a new interface that inherits all properties from Express Request |
| 5 | `user?: { id: string; role: string; email: string }` | Adds optional user property with typed structure |
| 12 | `interface CreateUserBody` | Defines the expected shape of request body for user creation |
| 17 | `interface UserParams` | Defines typed URL parameters |
| 22 | `interface ListQuery` | Defines typed query string parameters |
| 29 | `(req: AuthRequest, res: Response)` | Uses extended Request type in route handler |

## ⚠️ Common Mistakes

1. **Forgetting to cast req.body**: TypeScript doesn't know the shape of req.body by default, so you must cast it to your interface
2. **Not handling undefined properties**: When using optional properties (? :), always check they exist before using
3. **Using wrong generic parameters**: Request takes three generics - Params, ResBody, ReqBody - in that order

## ✅ Quick Recap

- Extend Request interface to add custom properties like authenticated user
- Cast req.body to your interface for type-safe access
- Use generics on Request for typed params, body, and query
- Create separate interfaces for different route handler needs

## 🔗 What's Next

Learn about TypeScript middleware patterns to pass typed data through the request chain.
