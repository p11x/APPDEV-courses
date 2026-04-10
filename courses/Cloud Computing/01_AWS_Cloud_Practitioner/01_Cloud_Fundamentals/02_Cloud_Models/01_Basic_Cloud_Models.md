---
Category: AWS Cloud Practitioner
Subcategory: Cloud Fundamentals
Concept: Cloud Models
Purpose: Understand cloud deployment models including public, private, and hybrid cloud
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_Cloud_Models.md, 03_Practical_Cloud_Models.md
UseCase: Choosing appropriate cloud deployment model for business requirements
CertificationExam: AWS Certified Cloud Practitioner - Domain 1: Cloud Concepts
LastUpdated: 2025
---

## WHY

Understanding cloud deployment models is critical because each model has distinct characteristics, benefits, tradeoffs, and use cases. Choosing the wrong model can lead to security compliance issues, cost inefficiencies, or inability to meet business requirements.

### Importance in Cloud Strategy

- **Security Requirements**: Different models offer different security controls. HIPAA requires specific protections; government data often requires private cloud.
- **Cost Optimization**: Public cloud is cheapest for variable workloads but can be expensive for steady-state usage.
- **Regulatory Compliance**: Some industries mandate specific deployment models—financial services often require private cloud components.
- **Legacy Integration**: Many organizations cannot migrate all workloads to public cloud; hybrid cloud extends existing investments.

### Industry Adoption

- **Public Cloud**: 90%+ of new workloads (Gartner) — startups, web applications, dev/test environments
- **Private Cloud**: Enterprise databases, sensitive data processing, legacy applications
- **Hybrid Cloud**: Majority of enterprise migrations — gradual transition from on-premises

### When NOT to Use Specific Models

- **Public Cloud**: Not recommended for extremely sensitive data requiring on-premises control
- **Private Cloud**: Not cost-effective for variable, bursty workloads
- **Hybrid Cloud**: Not needed when full migration is feasible

## WHAT

### Cloud Deployment Models Defined

**Public Cloud**: Cloud services provided over the internet to the general public. Multiple organizations (tenants) share common infrastructure, with logical isolation through hypervisors and security controls.

- **Characteristics**: Owned/operated by cloud provider, multi-tenant, pay-as-you-go
- **Examples**: AWS, Azure, GCP public offerings
- **Benefits**: Low upfront cost, global scale, managed services

**Private Cloud**: Cloud infrastructure operated for a single organization. Can be hosted on-premises or in a dedicated data center with enhanced isolation.

- **Characteristics**: Single-tenant, dedicated hardware, more control
- **Examples**: AWS Outposts, Azure Stack, on-premises OpenStack
- **Benefits**: Maximum control, customization, compliance

**Hybrid Cloud**: Integration of public cloud with private cloud or on-premises infrastructure, allowing data and applications to move between environments.

- **Characteristics**: Connected environments, workload portability, unified management
- **Examples**: AWS Outposts + AWS Cloud, Azure Stack Hub + Azure
- **Benefits**: Flexibility, gradual migration, burst capacity

### Architecture Comparison

```
    PUBLIC CLOUD              PRIVATE CLOUD           HYBRID CLOUD
    =============             ==============          =============
    
    ┌───────────┐           ┌───────────┐          ┌───────────┐
    │ Internet  │           │Internet/FW │          │Internet/FW│
    └─────┬─────┘           └─��───┬─────┘          └─────┬─────┘
          │                        │                      │
    ┌─────┴─────┐           ┌─────┴─────┐          ┌─────┴─────┐
    │  Shared  │           │ Dedicated │          │  Combined │
    │ Infrstr. │           │ Infrstr.  │          │  Infrstr. │
    │          │           │           │          │           │
    │Tenant A  │           │Tenant A  │          │On-prem    │
    │Tenant B  │           │ Only     │          │ + Cloud   │
    │Tenant C  │           │          │          │           │
    └───────────┘           └───────────┘          └───────────┘
           │                        │                      │
    Provider-managed         Customer-managed    Shared management
```

### Comparison Table

| Attribute | Public Cloud | Private Cloud | Hybrid Cloud |
|-----------|--------------|---------------|--------------|
| Ownership | Provider | Customer/Provider | Both |
| Multi-tenancy | Yes | No | Partial |
| Management | Full provider | Full customer | Shared |
| Cost model | Pay-as-you-go | Fixed + variable | Variable mix |
| Customization | Limited | Extensive | Moderate |
| Scale | Unlimited | Fixed | Elastic |
| Compliance | Standard + Plus | Maximum | Flexible |

## HOW

### Example 1: Launching Public Cloud Resources

```bash
# Launch EC2 in public cloud (shared infrastructure)
aws ec2 run-instances \
    --image-id ami-0c55b159cbfafe1f0 \
    --instance-type t3.micro \
    --count 1 \
    --key-name my-key \
    --security-group-ids sg-0123456789abcdef0 \
    --subnet-id subnet-0123456789abcdef0

# Create S3 bucket (multi-tenant, shared)
aws s3 mb s3://my-public-bucket

# Launch RDS instance
aws rds create-db-instance \
    --db-instance-identifier prod-db \
    --db-instance-class db.t3.micro \
    --engine mysql \
    --allocated-storage 20 \
    --master-username admin \
    --master-user-password 'SecurePass123!' \
    --vpc-security-group-ids sg-0123456789abcdef0
```

### Example 2: Private Cloud Setup with AWS Outposts

