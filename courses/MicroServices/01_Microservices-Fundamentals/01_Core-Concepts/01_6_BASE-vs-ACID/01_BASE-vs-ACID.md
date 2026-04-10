# BASE vs ACID: Understanding Consistency Models in Microservices

## Overview

In distributed systems and microservices architectures, choosing the right consistency model is crucial for system design and data management. This document explores the fundamental differences between ACID (traditional relational database properties) and BASE (modern distributed system properties) consistency models.

---

## 1. ACID Properties

ACID is an acronym representing four key properties that ensure reliable database transactions:

### 1.1 Atomicity

**Definition**: A transaction is atomic if it either completes in its entirety or fails completely. No partial execution is visible to the system.

**Explanation**: 
- All operations within a transaction are treated as a single logical unit
- If any operation fails, the entire transaction rolls back
- Either ALL changes are applied, or NONE are applied

**Example**:
```sql
--Bank transfer: money should either leave account A completely or not at all
BEGIN TRANSACTION;
    UPDATE accounts SET balance = balance - 100 WHERE account_id = 'A';
    UPDATE accounts SET balance = balance + 100 WHERE account_id = 'B';
COMMIT;
-- If either update fails, both are rolled back
```

### 1.2 Consistency

**Definition**: A transaction moves the database from one valid state to another valid state.

**Explanation**:
- All database constraints (unique keys, foreign keys, triggers) must be satisfied
- Data integrity rules are enforced before and after each transaction
- No orphan records or constraint violations allowed

**Example**:
```sql
-- This transaction maintains consistency
-- Foreign key constraint ensures valid reference
INSERT INTO orders (customer_id, product_id, quantity)
VALUES (1, 100, 5);
-- customer_id 1 must exist in customers table
-- product_id 100 must exist in products table
```

### 1.3 Isolation

**Definition**: Concurrent transactions appear to execute serially, without interfering with each other.

**Explanation**:
- Transactions execute independently
- Intermediate results are not visible to other transactions
- Prevents race conditions and dirty reads

**Isolation Levels**:
| Level | Dirty Reads | Non-Repeatable Reads | Phantom Reads |
|-------|-------------|---------------------|---------------|
| READ UNCOMMITTED | Allowed | Allowed | Allowed |
| READ COMMITTED | Prevented | Allowed | Allowed |
| REPEATABLE READ | Prevented | Prevented | Allowed |
| SERIALIZABLE | Prevented | Prevented | Prevented |

### 1.4 Durability

**Definition**: Once committed, data persists even through system failures.

**Explanation**:
- Committed data is written to permanent storage
- Survives crashes, power failures, or system restarts
- Typically achieved through write-ahead logging or disk writes

**Example**:
```java
// Durability in action - data persists after commit
@Transactional
public void saveOrder(Order order) {
    orderRepository.save(order);
    // Data is physically written to disk
    // Persists even if application crashes immediately after
}
```

---

## 2. BASE Properties

BASE is the complementary approach to ACID for distributed systems, famously associated with NoSQL databases:

### 2.1 Basically Available (BA)

**Definition**: The system guarantees availability of data despite failures.

**Explanation**:
- System responds to requests even during partial failures
- Data may be available from multiple nodes
- Some replicas may be down, but the system continues to serve requests
- Read/write operations succeed on available nodes

**Example**:
```yaml
# Cassandra configuration for basically available
# DataCenter1: 3 nodes, DataCenter2: 3 nodes
# Replication factor: 2 in each datacenter
datacenter_settings:
  dc1:
    nodes: [10.0.0.1, 10.0.0.2, 10.0.0.3]
    replication_factor: 2
  dc2:
    nodes: [10.0.1.1, 10.0.1.2, 10.0.1.3]
    replication_factor: 2
# If 2 nodes in DC1 fail, read/write still works via DC2
```

### 2.2 Soft State (S)

**Definition**: The state of the system may change over time, even without input.

**Explanation**:
- Data can change due to eventual consistency mechanisms
- State is not guaranteed to be consistent at all times
- Replicas may have different data during synchronization
- Internal processes reconcile differences

**Example**:
```typescript
// Soft state - data can change without direct updates
interface SoftStateEntity {
  id: string;
  version: number;
  lastModified: Date;
  // Status may change due to background sync
  syncStatus: 'synced' | 'pending' | 'conflict';
}

// Entity can be in "pending" state while sync occurs
const order: SoftStateEntity = {
  id: "order-123",
  version: 1,
  lastModified: new Date(),
  syncStatus: 'pending' // awaiting replication
};
```

