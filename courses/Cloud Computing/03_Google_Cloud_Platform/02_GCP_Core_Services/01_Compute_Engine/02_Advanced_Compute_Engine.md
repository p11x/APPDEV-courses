---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Compute Engine
Purpose: Advanced understanding of GCP Compute Engine virtual machines
Difficulty: intermediate
Prerequisites: 01_Basic_Compute_Engine.md
RelatedFiles: 01_Basic_Compute_Engine.md, 03_Practical_Compute_Engine.md
UseCase: Production VM deployments, specialized workloads, cost optimization
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced Compute Engine knowledge enables handling specialized workloads, optimizing costs, and ensuring high availability for production applications.

### Why Advanced Compute Engine

- **Machine Types**: Custom, shared, sole-tenant options
- **Cost Optimization**: Preemptible, committed use
- **High Availability**: Managed instance groups, live migration
- **Specialized Workloads**: GPUs, TPUs, custom images

## 📖 WHAT

### Machine Type Families

| Family | Use Case | vCPU:Memory Ratio |
|--------|----------|-------------------|
| General Purpose (E2, N2, N2D) | Standard workloads | 1:4 |
| Compute Optimized (C2) | Compute-intensive | 1:4 |
| Memory Optimized (M2, M1) | In-memory databases | 1:8+ |
| GPU | ML, rendering | Variable |

### Specialized Features

**Sole-Tenant Nodes**:
- Dedicated physical servers
- Regulatory compliance
- BYOL licensing benefits

**Preemptible VMs**:
- 60-91% discount
- Max 24 hours
- Can be preempted anytime

**Live Migration**:
- Zero downtime updates
- Maintenance events
- Automatic placement

## 🔧 HOW

### Example 1: Custom Machine Type with GPUs

```bash
# Create instance with custom machine type
gcloud compute instances create ml-instance \
    --zone=us-central1-a \
    --machine-type=custom-8-32768 \
    --image-family=tf-latest-gpu \
    --image-project=deep-learning-platform \
    --accelerator=type=nvidia-tesla-v100,count=2 \
    --boot-disk-size=100GB \
    --boot-disk-type=pd-ssd

# Verify GPU availability
gcloud compute accelerator-types list

# Install CUDA drivers
gcloud compute instances install-gpu-drivers ml-instance \
    --zone=us-central1-a
```

### Example 2: Managed Instance Groups with Autohealing

```bash
# Create instance template
gcloud compute instance-templates create app-template \
    --machine-type=e2-medium \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --metadata=startup-script='#!/bin/bash
apt-get update && apt-get install -y nginx'

# Create regional managed instance group
gcloud compute instance-groups managed create app-mig \
    --template=app-template \
    --size=3 \
    --region=us-central1 \
    --target-distribution-shape=balanced

# Create health check
gcloud compute health-checks create http app-health \
    --port=80 \
    --check-interval=30s \
    --timeout=5s \
    --healthy-threshold=2 \
    --unhealthy-threshold=3

# Apply autohealing
gcloud compute instance-groups managed update app-mig \
    --region=us-central1 \
    --health-check=app-health \
    --initial-delay=60s

# Add autoscaling
gcloud compute instance-groups managed set-autoscaling app-mig \
    --region=us-central1 \
    --mode=on \
    --min-num-replicas=2 \
    --max-num-replicas=10 \
    --cpu-utilization-threshold=75 \
    --metric=CPU_UTILIZATION
```

### Example 3: Sole-Tenant Node Configuration

```bash
# Reserve sole-tenant nodes
gcloud compute sole-tenancy node-groups create my-nodes \
    --zone=us-central1-a \
    --node-type=n2-node-8-32768 \
    --maintenance-policy=restart \
    --min-count=2

# Create template for sole-tenant
gcloud compute instance-templates create sole-tenant-template \
    --machine-type=n2-standard-8 \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --node-affinity-file=node-affinity.json

# Create instance on sole-tenant nodes
gcloud compute instances create sole-tenant-vm \
    --zone=us-central1-a \
    --instance-ids=my-nodes \
    --template=sole-tenant-template

# Verify node allocation
gcloud compute sole-tenancy node-groups describe my-nodes \
    --zone=us-central1-a
```

## ⚠️ COMMON ISSUES

### Troubleshooting VM Issues

| Issue | Solution |
|-------|----------|
| Instance creation fails | Check quotas, machine type availability |
| GPU not recognized | Install GPU drivers |
| Preemptible instance terminated | Expected behavior, design for restart |
| Live migration failed | Check maintenance events |

### Cost Optimization

- Use preemptible for fault-tolerant workloads
- Use committed use for baseline capacity
- Right-size with monitoring data

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP | AWS | Azure |
|---------|-----|-----|-------|
| Custom Machine Types | Yes | Yes | Limited |
| Preemptible/Spot | Yes | Yes | Yes |
| Live Migration | Yes | No | Limited |
| Sole-Tenant | Yes | Dedicated Hosts | Dedicated Hosts |
| GPUs | Yes | Yes | Yes |

## 🔗 CROSS-REFERENCES

### Related Topics

- Managed Instance Groups
- Autoscaling
- GPU/TPU Instances

### Study Resources

- Compute Engine documentation
- Machine types comparison

## ✅ EXAM TIPS

- Custom machine types: specify vCPUs and memory
- Preemptible = 60-91% discount, max 24 hours
- Live Migration = no downtime
- Managed instance groups for HA
- GPU drivers must be installed manually
