# Pipeline Configuration

## What You'll Learn
- Configure Turborepo pipelines
- Define complex workflows
- Optimize task orchestration

## Prerequisites
- Understanding of caching

## Do I Need This Right Now?
Pipeline configuration is essential for controlling how tasks run. Understanding this helps you optimize builds and create proper workflows.

## Concept Explained Simply

A pipeline is like a recipe for cooking. It defines:
- What ingredients are needed first (dependencies)
- What gets made (tasks)
- What the output looks like (outputs)
- How long it takes to make (caching)

## Complete Pipeline Examples

### Basic Pipeline

```json
// turbo.json
{
  "$schema": "https://turbo.build/schema.json",
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": [".next/**"]
    },
    "lint": {},
    "test": {
      "dependsOn": ["build"]
    },
    "dev": {
      "cache": false,
      "persistent": true
    }
  }
}
```

### Full-Featured Pipeline

```json
// turbo.json
{
  "$schema": "https://turbo.build/schema.json",
  "globalDependencies": [
    "**/.env.*local"
  ],
  "pipeline": {
    // Build pipeline - builds all apps and packages
    "build": {
      "dependsOn": ["^build"],
      "outputs": [
        "dist/**",
        ".next/**",
        "!.next/cache/**",
        "build/**"
      ],
      "env": [
        "NEXT_PUBLIC_API_URL",
        "APP_VERSION"
      ],
      "inputs": [
        "src/**",
        "package.json",
        "tsconfig.json"
      ]
    },
    
    // Test pipeline - runs tests
    "test": {
      "dependsOn": ["build"],
      "outputs": [
        "coverage/**"
      ],
      "inputs": [
        "src/**/*.test.ts",
        "src/**/*.test.tsx",
        "jest.config.*",
        "package.json"
      ]
    },
    
    // Lint pipeline - runs ESLint
    "lint": {
      "outputs": [],
      "inputs": [
        "src/**",
        "eslint.config.*",
        ".eslintrc*"
      ]
    },
    
    // Type checking
    "typecheck": {
      "dependsOn": ["^build"],
      "outputs": []
    },
    
    // Dev server - no caching
    "dev": {
      "cache": false,
      "persistent": true,
      "outputs": []
    },
    
    // Clean up - no caching, no outputs
    "clean": {
      "cache": false,
      "outputs": []
    },
    
    // Deploy - depends on build, test, lint
    "deploy": {
      "dependsOn": ["build", "test", "lint"],
      "outputs": []
    }
  }
}
```

## Understanding Dependencies

### Task Dependencies (`dependsOn`)

```typescript
// "^build" means "wait for dependencies to build first"
{
  "build": {
    "dependsOn": ["^build"]
  }
}

// ^ = parent dependencies
// No prefix = same workspace
```

### Dependency Graph

```
package-a (no deps)
    ↓
package-b (depends on a)
    ↓  
web-app (depends on a and b)

Task order:
1. package-a:build (runs first - no deps)
2. package-b:build (runs after a completes)
3. web-app:build (runs after a and b complete)
```

## Environment Variables

```json
{
  "pipeline": {
    "build": {
      "env": [
        "API_URL",          // Include in cache key
        "NEXT_PUBLIC_*"     // Wildcard supported
      ]
    }
  }
}
```

```bash
# If these change, cache is invalidated
API_URL=https://api.example.com
NEXT_PUBLIC_APP_NAME=MyApp
```

## Inputs

```json
{
  "pipeline": {
    "test": {
      "inputs": [
        "src/**/*.test.ts",
        "src/**/*.test.tsx",
        "jest.config.*",
        "package.json"
      ]
    }
  }
}
```

Only changes to these files will invalidate the cache.

## Parallel vs Sequential

### Parallel (when possible)

```
ui:build
utils:build
config:build
    ↓
All run in parallel!
web:build
```

### Sequential (when needed)

```
web:test
    ↓
waits for web:build
```

```json
{
  "test": {
    "dependsOn": ["build"]
  }
}
```

## Common Mistakes

### Mistake #1: Circular Dependencies
```typescript
// Wrong: A depends on B, B depends on A!
{
  "build": {
    "dependsOn": ["^build", "test"]
  },
  "test": {
    "dependsOn": ["build"]
  }
}
```

```typescript
// Correct: Build depends on build, test depends on build
{
  "build": {
    "dependsOn": ["^build"]
  },
  "test": {
    "dependsOn": ["build"]
  }
}
```

### Mistake #2: Missing Dependencies
```typescript
// Wrong: Might fail because ui isn't built yet
{
  "web:build": {
    "dependsOn": []  // Missing!
  }
}
```

```typescript
// Correct: Wait for dependencies to build
{
  "web:build": {
    "dependsOn": ["^build"]
  }
}
```

### Mistake #3: Too Many Outputs
```typescript
// Wrong: Caches too much junk
{
  "build": {
    "outputs": ["**/*"]  // Everything!
  }
}
```

```typescript
// Correct: Only cache actual outputs
{
  "build": {
    "outputs": [".next/**", "dist/**"]
  }
}
```

## Summary
- Define pipeline in turbo.json
- Use `dependsOn` to control task order
- Use `outputs` to mark what to cache
- Use `env` to include env vars in cache key
- Use `inputs` to specify what matters for cache
- Don't create circular dependencies
- Keep pipelines simple and clear

## Next Steps
- [installing-sentry.md](../20-error-monitoring/01-sentry-setup/installing-sentry.md) — Error monitoring setup
