# Serverless API Gateway & Advanced Patterns

## What You'll Learn

- API Gateway patterns across AWS, Azure, and GCP
- Serverless database integration with DynamoDB, Cosmos DB, and Firestore
- Serverless authentication providers
- Cold start optimization and warm-up strategies
- Serverless testing strategies
- Event-driven serverless architectures

## API Gateway Patterns

### AWS API Gateway: REST vs HTTP API

```javascript
// serverless.yml — HTTP API (v2, lower latency, cheaper)
service: my-api

provider:
    name: aws
    runtime: nodejs20.x
    stage: ${opt:stage, 'dev'}

functions:
    # HTTP API (recommended — 70% cheaper, faster)
    httpApiHandler:
        handler: src/handlers.httpHandler
        events:
            - httpApi:
                  path: /users
                  method: GET
            - httpApi:
                  path: /users
                  method: POST
            - httpApi:
                  path: /users/{id}
                  method: GET

    # REST API (more features — API keys, usage plans, WAF)
    restApiHandler:
        handler: src/handlers.restHandler
        events:
            - rest:
                  path: /v1/users/{id}
                  method: get
                  private: true # Requires API key
                  request:
                      parameters:
                          paths:
                              id: true
```

```javascript
// src/handlers.js — Unified handler for both API types
const { DynamoDBDocument } = require('@aws-sdk/lib-dynamodb');
const { DynamoDBClient } = require('@aws-sdk/client-dynamodb');

const ddb = DynamoDBDocument.from(new DynamoDBClient({}));
const TABLE = process.env.TABLE_NAME;

// HTTP API handler (v2 event format)
exports.httpHandler = async (event) => {
    const method = event.requestContext.http.method;
    const path = event.rawPath;

    try {
        if (method === 'GET' && path === '/users') {
            const result = await ddb.query({
                TableName: TABLE,
                IndexName: 'GSI1',
                KeyConditionExpression: 'GSI1PK = :pk',
                ExpressionAttributeValues: { ':pk': 'USER' },
                Limit: 50,
            });
            return response(200, result.Items);
        }

        if (method === 'POST' && path === '/users') {
            const body = JSON.parse(event.body);
            const user = { PK: `USER#${body.id}`, SK: 'PROFILE', ...body };
            await ddb.put({ TableName: TABLE, Item: user });
            return response(201, user);
        }

        return response(404, { error: 'Not found' });
    } catch (err) {
        console.error('Handler error:', err);
        return response(500, { error: 'Internal server error' });
    }
};

function response(statusCode, body) {
    return {
        statusCode,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body),
    };
}
```

### Azure API Management

```javascript
// src/functions/apiWithAuth.js — Azure Functions + APIM
const { app } = require('@azure/functions');

// JWT validation middleware (APIM handles auth, this is for extra validation)
app.http('secureEndpoint', {
    methods: ['GET'],
    authLevel: 'anonymous', // APIM handles auth
    route: 'api/secure/data',
    handler: async (request, context) => {
        const userId = request.headers.get('x-user-id');
        const role = request.headers.get('x-user-role');

        // APIM injects these headers after token validation
        if (!userId) {
            return { status: 401, jsonBody: { error: 'Unauthorized' } };
        }

        const data = await fetchUserData(userId);

        return {
            status: 200,
            jsonBody: { user: userId, role, data },
            headers: { 'Cache-Control': 'private, max-age=60' },
        };
    },
});
```

```xml
<!-- apim-policy.xml — API Management rate limiting policy -->
<policies>
    <inbound>
        <base />
        <!-- Rate limit: 100 calls per 60 seconds per subscription -->
        <rate-limit-by-key
            calls="100"
            renewal-period="60"
            counter-key="@(context.Subscription.Id)"
            increment-condition="@(context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)" />
        <!-- Validate JWT and inject headers -->
        <validate-jwt header-name="Authorization" failed-validation-httpcode="401">
            <openid-config url="https://login.microsoftonline.com/{tenant}/.well-known/openid-configuration" />
            <audiences>
                <audience>api://my-app-id</audience>
            </audiences>
        </validate-jwt>
        <set-header name="x-user-id" exists-action="override">
            <value>@(context.Request.Claims.Find(c => c.Type == "oid")?.Value)</value>
        </set-header>
        <set-header name="x-user-role" exists-action="override">
            <value>@(context.Request.Claims.Find(c => c.Type == "roles")?.Value)</value>
        </set-header>
        <!-- Response caching -->
        <cache-lookup vary-by-header="Authorization" vary-by-query-parameter="*" />
    </inbound>
    <outbound>
        <cache-store duration="60" />
        <base />
    </outbound>
