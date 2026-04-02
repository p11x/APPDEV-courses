# Multi-Cloud Deployment & Cost Optimization

## What You'll Learn

- DigitalOcean App Platform, Droplets, and DOKS deployment
- Alibaba Cloud, Oracle Cloud, and IBM Cloud patterns
- Cloud platform selection criteria and decision matrix
- Cost optimization: reserved, spot, right-sizing, auto-scaling
- Cost monitoring, alerting, and estimation scripts
- Multi-cloud networking and identity management
- Real-world migration scenarios

---

## DigitalOcean

### App Platform

```yaml
# .do/app.yaml — DigitalOcean App Platform
name: node-app
region: nyc
services:
  - name: api
    github:
      repo: org/node-app
      branch: main
      deploy_on_push: true
    run_command: node dist/index.js
    environment_slug: node-js
    instance_count: 2
    instance_size_slug: professional-xs
    http_port: 3000
    health_check:
      http_path: /health
      initial_delay_seconds: 10
      period_seconds: 10
      timeout_seconds: 5
      success_threshold: 1
      failure_threshold: 3
    envs:
      - key: NODE_ENV
        value: production
      - key: DATABASE_URL
        scope: RUN_TIME
        type: SECRET
        value: ${db.DATABASE_URL}
      - key: REDIS_URL
        scope: RUN_TIME
        type: SECRET
        value: ${redis.DATABASE_URL}
    autoscaling:
      min_instance_count: 1
      max_instance_count: 10
      metrics:
        - type: cpu
          target: 70
databases:
  - name: db
    engine: PG
    version: "16"
    size: db-s-1vcpu-1gb
    num_nodes: 1
  - name: redis
    engine: REDIS
    version: "7"
    size: db-s-1vcpu-1gb
    num_nodes: 1
```

```bash
# Deploy via doctl
doctl apps create --spec .do/app.yaml
doctl apps list
doctl apps logs <app-id> --follow
```

### Droplets with User Data

```bash
#!/bin/bash
# droplet-userdata.sh — Automated Node.js setup on Droplet
set -euo pipefail

apt-get update && apt-get upgrade -y
curl -fsSL https://deb.nodesource.com/setup_20.x | bash -
apt-get install -y nodejs nginx certbot python3-certbot-nginx

# Application setup
mkdir -p /var/www/node-app
cd /var/www/node-app
git clone https://github.com/org/node-app.git .
npm ci --production

# PM2 process management
npm install -g pm2
pm2 start dist/index.js --name node-app -i max
pm2 startup systemd
pm2 save

# Nginx reverse proxy
cat > /etc/nginx/sites-available/node-app << 'NGINX'
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
    }
}
NGINX

ln -sf /etc/nginx/sites-available/node-app /etc/nginx/sites-enabled/
rm -f /etc/nginx/sites-enabled/default
nginx -t && systemctl restart nginx

# Firewall
ufw allow 22/tcp
ufw allow 80/tcp
ufw allow 443/tcp
ufw --force enable
```

```bash
# Create Droplet with user data
doctl compute droplet create node-app-server \
    --image ubuntu-22-04-x64 \
    --size s-2vcpu-4gb \
    --region nyc1 \
    --user-data-file droplet-userdata.sh \
    --enable-monitoring \
    --tag-name production

# Create load balancer
doctl compute load-balancer create \
    --name node-app-lb \
    --algorithm round_robin \
    --forwarding-rules entry_protocol:https,entry_port:443,target_protocol:http,target_port:80 \
    --health-check protocol:http,port:80,path:/health,check_interval_seconds:10 \
    --region nyc1 \
    --tag-name production
```

### DigitalOcean Kubernetes (DOKS)

