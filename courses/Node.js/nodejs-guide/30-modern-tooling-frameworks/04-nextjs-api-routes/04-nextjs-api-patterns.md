# Next.js API Patterns

## What You'll Learn

- RESTful API route patterns
- File upload handling
- WebSocket integration
- Background job triggering

## RESTful Route Group

```
app/api/
├── users/
│   ├── route.ts          → GET (list), POST (create)
│   └── [id]/
│       ├── route.ts      → GET (read), PUT (update), DELETE
│       └── posts/
│           └── route.ts  → GET (user's posts)
├── posts/
│   ├── route.ts          → GET (list), POST (create)
│   └── [id]/
│       └── route.ts      → GET, PUT, DELETE
└── health/
    └── route.ts          → GET (health check)
```

## File Upload

```ts
// app/api/upload/route.ts

import { NextRequest, NextResponse } from 'next/server';

export async function POST(request: NextRequest) {
  const formData = await request.formData();
  const file = formData.get('file') as File;

  if (!file) {
    return NextResponse.json({ error: 'No file' }, { status: 400 });
  }

  // Convert to buffer
  const buffer = Buffer.from(await file.arrayBuffer());

  // Save to disk or upload to S3
  // await writeFile(`./uploads/${file.name}`, buffer);

  return NextResponse.json({
    name: file.name,
    size: file.size,
    type: file.type,
  });
}
```

## Webhook Verification

```ts
// app/api/webhooks/stripe/route.ts

import { NextRequest, NextResponse } from 'next/server';
import crypto from 'node:crypto';

export async function POST(request: NextRequest) {
  const body = await request.text();
  const signature = request.headers.get('stripe-signature')!;

  // Verify webhook signature
  const expected = crypto
    .createHmac('sha256', process.env.STRIPE_WEBHOOK_SECRET!)
    .update(body)
    .digest('hex');

  if (signature !== expected) {
    return NextResponse.json({ error: 'Invalid signature' }, { status: 401 });
  }

  const event = JSON.parse(body);

  switch (event.type) {
    case 'payment_intent.succeeded':
      await handlePaymentSuccess(event.data);
      break;
    case 'payment_intent.failed':
      await handlePaymentFailure(event.data);
      break;
  }

  return NextResponse.json({ received: true });
}
```

## Health Check

```ts
// app/api/health/route.ts

import { NextResponse } from 'next/server';

export async function GET() {
  const checks = {
    status: 'healthy',
    timestamp: new Date().toISOString(),
    uptime: process.uptime(),
    memory: process.memoryUsage(),
    version: process.env.npm_package_version,
  };

  return NextResponse.json(checks);
}
```

## Next Steps

For tRPC, continue to [tRPC Setup](../05-trpc-rpc/01-trpc-setup.md).