</policies>
```

## Serverless Database Patterns

### DynamoDB Single-Table Design

```javascript
// src/db/dynamodb-single-table.js
const { DynamoDBDocument } = require('@aws-sdk/lib-dynamodb');
const { DynamoDBClient } = require('@aws-sdk/client-dynamodb');

const ddb = DynamoDBDocument.from(new DynamoDBClient({}));
const TABLE = 'MyApp';

// Single-table key design:
// PK                    SK                      Data
// USER#123              PROFILE                 { name, email, ... }
// USER#123              ORDER#2024-01-15        { items, total, ... }
// ORG#456               METADATA                { name, plan, ... }
// ORG#456               USER#123                { role: 'admin', joinedAt }

class UserModel {
    static async get(userId) {
        const result = await ddb.get({
            TableName: TABLE,
            Key: { PK: `USER#${userId}`, SK: 'PROFILE' },
        });
        return result.Item;
    }

    static async getOrders(userId, limit = 20) {
        const result = await ddb.query({
            TableName: TABLE,
            KeyConditionExpression: 'PK = :pk AND begins_with(SK, :prefix)',
            ExpressionAttributeValues: {
                ':pk': `USER#${userId}`,
                ':prefix': 'ORDER#',
            },
            ScanIndexForward: false, // Newest first
            Limit: limit,
        });
        return result.Items;
    }

    static async getOrgUsers(orgId) {
        const result = await ddb.query({
            TableName: TABLE,
            KeyConditionExpression: 'PK = :pk AND begins_with(SK, :prefix)',
            ExpressionAttributeValues: {
                ':pk': `ORG#${orgId}`,
                ':prefix': 'USER#',
            },
            IndexName: 'GSI1', // If querying by org
        });
        return result.Items;
    }

    static async createUser(user) {
        const now = new Date().toISOString();
        await ddb.transactWrite({
            TransactItems: [
                {
                    Put: {
                        TableName: TABLE,
                        Item: {
                            PK: `USER#${user.id}`,
                            SK: 'PROFILE',
                            ...user,
                            createdAt: now,
                        },
                        ConditionExpression: 'attribute_not_exists(PK)',
                    },
                },
                {
                    Put: {
                        TableName: TABLE,
                        Item: {
                            PK: `ORG#${user.orgId}`,
                            SK: `USER#${user.id}`,
                            role: user.role,
                            GSI1PK: 'USER',
                            GSI1SK: now,
                        },
                    },
                },
            ],
        });
        return user;
    }
}
```

### Aurora Serverless Data API

```javascript
// src/db/aurora-serverless.js — No connection management needed
const { RDSDataClient, ExecuteStatementCommand } = require('@aws-sdk/client-rds-data');

const rds = new RDSDataClient({ region: 'us-east-1' });

const DB_CLUSTER = process.env.DB_CLUSTER_ARN;
const DB_SECRET = process.env.DB_SECRET_ARN;
const DB_NAME = 'myapp';

async function query(sql, parameters = []) {
    const result = await rds.send(new ExecuteStatementCommand({
        resourceArn: DB_CLUSTER,
        secretArn: DB_SECRET,
        database: DB_NAME,
        sql,
        parameters: parameters.map((v) => ({
            value: { stringValue: typeof v === 'string' ? v : JSON.stringify(v) },
        })),
        formatRecordsAs: 'JSON',
    }));

    return result.formattedRecords ? JSON.parse(result.formattedRecords) : [];
}

// Usage in Lambda handler
exports.getUsers = async (event) => {
    const users = await query(
        'SELECT * FROM users WHERE org_id = $1 ORDER BY created_at DESC LIMIT 20',
        [event.queryStringParameters.orgId]
    );
    return { statusCode: 200, body: JSON.stringify(users) };
};

exports.createUser = async (event) => {
    const { name, email, orgId } = JSON.parse(event.body);

    const result = await query(
        'INSERT INTO users (name, email, org_id) VALUES ($1, $2, $3) RETURNING *',
        [name, email, orgId]
    );

    return { statusCode: 201, body: JSON.stringify(result[0]) };
};
```

### Azure Cosmos DB & Google Firestore

```javascript
// src/db/cosmos-db.js — Azure Cosmos DB serverless tier
const { CosmosClient } = require('@azure/cosmos');

