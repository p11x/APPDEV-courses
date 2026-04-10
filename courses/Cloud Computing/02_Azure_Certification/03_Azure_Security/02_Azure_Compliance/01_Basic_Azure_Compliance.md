---
Category: Azure Certification
Subcategory: Azure Security
Concept: Azure Compliance
Purpose: Understanding Azure compliance offerings
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Compliance.md, 03_Practical_Azure_Compliance.md
UseCase: Compliance management
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

# Basic Azure Compliance

## Introduction to Azure Compliance

Azure compliance represents Microsoft's commitment to helping organizations meet their regulatory and compliance obligations when using Microsoft Azure cloud services. As organizations increasingly migrate workloads to the cloud, understanding compliance frameworks, certifications, and tools becomes essential for IT professionals and security teams alike.

This guide provides a foundational understanding of Azure's compliance offerings, including major certifications, compliance tools, and resources available to organizations. The content aligns with the AZ-900 Azure Fundamentals exam objectives and provides the groundwork for more advanced compliance management topics.

Microsoft invests billions annually in compliance and security infrastructure, maintaining one of the most extensive compliance portfolios in the cloud industry. Azure's compliance program covers over 90 certifications and attestations, making it one of the most comprehensive cloud compliance offerings available.

## Understanding Compliance Certifications

Compliance certifications independent verification that a cloud service provider meets specific security, privacy, and controls requirements. These certifications are typically awarded by third-party auditing organizations and provide customers with assurance about the security of their data in the cloud.

### ISO/IEC 27001 Certification

ISO 27001 is an international standard for information security management systems (ISMS). Azure has achieved ISO 27001 certification for its cloud services, demonstrating that Microsoft has implemented systematic approaches to managing sensitive company information and security risks.

The certification covers:
- Information security policies
- Organization of information security
- Human resource security
- Asset management
- Access control
- Cryptography
- Physical and environmental security
- Operations security
- Communications security
- System acquisition, development, and maintenance
- Supplier relationships
- Incident management
- Business continuity management
- Compliance

Azure's ISO 27001 certification applies to multiple cloud services and is regularly audited by external parties to maintain compliance. Organizations can use this certification as part of their own compliance documentation when conducting vendor assessments.

### SOC Certifications

SOC (Service Organization Control) reports provide detailed information about a service organization's controls relevant to security, availability, processing integrity, confidentiality, and privacy. Azure maintains several SOC certifications that are essential for enterprise customers.

SOC 1 certification focuses on financial reporting controls and is important for organizations that use Azure for applications affecting financial statements. The report provides details about the design and operating effectiveness of controls.

SOC 2 certification examines five trust service criteria: security, availability, processing integrity, confidentiality, and privacy. Azure undergoes regular audits to maintain SOC 2 compliance, providing customers with assurance about its controls.

SOC 3 certification is a shorter, seal-publication report that provides a high-level overview of SOC 2 controls without detailed testing procedures. Organizations can publicly share SOC 3 reports to demonstrate their security posture.

The SOC 2+ report combines multiple frameworks including SOC 2, ISO 27001, and ISO 27017 standards, providing comprehensive coverage of cloud security controls. This combined approach simplifies compliance reporting for organizations managing multiple certifications.

### GDPR Compliance

The General Data Protection Regulation is a comprehensive data protection law enforced in the European Union. Azure provides extensive GDPR compliance capabilities to help organizations meet their obligations under this regulation.

Key GDPR compliance features include:
- Data processing agreements that define roles and responsibilities
- Data subject rights management capabilities
- Data breach notification mechanisms
- Data location controls
- Consent management tools
- Privacy impact assessments

Microsoft was one of the first cloud providers to offer GDPR-compliant services, and its contractual commitments extend GDPR rights to customers using Azure services. The company provides detailed data processing terms that address GDPR requirements.

Azure's GDPR compliance extends to sub-processors, ensuring that the entire supply chain maintains appropriate data protection standards. Organizations can review the list of sub-processors and object to new processor additions.

Customers retain full control of their data in Azure and can implement various encryption, access control, and data retention policies to meet their specific GDPR obligations. Azure provides tools for data subject access requests and data portability.

### HIPAA Compliance

The Health Insurance Portability and Accountability Act establishes requirements for protecting sensitive patient health information in the United States. Azure offers HIPAA-compliant cloud services for healthcare organizations.

HIPAA compliance in Azure requires a Business Associate Agreement (BAA) between Microsoft and the customer. This agreement establishes the responsibilities of each party regarding protected health information (PHI).

Azure's HIPAA compliance covers:
- Physical safeguards for data centers
- Technical safeguards for data protection
- Administrative safeguards for policies and procedures
- Organizational requirements for business associates
- Documentation requirements

Healthcare organizations can use Azure for HIPAA-covered workloads by implementing appropriate administrative, physical, and technical safeguards. Microsoft provides the underlying secure infrastructure while customers maintain responsibility for their specific implementations.

The HIPAA BAAs are available to enterprise customers and cover various Azure services. Organizations should review the specific services covered under the BAA before deploying healthcare applications.

### Additional Certifications and Frameworks

