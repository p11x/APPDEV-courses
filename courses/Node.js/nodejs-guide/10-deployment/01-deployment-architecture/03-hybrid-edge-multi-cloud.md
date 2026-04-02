# Hybrid, Edge & Multi-Cloud Deployment

## What You'll Learn

- Serverless container deployment (AWS Fargate, Azure Container Instances, Google Cloud Run)
- Hybrid strategies: combining serverless + containers + VMs
- Multi-cloud active-active and active-passive architectures
- Edge deployment with Cloudflare Workers, Vercel Edge Functions, Lambda@Edge
- Multi-cloud Terraform modules
- Edge caching and compute strategies
- Performance benchmarks across deployment patterns

## Architecture Overview

```
Hybrid Deployment Architecture:
────────────────────────────────────────────────────────────────────────
                         ┌──────────────┐
       Clients ─────────▶│   CDN Edge   │◀── Static assets, edge compute
                         └──────┬───────┘
                                │
                         ┌──────▼───────┐
                         │ Cloudflare / │◀── Edge workers, A/B testing
                         │   Vercel CDN │
                         └──────┬───────┘
                                │
              ┌─────────────────┼─────────────────┐
              ▼                 ▼                 ▼
     ┌────────────────┐ ┌──────────────┐ ┌────────────────┐
     │  Cloud Run     │ │  Fargate     │ │  Kubernetes    │
     │  (Auto-scale   │ │  (Burst      │ │  (Steady       │
     │   to zero)     │ │   traffic)   │ │   state)       │
     └────────┬───────┘ └──────┬───────┘ └────────┬───────┘
              │                │                  │
              └────────────────┼──────────────────┘
                               ▼
                      ┌─────────────────┐
                      │   Shared Data   │
                      │  (Multi-region  │
                      │   databases)    │
                      └─────────────────┘
────────────────────────────────────────────────────────────────────────

Multi-Cloud Active-Active:
────────────────────────────────────────────────────────────────────────
     ┌─────────────────────────────────────────────────────────┐
     │                  Global DNS (Route53 / CloudFlare)       │
     └──────────────┬──────────────────────┬───────────────────┘
                    │                      │
              ┌─────▼─────┐          ┌─────▼─────┐
              │   AWS     │          │  GCP /    │
              │   Region  │◀────────▶│  Azure    │
              │  (Primary)│  Sync    │ (Secondary│
              └───────────┘          └───────────┘
────────────────────────────────────────────────────────────────────────
```

---

## Part 1: Serverless Container Deployment

### 1.1 Google Cloud Run

Cloud Run provides fully managed containers that scale to zero and charge only for actual request processing time.

```yaml
# cloudrun/service.yaml — Cloud Run service definition
apiVersion: serving.knative.dev/v1
kind: Service
metadata:
  name: node-api
  annotations:
    run.googleapis.com/launch-stage: GA
spec:
  template:
    metadata:
      annotations:
        autoscaling.knative.dev/minScale: "1"      # Always 1 instance (no cold starts)
        autoscaling.knative.dev/maxScale: "100"     # Scale up to 100 instances
        run.googleapis.com/cpu-throttling: "false"   # Always-allocated CPU
        run.googleapis.com/startup-cpu-boost: "true" # Extra CPU during startup
    spec:
      containerConcurrency: 80                       # Max requests per instance
      timeoutSeconds: 300
      containers:
        - image: us-docker.pkg.dev/my-project/node-api:latest
          ports:
            - name: http1
              containerPort: 3000
          env:
            - name: NODE_ENV
              value: production
            - name: DATABASE_URL
              valueFrom:
                secretKeyRef:
                  name: db-secrets
                  key: url
          resources:
            limits:
              cpu: "2"
              memory: 1Gi
          livenessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 10
            periodSeconds: 30
      serviceAccountName: node-api-sa@my-project.iam.gserviceaccount.com
```

```dockerfile
# Dockerfile for Cloud Run (multi-stage, optimized)
FROM node:20-slim AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci --omit=dev
COPY . .
RUN npm run build

FROM node:20-slim
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
WORKDIR /app
COPY --from=builder --chown=appuser:appgroup /app/dist ./dist
COPY --from=builder --chown=appuser:appgroup /app/node_modules ./node_modules
COPY --from=builder --chown=appuser:appgroup /app/package.json ./
USER appuser
ENV NODE_ENV=production
ENV PORT=3000
EXPOSE 3000
CMD ["node", "dist/server.js"]
```

```bash
# Deploy to Cloud Run
gcloud run deploy node-api \
  --source . \
  --region us-central1 \
  --platform managed \
  --allow-unauthenticated \
  --memory 512Mi \
  --cpu 1 \
  --min-instances 1 \
  --max-instances 100 \
  --concurrency 80 \
  --set-env-vars "NODE_ENV=production" \
  --set-secrets "DATABASE_URL=db-secret:latest" \
  --service-account node-api-sa@my-project.iam.gserviceaccount.com
```

### 1.2 AWS Fargate (ECS)

