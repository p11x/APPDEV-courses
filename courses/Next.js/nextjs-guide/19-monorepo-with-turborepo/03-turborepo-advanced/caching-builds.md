# Caching Builds

## What You'll Learn
- Configure Turborepo caching
- Understand cache behavior
- Optimize build performance

## Prerequisites
- Understanding of Turborepo basics

## Do I Need This Right Now?
Caching is what makes Turborepo powerful. Understanding how it works helps you optimize builds and avoid common issues.

## Concept Explained Simply

Caching is like taking photos of your work at the end of each day. When you come back tomorrow and nothing has changed, you can just look at the photos (cache) instead of doing everything again. If something changed, you do the work again and take a new photo.

## Local Caching

Turborepo caches builds locally in `.turbo/` folder:

```
.turbo/
├── cache/
│   ├── build/
│   │   └── <hash>/
│   │       ├── cache/
│   │       └── outputs/
│   └── <task>-<hash>/
│       └── <cached output>
```

### Configuring Cache

```json
// turbo.json
{
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**", ".next/**", "!.next/cache/**"],
      "cache": true  // This is the default
    },
    "dev": {
      "cache": false,  // Don't cache dev server
      "persistent": true
    },
    "lint": {
      "cache": true,
      "outputs": []  // Lint doesn't produce outputs
    }
  }
}
```

### Outputs Configuration

```json
// turbo.json
{
  "pipeline": {
    "build": {
      "outputs": [
        ".next/**",           // Next.js build output
        "!.next/cache/**",   // Exclude cache
        "dist/**",           // General dist folder
        "build/**",           // Other build outputs
        "coverage/**"         // Test coverage
      ]
    }
  }
}
```

## How Caching Works

### Cache Hit

```bash
$ turbo run build

# Task #1 - ui:build (cached)
ui:build → CACHED  (1.2s)

# Task #2 - web:build
# Actually runs because ui changed
web:build → COMPLETED (45.3s)

# Cache saved: 1 task
```

### Cache Miss

```bash
$ turbo run build

# No cache found - runs everything
ui:build → COMPLETED (23.4s)
web:build → COMPLETED (45.3s)

# Cache stored for next time
```

### Visualizing Cache

```bash
# See what's cached
turbo run build --dry

# See cache status for each task
turbo run build --dry-run=json | jq '.tasks[] | {task: .taskId, cache: .cache}'

# Force ignore cache
turbo run build --force
```

## Clearing Cache

```bash
# Clear local cache
rm -rf .turbo

# Clear and rebuild
turbo run build --force

# In CI, clear cache if needed
```

## Performance Tips

### 1. Minimize Dependencies

```typescript
// Wrong: Build has unnecessary dependencies
{
  "build": {
    "dependsOn": ["^build", "lint", "test"]  // Too many!
  }
}
```

```typescript
// Correct: Only what's needed
{
  "build": {
    "dependsOn": ["^build"]  // Only build dependencies
  }
}
```

### 2. Exclude Cached Folders

```typescript
// Wrong: Caches too much
{
  "build": {
    "outputs": [".next/**"]  // Includes cache!
  }
}
```

```typescript
// Correct: Exclude cache folder
{
  "build": {
    "outputs": [".next/**", "!.next/cache/**"]
  }
}
```

### 3. Use Persistent for Dev

```typescript
// Wrong: Caching breaks hot reload
{
  "dev": {
    "cache": true  // Bad!
  }
}
```

```typescript
// Correct: Don't cache dev
{
  "dev": {
    "cache": false,
    "persistent": true
  }
}
```

## Common Mistakes

### Mistake #1: Forgetting Outputs
```typescript
// Wrong: Nothing is cached
{
  "build": {
    // No outputs defined!
  }
}
```

```typescript
// Correct: Define outputs to cache
{
  "build": {
    "outputs": [".next/**"]
  }
}
```

### Mistake #2: Caching Dev Server
```typescript
// Wrong: Will cache stale dev server
{
  "dev": {
    "cache": true
  }
}
```

```typescript
// Correct: Don't cache dev
{
  "dev": {
    "cache": false,
    "persistent": true
  }
}
```

### Mistake #3: Too Many Dependencies
```typescript
// Wrong: Slow builds
{
  "build": {
    "dependsOn": ["^build", "lint", "test", "typecheck"]
  }
}
```

```typescript
// Correct: Only build dependencies
{
  "build": {
    "dependsOn": ["^build"]
  }
}
```

## Summary
- Turborepo caches build outputs locally in `.turbo/`
- Use `outputs` to specify what to cache
- Use `dependsOn` to define task order
- Don't cache dev server (use `cache: false`)
- Exclude cache folders from outputs
- Use `--dry-run` to see what would happen

## Next Steps
- [remote-caching.md](./remote-caching.md) — Share builds across team
