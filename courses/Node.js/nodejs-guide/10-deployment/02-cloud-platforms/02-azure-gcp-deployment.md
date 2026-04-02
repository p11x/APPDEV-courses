# Azure & Google Cloud Platform Deployment Guide

## What You'll Learn

- Azure App Service deployment with ARM templates and deployment slots
- Azure Functions (HTTP, queue, timer triggers)
- Azure Container Instances and DevOps pipelines
- Google Cloud Compute Engine, Cloud Run, Cloud Functions
- GKE cluster setup with workload identity
- Platform comparison matrix

---

## Azure App Service

### ARM Template Deployment

```json
// arm-template.json — Full App Service deployment
{
    "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
    "contentVersion": "1.0.0.0",
    "parameters": {
        "appName": { "type": "string", "defaultValue": "my-node-app" },
        "skuName": { "type": "string", "defaultValue": "S1" },
        "location": { "type": "string", "defaultValue": "[resourceGroup().location]" }
    },
    "variables": {
        "appServicePlanName": "[concat(parameters('appName'), '-plan')]"
    },
    "resources": [
        {
            "type": "Microsoft.Web/serverfarms",
            "apiVersion": "2022-09-01",
            "name": "[variables('appServicePlanName')]",
            "location": "[parameters('location')]",
            "sku": { "name": "[parameters('skuName')]" },
            "properties": { "reserved": true }
        },
        {
            "type": "Microsoft.Web/sites",
            "apiVersion": "2022-09-01",
            "name": "[parameters('appName')]",
            "location": "[parameters('location')]",
            "dependsOn": ["[resourceId('Microsoft.Web/serverfarms', variables('appServicePlanName'))]"],
            "properties": {
                "serverFarmId": "[resourceId('Microsoft.Web/serverfarms', variables('appServicePlanName'))]",
                "siteConfig": {
                    "linuxFxVersion": "NODE|20-lts",
                    "appSettings": [
                        { "name": "NODE_ENV", "value": "production" },
                        { "name": "WEBSITE_NODE_DEFAULT_VERSION", "value": "20.x" }
                    ],
                    "healthCheckPath": "/health",
                    "alwaysOn": true
                },
                "httpsOnly": true
            }
        },
        {
            "type": "Microsoft.Web/sites/slots",
            "apiVersion": "2022-09-01",
            "name": "[concat(parameters('appName'), '/staging')]",
            "location": "[parameters('location')]",
            "dependsOn": ["[resourceId('Microsoft.Web/sites', parameters('appName'))]"],
            "properties": {
                "siteConfig": {
                    "linuxFxVersion": "NODE|20-lts",
                    "appSettings": [
                        { "name": "NODE_ENV", "value": "staging" }
                    ]
                }
            }
        }
    ]
}
```

```bash
# Deploy ARM template
az deployment group create \
    --resource-group myResourceGroup \
    --template-file arm-template.json \
    --parameters appName=my-node-app skuName=S1
```

### Deployment Slots and Swapping

```bash
# Create staging slot
az webapp deployment slot create \
    --name my-node-app \
    --resource-group myResourceGroup \
    --slot staging

# Deploy to staging
az webapp deployment source config-zip \
    --name my-node-app \
    --resource-group myResourceGroup \
    --slot staging \
    --src app.zip

# Swap staging to production (with preview)
az webapp deployment slot swap \
    --name my-node-app \
    --resource-group myResourceGroup \
    --slot staging \
    --target-slot production \
    --action swap
```

### Custom Domain and Scaling Rules

```bash
# Add custom domain
az webapp config hostname add \
    --webapp-name my-node-app \
    --resource-group myResourceGroup \
    --hostname app.example.com

# Configure auto-scale rules
az monitor autoscale create \
    --resource-group myResourceGroup \
    --resource my-node-app \
    --resource-type Microsoft.Web/sites \
    --min-count 2 --max-count 10 --count 2

az monitor autoscale rule create \
    --resource-group myResourceGroup \
    --autoscale-name my-node-app \
    --condition "CpuPercentage > 70 avg 5m" \
    --scale out 2

az monitor autoscale rule create \
    --resource-group myResourceGroup \
    --autoscale-name my-node-app \
    --condition "CpuPercentage < 30 avg 10m" \
    --scale in 1
```