```yaml
# k8s/doks-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
  namespace: production
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
          image: registry.digital.co/org/node-app:latest
          ports:
            - containerPort: 3000
          env:
            - name: NODE_ENV
              value: production
            - name: DATABASE_URL
              valueFrom:
                secretKeyRef:
                  name: db-secret
                  key: url
          resources:
            requests:
              cpu: 250m
              memory: 256Mi
            limits:
              cpu: 500m
              memory: 512Mi
          readinessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 5
            periodSeconds: 10
---
apiVersion: v1
kind: Service
metadata:
  name: node-app-service
spec:
  type: LoadBalancer
  selector:
    app: node-app
  ports:
    - port: 80
      targetPort: 3000
---
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: node-app-ingress
  annotations:
    kubernetes.io/ingress.class: nginx
    cert-manager.io/cluster-issuer: letsencrypt-prod
spec:
  tls:
    - hosts: [app.example.com]
      secretName: app-tls
  rules:
    - host: app.example.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: node-app-service
                port:
                  number: 80
```

```bash
# Create DOKS cluster
doctl kubernetes cluster create node-app-cluster \
    --region nyc1 \
    --version latest \
    --node-pool "name=workers;size=s-2vcpu-4gb;count=3;auto-scale=true;min-nodes=2;max-nodes=10" \
    --enable-surge-upgrade \
    --ha

# Connect to cluster
doctl kubernetes cluster kubeconfig save node-app-cluster
```

---

## Alibaba Cloud

### ECS Deployment

```bash
#!/bin/bash
# alibaba-ecs-setup.sh
set -euo pipefail

# Install Node.js
curl -fsSL https://deb.nodesource.com/setup_20.x | bash -
apt-get install -y nodejs

# Application setup
mkdir -p /opt/node-app
cd /opt/node-app
git clone https://github.com/org/node-app.git .
npm ci --production

# PM2
npm install -g pm2
pm2 start dist/index.js --name node-app -i max
pm2 startup systemd
pm2 save
```

```bash
# Create ECS instance via CLI
aliyun ecs RunInstances \
    --ImageId ubuntu_22_04_x64_20G_alibase_20231221.vhd \
    --InstanceType ecs.g7.large \
    --SecurityGroupId sg-bp1example \
    --VSwitchId vsw-bp1example \
    --InstanceName node-app-server \
    --UserData "$(base64 alibaba-ecs-setup.sh)" \
    --SystemDisk.Category cloud_essd \
    --SystemDisk.Size 40 \
    --Amount 2 \
    --InternetMaxBandwidthOut 5 \
    --InstanceChargeType PostPaid \
    --Tag "[{\"Key\":\"env\",\"Value\":\"production\"}]"

# Create SLB (Server Load Balancer)
aliyun slb CreateLoadBalancer \
    --LoadBalancerName node-app-lb \
    --LoadBalancerSpec slb.s2.medium \
    --AddressType internet \
    --InternetChargeType paybytraffic

# Auto Scaling Group
aliyun ess CreateScalingGroup \
    --ScalingGroupName node-app-asg \
    --MinSize 2 \
    --MaxSize 10 \
    --DefaultCooldown 300 \
    --RemovalPolicy ["OldestInstance","OldestScalingConfiguration"] \
    --LoadBalancerIds ["lb-bp1example"]
```

### Alibaba Cloud Function Compute

```javascript
// index.js — Alibaba Cloud Function Compute (FC 3.0)
const express = require('express');
const app = express();

app.use(express.json());

app.get('/health', (req, res) => {
    res.json({ status: 'ok', provider: 'alibaba-fc' });
});

app.get('/api/products', async (req, res) => {
    const products = await fetchProducts(req.query);
    res.json({ data: products, count: products.length });
});

// FC 3.0 HTTP handler
exports.handler = async (req, res, context) => {
    return new Promise((resolve) => {
        app(req, res, () => resolve());
    });
};
```

