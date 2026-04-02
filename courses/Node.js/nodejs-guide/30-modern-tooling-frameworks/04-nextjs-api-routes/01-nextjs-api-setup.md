# Next.js API Setup

## What You'll Learn

- How to create API routes in Next.js App Router
- How route handlers work with the Web Standard Request/Response API
- How to structure API routes
- How to handle different HTTP methods

## Route Handlers

In Next.js App Router, API routes are defined as `route.ts` files inside the `app/` directory:

```
app/
├── api/
│   ├── users/
│   │   ├── route.ts          → GET /api/users, POST /api/users
│   │   └── [id]/
│   │       └── route.ts      → GET /api/users/:id
│   └── health/
│       └── route.ts          → GET /api/health
└── page.tsx                  → GET /
```

## Basic Route Handler

```ts
// app/api/users/route.ts

import { NextRequest, NextResponse } from 'next/server';

// GET /api/users
export async function GET(request: NextRequest) {
  const users = [
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' },
  ];

  return NextResponse.json(users);
}

// POST /api/users
export async function POST(request: NextRequest) {
  const body = await request.json();

  if (!body.name) {
    return NextResponse.json(
      { error: 'Name is required' },
      { status: 400 }
    );
  }

  const user = { id: 3, name: body.name };
  return NextResponse.json(user, { status: 201 });
}
```

## Dynamic Routes

```ts
// app/api/users/[id]/route.ts

import { NextRequest, NextResponse } from 'next/server';

interface RouteParams {
  params: Promise<{ id: string }>;
}

export async function GET(request: NextRequest, { params }: RouteParams) {
  const { id } = await params;

  // In production, fetch from database
  const user = { id: Number(id), name: 'Alice' };

  if (!user) {
    return NextResponse.json(
      { error: 'User not found' },
      { status: 404 }
    );
  }

  return NextResponse.json(user);
}

export async function PUT(request: NextRequest, { params }: RouteParams) {
  const { id } = await params;
  const body = await request.json();

  return NextResponse.json({ id: Number(id), ...body });
}

export async function DELETE(request: NextRequest, { params }: RouteParams) {
  const { id } = await params;

  return NextResponse.json({ deleted: true });
}
```

## Query Parameters

```ts
// app/api/search/route.ts

import { NextRequest, NextResponse } from 'next/server';

export async function GET(request: NextRequest) {
  const searchParams = request.nextUrl.searchParams;
  const query = searchParams.get('q');
  const page = Number(searchParams.get('page') || '1');
  const limit = Number(searchParams.get('limit') || '20');

  return NextResponse.json({
    query,
    page,
    limit,
    results: [],
  });
}
```

## Request Headers

```ts
// app/api/protected/route.ts

import { NextRequest, NextResponse } from 'next/server';

export async function GET(request: NextRequest) {
  const authHeader = request.headers.get('authorization');

  if (!authHeader?.startsWith('Bearer ')) {
    return NextResponse.json(
      { error: 'Unauthorized' },
      { status: 401 }
    );
  }

  const token = authHeader.slice(7);
  // Verify token...

  return NextResponse.json({ data: 'Protected data' });
}
```

## Common Mistakes

### Mistake 1: Using req/res Instead of Request/Response

```ts
// WRONG — Pages Router style (does not work in App Router)
export default function handler(req, res) {
  res.json({ data: 'hello' });
}

// CORRECT — App Router uses Web Standard API
export async function GET(request: NextRequest) {
  return NextResponse.json({ data: 'hello' });
}
```

### Mistake 2: Forgetting to Export Named Functions

```ts
// WRONG — default export does not work for route handlers
export default async function handler(request: NextRequest) {
  return NextResponse.json({});
}

// CORRECT — named exports for each HTTP method
export async function GET(request: NextRequest) {
  return NextResponse.json({});
}
```

## Next Steps

For middleware, continue to [Next.js Middleware](./02-nextjs-middleware.md).