```json
// task-definition.json — ECS Fargate task definition
{
    "family": "node-api",
    "networkMode": "awsvpc",
    "requiresCompatibilities": ["FARGATE"],
    "cpu": "512",
    "memory": "1024",
    "executionRoleArn": "arn:aws:iam::123456789:role/ecsTaskExecutionRole",
    "taskRoleArn": "arn:aws:iam::123456789:role/ecsTaskRole",
    "containerDefinitions": [
        {
            "name": "node-api",
            "image": "123456789.dkr.ecr.us-east-1.amazonaws.com/node-api:latest",
            "portMappings": [
                {
                    "containerPort": 3000,
                    "protocol": "tcp",
                    "appProtocol": "http"
                }
            ],
            "environment": [
                { "name": "NODE_ENV", "value": "production" },
                { "name": "PORT", "value": "3000" }
            ],
            "secrets": [
                {
                    "name": "DATABASE_URL",
                    "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:prod/db-url"
                },
                {
                    "name": "JWT_SECRET",
                    "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:prod/jwt-secret"
                }
            ],
            "healthCheck": {
                "command": ["CMD-SHELL", "curl -f http://localhost:3000/health || exit 1"],
                "interval": 30,
                "timeout": 5,
                "retries": 3,
                "startPeriod": 60
            },
            "logConfiguration": {
                "logDriver": "awslogs",
                "options": {
                    "awslogs-group": "/ecs/node-api",
                    "awslogs-region": "us-east-1",
                    "awslogs-stream-prefix": "api"
                }
            },
            "linuxParameters": {
                "initProcessEnabled": true
            }
        }
    ]
}
```

```yaml
# cloudformation/fargate-service.yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: Fargate service with ALB and auto-scaling

Parameters:
  Environment:
    Type: String
    Default: production
  ImageUri:
    Type: String

Resources:
  ECSCluster:
    Type: AWS::ECS::Cluster
    Properties:
      ClusterName: !Sub "${Environment}-cluster"
      CapacityProviders:
        - FARGATE
        - FARGATE_SPOT
      DefaultCapacityProviderStrategy:
        - CapacityProvider: FARGATE
          Weight: 1

  ServiceSecurityGroup:
    Type: AWS::EC2::SecurityGroup
    Properties:
      GroupDescription: Allow HTTP to node-api
      VpcId: !ImportValue VPCId
      SecurityGroupIngress:
        - IpProtocol: tcp
          FromPort: 3000
          ToPort: 3000
          SourceSecurityGroupId: !ImportValue ALBSecurityGroup

  ALBTargetGroup:
    Type: AWS::ElasticLoadBalancingV2::TargetGroup
    Properties:
      Name: !Sub "${Environment}-node-api-tg"
      Port: 3000
      Protocol: HTTP
      TargetType: ip
      VpcId: !ImportValue VPCId
      HealthCheckPath: /health
      HealthCheckIntervalSeconds: 30
      HealthyThresholdCount: 2
      UnhealthyThresholdCount: 5
      Matcher:
        HttpCode: "200"

  ECSService:
    Type: AWS::ECS::Service
    DependsOn: ALBListenerRule
    Properties:
      ServiceName: !Sub "${Environment}-node-api"
      Cluster: !Ref ECSCluster
      TaskDefinition: !Ref TaskDefinition
      DesiredCount: 3
      LaunchType: FARGATE
      DeploymentConfiguration:
        MaximumPercent: 200
        MinimumHealthyPercent: 100
        DeploymentCircuitBreaker:
          Enable: true
          Rollback: true
      NetworkConfiguration:
        AwsvpcConfiguration:
          AssignPublicIp: DISABLED
          Subnets:
            - !ImportValue PrivateSubnet1
            - !ImportValue PrivateSubnet2
          SecurityGroups:
            - !Ref ServiceSecurityGroup
      LoadBalancers:
        - ContainerName: node-api
          ContainerPort: 3000
          TargetGroupArn: !Ref ALBTargetGroup

  ScalableTarget:
    Type: AWS::ApplicationAutoScaling::ScalableTarget
    Properties:
      MaxCapacity: 20
      MinCapacity: 2
      ResourceId: !Sub "service/${ECSCluster}/${ECSService.Name}"
      ScalableDimension: ecs:service:DesiredCount
      ServiceNamespace: ecs

  ScalingPolicy:
    Type: AWS::ApplicationAutoScaling::ScalingPolicy
    Properties:
      PolicyName: cpu-tracking
      PolicyType: TargetTrackingScaling
      ScalingTargetRef:
        ScalableTargetId: !Ref ScalableTarget
      TargetTrackingScalingPolicyConfiguration:
        PredefinedMetricSpecification:
          PredefinedMetricType: ECSServiceAverageCPUUtilization
        TargetValue: 70
        ScaleInCooldown: 300
        ScaleOutCooldown: 60

  TaskDefinition:
    Type: AWS::ECS::TaskDefinition
    Properties:
      Family: node-api
      Cpu: "512"
      Memory: "1024"
      NetworkMode: awsvpc
      RequiresCompatibilities: [FARGATE]
      ExecutionRoleArn: !ImportValue ECSExecutionRole
      TaskRoleArn: !ImportValue ECSTaskRole
      ContainerDefinitions:
        - Name: node-api
          Image: !Ref ImageUri
          PortMappings:
            - ContainerPort: 3000
          LogConfiguration:
            LogDriver: awslogs
            Options:
              awslogs-group: /ecs/node-api
              awslogs-region: !Ref AWS::Region
              awslogs-stream-prefix: api
```

### 1.3 Azure Container Instances

