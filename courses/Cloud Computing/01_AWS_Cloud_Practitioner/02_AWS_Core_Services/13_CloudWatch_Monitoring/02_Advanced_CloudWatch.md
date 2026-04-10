---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: CloudWatch Monitoring
Purpose: Advanced CloudWatch including logs insights, metrics, alarms, and embedded metrics
Difficulty: advanced
Prerequisites: 01_Basic_CloudWatch.md
RelatedFiles: 01_Basic_CloudWatch.md, 03_Practical_CloudWatch.md
UseCase: Enterprise monitoring and observability
CertificationExam: AWS SysOps Administrator
LastUpdated: 2025
---

## 💡 WHY

Advanced CloudWatch features enable sophisticated monitoring, log analysis, and application performance optimization.

## 📖 WHAT

### Advanced Features

**CloudWatch Logs Insights**: Query language for log analysis

**Embedded Metrics**: High-cardinality custom metrics

**Metric Streams**: Continuous streaming to destinations

**Contributor Insights**: Identify top contributors

**Service Lens**: End-to-end distributed tracing

### Cross-Platform Comparison

| Feature | AWS CloudWatch | Azure Monitor | GCP Operations | Prometheus |
|---------|----------------|---------------|----------------|------------|
| Metrics | Yes | Yes | Yes | Yes |
| Logs | Yes | Yes | Yes | Yes |
| Alerts | Yes | Yes | Yes | Yes |
| Distributed Tracing | X-Ray | App Insights | Cloud Trace | Jaeger |
| Custom Metrics | Yes | Yes | Yes | Yes |
| Log Analytics | Insights | Log Analytics | Logs Explorer | Loki |
| Dashboards | Yes | Yes | Yes | Yes |

## 🔧 HOW

### Example 1: Logs Insights Queries

```bash
# Query application logs
aws logs start-query \
    --log-group-name /aws/lambda/my-function \
    --start-time 1704067200000 \
    --end-time 1704153600000 \
    --query-string 'fields @timestamp, @message | filter @message like "ERROR" | sort @timestamp desc | limit 20'

# Analyze Lambda performance
aws logs start-query \
    --log-group-name /aws/lambda/my-function \
    --start-time 1704067200000 \
    --end-time 1704153600000 \
    --query-string 'stats avg(billedDuration) as avgDuration, count(*) as invocations by bin(5m)'
```

### Example 2: Contributor Insights

```bash
# Create contributor insight for API requests
aws cloudwatch put-insight-rule \
    --rule-name "top-api-clients" \
    --rule-state ENABLED \
    --pattern '{$.httpMethod = "GET"} | top 10 by $.requestId as "count"' \
    --metric-name "APICall" \
    --metric-namespace "Custom/Insights" \
    --statistic "Sum"
```

### Example 3: Embedded Metrics

```javascript
const AWS = require('aws-sdk');
const cloudwatch = new AWS.CloudWatch();

exports.handler = async (event) => {
    // Emit embedded metric
    const params = {
        MetricData: [{
            MetricName: 'OrderProcessing',
            Value: 1,
            Timestamp: new Date(),
            Dimensions: [
                { Name: 'Service', Value: 'OrderAPI' },
                { Name: 'Region', Value: 'us-east-1' }
            ],
            StorageResolution: 1
        }],
        Namespace: 'Custom/Metrics'
    };
    
    await cloudwatch.putMetricData(params).promise();
    return { statusCode: 200 };
};
```

## ⚠️ COMMON ISSUES

### 1. Logs Not Streaming

**Problem**: Application logs not appearing

**Solution**: Check IAM role has logs:PutLogEvents, verify correct log group name

### 2. High CloudWatch Costs

**Problem**: Unexpected charges

**Solution**: Set log retention, use embedded metrics sparingly, configure metric filters

### 3. Alarm Not Triggering

**Problem**: Alarm doesn't fire

**Solution**: Check evaluation periods, ensure metric data is being published

## 🏃 PERFORMANCE

### Limits

| Feature | Limit |
|---------|-------|
| Metrics per custom namespace | 500 |
| Logs retention | 1 year |
| Dashboard widgets | 200 |
| Alarm history | 14 days |

## 🔗 CROSS-REFERENCES

**Related**: CloudTrail, X-Ray, EventBridge

**Prerequisite**: Basic CloudWatch understanding

## ✅ EXAM TIPS

- Logs Insights: powerful query language
- Contributor Insights: identify top contributors
- Embedded Metrics: high-cardinality metrics
- Metric Streams: continuous data export
- ServiceLens: distributed tracing integration