# Queries & Mutations

## What You'll Learn

- How mutations modify data (create, update, delete)
- How to define input types for structured arguments
- How to handle errors in GraphQL (the `errors` array)
- How to validate inputs and return meaningful error messages
- How queries and mutations differ in practice

## Mutations

Mutations are GraphQL's way of **writing data**. They work exactly like queries but semantically indicate a change:

```graphql
# Query — read data (no side effects)
query { users { name } }

# Mutation — write data (has side effects)
mutation { createUser(name: "Alice", email: "alice@test.com") { id name } }
```

## Complete Server with Mutations

```js
// server.js — GraphQL server with queries and mutations

import { createServer } from 'node:http';
import { createSchema, createYoga } from 'graphql-yoga';

// In-memory stores
const users = [
  { id: '1', name: 'Alice', email: 'alice@example.com' },
  { id: '2', name: 'Bob', email: 'bob@example.com' },
];

const posts = [
  { id: '1', title: 'Hello', body: 'First post', authorId: '1' },
];

let nextUserId = 3;
let nextPostId = 2;

const typeDefs = /* GraphQL */ `
  type Query {
    user(id: ID!): User
    users: [User!]!
    post(id: ID!): Post
    posts: [Post!]!
  }

  type Mutation {
    # Create a new user — returns the created user
    createUser(input: CreateUserInput!): User!
    # Update an existing user — all fields optional
    updateUser(id: ID!, input: UpdateUserInput!): User
    # Delete a user — returns true on success
    deleteUser(id: ID!): Boolean!
    # Create a new post
    createPost(input: CreatePostInput!): Post!
  }

  # Input types group related arguments into a single object
  # This is cleaner than having 5+ arguments on a mutation
  input CreateUserInput {
    name: String!
    email: String!
  }

  input UpdateUserInput {
    name: String
    email: String
  }

  input CreatePostInput {
    title: String!
    body: String!
    authorId: ID!
  }

  type User {
    id: ID!
    name: String!
    email: String!
    posts: [Post!]!
  }

  type Post {
    id: ID!
    title: String!
    body: String!
    author: User!
  }
`;

const resolvers = {
  Query: {
    user: (_, { id }) => users.find((u) => u.id === id) || null,
    users: () => users,
    post: (_, { id }) => posts.find((p) => p.id === id) || null,
    posts: () => posts,
  },

  Mutation: {
    createUser: (_, { input }) => {
      // Check for duplicate email — return a user-friendly error
      const existing = users.find((u) => u.email === input.email);
      if (existing) {
        // Throw a GraphQLError for structured error reporting
        throw new Error(`A user with email "${input.email}" already exists`);
      }

      const user = {
        id: String(nextUserId++),
        name: input.name,
        email: input.email,
      };
      users.push(user);
      return user;  // Return the created object — client picks which fields to get
    },

    updateUser: (_, { id, input }) => {
      const user = users.find((u) => u.id === id);
      if (!user) {
        throw new Error(`User with id "${id}" not found`);
      }

      // Only update fields that were provided (partial update)
      if (input.name !== undefined) user.name = input.name;
      if (input.email !== undefined) user.email = input.email;

      return user;
    },

    deleteUser: (_, { id }) => {
      const index = users.findIndex((u) => u.id === id);
      if (index === -1) return false;

      users.splice(index, 1);  // Remove the user from the array
      return true;
    },

    createPost: (_, { input }) => {
      // Validate that the author exists
      const author = users.find((u) => u.id === input.authorId);
      if (!author) {
        throw new Error(`Author with id "${input.authorId}" not found`);
      }

      const post = {
        id: String(nextPostId++),
        title: input.title,
        body: input.body,
        authorId: input.authorId,
      };
      posts.push(post);
      return post;
    },
  },

  User: {
    posts: (parent) => posts.filter((p) => p.authorId === parent.id),
  },

  Post: {
    author: (parent) => users.find((u) => u.id === parent.authorId),
  },
};

const schema = createSchema({ typeDefs, resolvers });
const yoga = createYoga({ schema });
const server = createServer(yoga);

server.listen(4000, () => {
  console.log('GraphQL server on http://localhost:4000/graphql');
});
```

## Testing Mutations in GraphiQL

### Create a User

```graphql
mutation {
  createUser(input: { name: "Diana", email: "diana@example.com" }) {
    id
    name
    email
  }
}
```

Response:

```json
{
  "data": {
    "createUser": {
      "id": "3",
      "name": "Diana",
      "email": "diana@example.com"
    }
  }
}
```

### Create a Post

```graphql
mutation {
  createPost(input: { title: "My Post", body: "Content here", authorId: "3" }) {
    id
    title
    author {
      name
    }
  }
}
```

### Error Response

```graphql
mutation {
  createUser(input: { name: "Eve", email: "alice@example.com" }) {
    id
  }
}
```

Response:

```json
{
  "data": null,
  "errors": [
    {
      "message": "A user with email \"alice@example.com\" already exists",
      "locations": [{ "line": 2, "column": 3 }],
      "path": ["createUser"]
    }
  ]
}
```

## How It Works

### Input Types

Input types group related arguments. Instead of:

```graphql
createUser(name: String!, email: String!): User!
```

You write:

```graphql
createUser(input: CreateUserInput!): User!
```

This is cleaner, easier to extend, and works better with client code generators.

### The `!` (Required) Marker

```graphql
createUser(input: CreateUserInput!): User!
#              ^^^^^^^^^^^^^^^^^^^^  ^^^^^
#              argument is required  return value is never null
```

If a client sends `null` for a required argument, GraphQL rejects the request before any resolver runs.

### Error Handling

GraphQL always returns a 200 status code. Errors are in the response body:

```json
{
  "data": { "createUser": null },
  "errors": [{ "message": "..." }]
}
```

This is different from REST, where 4xx/5xx status codes indicate errors.

## Common Mistakes

### Mistake 1: Using Mutations for Reads

```graphql
# WRONG — getUser is a read operation, use Query not Mutation
type Mutation {
  getUser(id: ID!): User  # Mutations should change data
}

# CORRECT — reads belong in Query
type Query {
  getUser(id: ID!): User
}
```

### Mistake 2: Not Returning the Modified Object

```graphql
# WRONG — client has to re-query to see the result
type Mutation {
  updateUser(id: ID!, name: String!): Boolean!  # Returns true/false
}

# CORRECT — return the updated object so the client can select fields
type Mutation {
  updateUser(id: ID!, input: UpdateUserInput!): User
}
```

### Mistake 3: No Input Validation

```js
// WRONG — accept any input without checking
createUser: (_, { input }) => {
  // What if name is empty? What if email is invalid?
  const user = { id: nextId++, ...input };
  users.push(user);
  return user;
};

// CORRECT — validate before creating
createUser: (_, { input }) => {
  if (!input.name.trim()) throw new Error('Name cannot be empty');
  if (!input.email.includes('@')) throw new Error('Invalid email');
  // ... proceed with creation
};
```

## Try It Yourself

### Exercise 1: Update and Delete Posts

Add `updatePost(id, input)` and `deletePost(id)` mutations. The update should allow changing `title` and `body`. The delete should return `true` or `false`.

### Exercise 2: Batch Create

Add a `createUsers(input: [CreateUserInput!]!)` mutation that creates multiple users at once. Return an array of the created users.

### Exercise 3: Custom Error Type

Instead of throwing plain `Error` objects, create a `UserError` type in your schema with `field` and `message` fields. Return structured errors so clients know which field failed.

## Next Steps

You can create and modify data. For real-time updates when data changes, continue to [Subscriptions](./03-subscriptions.md).
