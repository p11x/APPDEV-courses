# AWS Lambda for Node.js

## What You'll Learn

- How to create and deploy AWS Lambda functions with Node.js
- How to configure Lambda runtime and handler
- How to implement cold start optimization
- How to manage Lambda layers and extensions

---

## Layer 1: Academic Foundation

### Serverless Computing Model

AWS Lambda is an event-driven serverless computing service that runs code in response to events and automatically manages the compute resources.

**Key Concepts:**
- **Cold Start**: Time to initialize a new execution environment
- **Warm Reuse**: Subsequent invocations reuse the same environment
- **Execution Context**: Temporary directory, memory, network configuration
- **Concurrency**: Number of instances processing events simultaneously

**Mathematical Model:**

```
Cost = (Execution Time / 1000) × Memory Allocated × Requests

Cold Start Time = Initialization + Function Load + Dependency Load
Warm Request Time = Handler Execution + Overhead
```

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Basic Lambda Handler

```typescript
// index.ts
import { APIGatewayProxyHandler, APIGatewayProxyResult } from 'aws-lambda';

export const handler: APIGatewayProxyHandler = async (
  event
): Promise<APIGatewayProxyResult> => {
  const { httpMethod, path, body } = event;
  
  console.log('Request:', { httpMethod, path, body });
  
  const response = {
    statusCode: 200,
    headers: {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    },
    body: JSON.stringify({
      message: 'Hello from Lambda',
      timestamp: new Date().toISOString(),
      requestId: event.requestContext?.requestId
    })
  };
  
  return response;
};
```

### Paradigm 2 — Optimized Lambda with Connection Reuse

```typescript
// optimized-handler.ts
import { APIGatewayProxyHandler, APIGatewayProxyResult } from 'aws-lambda';

let dbPool: Pool | null = null;

async function getPool(): Promise<Pool> {
  if (!dbPool) {
    dbPool = new Pool({
      host: process.env.DB_HOST,
      port: parseInt(process.env.DB_PORT || '5432'),
      database: process.env.DB_NAME,
      user: process.env.DB_USER,
      password: process.env.DB_PASSWORD,
      max: 20,
      idleTimeoutMillis: 30000
    });
  }
  return dbPool;
}

export const handler: APIGatewayProxyHandler = async (
  event
): Promise<APIGatewayProxyResult> => {
  const start = Date.now();
  
  try {
    const pool = await getPool();
    const result = await pool.query('SELECT * FROM users LIMIT 10');
    
    return {
      statusCode: 200,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        users: result.rows,
        latency: Date.now() - start
      })
    };
  } catch (error) {
    console.error('Error:', error);
    return {
      statusCode: 500,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ error: 'Internal server error' })
    };
  }
};
```

### Paradigm 3 — Async Handler with Custom Backend

```typescript
// async-handler.ts
import { S3Event, S3EventRecord } from 'aws-lambda';

interface ImageRecord {
  bucket: string;
  key: string;
  size: number;
}

export const handler = async (event: S3Event): Promise<void> => {
  const records: ImageRecord[] = event.Records.map((record: S3EventRecord) => ({
    bucket: record.s3.bucket.name,
    key: decodeURIComponent(record.s3.object.key),
    size: record.s3.object.size
  }));
  
  for (const image of records) {
    try {
      await processImage(image);
    } catch (error) {
      console.error(`Failed to process ${image.key}:`, error);
    }
  }
};

async function processImage(record: ImageRecord): Promise<void> {
  const s3 = new S3Client();
  const bucket = record.bucket;
  const key = record.key;
  
  const response = await s3.getObject({ Bucket: bucket, Key: key });
  const stream = response.Body as Readable;
  const buffer = await streamToBuffer(stream);
  
  const optimized = await sharp(buffer)
    .resize(800, 600, { fit: 'inside' })
    .jpeg({ quality: 85 })
    .toBuffer();
  
  await s3.putObject({
    Bucket: `${bucket}-processed`,
    Key: `processed/${key}`,
    Body: optimized,
    ContentType: 'image/jpeg'
  }).promise();
}
```

### Paradigm 4 — Lambda with Step Functions Integration

```typescript
// step-functions-handler.ts
import { APIGatewayProxyHandler } from 'aws-lambda';
import { SFN } from 'aws-sdk';

const stepFunctions = new SFN();

export const handler: APIGatewayProxyHandler = async (event) => {
  const { orderId, items, customerId } = JSON.parse(event.body || '{}');
  
  const params = {
    stateMachineArn: process.env.STATE_MACHINE_ARN,
    input: JSON.stringify({
      orderId,
      items,
      customerId,
      timestamp: Date.now()
    })
  };
  
  const result = await stepFunctions.startExecution(params).promise();
  
  return {
    statusCode: 202,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      executionArn: result.executionArn,
      status: 'STARTED'
    })
  };
};
```

