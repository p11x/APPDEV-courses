---
Category: Certification Prep
Subcategory: GCP ACE
Concept: Practice Questions
Purpose: 100 practice questions for GCP Cloud Engineer Associate certification exam
Difficulty: intermediate
Prerequisites: GCP cloud concepts
RelatedFiles: 01_Exam_Guide.md, 02_Section1_Setup.md, 03_Section2_Infrastructure.md, 04_Section3_Deploy.md, 05_Section4_Optimize.md
UseCase: GCP Cloud Engineer Associate certification exam preparation
CertificationExam: Google Cloud Associate Cloud Engineer (ACE)
LastUpdated: 2025
---

# GCP Cloud Engineer Associate Practice Questions (100 Questions)

## Questions 1-20

### Question 1
What is the highest level in the GCP resource hierarchy?

A. Folder
B. Organization
C. Project
D. Resource

**Answer: B**  
**Explanation:** Organization is the highest level in the GCP resource hierarchy, containing folders, projects, and resources.

---

### Question 2
Which GCP service provides Infrastructure as Code using YAML templates?

A. Terraform
B. Deployment Manager
C. gcloud CLI
D. Cloud Build

**Answer: B**  
**Explanation:** GCP Deployment Manager uses YAML templates to define and deploy infrastructure resources.

---

### Question 3
Which machine type family is cost-optimized for workloads that can tolerate interrupts?

A. N2
B. E2
C. M2
D. C2

**Answer: B**  
**Explanation:** E2 machine types are cost-optimized for workloads that can tolerate interruptions, offering lower pricing.

---

### Question 4
What is the minimum number of zones required for high availability in GCP?

A. 1
B. 2
C. 3
D. 4

**Answer: B**  
**Explanation:** Deploying resources across at least 2 zones provides high availability and protection against zone failures.

---

### Question 5
Which Cloud Storage class is most cost-effective for data accessed less than once per year?

A. Standard
B. Nearline
C. Coldline
D. Archive

**Answer: D**  
**Explanation:** Archive storage is designed for long-term data archiving with retrieval times of 12+ hours and the lowest cost.

---

### Question 6
What does the gcloud compute instances create command do?

A. Deletes a VM
B. Creates a VM
C. Lists VMs
D. Updates a VM

**Answer: B**  
**Explanation:** `gcloud compute instances create` creates a new Compute Engine virtual machine instance.

---

### Question 7
Which GCP service provides fully managed Kubernetes?

A. Compute Engine
B. App Engine
C. Cloud Functions
D. GKE

**Answer: D**  
**Explanation:** Google Kubernetes Engine (GKE) provides a fully managed Kubernetes service for container orchestration.

---

### Question 8
What is the default service account attached to a new Compute Engine instance?

A. Custom service account
B. Default Compute service account
C. No service account
D. Owner account

**Answer: B**  
**Explanation:** New instances automatically get the default Compute service account with broad permissions.

---

### Question 9
Which IAM role provides full control over a project without being the owner?

A. Viewer
B. Editor
C. Owner
D. Admin

**Answer: B**  
**Explanation:** The Editor role provides full control over resources but cannot manage billing or IAM.

---

### Question 10
What is Cloud NAT used for?

A. Internal load balancing
B. Allowing outbound internet for private instances
C. VPN connections
D. CDN acceleration

**Answer: B**  
**Explanation:** Cloud NAT allows instances without external IP addresses to access the internet for updates and patches.

---

### Question 11
Which command lists all GCP projects?

A. gcloud list projects
B. gcloud projects list
C. gcloud project list
D. gcloud list project

**Answer: B**  
**Explanation:** `gcloud projects list` displays all projects accessible to the authenticated user.

---

### Question 12
What is the purpose of VPC Flow Logs?

A. Monitor billing
B. Capture network traffic for diagnostics
C. Encrypt traffic
D. Load balance traffic

**Answer: B**  
**Explanation:** VPC Flow Logs capture network traffic data for debugging, security analysis, and performance monitoring.