```yaml
# s.yaml — Serverless Devs deployment for FC
edition: 3.0.0
name: node-app-fc
access: default

resources:
  node-app:
    component: fc3
    props:
      region: cn-hangzhou
      functionName: node-app-http
      runtime: nodejs20
      handler: index.handler
      timeout: 60
      memorySize: 512
      code: ./src
      layers:
        - acs:fc:cn-hangzhou:official:layers/Nodejs20/versions/1
      environmentVariables:
        NODE_ENV: production
      triggers:
        - triggerName: http-trigger
          triggerType: http
          triggerConfig:
            authType: anonymous
            methods: [GET, POST]
```

### Alibaba Container Service (ACK)

```yaml
# ack-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
  namespace: production
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
      serviceAccountName: node-app-sa
      nodeSelector:
        node-type: compute
      containers:
        - name: node-app
          image: registry.cn-hangzhou.cr.aliyuncs.com/org/node-app:latest
          ports:
            - containerPort: 3000
          env:
            - name: NODE_ENV
              value: production
          resources:
            requests:
              cpu: 500m
              memory: 512Mi
            limits:
              cpu: "1"
              memory: 1Gi
          readinessProbe:
            httpGet:
              path: /health
              port: 3000
            initialDelaySeconds: 5
            periodSeconds: 10
      imagePullSecrets:
        - name: acr-secret
```

---

## Oracle Cloud

### Compute Instance

```bash
# oracle-cloud-init.sh
#!/bin/bash
set -euo pipefail

yum update -y
curl -fsSL https://rpm.nodesource.com/setup_20.x | bash -
yum install -y nodejs nginx

mkdir -p /opt/node-app
cd /opt/node-app
git clone https://github.com/org/node-app.git .
npm ci --production

# Systemd service
cat > /etc/systemd/system/node-app.service << 'EOF'
[Unit]
Description=Node.js App
After=network.target
[Service]
Type=simple
User=opc
WorkingDirectory=/opt/node-app
ExecStart=/usr/bin/node dist/index.js
Restart=always
Environment=NODE_ENV=production
[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable --now node-app
```

```bash
# Create instance via OCI CLI
oci compute instance launch \
    --availability-domain "AD-1" \
    --compartment-id ocid1.compartment.oc1..example \
    --shape VM.Standard.E4.Flex \
    --shape-config '{"ocpus":2,"memoryInGB":16}' \
    --image-id ocid1.image.oc1..example \
    --subnet-id ocid1.subnet.oc1..example \
    --display-name node-app-server \
    --user-data-file oracle-cloud-init.sh \
    --metadata '{"ssh_authorized_keys":"ssh-rsa AAAA..."}'
```

### Oracle Cloud Functions

```yaml
# func.yaml — Oracle Functions
schema_version: 20180708
name: node-app
version: 0.0.1
runtime: node
build_image: fnproject/node:20-dev
run_image: fnproject/node:20
entrypoint: node func.js
memory: 256
timeout: 30
```

```javascript
// func.js
const fdk = require('@fnproject/fdk');

fdk.handle(async (input) => {
    const name = input.name || 'World';
    return {
        message: `Hello ${name}!`,
        timestamp: new Date().toISOString(),
        platform: 'oracle-cloud-functions'
    };
});
```

```bash
# Deploy Oracle Function
fn -v deploy --app node-app-fn
```

---

## IBM Cloud

### IBM Code Engine

```yaml
# ibm-ce-job.yaml
apiVersion: codeengine.cloud.ibm.com/v1beta1
kind: Job
metadata:
  name: node-app-job
spec:
  imageReferences:
    - image: us.icr.io/namespace/node-app:latest
      name: node-app-job-image
  resourceReferences: []
  runEnvironmentVariables:
    - name: NODE_ENV
      value: production
    - name: TASK_TYPE
      value: batch-process
  scale:
    maxExecutionTime: 3600
    scale: 5
  timeout: 3600
```

```bash
# Deploy to IBM Code Engine
ibmcloud ce project create --name node-app-project
ibmcloud ce app create \
    --name node-app \
    --image us.icr.io/namespace/node-app:latest \
    --port 3000 \
    --min-scale 1 \
    --max-scale 10 \
    --cpu 1 \
    --memory 2G \
    --env NODE_ENV=production \
    --registry-secret icr-secret
```

