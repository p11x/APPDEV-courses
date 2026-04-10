---
Category: Certification Prep
Subcategory: AWS CLF-C02
Concept: Practice Questions
Purpose: 100 practice questions with answers and explanations for AWS Cloud Practitioner exam
Difficulty: intermediate
Prerequisites: Study of AWS Cloud Practitioner concepts
RelatedFiles: 01_Exam_Guide.md
UseCase: Exam preparation and self-assessment
CertificationExam: AWS Certified Cloud Practitioner (CLF-C02)
LastUpdated: 2025
---

# AWS Cloud Practitioner Practice Questions (100 Questions)

## Domain 1: Cloud Concepts (Questions 1-26)

**Question 1**: What is the primary benefit of cloud computing's "pay-as-you-go" pricing model?

A) No upfront costs
B) Pay only for what you use
C) Free resources always available
D) Unlimited resources

**Answer**: B) Pay only for what you use

**Explanation**: The pay-as-you-go model allows you to pay only for the compute, storage, and other resources you consume, eliminating large upfront capital expenses and only paying for what you actually use.

---

**Question 2**: Which AWS characteristic allows you to deploy applications in multiple geographic regions worldwide?

A) Edge locations
B) Availability Zones
C) Local Zones
D) AWS Regions

**Answer**: D) AWS Regions

**Explanation**: AWS has multiple Regions globally that allow you to deploy applications close to your users in different geographic areas for lower latency and better user experience.

---

**Question 3**: What does "elasticity" mean in cloud computing?

A) Ability to stretch like a rubber band
B) Automatically scale resources up and down based on demand
C) Using the smallest possible resources
D) Manual resource allocation

**Answer**: B) Automatically scale resources up and down based on demand

**Explanation**: Elasticity is the ability to automatically provision and de-provision compute resources to match demand, optimizing costs by adding resources during peaks and removing them during low usage.

---

**Question 4**: A company wants to migrate their existing applications to AWS with minimal changes. Which migration strategy should they use?

A) Rehosting (lift and shift)
B) Refactoring (re-architecting)
C) Repurchasing (SaaS)
D) Retiring

**Answer**: A) Rehosting (lift and shift)

**Explanation**: Rehosting involves moving applications to the cloud without making any code changes. It's the fastest migration strategy with the least risk, perfect for companies that want minimal changes.

---

**Question 5**: What is the main advantage of using a public cloud over a private cloud?

A) More control
B) Lower cost for variable workloads
C) Dedicated hardware
D) Higher security

**Answer**: B) Lower cost for variable workloads

**Explanation**: Public cloud offers lower costs for variable workloads because you only pay for what you use, avoiding the need to maintain and pay for dedicated infrastructure that might be underutilized.

---

**Question 6**: How many Availability Zones should you use for high availability architecture?

A) 1
B) 2
C) 3
D) 4

**Answer**: B) 2 (minimum for HA) or C) 3 (for highest availability)

**Explanation**: For high availability, use at least 2 Availability Zones. For the highest availability (99.99%), deploy across 3 or more AZs within a region.

---

**Question 7**: Which cloud deployment model provides dedicated infrastructure for a single organization?

A) Public Cloud
B) Private Cloud
C) Hybrid Cloud
D) Community Cloud

**Answer**: B) Private Cloud

**Explanation**: Private cloud provides dedicated infrastructure for a single organization, either hosted on-premises or in a dedicated data center, offering more control and customization.

---

**Question 8**: What does "multi-tenancy" mean in cloud computing?

A) Multiple applications running on one server
B) Multiple customers sharing the same infrastructure while remaining logically isolated
C) One customer using multiple cloud providers
D) Multiple data centers

**Answer**: B) Multiple customers sharing the same infrastructure while remaining logically isolated

**Explanation**: Multi-tenancy is when multiple customers (tenants) share common infrastructure (servers, storage, network) but their data and configurations remain logically isolated from each other.

---

**Question 9**: Which is NOT one of the six advantages of cloud computing according to AWS?

A) Trade capital expense for variable expense
B) Benefit from massive economies of scale
C) Deploy faster in months
D) Go global in minutes

**Answer**: C) Deploy faster in months

**Explanation**: The correct statement is "Go global in minutes," not "Deploy faster in months." The six advantages include: trade fixed for variable expense, benefit from massive economies of scale, stop guessing capacity, increase speed and agility, stop spending on data centers, and go global in minutes.

---

**Question 10**: What is the purpose of an edge location in AWS?

A) Primary data center
B) Content delivery closer to users
C) Backup storage location
D) Database location

**Answer**: B) Content delivery closer to users

