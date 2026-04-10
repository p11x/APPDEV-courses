---
Category: Certification Prep
Subcategory: GCP ACE
Concept: Section 4 - Configuring access and security
Purpose: Detailed coverage of GCP ACE Section 4 - Access and Security
Difficulty: intermediate
Prerequisites: Basic cloud knowledge
RelatedFiles: 01_Exam_Guide.md, 02_Section1_Setup.md, 03_Section2_Infrastructure.md, 04_Section3_Deploy.md, 06_Practice_Questions_100.md
UseCase: GCP Cloud Engineer Associate certification exam preparation
CertificationExam: Google Cloud Associate Cloud Engineer (ACE)
LastUpdated: 2025
---

# Section 4: Configuring Access and Security (25%)

## Section Objectives

1. Configure IAM
2. Configure VPC networks
3. Manage service accounts
4. Configure firewall rules
5. Understand security best practices

---

## Key Topics

### 4.1 IAM Configuration

**IAM Policy Structure:**
```json
{
  "bindings": [
    {
      "role": "roles/owner",
      "members": ["user:admin@example.com"]
    }
  ]
}
```

**Service Accounts:**
- Default service accounts
- User-managed service accounts
- Keys management
- Least privilege

**IAM Best Practices:**
- Use predefined roles
- Grant least privilege
- Audit with Cloud Audit Logs
- Rotate service account keys
- Use workload identity (GKE)

### 4.2 VPC Network Configuration

**VPC Types:**
| Type | Description |
|------|-------------|
| Auto mode | Pre-defined subnets |
| Custom mode | User-defined subnets |
| Shared VPC | Cross-project networking |

**Subnet Modes:**
- Regional subnets
- Private Google Access
- Flow logs

**VPC Peering:**
- Cross-project connections
- No transitive peering
- Shared VPC alternative

### 4.3 Firewall Rules

**Ingress/Egress Rules:**
```bash
# Allow SSH
gcloud compute firewall-rules create allow-ssh \
    --allow=tcp:22 \
    --source-ranges=0.0.0.0/0

# Allow internal traffic
gcloud compute firewall-rules create allow-internal \
    --allow=tcp:udp,icmp \
    --source-ranges=10.0.0.0/8
```

**Best Practices:**
- Use service accounts for VM access
- Restrict SSH key access
- Use bastion hosts
- Private instances with NAT

### 4.4 Network Security

**Cloud NAT:**
- Allow outbound internet
- Prevent inbound access
- Port exhaustion handling

**Private Google Access:**
- Access GCP APIs without internet
- Private IP addresses
- Enabled at subnet level

**Cloud Armor:**
- DDoS protection
- WAF rules
- Security policies

### 4.5 Resource Management

**Organization Policies:**
- Constraints
- Hierarchical inheritance
- Service restrictions

**Resource Manager:**
- Quotas
- Labels
- Tags

---

## Sample Questions

### Question 1
What is the principle of least privilege in IAM?

A. Give full access to all users
B. Grant minimum required permissions
C. Use primitive roles only
D. Share service accounts

**Answer: B**  
**Explanation:** The principle of least privilege means granting only the minimum permissions required for a user or service to perform their tasks.

### Question 2
Which VPC mode allows full control over subnet IP ranges?

A. Auto mode
B. Custom mode
C. Legacy mode
D. Default mode

**Answer: B**  
**Explanation:** Custom mode VPC allows users to define their own IP ranges for subnets, providing full control over the network architecture.

### Question 3
What is the purpose of a bastion host?

A. Database access
B. Secure SSH access to instances
C. Load balancing
D. DNS resolution

**Answer: B**  
**Explanation:** A bastion host is a jump server that provides secure SSH/RDP access to instances in private subnets, typically through the GCP IAP proxy.

### Question 4
What does Private Google Access enable?

A. Public internet access
B. Access GCP services without public IP
C. VPN connections
D. Direct peering

**Answer: B**  
**Explanation:** Private Google Access allows VMs without public IP addresses to access GCP APIs and services using private IP addresses.

### Question 5
Which method is recommended for GKE workloads to access GCP services?

A. Service account keys
B. OAuth tokens
C. Workload Identity
D. Default credentials

**Answer: C**  
**Explanation:** Workload Identity is the recommended way for GKE workloads to access GCP services securely, as it avoids storing credentials.

---

## Security Checklist

- [ ] Use service accounts with least privilege
- [ ] Enable VPC Flow Logs
- [ ] Use Cloud NAT for private instances
- [ ] Configure firewall rules restrictively
- [ ] Enable Cloud Audit Logs
- [ ] Use Private Google Access
- [ ] Implement organization policies

---

**Continue to Practice Questions**