Beyond the major certifications, Azure supports numerous additional compliance frameworks that address specific industry and regional requirements.

FedRAMP (Federal Risk and Authorization Management Program) provides a standardized approach to security assessment and authorization for cloud products used by U.S. federal agencies. Azure achieved FedRAMP High authorization, covering the most stringent security requirements.

PCI DSS (Payment Card Industry Data Security Standard) provides requirements for organizations handling credit card data. Azure maintains PCI DSS compliance, enabling customers to build payment applications on Azure infrastructure.

NIST (National Institute of Standards and Technology) frameworks provide guidance for cybersecurity risk management. Azure aligns with various NIST special publications, including NIST 800-53 for security controls.

ITU (International Telecommunication Union) recommendations provide telecommunications security standards that apply to global cloud services. Azure maintains compliance with relevant ITU recommendations.

Industry-specific certifications include those for financial services, government, education, and healthcare. Organizations should review their specific industry requirements when selecting Azure services.

## Azure Compliance Manager

Azure Compliance Manager is a unified compliance management tool that helps organizations manage their compliance posture across Azure and other Microsoft cloud services. The tool provides assessments, recommendations, and tracking capabilities for various compliance frameworks.

### Getting Started with Compliance Manager

Compliance Manager provides a dashboard that aggregates compliance information from multiple frameworks into a single view. Organizations can assess their compliance posture and identify gaps requiring attention.

To access Compliance Manager, organizations need appropriate Azure roles including Compliance Reader, Compliance Assessmentor, or Compliance Administrator. These roles are available through Azure Active Directory integration.

The initial setup involves connecting Compliance Manager to the organization's Azure environment and selecting applicable compliance frameworks. The tool then automatically detects implemented controls and maps them to compliance requirements.

Compliance Manager provides:
- Continuous compliance monitoring
- Risk-based scoring
- Recommended improvement actions
- Regulatory update tracking
- Audit readiness capabilities

The dashboard displays overall compliance scores along with specific scores for each framework. Organizations can drill down into specific controls to understand their compliance status.

### Understanding Compliance Scores

Compliance scores represent the percentage of implemented controls relative to total controls required for a specific framework. Scores are calculated based on implemented, partially implemented, and non-implemented controls.

The scoring system uses:
- Implemented (100%): Control requirements fully met
- Partially Implemented (50%): Some requirements met
- Planned (0%): Implementation planned but not complete
- Not Applicable (0%): Control does not apply to the organization
- Not Implemented (0%): No implementation started

Organizations can improve scores by implementing recommended controls and documenting existing implementations. Compliance Manager tracks progress over time and identifies regression issues.

Score breakdowns by category help organizations prioritize improvement efforts. Categories often include identity and access management, data protection, threat protection, and governance.

### Building Compliance Assessments

Compliance Manager enables organizations to create custom assessments based on their specific compliance requirements. Assessments can combine multiple regulatory frameworks into unified views.

Creating an assessment involves:
1. Selecting the applicable regulation or framework
2. Defining scope (specific services or subscriptions)
3. Mapping existing controls
4. Identifying gaps
5. Creating improvement tasks

The assessment wizard guides organizations through each step, providing recommendations based on Azure service configurations. Organizations can customize assessments to match their specific environments.

Assessments can be exported for audit purposes or shared with external auditors. The export includes detailed control implementations and supporting evidence.

### Managing Improvement Actions

Improvement actions are specific tasks that organizations can complete to enhance their compliance posture. Compliance Manager tracks these actions and monitors completion progress.

Each improvement action includes:
- Detailed implementation guidance
- Estimated effort and timeline
- Related compliance requirements
- Technical documentation links
- Responsibility assignments

Organizations can assign improvement actions to team members and track progress through the dashboard. The tool provides notifications for upcoming and overdue actions.

Priority scoring helps organizations focus on high-impact improvements first. Actions with greater compliance score impact are highlighted for immediate attention.

## Azure Trust Center

The Azure Trust Center is Microsoft's central resource for compliance and trust information. It provides documentation, reports, and resources to help organizations understand Azure's security and compliance posture.

### Trust Center Resources

The Trust Center provides access to:
- Compliance certifications and reports
- Whitepapers and technical documentation
- Audit reports and attestations
- Data protection resources
- Security information

Organizations can search the Trust Center for specific compliance requirements and find relevant documentation. The search functionality helps locate documents quickly.

The Trust Center includes an interactive compliance map that shows Azure's certifications by region and service. Organizations can filter based on their specific requirements.

### Accessing Audit Reports

Azure publishes various audit reports through the Trust Center, including SOC reports, ISO certifications, and penetration testing results. These reports provide detailed information about Azure's security controls.

Audit report access requires registration and acceptance of non-disclosure terms. Reports are updated regularly and available for download in PDF format.

Organizations can use these reports as part of their vendor assessment process. The reports provide independent verification of Azure's security controls.

### Data Protection Resources

The Trust Center includes extensive documentation about how Microsoft protects customer data in Azure. This includes information about:
- Data encryption standards
- Key management practices
- Data residency options
- Data deletion procedures
- Incident response processes

