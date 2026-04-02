# TypeScript Integration with Node.js

## What You'll Learn

- Setting up TypeScript with Node.js
- tsconfig.json configuration
- Type-safe API development
- Build and runtime patterns

## TypeScript Setup

```bash
# Initialize TypeScript project
npm init -y
npm install -D typescript @types/node tsx

# Initialize tsconfig
npx tsc --init
```

### tsconfig.json for Node.js

```json
{
  "compilerOptions": {
    "target": "ES2022",
    "module": "NodeNext",
    "moduleResolution": "NodeNext",
    "lib": ["ES2022"],
    "outDir": "./dist",
    "rootDir": "./src",
    "strict": true,
    "esModuleInterop": true,
    "skipLibCheck": true,
    "forceConsistentCasingInFileNames": true,
    "resolveJsonModule": true,
    "declaration": true,
    "declarationMap": true,
    "sourceMap": true,
    "noUnusedLocals": true,
    "noUnusedParameters": true,
    "noFallthroughCasesInSwitch": true
  },
  "include": ["src/**/*"],
  "exclude": ["node_modules", "dist"]
}
```

### Running TypeScript

```bash
# Option 1: tsx (fastest, recommended for development)
npx tsx watch src/server.ts

# Option 2: ts-node
npx ts-node src/server.ts

# Option 3: Compile then run
npx tsc
node dist/server.js
```

## Type-Safe Express Server

```typescript
// src/server.ts — Type-safe Express application

import express, { Request, Response, NextFunction } from 'express';

// Type definitions
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

// Typed route handlers
const app = express();
app.use(express.json());

app.get('/api/users', (req: Request, res: Response<User[]>) => {
    const users: User[] = [
        { id: 1, name: 'Alice', email: 'alice@example.com' },
    ];
    res.json(users);
});

app.get('/api/users/:id', (
    req: Request<GetUserParams>,
    res: Response<User | { error: string }>
) => {
    const id = parseInt(req.params.id, 10);
    const user: User | undefined = findUser(id);
    
    if (!user) {
        return res.status(404).json({ error: 'User not found' });
    }
    res.json(user);
});

app.post('/api/users', (
    req: Request<{}, User, CreateUserBody>,
    res: Response<User>
) => {
    const { name, email } = req.body; // Fully typed!
    const user: User = { id: Date.now(), name, email };
    res.status(201).json(user);
});

// Typed error handler
app.use((err: Error, req: Request, res: Response, next: NextFunction) => {
    console.error(err.stack);
    res.status(500).json({ error: 'Internal server error' });
});
```

## Environment Variables with Types

```typescript
// src/config.ts — Typed environment configuration

import { z } from 'zod';

const envSchema = z.object({
    PORT: z.coerce.number().default(3000),
    NODE_ENV: z.enum(['development', 'production', 'test']).default('development'),
    DATABASE_URL: z.string().url(),
    JWT_SECRET: z.string().min(32),
    REDIS_URL: z.string().url().optional(),
});

export const env = envSchema.parse(process.env);

// Usage: env.PORT is number, env.DATABASE_URL is string
// Throws on startup if validation fails
```

## Best Practices Checklist

- [ ] Use `"strict": true` in tsconfig.json
- [ ] Use `tsx` for development (faster than ts-node)
- [ ] Define interfaces for all API request/response types
- [ ] Use Zod for runtime environment validation
- [ ] Enable `declaration` for library packages
- [ ] Use path aliases for cleaner imports

## Cross-References

- See [ESLint Prettier](./02-eslint-prettier.md) for code quality
- See [Testing Frameworks](./03-testing-frameworks.md) for test setup
- See [CI/CD Integration](../../../26-cicd-github-actions/) for automation

## Next Steps

Continue to [ESLint and Prettier](./02-eslint-prettier.md) for code quality tools.