### IBM Cloud Foundry

```yaml
# manifest.yml — Cloud Foundry
applications:
  - name: node-app
    memory: 512M
    instances: 2
    buildpack: nodejs_buildpack
    command: node dist/index.js
    env:
      NODE_ENV: production
    routes:
      - route: node-app.us-south.cf.appdomain.cloud
    services:
      - node-app-db
      - node-app-redis
    health-check-type: http
    health-check-http-endpoint: /health
    timeout: 60
```

```bash
# Deploy to Cloud Foundry
ibmcloud cf push -f manifest.yml
ibmcloud cf scale node-app -i 5
ibmcloud cf map-route node-app us-south.cf.appdomain.cloud --hostname custom-domain
```

---

## Cloud Platform Decision Matrix

```
Criteria              │ Weight │ DigitalOcean │ Alibaba Cloud │ Oracle Cloud │ IBM Cloud
──────────────────────┼────────┼──────────────┼───────────────┼──────────────┼──────────
Ease of Setup         │  15%   │ ★★★★★       │ ★★★☆☆        │ ★★★☆☆       │ ★★★☆☆
Node.js Support       │  15%   │ ★★★★★       │ ★★★★☆        │ ★★★☆☆       │ ★★★★☆
Auto-scaling          │  15%   │ ★★★★☆       │ ★★★★★        │ ★★★★☆       │ ★★★★☆
Global Reach          │  10%   │ ★★★☆☆       │ ★★★★★ (APAC) │ ★★★☆☆       │ ★★★★☆
Serverless Options    │  10%   │ ★★★☆☆       │ ★★★★☆        │ ★★★★☆       │ ★★★★☆
Cost Effectiveness    │  15%   │ ★★★★★       │ ★★★★☆        │ ★★★★★       │ ★★★☆☆
Kubernetes            │  10%   │ ★★★★☆       │ ★★★★★ (ACK)  │ ★★★☆☆ (OKE) │ ★★★★☆
Enterprise Features   │  10%   │ ★★★☆☆       │ ★★★★☆        │ ★★★★☆       │ ★★★★★
──────────────────────┼────────┼──────────────┼───────────────┼──────────────┼──────────
Weighted Score        │        │ 4.05         │ 4.10          │ 3.65         │ 3.70
```

### Selection Guidelines

```
Choose When:
──────────────────────────────────────────────────
DigitalOcean  → Startups, simple deployments, cost-sensitive projects
Alibaba Cloud → APAC market, China compliance, high-scale compute
Oracle Cloud  → Oracle DB workloads, bare metal, always-free tier
IBM Cloud     → Enterprise, hybrid cloud, Watson AI integration
```

---

## Cost Optimization Strategies

### Reserved Instances & Committed Use

```bash
# AWS Reserved Instance
aws ec2 purchase-reserved-instances-offering \
    --reserved-instances-offering-id offering-id \
    --instance-count 3

# GCP Committed Use Discount
gcloud compute commitments create node-app-commitment \
    --region us-central1 \
    --plan 36-month \
    --type General-Purpose \
    --resources vcpu=8,memory=32GB

# Azure Reserved Instance
az vm reservation purchase \
    --reserved-instance-type VirtualMachines \
    --sku Standard_D2s_v3 \
    --term P1Y \
    --quantity 5
```

### Spot / Preemptible Instances

```yaml
# k8s/spot-deployment.yaml — Spot instance node pool
apiVersion: v1
kind: NodePool
metadata:
  name: spot-workers
spec:
  template:
    spec:
      nodeSelector:
        cloud.google.com/gke-spot: "true"      # GCP
        # karpenter.sh/capacity-type: spot      # AWS Karpenter
      tolerations:
        - key: "cloud.google.com/gke-spot"
          operator: "Equal"
          value: "true"
          effect: "NoSchedule"
      containers:
        - name: node-app
          resources:
            requests:
              cpu: 500m
              memory: 512Mi
```

