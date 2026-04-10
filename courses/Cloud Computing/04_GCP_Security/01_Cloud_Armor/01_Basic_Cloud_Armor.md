---
Category: Google Cloud Platform
Subcategory: GCP Security
Concept: Cloud Armor
Purpose: Understanding GCP Cloud Armor WAF and DDoS protection
Difficulty: intermediate
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_Cloud_Armor.md, 03_Practical_Cloud_Armor.md
UseCase: Security protection for GCP workloads
CertificationExam: GCP Professional Security Engineer
LastUpdated: 2025
---

## 💡 WHY

Cloud Armor provides DDoS protection and WAF capabilities on GCP. Understanding Cloud Armor helps secure web applications.

## 📖 WHAT

### Cloud Armor Features

- **DDoS Protection**: Layer 3-7 mitigation
- **WAF Rules**: OWASP, SQLi, XSS protection
- **IP Allow/Deny**: Geographic blocking
- **Rate Limiting**: Protect against traffic spikes
- **Security Policies**: Attach to backend services

## 🔧 HOW

### Example: Create Security Policy

```bash
# Create security policy
gcloud compute security-policies create my-policy \
    --description "My security policy"

# Add WAF rule
gcloud compute security-policies rules create 1000 \
    --security-policy my-policy \
    --expression "evaluatePreconfiguredExpr('xss-v33-stable')" \
    --action "deny-403" \
    --description "Block XSS"

# Attach to backend service
gcloud compute backend-services update my-backend \
    --security-policy my-policy
```

## ✅ EXAM TIPS

- Layer 7 WAF protection
- Preconfigured rules for common attacks
- Geo-based access control