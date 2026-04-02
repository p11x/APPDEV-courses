# Clerk Backend Auth

## What You'll Learn

- How to verify Clerk tokens on the backend
- How to protect API routes
- How to use Clerk with Express/Fastify
- How to access user data in API routes

## API Route Protection

```ts
// app/api/protected/route.ts

import { auth } from '@clerk/nextjs/server';
import { NextResponse } from 'next/server';

export async function GET() {
  const { userId } = await auth();

  if (!userId) {
    return NextResponse.json({ error: 'Unauthorized' }, { status: 401 });
  }

  return NextResponse.json({ userId, message: 'Protected data' });
}
```

## Clerk with Express

```ts
// express-server.ts

import express from 'express';
import { ClerkExpressRequireAuth } from '@clerk/clerk-sdk-node';

const app = express();

// Public routes
app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});

// Protected routes
app.use('/api', ClerkExpressRequireAuth());

app.get('/api/profile', (req, res) => {
  res.json({ userId: req.auth.userId });
});

app.listen(3000);
```

## Webhooks

```ts
// app/api/webhooks/clerk/route.ts

import { Webhook } from 'svix';
import { headers } from 'next/headers';
import { NextResponse } from 'next/server';

export async function POST(req: Request) {
  const headerPayload = await headers();
  const svixId = headerPayload.get('svix-id');
  const svixTimestamp = headerPayload.get('svix-timestamp');
  const svixSignature = headerPayload.get('svix-signature');

  const body = await req.text();

  const wh = new Webhook(process.env.CLERK_WEBHOOK_SECRET!);

  let event;
  try {
    event = wh.verify(body, {
      'svix-id': svixId!,
      'svix-timestamp': svixTimestamp!,
      'svix-signature': svixSignature!,
    });
  } catch (err) {
    return NextResponse.json({ error: 'Invalid signature' }, { status: 400 });
  }

  const eventType = (event as any).type;

  switch (eventType) {
    case 'user.created':
      // Handle new user
      console.log('New user:', (event as any).data);
      break;
    case 'user.updated':
      // Handle user update
      break;
    case 'user.deleted':
      // Handle user deletion
      break;
  }

  return NextResponse.json({ received: true });
}
```

## Next Steps

For security, continue to [Clerk Security](./04-clerk-security.md).
