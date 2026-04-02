# Deno Deploy

## What You'll Learn

- How to deploy to Deno Deploy
- How Deno Deploy's edge network works
- How to use Deno KV

## Setup

```bash
# Install deployctl
deno install -gArf jsr:@deno/deployctl

# Login
deployctl auth login

# Deploy
deployctl deploy --project=my-app main.ts
```

## Basic Deployment

```ts
// main.ts

Deno.serve((req) => {
  return new Response('Hello from Deno Deploy!', {
    headers: { 'Content-Type': 'text/plain' },
  });
});
```

## Deno KV (Global Key-Value Store)

```ts
// Deno KV is a globally distributed key-value store

const kv = await Deno.openKv();

// Set
await kv.set(['users', '1'], { name: 'Alice', email: 'alice@example.com' });

// Get
const user = await kv.get(['users', '1']);
console.log(user.value);  // { name: 'Alice', ... }

// List
const users = await kv.list({ prefix: ['users'] });
for await (const entry of users) {
  console.log(entry.key, entry.value);
}
```

## Next Steps

For Deno vs Node.js, continue to [Deno vs Node](./03-deno-vs-node.md).
