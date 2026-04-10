/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 09_Type_Libraries
 * Topic: Type_Safe_API_Clients
 * Purpose: Creating type-safe API client libraries
 * Difficulty: advanced
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: Type-checking overhead only
 * Security: Type-level request/response validation
 */

/**
 * Type-Safe API Clients - Comprehensive Guide
 * ===========================================
 * 
 * 📚 WHAT: Creating type-safe HTTP API clients
 * 💡 WHERE: API integrations, HTTP clients, service layers
 * 🔧 HOW: Generic types, request/response typing, inference
 */

// ============================================================================
// SECTION 1: WHAT - Type-Safe API Clients
// ============================================================================

/**
 * WHAT are type-safe API clients?
 * - HTTP clients with full type safety
 * - Request/response type inference
 * - Parameter and path typing
 * - Error type handling
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use type-safe API clients?
 * - Catch API changes at compile-time
 * - IDE autocomplete for endpoints
 * - Type-safe error handling
 * - Reduce integration bugs
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: API Endpoint Types
// --------------------------------

type HttpMethod = "GET" | "POST" | "PUT" | "PATCH" | "DELETE";

interface Endpoint<TPath extends string, TMethod extends HttpMethod, TParams, TBody, TResponse> {
  path: TPath;
  method: TMethod;
  params?: TParams;
  body?: TBody;
  response: TResponse;
}

// Example 3.2: API Definition
// --------------------------------

type ApiSpec = {
  "GET /users": Endpoint<"/users", "GET", { page?: number }, never, { users: User[] }>;
  "POST /users": Endpoint<"/users", "POST", never, CreateUserDTO, User>;
  "GET /users/:id": Endpoint<"/users/:id", "GET", never, never, User>;
  "PUT /users/:id": Endpoint<"/users/:id", "PUT", never, UpdateUserDTO, User>;
};

type User = { id: string; name: string; email: string };
type CreateUserDTO = { name: string; email: string };
type UpdateUserDTO = Partial<CreateUserDTO>;

// Example 3.3: Client Implementation
// --------------------------------

type Client<S extends Record<string, Endpoint<string, HttpMethod, any, any, any>>> = {
  [K in keyof S]: S[K] extends Endpoint<infer P, infer M, infer Params, infer Body, infer Res>
    ? (params?: Params, body?: Body) => Promise<Res>
    : never;
};

// Example 3.4: Type-Safe Request Function
// --------------------------------

async function request<
  TEndpoint extends Endpoint<string, HttpMethod, any, any, any>
>(
  endpoint: TEndpoint,
  params?: TEndpoint["params"],
  body?: TEndpoint["body"]
): Promise<TEndpoint["response"]> {
  const url = endpoint.path.replace(/:(\w+)/g, (_, key) => params?.[key as keyof typeof params] || "");
  const response = await fetch(url, {
    method: endpoint.method,
    body: body ? JSON.stringify(body) : undefined,
    headers: { "Content-Type": "application/json" },
  });
  return response.json();
}

// Example 3.5: Error Types
// --------------------------------

type ApiError<TStatus extends number, TMessage> = {
  status: TStatus;
  message: TMessage;
  timestamp: string;
};

type ErrorResponse<T extends number> = T extends 400 
  ? ApiError<400, "Bad Request">
  : T extends 401 
    ? ApiError<401, "Unauthorized">
    : T extends 404 
      ? ApiError<404, "Not Found">
      : ApiError<500, "Internal Server Error">;

// Example 3.6: Response Type Handling
// --------------------------------

type ExtractResponse<T> = T extends Endpoint<string, HttpMethod, any, any, infer R> ? R : never;

type UserResponse = ExtractResponse<ApiSpec["GET /users"]>;
// { users: User[] }

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Type-checking only, no runtime overhead
 * - Complex generics slow compilation slightly
 * - Efficient for typical APIs
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - TypeScript 4.1+ for full support
 * - Works with any HTTP client (fetch, axios)
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Type validation at compile-time
 * - Sanitize user input in requests
 * - Use environment variables for secrets
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test with various API specs
 * - Verify error type handling
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Hover over endpoint types
 * - Test with invalid paths
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - OpenAPI code generation
 * - tRPC
 * - Fetch with manual typing
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Type Safe API Clients Complete ===");
console.log("Next: 09_Type_Libraries/03_Validation_Libraries.ts");
console.log("Related: 01_Typed_Query_Builders.ts, 06_Macros_and_Code_Generation/04_Type_Safe_Builders.ts");