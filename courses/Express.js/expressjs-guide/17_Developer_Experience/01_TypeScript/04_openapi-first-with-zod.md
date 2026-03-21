# OpenAPI First with Zod

## 📌 What You'll Learn

- Zod schemas for runtime validation
- Generating OpenAPI specs from Zod
- Type safety and validation in one step

## 🧠 Concept Explained (Plain English)

Traditionally in web development, you'd write your API specification (OpenAPI/Swagger) separately from your code, or write your code first and then generate documentation. OpenAPI-first development flips this around: you define your data schemas first using Zod, and those schemas become both your runtime validators and your API documentation. This means you write your validation logic once, and it automatically generates type-safe code and OpenAPI documentation.

Zod is a TypeScript-first schema validation library. You define what your data should look like using Zod's schema builders, and Zod can validate data at runtime while also inferring TypeScript types from those schemas. The `zod-to-openapi` library bridges Zod schemas to OpenAPI specifications.

## 💻 Code Example

```js
import { z } from 'zod';
import { generateOpenAPISpec } from 'zod-to-openapi';
import express from 'express';

const app = express();
app.use(express.json());

// Define request/response schemas using Zod
const UserSchema = z.object({
  id: z.string().uuid(),
  email: z.string().email(),
  name: z.string().min(1),
  createdAt: z.string().datetime(),
});

const CreateUserSchema = z.object({
  email: z.string().email(),
  name: z.string().min(1),
  password: z.string().min(8),
});

const ErrorSchema = z.object({
  error: z.string(),
  details: z.array(z.object({
    path: z.string(),
    message: z.string(),
  })).optional(),
});

// Infer TypeScript types from Zod schemas
type User = z.infer<typeof UserSchema>;
type CreateUser = z.infer<typeof CreateUserSchema>;

// Validation middleware using Zod
const validate = <T>(schema: z.ZodSchema<T>) => {
  return (req: express.Request, res: express.Response, next: express.NextFunction): void => {
    const result = schema.safeParse(req.body);
    
    if (!result.success) {
      res.status(400).json({
        error: 'Validation failed',
        details: result.error.issues.map(issue => ({
          path: issue.path.join('.'),
          message: issue.message,
        })),
      });
      return;
    }
    
    // Attach validated and typed data to request
    req.body = result.data;
    next();
  };
};

// Routes with Zod validation
app.post('/users', validate(CreateUserSchema), (req, res) => {
  // req.body is now typed as CreateUser thanks to the validate middleware
  const { email, name, password } = req.body;
  
  // Create user logic
  const user: User = {
    id: crypto.randomUUID(),
    email,
    name,
    createdAt: new Date().toISOString(),
  };
  
  res.status(201).json(user);
});

app.get('/users/:id', (req, res) => {
  const { id } = req.params;
  
  const user: User = {
    id,
    email: 'user@example.com',
    name: 'John Doe',
    createdAt: new Date().toISOString(),
  };
  
  res.json(user);
});

// Generate OpenAPI spec from Zod schemas
const openAPISpec = generateOpenAPISpec({
  openapi: '3.0.0',
  info: {
    title: 'User API',
    version: '1.0.0',
  },
  paths: {
    '/users': {
      post: {
        requestBody: {
          content: {
            'application/json': {
              schema: CreateUserSchema,
            },
          },
        },
        responses: {
          '201': {
            description: 'User created',
            content: {
              'application/json': {
                schema: UserSchema,
              },
            },
          },
          '400': {
            description: 'Validation error',
            content: {
              'application/json': {
                schema: ErrorSchema,
              },
            },
          },
        },
      },
    },
    '/users/{id}': {
      get: {
        parameters: [{
          name: 'id',
          in: 'path',
          required: true,
          schema: { type: 'string' },
        }],
        responses: {
          '200': {
            description: 'User found',
            content: {
              'application/json': {
                schema: UserSchema,
              },
            },
          },
        },
      },
    },
  },
});

// Serve OpenAPI spec
app.get('/openapi.json', (req, res) => {
  res.json(openAPISpec);
});

// Swagger UI endpoint (serve swagger-ui-static package)
app.use('/docs', express.static('node_modules/swagger-ui-static'));

export default app;
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 5 | `import { z } from 'zod'` | Imports Zod library for schema definition |
| 8 | `const UserSchema = z.object({...})` | Defines user object schema with field validations |
| 9 | `z.string().uuid()` | Validates string is a valid UUID format |
| 10 | `z.string().email()` | Validates string is valid email format |
| 14 | `z.string().min(1)` | Validates string has at least 1 character |
| 24 | `type User = z.infer<typeof UserSchema>` | Infers TypeScript type from Zod schema automatically |
| 29 | `const validate = <T>(schema: z.ZodSchema<T>)` | Generic validation middleware factory |
| 31 | `schema.safeParse(req.body)` | Validates data without throwing exceptions |
| 43 | `req.body = result.data` | Replaces body with validated (and typed) data |
| 59 | `generateOpenAPISpec({...})` | Converts Zod schemas to OpenAPI specification |

## ⚠️ Common Mistakes

1. **Forgetting to use safeParse**: Using parse() instead of safeParse() will throw exceptions on validation failure, breaking Express error handling
2. **Not updating req.body type**: After validation, TypeScript still thinks req.body is `any` unless you explicitly type the middleware or cast it
3. **Missing .optional() on optional fields**: Required fields without optional() will fail validation when missing in request

## ✅ Quick Recap

- Zod provides runtime validation with TypeScript type inference
- Use `z.infer<typeof Schema>` to get TypeScript types from Zod schemas
- Create validation middleware that attaches validated data to req.body
- `zod-to-openapi` generates OpenAPI specs from Zod schemas automatically
- Define schemas once, get validation, types, and documentation

## 🔗 What's Next

Explore debugging Express applications with VS Code for better developer experience.
