---
Category: AWS Cloud Practitioner
Subcategory: AWS Security
Concept: Shared Responsibility
Purpose: Understanding the shared responsibility model between AWS and customers for security and compliance
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Shared_Responsibility.md, 03_Practical_Shared_Responsibility.md
UseCase: Ensuring proper security controls are applied at each layer
CertificationExam: AWS Certified Cloud Practitioner - Domain 2: Security
LastUpdated: 2025
---

## WHY

The shared responsibility model is fundamental to understanding security in the cloud. AWS secures the cloud infrastructure while customers must secure their data and configurations. Misunderstanding this boundary is the #1 cause of cloud security incidents.

### Why Shared Responsibility Matters

- **Clear Ownership**: Know who protects what
- **Compliance**: Meet regulatory requirements
- **Cost**: Don't pay for AWS-provided controls
- **Security**: Proper layered security
- **Incidents**: Faster resolution when issues arise

### Security Incidents

- 95% of breaches involve customer-side misconfigurations
- AWS provides tools; customers must use them
- S3 bucket exposures are 80%+ customer error

### What AWS Manages

- **Physical Security**: Data centers, hardware
- **Network Infrastructure**: Regions, Availability Zones
- **Hypervisor**: EC2 host security
- **Managed Services**: RDS, Lambda, etc.

### What Customers Manage

- **Data**: Encryption, classification, access
- **Applications**: Code, configurations
- **Identity**: IAM, credentials
- **Configurations**: Security groups, policies

## WHAT

### Responsibility Matrix

| Layer | AWS Responsibility | Customer Responsibility |
|-------|-------------------|----------------------|
| **Physical** | Data centers, power, cooling, network | None |
| **Infrastructure** | EC2, RDS, Lambda hosts, virtualization | None |
| **Operating System** | None (managed services) | EC2 OS patching, hardening |
| **Application** | Application services | Your application code |
| **Data** | None | Encryption, access control |
| **Identity** | Identity service (IAM) | IAM policies, users |
| **Network** | AWS networking | VPC, security groups |

### Visual Diagram

```
            SHARED RESPONSIBILITY MODEL
            ======================

    CUSTOMER                          AWS
    ┌──────────────┐               ┌──────────────┐
    │  Customer │               │     AWS     │
    │   Data   │◄─────────────►│  Cloud      │
    │          │               │  Infrastructure│
    │ ┌──────┐ │               │             │
    │ │ Apps │ │               │  ┌───────┐ │
    │ │Code  │ │               │  │Hardware│
    │ └──────┘ │               │  │Network│
    │ ┌──────┐ │               │  │Virtual│
    │ │ IAM  │ │               │  │ hyperv│
    │ │Policies│               │  │       │
    │ └──────┘ │               │  └───────┘ │
    └─────┬─────┘               └─────┬─────┘
          │                           │
          │      SHARED BOUNDARY       │
          └───────────────────────────┘

    ┌��────────────────────────────────────────┐
    │          CUSTOMER RESPONSIBILITY           │
    │                                         │
    │  ┌─────────┐ ┌──────────┐ ┌────────┐  │
    │  │  Data   │ │Identity │ │ Config│  │
    │  │Encrypt │ │ Access  │ │ EC2/S3│  │
    │  └─────────┘ └──────────┘ └────────┘  │
    └─────────────────────────────────────────┘
```

### Service Categories

**Inherited Controls**: Physical infrastructure (AWS responsibility)

**Customer Responsibility**:
- User access (IAM)
- Encryption options
- Data classification
- Application security

**Shared Controls**:
- Patching (OS and guest)
- Configuration management
- Awareness and training

## HOW

### Example 1: Customer Responsibilities in Practice

