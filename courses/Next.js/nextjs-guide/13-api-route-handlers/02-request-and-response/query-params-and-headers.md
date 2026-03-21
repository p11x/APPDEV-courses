# Query Parameters and Headers

## What You'll Learn
- Reading URL query parameters
- Accessing request headers
- Combining multiple data sources
- Type-safe parameter handling

## Prerequisites
- Understanding of route handlers
- Knowledge of URL structure
- Familiarity with HTTP headers

## Concept Explained Simply

Query parameters are like asking for specifics at a restaurant: "Can I see the menu?" is a basic request, but "Can I see the vegetarian options?" adds extra information. In URLs, these look like `?type=vegetarian` — they're optional additions after the main path.

Headers are like the metadata on an envelope — who sent it, what's inside, what language it's in. They provide context about the request itself, like authentication tokens, preferred formats, and client information.

## Complete Code Example

### Reading Query Parameters

```typescript
// app/api/search/route.ts
import { NextResponse } from "next/server";

export async function GET(request: Request) {
  // Get the URL from the request
  const { searchParams } = new URL(request.url);
  
  // Read individual parameters
  const query = searchParams.get("q");
  const category = searchParams.get("category");
  const page = searchParams.get("page") || "1";
  const limit = searchParams.get("limit") || "10";
  
  // Build a database query (mock)
  const results = await mockSearch(query, {
    category,
    page: parseInt(page),
    limit: parseInt(limit),
  });
  
  return NextResponse.json({
    query,
    category,
    page: parseInt(page),
    results,
  });
}

// Mock search function
async function mockSearch(query: string | null, options: any) {
  // In real app, query database
  return [
    { id: 1, title: `Result for ${query || "all"}` },
    { id: 2, title: "Another result" },
  ];
}
```

### Using with Dynamic Segments

```typescript
// app/api/products/[category]/route.ts
import { NextResponse } from "next/server";

export async function GET(
  request: Request,
  { params }: { params: Promise<{ category: string }> }
) {
  const { category } = await params;
  const { searchParams } = new URL(request.url);
  
  const sort = searchParams.get("sort") || "newest";
  const minPrice = searchParams.get("minPrice");
  const maxPrice = searchParams.get("maxPrice");
  
  // Build query with dynamic segment + query params
  const products = await getProducts(category, {
    sort,
    minPrice: minPrice ? parseFloat(minPrice) : undefined,
    maxPrice: maxPrice ? parseFloat(maxPrice) : undefined,
  });
  
  return NextResponse.json({ products });
}
```

### Reading Headers

```typescript
// app/api/profile/route.ts
import { NextResponse } from "next/server";

export async function GET(request: Request) {
  // Read headers
  const authHeader = request.headers.get("authorization");
  const acceptLanguage = request.headers.get("accept-language");
  const userAgent = request.headers.get("user-agent");
  const contentType = request.headers.get("content-type");
  
  // Check for authorization
  if (!authHeader) {
    return NextResponse.json(
      { error: "Authorization required" },
      { status: 401 }
    );
  }
  
  // Extract token
  const token = authHeader.replace("Bearer ", "");
  
  // Get user from token...
  const user = await verifyToken(token);
  
  if (!user) {
    return NextResponse.json(
      { error: "Invalid token" },
      { status: 403 }
    );
  }
  
  return NextResponse.json({
    user,
    language: acceptLanguage,
    client: userAgent,
  });
}
```

### Complete Example with All Sources

```typescript
// app/api/users/[id]/posts/route.ts
import { NextResponse } from "next/server";

export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
  const { searchParams } = new URL(request.url);
  
  // Query parameters
  const page = parseInt(searchParams.get("page") || "1");
  const limit = parseInt(searchParams.get("limit") || "10");
  const published = searchParams.get("published") === "true";
  
  // Headers
  const authToken = request.headers.get("x-auth-token");
  const apiKey = request.headers.get("x-api-key");
  
  // Validate API key (example)
  if (!apiKey || !isValidApiKey(apiKey)) {
    return NextResponse.json(
      { error: "Invalid API key" },
      { status: 403 }
    );
  }
  
  // Fetch posts
  const posts = await getUserPosts(id, {
    page,
    limit,
    published,
    authToken,
  });
  
  return NextResponse.json({
    page,
    limit,
    posts,
  });
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `new URL(request.url)` | Parse the request URL | Gives access to searchParams |
| `searchParams.get("key")` | Get single parameter | Returns string or null |
| `searchParams.getAll("key")` | Get all values | For parameters with multiple values |
| `request.headers.get()` | Get single header | Case-insensitive |
| `request.headers.getAll()` | Get all values | Returns string[] for headers like Set-Cookie |

## Common Mistakes

### Mistake 1: Accessing Params Without Await (Next.js 15)

```typescript
// WRONG - Params is a Promise in Next.js 15
export async function GET(
  request: Request,
  { params }: { params: { id: string } }
) {
  const { id } = params; // Wrong!
}

// CORRECT - Await params
export async function GET(
  request: Request,
  { params }: { params: Promise<{ id: string }> }
) {
  const { id } = await params;
}
```

### Mistake 2: Not Handling Null Values

```typescript
// WRONG - Parameter could be null
export async function GET(request: Request) {
  const page = searchParams.get("page");
  const results = await getResults(page); // page could be null!
}

// CORRECT - Provide defaults
export async function GET(request: Request) {
  const page = searchParams.get("page") || "1";
  const results = await getResults(page);
}
```

### Mistake 3: Header Names Are Case-Insensitive

```typescript
// WRONG - Thinking headers are case-sensitive
const token1 = request.headers.get("Authorization");
const token2 = request.headers.get("authorization");
console.log(token1 === token2); // true - they're the same!

// This doesn't matter, just use any case
const token = request.headers.get("authorization");
```

## Summary

- Use `new URL(request.url).searchParams` to access query parameters
- Parameters are always strings — convert to numbers/booleans as needed
- Headers are accessed via `request.headers.get()` and are case-insensitive
- Always handle null/undefined values with defaults
- In Next.js 15, `params` is a Promise — must await it
- Query params are great for filtering, pagination, and optional settings

## Next Steps

- [returning-json.md](./returning-json.md) - Sending responses back to clients
- [dynamic-route-handlers.md](../03-advanced-handlers/dynamic-route-handlers.md) - Using dynamic segments in API routes