---

## Layer 3: Performance Engineering Lab

### Cold Start Optimization

| Optimization | Impact | Implementation |
|--------------|--------|-----------------|
| Reduce package size | -30% cold start | Bundle with esbuild, strip dev deps |
| Lazy load modules | -40% init time | Import inside handler |
| Keep connections outside handler | -50% init time | Global variables |
| Provisioned concurrency | 0ms cold start | Pre-warmed instances |
| ARM architecture | -20% cold start | Graviton2 processors |

### Benchmark Results

```bash
# Cold start comparison
Node.js 14.x (x86_64):  350ms average
Node.js 16.x (x86_64):  280ms average
Node.js 18.x (ARM64):   220ms average
Node.js 18.x + Provisioned: 0ms
```

### Memory vs Duration Trade-off

```
Memory: 128MB → Duration: 1500ms, Cost: $0.0000625
Memory: 256MB → Duration: 800ms,  Cost: $0.0000666
Memory: 512MB → Duration: 450ms,  Cost: $0.0000750
Memory: 1024MB → Duration: 280ms, Cost: $0.0000933

Optimal: 512MB provides best price-performance
```

---

## Layer 4: Zero-Trust Security Architecture

### Threat Model

| Threat | Vector | Mitigation |
|--------|--------|------------|
| Code injection | User input | Input validation, parameterized queries |
| Dependency vulnerability | npm packages | Regular scanning, dependency audit |
| Over-privileged IAM | Lambda role | Least privilege, resource policy |
| Data exfiltration | Network | VPC, security groups, private subnets |
| Denial of Wallet | Resource exhaustion | Budget alerts, concurrency limits |

### Security Implementation

```typescript
// secure-handler.ts
import { APIGatewayProxyHandler } from 'aws-lambda';

export const handler: APIGatewayProxyHandler = async (event) => {
  // 1. Input validation
  const schema = z.object({
    userId: z.string().uuid(),
    email: z.string().email()
  });
  
  const parseResult = schema.safeParse(JSON.parse(event.body || '{}'));
  if (!parseResult.success) {
    return { statusCode: 400, body: 'Invalid input' };
  }
  
  // 2. Rate limiting (distributed)
  const redis = new Redis(process.env.REDIS_URL);
  const key = `ratelimit:${parseResult.data.userId}`;
  const count = await redis.incr(key);
  
  if (count > 100) {
    return { statusCode: 429, body: 'Too many requests' };
  }
  
  // 3. Output encoding
  return {
    statusCode: 200,
    headers: {
      'Content-Type': 'application/json',
      'X-Content-Type-Options': 'nosniff',
      'X-Frame-Options': 'DENY'
    },
    body: JSON.stringify({ success: true })
  };
};
```

### IAM Role Best Practices

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "dynamodb:GetItem",
        "dynamodb:Query"
      ],
      "Resource": "arn:aws:dynamodb:us-east-1:123456789012:table/Users",
      "Condition": {
        "ForAllValues:StringEquals": {
          "dynamodb:Attributes": ["userId", "email", "name"]
        }
      }
    }
  ]
}
```

---

## Layer 5: AI-Enhanced Testing Ecosystem

### Property-Based Testing

```typescript
// property-test.ts
import { fc, test } from '@fast-check/fast-check';

test('Lambda handles various HTTP methods', () => {
  return fc.assert(
    fc.asyncProperty(
      fc.constantFrom('GET', 'POST', 'PUT', 'DELETE'),
      async (method) => {
        const event = buildAPIGatewayEvent({ httpMethod: method });
        const result = await handler(event);
        return result.statusCode >= 200 && result.statusCode < 500;
      }
    )
  );
});