```yaml
# azure/container-group.yaml
apiVersion: 2021-09-01
location: eastus
name: node-api-group
properties:
  containers:
    - name: node-api
      properties:
        image: myregistry.azurecr.io/node-api:latest
        resources:
          requests:
            cpu: 1
            memoryInGb: 1.5
        ports:
          - port: 3000
        environmentVariables:
          - name: NODE_ENV
            value: production
          - name: DATABASE_URL
            secureValue: "Server=tcp:..."
        livenessProbe:
          httpGet:
            path: /health
            port: 3000
          periodSeconds: 30
          initialDelaySeconds: 10
  osType: Linux
  ipAddress:
    type: Public
    ports:
      - port: 3000
        protocol: TCP
  imageRegistryCredentials:
    - server: myregistry.azurecr.io
      username: myregistry
      password: <password>
  restartPolicy: Always
```

---

## Part 2: Hybrid Deployment Strategies

### When to Use Each Compute Model

```
Decision Matrix:
────────────────────────────────────────────────────────────────────────
Compute Type       Cold Start   Scale      Cost Model      Best For
────────────────────────────────────────────────────────────────────────
VMs (EC2/GCE)      None         Manual     Per hour        Steady-state, legacy
Containers (ECS)   Seconds      Auto       Per vCPU-hour   Microservices
K8s (EKS/GKE)      Seconds      Auto       Per node+pod    Multi-team enterprise
Serverless (Lambda) 100-500ms   Auto       Per invocation  Event-driven, bursty
Cloud Run            1-3s       Auto→Zero   Per request     APIs, scale-to-zero
Fargate             Seconds      Auto       Per vCPU-hour   Containers w/o K8s
Edge Workers        <1ms        Global     Per request     Latency-critical
────────────────────────────────────────────────────────────────────────
```

### Hybrid Deployment Architecture

```yaml
# docker-compose.hybrid.yml
# Local development simulating hybrid deployment
version: '3.8'

services:
  # Core API: always-on container (ECS/K8s production)
  api:
    build: ./api
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=production
      - ORDER_FUNCTION_URL=http://order-handler:3001
      - WEBHOOK_FUNCTION_URL=http://webhook-handler:3002
    deploy:
      resources:
        limits:
          cpus: '1'
          memory: 512M

  # Event handler: scale-to-zero (Lambda/Cloud Run production)
  order-handler:
    build: ./functions/order-handler
    ports:
      - "3001:3001"
    environment:
      - DATABASE_URL=postgresql://app:secret@db:5432/orders
    deploy:
      replicas: 1
      resources:
        limits:
          cpus: '0.5'
          memory: 256M

  # Webhook processor: scale-to-zero (Lambda/Cloud Run production)
  webhook-handler:
    build: ./functions/webhook-handler
    ports:
      - "3002:3002"
    deploy:
      replicas: 1
      resources:
        limits:
          cpus: '0.25'
          memory: 128M

  # Background worker: long-running container (ECS production)
  worker:
    build: ./worker
    environment:
      - REDIS_URL=redis://redis:6379
    deploy:
      replicas: 2
      resources:
        limits:
          cpus: '0.5'
          memory: 256M

  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: orders
      POSTGRES_USER: app
      POSTGRES_PASSWORD: secret
    volumes:
      - db-data:/var/lib/postgresql/data

  redis:
    image: redis:7-alpine

volumes:
  db-data:
```

```javascript
// api/src/dispatcher.js — Hybrid request dispatcher
// Routes requests to the appropriate compute model based on workload type
import axios from 'axios';

const ENDPOINTS = {
    // Always-on container endpoints
    synchronous: {
        url: 'http://api:3000',
        timeout: 5000,
    },
    // Scale-to-zero function endpoints
    async_order: {
        url: process.env.ORDER_FUNCTION_URL || 'http://order-handler:3001',
        timeout: 30000,
    },
    async_webhook: {
        url: process.env.WEBHOOK_FUNCTION_URL || 'http://webhook-handler:3002',
        timeout: 10000,
    },
};

export async function dispatchRequest(type, payload) {
    const endpoint = ENDPOINTS[type];
    if (!endpoint) throw new Error(`Unknown dispatch type: ${type}`);

    const start = Date.now();
    try {
        const response = await axios.post(`${endpoint.url}/process`, payload, {
            timeout: endpoint.timeout,
            headers: {
                'X-Request-Type': type,
                'X-Dispatch-Timestamp': new Date().toISOString(),
            },
        });

        console.log(`[${type}] ${response.status} ${Date.now() - start}ms`);
        return response.data;
    } catch (error) {
        console.error(`[${type}] FAILED after ${Date.now() - start}ms:`, error.message);
        throw error;
    }
}

// Usage in routes:
// app.post('/api/search', async (req, res) => {
//     const result = await dispatchRequest('synchronous', req.body);  // Always-on
//     res.json(result);
// });
// app.post('/api/orders', async (req, res) => {
//     const result = await dispatchRequest('async_order', req.body);  // Scale-to-zero
//     res.json(result);
// });
// app.post('/webhooks/github', async (req, res) => {
//     const result = await dispatchRequest('async_webhook', req.body); // Scale-to-zero
//     res.json(result);
// });
```

---

## Part 3: Multi-Cloud Deployment Architectures

### 3.1 Active-Active Multi-Cloud

```
Active-Active Multi-Cloud:
────────────────────────────────────────────────────────────────────────
                    ┌──────────────────┐
                    │   Global DNS     │
                    │ (Route53/Latency)│
                    └────────┬─────────┘
                    ┌────────┴─────────┐
                    │                  │
              ┌─────▼─────┐    ┌──────▼────┐
              │    AWS     │    │   GCP     │
              │  us-east-1 │    │ us-central│
              └─────┬─────┘    └──────┬────┘
                    │                 │
              ┌─────▼─────┐    ┌──────▼────┐
              │  RDS Post  │◀──▶│ Cloud SQL │
              │  (Primary) │Sync│ (Replica) │
              └───────────┘    └───────────┘
────────────────────────────────────────────────────────────────────────
```

