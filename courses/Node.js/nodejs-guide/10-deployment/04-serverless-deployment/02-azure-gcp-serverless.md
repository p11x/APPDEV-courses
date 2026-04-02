# Azure & Google Cloud Serverless

## What You'll Learn

- Azure Functions triggers, bindings, and Durable Functions
- Azure Functions Node.js programming model v4
- Google Cloud Functions 2nd gen and Cloud Run Jobs
- Firebase Cloud Functions integration
- Multi-cloud serverless comparison and patterns

## Azure Functions Deep Dive

### HTTP Trigger

```javascript
// src/functions/httpApi.js — Programming Model v4
const { app } = require('@azure/functions');

// Simple HTTP trigger
app.http('getUsers', {
    methods: ['GET'],
    authLevel: 'function',
    route: 'users',
    handler: async (request, context) => {
        context.log('GET /users called');

        const users = await fetchUsersFromCosmos();

        return {
            status: 200,
            jsonBody: users,
            headers: { 'Content-Type': 'application/json' },
        };
    },
});

// HTTP trigger with route parameters
app.http('getUserById', {
    methods: ['GET'],
    authLevel: 'function',
    route: 'users/{id}',
    handler: async (request, context) => {
        const id = request.params.id;
        const user = await fetchUserById(id);

        if (!user) {
            return { status: 404, jsonBody: { error: 'User not found' } };
        }

        return { status: 200, jsonBody: user };
    },
});

// HTTP trigger with input binding (Cosmos DB)
app.http('getUserPosts', {
    methods: ['GET'],
    authLevel: 'function',
    route: 'users/{userId}/posts',
    extraInputs: [
        {
            type: 'cosmosDB',
            connection: 'CosmosDbConnection',
            databaseName: 'blog',
            containerName: 'posts',
            partitionKey: '{userId}',
            sqlQuery: 'SELECT * FROM c WHERE c.userId = {userId}',
            name: 'posts',
        },
    ],
    handler: async (request, context) => {
        const posts = context.extraInputs.get('posts');
        return { status: 200, jsonBody: posts };
    },
});
```

### Queue & Blob Triggers

```javascript
// src/functions/queueProcessor.js — Queue trigger
const { app } = require('@azure/functions');

app.storageQueue('processOrder', {
    queueName: 'orders',
    connection: 'AzureWebJobsStorage',
    handler: async (queueItem, context) => {
        context.log('Processing order:', queueItem.orderId);

        try {
            await processOrder(queueItem);
            context.log(`Order ${queueItem.orderId} processed successfully`);
        } catch (err) {
            context.log.error(`Failed to process order: ${err.message}`);
            throw err; // Returns message to queue for retry
        }
    },
});

// Blob trigger — process uploaded images
app.storageBlob('processImageUpload', {
    path: 'uploads/{name}',
    connection: 'AzureWebJobsStorage',
    handler: async (blob, context) => {
        context.log(`Processing blob: ${context.triggerMetadata.name}`);

        const metadata = context.triggerMetadata;
        const buffer = Buffer.from(await blob.arrayBuffer());

        // Generate thumbnails
        const thumbnail = await generateThumbnail(buffer, 200);

        // Output binding writes to different container
        return thumbnail;
    },
});

// Timer trigger — scheduled cleanup
app.timer('dailyCleanup', {
    schedule: '0 0 2 * * *', // 2 AM daily
    handler: async (timer, context) => {
        context.log('Running daily cleanup at:', new Date().toISOString());

        const deleted = await cleanupExpiredSessions();
        context.log(`Cleaned up ${deleted} expired sessions`);
    },
});
```

### Durable Functions