### 2.3 Eventual Consistency (E)

**Definition**: Given no new updates, all replicas will eventually become consistent.

**Explanation**:
- System will converge to a consistent state
- No guarantee of immediate consistency
- Conflict resolution handles divergence
- Time-based convergence (bounded or unbounded)

**Example**:
```java
// Eventual consistency in DynamoDB
@DynamoDBTable(tableName = "products")
public class Product {
    private String productId;
    private String name;
    private int quantity;
    
    @DynamoDBVersionAttribute
    private Long version; // Optimistic locking for conflict resolution
    
    public void updateQuantity(int newQty) {
        // If versions match, update succeeds
        // If versions differ, conflict resolution triggered
        this.quantity = newQty;
    }
}
```

---

## 3. ACID vs BASE Comparison

### 3.1 Fundamental Differences

| Aspect | ACID | BASE |
|--------|-----|------|
| **Availability** |牺牲可用性保证一致性 | 牺牲一致性保证可用性 |
| **Model** |Strong consistency | Eventual consistency |
| **Latency** | Higher, synchronous | Lower, asynchronous |
| **Scalability** | Limited (vertical) | High (horizontal) |
| **Transactions** | Distributed locks, 2PC | Optimistic replication |
| **Failure Handling** | Stop/slow down | Continue operations |
| **Data Model** | Normalized relations | Denormalized documents |
| **Use Cases** | Financial, inventory | Social media, analytics |

### 3.2 CAP Theorem Perspective

```
┌─────────────────────────────────────────────────────────┐
│                    CAP THEOREM                           │
│                                                         │
│        Consistency  ─────────  Availability              │
│              ○                                         │
│             /|\                                        │
│            / | \                                       │
│           /  |  \                                      │
│          /   |   \                                     │
│         /    |    \                                    │
│        /     │     \                                   │
│       /      │      \                                  │
│      /       │       \                                │
│     /        │        \                               │
│    /    Partition Tolerance                           │
│   ○─���────────○──────────○                              │
│                                                         │
│  CA - Traditional RDBMS                                │
│  AP - Dynamo, Cassandra                                │
│  CP - Bigtable, HBase                                  │
└─────────────────────────────────────────────────────────┘
```

### 3.3 Decision Matrix

| Requirement | Choose ACID | Choose BASE |
|--------------|-------------|-------------|
| Financial transactions | ✓ | |
| Real-time inventory | ✓ | |
| Shopping carts | | ✓ |
| User sessions | | ✓ |
| Activity logs | | ✓ |
| Order management | ✓ (with Saga) | |
| Social media feeds | | ✓ |
| IoT sensor data | | ✓ |

---

## 4. Partition Tolerance and BASE

### 4.1 The Partition Problem

Network partitions are inevitable in distributed systems:

```
┌────────────────────┐      ┌────────────────────┐
│    Data Center A  │      │   Data Center B    │
│                   │      │                    │
│  [Node 1]───X────│──────│────[Node 3]        │
│  [Node 2]         │      │      [Node 4]      │
│                   │      │                    │
└────────────────────┘      └────────────────────┘
         Network Partition Occurred
```

### 4.2 How BASE Addresses Partitions

BASE systems handle partitions by:

1. **Continuing Operations**: Serve reads/writes despite partition
2. **Deferred Updates**: Queue operations for later reconciliation
3. **Conflict Resolution**: Merge divergent states later
4. **Vector Clocks**: Track causality between updates

```java
// Cassandra handles partitions by continuing operations
// Write to available nodes, queue for others
 CassandraDaemon.getInstance().init(
     Config.fromFile("cassandra.yaml")
         // Continue serving during partition
         .set("commitlog_sync", "batch")
         // Fail fast disabled - queue writes
         .set("fail_fast", false)
 );
```

### 4.3 Conflict Resolution Strategies

| Strategy | Description | Use Case |
|----------|-------------|----------|
| **Last Write Wins (LWW)** | Timestamp-based | High-throughput, low-conflict |
| **Vector Clocks** | Causality tracking | Collaborative apps |
| **CRDTs** | Commutative operations | Distributed counters, sets |
| **Application-defined** | Custom merge logic | Complex business rules |

