# AWS CDK

## What You'll Learn

- How to use AWS Cloud Development Kit (CDK) for infrastructure
- How to define stacks with TypeScript
- How to implement reusable constructs
- How to synthesize and deploy CloudFormation templates

---

## Layer 1: Academic Foundation

### CDK Overview

AWS CDK is a software development framework for defining cloud infrastructure in code and provisioning it through CloudFormation.

**Advantages:**
- Imperative programming with TypeScript/JavaScript
- Reusable components (constructs)
- Strong typing and IDE support
- Composition of infrastructure

---

## Layer 2: Code Evolution

### Basic Stack

```typescript
import { App, Stack, StackProps } from 'aws-cdk-lib';
import { LambdaRestApi } from 'aws-cdk-lib/aws-apigateway';
import { Function, Runtime, Code } from 'aws-cdk-lib/aws-lambda';
import { Table, AttributeType } from 'aws-cdk-lib/aws-dynamodb';

class NodeApiStack extends Stack {
  constructor(scope: App, id: string, props?: StackProps) {
    super(scope, id, props);

    const table = new Table(this, 'UsersTable', {
      tableName: 'users',
      partitionKey: { name: 'userId', type: AttributeType.STRING },
      billingMode: 'PAY_PER_REQUEST'
    });

    const handler = new Function(this, 'ApiHandler', {
      runtime: Runtime.NODEJS_18_X,
      handler: 'index.handler',
      code: Code.fromAsset('src'),
      environment: {
        TABLE_NAME: table.tableName
      }
    });

    table.grantReadWriteData(handler);

    new LambdaRestApi(this, 'Api', {
      handler
    });
  }
}

const app = new App();
new NodeApiStack(app, 'NodeApiStack');
```

---

## Layer 3: Performance

### Synthesis

```bash
cdk synth           # Generate CloudFormation
cdk deploy          # Deploy stack
cdk diff            # Show changes
```

---

## Next Steps

Continue to [AWS CDK TypeScript](./13-aws-cdk-typescript.md) for advanced TypeScript usage.