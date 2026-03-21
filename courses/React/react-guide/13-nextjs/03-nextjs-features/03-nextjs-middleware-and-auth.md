# Next.js Middleware and Auth

## Overview
Next.js Middleware allows you to run code before a request is completed, enabling features like authentication, redirects, rewrites, and A/B testing. Combined with proper authentication patterns, Middleware provides a powerful way to protect routes, personalize content, and handle cross-cutting concerns at the edge.

## Prerequisites
- Understanding of Next.js routing
- Familiarity with HTTP concepts
- Basic authentication knowledge

## Core Concepts

### Creating Middleware
Middleware is created in the root of your project:

```typescript
// [File: middleware.ts]
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// Run middleware for all routes
export function middleware(request: NextRequest) {
  // Log request
  console.log('Request:', request.url);
  
  // Get response
  const response = NextResponse.next();
  
  // Add custom header
  response.headers.set('x-custom-header', 'value');
  
  return response;
}

// Configure which routes to run middleware on
export const config = {
  matcher: [
    // Match all paths except static files
    '/((?!api|_next/static|_next/image|favicon.ico).*)',
  ],
};
```

### Authentication with Middleware
Protect routes based on authentication status:

```typescript
// [File: middleware.ts]
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
import { verifyToken } from '@/lib/auth';

export async function middleware(request: NextRequest) {
  // Get token from cookie or header
  const token = request.cookies.get('auth-token')?.value 
    || request.headers.get('authorization')?.replace('Bearer ', '');
  
  // Verify token
  const isValid = token ? await verifyToken(token) : false;
  
  // Get the pathname
  const { pathname } = request.nextUrl;
  
  // Public routes that don't require auth
  const publicPaths = ['/', '/login', '/signup', '/about'];
  const isPublicPath = publicPaths.some(p => pathname === p || pathname.startsWith(p + '/'));
  
  // Protected routes
  const isProtectedPath = pathname.startsWith('/dashboard') 
    || pathname.startsWith('/settings') 
    || pathname.startsWith('/api/protected');
  
  // Redirect unauthenticated users to login
  if (isProtectedPath && !isValid) {
    const loginUrl = new URL('/login', request.url);
    loginUrl.searchParams.set('redirect', pathname);
    return NextResponse.redirect(loginUrl);
  }
  
  // Redirect authenticated users away from public auth pages
  if (isValid && (pathname === '/login' || pathname === '/signup')) {
    return NextResponse.redirect(new URL('/dashboard', request.url));
  }
  
  return NextResponse.next();
}

export const config = {
  matcher: [
    '/((?!api|_next/static|_next/image|favicon.ico).*)',
  ],
};
```

### Role-Based Access Control
Middleware can enforce role-based access:

```typescript
// [File: middleware.ts]
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
import { getUserFromToken } from '@/lib/auth';

export async function middleware(request: NextRequest) {
  const token = request.cookies.get('auth-token')?.value;
  
  if (!token) {
    return NextResponse.redirect(new URL('/login', request.url));
  }
  
  const user = await getUserFromToken(token);
  
  if (!user) {
    return NextResponse.redirect(new URL('/login', request.url));
  }
  
  const { pathname } = request.nextUrl;
  
  // Admin-only routes
  if (pathname.startsWith('/admin') && user.role !== 'admin') {
    return NextResponse.redirect(new URL('/dashboard', request.url));
  }
  
  // Moderator routes
  if (pathname.startsWith('/moderation') && 
      !['admin', 'moderator'].includes(user.role)) {
    return NextResponse.redirect(new URL('/dashboard', request.url));
  }
  
  return NextResponse.next();
}

export const config = {
  matcher: [
    '/dashboard/:path*',
    '/admin/:path*',
    '/settings/:path*',
    '/moderation/:path*',
  ],
};
```

### Multi-Tenant Middleware
Handle multi-tenant applications:

```typescript
// [File: middleware.ts]
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export async function middleware(request: NextRequest) {
  const { pathname, hostname } = request.nextUrl;
  
  // Get tenant from subdomain
  const tenant = hostname.split('.')[0];
  
  // Check for custom tenant in header or cookie
  const customTenant = request.cookies.get('tenant')?.value;
  
  // Build the URL with tenant
  const tenantToUse = customTenant || tenant;
  
  // Skip for localhost or default domain
  if (hostname === 'localhost:3000' || hostname === 'example.com') {
    return NextResponse.next();
  }
  
  // Add tenant header for API routes
  if (pathname.startsWith('/api')) {
    const response = NextResponse.next();
    response.headers.set('x-tenant-id', tenantToUse);
    return response;
  }
  
  // Rewrite tenant routes
  if (tenantToUse && tenantToUse !== 'www') {
    const rewriteUrl = new URL(`/${tenantToUse}${pathname}`, request.url);
    return NextResponse.rewrite(rewriteUrl);
  }
  
  return NextResponse.next();
}

export const config = {
  matcher: [
    '/((?!api|_next/static|_next/image|favicon.ico).*)',
  ],
};
```

