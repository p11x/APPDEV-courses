# Serverless Deployment Patterns

## What You'll Learn

- AWS Lambda deployment patterns
- Cold start optimization
- Serverless API Gateway patterns
- Serverless database integration
- Serverless monitoring

## AWS Lambda Patterns

```javascript
// src/handler.js — API Gateway handler
import serverless from 'serverless-http';
import express from 'express';

const app = express();
app.use(express.json());

// Routes
app.get('/api/users', async (req, res) => {
    const users = await getUsers();
    res.json(users);
});

app.post('/api/users', async (req, res) => {
    const user = await createUser(req.body);
    res.status(201).json(user);
});

// Export for Lambda
export const handler = serverless(app, {
    binary: ['image/*', 'application/pdf'],
});

// Direct Lambda handler (no Express)
export const processOrder = async (event) => {
    const records = event.Records.map(r => JSON.parse(r.body));

    for (const order of records) {
        await processOrderItem(order);
    }

    return { statusCode: 200, body: JSON.stringify({ processed: records.length }) };
};
```

## Cold Start Optimization

```javascript
// 1. Move initialization outside handler (reused across invocations)
import { Pool } from 'pg';

const pool = new Pool({
    host: process.env.DB_HOST,
    database: process.env.DB_NAME,
    max: 1, // Lambda is single-concurrent
    connectionTimeoutMillis: 5000,
    idleTimeoutMillis: 60000,
});

// 2. Use Lambda layers for shared dependencies
// 3. Minimize bundle size
// 4. Use provisioned concurrency for critical paths

// serverless.yml cold start optimization
const serverlessConfig = {
    provider: {
        runtime: 'nodejs20.x',
        memorySize: 256, // More memory = faster cold starts
    },
    functions: {
        api: {
            handler: 'handler.handler',
            provisionedConcurrency: 5, // Keep 5 warm instances
            events: [{ http: { path: '/{proxy+}', method: 'ANY' } }],
        },
    },
    package: {
        individually: true,
        patterns: [
            '!**',
            'src/**',
            'node_modules/express/**',
            'node_modules/serverless-http/**',
        ],
    },
};
```

## DynamoDB Integration

```javascript
import { DynamoDBClient } from '@aws-sdk/client-dynamodb';
import {
    DynamoDBDocumentClient,
    GetCommand,
    PutCommand,
    QueryCommand,
} from '@aws-sdk/lib-dynamodb';

const client = new DynamoDBClient({ region: 'us-east-1' });
const ddb = DynamoDBDocumentClient.from(client);

// Single connection reuse (outside handler)
const TABLE = process.env.TABLE_NAME;

export const getUser = async (event) => {
    const { id } = event.pathParameters;

    const result = await ddb.send(new GetCommand({
        TableName: TABLE,
        Key: { id },
    }));

    if (!result.Item) {
        return { statusCode: 404, body: JSON.stringify({ error: 'Not found' }) };
    }

    return { statusCode: 200, body: JSON.stringify(result.Item) };
};

export const createUser = async (event) => {
    const user = JSON.parse(event.body);
    user.id = crypto.randomUUID();
    user.createdAt = new Date().toISOString();

    await ddb.send(new PutCommand({
        TableName: TABLE,
        Item: user,
    }));

    return { statusCode: 201, body: JSON.stringify(user) };
};
```

## SQS Event Processing

```javascript
// Process messages from SQS queue
export const processMessages = async (event) => {
    const results = { success: 0, failed: 0 };

    for (const record of event.Records) {
        try {
            const message = JSON.parse(record.body);
            await handleMessage(message);
            results.success++;
        } catch (err) {
            console.error('Failed to process message:', err);
            results.failed++;
            // Message will be retried or sent to DLQ
        }
    }

    return {
        batchItemFailures: event.Records
            .filter((_, i) => results.failed > 0)
            .map(r => ({ itemIdentifier: r.messageId })),
    };
};
```

## Serverless Best Practices

```
Serverless Optimization Checklist:
─────────────────────────────────────────────
Cold Start:
├── Initialize connections outside handler
├── Use provisioned concurrency for latency-critical
├── Minimize bundle size (tree-shake, exclude dev deps)
├── Use Lambda layers for shared code
└── Choose appropriate memory size (more = faster)

Cost:
├── Right-size memory allocation
├── Use ARM/Graviton (20% cheaper)
├── Set appropriate timeouts
├── Use SQS for async processing
└── Monitor with CloudWatch

Reliability:
├── Set DLQ for failed messages
├── Implement retry with backoff
├── Use idempotent operations
├── Handle partial failures in batch processing
└── Set alarms for error rates
```

## Best Practices Checklist

- [ ] Initialize connections outside handler function
- [ ] Use provisioned concurrency for critical paths
- [ ] Minimize deployment package size
- [ ] Use DynamoDB single-table design for simplicity
- [ ] Set DLQ for async processing failures
- [ ] Implement idempotent message processing
- [ ] Monitor cold start duration

## Cross-References

- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for patterns
- See [Cloud Platforms](../02-cloud-platforms/01-aws-deep-dive.md) for AWS setup
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for deployment automation

## Next Steps

Continue to [CI/CD Pipelines](../05-ci-cd-pipelines/01-github-actions.md).