```javascript
// multi-cloud/src/cloud-router.js — Intelligent multi-cloud request routing
import { DynamoDBClient } from '@aws-sdk/client-dynamodb';
import { Firestore } from '@google-cloud/firestore';

class MultiCloudDataLayer {
    constructor() {
        this.primaryCloud = process.env.PRIMARY_CLOUD || 'aws';
        this.enableDualWrite = process.env.DUAL_WRITE === 'true';

        // AWS clients
        this.dynamoClient = new DynamoDBClient({
            region: process.env.AWS_REGION || 'us-east-1',
        });

        // GCP clients
        this.firestore = new Firestore({
            projectId: process.env.GCP_PROJECT,
        });
    }

    async read(key, options = {}) {
        const preferredCloud = options.cloud || this.primaryCloud;

        try {
            if (preferredCloud === 'aws') {
                return await this.readFromDynamo(key);
            } else {
                return await this.readFromFirestore(key);
            }
        } catch (error) {
            // Failover to other cloud
            console.warn(`Primary cloud read failed (${preferredCloud}), failing over:`, error.message);
            if (preferredCloud === 'aws') {
                return await this.readFromFirestore(key);
            } else {
                return await this.readFromDynamo(key);
            }
        }
    }

    async write(key, data) {
        const results = [];

        if (this.enableDualWrite) {
            // Write to both clouds concurrently
            const [awsResult, gcpResult] = await Promise.allSettled([
                this.writeToDynamo(key, data),
                this.writeToFirestore(key, data),
            ]);

            if (awsResult.status === 'rejected' && gcpResult.status === 'rejected') {
                throw new Error('Both cloud writes failed');
            }

            return {
                aws: awsResult.status,
                gcp: gcpResult.status,
            };
        } else {
            // Write to primary only
            if (this.primaryCloud === 'aws') {
                return await this.writeToDynamo(key, data);
            } else {
                return await this.writeToFirestore(key, data);
            }
        }
    }

    async readFromDynamo(key) {
        // DynamoDB read implementation
        return { source: 'aws', key };
    }

    async readFromFirestore(key) {
        const doc = await this.firestore.collection('data').doc(key).get();
        return { source: 'gcp', key, data: doc.data() };
    }

    async writeToDynamo(key, data) {
        // DynamoDB write implementation
        return { source: 'aws', key, written: true };
    }

    async writeToFirestore(key, data) {
        await this.firestore.collection('data').doc(key).set(data);
        return { source: 'gcp', key, written: true };
    }
}

// Health-aware cloud selection
class CloudHealthMonitor {
    constructor() {
        this.health = {
            aws: { healthy: true, latency: 0, lastCheck: null },
            gcp: { healthy: true, latency: 0, lastCheck: null },
        };
    }

    async checkHealth(cloud, healthEndpoint) {
        const start = Date.now();
        try {
            await fetch(healthEndpoint, { timeout: 3000 });
            this.health[cloud] = {
                healthy: true,
                latency: Date.now() - start,
                lastCheck: new Date(),
            };
        } catch {
            this.health[cloud] = {
                ...this.health[cloud],
                healthy: false,
                lastCheck: new Date(),
            };
        }
    }

    getPreferredCloud() {
        const { aws, gcp } = this.health;
        if (aws.healthy && !gcp.healthy) return 'aws';
        if (!aws.healthy && gcp.healthy) return 'gcp';
        if (aws.healthy && gcp.healthy) {
            return aws.latency <= gcp.latency ? 'aws' : 'gcp';
        }
        return 'aws'; // fallback
    }

    startMonitoring() {
        setInterval(() => {
            this.checkHealth('aws', process.env.AWS_HEALTH_URL);
            this.checkHealth('gcp', process.env.GCP_HEALTH_URL);
        }, 10000);
    }
}

export { MultiCloudDataLayer, CloudHealthMonitor };
```

### 3.2 Active-Passive with Failover

```hcl
# terraform/multi-cloud-failover.tf
# Route53 health-checked failover between AWS (active) and GCP (passive)

resource "aws_route53_health_check" "primary" {
  fqdn              = "api-primary.example.com"
  port               = 443
  type               = "HTTPS"
  resource_path      = "/health"
  failure_threshold  = 3
  request_interval   = 10

  tags = {
    Name = "primary-health-check"
  }
}

resource "aws_route53_record" "primary" {
  zone_id = aws_route53_zone.main.zone_id
  name    = "api.example.com"
  type    = "A"

  alias {
    name                   = aws_lb.main.dns_name
    zone_id                = aws_lb.main.zone_id
    evaluate_target_health = true
  }

  failover_routing_policy {
    type = "PRIMARY"
  }

  set_identifier  = "primary"
  health_check_id = aws_route53_health_check.primary.id
}

resource "aws_route53_record" "secondary" {
  zone_id = aws_route53_zone.main.zone_id
  name    = "api.example.com"
  type    = "CNAME"
  ttl     = 60

  records = ["api-gcp.example.com"]

  failover_routing_policy {
    type = "SECONDARY"
  }

  set_identifier = "secondary"
}

# Cloud Run in GCP as passive standby
resource "google_cloud_run_v2_service" "passive" {
  name     = "node-api-passive"
  location = "us-central1"

  template {
    containers {
      image = "gcr.io/${var.gcp_project}/node-api:latest"

      resources {
        limits = {
          cpu    = "1"
          memory = "512Mi"
        }
      }

      env {
        name  = "NODE_ENV"
        value = "production"
      }

      env {
        name  = "ROLE"
        value = "passive"  # Read-only mode
      }
    }

    scaling {
      min_instance_count = 1   # Keep warm for fast failover
      max_instance_count = 10
    }
  }

  traffic {
    type    = "TRAFFIC_TARGET_ALLOCATION_TYPE_LATEST"
    percent = 100
  }
}
```