### A/B Testing with Middleware
Implement A/B testing:

```typescript
// [File: middleware.ts]
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export async function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  
  // Only run on landing pages
  if (pathname !== '/') {
    return NextResponse.next();
  }
  
  // Check if user already has a variant assigned
  let variant = request.cookies.get('ab-variant')?.value;
  
  // If not, assign a random variant (50/50 split)
  if (!variant) {
    variant = Math.random() < 0.5 ? 'control' : 'variant-a';
  }
  
  // Create response
  const response = NextResponse.next();
  
  // Set cookie if not already set
  if (!request.cookies.get('ab-variant')) {
    response.cookies.set('ab-variant', variant, {
      maxAge: 60 * 60 * 24 * 30, // 30 days
      path: '/',
    });
  }
  
  // Add header for downstream components
  response.headers.set('x-ab-variant', variant);
  
  return response;
}

export const config = {
  matcher: '/',
};
```

## Common Mistakes

### Mistake 1: Not Handling Static Files
```typescript
// ❌ WRONG - Middleware runs on all requests including static files
export function middleware(request) {
  // Expensive operation runs unnecessarily
}

// ✅ CORRECT - Exclude static files
export const config = {
  matcher: [
    '/((?!api|_next/static|_next/image|favicon.ico).*)',
  ],
};
```

### Mistake 2: Not Awaiting Async Operations
```typescript
// ❌ WRONG - Not awaiting async function
export function middleware(request) {
  verifyToken(request.cookies.get('token').value); // Not awaited!
}

// ✅ CORRECT - Always await async functions
export async function middleware(request) {
  await verifyToken(request.cookies.get('token').value);
}
```

## Real-World Example

Complete auth middleware with rate limiting:

```typescript
// [File: middleware.ts]
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
import { verifyToken, getUserFromToken } from '@/lib/auth';
import { rateLimit } from '@/lib/rate-limit';

// Simple in-memory rate limiting (use Redis in production)
const requestCounts = new Map<string, { count: number; resetTime: number }>();

export async function middleware(request: NextRequest) {
  const { pathname } = request.nextUrl;
  
  // Rate limiting for auth routes
  if (pathname === '/login' || pathname === '/signup') {
    const ip = request.ip || 'unknown';
    const rateLimitResult = checkRateLimit(ip);
    
    if (!rateLimitResult.allowed) {
      return NextResponse.json(
        { error: 'Too many requests. Please try again later.' },
        { status: 429 }
      );
    }
  }
  
  // Skip for public routes
  const publicPaths = ['/', '/login', '/signup', '/about', '/public'];
  if (publicPaths.some(p => pathname === p || pathname.startsWith(p + '/'))) {
    return NextResponse.next();
  }
  
  // Skip for API routes (handled separately)
  if (pathname.startsWith('/api/')) {
    return NextResponse.next();
  }
  
  // Check authentication for all other routes
  const token = request.cookies.get('auth-token')?.value;
  
  if (!token) {
    const loginUrl = new URL('/login', request.url);
    loginUrl.searchParams.set('redirect', pathname);
    return NextResponse.redirect(loginUrl);
  }
  
  // Verify token
  const isValid = await verifyToken(token);
  
  if (!isValid) {
    const response = NextResponse.redirect(new URL('/login', request.url));
    response.cookies.delete('auth-token');
    return response;
  }
  
  // Add user info to headers for downstream use
  const user = await getUserFromToken(token);
  const response = NextResponse.next();
  response.headers.set('x-user-id', user?.id || '');
  response.headers.set('x-user-role', user?.role || '');
  
  return response;
}

function checkRateLimit(ip: string): { allowed: boolean; remaining: number } {
  const now = Date.now();
  const windowMs = 15 * 60 * 1000; // 15 minutes
  const maxRequests = 5;
  
  const record = requestCounts.get(ip);
  
  if (!record || now > record.resetTime) {
    requestCounts.set(ip, {
      count: 1,
      resetTime: now + windowMs,
    });
    return { allowed: true, remaining: maxRequests - 1 };
  }
  
  if (record.count >= maxRequests) {
    return { allowed: false, remaining: 0 };
  }
  
  record.count++;
  return { allowed: true, remaining: maxRequests - record.count };
}

export const config = {
  matcher: [
    '/((?!api|_next/static|_next/image|favicon.ico).*)',
  ],
};
```

## Key Takeaways
- Middleware runs before requests complete
- Use matcher to control which routes trigger middleware
- Middleware can redirect, rewrite, or modify responses
- Combine with authentication for protected routes
- Implement rate limiting in middleware
- Use cookies and headers for state management
- Avoid expensive operations in middleware
- Exclude static files and API routes from middleware when not needed

## What's Next
This completes the Next.js module. Continue to [WCAG Guidelines for React](14-accessibility/01-a11y-foundations/01-wcag-guidelines-for-react.md) to learn about building accessible React applications.