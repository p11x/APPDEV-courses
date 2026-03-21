# Next.js 14 App Router Production Template

## Overview
A complete production-ready starter for Next.js 14 applications using the App Router, TypeScript, Tailwind CSS, and shadcn/ui component patterns.

## Project Structure

```
my-nextjs-app/
├── app/
│   ├── (auth)/                    # Route group for auth pages
│   │   ├── login/
│   │   │   ├── page.tsx
│   │   │   └── login-form.tsx
│   │   └── register/
│   │       └── page.tsx
│   │
│   ├── (dashboard)/               # Protected dashboard routes
│   │   ├── layout.tsx            # Dashboard-specific layout
│   │   ├── page.tsx              # /dashboard
│   │   └── settings/
│   │       └── page.tsx
│   │
│   ├── api/                       # API routes
│   │   ├── auth/
│   │   │   └── [...nextauth]/
│   │   │       └── route.ts
│   │   └── todos/
│   │       └── route.ts
│   │
│   ├── admin/                     # Admin routes (protected)
│   │   └── page.tsx
│   │
│   ├── layout.tsx                 # Root layout
│   ├── page.tsx                   # Home page
│   ├── globals.css               # Global styles
│   └── not-found.tsx             # 404 page
│
├── components/
│   ├── ui/                       # shadcn/ui primitives
│   │   ├── button.tsx
│   │   ├── input.tsx
│   │   └── card.tsx
│   │
│   ├── forms/                    # Form components
│   │   ├── login-form.tsx
│   │   └── todo-form.tsx
│   │
│   ├── layout/                   # Layout components
│   │   ├── header.tsx
│   │   ├── sidebar.tsx
│   │   └── footer.tsx
│   │
│   └── providers/                # Client-side providers
│       ├── query-provider.tsx
│       └── auth-provider.tsx
│
├── lib/                          # Utilities and helpers
│   ├── db/                       # Database client
│   │   ├── client.ts             # Prisma/DB client
│   │   └── schema.ts
│   │
│   ├── auth/                     # Auth utilities
│   │   ├── config.ts             # NextAuth config
│   │   └── utils.ts
│   │
│   ├── utils/                    # General utilities
│   │   ├── cn.ts                 # classnames utility
│   │   └── formatters.ts
│   │
│   └── constants.ts
│
├── types/                        # TypeScript types
│   ├── next-auth.d.ts
│   └── index.ts
│
├── public/                       # Static assets
│
├── middleware.ts                 # Edge middleware
├── next.config.js                # Next.js config
├── tailwind.config.ts            # Tailwind config
├── tsconfig.json                 # TypeScript config
└── package.json
```

## Required Packages

```bash
# Core Next.js
npm install next react react-dom

# Database (optional - example with Prisma)
npm install @prisma/client prisma

# Auth
npm install next-auth

# Styling
npm install tailwindcss postcss autoprefixer
npm install clsx tailwind-merge class-variance-authority
npm install @radix-ui/react-slot

# Forms & Validation
npm install react-hook-form zod @hookform/resolvers

# Data Fetching
npm install @tanstack/react-query

# Utilities
npm install date-fns lucide-react
```

## Next.js Configuration

### next.config.js

```javascript
/** @type {import('next').NextConfig} */
const nextConfig = {
  // Enable React strict mode
  reactStrictMode: true,
  
  // Image optimization domains
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: '*.supabase.co',
      },
    ],
  },
  
  // TypeScript (handled automatically in most cases)
  typescript: {
    ignoreBuildErrors: false,
  },
  
  // ESLint (handled automatically)
  eslint: {
    ignoreDuringBuilds: false,
  },
};

module.exports = nextConfig;
```

### middleware.ts

```typescript
// [File: middleware.ts]
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

/**
 * Edge middleware for authentication and route protection.
 * Runs before every request to check authorization.
 */
export function middleware(request: NextRequest) {
  // Skip middleware for static files and API routes
  if (
    request.nextUrl.pathname.startsWith('/_next') ||
    request.nextUrl.pathname.startsWith('/api/auth') ||
    request.nextUrl.pathname.includes('.')
  ) {
    return NextResponse.next();
  }

  // Get the session token from cookies
  const token = request.cookies.get('next-auth.session-token');

  // Protected routes require authentication
  const isProtectedRoute = 
    request.nextUrl.pathname.startsWith('/dashboard') ||
    request.nextUrl.pathname.startsWith('/admin');

  // Redirect unauthenticated users to login
  if (isProtectedRoute && !token) {
    const loginUrl = new URL('/login', request.url);
    loginUrl.searchParams.set('callbackUrl', request.nextUrl.pathname);
    return NextResponse.redirect(loginUrl);
  }

  // Redirect authenticated users away from auth pages
  if (request.nextUrl.pathname.startsWith('/login') && token) {
    return NextResponse.redirect(new URL('/dashboard', request.url));
  }

  return NextResponse.next();
}

// Configure which routes the middleware runs on
export const config = {
  matcher: [
    '/((?!_next/static|_next/image|favicon.ico).*)',
  ],
};
```

## Annotated Code Examples

### Root Layout

```tsx
// [File: app/layout.tsx]
import type { Metadata } from 'next';
import { Inter } from 'next/font/google';
import './globals.css';
import { Providers } from '@/components/providers';

const inter = Inter({ subsets: ['latin'] });

/**
 * Root layout for the application.
 * Server Components by default — add 'use client' for client-side.
 */
export const metadata: Metadata = {
  title: {
    template: '%s | My App',
    default: 'My App',
  },
  description: 'Production-ready Next.js application',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className={inter.className}>
        <Providers>
          {children}
        </Providers>
      </body>
    </html>
  );
}
```