### App Service Diagnostics Configuration

```javascript
// src/azure-diagnostics.js
import { DefaultAzureCredential } from '@azure/identity';
import { MonitorClient } from '@azure/monitor-query';

const credential = new DefaultAzureCredential();
const monitorClient = new MonitorClient(credential);

export async function getAppMetrics(resourceUri) {
    const metrics = await monitorClient.queryResource(
        resourceUri,
        ['Requests', 'CpuPercentage', 'MemoryPercentage', 'Http2xx', 'Http5xx'],
        { duration: 'PT1H', granularity: 'PT5M' }
    );
    return metrics;
}

// Enable Application Insights integration
// appsettings.json add: APPINSIGHTS_INSTRUMENTATIONKEY
```

---

## Azure Functions

### HTTP Trigger

```javascript
// HttpTrigger/index.js
module.exports = async function (context, req) {
    context.log('HTTP trigger processed a request.');

    const name = req.query.name || (req.body && req.body.name);
    const responseMessage = name
        ? `Hello, ${name}! Processed at ${new Date().toISOString()}`
        : 'Pass a name in query string or request body.';

    context.res = {
        status: 200,
        body: {
            message: responseMessage,
            invocationId: context.invocationId,
            functionName: context.executionContext.functionName
        },
        headers: { 'Content-Type': 'application/json' }
    };
};
```

```json
// HttpTrigger/function.json
{
    "bindings": [
        {
            "authLevel": "function",
            "type": "httpTrigger",
            "direction": "in",
            "name": "req",
            "methods": ["get", "post"],
            "route": "users/{id?}"
        },
        {
            "type": "http",
            "direction": "out",
            "name": "res"
        }
    ]
}
```

### Queue Trigger

```javascript
// QueueProcessor/index.js
module.exports = async function (context, queueMessage) {
    context.log('Queue trigger processed:', JSON.stringify(queueMessage));

    const { orderId, action, payload } = queueMessage;

    try {
        switch (action) {
            case 'process-payment':
                await processPayment(payload);
                break;
            case 'send-notification':
                await sendNotification(payload);
                break;
            default:
                context.log.warn(`Unknown action: ${action}`);
        }
        context.log(`Order ${orderId} processed successfully.`);
    } catch (error) {
        context.log.error(`Failed to process order ${orderId}:`, error.message);
        throw error; // Retries up to maxDequeueCount
    }
};
```

```json
// QueueProcessor/function.json
{
    "bindings": [
        {
            "type": "queueTrigger",
            "name": "queueMessage",
            "direction": "in",
            "queueName": "order-queue",
            "connection": "AzureWebJobsStorage"
        }
    ]
}
```

### Timer Trigger

```javascript
// ScheduledCleanup/index.js
module.exports = async function (context, myTimer) {
    const timeStamp = new Date().toISOString();

    if (myTimer.isPastDue) {
        context.log('Timer trigger is running late!');
    }

    context.log('Scheduled cleanup ran at:', timeStamp);

    // Delete expired sessions
    const deletedSessions = await cleanupExpiredSessions();
    context.log(`Cleaned up ${deletedSessions.count} expired sessions.`);

    // Archive old records
    const archived = await archiveOldRecords(30); // 30 days
    context.log(`Archived ${archived.count} records.`);
};
```

```json
// ScheduledCleanup/function.json
{
    "bindings": [
        {
            "type": "timerTrigger",
            "name": "myTimer",
            "direction": "in",
            "schedule": "0 0 2 * * *",
            "runOnStartup": false,
            "useMonitor": true
        }
    ]
}
```

### host.json — Global Function Configuration

