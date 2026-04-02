# Deno Modules

## What You'll Learn

- How Deno's module system works
- How to use JSR (JavaScript Registry)
- How to import npm packages in Deno
- How to create and publish Deno modules

## URL Imports

```ts
// Import from deno.land
import { serve } from 'https://deno.land/std@0.220.0/http/server.ts';

// Import from JSR
import { Hono } from 'jsr:@hono/hono';

// Import from npm
import { z } from 'npm:zod@3';
```

## JSR (JavaScript Registry)

```bash
# Install from JSR
deno add jsr:@std/path
deno add jsr:@hono/hono
```

```ts
// Use the import
import { join } from '@std/path';
import { Hono } from '@hono/hono';
```

## Publishing to JSR

```json
// deno.json
{
  "name": "@myorg/my-package",
  "version": "1.0.0",
  "exports": "./mod.ts"
}
```

```bash
# Publish
deno publish
```

## Next Steps

For edge caching, continue to [Cache Strategies](../05-edge-caching/01-cache-strategies.md).
