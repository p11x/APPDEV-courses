---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Databases
Purpose: Advanced database features including geo-replication, auto-failover, elastic pools, and performance optimization
Difficulty: advanced
Prerequisites: 01_Basic_Azure_Databases.md
RelatedFiles: 01_Basic_Azure_Databases.md, 03_Practical_Azure_Databases.md
UseCase: Enterprise database deployment with high availability and scalability
CertificationExam: AZ-304 Azure Solutions Architect
LastUpdated: 2025
---

## 💡 WHY

Advanced Azure database features provide enterprise-grade high availability, global distribution, and performance optimization for critical workloads.

## 📖 WHAT

### SQL Database Advanced Features

| Feature | Description | Benefit |
|---------|-------------|---------|
| Active Geo-Replication | Up to 4 secondaries | DR capability |
| Auto-Failover Groups | Automatic failover | Business continuity |
| Long-term Retention | Up to 10 years | Backup archives |
| Hyperscale | Up to 100TB | Scale on demand |

### Cosmos DB Advanced Features

| API | Features | Use Case |
|-----|---------|----------|
| SQL | JSON, SQL queries | General purpose |
| MongoDB | MongoDB wire protocol | Migration |
| Cassandra | CQL compatibility | Wide column |
| Gremlin | Graph queries | Social networks |
| Table | Key-value | Azure Table migration |

### PostgreSQL/MySQL Flexible Server

| Feature | Description |
|---------|-------------|
| High Availability | Zone-redundant standby |
| Read Replicas | Up to 5 replicas |
| Azure Advisor | Performance tuning |
| Auto-patching | Automated updates |

### Cross-Platform Comparison

| Feature | Azure | AWS | GCP |
|---------|-------|-----|----------|
| Managed SQL | SQL Database | RDS | Cloud SQL |
| PostgreSQL | Azure DB | RDS | Cloud SQL |
| MySQL | Azure DB | RDS | Cloud SQL |
| NoSQL | Cosmos DB | DynamoDB | Firestore |
| Analytics | Synapse | Redshift | BigQuery |
| Cache | Redis Cache | ElastiCache | Memorystore |

## 🔧 HOW

### Example 1: SQL Database Auto-Failover Group

```bash
# Create primary SQL Server
az sql server create \
    --name primary-sql \
    --resource-group db-rg \
    --location eastus \
    --admin-user sqladmin \
    --admin-password "SecurePass123!"

# Create database
az sql db create \
    --name mydb \
    --server primary-sql \
    --resource-group db-rg \
    --service-tier GeneralPurpose \
    --capacity 2

# Create secondary server in another region
az sql server create \
    --name secondary-sql \
    --resource-group db-rg \
    --location westus2 \
    --admin-user sqladmin \
    --admin-password "SecurePass123!"

# Create failover group
az sql failover-group create \
    --name my-failover-group \
    --resource-group db-rg \
    --server primary-sql \
    --partner-server secondary-sql \
    --databases mydb \
    --failover-policy Automatic \
    --grace-period 1

# Test failover
az sql failover-group set-primary \
    --name my-failover-group \
    --resource-group db-rg \
    --server primary-sql
```

### Example 2: Cosmos DB with Multi-region Write

```bash
# Create Cosmos DB account
az cosmosdb create \
    --name my-cosmos \
    --resource-group db-rg \
    --locations eastus=0 westus2=1 \
    --default-consistency-level Strong \
    --enable-multiple-write-locations true

# Create database and container
az cosmosdb sql database create \
    --name mydb \
    --account-name my-cosmos \
    --resource-group db-rg

az cosmosdb sql container create \
    --name mycontainer \
    --account-name my-cosmos \
    --database-name mydb \
    --partition-key-path "/id" \
    --throughput 1000

# Add region
az cosmosdb update \
    --name my-cosmos \
    --resource-group db-rg \
    --locations eastus=0 westus2=1 centralus=2

# Change throughput
az cosmosdb sql container throughput update \
    --name mycontainer \
    --account-name my-cosmos \
    --database-name mydb \
    --throughput 2000
```

