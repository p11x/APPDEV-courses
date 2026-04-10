---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Databases
Purpose: Practical hands-on labs for Azure database deployment and management
Difficulty: intermediate
Prerequisites: 01_Basic_Azure_Databases.md, 02_Advanced_Azure_Databases.md
RelatedFiles: 01_Basic_Azure_Databases.md, 02_Advanced_Azure_Databases.md
UseCase: Deploying and managing production databases
CertificationExam: AZ-104 Azure Administrator
LastUpdated: 2025
---

## 💡 WHY

Hands-on labs demonstrate real-world database deployment including high availability, geo-replication, and performance optimization for production workloads.

## 📖 WHAT

### Lab Overview

Deploy enterprise databases with failover, multi-region replication, and caching layers.

### Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    Database Architecture                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────────────────┐ │
│  │ Azure SQL    │  │ Cosmos DB   │  │ PostgreSQL/MySQL        │ │
│  │ Failover     │  │ Multi-Region│  │ Flexible Server         │ │
│  └──────────────┘  └──────────────┘  └──────────────────────────┘ │
│  ┌──────────────────────────────────────────┐                   │
│  │ Azure Cache for Redis                     │                   │
│  └──────────────────────────────────────────┘                   │
└─────────────────────────────────────────────────────────────────┘
```

## 🔧 HOW

### Module 1: Azure SQL Database with Failover Group

```bash
# Create resource group
az group create --name database-rg --location eastus

# Create primary server
az sql server create \
    --name sql-primary --resource-group database-rg \
    --location eastus --admin-user sqladmin --admin-password "P@ssw0rd123!"

# Create firewall rule
az sql server firewall-rule create \
    --name AllowAzureServices --server sql-primary \
    --resource-group database-rg --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# Create SQL Database
az sql db create --name productiondb --resource-group database-rg \
    --server sql-primary --service-tier GeneralPurpose --capacity 2 --max-size 2GB

# Create secondary server
az sql server create --name sql-secondary --resource-group database-rg \
    --location westus2 --admin-user sqladmin --admin-password "P@ssw0rd123!"

# Create failover group
az sql failover-group create --name fg-prod --resource-group database-rg \
    --server sql-primary --partner-server sql-secondary --failover-policy Automatic

# Add database to failover group
az sql failover-group update --name fg-prod --resource-group database-rg \
    --server sql-primary --add-db productiondb
```

### Module 2: Cosmos DB with Multi-Region

```bash
# Create Cosmos DB account with multi-region
az cosmosdb create --name cosmos-prod --resource-group database-rg \
    --locations region=eastus failoverpriority=0 iszoneprimary=true \
    --locations region=westus2 failoverpriority=1 iszoneprimary=true \
    --locations region=northeurope failoverpriority=2 iszoneprimary=false \
    --default-consistency-level Strong --enable-automatic-failover true \
    --kind GlobalDocumentDatabase

# Create database and container
az cosmosdb sql database create --name ordersdb --resource-group database-rg \
    --account-name cosmos-prod

az cosmosdb sql container create --name customers --resource-group database-rg \
    --account-name cosmos-prod --database-name ordersdb \
    --partition-key-path "/customerId" --throughput 400

az cosmosdb sql container create --name orders --resource-group database-rg \
    --account-name cosmos-prod --database-name ordersdb \
    --partition-key-path "/orderId" --throughput 400

# Enable autoscale
az cosmosdb sql container throughput update --resource-group database-rg \
    --account-name cosmos-prod --database-name ordersdb --container-name customers \
    --autoscale-max-throughput 1000

# Get keys
az cosmosdb keys list --name cosmos-prod --resource-group database-rg --type primary
```

### Module 3: Azure Database for PostgreSQL Flexible Server

```bash
# Create PostgreSQL flexible server
az postgres flexible-server create --name pg-prod --resource-group database-rg \
    --location eastus --sku-name Standard_B1s --tier Burstable \
    --storage-size 5120 --admin-user pgadmin --admin-password "P@ssw0rd123!" \
    --public-access Enabled

# Create database and firewall
az postgres flexible-server db create --name appdb --resource-group database-rg \
    --server-name pg-prod

az postgres flexible-server firewall-rule create --name AllowAll --resource-group database-rg \
    --server-name pg-prod --start-ip-address 0.0.0.0 --end-ip-address 255.255.255.255

# Enable geo-redundant backup
az postgres flexible-server update --name pg-prod --resource-group database-rg \
    --backup-retention 7 --geo-redundant-backup Enabled

# Set parameters
az postgres flexible-server parameter set --name log_min_duration_statement \
    --resource-group database-rg --server-name pg-prod --value 1000

# Get connection string
az postgres flexible-server show-connection-string --name pg-prod \
    --resource-group database-rg --database-name appdb
```

### Module 4: Azure Database for MySQL Flexible Server

```bash
# Create MySQL flexible server
az mysql flexible-server create --name mysql-prod --resource-group database-rg \
    --location eastus --sku-name Standard_B1s --tier Burstable \
    --storage-size 5120 --admin-user mysqladmin --admin-password "P@ssw0rd123!" \
    --public-access Enabled

# Create database and firewall
az mysql flexible-server db create --name appdb --resource-group database-rg \
    --server-name mysql-prod

az mysql flexible-server firewall-rule create --name AllowAll --resource-group database-rg \
    --server-name mysql-prod --start-ip-address 0.0.0.0 --end-ip-address 255.255.255.255

