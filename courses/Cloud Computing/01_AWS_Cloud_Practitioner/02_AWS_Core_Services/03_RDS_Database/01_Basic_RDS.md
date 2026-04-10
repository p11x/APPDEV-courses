---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: RDS Database
Purpose: Understanding Amazon RDS managed database service, instance types, and configurations
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_RDS.md, 03_Practical_RDS.md
UseCase: Running managed relational databases without operational overhead
CertificationExam: AWS Certified Cloud Practitioner - Domain 3: Technology
LastUpdated: 2025
---

## WHY

Amazon RDS provides managed relational databases, handling patching, backups, replication, and failover. This allows developers to focus on applications rather than database administration.

### Why RDS Matters

- **Managed Operations**: Automatic patching, backups, and software updates
- **High Availability**: Multi-AZ deployment for 99.99% availability
- **Scalability**: Read replicas for read-heavy workloads
- **Security**: Encryption at rest and in transit
- **Cost-Effective**: Pay for what you use

### Industry Statistics

- 85%+ of AWS customers use RDS
- Supports 6 major database engines
- Automatic failover typically < 60 seconds

### When NOT to Use RDS

- Ultra-high performance needs: Consider Aurora
- NoSQL workloads: Use DynamoDB
- Very large scale: Consider Redshift
- Complete control needed: Use EC2 with self-managed DB

## WHAT

### RDS Core Concepts

**Database Instance**: Isolated database environment in the cloud. Each instance runs a specific database engine.

**Database Engine**: Supported database software:
- Amazon Aurora (MySQL/PostgreSQL compatible)
- MySQL
- PostgreSQL
- MariaDB
- Oracle
- SQL Server

**DB Instance Class**: Compute and memory capacity:

| Class | vCPU | Memory | Best For |
|-------|------|--------|-----------|
| db.t3.micro | 2 | 1 GiB | Dev/test |
| db.t3.small | 2 | 2 GiB | Small apps |
| db.t3.medium | 2 | 4 GiB | Production small |
| db.r6g.large | 2 | 16 GiB | Production medium |
| db.r6g.xlarge | 4 | 32 GiB | Production large |

**Multi-AZ**: Standby replica in separate AZ for automatic failover.

**Read Replica**: Read-only copy for scaling read operations.

### Architecture Diagram

```
                    RDS ARCHITECTURE
                    ================

    ┌──────────────────────────────────────────────────────┐
    │                 APPLICATION                           │
    └──────────────────────────────────────────────────────┘
                           │
    ┌───────────────────────┼────────────────────────────┐
    │                       │                             │
    ▼                       ▼                             ▼
┌──────────┐        ┌──────────┐               ┌──────────┐
│ WRITE    │        │ READ     │               │ BACKUP   │
│Endpoint │        │Endpoint  │               │ S3       │
└──────────┘        └──────────┘               └──────────┘
    │                       │                             │
    ▼                       ▼                             ▼
┌─────────────────���───────────────────────────────────────────┐
│                  RDS DATABASE                          │
│  ┌───────────────────────────────────────────────┐   │
│  │              PRIMARY INSTANCE                  │   │
│  │  ┌─────────┐  ┌─────────┐  ┌─────────────┐   │   │
│  │  │   DB    │  │  Storage│  │  Replication│   │   │
│  │  │ Engine  │  │  (EBS)  │  │  to Standby │   │   │
│  │  └─────────┘  └─────────┘  └─────────────┘   │   │
│  └───────────────────────────────────────────────┘   │
│                      │                               │
│              ┌───────┴───────┐                       │
│              │              │                         │
│              ▼              ▼                        ▼
│      ┌──────────┐    ┌──────────┐           ┌──────────┐
│      │  PRIMARY │◄──►│ STANDBY  │           │ AUTOMATED │
│      │   (AZ-a) │    │  (AZ-b)  │           │ BACKUPS   │
│      └──────────┘    └──────────┘           └──────────┘
└─────────────────────────────────────────────────────────┘
```

### Storage Types

| Type | Performance | Use Case | Cost |
|------|-------------|---------|------|
| General Purpose (SSD) | 3,000 IOPS | General workloads | Lower |
| Provisioned IOPS SSD | 64,000 IOPS | High-performance | Higher |
| Magnetic | Standard | Backups, dev | Lowest |

## HOW

### Example 1: Create RDS Database

