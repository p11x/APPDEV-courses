---
Category: AWS Cloud Practitioner
Subcategory: AWS Security
Concept: Security Services - Practical
Purpose: Practical security implementation for production environments with monitoring and incident response
Difficulty: practical
Prerequisites: 01_Basic_Security_Services.md, 02_Advanced_Security_Services.md
RelatedFiles: 01_Basic_Security_Services.md, 02_Advanced_Security_Services.md
UseCase: Production security operations and incident response
CertificationExam: AWS Certified Cloud Practitioner - Domain 2
LastUpdated: 2025
---

## WHY

Production security operations require hands-on implementation of monitoring, alerting, and incident response procedures. This knowledge is essential for security teams managing cloud environments.

### Why Production Security Matters

- **Incident Response**: Quick detection and response
- **Continuous Monitoring**: 24/7 security operations
- **Evidence Collection**: Forensic capabilities
- **Compliance**: Audit-ready security

### Real-World Scenarios

- **Ransomware**: Malware detection and containment
- **Data Breach**: Investigation and response
- **DDoS Attack**: Mitigation procedures
- **Insider Threat**: Anomaly detection

## WHAT

### Production Security Architecture

```
                 SECURITY OPERATIONS
                 ==============

     ┌──────────────────────────────────┐
     │      DETECTION LAYER              │
     │  ┌────────┐  ┌────────┐  ┌────┐│
     │  │Guard  │  │Security│  │WAF ││
     │  │Duty   │  │Hub    │  │    ││
     │  └────────┘  └────────┘  └────┘│
     └──────────────┬───────────────────┘
                    │
     ┌─────────────┴──────────────────┐
     │      RESPONSE LAYER         │
     │  ┌────────┐  ┌────────┐  ┌────┐│
     │  │Event  │  │Lambda│  │SNS ││
     │  │Bridge │  │    │  │    ││
     │  └────────┘  └────────┘  └────┘│
     └───────────────────────────────┘
                    │
     ┌─────────────┴───────────────────┐
     │      OPS LAYER              │
     │  ┌───────┐  ┌──────────┐   │
     │  │Cloud │  │Incident │   │
     │  │Watch │  │Response │   │
     │  └───────┘  └──────────┘   │
     └─────────────────────────────┘
```

### Incident Response Workflow

| Phase | Activities | SLA |
|-------|-----------|-----|
| Detect | Alert triggered | 1 min |
| Triage | Validate finding | 5 min |
| Contain | Isolate threat | 15 min |
| Investigate | Root cause | 1 hour |
| Remediate | Fix issue | 4 hours |

## HOW

### Example 1: Security Monitoring Dashboard

```python
# Security monitoring dashboard
import boto3
import json
from datetime import datetime, timedelta

class SecurityDashboard:
    def __init__(self):
        self.guardduty = boto3.client('guardduty')
        self.securityhub = boto3.client('securityhub')
        self.cloudwatch = boto3.client('cloudwatch')
    
    def create_dashboard(self):
        # Create security metrics dashboard
        dashboard = {
            "widgets": [
                {
                    "type": "metric",
                    "properties": {
                        "title": "Active Security Findings",
                        "metrics": [
                            ["AWS/SecurityHub", "Findings"],
                            ["AWS/GuardDuty", "Findings"]
                        ],
                        "period": 3600,
                        "stat": "Sum"
                    }
                },
                {
                    "type": "metric",
                    "properties": {
                        "title": "GuardDuty by Severity",
                        "metrics": [
                            ["AWS/GuardDuty", "Findings", "Severity", "HIGH"],
                            [".", "Findings", "Severity", "MEDIUM"],
                            [".", "Findings", "Severity", "LOW"]
                        ],
                        "period": 86400,
                        "stat": "Sum"
                    }
                }
            ]
        }
        
        self.cloudwatch.put_dashboard(
            DashboardName='security-operations',
            DashboardBody=json.dumps(dashboard)
        )
    
    def create_alerts(self):
        # Critical finding alert
        self.cloudwatch.put_metric_alarm(
            AlarmName='critical-finding-alert',
            MetricName='Findings',
            Namespace='AWS/SecurityHub',
            Statistic='Sum',
            Period=300,
            Threshold=1,
            ComparisonOperator='GreaterThanThreshold',
            EvaluationPeriods=1,
            AlarmActions=[
                'arn:aws:sns:us-east-1:123456789:security-alerts'
            ]
        )
    
    def get_security_summary(self):
        # Get current security status
        findings = self.securityhub.get_findings(
            Filters={
                'RecordState': [{'Value': 'ACTIVE', 'Comparison': 'EQUAL'}]
            },
            MaxResults=100
        )
        
        return {
            'total': len(findings['Findings']),
            'critical': len([f for f in findings['Findings'] 
                           if f['Severity']['Label'] == 'CRITICAL']),
            'high': len([f for f in findings['Findings'] 
                      if f['Severity']['Label'] == 'HIGH'])
        }
```

### Example 2: Automated Incident Response

