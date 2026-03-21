# Remote Caching

## What You'll Learn
- Set up remote caching with Turborepo
- Use Vercel or custom remote cache
- Share builds across team and CI

## Prerequisites
- Understanding of local caching

## Do I Need This Right Now?
Remote caching is essential for teams and CI/CD. It makes builds much faster by sharing cache between developers and CI pipelines.

## Concept Explained Simple

Remote caching is like a shared Dropbox folder instead of just local folders. When Developer A builds something, Developer B and the CI server can all use that cached result. Everyone benefits from one person's work.

## Setting Up Remote Cache

### Option 1: Vercel (Easiest)

If you deploy to Vercel, remote caching is built in:

```bash
# Already configured when using Vercel!
# Just deploy and it works

turbo run build
# Uses Vercel remote cache automatically
```

### Option 2: Custom Remote (S3, Blob, etc.)

```typescript
// .npmrc or environment
TURBO_REMOTE_CACHE_URL=https://your-cache-server.com
TURBO_TOKEN=your-token
```

## Configuring Remote Cache

```json
// turbo.json
{
  "remoteCache": {
    "enabled": true,
    "signatureKeys": {
      "aws": ["key-id:private-key"]
    }
  }
}
```

### Using Vercel KV for Remote Cache

```bash
# Install Vercel KV
npm i @vercel/kv

# Set up environment
vercel link
vercel env add KV_URL production
```

```typescript
// turbo.json
{
  "remoteCache": {
    "enabled": true
  }
}
```

## CI/CD Integration

### GitHub Actions

```yaml
# .github/workflows/ci.yml
name: CI

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        
      - name: Setup Node.js
        uses: actions/setup-node@v4
        with:
          node-version: '20'
          cache: 'npm'
          
      - name: Install pnpm
        uses: pnpm/action-setup@v2
        with:
          version: 8
          
      - name: Install dependencies
        run: pnpm install
        
      - name: Build
        run: pnpm build
        env:
          TURBO_TOKEN: ${{ secrets.TURBO_TOKEN }}
          TURBO_TEAM: ${{ vars.TURBO_TEAM }}
```

### Setting Up Turborepo Remote

1. Create Vercel account (if not already)
2. Create a new project or use existing
3. Add these secrets to GitHub:

```bash
# Get your team name from Vercel dashboard
TURBO_TEAM=your-team-name

# Generate token
# Go to Vercel → Settings → Tokens → Create Token
TURBO_TOKEN=xxxxxxxxxxxxxxxxxxxx
```

## Cache Behavior

### How Remote Cache Works

```
Developer A runs build
    ↓
Build completes
    ↓
Uploads cache to remote (Vercel)
    ↓
Developer B runs same build
    ↓
Fetches cache from remote
    ↓
Build is instant!
```

### Cache Keys

Turborepo creates unique keys based on:
- Task name
- Package name
- Dependencies
- Environment variables
- Files content

```
turbo/build/ui/9a8b7c6d5e4f3g2h1i0j
                ↑
           hash of inputs
```

## Common Mistakes

### Mistake #1: Not Setting Up Token
```bash
# Wrong: No token, no remote cache
turbo run build
# Uses only local cache
```

```bash
# Correct: Set up environment
export TURBO_TOKEN=your-token
export TURBO_TEAM=your-team

turbo run build
# Uses remote cache!
```

### Mistake #2: Cache Not Being Used in CI
```yaml
# Wrong: Missing env vars
- name: Build
  run: pnpm build
  # No TURBO_TOKEN!
```

```yaml
# Correct: Pass the env vars
- name: Build
  run: pnpm build
  env:
    TURBO_TOKEN: ${{ secrets.TURBO_TOKEN }}
    TURBO_TEAM: ${{ vars.TURBO_TEAM }}
```

### Mistake #3: Wrong Team Name
```typescript
// Wrong: Wrong team name
TURBO_TEAM=my-org  # But actual is org:my-org
```

```typescript
// Correct: Use the exact team name from Vercel
TURBO_TEAM=org:my-org
```

## Summary
- Remote caching shares builds across team and CI
- Vercel provides built-in remote caching
- Set TURBO_TOKEN and TURBO_TEAM environment variables
- Cache keys are based on task, package, and inputs
- Dramatically speeds up CI/CD pipelines
- Works across all developers automatically

## Next Steps
- [pipelines-config.md](./pipelines-config.md) — Advanced pipeline configuration
