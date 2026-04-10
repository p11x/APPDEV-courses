---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudWatch Monitoring
Purpose: Understanding Amazon CloudWatch for monitoring and observability
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_CloudWatch.md, 03_Practical_CloudWatch.md
UseCase: Monitoring AWS resources and applications
CertificationExam: AWS SysOps Administrator
LastUpdated: 2025
---

## 💡 WHY

Amazon CloudWatch provides monitoring and observability for AWS resources and applications. It's essential for understanding system performance, troubleshooting issues, and optimizing costs.

### Why CloudWatch Matters

- **Unified Monitoring**: Centralized metrics across AWS
- **Real-time**: Near real-time data collection
- **Alerts**: Automated notifications on thresholds
- **Logs**: Centralized log management
- **Insights**: Application performance monitoring

### Industry Use Cases

- Infrastructure monitoring
- Application performance monitoring
- Cost optimization
- Security monitoring

## 📖 WHAT

### CloudWatch Components

**Metrics**: Time-series data points

**Alarms**: Automated notifications

**Logs**: Centralized log storage

**Events**: Event-driven automation

**Dashboards**: Visualizations

**Synthetics**: Canary monitoring

### Architecture Diagram

```
CloudWatch Architecture
========================

AWS Resources                          CloudWatch
┌─────────────┐                       ┌─────────────┐
│ EC2         │──────Metrics─────────►│             │
│ Lambda      │──────Metrics─────────►│   Metrics   │
│ RDS         │──────Metrics─────────►│  (CloudWatch│
│ ECS         │──────Metrics─────────►│   Logs)     │
└─────────────┘                       └──────┬──────┘
                                             │
                           ┌─────────────────┼─────────────────┐
                           │                 │                 │
                      ┌────┴─────┐      ┌────┴─────┐      ┌────┴─────┐
                      │  Alarms  │      │  Events  │      │Dashboard │
                      └──────────┘      └──────────┘      └──────────┘
```

## 🔧 HOW

### Example 1: Basic Metrics

```bash
# Get EC2 instance metrics
aws cloudwatch get-metric-statistics \
    --namespace AWS/EC2 \
    --metric-name CPUUtilization \
    --start-time 2024-01-01T00:00:00Z \
    --end-time 2024-01-01T01:00:00Z \
    --period 300 \
    --statistics Average,Maximum,Minimum

# Get RDS metrics
aws cloudwatch get-metric-statistics \
    --namespace AWS/RDS \
    --metric-name CPUUtilization \
    --start-time 2024-01-01T00:00:00Z \
    --end-time 2024-01-01T01:00:00Z \
    --period 60 \
    --statistics Average \
    --dimensions '[
        {"Name": "DBInstanceIdentifier", "Value": "mydb"}
    ]'

# Get S3 metrics
aws cloudwatch get-metric-statistics \
    --namespace AWS/S3 \
    --metric-name BucketSizeBytes \
    --start-time 2024-01-01T00:00:00Z \
    --end-time 2024-01-01T01:00:00Z \
    --period 86400 \
    --statistics Average \
    --dimensions '[
        {"Name": "BucketName", "Value": "my-bucket"},
        {"Name": "StorageType", "Value": "StandardStorage"}
    ]'
```

### Example 2: Create Alarms

```bash
# Create CPU alarm
aws cloudwatch put-metric-alarm \
    --alarm-name high-cpu-alarm \
    --alarm-description "Alarm when CPU exceeds 80%" \
    --metric-name CPUUtilization \
    --namespace AWS/EC2 \
    --statistic Average \
    --period 300 \
    --threshold 80 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 2 \
    --alarm-actions arn:aws:sns:us-east-1:123456789:my-topic

# Create billing alarm
aws cloudwatch put-metric-alarm \
    --alarm-name estimated-charges-alarm \
    --metric-name EstimatedCharges \
    --namespace AWS/Billing \
    --statistic Maximum \
    --period 86400 \
    --threshold 100 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 1 \
    --alarm-actions arn:aws:sns:us-east-1:123456789:my-topic

# Create Lambda error alarm
aws cloudwatch put-metric-alarm \
    --alarm-name lambda-errors-alarm \
    --alarm-description "Alarm on Lambda errors" \
    --metric-name Errors \
    --namespace AWS/Lambda \
    --statistic Sum \
    --period 300 \
    --threshold 10 \
    --comparison-operator GreaterThanThreshold \
    --evaluation-periods 1 \
    --dimensions '[
        {"Name": "FunctionName", "Value": "my-function"}
    ]'
```

### Example 3: CloudWatch Logs

```bash
# Create log group
aws logs create-log-group \
    --log-group-name /aws/lambda/my-function

# Create log stream
aws logs create-log-stream \
    --log-group-name /aws/lambda/my-function \
    --log-stream-name my-stream

# Put log events
aws logs put-log-events \
    --log-group-name /aws/lambda/my-function \
    --log-stream-name my-stream \
    --log-events '[
        {"timestamp": 1704067200000, "message": "Function started"},
        {"timestamp": 1704067201000, "message": "Processing request"}
    ]'

# Query logs with filter pattern
aws logs filter-log-events \
    --log-group-name /aws/lambda/my-function \
    --filter-pattern "ERROR"
```

### Example 4: CloudWatch Dashboard

```bash
# Create dashboard
aws cloudwatch put-dashboard \
    --dashboard-name my-dashboard \
    --dashboard-body '{
        "widgets": [
            {
                "type": "metric",
                "properties": {
                    "title": "CPU Utilization",
                    "metrics": [
                        ["AWS/EC2", "CPUUtilization", "InstanceId", "i-123456789"]
                    ],
                    "period": 300,
                    "stat": "Average"
                }
            },
            {
                "type": "metric",
                "properties": {
                    "title": "Request Count",
                    "metrics": [
                        ["AWS/Lambda", "Invocations", "FunctionName", "my-function"]
                    ],
                    "period": 60,
                    "stat": "Sum"
                }
            }
        ]
    }'
```

## ⚠️ COMMON ISSUES

### 1. Metrics Not Showing

**Problem**: No data in CloudWatch.

**Solution**: Check if CloudWatch agent is installed, verify IAM permissions.

### 2. Alarm Not Triggering

**Problem**: Alarm doesn't fire.

**Solution**: Check evaluation periods, threshold settings, action configuration.

### 3. High CloudWatch Costs

**Problem**: Unexpected charges.

**Solution**: Use detailed monitoring selectively, set retention periods, use custom metrics wisely.

### 4. Logs Not Streaming

**Problem**: Application logs not appearing.

**Solution**: Check IAM role has logs:CreateLogStream and logs:PutLogEvents permissions.

## 🏃 PERFORMANCE

### Limits

| Feature | Limit |
|---------|-------|
| Metrics per custom namespace | 500 |
| Dashboard widgets | 200 |
| Alarm history | 14 days |
| Log retention | 1 year |

## 🌐 COMPATIBILITY

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Metrics | CloudWatch | Monitor | Monitoring |
| Logs | CloudWatch Logs | Log Analytics | Cloud Logging |
| Dashboards | Yes | Yes | Yes |

## 🔗 CROSS-REFERENCES

**Related**: CloudTrail, X-Ray, SNS, EventBridge

**Prerequisite**: Basic AWS services

**Next**: CloudWatch Insights for log analysis

## ✅ EXAM TIPS

- Namespace groups related metrics
- Alarm states: OK, INSUFFICIENT_DATA, ALARM
- Metrics have timestamp and value
- Detailed monitoring = 1-minute granularity
- Use put-metric-data for custom metrics