```javascript
// src/functions/orchestrator.js — Durable Functions pattern
const { app, invoke } = require('@azure/functions');
const df = require('durable-functions');

// Orchestrator function — coordinates the workflow
app.orchestration('orderProcessing', function* (context) {
    const order = context.df.getInput();

    // Step 1: Validate inventory
    const inventoryResult = yield context.df.callActivity('checkInventory', order);

    if (!inventoryResult.available) {
        return { status: 'rejected', reason: 'Out of stock' };
    }

    // Step 2: Process payment (with retry policy)
    const paymentResult = yield context.df.callActivityWithRetry(
        'processPayment',
        { retries: 3, retryInterval: 5000 },
        order
    );

    // Step 3: Fan-out — send notifications in parallel
    const notifications = [
        context.df.callActivity('sendConfirmationEmail', { order, paymentResult }),
        context.df.callActivity('updateAnalytics', order),
        context.df.callActivity('notifyWarehouse', order),
    ];
    yield context.df.Task.all(notifications);

    // Step 4: Wait for external event (shipping confirmation)
    const shippingEvent = yield context.df.waitForExternalEvent('shippingConfirmed');

    return { status: 'completed', tracking: shippingEvent.trackingNumber };
});

// Activity functions
app.activity('checkInventory', {
    handler: async (order, context) => {
        const stock = await checkStock(order.items);
        return { available: stock.allInStock, items: stock.details };
    },
});

app.activity('processPayment', {
    handler: async (order, context) => {
        const result = await chargePayment(order.paymentMethod, order.total);
        return { transactionId: result.id, status: 'charged' };
    },
});

app.activity('sendConfirmationEmail', {
    handler: async (data, context) => {
        await sendEmail({
            to: data.order.customerEmail,
            subject: 'Order Confirmed',
            body: `Your order has been confirmed. Transaction: ${data.paymentResult.transactionId}`,
        });
        return { sent: true };
    },
});
```

## Azure Functions Bindings

```javascript
// Input and output bindings — reduce boilerplate
const { app } = require('@azure/functions');

// HTTP trigger with Cosmos DB input + Queue output
app.http('createOrder', {
    methods: ['POST'],
    route: 'orders',
    extraInputs: [
        {
            type: 'cosmosDB',
            connection: 'CosmosDbConnection',
            databaseName: 'shop',
            containerName: 'customers',
            name: 'customer',
            id: '{customerId}',
            partitionKey: '{customerId}',
        },
    ],
    extraOutputs: [
        {
            type: 'cosmosDB',
            connection: 'CosmosDbConnection',
            databaseName: 'shop',
            containerName: 'orders',
            name: 'orderOutput',
        },
        {
            type: 'storageQueue',
            connection: 'AzureWebJobsStorage',
            queueName: 'order-notifications',
            name: 'notificationQueue',
        },
    ],
    handler: async (request, context) => {
        const customer = context.extraInputs.get('customer');
        if (!customer) {
            return { status: 404, jsonBody: { error: 'Customer not found' } };
        }

        const order = {
            id: crypto.randomUUID(),
            customerId: customer.id,
            items: await request.json(),
            createdAt: new Date().toISOString(),
            status: 'pending',
        };

        // Write to Cosmos DB via output binding
        context.extraOutputs.set('orderOutput', order);
        // Enqueue notification via output binding
        context.extraOutputs.set('notificationQueue', {
            orderId: order.id,
            type: 'order_created',
        });

        return { status: 201, jsonBody: order };
    },
});
```

## Azure Functions Deployment

### Bicep Template

```bicep
// infra/main.bicep — Azure Functions infrastructure
@description('Location for all resources')
param location string = resourceGroup().location

@description('Name prefix')
param prefix string = 'myapp'

var functionAppName = '${prefix}-func'
var storageAccountName = '${prefix}funcstore'
var appServicePlanName = '${prefix}-asp'
var appInsightsName = '${prefix}-ai'

// Storage account (required for Azure Functions)
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
    name: storageAccountName
    location: location
    kind: 'StorageV2'
    sku: { name: 'Standard_LRS' }
}

// Application Insights (monitoring)
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
    name: appInsightsName
    location: location
    kind: 'web'
    properties: { Application_Type: 'nodejs' }
}

// Consumption plan (serverless)
resource plan 'Microsoft.Web/serverfarms@2023-01-01' = {
    name: appServicePlanName
    location: location
    sku: { name: 'Y1', tier: 'Dynamic' }
}

// Function App
resource functionApp 'Microsoft.Web/sites@2023-01-01' = {
    name: functionAppName
    location: location
    kind: 'functionapp'
    properties: {
        serverFarmId: plan.id
        siteConfig: {
            appSettings: [
                { name: 'AzureWebJobsStorage', value: storageAccount.properties.primaryEndpoints.blob }
                { name: 'FUNCTIONS_EXTENSION_VERSION', value: '~4' }
                { name: 'FUNCTIONS_WORKER_RUNTIME', value: 'node' }
                { name: 'WEBSITE_NODE_DEFAULT_VERSION', value: '~20' }
                { name: 'APPLICATIONINSIGHTS_CONNECTION_STRING', value: appInsights.properties.ConnectionString }
            ]
        }
    }
}

output functionAppUrl string = 'https://${functionApp.properties.defaultHostName}'
```

### Azure CLI Deployment

