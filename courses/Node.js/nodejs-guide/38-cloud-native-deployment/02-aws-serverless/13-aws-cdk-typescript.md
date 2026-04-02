# AWS CDK TypeScript

## What You'll Learn

- Advanced CDK TypeScript patterns
- How to create custom constructs
- How to implement testing for CDK stacks
- How to manage multi-environment deployments

---

## Layer 1: Advanced Patterns

### Custom Construct

```typescript
import { Construct } from 'constructs';
import { Function, Runtime, Code } from 'aws-cdk-lib/aws-lambda';
import { LambdaIntegration, RestApi } from 'aws-cdk-lib/aws-apigateway';
import { Table } from 'aws-cdk-lib/aws-dynamodb';

export interface NodeApiProps {
  table: Table;
  environment?: Record<string, string>;
}

export class NodeApi extends Construct {
  public readonly api: RestApi;
  public readonly handler: Function;

  constructor(scope: Construct, id: string, props: NodeApiProps) {
    super(scope, id);

    this.handler = new Function(this, 'Handler', {
      runtime: Runtime.NODEJS_18_X,
      handler: 'index.handler',
      code: Code.fromAsset('src'),
      environment: {
        TABLE_NAME: props.table.tableName,
        ...props.environment
      }
    });

    props.table.grantReadWriteData(this.handler);

    this.api = new RestApi(this, 'Api', {
      defaultIntegration: new LambdaIntegration(this.handler)
    });
  }
}
```

---

## Layer 2: Testing

### CDK Tests

```typescript
import { Template, Match } from 'aws-cdk-lib/assertions';
import { App, Stack } from 'aws-cdk-lib';
import { NodeApi } from './constructs/node-api';

test('creates Lambda function and API', () => {
  const app = new App();
  const stack = new Stack(app, 'TestStack');
  
  const table = Table.fromTableName(stack, 'Table', 'users');
  new NodeApi(stack, 'Api', { table });
  
  const template = Template.fromStack(stack);
  
  template.hasResourceProperties('AWS::Lambda::Function', {
    Runtime: 'nodejs18.x',
    Handler: 'index.handler'
  });
  
  template.hasResource('AWS::ApiGateway::RestApi', {});
});
```

---

## Next Steps

Continue to [AWS CDK Constructs](./14-aws-cdk-constructs.md) for library constructs.