```javascript
// CRDT - Counter that works across partitions
class GCounter {
  constructor(nodeId) {
    this.nodeId = nodeId;
    this.counts = {};
  }
  
  increment() {
    this.counts[this.nodeId] = (this.counts[this.nodeId] || 0) + 1;
  }
  
  merge(other) {
    // Merge takes maximum of each node's count
    for (let node in other.counts) {
      this.counts[node] = Math.max(
        this.counts[node] || 0,
        other.counts[node]
      );
    }
  }
  
  value() {
    return Object.values(this.counts).reduce((a, b) => a + b, 0);
  }
}
```

---

## 5. Eventual Consistency Models

### 5.1 Consistency Levels

```
Timeline:  0ms    100ms   500ms    1s      5s      30s
          │      │      │        │       │       │
Eventual ─┼──────┼──────┼────────┼───────┼───────┼──→ Consistent
Causal   ─┼──────┼──────┼────────┼───────│───────│──→ 
Read My  ─┼──────┼────���─���────────┼───────│───────│──→ 
Writes   
Monotonic ─┼──────┼──────────────┼──────────────┼───→ → Consistent
          └───────────────────────────────────────────
                   Eventual Consistency Spectrum
```

### 5.2 Model Definitions

#### Eventual Consistency
- No guarantees about order
- Updates may propagate arbitrarily
- Convergence eventually

#### Causal Consistency
- Respects causality
- If A causes B, B happens after A
- More intuitive ordering

#### Read My Writes (RYW)
- Read your own writes immediately
- Session-based guarantee
- Common in mobile apps

#### Monotonic Read
- Once seen, always seen
- No "going back in time"
- Improves user experience

### 5.3 Configuration Examples

```yaml
# DynamoDB eventual consistency configuration
dynamodb:
  tables:
    users:
      billing_mode: PAY_PER_REQUEST
      stream_specification:
        stream_new_image: true
      # Eventual consistency (default for reads)
      read_capacity_units: 5
      
products:
      # Strong consistency available but costlier
      reads:
        mode: EVENTUAL  # or STRONG
       consistent_reads: false  # faster, cheaper
        
# Cassandra tunable consistency
cassandra:
  consistency_levels:
    # Writes acknowledged by 2 replicas
    write: QUORUM     # (RF/2) + 1
    # Read from 2 replicas
    read: QUORUM
    
    # Or configure per-operation
    # write: ONE, TWO, QUORUM, ALL
    # read:  ONE, TWO, QUORUM, ALL
```

```typescript
// MongoDB read preference configuration
const connection = await MongoClient.connect(uri, {
  readPreference: 'secondaryPreferred', // Read from secondary if available
  // Options: primary, primaryPreferred, 
  //         secondary, secondaryPreferred, nearest
  w: 'majority',        // Write acknowledged by majority
  journal: true,       // Write to journal before ack
  wtimeoutMS: 5000     // Timeout for write concern
});

// Using tags for targeted reads
const collection = db.collection('products', {
  readPreference: new TagSet([
    { zone: 'us-east-1' },
    { replica: 'priority' }
  ])
});
```

---

## 6. Strong vs Eventual Consistency

### 6.1 Trade-offs

| Factor | Strong Consistency | Eventual Consistency |
|--------|-------------------|---------------------|
| **Latency** | High (synchronous) | Low (asynchronous) |
| **Availability** | Lower (quorum) | Higher (any node) |
| **Throughput** | Lower | Higher |
| **Complexity** | Lower | Higher |
| **UX** | Predictable | May show stale data |
| **Debugging** | Easier | Harder |

### 6.2 When to Use Strong Consistency

- Financial transactions (debits/credits)
- Inventory management (prevent overselling)
- Authentication/authorization
- Order processing
- Legal/regulatory requirements

### 6.3 When to Use Eventual Consistency

- Social media posts/likes
- User profile updates
- Cached data with TTL
- Analytics and metrics
- Recommendation systems
- Activity logs

---

## 7. Practical Implications for Microservices

### 7.1 Architecture Considerations

```
┌─────────────────────────────────────────────────────────────┐
│                    MICROSERVICE ARCHITECTURE                  │
│                                                             │
│   ┌─────────┐    ┌─────────┐    ┌─────────┐                 │
│   │ User    │    │ Order   │    │Product  │                 │
│   │ Service │    │ Service │    │ Service │                 │
│   └────┬────┘    └────┬────┘    └────┬────┘                 │
│        │               │               │                      │
│   ACID │            ACID │         BASE │                      │
│   (SQL)│           (SQL) │        (NoSQL)                    │
│        │               │               │                      │
│        └───────────────┬┴───────────────┘                      │
│                       │                                      │
│              ┌────────┴────────┐                            │
│              │   API Gateway   │                             │
│              └────────────────┘                            │
└─────────────────────────────────────────────────────────────┘
```