const client = new CosmosClient(process.env.COSMOS_CONNECTION_STRING);
const container = client.database('shop').container('products');

// Serverless Cosmos DB — pay per request, no provisioning
exports.searchProducts = async (category, searchTerm) => {
    const query = {
        query: `
            SELECT * FROM c
            WHERE c.category = @category
            AND CONTAINS(LOWER(c.name), @search)
            ORDER BY c.rating DESC
        `,
        parameters: [
            { name: '@category', value: category },
            { name: '@search', value: searchTerm.toLowerCase() },
        ],
    };

    const { resources } = await container.items
        .query(query, { maxItemCount: 20 })
        .fetchAll();

    return resources;
};

// src/db/firestore.js — Google Firestore
const { Firestore } = require('@google-cloud/firestore');

const db = new Firestore();

// Real-time document patterns
exports.createOrder = async (orderData) => {
    const orderRef = db.collection('orders').doc();

    await db.runTransaction(async (transaction) => {
        // Read and validate inventory
        const itemRefs = orderData.items.map((i) => db.doc(`products/${i.id}`));
        const itemDocs = await Promise.all(itemRefs.map((ref) => transaction.get(ref)));

        for (let i = 0; i < itemDocs.length; i++) {
            if (!itemDocs[i].exists || itemDocs[i].data().stock < orderData.items[i].qty) {
                throw new Error(`Insufficient stock for item ${orderData.items[i].id}`);
            }
        }

        // Deduct stock and create order atomically
        for (let i = 0; i < itemDocs.length; i++) {
            transaction.update(itemRefs[i], {
                stock: Firestore.FieldValue.increment(-orderData.items[i].qty),
            });
        }

        transaction.set(orderRef, {
            ...orderData,
            status: 'confirmed',
            createdAt: Firestore.FieldValue.serverTimestamp(),
        });
    });

    return { orderId: orderRef.id, status: 'confirmed' };
};
```

## Serverless Authentication

### AWS Cognito + Azure AD B2C + Firebase Auth

```javascript
// src/auth/cognito.js — Cognito JWT verification
const { CognitoJwtVerifier } = require('aws-jwt-verify');

const verifier = CognitoJwtVerifier.create({
    userPoolId: process.env.COGNITO_USER_POOL_ID,
    clientId: process.env.COGNITO_CLIENT_ID,
    tokenUse: 'access',
});

exports.authenticate = async (event) => {
    const token = event.headers.authorization?.replace('Bearer ', '');
    if (!token) {
        return { statusCode: 401, body: JSON.stringify({ error: 'No token' }) };
    }

    try {
        const payload = await verifier.verify(token);
        // Inject user context
        event.requestContext.authorizer = {
            claims: payload,
            userId: payload.sub,
        };
        return null; // Proceed to handler
    } catch {
        return { statusCode: 401, body: JSON.stringify({ error: 'Invalid token' }) };
    }
};

// src/auth/firebase-auth.js — Firebase Auth (for Cloud Functions)
const admin = require('firebase-admin');

exports.verifyToken = async (req, res, next) => {
    const token = req.headers.authorization?.split('Bearer ')[1];

    if (!token) {
        return res.status(401).json({ error: 'Authentication required' });
    }

    try {
        const decoded = await admin.auth().verifyIdToken(token);
        req.user = {
            uid: decoded.uid,
            email: decoded.email,
            emailVerified: decoded.email_verified,
            role: decoded.role || 'user',
        };
        next();
    } catch (err) {
        return res.status(403).json({ error: 'Invalid or expired token' });
    }
};

// Role-based access control
exports.requireRole = (...roles) => {
    return (req, res, next) => {
        if (!req.user || !roles.includes(req.user.role)) {
            return res.status(403).json({
                error: 'Insufficient permissions',
                required: roles,
                current: req.user?.role,
            });
        }
        next();
    };
};
```

## Serverless Monitoring

```javascript
// src/monitoring/cloudwatch.js — AWS structured logging
exports.handler = async (event) => {
    const startTime = Date.now();

    const logContext = {
        requestId: event.requestContext?.requestId,
        function: 'processOrder',
        coldStart: !global.initialized,
    };

    console.log(JSON.stringify({
        level: 'INFO',
        message: 'Function invoked',
        ...logContext,
        timestamp: new Date().toISOString(),
    }));

    try {
        const result = await processOrder(event);

        // CloudWatch Embedded Metric Format (EMF)
        console.log(JSON.stringify({
            _aws: {
                Timestamp: Date.now(),
                CloudWatchMetrics: [{
                    Namespace: 'MyApp/Serverless',
                    Dimensions: [['Function', 'Status']],
                    Metrics: [
                        { Name: 'Duration', Unit: 'Milliseconds' },
                        { Name: 'Invocations', Unit: 'Count' },
                    ],
                }],
            },
            Function: 'processOrder',
            Status: 'Success',
            Duration: Date.now() - startTime,
            Invocations: 1,
        }));

        return result;
    } catch (err) {
        console.error(JSON.stringify({
            level: 'ERROR',
            message: err.message,
            stack: err.stack,
            ...logContext,
            duration: Date.now() - startTime,
        }));
        throw err;
    }
};

