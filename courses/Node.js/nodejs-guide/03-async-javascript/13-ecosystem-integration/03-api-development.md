# Async API Development: REST, GraphQL, WebSockets

## What You'll Learn

- Async REST API patterns
- GraphQL async resolvers
- WebSocket async message handling
- API error handling patterns

## Async REST API

```javascript
import express from 'express';

const app = express();
app.use(express.json());

const asyncHandler = (fn) => (req, res, next) =>
    Promise.resolve(fn(req, res, next)).catch(next);

// Async CRUD endpoints
app.get('/api/posts', asyncHandler(async (req, res) => {
    const { page = 1, limit = 20 } = req.query;
    const posts = await db.query(
        'SELECT * FROM posts ORDER BY created_at DESC LIMIT $1 OFFSET $2',
        [limit, (page - 1) * limit]
    );
    const total = await db.query('SELECT COUNT(*) FROM posts');
    res.json({ data: posts, meta: { page: +page, limit: +limit, total: total.rows[0].count } });
}));

app.post('/api/posts', asyncHandler(async (req, res) => {
    const { title, content } = req.body;
    if (!title) return res.status(400).json({ error: 'Title required' });

    const post = await db.query(
        'INSERT INTO posts (title, content) VALUES ($1, $2) RETURNING *',
        [title, content]
    );
    res.status(201).json(post.rows[0]);
}));
```

## GraphQL Async Resolvers

```javascript
import { ApolloServer } from '@apollo/server';
import { startStandaloneServer } from '@apollo/server/standalone';

const typeDefs = `#graphql
    type User { id: ID!, name: String!, posts: [Post!]! }
    type Post { id: ID!, title: String!, author: User! }
    type Query { users: [User!]!, user(id: ID!): User }
`;

const resolvers = {
    Query: {
        users: async () => db.query('SELECT * FROM users').then(r => r.rows),
        user: async (_, { id }) => db.query('SELECT * FROM users WHERE id = $1', [id]).then(r => r.rows[0]),
    },
    User: {
        posts: async (user) => db.query('SELECT * FROM posts WHERE author_id = $1', [user.id]).then(r => r.rows),
    },
};

const server = new ApolloServer({ typeDefs, resolvers });
await startStandaloneServer(server, { listen: { port: 4000 } });
```

## WebSocket Async Patterns

```javascript
import { WebSocketServer } from 'ws';

const wss = new WebSocketServer({ port: 8080 });

wss.on('connection', (ws) => {
    ws.on('message', async (data) => {
        try {
            const message = JSON.parse(data);

            switch (message.type) {
                case 'subscribe':
                    ws.channels = ws.channels || new Set();
                    ws.channels.add(message.channel);
                    break;

                case 'publish':
                    // Async broadcast
                    const subscribers = getSubscribers(message.channel);
                    await Promise.allSettled(
                        subscribers.map(client => {
                            if (client.readyState === 1) {
                                client.send(JSON.stringify(message));
                            }
                        })
                    );
                    break;
            }
        } catch (err) {
            ws.send(JSON.stringify({ error: err.message }));
        }
    });
});
```

## Best Practices Checklist

- [ ] Use asyncHandler wrapper for Express routes
- [ ] Return promises from GraphQL resolvers
- [ ] Handle WebSocket message errors gracefully
- [ ] Implement proper API pagination
- [ ] Use connection pooling for database access

## Cross-References

- See [Frameworks](./01-frameworks.md) for HTTP frameworks
- See [Database Operations](./02-database-operations.md) for async DB patterns
- See [Error Handling](../07-async-error-handling/01-error-propagation.md) for error patterns

## Next Steps

This completes Chapter 3 of the Node.js guide. Proceed to [Chapter 4: npm and Packages](../../04-npm-and-packages/) for package management.