---

## Part 4: Edge Deployment Patterns

### 4.1 Cloudflare Workers

Cloudflare Workers run on Cloudflare's edge network across 300+ data centers globally, providing sub-millisecond cold starts.

```javascript
// cloudflare-worker/src/index.js — Full edge application with KV, D1, and Durable Objects
export default {
    async fetch(request, env, ctx) {
        const url = new URL(request.url);

        // ─── Routing ───────────────────────────────────────
        if (url.pathname === '/api/config') {
            return handleConfig(request, env);
        }
        if (url.pathname === '/api/cache') {
            return handleEdgeCache(request, env, ctx);
        }
        if (url.pathname === '/api/rate-limit') {
            return handleRateLimit(request, env);
        }
        if (url.pathname.startsWith('/api/geo')) {
            return handleGeoRouting(request, env);
        }
        if (url.pathname === '/api/ab-test') {
            return handleABTest(request, env, ctx);
        }

        // Default: proxy to origin with edge caching
        return handleProxyWithCache(request, env, ctx);
    },
};

// ─── Edge Configuration via KV ─────────────────────────────
async function handleConfig(request, env) {
    const config = await env.CONFIG_KV.get('app-config', 'json');
    return new Response(JSON.stringify(config), {
        headers: { 'Content-Type': 'application/json', 'Cache-Control': 's-maxage=60' },
    });
}

// ─── Edge Cache with Cache API ─────────────────────────────
async function handleEdgeCache(request, env, ctx) {
    const cacheUrl = new URL(request.url);
    const cacheKey = new Request(cacheUrl.toString(), request);
    const cache = caches.default;

    // Check edge cache
    let response = await cache.match(cacheKey);
    if (response) {
        response = new Response(response.body, response);
        response.headers.set('X-Cache', 'HIT');
        return response;
    }

    // Fetch from origin
    const originResponse = await fetch('https://api-origin.example.com/data', {
        headers: { 'Authorization': `Bearer ${env.API_TOKEN}` },
    });

    // Cache the response at the edge
    response = new Response(originResponse.body, originResponse);
    response.headers.set('X-Cache', 'MISS');
    response.headers.set('Cache-Control', 's-maxage=300, stale-while-revalidate=60');

    ctx.waitUntil(cache.put(cacheKey, response.clone()));
    return response;
}

// ─── Rate Limiting with Durable Objects ────────────────────
async function handleRateLimit(request, env) {
    const clientIP = request.headers.get('CF-Connecting-IP');
    const rateLimiterStub = env.RATE_LIMITER.get(env.RATE_LIMITER.idFromName(clientIP));

    const result = await rateLimiterStub.fetch(request, {
        method: 'POST',
        body: JSON.stringify({ limit: 100, window: 60 }),
    });

    const { allowed, remaining, resetAt } = await result.json();

    if (!allowed) {
        return new Response(JSON.stringify({ error: 'Rate limit exceeded' }), {
            status: 429,
            headers: {
                'Content-Type': 'application/json',
                'X-RateLimit-Remaining': '0',
                'X-RateLimit-Reset': resetAt.toString(),
            },
        });
    }

    // Forward to origin
    const originResponse = await fetch('https://api-origin.example.com/data');
    const response = new Response(originResponse.body, originResponse);
    response.headers.set('X-RateLimit-Remaining', remaining.toString());
    return response;
}

// ─── Geo-based Routing ─────────────────────────────────────
async function handleGeoRouting(request, env) {
    const country = request.headers.get('CF-IPCountry');
    const region = env.GEO_ROUTES[country] || 'us-east-1';

    const regionEndpoints = {
        'us-east-1': 'https://api-us.example.com',
        'eu-west-1': 'https://api-eu.example.com',
        'ap-southeast-1': 'https://api-ap.example.com',
    };

    const response = await fetch(`${regionEndpoints[region]}${new URL(request.url).pathname}`, {
        headers: { 'X-Original-Country': country },
    });

    return new Response(response.body, {
        ...response,
        headers: {
            ...Object.fromEntries(response.headers),
            'X-Routed-Region': region,
            'X-Client-Country': country,
        },
    });
}

// ─── A/B Testing at the Edge ───────────────────────────────
async function handleABTest(request, env, ctx) {
    const userId = request.headers.get('CF-Connecting-IP');
    const hash = await crypto.subtle.digest('SHA-256', new TextEncoder().encode(userId));
    const bucket = new Uint8Array(hash)[0] % 100;

    const experiment = bucket < 50 ? 'control' : 'variant-b';

    const originUrl = new URL(request.url);
    originUrl.hostname = 'api-origin.example.com';
    originUrl.searchParams.set('experiment', experiment);

    const response = await fetch(originUrl.toString());
    const modifiedResponse = new Response(response.body, response);
    modifiedResponse.headers.set('X-Experiment', experiment);
    modifiedResponse.headers.set('X-Bucket', bucket.toString());

    return modifiedResponse;
}

// ─── Proxy with origin fallback ────────────────────────────
async function handleProxyCache(request, env, ctx) {
    const originUrl = new URL(request.url);
    originUrl.hostname = 'api-origin.example.com';

    const response = await fetch(originUrl.toString(), {
        method: request.method,
        headers: request.headers,
        body: request.body,
    });

    return new Response(response.body, response);
}
```

