---
Category: Certification Prep
Subcategory: Azure AZ-900
Concept: Domain 2 - Core Azure Services
Purpose: Detailed coverage of Azure Fundamentals Domain 2 - Core Services
Difficulty: beginner
Prerequisites: Basic cloud knowledge
RelatedFiles: 01_Exam_Guide.md, 02_Domain1_Cloud_Concepts.md, 06_Practice_Questions_100.md
UseCase: Azure Fundamentals certification exam preparation
CertificationExam: Microsoft Azure Fundamentals (AZ-900)
LastUpdated: 2025
---

# Domain 2: Core Azure Services (35%)

## Domain Objectives

1. Identify core Azure services
2. Describe Azure compute options
3. Describe Azure storage services
4. Describe Azure networking options
5. Identify Azure database services

---

## Key Topics

### 2.1 Azure Compute Services

**Azure Virtual Machines (IaaS):**
- Windows and Linux VMs
- VM sizes (A, B, D, E, F, G series)
- Scale sets for auto-scaling
- Custom scripts extension
- Azure VMs: Full control over OS

**Azure App Service (PaaS):**
- Web Apps, API Apps, Mobile Apps
- Supports .NET, Java, Node.js, Python, PHP
- Auto-scaling
- Deployment slots
- Authentication/Authorization

**Azure Functions (PaaS):**
- Serverless compute
- Event-driven
- Pay-per-execution
- Triggers: HTTP, Timer, Blob, Queue
- Durable Functions for orchestration

**Azure Container Instances (PaaS):**
- Container deployment
- Fast startup
- Kubernetes integration (ACI connector)
- Windows and Linux containers

### 2.2 Azure Storage Services

**Azure Blob Storage:**
- Object storage for unstructured data
- Tier: Hot, Cool, Archive
- Use cases: Images, videos, backups
- CDN integration

**Azure Disk Storage:**
- Managed disks
- Types: HDD (Standard), SSD (Premium)
- Page blobs for VM disks
- Managed vs unmanaged

**Azure Files:**
- SMB file shares
- Cloud file sharing
- Mount on Windows/Linux
- Sync with on-premises (Azure File Sync)

**Azure Queue Storage:**
- Message queuing
- Async communication
- Decouple applications

### 2.3 Azure Networking

**Azure Virtual Network (VNet):**
- Isolated network in Azure
- IP address range
- Subnets
- Network security groups (NSG)
- Private endpoints

**Azure VPN Gateway:**
- Site-to-Site VPN
- Point-to-Site VPN
- VNet-to-VNet connections
- ExpressRoute for private connection

**Azure CDN:**
- Content delivery network
- Global distribution
- Cache static content
- Custom domain support

**Azure Load Balancer:**
- Layer 4 load balancing
- Health probes
- NAT rules
- HA ports

**Azure Application Gateway:**
- Layer 7 load balancing
- Web Application Firewall (WAF)
- SSL termination
- URL-based routing

### 2.4 Azure Database Services

**Azure SQL Database:**
- Managed SQL Server
- PaaS database
- Auto-scaling
- Advanced threat protection
- Point-in-time restore

**Azure Cosmos DB:**
- Global distributed NoSQL
- Multi-model (key-value, document, graph)
- Multiple consistency models
- Single-digit millisecond latency

**Azure Database for MySQL/PostgreSQL:**
- Managed open-source databases
- High availability
- Auto-patching
- Read replicas

---

## Sample Questions

### Question 1
Which Azure service is best for hosting a REST API that needs to scale automatically based on request volume?

A. Azure Virtual Machines
B. Azure App Service
C. Azure Container Instances
D. Azure Batch

**Answer: B**

**Explanation:** Azure App Service (Web Apps/API Apps) is ideal for hosting REST APIs with automatic scaling based on traffic, providing built-in scaling, authentication, and deployment slots.

### Question 2
A company needs to store large amounts of unstructured data (videos, images) at the lowest cost. Which storage type should they use?

A. Azure Blob Storage - Hot
B. Azure Blob Storage - Cool
C. Azure Blob Storage - Archive
D. Azure Files

**Answer: C**

**Explanation:** Azure Blob Storage Archive tier provides the lowest cost for storing large amounts of data that is rarely accessed, perfect for long-term video/image storage.

### Question 3
Which service provides fully managed hosting for containerized applications?

A. Azure VMs
B. Azure Functions
C. Azure Container Instances
D. Azure App Service

**Answer: C**

**Explanation:** Azure Container Instances (ACI) provides a fast and simple way to run containers without managing VMs, ideal for simple container deployments.

### Question 4
What type of load balancer is Azure Application Gateway?

A. Layer 3
B. Layer 4
C. Layer 7
D. Layer 2

**Answer: C**

**Explanation:** Azure Application Gateway is a Layer 7 (application layer) load balancer that provides URL-based routing, SSL termination, and Web Application Firewall (WAF) capabilities.

### Question 5
Which Azure database service provides multi-model database with global distribution?

A. Azure SQL Database
B. Azure Database for MySQL
C. Azure Cosmos DB
D. Azure SQL Managed Instance

**Answer: C**

**Explanation:** Azure Cosmos DB is a globally distributed, multi-model database service that supports key-value, document, column-family, and graph data models with automatic scaling.

---

## Service Comparison

| Category | IaaS | PaaS | Serverless |
|----------|------|------|------------|
| Azure | VMs | App Service, SQL DB | Functions |
| Control | Full | Partial | Minimal |
| Scaling | Manual | Auto | Automatic |
| Cost | Always running | Always running | Pay per use |

---

**Continue to Domain 3: Security**
