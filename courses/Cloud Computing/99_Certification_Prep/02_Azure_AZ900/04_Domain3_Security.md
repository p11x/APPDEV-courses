---
Category: Certification Prep
Subcategory: Azure AZ-900
Concept: Domain 3 - Security
Purpose: Detailed coverage of Azure Fundamentals Domain 3 - Security
Difficulty: beginner
Prerequisites: Basic cloud knowledge
RelatedFiles: 01_Exam_Guide.md, 02_Domain1_Cloud_Concepts.md, 03_Domain2_Core_Services.md, 06_Practice_Questions_100.md
UseCase: Azure Fundamentals certification exam preparation
CertificationExam: Microsoft Azure Fundamentals (AZ-900)
LastUpdated: 2025
---

# Domain 3: Security (30%)

## Domain Objectives

1. Describe Azure identity services
2. Describe Azure security features
3. Describe Azure network security
4. Describe Azure security tools

---

## Key Topics

### 3.1 Azure Active Directory

**Azure AD vs On-premises AD:**

| Feature | Azure AD | On-premises AD |
|---------|----------|-----------------|
| Protocol | HTTP/HTTPS | LDAP |
| Authentication | OAuth, SAML | Kerberos, NTLM |
| Management | Cloud-based | Server-based |
| Users | Cloud identities | Domain-joined |

**Azure AD Features:**
- Identity as a Service (IdaaS)
- Single Sign-On (SSO)
- Multi-Factor Authentication (MFA)
- Conditional Access
- Application proxy

**Azure AD Roles:**
- Global Administrator
- User Administrator
- Billing Administrator
- Custom roles

### 3.2 Authentication and Authorization

**Authentication (AuthN):**
- Verifies user identity
- Credentials: Username/password, MFA
- Who you are

**Authorization (AuthZ):**
- Determines permissions
- Role-based access
- What you can access

**Multi-Factor Authentication:**
- Something you know (password)
- Something you have (phone/token)
- Something you are (biometrics)

### 3.3 Azure Security Tools

| Tool | Purpose |
|------|---------|
| Azure Security Center | Unified security management |
| Azure Sentinel | SIEM/SOAR |
| Azure Defender | Threat protection |
| Azure Key Vault | Secrets management |
| Azure DDoS Protection | DDoS mitigation |

### 3.4 Network Security

**Network Security Groups (NSG):**
- Filter network traffic
- Inbound/Outbound rules
- Priority-based rules
- Allow/Deny

**Azure Firewall:**
- Stateful firewall
- Threat intelligence
- High availability
- Zone redundancy

**Application Security Groups (ASG):**
- Group VMs by workload
- Simplifies NSG rules
- Application-centric

### 3.5 Azure Identity Protection

**Conditional Access Policies:**
- Location-based access
- Device compliance
- Risk-based policies
- Grant/Block access

**Identity Protection Features:**
- Risk detection
- Remediation options
- Reporting

---

## Sample Questions

### Question 1
A company wants to implement Single Sign-On (SSO) for cloud applications. Which Azure service should they use?

A. Azure Key Vault
B. Azure AD
C. Azure Security Center
D. Azure Firewall

**Answer: B**

**Explanation:** Azure Active Directory (Azure AD) provides Single Sign-On (SSO) capabilities, allowing users to authenticate once and access multiple applications.

### Question 2
Which type of access control verifies user identity and determines what resources they can access?

A. Authentication
B. Authorization
C. Encryption
D. Auditing

**Answer: B**

**Explanation:** Authorization determines what resources a user can access after their identity has been verified. Authentication verifies who the user is.

### Question 3
What is Azure Multi-Factor Authentication (MFA) designed to prevent?

A. Network attacks
B. Unauthorized access
C. Data loss
D. SQL injection

**Answer: B**

**Explanation:** Azure MFA prevents unauthorized access by requiring multiple forms of verification, making it harder for attackers to compromise accounts even if they obtain passwords.

### Question 4
Which service should be used to securely store application secrets, keys, and certificates?

A. Azure Storage Explorer
B. Azure Key Vault
C. Azure Security Center
D. Azure Monitor

**Answer: B**

**Explanation:** Azure Key Vault provides secure storage for secrets, keys, certificates, and other sensitive information with audit logging and access control.

### Question 5
A company needs to filter network traffic between subnets in Azure. Which service should they use?

A. Azure Firewall
B. Network Security Group (NSG)
C. Azure AD
D. Azure CDN

**Answer: B**

**Explanation:** Network Security Groups (NSGs) filter network traffic between subnets and VMs within a virtual network, providing layer 3/4 security.

---

## Security Concepts

- Defense in Depth: Multiple layers of security
- Zero Trust: Never trust, always verify
- Least Privilege: Minimum necessary access
- Shared Responsibility: Microsoft + Customer

---

**Continue to Domain 4: Pricing & Support**