// src/monitoring/azure-monitor.js — Azure Application Insights
const appInsights = require('applicationinsights');

appInsights.setup(process.env.APPINSIGHTS_CONNECTION_STRING)
    .setAutoDependencyCorrelation(true)
    .setAutoCollectRequests(true)
    .setAutoCollectPerformance(true)
    .start();

const telemetryClient = appInsights.defaultClient;

// Custom metrics in Azure Functions
const { app } = require('@azure/functions');

app.http('monitoredEndpoint', {
    methods: ['GET'],
    handler: async (request, context) => {
        const startTime = Date.now();

        try {
            const result = await fetchData();

            telemetryClient.trackDependency({
                target: 'database',
                name: 'fetchData',
                data: 'SELECT * FROM items',
                duration: Date.now() - startTime,
                resultCode: 200,
                success: true,
                dependencyTypeName: 'SQL',
            });

            return { status: 200, jsonBody: result };
        } catch (err) {
            telemetryClient.trackException({ exception: err });
            return { status: 500, jsonBody: { error: 'Internal error' } };
        }
    },
});
```

## Cold Start Optimization

### Initialization Patterns & Provisioned Concurrency

```javascript
// src/optimized-handler.js — Cold start optimization strategies

// Strategy 1: Lazy initialization with connection reuse
let dbPool;

function getDbPool() {
    if (!dbPool) {
        dbPool = new (require('pg').Pool)({
            host: process.env.DB_HOST,
            max: 1,
            connectionTimeoutMillis: 3000,
            idleTimeoutMillis: 60000,
            allowExitOnIdle: true,
        });
    }
    return dbPool;
}

// Strategy 2: Pre-warm initialization (runs on cold start only)
const initPromise = (async () => {
    // Parallel initialization
    const [db, cache, config] = await Promise.all([
        initializeDatabase(),
        initializeCache(),
        loadConfiguration(),
    ]);
    return { db, cache, config };
})();

exports.handler = async (event) => {
    // Await pre-warmed resources (instant if already initialized)
    const { db, cache, config } = await initPromise;

    // Check cache first
    const cacheKey = `user:${event.pathParameters.id}`;
    const cached = await cache.get(cacheKey);
    if (cached) return response(200, JSON.parse(cached));

    // Query database
    const user = await db.query('SELECT * FROM users WHERE id = $1', [event.pathParameters.id]);
    await cache.set(cacheKey, JSON.stringify(user), 'EX', 300);

    return response(200, user);
};

// serverless.yml — AWS Lambda SnapStart-like patterns
// Use provisioned concurrency for latency-critical functions
/*
functions:
  api:
    handler: src/optimized-handler.handler
    provisionedConcurrency: 10
    reservedConcurrency: 50
    memorySize: 1024   # More memory = faster CPU = faster cold start
    timeout: 30
*/

// Strategy 3: Bundle optimization
// webpack.config.js
const path = require('path');
const TerserPlugin = require('terser-webpack-plugin');

module.exports = {
    target: 'node20',
    mode: 'production',
    entry: './src/handler.js',
    output: {
        path: path.resolve(__dirname, '.webpack'),
        filename: 'handler.js',
        libraryTarget: 'commonjs2',
    },
    optimization: {
        minimize: true,
        minimizer: [new TerserPlugin({
            terserOptions: {
                compress: { drop_console: true, passes: 2 },
                mangle: true,
            },
        })],
    },
    externals: [
        // Exclude AWS SDK if using Lambda's built-in SDK
        ({ request }, callback) => {
            if (/^@aws-sdk\//.test(request)) {
                return callback(null, `commonjs ${request}`);
            }
            callback();
        },
    ],
};
```

### Azure SnapStart & Warm-Up

```javascript
// src/functions/warmup.js — Azure Functions warm-up pattern
const { app } = require('@azure/functions');

