# Fresh Setup

## What You'll Learn

- What Fresh is and how it works
- How to create a Fresh project
- How Fresh's island architecture works
- How to define routes and pages

## What Is Fresh?

Fresh is a **Deno-native web framework** with zero JavaScript shipped to the client by default. It uses "islands architecture" — only interactive components (islands) ship JavaScript, while the rest is pure server-rendered HTML.

| Feature | Next.js | Fresh |
|---------|---------|-------|
| Runtime | Node.js | Deno |
| JS shipped by default | Yes (bundle) | Zero (islands only) |
| Hydration | Full page | Per-island |
| Routing | File-based | File-based |
| TypeScript | Yes | Yes (Deno native) |
| Deployment | Vercel, Docker | Deno Deploy |

## Setup

```bash
# Install Deno (if not already installed)
curl -fsSL https://deno.land/install.sh | sh

# Create Fresh project
deno run -A -r https://fresh.deno.dev my-fresh-app
cd my-fresh-app

# Start development server
deno task start
# Opens at http://localhost:8000
```

## Project Structure

```
my-fresh-app/
├── routes/              # File-based routing
│   ├── index.tsx        → GET /
│   ├── about.tsx        → GET /about
│   └── api/
│       └── hello.ts     → GET /api/hello
├── islands/             # Interactive components (ship JS)
│   └── Counter.tsx
├── components/          # Server-rendered components (no JS)
│   └── Header.tsx
├── static/              # Static assets
├── deno.json            → Deno configuration
├── fresh.gen.ts         → Auto-generated routes
└── main.ts              → Entry point
```

## Basic Page

```tsx
// routes/index.tsx

import { Head } from '$fresh/runtime.ts';

export default function Home() {
  return (
    <>
      <Head>
        <title>Fresh App</title>
      </Head>
      <div>
        <h1>Welcome to Fresh!</h1>
        <p>This page ships zero JavaScript.</p>
        <a href="/about">About</a>
      </div>
    </>
  );
}
```

## Island (Interactive Component)

```tsx
// islands/Counter.tsx

import { useState } from 'preact/hooks';

// Islands are the only components that ship JavaScript to the client
export default function Counter() {
  const [count, setCount] = useState(0);

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(count + 1)}>Increment</button>
    </div>
  );
}
```

```tsx
// routes/index.tsx — Use the island

import Counter from '../islands/Counter.tsx';

export default function Home() {
  return (
    <div>
      <h1>Fresh Counter</h1>
      {/* This island ships JS; the rest of the page doesn't */}
      <Counter />
    </div>
  );
}
```

## API Routes

```ts
// routes/api/hello.ts

import { Handlers } from '$fresh/server.ts';

export const handler: Handlers = {
  GET(req) {
    const url = new URL(req.url);
    const name = url.searchParams.get('name') || 'World';

    return new Response(JSON.stringify({ message: `Hello, ${name}!` }), {
      headers: { 'Content-Type': 'application/json' },
    });
  },
};
```

## Data Loading

```tsx
// routes/users.tsx

import { Handlers, PageProps } from '$fresh/server.ts';

interface User {
  id: number;
  name: string;
}

export const handler: Handlers<User[]> = {
  async GET(req, ctx) {
    // Load data on the server
    const users: User[] = [
      { id: 1, name: 'Alice' },
      { id: 2, name: 'Bob' },
    ];

    return ctx.render(users);
  },
};

export default function UsersPage({ data }: PageProps<User[]>) {
  return (
    <div>
      <h1>Users</h1>
      <ul>
        {data.map((user) => (
          <li key={user.id}>{user.name}</li>
        ))}
      </ul>
    </div>
  );
}
```

## Common Mistakes

### Mistake 1: Putting Server Code in Islands

```tsx
// WRONG — islands run on the client, no Deno APIs available
// islands/DataLoader.tsx
const data = await Deno.readTextFile('./data.json');  // Crashes in browser

// CORRECT — load data in routes, pass as props
export const handler: Handlers = {
  async GET(req, ctx) {
    const data = await Deno.readTextFile('./data.json');
    return ctx.render(JSON.parse(data));
  },
};
```

### Mistake 2: Adding State to Components

```tsx
// WRONG — non-island components don't ship JS, useState won't work
// components/Counter.tsx (in components/, not islands/)
import { useState } from 'preact/hooks';
export default function Counter() {
  const [count, setCount] = useState(0);  // Does not work!
  return <button onClick={() => setCount(count + 1)}>{count}</button>;
}

// CORRECT — put interactive components in islands/
// islands/Counter.tsx
import { useState } from 'preact/hooks';
export default function Counter() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(count + 1)}>{count}</button>;
}
```

## Next Steps

For components, continue to [Fresh Components](./02-fresh-components.md).