---

### Question 13
Which GCP load balancer is global and supports HTTP/HTTPS?

A. Regional Load Balancer
B. Internal Load Balancer
C. Global HTTP(S) Load Balancer
D. TCP Load Balancer

**Answer: C**  
**Explanation:** Global HTTP(S) Load Balancer provides global content distribution with health-based routing.

---

### Question 14
What is the recommended VPC mode for new projects?

A. Auto mode
B. Custom mode
C. Legacy mode
D. Shared VPC

**Answer: B**  
**Explanation:** Custom mode VPC is recommended as it provides full control over subnet IP ranges.

---

### Question 15
Which service provides serverless function execution?

A. Compute Engine
B. App Engine
C. Cloud Functions
D. GKE

**Answer: C**  
**Explanation:** Cloud Functions provides serverless, event-driven compute for individual functions.

---

### Question 16
What is the purpose of Cloud CDN?

A. Database caching
B. Content delivery
C. Image processing
D. DNS resolution

**Answer: B**  
**Explanation:** Cloud CDN caches content at edge locations globally to deliver content with low latency.

---

### Question 17
Which command enables an API in GCP?

A. gcloud enable [API]
B. gcloud services enable [API]
C. gcloud api enable [API]
D. gcloud enable services [API]

**Answer: B**  
**Explanation:** `gcloud services enable [API_NAME]` enables a Google Cloud API for the project.

---

### Question 18
What is a managed instance group?

A. Manually scaled instances
B. Auto-scaled instances using templates
C. Single instance only
D. Static instance group

**Answer: B**  
**Explanation:** Managed instance groups use instance templates to automatically scale and manage groups of identical VMs.

---

### Question 19
Which Cloud SQL instance provides 99.99% availability?

A. Single zone
B. Multi-region
C. High availability (HA)
D. Read replica

**Answer: C**  
**Explanation:** Cloud SQL High Availability (HA) configuration provides 99.99% SLA with automatic failover.

---

### Question 20
What is the purpose of organization policies?

A. Billing management
B. Enforce constraints across resources
C. User authentication
D. Network configuration

**Answer: B**  
**Explanation:** Organization policies provide centralized control to enforce constraints on GCP resources.

---

## Questions 21-40

### Question 21
What does Cloud Audit Logs track?

A. Network traffic
B. API calls and resource changes
C. User logins only
D. Billing only

**Answer: B**  
**Explanation:** Cloud Audit Logs tracks API calls and administrative actions on GCP resources for compliance and debugging.

---

### Question 22
Which service account type is recommended for applications?

A. User account
B. Service account
C. Group account
D. Domain account

**Answer: B**  
**Explanation:** Service accounts are designed for applications and VM instances to authenticate to GCP APIs.

---

### Question 23
What is the purpose of a bastion host?

A. Database proxy
B. Secure SSH access to private instances
C. Load balancing
D. API gateway

**Answer: B**  
**Explanation:** A bastion host provides secure external access to instances in private subnets via SSH/RDP.

---

### Question 24
Which storage option supports NFS file sharing?

A. Cloud Storage
B. Cloud SQL
C. Filestore
D. BigQuery

**Answer: C**  
**Explanation:** Filestore provides managed NFS file shares for enterprise file storage workloads.

---

### Question 25
What is the difference between preemptible VMs and regular VMs?

A. No difference
B. Preemptible are cheaper but can be reclaimed
C. Preemptible are faster
D. Preemptible have higher SLA

**Answer: B**  
**Explanation:** Preemptible VMs are up to 80% cheaper but can be reclaimed by GCP with 30-second notice.

---

### Question 26
Which GCP service provides a NoSQL document database?

A. Cloud SQL
B. Cloud Spanner
C. Firestore
D. BigQuery

**Answer: C**  
**Explanation:** Firestore is a NoSQL document database providing real-time sync and offline support.

---

### Question 27
What does the gsutil cp command do?

A. Lists storage buckets
B. Copies files to/from Cloud Storage
C. Deletes buckets
D. Creates buckets

