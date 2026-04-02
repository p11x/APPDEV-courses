# Auth in GraphQL

## What You'll Learn

- How to pass authentication context to GraphQL resolvers
- How to protect entire queries/mutations with auth checks
- How to implement field-level authorization
- How to mask internal errors from clients
- How JWT authentication integrates with GraphQL

## Authentication vs Authorization

- **Authentication** (authn): "Who are you?" — verifying identity (login, JWT, API key)
- **Authorization** (authz): "What can you do?" — checking permissions (admin vs user)

GraphQL does not have built-in auth. You implement it in the **context** function and **resolvers**.

> See: ../08-authentication/jwt/01-jwt-basics.md for JWT fundamentals.

## Full Example

```js
// server.js — GraphQL server with JWT auth and field-level authorization

import { createServer } from 'node:http';
import { createSchema, createYoga } from 'graphql-yoga';
import { verify } from 'jsonwebtoken';  // npm install jsonwebtoken

// Secret key for JWT verification (use env variable in production)
const JWT_SECRET = 'my-secret-key';

// Simulated database
const users = [
  { id: '1', name: 'Alice', email: 'alice@example.com', role: 'admin', password: 'secret123' },
  { id: '2', name: 'Bob', email: 'bob@example.com', role: 'user', password: 'password' },
];

const posts = [
  { id: '1', title: 'Public Post', body: 'Anyone can see this', authorId: '1', published: true },
  { id: '2', title: 'Draft', body: 'Only author can see this', authorId: '1', published: false },
];

const typeDefs = /* GraphQL */ `
  type Query {
    # Public — no auth required
    publicPosts: [Post!]!
    # Protected — must be logged in
    me: User!
    # Admin only
    allUsers: [User!]!
  }

  type Mutation {
    login(email: String!, password: String!): AuthPayload!
  }

  type AuthPayload {
    token: String!
    user: User!
  }

  type User {
    id: ID!
    name: String!
    email: String!          # Only the user themselves or admins can see email
    role: String!
    posts: [Post!]!
  }

  type Post {
    id: ID!
    title: String!
    body: String!
    author: User!
  }
`;

// Helper: extract and verify JWT from the request
function getUserFromToken(request) {
  const authHeader = request.headers.get('authorization');
  if (!authHeader?.startsWith('Bearer ')) return null;

  const token = authHeader.slice(7);  // Remove "Bearer " prefix

  try {
    // verify() throws if the token is invalid or expired
    const decoded = verify(token, JWT_SECRET);
    return decoded;  // { userId: '1', role: 'admin', iat: ..., exp: ... }
  } catch {
    return null;  // Invalid token — treat as unauthenticated
  }
}

const resolvers = {
  Query: {
    // Public endpoint — no auth check
    publicPosts: () => posts.filter((p) => p.published),

    // Protected endpoint — throws if not logged in
    me: (_, __, context) => {
      // context.currentUser is set in the context function below
      if (!context.currentUser) {
        throw new Error('Authentication required');
      }
      return context.currentUser;
    },

    // Admin-only endpoint
    allUsers: (_, __, context) => {
      if (!context.currentUser) {
        throw new Error('Authentication required');
      }
      if (context.currentUser.role !== 'admin') {
        throw new Error('Admin access required');
      }
      return users.map(({ password, ...user }) => user);  // Strip password
    },
  },

  Mutation: {
    login: (_, { email, password }) => {
      const user = users.find((u) => u.email === email);
      if (!user || user.password !== password) {
        throw new Error('Invalid email or password');
      }

      // Sign a JWT with the user's ID and role
      const token = sign({ userId: user.id, role: user.role }, JWT_SECRET, {
        expiresIn: '24h',
      });

      return {
        token,
        user: { ...user, password: undefined },  // Never return the password
      };
    },
  },

  User: {
    // Field-level authorization — email is only visible to the user or admins
    email: (parent, _, context) => {
      const currentUser = context.currentUser;
      if (!currentUser) return null;  // Not logged in — hide email

      // Users can see their own email; admins can see everyone's
      if (currentUser.id === parent.id || currentUser.role === 'admin') {
        return parent.email;
      }

      return null;  // Hide email from other users
    },

    posts: (parent) => {
      // Return only published posts for other users
      const currentUser = context?.currentUser;
      if (currentUser?.id === parent.id) {
        return posts.filter((p) => p.authorId === parent.id);  // All own posts
      }
      return posts.filter((p) => p.authorId === parent.id && p.published);
    },
  },
};

const schema = createSchema({ typeDefs, resolvers });

const yoga = createYoga({
  schema,
  // context is built per request — extracts user from JWT
  context: ({ request }) => {
    const currentUser = getUserFromToken(request);
    return { currentUser };
  },
  // Mask unexpected errors — do not leak stack traces to clients
  maskedErrors: true,
});

const server = createServer(yoga);
server.listen(4000, () => {
  console.log('GraphQL server with auth on http://localhost:4000/graphql');
});
```

