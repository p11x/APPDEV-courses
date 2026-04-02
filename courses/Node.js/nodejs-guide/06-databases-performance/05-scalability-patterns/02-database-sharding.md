# Database Sharding Strategies and Implementation

## What You'll Learn

- Sharding strategies and shard key selection
- Consistent hashing for shard distribution
- Cross-shard query handling
- Shard rebalancing strategies
- Application-level sharding implementation

## Hash-Based Sharding

```javascript
import { createHash } from 'node:crypto';
import { Pool } from 'pg';

class HashShardRouter {
    constructor(shardConfigs) {
        this.shards = shardConfigs.map(config => ({
            id: config.id,
            pool: new Pool({
                host: config.host,
                port: config.port || 5432,
                database: config.database,
                user: config.user,
                password: config.password,
                max: 20,
            }),
        }));
        this.shardCount = this.shards.length;
    }

    getShard(shardKey) {
        const hash = createHash('md5').update(String(shardKey)).digest();
        const shardIndex = hash.readUInt32BE(0) % this.shardCount;
        return this.shards[shardIndex];
    }

    async query(shardKey, sql, params) {
        const shard = this.getShard(shardKey);
        return shard.pool.query(sql, params);
    }

    async queryAll(sql, params) {
        const results = await Promise.all(
            this.shards.map(s => s.pool.query(sql, params))
        );
        return results.flatMap(r => r.rows);
    }

    async transaction(shardKey, callback) {
        const shard = this.getShard(shardKey);
        const client = await shard.pool.connect();
        try {
            await client.query('BEGIN');
            const result = await callback(client);
            await client.query('COMMIT');
            return result;
        } catch (err) {
            await client.query('ROLLBACK');
            throw err;
        } finally {
            client.release();
        }
    }

    async shutdown() {
        await Promise.all(this.shards.map(s => s.pool.end()));
    }
}

// Usage
const router = new HashShardRouter([
    { id: 0, host: 'shard0.db.local', database: 'app_shard0' },
    { id: 1, host: 'shard1.db.local', database: 'app_shard1' },
    { id: 2, host: 'shard2.db.local', database: 'app_shard2' },
]);

// Single-shard query
const user = await router.query(userId, 'SELECT * FROM users WHERE id = $1', [userId]);

// Cross-shard query (scatter-gather)
const total = await router.queryAll('SELECT COUNT(*) as count FROM users');
```

## Consistent Hashing

```javascript
class ConsistentHashRing {
    constructor(virtualNodes = 150) {
        this.ring = [];
        this.virtualNodes = virtualNodes;
        this.shards = new Map();
    }

    addShard(shardId, pool) {
        this.shards.set(shardId, pool);

        for (let i = 0; i < this.virtualNodes; i++) {
            const hash = createHash('md5')
                .update(`${shardId}:${i}`)
                .digest('hex');
            this.ring.push({ hash, shardId });
        }

        this.ring.sort((a, b) => a.hash.localeCompare(b.hash));
    }

    removeShard(shardId) {
        this.shards.delete(shardId);
        this.ring = this.ring.filter(n => n.shardId !== shardId);
    }

    getShard(key) {
        const hash = createHash('md5').update(String(key)).digest('hex');

        // Binary search for the first node >= hash
        let low = 0;
        let high = this.ring.length - 1;

        while (low < high) {
            const mid = Math.floor((low + high) / 2);
            if (this.ring[mid].hash < hash) low = mid + 1;
            else high = mid;
        }

        const node = this.ring[low] || this.ring[0];
        return this.shards.get(node.shardId);
    }

    async query(key, sql, params) {
        const pool = this.getShard(key);
        return pool.query(sql, params);
    }
}
```

## Range-Based Sharding

```javascript
class RangeShardRouter {
    constructor(shards) {
        this.shards = shards.map(s => ({
            rangeStart: s.rangeStart,
            rangeEnd: s.rangeEnd,
            pool: new Pool({ /* ... */ }),
        }));
        this.shards.sort((a, b) => a.rangeStart - b.rangeStart);
    }

    getShard(value) {
        for (const shard of this.shards) {
            if (value >= shard.rangeStart && value <= shard.rangeEnd) {
                return shard;
            }
        }
        throw new Error(`No shard for value: ${value}`);
    }

    getShardsForRange(start, end) {
        return this.shards.filter(s =>
            s.rangeStart <= end && s.rangeEnd >= start
        );
    }

    async query(sql, params, options = {}) {
        if (options.shardKey !== undefined) {
            const shard = this.getShard(options.shardKey);
            return shard.pool.query(sql, params);
        }

        // Range query across shards
        const shards = this.getShardsForRange(
            options.rangeStart ?? 0,
            options.rangeEnd ?? Infinity
        );

        const results = await Promise.all(
            shards.map(s => s.pool.query(sql, params))
        );

        return results.flatMap(r => r.rows);
    }
}
```

## Shard Rebalancing

```javascript
class ShardRebalancer {
    constructor(router, options = {}) {
        this.router = router;
        this.batchSize = options.batchSize || 1000;
        this.maxConcurrency = options.maxConcurrency || 5;
    }

    async migrateShard(sourceShard, targetShard, shardKeyRange) {
        console.log(`Migrating data from shard ${sourceShard.id} to ${targetShard.id}`);

        let lastKey = shardKeyRange.start;
        let totalMigrated = 0;

        while (true) {
            const { rows } = await sourceShard.pool.query(
                'SELECT * FROM users WHERE id > $1 ORDER BY id LIMIT $2',
                [lastKey, this.batchSize]
            );

            if (rows.length === 0) break;

            // Insert into target
            const client = await targetShard.pool.connect();
            try {
                await client.query('BEGIN');

                for (const row of rows) {
                    await client.query(
                        'INSERT INTO users (id, name, email) VALUES ($1, $2, $3) ON CONFLICT (id) DO NOTHING',
                        [row.id, row.name, row.email]
                    );
                }

                await client.query('COMMIT');
            } catch (err) {
                await client.query('ROLLBACK');
                throw err;
            } finally {
                client.release();
            }

            // Delete from source
            const ids = rows.map(r => r.id);
            await sourceShard.pool.query(
                `DELETE FROM users WHERE id = ANY($1)`,
                [ids]
            );

            lastKey = rows[rows.length - 1].id;
            totalMigrated += rows.length;
            console.log(`Migrated ${totalMigrated} records`);
        }

        return totalMigrated;
    }
}
```

## Best Practices Checklist

- [ ] Choose shard key based on query patterns and distribution
- [ ] Use consistent hashing to minimize redistribution on scaling
- [ ] Implement scatter-gather for cross-shard queries
- [ ] Plan for shard rebalancing as data grows
- [ ] Use local transactions (avoid distributed transactions)
- [ ] Monitor shard size distribution
- [ ] Keep related data on the same shard

## Cross-References

- See [Read Replicas](../02-database-performance-optimization/05-read-replicas-sharding.md) for replication
- See [Connection Pooling](../01-database-integration-patterns/04-connection-pooling.md) for pool management
- See [Microservices DB](./03-microservices-database.md) for service-level patterns

## Next Steps

Continue to [Database Clustering](./04-database-clustering.md) for HA patterns.
