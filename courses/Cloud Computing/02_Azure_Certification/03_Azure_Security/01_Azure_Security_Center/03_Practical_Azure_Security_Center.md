---
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Security_Center.md, 02_Advanced_Azure_Security_Center.md
RelatedFiles: 01_Basic_Azure_Security_Center.md, 02_Advanced_Azure_Security_Center.md
UseCase: Managing security
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

# Practical Azure Security Center

## Introduction

This document provides hands-on guidance for using Azure Security Center in practical operational scenarios. As an Azure Administrator, you will need to enable Security Center, review and action security recommendations, set up security alerts, and configure security policies. These practical skills are essential for maintaining a secure Azure environment and are tested on the AZ-104 Azure Administrator exam.

The tasks covered in this document build upon the foundational and advanced concepts presented in the prerequisite documents. You should understand secure score, security recommendations, and the basic Security Center architecture before proceeding. The practical guidance here focuses on the implementation steps required to secure an Azure environment.

Each section includes step-by-step instructions for performing common administrative tasks. The instructions assume that you have appropriate permissions to perform the described actions. In production environments, you should verify that you have the required RBAC roles before attempting to configure settings.

## Enabling Azure Security Center

### Prerequisites for Enabling Security Center

Before enabling Azure Security Center, ensure that you have the necessary permissions and that your subscription meets the requirements. Azure Security Center requires an active Azure subscription with at least contributor access to the subscription. The subscription must not be from a free trial offer that does not support Security Center.

The minimum required RBAC role for enabling Security Center is Contributor on the subscription. However, to configure policies and security settings, you will typically need Owner or Security Admin permissions. Verify your role before attempting to configure settings.

Security Center automatically discovers Azure resources in your subscription. No additional agent deployment is required for Azure virtual machines running the Azure VM Agent. For hybrid scenarios that include on-premises servers, you may need to configure additional settings.

### Enabling Security Center Through the Portal

To enable Security Center through the Azure portal, navigate to the Security Center service. The portal URL is https://portal.azure.com/#blade/Microsoft_Azure_Security/SecurityMenuBlade. If Security Center is not visible in your portal, verify that you have the required permissions.

On the Security Center overview page, click the "Start upgrade" button if prompted. This initiates the upgrade to Defender for Cloud if you are currently using the Free tier. Select the Defender plans you want to enable for your subscription.

When selecting Defender plans, consider your workload types and security requirements. The following table provides guidance for common scenarios:

| Workload Type | Recommended Plan | Consider Also |
|--------------|-----------------|--------------|
| General Azure VMs | Defender for Servers | Storage, SQL if applicable |
| Production Databases | Defender for SQL | Servers if VMs host databases |
| Storage Resources | Defender for Storage | All if storage is critical |
| Container Workloads | Defender for Containers | Servers if running Kubernetes |

### Enabling Defender Plans

To enable Defender plans for your subscription, navigate to the Security Center sidebar and select "Environment settings". Find your subscription in the list and click on it to open the settings blade.

In the Defender plans section, enable the plans appropriate for your workloads. Each plan can be enabled independently, allowing you to implement a phased approach. Consider starting with the Servers plan and adding others as needed.

After enabling plans, wait several minutes for the changes to take effect. The first threat protection assessments may take additional time to complete. You can verify that plans are enabled by returning to this blade and confirming that the status shows as enabled.

### Verifying the Enable

After enabling Security Center, verify that the service is active and functioning correctly. Return to the Security Center overview page and confirm that the security score is displayed. The initial score may take some time to calculate as resources are assessed.

Check the "Security alerts" section to confirm that alert processing is active. If alerts are generated within your environment, they will appear here. In new environments, the absence of alerts is expected until your resources are assessed.

Navigate to the "Recommendations" section to verify that security recommendations are being generated. The recommendations should include items relevant to your workspace types. If no recommendations appear, verify that resources exist in your subscription.

