# Event-Driven Data Replication

## Overview

Event-driven data replication is a pattern that enables real-time synchronization of data across multiple systems by capturing and propagating database changes as events. This pattern addresses the challenge of keeping distributed data stores consistent without requiring tight coupling between systems. Change Data Capture (CDC) is the core mechanism that detects and captures database changes (inserts, updates, deletes) and propagates them to downstream systems in real-time.

Change Data Capture works by monitoring database transaction logs or using database-specific triggers to identify changes as they occur. Unlike batch-based ETL approaches, CDC processes changes immediately as they happen, enabling near real-time data movement with minimal latency. Debezium is an open-source CDC platform that provides connectors for various databases including MySQL, PostgreSQL, MongoDB, SQL Server, and Oracle. It captures row-level changes and publishes them as events to Apache Kafka or other messaging systems.

The event-driven approach decouples source and target systems, allowing each to evolve independently. Producers (source systems) emit events without knowledge of consumers, while consumers can be added or removed without affecting the producer. This pattern supports multiple consumers with different processing requirements, enabling scenarios like maintaining a data warehouse alongside operational systems. The pattern also provides eventual consistency guarantees, accepting that all replicas may not be synchronized at the exact same moment but will converge over time.

Event-driven data replication is essential for microservices architectures where each service owns its data but needs to share relevant information with other services or analytical systems. It enables use cases like maintaining read replicas for query offloading, synchronizing search indexes, propagating changes to caching layers, and feeding data to analytics platforms. This pattern is fundamental to building event-centric architectures that can scale to handle high-volume data changes across distributed systems.

## Flow Chart

```mermaid
flowchart TD
    A[Source Database] -->|Transaction Log| B[Debezium Connector]
    B -->|CDC Events| C[Apache Kafka]
    C -->|Change Events| D[Kafka Connect Sink]
    D --> E[Target Data Store]
    
    F[Application] -->|INSERT/UPDATE/DELETE| A
    
    G[Search Index] <- H[Elasticsearch Sink]
    C -.->|CDC Events| H
    G <- I[Cache] <- J[Redis Sink]
    C -.->|CDC Events| J
    
    K[Data Warehouse] <- L[Snowflake Sink]
    C -.->|CDC Events| L
    
    M[Analytics] <- N[Stream Processing]
    C -.->|CDC Events| N
    
    style A fill:#e1f5fe
    style C fill:#fff3e0
    style E fill:#e8f5e9
    style K fill:#fce4ec
```

## Standard Example

### Java Implementation with Debezium

