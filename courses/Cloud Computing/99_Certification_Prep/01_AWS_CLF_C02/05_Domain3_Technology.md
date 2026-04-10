---
Category: Certification Prep
Subcategory: AWS CLF-C02
Concept: Domain 3 - Technology
Purpose: Detailed coverage of AWS Cloud Practitioner Domain 3 - Technology
Difficulty: intermediate
Prerequisites: Basic cloud knowledge
RelatedFiles: 01_Exam_Guide.md, 02_Practice_Questions.md, 03_Domain1_Cloud_Concepts.md, 04_Domain2_Security_Compliance.md
UseCase: AWS Cloud Practitioner certification exam preparation
CertificationExam: AWS Certified Cloud Practitioner (CLF-C02)
LastUpdated: 2025
---

# Domain 3: Technology (33%)

## Domain Objectives

1. Define core AWS services (compute, storage, database, networking)
2. Identify use cases for AWS services
3. Describe AWS serverless architecture
4. Understand AWS networking concepts
5. Compare database options

---

## Key Topics

### 3.1 Compute Services

**Amazon EC2:**

| Instance Type | Use Case |
|--------------|----------|
| On-Demand | Short-term, flexible workloads |
| Reserved Instances | Steady-state, 1-3 year commitment |
| Savings Plans | Flexible commitment, lower price |
| Spot Instances | Fault-tolerant batch, interruptible |
| Dedicated Hosts | Compliance, licensing requirements |

**AWS Lambda:**
- Serverless compute
- Pay per request
- Max timeout: 15 minutes
- Memory: 128MB - 10GB
- Triggers: S3, API Gateway, DynamoDB, etc.

**Amazon Lightsail:**
- Simple virtual private servers
- Pre-configured applications
- Fixed monthly pricing

### 3.2 Storage Services

**Amazon S3:**

| Storage Class | Use Case | Retrieval Time |
|--------------|----------|----------------|
| Standard | Frequent access | Immediate |
| Standard-IA | Infrequent access | Immediate |
| One Zone-IA | Non-critical data | Immediate |
| Glacier | Archival | Minutes to hours |
| Glacier Deep Archive | Long-term archive | 12+ hours |
| Intelligent-Tiering | Unknown access patterns | Variable |

**S3 Features:**
- Versioning
- Encryption (SSE-S3, SSE-KMS, SSE-C)
- Lifecycle policies
- Replication (CRR, SRR)
- Pre-signed URLs
- Access Points

**Other Storage:**
- EBS: Block storage for EC2
- EFS: File system (NFS)
- FSx: Windows/Lustre file systems
- Storage Gateway: Hybrid storage

### 3.3 Database Services

| Service | Type | Use Case |
|---------|------|----------|
| RDS | Managed relational | MySQL, PostgreSQL, Oracle, SQL Server |
| Aurora | Cloud-native relational | High performance, MySQL/PostgreSQL compatible |
| DynamoDB | NoSQL key-value | High throughput, serverless |
| ElastiCache | In-memory | Caching, session storage |
| Redshift | Data warehouse | Analytics, BI |
| Neptune | Graph | Social networks, fraud detection |

### 3.4 Networking

**Amazon VPC:**
- Isolated cloud resources
- CIDR block allocation
- Subnets: Public and Private
- Route Tables
- Internet Gateway
- NAT Gateway
- VPC Endpoints (Interface, Gateway)

**DNS Services:**
- Route 53: DNS, routing policies (Simple, Weighted, Latency, Failover)
- Health checks

**CDN:**
- CloudFront: Global content delivery
- Edge locations: 400+ locations
- Cache behavior
- Signed URLs

### 3.5 Serverless Architecture

| Service | Purpose |
|---------|---------|
| Lambda | Compute |
| API Gateway | API management |
| DynamoDB | Database |
| S3 | Storage |
| Step Functions | Orchestration |
| SNS/SQS | Messaging |
| EventBridge | Event-driven |

---

## Sample Questions

### Question 1
A company needs to run a batch processing job that can tolerate interruption. Which EC2 pricing model should they use?

A. On-Demand
B. Reserved Instance
C. Savings Plan
D. Spot Instance

**Answer: D**

**Explanation:** Spot Instances are ideal for fault-tolerant workloads that can handle interruption, such as batch processing jobs. They offer up to 90% discount compared to On-Demand pricing.

### Question 2
Which S3 storage class is MOST cost-effective for data that is accessed less than once per month?

A. Standard
B. Standard-IA
C. One Zone-IA
D. Glacier

**Answer: C**

**Explanation:** One Zone-IA is the most cost-effective for infrequently accessed data that can tolerate loss (stored in single AZ). However, if accessed less than once per month and can tolerate multi-hour retrieval, Glacier is most cost-effective.

### Question 3
Which database service provides automatic multi-AZ replication?

A. DynamoDB
B. ElastiCache
C. RDS
D. Redshift

**Answer: C**

**Explanation:** Amazon RDS supports Multi-AZ deployment for high availability. DynamoDB provides global tables for multi-region, ElastiCache supports replication groups, and Redshift has different availability features.

### Question 4
What is the maximum execution timeout for an AWS Lambda function?

A. 5 minutes
B. 15 minutes
C. 30 minutes
D. 1 hour

**Answer: B**

**Explanation:** AWS Lambda has a maximum execution timeout of 15 minutes (900 seconds). For longer-running workloads, consider using ECS or EC2.

### Question 5
A company needs to distribute content globally with low latency. Which AWS service should they use?

A. Route 53
B. CloudFront
C. S3 Transfer Acceleration
D. Global Accelerator

**Answer: B**

**Explanation:** Amazon CloudFront is a content delivery network (CDN) that delivers content globally with low latency through edge locations. Route 53 is DNS, S3 Transfer Acceleration improves S3 uploads, and Global Accelerator improves latency for applications.

---

## Key Services Summary

| Category | Key Service | Key Feature |
|----------|-------------|-------------|
| Compute | EC2 | Virtual servers |
| Compute | Lambda | Serverless |
| Storage | S3 | Object storage |
| Storage | EBS | Block storage |
| Database | RDS | Managed SQL |
| Database | DynamoDB | NoSQL |
| Network | VPC | Isolated network |
| Network | CloudFront | CDN |
| DNS | Route 53 | DNS routing |

---

**Continue to Domain 4: Billing & Pricing**