```bash
# Step 1: Create VPC first (if needed)
VPC_ID=$(aws ec2 create-vpc \
    --cidr-block 10.0.0.0/16 \
    --query 'Vpc.VpcId' \
    --output text)

# Step 2: Create subnet group
aws rds create-db-subnet-group \
    --db-subnet-group-name my-db-subnets \
    --subnet-ids subnet-0123456789abcdef0 subnet-1234567890abcdef1 \
    --description "Subnet group for RDS"

# Step 3: Create security group
SG_ID=$(aws ec2 create-security-group \
    --group-name rds-sg \
    --description "Security group for RDS" \
    --vpc-id $VPC_ID \
    --query 'GroupId' \
    --output text)

aws ec2 authorize-security-group-ingress \
    --group-id $SG_ID \
    --protocol tcp \
    --port 3306 \
    --cidr 10.0.0.0/16

# Step 4: Create RDS instance
aws rds create-db-instance \
    --db-instance-identifier my-database \
    --db-instance-class db.t3.medium \
    --engine mysql \
    --engine-version 8.0.35 \
    --allocated-storage 20 \
    --storage-type gp3 \
    --master-username admin \
    --master-user-password 'SecurePass123!' \
    --vpc-security-group-ids $SG_ID \
    --db-subnet-group-name my-db-subnets \
    --backup-retention-period 7 \
    --multi-az \
    --publicly-accessible

# Step 5: Wait for available
aws rds wait db-instance-available \
    --db-instance-identifier my-database

# Get endpoint
aws rds describe-db-instances \
    --db-instance-identifier my-database \
    --query 'DBInstances[0].Endpoint.Address'
```

### Example 2: Connect to RDS Database

```bash
# Get the endpoint
ENDPOINT=$(aws rds describe-db-instances \
    --db-instance-identifier my-database \
    --query 'DBInstances[0].Endpoint.Address' \
    --output text)

PORT=$(aws rds describe-db-instances \
    --db-instance-identifier my-database \
    --query 'DBInstances[0].Endpoint.Port' \
    --output text)

# Connect with MySQL client
mysql -h $ENDPOINT -P $PORT -u admin -p

# Or with psql for PostgreSQL
psql -h $ENDPOINT -p $PORT -U admin -d mydatabase
```

### Example 3: Create Read Replica

```bash
# Create read replica for read scaling
aws rds create-db-instance-read-replica \
    --db-instance-identifier my-database-replica \
    --source-db-instance-identifier my-database \
    --db-instance-class db.t3.medium \
    --availability-zone us-west-2a

# Promote read replica to standalone (if needed)
aws rds promote-read-replica \
    --db-instance-identifier my-database-replica
```

### Example 4: Configure Automated Backups

```bash
# Modify backup retention
aws rds modify-db-instance \
    --db-instance-identifier my-database \
    --backup-retention-period 30 \
    --apply-immediately

# Create manual snapshot
aws rds create-db-snapshot \
    --db-snapshot-identifier my-manual-backup \
    --db-instance-identifier my-database

# Restore from snapshot
aws rds restore-db-instance-from-snapshot \
    --db-instance-identifier my-database-restored \
    --db-snapshot-identifier my-manual-backup
```

## COMMON ISSUES

### 1. Connection Timeout

**Problem**: Cannot connect to RDS instance.

**Root causes and solutions**:
- Security group not allowing port: Add inbound rule
- Not publicly accessible: Modify instance
- Subnet has no route to internet: Check VPC routing

### 2. Storage Full

**Problem**: Database stops accepting writes.

**Solution**:
```bash
# Check storage usage
aws rds describe-db-instances \
    --db-instance-identifier my-database \
    --query 'DBInstances[0].AllocatedStorage'

# Modify storage
aws rds modify-db-instance \
    --db-instance-identifier my-database \
    --allocated-storage 100 \
    --apply-immediately
```

### 3. Failover Not Working

**Problem**: Primary fails but no automatic failover.

**Solution**:
- Ensure Multi-AZ is enabled
- Check that standby is in different AZ
- Verify both subnets are routable

### 4. Slow Performance

**Problem**: Queries running slow.

**Solution**:
- Use read replicas for read-heavy workloads
- Enable Performance Insights
- Consider Aurora for better performance

### 5. Accidental Deletion

**Problem**: Database deleted without backup.

**Solution**:
- Enable deletion protection
- Use final snapshot
- Configure CloudWatch deletion alerts

## PERFORMANCE

### Performance Benchmarks

| Instance | IOPS | Throughput | Connections |
|----------|------|-----------|------------|
| db.t3.micro | 3,000 | 250 MB/s | 85 |
| db.t3.medium | 3,000 | 250 MB/s | 305 |
| db.r6g.large | 16,000 | 1,000 MB/s | 500 |
| db.r6g.xlarge | 32,000 | 2,000 MB/s | 1,000 |

### Aurora Performance

- Up to 5x better than standard MySQL
- Up to 3x better than standard PostgreSQL
- Automatic storage scaling to 128 TB
- Serverless option available

## COMPATIBILITY

### Supported Engines

| Engine | Versions |
|--------|----------|
| MySQL | 5.7.40+, 8.0.x |
| PostgreSQL | 11.x - 15.x |
| MariaDB | 10.2 - 10.6 |
| Oracle | 11g R2, 12c, 19c, 21c |
| SQL Server | 2014, 2016, 2017, 2019 |

### Region Availability

- All commercial Regions
- Not all engines in all Regions

## CROSS-REFERENCES

### Related Services

- EC2: Self-managed database alternative
- Aurora: Higher performance option
- DynamoDB: NoSQL alternative
- Redshift: Data warehouse

### Prerequisites

- Basic Cloud Concepts
- VPC networking

### What to Study Next

1. Advanced RDS: Multi-AZ, read replicas
2. Practical RDS: Migration, optimization
3. Security: Encryption options