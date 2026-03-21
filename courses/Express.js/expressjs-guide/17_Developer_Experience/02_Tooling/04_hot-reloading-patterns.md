# Hot Reloading Patterns

## 📌 What You'll Learn

- tsx for fast TypeScript development
- nodemon alternatives
- Module caching pitfalls

## 🧠 Concept Explained (Plain English)

Hot reloading means automatically restarting your server or refreshing your application when code changes are detected. For Express development, this is essential - you want to see your changes immediately without manually stopping and starting the server.

Node.js caches modules when they're first imported. This is normally good for performance but can cause issues during development - if you change a file, Node won't see the changes until you restart. Tools like tsx and nodemon solve this by detecting file changes and restarting the process.

tsx is a modern alternative to ts-node that's significantly faster and has better TypeScript support. It compiles TypeScript on the fly and has native ESM support.

## 💻 Code Example

```js
// Installation
// npm install -D tsx nodemon

// Development scripts in package.json
{
  "scripts": {
    "dev": "tsx watch src/server.ts",
    "dev:nodemon": "nodemon --exec tsx src/server.ts",
    "start": "node dist/server.js",
    "build": "tsc"
  }
}

// Using tsx with watch mode
// npx tsx watch src/server.ts

// nodemon.json configuration
{
  "watch": ["src"],
  "ext": "ts,js,json",
  "ignore": ["src/**/*.test.ts", "src/**/*.spec.ts"],
  "exec": "tsx src/server.ts",
  "env": {
    "NODE_ENV": "development"
  }
}

// tsx is much faster than ts-node for:
// - Initial startup
// - Type checking
// - ESM support
// - JSON imports
```

## Module Caching Issues

```js
// Problem: When using watch mode, modules get cached
// Solution: Clear require cache for specific modules

// cache-helper.ts - Utility to clear module cache
export function clearCache(modulePath: string): void {
  delete require.cache[require.resolve(modulePath)];
}

export function clearAllCache(patterns: string[]): void {
  Object.keys(require.cache).forEach(key => {
    if (patterns.some(pattern => key.includes(pattern))) {
      delete require.cache[key];
    }
  });
}

// In development, you might need to:
// - Use dynamic imports instead of require()
// - Clear cache for database connections
// - Re-initialize singletons
```

## Fast Restart Strategies

```js
// 1. Separate app and server
// src/app.ts - Express app (loads on each restart)
import express from 'express';

export function createApp() {
  const app = express();
  app.use(express.json());
  
  app.get('/health', (req, res) => {
    res.json({ status: 'ok' });
  });
  
  return app;
}

// src/server.ts - Server entry point
import { createApp } from './app';

const app = createApp();
const PORT = process.env.PORT || 3000;

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});

// 2. Use environment-based initialization
// Only connect to DB in production, not during every restart
if (process.env.NODE_ENV === 'production') {
  await connectToDatabase();
}

// 3. Lazy load heavy modules
// src/services/heavy-service.ts
let _heavyService: HeavyService | null = null;

export async function getHeavyService() {
  if (!_heavyService) {
    console.log('Loading heavy service...');
    _heavyService = await import('./HeavyService');
  }
  return _heavyService;
}
```

## Production Considerations

```js
// For production, build and run compiled JavaScript
// package.json
{
  "scripts": {
    "build": "tsc",
    "start": "node dist/server.js",
    "prod": "npm run build && npm start"
  }
}

// tsconfig.json for production
{
  "compilerOptions": {
    "module": "CommonJS",
    "target": "ES2022",
    "outDir": "dist",
    "rootDir": "src"
  }
}
```

## ⚠️ Common Mistakes

1. **Using ts-node in production**: Never use ts-node in production - it's slow and for development only
2. **Ignoring cache issues**: Forgetting that Node caches modules can cause confusing bugs
3. **Watching too many files**: Watch only src directory, not node_modules or dist

## ✅ Quick Recap

- Use `tsx watch` for fast TypeScript development
- tsx is faster and has better ESM support than ts-node
- Separate app creation from server startup
- Clear module cache for files that change frequently
- Build for production, watch for development

## 🔗 What's Next

Learn about monorepo setup with Turborepo for scaling your projects.