### 7.2 Database Selection by Service

```typescript
// Example: Multi-database setup in microservices
interface ServiceDatabaseMapping {
  userService: {
    database: 'PostgreSQL',
    rationale: 'ACID needed for auth, session data',
    consistency: 'strong'
  };
  
  orderService: {
    database: 'PostgreSQL',
    rationale: 'ACID for transactions, inventory',
    consistency: 'strong'
  };
  
  catalogService: {
    database: 'Elasticsearch',
    rationale: 'Search, filtering, availability',
    consistency: 'eventual'
  };
  
  recommendationService: {
    database: 'Cassandra',
    rationale: 'High throughput, analytics',
    consistency: 'eventual'
  };
  
  notificationService: {
    database: 'Redis',
    rationale: 'Pub/sub, real-time delivery',
    consistency: 'eventual'
  };
}
```

### 7.3 Data Synchronization Patterns

```typescript
// Pattern 1: Change Data Capture (CDC)
interface CDCConfiguration {
  source: 'PostgreSQL',
  connector: 'Debezium',
  destination: 'Kafka',
  format: 'JSON',
  // Transformsbinlog to meaningful events
  transforms: [
    { type: 'unwrap', field: 'after' },
    { type: 'extract', field: 'ts', dateFormat: 'iso' }
  ]
}

// Pattern 2: Dual Write Problem
// Problem: Writing to multiple DBs atomically is hard
// Solution: Event sourcing or Saga

// Pattern 3: Event Sourcing
interface EventSourcingStore {
  eventStore: 'EventStoreDB',
  projections: 'MongoDB',
  // Every state change is an event
  aggregateId: 'order-123',
  events: [
    { type: 'OrderCreated', data: {...} },
    { type: 'ItemAdded', data: {...} },
    { type: 'OrderPlaced', data: {...} }
  ]
}
```

---

## 8. ACID vs BASE Database Selection

### 8.1 Decision Criteria

```
┌─────────────────────────────────────────────────────────────┐
│                 DATABASE SELECTION FLOWCHART                │
│                                                             │
│                    ┌─────────────────┐                     │
│                    │  Start Here     │                     │
│                    └────────┬────────┘                     │
│                             │                               │
│                             ▼                               │
│                    ┌─────────────────┐                     │
│         ┌─────────│ Need Strong    │─────────┐            │
│         │         │ Consistency?   │         │            │
│         │         └─────────────────┘         │            │
│         │                │                  │            │
│        Yes               │                 No             │
│         │                │                  │            │
│         ▼                ▼                  ▼            │
│  ┌─────────────┐   ┌─────────────┐   ┌─────────────┐      │
│  │  Financial  │   │   Complex   │   │ Need High   │──────┤
│  │ Transaction │   │   Queries?  │   │ Throughput? │      │
│  │    Data?    │   └─────────────┘   └─────────────┘      │
│  └─────────────┘          │                │              │
│       │                   │               │              │
│      Yes                  Yes             No              │
│       │                   │               │              │
│       ▼                   ▼               ▼              │
│  ┌──────────┐      ┌───────────┐   ┌───────────┐        │
│  │ PostgreSQL│     │ PostgreSQL│   │ Cassandra │        │
│  │ MySQL     │     │ MongoDB   │   │ DynamoDB  │        │
│  │ Oracle   │     │           │   │Riak       │        │
│  └──────────┘     └───────────┘   └───────────┘        │
└─────────────────────────────────────────────────────────────┘
```

### 8.2 Database Examples by Consistency Model

#### ACID Databases
- **PostgreSQL**: Enterprise-grade, full ACID support
- **MySQL**: Popular, reliable transactions
- **Oracle**: Enterprise features, RAC
- **SQL Server**: Microsoft ecosystem

#### BASE Databases
- **Cassandra**: Wide column, tunable consistency
- **DynamoDB**: AWS managed, pay-per-request
- **MongoDB**: Document store, eventual by default  
- **Riak**: Key-value, strong AP focus
- **Couchbase**: JSON documents, N1QL

---

