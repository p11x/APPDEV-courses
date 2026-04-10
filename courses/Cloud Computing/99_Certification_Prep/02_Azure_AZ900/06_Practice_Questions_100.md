---
Category: Certification Prep
Subcategory: Azure AZ-900
Concept: Practice Questions
Purpose: 100 practice questions for Azure Fundamentals certification exam
Difficulty: beginner
Prerequisites: Azure cloud concepts
RelatedFiles: 01_Exam_Guide.md, 02_Domain1_Cloud_Concepts.md, 03_Domain2_Core_Services.md, 04_Domain3_Security.md, 05_Domain4_Pricing_Support.md
UseCase: Azure Fundamentals certification exam preparation
CertificationExam: Microsoft Azure Fundamentals (AZ-900)
LastUpdated: 2025
---

# Azure Fundamentals Practice Questions (100 Questions)

## Questions 1-20

### Question 1
What is the primary benefit of cloud computing over traditional on-premises infrastructure?

A. Higher upfront costs
B. Pay-as-you-go pricing
C. Manual scaling
D. Longer deployment times

**Answer: B**  
**Explanation:** Cloud computing offers pay-as-you-go pricing, eliminating large upfront capital expenditures and allowing organizations to pay only for resources they use.

---

### Question 2
Which Azure availability option provides the highest level of availability for virtual machines?

A. Availability Set
B. Availability Zone
C. Single Instance
D. Backup and Restore

**Answer: B**  
**Explanation:** Availability Zones provide a 99.99% SLA by deploying VMs across physically isolated data centers within a region, offering superior redundancy.

---

### Question 3
What type of cloud deployment model uses Azure Arc for hybrid management?

A. Public Cloud
B. Private Cloud
C. Hybrid Cloud
D. Multi-Cloud

**Answer: C**  
**Explanation:** Azure Arc enables hybrid cloud management, allowing organizations to manage resources running in their own data centers alongside Azure resources.

---

### Question 4
Which Azure service provides fully managed virtual machines for Windows and Linux?

A. Azure App Service
B. Azure Functions
C. Azure Container Instances
D. Azure Virtual Machines

**Answer: D**  
**Explanation:** Azure Virtual Machines (VMs) provide Infrastructure as a Service (IaaS) with full control over the operating system.

---

### Question 5
A company needs to deploy a REST API that scales automatically. Which Azure service should they use?

A. Azure VMs
B. Azure App Service API Apps
C. Azure Container Instances
D. Azure Batch

**Answer: B**  
**Explanation:** Azure App Service API Apps provide auto-scaling, authentication, and deployment slots for REST APIs.

---

### Question 6
Which storage tier in Azure Blob Storage is most cost-effective for long-term archiving?

A. Hot
B. Cool
C. Cold
D. Archive

**Answer: D**  
**Explanation:** The Archive tier provides the lowest cost for storing data that is rarely accessed (typical retrieval time: 12+ hours).

---

### Question 7
What type of load balancer operates at Layer 7 (application layer)?

A. Azure Load Balancer
B. Application Gateway
C. VPN Gateway
D. CDN

**Answer: B**  
**Explanation:** Azure Application Gateway is a Layer 7 load balancer providing URL-based routing, SSL termination, and Web Application Firewall (WAF).

---

### Question 8
Which Azure database provides globally distributed NoSQL with single-digit millisecond latency?

A. Azure SQL Database
B. Azure Cosmos DB
C. Azure Database for MySQL
D. Azure SQL Managed Instance

**Answer: B**  
**Explanation:** Azure Cosmos DB is a globally distributed, multi-model NoSQL database with single-digit millisecond latency at the 99th percentile.

---

### Question 9
What is the main difference between Azure AD and on-premises Active Directory?

A. Protocol used
B. User management
C. Password policies
D. None

**Answer: A**  
**Explanation:** Azure AD uses HTTP/HTTPS protocols (OAuth, SAML) while on-premises AD uses LDAP and Kerberos.

---

### Question 10
Which service provides secure storage for secrets, keys, and certificates?

