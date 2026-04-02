# GraphQL and Socket.io Authentication

## What You'll Learn

- GraphQL authentication context setup
- GraphQL authorization directives
- Socket.io JWT authentication
- WebSocket authentication patterns

## GraphQL Authentication

```javascript
import { ApolloServer } from '@apollo/server';
import { expressMiddleware } from '@apollo/server/express4';
import { GraphQLError } from 'graphql';
import jwt from 'jsonwebtoken';

const typeDefs = `#graphql
    directive @auth(requires: Role = USER) on FIELD_DEFINITION

    enum Role {
        USER
        ADMIN
    }

    type Query {
        me: User! @auth
        users: [User!]! @auth(requires: ADMIN)
        publicPosts: [Post!]!
    }

    type Mutation {
        login(email: String!, password: String!): AuthPayload!
        createPost(title: String!, content: String!): Post! @auth
        deleteUser(id: ID!): Boolean! @auth(requires: ADMIN)
    }

    type User {
        id: ID!
        email: String!
        name: String!
        role: Role!
    }

    type Post {
        id: ID!
        title: String!
        content: String!
        author: User!
    }

    type AuthPayload {
        accessToken: String!
        refreshToken: String!
        user: User!
    }
`;

const resolvers = {
    Query: {
        me: (_, __, { user }) => {
            return User.findById(user.id);
        },
        users: (_, __, { user }) => {
            if (user.role !== 'admin') {
                throw new GraphQLError('Not authorized', {
                    extensions: { code: 'FORBIDDEN' },
                });
            }
            return User.findAll();
        },
        publicPosts: () => Post.findPublic(),
    },
    Mutation: {
        login: async (_, { email, password }) => {
            const user = await User.findByEmail(email);
            if (!user || !await bcrypt.compare(password, user.passwordHash)) {
                throw new GraphQLError('Invalid credentials', {
                    extensions: { code: 'UNAUTHENTICATED' },
                });
            }

            return {
                accessToken: generateAccessToken(user),
                refreshToken: await generateRefreshToken(user),
                user,
            };
        },
        createPost: (_, { title, content }, { user }) => {
            return Post.create({ title, content, authorId: user.id });
        },
    },
};

// Auth plugin for Apollo Server
const authPlugin = {
    async requestDidStart({ request }) {
        const token = request.http?.headers.get('authorization')
            ?.replace('Bearer ', '');

        if (token) {
            try {
                const decoded = jwt.verify(token, process.env.JWT_SECRET);
                return {
                    async didResolveOperation({ contextValue }) {
                        contextValue.user = decoded;
                    },
                };
            } catch {
                // Token invalid — will be caught by @auth directive
            }
        }
    },
};

const server = new ApolloServer({
    typeDefs,
    resolvers,
    plugins: [authPlugin],
});

// Context function
app.use('/graphql',
    express.json(),
    expressMiddleware(server, {
        context: async ({ req }) => {
            const token = req.headers.authorization?.replace('Bearer ', '');
            let user = null;

            if (token) {
                try {
                    user = jwt.verify(token, process.env.JWT_SECRET);
                } catch { /* invalid token */ }
            }

            return { user };
        },
    })
);
```

## Socket.io Authentication

```javascript
import { Server } from 'socket.io';
import jwt from 'jsonwebtoken';

const io = new Server(server, {
    cors: {
        origin: 'https://example.com',
        credentials: true,
    },
});

// Authentication middleware
io.use((socket, next) => {
    const token =
        socket.handshake.auth.token ||
        socket.handshake.headers.authorization?.replace('Bearer ', '');

    if (!token) {
        return next(new Error('Authentication required'));
    }

    try {
        const decoded = jwt.verify(token, process.env.JWT_SECRET);
        socket.user = decoded;
        next();
    } catch (err) {
        next(new Error('Invalid token'));
    }
});

io.on('connection', (socket) => {
    console.log(`User ${socket.user.sub} connected`);

    // Join user-specific room
    socket.join(`user:${socket.user.sub}`);

    // Role-based rooms
    if (socket.user.role === 'admin') {
        socket.join('admins');
    }

    // Protected event handlers
    socket.on('sendMessage', (data) => {
        // Verify user can send to this room
        if (!data.roomId) return;

        io.to(`room:${data.roomId}`).emit('message', {
            from: socket.user.sub,
            content: data.content,
            timestamp: Date.now(),
        });
    });

    // Admin-only events
    socket.on('broadcast', (data) => {
        if (socket.user.role !== 'admin') {
            socket.emit('error', { message: 'Admin only' });
            return;
        }
        io.emit('announcement', data);
    });

    socket.on('disconnect', () => {
        console.log(`User ${socket.user.sub} disconnected`);
    });
});

// Token refresh for long-lived connections
io.use((socket, next) => {
    const token = socket.handshake.auth.token;
    try {
        const decoded = jwt.verify(token, process.env.JWT_SECRET);

        // Check if token expires soon
        const expiresIn = decoded.exp - Math.floor(Date.now() / 1000);
        if (expiresIn < 300) { // Less than 5 minutes
            socket.emit('token:refresh', {
                message: 'Token expiring soon, please refresh',
            });
        }

        socket.user = decoded;
        next();
    } catch (err) {
        next(new Error('Invalid token'));
    }
});
```

## Common Mistakes

- Not authenticating WebSocket connections (anyone can connect)
- Not refreshing tokens for long-lived connections
- Not validating authorization in GraphQL resolvers
- Exposing sensitive fields in GraphQL schema

## Cross-References

- See [Express Integration](./01-express-fastify.md) for REST auth
- See [JWT Refresh](../jwt/04-jwt-refresh-tokens.md) for token management
- See [Security](../06-authentication-security/02-csrf-session-protection.md) for hardening

## Next Steps

Continue to [Monitoring: Audit Trail](../10-authentication-monitoring/02-audit-trail.md).
