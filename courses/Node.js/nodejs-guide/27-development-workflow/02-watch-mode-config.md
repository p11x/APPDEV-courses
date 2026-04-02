# Watch Mode Configuration

## What You'll Learn

- Configuring file watchers for different scenarios
- Watching TypeScript files
- Selective file watching

## Configuration

```json
// nodemon.json
{
  "watch": ["src"],
  "ext": "js,mjs,json",
  "ignore": ["node_modules", "dist", "*.test.js", "coverage"],
  "exec": "node --enable-source-maps src/server.js",
  "delay": 500,
  "env": {
    "NODE_ENV": "development"
  }
}
```

## Next Steps

Continue to [Pre-Commit Hooks](./03-pre-commit-hooks.md).
