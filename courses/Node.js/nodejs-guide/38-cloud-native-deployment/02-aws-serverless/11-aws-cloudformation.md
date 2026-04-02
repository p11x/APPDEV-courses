# AWS CloudFormation

## What You'll Learn

- How to create CloudFormation templates
- How to implement stack management
- How to use parameters and mappings
- How to handle stack outputs and exports

---

## Layer 1: Academic Foundation

### Infrastructure as Code

CloudFormation is AWS's native infrastructure as code service for modeling and provisioning AWS resources.

---

## Layer 2: Code Evolution

### Template Structure

```yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: Node.js Application Stack
Parameters:
  Environment:
    Type: String
    Default: dev
    AllowedValues:
      - dev
      - staging
      - production
Mappings:
  RegionMap:
    us-east-1:
      AmiId: ami-12345678
    us-west-2:
      AmiId: ami-87654321
Resources:
  NodeAppFunction:
    Type: AWS::Lambda::Function
    Properties:
      Handler: index.handler
      Runtime: nodejs18.x
      Role: !GetAtt LambdaExecutionRole.Arn
      Code:
        S3Bucket: !Ref CodeBucket
        S3Key: app.zip
      Environment:
        Variables:
          ENVIRONMENT: !Ref Environment
Outputs:
  ApiUrl:
    Description: API Gateway URL
    Value: !Sub https://${ApiGateway}.execute-api.${AWS::Region}.amazonaws.com/prod"
```

---

## Next Steps

Continue to [AWS CDK](./12-aws-cdk.md) for programmatic IaC.