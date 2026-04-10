---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: VPC GCP
Purpose: Understanding GCP Virtual Private Cloud networking
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_VPC_GCP.md, 03_Practical_VPC_GCP.md
UseCase: Isolated network environments on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

VPC networks provide isolated environments on GCP. Understanding VPC is essential for GCP network architecture.

## 📖 WHAT

### VPC Features

- **Auto Mode**: Creates subnets in each region
- **Custom Mode**: Full subnet control
- **Shared VPC**: Cross-project networking
- **Private Google Access**: Access GCP APIs without internet
- **Cloud NAT**: NAT for private instances

## 🔧 HOW

### Example: Create Network

```bash
# Create VPC
gcloud compute networks create my-vpc \
    --subnet-mode=custom

# Create subnet
gcloud compute networks subnets create my-subnet \
    --network my-vpc \
    --region us-central1 \
    --range 10.0.0.0/24

# Enable Private Google Access
gcloud compute networks subnets update my-subnet \
    --region us-central1 \
    --enable-private-ip-google-access
```

## ✅ EXAM TIPS

- Auto mode creates subnets in all regions
- Shared VPC for organization-wide networking
- VPC is global, subnets are regional