**Explanation**: Edge locations are points of presence (PoP) used by CloudFront for content delivery and Lambda@Edge for running code closer to users, reducing latency and improving performance.

---

**Question 11**: What is the minimum number of AWS Regions needed for a disaster recovery architecture?

A) 1
B) 2
C) 3
D) 4

**Answer**: B) 2

**Explanation**: For disaster recovery, you need at least 2 Regions - a primary region and a secondary (DR) region to which you can failover in case of a regional outage.

---

**Question 12**: Which term describes computing resources that can be provisioned and released automatically without human intervention?

A) On-demand resources
B) Manual resources
C) Reserved resources
D) Dedicated resources

**Answer**: A) On-demand resources

**Explanation**: On-demand resources are computing resources that are automatically provisioned when needed and released when no longer required, without human intervention.

---

**Question 13**: What is the primary benefit of "scalability" in cloud computing?

A) Running on larger servers
B) Adding or removing resources to meet demand without changing architecture
C) Reducing costs
D) Increasing security

**Answer**: B) Adding or removing resources to meet demand without changing architecture

**Explanation**: Scalability is the ability to increase or decrease compute resources (either vertically or horizontally) to handle varying workload demands while maintaining performance.

---

**Question 14**: A startup needs to quickly deploy a minimum viable product (MVP). Which cloud benefit is most important?

A) Massive economies of scale
B) Go global in minutes
C) Increase speed and agility
D) Stop spending on data centers

**Answer**: C) Increase speed and agility

**Explanation**: For quickly deploying an MVP, the ability to provision resources in minutes rather than weeks allows fast iteration and testing of ideas.

---

**Question 15**: Which deployment model combines public cloud with private infrastructure?

A) Public Cloud
B) Private Cloud
C) Hybrid Cloud
D) Multi-Cloud

**Answer**: C) Hybrid Cloud

**Explanation**: Hybrid cloud combines public cloud services with private cloud (or on-premises infrastructure), allowing data and applications to move between environments.

---

**Question 16**: What is the RTO in disaster recovery?

A) Recovery Time Objective - Maximum acceptable time to restore service
B) Recovery Point Objective - Maximum acceptable data loss
C) Regional Technology Operation
D) Redundant Technical Operations

**Answer**: A) Recovery Time Objective - Maximum acceptable time to restore service

**Explanation**: RTO (Recovery Time Objective) defines the maximum acceptable time to restore service after a disaster. RPO (Recovery Point Objective) defines maximum acceptable data loss measured in time.

---

**Question 17**: What type of cloud service would a government agency with strict data residency requirements most likely use?

A) Public Cloud
B) Private Cloud
C) Hybrid Cloud
D) Community Cloud

**Answer**: B) Private Cloud (or dedicated regions in Public Cloud)

**Explanation**: Government agencies with strict data sovereignty requirements often use private cloud or dedicated/isolated regions within public cloud to ensure data never leaves specific geographic boundaries.

---

**Question 18**: What does "serverless" mean in cloud computing?

A) No servers exist
B) You don't manage servers; cloud provider handles infrastructure
C) Servers are free
D) Only one server used

**Answer**: B) You don't manage servers; cloud provider handles infrastructure

**Explanation**: Serverless computing means you don't need to provision or manage servers. The cloud provider automatically handles the infrastructure, and you only pay for the execution time of your code.

---

**Question 19**: A company has predictable, steady-state workloads. Which pricing model would provide the BEST cost savings?

A) On-Demand Instances
B) Reserved Instances
C) Spot Instances
D) Dedicated Hosts

**Answer**: B) Reserved Instances

**Explanation**: Reserved Instances offer up to 72% discount for steady-state, predictable workloads where you know you'll need the resources for 1-3 years.

---

**Question 20**: What is the main difference between scalability and elasticity?

A) They are the same thing
B) Scalability is manual, elasticity is automatic
C) Scalability is adding capacity, elasticity is automatic scaling
D) Elasticity is only for compute

**Answer**: C) Scalability is adding capacity, elasticity is automatic scaling

**Explanation**: Scalability is the ability to increase capacity (can be manual or automatic). Elasticity specifically refers to automatic scaling up and down based on demand in real-time.

---

**Question 21**: Which AWS service enables global content delivery with low latency?

A) Route 53
B) CloudFront
C) ELB
D) S3

**Answer**: B) CloudFront

**Explanation**: CloudFront is AWS's content delivery network (CDN) that caches content at edge locations worldwide for low-latency delivery to users.

---

**Question 22**: What is the purpose of a data center's "Availability Zone"?

A) Physical location of all AWS services
B) Isolated data center within a region with independent power and networking
C) Geographic region for data residency
D) Backup location for data