```json
{
    "version": "2.0",
    "logging": {
        "applicationInsights": {
            "samplingSettings": { "isEnabled": true, "excludedTypes": "Request" }
        }
    },
    "extensionBundle": {
        "id": "Microsoft.Azure.Functions.ExtensionBundle",
        "version": "[3.*, 4.0.0)"
    },
    "functionTimeout": "00:05:00",
    "concurrency": {
        "dynamicConcurrencyEnabled": true,
        "snapshotPersistenceEnabled": true
    }
}
```

---

## Azure Container Instances

```yaml
# aci-deployment.yaml
apiVersion: 2021-10-01
location: eastus
name: node-app-container
properties:
  containers:
    - name: node-app
      properties:
        image: myregistry.azurecr.io/node-app:latest
        resources:
          requests:
            cpu: 1.0
            memoryInGB: 1.5
        ports:
          - port: 3000
            protocol: TCP
        environmentVariables:
          - name: NODE_ENV
            secureValue: production
          - name: DATABASE_URL
            secureValue: "mongodb://user:pass@host:27017/db"
          - name: REDIS_URL
            secureValue: "redis://:password@host:6380"
        volumeMounts:
          - name: app-config
            mountPath: /app/config
          - name: app-logs
            mountPath: /app/logs
  volumes:
    - name: app-config
      azureFile:
        shareName: config
        storageAccountName: mystorageaccount
        storageAccountKey: "base64key=="
    - name: app-logs
      azureFile:
        shareName: logs
        storageAccountName: mystorageaccount
        storageAccountKey: "base64key=="
  osType: Linux
  ipAddress:
    type: Public
    ports:
      - port: 3000
        protocol: TCP
  restartPolicy: OnFailure
tags:
  environment: production
  project: node-app
```

```bash
# Deploy to ACI
az container create \
    --resource-group myResourceGroup \
    --file aci-deployment.yaml

# View logs
az container logs \
    --resource-group myResourceGroup \
    --name node-app-container \
    --follow
```

---

## Azure DevOps Pipeline

```yaml
# azure-pipelines.yml — Full CI/CD pipeline
trigger:
  branches:
    include: [main, develop]
  paths:
    exclude: ['docs/**', '*.md']

pr:
  branches:
    include: [main]

variables:
  NODE_VERSION: '20.x'
  AZURE_SUBSCRIPTION: 'Azure-Connection'
  APP_NAME: 'my-node-app'
  RESOURCE_GROUP: 'myResourceGroup'

stages:
  - stage: Build
    jobs:
      - job: BuildJob
        pool:
          vmImage: 'ubuntu-latest'
        steps:
          - task: NodeTool@0
            inputs:
              versionSpec: $(NODE_VERSION)
            displayName: 'Install Node.js'

          - script: npm ci
            displayName: 'Install dependencies'

          - script: npm run lint
            displayName: 'Lint'

          - script: npm test -- --coverage
            displayName: 'Test with coverage'

          - script: npm run build
            displayName: 'Build'

          - task: ArchiveFiles@2
            inputs:
              rootFolderOrFile: '$(System.DefaultWorkingDirectory)'
              includeRootFolder: false
              archiveType: zip
              archiveFile: '$(Build.ArtifactStagingDirectory)/app.zip'

          - publish: $(Build.ArtifactStagingDirectory)/app.zip
            artifact: drop

  - stage: DeployStaging
    dependsOn: Build
    condition: succeeded()
    jobs:
      - deployment: DeployStaging
        environment: 'staging'
        strategy:
          runOnce:
            deploy:
              steps:
                - download: current
                  artifact: drop
                - task: AzureWebApp@1
                  inputs:
                    azureSubscription: $(AZURE_SUBSCRIPTION)
                    appName: $(APP_NAME)
                    deployToSlotOrASE: true
                    resourceGroupName: $(RESOURCE_GROUP)
                    slotName: staging
                    package: '$(Pipeline.Workspace)/drop/app.zip'

  - stage: DeployProduction
    dependsOn: DeployStaging
    condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
    jobs:
      - deployment: DeployProd
        environment: 'production'
        strategy:
          runOnce:
            deploy:
              steps:
                - task: AzureWebApp@1
                  inputs:
                    azureSubscription: $(AZURE_SUBSCRIPTION)
                    appName: $(APP_NAME)
                    deployToSlotOrASE: true
                    resourceGroupName: $(RESOURCE_GROUP)
                    slotName: staging
                    package: '$(Pipeline.Workspace)/drop/app.zip'
                - task: AzureAppServiceManage@0
                  inputs:
                    azureSubscription: $(AZURE_SUBSCRIPTION)
                    webAppName: $(APP_NAME)
                    resourceGroupName: $(RESOURCE_GROUP)
                    action: 'Swap Slots'
                    sourceSlot: staging
```