**Answer: B**  
**Explanation:** `gsutil cp` copies files between the local filesystem and Cloud Storage buckets.

---

### Question 28
What is Shared VPC?

A. VPC across regions
B. VPC shared across projects
C. VPN connection
D. VPC peering

**Answer: B**  
**Explanation:** Shared VPC allows an organization to share a VPC network across multiple projects.

---

### Question 29
Which command creates a GKE cluster?

A. gcloud container clusters list
B. gcloud container clusters create
C. gcloud create cluster
D. gcloud clusters create

**Answer: B**  
**Explanation:** `gcloud container clusters create` creates a new GKE cluster.

---

### Question 30
What is the purpose of Cloud Monitoring?

A. Only metrics
B. Metrics, alerts, and dashboards
C. Logging only
D. Billing only

**Answer: B**  
**Explanation:** Cloud Monitoring provides metrics, alerts, dashboards, and uptime checks for GCP resources.

---

### Question 31
Which IAM role should be used for least privilege?

A. Primitive role
B. Predefined role
C. Custom role
D. Owner role

**Answer: B**  
**Explanation:** Predefined roles provide granular, service-specific permissions aligned with least privilege.

---

### Question 32
What is the purpose of VPC peering?

A. Connect to the internet
B. Connect VPCs between projects
C. Load balancing
D. DNS resolution

**Answer: B**  
**Explanation:** VPC Peering allows private connectivity between VPC networks in the same or different projects.

---

### Question 33
Which service is best for petabyte-scale data warehousing?

A. Cloud SQL
B. Firestore
C. BigQuery
D. Spanner

**Answer: C**  
**Explanation:** BigQuery is a serverless, highly scalable data warehouse designed for petabyte-scale analytics.

---

### Question 34
What is Private Google Access?

A. Public IP addresses
B. Access GCP APIs from private IPs
C. VPN to on-premises
D. VPC encryption

**Answer: B**  
**Explanation:** Private Google Access allows instances without external IPs to access GCP APIs using private IPs.

---

### Question 35
What is Cloud Armor?

A. Load balancer
B. DDoS and WAF protection
C. CDN
D. VPN service

**Answer: B**  
**Explanation:** Cloud Armor provides DDoS protection and Web Application Firewall (WAF) capabilities.

---

### Question 36
Which command sets the active GCP project?

A. gcloud config set project
B. gcloud config set project [ID]
C. gcloud project set
D. gcloud set project

**Answer: B**  
**Explanation:** `gcloud config set project [PROJECT_ID]` sets the active project for gcloud commands.

---

### Question 37
What is the difference between App Engine Standard and Flexible?

A. No difference
B. Standard is serverless; Flexible uses containers
C. Standard uses containers
D. Flexible is cheaper

**Answer: B**  
**Explanation:** App Engine Standard is serverless with pay-per-use; Flexible runs containers with more control.

---

### Question 38
Which Cloud Storage feature keeps multiple versions of objects?

A. Lifecycle policies
B. Versioning
C. Object versioning
D. Replication

**Answer: B**  
**Explanation:** Versioning in Cloud Storage maintains historical versions of objects in a bucket.

---

### Question 39
What is the purpose of labels on GCP resources?

A. Billing only
B. Organization and filtering
C. Security only
D. Network only

**Answer: B**  
**Explanation:** Labels are key-value pairs used to organize, filter, and group GCP resources for management.

---

### Question 40
What is the maximum sustained use discount for preemptible VMs?

A. 50%
B. 60%
C. 70%
D. 80%

**Answer: D**  
**Explanation:** Preemptible VMs can provide up to 80% discount compared to regular on-demand pricing.

---

## Questions 41-60

### Question 41
Which GCP service is a globally distributed relational database?

A. Cloud SQL
B. Cloud Spanner
C. Firestore
D. BigQuery

**Answer: B**  
**Explanation:** Cloud Spanner is a globally distributed, strongly consistent relational database service.

