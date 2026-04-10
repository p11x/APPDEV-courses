---
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Monitor.md, 02_Advanced_Azure_Monitor.md
RelatedFiles: 01_Basic_Azure_Monitor.md, 02_Advanced_Azure_Monitor.md
UseCase: Managing monitoring
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

# Practical Azure Monitor: Hands-On Implementation

## Introduction

Practical implementation of Azure Monitor requires hands-on experience with configuration, querying, and visualization. This guide provides practical exercises that prepare you for real-world monitoring scenarios. The focus on implementation prepares you for the AZ-104 Azure Administrator exam and everyday operational tasks.

The skills covered in this guide build on foundational and advanced concepts from the other guides. You will learn to set up alerts, create log queries, configure dashboards, and integrate monitoring with Azure Kubernetes Service. These practical skills enable you to implement monitoring solutions that meet operational requirements.

Understanding practical implementation ensures that you can effectively use Azure Monitor in production environments. Theory combined with hands-on practice creates complete understanding. This guide provides the practical experience needed for certification success and effective cloud operations.

## Setting Up Alerts

### Creating Metric Alerts

Metric alerts provide proactive notification when performance metrics exceed defined thresholds. This exercise guides you through creating a metric alert for CPU usage on virtual machines. The same process applies to other metric alert scenarios.

Begin by navigating to Azure Monitor in the Azure Portal. Select Alerts from the navigation, then select Create alert rule. The create alert experience guides you through condition, action group, and alert details configuration.

For condition configuration, select the signal type as metrics. Choose the CPU percentage metric from the available metrics. Configure the threshold condition - in this case, when average exceeds 80 percent. Set the aggregation granularity to 4 hours to avoid alerts from momentary spikes.

The alert condition evaluates over a specified time period, triggering when the threshold is exceeded consistently. Aggregation type and period selection significantly impacts alert behavior. Practice configuring different conditions to understand their effects.

### Creating Log Search Alerts

Log search alerts evaluate queries against log data, enabling sophisticated detection scenarios. This exercise creates an alert for failed authentication attempts. The same approach detects any pattern identifiable in log data.

Navigate to the Log Analytics workspace containing authentication logs. Execute a query to identify failed authentication attempts. The query should count failed attempts over a time period. For example:

```
SigninLogs
| where ResultType != "0"
| summarize FailedCount = count() by bin(TimeGenerated, 5m)
| where FailedCount > 5
```

Convert this query to an alert by selecting New alert rule from the query results. Configure the alert logic to trigger when results exceed zero, indicating failed attempts detected in the evaluation period.

Log search alerts enable sophisticated detection beyond simple metrics. The KQL queries can detect patterns ranging from simple error counts to complex anomaly detection. Practice creating various log alerts to understand their capabilities.

### Configuring Action Groups

Action groups define who receives notifications and how they are notified. This exercise creates an action group configured for critical alerts. Proper action group configuration ensures timely notification.

Navigate to Azure Monitor, select Action groups from the navigation. Select Add to create a new action group. Provide a name and short name for the action group.

Add actions to the action group. Email notifications deliver alerts to specified email addresses. SMS notifications provide high-priority alerts to on-call personnel. Webhook actions enable integration with external systems including incident management.

Action group reuse across multiple alerts ensures consistent notification. Create action groups aligned with your organizational notification structure. Different action groups for different severity levels enable appropriate notification.

### Alert Management Best Practices

Effective alert management requires ongoing attention and optimization. This exercise reviews existing alerts and optimizes for effectiveness. Regular alert review prevents alert fatigue and ensures continued relevance.

Review alert history to identify frequently triggered alerts. Frequent alerts may indicate underlying issues requiring resolution. Adjust thresholds for alerts that trigger frequently but do not require action.

Document alert response procedures to ensure consistent handling. Runbooks documenting alert response reduce investigation time. Include procedures in alert configuration to guide responders.

## Creating Log Queries

### Basic Query Construction

Log Analytics queries retrieve and analyze log data using Kusto Query Language. This exercise teaches basic query construction through practical examples. Understanding KQL fundamentals enables effective log analysis.

Queries begin with a table reference, identifying the data source. The following example queries the SigninLogs table:

```
SigninLogs
| take 10
```

The take operator limits results for initial exploration. Replace with filtering, aggregation, and formatting for specific analysis needs.

The where operator filters results based on conditions. Multiple conditions combine with and/or operators:

```
SigninLogs
| where ResultType == "0"
| where TimeGenerated > ago(1d)
```

Practice building queries against different log tables to understand data structure. Each Azure service logs to specific tables with unique fields.

### Aggregation Queries

Aggregation queries calculate statistics across log data. These queries support analysis, trending, and reporting. This exercise creates aggregation queries for common analysis scenarios.

The summarize operator groups and aggregates data:

```
SigninLogs
| summarize SigninCount = count() by UserDisplayName
| order by SigninCount desc
```

