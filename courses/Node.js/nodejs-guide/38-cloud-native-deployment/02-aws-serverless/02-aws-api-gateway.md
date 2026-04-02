# AWS API Gateway

## What You'll Learn

- How to configure API Gateway for Lambda integration
- How to implement request/response transformations
- How to set up API keys and usage plans
- How to configure custom domains and certificates

---

## Layer 1: Academic Foundation

### API Gateway Types

| Type | Description | Use Case |
|------|-------------|----------|
| REST API | Full-featured REST API | Complex routing, caching |
| HTTP API | Lightweight HTTP API | Serverless, low cost |
| WebSocket API | Real-time bidirectional | Chat, live updates |

---

## Layer 2: Code Evolution

### REST API with Lambda Integration

```yaml
# serverless.yml
service: api-gateway-example

provider:
  name: aws
  runtime: nodejs18.x

functions:
  getUsers:
    handler: src/users.get
    events:
      - http:
          path: /users
          method: get
          cors: true
          cache: true
          cachePeriod: 300

  createUser:
    handler: src/users.create
    events:
      - http:
          path: /users
          method: post
          cors: true
          authorizer:
            name: authorizer
            type: COGNITO_USER_POOLS
            arn: !Ref CognitoUserPoolArn
```

### Lambda Authorizer

```typescript
// authorizer.ts
import { APIGatewayAuthorizerResult, APIGatewayRequestAuthorizerEvent } from 'aws-lambda';

export const handler = async (
  event: APIGatewayRequestAuthorizerEvent
): Promise<APIGatewayAuthorizerResult> => {
  const token = event.headers.authorization;
  
  if (!token) {
    return generatePolicy('Deny', 'AWS_IAM', '*');
  }
  
  try {
    const decoded = jwt.verify(token, process.env.JWT_SECRET);
    return generatePolicy('Allow', 'AWS_IAM', event.methodArn, decoded);
  } catch (error) {
    return generatePolicy('Deny', 'AWS_IAM', '*');
  }
};

function generatePolicy(
  effect: 'Allow' | 'Deny',
  resource: string,
  methodArn: string,
  context?: object
): APIGatewayAuthorizerResult {
  return {
    principalId: 'user',
    policyDocument: {
      Version: '2012-10-17',
      Statement: [
        {
          Action: 'execute-api:Invoke',
          Effect: effect,
          Resource: methodArn
        }
      ]
    },
    context
  };
}
```

---

## Layer 3: Performance

### Caching Strategies

| Strategy | TTL | Use Case |
|----------|-----|----------|
| No cache | 0 | Dynamic content |
| Low | 5-60s | Semi-static |
| Medium | 300-3600s | Static reference |
| High | 86400+ | Configuration |

---

## Layer 4: Security

### Request Validation

```yaml
# request-validator.yaml
- http:
    path: /users
    method: post
    request:
      schema:
        application/json:
          type: object
          required:
            - email
            - name
          properties:
            email:
              type: string
              format: email
            name:
              type: string
              minLength: 1
```

---

## Layer 5: Testing

### API Tests

```typescript
// api-test.ts
describe('API Gateway', () => {
  it('returns 401 without auth', async () => {
    const response = await request(app).get('/users');
    expect(response.status).toBe(401);
  });
  
  it('returns cached response', async () => {
    const response1 = await request(app).get('/users').set('Authorization', token);
    const response2 = await request(app).get('/users').set('Authorization', token);
    expect(response1.body).toEqual(response2.body);
  });
});
```

---

## Next Steps

Continue to [AWS DynamoDB](./03-aws-dynamodb.md) for data storage.