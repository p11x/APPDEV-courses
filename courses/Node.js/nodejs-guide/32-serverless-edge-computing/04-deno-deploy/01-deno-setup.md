# Deno Setup

## What You'll Learn

- What Deno is and how it differs from Node.js
- How to install and use Deno
- How Deno's module system works
- How to configure Deno

## What Is Deno?

Deno is a secure JavaScript/TypeScript runtime created by Ryan Dahl (Node.js creator). It fixes Node.js design regrets: no node_modules, secure by default, TypeScript native.

## Installation

```bash
# macOS / Linux
curl -fsSL https://deno.land/install.sh | sh

# Windows
irm https://deno.land/install.ps1 | iex

# Verify
deno --version
```

## Basic Usage

```ts
// hello.ts
console.log('Hello from Deno!');

# Run directly
deno run hello.ts

# With permissions
deno run --allow-net --allow-read server.ts
```

## Security

Deno requires explicit permissions:

```bash
# Allow network access
deno run --allow-net server.ts

# Allow file read
deno run --allow-read=readme.txt script.ts

# Allow all (not recommended)
deno run -A script.ts
```

## Module System

```ts
// Deno uses URLs (no node_modules)
import { serve } from 'https://deno.land/std@0.220.0/http/server.ts';

// Or from JSR (JavaScript Registry)
import { Hono } from 'jsr:@hono/hono';

// npm compatibility
import express from 'npm:express@4';
```

## Next Steps

For Deno Deploy, continue to [Deno Deploy](./02-deno-deploy.md).
