---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Lambda Serverless
Purpose: Understanding AWS Lambda for serverless computing and event-driven architectures
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Lambda.md, 03_Practical_Lambda.md
UseCase: Running code without managing servers, event-driven processing
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

AWS Lambda enables serverless computing, eliminating server management while paying only for compute time used. This is fundamental to modern cloud architectures and often reduces costs by 60-80% for variable workloads.

### Why Lambda Matters

- **No Server Management**: Focus on code, not infrastructure
- **Cost Efficiency**: Pay only for requests and compute time
- **Scalability**: Automatic scaling from 0 to thousands
- **Event-Driven**: Triggered by 200+ AWS services
- **Speed**: Deploy functions in seconds

### Industry Statistics

- 80%+ of new cloud-native apps use serverless
- Lambda executes billions of invocations monthly
- Up to 90% cost reduction for bursty workloads

### When NOT to Use Lambda

- Long-running processes (>15 minutes)
- Persistent connections
- Heavy stateful processing
- Predictable steady-state loads

## WHAT

### Lambda Core Concepts

**Function**: Deployable code package containing runtime code and dependencies.

**Runtime**: Programming language and execution environment (Python, Node.js, Java, Go, Ruby, .NET).

**Event**: Trigger that causes function execution (HTTP request, S3 upload, etc.).

**Invocation**: Single execution of a function.

**Execution Environment**: Isolated runtime environment where function executes.

**Concurrency**: Number of function executions happening simultaneously.

### Key Limits and Quotas

| Parameter | Default | Can Increase? |
|----------|---------|------------|
| Memory | 128-10,240 MB | Yes |
| Timeout | 3-900 seconds (15 min) | Yes |
| Deployment Package | 50 MB (direct), 250 MB (layer) | No |
| Ephemeral Disk (/tmp) | 512 MB | No |
| Concurrent Executions | 1,000 | Yes |
| Requests per second | 10,000 | Yes |

### Architecture Diagram

```
                    LAMBDA ARCHITECTURE
                    ===============

    ┌─────────────────────────────────────────────────────┐
    │              EVENT SOURCES                     │
    │  ┌────────┐  ┌────────┐  ┌────────┐  │
    │  │  API  │  │   S3  │  │ Timer  │  │
    │  │Gateway│  │ Upload│  │ Event │  │
    │  └───┬───┘  └───┬───┘  └───┬───┘  │
    └──────┼──────────┼──────────┼──────────┘
           │          │          │
           ▼          ▼          ▼
    ┌─────────────────────────────────────────────┐
    │               LAMBDA SERVICE               │
    │  ┌───────────────────────────────────┐  │
    │  │    Function 1 (Node.js)          │  │
    │  │  ├─ Handler                    │  │
    │  │  ├─ Runtime                  │  │
    │  │  └─ Environment             │  │
    │  └───────────────────────────────────┘  │
    │  ┌───────────────────────────────────┐  │
    │  │    Function 2 (Python)            │  │
    │  │  └─ Code + Dependencies       │  │
    │  └───────────────────────────────────┘  │
    └─────────────────────────────────────────────┘
           │
           ▼
    ┌─────────────────────────────────────────────┐
    │          RESPONSES / OUTPUT               │
    │  ┌─────────┐  ┌─────────┐              │
    │  │  API    │  │   S3   │              │
    │  │Response │  │  Write  │              │
    │  └─────────┘  └─────────┘              │
    └─────────────────────────────────────────────┘
```

## HOW

### Example 1: Create and Deploy Lambda Function

```bash
# Step 1: Create function package (basic Node.js)
cat > index.js << 'EOF'
exports.handler = async (event) => {
    const response = {
        statusCode: 200,
        body: JSON.stringify({
            message: 'Hello from Lambda!',
            input: event
        })
    };
    return response;
};
EOF

# Create ZIP package
zip function.zip index.js

# Step 2: Create Lambda function
aws lambda create-function \
    --function-name my-hello-function \
    --runtime nodejs18.x \
    --role arn:aws:iam::123456789012:role/lambda-exec \
    --handler index.handler \
    --zip-file fileb://function.zip \
    --timeout 30 \
    --memory-size 256

# Step 3: Test function
aws lambda invoke \
    --function-name my-hello-function \
    --payload '{"key": "value"}' \
    response.json

# View output
cat response.json
```

### Example 2: Lambda with Environment Variables

