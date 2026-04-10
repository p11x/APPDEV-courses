---
Category: Certification Prep
Subcategory: AWS CLF-C02
Concept: Domain 4 - Billing & Pricing
Purpose: Detailed coverage of AWS Cloud Practitioner Domain 4 - Billing and Pricing
Difficulty: intermediate
Prerequisites: Basic cloud knowledge
RelatedFiles: 01_Exam_Guide.md, 02_Practice_Questions.md, 03_Domain1_Cloud_Concepts.md, 04_Domain2_Security_Compliance.md, 05_Domain3_Technology.md
UseCase: AWS Cloud Practitioner certification exam preparation
CertificationExam: AWS Certified Cloud Practitioner (CLF-C02)
LastUpdated: 2025
---

# Domain 4: Billing & Pricing (16%)

## Domain Objectives

1. Understand AWS pricing models
2. Describe AWS cost management tools
3. Identify factors that affect costs
4. Understand AWS Organizations
5. Describe AWS Support Plans

---

## Key Topics

### 4.1 AWS Pricing Models

**On-Demand Pricing:**
- Pay per hour/second
- No upfront commitment
- Lowest commitment, highest price
- Use: Short-term, test/dev, unknown usage

**Reserved Instances:**
- 1-year or 3-year commitment
- Up to 72% discount
- All upfront, partial upfront, no upfront
- Use: Steady-state workloads

**Savings Plans:**
- Commitment to spend amount
- More flexibility than RI
- Compute Savings Plans: EC2, Lambda, Fargate
- Use: Flexible usage patterns

**Spot Instances:**
- Up to 90% discount
- Interruptible with 2-minute notice
- Use: Fault-tolerant batch jobs, stateless applications

**Other Pricing:**
- Dedicated Hosts: Physical servers
- Dedicated Instances: Isolation
- Free Tier: 12 months free

### 4.2 Pricing Factors

**EC2 Pricing Factors:**
- Instance type
- Region
- Pricing model (On-Demand, Reserved, Spot)
- Data transfer

**S3 Pricing Factors:**
- Storage class
- Storage amount
- Requests/retrieval
- Data transfer out

**Lambda Pricing Factors:**
- Request count
- Execution duration
- Memory allocated

**RDS Pricing Factors:**
- Instance class
- Multi-AZ deployment
- Storage
- I/O requests

### 4.3 AWS Cost Management Tools

| Tool | Purpose |
|------|---------|
| AWS Budgets | Set custom cost alerts |
| Cost Explorer | Visualize costs |
| AWS Cost Anomaly Detection | Unusual spending alerts |
| AWS Cost Categories | Organize costs |
| AWS Calculator | Estimate costs |
| Consolidated Billing | Combined billing |

### 4.4 AWS Organizations

**Features:**
- Centralized billing
- Service Control Policies (SCP)
- Organize accounts into OUs
- Cross-account access

**Benefits:**
- Consolidated billing
- Programmatic access
- Tag policies

### 4.5 AWS Support Plans

| Plan | Price | Use Case |
|------|-------|----------|
| Basic | Free | Anyone |
| Developer | $29+/month | Test/dev |
| Business | $100+/month | Production |
| Enterprise | $15,000+/month | Mission-critical |

**Support Features:**

| Feature | Developer | Business | Enterprise |
|---------|-----------|----------|------------|
| Email support | 24 hours | 24 hours | 15 minutes |
| Phone support | No | Yes | Yes |
| TAM | No | No | Yes |
| Health checks | No | Yes | Yes |

---

## Sample Questions

### Question 1
A company has a steady-state workload running 24/7. Which pricing model would provide the MOST cost savings?

A. On-Demand
B. Reserved Instance
C. Spot Instance
D. Savings Plan

**Answer: B**

**Explanation:** Reserved Instances provide up to 72% discount for steady-state workloads with predictable usage. Savings Plans also offer significant savings but provide more flexibility in instance type and size changes.

### Question 2
Which tool can alert you when spending exceeds a certain threshold?

A. AWS Calculator
B. AWS Budgets
C. Cost Explorer
D. AWS Organizations

**Answer: B**

**Explanation:** AWS Budgets allows you to set custom cost alerts that notify you when spending exceeds (or is forecast to exceed) your defined threshold.

### Question 3
What is the primary benefit of AWS Organizations with Consolidated Billing?

A. Single login for all accounts
B. Combined usage for volume discounts
C. Automatic backup of all accounts
D. Centralized security scanning

**Answer: B**

**Explanation:** Consolidated Billing in AWS Organizations combines usage across all linked accounts, potentially unlocking volume discounts from AWS.

### Question 4
Which AWS Support Plan provides a Technical Account Manager (TAM)?

A. Basic
B. Developer
C. Business
D. Enterprise

**Answer: D**

**Explanation:** Only the Enterprise Support Plan provides a dedicated Technical Account Manager (TAM) who serves as a single point of contact for technical guidance.

### Question 5
Which factor does NOT affect EC2 pricing?

A. Instance type
B. Region
C. Operating system
D. Number of security groups

**Answer: D**

**Explanation:** Instance type, Region, and Operating System all affect EC2 pricing. The number of security groups does not directly affect EC2 pricing.

---

## Key Pricing Points

| Service | Discount | Requirement |
|---------|----------|--------------|
| Reserved Instances | Up to 72% | 1-3 year commitment |
| Savings Plans | Up to 72% | Spend commitment |
| Spot Instances | Up to 90% | Interruptible |
| Free Tier | 12 months | New accounts |

---

## Cost Optimization Tips

1. Use Auto Scaling
2. Right-size instances
3. Use Spot for fault-tolerant workloads
4. Leverage Reserved Instances for steady workloads
5. Use S3 Lifecycle policies
6. Enable Cost Explorer
7. Use tags for cost allocation

---

**Continue to Practice Questions**
