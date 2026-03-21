# GraphQL with Express

## 📌 What You'll Learn

- How GraphQL differs from REST
- Setting up GraphQL with Apollo Server in Express
- Defining schemas and resolvers
- Query and mutation patterns

## 🧠 Concept Explained (Plain English)

**GraphQL** is a query language for APIs that lets clients request exactly the data they need. Unlike REST, where each endpoint returns fixed data, GraphQL lets clients specify what fields they want.

**REST vs GraphQL:**
- REST: `/users/1` returns all user fields
- GraphQL: `query { user(id: 1) { name email } }` returns only name and email

**Key concepts:**
- **Schema**: Defines types and operations
- **Resolver**: Functions that fetch data
- **Query**: Read operations (like GET)
- **Mutation**: Write operations (like POST/PUT/DELETE)

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';
import { ApolloServer } from '@apollo/server';
import { expressMiddleware } from '@apollo/server/express4';

const app = express();

// GraphQL schema
const typeDefs = `#graphql
  type User {
    id: ID!
    name: String!
    email: String!
    posts: [Post]
  }
  
  type Post {
    id: ID!
    title: String!
    content: String
    author: User
  }
  
  type Query {
    users: [User]
    user(id: ID!): User
    posts: [Post]
    post(id: ID!): Post
  }
  
  type Mutation {
    createUser(name: String!, email: String!): User
    createPost(title: String!, content: String, authorId: ID!): Post
  }
`;

// Mock data
const users = [
  { id: '1', name: 'John', email: 'john@example.com' },
  { id: '2', name: 'Jane', email: 'jane@example.com' }
];

const posts = [
  { id: 'p1', title: 'Hello World', content: 'First post', authorId: '1' }
];

// Resolvers
const resolvers = {
  Query: {
    users: () => users,
    user: (_, { id }) => users.find(u => u.id === id),
    posts: () => posts,
    post: (_, { id }) => posts.find(p => p.id === id)
  },
  
  Mutation: {
    createUser: (_, { name, email }) => {
      const user = { id: String(users.length + 1), name, email };
      users.push(user);
      return user;
    },
    createPost: (_, { title, content, authorId }) => {
      const post = { id: 'p' + Date.now(), title, content, authorId };
      posts.push(post);
      return post;
    }
  },
  
  User: {
    posts: (user) => posts.filter(p => p.authorId === user.id)
  },
  
  Post: {
    author: (post) => users.find(u => u.id === post.authorId)
  }
};

// Create Apollo Server
const server = new ApolloServer({ typeDefs, resolvers });
await server.start();

// Middleware
app.use('/graphql', express.json(), expressMiddleware(server));

app.get('/health', (req, res) => res.json({ status: 'ok' }));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server: ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 14-37 | typeDefs | GraphQL schema definition |
| 40-46 | Mock data | Sample data |
| 49-68 | resolvers | Data fetching functions |
| 73-77 | Apollo Server | Creates GraphQL server |
| 80 | Middleware | Connects to Express |

## ✅ Quick Recap

- GraphQL lets clients request exact data needed
- Schema defines types, resolvers fetch data
- Queries for reading, mutations for writing

## 🔗 What's Next

Learn about [Server-Sent Events](./02_server-sent-events.md).
