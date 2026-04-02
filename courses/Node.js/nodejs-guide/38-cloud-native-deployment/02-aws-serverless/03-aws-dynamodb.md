# AWS DynamoDB

## What You'll Learn

- How to design DynamoDB tables for Node.js applications
- How to implement single-table design patterns
- How to perform CRUD operations with the AWS SDK
- How to optimize query patterns and access patterns

---

## Layer 1: Academic Foundation

### Data Modeling Concepts

DynamoDB is a fully managed NoSQL database that provides single-digit millisecond latency at any scale.

**Key Concepts:**
- **Partition Key**: Determines data distribution
- **Sort Key**: Enables efficient range queries
- **GSI**: Global Secondary Index for alternative access patterns
- **LSI**: Local Secondary Index for additional sort options
- **Capacity Units**: Read/Write throughput measurement

---

## Layer 2: Code Evolution

### SDK v3 Implementation

```typescript
// dynamodb-client.ts
import { DynamoDBClient } from '@aws-sdk/client-dynamodb';
import {
  DynamoDBDocumentClient,
  GetCommand,
  PutCommand,
  QueryCommand,
  ScanCommand,
  DeleteCommand,
  UpdateCommand
} from '@aws-sdk/lib-dynamodb';

const client = new DynamoDBClient({ region: process.env.AWS_REGION });
const docClient = DynamoDBDocumentClient.from(client);

export class UserRepository {
  private tableName = 'users';
  
  async getUser(userId: string) {
    const result = await docClient.send(
      new GetCommand({
        TableName: this.tableName,
        Key: { userId }
      })
    );
    return result.Item;
  }
  
  async createUser(user: User) {
    await docClient.send(
      new PutCommand({
        TableName: this.tableName,
        Item: user,
        ConditionExpression: 'attribute_not_exists(userId)'
      })
    );
    return user;
  }
  
  async updateUser(userId: string, updates: Partial<User>) {
    const updateExpr = Object.keys(updates)
      .map((key, i) => `#key${i} = :val${i}`)
      .join(', ');
    const exprNames = Object.keys(updates).reduce(
      (acc, key, i) => ({ ...acc, [`#key${i}`]: key }), {}
    );
    const exprValues = Object.entries(updates).reduce(
      (acc, [_, value], i) => ({ ...acc, [`:val${i}`]: value }), {}
    );
    
    await docClient.send(
      new UpdateCommand({
        TableName: this.tableName,
        Key: { userId },
        UpdateExpression: `SET ${updateExpr}`,
        ExpressionAttributeNames: exprNames,
        ExpressionAttributeValues: exprValues,
        ReturnValues: 'ALL_NEW'
      })
    );
  }
  
  async queryByEmail(email: string) {
    const result = await docClient.send(
      new QueryCommand({
        TableName: this.tableName,
        IndexName: 'email-index',
        KeyConditionExpression: 'email = :email',
        ExpressionAttributeValues: { ':email': email }
      })
    );
    return result.Items;
  }
}
```

---

## Layer 3: Performance

### Query Optimization

| Pattern | RCU | Use Case |
|---------|-----|----------|
| GetItem | 0.5 | Single item lookup |
| Query | 0.5-1.5 | Partition + range |
| Scan | Full table | Full table scan (avoid) |

---

## Layer 4: Security

### Encryption

- Server-side encryption enabled by default
- CMK for customer-managed keys
- Fine-grained access with IAM policies

---

## Next Steps

Continue to [AWS EventBridge](./04-aws-eventbridge.md) for event-driven architecture.