A. Azure Key Vault
B. Azure Security Center
C. Azure Sentinel
D. Azure Defender

**Answer: A**  
**Explanation:** Azure Key Vault provides secure storage for secrets, keys, certificates, and other sensitive information.

---

### Question 11
What does Multi-Factor Authentication (MFA) provide?

A. Faster login
B. Additional security layer
C. Single sign-on
D. Password reset

**Answer: B**  
**Explanation:** MFA provides an additional security layer by requiring two or more verification factors.

---

### Question 12
Which Azure service filters network traffic between subnets?

A. Azure Firewall
B. Network Security Group (NSG)
C. Azure AD
D. Application Gateway

**Answer: B**  
**Explanation:** Network Security Groups filter network traffic within a VNet, including between subnets, providing layer 3/4 security.

---

### Question 13
For which pricing model do you pay only for compute time used?

A. Reserved Instances
B. Pay-as-you-go
C. Spot VMs
D. Dev/Test pricing

**Answer: B**  
**Explanation:** Pay-as-you-go pricing means you pay only for the compute time you actually use, billed per second.

---

### Question 14
What savings can Reserved Instances provide compared to pay-as-you-go pricing?

A. Up to 50%
B. Up to 72%
C. Up to 90%
D. No savings

**Answer: B**  
**Explanation:** Reserved Instances can provide up to 72% savings compared to pay-as-you-go pricing for steady-state workloads.

---

### Question 15
Which Azure Support Plan provides 24/7 phone support?

A. Basic
B. Standard
C. Professional Direct
D. All paid plans

**Answer: D**  
**Explanation:** All paid Azure Support Plans (Standard, Professional Direct, Premier) include 24/7 phone support.

---

### Question 16
What is the SLA for a single Azure Virtual Machine?

A. 99%
B. 99.9%
C. 99.95%
D. 99.99%

**Answer: B**  
**Explanation:** A single Azure VM has a 99.9% (three 9s) SLA when using premium-managed disks.

---

### Question 17
Which Azure service is best for serverless event-driven code?

A. Azure VMs
B. Azure Functions
C. Azure Container Instances
D. Azure Batch

**Answer: B**  
**Explanation:** Azure Functions provides serverless compute that runs code in response to events, with pay-per-execution pricing.

---

### Question 18
What does Azure Cost Management provide?

A. Resource deployment
B. Cost tracking and analysis
C. Performance monitoring
D. Backup services

**Answer: B**  
**Explanation:** Azure Cost Management provides cost tracking, budget alerts, and analysis of Azure spending.

---

### Question 19
Which Azure service provides a unified security management dashboard?

A. Azure Sentinel
B. Azure Security Center
C. Azure Defender
D. Azure Key Vault

**Answer: B**  
**Explanation:** Azure Security Center provides unified security management across hybrid cloud workloads.

---

### Question 20
What is Azure Resource Manager (ARM)?

A. A deployment tool
B. A management layer
C. Both A and B
D. None

**Answer: C**  
**Explanation:** ARM is a deployment and management layer that provides consistent resource deployment and management capabilities.

---

## Questions 21-40

### Question 21
Which option best describes Platform as a Service (PaaS)?

A. You manage infrastructure only
B. Azure manages infrastructure and runtime
C. You manage everything
D. No management required

**Answer: B**  
**Explanation:** PaaS provides a managed platform where Azure handles infrastructure, OS, and runtime, while you manage applications and data.

---

### Question 22
What does a geography in Azure represent?

A. A single data center
B. A region
C. Multiple regions
D. A continent

**Answer: C**  
**Explanation:** A geography is a discrete market (typically a country/region) containing multiple Azure regions to meet data residency requirements.

---

### Question 23
Which Azure service provides SMB file storage?

A. Azure Blob Storage
B. Azure Disk Storage
C. Azure Files
D. Azure Queue Storage

**Answer: C**  
**Explanation:** Azure Files provides fully managed SMB file shares that can be mounted simultaneously on cloud and on-premises.

---

### Question 24
What is Azure CDN primarily used for?

A. Database acceleration
B. Content delivery
C. Backup storage
D. Email delivery

