---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Compute Engine
Purpose: Hands-on exercises for deploying and managing Compute Engine VMs
Difficulty: advanced
Prerequisites: 01_Basic_Compute_Engine.md, 02_Advanced_Compute_Engine.md
RelatedFiles: 01_Basic_Compute_Engine.md, 02_Advanced_Compute_Engine.md
UseCase: Production VM deployments, autoscaling, high-availability configurations
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Compute Engine is essential for managing production workloads and passing the GCP Cloud Engineer exam.

### Lab Goals

- Deploy scalable VM infrastructure
- Configure high availability
- Implement cost optimization
- Handle specialized workloads

## 📖 WHAT

### Exercise Overview

1. **Scalable Deployment**: Managed instance groups
2. **HA Configuration**: Health checks, autohealing
3. **Cost Optimization**: Preemptible VMs
4. **Specialized Workloads**: GPUs

## 🔧 HOW

### Exercise 1: Deploy Scalable Web Application

```bash
#!/bin/bash
# Deploy scalable web application with MIG

PROJECT_ID="my-project-id"
gcloud config set project $PROJECT_ID

# Create startup script
cat > startup.sh << 'EOF'
#!/bin/bash
apt-get update
apt-get install -y nginx php-fpm
echo "<?php phpinfo(); ?>" > /var/www/html/info.php
echo "Web server configured on $(hostname)" > /var/www/html/index.html
EOF

# Create instance template
gcloud compute instance-templates create web-template \
    --machine-type=e2-medium \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --metadata=startup-script="$(cat startup.sh)"

# Create regional MIG
gcloud compute instance-groups managed create web-mig \
    --template=web-template \
    --size=3 \
    --region=us-central1 \
    --target-distribution-shape=balanced

# Create health check
gcloud compute health-checks create http web-health \
    --port=80 \
    --check-interval=30s \
    --timeout=5s

# Configure autoscaling
gcloud compute instance-groups managed set-autoscaling web-mig \
    --region=us-central1 \
    --mode=on \
    --min-num-replicas=2 \
    --max-num-replicas=10 \
    --cpu-utilization-threshold=70

echo "Scalable web application deployed!"
```

### Exercise 2: Configure High Availability

```bash
#!/bin/bash
# High availability configuration

PROJECT_ID="my-project-id"
gcloud config set project $PROJECT_ID

# Create health check
gcloud compute health-checks create http ha-health \
    --port=80 \
    --request-path=/health \
    --check-interval=15s \
    --timeout=5s \
    --healthy-threshold=2 \
    --unhealthy-threshold=3

# Create firewall rule for health checks
gcloud compute firewall-rules create allow-health-check \
    --network=default \
    --allow=tcp:80 \
    --source-ranges=130.211.0.0/22,35.191.0.0/16

# Create backend service
gcloud compute backend-services create web-backend \
    --protocol=HTTP \
    --port-name=http \
    --health-checks=ha-health

# Add MIG as backend
gcloud compute backend-services add-backend web-backend \
    --instance-group=web-mig \
    --instance-group-region=us-central1 \
    --balancing-mode=UTILIZATION \
    --max-utilization=0.8

# Configure autohealing
gcloud compute instance-groups managed update app-mig \
    --region=us-central1 \
    --health-check=ha-health \
    --initial-delay=60s

echo "HA configuration complete!"
```

### Exercise 3: Cost Optimization with Preemptible VMs

```bash
#!/bin/bash
# Cost-optimized deployment using preemptible VMs

PROJECT_ID="my-project-id"
gcloud config set project $PROJECT_ID

# Create preemptible instance template
gcloud compute instance-templates create batch-template \
    --machine-type=e2-medium \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --preemptible \
    --metadata=startup-script='#!/bin/bash
echo "Batch job started at $(date)" > /tmp/job.log
sleep 300
echo "Batch job completed at $(date)" > /tmp/job.log'

# Create regional MIG with preemptible VMs
gcloud compute instance-groups managed create batch-mig \
    --template=batch-template \
    --size=5 \
    --region=us-central1

# Add autoscaling for batch processing
gcloud compute instance-groups managed set-autoscaling batch-mig \
    --region=us-central1 \
    --mode=on \
    --min-num-replicas=1 \
    --max-num-replicas=20 \
    --metric=CLOUD_CPU-utilization \
    --cpu-utilization-threshold=50 \
    --scale-in-policy=fixed-or-percent

# Create scheduled job for batch processing
gcloud scheduler jobs create http batch-job \
    --schedule="0 2 * * *" \
    --uri="https://internal-ip/health" \
    --description="Daily batch job"

# Set up Cloud Monitoring for cost tracking
gcloud monitoring policies create \
    --display-name="Preemptible VM Cost" \
    --condition-display-name="Preemptible usage" \
    --condition-filter="resource.type = \"gce_instance\" AND metric.preemptible_vm = true" \
    --condition-threshold-value=80 \
    --condition-threshold-duration=300s

echo "Cost-optimized batch processing configured!"
echo "Estimated savings: 60-80%"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Health check failures | Check firewall rules, service status |
| MIG creation fails | Verify quota, template |
| Autoscaling not working | Check metrics, health check |
| Preemptible terminated early | Design for fault tolerance |

### Verification Commands

```bash
# Check MIG status
gcloud compute instance-groups managed describe web-mig --region=us-central1

# View autohealing status
gcloud compute instance-groups managed get-named-ports web-mig --region=us-central1

# Check preemptible VMs
gcloud compute instances list --filter="scheduling.preemptible=true"
```

## 🌐 COMPATIBILITY

### Integration

- Works with all load balancers
- Compatible with Cloud Monitoring
- Integrates with Cloud Logging

### Regional Availability

All features available in all GCP regions.

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Load Balancing
- Cloud Monitoring
- VPC Networking

### Next Steps

- Add Cloud CDN
- Configure Cloud Armor
- Set up Cloud Logging

## ✅ EXAM TIPS

- Practice gcloud commands for MIG
- Know health check configuration
- Understand preemptible VM behavior
- Remember to clean up resources