```toml
# cloudflare-worker/wrangler.toml
name = "edge-api"
main = "src/index.js"
compatibility_date = "2024-01-01"

[vars]
GEO_ROUTES = { "US" = "us-east-1", "DE" = "eu-west-1", "SG" = "ap-southeast-1" }

[[kv_namespaces]]
binding = "CONFIG_KV"
id = "abc123..."

[[durable_objects.bindings]]
name = "RATE_LIMITER"
class_name = "RateLimiter"

[[d1_databases]]
binding = "DB"
database_name = "edge-db"
database_id = "def456..."
```

### 4.2 Vercel Edge Functions

```typescript
// api/edge/geolocation.ts — Vercel Edge Function
import { NextRequest, NextResponse } from 'next/server';

export const config = {
    runtime: 'edge',
};

export default async function handler(request: NextRequest) {
    const geo = request.geo;
    const country = geo?.country || 'US';
    const city = geo?.city || 'Unknown';
    const lat = geo?.latitude;
    const lng = geo?.longitude;

    // Edge-computed nearest region
    const nearestRegion = findNearestRegion(lat, lng);

    // Personalized response without origin round-trip
    return NextResponse.json({
        geo: { country, city, lat, lng },
        nearestRegion,
        timestamp: new Date().toISOString(),
        computeLocation: 'edge',
        cf: {
            colo: request.headers.get('cf-ray')?.split('-')[1],
            country: request.headers.get('cf-ipcountry'),
        },
    });
}

function findNearestRegion(lat?: number, lng?: number) {
    if (!lat || !lng) return 'us-east-1';
    const regions = [
        { name: 'us-east-1', lat: 39.0, lng: -77.5 },
        { name: 'eu-west-1', lat: 53.3, lng: -6.3 },
        { name: 'ap-southeast-1', lat: 1.3, lng: 103.8 },
    ];
    let nearest = regions[0];
    let minDist = Infinity;
    for (const r of regions) {
        const dist = Math.hypot(r.lat - lat, r.lng - lng);
        if (dist < minDist) { minDist = dist; nearest = r; }
    }
    return nearest.name;
}
```

```typescript
// api/edge/auth.ts — Edge authentication middleware for Vercel
import { NextRequest, NextResponse } from 'next/server';
import { jwtVerify, createRemoteJWKSet } from 'jose';

const JWKS = createRemoteJWKSet(
    new URL('https://auth.example.com/.well-known/jwks.json')
);

export const config = { runtime: 'edge' };

export default async function handler(request: NextRequest) {
    const token = request.headers.get('authorization')?.replace('Bearer ', '');

    if (!token) {
        return NextResponse.json({ error: 'Unauthorized' }, { status: 401 });
    }

    try {
        const { payload } = await jwtVerify(token, JWKS, {
            issuer: 'https://auth.example.com',
            audience: 'api.example.com',
        });

        // Pass user info to origin via headers
        const response = await fetch(
            new URL(request.url).pathname,
            {
                headers: {
                    ...Object.fromEntries(request.headers),
                    'X-User-ID': payload.sub as string,
                    'X-User-Roles': JSON.stringify(payload.roles || []),
                },
            }
        );

        return new Response(response.body, response);
    } catch (error) {
        return NextResponse.json({ error: 'Invalid token' }, { status: 401 });
    }
}
```

### 4.3 AWS Lambda@Edge / CloudFront Functions

```javascript
// cloudfront-function/viewer-request.js
// CloudFront Function (not Lambda@Edge) — runs on every viewer request
// Lightweight, sub-ms execution, ~2ms max CPU time

function handler(event) {
    var request = event.request;
    var uri = request.uri;
    var headers = request.headers;
    var cookies = request.cookies;

    // ─── A/B Testing at the Edge ───────────────────────────
    var abCookie = cookies['ab-variant'];
    if (!abCookie) {
        // Assign variant based on deterministic hash
        var ip = headers['cloudfront-viewer-address']
            ? headers['cloudfront-viewer-address'].value.split(':')[0]
            : '0.0.0.0';
        var hash = 0;
        for (var i = 0; i < ip.length; i++) {
            hash = ((hash << 5) - hash) + ip.charCodeAt(i);
            hash = hash & hash;
        }
        var variant = (Math.abs(hash) % 2 === 0) ? 'a' : 'b';
        cookies['ab-variant'] = { value: variant };
    }

    // Route to variant-specific origin
    if (cookies['ab-variant'] && cookies['ab-variant'].value === 'b') {
        request.origin = {
            custom: {
                domainName: 'variant-b-origin.example.com',
                port: 443,
                protocol: 'https',
                path: '',
                sslProtocols: ['TLSv1.2'],
                readTimeout: 5,
                keepaliveTimeout: 5,
                customHeaders: {},
            },
        };
    }

    // ─── URL Rewrites for SPA ─────────────────────────────
    // Serve index.html for SPA routes, static assets directly
    if (uri.startsWith('/api/')) {
        // API requests pass through to origin
        return request;
    }

    if (uri.includes('.')) {
        // Static asset — add long cache header
        headers['cache-control'] = { value: 'public, max-age=31536000, immutable' };
        return request;
    }

    // SPA route — rewrite to index.html
    request.uri = '/index.html';
    return request;
}
```