### Dashboard Layout (with authentication)

```tsx
// [File: app/(dashboard)/layout.tsx]
import { redirect } from 'next/navigation';
import { getServerSession } from 'next-auth';
import { Header } from '@/components/layout/header';
import { Sidebar } from '@/components/layout/sidebar';

/**
 * Dashboard layout requires authentication.
 * This runs on the server before rendering.
 */
export default async function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // Server-side auth check
  const session = await getServerSession();
  
  if (!session) {
    redirect('/login');
  }

  return (
    <div className="flex h-screen">
      <Sidebar />
      <div className="flex-1 flex flex-col overflow-hidden">
        <Header user={session.user} />
        <main className="flex-1 overflow-y-auto p-6">
          {children}
        </main>
      </div>
    </div>
  );
}
```

### Server Component Page

```tsx
// [File: app/(dashboard)/page.tsx]
import { Suspense } from 'react';
import { getCachedTodos } from '@/lib/db/todos';
import { TodoList } from '@/components/todo-list';
import { TodoSkeleton } from '@/components/skeletons';

/**
 * Dashboard page - Server Component by default.
 * Data is fetched on the server automatically.
 * 
 * This page is statically generated at build time
 * and revalidated every 60 seconds (ISR).
 */
export const revalidate = 60;

export default async function DashboardPage() {
  // Direct database call - no API needed!
  const todos = await getCachedTodos();

  return (
    <div className="space-y-6">
      <h1 className="text-3xl font-bold">Dashboard</h1>
      
      <Suspense fallback={<TodoSkeleton />}>
        <TodoList todos={todos} />
      </Suspense>
    </div>
  );
}
```

### Client Component with useRouter

```tsx
// [File: components/todo-list.tsx]
'use client';

import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { Todo } from '@/types';

/**
 * Client Component for interactive todo features.
 * Must use 'use client' directive.
 */
interface TodoListProps {
  todos: Todo[];
}

export function TodoList({ todos: initialTodos }: TodoListProps) {
  const router = useRouter();
  const [todos, setTodos] = useState(initialTodos);

  const toggleTodo = async (id: string, completed: boolean) => {
    // Optimistic update
    setTodos(todos.map(t => 
      t.id === id ? { ...t, completed: !completed } : t
    ));

    // API call
    await fetch(`/api/todos/${id}`, {
      method: 'PATCH',
      body: JSON.stringify({ completed: !completed }),
    });

    // Revalidate the page data
    router.refresh();
  };

  return (
    <ul className="space-y-2">
      {todos.map(todo => (
        <li key={todo.id} className="flex items-center gap-2">
          <input
            type="checkbox"
            checked={todo.completed}
            onChange={() => toggleTodo(todo.id, todo.completed)}
            className="w-5 h-5"
          />
          <span className={todo.completed ? 'line-through' : ''}>
            {todo.title}
          </span>
        </li>
      ))}
    </ul>
  );
}
```

### API Route Handler

```typescript
// [File: app/api/todos/route.ts]
import { NextResponse } from 'next/server';
import { getServerSession } from 'next-auth';
import { db } from '@/lib/db/client';

/**
 * GET /api/todos
 * Fetch all todos for the authenticated user.
 */
export async function GET() {
  const session = await getServerSession();
  
  if (!session) {
    return NextResponse.json({ error: 'Unauthorized' }, { status: 401 });
  }

  const todos = await db.todo.findMany({
    where: { userId: session.user.id },
    orderBy: { createdAt: 'desc' },
  });

  return NextResponse.json(todos);
}

/**
 * POST /api/todos
 * Create a new todo.
 */
export async function POST(request: Request) {
  const session = await getServerSession();
  
  if (!session) {
    return NextResponse.json({ error: 'Unauthorized' }, { status: 401 });
  }

  const body = await request.json();
  
  const todo = await db.todo.create({
    data: {
      title: body.title,
      userId: session.user.id,
    },
  });

  return NextResponse.json(todo, { status: 201 });
}
```

## Environment Variables

Create `.env.local`:

```bash
# Database
DATABASE_URL="postgresql://user:password@localhost:5432/mydb"

# NextAuth
NEXTAUTH_URL="http://localhost:3000"
NEXTAUTH_SECRET="your-secret-key"

# OAuth (optional)
GOOGLE_CLIENT_ID=""
GOOGLE_CLIENT_SECRET=""
GITHUB_CLIENT_ID=""
GITHUB_CLIENT_SECRET=""
```

## Setup Commands

```bash
# Create new Next.js app
npx create-next-app@latest my-app --typescript --tailwind --eslint

# Install additional packages
npm install next-auth @prisma/client prisma

# Initialize Prisma
npx prisma init
npx prisma migrate dev

# Run development server
npm run dev
```

## Why This Structure Scales

1. **Route Groups** — Logical grouping with `(auth)` and `(dashboard)`
2. **Server Components** — Default in App Router for better performance
3. **Middleware** — Edge-based auth protection
4. **colocated Code** — API routes next to pages they support
5. **Type Safety** — Full TypeScript with auth types

## Next Steps

1. Run `npm run dev` to start the development server
2. Configure your database in `.env.local`
3. Set up NextAuth with your provider
4. Build your first feature!

For more details, see:
- [App Router Fundamentals](../../13-nextjs/01-nextjs-foundations/02-app-router-fundamentals.md)
- [Server Components](../../13-nextjs/02-rendering-strategies/01-server-components-explained.md)
- [Middleware and Auth](../../13-nextjs/03-nextjs-features/03-nextjs-middleware-and-auth.md)
