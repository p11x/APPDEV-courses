# Hot Reload Setup

## What You'll Learn

- How to set up automatic server restart on file changes
- Using node --watch mode (Node.js 18+)
- Using nodemon for development
- File watching with chokidar

## Node.js Built-in Watch Mode (Recommended)

```bash
# Simplest approach — Node.js 18+ built-in
node --watch server.js

# Watch specific extensions
node --watch --watch-path=./src server.js
```

## nodemon Setup

```bash
npm install -D nodemon
```

```json
// package.json
{
  "scripts": {
    "dev": "nodemon server.js",
    "start": "node server.js"
  }
}
```

```json
// nodemon.json
{
  "watch": ["src", "server.js"],
  "ext": "js,json,ts",
  "ignore": ["node_modules", "dist", "*.test.js"],
  "exec": "node server.js",
  "delay": 1000
}
```

## Next Steps

Continue to [Watch Mode Config](./02-watch-mode-config.md).
