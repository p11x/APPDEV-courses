---
Category: Certification Prep
Subcategory: AWS CLF-C02
Concept: Exam Guide
Purpose: Comprehensive exam preparation guide for AWS Cloud Practitioner CLF-C02
Difficulty: intermediate
Prerequisites: All AWS Cloud Practitioner concepts
RelatedFiles: 01_Basic_*.md in 01_AWS_Cloud_Practitioner
UseCase: AWS Cloud Practitioner certification exam preparation
CertificationExam: AWS Certified Cloud Practitioner (CLF-C02)
LastUpdated: 2025
---

## 💡 WHY

The AWS Cloud Practitioner certification validates foundational cloud knowledge. This exam guide helps you prepare systematically and pass on your first attempt.

## 📖 WHAT

### Exam Overview

- **Duration**: 90 minutes
- **Questions**: 65 questions (varies 65-75)
- **Format**: Multiple choice and multiple response
- **Passing Score**: 72% (approximately)
- **Cost**: $100 USD (varies by region)

### Domain Breakdown

| Domain | Weight | Topics |
|--------|--------|--------|
| Domain 1: Cloud Concepts | 26% | Cloud definitions, benefits, AWS infrastructure |
| Domain 2: Security & Architecture | 25% | Shared responsibility, AWS security services, Well-Architected |
| Domain 3: Technology | 33% | Core AWS services, compute, storage, database, networking |
| Domain 4: Billing & Pricing | 16% | Pricing models, cost management, AWS Organizations |

### Exam Strategy

1. **Read Carefully**: Understand what the question asks
2. **Eliminate Wrong Answers**: Remove obviously incorrect options
3. **Look for Keywords**: "most," "best," "lowest cost," "highest availability"
4. **Time Management**: ~1.5 minutes per question

## 🔧 HOW

### Study Approach

**Week 1-2: Fundamentals**
- Review Cloud Concepts
- Understand AWS Global Infrastructure
- Study Deployment Models

**Week 3: Security**
- Complete Shared Responsibility
- Review Security Services
- Understand IAM fundamentals

**Week 4: Core Services**
- EC2, S3, Lambda basics
- VPC, RDS, networking
- CloudFront, Route53

**Week 5: Architecture & Billing**
- Well-Architected Framework
- Pricing Models
- Cost Management

**Week 6: Practice & Review**
- Take practice exams
- Review weak areas
- Exam day preparation

### Key Topics by Domain

**Domain 1: Cloud Concepts**
- Cloud computing definition and benefits
- Six advantages of cloud
- AWS global infrastructure (Regions, AZs)
- Cloud deployment models (Public, Private, Hybrid)

**Domain 2: Security**
- Shared Responsibility Model (security IN the cloud vs security OF the cloud)
- IAM (Users, Groups, Roles, Policies)
- AWS security services (WAF, Shield, GuardDuty)
- Compliance programs (HIPAA, PCI-DSS, SOC)

**Domain 3: Technology**
- EC2 (Instance types, pricing models)
- S3 (Storage classes, encryption, versioning)
- RDS (Multi-AZ, read replicas)
- Lambda (Serverless, triggers)
- VPC (Subnets, Security Groups, Route Tables)
- CloudFront (CDN, caching)
- Route53 (DNS, routing policies)

**Domain 4: Billing**
- On-Demand, Reserved, Savings Plans, Spot
- AWS Organizations (SCP, Consolidated Billing)
- Cost Explorer and Budgets
- AWS Support Plans

## ⚠️ COMMON MISTAKES

### 1. Not Understanding Shared Responsibility

**Mistake**: Thinking AWS handles everything

**Correction**: Remember: AWS secures infrastructure, you secure data and configurations

### 2. Confusing Pricing Models

**Mistake**: Not knowing when to use On-Demand vs Reserved vs Spot

**Correction**: 
- On-Demand: Variable, unknown usage
- Reserved: Steady-state, predictable
- Spot: Fault-tolerant batch jobs

### 3. Missing Service Limits

**Mistake**: Not knowing default quotas

**Correction**: Know EC2 limits (20 per region), S3 (unlimited), IAM (5000 users)

### 4. Wrong Storage Class Selection

**Mistake**: Using wrong S3 storage class

**Correction**: 
- Standard: Frequent access
- IA: <30 days access
- Glacier: Archival, hours to retrieve

## 🏃 PERFORMANCE

### Practice Exam Strategy

- Take full-length practice exams
- Review incorrect answers thoroughly
- Focus on weak domains
- Time yourself

### Key Metrics to Know

- S3 durability: 99.999999999%
- S3 availability: 99.99%
- EC2 SLA: 99.99% (multi-AZ)
- Lambda timeout: 15 minutes max

## 🔗 CROSS-REFERENCES

### Related Study Materials

- AWS Cloud Practitioner Learning Path
- Official Practice Exams
- AWS Well-Architected Framework
- AWS re:Invent videos

### Recommended Next Steps

- Take official practice exam
- Review missed questions
- Schedule exam date

## ✅ EXAM TIPS

### Remember These Key Facts

1. **Six Advantages of Cloud**:
   - Trade fixed expense for variable expense
   - Benefit from massive economies of scale
   - Stop guessing capacity
   - Increase speed and agility
   - Stop spending money running and maintaining data centers
   - Go global in minutes

2. **Shared Responsibility**:
   - AWS: Physical security, hypervisor, networking
   - Customer: Data, applications, IAM, encryption

3. **Pricing**:
   - Pay only for what you use
   - Savings Plans offer up to 72% off
   - Spot offers up to 90% off

4. **Regions and AZs**:
   - Regions: Geographic areas
   - AZs: Isolated data centers within region
   - Minimum 2 AZs for High Availability

5. **Security Groups**:
   - Stateful
   - Allow rules only
   - Default: Deny all inbound

---

**Good luck with your exam preparation!**

**Register at**: https://aws.amazon.com/certification/certified-cloud-practitioner/