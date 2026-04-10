---
Category: Certification Prep
Subcategory: AWS CLF-C02
Concept: Domain 2 - Security & Compliance
Purpose: Detailed coverage of AWS Cloud Practitioner Domain 2 - Security and Compliance
Difficulty: intermediate
Prerequisites: Basic cloud knowledge
RelatedFiles: 01_Exam_Guide.md, 02_Practice_Questions.md, 03_Domain1_Cloud_Concepts.md
UseCase: AWS Cloud Practitioner certification exam preparation
CertificationExam: AWS Certified Cloud Practitioner (CLF-C02)
LastUpdated: 2025
---

# Domain 2: Security & Compliance (25%)

## Domain Objectives

1. Define the AWS Shared Responsibility Model
2. Identify AWS security services and features
3. Understand AWS compliance programs
4. Describe AWS identity and access management
5. Explain security best practices

---

## Key Topics

### 2.1 AWS Shared Responsibility Model

| Responsibility | AWS (Security OF the Cloud) | Customer (Security IN the Cloud) |
|---------------|------------------------------|----------------------------------|
| Infrastructure | Physical facilities, hardware | Instance operating systems |
| Network | Networking infrastructure | Security groups, NACLs |
| Hypervisor | Hypervisor, physical servers | Applications, data |
| Database | Database engine patching | Data encryption, access |
| Storage | Storage erasure, encryption | Data classification, encryption |

**Security "in" the Cloud:**
- Customer data
- Applications
- OS patches and updates
- Identity and access management
- Firewall configuration
- Encryption (customer-managed)

**Security "of" the Cloud:**
- Physical security of data centers
- Hardware infrastructure
- Software licensing
- Network infrastructure
- Hypervisor

### 2.2 Identity and Access Management (IAM)

**IAM Components:**

| Component | Description | Use Case |
|-----------|-------------|----------|
| Users | Individual access credentials | Personal access |
| Groups | Collections of users | Team-based permissions |
| Roles | Temporary elevated access | Cross-account, services |
| Policies | JSON permission documents | Fine-grained access control |

**IAM Best Practices:**
- Use IAM roles instead of access keys
- Enable MFA for privileged users
- Follow least privilege principle
- Use password policies
- Rotate credentials regularly

### 2.3 AWS Security Services

| Service | Purpose | Key Features |
|---------|---------|--------------|
| AWS WAF | Web Application Firewall | SQL injection, XSS protection |
| AWS Shield | DDoS protection | Standard/Advanced tiers |
| Amazon GuardDuty | Threat detection | ML-based anomaly detection |
| AWS Config | Resource configuration tracking | Compliance monitoring |
| Amazon Inspector | Security assessment | Vulnerability scanning |
| AWS Security Hub | Centralized security view | Aggregated findings |

### 2.4 AWS Compliance Programs

**Major Compliance Programs:**

| Program | Description | Industry |
|---------|-------------|----------|
| HIPAA | Health information protection | Healthcare |
| PCI-DSS | Payment card security | Finance/Retail |
| SOC 1/2/3 | Operational security controls | General |
| ISO 27001 | Information security | General |
| FedRAMP | Federal authorization | Government |
| GDPR | EU data protection | European Union |

### 2.5 Security Groups vs NACLs

| Feature | Security Groups | Network ACLs |
|---------|-----------------|--------------|
| Scope | Instance level | Subnet level |
| State | Stateful | Stateless |
| Rules | Allow only | Allow and deny |
| Evaluation | All rules evaluated | Processed in order |

---

## Sample Questions

### Question 1
A company is storing sensitive customer data in S3. What is AWS responsible for regarding this data?

A. Data encryption
B. Access control to the data
C. Physical security of the storage infrastructure
D. Data classification

**Answer: C**

**Explanation:** Under the Shared Responsibility Model, AWS is responsible for the security "of" the cloud - including physical security of data centers and storage infrastructure. The customer is responsible for "in" the cloud security - including data encryption, access control, and classification.

### Question 2
Which IAM feature provides temporary elevated access to AWS resources?

A. IAM User
B. IAM Group
C. IAM Role
D. IAM Policy

**Answer: C**

**Explanation:** IAM Roles provide temporary credentials and elevated access. They are ideal for cross-account access, EC2 instance roles, and federation scenarios where users need temporary access.

### Question 3
What type of attack does AWS Shield Standard protect against?

A. SQL injection
B. DDoS attacks
C. Cross-site scripting
D. Man-in-the-middle

**Answer: B**

**Explanation:** AWS Shield Standard provides protection against Distributed Denial of Service (DDoS) attacks. AWS WAF protects against SQL injection and XSS, while Shield Advanced provides additional DDoS protection.

### Question 4
Which compliance standard is required for handling credit card data?

A. HIPAA
B. FedRAMP
C. PCI-DSS
D. GDPR

**Answer: C**

**Explanation:** PCI-DSS (Payment Card Industry Data Security Standard) is required for any organization that stores, processes, or transmits credit cardholder data. HIPAA is for healthcare, GDPR for EU data, and FedRAMP for US federal agencies.

### Question 5
A security group has an inbound rule allowing port 22 from 0.0.0.0/0. What is the security concern?

A. Too restrictive
B. Allows SSH from anywhere
C. Should use UDP instead
D. Missing outbound rules

**Answer: B**

**Explanation:** Allowing SSH (port 22) from 0.0.0.0/0 (anywhere) is a significant security risk as it exposes instances to potential brute force attacks. Best practice is to limit SSH access to specific IP addresses or VPN.

---

## Key Concepts to Remember

- Security Groups: Stateful, allow rules only
- NACLs: Stateless, allow and deny
- Default VPC: All traffic allowed initially
- IAM Root: Full access, enable MFA
- S3 Block Public Access: Enable by default
- KMS: Customer-managed keys

---

**Continue to Domain 3: Technology**
