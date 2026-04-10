---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: EC2 Compute
Purpose: Understanding Amazon EC2 virtual machines, instance types, and pricing options
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_EC2.md, 03_Practical_EC2.md
UseCase: Running virtual servers in AWS for any compute workload
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

EC2 is the foundational compute service in AWS, providing resizable virtual servers. Understanding EC2 is essential because virtually every AWS workload runs on or uses EC2 either directly or through services that run on EC2.

### Why EC2 Matters

- **Foundation of AWS**: 70%+ of AWS services run on EC2 under the hood
- **Flexibility**: Wide range of instance types for any workload
- **Control**: Full root access, complete OS control
- **Scalability**: Launch hundreds of instances in minutes
- **Cost Options**: On-Demand, Reserved, Spot for any budget

### Industry Statistics

- 95%+ of new AWS customers start with EC2
- Over 500 instance type options across families
- Largest instance: x1e.32xlarge with 3,904 GiB memory

### When NOT to Use EC2

- Event-driven workloads: Use Lambda
- Short-lived batch jobs: Use Batch or Spot
- Container workloads: Use ECS/EKS/Fargate
- Simple static hosting: Use S3 + CloudFront

## WHAT

### EC2 Core Concepts

**Instance**: Virtual server in AWS cloud. Each instance runs on hypervisor with allocated CPU, memory, storage, and network.

**Instance Type**: Defines hardware capabilities including vCPU count, memory, storage type, and network performance.

- **Families**: 
  - General Purpose (t, m, a) - balanced
  - Compute Optimized (c) - CPU-intensive
  - Memory Optimized (r, x, z) - large datasets
  - Storage Optimized (i, d, h) - I/O-intensive
  - Accelerated (p, g, inf) - GPU/FPGA

**Amazon Machine Image (AMI)**: Pre-configured template containing OS and software. Select by OS (Amazon Linux, Ubuntu, Windows) and architecture (x86, ARM).

**Security Group**: Virtual firewall controlling inbound/outbound traffic. Stateful, operating at instance level.

### Instance Type Comparison

| Family | Types | Best For | Example Use Cases |
|--------|-------|---------|-------------------|
| t | t3, t4g | Burstable, dev/test | Web servers, small DBs |
| m | m6i, m7g | General purpose | App servers, mid-tier DBs |
| c | c7i, c7g | Compute intensive | HPC, batch processing |
| r | r7g, r6i | Memory intensive | Large caches, in-memory DBs |
| x | x2gd | Extreme memory | SAP, Oracle |
| p | p4d, p5 | GPU/ML | Deep learning |
| g | g5 | Graphics intensive | Gaming, video encoding |
| i | i4i, im4dn | Storage intensive | NoSQL, data warehousing |
| d | d3, d3en | Dense storage | Distributed file systems |

### Architecture Diagram

```
                    EC2 INSTANCE ARCHITECTURE
                    ========================

    ┌─────────────────────────────────────────────────┐
    │               AWS CLOUD                         │
    │                                                 │
    │  ┌─────────────────────────────────────────┐   │
    │  │          HYPERVISOR LAYER                 │   │
    │  │     (Nitro / Xen)                      │   │
    │  └────────────────���────────────────────────┘   │
    │                    │                         │
    │    ┌───────────────┼───────────────┐         │
    │    ▼               ▼               ▼         │
    │ ┌──────┐      ┌──────┐      ┌──────┐        │
    │ │ EC2  │      │ EC2  │      │ EC2  │        │
    │ │Int-1A│      │Int-1B│      │Int-1C│        │
    │ └──┬───┘      └──┬───┘      └──┬───┘        │
    │    │             │             │              │
    │ ┌──┴───┐    ┌──┴───┐    ┌──┴───┐        │
    │ │SecGrp│    │SecGrp│    │SecGrp│        │
    │ │Allow │    │Allow │    │Allow │        │
    │ │ 80   │    │ 443  │    │ 22   │        │
    │ └──┬───┘    └──┬───┘    └──┬───┘        │
    │    │             │             │              │
    │    ▼             ▼             ▼               │
    │ ┌─────────────────────────────┐             │
    │ │   VPC / SUBNET              │             │
    │ │   Elastic Network Interface │             │
    │ │   (ENI) with IP Address    │             │
    │ └─────────────────────────────┘             │
    └─────────────────────────────────────────────┘
```

### EC2 Pricing Options

| Model | Discount | Use When | Key Feature |
|-------|----------|----------|-------------|
| On-Demand | 0% | Unknown usage | Pay by hour, no commitment |
| Savings Plans | 72% max | Predictable | Commit to usage, lower hourly |
| Reserved Instances | 72% max | Long-term | 1 or 3 year commitment |
| Spot Instances | 90% max | Fault-tolerant | Bid for spare capacity |
| Dedicated | Full price | Compliance | Single-tenant hardware |
| Dedicated Host | Full price | BYOL | Physical server access |

