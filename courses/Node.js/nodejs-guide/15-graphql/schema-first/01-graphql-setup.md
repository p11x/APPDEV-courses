# GraphQL Setup

## What You'll Learn

- What GraphQL is and how it differs from REST
- How to set up a GraphQL server with graphql-yoga
- How to define typeDefs (schema) and resolvers
- How to test queries with the built-in GraphiQL IDE
- How GraphQL's type system enforces valid requests

## What Is GraphQL?

REST APIs have fixed endpoints — `GET /users`, `GET /users/:id`, `GET /users/:id/posts`. The server decides what data to return. If you need different fields, you make multiple requests or create new endpoints.

**GraphQL** replaces this with a single endpoint (`/graphql`) where clients specify exactly what data they need. The client sends a **query** describing the shape of the response, and the server returns precisely that shape.

```
REST:    GET /users/1          → { id, name, email, age, address, ... }
GraphQL: POST /graphql         → { user(id: 1) { name, email } }  → { name, email }
```

## Project Setup

```bash
mkdir graphql-demo && cd graphql-demo
npm init -y
npm install graphql-yoga
```

Add `"type": "module"` to `package.json`.

## Basic GraphQL Server

```js
// server.js — GraphQL server with graphql-yoga

import { createServer } from 'node:http';
import { createSchema, createYoga } from 'graphql-yoga';

// In-memory data store (replace with a database in production)
const users = [
  { id: '1', name: 'Alice', email: 'alice@example.com', age: 30 },
  { id: '2', name: 'Bob', email: 'bob@example.com', age: 25 },
  { id: '3', name: 'Charlie', email: 'charlie@example.com', age: 35 },
];

const posts = [
  { id: '1', title: 'Hello GraphQL', body: 'GraphQL is great!', authorId: '1' },
  { id: '2', title: 'Node.js Tips', body: 'Use async/await.', authorId: '2' },
  { id: '3', title: 'REST vs GraphQL', body: 'They each have strengths.', authorId: '1' },
];

// Define the schema — this is the contract between client and server
// typeDefs uses SDL (Schema Definition Language)
const typeDefs = /* GraphQL */ `
  # The Query type defines all available read operations
  type Query {
    # Get a single user by ID
    user(id: ID!): User
    # Get all users, optionally filtered by name
    users(name: String): [User!]!
    # Get a single post by ID
    post(id: ID!): Post
    # Get all posts
    posts: [Post!]!
  }

  # The User type defines what fields a user object has
  type User {
    id: ID!
    name: String!
    email: String!
    age: Int
    # A user can have many posts — this is a nested field with its own resolver
    posts: [Post!]!
  }

  # The Post type
  type Post {
    id: ID!
    title: String!
    body: String!
    authorId: ID!
    # Resolve the author relationship
    author: User!
  }