This query counts sign-ins per user, ordered by count. The same pattern applies to other aggregation scenarios.

Time-based aggregation enables trend analysis:

```
SigninLogs
| summarize SigninCount = count() by bin(TimeGenerated, 1d)
| render timechart
```

This query aggregates sign-ins by day and renders a timechart. Timecharts visualize time-series trends effectively.

### Correlation Queries

Correlation queries analyze relationships across multiple data sources. This exercise correlates sign-in logs with audit logs to understand user activity. Correlation enables deeper analysis than single-table queries.

Join operations correlate data across tables:

```
SigninLogs
| where ResultType == "0"
| project SigninTime = TimeGenerated, UserId
| join kind=inner (
    AuditLogs
    | project AuditTime = TimeGenerated, UserId, OperationName
) on UserId
| where SigninTime <= AuditTime and AuditTime <= SigninTime + 1h
```

This query correlates successful sign-ins with subsequent audit operations. The correlation timeframe ensures relevant relationship.

Complex correlations may require multiple query approaches. Practice correlation patterns to build effective analytical queries.

### Practical Query Examples

This section provides practical queries for common monitoring scenarios. These queries can be adapted for your specific needs. Keep a collection of useful queries for quick reference.

Failed authentication analysis:

```
SigninLogs
| where ResultType != "0"
| summarize FailedCount = count(), 
            FailedIPs = make_set(IPAddress) 
by UserDisplayName, bin(TimeGenerated, 1d)
| where FailedCount > 10
```

Resource allocation analysis:

```
AzureActivity
| where ResourceGroup contains "prod"
| summarize ResourcesCreated = count() by ResourceId, bin(TimeGenerated, 1d)
```

Application error analysis:

```
AppErrors
| summarize ErrorCount = count() by Type, bin(TimeGenerated, 1h)
| render timechart
```

Save useful queries to a query library for reuse. Query libraries enable consistent analysis and share effective queries with team members.

## Configuring Dashboards

### Creating Azure Dashboards

Azure dashboards provide consolidated views of monitoring data. This exercise creates a dashboard with key monitoring visualizations. Effective dashboards improve operational visibility and response time.

Navigate to Azure Portal and select Dashboard from the navigation. Select New dashboard and provide a name. The dashboard designer provides tile selection and configuration.

Add monitoring tiles to the dashboard. Metrics tiles display metric values and trends. Log query tiles display query results. These tiles update automatically as underlying data changes.

Dashboard sharing makes monitoring visible across teams. Share dashboards with specific users or publish to the organization. Appropriate access controls ensure security while enabling visibility.

### Customizing Dashboard Tiles

Customizing dashboard tiles improves their usefulness for specific scenarios. This exercise configures tiles for different monitoring needs. Effective tile configuration displays relevant information clearly.

Tile configuration options vary by tile type. Metric tiles support multiple metrics, aggregation methods, and time ranges. Configure these options to display information appropriate to your needs.

Log query tiles display query results in various formats. Table, bar chart, and timechart formats suit different analysis needs. Configure format based on the information being displayed.

Pin functionality adds frequently used tiles quickly. Pin tiles from metrics explorer, log analytics, or other monitoring views. This functionality streamlines dashboard creation for common views.

### Integrating Multiple Data Sources

Consolidated dashboards integrate data from multiple monitoring sources. This exercise creates a dashboard aggregating application and infrastructure monitoring. Comprehensive dashboards improve incident response and trend analysis.

Application Insights tiles display application performance metrics. Add tiles for request latency, failure rates, and dependencies. These tiles provide application-specific visibility.

Infrastructure monitoring tiles provide VM and container visibility. Add tiles for key VMs and AKS clusters. Integration with infrastructure monitoring completes the operational picture.

Azure Service Health tiles display Azure service status. Include these tiles during Azure incidents for quick correlation. Service health integration helps identify external causes of issues.

### Dashboard Management

Effective dashboard management ensures continued usefulness. This exercise reviews and optimizes existing dashboards. Regular review maintains dashboard effectiveness.

Review dashboard usage to identify frequently viewed dashboards. Focus optimization efforts on high-use dashboards. Remove unused dashboards to reduce clutter.

Update dashboards as monitoring requirements change. Add new tiles for new monitoring needs. Remove tiles for deprecated services or processes.

Document dashboard usage and ownership. Documentation ensures appropriate maintenance and supports knowledge transfer. Include dashboard purpose and update procedures in documentation.

## Integration with AKS

### Enabling Container Insights

Container Insights provides comprehensive monitoring for Azure Kubernetes Service. This exercise enables Container Insights for an AKS cluster. Proper configuration ensures complete container visibility.

Navigate to the AKS cluster in Azure Portal. Select Insights from the monitoring section. Select Enable to enable Container Insights.

The enable process deploys the monitoring agent to the cluster. Deployment uses DaemonSet to ensure agent presence on all nodes. Configuration may take several minutes for initial deployment.