**Answer**: B) Isolated data center within a region with independent power and networking

**Explanation**: An Availability Zone (AZ) is a physically isolated data center within a region, with its own power, cooling, and networking, connected to other AZs with low-latency networking.

---

**Question 23**: What is the "cloud-first" principle?

A) Use only cloud providers based in your country
B) Prioritize cloud solutions over traditional infrastructure when possible
C) All applications must be in the cloud immediately
D) Use only AWS cloud services

**Answer**: B) Prioritize cloud solutions over traditional infrastructure when possible

**Explanation**: Cloud-first means defaulting to cloud-based solutions for new workloads and modernization efforts, taking advantage of cloud benefits like scalability, managed services, and cost optimization.

---

**Question 24**: Which migration approach involves rewriting applications to use cloud-native features?

A) Rehosting
B) Replatforming
C) Refactoring
D) Retiring

**Answer**: C) Refactoring

**Explanation**: Refactoring (or re-architecting) involves rewriting or restructuring applications to take full advantage of cloud-native features and services.

---

**Question 25**: What is the primary benefit of "global infrastructure" in cloud computing?

A) Cheaper costs everywhere
B) Lower latency for international users
C) More security
D) Unlimited storage

**Answer**: B) Lower latency for international users

**Explanation**: Global infrastructure (multiple regions and edge locations) allows you to deploy applications closer to users worldwide, reducing latency and improving user experience.

---

**Question 26**: What does the term "durability" mean in cloud storage?

A) How fast data can be accessed
B) Data never being lost - 99.999999999% probability of preservation
C) How long storage lasts
D) Cost of storage

**Answer**: B) Data never being lost - 99.999999999% probability of preservation

**Explanation**: Durability refers to the probability that data will not be lost. AWS S3 guarantees 99.999999999% (eleven 9's) durability, meaning data is virtually guaranteed to be preserved.

---

## Domain 2: Security and Architecture (Questions 27-52)

**Question 27**: In the AWS Shared Responsibility Model, who is responsible for patching the guest operating system on EC2?

A) AWS
B) Customer
C) Both AWS and Customer
D) Third-party vendor

**Answer**: B) Customer

**Explanation**: For EC2, AWS manages the underlying host infrastructure and hypervisor, but customers are responsible for patching their guest OS and applications.

---

**Question 28**: Which type of security group rule allows return traffic automatically?

A) Inbound rules only
B) Stateful
C) Stateless
D) Network ACL

**Answer**: B) Stateful

**Explanation**: Security groups are stateful, meaning if you allow inbound traffic, the outbound response is automatically allowed without explicit outbound rules.

---

**Question 29**: What does the AWS Shared Responsibility Model define?

A) Only AWS is responsible for security
B) Only customer is responsible for security
C) AWS secures infrastructure; customer secures their data and configurations
D) Security is shared equally

**Answer**: C) AWS secures infrastructure; customer secures their data and configurations

**Explanation**: The Shared Responsibility Model divides security responsibilities: AWS manages security OF the cloud (infrastructure), while customers manage security IN the cloud (data, access, applications).

---

**Question 30**: Which AWS service provides centralized logging of API calls?

A) CloudWatch
B) CloudTrail
C) Config
D) GuardDuty

**Answer**: B) CloudTrail

**Explanation**: CloudTrail records API calls made in your AWS account, providing audit logs of who did what, when, and from where.

---

**Question 31**: What is the principle of "least privilege"?

A) Give users the maximum permissions they might need
B) Give users only the minimum permissions they need to do their job
C) Give all users the same permissions
D) Remove all permissions

**Answer**: B) Give users only the minimum permissions they need to do their job

**Explanation**: Least privilege means granting only the permissions required to perform a specific task, reducing the risk of accidental or malicious actions.

---

**Question 32**: Which AWS security service provides DDoS protection?

A) WAF
B) Shield
C) GuardDuty
D) Inspector

**Answer**: B) Shield

**Explanation**: AWS Shield provides DDoS protection. Shield Standard is free and automatically enabled for all AWS customers. Shield Advanced provides additional protection and WAF integration.

---

**Question 33**: What does "encryption at rest" mean?

A) Data encrypted during transmission
B) Data encrypted when stored on disk
C) Data encrypted only in memory
D) Data not encrypted

**Answer**: B) Data encrypted when stored on disk

**Explanation**: Encryption at rest protects data that's stored on disks, databases, or any persistent storage, ensuring it's unreadable without the encryption key.

---

**Question 34**: Which IAM entity is used to grant permissions to an application running on EC2?

A) IAM User
B) IAM Group
C) IAM Role
D) IAM Policy

