# Serverless Framework

## What You'll Learn

- How to use Serverless Framework for AWS Lambda deployment
- How to configure serverless.yml
- How to implement plugins and custom resources
- How to manage multiple environments

---

## Layer 1: Academic Foundation

### Framework Overview

Serverless Framework is an open-source framework for building serverless applications across multiple cloud providers.

---

## Layer 2: Code Evolution

### serverless.yml

```yaml
service: my-node-api

provider:
  name: aws
  runtime: nodejs18.x
  stage: ${opt:stage, 'dev'}
  region: ${opt:region, 'us-east-1'}
  memorySize: 512
  timeout: 30
  environment:
    STAGE: ${self:provider.stage}
  iam:
    role:
      statements:
        - Effect: Allow
          Action:
            - dynamodb:GetItem
            - dynamodb:PutItem
          Resource: !GetAtt UsersTable.Arn

functions:
  users:
    handler: src/users.handler
    events:
      - http:
          path: /users
          method: get
      - http:
          path: /users
          method: post

resources:
  Resources:
    UsersTable:
      Type: AWS::DynamoDB::Table
      Properties:
        TableName: ${self:service}-users-${self:provider.stage}
        BillingMode: PAY_PER_REQUEST
        AttributeDefinitions:
          - AttributeName: id
            AttributeType: S
        KeySchema:
          - AttributeName: id
            KeyType: HASH

plugins:
  - serverless-offline
  - serverless-dotenv-plugin

package:
  individually: true
  exclude:
    - "**/*"
  include:
    - src/handler.js
```

---

## Layer 3: Performance

### Optimization

- Use `package.individually` for smaller functions
- Exclude node_modules, include only needed packages

---

## Next Steps

Continue to [AWS SAM](./10-aws-sam.md) for alternative deployment framework.