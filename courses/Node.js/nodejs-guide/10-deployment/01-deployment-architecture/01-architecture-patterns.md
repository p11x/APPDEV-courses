# Deployment Architecture Patterns

## What You'll Learn

- Monolith vs microservices deployment
- Serverless deployment patterns
- Container-based strategies
- Hybrid and multi-cloud architectures
- Edge deployment patterns

## Deployment Architecture Comparison

```
Deployment Architecture Comparison:
─────────────────────────────────────────────
Pattern        Complexity  Scalability  Cost     Use Case
─────────────────────────────────────────────
Monolith       Low         Vertical     Low      Small-medium apps
Microservices  High        Horizontal   Medium   Large distributed apps
Serverless     Medium      Auto         Variable Event-driven, APIs
Container      Medium      Horizontal   Medium   Portable, scalable
Kubernetes     High        Auto         High     Enterprise, multi-team
Edge           High        Global       High     Low-latency, global
```

## Monolith Deployment

```javascript
// ecosystem.config.js (PM2)
module.exports = {
    apps: [{
        name: 'my-app',
        script: 'dist/index.js',
        instances: 'max', // Use all CPUs
        exec_mode: 'cluster',
        env: {
            NODE_ENV: 'development',
            PORT: 3000,
        },
        env_production: {
            NODE_ENV: 'production',
            PORT: 3000,
        },
        max_memory_restart: '512M',
        error_file: '/var/log/pm2/error.log',
        out_file: '/var/log/pm2/out.log',
        merge_logs: true,
    }],
};

// nginx.conf for monolith
const nginxConfig = `
upstream node_app {
    least_conn;
    server 127.0.0.1:3000;
    server 127.0.0.1:3001;
    server 127.0.0.1:3002;
    server 127.0.0.1:3003;
}

server {
    listen 80;
    server_name example.com;

    location / {
        proxy_pass http://node_app;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }

    location /health {
        proxy_pass http://node_app/health;
        access_log off;
    }

    location /static/ {
        alias /var/www/static/;
        expires 30d;
        add_header Cache-Control "public, immutable";
    }
}
`;
```

## Microservices Deployment

```yaml
# docker-compose.microservices.yml
version: '3.8'

services:
  api-gateway:
    image: myapp/api-gateway:latest
    ports:
      - "80:80"
    depends_on:
      - user-service
      - order-service
    environment:
      - USER_SERVICE_URL=http://user-service:3001
      - ORDER_SERVICE_URL=http://order-service:3002

  user-service:
    image: myapp/user-service:latest
    environment:
      - DATABASE_URL=postgresql://user:pass@user-db:5432/users
      - REDIS_URL=redis://redis:6379/0
    depends_on:
      - user-db
      - redis
    deploy:
      replicas: 2
      resources:
        limits:
          memory: 256M
          cpus: '0.5'

  order-service:
    image: myapp/order-service:latest
    environment:
      - DATABASE_URL=postgresql://user:pass@order-db:5432/orders
      - RABBITMQ_URL=amqp://rabbitmq:5672
    depends_on:
      - order-db
      - rabbitmq
    deploy:
      replicas: 2

  user-db:
    image: postgres:15-alpine
    environment:
      POSTGRES_DB: users
      POSTGRES_USER: user
      POSTGRES_PASSWORD: pass
    volumes:
      - user-db-data:/var/lib/postgresql/data

  order-db:
    image: postgres:15-alpine
    environment:
      POSTGRES_DB: orders
      POSTGRES_USER: user
      POSTGRES_PASSWORD: pass

  redis:
    image: redis:7-alpine
    volumes:
      - redis-data:/data

  rabbitmq:
    image: rabbitmq:3-management-alpine
    ports:
      - "15672:15672"

volumes:
  user-db-data:
  redis-data:
```

## Serverless Deployment

```yaml
# serverless.yml
service: my-api

provider:
  name: aws
  runtime: nodejs20.x
  region: us-east-1
  stage: ${opt:stage, 'dev'}
  memorySize: 256
  timeout: 30
  environment:
    DATABASE_URL: ${ssm:/myapp/${self:provider.stage}/database-url}
    REDIS_URL: ${ssm:/myapp/${self:provider.stage}/redis-url}
  iam:
    role:
      statements:
        - Effect: Allow
          Action:
            - dynamodb:*
          Resource: !GetAtt UsersTable.Arn

functions:
  api:
    handler: src/handler.handler
    events:
      - http:
          path: /
          method: ANY
      - http:
          path: /{proxy+}
          method: ANY
    provisionedConcurrency: 5

  processOrder:
    handler: src/jobs/processOrder.handler
    events:
      - sqs:
          arn: !GetAtt OrderQueue.Arn
          batchSize: 10

resources:
  Resources:
    UsersTable:
      Type: AWS::DynamoDB::Table
      Properties:
        TableName: users-${self:provider.stage}
        BillingMode: PAY_PER_REQUEST
        AttributeDefinitions:
          - AttributeName: id
            AttributeType: S
        KeySchema:
          - AttributeName: id
            KeyType: HASH

    OrderQueue:
      Type: AWS::SQS::Queue
      Properties:
        QueueName: orders-${self:provider.stage}
        VisibilityTimeout: 60
```

## Kubernetes Deployment

```yaml
# k8s/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
  labels:
    app: node-app
spec:
  replicas: 3
  selector:
    matchLabels:
      app: node-app
  template:
    metadata:
      labels:
        app: node-app
    spec:
      containers:
        - name: node-app
          image: myapp/node-app:latest
          ports:
            - containerPort: 3000
          env:
            - name: NODE_ENV
              value: production
            - name: DATABASE_URL
              valueFrom:
                secretKeyRef:
                  name: db-secrets
                  key: url
          resources:
            requests:
              memory: "128Mi"
              cpu: "100m"
            limits:
              memory: "256Mi"
              cpu: "500m"
          livenessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 10
            periodSeconds: 30
          readinessProbe:
            httpGet:
              path: /ready
              port: 3000
            initialDelaySeconds: 5
            periodSeconds: 10
      imagePullSecrets:
        - name: registry-credentials
---
apiVersion: v1
kind: Service
metadata:
  name: node-app-service
spec:
  selector:
    app: node-app
  ports:
    - port: 80
      targetPort: 3000
  type: ClusterIP
---
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: node-app-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: node-app
  minReplicas: 3
  maxReplicas: 20
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          type: Utilization
          averageUtilization: 70
```

## Best Practices Checklist

- [ ] Choose architecture based on team size and requirements
- [ ] Start with monolith, extract services when needed
- [ ] Use containers for portability
- [ ] Implement health checks for all architectures
- [ ] Use infrastructure as code for all deployments
- [ ] Plan for horizontal scaling from the start
- [ ] Implement proper logging and monitoring

## Cross-References

- See [Docker](../docker/01-dockerfile.md) for container setup
- See [Kubernetes](./02-kubernetes-patterns.md) for K8s patterns
- See [Serverless](./02-kubernetes-patterns.md) for serverless patterns
- See [CI/CD](../05-ci-cd-pipelines/01-github-actions.md) for automation

## Next Steps

Continue to [Cloud Platform Deployment](../02-cloud-platforms/01-aws-deep-dive.md).
