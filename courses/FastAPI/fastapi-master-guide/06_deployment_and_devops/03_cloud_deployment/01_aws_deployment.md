# AWS Deployment

## Overview

AWS provides multiple services for deploying FastAPI applications. This guide covers ECS, Lambda, and Elastic Beanstalk deployment patterns.

## ECS Deployment

### ECS with Fargate

```yaml
# Example 1: ECS task definition (task-definition.json)
{
  "family": "fastapi-app",
  "networkMode": "awsvpc",
  "requiresCompatibilities": ["FARGATE"],
  "cpu": "256",
  "memory": "512",
  "executionRoleArn": "arn:aws:iam::role/ecsTaskExecutionRole",
  "containerDefinitions": [
    {
      "name": "fastapi",
      "image": "123456789.dkr.ecr.us-east-1.amazonaws.com/fastapi-app:latest",
      "portMappings": [
        {
          "containerPort": 8000,
          "protocol": "tcp"
        }
      ],
      "environment": [
        {"name": "ENVIRONMENT", "value": "production"}
      ],
      "secrets": [
        {
          "name": "DATABASE_URL",
          "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:db-url"
        }
      ],
      "logConfiguration": {
        "logDriver": "awslogs",
        "options": {
          "awslogs-group": "/ecs/fastapi-app",
          "awslogs-region": "us-east-1",
          "awslogs-stream-prefix": "ecs"
        }
      },
      "healthCheck": {
        "command": ["CMD-SHELL", "curl -f http://localhost:8000/health || exit 1"],
        "interval": 30,
        "timeout": 5,
        "retries": 3
      }
    }
  ]
}
```

### ECS Service

```yaml
# Example 2: CloudFormation ECS service
AWSTemplateFormatVersion: '2010-09-09'
Resources:
  ECSCluster:
    Type: AWS::ECS::Cluster
    Properties:
      ClusterName: fastapi-cluster

  ECSService:
    Type: AWS::ECS::Service
    Properties:
      ServiceName: fastapi-service
      Cluster: !Ref ECSCluster
      TaskDefinition: !Ref TaskDefinition
      DesiredCount: 2
      LaunchType: FARGATE
      NetworkConfiguration:
        AwsvpcConfiguration:
          Subnets:
            - subnet-12345
            - subnet-67890
          SecurityGroups:
            - sg-12345

  TaskDefinition:
    Type: AWS::ECS::TaskDefinition
    Properties:
      Family: fastapi-app
      Cpu: '256'
      Memory: '512'
      NetworkMode: awsvpc
      RequiresCompatibilities: [FARGATE]
      ContainerDefinitions:
        - Name: fastapi
          Image: 123456789.dkr.ecr.us-east-1.amazonaws.com/fastapi-app:latest
          PortMappings:
            - ContainerPort: 8000
```

## AWS Lambda

### Lambda Deployment

```python
# Example 3: FastAPI on Lambda with Mangum
# app/main.py
from fastapi import FastAPI
from mangum import Mangum

app = FastAPI()

@app.get("/")
def root():
    return {"message": "Hello from Lambda!"}

@app.get("/health")
def health():
    return {"status": "healthy"}

# Lambda handler
handler = Mangum(app)
```

```yaml
# Example 4: Serverless Framework (serverless.yml)
service: fastapi-app

provider:
  name: aws
  runtime: python3.11
  region: us-east-1
  memorySize: 256
  timeout: 30

functions:
  api:
    handler: app.main.handler
    events:
      - http:
          path: /
          method: ANY
      - http:
          path: /{proxy+}
          method: ANY

package:
  patterns:
    - '!node_modules/**'
    - '!.git/**'
    - '!tests/**'
```

### Lambda with SAM

```yaml
# Example 5: SAM template (template.yaml)
AWSTemplateFormatVersion: '2010-09-09'
Transform: AWS::Serverless-2016-10-31

Globals:
  Function:
    Timeout: 30
    MemorySize: 256

Resources:
  FastAPIFunction:
    Type: AWS::Serverless::Function
    Properties:
      Handler: app.main.handler
      Runtime: python3.11
      CodeUri: .
      Events:
        Api:
          Type: HttpApi
          Path: /{proxy+}
          Method: ANY

Outputs:
  ApiUrl:
    Description: "API Gateway endpoint URL"
    Value: !Sub "https://${ServerlessHttpApi}.execute-api.${AWS::Region}.amazonaws.com/"
```

