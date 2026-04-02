# Express.js TypeScript Integration

## What You'll Learn

- TypeScript setup for Express
- Type-safe route definitions
- Type-safe middleware
- Type-safe error handling

## TypeScript Setup

```bash
npm install -D typescript @types/node @types/express
npx tsc --init
```

```json
// tsconfig.json
{
    "compilerOptions": {
        "target": "ES2022",
        "module": "NodeNext",
        "moduleResolution": "NodeNext",
        "outDir": "./dist",
        "rootDir": "./src",
        "strict": true,
        "esModuleInterop": true,
        "skipLibCheck": true
    },
    "include": ["src/**/*"]
}
```

## Type-Safe Routes

```typescript
import express, { Request, Response, NextFunction } from 'express';

interface User {
    id: number;
    name: string;
    email: string;
}

interface CreateUserBody {
    name: string;
    email: string;
}

interface GetUserParams {
    id: string;
}

const app = express();
app.use(express.json());

app.get('/api/users', (req: Request, res: Response<User[]>) => {
    res.json([{ id: 1, name: 'Alice', email: 'alice@example.com' }]);
});

app.get('/api/users/:id', (
    req: Request<GetUserParams>,
    res: Response<User | { error: string }>
) => {
    res.json({ id: +req.params.id, name: 'Alice', email: 'alice@example.com' });
});

app.post('/api/users', (
    req: Request<{}, User, CreateUserBody>,
    res: Response<User>
) => {
    const { name, email } = req.body; // Fully typed
    res.status(201).json({ id: Date.now(), name, email });
});
```

## Type-Safe Middleware

```typescript
import { Request, Response, NextFunction } from 'express';

interface AuthRequest extends Request {
    user?: { id: number; role: string };
}

function authenticate(req: AuthRequest, res: Response, next: NextFunction) {
    const token = req.headers.authorization?.split(' ')[1];
    if (!token) {
        return res.status(401).json({ error: 'Unauthorized' });
    }
    req.user = verifyToken(token);
    next();
}

function authorize(...roles: string[]) {
    return (req: AuthRequest, res: Response, next: NextFunction) => {
        if (!req.user || !roles.includes(req.user.role)) {
            return res.status(403).json({ error: 'Forbidden' });
        }
        next();
    };
}
```

## Best Practices Checklist

- [ ] Use TypeScript for all new Express projects
- [ ] Define interfaces for request/response types
- [ ] Use type-safe middleware
- [ ] Enable strict mode in tsconfig
- [ ] Use tsx for development

## Cross-References

- See [Error Handling](../08-error-handling/01-centralized-errors.md) for error types
- See [Middleware](../03-middleware-guide/01-custom-middleware.md) for middleware
- See [API Design](../10-api-design-patterns/01-rest-api.md) for API patterns

## Next Steps

Continue to [API Design Patterns](../10-api-design-patterns/01-rest-api.md) for API design.
