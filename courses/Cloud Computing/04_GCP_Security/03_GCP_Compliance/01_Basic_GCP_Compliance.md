---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: GCP Compliance
Purpose: Understanding GCP compliance programs and certifications
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_GCP_Compliance.md, 03_Practical_GCP_Compliance.md
UseCase: Understanding GCP compliance, certifications, and regulatory requirements
CertificationExam: GCP Associate Cloud Engineer / Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

GCP maintains extensive compliance certifications. Understanding compliance helps ensure applications meet regulatory requirements.

## 📖 WHAT

### Major Certifications

| Certification | Description |
|---------------|-------------|
| ISO 27001 | Information security |
| SOC 1/2/3 | Service organization controls |
| HIPAA | Healthcare data |
| PCI DSS | Payment card data |
| FedRAMP | US government |
| GDPR | EU data protection |

### Compliance Programs

- **Regular Audits**: Independent assessments
- **Artifact Repository**: Compliance reports
- **Shared Responsibility**: Customer vs GCP

## 🔧 HOW

### Example: Check Compliance

```bash
# View compliance certifications
gcloud artifacts repositories list

# Access compliance reports
# Visit: https://cloud.google.com/security/compliance

# Enable Access Transparency
gcloud organization policies enable-access-transparency --organization=ORG_ID

# Enable Audit Logs
gcloud logging sinks create my-sink storage.googleapis.com/my-bucket
```

## ✅ EXAM TIPS

- GCP manages physical security
- Customer manages data access
- Audit logs available
- Compliance reports in Artifact Registry