## 9. Transaction Models in Distributed Systems

### 9.1 Two-Phase Commit (2PC)

```
┌─────────────────────────────────────────────────────────────┐
│              TWO-PHASE COMMIT (2PC)                          │
│                                                             │
│  Coordinator          Participant 1   Participant 2        │
│       │                   │                │                │
│       │────VOTE-REQUEST──►│                │                │
│       │◄─────VOTE────────│                │                │
│       │                   │        ┌───────VOTE-REQUEST──►│
│       │                   │        │◄───────VOTE──────────│
│       │                   │        │                       │
│       │────GLOBAL COMMIT─►│                │                │
│       │◀─────ACK─────────│                │                │
│       │                   │        ┌──────GLOBAL COMMIT───►│
│       │                   │        │◀──────ACK────────────│
│       │                   │        │                       │
│  Phase 1:    Prepare/Precommit                              │
│  Phase 2:    Commit/Rollback                                 │
└─────────────────────────────────────────────────────────────┘
```

```java
// 2PC Implementation Example
public class TwoPhaseCommit {
    
    public boolean prepare(List<Participant> participants) {
        for (Participant p : participants) {
            try {
                Vote vote = p.prepare();
                if (vote == Vote.ABORT) {
                    return false;
                }
            } catch (Exception e) {
                return false;
            }
        }
        return true;
    }
    
    public void commit(List<Participant> participants) {
        for (Participant p : participants) {
            try {
                p.commit();
            } catch (Exception e) {
                // Recovery needed
            }
        }
    }
    
    public void rollback(List<Participant> participants) {
        for (Participant p : participants) {
            p.rollback();
        }
    }
}
```

### 9.2Saga Pattern

The Saga pattern manages distributed transactions through a sequence of local transactions:

```
┌─────────────────────────────────────────────────────────────┐
│                    SAGA PATTERN                              │
│                                                             │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Order Service Saga                       │   │
│  │                                                       │   │
│  │  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐  │   │
│  │  │ Create │  │ Reserve │  │ Charge  │  │ Update  │  │   │
│  │  │ Order  │─►│Inventory│─►│Payment  │─►│ Status  │  │   │
│  │  │  (T1)  │  │  (T2)   │  │  (T3)   │  │  (T4)   │  │   │
│  │  └─────────┘  └─────────┘  └─────────┘  └─────────┘  │   │
│  │      │            │            │            │         │   │
│  │      ▼            ▼            ▼            ▼         │   │
│  │  ┌─────────┐  ┌─────────┐  ┌─────────┐              │   │
│  │  │Compensate│ │Compensate│ │Compensate│              │   │
│  │  │(T1-1)   │ │(T2-1)   │ │(T3-1)   │              │   │
│  │  │         │ │         │ │         │              │   │
│  │  │DELETE   │ │RELEASE  │ │REFUND   │              │   │
│  │  │ORDER    │ │INVENTORY│ │PAYMENT  │              │   │
│  │  └─────────┘  └─────────┘  └─────────┘              │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

```typescript
// Saga Implementation Example
interface SagaStep<S, C> {
  execute(context: S, compensate: C): Promise<void>;
  compensate(context: S): Promise<void>;
}

class OrderSaga {
  private steps: SagaStep<OrderContext, any>[];
  
  async execute(context: OrderContext): Promise<void> {
    const completedSteps: SagaStep<OrderContext, any>[] = [];
    
    try {
      for (const step of this.steps) {
        await step.execute(context);
        completedSteps.push(step);
      }
    } catch (error) {
      // Compensate in reverse order
      for (const step of completedSteps.reverse()) {
        await step.compensate(context);
      }
      throw new SagaFailedException(error);
    }
  }
}

// Saga Steps Implementation
class ReserveInventoryStep implements SagaStep<OrderContext, Inventory> {
  async execute(context: OrderContext): Promise<void> {
    context.inventory = await inventoryApi.reserve(
      context.items,
      context.orderId
    );
  }
  
  async compensate(context: OrderContext): Promise<void> {
    await inventoryApi.release(context.inventory);
  }
}

class ChargePaymentStep implements SagaStep<OrderContext, Payment> {
  async execute(context: OrderContext): Promise<void> {
    context.payment = await paymentApi.charge(
      context.customerId,
      context.total
    );
  }
  