// Warm-up trigger — Azure calls this on new instance
app.http('warmup', {
    methods: ['GET'],
    authLevel: 'anonymous',
    route: 'api/warmup',
    handler: async (request, context) => {
        // Pre-load modules, establish connections
        await Promise.all([
            initializeDatabasePool(),
            loadSharedConfiguration(),
            warmUpCache(),
        ]);

        context.log('Warm-up complete');
        return { status: 200, body: 'Warm' };
    },
});

// Keep-alive pattern for preventing cold starts
// Deploy a timer function that pings itself
app.timer('keepAlive', {
    schedule: '0 */4 * * * *', // Every 4 minutes
    handler: async (timer, context) => {
        try {
            const response = await fetch(
                `${process.env.FUNCTION_APP_URL}/api/warmup`,
                { method: 'GET', signal: AbortSignal.timeout(10000) }
            );
            context.log(`Keep-alive: ${response.status}`);
        } catch (err) {
            context.log.error('Keep-alive failed:', err.message);
        }
    },
});
```

## Serverless Testing Strategies

```javascript
// __tests__/handler.test.js — Unit testing serverless handlers
const { mockClient } = require('aws-sdk-client-mock');
const { DynamoDBDocumentClient, GetCommand, PutCommand } = require('@aws-sdk/lib-dynamodb');

const ddbMock = mockClient(DynamoDBDocumentClient);

beforeEach(() => ddbMock.reset());

describe('getUser handler', () => {
    it('returns user when found', async () => {
        ddbMock.on(GetCommand).resolves({
            Item: { id: '123', name: 'Alice', email: 'alice@example.com' },
        });

        const event = {
            pathParameters: { id: '123' },
            requestContext: { requestId: 'test-123' },
        };

        const result = await handler(event);

        expect(result.statusCode).toBe(200);
        const body = JSON.parse(result.body);
        expect(body.name).toBe('Alice');
    });

    it('returns 404 when user not found', async () => {
        ddbMock.on(GetCommand).resolves({ Item: undefined });

        const event = { pathParameters: { id: '999' } };
        const result = await handler(event);

        expect(result.statusCode).toBe(404);
    });
});

// Integration test with local emulators
// __tests__/integration.test.js
const { DynamoDBClient, CreateTableCommand } = require('@aws-sdk/client-dynamodb');

describe('Order integration', () => {
    let table;

    beforeAll(async () => {
        // Point to local DynamoDB (e.g., DynamoDB Local)
        process.env.AWS_ENDPOINT = 'http://localhost:8000';
        process.env.TABLE_NAME = 'test-orders';

        const client = new DynamoDBClient({ endpoint: 'http://localhost:8000' });
        await client.send(new CreateTableCommand({
            TableName: 'test-orders',
            KeySchema: [{ AttributeName: 'PK', KeyType: 'HASH' }],
            AttributeDefinitions: [{ AttributeName: 'PK', AttributeType: 'S' }],
            BillingMode: 'PAY_PER_REQUEST',
        }));
    });

    it('creates and retrieves order', async () => {
        const createEvent = {
            httpMethod: 'POST',
            body: JSON.stringify({ customerId: 'c1', items: [{ id: 'i1', qty: 2 }] }),
        };

        const createResult = await createHandler(createEvent);
        expect(createResult.statusCode).toBe(201);

        const order = JSON.parse(createResult.body);
        const getResult = await getHandler({ pathParameters: { id: order.id } });
        expect(getResult.statusCode).toBe(200);
    });
});
```

```javascript
// __tests__/azure-functions.test.js — Azure Functions testing
const { app } = require('@azure/functions');
const { InvocationContext } = require('@azure/functions/src/InvocationContext');

