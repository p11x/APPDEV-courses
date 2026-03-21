# VPA and KEDA

## Overview

While HPA handles horizontal scaling (more pods), Vertical Pod Autoscaler (VPA) adjusts pod resource requests (bigger pods), and KEDA enables event-driven scaling from external sources like message queues. Together, these provide comprehensive autoscaling strategies for diverse workload patterns.

## Prerequisites

- Understanding of HPA
- Knowledge of Kubernetes custom resources

## Vertical Pod Autoscaler (VPA)

### How VPA Works

1. Analyzes actual resource usage of running pods
2. Recommends or automatically applies new resource requests
3. Evicts and recreates pods with updated requests

### VPA Modes

- **Auto**: Updates resources and evicts pods
- **Recreate**: Updates resources and evicts pods (same as Auto)
- **Initial**: Sets resources only at pod creation
- **Off**: Only provides recommendations, no changes

### Installing VPA

```bash
# Install VPA components
kubectl apply -f https://github.com/kubernetes/autoscaler/releases/latest/download/vertical-pod-autoscaler.yaml

# Check VPA pods
kubectl get pods -n kube-system | grep vpa
```

### Creating VPA

```yaml
apiVersion: autoscaling.k8s.io/v1
kind: VerticalPodAutoscaler
metadata:
  name: web-app-vpa
spec:
  targetRef:
    apiVersion: "apps/v1"
    kind: Deployment
    name: web-app
  updatePolicy:
    updateMode: "Auto"    # Auto, Recreate, Initial, Off
  resourcePolicy:
    containerPolicies:
    - containerName: web
      minAllowed:
        memory: "64Mi"
        cpu: "50m"
      maxAllowed:
        memory: "4Gi"
        cpu: "2"
      controlledResources: ["cpu", "memory"]
```

```bash
# Apply VPA
kubectl apply -f vpa.yaml

# Get recommendations
kubectl get vpa web-app-vpa -o yaml
# Shows: recommendation: {containerRecommendations: {...}}
```

### VPA Recommendations

```bash
# View VPA recommendations
kubectl describe vpa web-app-vpa

# Output shows:
# Recommendation:
#   Container: web
#     Target: 256Mi / 200m
#     Lower Bound: 128Mi / 100m
#     Upper Bound: 512Mi / 400m
```

## KEDA - Event-Driven Autoscaling

### How KEDA Works

- Monitors external event sources (Prometheus, Kafka, RabbitMQ, AWS SQS, etc.)
- Scales pods from 0 to N based on event queue depth
- Scales to 0 when no events (cost saving)

### Installing KEDA

```bash
# Install KEDA using Helm
# KEDA adds custom resources for event-driven scaling
helm repo add kedacore https://kedacore.github.io/charts
helm install keda kedacore/keda --namespace keda --create-namespace
```

### KEDA ScaledObject - Prometheus Example

```yaml
apiVersion: keda.sh/v1alpha1
kind: ScaledObject
metadata:
  name: web-app-scaler
spec:
  scaleTargetRef:
    name: web-app              # Target deployment
  pollingInterval: 15         # Check every 15 seconds
  cooldownPeriod: 300         # Wait 5 min before scaling to 0
  minReplicaCount: 0          # Can scale to 0
  maxReplicaCount: 10
  triggers:
  - type: prometheus
    metadata:
      serverAddress: http://prometheus:9090
      metricName: http_requests_per_second
      threshold: "100"
      query: sum(rate(http_requests_total[2m]))
```

### KEDA ScaledObject - RabbitMQ Example

```yaml
apiVersion: keda.sh/v1alpha1
kind: ScaledObject
metadata:
  name: worker-scaler
spec:
  scaleTargetRef:
    name: message-worker
  pollingInterval: 5
  cooldownPeriod: 60
  minReplicaCount: 0
  maxReplicaCount: 5
  triggers:
  - type: rabbitmq
    metadata:
      queueName: tasks
      queueLengthTarget: "10"
      host: amqp://guest:guest@rabbitmq:5672
```

### KEDA ScaledObject - Kafka Example

```yaml
apiVersion: keda.sh/v1alpha1
kind: ScaledObject
metadata:
  name: kafka-consumer-scaler
spec:
  scaleTargetRef:
    name: kafka-consumer
  pollingInterval: 30
  cooldownPeriod: 300
  minReplicaCount: 1
  maxReplicaCount: 10
  triggers:
  - type: kafka
    metadata:
      bootstrapServers: kafka:9092
      consumerGroup: my-group
      topic: my-topic
      lagThreshold: "100"
```

### Checking KEDA ScaledObject

```bash
# List ScaledObjects
kubectl get scaledobject

# Check scaled object status
kubectl get scaledobject web-app-scaler -o yaml

# Get ScaledJobs (when minReplicas: 0)
kubectl get scaledjob
```

## Comparing Scaling Methods

| Method | What it Scales | Use Case |
|--------|---------------|----------|
| HPA | Pods (horizontal) | Traffic-based |
| VPA | Resources (vertical) | Right-sizing |
| KEDA | Pods (event-driven) | Queue-based processing |
| Cluster Autoscaler | Nodes | Cluster capacity |

## Combining VPA and HPA

```yaml
# VPA handles resources, HPA handles replicas
# Use both for complete optimization

# VPA - adjusts memory/CPU requests
apiVersion: autoscaling.k8s.io/v1
kind: VerticalPodAutoscaler
metadata:
  name: web-app-vpa
spec:
  targetRef:
    apiVersion: "apps/v1"
    kind: Deployment
    name: web-app
  updatePolicy:
    updateMode: "Initial"  # Set once at creation
  resourcePolicy:
    containerPolicies:
    - containerName: web
      controlledResources: ["cpu", "memory"]

---
# HPA - scales based on actual demand
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: web-app-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: web-app
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
```

## Common Mistakes

- **VPA and HPA conflict**: Both managing same deployment can cause issues
- **VPA eviction**: Frequent evictions disrupt workload
- **KEDA polling too fast**: Can overwhelm external systems
- **Missing authentication**: KEDA triggers need credentials

## Quick Reference

| Tool | Installation | Scale Target |
|------|--------------|--------------|
| VPA | From kubernetes/autoscaler repo | Pod resources |
| KEDA | Helm chart | Pods based on events |

| VPA Mode | Behavior |
|----------|----------|
| Auto | Updates and evicts |
| Off | Recommendations only |
| Initial | Sets at creation |

| KEDA Trigger | Example Source |
|--------------|-----------------|
| prometheus | Metrics |
| kafka | Message queue |
| rabbitmq | Message queue |
| aws-sqs | AWS queue |

## What's Next

Continue to [kubectl logs and events](./01-kubectl-logs-and-events.md) for observability.