```bash
# AWS Spot Fleet Request
aws ec2 request-spot-fleet \
    --spot-fleet-request-config file://spot-fleet-config.json

# spot-fleet-config.json
{
    "IamFleetRole": "arn:aws:iam::role/aws-ec2-spot-fleet-tagging-role",
    "AllocationStrategy": "capacityOptimized",
    "TargetCapacity": 10,
    "SpotPrice": "0.05",
    "LaunchSpecifications": [
        {
            "ImageId": "ami-0c55b159cbfafe1f0",
            "InstanceType": "t3.medium",
            "SubnetId": "subnet-12345",
            "UserData": "base64-encoded-script"
        }
    ]
}
```

### Auto-Scaling Configuration

```yaml
# k8s/hpa-with-custom-metrics.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: node-app-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: node-app
  minReplicas: 2
  maxReplicas: 50
  behavior:
    scaleUp:
      stabilizationWindowSeconds: 60
      policies:
        - type: Percent
          value: 100
          periodSeconds: 60
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
        - type: Percent
          value: 25
          periodSeconds: 120
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          type: Utilization
          averageUtilization: 70
    - type: Pods
      pods:
        metric:
          name: http_requests_per_second
        target:
          type: AverageValue
          averageValue: "1000"
```

---

## Cost Monitoring and Alerting

### CloudWatch Budget (AWS)

```json
// budget.json
{
    "BudgetName": "node-app-monthly",
    "BudgetLimit": {
        "Amount": "500",
        "Unit": "USD"
    },
    "CostFilters": {
        "TagKeyValue": ["Project$node-app"]
    },
    "CostTypes": {
        "IncludeTax": true,
        "IncludeSubscription": true,
        "UseBlended": false
    },
    "TimeUnit": "MONTHLY",
    "BudgetType": "COST"
}
```

```bash
# Create budget with alerts
aws budgets create-budget \
    --account-id 123456789012 \
    --budget file://budget.json \
    --notifications-with-subscribers file://notifications.json

# notifications.json
[
    {
        "Notification": {
            "NotificationType": "ACTUAL",
            "ComparisonOperator": "GREATER_THAN",
            "Threshold": 80,
            "ThresholdType": "PERCENTAGE"
        },
        "Subscribers": [
            { "SubscriptionType": "EMAIL", "Address": "team@example.com" },
            { "SubscriptionType": "SNS", "Address": "arn:aws:sns:us-east-1:123456789012:alerts" }
        ]
    }
]
```

### GCP Billing Alert

```bash
# Create billing budget
gcloud billing budgets create \
    --billing-account=XXXXXX-XXXXXX-XXXXXX \
    --display-name="Node App Budget" \
    --budget-amount=500USD \
    --threshold-rule=percent=80,spend-basis=current-spend \
    --threshold-rule=percent=100,spend-basis=current-spend \
    --notifications-rule-pubsub-topic=projects/PROJECT_ID/topics/billing-alerts
```

---

## Cost Estimation Script