```bash
# AWS responsibility: Underlying EC2 host patching
# Customer responsibility: EC2 instance patching

# Update EC2 instance
ssh ec2-user@instance
sudo yum update -y

# Customer responsibility: Security group configuration
aws ec2 create-security-group \
    --group-name secure-sg \
    --description "Restrictive security group"

# Only allow specific traffic
aws ec2 authorize-security-group-ingress \
    --group-id sg-xxx \
    --protocol tcp \
    --port 443 \
    --cidr 10.0.0.0/16

# Customer responsibility: IAM policies
aws iam create-policy \
    --policy-name ReadOnlyS3 \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": ["s3:GetObject"],
            "Resource": "arn:aws:s3:::my-bucket/*"
        }]
    }'
```

### Example 2: Data Protection

```bash
# S3: Customer manages encryption keys
aws s3api put-bucket-encryption \
    --bucket my-bucket \
    --server-side-encryption-configuration '{
        "Rules": [{
            "ApplyServerSideEncryptionByDefault": {
                "SSEAlgorithm": "AES256"
            }
        }]
    }'

# Enable versioning (customer data protection)
aws s3api put-bucket-versioning \
    --bucket my-bucket \
    --versioning-configuration Status=Enabled

# Customer: Data classification
# Tag data by sensitivity
aws s3api put-object-tagging \
    --bucket my-bucket \
    --key sensitive-data.csv \
    --tagging 'TagSet=[{Key=Classification,Value=PII}]'
```

### Example 3: Identity and Access

```bash
# AWS: Manages IAM service
# Customer: Manages policies and users

# Create users with least privilege
aws iam create-user --user-name app-developer

# Attach specific policies
aws iam attach-user-policy \
    --user-name app-developer \
    --policy-arn arn:aws:iam::aws:policy/AmazonS3FullAccess

# Enable MFA (customer responsibility)
aws iam create-virtual-mfa-device \
    --user-name root \
    --virtual-mfa-device-serial-number arn:aws:iam::123456789012:mfa/root

# Enforce password policy
aws iam update-account-password-policy \
    --require-uppercase-characters \
    --require-symbols \
    --minimum-length 16
```

### Example 4: Configuration Management

```bash
# AWS Config tracks resources
# AWS: Monitor changes
# Customer: Respond to issues

# Enable AWS Config
aws configservice put-configuration-recorder \
    --configuration-recorder '{
        "name": "default",
        "roleARN": "arn:aws:iam::123456789012:role/config-role"
    }'

# Rules for best practices
aws configservice put-config-rule \
    --config-rule '{
        "name": "required-tag",
        "source": {
            "owner": "AWS",
            "identifier": "REQUIRES_TAG"
        },
        "inputParameters": {
            "tag1Key": "CostCenter"
        }
    }'
```

## COMMON ISSUES

### 1. Assuming AWS Handles Everything

**Problem**: Customer assumes security is AWS's job.

**Solution**: Understand this model exactly as described here.

### 2. Weak IAM Policies

**Problem**: Full admin access for convenience.

**Solution**: Use least privilege, regularly audit.

### 3. Unencrypted Data

**Problem**: Data at rest unencrypted.

**Solution**: Enable encryption options on all services.

### 4. Public Resources

**Problem**: S3 buckets, security groups too open.

**Solution**: Regular audits, block public access.

## COMPLIANCE

### AWS Compliance Certifications

- SOC 1/2/3
- ISO 27001, 27017, 27018
- FedRAMP Moderate/High
- HIPAA
- PCI DSS
- NIST 800-53

### Customer Responsibility

- Validate controls for your compliance
- Use AWS artifacts
- Configure services securely

## CROSS-REFERENCES

### Related Concepts

- IAM: Customer identity and access
- VPC: Network isolation
- Encryption: Data protection

### Service-Specific

- EC2: Customer manages OS
- RDS: Customer configures access
- S3: Customer sets bucket policies

### Prerequisites

- Basic Cloud Concepts

### What to Study Next

1. Security Services overview
2. Encryption best practices
3. Compliance programs