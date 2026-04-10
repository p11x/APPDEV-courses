---
Category: Azure Certification
Subcategory: Azure Security
Concept: Azure Security Center
Purpose: Understanding Azure Security Center for cloud security posture
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Security_Center.md, 03_Practical_Azure_Security_Center.md
UseCase: Cloud security
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

# Basic Azure Security Center

## Introduction to Azure Security Center

Azure Security Center is Microsoft's unified security management system that provides advanced threat protection across hybrid cloud workloads. Formerly known as Azure Security Center, it has evolved into Microsoft Defender for Cloud, which represents Microsoft's comprehensive cloud security posture management (CSPM) and cloud workload protection solution. Understanding the fundamentals of this service is essential for anyone preparing for Azure certifications or working with Azure cloudenvironments.

Azure Security Center was designed to address the unique security challenges that organizations face when migrating to the cloud. Traditional on-premises security tools and approaches often fall short when dealing with the dynamic nature of cloud resources, which can be provisioned and decommissioned rapidly. Security Center provides continuous security assessment, real-time threat detection, and actionable recommendations to help organizations maintain a strong security posture in their Azure environments.

The service integrates deeply with Azure's infrastructure, automatically discovering and securing resources as they are deployed. This agentless approach means organizations can begin using Security Center immediately without needing to install additional software on their virtual machines. The centralized dashboard provides a single pane of glass for viewing security status across all subscribed Azure resources, on-premises servers, and even resources in other cloud providers.

Security Center operates on a principle of prevention, detection, and response. Preventive measures include security policies that define the desired configuration state of resources, along with security recommendations that guide users toward implementing security best practices. Detection capabilities analyze resource configurations, network traffic, and behavioral patterns to identify potential security threats. Response capabilities include automated remediation actions and integration with security information and event management (SIEM) systems for incident handling.

## Core Features of Azure Security Center

### Security Posture Management

Security Center provides comprehensive security posture management capabilities that enable organizations to assess, monitor, and improve the security of their cloud resources. The security posture refers to an organization's overall ability to protect its environment from cyber threats, encompassing preventive controls, detective controls, and corrective controls. Security Center continuously monitors resources and compares their configurations against security best practices defined in industry standards such as the Center for Internet Security (CIS) benchmarks.

The assessment engine analyzes resource configurations across multiple dimensions including network security, identity management, data protection, and compute configurations. For virtual machines, this includes checking whether appropriate firewall rules are in place, whether encryption is enabled for disks, and whether the latest security patches have been applied. For storage accounts, checks verify whether blob immutability is enabled, whether secure transfer is required, and whether access levels are appropriately restricted.

Security Center assigns a secure score to each subscription, which serves as a quantitative measure of the organization's security posture. This score starts at zero and increases as security recommendations are implemented. The score is calculated based on the severity of identified issues, the number of resources affected, and the potential security impact of addressing each finding. Organizations can track their secure score over time to measure improvements in their security posture.

The service provides detailed recommendations for each security finding, explaining what the issue is, why it matters from a security perspective, and how to remediate it. Recommendations are categorized by severity (high, medium, low) and by the security control they relate to. This structured approach helps security teams prioritize their remediation efforts based on the potential impact of each security finding.

### Threat Protection

Azure Security Center includes integrated threat protection capabilities that detect suspicious activities and potential attacks in real-time. The threat protection features analyze network traffic, operating system events, authentication events, and application behavior to identify indicators of compromise. When suspicious activity is detected, Security Center generates security alerts that include details about the nature of the threat, affected resources, and recommended response actions.

The threat detection capabilities leverage Microsoft's global threat intelligence, which processes trillions of signals daily from across Microsoft's ecosystem of services. This includes signals from Azure, Microsoft 365, Microsoft Dynamics 365, and other Microsoft services. The threat intelligence is continuously updated to reflect new attack techniques and known malicious IP addresses, ensuring that detection capabilities remain current against evolving threats.

Security Center integrates with Azure Sentinel for advanced security analytics and incident response. Azure Sentinel is Microsoft's cloud-native SIEM solution that provides sophisticated analytics and automated response capabilities. The integration allows security alerts from Security Center to be correlated with other data sources in Azure Sentinel, enabling more comprehensive security monitoring and incident response workflows.

The just-in-time VM access feature provides an additional layer of protection by limiting administrative access to virtual machines. When enabled, users must request access through Security Center before connecting to VMs via RDP or SSH. These requests are approved automatically unless configured for manual approval, and access is granted for a limited duration. This approach reduces the attack surface by ensuring that VMs are not continuously exposed to administrative access attempts from the internet.

