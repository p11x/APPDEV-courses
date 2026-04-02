# Multi-Model Databases and Technology Selection

## What You'll Learn

- Multi-model database patterns (ArangoDB, Couchbase)
- Database technology selection criteria
- Polyglot persistence strategies
- Database comparison matrix
- Migration between database technologies

## ArangoDB Multi-Model

```javascript
import { Database } from 'arangojs';

const db = new Database({ url: 'http://localhost:8529' });
db.useDatabase('myapp');

// Document model (like MongoDB)
const users = db.collection('users');
await users.save({ name: 'Alice', email: 'alice@example.com' });

// Graph model (like Neo4j)
const friendships = db.collection('friendships', { type: 3 });
await friendships.save({
    _from: 'users/alice',
    _to: 'users/bob',
    since: '2024-01-01',
});

// Key-value model
const cache = db.collection('cache');
await cache.save({ _key: 'config:theme', value: 'dark', ttl: 3600 });

// AQL combines all models
const cursor = await db.query(`
    // Document query + graph traversal + aggregation
    FOR user IN users
        LET friends = (
            FOR v IN 1..1 OUTBOUND user._id friendships
                RETURN v.name
        )
        LET friendCount = LENGTH(friends)
        FILTER friendCount > 5
        SORT friendCount DESC
        RETURN { name: user.name, friends, friendCount }
`);

// Full-text search
const searchResults = await db.query(`
    FOR doc IN FULLTEXT(articles, 'body', 'prefix:nodejs')
        RETURN { title: doc.title, snippet: LEFT(doc.body, 200) }
`);
```

## Database Technology Selection Matrix

```
Database Selection Guide:
─────────────────────────────────────────────
Data Model          Best Options              When to Choose
─────────────────────────────────────────────
Document            MongoDB, Couchbase        Flexible schema, nested data
Relational          PostgreSQL, MySQL         Structured data, ACID, joins
Key-value           Redis, DynamoDB           Simple lookups, caching
Graph               Neo4j, ArangoDB           Relationships, traversal
Time-series         InfluxDB, TimescaleDB     Metrics, events, IoT
Search              Elasticsearch, Typesense  Full-text search, analytics
Column-family       Cassandra, HBase          Wide columns, time-series
Multi-model         ArangoDB, Couchbase       Multiple patterns in one

Decision factors:
├── Data structure (structured vs flexible)
├── Query patterns (simple lookups vs complex joins)
├── Scale requirements (read-heavy vs write-heavy)
├── Consistency needs (strong vs eventual)
├── Team expertise
├── Operational complexity
└── Cost (licensing, infrastructure)
```

## Polyglot Persistence

```javascript
class PolyglotPersistence {
    constructor() {
        this.databases = {};
    }

    async initialize() {
        // PostgreSQL for transactional data
        this.databases.relational = new Pool({
            host: 'pg.local',
            database: 'transactions',
        });

        // Redis for caching
        this.databases.cache = createClient({ url: 'redis://localhost:6379' });
        await this.databases.cache.connect();

        // MongoDB for flexible documents
        this.databases.documents = new MongoClient('mongodb://mongo.local:27017');
        await this.databases.documents.connect();

        // Elasticsearch for search
        this.databases.search = new Client({ node: 'http://es.local:9200' });
    }

    // Route to appropriate database based on operation type
    async storeOrder(order) {
        // Transactional data → PostgreSQL
        const pgResult = await this.databases.relational.query(
            'INSERT INTO orders (user_id, total, status) VALUES ($1, $2, $3) RETURNING id',
            [order.userId, order.total, 'pending']
        );

        // Detailed order data → MongoDB (flexible schema)
        await this.databases.documents.db().collection('order_details').insertOne({
            orderId: pgResult.rows[0].id,
            items: order.items,
            metadata: order.metadata,
            shippingAddress: order.shippingAddress,
        });

        // Index for search → Elasticsearch
        await this.databases.search.index({
            index: 'orders',
            id: String(pgResult.rows[0].id),
            document: {
                userId: order.userId,
                total: order.total,
                itemCount: order.items.length,
                createdAt: new Date(),
            },
        });

        // Cache recent order → Redis
        await this.databases.cache.set(
            `order:${pgResult.rows[0].id}`,
            JSON.stringify(order),
            { EX: 300 }
        );

        return pgResult.rows[0];
    }
}
```

## Best Practices Checklist

- [ ] Choose database based on primary data model and query patterns
- [ ] Consider polyglot persistence for complex applications
- [ ] Evaluate operational complexity of each technology
- [ ] Benchmark with realistic data volumes
- [ ] Plan for data migration between technologies
- [ ] Use the right tool for each data access pattern
- [ ] Document technology decisions and rationale

## Cross-References

- See [NoSQL Patterns](./01-nosql-patterns.md) for document databases
- See [Schema Design](../10-database-design-architecture/01-schema-design.md) for modeling
- See [Integration Patterns](../01-database-integration-patterns/01-mongodb-postgres.md) for setup

## Next Steps

Continue to [Database Design](../10-database-design-architecture/01-schema-design.md) for design principles.
