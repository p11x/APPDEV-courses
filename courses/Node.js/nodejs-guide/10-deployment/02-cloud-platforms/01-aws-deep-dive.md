# AWS and Multi-Cloud Deployment Deep Dive

## What You'll Learn

- AWS deployment patterns (EC2, Lambda, ECS, EKS)
- Azure deployment strategies
- Google Cloud deployment patterns
- Multi-cloud architecture
- Cloud cost optimization

## AWS Lambda (Serverless)

```javascript
// src/handler.js
import serverless from 'serverless-http';
import express from 'express';

const app = express();
app.use(express.json());

app.get('/health', (req, res) => {
    res.json({ status: 'ok', timestamp: new Date().toISOString() });
});

app.get('/api/users', async (req, res) => {
    const users = await db.query('SELECT * FROM users LIMIT 100');
    res.json(users);
});

// Export for serverless
export const handler = serverless(app);

// Warm-up handler to reduce cold starts
export const warmup = async () => {
    await db.connect();
    return { statusCode: 200 };
};
```

```yaml
# serverless.yml for AWS Lambda
service: my-node-api

provider:
  name: aws
  runtime: nodejs20.x
  region: us-east-1
  memorySize: 256
  timeout: 30
  apiGateway:
    shouldStartNameWithService: true

functions:
  api:
    handler: src/handler.handler
    events:
      - httpApi:
          path: /{proxy+}
          method: ANY
    provisionedConcurrency: 3
    reservedConcurrency: 100

package:
  patterns:
    - '!node_modules/**'
    - '!test/**'
    - '!.env*'

plugins:
  - serverless-offline
  - serverless-plugin-warmup
```

## AWS ECS (Fargate)

```json
// task-definition.json
{
    "family": "node-app",
    "networkMode": "awsvpc",
    "requiresCompatibilities": ["FARGATE"],
    "cpu": "256",
    "memory": "512",
    "executionRoleArn": "arn:aws:iam::role/ecsTaskExecutionRole",
    "containerDefinitions": [
        {
            "name": "node-app",
            "image": "123456789.dkr.ecr.us-east-1.amazonaws.com/my-app:latest",
            "portMappings": [
                {
                    "containerPort": 3000,
                    "protocol": "tcp"
                }
            ],
            "environment": [
                { "name": "NODE_ENV", "value": "production" }
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
                    "awslogs-group": "/ecs/node-app",
                    "awslogs-region": "us-east-1",
                    "awslogs-stream-prefix": "ecs"
                }
            },
            "healthCheck": {
                "command": ["CMD-SHELL", "curl -f http://localhost:3000/health || exit 1"],
                "interval": 30,
                "timeout": 5,
                "retries": 3
            }
        }
    ]
}
```

## Azure App Service

```yaml
# azure-pipelines.yml
trigger:
  - main

pool:
  vmImage: 'ubuntu-latest'

steps:
  - task: NodeTool@0
    inputs:
      versionSpec: '20.x'

  - script: npm ci
    displayName: 'Install dependencies'

  - script: npm run build
    displayName: 'Build'

  - script: npm test
    displayName: 'Test'

  - task: AzureWebApp@1
    inputs:
      azureSubscription: 'Azure-Connection'
      appName: 'my-node-app'
      package: '$(Build.ArtifactStagingDirectory)'
```

```json
// Azure App Service web.config
{
    "name": "my-node-app",
    "runtime": "node|20-lts",
    "sku": "B1",
    "appSettings": {
        "NODE_ENV": "production",
        "WEBSITE_NODE_DEFAULT_VERSION": "20.x"
    }
}
```

## Google Cloud Run

```dockerfile
# Dockerfile for Cloud Run
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM node:20-alpine
WORKDIR /app
COPY --from=builder /app/dist ./dist
COPY --from=builder /app/node_modules ./node_modules
COPY package*.json ./

ENV NODE_ENV=production
ENV PORT=8080
EXPOSE 8080

# Cloud Run requires non-root user
RUN addgroup -g 1001 -S nodejs
RUN adduser -S nodejs -u 1001
USER nodejs

CMD ["node", "dist/index.js"]
```

```bash
# Deploy to Cloud Run
gcloud run deploy my-node-app \
    --source . \
    --region us-central1 \
    --platform managed \
    --allow-unauthenticated \
    --memory 512Mi \
    --cpu 1 \
    --min-instances 0 \
    --max-instances 10 \
    --set-env-vars NODE_ENV=production
```

## Multi-Cloud Architecture

```yaml
# terraform/main.tf — Multi-cloud setup
terraform {
  required_providers {
    aws = { source = "hashicorp/aws" }
    azure = { source = "hashicorp/azurerm" }
    google = { source = "hashicorp/google" }
  }
}

# Primary: AWS
module "aws_primary" {
  source = "./modules/aws"
  region = "us-east-1"
  app_name = "my-app"
  environment = "production"
}

# Secondary: GCP
module "gcp_secondary" {
  source = "./modules/gcp"
  region = "us-central1"
  app_name = "my-app"
  environment = "production"
}

# Azure: Disaster recovery
module "azure_dr" {
  source = "./modules/azure"
  region = "eastus"
  app_name = "my-app"
  environment = "production"
}
```

## Cost Optimization

```
Cloud Cost Optimization Strategies:
─────────────────────────────────────────────
AWS:
├── Use Reserved Instances for predictable workloads (up to 72% savings)
├── Use Spot Instances for batch processing (up to 90% savings)
├── Right-size instances based on CloudWatch metrics
├── Use Lambda for variable workloads
└── Enable S3 lifecycle policies

GCP:
├── Use Committed Use Discounts
├── Use Preemptible VMs for fault-tolerant workloads
├── Use Cloud Run for auto-scaling
└── Optimize Cloud SQL instance sizes

Azure:
├── Use Reserved Instances
├── Use Azure Spot VMs
├── Use Azure Functions for event-driven workloads
└── Optimize App Service plans
```

## Best Practices Checklist

- [ ] Use managed services where possible
- [ ] Implement multi-region for high availability
- [ ] Use Infrastructure as Code for all deployments
- [ ] Monitor costs with cloud cost management tools
- [ ] Use reserved/spot instances for cost savings
- [ ] Implement proper secrets management
- [ ] Use container registries for image management

## Cross-References

- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for architecture patterns
- See [Serverless](./03-serverless-deep-dive.md) for serverless patterns
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for automation
- See [IaC](../06-infrastructure-as-code/01-terraform.md) for Terraform

## Next Steps

Continue to [Serverless Deep Dive](./03-serverless-deep-dive.md).