---

## Google Cloud Compute Engine

### Startup Script

```bash
#!/bin/bash
# startup-script.sh
set -euo pipefail

# Install Node.js
curl -fsSL https://deb.nodesource.com/setup_20.x | bash -
apt-get install -y nodejs nginx

# Clone and setup application
cd /opt
git clone https://github.com/org/node-app.git
cd node-app
npm ci --production

# Configure environment
cat > /opt/node-app/.env << EOF
NODE_ENV=production
PORT=3000
DATABASE_URL=${DATABASE_URL}
REDIS_URL=${REDIS_URL}
EOF

# Create systemd service
cat > /etc/systemd/system/node-app.service << 'EOF'
[Unit]
Description=Node.js Application
After=network.target

[Service]
Type=simple
User=www-data
WorkingDirectory=/opt/node-app
ExecStart=/usr/bin/node dist/index.js
Restart=always
RestartSec=5
Environment=NODE_ENV=production
EnvironmentFile=/opt/node-app/.env

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable node-app
systemctl start node-app

# Configure nginx reverse proxy
cat > /etc/nginx/sites-available/node-app << 'EOF'
server {
    listen 80;
    server_name _;

    location / {
        proxy_pass http://127.0.0.1:3000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_cache_bypass $http_upgrade;
    }
}
EOF

ln -sf /etc/nginx/sites-available/node-app /etc/nginx/sites-enabled/
nginx -t && systemctl restart nginx
```

### Instance Template and Managed Instance Group

```bash
# Create instance template
gcloud compute instance-templates create node-app-template \
    --machine-type=e2-medium \
    --network-interface=network=default,network-tier=PREMIUM \
    --metadata-from-file=startup-script=startup-script.sh \
    --metadata=DATABASE_URL="mongodb://...",REDIS_URL="redis://..." \
    --maintenance-policy=MIGRATE \
    --provisioning-model=STANDARD \
    --service-account=node-app@project.iam.gserviceaccount.com \
    --scopes=https://www.googleapis.com/auth/cloud-platform \
    --tags=http-server,https-server \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --boot-disk-size=20GB \
    --boot-disk-type=pd-balanced \
    --reservation-affinity=any

# Create managed instance group
gcloud compute instance-groups managed create node-app-mig \
    --template=node-app-template \
    --size=2 \
    --zone=us-central1-a

# Set named ports for load balancer
gcloud compute instance-groups managed set-named-ports node-app-mig \
    --named-ports=http:3000 \
    --zone=us-central1-a

# Configure autoscaling
gcloud compute instance-groups managed set-autoscaling node-app-mig \
    --zone=us-central1-a \
    --min-num-replicas=2 \
    --max-num-replicas=10 \
    --target-cpu-utilization=0.65 \
    --cool-down-period=60

# Health check
gcloud compute health-checks create http node-app-health-check \
    --port=3000 \
    --request-path=/health \
    --check-interval=30s \
    --timeout=5s \
    --healthy-threshold=2 \
    --unhealthy-threshold=3

gcloud compute instance-groups managed update node-app-mig \
    --zone=us-central1-a \
    --health-check=node-app-health-check \
    --initial-delay=120
```

---

## Google Cloud Run

### Full Service Deployment