```bash
#!/bin/bash
# Automated incident response

# GuardDuty finding handler
handle_guardduty_finding() {
    local finding_id=$1
    local severity=$2
    local resource=$3
    
    # Log the finding
    echo "$(date): Handling GuardDuty finding $finding_id"
    
    # Create incident ticket
    aws support create-case \
        --subject "Security Finding: $finding_id" \
        --service-code "general" \
        --category-code "security" \
        --severity-code "high" \
        --language "en" \
        --email-to "security@example.com" \
        --communication-body "GuardDuty finding: $finding_id"
    
    # If critical, isolate instance
    if [ "$severity" == "HIGH" ]; then
        INSTANCE_ID=$(echo $resource | jq -r '.InstanceId')
        if [ -n "$INSTANCE_ID" ]; then
            # Isolate instance
            aws ec2 modify-instance-attribute \
                --instance-id $INSTANCE_ID \
                --disable-api-terminate \
                --instance-initiated-shutdown-behavior terminate
            
            # Create security group for isolation
            SG_ID=$(aws ec2 create-security-group \
                --group-name "ISOLATED_$finding_id" \
                --description "Isolated due to security finding" \
                --query 'GroupId' --output text)
            
            # Move instance to isolation group
            aws ec2 modify-instance-attribute \
                --instance-id $INSTANCE_ID \
                --groups "$SG_ID"
        fi
    fi
}

# WAF attack handler
handle_waf_block() {
    local rule_id=$1
    local action=$2
    
    # Get blocked requests
    aws wafv2 get-sampled-requests \
        --web-acl-arn $WEB_ACL_ARN \
        --rule-metric-name $rule_id \
        --time-window StartTime=$(date -d '1 hour ago' +%s),EndTime=$(date +%s)
    
    # Analyze and update rule if needed
    echo "$(date): Analyzed WAF blocks for rule $rule_id"
}
```

### Example 3: Incident Response Runbook

```yaml
# incident-response-runbook.yaml
version: 1.0

incident_types:
  - name: compromised-instance
    severity: HIGH
    steps:
      - identify:
          source: GuardDuty
          finding_type: Backdoor:EC2
          
      - contain:
          - name: Isolate instance
            command: |
              aws ec2 modify-instance-attribute \
                --instance-id {resource} \
                --groups {isolation_sg}
          - name: Revoke credentials
            command: |
              aws iam update-access-key \
                --status Inactive \
                --access-key-id {key}
              
      - investigate:
          - name: Check CloudTrail
            command: |
              aws cloudtrail lookup-events \
                --lookup-attributes AttributeKey=ResourceName,AttributeValue={resource}
          - name: Review logs
            command: |
              aws logs filter-log-events \
                --log-group-name /ec2/{resource} \
                --start-time {incident_time}
                
      - remediate:
          - name: Terminate instance
            command: |
              aws ec2 terminate-instances \
                --instance-ids {resource}
          - name: Clean up AMI
            command: |
              aws ec2 deregister-image --image-id {ami}

  - name: data-exfiltration
    severity: CRITICAL
    steps:
      - identify:
          source: GuardDuty
          finding_type: Exfiltration
          
      - contain:
          - name: Block outbound
            command: aws network-inspector-block
          - name: Revoke access
            command: aws iam revoke-permissions
```

## COMMON ISSUES

### 1. Alert Fatigue

**Problem**: Too many security alerts.

**Solution**:
- Tune alert thresholds
- Create aggregation rules
- Use SOAR automation

### 2. Finding Not Actionable

**Problem**: Unclear findings.

**Solution**:
- Review finding details
- Use Security Hub insights
- Create playbooks

### 3. Evidence Collection Failed

**Problem**: Missing logs.

**Solution**:
```bash
# Verify logging enabled
aws cloudtrail get-trail-status --name main-trail

# Check CloudWatch
aws logs describe-log-groups --log-group-name-prefix /aws/cloudtrail
```

### 4. IAM Misconfiguration

**Problem**: Cannot take action.

**Solution**:
- Use dedicated incident response role
- Pre-stage credentials
- Use cross-account access

### 5. Communication Failure

**Problem**: Alerts not delivered.

**Solution**:
- Verify SNS permissions
- Use fallback channel
- Test regularly

## PERFORMANCE

### Real-World Metrics

| Metric | Target | Actual |
|--------|--------|--------|
| MTTD | < 5 min | 3 min |
| MTTR | < 1 hour | 45 min |
| False Positive | < 20% | 15% |
| Coverage | 100% | 95% |

### Performance Monitoring

| Metric | Collection | Alert |
|--------|------------|-------|
| Findings | Continuous | Real-time |
| Instances | Hourly | Daily |
| Network | Continuous | Real-time |

## COMPATIBILITY

### Production Tool Integration

| Tool | Integration | Notes |
|------|-------------|-------|
| Jira | API | Ticketing |
| PagerDuty | SDK | On-call |
| Slack | Webhook | ChatOps |
| Splunk | HTTP | SIEM |

### Forensics Support

| Artifact | Collection | Retention |
|----------|------------|-----------|
| Memory | LiveRD | 30 days |
| Disk | EBS Snapshot | 90 days |
| Network | VPC Flow Logs | 90 days |
| Logs | CloudWatch | 1 year |

## CROSS-REFERENCES

### Related Patterns

- Compliance Programs: Audit evidence
- Disaster Recovery: Backup and restore
- Cost Management: Security costs

### Integration Tools

- SIEM: Splunk, QRadar
- SOAR: Demisto, Splunk SOAR
- Ticketing: Jira, ServiceNow

### What to Study Next

1. Incident Response: Advanced
2. Forensics: Investigation
3. Architecture: Security design

## EXAM TIPS

### Key Facts

- GuardDuty: Automated detection, no agent needed
- Security Hub: Aggregates all findings
- WAF: Layer 7 protection
- Incident response: Contain first, then investigate

### Problem Scenarios

- **Scenario**: "EC2 compromised" = Isolate, investigate, terminate
- **Scenario**: "DDoS attack" = Shield, WAF rules
- **Scenario**: "Data exfil" = Block, investigate
- **Scenario**: "SQL injection" = WAF rule