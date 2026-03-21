# Satisfies Operator and Const Assertions

## Overview
The `satisfies` operator and `as const` assertion are advanced TypeScript features that provide precise type control while maintaining flexibility. The `satisfies` operator validates that a value matches a type without widening it, while `as const` creates readonly literal types. These are incredibly useful in React for creating typed config objects, route definitions, theme objects, and validation schemas where you want both type safety and exact inference. This guide covers both features with practical React examples.

## Prerequisites
- TypeScript fundamentals
- Understanding of TypeScript types and interfaces
- Familiarity with React component props and configuration

## Core Concepts

### Understanding Type Widening
Before understanding `satisfies`, it's important to understand type widening:

```typescript
// [File: src/examples/typeWidening.ts]

// When you assign a literal, TypeScript widens to the general type
let name = 'Alice'; // type: string (not 'Alice')

// With const, it stays narrow
const nameConst = 'Alice'; // type: 'Alice'

// Object properties also widen
const user = {
  role: 'admin', // role: string (widened!)
};

// To prevent widening, use 'as const'
const userFixed = {
  role: 'admin',
} as const; // role: 'admin' (readonly!)

// This matters for type safety
function setRole(role: 'admin' | 'user') {
  console.log('Setting role to:', role);
}

setRole('admin'); // Works
setRole(user.role); // Error: might not be 'admin' or 'user'
setRole(userFixed.role); // Works! TypeScript knows it's exactly 'admin'
```

### The `as const` Assertion
The `as const` assertion converts literal types to their readonly equivalents:

```typescript
// [File: src/examples/asConst.ts]

// ======== Basic Usage ========

// Without as const - values can change, type is widened
const config = {
  env: 'production',
  debug: false,
  maxRetries: 3,
};
// Type: { env: string; debug: boolean; maxRetries: number }

// With as const - all become readonly literals
const configFixed = {
  env: 'production',
  debug: false,
  maxRetries: 3,
} as const;
// Type: { readonly env: 'production'; readonly debug: false; readonly maxRetries: 3 }

// ======== Arrays ========

// Without as const
const colors = ['red', 'green', 'blue'];
// Type: string[]

// With as const
const colorsFixed = ['red', 'green', 'blue'] as const;
// Type: readonly ['red', 'green', 'blue'] (tuple!)

// Accessing tuple elements is type-safe!
type Color = typeof colorsFixed[number]; // 'red' | 'green' | 'blue'

// ======== Route Configuration Example ========

const routes = {
  home: '/',
  about: '/about',
  contact: '/contact',
  userProfile: '/users/:id',
  userPosts: '/users/:userId/posts/:postId',
} as const;
// Type: {
//   readonly home: '/';
//   readonly about: '/about';
//   readonly contact: '/contact';
//   readonly userProfile: '/users/:id';
//   readonly userPosts: '/users/:userId/posts/:postId';
// }

// Use with path parameters
function buildRoute(route: string, params: Record<string, string>): string {
  let path = route;
  Object.entries(params).forEach(([key, value]) => {
    path = path.replace(`:${key}`, value);
  });
  return path;
}

// Works perfectly with as const routes!
buildRoute(routes.userProfile, { id: '123' }); // '/users/123'
buildRoute(routes.userProfile, { id: '456' }); // '/users/456'
```

### The `satisfies` Operator
The `satisfies` operator validates that a value matches a type while preserving the original literal types:

