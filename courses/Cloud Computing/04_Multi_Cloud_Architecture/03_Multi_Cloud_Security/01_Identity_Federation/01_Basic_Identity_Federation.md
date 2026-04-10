---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud Security
Concept: Identity Federation
Difficulty: beginner
Prerequisites: Basic Cloud Computing, Identity Management Basics
RelatedFiles: 02_Advanced_Identity_Federation.md, 03_Practical_Identity_Federation.md
UseCase: Implementing identity federation for multi-cloud environments
CertificationExam: AWS Solutions Architect / Professional
LastUpdated: 2025
---

## WHY

Identity federation enables single sign-on across multiple cloud providers, essential for multi-cloud security and operational efficiency.

### Why Identity Federation Matters

- **Single Sign-On**: One credential for all clouds
- **Security**: Centralized access control
- **Compliance**: Audit trails, access policies
- **Productivity**: Reduced password fatigue
- **Reduced Admin**: Centralized user management

### Federation Benefits

| Benefit | Description | Impact |
|---------|-------------|--------|
| SSO | One login for all | User productivity |
| Centralized Auth | Single source of truth | Security |
| RBAC | Role-based access | Compliance |
| Audit Logging | Centralized logs | Governance |

## WHAT

### Cloud Identity Services

**AWS Identity and Access Management (IAM)**
- IAM roles for federation
- IAM identity providers (SAML, OIDC)
- AWS SSO
- AWS IAM Identity Center

**Azure Active Directory (Entra ID)**
- Enterprise applications
- SAML/OIDC federation
- Conditional access
- Azure AD Connect

**GCP Cloud Identity**
- Cloud Identity (free/paid)
- SAML federation
- IAM federation
- BeyondCorp

### Federation Architecture

```
IDENTITY FEDERATION ARCHITECTURE
================================

┌─────────────────────────────────────────────────────────────┐
│                    IDENTITY PROVIDER                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  Okta       │  │  Azure AD   │  │   On-Prem    │      │
│  │  Ping       │  │  OneLogin   │  │   AD/LDAP    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                    │         │         │
                    ▼         ▼         ▼
┌─────────────────────────────────────────────────────────────┐
│                    FEDERATION PROTOCOL                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   SAML 2.0  │  │    OIDC      │  │     OAuth    │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                    │         │         │
                    ▼         ▼         ▼
┌─────────────────────────────────────────────────────────────┐
│                    CLOUD PROVIDERS                           │
│  ┌────────┐    ┌────────┐    ┌────────┐                   │
│  │  AWS  │    │ Azure  │    │  GCP   │                   │
│  │   IAM │    │ Entra  │    │ Cloud  │                   │
│  └────────┘    └────────┘    └────────┘                   │
└─────────────────────────────────────────────────────────────┘
```

## HOW

### Example 1: AWS IAM Identity Provider

```hcl
# AWS IAM identity provider for SAML
resource "aws_iam_saml_provider" "main" {
  name                   = "okta-saml-provider"
  saml_metadata_document = file("saml-metadata.xml")
}

# IAM role for federated users
resource "aws_iam_role" "federated_admin" {
  name = "federated-admin-role"
  
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Principal = {
          Federated = aws_iam_saml_provider.main.arn
        }
        Action = "sts:AssumeRoleWithSAML"
        Condition = {
          StringEquals = {
            "SAML:aud": "https://signin.aws.amazon.com/saml"
          }
        }
      }
    ]
  })
}

# IAM policy for federated role
resource "aws_iam_policy" "federated_admin_policy" {
  name = "federated-admin-policy"
  
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "ec2:*",
          "s3:*",
          "rds:*"
        ]
        Resource = "*"
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "federated_admin" {
  role       = aws_iam_role.federated_admin.name
  policy_arn = aws_iam_policy.federated_admin_policy.arn
}
```

### Example 2: Azure AD Enterprise Application

```powershell
# Azure AD federation configuration
# Create enterprise application
$app = New-AzureADMSApplication `
    -DisplayName "MultiCloud App" `
    -Web `
    @{
        HomePageUrl = "https://app.example.com"
        RedirectUris = "https://app.example.com/callback"
    }

# Configure SAML settings
$ Sammy = Set-AzureADMSApplication `
    -ObjectId $app.ObjectId `
    -SamlMetadataUrl "https://idp.example.com/metadata"

# Create service principal
New-AzureADServicePrincipal `
    -AppId $app.AppId

# Configure single sign-on
Set-AzureADMSApplication `
    -ObjectId $app.ObjectId `
    -SignInUrl "https://idp.example.com/login"
```

### Example 3: GCP Workload Identity Federation

```hcl
# GCP Workload Identity Pool
resource "google_iam_workload_identity_pool" "main" {
  workload_identity_pool_id = "multi-cloud-pool"
  display_name              = "Multi-Cloud Identity Pool"
  description               = "Federated identities from IdP"
  
  attribute_mapping = {
    "google.subject"           = "assertion.sub"
    "attribute.actor"         = "assertion.act"
    "attribute.department"    = "assertion.dept"
    "attribute.email"         = "assertion.email"
  }
}

# Workload identity provider
resource "google_iam_workload_identity_pool_provider" "okta" {
  workload_identity_pool_id          = google_iam_workload_identity_pool.main.workload_identity_pool_id
  workload_identity_pool_provider_id = "okta-provider"
  display_name                      = "Okta Provider"
  description                       = "Okta as identity provider"
  
  attribute_mapping = {
    "google.subject" = "assertion.sub"
  }
  
  oidc {
    issuer_uri = "https://example.okta.com"
    jwks_uri   = "https://example.okta.com/.well-known/jwks.json"
  }
}

# GCP service account for impersonation
resource "google_service_account" "federated" {
  account_id   = "federated-sa"
  display_name = "Federated Service Account"
}

# IAM policy to allow impersonation
resource "google_iam_service_account_iam_member" "workload_identity_user" {
  service_account_id = google_service_account.federated.account_id
  member             = "principal://iam.googleapis.com/projects/${var.project_number}/locations/global/workloadIdentityPools/${google_iam_workload_identity_pool.main.workload_identity_pool_id}/subject/${var.subject}"
  condition {
    title       = "Condition for federation"
    expression = "assertion.department == 'engineering'"
  }
}
```

## COMMON ISSUES

### 1. Attribute Mapping

- Different claims between IdPs
- Solution: Use flexible mapping

### 2. Session Duration

- Token expiration
- Solution: Configure appropriate TTL

### 3. MFA Requirements

- Conditional access policies
- Solution: Configure in IdP

## CROSS-REFERENCES

### Prerequisites

- Identity management basics
- SAML/OIDC concepts
- Cloud provider IAM

### What to Study Next

1. CSPM
2. Zero Trust
3. Multi-Cloud Security

## EXAM TIPS

- Know federation protocols
- Understand identity provider configuration
- Be able to implement SSO for multi-cloud