---

### Question 42
What is the purpose of instance health checks in managed instance groups?

A. Billing
B. Auto-healing
C. Load balancing
D. Monitoring

**Answer: B**  
**Explanation:** Health checks detect unhealthy instances for auto-healing and load balancing distribution.

---

### Question 43
Which command creates a Cloud Storage bucket?

A. gsutil create
B. gsutil mb
C. gsutil new
D. gsutil init

**Answer: B**  
**Explanation:** `gsutil mb` creates a new Cloud Storage bucket.

---

### Question 44
What is the purpose of Cloud DNS?

A. CDN
B. Managed DNS service
C. Load balancing
D. VPN

**Answer: B**  
**Explanation:** Cloud DNS provides scalable, reliable DNS service for publishing and managing domain zones.

---

### Question 45
Which GCP service provides managed message queuing?

A. Cloud Storage
B. Pub/Sub
C. BigQuery
D. Firestore

**Answer: B**  
**Explanation:** Cloud Pub/Sub provides managed message queuing and event streaming for asynchronous communication.

---

### Question 46
What is the purpose of VPC connector in Cloud Functions?

A. Database connection
B. Access resources in a VPC network
C. Load balancing
D. DNS resolution

**Answer: B**  
**Explanation:** A VPC connector allows Cloud Functions to access resources in a VPC network securely.

---

### Question 47
What is the recommended way to authenticate GKE workloads to GCP APIs?

A. Service account keys
B. Workload Identity
C. Default credentials
D. OAuth tokens

**Answer: B**  
**Explanation:** Workload Identity is the recommended method for GKE to securely access GCP services without storing credentials.

---

### Question 48
What does the --preemptible flag do when creating a VM?

A. Makes it faster
B. Makes it cheaper and interruptible
C. Makes it more secure
D. Makes it larger

**Answer: B**  
**Explanation:** The --preemptible flag creates a preemptible VM at a lower price that can be reclaimed by GCP.

---

### Question 49
Which Cloud Logging feature provides filtering?

A. Logs Viewer only
B. Log filters and sinks
C. Export only
D. Metrics only

**Answer: B**  
**Explanation:** Cloud Logging provides log filters and sinks to route and analyze logs.

---

### Question 50
What is Cloud Scheduler?

A. Cron job service
B. Background processing
C. Batch jobs
D. Load testing

**Answer: A**  
**Explanation:** Cloud Scheduler provides cron job scheduling for triggering HTTP, Pub/Sub, or other actions.

---

### Question 51
Which command deploys an App Engine application?

A. gcloud app create
B. gcloud app deploy
C. gcloud deploy app
D. gcloud application deploy

**Answer: B**  
**Explanation:** `gcloud app deploy` deploys an App Engine application using app.yaml configuration.

---

### Question 52
What is the purpose of Cloud Build?

A. Monitoring
B. CI/CD pipeline
C. Logging
D. Storage

**Answer: B**  
**Explanation:** Cloud Build is a CI/CD service that builds, tests, and deploys applications.

---

### Question 53
What is the purpose of custom metadata on Compute Engine?

A. Only for display
B. Pass startup scripts and configuration
C. Billing only
D. Network only

**Answer: B**  
**Explanation:** Custom metadata passes startup scripts, custom data, and configuration to VM instances.

---

### Question 54
Which service provides managed Apache Spark and Hadoop?

A. Compute Engine
B. Dataproc
C. Dataflow
D. BigQuery

**Answer: B**  
**Explanation:** Dataproc provides managed Apache Spark and Hadoop clusters for big data processing.

---

### Question 55
What is the purpose of Cloud Asset Inventory?

A. Track billing
B. Query resource configurations
C. Monitor costs
D. Network analysis

**Answer: B**  
**Explanation:** Cloud Asset Inventory provides visibility into GCP resource configurations and their relationships.

---

### Question 56
Which command creates a service account?

A. gcloud iam service-account create
B. gcloud create service-account
C. gcloud service-account create
D. gcloud accounts create

