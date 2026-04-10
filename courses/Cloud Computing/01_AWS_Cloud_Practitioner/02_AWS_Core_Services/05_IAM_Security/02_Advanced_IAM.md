---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: IAM Security
Purpose: Advanced IAM implementation including federation, organizations, and permission boundaries
Difficulty: advanced
Prerequisites: 01_Basic_IAM.md
RelatedFiles: 01_Basic_IAM.md, 03_Practical_IAM.md
UseCase: Enterprise identity management and cross-account access
CertificationExam: AWS Security Specialty
LastUpdated: 2025
---

## 💡 WHY

Advanced IAM is critical for enterprise environments requiring federated identity, cross-account access, and fine-grained permissions. Understanding these concepts prevents security vulnerabilities.

## 📖 WHAT

### Key Concepts

**IAM Identity Center**: SSO for multiple AWS accounts

**SAML Federation**: Integrate with corporate IdP (Active Directory)

**OIDC Federation**: External identity providers

**Permission Boundaries**: Delegate permissions safely

**Service Control Policies**: Organization-wide access control

### Architecture: Cross-Account Access

```
Cross-Account Access Architecture
=================================

  Account A (Dev)           Account B (Prod)
  ┌──────────────┐         ┌──────────────┐
  │ Developer    │────────►│ S3 Bucket    │
  │ (IAM User)  │ Assume  │ (Prod Data)  │
  └──────────────┘ Role   └──────────────┘
```

## 🔧 HOW

### Example 1: SAML Federation

```bash
# Create SAML provider
aws iam create-saml-provider \
    --name CorpAD \
    --saml-metadata-document file://metadata.xml \
    --output json

# Create role for federated users
aws iam create-role \
    --role-name DeveloperRole \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {
                "Federated": "arn:aws:iam::123456789:saml-provider/CorpAD"
            },
            "Action": "sts:AssumeRoleWithSAML",
            "Condition": {
                "StringEquals": {
                    "SAML:aud": "https://signin.aws.amazon.com/saml"
                }
            }
        }]
    }'

# Attach policy
aws iam attach-role-policy \
    --role-name DeveloperRole \
    --policy-arn arn:aws:iam::aws:policy/ReadOnlyAccess
```

### Example 2: Cross-Account Access

```bash
# Account A: Create role for Account B
aws iam create-role \
    --name CrossAccountReader \
    --assume-role-policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Principal": {"AWS": "arn:aws:iam::ACCOUNT_B:root"},
            "Action": "sts:AssumeRole"
        }]
    }'

# Account A: Attach policy to role
aws iam put-role-policy \
    --role-name CrossAccountReader \
    --policy-name allow-read-s3 \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": ["s3:ListBucket", "s3:GetObject"],
            "Resource": "arn:aws:s3:::prod-reports/*"
        }]
    }'

# Account B: Assume the role
aws sts assume-role \
    --role-arn arn:aws:iam::ACCOUNT_A:role/CrossAccountReader \
    --role-session-name prod-access
```

### Example 3: Permission Boundaries

```bash
# Create policy for delegated admin
aws iam create-policy \
    --policy-name DevAdminPolicy \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": [
                "ec2:*",
                "s3:*",
                "rds:*"
            ],
            "Resource": "*"
        }]
    }'

# Create permission boundary
aws iam create-policy \
    --policy-name DevBoundary \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": "*",
            "Resource": "*"
        }, {
            "Effect": "Deny",
            "Action": ["iam:*", "aws-portal:*"],
            "Resource": "*"
        }]
    }'

# Create user with boundary
aws iam create-user --user-name developer
aws iam put-user-policy \
    --user-name developer \
    --policy-name AdminWithBoundary \
    --policy-document '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Allow",
            "Action": ["ec2:*", "s3:*"],
            "Resource": "*"
        }]
    }'
aws iam put-user-permissions-boundary \
    --user-name developer \
    --permissions-boundary arn:aws:iam::123456789:policy/DevBoundary
```

### Example 4: AWS Organizations SCP

```bash
# Create SCP to restrict regions
aws organizations create-policy \
    --content '{
        "Version": "2012-10-17",
        "Statement": [{
            "Effect": "Deny",
            "Action": "*",
            "Resource": "*",
            "Condition": {
                "StringNotEquals": {
                    "aws:RequestedRegion": ["us-east-1", "us-west-2"]
                }
            }
        }]
    }' \
    --description "Restrict to US regions" \
    --name RestrictRegions \
    --type SERVICE_CONTROL_POLICY
```

## ⚠️ COMMON ISSUES

### 1. Federation Setup Failures

**Problem**: SAML assertion not accepted.

**Solution**: Verify SAML metadata is current, check attribute mapping.

### 2. Cross-Account Access Denied

**Problem**: Cannot assume role in other account.

**Solution**: Check trust policy allows your account, verify permissions.

### 3. Permission Boundary Not Working

**Problem**: User has more permissions than expected.

**Solution**: Ensure boundary explicitly denies restricted actions.

## 🏃 PERFORMANCE

**Limits**: 500 roles per account, 10 federated providers, unlimited users

## 🌐 COMPATIBILITY

| Feature | AWS | Azure AD | GCP IAM |
|---------|-----|----------|---------|
| SAML | Yes | Native | Yes |
| OIDC | Yes | Yes | Yes |
| SCPS | Yes | N/A | Organization policies |

## 🔗 CROSS-REFERENCES

**Related**: CloudTrail, Config, Security Hub

**Prerequisite**: Basic IAM understanding

## ✅ EXAM TIPS

- Cross-account uses STS AssumeRole
- SAML uses sts:AssumeRoleWithSAML
- SCPs affect all accounts in organization
- Permission boundaries limit effective permissions