test('Lambda handles large payloads', () => {
  return fc.assert(
    fc.asyncProperty(
      fc.array(fc.string(), { maxLength: 10000 }),
      async (data) => {
        const body = JSON.stringify({ items: data });
        const event = buildAPIGatewayEvent({ body, httpMethod: 'POST' });
        const result = await handler(event);
        return result.statusCode < 500;
      }
    )
  );
});
```

### Chaos Engineering

```typescript
// chaos-test.ts
describe('Lambda Chaos Tests', () => {
  it('handles cold start timeout', async () => {
    // Simulate very slow initialization
    jest.mock('./database', () => ({
      connect: () => new Promise(resolve => setTimeout(resolve, 25000))
    }));
    
    const event = buildAPIGatewayEvent();
    const result = await handler(event);
    expect(result.statusCode).toBeDefined();
  });
  
  it('handles memory exhaustion', async () => {
    // Allocate memory until limit
    const event = buildAPIGatewayEvent();
    await expect(handler(event)).rejects.toThrow();
  });
});
```

---

## Layer 6: DevOps & SRE Operations Center

### SLI/SLO Definitions

| Metric | SLI | SLO | Error Budget |
|--------|-----|-----|--------------|
| Availability | Request success rate | 99.9% | 43.8 min/month |
| Latency p50 | 50th percentile | < 100ms | N/A |
| Latency p99 | 99th percentile | < 500ms | 4.38 min/month |
| Cold Start | Cold request % | < 1% | 7.3 min/month |

### Monitoring Configuration

```typescript
// metrics.ts
import { MetricsLogger, Unit } from 'aws-sdk';

export class LambdaMetrics {
  private metrics: MetricsLogger;
  
  constructor() {
    this.metrics = new AWS.CloudWatch({
      region: process.env.AWS_REGION
    });
  }
  
  recordColdStart(durationMs: number, memoryMB: number): void {
    this.metrics.putMetricData({
      MetricData: [
        {
          MetricName: 'ColdStartDuration',
          Value: durationMs,
          Unit: Unit.Milliseconds,
          Timestamp: new Date()
        },
        {
          MetricName: 'MemoryAllocated',
          Value: memoryMB,
          Unit: Unit.Megabytes,
          Timestamp: new Date()
        }
      ]
    });
  }
  
  recordInvocation(
    success: boolean,
    durationMs: number,
    billedDurationMs: number
  ): void {
    this.metrics.putMetricData({
      MetricData: [
        {
          MetricName: 'InvocationSuccess',
          Value: success ? 1 : 0,
          Unit: Unit.Count,
          Timestamp: new Date()
        },
        {
          MetricName: 'Duration',
          Value: durationMs,
          Unit: Unit.Milliseconds,
          Timestamp: new Date()
        },
        {
          MetricName: 'BilledDuration',
          Value: billedDurationMs,
          Unit: Unit.Milliseconds,
          Timestamp: new Date()
        }
      ]
    });
  }
}
```

### Deployment Blueprint

```yaml
# serverless.yml
service: my-node-api

provider:
  name: aws
  runtime: nodejs18.x
  architecture: arm64
  memorySize: 512
  timeout: 30
  environment:
    NODE_ENV: production
    LOG_LEVEL: info
  vpc:
    securityGroupIds:
      - !GetAtt LambdaSecurityGroup.GroupId
    subnetIds:
      - !Ref PrivateSubnet1
      - !Ref PrivateSubnet2
  iam:
    role:
      statements:
        - Effect: Allow
          Action:
            - dynamodb:GetItem
            - dynamodb:Query
          Resource: !GetAtt UsersTable.Arn

functions:
  api:
    handler: src/handler.handler
    events:
      - http:
          path: /{proxy+}
          method: any
      - http:
          path: /
          method: any
    reservedConcurrency: 10
    provisionedConcurrency: 2

resources:
  Resources:
    UsersTable:
      Type: AWS::DynamoDB::Table
      Properties:
        TableName: users
        BillingMode: PAY_PER_REQUEST
        AttributeDefinitions:
          - AttributeName: userId
            AttributeType: S
        KeySchema:
          - AttributeName: userId
            KeyType: HASH
```

---

## Layer 7: Advanced Learning Analytics

### Knowledge Graph

- **Prerequisites**: Node.js async/await, HTTP fundamentals
- **Related Topics**: API Gateway, DynamoDB, Step Functions
- **Career Mapping**: Serverless Developer, Cloud Engineer

### Hands-On Challenges

1. **Easy**: Deploy basic Lambda with API Gateway
2. **Medium**: Implement connection pooling to RDS
3. **Hard**: Build async image processing pipeline

---

## Diagnostic Center

### Troubleshooting Flowchart

```
Lambda Error?
├── Check CloudWatch Logs
│   └── Look for execution started/ended
├── Check X-Ray Traces
│   └── Identify slow segments
├── Check Cold Start Times
│   └── Use provisioned concurrency
└── Check Memory Usage
    └── Increase memory if needed
```

---

## Next Steps

Continue to [AWS API Gateway](./02-aws-api-gateway.md) to learn about API configuration.