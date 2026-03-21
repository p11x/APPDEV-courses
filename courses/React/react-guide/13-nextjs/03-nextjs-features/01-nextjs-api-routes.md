# Next.js API Routes

## Overview
Next.js provides a built-in API routing system that allows you to create backend endpoints within your Next.js application. These route handlers can handle HTTP requests, process form data, connect to databases, and return JSON responses. API routes are perfect for building full-stack applications without needing a separate backend server.

## Prerequisites
- Understanding of HTTP methods and REST APIs
- Familiarity with Next.js App Router
- Basic knowledge of async/await

## Core Concepts

### Creating API Routes
API routes are created in the `app/api/` directory:

```typescript
// [File: app/api/hello/route.ts]
import { NextResponse } from 'next/server';

// GET handler
export async function GET() {
  return NextResponse.json({ 
    message: 'Hello from API!' 
  });
}

// POST handler
export async function POST(request: Request) {
  const body = await request.json();
  
  return NextResponse.json({
    received: body,
    message: 'Data received!'
  });
}
```

### Handling Different HTTP Methods
Create handlers for different HTTP methods:

```typescript
// [File: app/api/users/route.ts]
import { NextRequest, NextResponse } from 'next/server';
import { prisma } from '@/lib/prisma';

// GET - Fetch users
export async function GET(request: NextRequest) {
  const { searchParams } = new URL(request.url);
  const page = parseInt(searchParams.get('page') || '1');
  const limit = parseInt(searchParams.get('limit') || '10');
  
  const [users, total] = await Promise.all([
    prisma.user.findMany({
      skip: (page - 1) * limit,
      take: limit,
      orderBy: { createdAt: 'desc' },
    }),
    prisma.user.count(),
  ]);
  
  return NextResponse.json({
    users,
    pagination: {
      page,
      limit,
      total,
      pages: Math.ceil(total / limit),
    },
  });
}

// POST - Create new user
export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    
    // Validate required fields
    if (!body.email || !body.name) {
      return NextResponse.json(
        { error: 'Email and name are required' },
        { status: 400 }
      );
    }
    
    // Check if user exists
    const existing = await prisma.user.findUnique({
      where: { email: body.email },
    });
    
    if (existing) {
      return NextResponse.json(
        { error: 'User already exists' },
        { status: 409 }
      );
    }
    
    // Create user
    const user = await prisma.user.create({
      data: {
        name: body.name,
        email: body.email,
      },
    });
    
    return NextResponse.json(user, { status: 201 });
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to create user' },
      { status: 500 }
    );
  }
}

// PUT - Update user
export async function PUT(request: NextRequest) {
  const body = await request.json();
  const { id, ...data } = body;
  
  const user = await prisma.user.update({
    where: { id },
    data,
  });
  
  return NextResponse.json(user);
}

// DELETE - Delete user
export async function DELETE(request: NextRequest) {
  const { searchParams } = new URL(request.url);
  const id = searchParams.get('id');
  
  if (!id) {
    return NextResponse.json(
      { error: 'User ID required' },
      { status: 400 }
    );
  }
  
  await prisma.user.delete({
    where: { id },
  });
  
  return NextResponse.json({ success: true });
}
```

### Dynamic Route Parameters
Access route parameters in API routes:

```typescript
// [File: app/api/users/[id]/route.ts]
import { NextRequest, NextResponse } from 'next/server';
import { prisma } from '@/lib/prisma';

export async function GET(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  const user = await prisma.user.findUnique({
    where: { id: params.id },
  });
  
  if (!user) {
    return NextResponse.json(
      { error: 'User not found' },
      { status: 404 }
    );
  }
  
  return NextResponse.json(user);
}

export async function PUT(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  const body = await request.json();
  
  try {
    const user = await prisma.user.update({
      where: { id: params.id },
      data: body,
    });
    
    return NextResponse.json(user);
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to update user' },
      { status: 500 }
    );
  }
}

export async function DELETE(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    await prisma.user.delete({
      where: { id: params.id },
    });
    
    return NextResponse.json({ success: true });
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to delete user' },
      { status: 500 }
    );
  }
}
```

### Error Handling
Properly handle errors with appropriate status codes:

```typescript
// [File: app/api/data/route.ts]
import { NextRequest, NextResponse } from 'next/server';

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    
    // Validate input
    if (!body.name || body.name.length < 2) {
      return NextResponse.json(
        { error: 'Name must be at least 2 characters' },
        { status: 400 } // Bad Request
      );
    }
    
    if (!body.email || !body.email.includes('@')) {
      return NextResponse.json(
        { error: 'Valid email required' },
        { status: 400 }
      );
    }
    
    // Process data
    const result = await processData(body);
    
    return NextResponse.json(result, { status: 201 });
  } catch (error) {
    console.error('API Error:', error);
    
    // Different error handling based on error type
    if (error instanceof ValidationError) {
      return NextResponse.json(
        { error: error.message },
        { status: 422 } // Unprocessable Entity
      );
    }
    
    return NextResponse.json(
      { error: 'Internal server error' },
      { status: 500 }
    );
  }
}

class ValidationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'ValidationError';
  }
}

async function processData(data: any) {
  // Simulate processing
  return { id: '123', ...data };
}
```

### Request Validation with Zod
Use Zod for type-safe request validation:

