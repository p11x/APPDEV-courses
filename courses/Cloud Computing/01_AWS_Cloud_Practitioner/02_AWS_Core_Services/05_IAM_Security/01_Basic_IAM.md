---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: IAM Security
Purpose: Understanding AWS Identity and Access Management for authentication, authorization, and access control
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_IAM.md, 03_Practical_IAM.md
UseCase: Managing user access and permissions securely
CertificationExam: AWS Certified Cloud Practitioner - Domain 2: Security
LastUpdated: 2025
---

## WHY

IAM is the foundation of AWS security, controlling who can access what resources. Poor IAM practices are the #1 cause of security incidents in AWS. Understanding IAM is essential for any AWS practitioner.

### Why IAM Matters

- **Security**: Controls access to all AWS resources
- **Compliance**: Meets regulatory requirements
- **Audit**: Provides access logging
- **Least Privilege**: Minimizes attack surface
- **Automation**: Enables programmatic access

### Security Statistics

- 95% of data breaches involve IAM misconfiguration
- Compromised credentials cause 80%+ of AWS incidents
- Organizations using MFA see 99.9% fewer account compromises

### When NOT to Use IAM

- Use instance metadata for EC2 service roles
- Use service-linked roles for AWS services

## WHAT

### IAM Core Concepts

**Root Account**: First AWS account with full access. Should not be used regularly.

**IAM User**: Individual identity for accessing AWS. Can have long-term credentials (access keys, passwords).

**IAM Group**: Collection of users with shared permissions.

**IAM Role**: Temporary credentials for temporary access. Can be assumed by anyone or service.

**Policy**: JSON document defining permissions. Types:

- **Managed Policies**: AWS-created, reusable
- **Inline Policies**: Attached directly to user/role/group
- **Service Control Policies**: Organization-level

### Policy Structure

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "s3:GetObject",
                "s3:ListBucket"
            ],
            "Resource": [
                "arn:aws:s3:::my-bucket",
                "arn:aws:s3:::my-bucket/*"
            ]
        }
    ]
}
```

### Key Policy Elements

| Element | Purpose |
|---------|--------|
| Version | Policy language version |
| Effect | Allow or Deny |
| Action | Operations to permit/deny |
| Resource | ARNs of affected resources |
| Principal | Who the policy applies to |
| Condition | When policy applies |

### Architecture Diagram

```
                    IAM ARCHITECTURE
                    ==============

    ┌─────────────────────────────────────────────────────────────┐
    │                   AUTHENTICATION                       │
    │         (Who are you?)                                │
    └─────────────────────────────────────────────────────────────┘
                           │
    ┌───────────────────────┼───────────────────────────────┐
    │                   │                           │
    ▼                   ▼                           ▼
┌──────────┐      ┌──────────┐             ┌──────────┐
│  User   │      │  Role   │             │Root Acct │
│Credentials│      │Assume  │             │ Full    │
└────┬────┘      └────┬────┘             └───┬────┘
     │                 │                      │
     └────────────────┼──────────────────────┘
                    │
    ┌───────────────┴───────────────────────────────┐
    │            AUTHORIZATION                     │
    │         (What can you do?)                 │
    └─────────────────────────────────────────┘
                    │
    ┌───────────────┼─────────────────────────────┐
    │               │                         │
    ▼               ▼                         ▼
┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│ Allow     │  │ Deny      │  │   Audit   │
│ Actions  │  │ Override │  │  Access  │
│ (Policy) │  │ (Deny)  │  │ (CloudTrail)│
└──────────────┘  └──────────────┘  └──────────────┘
```

### Best Practice Policies

| Group | Use Case | Policy |
|-------|---------|-------|
| Administrators | Full access | AdministratorAccess |
| Developers | Deploy, manage | PowerUserAccess |
| Auditors | Read-only | ReadOnlyAccess |
| Billing | Billing only | Billing |
| Support | Support cases | SupportUserAccess |

## HOW

### Example 1: Create IAM User with Programmatic Access

```bash
# Step 1: Create IAM user
aws iam create-user \
    --user-name developer \
    --tags 'Key=Department,Value=Engineering'

# Step 2: Create access key
aws iam create-access-key \
    --user-name developer

# Output includes AccessKeyId and SecretAccessKey
# IMPORTANT: Save SecretAccessKey immediately

# Step 3: Attach policy
aws iam attach-user-policy \
    --user-name developer \
    --policy-arn arn:aws:iam::aws:policy/PowerUserAccess

# Step 4: Add to group
aws iam create-group --group-name Developers
aws iam add-user-to-group \
    --user-name developer \
    --group-name Developers