```javascript
// lambda-edge/origin-response.js
// Lambda@Edge: runs at CloudFront edge location on origin response
// Max 5s execution, 128MB memory

'use strict';

exports.handler = async (event) => {
    const response = event.Records[0].cf.response;
    const request = event.Records[0].cf.request;
    const uri = request.uri;

    // ─── Security headers ─────────────────────────────────
    response.headers['strict-transport-security'] = [{
        key: 'Strict-Transport-Security',
        value: 'max-age=63072000; includeSubDomains; preload',
    }];
    response.headers['x-content-type-options'] = [{
        key: 'X-Content-Type-Options',
        value: 'nosniff',
    }];
    response.headers['x-frame-options'] = [{
        key: 'X-Frame-Options',
        value: 'DENY',
    }];
    response.headers['content-security-policy'] = [{
        key: 'Content-Security-Policy',
        value: "default-src 'self'; script-src 'self'",
    }];

    // ─── Cache control by content type ────────────────────
    if (uri.match(/\.(js|css|woff2?|png|jpg|svg|ico)$/)) {
        response.headers['cache-control'] = [{
            key: 'Cache-Control',
            value: 'public, max-age=31536000, immutable',
        }];
    } else if (uri.startsWith('/api/')) {
        response.headers['cache-control'] = [{
            key: 'Cache-Control',
            value: 'private, no-cache, no-store, must-revalidate',
        }];
    } else {
        response.headers['cache-control'] = [{
            key: 'Cache-Control',
            value: 'public, max-age=0, s-maxage=300, stale-while-revalidate=86400',
        }];
    }

    // ─── Add server timing header ─────────────────────────
    const originTime = request.headers['x-origin-time']
        ? request.headers['x-origin-time'][0].value
        : '0';
    response.headers['server-timing'] = [{
        key: 'Server-Timing',
        value: `origin;dur=${originTime}, edge;dur=1`,
    }];

    return response;
};
```

---

## Part 5: Multi-Cloud Terraform Modules

```hcl
# terraform/modules/multi-cloud-service/main.tf
# Reusable module that deploys a service across AWS + GCP

variable "service_name" {
  type = string
}

variable "image" {
  type = string
}

variable "port" {
  type    = number
  default = 3000
}

variable "min_instances" {
  type    = number
  default = 1
}

variable "max_instances" {
  type    = number
  default = 20
}

variable "cpu" {
  type    = string
  default = "512"
}

variable "memory" {
  type    = string
  default = "1024"
}

variable "environment_variables" {
  type    = map(string)
  default = {}
}

variable "health_check_path" {
  type    = string
  default = "/health"
}

# ─── AWS: ECS on Fargate ───────────────────────────────────
resource "aws_ecs_task_definition" "service" {
  family                   = var.service_name
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  cpu                      = var.cpu
  memory                   = var.memory
  execution_role_arn       = aws_iam_role.ecs_execution.arn
  task_role_arn            = aws_iam_role.ecs_task.arn

  container_definitions = jsonencode([{
    name      = var.service_name
    image     = var.image
    essential = true
    portMappings = [{
      containerPort = var.port
      protocol      = "tcp"
    }]
    environment = [
      for k, v in var.environment_variables : { name = k, value = v }
    ]
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group"         = "/ecs/${var.service_name}"
        "awslogs-region"        = data.aws_region.current.name
        "awslogs-stream-prefix" = "ecs"
      }
    }
    healthCheck = {
      command     = ["CMD-SHELL", "curl -f http://localhost:${var.port}${var.health_check_path} || exit 1"]
      interval    = 30
      timeout     = 5
      retries     = 3
      startPeriod = 60
    }
  }])
}

resource "aws_ecs_service" "service" {
  name            = var.service_name
  cluster         = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.service.arn
  desired_count   = var.min_instances
  launch_type     = "FARGATE"

  deployment_configuration {
    maximum_percent         = 200
    minimum_healthy_percent = 100
  }

  network_configuration {
    subnets          = var.private_subnet_ids
    security_groups  = [aws_security_group.service.id]
    assign_public_ip = false
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.service.arn
    container_name   = var.service_name
    container_port   = var.port
  }
}

resource "aws_appautoscaling_target" "service" {
  max_capacity       = var.max_instances
  min_capacity       = var.min_instances
  resource_id        = "service/${aws_ecs_cluster.main.name}/${aws_ecs_service.service.name}"
  scalable_dimension = "ecs:service:DesiredCount"
  service_namespace  = "ecs"
}

resource "aws_appautoscaling_policy" "cpu" {
  name               = "${var.service_name}-cpu-scaling"
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.service.resource_id
  scalable_dimension = aws_appautoscaling_target.service.scalable_dimension
  service_namespace  = aws_appautoscaling_target.service.service_namespace

  target_tracking_scaling_policy_configuration {
    predefined_metric_specification {
      predefined_metric_type = "ECSServiceAverageCPUUtilization"
    }
    target_value       = 70
    scale_in_cooldown  = 300
    scale_out_cooldown = 60
  }
}

# ─── GCP: Cloud Run (passive/active standby) ───────────────
resource "google_cloud_run_v2_service" "service" {
  name     = var.service_name
  location = var.gcp_region

  template {
    containers {
      image = var.image

      ports {
        container_port = var.port
      }

      resources {
        limits = {
          cpu    = "1"
          memory = "${tonumber(var.memory) / 1024}Gi"
        }
      }

      dynamic "env" {
        for_each = var.environment_variables
        content {
          name  = env.key
          value = env.value
        }
      }

      startup_probe {
        http_get {
          path = var.health_check_path
          port = var.port
        }
        initial_delay_seconds = 10
        period_seconds        = 10
        failure_threshold     = 3
      }

      liveness_probe {
        http_get {
          path = var.health_check_path
          port = var.port
        }
        period_seconds = 30
      }
    }

    scaling {
      min_instance_count = var.min_instances
      max_instance_count = var.max_instances
    }
  }

  traffic {
    type    = "TRAFFIC_TARGET_ALLOCATION_TYPE_LATEST"
    percent = 100
  }
}

resource "google_cloud_run_v2_service_iam_member" "public" {
  name     = google_cloud_run_v2_service.service.name
  location = google_cloud_run_v2_service.service.location
  role     = "roles/run.invoker"
  member   = "allUsers"
}

# ─── Outputs ───────────────────────────────────────────────
output "aws_service_url" {
  value = "https://${aws_lb.main.dns_name}"
}

output "gcp_service_url" {
  value = google_cloud_run_v2_service.service.uri
}
```