### Example 3: PostgreSQL Flexible Server HA

```bash
# Create PostgreSQL with HA
az postgres flexible-server create \
    --name my-postgres \
    --resource-group db-rg \
    --location eastus \
    --sku-name Standard_D4s_v3 \
    --tier GeneralPurpose \
    --storage-size 51200 \
    --high-availability ZoneRedundant \
    --admin-user pgadmin \
    --admin-password "SecurePass123!"

# Create read replica
az postgres flexible-server replica create \
    --name my-postgres-replica \
    --resource-group db-rg \
    --source-server my-postgres \
    --location westus2

# Configure backup
az postgres flexible-server update \
    --name my-postgres \
    --resource-group db-rg \
    --backup-retention 35 \
    --geo-redundant-backup Enabled

# Enable auto-scaling
az postgres flexible-server parameter set \
    --name max_connections \
    --resource-group db-rg \
    --server-name my-postgres \
    --value 200
```

### Example 4: Redis Cache Enterprise

```bash
# Create Redis Cache Enterprise
az redis create \
    --name my-redis \
    --resource-group db-rg \
    --location eastus \
    --sku Enterprise \
    --vm-size c5 \
    --replicas-per-primary 3 \
    --zones 1 2 3

# Enable clustering
az redis update \
    --name my-redis \
    --resource-group db-rg \
    --shard-count 3

# Configure data persistence
az redis update \
    --name my-redis \
    --resource-group db-rg \
    --rdb-enabled true \
    --rdb-storage-connection-string "DefaultEndpointsProtocol=https;AccountName=mystorage;AccountKey=xxx;EndpointSuffix=core.windows.net"

# Enable georeplication
az redis create \
    --name my-redis-replica \
    --resource-group db-rg \
    --location westus2 \
    --sku Enterprise \
    --vm-size c5 \
    --linked-server my-redis
```

## ⚠️ COMMON ISSUES

### SQL Database Issues

- **DTU exhaustion**: Upgrade service tier
- **Connection timeouts**: Enable retry logic
- **Geo-failover delays**: Check network latency

### Cosmos DB Issues

- **RU exhaustion**: Increase throughput
- **Partition hot spots**: Optimize partition key
- **Consistency issues**: Use session consistency

### Performance Issues

- **High CPU**: Upgrade compute
- **IOPS limits**: Use Premium tier
- **Memory pressure**: Use larger tier

## 🏃 PERFORMANCE

### SQL Database Optimization

| Optimization | Impact |
|-------------|--------|
| Columnstore indexes | 10x compression |
| In-memory OLTP | 5x faster |
| Auto-tuning | Automatic optimization |
| Query Store | Performance insights |

### Cosmos DB Optimization

| Strategy | Impact |
|----------|--------|
| Partition key | Throughput distribution |
| Cost optimization | Serverless for variable |
| Analytical store | OLAP workloads |

## 🌐 COMPATIBILITY

### SQL Database

- SSMS, Azure Data Studio
- Entity Framework
- ADO.NET, JDBC, ODBC
- SQLCMD

### Cosmos DB

- SDKs (.NET, Java, Node, Python)
- REST API
- MongoDB Driver
- Cassandra Driver

## 🔗 CROSS-REFERENCES

### Related Services

- **Azure Storage**: Backup storage
- **Azure Key Vault**: Secrets
- **Azure Monitor**: Metrics
- **Azure AD**: Authentication

### Related Concepts

- **Synapse Analytics**: Data warehouse
- **Data Factory**: ETL
- **SQL Managed Instance**: IaaS migration

## ✅ EXAM TIPS

### HA Options

- **SQL Database**: Failover groups for automatic
- **Cosmos DB**: Multi-region with RA
- **PostgreSQL/MySQL**: Zone-redundant HA

### Cost Optimization

- Serverless for development
- DTU-basic for small workloads
- Reserved capacity for production

### Data Protection

- LTR for compliance
- TDE for encryption
- Auditing for compliance