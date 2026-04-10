---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Monitor
Purpose: Understanding Azure Monitor for observability
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Monitor.md, 03_Practical_Azure_Monitor.md
UseCase: Cloud monitoring
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

# Azure Monitor: Comprehensive Observability Platform

## Introduction

Azure Monitor is Microsoft's unified observability service for comprehensive cloud monitoring, providing end-to-end visibility into your applications, infrastructure, and network. As organizations migrate to cloud environments, having robust monitoring capabilities becomes essential for maintaining application health, performance, and security. Azure Monitor serves as the central platform for collecting, analyzing, and acting on telemetry data from across your entire Azure ecosystem and hybrid cloud environments.

The platform addresses the fundamental challenges of modern cloud operations by providing a unified approach to monitoring that eliminates the need for multiple disconnected tools. Whether you are running virtual machines, containerized applications, serverless functions, or complex microservices architectures, Azure Monitor delivers consistent monitoring capabilities across all these resources. This comprehensive approach to observability enables teams to quickly identify issues, understand root causes, and take automated actions to maintain service quality.

Understanding Azure Monitor is particularly important for the AZ-900 Azure Fundamentals certification, as it demonstrates your understanding of cloud monitoring concepts and Microsoft's approach to operational excellence. The service integrates seamlessly with other Azure services, enabling organizations to build comprehensive monitoring solutions that scale with their cloud adoption journey. From small development environments to enterprise-scale production deployments, Azure Monitor provides the flexibility and capabilities needed to meet diverse monitoring requirements.

This guide will take you through the foundational concepts of Azure Monitor, focusing on the core components that form the basis of any monitoring strategy. You will learn about metrics and logs, the fundamental data types that Azure Monitor collects, and how these provide insights into your cloud resources. Additionally, we will explore Log Analytics for querying and analyzing collected data, and the alerting system that enables proactive issue resolution. By the end of this guide, you will have a solid foundation in Azure Monitor that will serve you well in both certification preparation and real-world cloud operations.

## Understanding Azure Monitor Architecture

### Core Components Overview

Azure Monitor consists of several integrated components that work together to provide comprehensive observability. At its foundation, Azure Monitor collects telemetry data from Azure resources, applications, and operating systems. This data then flows through various processing stages, enabling different analytical and alerting capabilities. Understanding this architecture is crucial for effectively implementing monitoring solutions and troubleshooting issues when they arise.

The primary data collection mechanisms include Azure Agent for virtual machines, diagnostic settings for Azure services, and automatic ingestion from integrated Azure services. These collection agents and settings gather metrics and logs, transmitting them to Azure Monitor for processing and storage. The platform handles data at scale, supporting organizations with thousands of resources and millions of data points per day. This scalability ensures that monitoring overhead remains minimal while still capturing comprehensive telemetry data.

Data storage in Azure Monitor utilizes two primary destinations: Azure Monitor Metrics for time-series numerical data and Azure Monitor Logs for structured log data. Each storage type serves different analytical purposes and integrates with different tools and APIs. Metrics store supports efficient querying for performance data, while Logs store enables detailed diagnostic analysis through Log Analytics. The separation of these data types optimizes both storage efficiency and query performance for different use cases.

The visualization and analysis layer includes Azure Portal dashboards, Log Analytics for interactive queries, and various integration points for third-party tools. These components enable teams to explore monitoring data, create custom visualizations, and share insights across the organization. The modular design of Azure Monitor ensures that you can start with basic monitoring and progressively adopt more advanced capabilities as your requirements evolve.

### Data Types and Sources

Azure Monitor collects two fundamental types of data that serve different monitoring purposes. Understanding the distinction between these data types is essential for effective monitoring implementation and query optimization. Each type has specific characteristics that make it suitable for particular analytical scenarios, and both play crucial roles in comprehensive observability.

