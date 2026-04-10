---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: API Gateway Advanced
Purpose: Advanced API Gateway configurations, authorizers, and optimization
Difficulty: advanced
Prerequisites: 01_Basic_API_Gateway.md
RelatedFiles: 01_Basic_API_Gateway.md, 03_Practical_API_Gateway.md
UseCase: Production API with custom authentication and optimization
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Advanced API Gateway configurations enable production-grade APIs with custom authentication, request validation, and performance optimization. Understanding these concepts is essential for building secure, scalable APIs.

### Why Advanced Configuration Matters

- **Security**: Custom authorization logic
- **Validation**: Input validation at the edge
- **Optimization**: Caching, throttling
- **Observability**: Detailed metrics
- **Versioning**: Multiple API versions

### Advanced Use Cases

- **Multi-tenant APIs**: Tenant isolation
- **OAuth/Op Access**: Third-party integrations
- **Rate Limiting**: Per-client throttling
- **API Versioning**: Smooth migrations

## WHAT

### Authorizer Types

| Type | Auth Method | Use Case |
|------|-------------|----------|
| Lambda (Token) | Bearer token | Custom auth |
| Lambda (Request) | Headers, query | Complex auth |
| Cognito User Pools | JWT | User authentication |
| IAM | AWS SigV4 | AWS service calls |

### Request Validation

```
    REQUEST VALIDATION FLOW
    ======================

    Client Request
           │
           ▼
    ┌──────────────────────┐
    │  Validation Check   │
    │  - Headers           │
    │  - Query params     │
    │  - Body schema       │
    └──────────────────────┘
           │
    ┌───────┴───────┐
    │               │
    ▼               ▼
 Invalid        Valid
 Response        Request
                    │
                    ▼
            Backend (Lambda)
```

### Cache Configuration

| Setting | Value | Effect |
|---------|-------|--------|
| Cache enabled | On | Cache responses |
| Cache TTL | 300s | Cache duration |
| Cache key | $input.params('id') | What to cache |

## HOW

### Example 1: Lambda Authorizer

```bash
# Create Lambda authorizer function
aws lambda create-function \
    --function-name api-authorizer \
    --runtime python3.11 \
    --role authorizer-role \
    --handler index.handler \
    --zip-file fileb://authorizer.zip

# Create authorizer in API Gateway
aws apigateway create-authorizer \
    --rest-api-id abc123def45 \
    --name token-authorizer \
    --type TOKEN \
    --authorizer-uri 'arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/arn:aws:lambda:us-east-1:123456789012:function:api-authorizer/invocations' \
    --identity-source header=Authorization \
    --type TOKEN \
    --authorizer-result-ttl-in-seconds 300

# Lambda authorizer code
import json
import jwt

def handler(event, context):
    # Get token from header
    token = event.get('authorizationToken')
    
    if not token:
        raise Exception('Unauthorized')
    
    try:
        # Decode JWT
        payload = jwt.decode(token, 'secret', algorithms=['HS256'])
        
        # Return policy
        return {
            'principalId': payload.get('user_id'),
            'policyDocument': {
                'Version': '2012-10-17',
                'Statement': [{
                    'Action': 'execute-api:Invoke',
                    'Effect': 'Allow',
                    'Resource': event['methodArn']
                }]
            },
            'context': {
                'user_id': payload.get('user_id'),
                'email': payload.get('email')
            }
        }
    except jwt.InvalidTokenError:
        raise Exception('Unauthorized')
```

### Example 2: Cognito Authorizer

```bash
# Create Cognito User Pool
aws cognito-idp create-user-pool \
    --pool-name my-user-pool \
    --username-attributes email

# Create User Pool Client
aws cognito-idp create-user-pool-client \
    --user-pool-id us-east-1_xxxxx \
    --client-name my-app-client

# Create authorizer
aws apigateway create-authorizer \
    --rest-api-id abc123def45 \
    --name cognito-authorizer \
    --type COGNITO_USER_POOLS \
    --provider-arns 'arn:aws:cognito-idp:us-east-1:123456789012:userpool/us-east-1_xxxxx' \
    --identity-source header=Authorization

# Update method to use Cognito authorizer
aws apigateway put-method \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method GET \
    --authorization-type COGNITO_USER_POOLS \
    --authorizer-id authorizer-id
```

### Example 3: Request Validation

```bash
# Create request validator
aws apigateway update-rest-api \
    --rest-api-id abc123def45 \
    --patch-operations '[
        {"op": "replace", "path": "/~1settings/defaultCorsCorsAllowOrigin", "value": "*"},
        {"op": "replace", "path": "/~1settings/defaultCorsCorsAllowCredentials", "value": "false"}
    ]'

# Enable request validation on method
aws apigateway put-method \
    --rest-api-id abc123def45 \
    --resource-id resource-id \
    --http-method POST \
    --authorization-type NONE \
    --request-validator-id request-validator-id \
    --request-parameters '{
        "method.request.header.Content-Type": true,
        "method.request.body.schema": {"$ref": "#/definitions/UserInput"}
    }'

# Define model
aws apigateway create-model \
    --rest-api-id abc123def45 \
    --content-type application/json \
    --name UserInput \
    --schema '{
        "type": "object",
        "required": ["email", "name"],
        "properties": {
            "email": {"type": "string", "format": "email"},
            "name": {"type": "string", "minLength": 1}
        }
    }'
```

