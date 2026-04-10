---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud Security
Concept: CSPM
Difficulty: beginner
Prerequisites: Basic Cloud Computing, Cloud Security Basics
RelatedFiles: 02_Advanced_CSPM.md, 03_Practical_CSPM.md
UseCase: Understanding Cloud Security Posture Management for multi-cloud environments
CertificationExam: AWS Solutions Architect / Professional
LastUpdated: 2025
---

## WHY

CSPM (Cloud Security Posture Management) provides continuous security monitoring and compliance for multi-cloud environments, essential for identifying misconfigurations and security gaps.

### Why CSPM Matters

- **Misconfiguration Detection**: Find security gaps across clouds
- **Compliance Monitoring**: Maintain regulatory compliance
- **Continuous Assessment**: Real-time security posture
- **Remediation**: Automated or guided fixes
- **Visibility**: Single pane of glass for all clouds

### CSPM Benefits

| Benefit | Description | Impact |
|---------|-------------|--------|
| Automated Scanning | Continuous checks | Early detection |
| Compliance Mapping | Map to controls | Audit readiness |
| Risk Scoring | Prioritize issues | Efficient remediation |
| Trend Analysis | Track improvements | Progress visibility |

## WHAT

### CSPM Tools and Services

**AWS Security Hub**
- Security standards (CIS, PCI DSS)
- Finding aggregation
- CloudTrail events
- Automated remediation

**Microsoft Defender for Cloud**
- Azure security center
- Multi-cloud support
- Just-in-time access
- Regulatory compliance

**GCP Security Command Center**
- Security Health Analytics
- Event Threat Detection
- Web Security Scanner
- Container Security

### CSPM Architecture

```
CSPM ARCHITECTURE
=================

┌─────────────────────────────────────────────────────────────┐
│                    CSPM DASHBOARD                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ Security    │  │ Compliance  │  │ Risk         │      │
│  │ Findings    │  │ Score       │  │ Trends       │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                         │
┌─────────────────────────┼───────────────────────────────────┐
│                  SCANNING ENGINE                            │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │ AWS Config   │  │ Azure Policy│  │ GCP Security│       │
│  │ Rules        │  │ Evaluator   │  │ Health      │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                         │
┌─────────────────────────┼───────────────────────────────────┐
│                    CLOUD RESOURCES                          │
│  ┌────────┐    ┌────────┐    ┌────────┐    ┌────────┐      │
│  │  AWS   │    │ Azure  │    │  GCP   │    │On-Prem │      │
│  │Resources│    │Resources│   │Resources│   │Resources│     │
│  └────────┘    └────────┘    └────────┘    └────────┘     │
└─────────────────────────────────────────────────────────────┘
```

## HOW

### Example 1: AWS Security Hub Configuration

```hcl
# AWS Security Hub configuration
resource "aws_securityhub_account" "main" {}

resource "aws_securityhub_standards_subscription" "cis" {
  standards_arn = "arn:aws:securityhub:::ruleset/cis-aws-foundations-benchmark/v/1.2.0"
}

resource "aws_securityhub_standards_subscription" "pci_dss" {
  standards_arn = "arn:aws:securityhub:::ruleset/pci-dss/v/3.2.1"
}

resource "aws_securityhub_standards_subscription" "aws_best_practices" {
  standards_arn = "arn:aws:securityhub:::ruleset/aws-foundational-security-best-practices/v/1.0.0"
}

# Enable security hub master
resource "aws_securityhub_member" "member_account" {
  account_id = var.member_account_id
  email      = var.member_email
  
  depends_on = [aws_securityhub_account.main]
}

# Security Hub findings filter
resource "aws_securityhub_finding" "example" {
  provider {
    company_name  = "AWS"
    product_name  = "Security Hub"
  }
  
  schema_version = "2018-10-08"
  
  title = "S3 Bucket Public Access"
  description = "S3 bucket should not be publicly accessible"
  
  severity {
    label = "HIGH"
  }
  
  resource {
    id = "arn:aws:s3:::my-bucket"
    partition = "aws"
    region = "us-east-1"
    type = "AwsS3Bucket"
  }
}
```

### Example 2: Azure Defender for Cloud

```powershell
# Azure Defender for Cloud configuration
# Enable Defender for Cloud
Set-AzSecurityPricing `
    -Name "CloudDefender" `
    -PricingTier "Standard"

# Enable specific Defender plans
Set-AzSecurityPricing `
    -Name "VirtualMachines" `
    -PricingTier "Standard"

Set-AzSecurityPricing `
    -Name "StorageAccounts" `
    -PricingTier "Standard"

Set-AzSecurityPricing `
    -Name "Kubernetes" `
    -PricingTier "Standard"

Set-AzSecurityPricing `
    -Name "SqlServers" `
    -PricingTier "Standard"

# Create security contacts
New-AzSecurityContact `
    -Name "Security Team" `
    -Email "security@example.com" `
    -Phone "+1-555-0100" `
    -AlertNotifications "On" `
    -AlertsToAdmins "On"

# Set security policy
Set-AzSecurityPolicy `
    -Name "SecurityPolicy" `
    -PolicyDefinitionGuid "e4b3c4c0-0000-0000-0000-000000000000" `
    -Enabled $true
```

### Example 3: GCP Security Command Center

```hcl
# GCP Security Command Center configuration
resource "google_scc_source" "security" {
  display_name = "Security Command Center"
  description  = "Security findings source"
  
  provider_config {
    name = "default"
  }
}

resource "google_scc_notification_config" "security" {
  name        = "security-notification"
  source_id   = google_scc_source.security.name
  description = "Security notifications"
  
  notification_config {
    filter = "category=\"SECURITY_RULE_VIOLATION\""
    
    pubsub_target {
      topic    = google_pubsub_topic.security.id
      message_format = "JSON"
    }
  }
}

# Security Health Analytics
resource "google_scc_security_health_analytics_config" "main" {
  name       = "security-health-analytics-config"
  provider   = google_scc_source.security.name
  state      = "ENABLED"
  
  modules {
    name = "HARDENED_DRIVER"
    state = "ENABLED"
  }
  
  modules {
    name = "SQL_DENIAL_OF_SERVICE"
    state = "ENABLED"
  }
  
  modules {
    name = "RESOURCE_MANAGER_CLOUDSOURCE"
    state = "ENABLED"
  }
}
```

## COMMON ISSUES

### 1. Alert Fatigue

- Too many findings
- Solution: Use risk-based prioritization

### 2. False Positives

- Invalid security findings
- Solution: Tune detection rules

### 3. Remediation Delays

- Slow issue resolution
- Solution: Automated remediation

## CROSS-REFERENCES

### Prerequisites

- Cloud security basics
- Compliance frameworks
- Security tools

### What to Study Next

1. Zero Trust
2. Identity Federation
3. Multi-Cloud DevOps

## EXAM TIPS

- Know CSPM components
- Understand compliance mapping
- Be able to configure CSPM tools