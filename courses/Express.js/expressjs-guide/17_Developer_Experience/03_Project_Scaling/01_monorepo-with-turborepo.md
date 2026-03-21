# Monorepo with Turborepo

## 📌 What You'll Learn

- Setting up Turborepo
- Shared packages in monorepo
- Building Express API with frontend

## 🧠 Concept Explained (Plain English)

A monorepo (monolithic repository) is a single repository that contains multiple projects. Instead of having separate repositories for your Express API, React frontend, and shared code, you keep everything in one place. This makes sharing code between projects easier and ensures everyone uses the same versions of libraries.

Turborepo is a build system that speeds up monorepos by caching build outputs and running tasks in parallel. It knows which projects depend on which and only rebuilds what changed. For example, if you update a shared utility, Turborepo knows which packages need to be rebuilt.

## Directory Structure

```
my-monorepo/
├── apps/
│   ├── web/           # Next.js or React app
│   ├── api/           # Express API
│   └── admin/         # Admin dashboard
├── packages/
│   ├── ui/            # Shared UI components
│   ├── config/       # Shared ESLint, TypeScript config
│   ├── database/     # Database utilities
│   └── utils/        # Shared utilities
├── turbo.json        # Turborepo configuration
├── package.json      # Root package.json
└── pnpm-workspace.yaml  # Workspace configuration
```

## 💻 Code Example

```js
// Root package.json
{
  "name": "my-monorepo",
  "private": true,
  "workspaces": [
    "apps/*",
    "packages/*"
  ],
  "scripts": {
    "build": "turbo run build",
    "dev": "turbo run dev",
    "lint": "turbo run lint",
    "test": "turbo run test",
    "clean": "turbo run clean"
  },
  "devDependencies": {
    "turbo": "^2.0.0"
  }
}

// pnpm-workspace.yaml
packages:
  - 'apps/*'
  - 'packages/*'

// turbo.json
{
  "$schema": "https://turbo.build/schema.json",
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**", ".next/**"]
    },
    "dev": {
      "cache": false,
      "persistent": true
    },
    "lint": {},
    "test": {
      "dependsOn": ["build"]
    },
    "clean": {
      "cache": false
    }
  }
}
```

## Shared Package Example

```js
// packages/database/src/index.ts
import { Pool } from 'pg';

export interface DatabaseConfig {
  host: string;
  port: number;
  database: string;
  user: string;
  password: string;
}

export function createPool(config: DatabaseConfig): Pool {
  return new Pool({
    host: config.host,
    port: config.port,
    database: config.database,
    user: config.user,
    password: config.password,
    max: 20,
    idleTimeoutMillis: 30000,
    connectionTimeoutMillis: 2000,
  });
}

export type { Pool, PoolClient, QueryResult } from 'pg';
```

## Express App Using Shared Package

```js
// apps/api/package.json
{
  "name": "api",
  "version": "1.0.0",
  "dependencies": {
    "express": "^5.0.0",
    "@my-monorepo/database": "*",
    "@my-monorepo/config": "*"
  },
  "scripts": {
    "dev": "tsx watch src/index.ts",
    "build": "tsc",
    "start": "node dist/index.js"
  }
}

// apps/api/src/index.ts
import express from 'express';
import { createPool } from '@my-monorepo/database';
import { getDatabaseConfig } from '@my-monorepo/config';

const app = express();
const pool = createPool(getDatabaseConfig());

app.get('/users', async (req, res) => {
  const result = await pool.query('SELECT * FROM users');
  res.json(result.rows);
});

app.listen(3000, () => {
  console.log('API running on port 3000');
});
```

## ⚠️ Common Mistakes

1. **Not using workspace protocols**: Always use `*` or specific versions in package.json
2. **Circular dependencies**: Don't let apps depend on each other directly
3. **Forgetting turbo.json**: Without it, Turborepo won't know your build pipeline

## ✅ Quick Recap

- Monorepo keeps all projects in one repository
- Turborepo caches builds and runs tasks in parallel
- Use workspaces (npm/pnpm) to link packages
- Shared packages go in packages/, apps go in apps/
- Build pipeline defined in turbo.json

## 🔗 What's Next

Learn about contract testing with Pact for API integration testing.