**Answer: B**  
**Explanation:** Azure Content Delivery Network (CDN) caches and delivers content from edge locations globally for lower latency.

---

### Question 25
Which Azure AD feature enables conditional access policies?

A. MFA
B. Conditional Access
C. Identity Protection
D. Application Proxy

**Answer: B**  
**Explanation:** Conditional Access allows policies based on signals like user, location, device, and risk level.

---

### Question 26
What is the main purpose of Azure DDoS Protection?

A. SQL injection prevention
B. DDoS attack mitigation
C. Firewall protection
D. Intrusion detection

**Answer: B**  
**Explanation:** Azure DDoS Protection provides defense against volumetric and protocol attacks.

---

### Question 27
Which Azure service is serverless for container hosting?

A. Azure VMs
B. Azure App Service
C. Azure Container Instances
D. Azure Kubernetes Service

**Answer: C**  
**Explanation:** Azure Container Instances provides serverless container hosting without managing underlying infrastructure.

---

### Question 28
What is the default TTL for Azure DNS?

A. 300 seconds
B. 600 seconds
C. 3600 seconds
D. 1 hour

**Answer: C**  
**Explanation:** The default TTL for Azure DNS is 3600 seconds (1 hour).

---

### Question 29
Which Azure service provides managed PostgreSQL?

A. Azure SQL Database
B. Cosmos DB
C. Azure Database for PostgreSQL
D. Azure Synapse

**Answer: C**  
**Explanation:** Azure Database for PostgreSQL provides a fully managed PostgreSQL database service.

---

### Question 30
What type of resources can Azure Policy enforce?

A. Resource configurations and compliance
B. User authentication
C. Network routing
D. Database queries

**Answer: A**  
**Explanation:** Azure Policy enforces organizational standards and assesses compliance for resource configurations.

---

### Question 31
Which Azure pricing option is best for fault-tolerant batch processing?

A. Pay-as-you-go
B. Reserved Instances
C. Spot VMs
D. Dev/Test pricing

**Answer: C**  
**Explanation:** Spot VMs offer up to 90% discount and are suitable for fault-tolerant batch jobs that can handle interruption.

---

### Question 32
What does Azure Advisor provide?

A. Cost recommendations
B. Security recommendations
C. Both A and B
D. Performance recommendations

**Answer: C**  
**Explanation:** Azure Advisor provides personalized recommendations for cost, security, performance, and reliability.

---

### Question 33
Which Azure storage provides locally redundant storage (LRS)?

A. 3 copies in one region
B. 3 copies in 3 regions
C. 6 copies in 3 regions
D. None

**Answer: A**  
**Explanation:** LRS maintains three copies of data within a single region.

---

### Question 34
What is Azure Private Link used for?

A. Public internet access
B. Private endpoint access
C. VPN connections
D. CDN access

**Answer: B**  
**Explanation:** Azure Private Link provides private connectivity between VNets and Azure services without going over the internet.

---

### Question 35
Which service automatically distributes traffic across VMs in an availability set?

A. Azure Load Balancer
B. Application Gateway
C. Traffic Manager
D. Both A and B

**Answer: A**  
**Explanation:** Azure Load Balancer operates at Layer 4 and distributes traffic across VMs in a scale set or availability set.

---

### Question 36
What is an Azure resource group?

A. A logical container for related resources
B. A physical container
C. A billing entity
D. An admin group

**Answer: A**  
**Explanation:** A resource group is a logical container that holds related Azure resources for a solution.

---

### Question 37
What does Azure ExpressRoute provide?

A. Internet VPN
B. Private dedicated connection
C. Public endpoint
D. CDN

**Answer: B**  
**Explanation:** ExpressRoute provides a private, dedicated connection from on-premises to Azure over a connectivity provider.

---

### Question 38
Which Azure feature enables disaster recovery between regions?

A. Azure Site Recovery
B. Azure Backup
C. Both A and B
D. Azure Archive

**Answer: A**  
**Explanation:** Azure Site Recovery provides business continuity and disaster recovery (BCDR) between regions.

