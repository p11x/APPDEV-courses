---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: API Gateway
Purpose: Understanding Amazon API Gateway for API management
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_API_Gateway.md, 03_Practical_API_Gateway.md
UseCase: Creating and managing REST APIs
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Amazon API Gateway is a fully managed service for creating, publishing, maintaining, and securing APIs at any scale. Understanding API Gateway is essential for building serverless and microservice architectures.

### Why API Gateway Matters

- **Serverless**: No servers to manage
- **Scalability**: Handles millions of requests
- **Security**: Built-in auth and throttling
- **Monitoring**: Integrated CloudWatch
- **Versioning**: Multiple API versions
- **Mock Responses**: Test without backend

### Industry Statistics

- Powers 70%+ of AWS serverless APIs
- Handles billions of API requests daily
- 99.95% availability SLA
- Integrated with 100+ AWS services

### When NOT to Use API Gateway

- Simple static content: Use S3 + CloudFront
- WebSocket connections: Use AppSync
- Complex routing: Use ALB
- GraphQL: Use AppSync

## WHAT

### API Types

| Type | Protocol | Use Case |
|------|----------|----------|
| REST | HTTP | Standard APIs |
| HTTP | HTTP | Simple, lightweight |
| WebSocket | WebSocket | Real-time apps |
| GraphQL | GraphQL | Flexible queries |

### Core Concepts

**API Endpoint Types**:

| Type | URL Format | Use |
|------|-----------|-----|
| Regional | api.region.amazonaws.com | Low latency |
| Edge-Optimized | api.amazonaws.com | Global |
| Private | api.id.vpce.amazonaws.com | VPC |

**Stages**: Environment (dev, staging, prod).

**Resources**: URL paths (/users, /orders).

**Methods**: HTTP verbs (GET, POST, PUT, DELETE).

### Architecture Diagram

```
                    API GATEWAY ARCHITECTURE
                    ========================

    ┌──────────────┐    ┌──────────────┐    ┌──────────────┐
    │  Web App   │    │  Mobile App │    │  IoT Device │
    └──────┬──────┘    └──────┬──────┘    └──────┬──────┘
           │                   │                   │
           ▼                   ▼                   ▼
    ┌─────────────────────────────────────────────────────┐
    │              API GATEWAY                           │
    │  ┌────────────┐ ┌────────────┐ ┌──────────────────┐ │
    │  │ Throttling│ │    Auth   │ │ Request/Response│ │
    │  │           │ │   (Cognito)│ │    Transform    │ │
    │  └────────────┘ └────────────┘ └──────────────────┘ │
    └─────────────────────────────────────────────────────┘
           │                   │                   │
           ▼                   ▼                   ▼
    ┌──────────────┐    ┌──────────────┐    ┌──────────────┐
    │   Lambda    │    │    EC2      │    │    HTTP     │
    │ (Serverless)│    │  (Compute)  │    │  (External)  │
    └──────────────┘    └──────────────┘    └──────────────┘
```

### Integration Types

| Type | Backend | Use Case |
|------|---------|----------|
| Lambda | AWS Lambda | Serverless |
| HTTP | External HTTP | Integrations |
| AWS | AWS Service | Direct calls |
| Mock | Custom response | Testing |

## HOW

### Example 1: Create REST API

```bash
# Create API
aws apigateway create-rest-api \
    --name my-api \
    --description "My first API" \
    --endpoint-configuration types REGIONAL

# Get API ID
# "id": "abc123def45"

# Get root resource ID
aws apigateway get-resources \
    --rest-api-id abc123def45

# Create resource
aws apigateway create-resource \
    --rest-api-id abc123def45 \
    --parent-id root-resource-id \
    --path-part users

# Create GET method
aws apigateway put-method \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method GET \
    --authorization-type NONE \
    --request-parameters method.request.querystring.id=false

# Create Lambda integration
aws apigateway put-integration \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method GET \
    --type AWS \
    --integration-http-method POST \
    --uri 'arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-1:123456789012:function:getUsers/invocations'

# Deploy API
aws apigateway create-deployment \
    --rest-api-id abc123def45 \
    --stage-name prod

# Get invoke URL
# https://abc123def45.execute-api.us-east-1.amazonaws.com/prod/users
```

### Example 2: Configure API Keys

```bash
# Create API key
aws apigateway create-api-key \
    --name my-api-key \
    --description "API key for mobile app"

# Enable API key for stage
aws apigateway update-stage \
    --rest-api-id abc123def45 \
    --stage-name prod \
    --patch-operations '[
        {"op": "replace", "path": "/~1prod/settings/apiKeyRequired", "value": "true"}
    ]'

# Create usage plan
aws apigateway create-usage-plan \
    --name mobile-usage-plan \
    --description "Usage plan for mobile app" \
    --quota-settings '{"Limit": 1000000, "Period": "MONTH"}' \
    --throttle-settings '{"BurstLimit": 500, "RateLimit": 1000}'

# Associate API key with usage plan
aws apigateway create-usage-plan-key \
    --usage-plan-id usage-plan-id \
    --key-type API_KEY \
    --key-id api-key-id

# Use API key in requests
curl -H "x-api-key: api-key-value" \
     https://abc123def45.execute-api.us-east-1.amazonaws.com/prod/users
```

