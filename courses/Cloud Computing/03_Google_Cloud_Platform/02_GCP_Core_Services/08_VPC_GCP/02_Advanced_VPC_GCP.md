---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: VPC GCP
Purpose: Advanced understanding of GCP VPC networking features
Difficulty: intermediate
Prerequisites: 01_Basic_VPC_GCP.md
RelatedFiles: 01_Basic_VPC_GCP.md, 03_Practical_VPC_GCP.md
UseCase: Enterprise networking, hybrid cloud, security
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Advanced VPC knowledge enables building complex network architectures, implementing hybrid cloud connectivity, and securing network traffic.

### Why Advanced VPC

- **Shared VPC**: Cross-project networking
- **VPC Peering**: Private connectivity
- **Cloud NAT**: Outbound internet access
- **Private Google Access**: GCP API access

## 📖 WHAT

### Advanced Networking Features

**Shared VPC**:
- Organization-level configuration
- Host project provides networks
- Service projects use networks

**VPC Network Peering**:
- Private network connection
- No data traverses internet
- No external IP needed

### Traffic Management

| Feature | Purpose |
|---------|---------|
| Cloud NAT | NAT for private instances |
| Cloud CDN | Content delivery |
| Cloud Load Balancing | Traffic distribution |
| VPC Flow Logs | Network monitoring |

## 🔧 HOW

### Example 1: Shared VPC Configuration

```bash
# Create host project
gcloud projects create host-project --name="Host Project"

# Enable Shared VPC
gcloud compute shared-vpc enable host-project

# Configure host project subnets
gcloud compute networks subnets create subnet-host \
    --network=my-vpc \
    --region=us-central1 \
    --range=10.0.1.0/24 \
    --enable-private-ip-google-access

# Attach service project
gcloud compute shared-vpc associated-projects add service-project \
    --host-project=host-project

# Grant network user role to service project
gcloud projects add-iam-policy-binding host-project \
    --member=serviceAccount:service-project@host-project.iam.gserviceaccount.com \
    --role=roles/compute.networkUser
```

### Example 2: VPC Peering Setup

```bash
# Create VPC in project A
gcloud compute networks create vpc-a \
    --subnet-mode=custom

# Create VPC in project B
gcloud compute networks create vpc-b \
    --subnet-mode=custom

# Create subnet in each VPC
gcloud compute networks subnets create subnet-a \
    --network=vpc-a \
    --region=us-central1 \
    --range=10.1.0.0/24

gcloud compute networks subnets create subnet-b \
    --network=vpc-b \
    --region=us-east1 \
    --range=10.2.0.0/24

# Create VPC peering (from project A)
gcloud compute networks peerings create peering-a-b \
    --network=vpc-a \
    --peer-network=vpc-b \
    --peer-project=project-b-id

# Create VPC peering (from project B)
gcloud compute networks peerings create peering-b-a \
    --network=vpc-b \
    --peer-network=vpc-a \
    --peer-project=project-a-id
```

### Example 3: Cloud NAT Configuration

```bash
# Create Cloud Router
gcloud compute routers create nat-router \
    --network=my-vpc \
    --region=us-central1

# Create NAT configuration
gcloud compute routers nats create nat-config \
    --router=nat-router \
    --region=us-central1 \
    --nat-all-subnet-ip-ranges \
    --enable-logging

# Create instance without external IP
gcloud compute instances create private-instance \
    --zone=us-central1-a \
    --machine-type=e2-medium \
    --network-interface=subnet=subnet-a,no-address

# Test NAT connectivity
gcloud compute ssh private-instance \
    --zone=us-central1-a \
    --internal-ip

# Verify NAT is working
gcloud compute routers get-nat-mapping-info nat-router \
    --region=us-central1
```

## ⚠️ COMMON ISSUES

### Troubleshooting VPC Issues

| Issue | Solution |
|-------|----------|
| No internet access | Check Cloud NAT |
| Peering fails | Check overlapping CIDRs |
| Private Google Access | Enable on subnet |

### Best Practices

- Use custom mode VPC for production
- Enable Private Google Access
- Use Cloud NAT for private instances

## 🌐 COMPATIBILITY

### Cross-Platform Comparison

| Feature | GCP VPC | AWS VPC | Azure VNet |
|---------|---------|---------|------------|
| Shared VPC | Yes | No | Yes |
| VPC Peering | Yes | Yes | Yes |
| Cloud NAT | Yes | NAT Gateway | NAT Gateway |
| Flow Logs | Yes | Flow Logs | NSG Flow Logs |

## 🔗 CROSS-REFERENCES

### Related Topics

- Cloud Load Balancing
- Cloud VPN
- Cloud Interconnect

### Study Resources

- VPC documentation
- Networking best practices

## ✅ EXAM TIPS

- VPC is global, subnets are regional
- Shared VPC for organization networks
- VPC Peering for private connectivity
- Cloud NAT for private instance internet
