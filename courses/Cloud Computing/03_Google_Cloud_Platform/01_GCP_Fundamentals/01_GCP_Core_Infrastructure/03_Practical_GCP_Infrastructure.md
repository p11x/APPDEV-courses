---
Category: Google Cloud Platform
Subcategory: GCP Fundamentals
Concept: GCP Core Infrastructure
Purpose: Hands-on exercises for deploying and managing GCP infrastructure
Difficulty: advanced
Prerequisites: 01_Basic_GCP_Infrastructure.md, 02_Advanced_GCP_Infrastructure.md
RelatedFiles: 01_Basic_GCP_Infrastructure.md, 02_Advanced_GCP_Infrastructure.md
UseCase: Production deployments, disaster recovery, cost optimization on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Practical experience with GCP infrastructure is essential for passing the Cloud Engineer exam and managing production workloads.

### Hands-on Learning Goals

- Deploy resources across multiple zones/regions
- Implement high-availability architectures
- Configure disaster recovery procedures
- Optimize for cost and performance

## 📖 WHAT

### Lab Exercises Overview

1. **Zone-based Deployment**: Deploy VMs in multiple zones
2. **Regional HA**: Implement multi-zone failover
3. **Global Load Balancing**: Distribute traffic worldwide
4. **Disaster Recovery**: Configure backups and recovery

### Expected Outcomes

- Ability to design resilient architectures
- Understanding of HA patterns
- Cost-effective resource deployment

## 🔧 HOW

### Exercise 1: Deploy Multi-zone Application

```bash
#!/bin/bash
# Script to deploy application across multiple zones

# Configuration
PROJECT_ID="my-project-id"
REGIONS=("us-central1" "us-east1" "europe-west1")
ZONES=("us-central1-a" "us-east1-b" "europe-west1-a")

# Set project
gcloud config set project $PROJECT_ID

# Create instance template
gcloud compute instance-templates create app-template \
    --machine-type=e2-medium \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --metadata=startup-script='#!/bin/bash
apt-get update
apt-get install -y nginx
echo "App running on $(hostname)" > /var/www/html/index.html'

# Deploy to each region
for i in "${!REGIONS[@]}"; do
    region="${REGIONS[$i]}"
    zone="${ZONES[$i]}"
    
    echo "Deploying to $region ($zone)..."
    
    gcloud compute instance-groups managed create app-mig-$i \
        --template=app-template \
        --size=2 \
        --zone=$zone
done

echo "Deployment complete!"
```

### Exercise 2: Configure Global HTTPS Load Balancer

```bash
#!/bin/bash
# Global HTTPS Load Balancer setup

PROJECT_ID="my-project-id"
gcloud config set project $PROJECT_ID

# Create VPC
gcloud compute networks create app-vpc \
    --subnet-mode=custom

# Create subnets in multiple regions
for region in us-central1 us-east1 europe-west1; do
    gcloud compute networks subnets create app-subnet-$region \
        --network=app-vpc \
        --region=$region \
        --range=10.$(echo $region | sed 's/us-central1/1/g; s/us-east1/2/g; s/europe-west1/3/g').0.0/24
done

# Create health check
gcloud compute health-checks create https app-health-check \
    --port=443 \
    --request-path=/health

# Create backend service
gcloud compute backend-services create app-backend \
    --protocol=HTTPS \
    --port-name=https \
    --health-checks=app-health-check \
    --enable-cdn

# Add backends from each region
gcloud compute backend-services add-backend app-backend \
    --instance-group=app-mig-0 \
    --instance-group-region=us-central1

gcloud compute backend-services add-backend app-backend \
    --instance-group=app-mig-1 \
    --instance-group-region=us-east1

# Create SSL certificate
gcloud compute ssl-certificates create app-ssl-cert \
    --certificate=server.crt \
    --private-key=server.key

# Create target HTTPS proxy
gcloud compute target-https-proxies create app-https-proxy \
    --url-map=app-url-map \
    --ssl-certificates=app-ssl-cert

# Create global forwarding rule
gcloud compute forwarding-rules create app-https-lb \
    --global \
    --target-https-proxies=app-https-proxy \
    --ports=443

echo "Global HTTPS Load Balancer configured!"
```

### Exercise 3: Disaster Recovery with Cross-region Replication

```bash
#!/bin/bash
# Disaster Recovery configuration

PROJECT_ID="my-project-id"
PRIMARY_REGION="us-central1"
DR_REGION="europe-west1"

gcloud config set project $PROJECT_ID

# Create snapshot schedule
gcloud compute resource-policies create snapshot-schedule daily-backup \
    --schedule="0 3 * * *" \
    --retention-days=30 \
    --region=$PRIMARY_REGION

# List disks in primary region
DISKS=$(gcloud compute disks list --region=$PRIMARY_REGION --format="value(name)")

for disk in $DISKS; do
    echo "Adding backup policy to $disk..."
    gcloud compute disks add-resource-policies $disk \
        --resource-policies=daily-backup \
        --region=$PRIMARY_REGION
done

# Create custom image from primary disk
gcloud compute images create app-recovery-image \
    --source-disk=app-disk \
    --source-disk-zone=$PRIMARY_REGION-a \
    --family=app-recovery

# Copy image to DR region
gcloud compute images create app-recovery-image-dr \
    --source-image=app-recovery-image \
    --storage-location=EU

# Create instance in DR region from image
gcloud compute instances create app-dr-instance \
    --zone=$DR_REGION-a \
    --machine-type=e2-medium \
    --image=app-recovery-image-dr

echo "DR configuration complete!"
echo "RTO: ~15 minutes (instance creation time)"
echo "RPO: 24 hours (snapshot frequency)"
```

## ⚠️ COMMON ISSUES

### Troubleshooting Guide

| Issue | Solution |
|-------|----------|
| Instance won't start | Check quota, machine type availability |
| Can't connect to instances | Verify firewall rules, VPC config |
| High latency | Choose closer region, use CDN |
| Cross-region connection fails | Check VPC peering, firewall |

### Validation Commands

```bash
# Verify instance deployment
gcloud compute instances list --format="table(name,zone,status)"

# Check regional resources
gcloud compute instance-groups managed list --regions=us-central1

# Verify load balancer health
gcloud compute backend-services get-health app-backend
```

## 🌐 COMPATIBILITY

### Integration Points

- Works with Cloud CDN for caching
- Integrates with Cloud Armor for security
- Compatible with Cloud Monitoring

### Regional Availability

All exercises work in all GCP regions. Some features:
- Global HTTP(S) load balancing: All regions
- Managed instance groups: All regions
- Snapshot schedules: Regional

## 🔗 CROSS-REFERENCES

### Related Labs

- VPC Network Configuration
- Cloud Load Balancing
- Cloud Monitoring Setup

### Next Steps

- Implement autoscaling
- Add Cloud CDN
- Configure Cloud Armor WAF

## ✅ EXAM TIPS

- Practice gcloud commands for all scenarios
- Know how to check resource status
- Understand regional vs global resources
- Remember to clean up resources to avoid charges