# Enable geo-redundant backup
az mysql flexible-server update --name mysql-prod --resource-group database-rg \
    --backup-retention 7 --geo-redundant-backup Enabled

# Set parameters
az mysql flexible-server parameter set --name max_connections \
    --resource-group database-rg --server-name mysql-prod --value 250

# Get connection string
az mysql flexible-server show-connection-string --name mysql-prod \
    --resource-group database-rg --database-name appdb
```

### Module 5: Azure Cache for Redis

```bash
# Create Redis Cache (Basic)
az redis create --name redis-prod --resource-group database-rg \
    --location eastus --sku Basic --vm-size c0 --enable-non-ssl-port false

# Create Premium cache with clustering
az redis create --name redis-cluster --resource-group database-rg \
    --location eastus --sku Premium --vm-size p1 --replicas-per-primary 2 --shard-count 3

# Update cache settings
az redis update --name redis-prod --resource-group database-rg \
    --maxmemory-policy allkeys-lru --maxmemory-reserved 2048

# Firewall rules and get keys
az redis firewall-rule create --name AllowAll --redis-name redis-prod \
    --resource-group database-rg --start-ip 0.0.0.0 --end-ip 255.255.255.255

az redis list-keys --name redis-prod --resource-group database-rg

az redis show-connection-string --name redis-prod --resource-group database-rg

# Reboot if needed
az redis reboot --name redis-prod --resource-group database-rg --reboot-type all
```

## ⚠️ TROUBLESHOOTING

### Common Issues

```bash
# Check SQL Database status
az sql db show --name productiondb --resource-group database-rg --server sql-primary

# Check failover group status
az sql failover-group show --name fg-prod --resource-group database-rg --server sql-primary

# Check Cosmos DB account
az cosmosdb show --name cosmos-prod --resource-group database-rg

# Check PostgreSQL server metrics
az monitor metrics list --resource pg-prod --metric cpu_percent,memory_percent,storage_percent

# Check MySQL server metrics
az monitor metrics list --resource mysql-prod --metric cpu_percent,memory_percent,storage_percent

# Check Redis Cache diagnostics
az redis show --name redis-prod --resource-group database-rg
```

### Performance Issues

```bash
# Check SQL DTU utilization
az sql db show --name productiondb --resource-group database-rg --server sql-primary

# Check Cosmos DB RU/s
az cosmosdb sql database show --name ordersdb --resource-group database-rg --account-name cosmos-prod

# Check Redis server load
az redis show --name redis-cluster --resource-group database-rg

# Get diagnostic logs
az monitor diagnostic-logs list --resource-group database-rg
```

## ✅ VERIFICATION

### Test SQL Database Connection

```bash
# Test connection via firewall rule
az sql server show --name sql-primary --resource-group database-rg

# Query database
az sql db show --name productiondb --resource-group database-rg --server sql-primary

# Check failover status
az sql failover-group list --resource-group database-rg --server sql-primary
```

### Test Cosmos DB

```bash
# List databases
az cosmosdb sql database list --resource-group database-rg --account-name cosmos-prod

# List containers
az cosmosdb sql container list --resource-group database-rg --account-name cosmos-prod --database-name ordersdb
```

### Test PostgreSQL

```bash
# List databases
az postgres flexible-server db list --resource-group database-rg --server-name pg-prod

# Check server status
az postgres flexible-server show --name pg-prod --resource-group database-rg
```

### Test MySQL

```bash
# List databases
az mysql flexible-server db list --resource-group database-rg --server-name mysql-prod

# Check server status
az mysql flexible-server show --name mysql-prod --resource-group database-rg
```

### Test Redis Cache

```bash
# List keys
az redis list-keys --name redis-prod --resource-group database-rg

# Check cache info
az redis show --name redis-prod --resource-group database-rg
```

## 🧹 CLEANUP

```bash
# Delete Redis caches
az redis delete --name redis-prod --resource-group database-rg --yes
az redis delete --name redis-cluster --resource-group database-rg --yes

# Delete MySQL server
az mysql flexible-server delete --name mysql-prod --resource-group database-rg --yes

# Delete PostgreSQL server
az postgres flexible-server delete --name pg-prod --resource-group database-rg --yes

# Delete Cosmos DB account
az cosmosdb delete --name cosmos-prod --resource-group database-rg

# Delete failover group
az sql failover-group delete --name fg-prod --resource-group database-rg --server sql-primary

# Delete SQL databases
az sql db delete --name productiondb --resource-group database-rg --server sql-primary --yes

# Delete SQL servers
az sql server delete --name sql-primary --resource-group database-rg --yes
az sql server delete --name sql-secondary --resource-group database-rg --yes

# Delete resource group
az group delete --name database-rg --yes
```

## 🔗 CROSS-REFERENCES

### Related Labs

- Basic Azure Databases: SQL Database fundamentals
- Advanced Azure Databases: Geo-replication and scaling
- Azure Storage: Backup storage integration
- Azure Key Vault: Database credential management

### Azure Services

- Azure Monitor: Database metrics and alerting
- Azure Backup: Backup and recovery
- Azure Private Link: Secure database access
- Azure Virtual Network: Network isolation

### Exam Topics

- AZ-104: Manage database resources
- AZ-104: Configure geo-replication
- AZ-104: Implement caching strategies
- AZ-104: Configure high availability