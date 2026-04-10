---
Category: Azure Certification
Subcategory: Azure Core Services
Concept: Azure Databases
Purpose: Understanding Azure SQL Database, Cosmos DB, and database services
Difficulty: beginner
Prerequisites: 01_Basic_Azure_Core.md
RelatedFiles: 02_Advanced_Azure_Databases.md, 03_Practical_Azure_Databases.md
UseCase: Deploying managed databases in Azure
CertificationExam: AZ-900 Azure Fundamentals
LastUpdated: 2025
---

## 💡 WHY

Azure provides managed database services that reduce operational overhead. Understanding database options helps choose the right solution.

## 📖 WHAT

### Azure Database Services

| Service | Type | Description |
|---------|------|-------------|
| Azure SQL | Relational | Managed SQL Server |
| Cosmos DB | NoSQL | Globally distributed |
| Azure DB for MySQL | Relational | Managed MySQL |
| Azure DB for PostgreSQL | Relational | Managed PostgreSQL |
| Azure Database for MariaDB | Relational | Managed MariaDB |
| SQL Managed Instance | Relational | SQL Server VM alternative |

## 🔧 HOW

### Example 1: Azure SQL

```bash
# Create SQL Database
az sql db create \
    --name mydb \
    --resource-group myrg \
    --server myserver \
    --service-tier Basic \
    --capacity 5

# Get connection string
az sql db show-connection-string \
    --name mydb \
    --server myserver \
    --client ado.net
```

### Example 2: Cosmos DB

```bash
# Create Cosmos DB account
az cosmosdb create \
    --name mycosmosdb \
    --resource-group myrg \
    --locations regionName=eastus failoverLocation=westus

# Create database
az cosmosdb sql database create \
    --account-name mycosmosdb \
    --name mydb

# Create container
az cosmosdb sql container create \
    --account-name mycosmosdb \
    --database-name mydb \
    --name mycontainer \
    --partition-key-path "/id"
```

## ✅ EXAM TIPS

- SQL Database = PaaS SQL Server
- Cosmos DB = globally distributed NoSQL
- Managed instances for lift-and-shift