`;

// Resolvers are functions that fetch data for each field
// Each field in the schema can have a resolver
const resolvers = {
  Query: {
    // (parent, args, context, info) — args contains the query arguments
    user: (_, args) => {
      // Find user by ID — args.id comes from the query: user(id: "1")
      return users.find((u) => u.id === args.id) || null;
    },

    users: (_, args) => {
      // Filter by name if the name argument is provided
      if (args.name) {
        return users.filter((u) =>
          u.name.toLowerCase().includes(args.name.toLowerCase())
        );
      }
      return users;
    },

    post: (_, args) => {
      return posts.find((p) => p.id === args.id) || null;
    },

    posts: () => {
      return posts;  // Return all posts
    },
  },

  // Resolvers for the User type's nested fields
  User: {
    // parent is the User object — we use it to find their posts
    posts: (parent) => {
      return posts.filter((p) => p.authorId === parent.id);
    },
  },

  // Resolvers for the Post type's nested fields
  Post: {
    author: (parent) => {
      // parent is the Post object — find the author by authorId
      return users.find((u) => u.id === parent.authorId);
    },
  },
};

// Create the GraphQL schema from typeDefs and resolvers
const schema = createSchema({ typeDefs, resolvers });

// Create the Yoga server — Yoga is a GraphQL server built on standard Request/Response
const yoga = createYoga({ schema });

// Yoga works with any HTTP framework or plain Node.js http
const server = createServer(yoga);

const PORT = 4000;
server.listen(PORT, () => {
  console.log(`GraphQL server on http://localhost:${PORT}/graphql`);
  console.log('Open GraphiQL at http://localhost:4000/graphql');
});
```

## How It Works

### The SDL (Schema Definition Language)

The schema defines **what** the API can do. It is a type system:

```graphql
type Query {
  user(id: ID!): User    # Returns a User, requires an ID argument (! = required)
  users: [User!]!        # Returns a non-null list of non-null Users
}
```

- `ID!` — required unique identifier
- `String` — optional string (no `!` means nullable)
- `[User!]!` — required list of required Users
- `Query` — the root type for read operations

### Resolver Arguments

Every resolver receives four arguments:

```js
user: (parent, args, context, info) => { ... }
```

| Argument | Description |
|----------|-------------|
| `parent` | The result from the parent field (null for root queries) |
| `args` | The arguments passed in the query |
| `context` | Shared data across all resolvers (auth, DB connection) |
| `info` | AST metadata about the query (rarely used) |

### The Yoga Server

`graphql-yoga` is a batteries-included GraphQL server. It provides:
- A `/graphql` endpoint for queries and mutations
- **GraphiQL** — a built-in IDE for testing queries (open in browser)
- File uploads, subscriptions, and more out of the box

## Testing with GraphiQL

Open `http://localhost:4000/graphql` in your browser. You will see the GraphiQL IDE. Try this query:

```graphql
query {
  users {
    name
    email
    posts {
      title
    }
  }
}
```

Response:

```json
{
  "data": {
    "users": [
      {
        "name": "Alice",
        "email": "alice@example.com",
        "posts": [
          { "title": "Hello GraphQL" },
          { "title": "REST vs GraphQL" }
        ]
      },
      {
        "name": "Bob",
        "email": "bob@example.com",
        "posts": [
          { "title": "Node.js Tips" }
        ]
      },
      {
        "name": "Charlie",
        "email": "charlie@example.com",
        "posts": []
      }
    ]
  }
}
```

Notice: Charlie has no posts, and the response only includes `name`, `email`, and `posts.title` — exactly what we asked for.

## Common Mistakes

### Mistake 1: Forgetting the ! on Return Types

```graphql
# WRONG — nullable lists can cause confusing null responses
type Query {
  users: [User]    # List might be null, or contain null elements
}

# CORRECT — be explicit about nullability
type Query {
  users: [User!]!  # List is never null, elements are never null
}
```

### Mistake 2: Missing Resolver for a Field

```js
// WRONG — if the schema defines Post.author but no resolver exists,
// GraphQL returns null for that field (silently)
const resolvers = {
  Query: { post: (_, args) => posts.find(p => p.id === args.id) },
  // Missing Post.author resolver!
};

// CORRECT — provide resolvers for all non-scalar fields
const resolvers = {
  Query: { post: (_, args) => posts.find(p => p.id === args.id) },
  Post: {
    author: (parent) => users.find(u => u.id === parent.authorId),
  },
};
```

### Mistake 3: Using REST-Style Endpoints

```js
// WRONG — GraphQL uses a single endpoint
app.get('/graphql/users', ...);     // This is REST, not GraphQL
app.get('/graphql/users/:id', ...); // This is REST, not GraphQL

// CORRECT — one endpoint, query describes the data
app.all('/graphql', yoga.handleRequest);  // All queries go here
```

## Try It Yourself

### Exercise 1: Add a Field

Add a `postCount` field to the `User` type. It should return the number of posts the user has written. Add the corresponding resolver.

### Exercise 2: Filter Posts

Add an `authorId` argument to the `posts` query so clients can fetch posts by a specific author.

### Exercise 3: Add Comments

Create a `Comment` type with `id`, `text`, and `postId` fields. Add a `comments` field to `Post` that returns the post's comments.

## Next Steps

You have a working GraphQL server with read operations. Let's add write operations. Continue to [Queries & Mutations](./02-queries-mutations.md).