```yaml
# cloudrun-service.yaml
apiVersion: serving.knative.dev/v1
kind: Service
metadata:
  name: node-app
  labels:
    cloud.googleapis.com/location: us-central1
  annotations:
    run.googleapis.com/ingress: all
    run.googleapis.com/vpc-access-connector: projects/PROJECT_ID/locations/us-central1/connectors/my-vpc-connector
spec:
  template:
    metadata:
      annotations:
        autoscaling.knative.dev/minScale: "1"
        autoscaling.knative.dev/maxScale: "100"
        run.googleapis.com/cpu: "1000m"
        run.googleapis.com/memory: "512Mi"
        run.googleapis.com/execution-environment: gen2
        run.googleapis.com/startup-cpu-boost: "true"
    spec:
      containerConcurrency: 80
      timeoutSeconds: 300
      serviceAccountName: node-app@PROJECT_ID.iam.gserviceaccount.com
      containers:
        - image: us-central1-docker.pkg.dev/PROJECT_ID/node-app-repo/node-app:latest
          ports:
            - name: http1
              containerPort: 3000
          env:
            - name: NODE_ENV
              value: production
            - name: PORT
              value: "3000"
            - name: DB_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: db-password
                  key: latest
          resources:
            limits:
              cpu: "1000m"
              memory: "512Mi"
          livenessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 10
            periodSeconds: 30
          startupProbe:
            httpGet:
              path: /health
              port: 3000
            failureThreshold: 5
            periodSeconds: 10
  traffic:
    - percent: 90
      latestRevision: true
    - percent: 10
      revisionName: node-app-prev
```

```bash
# Build and push image
gcloud builds submit --tag us-central1-docker.pkg.dev/PROJECT_ID/node-app-repo/node-app

# Deploy with traffic splitting
gcloud run services replace cloudrun-service.yaml \
    --region us-central1

# Manual traffic split
gcloud run services update-traffic node-app \
    --region us-central1 \
    --to-revisions node-app-00001-abc=20,node-app-00002-def=80

# Configure VPC connector
gcloud compute networks vpc-access connectors create my-vpc-connector \
    --region us-central1 \
    --subnet default \
    --min-instances 2 \
    --max-instances 10
```

### Cloud Run Application Code

```javascript
// src/index.js — Cloud Run optimized Express app
import express from 'express';

const app = express();
app.use(express.json());

const PORT = process.env.PORT || 3000;
let requestCount = 0;
const startTime = Date.now();

app.get('/health', (req, res) => {
    res.json({
        status: 'healthy',
        uptime: Math.floor((Date.now() - startTime) / 1000),
        requestCount,
        revision: process.env.K_REVISION || 'local'
    });
});

app.get('/api/data', async (req, res) => {
    requestCount++;
    // Cloud Run sends SIGTERM for graceful shutdown
    res.json({ message: 'Hello from Cloud Run', instance: process.env.CLOUD_RUN_EXECUTION });
});

// Graceful shutdown for Cloud Run
process.on('SIGTERM', () => {
    console.log('SIGTERM received, shutting down gracefully...');
    server.close(() => {
        console.log('Server closed.');
        process.exit(0);
    });
    setTimeout(() => process.exit(1), 10000);
});

const server = app.listen(PORT, () => {
    console.log(`Server listening on port ${PORT}`);
});
```

---

## Google Cloud Functions

### HTTP Function

```javascript
// index.js — Cloud Functions (2nd gen, built on Cloud Run)
const functions = require('@google-cloud/functions-framework');
const { Firestore } = require('@google-cloud/firestore');

const firestore = new Firestore();

functions.http('getUser', async (req, res) => {
    const { method, query, body } = req;

    // CORS headers
    res.set('Access-Control-Allow-Origin', '*');
    if (method === 'OPTIONS') {
        res.set('Access-Control-Allow-Methods', 'GET, POST');
        res.set('Access-Control-Allow-Headers', 'Content-Type');
        res.status(204).send('');
        return;
    }

    try {
        if (method === 'GET') {
            const doc = await firestore.collection('users').doc(query.id).get();
            if (!doc.exists) return res.status(404).json({ error: 'User not found' });
            return res.json({ id: doc.id, ...doc.data() });
        }

        if (method === 'POST') {
            const ref = await firestore.collection('users').add({
                ...body,
                createdAt: new Date()
            });
            return res.status(201).json({ id: ref.id });
        }

        res.status(405).json({ error: 'Method not allowed' });
    } catch (error) {
        console.error('Error:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});
```