## Reviewing and Managing Recommendations

### Understanding the Recommendations Dashboard

The Security Center recommendations dashboard provides a centralized view of all security findings in your environment. Access it through the Security Center sidebar by clicking "Recommendations". The dashboard displays recommendations grouped by category, severity, and resource type.

Each recommendation includes detailed information about the security issue, affected resources, and remediation steps. The severity indicator (High, Medium, Low) helps prioritize which recommendations to address first. The secure score impact shows how implementing the recommendation will affect your score.

The dashboard supports filtering and sorting to focus on specific areas. You can filter by severity, category, resource type, or subscription. Sorting options include by severity, name, or secure score impact. These features help focus remediation efforts efficiently.

### Actioning Recommendations

To implement a recommendation, click on the recommendation to view its details. The detail blade shows the affected resources, the security issue explanation, and remediation steps. For recommendations that can be remediated automatically, an "Apply" or "Fix" button may be available.

For manual remediation, follow the provided remediation steps. Each step should be performed in the order specified. After completing remediation, return to the recommendation and mark it as resolved. This updates the secure score to reflect the improvement.

For recommendations affecting multiple resources, you can bulk-remediate if automatic remediation is supported. Select the affected resources using checkboxes, then click the appropriate action button. Confirmation will be required before remediation proceeds.

### Prioritizing Recommendations

Not all recommendations require immediate attention. Effective prioritization considers severity, secure score impact, and resource criticality. High-severity recommendations affecting production resources should be addressed first.

Create a remediation plan that addresses recommendations in priority order. Document the planned order and assigned owners. Regular review of the plan ensures that remediation proceeds according to schedule.

Recommendations that cannot be addressed for technical reasons can be dismissed. When dismissing, provide a reason that documents why the recommendation is not applicable. Periodically review dismissed recommendations to determine if circumstances have changed.

### Tracking Remediation Progress

Track remediation progress using the secure score timeline. Navigate to the overview page and review the score history. The score should improve as recommendations are implemented.

Create a weekly or monthly review process for Security Center recommendations. Regular review ensures that new recommendations are addressed promptly. Assigning specific individuals to recommendation categories ensures accountability.

Export recommendation data for reporting and analysis. The export functionality supports CSV format for import into reporting tools. Regular exports enable trend analysis and management reporting.

## Setting Up Security Alerts

### Understanding Security Alerts

Security alerts are generated when Security Center detects potential security threats. Alerts include information about the detected threat, affected resources, and recommended response actions. Alert severity ranges from Critical to Informational.

Alerts are processed through the Azure portal, API, or integration with external systems. The portal provides a visual interface for alert review and management. API access enables programmatic alert processing and integration.

Alert data is retained for 90 days in Security Center. For longer retention, configure export to Log Analytics or event hubs. Long-term retention supports forensic analysis and compliance requirements.

### Configuring Alert Notifications

To receive notifications of security alerts, configure email notifications in Security Center. Navigate to "Price & settings" in the Security Center sidebar, then select your subscription. Configure the notification settings to specify recipients and alert severities.

Email notifications can be sent to multiple recipients. Consider sending to a security distribution list rather than individual emails. This ensures that notifications are seen even when primary recipients are unavailable.

You can also configure Microsoft Teams notifications through Logic Apps. Create a Logic App that receives Security Center alerts and posts them to a Teams channel. This provides real-time visibility into security events within existing communication tools.

### Integrating with Azure Sentinel

For advanced alert processing, integrate Security Center with Azure Sentinel. Azure Sentinel provides sophisticated analytics and automation capabilities. The integration enables correlation of Security Center alerts with data from other sources.

To configure the integration, locate the "Data connectors" section in Azure Sentinel. Find the Microsoft Defender for Cloud connector and configure it with your Security Center workspace. Connection typically requires Reader permissions on the Security Center workspace.

