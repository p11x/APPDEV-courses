---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud Security
Concept: CSPM
Difficulty: advanced
Prerequisites: Basic Cloud Computing, Basic CSPM Concepts
RelatedFiles: 01_Basic_CSPM.md, 03_Practical_CSPM.md
UseCase: Advanced CSPM implementation for enterprise multi-cloud environments
CertificationExam: AWS Solutions Architect Professional, Azure Architect Expert
LastUpdated: 2025
---

## WHY

Advanced CSPM implementation requires sophisticated configurations, custom policies, and integration with security tools for comprehensive multi-cloud security posture management.

### Strategic Requirements

- **Custom Policies**: Organization-specific rules
- **Integration**: Security tool ecosystem
- **Automation**: Automated remediation
- **Governance**: Multi-account management
- **Reporting**: Executive and technical reports

### Advanced Architecture Patterns

| Pattern | Complexity | Features | Use Case |
|---------|------------|----------|----------|
| Single Cloud | Low | Basic monitoring | Simple environments |
| Multi-Cloud | Medium | Cross-cloud | Multiple providers |
| Multi-Account | High | AWS Orgs, Management Groups | Enterprise |
| Zero Trust | Very High | Full integration | High security |

## WHAT

### Advanced CSPM Features

**Custom Policy Engine**
- Organization-specific rules
- Rego/Python policies
- Custom findings format

**Remediation Automation**
- Auto-remediation workflows
- Ticketing integration
- Runbook automation

**Governance**
- Multi-account scanning
- Delegated administration
- Policy inheritance

### Cross-Platform Comparison

| Feature | AWS Security Hub | Defender for Cloud | GCP SCC |
|---------|-----------------|-------------------|---------|
| Multi-Cloud | Yes (AWS only) | Yes | Yes |
| Custom Policies | Yes | Yes | Yes |
| Auto-Remediation | Yes | Yes | Limited |
| Integrations | Many | Many | Moderate |
| Compliance Standards | CIS, PCI, NIST | CIS, PCI, NIST | CIS, PCI |

## HOW

### Example 1: Custom Security Policies

```hcl
# AWS Security Hub custom policy using CloudFormation
resource "aws_cloudformation_stack" "custom_policy" {
  name = "custom-security-policy"
  
  template_body = <<EOF
AWSTemplateFormatVersion: '2010-09-09'
Description: Custom Security Hub Policy
Resources:
  SecurityRule:
    Type: AWS::SecurityHub::SecurityRule
    Properties:
      SecurityRuleName: S3 Bucket Policy Check
      Description: Check S3 bucket policies for public access
      Severity: CRITICAL
      FindingProviderFields:
        Types:
          - Software and Configuration Checks
      Detection:
        RegionScope: REGIONAL
        Predicate:
          Compare:
            Attribute: S3BucketPublicAccessBlock
            Operator: EQUALS
            Value: false
      Recommendation:
        Remediation: Ensure S3 buckets are not publicly accessible
        Related Findings: arn:aws:securityhub:*:*::Finding/S3/1.2.3.4
      ProductFields:
        RuleName: S3PublicAccessCheck
        ProductName: Custom Security Rules
EOF
}

# Azure custom policy definition
resource "azurerm_policy_definition" "custom_s3" {
  name                  = "custom-s3-public-access"
  display_name          = "S3 Buckets should not be publicly accessible"
  description          = "This policy ensures S3 buckets are not publicly accessible"
  policy_type          = "Custom"
  mode                 = "All"
  
  metadata = jsonencode({
    category: "Storage"
    version: "1.0.0"
  })
  
  policy_rule = jsonencode({
    if {
      anyOf: [
        {
          allOf: [
            {
              field: "type"
              equals: "Microsoft.Storage/storageAccounts"
            },
            {
              not: {
                field: "Microsoft.Storage/storageAccounts/allowBlobPublicAccess"
                equals: false
              }
            }
          ]
        }
      ]
    }
    then {
      effect = "audit"
    }
  })
  
  parameters = jsonencode({})
}

# GCP custom security health analytics module
resource "google_scc_security_health_analytics_custom_config" "s3_policy" {
  name    = "organizations/${var.org_id}/securityHealthAnalyticsConfigs/custom-s3-policy"
  display_name = "Custom S3 Policy"
  description = "Custom policy for S3 bucket security"
  enablement_state = "ENABLED"
  
  predicate {
    condition {
      expression = "resource.pubicAccess == true"
      logic = "AND"
    }
  }
  
  scoring {
    severity = "CRITICAL"
  }
}
```

### Example 2: Automated Remediation