### Security Policy and Governance

Security Center enables organizations to define and enforce security policies that align with their organizational security requirements. Policies can be defined at the management group level for enterprise-wide coverage or at the subscription level for more granular control. Each policy includes a set of rules that determine which resources are monitored and how they are assessed.

The policy framework supports regulatory compliance standards out of the box, including standards such as CIS, Payment Card Industry Data Security Standard (PCI DSS), and ISO 27001. Organizations can also create custom policies using Azure Policy definitions. This flexibility enables Security Center to support industry-specific security requirements and organizational policies.

Policies can be set to audit-only mode, where they identify violations without taking automatic action, or they can be set to enforce mode, where they prevent resource deployment if the configuration would violate security standards. The enforcement mode integrates with Azure Resource Manager templates and deployment pipelines to provide preemptive security controls.

The initiative definitions in Security Center group related policies together for easier management. For example, the Azure Security Benchmark initiative includes all policies that implement the security controls defined in the Azure Security Benchmark. Organizations can assign initiatives to subscriptions and track compliance across multiple standards simultaneously.

## Secure Score Explained

### Understanding the Secure Score

The secure score is a numerical representation of an organization's security posture, expressed as a percentage of the maximum possible score. The score is calculated by analyzing the security recommendations that apply to an organization's resources and determining how many of those recommendations have been implemented. Each recommendation has a point value based on its severity and security impact, with high-severity issues contributing more points than low-severity issues.

The secure score calculation begins with the maximum possible score, which represents full implementation of all security recommendations. As security findings are identified and left unaddressed, points are deducted from the current score. The score updates in real-time as recommendations are implemented or new resources are provisioned that introduce new security considerations. This dynamic scoring ensures that organizations always have an accurate view of their current security posture.

Security Center displays the secure score at multiple levels of granularity. The overall score represents the entire subscription, while category-specific scores show security posture for individual areas such as compute, networking, storage, and identity. This category breakdown helps organizations identify specific areas where security improvements are needed most urgently.

The secure score history is retained for 90 days, allowing organizations to track improvements over time. The history view shows daily snapshots of the score, making it easy to identify trends and measure the effectiveness of security improvement initiatives. Organizations can also export the score history for integration with external reporting tools.

### Improving Your Secure Score

Improving the secure score requires implementing the security recommendations provided by Security Center. Each recommendation includes clear remediation steps that, when followed, will resolve the security finding and restore the points associated with that recommendation. The recommendations panel shows the potential score increase that would result from implementing each recommendation, helping organizations prioritize their remediation efforts.

The highest-impact recommendations typically relate to enabling security features that provide broad protection across multiple resources. For example, enabling Azure Defender provides threat protection across all supported resources, which can significantly improve the secure score. Similarly, configuring vulnerability assessment for virtual machines addresses security findings across all assessed VMs, maximizing the return on remediation effort.

Organizations should create a remediation plan that prioritizes recommendations based on their security impact and the effort required to implement them. High-severity recommendations that can be implemented quickly should be addressed first, as they provide the greatest security improvement with minimal effort. Lower-priority recommendations that require significant architectural changes can be scheduled for future implementation.

Security Center provides the ability to dismiss recommendations that do not apply to the organization's environment. When a recommendation is dismissed, it is removed from the secure score calculation, reflecting that the organization has made an informed decision not to address that particular finding. This capability ensures that the secure score accurately represents the organization's actual security posture.

## Azure Security Center Pricing Tiers

### Free Tier

The Free tier of Azure Security Center provides basic security management capabilities at no cost to Azure subscribers. This tier is automatically enabled for all Azure subscriptions and provides continuous security assessment and security recommendations for Azure resources. Organizations can use the Free tier to gain visibility into their security posture and receive guidance on implementing security best practices.

The Free tier includes security assessment for Azure resources including virtual machines, storage accounts, virtual networks, and Azure SQL databases. The assessment compares resource configurations against built-in security best practices and generates recommendations when deviations are identified. These recommendations include remediation steps that can be followed to improve the security posture.

The secure score is available in the Free tier, allowing organizations to track their security posture over time. The score provides a quantitative measure that can be used to set security improvement goals and measure progress. However, certain advanced features such as regulatory compliance assessments and threat protection are not available in the Free tier.

