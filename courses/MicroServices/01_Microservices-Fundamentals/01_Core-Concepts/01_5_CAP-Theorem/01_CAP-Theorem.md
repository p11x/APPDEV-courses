# CAP Theorem: Understanding the Fundamental Trade-offs in Distributed Systems

## Overview

The CAP Theorem, also known as Brewer's Theorem, is a fundamental principle in distributed systems theory that describes the inherent trade-offs a system must make when designing distributed databases or services. Proposed by computer scientist Eric Brewer in 2000, this theorem states that a distributed data store can only provide two of three guarantees simultaneously: **Consistency**, **Availability**, and **Partition Tolerance**.

---

## 1. Understanding the Three Guarantees

### 1.1 Consistency (C)

**Consistency** in the context of CAP refers to **linearizable consistency** (also known as strong consistency). This means that all nodes in a distributed system see the same data at the same time. When a write operation completes, all subsequent read operations must reflect that write.

**Key characteristics:**
- Every read receives the most recent write or an error
- All nodes must agree on the current state of data
- Operations appear to execute atomically
- Data is synchronized across all replicas before acknowledging writes

```java
// Example: Strong consistency in a distributed system
public class ConsistentOrderService {
    
    public void updateOrder(Order order) {
        // Write must be replicated to ALL nodes before acknowledgment
        for (Node node : allNodes) {
            node.replicateSynchronously(order);
        }
        // Only return success after ALL nodes confirm
    }
    
    public Order getOrder(String orderId) {
        // Read must consult quorum of nodes
        return quorumRead(orderId);
    }
}
```

### 1.2 Availability (A)

**Availability** means that every request received by a non-failing node must receive a response. The system guarantees that every request will eventually be answered, even if some nodes are down.

**Key characteristics:**
- Every working node must respond to requests
- No node can be allowed to hang indefinitely
- Response may not be the most recent data (eventual consistency)
- System remains operational even during failures

```python
# Example: Availability in a distributed system
class AvailableOrderService:
    
    def get_order(self, order_id):
        # Try each node until one responds
        for node in self.nodes:
            try:
                return node.get(order_id)
            except NodeUnavailable:
                continue
        
        # If all primary nodes fail, try replicas
        return self.replica.get(order_id)
    
    def update_order(self, order):
        # Write to any available node
        available_nodes = self.get_available_nodes()
        if available_nodes:
            return random.choice(available_nodes).update(order)
        raise NoAvailableNodeException()
```

### 1.3 Partition Tolerance (P)

**Partition Tolerance** means the system continues to operate despite network failures that split the system into isolated partitions. Network partitions are inevitable in distributed systems.

**Key characteristics:**
- System continues functioning during network failures
- Messages may be lost between nodes
- System must handle communication breakdowns
- Partitions can occur at any time due to network issues

```go
// Example: Partition tolerance handling in Go
type PartitionTolerantStore struct {
    nodes []Node
    writeQuorum int
    readQuorum int
}

func (s *PartitionTolerantStore) Write(key, value string) error {
    // Write to as many nodes as possible
    successfulWrites := 0
    for _, node := range s.nodes {
        if err := node.Write(key, value); err == nil {
            successfulWrites++
        }
    }
    
    // Return success if quorum is reached
    if successfulWrites >= s.writeQuorum {
        return nil
    }
    return ErrQuorumNotReached
}
```

---

## 2. Why Only Two of Three?

The fundamental reason you can only guarantee two of three properties is that **partitions will happen** in distributed systems. When a partition occurs, you must make a choice:

### 2.1 The Partition Scenario