**Answer**: C) IAM Role

**Explanation**: IAM Roles are used to grant temporary permissions to applications, services, or users. For EC2, you attach an IAM role to the instance profile to grant permissions to applications running on that instance.

---

**Question 35**: What is a security group's default behavior for inbound traffic?

A) Allow all
B) Deny all
C) Allow HTTP/HTTPS only
D) Allow within VPC

**Answer**: B) Deny all

**Explanation**: Security groups default to denying all inbound traffic. You must explicitly add rules to allow traffic.

---

**Question 36**: Which AWS service identifies security threats using machine learning?

A) WAF
B) Shield
C) GuardDuty
D) Macie

**Answer**: C) GuardDuty

**Explanation**: GuardDuty is a threat detection service that uses machine learning, anomaly detection, and integrated threat intelligence to identify potential security threats.

---

**Question 37**: In the shared responsibility model, who is responsible for data classification?

A) AWS
B) Customer
C) Both equally
D) Regulatory body

**Answer**: B) Customer

**Explanation**: Customers are responsible for classifying their data and determining appropriate security controls based on data sensitivity.

---

**Question 38**: What is the purpose of a Multi-Factor Authentication (MFA)?

A) Make login faster
B) Add additional layer of security beyond password
C) Track user activity
D) Store passwords securely

**Answer**: B) Add additional layer of security beyond password

**Explanation**: MFA requires two or more verification methods (something you know, have, or are), significantly reducing the risk of unauthorized access even if passwords are compromised.

---

**Question 39**: Which Well-Architected Framework pillar focuses on recovering from failures?

A) Operational Excellence
B) Security
C) Reliability
D) Cost Optimization

**Answer**: C) Reliability

**Explanation**: The Reliability pillar focuses on the ability to recover from infrastructure or service disruptions, dynamically acquire computing resources, and mitigate disruptions.

---

**Question 40**: What does AWS KMS manage?

A) Network keys
B) Encryption keys
C) Passwords only
D) SSL certificates

**Answer**: B) Encryption keys

**Explanation**: AWS Key Management Service (KMS) creates and manages encryption keys for data encryption across AWS services and applications.

---

**Question 41**: Which type of policy defines permissions for an IAM user?

A) Resource-based policy
B) Identity-based policy
C) Service control policy
D) Bucket policy

**Answer**: B) Identity-based policy

**Explanation**: Identity-based policies attach to IAM users, groups, or roles and define what actions those identities can perform on which resources.

---

**Question 42**: What is the primary purpose of AWS WAF?

A) DDoS protection
B) Web application firewall - blocks malicious web traffic
C) Database security
D) Email security

**Answer**: B) Web application firewall - blocks malicious web traffic

**Explanation**: AWS WAF is a web application firewall service that lets you monitor web requests and protect your web applications from malicious requests.

---

**Question 43**: Which AWS service discovers sensitive data like PII in S3?

A) GuardDuty
B) Macie
C) Inspector
D) Config

**Answer**: B) Macie

**Explanation**: Amazon Macie uses machine learning to automatically discover, classify, and protect sensitive data like PII, intellectual property, etc., in S3.

---

**Question 44**: What is the maximum password length for IAM users?

A) 8 characters
B) 16 characters
C) 128 characters
D) Unlimited

**Answer**: C) 128 characters

**Explanation**: IAM user passwords can be up to 128 characters long.

---

**Question 45**: Which architecture principle focuses on assuming that components can fail?

A) Design for success
B) Design for failure
C) Design for speed
D) Design for cost

**Answer**: B) Design for failure

**Explanation**: Well-Architected Framework's Reliability pillar emphasizes designing for failure by implementing redundancy, automated recovery, and graceful degradation.

---

**Question 46**: What does SCP stand for in AWS Organizations?

A) Standard Cloud Policy
B) Service Control Policy
C) Security Compliance Program
D) Server Configuration Protocol

**Answer**: B) Service Control Policy

**Explanation**: Service Control Policies (SCPs) are organization-wide policies that restrict permissions for accounts, regions, or services.

---

**Question 47**: Which encryption type requires you to manage your own keys?

A) SSE-S3
B) SSE-KMS
C) SSE-C
D) All of the above

**Answer**: C) SSE-C (Server-Side Encryption with Customer-Provided Keys)

**Explanation**: With SSE-C, you provide and manage your own encryption keys. AWS uses your keys to encrypt/decrypt data but doesn't store them.

---

**Question 48**: What is the purpose of a VPC's Internet Gateway?

A) Connect to other VPCs
B) Allow instances to access the internet
C) Connect to on-premises networks
D) Load balance traffic

