---
Category: Google Cloud Platform
Subcategory: GCP Core Services
Concept: Cloud Spanner
Purpose: Hands-on exercises for Cloud Spanner database management
Difficulty: advanced
Prerequisites: 01_Basic_Cloud_Spanner.md, 02_Advanced_Cloud_Spanner.md
RelatedFiles: 01_Basic_Cloud_Spanner.md, 02_Advanced_Cloud_Spanner.md
UseCase: Global database deployment, schema design, backup management
CertificationExam: GCP Data Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Cloud Spanner is essential for building global-scale databases, optimizing schema design, and managing enterprise workloads.

### Lab Goals

- Create Spanner databases
- Design schema with interleaving
- Configure backups

## 📖 WHAT

### Exercise Overview

1. **Database Creation**: Instance and database setup
2. **Schema Design**: Interleaved tables and indexes
3. **Backup Management**: Automated backups

## 🔧 HOW

### Exercise 1: Create and Configure Database

```bash
#!/bin/bash
# Create Cloud Spanner database

PROJECT_ID="my-project-id"
INSTANCE_NAME="production-spanner"

gcloud config set project $PROJECT_ID

# Create instance
gcloud spanner instances create $INSTANCE_NAME \
    --config=regional-us-central1 \
    --description="Production Spanner" \
    --nodes=3

# Create database
gcloud spanner databases create orders-db \
    --instance=$INSTANCE_NAME

# Create schema
gcloud spanner databases ddl update orders-db \
    --instance=$INSTANCE_NAME \
    --ddl='CREATE TABLE Customers (
        CustomerId STRING(36) NOT NULL,
        Name STRING(100) NOT NULL,
        Email STRING(255),
        Region STRING(50),
        CreatedAt TIMESTAMP NOT NULL OPTIONS (allow_commit_timestamp=true)
    ) PRIMARY KEY (CustomerId)'

# Create interleaved tables
gcloud spanner databases ddl update orders-db \
    --instance=$INSTANCE_NAME \
    --ddl='CREATE TABLE Orders (
        OrderId STRING(36) NOT NULL,
        CustomerId STRING(36) NOT NULL,
        Total DECIMAL(10,2),
        Status STRING(20),
        CreatedAt TIMESTAMP NOT NULL OPTIONS (allow_commit_timestamp=true)
    ) PRIMARY KEY (CustomerId, OrderId),
    INTERLEAVE IN PARENT Customers ON DELETE CASCADE'

gcloud spanner databases ddl update orders-db \
    --instance=$INSTANCE_NAME \
    --ddl='CREATE TABLE OrderItems (
        OrderId STRING(36) NOT NULL,
        CustomerId STRING(36) NOT NULL,
        ItemId STRING(36) NOT NULL,
        ProductName STRING(200),
        Quantity INT64,
        Price DECIMAL(10,2)
    ) PRIMARY KEY (CustomerId, OrderId, ItemId),
    INTERLEAVE IN PARENT Orders ON DELETE CASCADE'

# Add indexes
gcloud spanner databases ddl update orders-db \
    --instance=$INSTANCE_NAME \
    --ddl='CREATE INDEX idx_orders_status ON Orders(Status),
    CREATE INDEX idx_orders_created ON Orders(CreatedAt)'

echo "Database created and configured!"
```

### Exercise 2: Query and Manage Data

```bash
#!/bin/bash
# Query and manage Spanner data

PROJECT_ID="my-project-id"
INSTANCE_NAME="production-spanner"

gcloud config set project $PROJECT_ID

# Insert data
gcloud spanner databases execute-sql orders-db \
    --instance=$INSTANCE_NAME \
    --mutation=insert \
    --insert-mutations='[
        {
            table: "Customers",
            columns: ["CustomerId", "Name", "Email", "Region", "CreatedAt"],
            values: [["C001", "John Doe", "john@example.com", "US", PENDING_COMMIT_TIMESTAMP()]]
        }
    ]'

# Read data
gcloud spanner databases execute-sql orders-db \
    --instance=$INSTANCE_NAME \
    --read-only \
    --query='SELECT * FROM Customers'

# Transactional read/write
gcloud spanner databases execute-sql orders-db \
    --instance=$INSTANCE_NAME \
    --read-write \
    --query='INSERT INTO Orders (OrderId, CustomerId, Total, Status, CreatedAt)
             VALUES (@orderId, @customerId, @total, @status, PENDING_COMMIT_TIMESTAMP())' \
    --params='{"orderId":"O001","customerId":"C001","total":100.00,"status":"PENDING"}'

# Profile query
gcloud spanner databases execute-sql orders-db \
    --instance=$INSTANCE_NAME \
    --query-mode=PROFILE \
    --query='SELECT o.OrderId, c.Name, o.Total
             FROM Orders o JOIN Customers c ON o.CustomerId = c.CustomerId'

echo "Data operations completed!"
```

### Exercise 3: Configure Backups

```bash
#!/bin/bash
# Configure Spanner backups

PROJECT_ID="my-project-id"
INSTANCE_NAME="production-spanner"

gcloud config set project $PROJECT_ID

# Create on-demand backup
gcloud spanner backups create daily-backup \
    --instance=$INSTANCE_NAME \
    --database=orders-db \
    --retention-days=30

# List backups
gcloud spanner backups list --instance=$INSTANCE_NAME

# Describe backup
gcloud spanner backups describe daily-backup \
    --instance=$INSTANCE_NAME

# Restore database from backup
gcloud spanner databases restore \
    --backup=daily-backup \
    --destination-instance=$INSTANCE_NAME \
    --destination-database=orders-db-restore

# Enable point-in-time recovery
gcloud spanner databases update orders-db \
    --instance=$INSTANCE_NAME \
    --enable-point-in-time-recovery

# Restore to specific time
gcloud spanner databases restore \
    --backup=daily-backup \
    --destination-instance=$INSTANCE_NAME \
    --destination-database=orders-db-pitr \
    --restore-point-time="2025-01-15T10:00:00Z"

# Delete old backups
gcloud spanner backups delete old-backup \
    --instance=$INSTANCE_NAME

echo "Backup management configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Slow queries | Check indexes |
| High costs | Reduce nodes |
| Unavailable | Use multi-region |

### Validation

```bash
# Check instance
gcloud spanner instances describe $INSTANCE_NAME

# Check database
gcloud spanner databases describe orders-db --instance=$INSTANCE_NAME
```

## 🌐 COMPATIBILITY

### Integration

- BigQuery: Export for analytics
- Cloud Storage: Backup storage
- Cloud Logging: Audit logs

## 🔗 CROSS-REFERENCES

### Related Labs

- Cloud SQL
- BigQuery
- Cloud Storage

### Next Steps

- Configure monitoring
- Set up alerting
- Implement encryption

## ✅ EXAM TIPS

- Practice schema creation
- Know interleaving benefits
- Understand backup options
- Monitor query performance
