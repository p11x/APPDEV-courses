# Deno vs Node.js

## What You'll Learn

- Key differences between Deno and Node.js
- When to choose each
- How to migrate between them

## Comparison

| Feature | Node.js | Deno |
|---------|---------|------|
| TypeScript | Needs tsc | Native |
| Package manager | npm | URLs / JSR |
| Security | All permissions | Explicit permissions |
| node_modules | Yes | No |
| Standard library | No | std library |
| Web APIs | Partial | Full |
| Package registry | npm | JSR |

## Deno Compatibility with Node.js

```ts
// Deno can run most Node.js code with the --compat flag
deno run --compat --allow-all server.ts

// Or use npm: specifier
import express from 'npm:express@4';
```

## Next Steps

For modules, continue to [Deno Modules](./04-deno-modules.md).