```yaml
# Automated remediation with AWS Systems Manager
apiVersion: workflows.k8s.io/v1
kind: AWSSystemsManagerAutomation
metadata:
  name: s3-public-access-remediation
spec:
  name: S3-Public-Access-Remediation
  description: Automatically remediate S3 public access
  assumeRole: arn:aws:iam::${account}:role/AutomationRole
  
  parameters:
    BucketName:
      type: String
      description: S3 bucket name
    AllowPublicAccess:
      type: Boolean
      default: false
  
  mainSteps:
  - name: CheckPublicAccess
    action: aws:executeScript
    inputs:
      Runtime: python3.10
      Handler: check_bucket_public_access
      Script: |
        import boto3
        import json
        
        s3 = boto3.client('s3')
        bucket_name = $BucketName
        
        response = s3.get_public_access_block(Bucket=bucket_name)
        block_config = response.get('PublicAccessBlockConfiguration', {})
        
        output = {
            'BlockPublicAcls': block_config.get('BlockPublicAcls'),
            'BlockPublicPolicy': block_config.get('BlockPublicPolicy')
        }
        print(json.dumps(output))
        
  - name: EnableBlockPublicAccess
    condition: AllowPublicAccess == false
    action: aws:executeScript
    inputs:
      Runtime: python3.10
      Handler: enable_block_public_access
      Script: |
        import boto3
        
        s3 = boto3.client('s3')
        bucket_name = $BucketName
        
        s3.put_public_access_block(
            Bucket=bucket_name,
            PublicAccessBlockConfiguration={
                'BlockPublicAcls': True,
                'BlockPublicPolicy': True,
                'IgnorePublicAcls': True,
                'BlockPublicPolicy': True
            }
        )
        
  - name: Notify
    action: aws:ssm:SendCommand
    inputs:
      DocumentName: AWS-RunPowerShellCommand
      InstanceIds: ["*"]
      Parameter:
        commands:
          - 'Write-S3BucketRemediationLog -BucketName $BucketName'
```

### Example 3: Multi-Cloud Governance

```python
# Multi-cloud security governance script
from abc import ABC, abstractmethod

class CloudSecurityScanner(ABC):
    @abstractmethod
    def scan(self):
        pass
    
    @abstractmethod
    def remediate(self, finding):
        pass

class AWSSecurityScanner(CloudSecurityScanner):
    def __init__(self):
        import boto3
        self.securityhub = boto3.client('securityhub')
        
    def scan(self):
        """Scan AWS resources"""
        response = self.securityhub.get_findings(
            Filters={
                'SeverityLabel': [{'Value': 'CRITICAL'}]
            }
        )
        return response['Findings']
        
    def remediate(self, finding):
        """Remediate AWS finding"""
        pass

class AzureSecurityScanner(CloudSecurityScanner):
    def __init__(self, config):
        from azure.mgmt.security import SecurityCenter
        self.client = SecurityCenter(config)
        
    def scan(self):
        """Scan Azure resources"""
        return self.client.alerts.list()
        
    def remediate(self, finding):
        """Remediate Azure finding"""
        pass

class GCPSecurityScanner(CloudSecurityScanner):
    def __init__(self, config):
        from google.cloud import securitycenter
        self.client = securitycenter.Client()
        
    def scan(self):
        """Scan GCP resources"""
        return self.client.list_findings()
        
    def remediate(self, finding):
        """Remediate GCP finding"""
        pass

class MultiCloudGovernance:
    def __init__(self, config):
        self.scanners = {
            'aws': AWSSecurityScanner(),
            'azure': AzureSecurityScanner(config['azure']),
            'gcp': GCPSecurityScanner(config['gcp'])
        }
        
    def scan_all(self):
        """Scan all cloud environments"""
        results = {}
        for cloud, scanner in self.scanners.items():
            results[cloud] = scanner.scan()
        return results
        
    def generate_report(self, findings):
        """Generate governance report"""
        report = {
            'total_findings': sum(len(f) for f in findings.values()),
            'by_severity': self.categorize_by_severity(findings),
            'by_cloud': {cloud: len(finds) for cloud, finds in findings.items()},
            'remediation_required': self.identify_remediation(findings)
        }
        return report
        
    def categorize_by_severity(self, findings):
        """Categorize findings by severity"""
        severity_map = {}
        for cloud, finds in findings.items():
            for finding in finds:
                severity = finding.get('Severity', 'UNKNOWN')
                severity_map[severity] = severity_map.get(severity, 0) + 1
        return severity_map
```

## COMMON ISSUES

### 1. Cross-Cloud Correlation

- Different finding formats
- Solution: Normalize findings

### 2. Remediation Permissions

- Lack of permissions
- Solution: Use service roles

### 3. Performance at Scale

- Slow scanning
- Solution: Incremental scanning

## PERFORMANCE

### Scanning Performance

| Environment | Scan Time | Finding Count |
|-------------|-----------|---------------|
| Single Account | < 5 min | ~100 |
| Multi-Account | < 30 min | ~1000 |
| Multi-Cloud | < 60 min | ~3000 |

## COMPATIBILITY

### Integration Support

| Tool | AWS | Azure | GCP |
|------|-----|-------|-----|
| Jira | Yes | Yes | Yes |
| Slack | Yes | Yes | Yes |
| PagerDuty | Yes | Yes | Yes |
| ServiceNow | Yes | Yes | Limited |

## CROSS-REFERENCES

### Prerequisites

- Basic CSPM concepts
- Cloud security tools
- Automation basics

### Related Topics

1. Zero Trust
2. Identity Federation
3. Multi-Cloud DevOps

## EXAM TIPS

- Know custom policy creation
- Understand remediation automation
- Be able to design enterprise governance