---

### Question 39
What is Azure Virtual Desktop (AVD)?

A. Cloud-hosted virtual machines
B. Desktop-as-a-Service
C. Both A and B
D. On-premises VDI

**Answer: B**  
**Explanation:** Azure Virtual Desktop is a Desktop-as-a-Service (DaaS) that enables secure remote work.

---

### Question 40
What does Azure Metrics provide?

A. Cost monitoring
B. Resource performance monitoring
C. Security monitoring
D. Network monitoring

**Answer: B**  
**Explanation:** Azure Metrics provides performance data for Azure resources, enabling monitoring and alerting.

---

## Questions 41-60

### Question 41
Which RBAC role provides full administrative access?

A. Contributor
B. Reader
C. Owner
D. User Access Administrator

**Answer: C**  
**Explanation:** The Owner role provides full administrative access to resources, including the ability to manage permissions.

---

### Question 42
What is Azure Data Lake Storage?

A. Object storage for big data
B. File storage
C. Block storage
D. Archive storage

**Answer: A**  
**Explanation:** Azure Data Lake Storage isoptimized for big data analytics workloads, providing massive scale.

---

### Question 43
Which Azure service is used for container orchestration?

A. Azure Container Instances
B. Azure Kubernetes Service
C. Azure Batch
D. Azure Functions

**Answer: B**  
**Explanation:** Azure Kubernetes Service (AKS) provides managed Kubernetes for container orchestration.

---

### Question 44
What is a VNet in Azure?

A. A VPN service
B. An isolated network
C. A load balancer
D. A CDN

**Answer: B**  
**Explanation:** Azure Virtual Network (VNet) provides an isolated network environment in Azure for resources.

---

### Question 45
Which Azure service provides automatic scaling for web applications?

A. Azure VMs
B. Azure App Service
C. Azure Functions
D. Both B and C

**Answer: D**  
**Explanation:** Both Azure App Service and Azure Functions provide automatic scaling based on demand.

---

### Question 46
What does Azure Backup protect against?

A. Malware
B. Data loss
C. Network attacks
D. Hardware failure

**Answer: B**  
**Explanation:** Azure Backup provides data protection through backup and restore of Azure resources.

---

### Question 47
Which Azure storage type supports static website hosting?

A. Azure Files
B. Azure Blob Storage
C. Azure Disk Storage
D. Azure Queue Storage

**Answer: B**  
**Explanation:** Azure Blob Storage supports static website hosting, enabling delivery of static content.

---

### Question 48
What is Azure Service Health?

A. Service status
B. Resource health
C. Both A and B
D. Security status

**Answer: C**  
**Explanation:** Service Health provides both Azure service status and individual resource health information.

---

### Question 49
Which Azure service provides a graph database?

A. Azure SQL Database
B. Azure Cosmos DB
C. Azure Database for MySQL
D. Azure Synapse

**Answer: B**  
**Explanation:** Azure Cosmos DB supports graph data models through its Gremlin API.

---

### Question 50
What does Azure Marketplace offer?

A. Free services only
B. Third-party solutions
C. Microsoft solutions only
D. Both B and C

**Answer: B**  
**Explanation:** Azure Marketplace offers certified third-party and Microsoft solutions for deployment.

---

### Question 51
Which Azure feature provides a global DNS-based routing service?

A. Azure Load Balancer
B. Application Gateway
C. Traffic Manager
D. Azure Front Door

**Answer: C**  
**Explanation:** Azure Traffic Manager provides global DNS-based routing across Azure regions.

---

### Question 52
What is the maximum number of resources per resource group?

A. 200
B. 500
C. 800
D. Unlimited

**Answer: D**  
**Explanation:** There is no limit on the number of resources per resource group (soft limit of 800 per deployment).

---

### Question 53
Which Azure service is best for IoT data ingestion?

A. Azure Functions
B. Azure IoT Hub
C. Azure Event Hubs
D. Both B and C

**Answer: D**  
**Explanation:** Both Azure IoT Hub (device management) and Azure Event Hubs (big data ingestion) are used for IoT.

---