## Elastic Beanstalk

### EB Configuration

```yaml
# Example 6: .ebextensions/python.config
option_settings:
  aws:elasticbeanstalk:container:python:
    WSGIPath: app.main:app
  aws:elasticbeanstalk:environment:proxy:
    ProxyServer: nginx
  aws:elasticbeanstalk:application:environment:
    ENVIRONMENT: production
```

```bash
# Example 7: EB deployment commands

# Initialize EB
eb init -p python-3.11 my-fastapi-app

# Create environment
eb create production --instance-type t3.small

# Deploy
eb deploy

# View logs
eb logs

# Open application
eb open
```

## RDS Integration

### Database Setup

```python
# Example 8: RDS configuration
import os
from sqlalchemy import create_engine

DATABASE_URL = os.environ.get(
    "DATABASE_URL",
    "postgresql://user:password@localhost/dbname"
)

engine = create_engine(
    DATABASE_URL,
    pool_size=5,
    max_overflow=10,
    pool_pre_ping=True
)
```

## Secrets Manager

### Managing Secrets

```python
# Example 9: AWS Secrets Manager integration
import boto3
import json
from functools import lru_cache

@lru_cache()
def get_secret(secret_name: str) -> dict:
    """Retrieve secret from AWS Secrets Manager"""
    client = boto3.client('secretsmanager')
    response = client.get_secret_value(SecretId=secret_name)
    return json.loads(response['SecretString'])

# Usage
secrets = get_secret("fastapi/production")
DATABASE_URL = secrets["DATABASE_URL"]
SECRET_KEY = secrets["SECRET_KEY"]
```

## CloudFront + S3

### Static Assets

```yaml
# Example 10: CloudFront for static assets
Resources:
  StaticBucket:
    Type: AWS::S3::Bucket
    Properties:
      BucketName: my-fastapi-static

  CloudFrontDistribution:
    Type: AWS::CloudFront::Distribution
    Properties:
      DistributionConfig:
        Origins:
          - DomainName: !GetAtt StaticBucket.DomainName
            Id: S3Origin
            S3OriginConfig:
              OriginAccessIdentity: !Sub "origin-access-identity/cloudfront/${OAI}"
        DefaultCacheBehavior:
          TargetOriginId: S3Origin
          ViewerProtocolPolicy: redirect-to-https
          ForwardedValues:
            QueryString: false
```

## Monitoring

### CloudWatch Integration

```python
# Example 11: CloudWatch metrics
import boto3
from datetime import datetime

cloudwatch = boto3.client('cloudwatch')

def put_metric(metric_name: str, value: float, unit: str = 'Count'):
    """Put custom metric to CloudWatch"""
    cloudwatch.put_metric_data(
        Namespace='FastAPI/Application',
        MetricData=[
            {
                'MetricName': metric_name,
                'Timestamp': datetime.utcnow(),
                'Value': value,
                'Unit': unit
            }
        ]
    )

# Usage
put_metric('RequestCount', 1)
put_metric('ResponseTime', 0.125, 'Seconds')
```

## Best Practices

### AWS Deployment Guidelines

```yaml
# Example 12: Production checklist
"""
AWS Deployment Checklist:

1. Security
   ✓ Use IAM roles, not access keys
   ✓ Enable VPC for ECS/Lambda
   ✓ Use Secrets Manager for credentials
   ✓ Enable CloudTrail logging

2. High Availability
   ✓ Deploy to multiple AZs
   ✓ Use Application Load Balancer
   ✓ Configure auto-scaling
   ✓ Set up health checks

3. Monitoring
   ✓ Enable CloudWatch logs
   ✓ Set up custom metrics
   ✓ Configure alarms
   ✓ Enable X-Ray tracing

4. Cost Optimization
   ✓ Right-size instances
   ✓ Use Spot instances where possible
   ✓ Set up billing alerts
   ✓ Review unused resources
"""
```

## Summary

| Service | Use Case | Complexity |
|---------|----------|------------|
| ECS Fargate | Containerized apps | Medium |
| Lambda | Serverless, low traffic | Low |
| Elastic Beanstalk | Simple deployment | Low |
| EKS | Kubernetes workloads | High |

## Next Steps

Continue learning about:
- [Azure Deployment](./02_azure_deployment.md) - Azure services
- [GCP Deployment](./03_gcp_deployment.md) - Google Cloud
- [Serverless Deployment](../04_serverless_deployment/01_serverless_concepts.md)
