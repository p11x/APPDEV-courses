---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: Lambda Serverless
Purpose: Practical serverless labs including API Gateway integration and event-driven architectures
Difficulty: intermediate
Prerequisites: 01_Basic_Lambda.md, 02_Advanced_Lambda.md
RelatedFiles: 01_Basic_Lambda.md, 02_Advanced_Lambda.md
UseCase: Production serverless application deployment
CertificationExam: AWS Developer Associate
LastUpdated: 2025
---

## WHY

Hands-on Lambda labs provide practical serverless development experience.

## WHAT

### Lab: Serverless API

Build REST API with Lambda and API Gateway.

## HOW

### Module 1: Create Lambda Function

```bash
# Create function
aws lambda create-function \
    --function-name api-handler \
    --runtime nodejs18.x \
    --handler index.handler \
    --zip-file fileb://function.zip \
    --role arn:aws:iam::123456789:role/lambda-exec
```

### Module 2: Create API Gateway

```bash
# Create API
aws apigatewayv2 create-api \
    --name my-api \
    --protocol-type HTTP
```

### Module 3: Integrate

```bash
# Create integration
aws apigatewayv2 create-integration \
    --api-id api-id \
    --integration-type LAMBDA_PROXY \
    --integration-uri lambda-arn
```

## TESTING

```bash
# Invoke API
curl https://api-id.execute-api.us-east-1.amazonaws.com/prod/items
```

## CLEANUP

```bash
# Delete resources
aws lambda delete-function --function-name api-handler
aws apigatewayv2 delete-api --api-id api-id
```

## CROSS-REFERENCES

### Prerequisites

- Basic Lambda knowledge