### Background / Event Function

```javascript
// Background function triggered by Cloud Storage events
const functions = require('@google-cloud/functions-framework');
const { Storage } = require('@google-cloud/storage');
const sharp = require('sharp');

const storage = new Storage();

functions.cloudEvent('processImage', async (cloudEvent) => {
    const { bucket, name } = cloudEvent.data;

    if (!name.match(/\.(jpg|jpeg|png)$/i)) {
        console.log(`Skipping non-image file: ${name}`);
        return;
    }

    console.log(`Processing image: ${name} from bucket: ${bucket}`);

    const sourceBucket = storage.bucket(bucket);
    const file = sourceBucket.file(name);
    const [buffer] = await file.download();

    // Generate thumbnail
    const thumbnail = await sharp(buffer)
        .resize(200, 200, { fit: 'cover' })
        .toFormat('jpeg', { quality: 80 })
        .toBuffer();

    const thumbBucket = storage.bucket(`${bucket}-thumbnails`);
    const thumbFile = thumbBucket.file(`thumb_${name}`);
    await thumbFile.save(thumbnail, { contentType: 'image/jpeg' });

    console.log(`Thumbnail created: thumb_${name}`);
});
```

```bash
# Deploy HTTP function
gcloud functions deploy getUser \
    --gen2 \
    --runtime=nodejs20 \
    --region=us-central1 \
    --source=. \
    --entry-point=getUser \
    --trigger-http \
    --allow-unauthenticated \
    --memory=256MB \
    --timeout=60s

# Deploy background function
gcloud functions deploy processImage \
    --gen2 \
    --runtime=nodejs20 \
    --region=us-central1 \
    --source=. \
    --entry-point=processImage \
    --trigger-event=google.storage.object.finalize \
    --trigger-resource=projects/_/buckets/my-uploads-bucket \
    --memory=512MB \
    --timeout=120s
```

---

## GKE (Google Kubernetes Engine)

### Cluster Setup and Workload Identity

```bash
# Create GKE cluster with Workload Identity
gcloud container clusters create node-app-cluster \
    --region us-central1 \
    --num-nodes 3 \
    --min-nodes 2 \
    --max-nodes 10 \
    --enable-autoscaling \
    --machine-type e2-standard-4 \
    --enable-workload-identity \
    --workload-pool=PROJECT_ID.svc.id.goog \
    --enable-ip-alias \
    --network default \
    --subnetwork default \
    --enable-network-policy \
    --disk-size 50GB \
    --disk-type pd-balanced

# Get credentials
gcloud container clusters get-credentials node-app-cluster --region us-central1

# Create GSA and bind to KSA for Workload Identity
gcloud iam service-accounts create gke-node-app-sa \
    --display-name="GKE Node App Service Account"

gcloud projects add-iam-policy-binding PROJECT_ID \
    --member="serviceAccount:gke-node-app-sa@PROJECT_ID.iam.gserviceaccount.com" \
    --role="roles/cloudsql.client"

kubectl create namespace production

kubectl create serviceaccount node-app-ksa \
    --namespace production

gcloud iam service-accounts add-iam-policy-binding \
    gke-node-app-sa@PROJECT_ID.iam.gserviceaccount.com \
    --role roles/iam.workloadIdentityUser \
    --member "serviceAccount:PROJECT_ID.svc.id.goog[production/node-app-ksa]"

kubectl annotate serviceaccount node-app-ksa \
    --namespace production \
    iam.gke.io/gcp-service-account=gke-node-app-sa@PROJECT_ID.iam.gserviceaccount.com
```