```javascript
// scripts/cost-estimator.js
const PROVIDERS = {
    aws: {
        name: 'AWS',
        compute: { t3_medium: 0.0416, t3_large: 0.0832, m5_large: 0.096 },
        lambda: { request: 0.0000002, gbSecond: 0.0000166667 },
        dataTransfer: 0.09 // per GB
    },
    gcp: {
        name: 'Google Cloud',
        compute: { e2_medium: 0.0335, e2_standard_2: 0.067 },
        cloudRun: { vCPUSec: 0.00002400, gibSec: 0.00000250 },
        dataTransfer: 0.12
    },
    azure: {
        name: 'Azure',
        compute: { b2s: 0.0416, d2s_v3: 0.096 },
        functions: { execution: 0.0000002, gbSec: 0.000016 },
        dataTransfer: 0.087
    },
    digitalocean: {
        name: 'DigitalOcean',
        compute: { s_2vcpu_4gb: 0.0357, s_4vcpu_8gb: 0.0714 },
        appPlatform: { professional_xs: 0.03 },
        dataTransfer: 0.01 // Free egress
    }
};

function estimateMonthlyCost(config) {
    const { provider, instances, hoursPerMonth, dataGB, serverlessRequests } = config;
    const p = PROVIDERS[provider];
    if (!p) throw new Error(`Unknown provider: ${provider}`);

    const computeCost = instances * p.compute[config.instanceType] * hoursPerMonth;
    const transferCost = dataGB * p.dataTransfer;
    const serverlessCost = serverlessRequests
        ? serverlessRequests * (p.lambda?.request || p.cloudRun?.vCPUSec * 0.0001 || p.functions?.execution || 0)
        : 0;

    const total = computeCost + transferCost + serverlessCost;

    return {
        provider: p.name,
        compute: computeCost.toFixed(2),
        transfer: transferCost.toFixed(2),
        serverless: serverlessCost.toFixed(2),
        total: total.toFixed(2),
        annual: (total * 12).toFixed(2)
    };
}

// Compare all providers
function compareProviders(baseConfig) {
    const results = Object.keys(PROVIDERS).map((provider) =>
        estimateMonthlyCost({ ...baseConfig, provider })
    );

    results.sort((a, b) => parseFloat(a.total) - parseFloat(b.total));
    console.table(results);
    return results;
}

// Example usage
compareProviders({
    instances: 3,
    instanceType: 'e2_medium',
    hoursPerMonth: 730,
    dataGB: 100,
    serverlessRequests: 1000000
});
```

```bash
node scripts/cost-estimator.js
```

---

## Multi-Cloud Networking

### VPN Interconnection

```yaml
# terraform/multi-cloud-vpn.tf
# AWS side
resource "aws_vpn_gateway" "main" {
  vpc_id = aws_vpc.main.id
  tags   = { Name = "multi-cloud-vgw" }
}

resource "aws_customer_gateway" "gcp" {
  bgp_asn    = 65515
  ip_address = google_compute_ha_vpn_gateway.main.vpn_interfaces[0].ip_address
  type       = "ipsec.1"
}

resource "aws_vpn_connection" "to_gcp" {
  vpn_gateway_id      = aws_vpn_gateway.main.id
  customer_gateway_id = aws_customer_gateway.gcp.id
  type                = "ipsec.1"
  static_routes_only  = true
}

# GCP side
resource "google_compute_ha_vpn_gateway" "main" {
  name    = "gcp-to-aws-vpn"
  network = google_compute_network.main.id
  region  = "us-central1"
}

resource "google_compute_vpn_tunnel" "to_aws" {
  name                  = "gcp-aws-tunnel"
  vpn_gateway           = google_compute_ha_vpn_gateway.main.id
  peer_ip               = aws_vpn_connection.to_gcp.tunnel1_address
  shared_secret         = aws_vpn_connection.to_gcp.tunnel1_preshared_key
  vpn_gateway_interface = 0
  router                = google_compute_router.vpn.id
}
```

### Unified Identity Management

```yaml
# terraform/identity.tf — Federated identity across clouds
# OIDC provider for cross-cloud auth
resource "aws_iam_open_id_connect_provider" "gcp" {
  url = "https://accounts.google.com"
  client_id_list = ["sts.amazonaws.com"]
  thumbprint_list = ["1234567890abcdef"]
}

resource "aws_iam_role" "gcp_workload_identity" {
  name = "gcp-workload-role"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect = "Allow"
      Principal = { Federated = "arn:aws:iam::oidc-provider/accounts.google.com" }
      Action = "sts:AssumeRoleWithWebIdentity"
      Condition = {
        StringEquals = {
          "accounts.google.com:sub" = "serviceAccount:gke-sa@project.iam.gserviceaccount.com"
        }
      }
    }]
  })
}
```