**Answer: A**  
**Explanation:** `gcloud iam service-account create [NAME]` creates a new service account.

---

### Question 57
What is the difference between regional and global persistent disks?

A. No difference
B. Regional replicated across zones
C. Global is faster
D. Regional is cheaper

**Answer: B**  
**Explanation:** Regional persistent disks replicate data across two zones in a region for high availability.

---

### Question 58
What is the purpose of Confidential Computing?

A. At-rest encryption
B. Encryption during processing
C. Network encryption
D. Backup encryption

**Answer: B**  
**Explanation:** Confidential Computing encrypts data during computation using hardware-based enclaves.

---

### Question 59
Which GCP service provides a managed Redis?

A. Cloud Memorystore
B. Firestore
C. Cloud SQL
D. Spanner

**Answer: A**  
**Explanation:** Cloud Memorystore provides a fully managed Redis and Memcached service.

---

### Question 60
What is the purpose of Cloud Endpoints?

A. API hosting only
B. API management, security, monitoring
C. CDN
D. Load balancing

**Answer: B**  
**Explanation:** Cloud Endpoints provides API management, including authentication, monitoring, and security.

---

## Questions 61-80

### Question 61
Which command lists all buckets in Cloud Storage?

A. gsutil ls
B. gsutil list
C. gsutil buckets
D. gsutil show

**Answer: A**  
**Explanation:** `gsutil ls` lists all Cloud Storage buckets in the project.

---

### Question 62
What is the purpose of the --tags flag in firewall rules?

A. Labeling only
B. Applying firewall rules
C. Network classification
D. Billing

**Answer: B**  
**Explanation:** The --tags flag applies firewall rules to instances with matching network tags.

---

### Question 63
Which service is best for real-time streaming analytics?

A. BigQuery
B. Dataflow
C. Dataproc
D. Cloud Storage

**Answer: B**  
**Explanation:** Dataflow provides serverless real-time and batch processing using Apache Beam.

---

### Question 64
What is the purpose of Cloud Scheduler in VPC?

A. VPC management
B. Scheduled triggers
C. Network monitoring
D. DNS

**Answer: B**  
**Explanation:** Cloud Scheduler provides reliable scheduled triggering for HTTP, Pub/Sub, or App Engine.

---

### Question 65
What is the purpose of organization ID in resource hierarchy?

A. Billing only
B. Identifies organization root
C. Network only
D. IAM only

**Answer: B**  
**Explanation:** The organization ID uniquely identifies the GCP organization at the top of the resource hierarchy.

---

### Question 66
Which command sets lifecycle policy on a bucket?

A. gsutil lifecycle set
B. gsutil lifecycle set policy.json
C. gsutil set lifecycle
D. gsutil policy set

**Answer: A**  
**Explanation:** `gsutil lifecycle set [POLICY_FILE] gs://[BUCKET]` configures lifecycle policies.

---

### Question 67
What is the purpose of Cloud Run?

A. Batch processing
B. Serverless containers
C. VMs
D. Database

**Answer: B**  
**Explanation:** Cloud Run provides serverless container execution that automatically scales.

---

### Question 68
What is the purpose of Cloud IAM?

A. Only user management
B. Access control to GCP resources
C. Billing only
D. Network only

**Answer: B**  
**Explanation:** Cloud IAM provides unified access control across all GCP services and resources.

---

### Question 69
Which GCP service is used for API gateway?

A. Cloud Endpoints
B. API Gateway
C. Both A and B
D. Load Balancer

**Answer: C**  
**Explanation:** Both Cloud Endpoints and API Gateway provide API gateway capabilities for GCP.

---

### Question 70
What does the --allow flag do in firewall rules?

A. Deny traffic
B. Allow traffic
C. Log traffic
D. Monitor traffic

**Answer: B**  
**Explanation:** The --allow flag specifies which protocols and ports are permitted.

---

### Question 71
What is the purpose of Cloud Deployment Manager?

A. Monitoring
B. Infrastructure deployment
C. Logging
D. Billing