Users of the Free tier can access Security Center through the Azure portal, where they will see the security dashboard and recommendations panel. The portal interface provides filtering and sorting capabilities that help users focus on the most important security findings. Recommendations can be examined individually, with full details about the security issue and remediation steps.

### Defender for Cloud (Paid Tiers)

The paid tiers of Azure Security Center, collectively known as Defender for Cloud, provide enhanced security capabilities including advanced threat protection and regulatory compliance management. These tiers are licensed on a per-resource basis, with pricing determined by the number and type of resources protected. The paid tiers build upon the capabilities of the Free tier, adding additional security features.

The Defender for Cloud pricing is structured around workload protection plans. Each plan covers specific resource types: Defender for Storage covers storage accounts, Defender for SQL covers Azure SQL databases and related services, Defender for Servers covers virtual machines, and Defender for Containers covers container registries and Kubernetes workloads. Organizations can enable individual plans based on their specific needs.

The P1 tier includes threat detection capabilities that analyze resource behavior and network traffic to identify potential security threats. When threats are detected, security alerts are generated with details about the nature of the threat and recommended response actions. The P1 tier also includes regulatory compliance assessments that compare configurations against industry standards such as CIS and PCI DSS.

The P2 tier adds advanced threat protection capabilities including security analytics and machine learning-based detection. P2 also includes vulnerability assessment for virtual machines and containers, identifying security vulnerabilities in installed software. The Just-in-Time VM access feature is available in P2, providing additional protection for administrative access to virtual machines.

### Feature Comparison

| Feature | Free Tier | P1 (Defender) | P2 (Defender) |
|---------|----------|---------------|---------------|
| Security Assessment | Yes | Yes | Yes |
| Secure Score | Yes | Yes | Yes |
| Security Recommendations | Yes | Yes | Yes |
| Threat Detection Alerts | No | Yes | Yes |
| Regulatory Compliance | No | Yes | Yes |
| Just-in-Time VM Access | No | No | Yes |
| Vulnerability Assessment | No | No | Yes |
| Security Analytics | No | No | Yes |

The pricing model allows organizations to start with the Free tier and add paid plans as their security requirements mature. The hybrid approach enables organizations to protect critical workloads with paid plans while maintaining basic security visibility across all resources. This flexibility ensures that organizations can scale their security investment based on their specific needs and budget constraints.

## Azure Security CLI Commands

### Getting Started with az security

Azure Security Center can be managed through the Azure CLI using the az security commands. These commands provide programmatic access to security settings, recommendations, alerts, and regulatory compliance information. The CLI enables automation of security tasks through scripts and integration with CI/CD pipelines.

To view the secure score using the CLI, use the following command:

```bash
az security show --query "score.percentage"
```

This command retrieves the current secure score as a percentage value. The output can be captured in scripts to track security posture over time or used in automation workflows that trigger alerts when the score falls below a threshold.

To list all security recommendations, use the following command:

```bash
az security recommendations list
```

This command returns all active security recommendations for the subscription. The output includes recommendation details such as the affected resource, the security issue, and the remediation steps. The results can be filtered using the --query parameter to focus on specific issue categories or severity levels.

### Working with Security Alerts

To view security alerts, use the following command:

```bash
az security alerts list
```

This command returns all security alerts generated by Security Center. Each alert includes details about the threat, affected resources, and recommended response actions. The alerts can be filtered by time range, severity, and other criteria using the available parameters.

To get details about a specific alert, use the following command:

```bash
az security alerts show --alert-name "<alert-name>"
```

Replace <alert-name> with the name of the alert to retrieve detailed information. The output includes the full alert details including the attack chain, if available, and recommended response actions.

To acknowledge or dismiss alerts, use the following command:

```bash
az security alerts update --alert-name "<alert-name>" --status "[Dismissed|Acknowledged]"
```

This command updates the status of an alert, marking it as acknowledged or dismissed. Acknowledged alerts are typically being worked on, while dismissed alerts have been reviewed and determined to be false positives or otherwise not requiring action.

### Managing Security Policies

To view the effective security policy for a subscription, use the following command:

```bash
az security policy show
```

This command returns the security policy definitions that are in effect for the subscription. The output includes all policy assignments and their configurations. Policies can be modified using the az security policy assignment commands.

To list all security policy definitions available in Security Center, use the following command:

```bash
az security definition list
```

This command returns all built-in security definitions that can be used to create security policies. Each definition includes the logic that determines whether a resource meets the security requirement.

### Regulatory Compliance

To view the regulatory compliance assessment results, use the following command:

