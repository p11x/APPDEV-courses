---
Category: AWS Cloud Practitioner
Subcategory: AWS Billing
Concept: Support Plans
Purpose: Understanding AWS support plans and their features
Difficulty: beginner
Prerequisites: 01_Basic_Pricing_Models.md
RelatedFiles: 01_Basic_Pricing_Models.md, 02_Advanced_Pricing_Models.md
UseCase: Selecting appropriate AWS support plan
CertificationExam: AWS Cloud Practitioner
LastUpdated: 2025
---

## 💡 WHY

AWS Support plans provide different levels of technical assistance and resources. Understanding the options helps choose the right support for your needs.

### Why Support Plans Matter

- **Technical Help**: Access to AWS expertise
- **Response Time**: SLA guarantees for issues
- **Tools**: Additional troubleshooting resources
- **Cost**: Included in overall AWS spend

## 📖 WHAT

### Support Plan Tiers

| Plan | Price | Response Time | Use Case |
|------|-------|---------------|----------|
| Basic | Free | Community only | Development |
| Developer | $29/mo | 24-72 hours | Small teams |
| Business | $100/mo | 1-24 hours | Production |
| Enterprise | $15,000/mo | 15 min - 24 hours | Enterprise |

### Plan Comparison

**Basic (Free)**:
- Customer Service & Communities
- AWS Documentation & Whitepapers
- AWS Support Center

**Developer ($29/month)**:
- All Basic features
- Email support
- Business hours (8am-5pm)
- Response: 24-72 hours

**Business ($100/month)**:
- All Developer features
- Email, chat, phone
- 24/7 support
- Response: 1-24 hours
- AWS Trusted Advisor (7 checks)
- AWS Health Dashboard

**Enterprise ($15,000/month)**:
- All Business features
- Designated Technical Account Manager (TAM)
- 15-minute response (critical)
- Architecture guidance
- Concierge support
- Quarterly business review

### Architecture Diagram

```
Support Plan Features
======================

┌────────────────────────────────────────┐
│           Enterprise ($15K/mo)          │
│  ┌──────────────────────────────────┐  │
│  │ TAM, Concierge, 15min response   │  │
│  │ Architecture reviews, QBR        │  │
│  └──────────────────────────────────┘  │
├────────────────────────────────────────┤
│            Business ($100/mo)          │
│  ┌──────────────────────────────────┐  │
│  │ 24/7 phone/chat/email            │  │
│  │ 1-24hr response, Trusted Advisor │  │
│  │ Full AWS Health                   │  │
│  └──────────────────────────────────┘  │
├────────────────────────────────────────┤
│            Developer ($29/mo)          │
│  ┌──────────────────────────────────┐  │
│  │ Email, business hours            │  │
│  │ 24-72hr response                 │  │
│  └──────────────────────────────────┘  │
├────────────────────────────────────────┤
│               Basic (Free)              │
│  ┌──────────────────────────────────┐  │
│  │ Community, Documentation         │  │
│  │ Support Center access            │  │
│  └──────────────────────────────────┘  │
└────────────────────────────────────────┘
```

## 🔧 HOW

### Example 1: View Current Plan

```bash
# Describe support plan
aws support describe-severity-levels

# Get support info
aws support describe-cases \
    --after-time 2024-01-01T00:00:00Z \
    --max-results 10
```

### Example 2: Create Support Case

```bash
# Create case
aws support create-case \
    --subject "EC2 instance not starting" \
    --service-code ec2 \
    --category-code instance \
    --severity low \
    --description "Getting error when starting EC2 instance" \
    --language en

# Add communication to case
aws support add-communication-to-case \
    --case-id "case-id" \
    --communication-body "Additional information about the issue"
```

### Example 3: Trusted Advisor Checks

```bash
# Get Trusted Advisor checks (Business + only)
aws support describe-trusted-advisor-checks \
    --language en

# Get specific check result
aws support describe-trusted-advisor-check-result \
    --check-id "check-id" \
    --language en

# Common checks:
# - Service Limits
# - Security Groups - Specific Ports Unrestricted
# - IAM Use
# - MFA on Root Account
# - EBS Public Snapshots
# - RDS Public Snapshots
```

## ⚠️ COMMON ISSUES

### 1. Wrong Plan Selected

**Problem**: Support not meeting needs.

**Solution**: Assess team size, production needs, SLA requirements.

### 2. Severity Level Unknown

**Problem**: Don't know which severity to use.

**Solution**: Production down = urgent, general questions = normal.

### 3. Support Not Responding

**Problem**: No response within SLA.

**Solution**: Escalate through account rep, check case severity.

## 🏃 PERFORMANCE

### Severity Response Times

| Severity | Enterprise | Business | Developer |
|----------|------------|----------|-----------|
| Urgent | 15 min | 1 hour | N/A |
| High | 1 hour | 4 hours | 24 hours |
| Normal | 4 hours | 12 hours | 48 hours |
| Low | 24 hours | 72 hours | 72 hours |

## 🌐 COMPATIBILITY

| Plan | Trusted Advisor | TAM | Phone Support |
|------|-----------------|-----|---------------|
| Basic | 0 checks | No | No |
| Developer | 0 checks | No | No |
| Business | 7 checks | No | Yes |
| Enterprise | All checks | Yes | Yes |

## 🔗 CROSS-REFERENCES

**Related**: Cost Management, AWS Organizations

**Prerequisite**: Basic billing concepts

**Next**: Technical Account Manager engagement

## ✅ EXAM TIPS

- Basic is free, includes documentation only
- Business required for Trusted Advisor (7 checks)
- Enterprise includes all checks + TAM
- Phone support available 24/7 on Business+
- Response time improves with higher tier