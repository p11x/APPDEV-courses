# Graph Databases with Node.js (Neo4j, ArangoDB)

## What You'll Learn

- Neo4j integration with Node.js
- ArangoDB usage patterns
- Graph query languages (Cypher, AQL)
- Graph data modeling
- Performance optimization for graph queries

## Neo4j Integration

```bash
npm install neo4j-driver
```

```javascript
import neo4j from 'neo4j-driver';

const driver = neo4j.driver(
    process.env.NEO4J_URL || 'neo4j://localhost:7687',
    neo4j.auth.basic(process.env.NEO4J_USER || 'neo4j', process.env.NEO4J_PASSWORD)
);

// Create session
const session = driver.session();

// Create nodes and relationships
async function createUser(name, email) {
    const result = await session.run(
        'CREATE (u:User {name: $name, email: $email, createdAt: datetime()}) RETURN u',
        { name, email }
    );
    return result.records[0].get('u').properties;
}

async function createFriendship(userId1, userId2) {
    await session.run(
        'MATCH (a:User {email: $email1}), (b:User {email: $email2}) ' +
        'CREATE (a)-[:FRIEND {since: datetime()}]->(b)',
        { email1: userId1, email2: userId2 }
    );
}

async function getFriendsOfFriends(email) {
    const result = await session.run(
        'MATCH (me:User {email: $email})-[:FRIEND]->(friend)-[:FRIEND]->(fof) ' +
        'WHERE NOT (me)-[:FRIEND]->(fof) AND me <> fof ' +
        'RETURN DISTINCT fof.name as name, fof.email as email, count(friend) as mutualFriends ' +
        'ORDER BY mutualFriends DESC LIMIT 10',
        { email }
    );

    return result.records.map(r => ({
        name: r.get('name'),
        email: r.get('email'),
        mutualFriends: r.get('mutualFriends').toNumber(),
    }));
}

// Shortest path
async function findShortestPath(fromEmail, toEmail) {
    const result = await session.run(
        'MATCH path = shortestPath((a:User {email: $from})-[:FRIEND*]-(b:User {email: $to})) ' +
        'RETURN [n IN nodes(path) | n.name] as names, length(path) as distance',
        { from: fromEmail, to: toEmail }
    );

    return result.records[0] ? {
        names: result.records[0].get('names'),
        distance: result.records[0].get('distance').toNumber(),
    } : null;
}
```

## ArangoDB Integration

```bash
npm install arangojs
```

```javascript
import { Database } from 'arangojs';

const db = new Database({ url: process.env.ARANGO_URL || 'http://localhost:8529' });
db.useBasicAuth(process.env.ARANGO_USER || 'root', process.env.ARANGO_PASSWORD);
db.useDatabase(process.env.ARANGO_DB || 'myapp');

// Create collection
const users = db.collection('users');
await users.create();

// Create edge collection (for relationships)
const friendships = db.collection('friendships');
await friendships.create({ type: 3 }); // Type 3 = edge collection

// CRUD operations
async function createUser(data) {
    return users.save(data);
}

async function createFriendship(fromId, toId) {
    return friendships.save({
        _from: `users/${fromId}`,
        _to: `users/${toId}`,
        since: new Date().toISOString(),
    });
}

// AQL (ArangoDB Query Language)
async function getFriendsOfFriends(userId) {
    const cursor = await db.query(`
        FOR friend IN OUTBOUND @userId friendships
            FOR fof IN OUTBOUND friend._id friendships
                FILTER fof._id != @userId
                AND NOT (LENGTH(FOR f IN OUTBOUND @userId friendships FILTER f._id == fof._id RETURN 1) > 0)
                COLLECT name = fof.name, email = fof.email WITH COUNT INTO mutualFriends
                SORT mutualFriends DESC
                LIMIT 10
                RETURN { name, email, mutualFriends }
    `, { userId: `users/${userId}` });

    return cursor.all();
}

// Graph traversal
async function traverseNetwork(startUserId, depth = 3) {
    const cursor = await db.query(`
        FOR v, e, p IN 1..@depth OUTBOUND @startId friendships
            RETURN {
                user: v.name,
                path: p.vertices[*].name,
                depth: LENGTH(p.vertices) - 1
            }
    `, { startId: `users/${startUserId}`, depth });

    return cursor.all();
}
```

## Graph Data Modeling Patterns

```
Graph Modeling Patterns:
─────────────────────────────────────────────
Pattern          Example                    Use Case
─────────────────────────────────────────────
Social graph     User-FRIEND->User          Social networks
Knowledge graph  Entity-RELATES->Entity     Knowledge bases
Recommendation   User-BOUGHT->Product       E-commerce
Access control   User-HAS->Role-GRANTS->Permission  Auth
Supply chain     Supplier-SUPPLIES->Product-ORDERED->Customer

Node properties:
├── Unique identifier
├── Label/type classification
├── Timestamps
└── Domain-specific attributes

Relationship properties:
├── Weight/strength
├── Timestamp
├── Metadata
└── Direction (always meaningful)
```

## Best Practices Checklist

- [ ] Model relationships as first-class entities
- [ ] Use appropriate indexes on frequently queried properties
- [ ] Keep node properties minimal
- [ ] Use parameterized queries to prevent injection
- [ ] Close sessions when done
- [ ] Use connection pooling
- [ ] Profile graph queries for performance

## Cross-References

- See [NoSQL Patterns](./01-nosql-patterns.md) for document databases
- See [Database Design](../10-database-design-architecture/01-schema-design.md) for modeling
- See [Query Optimization](../02-database-performance-optimization/01-query-optimization.md) for tuning

## Next Steps

Continue to [Time-Series Databases](./03-time-series-databases.md) for time-series data.