Verify collection after enabling. Navigate to Container Insights and verify metrics and logs appear. Data should appear within minutes of enabling.

### Configuring AKS Monitoring

Custom configuration enhances AKS monitoring beyond default settings. This exercise configures custom metrics collection and log filtering. Appropriate configuration captures necessary data while controlling costs.

Configure data collection settings in Container Insights. Select Data collection settings from configuration options. Configure which namespaces to collect and which to exclude.

Log filtering reduces unnecessary log volume. Filter verbose logs that do not provide operational value. Configure exclusion rules for non-essential log categories.

Custom metrics enable application-specific monitoring. Deploy Prometheus scraping to collect custom application metrics. Integration with Azure Monitor provides metric storage and alerting.

### Creating AKS Dashboards

AKS-specific dashboards provide targeted visibility into Kubernetes workloads. This exercise creates a dashboard for AKS operational monitoring. Effective dashboards improve container operational visibility.

Create dashboard tiles for key AKS metrics. Add tiles for node CPU, memory, and pod status. These tiles provide quick operational assessment.

Add log query tiles for common troubleshooting queries. Pod logs, node logs, and K8s events support investigation. Include these tiles for quick access to diagnostic data.

Service health tiles display AKS and dependent service status. Include these tiles for incident correlation. Service health integration helps identify external causes.

### Monitoring AKS Applications

Application monitoring within AKS requires additional configuration beyond cluster monitoring. This exercise configures Application Insights for applications in AKS. Complete monitoring requires both infrastructure and application visibility.

Deploy Application Insights to AKS using code-based or agent-based configuration. Code-based configuration uses the Application Insights SDK. Agent-based Java configuration requires no code changes.

Configure application insights resource for AKS. The instrumentation key connects deployed applications to correct resources. Environment variable configuration passes the key to containers.

Verify application monitoring after configuration. Application Insights should show application data. Begin with basic requests and dependency tracking.

### AKS Alert Configuration

Alert configuration for AKS ensures proactive notification of cluster issues. This exercise configures alerts for common AKS scenarios. Appropriate alerting enables quick response to issues.

Create alerts for node resource constraints. Alert on CPU and memory limits approaching thresholds. Early notification enables capacity management before user impact.

Create alerts for pod health issues. Alert on pod restarts, crashes, and deployment issues. Pod alerts identify application problems before broad impact.

Create alerts for cluster-level issues. Alert on API server availability and hub health. Cluster alerts identify infrastructure problems affecting all workloads.

## Practical Scenarios

### Incident Response with Azure Monitor

Azure Monitor supports effective incident response through comprehensive visibility. This exercise walks through using monitoring during an incident. Practical experience improves response efficiency.

Begin incident response with alert notification. Review alert details to understand the affected area. Alert history provides context for current incidents.

Use metrics to assess current state. Check affected resource metrics for anomaly identification. Metrics provide real-time state visibility.

Use logs for detailed investigation. Query logs for error patterns and user impact. Log queries identify root cause and remediation steps.

Document findings throughout the incident. Documentation improves post-incident review. Include relevant queries and observations.

### Performance Analysis

Performance analysis using Azure Monitor identifies optimization opportunities. This exercise analyzes application performance using monitoring data. Practical analysis skills improve performance management.

Baseline performance using historical metrics. Identify normal ranges for key metrics. Baseline comparison identifies anomalies requiring attention.

Trend analysis over time reveals patterns. Analyze metrics over days, weeks, and months. Trends inform capacity planning and optimization efforts.

Compare performance across resources. Identify high and low performers. Performance comparison identifies best practices worth replicating.

### Cost Optimization

Monitoring data supports cost optimization through visibility into resource utilization. This exercise uses monitoring for cost optimization. Understanding utilization enables right-sizing.

Analyze VM utilization for right-sizing opportunities. Identify underutilized VMs suitable for smaller sizes. Right-sizing reduces compute costs while maintaining performance.

Analyze storage utilization for cleanup opportunities. Identify unused disks and blobs. Storage cleanup reduces storage costs.

Analyze monitoring costs for optimization opportunities. Review data ingestion for unnecessary collection. Optimize collection to reduce monitoring costs.

## Summary

Practical Azure Monitor implementation combines fundamental knowledge with hands-on skills. Alert configuration, log queries, dashboards, and AKS integration provide comprehensive monitoring capabilities. These skills prepare you for certification exams and operational responsibilities.

Your monitoring skills enable effective cloud operations. Continue practicing through hands-on implementation in your Azure environment. Build monitoring solutions that meet your organization's requirements.

Continue exploring Azure Monitor through documentation and hands-on experience. The platform continuously evolves with new capabilities. Stay current through ongoing learning and practice.

## Related Files

- [01_Basic_Azure_Monitor.md](./01_Basic_Azure_Monitor.md)
- [02_Advanced_Azure_Monitor.md](./02_Advanced_Azure_Monitor.md)