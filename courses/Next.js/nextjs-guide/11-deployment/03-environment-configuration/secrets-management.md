# Secrets Management

## What You'll Learn
- Best practices for storing sensitive information
- How to manage secrets across different environments
- Tools and strategies for production secret rotation

## Prerequisites
- Understanding of environment variables
- Knowledge of client vs server environment variables
- Basic deployment concepts

## Concept Explained Simply

Think of secrets management like managing keys to a safe:
- You don't write combinations on sticky notes stuck to the safe
- You don't share the combination with everyone who asks
- You change the combination periodically
- You have a system to track who has access

In web development, secrets are things like API keys, database passwords, and encryption keys. If hackers get these, they can access your data, use your services for free, or steal user information. Good secrets management keeps your application secure even if someone gets access to your code repository.

The key principles are: never commit secrets to git, use environment-specific values, rotate secrets regularly, and use secret management services for production applications.

## Complete Code Example

### Basic Secrets Setup

```typescript
// .env.local (add to .gitignore!)
DATABASE_URL="postgres://user:password@localhost:5432/mydb"
STRIPE_SECRET_KEY="sk_test_12345"
JWT_SECRET="your-jwt-secret-change-in-production"
```

```typescript
// .env.production
DATABASE_URL="postgres://prod-user:prod-pass@aws RDS endpoint"
STRIPE_SECRET_KEY="sk_live_12345"
JWT_SECRET="very-long-random-string-at-least-32-chars"
```

```typescript
// lib/db.ts - Using secrets safely
import { PrismaClient } from "@prisma/client";

const globalForPrisma = globalThis as unknown as {
  prisma: PrismaClient | undefined;
};

export const db = globalForPrisma.prisma ?? new PrismaClient();

if (process.env.NODE_ENV !== "production") globalForPrisma.prisma = db;
```

```typescript
// Server Action using secrets securely
"use server";

import { db } from "@/lib/db";
import { stripe } from "@/lib/stripe";

export async function createSubscription(userId: string, priceId: string) {
  // All secrets stay on server - safe from client exposure
  const user = await db.user.findUnique({
    where: { id: userId },
  });

  if (!user) {
    throw new Error("User not found");
  }

  const session = await stripe.checkout.sessions.create({
    customer_email: user.email,
    line_items: [{ price: priceId, quantity: 1 }],
    mode: "subscription",
  });

  return { url: session.url };
}
```

### Using a Secrets Manager (Production)

```typescript
// lib/secrets.ts - Example with AWS Secrets Manager
import { SecretsManagerClient, GetSecretValueCommand } from "@aws-sdk/client-secrets-manager";

const client = new SecretsManagerClient({ region: "us-east-1" });

export async function getSecret(secretName: string): Promise<string> {
  const response = await client.send(
    new GetSecretValueCommand({ SecretId: secretName })
  );
  
  if (response.SecretString) {
    return response.SecretString;
  }
  
  throw new Error("Secret not found");
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `.gitignore` | Prevents committing secrets to git | Ensures secrets never enter version control |
| `process.env.JWT_SECRET` | Accesses secret at runtime | Secrets loaded from environment, not hardcoded |
| `"use server"` | Marks function as server-only | Prevents client from directly accessing secret-handling logic |
| `globalForPrisma` | Caches Prisma client globally | Prevents creating multiple connections in development |

## Common Mistakes

### Mistake 1: Committing Secrets to Git

```typescript
// WRONG - Secret in source code
const API_KEY = "sk_live_123456789";

// CORRECT - Use environment variable
const API_KEY = process.env.API_KEY;
// Set the actual value in .env.local, never commit this file
```

### Mistake 2: Using Same Secrets Everywhere

```typescript
// WRONG - Production secret in development
DATABASE_URL="postgres://localhost:5432/devdb"  // Works everywhere by accident

// CORRECT - Environment-specific files
# .env.development - dev values
# .env.production - production values  
# These files should be different!
```

### Mistake 3: Hardcoding Fallback Secrets

```typescript
// WRONG - Fallback exposes secret
const dbUrl = process.env.DATABASE_URL || "postgres://admin:password@localhost";

// CORRECT - Fail if secret is missing
const dbUrl = process.env.DATABASE_URL;
if (!dbUrl) {
  throw new Error("DATABASE_URL environment variable is required");
}
```

## Summary

- Always use environment variables for secrets, never hardcode values
- Add `.env*.local` files to `.gitignore` to prevent accidental commits
- Use different secrets for development, staging, and production
- Consider using a secrets manager service for production applications
- Never expose secrets to client-side code — use Server Actions instead
- Validate that required secrets exist at startup to fail fast

## Next Steps

- [deploying-to-vercel.md](../01-vercel/deploying-to-vercel.md) - Learn how Vercel manages secrets
- [docker-setup.md](../02-self-hosting/docker-setup.md) - See how to handle secrets in Docker deployments
