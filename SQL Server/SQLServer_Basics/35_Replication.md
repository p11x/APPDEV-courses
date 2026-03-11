# SQL Server Replication

## What is Replication?

**Replication** copies and distributes data from one database to another, enabling multiple servers to share the same data for improved availability and performance.

## Why Use Replication?

- **High Availability**: Read replicas for reporting
- **Load Balancing**: Distribute queries across servers
- **Disaster Recovery**: Off-site data copies
- **Data Distribution**: Multiple office locations

## Types of Replication

### 1. Snapshot Replication

Copies entire database at scheduled intervals:

```
┌──────────────┐     Full Copy     ┌──────────────┐
│  Publisher   │ ───────────────►  │  Subscriber  │
│  (Source)    │    (Snapshot)     │  (Replica)   │
└──────────────┘                   └──────────────┘
```

### 2. Transactional Replication

Copies transactions in near real-time:

```
┌──────────────┐    Transactions    ┌──────────────┐
│  Publisher   │ ───────────────►   │  Subscriber  │
│  (Source)    │   (Continuous)      │  (Replica)   │
└──────────────┘                    └──────────────┘
```

### 3. Merge Replication

Allows changes at multiple sites:

```
┌──────────┐                      ┌──────────┐
│ Site A   │ ◄─── Merge ───────► │ Site B   │
│ (Pub/Sub)│                     │ (Pub/Sub)│
└──────────┘                     └──────────┘
```

## Replication Components

| Component | Role |
|-----------|------|
| Publisher | Source database |
| Distributor | Manages distribution |
| Subscriber | Receives data |
| Article | Table/article to replicate |
| Publication | Collection of articles |

## Setting Up Snapshot Replication

### Step 1: Configure Distributor

```sql
-- Enable publisher with local distributor
USE master;
EXEC sp_adddistributor 
    @distributor = @@SERVERNAME, 
    @password = 'DistPass2024!';

-- Create distribution database
EXEC sp_adddistributiondb 
    @database = 'distribution',
    @data_folder = 'C:\Data',
    @log_folder = 'C:\Logs';
```

### Step 2: Create Publication

```sql
-- Enable database for replication
USE MyDatabase;
EXEC sp_replicationdboption 
    @dbname = 'MyDatabase', 
    @optname = 'publish', 
    @value = 'true';

-- Create snapshot publication
EXEC sp_addpublication 
    @publication = 'MySnapshotPub',
    @sync_method = 'native',
    @repl_freq = 'snapshot',
    @status = 'active';

-- Add article (table)
EXEC sp_addarticle 
    @publication = 'MySnapshotPub',
    @article = 'Customers',
    @source_table = 'Customers';
```

### Step 3: Create Subscription

```sql
-- Create pull subscription
EXEC sp_addsubscription 
    @publication = 'MySnapshotPub',
    @subscriber = 'SubscriberServer',
    @destination_db = 'MyDatabase_Copy',
    @subscription_type = 'pull';

-- Create subscription job
EXEC sp_addpushsubscription_agent 
    @publication = 'MySnapshotPub',
    @subscriber = 'SubscriberServer',
    @subscriber_db = 'MyDatabase_Copy',
    @subscriber_security_mode = 0,
    @subscriber_login = 'sa',
    @subscriber_password = 'Password123';
```

## Setting Up Transactional Replication

```sql
-- Create transactional publication
USE MyDatabase;
EXEC sp_addpublication 
    @publication = 'MyTransPub',
    @allow_push = 'true',
    @allow_pull = 'true',
    @repl_freq = 'continuous';

-- Add article with primary key
EXEC sp_addarticle 
    @publication = 'MyTransPub',
    @article = 'Orders',
    @source_table = 'Orders',
    @type = 'logbased',
    @ins_cmd = 'CALL sp_MSins_Orders',
    @del_cmd = 'CALL sp_MSdel_Orders',
    @upd_cmd = 'SCALL sp_MSupd_Orders';
```

## Monitoring Replication

```sql
-- Check replication status
EXEC sp_helppublication;

-- Check subscribers
EXEC sp_helpsubscription;

-- Monitor sync status
EXEC sp_replmonitorhelppublication;
EXEC sp_replmonitorhelpsubscription;

-- View replication agents
EXEC sp_replhelpagent;
```

## Conflict Resolution (Merge Replication)

| Type | Description |
|------|-------------|
| Default | Publisher wins |
| Subscriber Wins | Subscriber changes kept |
| Interactive | Manual resolution |

```sql
-- Set conflict resolver
EXEC sp_mergesubscriptionarticle 
    @publication = 'MyMergePub',
    @article = 'Customers',
    @resolver_clsid = NULL,  -- Use default
    @conflict_logging = 'publisher wins';
```

## Replication Best Practices

| Practice | Description |
|----------|-------------|
| Primary Keys | All replicated tables need PK |
| Initial Sync | Plan for initial data transfer |
| Latency | Monitor for delays |
| Filtering | Use row filters to reduce data |

## Removing Replication

```sql
-- Drop subscription
EXEC sp_dropsubscription 
    @publication = 'MyTransPub',
    @subscriber = 'SubscriberServer';

-- Drop publication
EXEC sp_droppublication 
    @publication = 'MyTransPub';

-- Remove distributor
EXEC sp_dropdistributor 
    @no_checks = 1;
```

## Key Points Summary

| Type | Use Case | Latency |
|------|----------|---------|
| Snapshot | Infrequent changes | Hours |
| Transactional | Near real-time | Minutes |
| Merge | Multiple sites | Hours |

---

*This topic should take about 5-7 minutes to explain in class.*