## Testing Auth

### Login (no auth required)

```graphql
mutation {
  login(email: "alice@example.com", password: "secret123") {
    token
    user { name role }
  }
}
```

Copy the `token` from the response.

### Protected Query (with auth)

In GraphiQL, click "HTTP Headers" at the bottom and add:

```json
{
  "authorization": "Bearer <paste-token-here>"
}
```

Then run:

```graphql
query {
  me {
    name
    email
    role
  }
}
```

### Field-Level Auth

Query as Bob (non-admin) trying to see Alice's email:

```graphql
query {
  allUsers {
    name
    email  # Returns null for non-admin users
  }
}
```

## How It Works

### The Context Flow

```
Request → yoga context() → extract JWT → { currentUser }
                                      ↓
                              Resolvers use context.currentUser
                                      ↓
                              Allow or throw
```

Every resolver receives `context` as its third argument. By populating `context.currentUser` once in the context function, all resolvers can check auth without re-parsing the JWT.

### Error Masking

`maskedErrors: true` ensures that internal errors (stack traces, database errors) are replaced with a generic "Unexpected error" message. Only errors you explicitly `throw` with user-friendly messages reach the client.

### Field-Level vs Query-Level Auth

| Level | Where | Use Case |
|-------|-------|----------|
| Query/Mutation | Resolver entry | Block entire operations (e.g., "must be logged in") |
| Field | Individual field resolver | Hide sensitive fields from unauthorized users |

## Common Mistakes

### Mistake 1: Auth Check in Every Resolver

```js
// WRONG — duplicated auth logic everywhere
me: (_, __, context) => {
  if (!context.currentUser) throw new Error('Auth required');
  return context.currentUser;
},
allUsers: (_, __, context) => {
  if (!context.currentUser) throw new Error('Auth required');  // Repeated
  return users;
},

// CORRECT — create a helper or directive
function requireAuth(context) {
  if (!context.currentUser) throw new Error('Authentication required');
  return context.currentUser;
}

me: (_, __, context) => requireAuth(context),
allUsers: (_, __, context) => {
  const user = requireAuth(context);
  if (user.role !== 'admin') throw new Error('Admin required');
  return users;
},
```

### Mistake 2: Not Masking Errors

```js
// WRONG — stack traces leak to clients
const yoga = createYoga({ schema });
// Client sees: "Error: connect ECONNREFUSED 127.0.0.1:5432"

// CORRECT — mask unexpected errors
const yoga = createYoga({ schema, maskedErrors: true });
// Client sees: "Unexpected error."
```

### Mistake 3: Returning Passwords

```js
// WRONG — password hash leaked to clients
user: (_, { id }) => users.find(u => u.id === id);

// CORRECT — strip sensitive fields
user: (_, { id }) => {
  const { password, ...safe } = users.find(u => u.id === id);
  return safe;
};
```

## Try It Yourself

### Exercise 1: Role-Based Access

Add a `deleteUser(id: ID!)` mutation that only admins can call. Non-admin users get "Admin access required".

### Exercise 2: Resource Ownership

Add an `updatePost(id: ID!, title: String!)` mutation. Only the post's author should be allowed to update it.

### Exercise 3: Rate Limiting

Add per-user rate limiting in the context function. Track the number of requests per user ID and reject after 100 requests per minute.

## Next Steps

You understand GraphQL auth. For a simpler REST-based API approach, review [Chapter 08: Authentication](../../08-authentication/jwt/01-jwt-basics.md).