**Answer**: B) Allow instances to access the internet

**Explanation**: An Internet Gateway (IGW) enables communication between instances in a VPC and the internet, allowing outbound and inbound internet traffic.

---

**Question 49**: Which AWS service provides automated security assessments?

A) GuardDuty
B) Inspector
C) Config
D) Macie

**Answer**: B) Inspector

**Explanation**: Amazon Inspector is an automated security assessment service that helps identify security vulnerabilities and deviations in AWS resources.

---

**Question 50**: What type of firewall operates at the subnet level in a VPC?

A) Security Groups
B) Network ACLs (NACLs)
C) WAF
D) Shield

**Answer**: B) Network ACLs (NACLs)

**Explanation**: Network ACLs (NACLs) are stateless firewall controls that operate at the subnet level, filtering traffic entering and leaving subnets.

---

**Question 51**: What does MFA on the root account protect against?

A) Server failures
B) Unauthorized access to the account
C) Data loss
D) Network attacks

**Answer**: B) Unauthorized access to the account

**Explanation**: MFA on the root account provides an extra layer of protection against unauthorized access to the account, even if the root password is compromised.

---

**Question 52**: In the Well-Architected Framework, which pillar ensures you can monitor and respond to changes?

A) Operational Excellence
B) Security
C) Reliability
D) Performance Efficiency

**Answer**: A) Operational Excellence

**Explanation**: The Operational Excellence pillar focuses on operating and monitoring systems to deliver business value, including responding to changes and improving processes.

---

## Domain 3: Technology (Questions 53-80)

**Question 53**: Which EC2 instance type is optimized for compute-intensive workloads?

A) T3
B) M5
C) C5
D) R5

**Answer**: C) C5

**Explanation**: C5 instances are Compute Optimized, designed for compute-bound applications that benefit from high-performance processors (e.g., batch processing, HPC, gaming).

---

**Question 54**: What is the maximum object size in S3?

A) 5 GB
B) 5 TB
C) 1 PB
D) Unlimited

**Answer**: B) 5 TB

**Explanation**: Amazon S3 supports individual objects up to 5 terabytes (TB) in size.

---

**Question 55**: Which RDS feature creates a standby database in another Availability Zone for automatic failover?

A) Read Replica
B) Multi-AZ
C) Backup
D) Encryption

**Answer**: B) Multi-AZ

**Explanation**: Multi-AZ deployment automatically provisions and maintains a synchronous standby replica in a different Availability Zone for automatic failover in case of an AZ failure.

---

**Question 56**: What is the maximum execution timeout for an AWS Lambda function?

A) 5 minutes
B) 15 minutes
C) 1 hour
D) 12 hours

**Answer**: B) 15 minutes

**Explanation**: AWS Lambda functions have a maximum execution timeout of 15 minutes (900 seconds).

---

**Question 57**: Which S3 storage class offers the lowest cost for archiving data that is accessed once per year?

A) S3 Standard
B) S3 IA
C) S3 Glacier
D) S3 Glacier Deep Archive

**Answer**: D) S3 Glacier Deep Archive

**Explanation**: S3 Glacier Deep Archive is the lowest-cost option, designed for long-term archival of data that may be accessed once or twice per year, with retrieval times of 12-48 hours.

---

**Question 58**: What does the "stateless" characteristic mean for Security Groups?

A) Remembers previous connections
B) Does not remember previous connections
C) Tracks all connections
D) Blocks all traffic

**Answer**: B) Does not remember previous connections

**Explanation**: Security groups are stateful - they remember connections. Network ACLs (NACLs) are stateless - they don't remember previous connections and evaluate each packet independently.

---

**Question 59**: Which AWS service is a fully managed NoSQL database?

A) RDS
B) DynamoDB
C) ElastiCache
D) Neptune

**Answer**: B) DynamoDB

**Explanation**: Amazon DynamoDB is a fully managed NoSQL database service that provides fast and predictable performance with seamless scaling.

---

**Question 60**: What is the primary function of an Application Load Balancer?

A) Distribute traffic across instances in multiple AZs
B) Route based on path or host
C) Both A and B
D) None of the above

**Answer**: C) Both A and B

**Explanation**: Application Load Balancer (ALB) routes traffic to targets based on path, host, or HTTP headers, and distributes traffic across instances in multiple AZs.

---

**Question 61**: Which AWS database service is best for highly available, globally distributed applications?

A) RDS
B) DynamoDB
C) Aurora Global Database
D) ElastiCache

**Answer**: C) Aurora Global Database

**Explanation**: Amazon Aurora Global Database allows you to span a database across multiple AWS regions, providing low-latency global reads and disaster recovery.