```
┌─────────────────────────────────────────────────────────────────────┐
│                    NETWORK PARTITION SCENARIO                       │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│    [Client]                                                         │
│        │                                                            │
│        │                                                            │
│   ┌────┴────┐                                                       │
│   │         │                                                       │
│   ▼         ▼                                                       │
│ ┌─────┐   ┌─────┐                                                   │
│ │Node1│   │Node2│                                                   │
│ │ DB A│   │ DB B│  ────── NETWORK FAILURE ──────                 │
│ └─────┘   └─────┘                                                   │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

When Node1 and Node2 cannot communicate:
- **If you choose C + P**: Node1 must reject writes to maintain consistency (system unavailable)
- **If you choose A + P**: Both nodes accept writes, but data diverges (inconsistent)
- **C + A is impossible**: If there's a partition, you cannot be both consistent and available

### 2.2 The Trade-off Flow

```
                    ┌─────────────────────┐
                    │  Network Partition  │
                    │      Occurs          │
                    └──────────┬────────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │   Must Choose:      │
                    └──────────┬────────────┘
                               │
              ┌────────────────┼────────────────┐
              │                │                │
              ▼                ▼                ▼
     ┌────────────────┐ ┌────────────────┐ ┌────────────────┐
     │   CONSISTENCY  │ │   AVAILABILITY │ │  PARTITION     │
     │   + PARTITION  │ │   + PARTITION  │ │  TOLERANCE     │
     │   TOLERANCE    │ │   TOLERANCE    │ │  (ALWAYS)      │
     └───────┬────────┘ └───────┬────────┘ └───────┬────────┘
             │                  │                  │
             ▼                  ▼                  ▼
    ┌────────────────┐  ┌────────────────┐  ┌────────────────┐
    │    CP Systems  │  │    AP Systems  │  │   Must Choose  │
    │  (e.g., etcd,  │  │  (e.g.,        │  │    During      │
    │   HBase, Zoo   │  │   Cassandra,   │  │   Partition    │
    │   Keeper)      │  │   DynamoDB)    │  │                │
    └────────────────┘  └────────────────┘  └────────────────┘
```

---

## 3. How Partitions Occur in Distributed Systems

### 3.1 Common Causes of Network Partitions

1. **Hardware Failures**: Network interface cards, switches, routers failing
2. **Network Congestion**: High latency or packet loss
3. **Data Center Issues**: Power failures, cooling failures
4. **Software Bugs**: Network stack bugs, misconfigured firewalls
5. **Geographic Distribution**: Cross-region latency and failures

### 3.2 Real-world Partition Examples

```yaml
# Kubernetes pod network partition scenario
apiVersion: v1
kind: Pod
metadata:
  name: order-service
spec:
  affinity:
    podAntiAffinity:
      preferredDuringSchedulingIgnoredDuringExecution:
      - weight: 100
        podAffinityTerm:
          labelSelector:
            matchExpressions:
            - key: app
              operator: In
              values:
              - order-service
          topologyKey: topology.kubernetes.io/zone
```

---

## 4. CP Systems (Consistency + Partition Tolerance)

### 4.1 Characteristics of CP Systems

CP systems prioritize consistency over availability when a partition occurs. They will:
- Reject writes to prevent data inconsistency
- Become partially unavailable during partitions
- Ensure all nodes have identical data after recovery

### 4.2 Examples of CP Databases

| Database | Use Case | Consistency Model |
|----------|----------|-------------------|
| **etcd** | Service discovery, config management | Strong consistency (Raft) |
| **ZooKeeper** | Coordination, leadership election | Strong consistency |
| **HBase** | Big data storage, random access | Strong consistency |
| **MongoDB (wired tiger)** | Document storage | Strong consistency (single shard) |
| **Google Spanner** | Global distributed SQL | Strong consistency (Paxos) |

### 4.3 CP Configuration Example: etcd

```yaml
# etcd configuration for strong consistency
# etcd.yaml
name: etcd-node-1
data-dir: /var/lib/etcd
listen-peer-urls: https://localhost:2380
listen-client-urls: https://localhost:2379
advertise-client-urls: https://localhost:2379

# Election timeout for strong consistency
election-timeout: 1000
heartbeat-interval: 100

# Quorum settings
quota-backend-bytes: 8589934592
```

```go
// Go example: Using etcd for CP guarantees
import (
    clientv3 "go.etcd.io/etcd/client/v3"
    "context"
    "time"
)