  async compensate(context: OrderContext): Promise<void> {
    await paymentApi.refund(context.payment.id);
  }
}
```

### 9.3 Saga Orchestration vs Choreography

| Aspect | Orchestration | Choreography |
|--------|---------------|--------------|
| Flow Control | Central coordinator | Distributed events |
| Coupling | Tight to orchestrator | Loose between services |
| Complexity | Lower | Higher |
| Failure Handling | Centralized | Distributed |
| Visibility | One place | Distributed |

```typescript
// Choreography-based Saga
interface EventBus {
  publish(event: Event): void;
  subscribe(handler: EventHandler): void;
}

// Each service publishes and subscribes to events
class OrderService {
  constructor(private eventBus: EventBus) {}
  
  async onOrderCreated(event: OrderCreatedEvent) {
    // Reserve inventory
    await this.eventBus.publish(new InventoryReservedEvent(
      event.orderId,
      event.items
    ));
  }
  
  onInventoryReserved(event: InventoryReservedEvent) {
    // Continue workflow
  }
}

class PaymentService {
  constructor(private eventBus: EventBus) {}
  
  onInventoryReserved(event: InventoryReservedEvent) {
    // Charge payment
  }
}
```

---

## 10. Real-World Examples

### 10.1 Amazon Dynamo

**Architecture**: 
- Key-value store with eventual consistency
- Tunable consistency (R + W > N)
- Gossip-based membership

**Configuration**:
```yaml
# DynamoDB Consistency Configuration
dynamodb:
  # N = 3 (replication factor)
  # W = 2 (write acknowledgment)
  # R = 2 (read acknowledgment)
  
  # Strong consistency: R = 3
  # Eventual consistency: R = 1
  
  table_mode: PAY_PER_REQUEST
  # Single item atomic counters
  # Client-side tracking
  # Optimistic locking with version
```

**Use Cases**:
- Shopping cart
- Session management
- Browse history

### 10.2 Apache Cassandra

**Architecture**:
- Wide column store
- Tunable consistency per query
- Gossip protocol for membership

**Configuration**:
```yaml
# Cassandra Consistency Configuration
cassandra:
  keyspace: myapp
  replication_factor: 3
  
  # Consistency levels
  # ONE: Fastest, weakest consistency
  # QUORUM: Balanced (2 of 3)
  # ALL: Strongest, slowest
  
  # Example: Write with QUORUM, read with ONE
  write_consistency: QUORUM
  read_consistency: ONE
  
  # Or configure for each operation
  # session.execute(stmt, consistency_level=ONE)
```

**CQL Examples**:
```sql
-- Eventual consistent read (default)
SELECT * FROM users WHERE user_id = '123';

-- Strong consistent read
SELECT * FROM users WHERE user_id = '123'
USING CONSISTENCY SERIAL;  -- For LWT

-- Write with quorum
INSERT INTO users (user_id, name, email)
VALUES ('123', 'John', 'john@email.com')
USING CONSISTENCY QUORUM;
```

### 10.3 Google Cloud Spanner

**Architecture**:
- Globally distributed relational database
- Strong consistency via Paxos/Raft
- TrueTime for commit wait

**Configuration**:
```java
// Cloud Spanner Configuration
DatabaseId instanceDatabaseId = 
    DatabaseId.of("my-instance", "my-database");

SpannerOptions options = SpannerOptions.newBuilder()
    .setProjectId("my-project")
    .build();

Spanner spanner = options.getService();

DatabaseClient dbClient = spanner.getDatabaseClient(
    instanceDatabaseId
);

// Strong consistency (default)
try (ReadOnlyTransaction tx = dbClient.readOnlyTransaction(
    TransactionOption.ofTimestampBound(
        // Read from replica within 10 seconds
        TransactionStampBound.minReadTimestamp(
            Instant.now().minusSeconds(10)
        )
    )
)) {
    // All reads see consistent snapshot
    ResultSet resultSet = tx.execute(
        QueryParser.parse("SELECT * FROM orders")
    );
}
```

### 10.4 Netflix Recommendations

**Architecture**:
- Cassandra for high write throughput
- Eventual consistency for ranking
- Multiple data centers

**Use Case**:
- User viewing history
- Movie recommendations
- Ratings and reviews

```java
// Netflix use case example
public class RecommendationService {
    
    // High throughput writes (eventual)
    public CompletableFuture<Void> recordViewing(
        String userId, 
        String movieId
    ) {
        return cassandraTemplate.insert(
            ViewingHistory.builder()
                .userId(userId)
                .movieId(movieId)
                .timestamp(now())
                .build()
        ).whenComplete((r, e) -> {
            // Immediate acknowledgment
            // Background sync to other DCs
        });
    }
    