Metrics represent numerical measurements collected at regular intervals, providing a quantitative view of system behavior over time. These measurements include CPU usage, memory consumption, network bandwidth, request latency, and countless other performance indicators. Metrics are particularly useful for identifying trends, detecting anomalies, and triggering alerts based on thresholds. The efficient storage and querying of metrics supports real-time monitoring and historical analysis spanning months or years of data.

Logs contain detailed event records that provide context about system operations and occurrences. These records range from simple informational messages to complex diagnostic data including stack traces, configuration changes, and security events. Logs enable deep troubleshooting by providing context that metrics alone cannot convey. Every application event, system change, and error condition can be captured in logs for later analysis and audit purposes.

Data sources span the entire Azure resource portfolio, including virtual machines running Windows or Linux, Azure Kubernetes Service clusters, Azure App Service web applications, Azure SQL databases, and many more. Additionally, Azure Monitor can collect data from on-premises resources through the Azure Arc-enabled servers feature. This comprehensive source coverage ensures consistent monitoring regardless of where your resources reside.

## Metrics in Azure Monitor

### Understanding Platform Metrics

Platform metrics are automatically collected from Azure resources without requiring any configuration. These metrics provide baseline visibility into resource health and performance, covering the most common monitoring scenarios out of the box. Understanding which platform metrics are available for your resources helps you plan effective monitoring strategies without unnecessary complexity.

Virtual machines expose numerous platform metrics including CPU percentage, network bytes sent and received, disk read and write operations, and memory usage. These metrics enable quick assessment of VM performance and capacity planning. Azure Monitor automatically collects these metrics at one-minute intervals for standard virtual machines, with premium tier virtual machines supporting one-second granularity for more detailed analysis.

Azure SQL Database provides platform metrics for database throughput units, DTU consumption, deadlocks, storage used, and connection failures. These metrics enable database administrators to monitor performance and quickly identify resource constraints or configuration issues. The platform metrics integrate with Azure Monitor's alerting system, enabling automated notifications when thresholds are exceeded.

Storage accounts expose metrics for transaction counts, latency, availability, and capacity utilization. These metrics help ensure storage services meet performance requirements and enable capacity planning for storage growth. The rich set of storage metrics supports comprehensive monitoring of blob, table, queue, and file storage services.

### Custom Metrics

Custom metrics extend monitoring beyond platform metrics, enabling collection of application-specific performance data. Organizations can define custom metrics that align with their business requirements and application architecture. This capability is particularly valuable for monitoring business-critical processes that require metrics not available from platform implementations.

The Azure Monitor REST API enables programmatic submission of custom metrics from applications. Applications send metric values along with dimensional data that enables granular filtering and analysis. Applications can submit metrics in batch for efficiency, with Azure Monitor aggregating values over one-minute intervals. This approach supports high-volume metric collection without overwhelming the monitoring infrastructure.

Custom metrics integrate with the same alerting and visualization capabilities as platform metrics. This integration ensures that custom metrics receive the same treatment as standard metrics, enabling consistent operational processes. Teams can create alerts on custom metrics just like platform metrics, using familiar threshold and dynamic criteria.

### Metrics Explorer

Metrics Explorer provides the primary interface for visualizing and analyzing metrics data in Azure Monitor. This tool enables interactive exploration of metrics across resources, time ranges, and aggregation methods. The intuitive interface supports both beginners learning monitoring concepts and experienced professionals performing detailed performance analysis.

The interface allows selection of multiple metrics for comparison, with options to apply different aggregations including average, minimum, maximum, sum, and count. Users can filter metrics by resource, resource group, or subscription, enabling focused analysis on specific areas of interest. The time range selector supports quick navigation from real-time data to historical analysis spanning months.

Chart customization options enable users to create visualizations tailored to their specific analysis needs. Users can add multiple charts to a view, pin charts to Azure dashboards, and share views with team members. This flexibility supports various monitoring scenarios from ad-hoc troubleshooting to regular performance reviews.

## Logs in Azure Monitor

### Log Categories

Azure Monitor Logs organize log data into categories that reflect the type of source and data collected. Each category contains specific types of events and fields that enable targeted analysis. Understanding these categories helps you locate relevant log data when troubleshooting issues and designing efficient log queries.