### Question 54
What does Azure Log Analytics provide?

A. Cost analysis
B. Log analysis
C. Network analysis
D. Security analysis

**Answer: B**  
**Explanation:** Azure Log Analytics enables querying and analyzing log data from Azure resources.

---

### Question 55
Which Azure service provides a web application firewall?

A. Azure Firewall
B. Application Gateway
C. Azure WAF
D. Both B and C

**Answer: D**  
**Explanation:** Both Azure Application Gateway and Azure Front Door include Web Application Firewall (WAF).

---

### Question 56
What is Azure Batch used for?

A. Web applications
B. Parallel compute workloads
C. Database workloads
D. Network workloads

**Answer: B**  
**Explanation:** Azure Batch provides parallel and high-performance computing (HPC) job scheduling.

---

### Question 57
What does Azure Cloud Shell provide?

A. Storage access
B. Command-line access
C. GUI access
D. Database access

**Answer: B**  
**Explanation:** Azure Cloud Shell provides browser-based command-line (Bash/PowerShell) access to Azure.

---

### Question 58
Which Azure service provides a message queue?

A. Azure Queue Storage
B. Service Bus
C. Both A and B
D. Azure Files

**Answer: C**  
**Explanation:** Both Azure Queue Storage and Service Bus provide asynchronous messaging.

---

### Question 59
What is Azure Event Grid?

A. Event routing service
B. Event storage
C. Event processing
D. Event monitoring

**Answer: A**  
**Explanation:** Azure Event Grid is an event routing service that enables event-driven architectures.

---

### Question 60
What is Azure Durable Functions?

A. Long-running functions
B. Stateful function orchestration
C. Background jobs
D. Timer functions

**Answer: B**  
**Explanation:** Durable Functions extend Azure Functions with stateful orchestration for long-running workflows.

---

## Questions 61-80

### Question 61
Which Azure service provides a managed Redis cache?

A. Azure Cache for Redis
B. Azure Cosmos DB
C. Azure SQL
D. Azure Database for MySQL

**Answer: A**  
**Explanation:** Azure Cache for Redis provides a fully managed in-memory caching service.

---

### Question 62
What is Azure API Management used for?

A. API hosting
B. API management/gateway
C. API security
D. All of the above

**Answer: D**  
**Explanation:** Azure API Management provides a complete solution for API hosting, security, and management.

---

### Question 63
Which Azure service provides AI/ML capabilities?

A. Azure Machine Learning
B. Azure Cognitive Services
C. Both A and B
D. Azure Functions

**Answer: C**  
**Explanation:** Both Azure Machine Learning (custom ML) and Cognitive Services (pre-built AI) are available.

---

### Question 64
What does Azure Monitor include?

A. Metrics only
B. Logs only
C. Both Metrics and Logs
D. Alerts only

**Answer: C**  
**Explanation:** Azure Monitor provides both metrics and logs for monitoring Azure resources.

---

### Question 65
Which Azure feature enables tag-based organization?

A. Resource groups
B. Tags
C. Subscriptions
D. Management groups

**Answer: B**  
**Explanation:** Tags enable logical organization of resources across resource groups.

---

### Question 66
What is an Azure Management Group?

A. Resource grouping
B. Subscription organization
C. Resource management
D. User management

**Answer: B**  
**Explanation:** Management Groups provide organizational control over subscriptions.

---

### Question 67
Which Azure VM size is memory-optimized?

A. D series
B. E series
C. M series
D. F series

**Answer: C**  
**Explanation:** M-series VMs are memory-optimized for large in-memory workloads.

---

### Question 68
What does Azure Shared Access Signature (SAS) provide?

A. User authentication
B. Temporary resource access
C. Single sign-on
D. Role-based access

**Answer: B**  
**Explanation:** SAS provides temporary, restricted access to Azure storage resources.

---

### Question 69
Which Azure service provides a managed Kubernetes experience?

A. Azure Kubernetes Service (AKS)
B. Azure Container Instances
C. Azure Container Apps
D. Both A and C

**Answer: D**  
**Explanation:** Both AKS and Container Apps provide managed Kubernetes experiences.

