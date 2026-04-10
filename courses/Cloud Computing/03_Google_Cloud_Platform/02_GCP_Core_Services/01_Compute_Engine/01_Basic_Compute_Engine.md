---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Compute Engine
Purpose: Understanding GCP Compute Engine virtual machines
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Compute_Engine.md, 03_Practical_Compute_Engine.md
UseCase: Running virtual machines on GCP
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Compute Engine provides virtual machines in Google's global infrastructure. Understanding GCP compute options is essential.

## 📖 WHAT

### Compute Engine Features

- **Custom Machine Types**: Tailor vCPU/memory
- **Preemptible VMs**: Low-cost interruptible
- **Sole-Tenant Nodes**: Dedicated hardware
- **Live Migration**: Zero-downtime updates
- **GPU Support**: GPU and TPU acceleration

## 🔧 HOW

### Example: Create Instance

```bash
# Create instance
gcloud compute instances create my-instance \
    --zone=us-central1-a \
    --machine-type=e2-medium \
    --image-family=debian-11 \
    --image-project=debian-cloud

# List instances
gcloud compute instances list
```

## ✅ EXAM TIPS

- GCE = IaaS virtual machines
- Live Migration reduces maintenance impact
- Preemptible = 60-91% discount