```bash
# Create function with environment variables
aws lambda create-function \
    --function-name my-env-function \
    --runtime nodejs18.x \
    --role arn:aws:iam::123456789012:role/lambda-exec \
    --handler index.handler \
    --zip-file fileb://function.zip \
    --environment 'Variables={ENVIRONMENT=prod,LOG_LEVEL=info}'

# Update environment variables
aws lambda update-function-configuration \
    --function-name my-env-function \
    --environment 'Variables={ENVIRONMENT=staging}'

# Access in code:
# process.env.ENVIRONMENT
```

### Example 3: Lambda with S3 Trigger

```bash
# Create S3 event notification (via S3 bucket config)
aws s3api put-bucket-notification-configuration \
    --bucket my-upload-bucket \
    --notification-configuration '{
        "LambdaFunctionConfigurations": [{
            "Id": "image-processor",
            "LambdaFunctionArn": "arn:aws:lambda:us-east-1:123456789012:function:image-processor",
            "Events": ["s3:ObjectCreated:*"],
            "Filter": {
                "Key": {
                    "FilterRules": [{
                        "Name": "suffix",
                        "Value": ".jpg"
                    }]
                }
            }
        }]
    }'

# Function code to process S3 upload
# exports.handler = async (event) => {
#     for (const record of event.Records) {
#         const bucket = record.s3.bucket.name;
#         const key = record.s3.object.key;
#         // Process the image
#     }
# };
```

### Example 4: Lambda with API Gateway

```bash
# Step 1: Create API
aws apigatewayv2 create-api \
    --name my-api \
    --protocol-type HTTP \
    --route-key "GET /hello"

# Step 2: Create integration
aws apigatewayv2 create-integration \
    --api-id <api-id> \
    --integration-type LAMBDA_PROXY \
    --integration-uri arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-1:123456789012:function:my-hello-function/invocations \
    --payload-format-version 1.0

# Step 3: Default route to function
aws apigatewayv2 create-route \
    --api-id <api-id> \
    --route-key "GET /hello" \
    --target "integrations/<integration-id>"

# Step 4: Permissions
aws lambda add-permission \
    --function-name my-hello-function \
    --source-arn "arn:aws:execute-api:us-east-1:123456789012:<api-id>/*" \
    --statement-id "api-gateway" \
    --action lambda:InvokeFunction

# Invoke endpoint
curl https://<api-id>.execute-api.us-east-1.amazonaws.com/hello
```

## COMMON ISSUES

### 1. Function Timeout

**Problem**: Function exceeds 3-second default timeout.

**Solution**:
```bash
aws lambda update-function-configuration \
    --function-name my-function \
    --timeout 300
```

### 2. Out of Memory

**Problem**: Function exceeds memory limit.

**Solution**:
```bash
aws lambda update-function-configuration \
    --function-name my-function \
    --memory-size 10240
```

### 3. Permissions Denied

**Problem**: Function cannot access resources.

**Solution**:
- Add IAM role permissions for resource access
- Use environment variables for credentials
- Never embed credentials in code

### 4. Cold Start Latency

**Problem**: First invocation slow.

**Solution**:
- Use provisioned concurrency
- Keep functions warm with scheduled invocations
- Use Lambda@Edge for global distribution

## PERFORMANCE

### Performance Metrics

| Metric | Value |
|--------|-------|
| Cold start | 0.5-3 seconds |
| Warm start | <100ms |
| Max duration | 900 seconds |
| Max retries | 2 |

### Cost Model

- **Request**: $0.20 per 1M requests
- **Duration**: $0.0000166667 per GB-second

### Cost Optimization

- Use minimal memory (adds no performance)
- Remove unused dependencies
- Use provisioned concurrency wisely

## COMPATIBILITY

### Supported Runtimes

- Node.js (18.x, 20.x)
- Python (3.9, 3.10, 3.11)
- Java (11, 17, 21)
- Go (1.x)
- Ruby (3.0)
- .NET (6, 7, 8)
- Custom Runtime

### Event Sources

- API Gateway
- S3
- DynamoDB
- SNS
- SQS
- EventBridge
- CloudWatch Events
- Kinesis

## CROSS-REFERENCES

### Related Services

- API Gateway: HTTP endpoints
- Step Functions: Orchestration
- DynamoDB: Data persistence

### Alternatives

| Use Case | Use Instead |
|----------|------------|
| Long-running containers | ECS/Fargate |
| Batch processing | AWS Batch |
| Persistent containers | App Runner |

### Prerequisites

- Cloud Concepts basics

### What to Study Next

1. Advanced Lambda: Layers, VPC
2. Step Functions: Workflows
3. API Gateway: REST APIs