### Example 4: Usage Plans and Throttling

```bash
# Create basic usage plan
aws apigateway create-usage-plan \
    --name basic-plan \
    --description "Basic tier" \
    --quota-settings '{"Limit": 10000, "Period": "MONTH"}' \
    --throttle-settings '{"BurstLimit": 100, "RateLimit": 50}'

# Create premium usage plan
aws apigateway create-usage-plan \
    --name premium-plan \
    --description "Premium tier" \
    --quota-settings '{"Limit": 1000000, "Period": "MONTH"}' \
    --throttle-settings '{"BurstLimit": 5000, "RateLimit": 1000}'

# Associate stage with usage plan
aws apigateway create-usage-plan-key \
    --usage-plan-id plan-id \
    --key-type API_KEY \
    --key-id key-id

# Per-client throttling with Lambda
# In Lambda, track client usage in DynamoDB
def check_rate_limit(client_id):
    # Get client quota from DynamoDB
    client = dynamodb.get_item(
        Key={'client_id': client_id}
    )
    
    limit = client.get('limit', 100)
    used = client.get('used', 0)
    
    if used >= limit:
        raise Exception('Rate limit exceeded')
    
    # Increment usage
    dynamodb.update_item(
        Key={'client_id': client_id},
        UpdateExpression='SET used = used + :inc',
        ExpressionAttributeValues={':inc': 1}
    )
```

## COMMON ISSUES

### 1. Authorizer Caching Issues

**Problem**: Stale token accepted.

**Solution**:
```bash
# Reduce TTL
aws apigateway update-authorizer \
    --rest-api-id abc123def45 \
    --authorizer-id authorizer-id \
    --authorizer-result-ttl-in-seconds 60
```

### 2. CORS Configuration Complexity

**Problem**: CORS not working.

**Solution**:
```python
# Use Lambda for CORS handling
def lambda_handler(event, context):
    if event['httpMethod'] == 'OPTIONS':
        return {
            'statusCode': 200,
            'headers': {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Headers': 'Content-Type,Authorization',
                'Access-Control-Allow-Methods': 'GET,POST,PUT,DELETE,OPTIONS'
            },
            'body': ''
        }
    
    # Handle actual request
    return handle_request(event)
```

### 3. Cache Invalidation

**Problem**: Need to clear cache.

**Solution**:
```bash
# Flush entire cache
aws apigateway flush-stage-cache \
    --rest-api-id abc123def45 \
    --stage-name prod

# Use cache key parameters
# Include all varying parameters in cache key
```

## PERFORMANCE

### Performance Optimization

| Setting | Recommended | Effect |
|---------|-------------|--------|
| Caching | Enable | 95%+ cache hit |
| Compression | Enable | 60% size reduction |
| Regional | Default | Low latency |
| Integration | Lambda | Fast cold start |

### Monitoring Metrics

| Metric | Description | Alert |
|--------|-------------|-------|
| Latency | Request latency | >5s |
| ErrorRate | Error percentage | >1% |
| CacheHitRate | Cache hit % | <80% |
| Throttle | Throttled requests | >0 |

## COMPATIBILITY

### Cross-Platform Comparison

| Feature | AWS API Gateway | Azure APIM | GCP Apigee |
|---------|-----------------|------------|------------|
| TLS | Yes | Yes | Yes |
| Custom domain | Yes | Yes | Yes |
| JWT validation | Yes | Yes | Yes |
| Caching | Yes | Yes | Yes |
| Mock responses | Yes | Yes | Yes |
| WebSocket | Yes | No | Yes |

### Supported Auth

| Auth Type | API Type |
|-----------|-----------|
| None | All |
| API Key | REST, HTTP |
| IAM | REST, HTTP |
| Cognito | REST, HTTP |
| Lambda | REST |

## CROSS-REFERENCES

### Related Services

- Lambda: Backend
- Cognito: User pools
- WAF: Rate limiting
- CloudWatch: Monitoring

### Prerequisites

- Basic API Gateway
- Lambda basics
- IAM roles

### What to Study Next

1. Practical API Gateway: Implementation
2. AppSync: GraphQL
3. Serverless: Best practices

## EXAM TIPS

### Key Exam Facts

- Lambda authorizer: Custom JWT/Token validation
- Cognito: Managed user pools
- Request validator: Schema validation
- Usage plans: Quota and throttling

### Exam Questions

- **Question**: "Validate JSON body" = Request validator
- **Question**: "JWT validation" = Cognito authorizer
- **Question**: "Per-client limits" = Usage plan
