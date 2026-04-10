---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: VPC GCP
Purpose: Hands-on exercises for VPC networking configuration
Difficulty: advanced
Prerequisites: 01_Basic_VPC_GCP.md, 02_Advanced_VPC_GCP.md
RelatedFiles: 01_Basic_VPC_GCP.md, 02_Advanced_VPC_GCP.md
UseCase: Enterprise networking, hybrid cloud, private connectivity
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with VPC networking is essential for building secure, scalable GCP architectures and managing network connectivity.

### Lab Goals

- Configure VPC networks
- Set up VPC peering
- Implement Cloud NAT

## 📖 WHAT

### Exercise Overview

1. **VPC Creation**: Custom mode VPC
2. **Peering**: Cross-project connectivity
3. **NAT**: Private instance access

## 🔧 HOW

### Exercise 1: Configure Production VPC

```bash
#!/bin/bash
# Configure production VPC network

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create custom mode VPC
gcloud compute networks create prod-vpc \
    --subnet-mode=custom \
    --bgp-routing-mode=regional

# Create subnets in multiple regions
REGIONS=("us-central1" "us-east1" "europe-west1")
RANGES=("10.0.1.0/24" "10.0.2.0/24" "10.0.3.0/24")

for i in "${!REGIONS[@]}"; do
    gcloud compute networks subnets create "subnet-${i}" \
        --network=prod-vpc \
        --region="${REGIONS[$i]}" \
        --range="${RANGES[$i]}" \
        --enable-private-ip-google-access
done

# Enable VPC flow logs
gcloud compute networks subnets update subnet-0 \
    --region=us-central1 \
    --enable-flow-logs \
    --logging-flow-sampling=0.5 \
    --logging-metadata=include-all

echo "Production VPC configured!"
```

### Exercise 2: Configure VPC Peering

```bash
#!/bin/bash
# Configure VPC peering between two projects

PROJECT_A="project-a-id"
PROJECT_B="project-b-id"

# Project A: Create VPC
gcloud config set project $PROJECT_A
gcloud compute networks create vpc-a --subnet-mode=custom

gcloud compute networks subnets create subnet-a \
    --network=vpc-a \
    --region=us-central1 \
    --range=10.1.0.0/24

# Project B: Create VPC
gcloud config set project $PROJECT_B
gcloud compute networks create vpc-b --subnet-mode=custom

gcloud compute networks subnets create subnet-b \
    --network=vpc-b \
    --region=us-east1 \
    --range=10.2.0.0/24

# Project A: Create peering
gcloud config set project $PROJECT_A
gcloud compute networks peerings create peer-a-to-b \
    --network=vpc-a \
    --peer-network=vpc-b \
    --peer-project=$PROJECT_B

# Project B: Create peering
gcloud config set project $PROJECT_B
gcloud compute networks peerings create peer-b-to-a \
    --network=vpc-b \
    --peer-network=vpc-a \
    --peer-project=$PROJECT_A

# Verify peering
gcloud compute networks peerings list

echo "VPC peering configured!"
```

### Exercise 3: Configure Cloud NAT

```bash
#!/bin/bash
# Configure Cloud NAT for private instances

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Create VPC and subnet
gcloud compute networks create nat-vpc --subnet-mode=custom

gcloud compute networks subnets create nat-subnet \
    --network=nat-vpc \
    --region=us-central1 \
    --range=10.0.1.0/24 \
    --enable-private-ip-google-access

# Create Cloud Router
gcloud compute routers create nat-router \
    --network=nat-vpc \
    --region=us-central1

# Create NAT config
gcloud compute routers nats create nat-gateway \
    --router=nat-router \
    --region=us-central1 \
    --nat-all-subnet-ip-ranges \
    --enable-logging

# Create private instance (no external IP)
gcloud compute instances create private-vm \
    --zone=us-central1-a \
    --machine-type=e2-medium \
    --network-interface=subnet=nat-subnet,no-address

# Test connectivity from private instance
gcloud compute ssh private-vm --zone=us-central1-a \
    --internal-ip --command="curl https://www.google.com"

echo "Cloud NAT configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Peering fails | Check for overlapping CIDRs |
| No internet | Check NAT configuration |
| Private GCP access | Enable on subnet |

### Validation

```bash
# Check VPC
gcloud compute networks list

# Check peering status
gcloud compute networks peerings describe peer-name

# Check NAT mapping
gcloud compute routers get-nat-mapping-info nat-router
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Load Balancing
- Cloud VPN
- Cloud Interconnect

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud Load Balancing
- Cloud VPN
- Cloud Interconnect

### Next Steps

- Set up Cloud VPN
- Configure Cloud Load Balancer
- Implement VPC Flow Logs

## ✅ EXAM TIPS

- Practice VPC configuration
- Know peering requirements
- Understand NAT configuration
- Remember Private Google Access