```bash
az security regulatory-compliance standards show --name "<standard-name>"
```

Replace <standard-name> with the name of the compliance standard such as "PCI-DSS" or "ISO-27001". This command returns the compliance status for each control in the selected standard, showing which controls are compliant and which have findings.

To assess compliance using a specific regulatory standard, organizations must first enable the standard in Security Center. This can be done through the Azure portal or using the CLI commands for regulatory compliance settings. Once enabled, compliance assessments are automatically performed as part of the Security Center assessment process.

## Integration with Azure Services

### Azure Monitor Integration

Azure Security Center integrates with Azure Monitor to provide comprehensive logging and alerting capabilities. Security alerts are automatically sent to Azure Monitor, where they can be processed by alert rules or forwarded to external SIEM systems. The integration enables organizations to create custom alert rules based on security findings.

The Log Analytics workspace integration allows security data to be centralized for analysis. Security Center can be configured to forward security recommendations, alerts, and compliance data to a Log Analytics workspace. This data can then be queried using Kusto Query Language (KQL) for custom analysis and reporting.

The Azure Monitor integration supports automated response workflows using action groups. When security alerts are generated, they can trigger automated actions such as sending email notifications, creating ITSM tickets, or invoking webhook URLs. This automation enables rapid response to security threats without manual intervention.

### Azure Policy Integration

Azure Security Center leverages Azure Policy for security assessment and enforcement. Policy definitions determine which configurations are assessed and how findings are generated. The integration enables Security Center to use the full range of Azure Policy capabilities including policy assignments, initiatives, and exclusions.

Organizations can create custom policy definitions for organization-specific security requirements. These custom policies can be added to Security Center initiatives to extend the built-in security assessment capabilities. Custom policies support advanced assessment logic using Azure Policy language features.

The Azure Policy integration also enables preventive security controls through policy enforcement. When policies are set to deny deployments that violate security standards, Security Center can prevent resources from being deployed in non-compliant configurations. This approach ensures that security best practices are followed from the moment resources are created.

## Best Practices for Getting Started

### Initial Configuration Steps

When starting with Azure Security Center, organizations should first review the security dashboard to understand their current security posture. The dashboard provides an overview of the secure score, pending recommendations, and active security alerts. This initial assessment helps identify the most critical security issues that need attention.

The first action should be to enable Azure Defender plans for critical workloads. Organizations should identify the resource types that are most critical to their operations and enable the corresponding Defender plans. The Defender plans add threat protection capabilities that are essential for detecting and responding to security threats.

Organizations should also review and customize the security email notifications. Security Center can be configured to send email notifications when new recommendations are generated or when security alerts occur. Configuring these notifications ensures that the appropriate personnel are informed of security issues in a timely manner.

Finally, organizations should establish a regular review cadence for security recommendations. Weekly or monthly reviews of pending recommendations help ensure that security issues are addressed in a timely manner. Assigning responsibility for recommendation remediation to specific individuals ensures accountability for security improvements.

### Ongoing Security Operations

Effective use of Security Center requires ongoing attention to security recommendations and alerts. Organizations should establish processes for reviewing and addressing security findings, treating Security Center recommendations as a prioritized task list for the security team.

The secure score should be tracked over time as a key performance indicator for security operations. Improvements in the secure score demonstrate the effectiveness of security improvement initiatives. A declining secure score may indicate new security issues that need attention.

Security alerts should be reviewed and responded to promptly, following the organization's incident response procedures. Security Center alerts that are not addressed may represent actual security breaches that are continuing to impact the organization. Integration with Azure Sentinel enables more sophisticated alert processing and automated response.

## Conclusion

Azure Security Center provides the foundation for security management in Azure cloud environments. Its comprehensive assessment capabilities, threat protection features, and secure score metric enable organizations to understand and improve their security posture. Whether supporting compliance requirements, protecting against threats, or simply following security best practices, Security Center provides the tools and guidance needed to secure Azure workloads effectively.

The free tier provides sufficient capabilities for many organizations to get started with security management, while the paid Defender plans add the advanced threat protection capabilities needed for production workloads. The integration with Azure Policy and Azure Monitor enables automation and customization that can adapt Security Center to organization-specific requirements.

Understanding Security Center is essential for Azure certification exams, particularly the AZ-900 Azure Fundamentals exam. The concepts covered in this document, including secure score, pricing tiers, and key features, represent the core knowledge areas that are tested on these exams. Practice with the az security CLI commands will reinforce this understanding and prepare you for exam success.