---

**Question 62**: What is the purpose of a VPC's NAT Gateway?

A) Allow inbound internet traffic
B) Allow outbound internet traffic from private instances while blocking inbound
C) Connect VPCs together
D) Load balance traffic

**Answer**: B) Allow outbound internet traffic from private instances while blocking inbound

**Explanation**: A NAT (Network Address Translation) Gateway allows instances in private subnets to access the internet for updates/patches while preventing inbound internet traffic from reaching those instances.

---

**Question 63**: Which AWS service provides serverless container orchestration?

A) ECS
B) EKS
C) Fargate
D) Lightsail

**Answer**: C) Fargate

**Explanation**: AWS Fargate is a serverless compute engine for containers that works with both ECS and EKS, eliminating the need to manage servers.

---

**Question 64**: What is the default port for HTTPS?

A) 80
B) 443
C) 22
D) 3306

**Answer**: B) 443

**Explanation**: HTTPS (HTTP Secure) uses port 443 by default. HTTP uses port 80, SSH uses port 22, and MySQL uses port 3306.

---

**Question 65**: Which S3 feature enables automatic transition of objects to lower-cost storage classes?

A) Replication
B) Lifecycle Policy
C) Versioning
D) Encryption

**Answer**: B) Lifecycle Policy

**Explanation**: S3 Lifecycle policies automatically transition objects between storage classes based on age or access patterns, optimizing storage costs.

---

**Question 66**: What is the maximum storage size for a single EBS volume?

A) 1 TB
B) 16 TB
C) 64 TB
D) Unlimited

**Answer**: B) 16 TB

**Explanation**: The maximum size for a single EBS volume is 16 TiB (tebibytes).

---

**Question 67**: Which AWS service would you use to host a static website at low cost?

A) EC2
B) S3
C) Lambda
D) RDS

**Answer**: B) S3

**Explanation**: Amazon S3 can host static websites at very low cost (only for storage and requests), making it ideal for static content.

---

**Question 68**: What does a Route 53 "weighted" routing policy do?

A) Routes to the lowest latency region
B) Routes based on geographic location
C) Distributes traffic across multiple resources based on weight
D) Routes to healthy endpoints only

**Answer**: C) Distributes traffic across multiple resources based on weight

**Explanation**: Weighted routing lets you associate weights with resource record sets to route traffic proportionally to different resources.

---

**Question 69**: Which DynamoDB feature provides automatic data replication across multiple regions?

A) Streams
B) Global Tables
C) DAX
D) TTL

**Answer**: B) Global Tables

**Explanation**: DynamoDB Global Tables provide fully managed, multi-region, multi-active replication, allowing you to replicate tables across multiple AWS regions.

---

**Question 70**: What is the purpose of an Amazon VPC's route table?

A) Define which resources are in the VPC
B) Determine where network traffic is directed
C) Create subnets
D) Attach to the internet

**Answer**: B) Determine where network traffic is directed

**Explanation**: Route tables contain rules (routes) that determine where network traffic from your subnets is directed.

---

**Question 71**: Which AWS service provides a content delivery network (CDN)?

A) Route 53
B) CloudFront
C) API Gateway
D) ELB

**Answer**: B) CloudFront

**Explanation**: Amazon CloudFront is AWS's content delivery network (CDN) that delivers content with low latency using a global network of edge locations.

---

**Question 72**: What is the minimum guaranteed Lambda concurrency?

A) 0 (scales from zero)
B) 10
C) 100
D) 1000

**Answer**: A) 0 (scales from zero)

**Explanation**: Lambda scales from zero to handle incoming requests, but you can also configure "Provisioned Concurrency" to keep functions initialized and ready.

---

**Question 73**: Which type of RDS instance is best for a read-heavy workload?

A) Primary instance
B) Read Replica
C) Standby instance
D) Single-AZ instance

**Answer**: B) Read Replica

**Explanation**: Read Replicas create read-only copies of your database that can handle read queries, offloading the primary database for better read performance.

---

**Question 74**: What does S3 versioning preserve?

A) Only the latest version
B) Every version of every object
C) Only deleted objects
D) Nothing by default

**Answer**: B) Every version of every object

**Explanation**: S3 versioning preserves every version of every object, allowing you to restore previous versions or retrieve deleted objects.

---

**Question 75**: Which AWS service allows you to run code without provisioning servers?

A) EC2
B) Lambda
C) ECS
D) Batch

**Answer**: B) Lambda

**Explanation**: AWS Lambda is a serverless compute service that lets you run code without provisioning or managing servers, responding to events and scaling automatically.

---

**Question 76**: What is the purpose of a CloudFront origin?

