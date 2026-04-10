---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: API Gateway Practical
Purpose: Practical API Gateway implementation and best practices
Difficulty: practical
Prerequisites: 01_Basic_API_Gateway.md, 02_Advanced_API_Gateway.md
RelatedFiles: 01_Basic_API_Gateway.md, 02_Advanced_API_Gateway.md
UseCase: Production API implementation
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Practical API Gateway implementation involves building production-ready APIs with error handling, documentation, and operational best practices. This knowledge is essential for deploying enterprise APIs.

### Why Practical Implementation Matters

- **Reliability**: Production-grade error handling
- **Observability**: Proper logging and metrics
- **Security**: Comprehensive validation
- **Maintainability**: Clear documentation
- **Automation**: CI/CD integration

### Common Production Use Cases

- **REST API**: CRUD operations
- **Microservices**: Service communication
- **Mobile Backend**: Mobile app API
- **Webhook Handler**: Third-party callbacks

## WHAT

### API Design Best Practices

| Practice | Description |
|-----------|-------------|
| RESTful | Proper HTTP methods |
| Versioning | /v1, /v2 |
| Error Codes | Consistent responses |
| Pagination | Cursor-based |
| Rate Limiting | Clear limits |

### Error Response Format

```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid email format",
    "details": [
      {"field": "email", "message": "Must be valid email"}
    ]
  }
}
```

## HOW

### Example 1: REST API with CRUD

```python
# Lambda handler for CRUD operations
import json
import boto3
from datetime import datetime

dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table('users')

def handler(event, context):
    http_method = event['httpMethod']
    path_params = event.get('pathParameters', {})
    body = event.get('body', '{}')
    
    if http_method == 'GET':
        if path_params and 'id' in path_params:
            return get_user(path_params['id'])
        else:
            return list_users(event.get('queryStringParameters', {}))
    
    elif http_method == 'POST':
        return create_user(body)
    
    elif http_method == 'PUT':
        return update_user(path_params.get('id'), body)
    
    elif http_method == 'DELETE':
        return delete_user(path_params.get('id'))
    
    else:
        return error_response(405, "Method not allowed")

def get_user(user_id):
    response = table.get_item(Key={'user_id': user_id})
    item = response.get('Item')
    
    if item:
        return success_response(item)
    return error_response(404, "User not found")

def list_users(params):
    limit = int(params.get('limit', 10))
    cursor = params.get('cursor')
    
    query_params = {'Limit': limit}
    if cursor:
        query_params['ExclusiveStartKey'] = {'user_id': cursor}
    
    response = table.scan(**query_params)
    
    return success_response({
        'users': response.get('Items', []),
        'next_cursor': response.get('LastEvaluatedKey', {}).get('user_id')
    })

def create_user(body):
    data = json.loads(body)
    user_id = data.get('email').split('@')[0]
    
    item = {
        'user_id': user_id,
        'email': data['email'],
        'name': data.get('name'),
        'created_at': datetime.utcnow().isoformat()
    }
    
    table.put_item(Item=item)
    return success_response(item, 201)

def update_user(user_id, body):
    data = json.loads(body)
    updates = []
    values = {}
    
    for field in ['name', 'email']:
        if field in data:
            updates.append(f"{field} = :{field}")
            values[f":{field}"] = data[field]
    
    if updates:
        values[':updated_at'] = datetime.utcnow().isoformat()
        updates.append("updated_at = :updated_at")
        
        table.update_item(
            Key={'user_id': user_id},
            UpdateExpression=f"SET {', '.join(updates)}",
            ExpressionAttributeValues=values
        )
    
    return get_user(user_id)

def delete_user(user_id):
    table.delete_item(Key={'user_id': user_id})
    return success_response({"deleted": True})

def success_response(data, status_code=200):
    return {
        'statusCode': status_code,
        'headers': {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        'body': json.dumps(data)
    }

def error_response(code, message, status_code=400):
    return {
        'statusCode': status_code,
        'headers': {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        'body': json.dumps({
            'error': {
                'code': code,
                'message': message
            }
        })
    }
```

### Example 2: Webhook Handler

```python
# Webhook handler for external integrations
import json
import hmac
import hashlib
import boto3

def verify_signature(payload, signature, secret):
    """Verify webhook signature"""
    expected = hmac.new(
        secret.encode(),
        payload.encode(),
        hashlib.sha256
    ).hexdigest()
    return hmac.compare_digest(expected, signature)

def handler(event, context):
    headers = event.get('headers', {})
    signature = headers.get('X-Webhook-Signature')
    payload = event.get('body', '')
    
    # Verify signature
    if not verify_signature(payload, signature, 'webhook-secret'):
        return {
            'statusCode': 401,
            'body': json.dumps({'error': 'Invalid signature'})
        }
    
    # Parse payload
    data = json.loads(payload)
    event_type = headers.get('X-Event-Type', 'unknown')
    
    # Process based on event type
    handlers = {
        'payment.completed': handle_payment,
        'order.created': handle_order,
        'user.registered': handle_user
    }
    
    handler = handlers.get(event_type)
    if handler:
        result = handler(data)
        return success_response(result)
    
    return error_response(400, f"Unknown event: {event_type}")

def handle_payment(data):
    # Process payment
    payment_id = data.get('payment_id')
    amount = data.get('amount')
    
    # Process in background
    return {'processed': True, 'payment_id': payment_id}

def handle_order(data):
    order_id = data.get('order_id')
    return {'processed': True, 'order_id': order_id}

def handle_user(data):
    user_id = data.get('user_id')
    return {'processed': True, 'user_id': user_id}
```