func main() {
    cli, err := clientv3.New(clientv3.Config{
        Endpoints:   []string{"localhost:2379"},
        DialTimeout: 5 * time.Second,
    })
    if err != nil {
        log.Fatal(err)
    }
    defer cli.Close()
    
    ctx := context.Background()
    
    // Strong consistency write - waits for quorum
    _, err = cli.Put(ctx, "order/123", "pending")
    if err != nil {
        // Partition occurred - write rejected
        log.Printf("Write rejected due to partition: %v", err)
    }
    
    // Strong consistency read - reads from quorum
    resp, err := cli.Get(ctx, "order/123")
    // Guaranteed to see latest write
}
```

### 4.4 CP System Decision Flow

```
┌────────────────────────────────────────────────────────────┐
│                    CP SYSTEM BEHAVIOR                      │
├────────────────────────────────────────────────────────────┤
│                                                             │
│   Client Request                                            │
│        │                                                    │
│        ▼                                                    │
│   ┌─────────────┐                                           │
│   │ Partition? │                                           │
│   └──────┬──────┘                                           │
│        Yes │ No                                             │
│    ┌───────┴───────┐                                        │
│    ▼               ▼                                        │
│ ┌─────────┐   Process Normally                             │
│ │ Reject  │   (Strong Consistency)                          │
│ │ Write/  │   ┌─────────────┐                                │
│ │ Read    │   │ Write to   │                                │
│ └─────────┘   │ Quorum      │                                │
│               │ Return OK   │                                │
│               └─────────────┘                                │
│                                                             │
└────────────────────────────────────────────────────────────┘
```

---

## 5. AP Systems (Availability + Partition Tolerance)

### 5.1 Characteristics of AP Systems

AP systems prioritize availability over consistency during partitions. They will:
- Accept writes on all available nodes
- Continue serving reads even during partitions
- Resolve inconsistencies later through reconciliation

### 5.2 Examples of AP Databases

| Database | Use Case | Availability Model |
|----------|----------|-------------------|
| **Apache Cassandra** | High write throughput, geo-distribution | Multi-master replication |
| **Amazon DynamoDB** | Key-value, document storage | Multi-AZ replication |
| **CouchDB** | Document database with sync | Master-master replication |
| **Riak** | Distributed key-value store | Vector clocks, eventual consistency |
| **Amazon S3** | Object storage | High availability |

### 5.3 AP Configuration Example: Cassandra

```yaml
# Cassandra configuration for high availability
# cassandra.yaml
cluster_name: "OrderProcessingCluster"

# Replication strategy
keyspaces:
  orders:
    replication_factor: 3
    network_topology_strategy:
      dc1: 3
      dc2: 3

# Consistency settings - tunable consistency
# For availability: ANY, ONE, LOCAL_ONE
# For consistency: QUORUM, ALL, LOCAL_QUORUM
write_consistency_level: ANY
read_consistency_level: ONE

# Gossip settings for partition tolerance
endpoint_snitch: GossipingPropertyFileSnitch
```

```java
// Java example: Cassandra AP configuration
import com.datastax.oss.driver.api.core.CqlSession;
import com.datastax.oss.driver.api.core.config.Options;

public class APOrderRepository {
    
    private CqlSession session;
    
    public APOrderRepository() {
        this.session = CqlSession.builder()
            .withLocalDatacenter("dc1")
            .addContactPoints("10.0.0.1", "10.0.0.2", "10.0.0.3")
            .withQueryConfig(QueryConfig.builder()
                .setConsistencyLevel(ConsistencyLevel.ONE)  // High availability
                .setSerialConsistencyLevel(ConsistencyLevel.LOCAL_SERIAL)
                .build())
            .build();
    }
    
    // Available even during partition - may return stale data
    public CompletableFuture<Order> findById(String id) {
        return session.executeAsync(
            SimpleStatement.builder("SELECT * FROM orders WHERE id = ?")
                .setConsistencyLevel(ConsistencyLevel.ONE)
                .build(id)
        ).toCompletableFuture();
    }
    