**Answer: B**  
**Explanation:** Deployment Manager deploys infrastructure using declarative templates.

---

### Question 72
Which command creates a VPC?

A. gcloud compute networks create
B. gcloud create network
C. gcloud networks create
D. gcloud vpc create

**Answer: A**  
**Explanation:** `gcloud compute networks create` creates a new VPC network.

---

### Question 73
What is the purpose of Cloud SQL Proxy?

A. Database backup
B. Secure database connections
C. Database migration
D. Database replication

**Answer: B**  
**Explanation:** Cloud SQL Proxy provides secure connections to Cloud SQL instances without public IPs.

---

### Question 74
What is the purpose of the startup-script metadata?

A. Network config
B. Run script on VM boot
C. Billing config
D. Security config

**Answer: B**  
**Explanation:** The startup-script metadata executes a script when a VM instance starts.

---

### Question 75
Which GCP service provides ETL capabilities?

A. Cloud Storage
B. Dataflow
C. Dataproc
D. Both B and C

**Answer: D**  
**Explanation:** Both Dataflow (serverless) and Dataproc (Spark/Hadoop) provide ETL capabilities.

---

### Question 76
What is the purpose of Cloud Logging?

A. Metrics
B. Centralized logging
C. Monitoring
D. Billing

**Answer: B**  
**Explanation:** Cloud Logging provides centralized log collection, search, and analysis for GCP resources.

---

### Question 77
Which command updates firewall rules?

A. gcloud compute firewall-rules update
B. gcloud firewall-rules update
C. gcloud update firewall
D. gcloud rules update

**Answer: A**  
**Explanation:** `gcloud compute firewall-rules update` modifies existing firewall rule configurations.

---

### Question 78
What is the purpose of Cloud Memorystore?

A. Database
B. In-memory caching
C. Storage
D. Analytics

**Answer: B**  
**Explanation:** Cloud Memorystore provides in-memory caching using Redis and Memcached.

---

### Question 79
What is the purpose of the --scopes flag when creating a VM?

A. Network access
B. API access permissions
C. Storage access
D. CPU allocation

**Answer: B**  
**Explanation:** The --scopes flag specifies which APIs the VM's service account can access.

---

### Question 80
What does Cloud Resource Manager provide?

A. Only resource listing
B. Managing organization resources and policies
C. Billing only
D. Monitoring only

**Answer: B**  
**Explanation:** Cloud Resource Manager provides centralized management of organization resources and policies.

---

## Questions 81-100

### Question 81
Which command creates a SQL instance?

A. gcloud sql instances create
B. gcloud create sql instance
C. gcloud create instance
D. gcloud sql create

**Answer: A**  
**Explanation:** `gcloud sql instances create` creates a new Cloud SQL instance.

---

### Question 82
What is the purpose of Cloud Armor Security Policy?

A. Only monitoring
B. WAF and DDoS rules
C. Logging only
D. Network only

**Answer: B**  
**Explanation:** Cloud Armor Security Policy provides WAF rules to protect against web attacks and DDoS.

---

### Question 83
What is the purpose of Confidential VMs?

A. Lower cost
B. Shielded workloads with encryption
C. Faster VMs
D. More CPU

**Answer: B**  
**Explanation:** Confidential VMs use AMD SEV to encrypt VM memory, protecting sensitive workloads.

---

### Question 84
Which command creates a topic in Pub/Sub?

A. gcloud pubsub create topic
B. gcloud pubsub topics create
C. gcloud topics create
D. gcloud create topic

**Answer: B**  
**Explanation:** `gcloud pubsub topics create` creates a new Pub/Sub topic.

---

### Question 85
What is the purpose of Cloud Storage Transfer Service?

A. Bucket management
B. Transfer data between buckets
C. On-premises to Cloud
D. Both B and C

**Answer: D**  
**Explanation:** Cloud Storage Transfer Service transfers data between buckets and from on-premises sources.

---

### Question 86
What is the purpose of Cloud Build triggers?