### Example 3: API Documentation

```yaml
# OpenAPI 3.0 specification
openapi: 3.0.0
info:
  title: User Management API
  version: 1.0.0
  description: API for managing users

servers:
  - url: https://api.example.com/v1
    description: Production

paths:
  /users:
    get:
      summary: List users
      parameters:
        - name: limit
          in: query
          schema:
            type: integer
            default: 10
        - name: cursor
          in: query
          schema:
            type: string
      responses:
        '200':
          description: List of users
          content:
            application/json:
              schema:
                type: object
                properties:
                  users:
                    type: array
                    items:
                      $ref: '#/components/schemas/User'
                  next_cursor:
                    type: string
    post:
      summary: Create user
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/UserInput'
      responses:
        '201':
          description: User created

  /users/{user_id}:
    get:
      summary: Get user
      parameters:
        - name: user_id
          in: path
          required: true
          schema:
            type: string
      responses:
        '200':
          description: User details
        '404':
          description: User not found

components:
  schemas:
    User:
      type: object
      properties:
        user_id:
          type: string
        email:
          type: string
          format: email
        name:
          type: string
        created_at:
          type: string

    UserInput:
      type: object
      required:
        - email
      properties:
        email:
          type: string
          format: email
        name:
          type: string
```

### Example 4: API Gateway with CloudWatch

```bash
# Enable CloudWatch logging
aws apigateway update-stage \
    --rest-api-id abc123def45 \
    --stage-name prod \
    --patch-operations '[
        {"op": "replace", "path": "/~1prod/settings/loggingLevel", "value": "INFO"},
        {"op": "replace", "path": "/~1prod/settings/metricsEnabled", "value": "true"},
        {"op": "replace", "path": "/~1prod/settings/tracingEnabled", "value": "true"}
    ]'

# Create dashboard
aws cloudwatch put-dashboard \
    --dashboard-name api-dashboard \
    --dashboard-body '{
        "widgets": [
            {
                "type": "metric",
                "properties": {
                    "title": "API Latency",
                    "metrics": [
                        ["AWS/ApiGateway", "Latency", {"stat": "Average"}],
                        [".", "IntegrationLatency", {"stat": "Average"}]
                    ],
                    "period": 300,
                    "stat": "Average"
                }
            },
            {
                "type": "metric",
                "properties": {
                    "title": "API Errors",
                    "metrics": [
                        ["AWS/ApiGateway", "Error4xx", {"stat": "Sum"}],
                        [".", "Error5xx", {"stat": "Sum"}]
                    ],
                    "period": 300,
                    "stat": "Sum"
                }
            }
        ]
    }'

# Create alarm
aws cloudwatch put-metric-alarm \
    --alarm-name api-high-errors \
    --metric-name Error5xx \
    --namespace AWS/ApiGateway \
    --statistic Sum \
    --period 300 \
    --threshold 100 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 2 \
    --alarm-actions arn:aws:sns:us-east-1:123456789012:alerts
```

## COMMON ISSUES

### 1. Invalid JSON Body

**Problem**: Lambda fails to parse JSON.

**Solution**:
```python
# Handle JSON parsing
def handler(event, context):
    try:
        body = json.loads(event.get('body', '{}'))
    except json.JSONDecodeError:
        return error_response(400, "Invalid JSON")
```

### 2. Large Payload

**Problem**: Payload too large.

**Solution**:
- Use S3 for large uploads
- Use multipart upload
- Implement chunking

### 3. Concurrent Limit

**Problem**: Throttled.

**Solution**:
- Use usage plans for predictable limits
- Implement exponential backoff in client
- Request quota increase

## PERFORMANCE

### Performance Best Practices

| Practice | Impact |
|----------|--------|
| Enable caching | 95%+ hit rate |
| Regional endpoint | Lower latency |
| Gzip compression | 60% smaller |
| Lambda cold start | Use provisioned |

### Cost Optimization

| Practice | Savings |
|----------|---------|
| HTTP API over REST | 70% cheaper |
| Caching | 95%+ |
| Regional | Lower latency |

## COMPATIBILITY

### SDK Support

| Language | SDK |
|----------|-----|
| Python | boto3 |
| JavaScript | aws-sdk |
| Java | aws-java-sdk |
| Go | aws-sdk-go |

## CROSS-REFERENCES

### Related Patterns

- Microservices: Service communication
- REST: CRUD operations
- Webhook: Event handling

### What to Study Next

1. AppSync: GraphQL API
2. EventBridge: Event-driven
3. SAM: Serverless framework

## EXAM TIPS

### Key Exam Facts

- REST: Full-featured API
- HTTP API: Cheaper, simpler
- Caching: Reduces costs
- Authorizers: Security

### Exam Questions

- **Question**: "CRUD API" = REST API
- **Question**: "Cheapest option" = HTTP API
- **Question**: "Response caching" = Enable API Gateway cache