```java
// pom.xml dependencies
/*
<dependencies>
    <dependency>
        <groupId>io.debezium</groupId>
        <artifactId>debezium-api</artifactId>
        <version>2.4.0.Final</version>
    </dependency>
    <dependency>
        <groupId>io.debezium</groupId>
        <artifactId>debezium-embedded</artifactId>
        <version>2.4.0.Final</version>
    </dependency>
    <dependency>
        <groupId>io.debezium</groupId>
        <artifactId>debezium-connector-mysql</artifactId>
        <version>2.4.0.Final</version>
    </dependency>
    <dependency>
        <groupId>org.apache.kafka</groupId>
        <artifactId>connect-api</artifactId>
        <version>3.6.0</version>
    </dependency>
</dependencies>
*/

import io.debezium.config.Configuration;
import io.debezium.data.Envelope;
import io.debezium.embedded.EmbeddedEngine;
import io.debezium.relational.history.FileHistory;
import io.debezium.util.Collect;

import java.io.File;
import java.nio.file.Files;
import java.time.Duration;
import java.util.Properties;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;

public class EventDrivenReplicationExample {
    
    public static class CustomerChangeHandler {
        
        public void handleChange(Envelope.RecordChange record) {
            Envelope.Operation operation = record.operation();
            
            switch (operation) {
                case CREATE:
                    handleInsert(record);
                    break;
                case UPDATE:
                    handleUpdate(record);
                    break;
                case DELETE:
                    handleDelete(record);
                    break;
                default:
                    break;
            }
        }
        
        private void handleInsert(Envelope.RecordChange record) {
            System.out.println("INSERT: " + record.after());
            String customerId = record.after().getString("id");
            String customerName = record.after().getString("name");
            String email = record.after().getString("email");
            System.out.println("New customer created: id=" + customerId + 
                           ", name=" + customerName + ", email=" + email);
        }
        
        private void handleUpdate(Envelope.RecordChange record) {
            System.out.println("UPDATE from: " + record.before());
            System.out.println("UPDATE to: " + record.after());
            String customerId = record.after().getString("id");
            System.out.println("Customer updated: id=" + customerId);
        }
        
        private void handleDelete(Envelope.RecordChange record) {
            System.out.println("DELETE: " + record.before());
            String customerId = record.before().getString("id");
            System.out.println("Customer deleted: id=" + customerId);
        }
    }
    
    public static class DebeziumRunner {
        
        private final EmbeddedEngine engine;
        private final ExecutorService executor;
        
        public DebeziumRunner(Configuration config, CustomerChangeHandler handler) {
            this.executor = Executors.newSingleThreadExecutor();
            
            this.engine = EmbeddedEngine.builder()
                .using(config)
                .using(this.getClass().getClassLoader())
                .build();
        }
        
        public void start() {
            executor.execute(() -> engine.run());
            System.out.println("Debezium engine started");
        }
        
        public void stop() {
            engine.stop();
            executor.shutdown();
            try {
                executor.awaitTermination(10, TimeUnit.SECONDS);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        }
        
        public static Configuration createMySQLConfig(String serverName, 
                                                  String topicPrefix) {
            return Configuration.create()
                .with("name", serverName + "-connector")
                .with("connector.class", 
                     "io.debezium.connector.mysql.MySqlConnector")
                .with("database.hostname", "localhost")
                .with("database.port", 3306)
                .with("database.user", "debezium")
                .with("database.password", "dbz123")
                .with("database.server.id", "184054")
                .with("database.server.name", serverName)
                .with("topic.prefix", topicPrefix)
                .with("database.include.list", "inventory")
                .with("table.include.list", "inventory.customers")
                .with("schema.history.internal.file.filename",
                     "/tmp/schema_history.dat")
                .with("include.schema.changes", "false")
                .with("transforms", "unwrap")
                .with("transforms.unwrap.type", 
                     "io.debezium.transforms.ExtractNewRecordState")
                .with("key.converter", "org.apache.kafka.connect.json.JsonConverter")
                .with("value.converter", 
                     "org.apache.kafka.connect.json.JsonConverter")
                .with("key.converter.schemas.enable", "false")
                .with("value.converter.schemas.enable", "false")
                .build();
        }
    }
    
    public static void main(String[] args) throws Exception {
        File tempFile = File.createTempFile("schema_history_", ".dat");
        tempFile.deleteOnExit();
        
        Configuration config = DebeziumRunner.createMySQLConfig(
            "mysql-server", 
            "mysql-server"
        ).override("schema.history.internal.file.filename", 
                   tempFile.getAbsolutePath());
        
        CustomerChangeHandler handler = new CustomerChangeHandler();
        DebeziumRunner runner = new DebeziumRunner(config, handler);
        
        runner.start();
        Thread.sleep(30000);
        runner.stop();
    }
}

// Kafka Connect Sink Example
import org.apache.kafka.connect.data.Struct;
import org.apache.kafka.connect.sink.SinkRecord;
import org.apache.kafka.connect.sink.SinkTask;

public class ElasticsearchSinkTask extends SinkTask {
    
    private ElasticsearchClient client;
    
    @Override
    public void start(Map<String, String> props) {
        this.client = new ElasticsearchClient(
            props.get("elasticsearch.url")
        );
        System.out.println("Elasticsearch sink task started");
    }
    
    @Override
    public void put(Collection<SinkRecord> records) {
        for (SinkRecord record : records) {
            Struct valueStruct = (Struct) record.value();
            
            String id = valueStruct.getString("id");
            String name = valueStruct.getString("name");
            String email = valueStruct.getString("email");
            
            Map<String, Object> document = Map.of(
                "id", id,
                "name", name,
                "email", email
            );
            
            client.indexDocument("customers", id, document);
            System.out.println("Indexed document: " + id);
        }
    }
    
    @Override
    public void stop() {
        client.close();
        System.out.println("Elasticsearch sink task stopped");
    }
}
```

### Kafka Connect Configuration

