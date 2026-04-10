---
Category: Google Cloud Platform
Subcategory: GCP Fundamentals
Concept: GCP Core Infrastructure
Purpose: Understanding Google Cloud Platform's global infrastructure, regions, and zones
Difficulty: beginner
Prerequisites: Basic Cloud Computing, Basic AWS Equivalent
RelatedFiles: 02_Advanced_GCP_Infrastructure.md, 03_Practical_GCP_Infrastructure.md
UseCase: Deploying globally distributed applications on GCP
CertificationExam: Google Cloud Engineer / Professional Data Engineer
LastUpdated: 2025
---

## WHY

Google Cloud Platform offers unique infrastructure advantages built on Google's global private network that powers Google's own products serving billions of users. Understanding GCP infrastructure allows leveraging these advantages.

### Why GCP Infrastructure Differs

- **Google's Network**: Same network powering Google Search, YouTube, Gmail
- **Low Latency**: Custom hardware, software-defined networking
- **Global Footprint**: 40+ Regions, 100+ Zones globally
- **Sustainability**: Carbon neutral since 2007

### Key Distinctions from AWS

- **Zones**: Named differently; GCP uses "zones" more like AWS AZs
- **Regions**: Similar to AWS but fewer global services
- **Network**: Google's private global network
- **Default High Availability**: More services multi-region by default

## WHAT

### GCP Infrastructure Components

**Zone**: Individual data center within a region. Similar to AWS Availability Zone.

**Region**: Geographic area with multiple zones.

**Multi-region**: Geographic area spanning multiple regions (EU, US).

**Default Network**: Auto-created VPC per project.

### Regions and Zones

GCP operates in 40+ regions:

**Global Regions**:
- us-central1 (Iowa)
- us-east1 (South Carolina)
- europe-west1 (Belgium)
- asia-east1 (Taiwan)
- asia-northeast1 (Tokyo)

**Similar AWS Mapping**:
- us-central1 ≈ us-east-1
- us-west1 ≈ us-west-2
- europe-west1 ≈ eu-west-1
- asia-northeast1 ≈ ap-northeast-1

### Architecture Diagram

```
          GCP GLOBAL INFRASTRUCTURE
          =====================

    ┌──────────────────────────────────────┐
    │          GLOBAL RESOURCES              │
    │   Cloud CDN  │ IAM  │ Storage      │
    └──────────────────────────────────────┘
                    │
    ┌───────────────┬┴───────────────┐
    │               │               │
    ▼               ▼               ▼
┌──────────┐  ┌──────────┐  ┌──────────┐
│ Region A │  │ Region B │  │ Region C │
│          │  │          │  │          │
│ ┌────┐  │  │ ┌────┐  │  │ ┌────┐  │
│ │Zone│  │  │ │Zone│  │  │ │Zone│  │
│ │ A-a│  │  │ │ B-a│  │  │ │ C-a│  │
│ └────┘  │  │ └────┘  │  │ └────┘  │
│ ┌────┐  │  │ ┌────┐  │  │ ┌────┐  │
│ │Zone│  │  │ │Zone│  │  │ │Zone│  │
│ │ A-b│  │  │ │ B-b│  │  │ │ C-b│  │
│ └────┘  │  │ └────┘  │  │ └────┘  │
└──────────┘  └──────────┘  └───────��──┘
```

## HOW

### Example 1: Deploying in GCP Regions

```bash
# Set default region
gcloud config set compute/region us-central1
gcloud config set compute/zone us-central1-a

# List available regions
gcloud compute regions list

# List zones in a region
gcloud compute zones list --filter=region:us-central1

# Create instance in specific zone
gcloud compute instances create my-instance \
    --zone=us-central1-a \
    --machine-type=e2-medium \
    --image-family=debian-11 \
    --image-project=debian-cloud

# Create instance in different zone
gcloud compute instances create my-instance-2 \
    --zone=us-east1-b \
    --machine-type=e2-medium
```

### Example 2: Multi-Regional Deployment

```bash
# Get available regions for deployment
gcloud compute regions list --filter="status=UP"

# Check latency between zones using GCE
# Use internal IP for low latency communication

# Deploy load balancer across regions (managed instance groups)
gcloud compute instance-groups managed create europe-mig \
    --template=web-template \
    --size=3 \
    --zone=europe-west4-a

gcloud compute instance-groups managed create us-mig \
    --template=web-template \
    --size=3 \
    --zone=us-central1-a
```

### Example 3: Using Default Network

```bash
# GCP provides default network in new projects
# List networks
gcloud compute networks list

# List subnets in a project
gcloud compute networks subnets list

# Default network contains subnets in each region
# Check auto-created subnets
gcloud compute networks subnets list \
    --network=default
```

## GCP VS AWS COMPARISON

| Feature | GCP | AWS | Notes |
|---------|----|----|-------|
| Region | 40+ | 33+ | Similar global coverage |
| Zones | 100+ | 105+ | Similar concept |
| Default Network | Yes | No | GCP auto-creates |
| Network | Custom | Shared | Google's advantage |
| Preemptible | Yes | Spot | Similar discount |

## PERFORMANCE

### Latency Benefits

- **Internal Google Traffic**: <1ms between VMs
- **Cross-zone**: ~0.5-1ms
- **Cross-region**: 50-100ms typical

### Network Advantages

- Google's network is private (not internet)
- B4 network handles Google traffic
- Lower latency to end users

## CROSS-REFERENCES

### Prerequisites

- Basic cloud concepts
- Networking basics

### What to Study Next

1. GCP Resource Hierarchy
2. Compute Engine deep dive
3. GCP Networking