## HOW

### Example 1: Launch a Basic EC2 Instance

```bash
# Step 1: Find an AMI (using Amazon Linux 2)
aws ec2 describe-images \
    --owners amazon \
    --filters 'Name=name,Values=amzn2-ami-hvm-*-x86_64-gp2' \
    --query 'Images[0].ImageId' \
    --output text

# Output: ami-0c55b159cbfafe1f0

# Step 2: Create a key pair
aws ec2 create-key-pair \
    --key-name my-key-pair \
    --query 'KeyMaterial' \
    --output text > my-key-pair.pem
chmod 400 my-key-pair.pem

# Step 3: Create security group
SG_ID=$(aws ec2 create-security-group \
    --group-name web-sg \
    --description "Security group for web server" \
    --vpc-id vpc-0123456789abcdef0 \
    --query 'GroupId' \
    --output text)

# Step 4: Add security group rules
aws ec2 authorize-security-group-ingress \
    --group-id $SG_ID \
    --protocol tcp \
    --port 80 \
    --cidr 0.0.0.0/0

aws ec2 authorize-security-group-ingress \
    --group-id $SG_ID \
    --protocol tcp \
    --port 443 \
    --cidr 0.0.0.0/0

aws ec2 authorize-security-group-ingress \
    --group-id $SG_ID \
    --protocol tcp \
    --port 22 \
    --cidr 0.0.0.0/0

# Step 5: Launch instance
aws ec2 run-instances \
    --image-id ami-0c55b159cbfafe1f0 \
    --instance-type t3.micro \
    --key-name my-key-pair \
    --security-group-ids $SG_ID \
    --subnet-id subnet-0123456789abcdef0 \
    --tag-specifications 'ResourceType=instance,Tags=[{Key=Name,Value=WebServer}]'

# Expected output includes InstanceId: i-0123456789abcdef0
```

### Example 2: Manage EC2 Instances

```bash
# Describe instances
aws ec2 describe-instances \
    --filters "Name=instance-state-name,Values=running" \
    --query 'Reservations[].Instances[].{ID:InstanceId,Type:InstanceType,IP:PrivateIpAddress}'

# Start stopped instance
aws ec2 start-instances --instance-ids i-0123456789abcdef0

# Stop instance (graceful shutdown)
aws ec2 stop-instances --instance-ids i-0123456789abcdef0

# Reboot instance
aws ec2 reboot-instances --instance-ids i-0123456789abcdef0

# Terminate instance (deletion)
aws ec2 terminate-instances --instance-ids i-0123456789abcdef0
```

### Example 3: Connect to EC2 Instance

```bash
# Get instance public IP
aws ec2 describe-instances \
    --instance-ids i-0123456789abcdef0 \
    --query 'Reservations[0].Instances[0].PublicIpAddress'

# Connect via SSH (Linux/Mac)
ssh -i my-key-pair.pem ec2-user@<PUBLIC_IP>

# Connect via RDP (Windows)
# Use Remote Desktop with Administrator username

# Transfer files
scp -i my-key-pair.pem file.txt ec2-user@<PUBLIC_IP>:/tmp/
```

### Example 4: EC2 with User Data (Startup Script)

```bash
# Create user data script to auto-configure on launch
USER_DATA='#!/bin/bash
yum update -y
yum install -y httpd php mysql
systemctl start httpd
systemctl enable httpd
echo "<h1>Hello from EC2</h1>" > /var/www/html/index.html'

# Launch with user data
aws ec2 run-instances \
    --image-id ami-0c55b159cbfafe1f0 \
    --instance-type t3.micro \
    --key-name my-key-pair \
    --security-group-ids sg-0123456789abcdef0 \
    --subnet-id subnet-0123456789abcdef0 \
    --user-data "$USER_DATA" \
    --tag-specifications 'ResourceType=instance,Tags=[{Key=Name,Value=WebServer}]'
```

## COMMON ISSUES

### 1. Instance Launch Failures

**Problem**: Instance fails to launch with "InsufficientInstanceCapacity" or "InstanceLimitExceeded".

**Solutions**:
- Try different instance type or instance family
- Try different Availability Zone
- Request limit increase via AWS Support
- Use different Region

### 2. Cannot Connect to Instance

**Problem**: SSH/RDP connection refused or timeout.

**Checklist**:
- Security group allows traffic on port 22 (SSH) or 3389 (RDP)
- Instance has a public IP or is behind NAT
- Instance state is "running"
- Key pair permissions correct (chmod 400)
- Network ACLs allow traffic