A. Manual builds only
B. Automated builds on code changes
C. Monitoring builds
D. Billing builds

**Answer: B**  
**Explanation:** Cloud Build triggers automate builds when code changes are pushed to repositories.

---

### Question 87
Which command adds an IAM policy binding?

A. gcloud iam add-binding
B. gcloud projects add-iam-policy-binding
C. gcloud add iam
D. gcloud iam policy add

**Answer: B**  
**Explanation:** `gcloud projects add-iam-policy-binding` adds an IAM policy binding to a project.

---

### Question 88
What is the purpose of the --machine-type flag?

A. Network type
B. VM resource allocation
C. Disk size
D. Region

**Answer: B**  
**Explanation:** The --machine-type flag specifies the CPU and memory configuration for a VM.

---

### Question 89
What is the purpose of Cloud Filestore?

A. Object storage
B. File storage for enterprises
C. Block storage
D. Archive storage

**Answer: B**  
**Explanation:** Cloud Filestore provides managed file storage (NFS) for enterprise applications.

---

### Question 90
Which GCP service provides a managed Kafka?

A. Pub/Sub
B. Dataproc
C. Confluent Cloud
D. Dataflow

**Answer: C**  
**Explanation:** Confluent Cloud provides a managed Kafka service on GCP.

---

### Question 91
What is the purpose of the --boot-disk-size flag?

A. Memory size
B. Disk size for VM
C. Network size
D. CPU size

**Answer: B**  
**Explanation:** The --boot-disk-size flag specifies the size of the VM's boot disk.

---

### Question 92
What is Cloud Billing Account?

A. Payment only
B. Tracks costs and payments
C. Monitoring only
D. Reporting only

**Answer: B**  
**Explanation:** Cloud Billing Account tracks costs, manages payments, and provides billing reports.

---

### Question 93
Which command creates a subscription in Pub/Sub?

A. gcloud pubsub create subscription
B. gcloud pubsub subscriptions create
C. gcloud subscriptions create
D. gcloud create subscription

**Answer: B**  
**Explanation:** `gcloud pubsub subscriptions create` creates a new Pub/Sub subscription.

---

### Question 94
What is the purpose of Cloud Security Scanner?

A. Network scanning
B. Web application vulnerability scanning
C. Database scanning
D. VM scanning

**Answer: B**  
**Explanation:** Cloud Security Scanner identifies vulnerabilities in GAE, GCS, and GKE applications.

---

### Question 95
What is the purpose of the --zone flag in gcloud commands?

A. Region only
B. Availability zone
C. Global
D. Multi-region

**Answer: B**  
**Explanation:** The --zone flag specifies the availability zone for regional resources.

---

### Question 96
What is the purpose of Cloud Natural Language API?

A. Translation
B. Text analysis
C. Speech recognition
D. Vision

**Answer: B**  
**Explanation:** Cloud Natural Language API provides text analysis including sentiment and entity extraction.

---

### Question 97
What is the purpose of the --labels flag?

A. Network labeling
B. Resource organization
C. Billing tags
D. Security labeling

**Answer: B**  
**Explanation:** The --labels flag adds labels to resources for organization and cost tracking.

---

### Question 98
What is Cloud Speech-to-Text?

A. Text to speech
B. Speech recognition
C. Translation
D. NLP

**Answer: B**  
**Explanation:** Cloud Speech-to-Text converts audio to text using machine learning.

---

### Question 99
What is the purpose of the --service-account flag?

A. Billing only
B. Attach specific service account to VM
C. Network only
D. Storage only

**Answer: B**  
**Explanation:** The --service-account flag attaches a specific service account to a VM instance.

---

### Question 100
What is Cloud Vision API?

A. Text analysis
B. Image recognition and classification
C. Video analysis
D. Speech recognition

**Answer: B**  
**Explanation:** Cloud Vision API analyzes images to detect objects, faces, text, and other content.

---

**End of Practice Questions**

**Good luck with your exam preparation!**