# Step 5: Enable MFA
aws iam create-virtual-mfa-device \
    --user-name developer \
    --virtual-mfa-device-serial-number arn:aws:iam::123456789012:mfa/developer

# Respond to enable (requires QR code scan in authenticator app)
aws iam enable-mfa-device \
    --user-name developer \
    --serial-number arn:aws:iam::123456789012:mfa/developer \
    --authentication-code-1 123456 \
    --authentication-code-2 789012
```

### Example 2: Create IAM Role for Service

```bash
# Create role for EC2 service
aws iam create-role \
    --role-name EC2WebServerRole \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"Service": "ec2.amazonaws.com"},
            "Action": "sts:AssumeRole"
        }]
    }'

# Attach AmazonS3ReadOnly policy
aws iam attach-role-policy \
    --role-name EC2WebServerRole \
    --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess

# Create instance profile
aws iam create-instance-profile \
    --instance-profile-name EC2WebProfile
aws iam add-role-to-instance-profile \
    --instance-profile-name EC2WebProfile \
    --role-name EC2WebServerRole

# Associate with EC2 instance at launch
aws ec2 run-instances \
    ... (--iam-instance-profile Arn=...)
```

### Example 3: Create Custom Policy

```bash
# Create custom policy
aws iam create-policy \
    --policy-name S3BucketAccess \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": [
                "s3:ListBucket",
                "s3:GetObject",
                "s3:PutObject"
            ],
            "Resource": [
                "arn:aws:s3:::my-project-bucket",
                "arn:aws:s3:::my-project-bucket/*"
            ]
        }]
    }' \
    --description "Read-write access to project bucket"

# Attach to user
aws iam attach-user-policy \
    --user-name developer \
    --policy-arn arn:aws:iam::123456789012:policy/S3BucketAccess
```

### Example 4: Password Policy Configuration

```bash
# Update account password policy
aws iam update-account-password-policy \
    --minimum-length 16 \
    --require-uppercase-characters \
    --require-lowercase-characters \
    --require-numbers \
    --require-symbols \
    --allow-users-to-change-password \
    --max-password-age 90 \
    --password-reuse-prevention 24
```

### Example 5: Cross-Account Access

```bash
# Account A: Create role for Account B
aws iam create-role \
    --name CrossAuditRole \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"AWS": "arn:aws:iam::OTHER_ACCOUNT:root"},
            "Action": "sts:AssumeRole"
        }]
    }'

# Attach read-only policy
aws iam attach-role-policy \
    --role-name CrossAuditRole \
    --policy-arn arn:aws:iam::aws:policy/ReadOnlyAccess

# Account B: Assume the role
aws sts assume-role \
    --role-arn arn:aws:iam::123456789012:role/CrossAuditRole \
    --role-session-name audit-session

# Returns temporary credentials
```

## COMMON ISSUES

### 1. Accidental Admin Access

**Problem**: User has more permissions than needed.

**Solution**:
- Use least privilege
- Regularly audit policies
- Use IAM Access Analyzer

### 2. Lost Credentials

**Problem**: Lost access key or password.

**Solution**:
- Always have backup admin access
- Use MFA
- Rotate credentials regularly

### 3. Hardcoded Credentials

**Problem**: Credentials in code.

**Solution**:
- Use IAM roles
- Use environment variables
- Use secrets management

### 4. Overly Permissive Policies

**Problem**: Using "Action": "*" or "Resource": "*".

**Solution**:
- Be specific
- Use IAM Access Analyzer
- Follow least privilege

## PERFORMANCE

### IAM Limits

| Resource | Limit |
|----------|-------|
| Users per account | 5,000 |
| Groups per account | 1,000 |
| Roles per account | 1,000 |
| Policies per account | 1,000 |
| Access keys per user | 2 |

### Permission Strategies

| Strategy | Use Case | Overhead |
|----------|----------|----------|
| Managed policies | Common patterns | Low |
| Custom policies | Specific resources | Medium |
| Inline policies | One-off | High |

## COMPATIBILITY

### MFA Options

- Virtual (Google Authenticator, Authy)
- Hardware (YubiKey)
- SMS (less recommended)

### Federation

- SAML 2.0 (Active Directory)
- OIDC (external IdP)
- AWS SSO

## CROSS-REFERENCES

### Related Services

- All AWS services use IAM
- CloudTrail: Access logging
- AWS Organizations: SCPs

### Prerequisites

- Basic Cloud Concepts

### What to Study Next

1. Advanced IAM: Policies, federation
2. Security Services: GuardDuty, Config
3. Cost Management: Tags