# Application Scenarios and Architecture Patterns

## What You'll Learn

- Real-world application architecture patterns
- When to use Node.js for specific scenarios
- Performance characteristics by application type
- Migration strategies from other platforms

## Application Type Deep Dives

### REST API Server

```javascript
// production-api.js — Production-ready REST API pattern

import express from 'express';
import helmet from 'helmet';
import cors from 'cors';
import compression from 'compression';
import rateLimit from 'express-rate-limit';

const app = express();

// Security middleware
app.use(helmet());
app.use(cors({ origin: process.env.ALLOWED_ORIGINS?.split(',') }));
app.use(compression());
app.use(express.json({ limit: '10mb' }));

// Rate limiting
const limiter = rateLimit({
    windowMs: 15 * 60 * 1000,
    max: 100,
    standardHeaders: true,
});
app.use('/api/', limiter);

// Routes
app.get('/api/users', async (req, res, next) => {
    try {
        const users = await userService.list(req.query);
        res.json({ data: users, meta: { total: users.length } });
    } catch (err) {
        next(err);
    }
});

// Error handling
app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(err.status || 500).json({
        error: { message: err.message, code: err.code }
    });
});

app.listen(process.env.PORT || 3000);
```

**Performance Profile:**
```
REST API Performance (Node.js):
─────────────────────────────────────────────
Concurrent connections:  10,000+
Requests/sec:            30,000-50,000
Avg response time:       5-15ms
Memory per connection:   ~1KB
CPU utilization:         Low (I/O bound)
Best frameworks:         Express, Fastify, Hono
```

### Real-Time WebSocket Server

```javascript
// websocket-server.js — Real-time chat/notifications

import { WebSocketServer } from 'ws';
import { createServer } from 'node:http';

const server = createServer();
const wss = new WebSocketServer({ server });

// Room-based broadcasting
const rooms = new Map();

wss.on('connection', (ws, req) => {
    const userId = authenticate(req);
    ws.userId = userId;
    
    ws.on('message', (data) => {
        const msg = JSON.parse(data);
        
        switch (msg.type) {
            case 'join':
                joinRoom(ws, msg.room);
                break;
            case 'message':
                broadcastToRoom(msg.room, {
                    type: 'message',
                    userId,
                    text: msg.text,
                    timestamp: Date.now(),
                });
                break;
        }
    });
    
    ws.on('close', () => {
        leaveAllRooms(ws);
    });
});

function broadcastToRoom(room, message) {
    const clients = rooms.get(room) || new Set();
    const payload = JSON.stringify(message);
    
    for (const client of clients) {
        if (client.readyState === 1) { // OPEN
            client.send(payload);
        }
    }
}

server.listen(8080);
```

**Performance Profile:**
```
WebSocket Performance (Node.js):
─────────────────────────────────────────────
Concurrent connections:  100,000+
Messages/sec:            500,000+
Memory per connection:   ~5KB
Best libraries:          ws, Socket.io, uWebSockets
Scaling:                 Redis adapter for multi-process
```

### Streaming Data Pipeline

```javascript
// stream-pipeline.js — ETL data processing

import { createReadStream, createWriteStream } from 'node:fs';
import { Transform, pipeline } from 'node:stream';
import { createGzip } from 'node:zlib';
import { parse } from 'csv-parse';

// Transform: Filter and enrich records
const enrichRecords = new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        // Filter
        if (record.status !== 'active') return callback();
        
        // Enrich
        record.processedAt = new Date().toISOString();
        record.fullName = `${record.firstName} ${record.lastName}`;
        
        callback(null, record);
    }
});

// Transform: Format output
const formatOutput = new Transform({
    objectMode: true,
    transform(record, encoding, callback) {
        callback(null, JSON.stringify(record) + '\n');
    }
});

// Process 10GB file with 100MB memory
pipeline(
    createReadStream('input.csv'),
    parse({ columns: true }),
    enrichRecords,
    formatOutput,
    createGzip(),
    createWriteStream('output.json.gz'),
    (err) => {
        if (err) console.error('Pipeline failed:', err);
        else console.log('Pipeline complete');
    }
);
```

