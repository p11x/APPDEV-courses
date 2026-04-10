---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: IAM Security
Purpose: Hands-on IAM implementation including users, roles, policies, and MFA configuration
Difficulty: intermediate
Prerequisites: 01_Basic_IAM.md, 02_Advanced_IAM.md
RelatedFiles: 01_Basic_IAM.md, 02_Advanced_IAM.md
UseCase: Production IAM setup and management
CertificationExam: AWS SysOps Administrator
LastUpdated: 2025
---

## 💡 WHY

Hands-on IAM implementation is essential for securing AWS environments. This lab covers user management, role-based access, and security best practices.

## 📖 WHAT

### Lab Architecture

```
IAM Lab Structure
=================

┌─────────────────────────────────────────┐
│            AWS Account                   │
│  ┌──────────┐  ┌──────────┐  ┌────────┐ │
│  │  Users   │  │  Roles   │  │Groups │ │
│  │ - Admin  │  │ - AppRole│  │ - Devs │ │
│  │ - Dev    │  │ - Batch  │  │ - Ops  │ │
│  │ - App    │  │ - Lambda │  │ - DBA  │ │
│  └──────────┘  └──────────┘  └────────┘ │
│          │          │          │         │
│          └──────────┼──────────┘         │
│                     │                    │
│              ┌──────┴──────┐             │
│              │   Policies   │             │
│              │ - S3Full     │             │
│              │ - EC2Full    │             │
│              │ - LambdaExec │             │
│              └─────────────┘             │
└─────────────────────────────────────────┘
```

## 🔧 HOW

### Module 1: User and Group Setup

```bash
#!/bin/bash
# IAM Setup Script

# Create groups
aws iam create-group --group-name Developers
aws iam create-group --group-name Administrators
aws iam create-group --group-name DatabaseAdmins

# Create users
aws iam create-user --user-name developer1
aws iam create-user --user-name developer2
aws iam create-user --user-name dbadmin

# Add users to groups
aws iam add-user-to-group \
    --user-name developer1 \
    --group-name Developers
aws iam add-user-to-group \
    --user-name developer2 \
    --group-name Developers
aws iam add-user-to-group \
    --user-name dbadmin \
    --group-name DatabaseAdmins

echo "Users and groups created"
```

### Module 2: Policy Creation

```bash
# Developer policy - S3 and EC2 read access
aws iam create-policy \
    --policy-name DeveloperReadPolicy \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Sid": "S3ReadAccess",
            "Effect": "Allow",
            "Action": [
                "s3:ListBucket",
                "s3:GetObject"
            ],
            "Resource": [
                "arn:aws:s3:::dev-bucket",
                "arn:aws:s3:::dev-bucket/*"
            ]
        }, {
            "Sid": "EC2ReadAccess",
            "Effect": "Allow",
            "Action": [
                "ec2:Describe*",
                "ec2:GetConsoleOutput"
            ],
            "Resource": "*"
        }]
    }'

# Database admin policy
aws iam create-policy \
    --policy-name DatabaseAdminPolicy \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": [
                "rds:*",
                "dynamodb:*"
            ],
            "Resource": "*"
        }]
    }'

# Attach policies to groups
aws iam attach-group-policy \
    --group-name Developers \
    --policy-arn arn:aws:iam::aws:policy/ReadOnlyAccess

aws iam attach-group-policy \
    --group-name Developers \
    --policy-arn arn:aws:iam::123456789:policy/DeveloperReadPolicy

aws iam attach-group-policy \
    --group-name DatabaseAdmins \
    --policy-arn arn:aws:iam::123456789:policy/DatabaseAdminPolicy
```

### Module 3: Role Creation for Services

```bash
# EC2 application role
aws iam create-role \
    --role-name AppServerRole \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"Service": "ec2.amazonaws.com"},
            "Action": "sts:AssumeRole"
        }]
    }'

aws iam attach-role-policy \
    --role-name AppServerRole \
    --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess

# Lambda execution role
aws iam create-role \
    --role-name LambdaExecutionRole \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"Service": "lambda.amazonaws.com"},
            "Action": "sts:AssumeRole"
        }]
    }'

aws iam attach-role-policy \
    --role-name LambdaExecutionRole \
    --policy-arn arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole

# Create instance profile
aws iam create-instance-profile \
    --instance-profile-name AppServerProfile
aws iam add-role-to-instance-profile \
    --instance-profile-name AppServerProfile \
    --role-name AppServerRole

echo "Roles created"
```

### Module 4: MFA Configuration

```bash
# Enable MFA on root account (for root user)
# Note: Must be done via AWS Console or as root

# For IAM users, enable virtual MFA
aws iam create-virtual-mfa-device \
    --virtual-mfa-device-name admin-mfa \
    --outfile ./mfa-qr.png

# Enable MFA for user
aws iam enable-mfa-device \
    --user-name admin \
    --serial-number arn:aws:iam::123456789:mfa/admin \
    --authentication-code-1 123456 \
    --authentication-code-2 789012

# Set password policy
aws iam update-account-password-policy \
    --minimum-length 16 \
    --require-uppercase-characters \
    --require-lowercase-characters \
    --require-numbers \
    --require-symbols \
    --allow-users-to-change-password \
    --max-password-age 90
```

### Module 5: Access Key Management

```bash
# Create access keys for programmatic access
aws iam create-access-key --user-name developer1

# List access keys
aws iam list-access-keys --user-name developer1

# Rotate access keys
# 1. Create new key
NEW_KEY=$(aws iam create-access-key --user-name developer1)
# 2. Update applications
# 3. Delete old key
aws iam delete-access-key \
    --user-name developer1 \
    --access-key-id AKIAIOSFODNN7EXAMPLE
```

## ⚠️ COMMON ISSUES

### 1. Access Denied Errors

**Problem**: User can't perform action.

**Solution**: Check IAM policy, group membership, and permission boundaries.

### 2. MFA Not Working

**Problem**: MFA validation fails.

**Solution**: Ensure time sync is correct, use correct code from device.

### 3. Role Assumption Fails

**Problem**: Cannot assume role.

**Solution**: Verify trust policy allows your principal, check session duration.

## 🔗 CROSS-REFERENCES

**Related**: CloudTrail, Config, Security Hub

**Next**: Add IAM Access Analyzer, enable CloudTrail