After integration, Security Center alerts appear in Azure Sentinel along with other security data. Azure Sentinel analytics can correlate these alerts with other data sources for enhanced detection. Automated response playbooks can process alerts automatically.

### Managing Alert Rules

In Azure Sentinel, you can create custom analytics rules to detect additional threats. Analytics rules use Kusto Query Language (KQL) to analyze security data. Rules can generate new alerts when specific conditions are met.

Create custom alert rules based on your organization's threat landscape. Consider common attack vectors specific to your industry. Testing rules against historical data validates their effectiveness.

Alert rules should be reviewed and updated regularly. As the threat landscape evolves, new detection logic may be required. Regular review ensures that detection capabilities remain current.

## Configuring Security Policies

### Understanding Policy Architecture

Security policies define the security requirements that Security Center assesses. Policies are organized into initiatives, which group related policies together. The Azure Security Benchmark initiative provides comprehensive security baseline coverage.

Policies can be assigned at subscription or management group levels. Policy assignments at higher levels apply to child subscriptions through inheritance. This enables centralized policy management across the organization.

Policy settings determine whether policies are auditor enabled or enforce mode. Audit mode identifies violations without blocking deployments. Enforce mode prevents non-compliant deployments, providing preemptive security controls.

### Accessing Policy Settings

To access policy settings, navigate to "Security policy" in the Security Center sidebar. Select the subscription or management group for which you want to configure policies. The policy assignments blade shows the initiatives currently assigned.

To assign a new initiative, click "Add initiative assignment". Select the initiative from the available definitions. Configure the parameters including assignment name, category, and enforcement mode.

Built-in initiatives include the Azure Security Benchmark, CIS benchmarks, and regulatory standards. Custom initiatives can be created for organization-specific requirements. Custom initiatives require Azure Policy definition creation.

### Creating Custom Policies

Custom policies extend Security Center assessment for organization-specific requirements. Custom policies are created using Azure Policy definition format. The policy definition includes the logic for evaluating resource configurations.

To create a custom policy, navigate to Azure Policy in the Azure portal. Create a policy definition using the Azure Policy definition format. Include the policy rules that specify the conditions being evaluated.

After creating the policy definition, add it to a Security Center initiative. This makes the policy part of your security assessment. The custom policy will then generate recommendations like built-in policies.

### Setting Enforcement Controls

Enforcement controls prevent non-compliant resource deployments. When enabled, Resource Manager deployments that would violate security policies are blocked. This provides preemptive security controls.

To enable enforcement, edit the policy assignment and set enforcement mode to "True". Understand that enforcement can block legitimate deployments. Test enforcement in audit mode before enabling.

When deploying with enforcement enabled, carefully test deployments in non-production environments first. Resolve any policy violations before deploying to production. Enforcement violations should include clear error messages to guide remediation.

## Ongoing Security Operations

### Daily Security Operations

Effective security operations require regular attention to Security Center. Establish daily habits for reviewing the security dashboard. This ensures that new security issues are identified and addressed promptly.

Each day, review the secure score for any significant changes. Significant decreases may indicate new recommendations or security alerts. Investigate score decreases to understand and address the cause.

Review any new security alerts that were generated. High and critical severity alerts should be addressed immediately. Document alert response actions for audit purposes.

### Weekly Security Reviews

Weekly reviews provide more comprehensive security assessment. Set aside time each week for thorough review of Security Center recommendations. This includes reviewing recommendations across all subscriptions.

Weekly tasks should include reviewing compliance status if regulatory standards are enabled. Assess progress toward compliance goals. Identify any new compliance findings that require attention.

Review and update any security automation that is in place. Test automated responses to ensure they continue to function correctly. Update automation logic as needed based on changes in the environment.

### Monthly Security Reporting

Monthly security reporting supports management visibility into security posture. Create monthly reports that include secure score trends, outstanding recommendations, and security alerts. Use exported data to create standardized reports.