---

## Real-World Migration Scenario

### Heroku to Multi-Cloud (GCP Primary + DigitalOcean DR)

```
Migration Plan: Heroku → GCP (primary) + DigitalOcean (DR)
─────────────────────────────────────────────────────────
Phase 1 — Preparation (Week 1-2)
├── Audit Heroku addons (Postgres, Redis, Scheduler)
├── Map to GCP equivalents (Cloud SQL, Memorystore, Cloud Scheduler)
├── Set up GCP project + DigitalOcean account
└── CI/CD pipeline build (GitHub Actions → GCP + DO)

Phase 2 — Database Migration (Week 3)
├── Create Cloud SQL PostgreSQL instance
├── pg_dump from Heroku → pg_restore to Cloud SQL
├── Set up continuous replication via pglogical
├── Verify data integrity
└── Sync Redis data (DUMP/RESTORE or AOF)

Phase 3 — Application Deployment (Week 4)
├── Containerize app (Dockerfile)
├── Deploy to Cloud Run (staging)
├── Run integration tests against staging
├── Deploy to DigitalOcean App Platform (DR)
└── DNS weight shift: 90% GCP, 10% Heroku

Phase 4 — Cutover (Week 5)
├── Full DNS switch to GCP (TTL reduced to 60s beforehand)
├── Monitor error rates + latency (GCP Cloud Monitoring)
├── Keep Heroku standby for 7 days
├── Update all webhook URLs + API callbacks
└── Decommission Heroku resources

Cost Impact:
├── Before: Heroku Standard-2X × 4 dynos = $500/mo
├── After:  Cloud Run (~$120) + Cloud SQL (~$80) + DO DR (~$25)
└── Savings: ~55% ($275/mo)
```

---

## Cost Optimization Best Practices Checklist

### Compute
- [ ] Right-size instances based on actual utilization (target 60-75% CPU)
- [ ] Use reserved instances / committed use for baseline (1-3 year terms)
- [ ] Use spot/preemptible instances for fault-tolerant workloads (up to 90% savings)
- [ ] Implement auto-scaling with scale-down policies during off-peak
- [ ] Schedule non-production environments to stop outside business hours

### Storage & Data
- [ ] Enable lifecycle policies (move to cold storage after 30-90 days)
- [ ] Compress responses and enable CDN for static assets
- [ ] Use regional storage vs multi-region when DR isn't critical
- [ ] Delete unused snapshots and old container images

### Networking
- [ ] Minimize cross-region data transfer (keep services co-located)
- [ ] Use VPC endpoints / Private Link to avoid public egress charges
- [ ] Compress API responses (gzip/brotli)
- [ ] Use free tier egress providers (DigitalOcean, Cloudflare)

### Monitoring
- [ ] Set up billing alerts at 50%, 80%, and 100% thresholds
- [ ] Review cost anomaly reports weekly
- [ ] Tag all resources for cost attribution
- [ ] Run cost estimation scripts before scaling decisions

---

## Cross-References

- See [AWS Deep Dive](./01-aws-deep-dive.md) for AWS-specific deployment patterns
- See [Azure & GCP Deployment](./02-azure-gcp-deployment.md) for Azure and GCP details
- See [Architecture Patterns](../01-deployment-architecture/01-architecture-patterns.md) for system design
- See [Kubernetes Patterns](../03-container-orchestration/01-kubernetes-patterns.md) for K8s workloads
- See [CI/CD Pipelines](../05-ci-cd-pipelines/01-github-actions.md) for automation
- See [Infrastructure as Code](../06-infrastructure-as-code/01-terraform.md) for Terraform
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability setup

---

## Next Steps

Review the [Deployment Checklist](../12-deployment-best-practices/01-deployment-checklist.md) before going live.