```typescript
// [File: app/api/users/route.ts]
import { NextRequest, NextResponse } from 'next/server';
import { z } from 'zod';
import { prisma } from '@/lib/prisma';

const userSchema = z.object({
  name: z.string().min(2).max(100),
  email: z.string().email(),
  age: z.number().int().min(0).max(150).optional(),
});

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    
    // Validate with Zod
    const validated = userSchema.parse(body);
    
    const user = await prisma.user.create({
      data: validated,
    });
    
    return NextResponse.json(user, { status: 201 });
  } catch (error) {
    if (error instanceof z.ZodError) {
      return NextResponse.json(
        { error: 'Validation failed', details: error.errors },
        { status: 400 }
      );
    }
    
    return NextResponse.json(
      { error: 'Internal server error' },
      { status: 500 }
    );
  }
}
```

## Common Mistakes

### Mistake 1: Not Handling Async Errors
```typescript
// ❌ WRONG - Missing try/catch
export async function POST(request: Request) {
  const data = await request.json();
  const result = await processData(data); // If this throws, it crashes!
  return NextResponse.json(result);
}

// ✅ CORRECT - Always wrap in try/catch
export async function POST(request: Request) {
  try {
    const data = await request.json();
    const result = await processData(data);
    return NextResponse.json(result);
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to process request' },
      { status: 500 }
    );
  }
}
```

### Mistake 2: Not Validating Input
```typescript
// ❌ WRONG - Trusting all input
export async function POST(request: Request) {
  const body = await request.json();
  // No validation - could cause database errors or security issues
  await db.user.create({ data: body });
  return NextResponse.json({ success: true });
}

// ✅ CORRECT - Always validate input
export async function POST(request: Request) {
  const body = await request.json();
  
  if (!body.email || !body.name) {
    return NextResponse.json(
      { error: 'Email and name required' },
      { status: 400 }
    );
  }
  
  await db.user.create({ data: body });
  return NextResponse.json({ success: true });
}
```

## Real-World Example

Complete CRUD API for products:

```typescript
// [File: app/api/products/route.ts]
import { NextRequest, NextResponse } from 'next/server';
import { prisma } from '@/lib/prisma';
import { z } from 'zod';

const productSchema = z.object({
  name: z.string().min(1).max(200),
  description: z.string().optional(),
  price: z.number().positive(),
  category: z.string().optional(),
  inStock: z.boolean().default(true),
});

export async function GET(request: NextRequest) {
  const { searchParams } = new URL(request.url);
  const category = searchParams.get('category');
  const inStock = searchParams.get('inStock');
  
  const where: any = {};
  if (category) where.category = category;
  if (inStock) where.inStock = inStock === 'true';
  
  const products = await prisma.product.findMany({ where });
  
  return NextResponse.json(products);
}

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    const validated = productSchema.parse(body);
    
    const product = await prisma.product.create({
      data: validated,
    });
    
    return NextResponse.json(product, { status: 201 });
  } catch (error) {
    if (error instanceof z.ZodError) {
      return NextResponse.json(
        { error: 'Validation failed', details: error.errors },
        { status: 400 }
      );
    }
    
    return NextResponse.json(
      { error: 'Failed to create product' },
      { status: 500 }
    );
  }
}
```

```typescript
// [File: app/api/products/[id]/route.ts]
import { NextRequest, NextResponse } from 'next/server';
import { prisma } from '@/lib/prisma';
import { z } from 'zod';

const productUpdateSchema = z.object({
  name: z.string().min(1).max(200).optional(),
  description: z.string().optional(),
  price: z.number().positive().optional(),
  category: z.string().optional(),
  inStock: z.boolean().optional(),
});

export async function GET(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  const product = await prisma.product.findUnique({
    where: { id: params.id },
  });
  
  if (!product) {
    return NextResponse.json(
      { error: 'Product not found' },
      { status: 404 }
    );
  }
  
  return NextResponse.json(product);
}

export async function PUT(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    const body = await request.json();
    const validated = productUpdateSchema.parse(body);
    
    const product = await prisma.product.update({
      where: { id: params.id },
      data: validated,
    });
    
    return NextResponse.json(product);
  } catch (error) {
    if (error instanceof z.ZodError) {
      return NextResponse.json(
        { error: 'Validation failed', details: error.errors },
        { status: 400 }
      );
    }
    
    return NextResponse.json(
      { error: 'Failed to update product' },
      { status: 500 }
    );
  }
}

export async function DELETE(
  request: NextRequest,
  { params }: { params: { id: string } }
) {
  try {
    await prisma.product.delete({
      where: { id: params.id },
    });
    
    return NextResponse.json({ success: true });
  } catch (error) {
    return NextResponse.json(
      { error: 'Failed to delete product' },
      { status: 500 }
    );
  }
}
```

## Key Takeaways
- API routes are created in `app/api/` directory
- Export functions named after HTTP methods: GET, POST, PUT, DELETE, PATCH
- Use NextRequest and NextResponse for handling requests and responses
- Always validate input using Zod or similar libraries
- Handle errors with appropriate HTTP status codes
- Access dynamic route parameters via the `params` object
- API routes run on the server - can access databases and server resources

## What's Next
Continue to [Next.js Image and Font Optimization](02-nextjs-image-and-font-optimization.md) to learn about Next.js built-in optimization features.