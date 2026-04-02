# Express and Fastify Authentication Integration

## What You'll Learn

- Express.js authentication middleware
- Fastify.js authentication plugins
- GraphQL authentication
- Socket.io authentication
- REST API auth patterns

## Express.js Authentication Middleware

```javascript
import express from 'express';
import jwt from 'jsonwebtoken';
import bcrypt from 'bcrypt';

const app = express();
app.use(express.json());

// Authentication middleware factory
function createAuthMiddleware(options = {}) {
    const {
        secret = process.env.JWT_SECRET,
        algorithms = ['HS256'],
        required = true,
    } = options;

    return (req, res, next) => {
        const authHeader = req.headers.authorization;

        if (!authHeader?.startsWith('Bearer ')) {
            if (!required) return next();
            return res.status(401).json({ error: 'Authentication required' });
        }

        const token = authHeader.slice(7);

        try {
            const decoded = jwt.verify(token, secret, { algorithms });
            req.user = decoded;
            req.token = token;
            next();
        } catch (err) {
            return res.status(401).json({
                error: err.name === 'TokenExpiredError' ? 'Token expired' : 'Invalid token',
            });
        }
    };
}

// Authorization middleware factory
function authorize(...roles) {
    return (req, res, next) => {
        if (!req.user) {
            return res.status(401).json({ error: 'Authentication required' });
        }

        if (roles.length && !roles.includes(req.user.role)) {
            return res.status(403).json({ error: 'Insufficient permissions' });
        }

        next();
    };
}

// Routes
app.post('/auth/register', async (req, res) => {
    const { email, password } = req.body;
    const hash = await bcrypt.hash(password, 12);
    const user = await db.users.create({ email, passwordHash: hash });
    res.status(201).json({ id: user.id, email: user.email });
});

app.post('/auth/login', async (req, res) => {
    const { email, password } = req.body;
    const user = await db.users.findByEmail(email);

    if (!user || !await bcrypt.compare(password, user.passwordHash)) {
        return res.status(401).json({ error: 'Invalid credentials' });
    }

    const accessToken = jwt.sign(
        { sub: user.id, role: user.role },
        process.env.JWT_SECRET,
        { expiresIn: '15m' }
    );

    const refreshToken = jwt.sign(
        { sub: user.id, type: 'refresh' },
        process.env.JWT_REFRESH_SECRET,
        { expiresIn: '7d' }
    );

    res.json({ accessToken, refreshToken });
});

// Protected routes
app.get('/api/profile',
    createAuthMiddleware(),
    (req, res) => {
        res.json({ user: req.user });
    }
);

app.get('/api/admin/users',
    createAuthMiddleware(),
    authorize('admin'),
    (req, res) => {
        res.json({ users: [] });
    }
);
```

## Fastify.js Authentication Plugin

```javascript
import Fastify from 'fastify';
import fastifyJwt from '@fastify/jwt';
import fastifyCookie from '@fastify/cookie';

const fastify = Fastify({ logger: true });

// Register JWT plugin
await fastify.register(fastifyJwt, {
    secret: process.env.JWT_SECRET,
    sign: { expiresIn: '15m' },
    cookie: { cookieName: 'token' },
});

await fastify.register(fastifyCookie);

// Decorator for authentication
fastify.decorate('authenticate', async (request, reply) => {
    try {
        await request.jwtVerify();
    } catch (err) {
        reply.code(401).send({ error: 'Authentication required' });
    }
});

// Role-based authorization decorator
fastify.decorate('authorize', (...roles) => {
    return async (request, reply) => {
        if (!roles.includes(request.user.role)) {
            reply.code(403).send({ error: 'Insufficient permissions' });
        }
    };
});

// Login route
fastify.post('/auth/login', async (request, reply) => {
    const { email, password } = request.body;
    const user = await db.users.findByEmail(email);

    if (!user || !await bcrypt.compare(password, user.passwordHash)) {
        return reply.code(401).send({ error: 'Invalid credentials' });
    }

    const token = fastify.jwt.sign({ sub: user.id, role: user.role });
    return { accessToken: token };
});

// Protected routes
fastify.get('/api/profile', {
    preHandler: [fastify.authenticate],
}, async (request) => {
    return { user: request.user };
});

fastify.get('/api/admin', {
    preHandler: [fastify.authenticate, fastify.authorize('admin')],
}, async () => {
    return { data: 'admin only' };
});
```

## GraphQL Authentication

```javascript
import { ApolloServer } from '@apollo/server';
import { expressMiddleware } from '@apollo/server/express4';
import jwt from 'jsonwebtoken';

const typeDefs = `#graphql
    type Query {
        me: User
        users: [User!]!
    }

    type Mutation {
        login(email: String!, password: String!): AuthPayload!
        register(email: String!, password: String!): AuthPayload!
    }

    type User {
        id: ID!
        email: String!
        role: String!
    }

    type AuthPayload {
        accessToken: String!
        user: User!
    }
`;

const resolvers = {
    Query: {
        me: (_, __, { user }) => {
            if (!user) throw new GraphQLError('Not authenticated', {
                extensions: { code: 'UNAUTHENTICATED' },
            });
            return user;
        },
        users: (_, __, { user }) => {
            if (user?.role !== 'admin') {
                throw new GraphQLError('Not authorized', {
                    extensions: { code: 'FORBIDDEN' },
                });
            }
            return db.users.findAll();
        },
    },
    Mutation: {
        login: async (_, { email, password }) => {
            const user = await db.users.findByEmail(email);
            if (!user || !await bcrypt.compare(password, user.passwordHash)) {
                throw new GraphQLError('Invalid credentials', {
                    extensions: { code: 'UNAUTHENTICATED' },
                });
            }
            const accessToken = jwt.sign(
                { sub: user.id, role: user.role },
                process.env.JWT_SECRET,
                { expiresIn: '15m' }
            );
            return { accessToken, user };
        },
    },
};

const server = new ApolloServer({ typeDefs, resolvers });

app.use('/graphql',
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
    cors: { origin: 'https://example.com' },
});

// Authentication middleware for Socket.io
io.use((socket, next) => {
    const token = socket.handshake.auth.token ||
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

    // Role-based room
    if (socket.user.role === 'admin') {
        socket.join('admins');
    }

    socket.on('message', (data) => {
        // Broadcast to user's room
        io.to(`user:${data.recipientId}`).emit('message', {
            from: socket.user.sub,
            content: data.content,
        });
    });

    socket.on('disconnect', () => {
        console.log(`User ${socket.user.sub} disconnected`);
    });
});
```

## Best Practices Checklist

- [ ] Use framework-specific auth plugins when available
- [ ] Implement auth middleware as reusable factories
- [ ] Extract user from token in GraphQL context
- [ ] Authenticate Socket.io connections on handshake
- [ ] Use role-based authorization decorators
- [ ] Handle token expiration gracefully in all protocols

## Cross-References

- See [JWT Refresh](../jwt/04-jwt-refresh-tokens.md) for token management
- See [Security](../06-authentication-security/01-security-headers.md) for hardening
- See [Performance](../08-authentication-performance/01-performance-optimization.md) for optimization

## Next Steps

Continue to [Monitoring and Observability](../10-authentication-monitoring/01-monitoring-metrics.md).