    // Write succeeds even to single node
    public CompletableFuture<Void> save(Order order) {
        return session.executeAsync(
            SimpleStatement.builder("INSERT INTO orders JSON ?")
                .setConsistencyLevel(ConsistencyLevel.ANY)  // Any node
                .build(order.toJson())
        ).toCompletableFuture();
    }
}
```

### 5.4 AP System Decision Flow

```
┌────────────────────────────────────────────────────────────┐
│                    AP SYSTEM BEHAVIOR                       │
├────────────────────────────────────────────────────────────┤
│                                                             │
│   Client Request                                            │
│        │                                                    │
│        ▼                                                    │
│   ┌─────────────┐                                           │
│   │ Partition? │                                           │
│   └──────┬──────┘                                           │
│        Yes │ No                                             │
│    ┌───────┴───────┐                                        │
│    ▼               ▼                                        │
│ Accept Writes    Process Normally                           │
│ On All Nodes     (Eventual Consistency)                      │
│ ┌─────────┐      ┌─────────────┐                              │
│ │ Return  │      │ Write to    │                              │
│ │ Success │      │ Any Node    │                              │
│ │ Read    │      │ Return OK   │                              │
│ │ From    │      └─────────────┘                              │
│ │ Any     │                                                │
│ └─────────┘                                                │
│                                                             │
└────────────────────────────────────────────────────────────┘
```

---

## 6. CA Systems (Consistency + Availability)

### 6.1 Why CA is Not Possible with Partitions

CA systems are **theoretically impossible** in distributed systems because:
1. Networks will always have partitions at some point
2. When a partition occurs, you must choose between C and A
3. Without partition tolerance, a single node failure breaks the system

### 6.2 CA in Single-Node or Non-Distributed Systems

CA is achievable in:
- Single-node databases (no network)
- Traditional RDBMS on a single server
- Systems that don't need to scale

```sql
-- Traditional RDBMS (CA-like, but not distributed)
CREATE TABLE orders (
    id VARCHAR(36) PRIMARY KEY,
    customer_id VARCHAR(36),
    status VARCHAR(20),
    total DECIMAL(10,2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ACID transaction ensures consistency
BEGIN TRANSACTION;
    INSERT INTO orders (id, customer_id, status, total)
    VALUES ('123', 'cust-1', 'PENDING', 100.00);
    UPDATE customers SET balance = balance - 100 WHERE id = 'cust-1';
COMMIT;
```

---

## 7. Practical Implications for Microservices

### 7.1 Data Architecture Decisions

When designing microservices, CAP Theorem directly impacts:

```yaml
# Microservices data strategy example
services:
  order_service:
    database: cassandra  # AP - high availability
    rationale: "Orders must be always available, eventual consistency acceptable"
    replication: 3
    write_consistency: ANY
    read_consistency: ONE
    
  inventory_service:
    database: postgresql  # CA-like - consistency critical
    rationale: "Inventory must be accurate, stock deducts must be exact"
    use_cases:
      - Reserve stock atomically
      - Prevent overselling
      
  user_service:
    database: dynamodb  # AP with tunable consistency
    rationale: "User sessions must be available, slight staleness OK"
    read_after_write_consistency: true
```

### 7.2 Service Communication Trade-offs

```java
// Microservice communication patterns based on CAP

// Order Service -> Inventory Service (Needs Consistency)
@Service
public class OrderService {
    
    @Transactional(isolation = Isolation.SERIALIZABLE)
    public void placeOrder(Order order) {
        // Strong consistency required - use synchronous call
        boolean reserved = inventoryClient.reserveStock(order.getItems());
        if (!reserved) {
            throw new InsufficientStockException();
        }
        orderRepository.save(order);
    }
}

// Notification Service -> Order Service (Availability OK)
@Service
public class NotificationService {
    
    public void notifyCustomer(String orderId) {
        // Eventual consistency OK - use async/event-driven
        orderEventSubscriber.onEvent(event -> {
            sendEmail(event.getCustomerId(), event.getOrderId());
        });
    }
}
```

---

## 8. Database Choices Based on CAP

### 8.1 Decision Matrix

| Requirement | Recommended Database | CAP Type |
|-------------|---------------------|----------|
| Financial transactions | PostgreSQL, Oracle | CA |
| User sessions, profiles | DynamoDB, Cassandra | AP |
| Distributed coordination | etcd, ZooKeeper | CP |
| Shopping cart | DynamoDB, Cassandra | AP |
| Product catalog | MongoDB, Elasticsearch | AP |
| Message queue | Kafka, RabbitMQ | CP |
| Configuration management | etcd, Consul | CP |
| Analytics, data warehouse | BigQuery, Redshift | CA |

### 8.2 Multi-Database Strategy

```yaml
# Kubernetes multi-database deployment for microservices
apiVersion: v1
kind: ConfigMap
metadata:
  name: database-config
data:
  order-service: |
    type: cassandra
    endpoints: cassandra:9042
    keyspace: orders
    
  inventory-service: |
    type: postgresql
    endpoints: postgresql:5432
    database: inventory
    
  user-service: |
    type: dynamodb
    region: us-east-1
    table: users
```

---

## 9. Real-world Examples from Companies

### 9.1 Amazon (DynamoDB - AP)

Amazon's DynamoDB was designed with AP principles:
- **Availability**: 99.99% uptime SLA
- **Eventual consistency**: Reads may return stale data
- **Tunable consistency**: Developers choose consistency level

```java
// DynamoDB example from Amazon
DynamoDbClient client = DynamoDbClient.builder()
    .region(Region.US_EAST_1)
    .build();

GetItemRequest request = GetItemRequest.builder()
    .tableName("Orders")
    .key(Map.of("orderId", AttributeValue.builder().s("123").build()))
    .consistentRead(false)  // Eventually consistent - faster, more available
    .build();

// vs. consistentRead(true) - strong consistency, may be slower
```

### 9.2 Google (Spanner - CP)

Google Spanner provides strong consistency:
- **TrueTime**: GPS and atomic clocks for global consistency
- **Paxos consensus**: Majority agreement for writes
- **Higher latency**: Trade-off for consistency

### 9.3 Netflix (Cassandra - AP)

Netflix uses Cassandra for:
- **Customer data**: Always available for viewing
- **Viewing history**: Eventual consistency acceptable
- **Global scale**: Multi-region deployment

### 9.4 Uber (PostgreSQL + Custom Sync - CP)

Uber's original architecture:
- **Primary PostgreSQL**: Strong consistency for ride data
- **Schemaless**: MySQL for real-time data
- **Sync service**: Custom replication for availability

---

## 10. Flow Charts Showing Trade-offs

### 10.1 Database Selection Flowchart

```
┌─────────────────────────────────────────────────────────────────┐
│                  DATABASE SELECTION FLOW                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  Start: Do you need distributed system?                        │
│              │                                                  │
│              │ No                                               │
│              ▼                                                  │
│      ┌─────────────────┐                                        │
│      │ Use Traditional │  ──► CA (Single-node RDBMS)          │
│      │ Database        │                                        │
│      └─────────────────┘                                        │
│              │                                                  │
│              │ Yes                                              │
│              ▼                                                  │
│  Can you tolerate downtime during network failures?            │
│              │                                                  │
│        ┌─────┴─────┐                                            │
│        │           │                                            │
│       No          Yes                                           │
│        │           │                                            │
│        ▼           ▼                                            │
│ ┌────────────┐  Can you tolerate stale data?                     │
│ │ Need Strong│        │                                         │
│ │ Consistency│       No                                         │
│ │    ?       │        │                                         │
│ └─────┬──────┘        ▼                                         │
│       │        ┌────────────┐                                    │
│      No        │ Need Strong │                                   │
│       │        │ Consistency │                                   │
│       ▼        └─────┬──────┘                                   │
│  ┌─────────┐         │                                          │
│  │ AP      │        No                                          │
│  │ Cassandra│         │                                          │
│  │ DynamoDB │         ▼                                          │
│  └─────────┘    ┌─────────┐                                      │
│                 │ AP      │                                      │
│                 │ Cassandra│                                     │
│                 │ DynamoDB │                                      │
│                 └─────────┘                                      │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### 10.2 Partition Handling Decision Flow

```
┌─────────────────────────────────────────────────────────────────┐
│              PARTITION OCCURS - WHAT TO DO?                     │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│              ┌───────────────────┐                             │
│              │  Partition Detected│                             │
│              └─────────┬─────────┘                             │
│                        │                                        │
│                        ▼                                        │
│         ┌──────────────────────────┐                           │
│         │ What's your priority?    │                           │
│         └────────────┬─────────────┘                           │
│                      │                                          │
│      ┌───────────────┼───────────────┐                         │
│      │               │               │                         │
│      ▼               ▼               ▼                         │
│ ┌──────────┐    ┌──────────┐    ┌──────────┐                   │
│ │Data Must │    │System    │    │Balanced  │                   │
│ │Be Exact  │    │Must Stay │    │Approach  │                   │
│ │  (CP)    │    │Up (AP)   │    │          │                   │
│ └────┬─────┘    └────┬─────┘    └────┬─────┘                   │
│      │               │               │                          │
│      ▼               ▼               ▼                          │
│ Reject writes │ Accept writes │ Queue writes                   │
│ Return errors │ on all nodes   │ for later                     │
│ to clients    │                │ reconciliation                │
│               │                │                               │
│               │                │                               │
└─────────────────────────────────────────────────────────────────┘
```

---

## 11. Code Examples Showing Configuration

### 11.1 Spring Boot + Cassandra (AP)

```java
// application.yml - Cassandra AP configuration
spring:
  data:
    cassandra:
      contact-points: cassandra-node-1:9042,cassandra-node-2:9042
      keyspace-name: orders
      local-datacenter: us-east-1
      
      # AP settings
      consistency-level: LOCAL_ONE  # High availability
      serial-consistency-level: LOCAL_SERIAL
      
      # Connection pool for availability
      max-connections-per-host: 100
      connection-timeout: 5000ms
```

```java
// OrderRepository.java
@Repository
public class OrderRepository {
    
    @Autowired
    private CassandraTemplate cassandraTemplate;
    
    @Override
    public Order save(Order order) {
        // Write to any node - returns immediately
        return cassandraTemplate.insert(order);
    }
    
    @Override
    public Optional<Order> findById(String id) {
        // Read from closest node - may return stale data
        return Optional.ofNullable(
            cassandraTemplate.selectOne(
                Query.query(Criteria.where("id").is(id))
                    .withConsistency(ConsistencyLevel.ONE),
                Order.class
            )
        );
    }
}
```

### 11.2 Spring Boot + PostgreSQL (CP)

```java
// application.yml - PostgreSQL CP configuration
spring:
  datasource:
    url: jdbc:postgresql://postgres-primary:5432/orders
    hikari:
      maximum-pool-size: 20
      minimum-idle: 5
      connection-timeout: 30000
      
  jpa:
    hibernate:
      ddl-auto: validate
    properties:
      hibernate:
        # Strong consistency settings
        jdbc:
          time_zone: UTC
    open-in-view: false  # Don't hold connection during view rendering
```

```java
// OrderService.java - CP behavior
@Service
public class OrderService {
    
    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Order placeOrder(Order order) {
        // Exclusive lock prevents race conditions
        // Ensures consistent view of inventory
        Inventory inventory = inventoryRepository
            .findByProductIdWithLock(order.getProductId());
            
        if (inventory.getQuantity() < order.getQuantity()) {
            throw new InsufficientStockException();
        }
        
        inventory.reserve(order.getQuantity());
        inventoryRepository.save(inventory);
        
        return orderRepository.save(order);
    }
}
```

### 11.3 Multi-Database Service Configuration

```java
// MultiDatabaseConfig.java
@Configuration
public class MultiDatabaseConfig {
    
    @Bean
    @Primary
    @Qualifier("orderRepository")
    public OrderRepository orderRepository(
            @Value("${order.db.type: cassandra}") String dbType) {
        
        switch (dbType.toLowerCase()) {
            case "cassandra":
                return new CassandraOrderRepository();
            case "dynamodb":
                return new DynamoDBOrderRepository();
            case "postgresql":
                return new PostgresOrderRepository();
            default:
                throw new IllegalArgumentException("Unknown DB type: " + dbType);
        }
    }
}
```

---

## 12. Best Practices for Handling CAP in Microservices

### 12.1 Design Principles

1. **Understand Your Requirements**
   - Not every service needs strong consistency
   - Analyze business tolerance for stale data
   - Define availability vs consistency priorities per service

2. **Use the Right Tool for the Job**
   - Don't force one database for all services
   - Polyglot persistence is acceptable
   - Each service chooses its database based on needs

3. **Design for Failure**
   - Partitions will happen - plan for it
   - Implement graceful degradation
   - Have recovery procedures ready

### 12.2 Implementation Best Practices

```java
// Best Practice: Implement saga pattern for distributed transactions
// instead of trying to maintain strong consistency

@Component
public class OrderSaga {
    
    private final OrderService orderService;
    private final InventoryService inventoryService;
    private final PaymentService paymentService;
    
    @Saga
    public void placeOrderSaga(Order order) {
        try {
            // Step 1: Reserve inventory (compensatable)
            inventoryService.reserve(order.getItems());
            
            // Step 2: Process payment (compensatable)
            paymentService.charge(order.getCustomerId(), order.getTotal());
            
            // Step 3: Create order (final step)
            orderService.create(order);
            
        } catch (Exception e) {
            // Compensating transactions on failure
            compensate(order);
        }
    }
    
    private void compensate(Order order) {
        // Rollback in reverse order
        if (order.getId() != null) {
            orderService.cancel(order.getId());
        }
        paymentService.refund(order.getCustomerId(), order.getTotal());
        inventoryService.release(order.getItems());
    }
}
```

### 12.3 Monitoring and Observability

```yaml
# Prometheus metrics for CAP-related monitoring
apiVersion: v1
kind: ConfigMap
metadata:
  name: prometheus-rules
data:
  rules.yml |
    groups:
    - name: consistency
      rules:
        - alert: HighReadLatency
          expr: histogram_quantile(0.99, rate(db_read_duration_seconds_bucket[5m])) > 1
          annotations:
            description: "Strong consistency reads are slow"
            
        - alert: ReplicationLag
          expr: rate(replication_lag_seconds[5m]) > 10
          annotations:
            description: "Replication lag indicates partition or load"
            
    - name: availability
      rules:
        - alert: NodeDown
          expr: up{job="database"} == 0
          annotations:
            description: "Database node is down"
            
        - alert: HighErrorRate
          expr: rate(http_requests_total{status=~"5.."}[5m]) > 0.05
          annotations:
            description: "High error rate may indicate partition"
```

### 12.4 Configuration Checklist

```markdown
## CAP Implementation Checklist

### Design Phase
- [ ] Identify consistency requirements per service
- [ ] Document tolerance for stale data
- [ ] Define RTO (Recovery Time Objective) and RPO (Recovery Point Objective)
- [ ] Choose database per service based on CAP

### Implementation Phase
- [ ] Configure appropriate consistency levels
- [ ] Implement retry mechanisms with exponential backoff
- [ ] Add circuit breakers for failed operations
- [ ] Set up proper timeouts

### Operations Phase
- [ ] Monitor replication lag
- [ ] Track partition events
- [ ] Test failure scenarios
- [ ] Document recovery procedures
```

---

## Summary

The CAP Theorem is a fundamental constraint in distributed systems design. Key takeaways:

1. **Partitions are inevitable** - Plan for them, don't try to avoid them
2. **Choose between CP and AP** - You cannot have both during a partition
3. **CA is not achievable** - Without partition tolerance in distributed systems
4. **Match database to requirements** - Different services have different needs
5. **Embrace eventual consistency** - Many applications don't need strong consistency
6. **Use patterns like sagas** - For distributed transactions across services

Understanding CAP helps you make informed decisions about data architecture in microservices, leading to more resilient and scalable systems.

---

## References

- Brewer, E. (2000). "Towards Robust Distributed Systems"
- Daniel Abadi. "Consistency Tradeoffs in Modern Distributed Database Systems"
- Amazon DynamoDB Whitepaper
- Google Spanner Paper
- Martin Kleppmann. "Designing Data-Intensive Applications"