---

## Part 6: Performance Benchmarks

```
Performance Comparison (Node.js API, p99 latency):
────────────────────────────────────────────────────────────────────────
Deployment           Cold Start    Warm p50    Warm p99    Cost/mo
────────────────────────────────────────────────────────────────────────
VM (t3.medium)       N/A           12ms        45ms        $30
ECS Fargate          3-8s          15ms        55ms        $35
EKS Pod              2-5s          14ms        50ms        $80
Lambda               200-800ms     18ms        90ms        $20*
Lambda (provisioned) N/A           16ms        60ms        $45
Cloud Run            1-4s          16ms        65ms        $15*
Cloudflare Worker    <1ms          2ms         8ms         $5**
Vercel Edge          <1ms          3ms         10ms        $20**
────────────────────────────────────────────────────────────────────────
* Variable: pay-per-request
** Variable: pay-per-invocation at edge

Recommendations:
  - Sub-10ms latency globally  → Edge Workers (Cloudflare/Vercel)
  - Scale-to-zero APIs         → Cloud Run / Lambda
  - Steady high-throughput     → ECS Fargate / K8s pods
  - Complex stateful services  → VMs or StatefulSets
────────────────────────────────────────────────────────────────────────
```

---

## Troubleshooting

| Problem | Cause | Solution |
|---------|-------|----------|
| Cloud Run cold starts >3s | Container image too large | Use multi-stage builds, target <100MB |
| Lambda@Edge timeout | Code exceeds 5s (viewer) / 30s (origin) | Move heavy logic to origin, keep edge lightweight |
| Multi-cloud data inconsistency | Dual-write race condition | Use CRDTs or event sourcing with idempotent consumers |
| Edge cache not invalidating | Cache key mismatch | Standardize cache keys, use surrogate keys for purge |
| Fargate tasks failing health checks | App not binding to 0.0.0.0 | Ensure `server.listen(PORT, '0.0.0.0')` |
| Cloudflare Worker hitting CPU limit | Complex computation at edge | Offload heavy work to origin, keep worker <2ms CPU |
| Terraform drift between clouds | State file out of sync | Use remote state with locking, run `terraform plan` regularly |
| Vercel Edge Function 502s | Unsupported Node.js APIs at edge | Use Web Standard APIs (`fetch`, `crypto`, `Response`) |

---

## Best Practices Checklist

- [ ] Use edge workers for latency-sensitive, read-heavy operations (A/B testing, geo-routing, auth)
- [ ] Deploy serverless containers (Cloud Run/Fargate) for variable-traffic APIs
- [ ] Keep always-on VMs/containers for steady-state workloads and stateful services
- [ ] Implement multi-cloud failover with health-checked DNS routing
- [ ] Use dual-write or event sourcing for multi-cloud data consistency
- [ ] Minimize edge worker code size — offload heavy logic to origin
- [ ] Implement proper cache invalidation strategy across CDN layers
- [ ] Monitor cold start times and set minimum instances for latency-critical paths
- [ ] Use infrastructure as code (Terraform) for all multi-cloud deployments
- [ ] Test failover scenarios regularly — don't wait for production incidents

---

## Cross-References

- See [Architecture Patterns](./01-architecture-patterns.md) for deployment pattern overview
- See [Monolith & Microservices](./02-monolith-microservices-deep-dive.md) for migration strategies
- See [Docker](../docker/01-dockerfile.md) for container image best practices
- See [Kubernetes](../03-container-orchestration/01-kubernetes-patterns.md) for K8s deployment
- See [Serverless](../04-serverless-deployment/01-lambda-patterns.md) for Lambda patterns
- See [Terraform](../06-infrastructure-as-code/01-terraform.md) for IaC setup
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for deployment pipelines
- See [Caching & Redis](../../16-caching-redis/) for caching strategies
- See [WebSockets & Realtime](../../14-websockets-realtime/) for edge WebSocket patterns

---

## Next Steps

Continue to [Cloud Platforms Deep Dive](../02-cloud-platforms/01-aws-deep-dive.md).