```bash
# Deploy Azure Functions with Azure CLI
RESOURCE_GROUP="myapp-rg"
LOCATION="eastus"
STORAGE_ACCOUNT="myappfuncstore"
FUNCTION_APP="myapp-func"

# Create resource group
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create storage account
az storage account create \
    --name $STORAGE_ACCOUNT \
    --resource-group $RESOURCE_GROUP \
    --sku Standard_LRS

# Create function app
az functionapp create \
    --name $FUNCTION_APP \
    --resource-group $RESOURCE_GROUP \
    --storage-account $STORAGE_ACCOUNT \
    --consumption-plan-location $LOCATION \
    --runtime node \
    --runtime-version 20 \
    --functions-version 4

# Deploy code
func azure functionapp publish $FUNCTION_APP

# Set application settings
az functionapp config appsettings set \
    --name $FUNCTION_APP \
    --resource-group $RESOURCE_GROUP \
    --settings "COSMOS_CONNECTION=@Microsoft.KeyVault(...)"
```

## Google Cloud Functions — 2nd Gen

### HTTP & Event-Driven Functions

```javascript
// src/index.js — 2nd gen Cloud Functions
const { onRequest } = require('firebase-functions/v2/https');
const { onDocumentCreated } = require('firebase-functions/v2/firestore');
const { onMessagePublished } = require('firebase-functions/v2/pubsub');
const { logger } = require('firebase-functions');

// HTTP function (2nd gen)
exports.apiHandler = onRequest(
    { region: 'us-central1', memory: '256Mi', timeoutSeconds: 60 },
    async (req, res) => {
        if (req.method === 'GET') {
            const users = await fetchUsers();
            res.json(users);
        } else if (req.method === 'POST') {
            const user = await createUser(req.body);
            res.status(201).json(user);
        } else {
            res.status(405).json({ error: 'Method not allowed' });
        }
    }
);

// Cloud Events — Pub/Sub trigger (2nd gen)
exports.processEvent = onMessagePublished(
    { topic: 'user-events', region: 'us-central1' },
    async (event) => {
        const message = event.data.message;
        const payload = JSON.parse(Buffer.from(message.data, 'base64').toString());

        logger.info('Processing event:', { type: payload.type, id: payload.id });

        switch (payload.type) {
            case 'user_signup':
                await sendWelcomeEmail(payload.email);
                break;
            case 'order_placed':
                await processOrder(payload.orderId);
                break;
            default:
                logger.warn('Unknown event type:', payload.type);
        }
    }
);

// Firestore trigger (2nd gen)
exports.onNewReview = onDocumentCreated(
    'reviews/{reviewId}',
    async (event) => {
        const review = event.data.data();
        logger.info('New review:', { reviewId: event.params.reviewId });

        // Update product rating
        await updateProductRating(review.productId);

        // Check for inappropriate content
        const flagged = await moderateContent(review.text);
        if (flagged) {
            await event.data.ref.update({ status: 'flagged' });
        }
    }
);
```

## Google Cloud Run Jobs

```javascript
// src/batch-job.js — Cloud Run Job for batch processing
const { PubSub } = require('@google-cloud/pubsub');
const { Storage } = require('@google-cloud/storage');
const { Firestore } = require('@google-cloud/firestore');

const pubsub = new PubSub();
const storage = new Storage();
const firestore = new Firestore();

async function processReports() {
    const bucket = storage.bucket(process.env.BUCKET_NAME);
    const [files] = await bucket.getFiles({ prefix: 'reports/pending/' });

    console.log(`Found ${files.length} reports to process`);

    for (const file of files) {
        try {
            const [contents] = await file.download();
            const report = JSON.parse(contents.toString());

            // Process and store results
            const result = await analyzeReport(report);

            await firestore.collection('reportResults').add({
                reportId: report.id,
                result,
                processedAt: new Date(),
            });

            // Move to processed folder
            await file.move(file.name.replace('pending/', 'processed/'));
            console.log(`Processed report: ${report.id}`);
        } catch (err) {
            console.error(`Failed to process ${file.name}:`, err.message);
        }
    }
}

// Cloud Run Job entry point
const http = require('http');
const server = http.createServer(async (req, res) => {
    if (req.url === '/health') {
        res.writeHead(200);
        return res.end('OK');
    }

    try {
        await processReports();
        res.writeHead(200);
        res.end(JSON.stringify({ status: 'completed' }));
    } catch (err) {
        console.error('Job failed:', err);
        res.writeHead(500);
        res.end(JSON.stringify({ error: err.message }));
    }
});

const PORT = process.env.PORT || 8080;
server.listen(PORT, () => console.log(`Job server listening on ${PORT}`));
```