    // Read for recommendations (eventual)
    public Mono<List<Recommendation>> getRecommendations(
        String userId
    ) {
        // Aggregate from multiple replicas
        return cassandraTemplate.findAll(
            ViewingHistory.class)
            .where(UserId.eq(userId))
            .orderBy(timestamp.desc())
            .limit(100);
    }
}
```

---

## 11. Code Examples

### 11.1 Configuring Eventual Consistency

```typescript
// MongoDB eventual consistency setup
import { MongoClient, Db } from 'mongodb';

async function connectWithEventualConsistency(): Promise<Db> {
  const client = new MongoClient('mongodb://localhost:27017', {
    // Read from secondary (eventual)
    readPreference: 'secondaryPreferred',
    // Write to primary (acknowledged)
    w: 'majority',             
    // Wait for journal
    journal: true,             
    // 5 second timeout
    wtimeoutMS: 5000           
  });
  
  return client.db('myapp');
}

// Query with strong consistency when needed
async function getStrongConsistency<T>(
  db: Db,
  collectionName: string,
  query: object
): Promise<T> {
  const collection = db.collection(collectionName);
  
  return collection.findOne(query, {
    // Force primary read
    readPreference: 'primary'    
  });
}
```

### 11.2 Cassandra Tunable Consistency

```java
// Cassandra Java driver configuration
import com.datastax.oss.driver.api.core.CqlSession;
import com.datastax.oss.driver.api.core.config.DefaultDriverOption;
import com.datastax.oss.driver.api.core.config.DriverConfigLoader;

public class CassandraConfig {
  
  public CqlSession createSession() {
    return CqlSession.builder()
      .withLocalDatacenter("dc1")
      .addContactPoints(ImmutableSet.of(
        InetSocketAddress.create("10.0.0.1", 9042),
        InetSocketAddress.create("10.0.0.2", 9042),
        InetSocketAddress.create("10.0.0.3", 9042)
      ))
      .withKeyspace("myapp")
      // Default consistency: QUORUM
      .withQueryConfig(QueryConfig.builder()
        .setDefaultConsistencyLevel(DefaultConsistencyLevel.QUORUM)
        .build()
      )
      .withRetryPolicy(RetryPolicy.background())
      .build();
  }
  
  // Per-query consistency override
  public void runQueries(CqlSession session) {
    // Eventual: fast reads
    session.execute(
      SimpleStatement.builder("SELECT * FROM users")
        .setConsistency(ConsistencyLevel.ONE)
        .build()
    );
    
    // Strong: quorum reads
    session.execute(
      SimpleStatement.builder("SELECT * FROM accounts")
        .setConsistency(ConsistencyLevel.ALL)
        .build()
    );
  }
}
```

### 11.3 Saga Orchestrator Implementation

```typescript
// TypeScript Saga Orchestrator
interface SagaState<T> {
  id: string;
  status: 'running' | 'completed' | 'failed' | 'compensating';
  currentStep: number;
  context: T;
  failedStep?: number;
  error?: Error;
}

class SagaOrchestrator<T> {
  private steps: SagaStepDefinition<T>[];
  private stateStore: StateStore;
  
  async execute(sagaId: string, context: T): Promise<void> {
    const state: SagaState<T> = {
      id: sagaId,
      status: 'running',
      currentStep: 0,
      context
    };
    
    await this.stateStore.save(state);
    
    try {
      for (state.currentStep = 0; 
           state.currentStep < this.steps.length;
           state.currentStep++) {
        const step = this.steps[state.currentStep];
        
        await this.executeStep(step, state.context);
        await this.stateStore.save(state);
      }
      
      state.status = 'completed';
      await this.stateStore.save(state);
      
    } catch (error) {
      state.status = 'failed';
      state.failedStep = state.currentStep;
      state.error = error;
      await this.stateStore.save(state);
      
      // Compensate
      await this.compensate(state);
    }
  }
  
  private async compensate(state: SagaState<T>): Promise<void> {
    state.status = 'compensating';
    await this.stateStore.save(state);
    
    for (let i = state.currentStep - 1; i >= 0; i--) {
      const step = this.steps[i];
      if (step.compensate) {
        state.currentStep = i;
        await this.stateStore.save(state);
        
        try {
          await step.compensate(state.context);
        } catch (compensateError) {
          // Log but continue compensating
          console.error(`Compensation failed for step ${i}`, 
            compensateError);
        }
      }
    }
  }
  