```bash
# Create Outposts VPC (runs on Outposts, not AWS Region)
aws ec2 create-vpc \
    --cidr-block 10.0.0.0/16 \
    --outpost-arn arn:aws:outposts:us-east-1:123456789:outpost/op-1234567890abcdef

# Launch instance on Outposts (dedicated hardware)
aws ec2 run-instances \
    --image-id ami-0c55b159cbfafe1f0 \
    --instance-type t3.micro \
    --placement '{"OutpostArn": "arn:aws:outposts:us-east-1:123456789:outpost/op-1234567890abcdef"}' \
    --key-name my-key

# Create S3 bucket on Outposts
aws s3control create-bucket \
    --bucket my-outposts-bucket \
    --outpost-arn arn:aws:outposts:us-east-1:123456789:outpost/op-1234567890abcdef
```

### Example 3: Hybrid Cloud Connection

```bash
# Create Direct Connect connection to on-premises
aws directconnect connect-to-resource-portal \
    --connection-id dxcon-fghj567890abcdef

# Create transit gateway for hybrid networking
aws ec2 create-transit-gateway \
    --description "Hybrid cloud connection" \
    --amazon-asn 64512

# Attach VPC to transit gateway
aws ec2 attach-transit-gateway-vpc-attachments \
    --transit-gateway-attachment-id tgw-attach-1234567890abcdef \
    --vpc-id vpc-0123456789abcdef \
    --subnet-ids subnet-0123456789abcdef0

# Create S3 File Gateway for hybrid storage
aws storagegateway create-file-share \
    --file-share-name hybrid-storage-share \
    --gateway-arn arn:aws:storagegateway:us-east-1:123456789:gateway/fs-1234567890abcdef \
    --role role/create/file/share \
    --bucket my-hybrid-bucket

# Set up DataSync for hybrid data transfer
aws datasync create-task \
    --source-location-arn $ON_PREM_NFS \
    --destination-location-arn $AWS_S3 \
    --schedule '{"ScheduleExpression": "cron(0 0 ? * * *)"}'
```

## COMMON ISSUES

### 1. Choosing Wrong Model

**Problem**: Using public cloud for workloads requiring private cloud resources.

**Solution**: Evaluate requirements before choosing:
- Compliance requirements
- Data sensitivity
- Cost projection
- Management capabilities needed

### 2. Hybrid Integration Challenges

**Problem**: Connectivity or data portability issues between environments.

**Solution**: Plan integration carefully:
- Network latency considerations
- Data synchronization strategy
- Security policy alignment

### 3. Cost Mismatch

**Problem**: Private cloud costs more than anticipated.

**Solution**: Model total cost of ownership:
- Hardware procurement
- Facilities (power, cooling, space)
- Operations staff
- Maintenance

### 4. Regulatory Compliance Violations

**Problem**: Using public cloud for regulated data without proper controls.

**Solution**: Understand compliance certifications:
- Public cloud: SOC 2, ISO 27001, FedRAMP Moderate
- Private cloud: FedRAMP High, DoD CC
- Hybrid: Careful boundary management

### 5. Vendor Lock-in Issues

**Problem**: Inability to migrate between models or providers.

**Solution**: Use portable architectures:
- Container-based workloads
- Kubernetes for orchestration
- Open-source databases

## PERFORMANCE

### Comparison Metrics

| Metric | Public | Private | Hybrid |
|-------|--------|---------|--------|
| Provisioning time | Minutes | Weeks | Days + minutes |
| Scale speed | Seconds | Weeks | Variable |
| Availability SLA | 99.99% | Custom | Variable |
| Initial cost | $0 | $100K+ | $50K+ |
| Monthly cost | Variable | Fixed | Mixed |
| Management | Full provider | Full customer | Shared |

### Cost Optimization by Model

- **Public Cloud**: Use Reserved Instances/Savings Plans for predictable workloads
- **Private Cloud**: Right-size capacity, use open-source software
- **Hybrid**: Only migrate workloads that benefit from cloudeconomics

## COMPATIBILITY

### Service Availability by Model

| Service | Public | Private | Hybrid |
|---------|--------|---------|--------|
| EC2 | Full | Outposts | Both |
| S3 | Full | Outposts | File Gateway |
| RDS | Full | Outposts | DMS |
| Lambda | Full | Outposts | Edge |
| DynamoDB | Full | Not Available | Full |

### Compliance Certifications

Note: Compliance certifications vary. Always verify current certifications at:
- AWS: https://aws.amazon.com/compliance/
- Azure: https://docs.microsoft.com/en-us/azure/compliance/
- GCP: https://cloud.google.com/security/compliance

## CROSS-REFERENCES

### Related Concepts

- Cloud Concepts (Basic)—foundation of cloud understanding
- AWS Global Infrastructure—physical model implementation
- Security Services—all models have security implications

### Multi-Cloud Equivalents

| Model | AWS | Azure | GCP |
|-------|-----|-------|------|
| Public | AWS Cloud | Azure Cloud | GCP Cloud |
| Private | Outposts | Stack HCI/Hub | GCP Distributed Cloud |
| Hybrid | Direct Connect + Outposts | Arc + ExpressRoute | Anthos |

### Prerequisites

- Basic Cloud Concepts required before this file
- Study before: AWS Core Services Overview

### What to Study Next

1. Deployment Models (Advanced) for technical deep dive
2. AWS Global Infrastructure for understanding physical infrastructure
3. Shared Responsibility Model for security implications

## EXAM TIPS

### Key Facts

- Three cloud models: Public, Private, Hybrid
- Public: Multi-tenant, managed by provider, pay-as-you-go
- Private: Single-tenant, dedicated infrastructure
- Hybrid: Connected public and private for gradual migration

### Exam Questions

- **Question**: "Data must remain on-premises due to compliance" = Private Cloud
- **Question**: "Maximum control for custom workloads" = Private Cloud
- **Question**: "Migrate gradually from on-premises" = Hybrid Cloud
- **Question**: "Lowest cost for bursty workloads" = Public Cloud