### 3. Key Pair Issues

**Problem**: "Permissions for key pair are too open" or "Host key verification failed".

**Solution**:
```bash
# Fix key pair permissions (Linux/Mac)
chmod 400 my-key-pair.pem

# Fix key pair permissions (Windows)
# Right-click file > Properties > Security > Edit > Remove except your user
```

### 4. Wrong Instance Type Selected

**Problem**: Instance runs out of resources or costs too much.

**Solution**:
```bash
# Check instance monitoring
aws cloudwatch get-metric-statistics \
    --namespace AWS/EC2 \
    --metric-name CPUUtilization \
    --start-time 2024-01-01T00:00:00Z \
    --end-time 2024-01-02T00:00:00Z \
    --period 3600 \
    --statistics Maximum

# Right-size using AWS Compute Optimizer
aws compute-optimizer get-ec2-instance-recommendations \
    --instance Arns
```

### 5. Billing Surprises

**Problem**: Unexpected EC2 charges.

**Prevention**:
```bash
# Set up billing alerts
aws cloudwatch put-metric-alarm \
    --alarm-name "EC2-Cost-Alarm" \
    --metric-name EstimatedCharges \
    --namespace AWS/Billing \
    --statistic Maximum \
    --period 3600 \
    --threshold 50 \
    --comparison-operator GreaterThanThreshold

# Use cost allocation tags
aws ec2 create-tags \
    --resources i-0123456789abcdef0 \
    --tags "Key=CostCenter,Value=Marketing"
```

## PERFORMANCE

### Instance Performance Characteristics

| Instance | vCPU | Memory | Network | EBS |
|----------|------|--------|---------|-----|
| t3.micro | 2 | 1 GiB | Up to 5 Gbps | 64 GB |
| t3.small | 2 | 2 GiB | Up to 5 Gbps | 64 GB |
| t3.medium | 2 | 4 GiB | Up to 5 Gbps | 64 GB |
| m5.large | 2 | 8 GiB | Up to 10 Gbps | 64 GB |
| c5.large | 2 | 4 GiB | Up to 10 Gbps | 64 GB |
| r5.large | 2 | 16 GiB | Up to 10 Gbps | 64 GB |

### Performance Benchmarks

- t3.micro: Good for dev/test, low-traffic sites
- t3.medium: Suitable for small production web servers
- m5 series: General purpose production workloads
- c5 series: 15-25% faster than c4 for compute workloads
- r5 series: Optimized for memory-intensive workloads

### Network Performance

| Instance Size | Network Performance | ENA Enhanced |
|--------------|-------------------|-------------|
| Up to medium | Up to 10 Gbps | Not required |
| Large+ | 10+ Gbps | Required |
| terabyte-scale | 100 Gbps | Required with ENA |

## COMPATIBILITY

### Supported Operating Systems

- Amazon Linux 2 (recommended)
- Ubuntu Server (20.04, 22.04)
- Red Hat Enterprise Linux
- SUSE Linux Enterprise Server
- Debian
- Windows Server (2016, 2019, 2022)
- FreeBSD

### Architecture Support

- x86_64 (most common)
- arm64 (Graviton - a1, t4g, m6g, c6g, r6g)

### Region Availability

- All Regions except some government/特殊 Regions
- Some instance types limited to certain Regions

## CROSS-REFERENCES

### Related Services

- VPC: Required for networking
- IAM: For access control
- EBS: For storage
- VPC: Security groups in context
- CloudWatch: For monitoring

### Alternative Services

| Need | Use Instead |
|------|-------------|
| Serverless compute | Lambda |
| Containers | ECS/EKS |
| Batch jobs | Batch |
| No server management | Lightsail |

### Prerequisites

- Basic Cloud Concepts required

### What to Study Next

1. Advanced EC2: VPC, security groups, placement groups
2. Practical EC2: Hands-on labs
3. Auto Scaling: For production workloads

## EXAM TIPS

### Key Exam Facts

- EC2 = Elastic Compute Cloud = virtual servers
- Instance types: t (burstable), m (general), c (compute), r (memory), p/g (GPU)
- Pricing: On-Demand (1x), Reserved (72% off), Spot (90% off)
- Security groups: Stateful, allow rules only
- AMI = Amazon Machine Image = OS + software template

### Exam Questions

- **Question**: "Burstable workloads, pay per use" = t3 instance
- **Question**: "Maximum savings, 3 year commitment" = Reserved Instances
- **Question**: "Stateless firewall" = Security groups
- **Question**: "Fault-tolerant, batch processing" = Spot Instances