Reports should highlight significant changes or incidents. Include recommendations that have been addressed and those that remain outstanding. This documents security improvement progress.

Share reports with relevant stakeholders. Provide context for findings and remediation progress. Seek additional resources or exception approvals as needed.

## Common Practical Scenarios

### Securing Virtual Machines

Virtual machines require specific security configurations for production use. Security Center provides recommendations for VM security including network security, patch management, and disk encryption.

Ensure that network security groups are properly configured for each VM. Restrict unnecessary inbound access. Use just-in-time access for administrative access when possible.

Enable disk encryption for VMs that handle sensitive data. Azure Disk Encryption uses BitLocker (Windows) or dm-crypt (Linux). Ensure that encryption keys are stored securely in Key Vault.

Regularly apply security patches to VMs. Security Center can identify VMs that are missing security patches. Establish a patching cadence that balances availability with security.

### Securing Storage Accounts

Storage accounts contain data that requires protection. Security Center provides recommendations for storage security including secure access, encryption, and network access.

Enable secure transfer required for storage accounts. This ensures that data is encrypted in transit. Disable legacy protocols that do not support encryption.

Configure blob immutability for data that must be retained unmodified. Immutability policies prevent data modification or deletion. This supports compliance and data integrity requirements.

Use Azure Storage analytics to monitor access patterns. Identify unusual access that may indicate compromise. Configure alerts for anomalous access patterns.

### Securing Database Resources

Database resources including Azure SQL and Cosmos DB require specific security configurations. Security Center provides recommendations for authentication, encryption, and network access.

Use Azure Active Directory authentication for database access. This provides centralized identity management and audit capabilities. Avoid SQL authentication where possible.

Enable transparent data encryption for databases. This ensures that data is encrypted at rest. Encryption keys should be managed through Azure Key Vault.

Configure firewall rules to restrict database access. Limit access to specific IP addresses or virtual networks. Avoid allowing public internet access to databases.

## Troubleshooting Common Issues

### Recommendations Not Appearing

If recommendations are not appearing, verify that resources exist in the subscription. Security Center only generates recommendations for resources it can assess. New resources may take time to be assessed.

Check that the correct subscription is selected in the Security Center portal. Recommendations may be filtered by subscription. Ensure that all relevant subscriptions are included.

Verify that Security Center is enabled for the subscription. Some subscriptions may have Security Center disabled. Navigate to Price & Settings to verify.

### Alerts Not Generating

If security alerts are not generating, verify that Defender plans are enabled. The Free tier does not include threat detection. Enable appropriate Defender plans for threat alerts.

Verify that the subscription has resources that would generate alerts. New subscriptions may not have activity that triggers alerts. Simulate alert conditions to test.

Check the alert settings to ensure notifications are configured correctly. Verify email addresses and notification preferences. Check spam folders for notification emails.

### Policy Issues

If policies are not working as expected, verify the policy assignment settings. Check that policies are assigned to the correct scope. Ensure that the policy definition exists.

Test policies using the Azure Policy "Test" functionality. This validates policy logic without applying enforcement. Policy syntax errors can prevent proper evaluation.

Check Azure Policy insights for evaluation errors. The insights show any errors that occurred during policy evaluation. Errors can indicate permission issues or definition problems.

## Summary

Practical Security Center operations require ongoing attention and established processes. Enable Security Center and Defender plans to establish your security foundation. Regular review of recommendations ensures that security issues are addressed promptly.

Configure alerting and notification to ensure that security events are communicated. Integration with Azure Sentinel provides advanced capabilities for enterprise environments. Policy configuration enables preventive security controls.

Establish daily, weekly, and monthly security review processes. Track remediation progress through secure score trends. Regular reporting supports management visibility and continuous improvement.

The practical skills in this document prepare Azure Administrators for the operational aspects of cloud security. Combined with foundational knowledge and advanced concepts, they provide comprehensive preparation for maintaining secure Azure environments.