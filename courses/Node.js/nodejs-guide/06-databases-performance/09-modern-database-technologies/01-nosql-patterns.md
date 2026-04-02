# Modern Database Technologies with Node.js

## What You'll Learn

- NoSQL database patterns
- Search engine integration (Elasticsearch)
- In-memory databases (Redis)
- Database technology selection criteria

## Redis for Caching and Sessions

```javascript
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

// Session storage
async function setSession(sessionId, data, ttl = 3600) {
    await redis.set(`session:${sessionId}`, JSON.stringify(data), { EX: ttl });
}

async function getSession(sessionId) {
    const data = await redis.get(`session:${sessionId}`);
    return data ? JSON.parse(data) : null;
}

// Rate limiting
async function checkRateLimit(ip, limit = 100, window = 60) {
    const key = `ratelimit:${ip}`;
    const current = await redis.incr(key);
    if (current === 1) await redis.expire(key, window);
    return current <= limit;
}
```

## Elasticsearch Integration

```bash
npm install @elastic/elasticsearch
```

```javascript
import { Client } from '@elastic/elasticsearch';

const es = new Client({ node: process.env.ELASTICSEARCH_URL });

// Index document
async function indexDocument(index, id, document) {
    await es.index({ index, id, document, refresh: true });
}

// Search
async function search(index, query) {
    const result = await es.search({
        index,
        body: {
            query: {
                multi_match: {
                    query,
                    fields: ['title', 'content', 'tags'],
                },
            },
        },
    });
    return result.hits.hits.map(hit => hit._source);
}
```

## Database Selection Matrix

```
Use Case                    Recommended Database
─────────────────────────────────────────────────
Document storage            MongoDB, Couchbase
Relational data             PostgreSQL, MySQL
Caching / sessions          Redis, Memcached
Full-text search            Elasticsearch, Typesense
Time-series data            InfluxDB, TimescaleDB
Graph relationships         Neo4j, ArangoDB
Key-value store             Redis, LevelDB
Embedded / local            SQLite, LevelDB
```

## Best Practices Checklist

- [ ] Choose database based on data model
- [ ] Use Redis for caching and sessions
- [ ] Use Elasticsearch for full-text search
- [ ] Consider multi-model databases for complex needs
- [ ] Benchmark databases for your workload

## Cross-References

- See [Integration Patterns](../01-database-integration-patterns/01-mongodb-postgres.md) for setup
- See [Caching](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching
- See [Database Design](../10-database-design-architecture/01-schema-design.md) for design

## Next Steps

Continue to [Database Design](../10-database-design-architecture/01-schema-design.md).
