---
Category: Certification Prep
Subcategory: GCP ACE
Concept: Section 2 - Deploying and implementing a cloud solution
Purpose: Detailed coverage of GCP ACE Section 2 - Infrastructure
Difficulty: intermediate
Prerequisites: Basic cloud knowledge
RelatedFiles: 01_Exam_Guide.md, 02_Section1_Setup.md, 06_Practice_Questions_100.md
UseCase: GCP Cloud Engineer Associate certification exam preparation
CertificationExam: Google Cloud Associate Cloud Engineer (ACE)
LastUpdated: 2025
---

# Section 2: Deploying and Implementing a Cloud Solution (25%)

## Section Objectives

1. Deploy and configure compute resources
2. Deploy and configure storage resources
3. Deploy and configure database resources
4. Configure networking

---

## Key Topics

### 2.1 Compute Services

**Compute Engine (IaaS):**

| Machine Type | Use Case |
|--------------|----------|
| E2 | Cost-optimized |
| N2 | General purpose |
| N1 | Balanced (standard) |
| M2 | Memory-optimized |
| GPU | GPU workloads |

**Machine Family Types:**
- Standard (N1): Balanced compute and memory
- Memory-optimized (M1, M2): Large in-memory databases
- Compute-optimized (C2): High-performance compute

**Deployment Options:**
- Single instance
- Instance templates
- Managed instance groups
- Unmanaged instance groups

**App Engine (PaaS):**
- Standard: Serverless, pay per use
- Flexible: Containers, pay per resources
- app.yaml configuration

**Cloud Functions (Serverless):**
- Event-driven
- HTTP triggers
- Background functions
- 2nd gen: Cloud Run

**GKE (Kubernetes):**
- Managed Kubernetes
- Autopilot mode
- Standard mode
- Node pools

### 2.2 Storage Services

**Cloud Storage:**

| Storage Class | Use Case | Availability |
|---------------|----------|--------------|
| Standard | Frequent access | 99.9% |
| Nearline | Monthly access | 99.0% |
| Coldline | Quarterly access | 99.0% |
| Archive | Yearly access | 99.0% |

**Bucket Configuration:**
- Location (region/multi-region)
- Storage class
- Versioning
- Lifecycle policies
- Access control (ACLs, IAM)

**Filestore:**
- Managed NFS
- File storage
- Enterprise filestore

### 2.3 Database Services

**Cloud SQL:**

| Instance | Database | Use Case |
|----------|----------|----------|
| Primary | MySQL, PostgreSQL, SQL Server | Standard relational |
| HA | High availability | Production |
| Read Replica | Scaling reads | Read-heavy apps |

**Cloud Spanner:**
- Globally distributed
- Relational + NoSQL
- Horizontal scaling
- 99.99% SLA

**Firestore:**
- NoSQL document database
- Native mode
- Datastore mode
- Real-time sync

**BigQuery:**
- Data warehouse
- SQL-like queries
- Petabyte scale
- Serverless

### 2.4 Networking

**VPC Networks:**
- Global scope
- Custom mode (recommended)
- Auto mode
- Subnets

**Cloud Load Balancing:**

| Type | Traffic | Scope |
|------|---------|-------|
| Global HTTP(S) | HTTP(S) | Global |
| Global SSL Proxy | SSL | Global |
| Global TCP Proxy | TCP | Global |
| Regional | TCP/UDP | Regional |
| Internal | Internal | Regional |

**Cloud CDN:**
- HTTP(S) content delivery
- Cache configuration
- Signed URLs
- Origin fetch

**Cloud NAT:**
- NAT gateway
- Port exhaustion
- Private instance internet access

---

## Sample Questions

### Question 1
A company needs to deploy a stateless web application that auto-scales based on traffic. Which GCP service should they use?

A. Compute Engine
B. App Engine Flexible
C. Cloud Functions
D. GKE

**Answer: B**  
**Explanation:** App Engine Flexible is ideal for stateless web applications that need to auto-scale. Cloud Functions is also an option for smaller, event-driven applications.

### Question 2
Which Cloud Storage class should be used for data that is accessed less than once a year?

A. Standard
B. Nearline
C. Coldline
D. Archive

**Answer: D**  
**Explanation:** Archive storage is the most cost-effective for data that is accessed less than once a year, with 99.0% availability.

### Question 3
Which database service provides horizontal scaling with global distribution?

A. Cloud SQL
B. Cloud Spanner
C. Firestore
D. BigQuery

**Answer: B**  
**Explanation:** Cloud Spanner is a globally distributed relational database that provides horizontal scaling with strong consistency.

### Question 4
What is the recommended VPC network mode for new projects?

A. Auto mode
B. Custom mode
C. Legacy mode
D. Default

**Answer: B**  
**Explanation:** Custom mode VPC is recommended for new projects as it gives full control over subnet IP ranges and allows for more flexible network design.

### Question 5
Which load balancer should be used for global HTTP(S) traffic?

A. Regional Load Balancer
B. Internal Load Balancer
C. Global HTTP(S) Load Balancer
D. TCP Proxy Load Balancer

**Answer: C**  
**Explanation:** Global HTTP(S) Load Balancer is designed for global HTTP and HTTPS traffic with latency-based routing.

---

## Service Comparison

| Service | Type | Scaling | Use Case |
|---------|------|---------|----------|
| Compute Engine | IaaS | Manual/Auto | Full control |
| App Engine | PaaS | Auto | Web apps |
| Cloud Functions | Serverless | Auto | Event-driven |
| GKE | Container | Auto | Containers |

---

**Continue to Section 3: Deploy**