```typescript
// [File: src/examples/satisfies.ts]

// ======== Problem: Type Widening Loses Information ========

interface Config {
  env: string;
  debug: boolean;
}

// Without satisfies - loses literal types
const config1: Config = {
  env: 'production', // becomes string
  debug: false,       // becomes boolean
};
// Type: Config (not the literal types!)

// With satisfies - validates but preserves literals!
const config2 = {
  env: 'production',
  debug: false,
} satisfies Config;
// Type: { env: 'production'; debug: false; }
// - Validates it matches Config
// - BUT keeps the exact literal types!

// ======== Real React Example: Theme Colors ========

interface ThemeColors {
  primary: string;
  secondary: string;
  background: string;
  text: string;
}

// Using satisfies - get validation + literal types
const lightTheme = {
  primary: '#007bff',
  secondary: '#6c757d',
  background: '#ffffff',
  text: '#000000',
} satisfies ThemeColors;
// Type: { primary: '#007bff'; secondary: '#6c757d'; background: '#ffffff'; text: '#000000'; }

// Using type annotation - widens to string
const lightThemeBad: ThemeColors = {
  primary: '#007bff',
  secondary: '#6c757d',
  background: '#ffffff',
  text: '#000000',
};
// Type: ThemeColors (all strings!)

// Why does this matter? Accessing values:
lightTheme.primary; // Type: '#007bff' - exact literal!
lightThemeBad.primary; // Type: 'string' - too broad!

// ======== Validating While Preserving Inference ========

type Route = '/' | '/about' | '/contact' | '/users/[id]';

const routes = {
  home: '/',
  about: '/about',
  contact: '/contact',
  userProfile: '/users/[id]',
} satisfies Record<string, Route>;

// Each route is typed as exact literal, not just Route
routes.home; // Type: '/'
routes.userProfile; // Type: '/users/[id]'

// But it still validates against Record<string, Route>!
```

### Combining as const and satisfies
Use both together for maximum type precision:

```typescript
// [File: src/examples/combined.ts]

// ======== Complex Configuration ========

interface ApiEndpoint {
  path: string;
  method: 'GET' | 'POST' | 'PUT' | 'DELETE';
  auth: boolean;
}

const apiEndpoints = {
  getUsers: {
    path: '/api/users',
    method: 'GET',
    auth: true,
  },
  createUser: {
    path: '/api/users',
    method: 'POST',
    auth: true,
  },
  getPublicData: {
    path: '/api/public',
    method: 'GET',
    auth: false,
  },
} as const satisfies Record<string, ApiEndpoint>;

// Type is fully inferred and validated!
type Endpoint = typeof apiEndpoints;
// Type: {
//   readonly getUsers: { readonly path: '/api/users'; readonly method: 'GET'; readonly auth: true; };
//   readonly createUser: { readonly path: '/api/users'; readonly method: 'POST'; readonly auth: true; };
//   readonly getPublicData: { readonly path: '/api/public'; readonly method: 'GET'; readonly auth: false; };
// }

// Access values with exact types
apiEndpoints.getUsers.method; // Type: 'GET'
apiEndpoints.getPublicData.auth; // Type: false
```

## Common Mistakes

### Mistake 1: Using Type Annotation Instead of satisfies
```typescript
// ❌ WRONG - Type annotation widens literals
const colors: Record<string, string> = {
  primary: '#007bff',
  secondary: '#6c757d',
};
// colors.primary is type string, not '#007bff'

// ✅ CORRECT - Use satisfies to validate and preserve
const colors = {
  primary: '#007bff',
  secondary: '#6c757d',
} satisfies Record<string, string>;
// colors.primary is type '#007bff' (literal!)
```

### Mistake 2: Forgetting as const for Arrays
```typescript
// ❌ WRONG - Array type is string[]
const sizes = ['small', 'medium', 'large'];

// ✅ CORRECT - Use as const for readonly tuple
const sizes = ['small', 'medium', 'large'] as const;
// Type: readonly ['small', 'medium', 'large']

// Now you can get exact types
type Size = typeof sizes[number]; // 'small' | 'medium' | 'large'
```

### Mistake 3: Not Using readonly for Function Parameters
```typescript
// ❌ WRONG - Array can be modified
function setSizes(sizes: string[]) {
  sizes.push('extra'); // Modifies original!
}

// ✅ CORRECT - Use readonly array
function setSizes(sizes: readonly string[]) {
  // sizes.push('extra'); // Error! Cannot modify
}
```

## Real-World Example

Complete typed routing system with satisfies:

