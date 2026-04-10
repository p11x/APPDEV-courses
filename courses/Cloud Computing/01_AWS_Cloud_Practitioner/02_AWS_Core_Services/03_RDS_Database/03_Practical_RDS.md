---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: RDS Database
Purpose: Practical RDS deployment, migration, and automation labs
Difficulty: intermediate
Prerequisites: 01_Basic_RDS.md, 02_Advanced_RDS.md
RelatedFiles: 01_Basic_RDS.md, 02_Advanced_RDS.md
UseCase: Production database deployment and management
CertificationExam: AWS Database Specialty
LastUpdated: 2025
---

## WHY

Hands-on RDS labs provide practical database deployment experience.

## WHAT

### Lab: Production Database

Deploy high-availability database with automated backup.

## HOW

### Module 1: Database Setup

```bash
# Create DB subnet group
aws rds create-db-subnet-group \
    --db-subnet-group-name production \
    --subnet-ids subnet-a subnet-b \
    --description "Production DB subnet group"
```

### Module 2: Create Database

```bash
# Create RDS with Multi-AZ
aws rds create-db-instance \
    --db-instance-identifier production-db \
    --db-instance-class db.r6g.large \
    --engine postgres \
    --allocated-storage 100 \
    --master-username admin \
    --master-user-password 'SecurePass123!' \
    --vpc-security-group-ids sg-rds \
    --db-subnet-group-name production \
    --multi-az \
    --backup-retention-period 30
```

### Module 3: Configure Read Replicas

```bash
# Create read replica
aws rds create-db-instance-reader \
    --db-instance-identifier read-replica \
    --source-db-instance-identifier production-db \
    --db-instance-class db.r6g.large
```

## VERIFICATION

```bash
# Check DB status
aws rds describe-db-instances \
    --db-instance-identifier production-db
```

## CROSS-REFERENCES

### Prerequisites

- VPC networking knowledge