**Performance Profile:**
```
Streaming Performance (Node.js):
─────────────────────────────────────────────
Throughput:     100-500 MB/sec (depends on transforms)
Memory usage:   ~64MB (regardless of file size)
CPU:            Depends on transform complexity
Best for:       ETL, log processing, data migration
```

### GraphQL API Server

```javascript
// graphql-server.js — GraphQL API

import { ApolloServer } from '@apollo/server';
import { startStandaloneServer } from '@apollo/server/standalone';

const typeDefs = `#graphql
    type User {
        id: ID!
        name: String!
        email: String!
        posts: [Post!]!
    }
    
    type Post {
        id: ID!
        title: String!
        content: String!
        author: User!
    }
    
    type Query {
        users: [User!]!
        user(id: ID!): User
        posts(limit: Int = 10): [Post!]!
    }
    
    type Mutation {
        createUser(name: String!, email: String!): User!
        createPost(title: String!, content: String!, authorId: ID!): Post!
    }
`;

const resolvers = {
    Query: {
        users: (_, __, { dataSources }) => dataSources.userAPI.getAll(),
        user: (_, { id }, { dataSources }) => dataSources.userAPI.getById(id),
        posts: (_, { limit }, { dataSources }) => dataSources.postAPI.getRecent(limit),
    },
    User: {
        posts: (user, _, { dataSources }) => dataSources.postAPI.getByAuthor(user.id),
    },
};

const server = new ApolloServer({ typeDefs, resolvers });
const { url } = await startStandaloneServer(server, { listen: { port: 4000 } });
console.log(`GraphQL server at ${url}`);
```

### CLI Tool

```javascript
#!/usr/bin/env node
// cli-tool.js — Command-line interface tool

import { parseArgs } from 'node:util';
import { readFile, writeFile } from 'node:fs/promises';

const { values, positionals } = parseArgs({
    options: {
        output: { type: 'string', short: 'o' },
        format: { type: 'string', short: 'f', default: 'json' },
        verbose: { type: 'boolean', short: 'v', default: false },
        help: { type: 'boolean', short: 'h' },
    },
    allowPositionals: true,
    strict: true,
});

if (values.help || positionals.length === 0) {
    console.log(`
Usage: mycli [options] <file>

Options:
  -o, --output <file>   Output file (default: stdout)
  -f, --format <type>   Output format: json, csv, table
  -v, --verbose         Verbose output
  -h, --help            Show help
    `);
    process.exit(0);
}

async function main() {
    const inputFile = positionals[0];
    const data = await readFile(inputFile, 'utf-8');
    
    if (values.verbose) console.error(`Processing ${inputFile}...`);
    
    const result = processFile(data, values.format);
    
    if (values.output) {
        await writeFile(values.output, result);
        if (values.verbose) console.error(`Written to ${values.output}`);
    } else {
        console.log(result);
    }
}

main().catch(err => {
    console.error('Error:', err.message);
    process.exit(1);
});
```

## Architecture Decision Matrix

```
Architecture Pattern Selection:
─────────────────────────────────────────────
Monolith:
├── Simple CRUD API ✓
├── Small team (< 5 developers) ✓
├── Low traffic (< 10K req/sec) ✓
└── Node.js: Express, Fastify, Koa

Microservices:
├── Multiple independent services ✓
├── Large team (> 10 developers) ✓
├── High traffic (> 100K req/sec) ✓
└── Node.js: Each service in Express/Fastify

Serverless:
├── Event-driven workloads ✓
├── Variable traffic patterns ✓
├── Cost optimization priority ✓
└── Node.js: AWS Lambda, Vercel, Cloudflare Workers

Event-Driven:
├── Real-time features critical ✓
├── Complex async workflows ✓
├── High throughput requirements ✓
└── Node.js: EventEmitter, Bull, BullMQ
```

## Cross-References

- See [Real-World Cases](../11-real-world-cases/01-industry-implementations.md) for production examples
- See [Express Practical Intro](../15-express-practical-intro/01-basic-server.md) for web framework
- See [Event Loop Mechanics](../06-event-loop-mechanics/01-event-loop-deep-dive.md) for async model

## Next Steps

Continue to [Team and Project Considerations](./03-team-project-considerations.md) for organizational guidance.