```bash
# Deploy Cloud Run Job
gcloud run jobs create report-processor \
    --source . \
    --region us-central1 \
    --memory 512Mi \
    --task-timeout 3600 \
    --max-retries 3 \
    --set-env-vars "BUCKET_NAME=my-reports-bucket"

# Execute the job
gcloud run jobs execute report-processor --region us-central1

# Schedule with Cloud Scheduler
gcloud scheduler jobs create http process-reports-schedule \
    --schedule "0 */6 * * *" \
    --uri "https://report-processor-xxxx.us-central1.run.app" \
    --http-method POST
```

## Google Cloud Tasks

```javascript
// src/tasks.js — Cloud Tasks for async processing
const { CloudTasksClient } = require('@google-cloud/tasks');

const client = new CloudTasksClient();
const project = process.env.GOOGLE_CLOUD_PROJECT;
const location = 'us-central1';
const queue = 'async-processing';

// Create an async task
async function enqueueEmailTask(to, subject, body) {
    const parent = client.queuePath(project, location, queue);

    const task = {
        httpRequest: {
            httpMethod: 'POST',
            url: `https://${project}.cloudfunctions.net/sendEmail`,
            headers: { 'Content-Type': 'application/json' },
            body: Buffer.from(JSON.stringify({ to, subject, body })).toString('base64'),
            oidcToken: {
                serviceAccountEmail: process.env.SERVICE_ACCOUNT,
            },
        },
        scheduleTime: {
            seconds: Date.now() / 1000 + 60, // Delay by 60 seconds
        },
    };

    const [response] = await client.createTask({ parent, task });
    console.log(`Task created: ${response.name}`);
    return response;
}

// Task handler (Cloud Function)
exports.sendEmail = async (req, res) => {
    const { to, subject, body } = req.body;

    try {
        await sendgrid.send({ to, subject, text: body });
        res.status(200).send('Email sent');
    } catch (err) {
        console.error('Email send failed:', err);
        res.status(500).send('Failed');
    }
};

// Batch enqueue example
async function processNotificationBatch(notifications) {
    const promises = notifications.map((n) =>
        enqueueEmailTask(n.email, n.subject, n.message)
    );
    const results = await Promise.allSettled(promises);

    const failed = results.filter((r) => r.status === 'rejected');
    if (failed.length > 0) {
        console.error(`${failed.length} tasks failed to enqueue`);
    }

    return { total: notifications.length, failed: failed.length };
}
```

## Firebase Cloud Functions

```javascript
// src/firebase-functions.js — Firebase integration
const { onRequest, onCall } = require('firebase-functions/v2/https');
const { onSchedule } = require('firebase-functions/v2/scheduler');
const { initializeApp } = require('firebase-admin/app');
const { getFirestore, FieldValue } = require('firebase-admin/firestore');
const { getAuth } = require('firebase-admin/auth');

initializeApp();
const db = getFirestore();
const auth = getAuth();

// Callable function (client SDK invokes directly)
exports.createUserProfile = onCall(async (request) => {
    // Auth context available via request.auth
    if (!request.auth) {
        throw new Error('Authentication required');
    }

    const { displayName, bio } = request.data;

    const profile = {
        uid: request.auth.uid,
        email: request.auth.token.email,
        displayName,
        bio,
        createdAt: FieldValue.serverTimestamp(),
        role: 'member',
    };

    await db.collection('profiles').doc(request.auth.uid).set(profile);
    return { success: true, profile };
});

// HTTP function with auth verification
exports.adminDashboard = onRequest(async (req, res) => {
    try {
        const token = req.headers.authorization?.split('Bearer ')[1];
        const decoded = await auth.verifyIdToken(token);

        if (decoded.admin !== true) {
            return res.status(403).json({ error: 'Admin access required' });
        }

        const users = await db.collection('profiles').limit(100).get();
        const stats = await db.collection('stats').doc('daily').get();

        res.json({
            users: users.docs.map((d) => d.data()),
            stats: stats.data(),
        });
    } catch (err) {
        res.status(401).json({ error: 'Invalid token' });
    }
});

