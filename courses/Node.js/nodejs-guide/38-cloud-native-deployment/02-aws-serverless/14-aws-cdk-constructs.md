# AWS CDK Constructs

## What You'll Learn

- How to use CDK Construct Library
- How to implement well-architected patterns
- How to create reusable infrastructure components
- How to compose multiple constructs

---

## Layer 1: Construct Library

### Common Constructs

```typescript
import { 
  aws_lambda as lambda,
  aws_dynamodb as dynamodb,
  aws_apigateway as apigateway
} from 'aws-cdk-lib';

// Lambda URL
const url = lambda.Function.fromFunctionArn(stack, 'Url', fnArn).addFunctionUrl({
  authType: lambda.FunctionUrlAuthType.AWS_IAM
});

// DynamoDB Stream to Lambda
const stream = new dynamodb.Table(this, 'Table', {
  stream: dynamodb.StreamViewType.NEW_IMAGE
});

stream.grantStreamRead(handler);
```

---

## Next Steps

Continue to [CDK Best Practices](./15-aws-cdk-best-practices.md)