```yaml
# k8s/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
  namespace: production
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
      serviceAccountName: node-app-ksa
      containers:
        - name: node-app
          image: us-central1-docker.pkg.dev/PROJECT_ID/node-app-repo/node-app:latest
          ports:
            - containerPort: 3000
          env:
            - name: NODE_ENV
              value: production
            - name: DB_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: db-credentials
                  key: password
          resources:
            requests:
              cpu: 250m
              memory: 256Mi
            limits:
              cpu: 1000m
              memory: 512Mi
          livenessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 15
            periodSeconds: 20
          readinessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 5
            periodSeconds: 10
          lifecycle:
            preStop:
              exec:
                command: ["sh", "-c", "sleep 10"]
      terminationGracePeriodSeconds: 30
---
apiVersion: v1
kind: Service
metadata:
  name: node-app-service
  namespace: production
spec:
  type: ClusterIP
  selector:
    app: node-app
  ports:
    - port: 80
      targetPort: 3000
      protocol: TCP
---
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: node-app-hpa
  namespace: production
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
    - type: Resource
      resource:
        name: memory
        target:
          type: Utilization
          averageUtilization: 80
```

---

## Platform Comparison Matrix

```
Feature               │ Azure App Service │ Azure Functions │ Azure ACI │ Cloud Run │ Cloud Functions │ GCE │ GKE
──────────────────────┼───────────────────┼─────────────────┼───────────┼───────────┼─────────────────┼─────┼─────
Compute Model         │ PaaS              │ Serverless      │ Container │ Serverless│ Serverless      │ IaaS│ K8s
Cold Start            │ None (Always On)  │ ~1-3s           │ ~10-30s   │ ~1-2s     │ ~1-2s           │ None│ None
Max Timeout           │ Unlimited         │ 10 min          │ Unlimited │ 60 min    │ 9 min           │ N/A │ N/A
Auto-scaling          │ Yes (rules)       │ Automatic       │ Manual    │ 0 to N    │ Automatic       │ MIG │ HPA
Concurrency           │ Per instance      │ Per function    │ Per inst. │ 80-1000   │ 1 per instance  │ N/A │ Config
Custom Domains        │ Yes               │ No              │ No        │ Yes       │ No              │ Yes │ Yes
VPC Integration       │ VNet Integration  │ VNet Integration│ VNet      │ VPC Conn. │ VPC Connector   │ VPC │ VPC
Deployment Slots      │ Yes               │ Slots           │ No        │ Revisions │ No              │ No  │ No
WebSockets            │ Yes               │ No              │ Yes       │ Yes       │ No              │ Yes │ Yes
Best For              │ Web apps, APIs    │ Event-driven    │ Batch     │ APIs, microsvc│ Event-driven  │ Full│ Microsvc
Pricing Model         │ Per instance      │ Per execution   │ Per sec   │ Per req.  │ Per execution   │ Per │ Per node
```

---

## Best Practices Checklist

- [ ] Use deployment slots for zero-downtime deployments
- [ ] Enable Application Insights / Cloud Monitoring for observability
- [ ] Configure auto-scaling rules based on CPU and memory thresholds
- [ ] Use managed identity / service accounts instead of embedded credentials
- [ ] Set up VPC integration for private resource access
- [ ] Implement health checks and readiness probes
- [ ] Use Cloud Run revisions for instant rollbacks
- [ ] Enable workload identity on GKE for pod-level IAM
- [ ] Configure concurrency limits based on application profiling
- [ ] Use ARM templates or Terraform for reproducible infrastructure

---

## Cross-References

- See [AWS Deep Dive](./01-aws-deep-dive.md) for AWS-specific patterns
- See [Multi-Cloud & Cost Optimization](./03-multi-cloud-cost-optimization.md) for cost strategies
- See [CI/CD Pipelines](../05-ci-cd-pipelines/01-github-actions.md) for pipeline automation
- See [Infrastructure as Code](../06-infrastructure-as-code/01-terraform.md) for Terraform modules
- See [Container Orchestration](../03-container-orchestration/01-kubernetes-patterns.md) for K8s patterns
- See [Serverless Patterns](../04-serverless-deployment/01-lambda-patterns.md) for serverless architectures

---

## Next Steps

Continue to [Multi-Cloud & Cost Optimization](./03-multi-cloud-cost-optimization.md).
