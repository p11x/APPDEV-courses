---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudWatch Monitoring
Purpose: Hands-on CloudWatch implementation including dashboards, alarms, and log analysis
Difficulty: intermediate
Prerequisites: 01_Basic_CloudWatch.md, 02_Advanced_CloudWatch.md
RelatedFiles: 01_Basic_CloudWatch.md, 02_Advanced_CloudWatch.md
UseCase: Production monitoring setup
CertificationExam: AWS SysOps Administrator
LastUpdated: 2025
---

## 💡 WHY

Hands-on CloudWatch implementation provides practical experience building comprehensive monitoring and alerting systems.

## 📖 WHAT

### Lab: Production Monitoring Dashboard

```
┌─────────────────────────────────────────────────────┐
│              CloudWatch Dashboard                    │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐   │
│  │   CPU       │ │   Memory    │ │  Requests   │   │
│  │   Usage     │ │   Usage     │ │   Count     │   │
│  └─────────────┘ └─────────────┘ └─────────────┘   │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐   │
│  │  Error      │ │ Latency     │ │  Lambda     │   │
│  │  Rate       │ │   P99       │ │  Invocations │   │
│  └─────────────┘ └─────────────┘ └─────────────┘   │
└─────────────────────────────────────────────────────┘
```

## 🔧 HOW

### Module 1: Create Monitoring Dashboard

```bash
#!/bin/bash
# CloudWatch Dashboard Setup

# Create dashboard
aws cloudwatch put-dashboard \
    --dashboard-name production-monitoring \
    --dashboard-body '{
        "widgets": [
            {
                "type": "metric",
                "properties": {
                    "title": "EC2 CPU Utilization",
                    "metrics": [
                        ["AWS/EC2", "CPUUtilization", "InstanceId", "i-123456789"],
                        [".", "CPUUtilization", "InstanceId", "i-987654321"]
                    ],
                    "period": 300,
                    "stat": "Average",
                    "region": "us-east-1"
                },
                "width": 12,
                "height": 6
            },
            {
                "type": "metric",
                "properties": {
                    "title": "Lambda Invocations",
                    "metrics": [
                        ["AWS/Lambda", "Invocations", "FunctionName", "my-function"],
                        [".", "Errors", ".", "."]
                    ],
                    "period": 300,
                    "stat": "Sum",
                    "region": "us-east-1"
                },
                "width": 12,
                "height": 6
            }
        ]
    }'
```

### Module 2: Create Alarms

```bash
# CPU High Alarm
aws cloudwatch put-metric-alarm \
    --alarm-name high-cpu-alarm \
    --alarm-description "CPU utilization exceeds 80%" \
    --metric-name CPUUtilization \
    --namespace AWS/EC2 \
    --statistic Average \
    --period 300 \
    --threshold 80 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 3 \
    --alarm-actions arn:aws:sns:us-east-1:123456789:alerts

# Lambda Error Alarm
aws cloudwatch put-metric-alarm \
    --alarm-name lambda-errors-alarm \
    --alarm-description "Lambda function errors" \
    --metric-name Errors \
    --namespace AWS/Lambda \
    --statistic Sum \
    --period 300 \
    --threshold 10 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 2 \
    --alarm-actions arn:aws:sns:us-east-1:123456789:alerts

# S3 Bucket Size Alarm
aws cloudwatch put-metric-alarm \
    --alarm-name s3-size-alarm \
    --alarm-description "S3 bucket approaching size limit" \
    --metric-name BucketSizeBytes \
    --namespace AWS/S3 \
    --statistic Average \
    --period 86400 \
    --threshold 100000000000 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 1 \
    --dimensions '[{"Name": "BucketName", "Value": "my-bucket"}, {"Name": "StorageType", "Value": "StandardStorage"}]'
```

### Module 3: Configure Log Aggregation

```bash
# Create log group
aws logs create-log-group \
    --log-group-name /aws/eks/production/cluster

# Create metric filter for errors
aws logs put-metric-filter \
    --log-group-name /aws/eks/production/cluster \
    --filter-name error-filter \
    --filter-pattern '[w1="ERROR" || w1="CRITICAL"]' \
    --metric-transformations '[
        {
            "metricName": "ErrorCount",
            "metricNamespace": "Custom/Application",
            "metricValue": "1",
            "defaultValue": 0
        }
    ]'

# Create alarm from metric filter
aws cloudwatch put-metric-alarm \
    --alarm-name app-errors-alarm \
    --alarm-description "Application error detected" \
    --metric-name ErrorCount \
    --namespace Custom/Application \
    --statistic Sum \
    --period 60 \
    --threshold 1 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 1 \
    --alarm-actions arn:aws:sns:us-east-1:123456789:alerts
```

## VERIFICATION

```bash
# List dashboards
aws cloudwatch list-dashboards

# Describe alarm
aws cloudwatch describe-alarms --alarm-names high-cpu-alarm

# Query logs
aws logs filter-log-events \
    --log-group-name /aws/eks/production/cluster \
    --filter-pattern "ERROR"
```

## CLEANUP

```bash
# Delete dashboard
aws cloudwatch delete-dashboard --dashboard-name production-monitoring

# Delete alarms
aws cloudwatch delete-alarms --alarm-names high-cpu-alarm lambda-errors-alarm app-errors-alarm
```

## 🔗 CROSS-REFERENCES

**Related**: CloudTrail, SNS, EventBridge