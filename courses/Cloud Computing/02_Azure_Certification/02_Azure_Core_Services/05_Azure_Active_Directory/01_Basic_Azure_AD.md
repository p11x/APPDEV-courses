---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Active Directory
Purpose: Understanding Azure AD identity and access management
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_AD.md, 03_Practical_Azure_AD.md
UseCase: Managing identity and access in Azure
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure AD is the identity and access management solution for Azure. Understanding authentication and authorization is critical for security.

## 📖 WHAT

### Azure AD Concepts

**Tenant**: Azure AD directory instance

**User**: Identity for people

**Group**: Collection of users

**Application**: Software that uses Azure AD

**Service Principal**: Identity for applications

**RBAC**: Role-based access control

### Authentication Methods

| Method | Description |
|--------|-------------|
| Single Sign-On (SSO) | One login for multiple apps |
| MFA | Multi-factor authentication |
| Conditional Access | Policy-based access |
| B2B | Guest user access |
| B2C | Consumer identity |

## 🔧 HOW

### Example 1: Create User

```bash
# Create user
az ad user create \
    --display-name "John Doe" \
    --password "SecurePassword123!" \
    --user-principal-name johnd@contoso.com

# List users
az ad user list

# Assign role
az role assignment create \
    --assignee johnd@contoso.com \
    --role "Reader" \
    --scope /subscriptions/sub-id/resourceGroups/myrg
```

### Example 2: Create Service Principal

```bash
# Create SP
az ad sp create-for-rbac \
    --name myapp

# Get SP details
az ad sp list --display-name myapp
```

## ✅ EXAM TIPS

- Azure AD = Identity as a Service
- RBAC for resource access
- Conditional Access for security policies