```typescript
// [File: src/routing/routes.ts]

// ======== Route Definitions ========

interface RouteConfig {
  path: string;
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';
  authRequired: boolean;
  roles?: readonly string[];
}

// Define all routes with full validation
const appRoutes = {
  // Auth routes
  login: {
    path: '/auth/login',
    method: 'POST',
    authRequired: false,
  },
  register: {
    path: '/auth/register',
    method: 'POST',
    authRequired: false,
  },
  logout: {
    path: '/auth/logout',
    method: 'POST',
    authRequired: true,
  },
  
  // User routes
  getUsers: {
    path: '/api/users',
    method: 'GET',
    authRequired: true,
    roles: ['admin'],
  },
  getUser: {
    path: '/api/users/:id',
    method: 'GET',
    authRequired: true,
  },
  updateUser: {
    path: '/api/users/:id',
    method: 'PUT',
    authRequired: true,
  },
  deleteUser: {
    path: '/api/users/:id',
    method: 'DELETE',
    authRequired: true,
    roles: ['admin'],
  },
  
  // Product routes
  getProducts: {
    path: '/api/products',
    method: 'GET',
    authRequired: false,
  },
  getProduct: {
    path: '/api/products/:id',
    method: 'GET',
    authRequired: false,
  },
  createProduct: {
    path: '/api/products',
    method: 'POST',
    authRequired: true,
    roles: ['admin'],
  },
} as const satisfies Record<string, RouteConfig>;

// Type-safe route access
type RouteName = keyof typeof appRoutes;
type RouteConfigType = typeof appRoutes[RouteName];

// ======== Route Builder ========

function buildPath(
  routeName: RouteName,
  params?: Record<string, string | number>
): string {
  let path = appRoutes[routeName].path;
  
  if (params) {
    Object.entries(params).forEach(([key, value]) => {
      path = path.replace(`:${key}`, String(value));
    });
  }
  
  return path;
}

// Usage - fully typed!
buildPath('login'); // '/auth/login'
buildPath('getUser', { id: '123' }); // '/api/users/123'
buildPath('getProduct', { id: 456 }); // '/api/products/456'

// ======== API Client with Full Typing ========

type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';

interface RequestConfig {
  method: HttpMethod;
  path: string;
  authRequired: boolean;
  roles?: readonly string[];
}

// Generic fetch function with route typing
async function apiRequest<
  R extends RouteName,
  T = unknown
>(
  routeName: R,
  options?: {
    params?: Record<string, string | number>;
    body?: unknown;
    token?: string;
  }
): Promise<T> {
  const config = appRoutes[routeName];
  const path = buildPath(routeName, options?.params);
  
  // Check auth requirement
  if (config.authRequired && !options?.token) {
    throw new Error(`Route ${routeName} requires authentication`);
  }
  
  // Make request
  const response = await fetch(path, {
    method: config.method,
    headers: {
      'Content-Type': 'application/json',
      ...(options?.token && { Authorization: `Bearer ${options.token}` }),
    },
    body: options?.body ? JSON.stringify(options.body) : undefined,
  });
  
  if (!response.ok) {
    throw new Error(`API error: ${response.status}`);
  }
  
  return response.json();
}

// ======== Usage in Components ========

interface Product {
  id: string;
  name: string;
  price: number;
}

function ProductList() {
  React.useEffect(() => {
    // Fully typed API call!
    const loadProducts = async () => {
      const products = await apiRequest<'getProducts', Product[]>('getProducts');
      console.log(products);
    };
    
    loadProducts();
  }, []);
  
  return <div>Products</div>;
}

function CreateProduct({ token }: { token: string }) {
  const handleCreate = async () => {
    // TypeScript knows this route requires auth
    const product = await apiRequest<'createProduct', Product>('createProduct', {
      token,
      body: { name: 'New Product', price: 99 },
    });
    console.log('Created:', product);
  };
  
  return <button onClick={handleCreate}>Create Product</button>;
}

export { appRoutes, buildPath, apiRequest };
```

## Key Takeaways
- Use `as const` to create readonly literal types for objects and tuples
- Use `satisfies` to validate types while preserving literal inference
- `satisfies` validates against a type but doesn't widen literals
- `as const` is essential for route definitions, theme configs, and enum-like objects
- Combine both for maximum type precision and validation
- TypeScript can infer exact types when you use these operators
- Prevents runtime errors by catching mismatches at compile time

## What's Next
This completes the TypeScript module. Continue to [Next.js vs React SPA](13-nextjs/01-nextjs-foundations/01-nextjs-vs-react-spa.md) to understand when to choose Next.js over a traditional React SPA.