```properties
# mysql-source-connector.properties
name=mysql-source-connector
connector.class=io.debezium.connector.mysql.MySqlConnector
database.hostname=localhost
database.port=3306
database.user=debezium
database.password=dbz123
database.server.id=184054
database.server.name=inventory
topic.prefix=inventory
database.include.list=inventory
table.include.list=inventory.customers,inventory.orders
include.schema.changes=false
transforms=unwrap
transforms.unwrap.type=io.debezium.transforms.ExtractNewRecordState
key.converter=org.apache.kafka.connect.json.JsonConverter
value.converter=org.apache.kafka.connect.json.JsonConverter
key.converter.schemas.enable=false
value.converter.schemas.enable=false

# elasticsearch-sink-connector.properties
name=elasticsearch-sink-connector
connector.class=io.confluent.connect.elasticsearch.ElasticsearchSinkConnector
connection.url=http://localhost:9200
type.name=_doc
topics.index.reqs=inventory.customers
transforms=unwrap
transforms.unwrap.type=io.debezium.transforms.ExtractNewRecordState
key.converter=org.apache.kafka.connect.json.JsonConverter
value.converter=org.apache.kafka.connect.json.JsonConverter
key.converter.schemas.enable=false
value.converter.schemas.enable=false
```

## Real-World Examples

### Walmart

Walmart, one of the world's largest retailers, utilizes event-driven data replication through Debezium and Kafka to synchronize data across their extensive distributed systems. Their inventory management system processes millions of daily transactions across thousands of stores. When a sale occurs, the transaction is captured via CDC and propagated to multiple systems: the data warehouse for analytics, the supply chain system for replenishment planning, the e-commerce platform for inventory visibility, and the search system for product availability queries. This real-time data flow enables Walmart to maintain accurate inventory visibility across all channels, reducing stockouts and enabling their same-day delivery capabilities. The CDC-based approach handles the scale of billions of annual transactions while maintaining sub-second latency for critical inventory updates.

### LinkedIn

LinkedIn employs event-driven data replication for their member data synchronization across multiple data centers and services. When members update their profiles, post content, or make connections, these changes are captured via CDC and propagated to search indexes, recommendation engines, and notification systems. LinkedIn's use of Debezium enables them to maintain consistency across their CouchDB, Voldemort, and Espresso data stores. The pattern supports their real-time "people you may know" recommendations and activity feeds by ensuring that connection changes are immediately visible across their infrastructure. LinkedIn processes billions of events daily through their Kafka-based event streaming infrastructure, built on top of CDC captures from their primary databases.

## Output Statement

Event-driven data replication using CDC enables real-time data synchronization across distributed systems with the following characteristics. It provides eventual consistency across data stores while maintaining high throughput and low latency. The pattern decouples producers from consumers through an event streaming backbone, enabling independent evolution of source and target systems. CDC captures database changes from transaction logs without impacting source system performance. Multiple consumers can process the same change events for different purposes. This pattern is foundational for building responsive, data-consistent distributed systems that can scale to handle millions of changes per second while maintaining data integrity across all replicas.

## Best Practices

1. **Design for Idempotency**: Consumers must handle duplicate events gracefully. Include unique event identifiers and sequence numbers to enable deduplication. Design replay mechanisms that can reprocess events without creating duplicate data.

2. **Handle Schema Evolution**: Plan for database schema changes by using schema registry services. Debezium supports schema changes but consumers must handle evolving schemas. Register and version schemas to enable backward compatibility.

3. **Implement Proper Backpressure**: Monitor message throughput and implement flow control when downstream systems cannot keep pace. Use partition strategies that balance load while maintaining event order within partitions.

4. **Ensure Exactly-Once Delivery**: Configure appropriate acknowledgment settings for at-least-once delivery, then implement deduplication in consumers. For critical systems, implement transactions that span the event consumption and processing.

5. **Monitor Lag and Health**: Track consumer lag to ensure systems remain synchronized. Set up alerts for processing delays and implement circuit breakers to prevent cascade failures.

6. **Secure Event Streams**: Encrypt events in transit using TLS. Implement authentication and authorization for access to CDC streams. Audit access to sensitive data in event streams.

7. **Separate Concerns with Topics**: Design topic structures that balance partition throughput with the need for total ordering. Consider co-partitioning related entities to enable efficient joins in stream processing.

8. **Test Failure Scenarios**: Regularly test disaster recovery procedures including consumer failures, broker failures, and source database failures. Document recovery procedures and runbooks.