Audit logs capture security-relevant events including authentication attempts, authorization decisions, and configuration changes. These logs support compliance requirements and security incident investigation. Azure Active Directory audit logs provide comprehensive coverage of identity and access management activities. Security teams rely on these logs for threat detection and forensic analysis.

Diagnostic logs provide resource-specific operational data that varies by Azure service. These logs capture detailed information about resource operations, enabling troubleshooting of specific service issues. Configuring diagnostic logs requires explicit enablement for each resource, allowing organizations to balance logging detail against cost considerations.

Activity logs capture operational events at the subscription level, including resource creation, modification, and deletion. These logs provide an audit trail of all control plane operations within a subscription. The immutable nature of activity logs ensures reliable historical records for compliance and governance purposes.

### Log Analytics Fundamentals

Log Analytics serves as the primary interface for querying and analyzing Azure Monitor Logs. This powerful tool supports the Kusto Query Language (KQL) for flexible data exploration and analysis. Learning KQL is essential for effective log analysis and forms the foundation of advanced monitoring scenarios.

Basic queries retrieve log records matching specific criteria, enabling straightforward filtering and presentation. Queries start with a table reference, apply filters using where operators, and specify columns to display. The query language supports various data types including strings, numbers, dates, and times. Understanding basic query structure enables quick retrieval of relevant log data.

Aggregation operations enable analysis beyond simple record retrieval. Queries can group data by dimensions, calculate statistics including sums, averages, and percentiles, and visualize results. These operations support trend analysis, capacity planning, and performance optimization efforts. The aggregation syntax enables complex analytical scenarios with familiar SQL-like patterns.

Time-based analysis utilizes the timestamp field common to all Azure Monitor logs. Queries can filter by time ranges, perform time series analysis, and compare values across time periods. This temporal focus enables identification of when issues occurred and how performance changes over time. The timestamp handling supports both real-time monitoring and historical investigation.

### Log Analytics Workspace

Log Analytics workspaces serve as the logical container for log data in Azure Monitor. Each workspace stores logs from configured data sources, providing isolation and access control for different teams and purposes. Workspace configuration determines data retention, access controls, and pricing tier characteristics.

Workspace design should consider data isolation requirements, data residency needs, and cost optimization. Organizations may use separate workspaces for different departments, environments, or regulatory domains. Azure Lighthouse enables cross-subscription monitoring while maintaining appropriate access controls.

Data retention configuration in workspaces balances visibility requirements against storage costs. Azure Monitor supports retention periods from 30 to 730 days, with options for different retention per data type. Organizations should evaluate their retention requirements based on compliance needs and troubleshooting patterns. Longer retention enables historical analysis but increases storage costs proportionally.

Cost management for Log Analytics depends on data volume and retention. The pay-as-you-go pricing model charges per gigabyte of data ingested and stored. Organizations should monitor data volumes and implement optimization strategies including log exclusion rules and retention adjustment. These optimizations can significantly reduce monitoring costs without sacrificing operational visibility.

## Azure Monitor Alerts

### Alert Types and Concepts

Azure Monitor alerts provide proactive notification when monitoring data indicates conditions requiring attention. The alert system evaluates conditions continuously, triggering notifications when criteria are met. Effective alert configuration ensures that teams respond quickly to issues while avoiding alert fatigue from excessive notifications.

Metric alerts evaluate numerical threshold conditions against metrics data, triggering when values exceed or fall below defined thresholds. These alerts support common monitoring scenarios including CPU usage exceeding limits, disk space running low, or request latency increasing beyond acceptable levels. Metric alerts can evaluate conditions over multiple time periods, reducing spurious alerts from momentary spikes.

Activity log alerts respond to specific activity log events, enabling notification when significant operations occur. These alerts support monitoring for resource changes, security events, and service health notifications. Activity log alerts can trigger automation runbooks for automated response to specific events.