A) The edge location that serves content
B) The source of the content to be distributed
C) The user viewing content
D) The route53 record

**Answer**: B) The source of the content to be distributed

**Explanation**: In CloudFront, an origin is the source of the content that CloudFront distributes - it can be an S3 bucket, EC2 instance, or custom HTTP server.

---

**Question 77**: Which database service is best for graph data models?

A) RDS
B) DynamoDB
C) Neptune
D) Redshift

**Answer**: C) Neptune

**Answer**: Explanation: Amazon Neptune is a fast, reliable, fully managed graph database service that makes it easy to build and run applications that work with highly connected datasets.

---

**Question 78**: What does an Elastic IP address provide?

A) Random public IP on each restart
B) Persistent public IP address for an instance
C) Private IP only
D) Free IP address

**Answer**: B) Persistent public IP address for an instance

**Explanation**: An Elastic IP (EIP) address is a static, public IPv4 address that you can associate with an instance and keeps it even if you stop and start the instance.

---

**Question 79**: Which AWS service enables building RESTful APIs easily?

A) EC2
B) API Gateway
C) S3
D) CloudFormation

**Answer**: B) API Gateway

**Explanation**: Amazon API Gateway is a fully managed service for creating, publishing, and managing RESTful and WebSocket APIs at any scale.

---

**Question 80**: What is the primary use case for ElastiCache?

A) Storing large files
B) Caching frequently accessed data in memory
C) Running databases
D) File storage

**Answer**: B) Caching frequently accessed data in memory

**Explanation**: Amazon ElastiCache is a fully managed in-memory caching service (supporting Redis and Memcached) that speeds up application performance by caching frequently accessed data.

---

## Domain 4: Billing and Pricing (Questions 81-100)

**Question 81**: Which pricing model offers the deepest discount but instances can be interrupted?

A) On-Demand
B) Reserved Instances
C) Savings Plans
D) Spot Instances

**Answer**: D) Spot Instances

**Explanation**: Spot Instances offer up to 90% discount but can be interrupted by AWS when capacity is needed back. They're ideal for fault-tolerant, flexible workloads.

---

**Question 82**: What is the minimum commitment term for a Reserved Instance?

A) 6 months
B) 1 year
C) 3 years
D) No commitment

**Answer**: B) 1 year

**Explanation**: Reserved Instances require a 1-year or 3-year commitment. There are also 1-year and 3-year Savings Plans.

---

**Question 83**: What does the AWS Free Tier include for new customers?

A) Unlimited usage for 12 months
B) Limited usage of selected services for 12 months
C) $1000 credit
D) Everything free forever

**Answer**: B) Limited usage of selected services for 12 months

**Explanation**: AWS Free Tier provides limited free usage for many services (some permanent, some for 12 months after sign-up), but not unlimited or everything free.

---

**Question 84**: Which AWS Support plan includes a dedicated Technical Account Manager?

A) Basic
B) Developer
C) Business
D) Enterprise

**Answer**: D) Enterprise

**Explanation**: The Enterprise Support plan includes a dedicated Technical Account Manager (TAM) who provides proactive guidance and support.

---

**Question 85**: What is the main benefit of AWS Organizations?

A) Single sign-on for all accounts
B) Centralized governance and management of multiple AWS accounts
C) Free compute resources
D) Unlimited storage

**Answer**: B) Centralized governance and management of multiple AWS accounts

**Explanation**: AWS Organizations helps you centrally manage and govern multiple AWS accounts with features like Service Control Policies (SCPs), Consolidated Billing, and account creation.

---

**Question 86**: Which pricing model provides up to 72% discount?

A) On-Demand
B) Reserved Instances
C) Spot
D) Dedicated Hosts

**Answer**: B) Reserved Instances

**Explanation**: Reserved Instances offer up to 72% discount compared to On-Demand pricing for steady-state workloads with 1-3 year commitments.

---

**Question 87**: What is consolidated billing in AWS Organizations?

A) Combining all accounts into one
B) Receiving a single bill for all member accounts
C) Free for all accounts
D) One account per department

**Answer**: B) Receiving a single bill for all member accounts

**Explanation**: Consolidated Billing allows you to receive a single monthly bill for all accounts in your organization, with aggregated usage for volume discounts.

---

**Question 88**: Which cost optimization tool provides recommendations for right-sizing resources?

A) Cost Explorer
B) Budgets
C) Trusted Advisor
D) Personal Health Dashboard

**Answer**: C) Trusted Advisor

**Explanation**: Trusted Advisor (available in Business and Enterprise plans) provides recommendations for cost optimization, including right-sizing EC2 instances and removing idle resources.

---

