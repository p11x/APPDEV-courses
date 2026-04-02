# AWS SAM

## What You'll Learn

- How to use AWS SAM for serverless application deployment
- How to define SAM templates
- How to implement local testing with SAM CLI
- How to manage deployments with SAM pipelines

---

## Layer 1: Academic Foundation

### SAM Overview

AWS Serverless Application Model (SAM) is an open-source framework for building serverless applications on AWS.

---

## Layer 2: Code Evolution

### template.yaml

```yaml
AWSTemplateFormatVersion: '2010-09-09'
Transform: AWS::Serverless-2016-10-31

Description: Node.js API with SAM

Globals:
  Function:
    Runtime: nodejs18.x
    MemorySize: 512
    Timeout: 30

Resources:
  ApiFunction:
    Type: AWS::Serverless::Function
    Properties:
      Handler: src/handler.handler
      CodeUri: ./src
      Events:
        Api:
          Type: Api
          Properties:
            Path: /{proxy+}
            Method: ANY
      Environment:
        Variables:
          TABLE_NAME: !Ref UsersTable

  UsersTable:
    Type: AWS::Serverless::SimpleTable
    Properties:
      TableName: users
```

---

## Layer 3: Performance

### Local Testing

```bash
sam build
sam local invoke
sam local start-api
```

---

## Next Steps

Continue to [AWS CloudFormation](./11-aws-cloudformation.md) for infrastructure as code.