---

### Question 70
What is Azure Static Web Apps?

A. Web hosting
B. Static content hosting
C. CDN
D. None

**Answer: B**  
**Explanation:** Azure Static Web Apps provides static content hosting with GitHub integration.

---

### Question 71
Which Azure service provides ETL capabilities?

A. Azure Data Factory
B. Azure Synapse
C. Both A and B
D. Azure Functions

**Answer: C**  
**Explanation:** Both Azure Data Factory and Azure Synapse provide ETL capabilities.

---

### Question 72
What is Azure Time Series Insights?

A. Time series analytics
B. IoT analytics
C. Both A and B
D. Database

**Answer: C**  
**Explanation:** Time Series Insights provides analytics for time series and IoT data.

---

### Question 73
Which Azure service provides real-time analytics?

A. Azure Stream Analytics
B. Azure Event Hubs
C. Both A and B
D. Azure Functions

**Answer: C**  
**Explanation:** Stream Analytics provides real-time analytics on Event Hubs streaming data.

---

### Question 74
What is Azure Arc-enabled Kubernetes?

A. Azure Kubernetes
B. Hybrid Kubernetes
C. Multi-cloud Kubernetes
D. Both B and C

**Answer: D**  
**Explanation:** Azure Arc enables Kubernetes in multi-cloud and hybrid environments.

---

### Question 75
Which Azure service provides a content delivery network?

A. Azure CDN
B. Azure Front Door
C. Both A and B
D. Azure Cache

**Answer: C**  
**Explanation:** Both Azure CDN and Azure Front Door provide CDN capabilities.

---

### Question 76
What does Azure Service Bus support?

A. Queues only
B. Topics only
C. Both Queues and Topics
D. Events

**Answer: C**  
**Explanation:** Service Bus supports both Queues (point-to-point) and Topics (pub/sub).

---

### Question 77
Which Azure service provides serverless SQL?

A. Azure SQL Database
B. Azure SQL Database serverless
C. Azure Synapse
D. Azure Database for PostgreSQL

**Answer: B**  
**Explanation:** Azure SQL Database serverless provides auto-scaling compute for single databases.

---

### Question 78
What is Azure Durable Entities?

A. Serverless state
B. Durable Functions
C. Both A and B
D. State storage

**Answer: C**  
**Explanation:** Durable Entities provide serverless, durable state management with Durable Functions.

---

### Question 79
What is Azure Confidential Computing?

A. Encrypted compute
B. Confidential workloads
C. Both A and B
D. Encryption keys

**Answer: C**  
**Explanation:** Confidential Computing provides encrypted data during processing using enclaves.

---

### Question 80
What does Azure Purview provide?

A. Data governance
B. Data catalog
C. Both A and B
D. Data storage

**Answer: C**  
**Explanation:** Microsoft Purview provides unified data governance and catalog capabilities.

---

## Questions 81-100

### Question 81
Which Azure service is used for virtual desktop infrastructure?

A. Azure Virtual Desktop (AVD)
B. Windows 365
C. Both A and B
D. Azure VMs

**Answer: C**  
**Explanation:** Both Azure Virtual Desktop and Windows 365 provide virtual desktop solutions.

---

### Question 82
What is Azure Orbital?

A. Ground station
B. Satellite data
C. Both A and B
D. None

**Answer: C**  
**Explanation:** Azure Orbital provides ground station services for satellite communications and data.

---

### Question 83
Which Azure service provides a managed Power Platform?

A. Power Apps
B. Power Automate
C. Both with Dataverse
D. None

**Answer: C**  
**Explanation:** Microsoft Power Platform with Dataverse provides a complete low-code managed solution.

---

### Question 84
What does Azure Communication Services provide?

A. Communication APIs
B. Messaging
C. Both A and B
D. None

**Answer: C**  
**Explanation:** Azure Communication Services provides communication APIs for voice, video, and messaging.

---

### Question 85
Which Azure service is for digital twins?

A. Azure Digital Twins
B. Azure IoT
C. Both A and B
D. Azure Functions