Organizations can review these resources to understand how their data is protected and to support their own compliance documentation. The resources are updated as Microsoft implements new security measures.

The data protection resources also cover customer responsibilities for specific configurations. Understanding the shared responsibility model is essential for maintaining compliance.

### Trust Documentation

Trust documentation provides technical details about Azure's security architecture, including:
- Network security architecture
- Physical security measures
- Identity and access management
- Data encryption approaches
- Operational security practices

These documents support organizations conducting technical reviews of Azure services. They provide the detailed information needed to assess Azure's suitability for specific workloads.

Trust documentation also includes information about third-party audits and certifications. Organizations can verify independent validation of Azure's security practices.

## Compliance in Azure Portal

The Azure Portal provides built-in compliance features that help organizations monitor and manage their compliance posture directly from the management interface.

### Compliance Dashboard

The Azure Portal compliance dashboard provides:
- Service-specific compliance information
- Regulatory compliance status
- Security recommendations
- Audit log access

Organizations can access compliance information from the portal by navigating to the Security service. The dashboard provides an overview of the organization's compliance posture.

The dashboard integrates with Compliance Manager for organizations that have implemented the tool. This integration provides unified views of compliance status across Azure services.

### Policy Compliance

Azure Policy is a service that helps enforce organizational standards and assess compliance at scale. Organizations can create custom policies or use built-in definitions.

Policy features include:
- Resource consistency enforcement
- Compliance assessment at scale
- Remediation automation
- Policy exemption management

Organizations can use Azure Policy to automatically assess new resources for compliance and remediate non-compliant configurations. This proactive approach helps maintain continuous compliance.

### Regulatory Compliance

Azure regulatory compliance features provide built-in mappings to common regulatory frameworks. Organizations can use these mappings to assess their compliance with regulations like GDPR, ISO 27001, and NIST.

The regulatory compliance dashboard displays:
- Assigned compliance standards
- Current compliance status
- Non-compliant resources
- Remediation recommendations

Organizations can assign specific standards and customize assessments based on their requirements. The dashboard provides actionable information for improving compliance.

## Cloud Compliance Comparison

Understanding how Azure's compliance compares with other cloud providers helps organizations make informed decisions about their cloud strategy.

### Major Cloud Provider Certifications

All major cloud providers maintain extensive compliance certifications to support enterprise customers. However, specific certifications and coverage may vary.

Common certifications maintained by cloud providers include:
- ISO 27001 (information security)
- SOC 1, 2, 3 (control reports)
- FedRAMP (U.S. federal)
- HIPAA (healthcare)
- PCI DSS (payments)
- GDPR (data protection)

Azure's compliance portfolio includes over 90 certifications, providing comprehensive coverage for most organizational requirements. Providers typically maintain certifications for their core services.

### Regional Compliance

Compliance requirements vary by region, and cloud providers maintain region-specific certifications to meet local requirements.

Azure's sovereign cloud deployments provide compliance with regional data localization requirements. These specialized deployments meet specific government and regulatory requirements.

AWS and GCP also maintain regional certifications and offer sovereign cloud options. Organizations should evaluate regional requirements when selecting cloud providers.

### Compliance Tools

Cloud providers offer various compliance management tools to help organizations assess and maintain compliance.

Azure Compliance Manager provides unified compliance management across Microsoft cloud services. The tool integrates with Azure Policy and other management services.

AWS offers AWS Artifact for compliance report access and AWS Config for compliance assessment. These tools provide similar capabilities to Azure's offerings.

GCP provides Security Command Center and Asset Inventory for compliance management. Organizations can assess their compliance posture across GCP services.

### Shared Responsibility Model

Cloud compliance operates under a shared responsibility model where providers and customers share compliance responsibilities.

Cloud providers are responsible for:
- Physical infrastructure security
- Network infrastructure
- Hypervisor security
- Physical data center controls

Customers are responsible for:
- Data classification
- Identity and access management
- Application security
- Data encryption decisions
- Configuration management

Understanding the shared responsibility model is essential for maintaining proper compliance. Organizations must implement their side of the responsibility equation.

## Summary

Azure provides comprehensive compliance offerings through certifications, tools, and resources that help organizations meet their regulatory obligations. Key takeaways include:

1. Azure maintains over 90 certifications and attestations, covering major frameworks like ISO 27001, SOC, GDPR, and HIPAA.

2. Azure Compliance Manager provides unified compliance management with assessments, scores, and improvement recommendations.

3. The Azure Trust Center serves as the central resource for compliance documentation, audit reports, and trust information.

4. Cloud compliance operates under a shared responsibility model, requiring organizations to implement their side of compliance controls.

5. Azure's compliance tools integrate with the Azure Portal for seamless compliance management across services.

These foundational concepts prepare organizations for more advanced compliance management topics covered in the advanced and practical compliance guides.

## Next Steps

After completing this basic compliance overview, continue learning with:
- Advanced Azure Compliance for enterprise compliance strategies
- Practical Azure Compliance for hands-on compliance management

Explore Azure Compliance Manager in the Azure Portal to begin assessing your organization's compliance posture. Review the Trust Center for detailed compliance documentation relevant to your requirements.