**Question 89**: What does a Savings Plan help you save on?

A) Only EC2
B) Only Lambda
C) EC2, Lambda, and Fargate
D) Only S3

**Answer**: C) EC2, Lambda, and Fargate

**Explanation**: Compute Savings Plans (EC2) offer savings on EC2, Lambda, and Fargate usage in exchange for a commitment to a specific amount of spend.

---

**Question 90**: What is the primary purpose of AWS Budgets?

A) Track historical spending
B) Set spending limits with alerts when thresholds are approached or exceeded
C) Generate invoices
D) Pay bills

**Answer**: B) Set spending limits with alerts when thresholds are approached or exceeded

**Explanation**: AWS Budgets allows you to set custom budgets with alerts when spending exceeds (or is forecasted to exceed) your defined thresholds.

---

**Question 91**: Which is NOT a factor in EC2 pricing?

A) Instance type
B) Region
C) Color of the instance
D) Operating system

**Answer**: C) Color of the instance

**Explanation**: EC2 pricing is based on instance type, region, operating system, and pricing model (On-Demand, Reserved, etc.), not on visual attributes.

---

**Question 92**: What is the primary benefit of the AWS Billing Concierge?

A) Automated payment
B) Enterprise Support feature providing dedicated support for billing questions
C) Free consulting
D) Account creation

**Answer**: B) Enterprise Support feature providing dedicated support for billing questions

**Explanation**: The Billing Concierge is an Enterprise Support feature that provides a dedicated team for billing questions and optimization recommendations.

---

**Question 93**: Which tool helps visualize and analyze your AWS costs over time?

A) Budgets
B) Cost Explorer
C) Personal Health Dashboard
D) CloudWatch

**Answer**: B) Cost Explorer

**Explanation**: AWS Cost Explorer lets you visualize, understand, and manage your AWS costs and usage over time with interactive charts and filtering.

---

**Question 94**: What is the benefit of All Upfront payment for Reserved Instances?

A) No payment
B) Maximum discount
C) Monthly payment
D) Free instance

**Answer**: B) Maximum discount

**Explanation**: Paying All Upfront for Reserved Instances provides the maximum discount (up to 72%) compared to Partial Upfront or No Upfront options.

---

**Question 95**: Which AWS cost category groups resources by tags?

A) Cost Explorer
B) Cost Categories
C) Budgets
D) Cur

**Answer**: B) Cost Categories

**Explanation**: AWS Cost Categories allows you to group your costs using custom dimensions like tags, accounts, or services.

---

**Question 96**: What is the primary purpose of the AWS Cost & Usage Report (CUR)?

A) Real-time cost display
B) Detailed, customizable data export for cost analysis
C) Budget alerts
D) Forecasting

**Answer**: B) Detailed, customizable data export for cost analysis

**Explanation**: The AWS Cost & Usage Report provides the most detailed data about your costs, including granular usage by service, operation, and tags, delivered to S3.

---

**Question 97**: What does "Pay-as-you-go" mean in AWS pricing?

A) Pay after using services
B) Pay only for what you use, no upfront costs
C) Prepay for all services
D) Pay once and use forever

**Answer**: B) Pay only for what you use, no upfront costs

**Explanation**: Pay-as-you-go means you pay only for the services you consume, with no long-term commitments or upfront payments required (except for Reserved Instances).

---

**Question 98**: Which Support plan provides 24/7 phone support?

A) Basic
B) Developer
C) Business and Enterprise
D) All plans

**Answer**: C) Business and Enterprise

**Explanation**: Developer Support provides email support during business hours. Business and Enterprise Support provide 24/7 phone, email, and chat support.

---

**Question 99**: What is the primary advantage of the Enterprise Support plan?

A) Cheapest price
B) Dedicated Technical Account Manager and proactive guidance
C) Unlimited resources
D) Free services

**Answer**: B) Dedicated Technical Account Manager and proactive guidance

**Explanation**: Enterprise Support includes a dedicated Technical Account Manager (TAM) who provides proactive guidance, architecture reviews, and support across your AWS environment.

---

**Question 100**: What happens when you exceed your AWS Budget threshold?

A) Services are automatically stopped
B) Alerts are sent, but services continue
C) Account is suspended
D) Nothing happens

**Answer**: B) Alerts are sent, but services continue

**Explanation**: AWS Budgets triggers alerts when you approach or exceed your set thresholds, but services continue to run. You need to manually take action or set up automated responses.

---

## Score Calculation

- 72 correct answers = 72% (passing score approximately)
- Review incorrect answers and revisit those topics
- Focus on weak domains

**Good luck with your AWS Cloud Practitioner exam!**