  private async executeStep(
    step: SagaStepDefinition<T>, 
    context: T
  ): Promise<void> {
    await step.execute(context);
  }
}
```

### 11.4 Event Sourcing Configuration

```javascript
// Event Store Configuration
const eventstore = require('eventstore');

const es = eventstore({
  // Use MongoDB for storage
  type: 'mongo',  
  uri: 'mongodb://localhost:27017/eventstore',
  
  // Enable snapshots every 100 events
  snapshot: {
    enabled: true,
    frequency: 100
  },
  
  // Track correlations
  cors: {
    enabled: true,
    origin: '*'
  }
});

// Subscribe to events
es.subscribe((event) => {
  console.log('Event occurred:', event);
});

// Create new event
es.createEvent({
  aggregateId: 'order-123',
  type: 'OrderPlaced',
  data: {
    total: 100.00,
    items: [...]
  },
  metadata: {
    userId: 'user-456',
    correlationId: 'cmd-789'
  }
});
```

---

## 12. Best Practices

### 12.1 Choosing Consistency Models

| Factor | Strong Consistency | Eventual Consistency |
|--------|-------------------|---------------------|
| **Business Impact** | High (money, legal) | Medium (UI, analytics) |
| **Conflict Rate** | High | Low |
| **User Tolerance** | Low | High |
| **System Scale** | Small to medium | Large |
| **Latency Req** | < 100ms acceptable | Higher acceptable |
| **Write Ratio** | Balanced | Write-heavy |

### 12.2 Design Guidelines

```markdown
## 1. Understand Your Requirements
- Data importance level
- User expectations
- Conflict frequency

## 2. Start with Weaker Consistency
- Less cost, better performance
- Add strong only where needed

## 3. Use the Right Tool
- Don't force BASE into ACID
- Don't force ACID into BASE

## 4. Monitor and Adjust
- Track staleness
- Measure latency
- Adjust based on metrics

## 5. Handle Conflicts Gracefully
- Design conflict resolution
- Build compensation
- Log for debugging

## 6. Document Decisions
- Why strong/eventual?
- What are the trade-offs?
- What to do if issues?
```

### 12.3 Implementation Checklist

```yaml
# Before going to production
consistency_checklist:
  - [ ] Identify all data entities
  - [ ] Categorize by criticality
  - [ ] Define consistency requirements per entity
  - [ ] Select appropriate database
  - [ ] Configure consistency levels
  - [ ] Implement conflict resolution
  - [ ] Add compensation mechanisms
  - [ ] Test failure scenarios
  - [ ] Document for operations
  - [ ] Set up monitoring/alerting
```

### 12.4 Common Pitfalls to Avoid

| Pitfall | Impact | Solution |
|---------|--------|----------|
| Using eventual for payments | Data loss | Use ACID |
| Using strong for logs | Latency | Use eventual |
| No conflict resolution | Data divergence | Implement merge |
| No compensation | Incomplete rollback | Implement Saga |
| Ignoring monitoring | Issues undetected | Monitor consistency |

---

## 13. Summary

| Aspect | ACID | BASE |
|--------|------|------|
| **Full Form** | Atomicity, Consistency, Isolation, Durability | Basically Available, Soft State, Eventual Consistency |
| **Consistency** | Strong | Eventual |
| **Availability** | Lower | Higher |
| **Latency** | Higher | Lower |
| **Partitions** | Unavailable | Available |
| **Trade-off** | Consistency > Availability | Availability > Consistency |
| **Databases** | PostgreSQL, MySQL, Oracle | Cassandra, DynamoDB, MongoDB |
| **Use Cases** | Financial, Transactions | Logs, Analytics, Caching |

---

## References

- [CAP Theorem - Eric Brewer](https://www.infoq.com/articles/cap-twelve-years-later/)
- [BASE: An ACID Alternative - Dan Pritchett](https://queue.acm.org/detail.cfm?id=1394128)
- [Amazon Dynamo Paper](https://www.allthingsdistributed.com/2007/10/amazons_dynamo.html)
- [CQRS and Saga Patterns](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/cqrs/)

---

**Last Updated**: 2026-04-08
**Author**: Microservices Fundamentals Course
**Version**: 1.0