describe('Azure Functions handlers', () => {
    it('creates user via HTTP trigger', async () => {
        const context = new InvocationContext({ functionName: 'createUser' });
        const request = new Request('http://localhost/api/users', {
            method: 'POST',
            body: JSON.stringify({ name: 'Bob', email: 'bob@test.com' }),
            headers: { 'Content-Type': 'application/json' },
        });

        // Mock Cosmos DB
        const mockContainer = {
            items: { create: jest.fn().resolvingValue({ resource: { id: '1', name: 'Bob' } }) },
        };

        const response = await createUserHandler(request, context);
        expect(response.status).toBe(201);
    });
});
```

## Event-Driven Serverless Architecture

```
┌──────────────┐     ┌───────────────┐     ┌──────────────────┐
│  API Gateway  │────▶│  Lambda/Func  │────▶│  Event Bus       │
│  (REST/HTTP)  │     │  (Command)    │     │  (EventBridge)   │
└──────────────┘     └───────────────┘     └────────┬─────────┘
                                                     │
                              ┌──────────────────────┼──────────────────────┐
                              │                      │                      │
                              ▼                      ▼                      ▼
                     ┌────────────────┐    ┌────────────────┐    ┌────────────────┐
                     │  Lambda:       │    │  Lambda:       │    │  Lambda:       │
                     │  Notification  │    │  Analytics     │    │  Saga Orch.    │
                     │  Service       │    │  Service       │    │  (compensate)  │
                     └────────────────┘    └────────────────┘    └────────────────┘
```

```javascript
// src/events/eventbridge.js — Event-driven architecture
const { EventBridgeClient, PutEventsCommand } = require('@aws-sdk/client-eventbridge');

const eb = new EventBridgeClient({});

// Publish domain events
async function publishEvent(source, detailType, detail) {
    await eb.send(new PutEventsCommand({
        Entries: [{
            Source: source,
            DetailType: detailType,
            Detail: JSON.stringify(detail),
            EventBusName: process.env.EVENT_BUS_NAME,
        }],
    }));
}

// Order command handler
exports.createOrder = async (event) => {
    const order = JSON.parse(event.body);

    // Process order
    const created = await saveOrder(order);

    // Publish event for downstream services
    await publishEvent('orders.service', 'OrderCreated', {
        orderId: created.id,
        customerId: created.customerId,
        total: created.total,
        items: created.items,
    });

    return { statusCode: 201, body: JSON.stringify(created) };
};

// Notification service (subscribed to OrderCreated)
exports.handleOrderEvent = async (event) => {
    for (const record of event.Records) {
        const { source, 'detail-type': detailType, detail } = JSON.parse(record.body);

        switch (detailType) {
            case 'OrderCreated':
                await sendOrderConfirmation(detail);
                await updateInventory(detail.items);
                break;
            case 'PaymentProcessed':
                await notifyWarehouse(detail);
                break;
            case 'OrderCancelled':
                await restoreInventory(detail.items);
                await sendCancellationNotice(detail);
                break;
            default:
                console.warn('Unknown event type:', detailType);
        }
    }
};

// Saga pattern for distributed transactions
exports.orderSaga = async (event) => {
    const { orderId, steps } = JSON.parse(event.body);
    const completedSteps = [];

    for (const step of steps) {
        try {
            await executeStep(step);
            completedSteps.push(step);
        } catch (err) {
            console.error(`Step ${step.name} failed:`, err.message);

            // Compensate completed steps (reverse order)
            for (const completed of completedSteps.reverse()) {
                try {
                    await compensateStep(completed);
                } catch (compErr) {
                    console.error(`Compensation failed for ${completed.name}:`, compErr);
                    // Alert — manual intervention required
                }
            }

            await publishEvent('orders.service', 'OrderFailed', { orderId, reason: err.message });
            return;
        }
    }

    await publishEvent('orders.service', 'OrderCompleted', { orderId });
};
```

## Best Practices Checklist

- [ ] Use HTTP API over REST API for simpler, cheaper gateways
- [ ] Implement request/response transformation at gateway level
- [ ] Use connection pooling with Data API (Aurora) or single-table design (DynamoDB)
- [ ] Verify JWTs at the gateway, pass claims as headers to functions
- [ ] Use provisioned concurrency for latency-critical endpoints
- [ ] Optimize bundles: tree-shake, exclude dev dependencies, use layers
- [ ] Write unit tests with SDK mocks, integration tests with local emulators
- [ ] Use structured logging with EMF (AWS) or Application Insights (Azure)
- [ ] Implement circuit breakers for downstream service calls
- [ ] Use event-driven patterns for decoupled, resilient architectures

## Cross-References

- See [Lambda Patterns](01-lambda-patterns.md) for AWS Lambda fundamentals
- See [Azure & GCP Serverless](02-azure-gcp-serverless.md) for multi-cloud serverless
- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for event-driven patterns
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability
- See [Security](../09-deployment-security/01-security-scanning.md) for API security
- See [Testing](../../09-testing/01-unit-testing.md) for testing strategies

## Next Steps

Continue to [CI/CD Pipelines](../05-ci-cd-pipelines/01-github-actions.md) for automating serverless deployments.