**Answer: A**  
**Explanation:** Azure Digital Twins creates digital twin models of physical environments.

---

### Question 86
What does Azure Percept provide?

A. Edge AI
B. IoT devices
C. Both A and B
D. None

**Answer: C**  
**Explanation:** Azure Percept provides edge AI and IoT devices for intelligent edge solutions.

---

### Question 87
Which Azure service provides a low-code integration?

A. Azure Logic Apps
B. Azure Functions
C. Both A and B
D. Power Automate

**Answer: A**  
**Explanation:** Azure Logic Apps provides enterprise integration with a low-code visual designer.

---

### Question 88
What is Azure Spatial Anchors?

A. Mixed reality
B. Location services
C. Both A and B
D. GPS

**Answer: C**  
**Explanation:** Azure Spatial Anchors enables mixed reality experiences with spatial awareness.

---

### Question 89
Which Azure service provides data virtualization?

A. Azure Synapse
B. Azure Data Lake
C. Both A and B
D. Azure SQL

**Answer: C**  
**Explanation:** Both Azure Synapse and Data Lake enable data virtualization without data movement.

---

### Question 90
What does Azure Chaos Studio provide?

A. Chaos engineering
B. Testing resilience
C. Both A and B
D. Monitoring

**Answer: C**  
**Explanation:** Azure Chaos Studio enables chaos engineering to test application resilience.

---

### Question 91
Which Azure service is for healthcare data?

A. Azure API for FHIR
B. Azure Health Data Services
C. Both A and B
D. None

**Answer: C**  
**Explanation:** Azure provides healthcare-specific services for FHIR and health data management.

---

### Question 92
What is Azure Maps used for?

A. Location services
B. Mapping
C. Both A and B
D. GIS

**Answer: C**  
**Explanation:** Azure Maps provides location services, mapping, and geospatial APIs.

---

### Question 93
Which Azure service connects operational data?

A. Azure IoT Edge
B. Azure Edge Zone
C. Both A and B
D. Azure VMs

**Answer: C**  
**Explanation:** Azure IoT Edge and Edge Zone extend Azure services to the edge for operational data.

---

### Question 94
What does Azure VMware Solution provide?

A. VMware on Azure
B. Hybrid
C. Both A and B
D. Migration

**Answer: C**  
**Explanation:** Azure VMware Solution allows running VMware workloads on Azure infrastructure.

---

### Question 95
Which Azure service is for open-source databases?

A. Azure Database for MySQL/PostgreSQL
B. Azure Cosmos DB
C. Both A and B
D. Azure SQL

**Answer: C**  
**Explanation:** Azure supports MySQL, PostgreSQL, MongoDB (Cosmos DB), and other open-source databases.

---

### Question 96
What is Azure Red Hat OpenShift?

A. Managed OpenShift
B. Joint solution
C. Both A and B
D. None

**Answer: C**  
**Explanation:** Azure Red Hat OpenShift provides a fully managed OpenShift service.

---

### Question 97
Which Azure service provides an AI-powered search?

A. Azure Cognitive Search
B. Azure Search
C. Both A and B
D. Azure Bot

**Answer: C**  
**Explanation:** Azure Cognitive Search (formerly Azure Search) provides AI-powered search.

---

### Question 98
What does Azure Video Indexer do?

A. Analyze videos
B. Extract insights
C. Both A and B
D. Edit videos

**Answer: C**  
**Explanation:** Video Indexer analyzes videos and extracts audio/video insights using AI.

---

### Question 99
Which Azure service provides bot development?

A. Azure Bot Service
B. Bot Framework
C. Both A and B
D. Azure Functions

**Answer: C**  
**Explanation:** Azure Bot Service and Bot Framework provide comprehensive bot development.

---

### Question 100
What is Azure Spring Apps?

A. Spring Cloud service
B. Managed Spring Boot
C. Both A and B
D. None

**Answer: C**  
**Explanation:** Azure Spring Apps provides a fully managed Spring Cloud service for Spring Boot applications.

---

**End of Practice Questions**

**Good luck with your exam preparation!**