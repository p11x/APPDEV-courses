---
Category: Certification Prep
Subcategory: Azure AZ-900
Concept: Domain 4 - Pricing & Support
Purpose: Detailed coverage of Azure Fundamentals Domain 4 - Pricing and Support
Difficulty: beginner
Prerequisites: Basic cloud knowledge
RelatedFiles: 01_Exam_Guide.md, 02_Domain1_Cloud_Concepts.md, 03_Domain2_Core_Services.md, 04_Domain3_Security.md, 06_Practice_Questions_100.md
UseCase: Azure Fundamentals certification exam preparation
CertificationExam: Microsoft Azure Fundamentals (AZ-900)
LastUpdated: 2025
---

# Domain 4: Pricing & Support (10%)

## Domain Objectives

1. Understand Azure pricing models
2. Describe Azure cost management tools
3. Identify Azure support plans
4. Describe the Azure Service Level Agreements

---

## Key Topics

### 4.1 Azure Pricing Models

**Pay-as-you-go:**
- Pay only for what you use
- No upfront commitment
- Per-minute or per-second billing
- Flexible scaling

**Reserved Virtual Machine Instances:**
- 1-year or 3-year commitment
- Up to 72% savings
- Best for steady-state workloads
- Resize option available

**Azure Savings Plans:**
- Flexible commitment
- Apply to multiple services
- Compute savings plan
- More flexibility than Reserved

**Azure Spot Pricing:**
- Interruptible VMs
- Up to 90% discount
- For fault-tolerant workloads
- 30-second notice before eviction

### 4.2 Azure Cost Management

**Azure Cost Management + Billing:**
- Track spending
- Create budgets
- Set alerts
- Analyze costs

**Azure Pricing Calculator:**
- Estimate costs before deployment
- Select services and configurations
- Monthly/annual estimates
- Region-specific pricing

**Azure TCO Calculator:**
- Compare on-premises vs Azure
- Consider infrastructure, labor, storage
- Detailed report generation

**Cost Optimization Best Practices:**
- Right-size resources
- Use auto-scaling
- Delete unused resources
- Use reserved instances
- Leverage tags

### 4.3 Azure Support Plans

| Plan | Price | Use Case |
|------|-------|----------|
| Basic | Free | Learning |
| Standard | $29+/month | Development |
| Professional Direct | $100+/month | Business |
| Premier | Custom | Enterprise |

**Support Features:**

| Feature | Basic | Standard | Professional Direct | Premier |
|---------|-------|----------|---------------------|---------|
| Email support | Yes | Yes | Yes | Yes |
| Phone support | No | Yes | Yes | Yes |
| Business hours | N/A | Business | Business | 24/7 |
| Response time | N/A | 8 hours | 4 hours | 1 hour |
| TAM | No | No | No | Yes |
| Architecture support | No | No | Yes | Yes |

### 4.4 Azure Service Level Agreements (SLA)

**SLA Overview:**
- Service commitment percentage
- Uptime guarantees
- Credits for not meeting SLA

**Common SLAs:**

| Service | SLA |
|---------|-----|
| Virtual Machines (Single) | 99.9% |
| Virtual Machines (Availability Set) | 99.95% |
| Virtual Machines (Availability Zone) | 99.99% |
| Azure Storage | 99.9% |
| Azure SQL | 99.99% |
| App Service | 99.95% |

**SLA Formula:**
- Downtime per month = (1 - SLA%) × 43200 minutes
- Example: 99.9% = 43.2 minutes downtime/month

### 4.5 Azure Subscription Types

| Subscription | Description |
|--------------|-------------|
| Free | $200 credit, 12 months |
| Pay-as-you-go | Standard billing |
| Enterprise Agreement | Volume licensing |
| Visual Studio | MSDN benefits |
| Cloud Solution Provider | Partner reselling |

---

## Sample Questions

### Question 1
A company wants to reduce costs for VMs that run 24/7 with predictable usage. Which pricing option provides the best savings?

A. Pay-as-you-go
B. Reserved Instances
C. Spot VMs
D. Free Tier

**Answer: B**

**Explanation:** Reserved Instances provide up to 72% savings for steady-state workloads running 24/7 with predictable usage patterns.

### Question 2
Which tool should be used to estimate costs before deploying Azure resources?

A. Azure Cost Management
B. Azure Pricing Calculator
C. Azure Monitor
D. Azure Advisor

**Answer: B**

**Explanation:** Azure Pricing Calculator helps estimate costs before deployment by selecting services, configurations, and regions.

### Question 3
Which Azure Support Plan provides a Technical Account Manager (TAM)?

A. Basic
B. Standard
C. Professional Direct
D. Premier

**Answer: D**

**Explanation:** Only the Premier Support Plan provides a dedicated Technical Account Manager for architectural guidance and proactive support.

### Question 4
What is the SLA for a single virtual machine in Azure?

A. 99%
B. 99.9%
C. 99.95%
D. 99.99%

**Answer: B**

**Explanation:** A single virtual machine has a 99.9% SLA (three 9s). Using Availability Sets provides 99.95%, and Availability Zones provides 99.99%.

### Question 5
If an Azure service fails to meet its SLA, what compensation does Microsoft provide?

A. Refund
B. Service credits
C. Free month of service
D. Extended support

**Answer: B**

**Explanation:** Azure SLA provides service credits (discounts on future bills) when services fail to meet their committed uptime percentages, not refunds.

---

## Pricing Summary

- Pay-as-you-go: Most flexible, standard price
- Reserved: Up to 72% savings for 1-3 years
- Spot: Up to 90% for interruptible workloads
- Free: $200 credit + 12 months

---

**Continue to Practice Questions**