// Scheduled function
exports.dailyAggregation = onSchedule('every day 00:00', async (event) => {
    const yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);

    const orders = await db
        .collection('orders')
        .where('createdAt', '>=', yesterday)
        .get();

    const total = orders.docs.reduce((sum, doc) => sum + doc.data().amount, 0);

    await db.collection('stats').doc('daily').set({
        date: yesterday.toISOString().split('T')[0],
        orderCount: orders.size,
        totalRevenue: total,
        updatedAt: FieldValue.serverTimestamp(),
    });

    console.log(`Aggregated ${orders.size} orders, total: $${total}`);
});
```

## Serverless Comparison: AWS vs Azure vs GCP

```
Feature              │ AWS Lambda         │ Azure Functions    │ Google Cloud Functions
─────────────────────┼────────────────────┼────────────────────┼──────────────────────
Runtime              │ Node 18/20/22      │ Node 18/20/22      │ Node 18/20/22
Max Timeout          │ 15 min             │ Unlimited*         │ 60 min (2nd gen)
Memory               │ 128MB–10GB         │ 128MB–14GB         │ 128MB–32GB
Concurrency          │ 1000/account       │ 200/instance       │ 1000/function
Cold Start           │ ~500ms             │ ~800ms             │ ~400ms (2nd gen)
Triggers             │ SQS, S3, etc.      │ Queue, Blob, Timer │ Pub/Sub, GCS, etc.
Orchestration        │ Step Functions     │ Durable Functions  │ Workflows
Local Dev            │ SAM, LocalStack    │ func CLI           │ Functions Framework
Pricing (1M invok)   │ $0.20              │ $0.20              │ $0.40
Free Tier            │ 1M/month           │ 1M/month           │ 2M/month
```

## Multi-Cloud Serverless Patterns

```javascript
// src/cloud-agnostic/handler.js — Abstracted cloud layer
// Pattern: Abstract cloud-specific code behind a common interface

class ServerlessProvider {
    async sendQueueMessage(queueName, message) {
        throw new Error('Not implemented');
    }

    async getSecret(secretName) {
        throw new Error('Not implemented');
    }

    async storeObject(bucket, key, data) {
        throw new Error('Not implemented');
    }
}

// AWS implementation
class AWSProvider extends ServerlessProvider {
    constructor() {
        super();
        this.sqs = new SQSClient({});
        this.ssm = new SSMClient({});
        this.s3 = new S3Client({});
    }

    async sendQueueMessage(queueName, message) {
        const queueUrl = process.env[`QUEUE_${queueName.toUpperCase()}`];
        await this.sqs.send(new SendMessageCommand({
            QueueUrl: queueUrl,
            Body: JSON.stringify(message),
        }));
    }

    async getSecret(secretName) {
        const response = await this.ssm.send(new GetParameterCommand({
            Name: secretName,
            WithDecryption: true,
        }));
        return response.Parameter.Value;
    }
}

// Azure implementation
class AzureProvider extends ServerlessProvider {
    constructor() {
        super();
        this.serviceBus = new ServiceBusClient(process.env.SERVICEBUS_CONNECTION);
    }

    async sendQueueMessage(queueName, message) {
        const sender = this.serviceBus.createSender(queueName);
        await sender.sendMessages({ body: message });
        await sender.close();
    }

    async getSecret(secretName) {
        const client = new SecretClient(
            process.env.KEYVAULT_URL,
            new DefaultAzureCredential()
        );
        const secret = await client.getSecret(secretName);
        return secret.value;
    }
}

// Factory function
function createProvider() {
    switch (process.env.CLOUD_PROVIDER) {
        case 'aws': return new AWSProvider();
        case 'azure': return new AzureProvider();
        default: throw new Error(`Unknown provider: ${process.env.CLOUD_PROVIDER}`);
    }
}

// Business logic uses abstracted interface
const provider = createProvider();

async function handleOrder(order) {
    await provider.sendQueueMessage('orders', order);
    const apiKey = await provider.getSecret('/app/api-key');
    return processWithKey(order, apiKey);
}
```

## Best Practices Checklist

- [ ] Use Azure Functions v4 programming model for new projects
- [ ] Leverage Durable Functions for complex workflows
- [ ] Use Cloud Run Jobs for batch processing (not Cloud Functions)
- [ ] Implement idempotent operations for all triggers
- [ ] Use output bindings to reduce boilerplate code
- [ ] Deploy with infrastructure-as-code (Bicep/Terraform)
- [ ] Configure Application Insights / Cloud Monitoring
- [ ] Use Cloud Tasks / Service Bus for delayed processing

## Cross-References

- See [Lambda Patterns](01-lambda-patterns.md) for AWS serverless patterns
- See [API Gateway Advanced](03-serverless-api-gateway-advanced.md) for API management
- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for multi-cloud patterns
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for deployment automation
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability setup

## Next Steps

Continue to [Serverless API Gateway & Advanced Patterns](03-serverless-api-gateway-advanced.md).