Log search alerts evaluate complex conditions using KQL queries, enabling sophisticated alerting scenarios. These alerts support detection of patterns that metric alerts cannot identify, including error rate thresholds, specific error messages, or business transaction failures. Log search alerts can evaluate conditions at specific intervals or trigger based on the query results.

### Action Groups

Action groups define the recipients and notification methods for Azure Monitor alerts. Proper action group configuration ensures that alerts reach appropriate team members through appropriate channels. Action groups support multiple notification types including email, SMS, voice call, and webhook integrations.

Email notifications provide immediate awareness of alert conditions for on-call personnel and distribution lists. The email includes alert details, affected resources, and links to additional information in Azure Monitor. Organizations should configure appropriate distribution lists to ensure prompt attention to alerts.

SMS and voice call notifications provide high-priority alert delivery for critical conditions. These notification methods ensure awareness even when email may go unnoticed. Organizations should carefully select which alerts warrant these notification types to avoid desensitization to critical alerts.

Webhook integrations enable automated response to alert conditions. Integrations can trigger Azure Functions, Logic Apps, or external systems for automated remediation. This automation capability supports DevOps practices and reduces mean time to resolution for known issue patterns.

### Alert Configuration Best Practices

Effective alert configuration requires balancing notification frequency against operational requirements. Organizations should start with critical alerts and progressively add alerts as monitoring patterns emerge. Regular alert review ensures continued relevance and prevents alert accumulation that createsnoise.

Alert recommendations in Azure Monitor provide starting points for common monitoring scenarios. These recommendations analyze your resource configuration and suggest alerts based on best practices. Organizations should evaluate recommendations and implement appropriate alerts for their environment.

Alert management includes regular review of alert history and optimization of alerting rules. Teams should investigate frequent alerts to determine whether underlying issues require remediation. Alert history analysis reveals patterns that may indicate systematic problems requiring attention beyond alert configuration.

## Azure CLI Monitoring Commands

### Basic az monitor Commands

The Azure CLI provides comprehensive commands for interacting with Azure Monitor. These commands enable script-based automation and integration with deployment pipelines. Understanding available commands helps you automate monitoring tasks and implement infrastructure-as-code practices.

The az monitor command group provides access to all monitoring functionality. Subcommands include metrics, log-analytics, app-insights, and diagnostic-settings. Each subcommand group provides operations specific to the associated monitoring component.

Metric retrieval uses the az monitor metrics command to query metric data. The command supports various aggregation methods, time ranges, and filtering options. Results can be formatted as table or JSON output for further processing. This capability enables script-based metric collection for dashboards and automation.

### Log Query Commands

Log Analytics queries can be executed using Azure CLI for script-based analysis. The az monitor log-analytics command provides query execution capabilities. Queries return results in formats suitable for processing and integration with other tools.

Query execution syntax uses the query parameter for KQL queries. Results return as JSON by default, enabling straightforward parsing in scripts. The workspace reference specifies the Log Analytics workspace for query execution. Scripts can parameterize queries for flexible analysis across different data sets.

Result formatting options support different consumption patterns. Table format provides human-readable output for terminal display. JSON format enables programmatic processing in scripts and automation. The format parameter controls output type based on use case requirements.

### Alert Management Commands

Azure CLI supports full alert lifecycle management including creation, modification, and deletion. The az monitor alert command group provides these capabilities. Automation of alert management ensures consistent monitoring configuration across environments.

Alert rule creation requires specification of condition, action group, and alert parameters. The command syntax varies by alert type, with metric and log search alerts having different parameters. Scripts should validate alert creation to ensure proper configuration before deployment.

Alert rule management also includes listing, showing, and updating existing alert rules. These commands enable audit and governance of monitoring configuration. Organizations should implement alerting management as part of their infrastructure-as-code practices.

## Practical Applications

### Setting Up Basic Monitoring

Implementing basic monitoring for Azure resources requires systematic configuration of diagnostic settings and alert rules. Organizations should develop monitoring standards that ensure consistent visibility across their resource portfolio. These standards should define baseline alerts and logging configuration for common resource types.

