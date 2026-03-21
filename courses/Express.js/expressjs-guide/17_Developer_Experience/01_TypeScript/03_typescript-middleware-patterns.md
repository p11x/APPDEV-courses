# TypeScript Middleware Patterns

## 📌 What You'll Learn

- Creating typed middleware functions
- Passing typed data through req
- Typed error handlers

## 🧠 Concept Explained (Plain English)

Middleware in Express is functions that have access to the request object (req), response object (res), and the next middleware function (next). In TypeScript, we want these middleware functions to be properly typed so that when we add custom properties to the request object, TypeScript knows about them. This prevents runtime errors and gives us autocomplete in our IDEs.

A typed middleware means specifying the exact types for req, res, and next parameters. When you extend the Request object with custom properties, you need to create middleware that uses your extended type so TypeScript recognizes those properties in subsequent middleware and route handlers.

## 💻 Code Example

```js
import { Request, Response, NextFunction, RequestHandler } from 'express';

// Extended request with user property
interface AuthRequest extends Request {
  user?: { id: string; role: string; email: string };
}

// Extended request with tenant property for multi-tenancy
interface TenantRequest extends Request {
  tenantId?: string;
}

// Authentication middleware with proper typing
export const authenticate: RequestHandler = (
  req: AuthRequest,
  res: Response,
  next: NextFunction
): void => {
  const authHeader = req.headers.authorization;
  
  if (!authHeader) {
    res.status(401).json({ error: 'No authorization header' });
    return;
  }
  
  // Verify token and set user (simplified)
  const user = { id: 'user-123', role: 'admin', email: 'test@example.com' };
  req.user = user;
  
  next();
};

// Authorization middleware - checks user role
export const authorize = (roles: string[]): RequestHandler => {
  return (req: AuthRequest, res: Response, next: NextFunction): void => {
    if (!req.user) {
      res.status(401).json({ error: 'Not authenticated' });
      return;
    }
    
    if (!roles.includes(req.user.role)) {
      res.status(403).json({ error: 'Insufficient permissions' });
      return;
    }
    
    next();
  };
};

// Tenant isolation middleware
export const isolateTenant: RequestHandler = (
  req: TenantRequest,
  res: Response,
  next: NextFunction
): void => {
  // Extract tenant from subdomain, header, or JWT
  const tenantId = req.headers['x-tenant-id'] as string || 'default';
  req.tenantId = tenantId;
  next();
};

// Typed error handler
interface ApiError extends Error {
  statusCode?: number;
  isOperational?: boolean;
}

export const errorHandler = (
  err: ApiError,
  req: Request,
  res: Response,
  next: NextFunction
): void => {
  console.error('Error:', err.message);
  
  const statusCode = err.statusCode || 500;
  const message = err.isOperational ? err.message : 'Internal server error';
  
  res.status(statusCode).json({ error: message });
};

// Using middleware with TypeScript
import express from 'express';
const app = express();

app.use(express.json());

// Public route
app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});

// Protected route with multiple middleware
app.post(
  '/admin/users',
  authenticate,
  authorize(['admin']),
  (req: AuthRequest, res: Response) => {
    // TypeScript knows req.user exists here
    res.json({ 
      message: 'User created',
      createdBy: req.user?.email 
    });
  }
);

// Apply error handler last
app.use(errorHandler);

export default app;
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 4 | `interface AuthRequest extends Request` | Extends Express Request to add user property |
| 9 | `interface TenantRequest extends Request` | Extends Request for multi-tenant apps |
| 13 | `export const authenticate: RequestHandler` | Declares middleware with RequestHandler type |
| 15 | `(req: AuthRequest, res: Response, next: NextFunction): void` | Typed parameters, void return |
| 28 | `req.user = user` | Attaches user to typed request |
| 34 | `export const authorize = (roles: string[])` | Higher-order function returning typed middleware |
| 47 | `return (req: AuthRequest, res: Response, next: NextFunction): void` | Returns middleware with full typing |
| 63 | `interface ApiError extends Error` | Extends Error for API-specific properties |
| 71 | `(err: ApiError, req: Request, res: Response, next: NextFunction)` | Error handler has err as first parameter |

## ⚠️ Common Mistakes

1. **Not using RequestHandler type**: Using generic function types instead of Express's RequestHandler can cause type inference issues
2. **Forgetting to call next()**: TypeScript won't catch this at compile time, but your request will hang
3. **Mismatched extended interfaces**: If you extend Request differently in different files, TypeScript might get confused - keep extensions in one place

## ✅ Quick Recap

- Use `RequestHandler` type from Express for middleware functions
- Extend the Request interface to add custom properties
- Use higher-order functions to create parameterized middleware like authorize(roles)
- Error handlers have four parameters (err, req, res, next) - TypeScript knows it's an error handler
- Always type the return as void for middleware

## 🔗 What's Next

Learn about OpenAPI-first development with Zod for runtime validation and type safety.