### Example 3: Configure CORS

```bash
# Enable CORS on resource
aws apigateway put-method-response \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method OPTIONS \
    --status-code 200 \
    --response-models '{"application/json": "Empty"}' \
    --response-parameters '{
        "method.response.header.Access-Control-Allow-Headers": true,
        "method.response.header.Access-Control-Allow-Methods": true,
        "method.response.header.Access-Control-Allow-Origin": true
    }'

# Create OPTIONS integration (mock)
aws apigateway put-integration \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method OPTIONS \
    --type MOCK \
    --request-templates '{"application/json": "{\"statusCode\": 200}"}'

# Add headers
aws apigateway put-integration-response \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method OPTIONS \
    --status-code 200 \
    --response-parameters '{
        "method.response.header.Access-Control-Allow-Headers": "'Content-Type,Authorization'",
        "method.response.header.Access-Control-Allow-Methods": "'GET,POST,PUT,DELETE,OPTIONS'",
        "method.response.header.Access-Control-Allow-Origin": "'*'"
    }'
```

### Example 4: Request/Response Mapping

```bash
# Create mapping template for request
aws apigateway put-integration \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method POST \
    --type LAMBDA \
    --integration-http-method POST \
    --uri 'arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-1:123456789012:function:createUser/invocations' \
    --request-templates '{"application/json": "{\"body\": $input.json(\"$\"), \"id\": \"$input.params(\"id\")\"}"}'

# Create mapping template for response
aws apigateway put-method-response \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method POST \
    --status-code 201

aws apigateway put-integration-response \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method POST \
    --status-code 201 \
    --response-templates '{"application/json": "{\"result\": $input.json(\"result\"), \"id\": $input.json(\"id\")}"}'

# Velocity template examples
# $input.json('$.user.name')
# $input.params('id')
# $input.path('$')
```

## COMMON ISSUES

### 1. 403 Access Denied

**Problem**: Lambda permissions missing.

**Solution**:
```bash
# Add Lambda permission
aws lambda add-permission \
    --function-name my-function \
    --statement-id api-gateway \
    --action lambda:InvokeFunction \
    --principal apigateway.amazonaws.com \
    --source-arn "arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/my-function/invocations"
```

### 2. 504 Gateway Timeout

**Problem**: Lambda taking too long.

**Solution**:
```bash
# Increase timeout in Lambda
aws lambda update-function-configuration \
    --function-name my-function \
    --timeout 30

# Or use async invocation
# Set integration to Lambda non-proxy
# Use callback URL pattern
```

### 3. High Latency

**Problem**: API responses slow.

**Solution**:
- Enable caching
- Use regional endpoint
- Optimize Lambda cold start
- Use CloudFront with API Gateway

### 4. CORS Issues

**Problem**: OPTIONS not configured.

**Solution**:
- Add OPTIONS method on all resources
- Use Lambda authorizer for validation
- Test with curl:
```bash
curl -X OPTIONS https://api.example.com/users \
  -H "Origin: http://example.com" \
  -H "Access-Control-Request-Method: GET"
```

## PERFORMANCE

### Performance Characteristics

| Metric | Value |
|--------|-------|
| Latency | 10-100ms |
| Max request size | 10MB |
| Max response size | 10MB |
| Timeout | 29s (Lambda) |
| Concurrent calls | Regional limit |

### Cost Optimization

| Strategy | Savings |
|----------|---------|
| Caching | 95%+ requests |
| Regional endpoint | Lower latency |
| HTTP API | 70% cheaper |
| Reserved capacity | 40% cheaper |

## COMPATIBILITY

### Region Availability

- All commercial AWS Regions
- GovCloud available
- China requires account

### Integration

| Service | Use Case |
|---------|----------|
| Lambda | Serverless backend |
| Cognito | User auth |
| CloudWatch | Monitoring |
| WAF | Security |

## CROSS-REFERENCES

### Related Services

- Lambda: Backend compute
- Cognito: Authentication
- CloudFront: CDN
- AppSync: GraphQL

### Alternatives

| Need | Use |
|------|-----|
| GraphQL | AppSync |
| WebSocket | AppSync |
| Simple HTTP | HTTP API |
| Load balancing | ALB |

### What to Study Next

1. Advanced API Gateway: Custom authorizers
2. Practical API Gateway: Best practices
3. Serverless: Lambda + API Gateway

## EXAM TIPS

### Key Exam Facts

- REST API: Full features
- HTTP API: Lightweight, cheaper
- WebSocket: Real-time
- Edge-optimized: Global users
- Caching: Reduces calls

### Exam Questions

- **Question**: "Cheapest API type" = HTTP API
- **Question**: "Real-time apps" = WebSocket
- **Question**: "Global users" = Edge-optimized