Diagnostic settings configuration enables log collection for Azure resources. Each resource type supports different diagnostic log categories, and organizations should evaluate their logging needs against cost considerations. Start with critical log categories and expand as operational requirements emerge.

Baseline alert configuration should include critical performance and availability indicators. Standard alerts for CPU, memory, disk, and network provide foundational monitoring. Organizations should expand baseline alerts based on application requirements and operational experience.

### Monitoring Dashboards

Azure Monitor integrates with Azure dashboards for visualization of monitoring data. Custom dashboards can aggregate monitoring data across resources into unified views. These dashboards support operational visibility and management review requirements.

Dashboard creation uses the Azure Portal or programmatic approaches. Portal-based creation provides interactive design with drag-and-drop components. Programmatic approaches using Azure CLI or ARM templates enable version-controlled dashboard management.

Dashboard sharing enables team visibility into operational status. Dashboards can be shared with specific users or made available organization-wide. Access controls ensure appropriate visibility while maintaining security requirements.

### Integration with Azure Services

Azure Monitor integrates with numerous Azure services for comprehensive monitoring. These integrations extend monitoring coverage beyond Azure resources to include applications and dependencies. Understanding available integrations helps you design complete monitoring solutions.

Azure Security Center integration enables security monitoring and threat protection. Security findings appear in Azure Monitor for correlation with operational data. This integration supports security operations and compliance requirements.

Azure Service Health provides visibility into Azure service status and planned maintenance. Azure Monitor can correlate service health events with application monitoring data. This correlation helps identify whether application issues result from Azure service problems or application configuration.

## Certification Preparation

### Key Concepts for AZ-900

Understanding Azure Monitor concepts for the AZ-900 exam requires familiarity with core monitoring terminology and capabilities. Candidates should understand the difference between metrics and logs, the role of Log Analytics, and alert configuration basics. The exam tests practical understanding rather than deep technical knowledge.

Key topics include Azure Monitor data types, Log Analytics purpose, alert configuration, and dashboard capabilities. Candidates should understand when to use each monitoring component and how components work together. Practical experience with Azure Monitor significantly improves exam readiness.

Practice questions should cover monitoring configuration scenarios and interpretation of monitoring data. Candidates should be able to determine appropriate monitoring approaches for different situations. Understanding the breadth of Azure Monitor capabilities ensures comprehensive exam preparation.

## Summary

Azure Monitor provides comprehensive observability capabilities for Azure and hybrid cloud environments. The platform's unified approach to metrics and logs simplifies monitoring implementation while delivering powerful analytical capabilities. Understanding core components including Metrics, Logs, Log Analytics, and Alerts forms the foundation for effective cloud operations.

Your monitoring strategy should evolve with your cloud adoption, starting with basic alerts and progressively incorporating more sophisticated capabilities. Azure Monitor's modular design enables this progressive adoption without requiring upfront commitment to advanced features. As your operational requirements mature, additional capabilities like Application Insights and container monitoring extend your visibility.

The skills you have developed in this guide provide a solid foundation for both certification success and real-world cloud monitoring. Continue exploring Azure Monitor features through hands-on practice in your Azure environment. As you gain experience, you will discover additional capabilities that further enhance your operational visibility.

## Next Steps

Continue your learning journey with the advanced Azure Monitor concepts in the next guide, covering Application Insights, Container Insights, and enterprise monitoring scenarios. These advanced topics build on the foundational knowledge from this guide and prepare you for more sophisticated monitoring implementations.

Practical Azure Monitor skills in the third guide provide hands-on experience with alert configuration, log queries, and dashboard creation. This practical focus enables immediate application of monitoring concepts in your environment.

##RelatedFiles

- [02_Advanced_Azure_Monitor.md](./02_Advanced_Azure_Monitor.md)
- [03_Practical_Azure_Monitor.md](./03_Practical_Azure_Monitor.md)
- [01_Basic_Azure_Core.md](../09_Azure_Core_Services/01_Basic_Azure_Core.md)