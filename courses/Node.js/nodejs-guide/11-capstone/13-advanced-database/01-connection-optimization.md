# Advanced Database Patterns for NodeMark

## What You'll Build In This File

Advanced database patterns including connection optimization, migration management, caching layers, and multi-tenancy support.

## Connection Pool Optimization

```javascript
// src/db/index.js — Optimized connection pool
import { Pool } from 'pg';
import { config } from '../config/index.js';

const pool = new Pool({
    host: config.db.host,
    port: config.db.port,
    database: config.db.name,
    user: config.db.user,
    password: config.db.password,

    // Pool sizing
    max: config.db.poolMax || 20,
    min: config.db.poolMin || 2,

    // Timeouts
    idleTimeoutMillis: 30000,
    connectionTimeoutMillis: 5000,
    statement_timeout: 30000,

    // Connection identification
    application_name: 'nodemark',
});

// Query wrapper with timing
export async function query(text, params) {
    const start = performance.now();
    try {
        const result = await pool.query(text, params);
        const duration = performance.now() - start;

        if (duration > 1000) {
            console.warn(`Slow query (${duration.toFixed(1)}ms):`, text.slice(0, 80));
        }

        return result;
    } catch (err) {
        const duration = performance.now() - start;
        console.error(`Query failed (${duration.toFixed(1)}ms):`, err.message);
        throw err;
    }
}

// Transaction helper
export async function transaction(callback) {
    const client = await pool.connect();
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

// Pool monitoring
export function getPoolStats() {
    return {
        totalCount: pool.totalCount,
        idleCount: pool.idleCount,
        waitingCount: pool.waitingCount,
    };
}

// Graceful shutdown
export async function closePool() {
    await pool.end();
}
```

## Migration Manager

```javascript
// scripts/migrate.js — Migration runner with locking
import { query, pool, transaction } from '../src/db/index.js';
import fs from 'node:fs/promises';
import path from 'node:path';

class MigrationManager {
    constructor(migrationsDir) {
        this.migrationsDir = migrationsDir;
    }

    async init() {
        await query(`
            CREATE TABLE IF NOT EXISTS schema_migrations (
                version VARCHAR(255) PRIMARY KEY,
                name VARCHAR(255) NOT NULL,
                applied_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        `);
    }

    async getApplied() {
        const { rows } = await query(
            'SELECT version FROM schema_migrations ORDER BY version'
        );
        return new Set(rows.map(r => r.version));
    }

    async getPending() {
        const applied = await this.getApplied();
        const files = await fs.readdir(this.migrationsDir);
        const migrations = files
            .filter(f => f.endsWith('.sql'))
            .map(f => ({
                version: f.split('_')[0],
                name: f.replace('.sql', ''),
                path: path.join(this.migrationsDir, f),
            }))
            .sort((a, b) => a.version.localeCompare(b.version));

        return migrations.filter(m => !applied.has(m.version));
    }

    async runPending() {
        const pending = await this.getPending();

        if (pending.length === 0) {
            console.log('No pending migrations');
            return;
        }

        for (const migration of pending) {
            console.log(`Applying: ${migration.name}`);

            const sql = await fs.readFile(migration.path, 'utf-8');

            await transaction(async (client) => {
                await client.query(sql);
                await client.query(
                    'INSERT INTO schema_migrations (version, name) VALUES ($1, $2)',
                    [migration.version, migration.name]
                );
            });

            console.log(`Applied: ${migration.name}`);
        }

        console.log(`Applied ${pending.length} migrations`);
    }

    async rollback(steps = 1) {
        const { rows } = await query(
            'SELECT version, name FROM schema_migrations ORDER BY version DESC LIMIT $1',
            [steps]
        );

        for (const migration of rows) {
            console.log(`Rolling back: ${migration.name}`);
            // In production, you'd have .down.sql files
            await query('DELETE FROM schema_migrations WHERE version = $1', [migration.version]);
        }
    }
}

// CLI usage
const manager = new MigrationManager('./src/db/migrations');
await manager.init();

const command = process.argv[2];
if (command === 'up') await manager.runPending();
else if (command === 'down') await manager.rollback(parseInt(process.argv[3] || 1));
else if (command === 'status') {
    const pending = await manager.getPending();
    console.log(`Pending migrations: ${pending.length}`);
    pending.forEach(m => console.log(`  - ${m.name}`));
}
```

## Database Caching Layer

```javascript
// src/db/cache.js — Query-level caching with Redis
import { createClient } from 'redis';

const redis = createClient({ url: process.env.REDIS_URL });
await redis.connect();

export class CachedRepository {
    constructor(prefix, defaultTTL = 300) {
        this.prefix = prefix;
        this.defaultTTL = defaultTTL;
    }

    cacheKey(method, params) {
        return `${this.prefix}:${method}:${JSON.stringify(params)}`;
    }

    async cached(key, fetcher, ttl) {
        const cached = await redis.get(key);
        if (cached) return JSON.parse(cached);

        const data = await fetcher();
        await redis.set(key, JSON.stringify(data), { EX: ttl || this.defaultTTL });
        return data;
    }

    async invalidate(pattern) {
        let cursor = 0;
        do {
            const result = await redis.scan(cursor, {
                MATCH: `${this.prefix}:${pattern}*`,
                COUNT: 100,
            });
            cursor = result.cursor;
            if (result.keys.length) await redis.del(result.keys);
        } while (cursor !== 0);
    }

    // Usage: cache user bookmarks
    async getUserBookmarks(userId, params) {
        const key = this.cacheKey('list', { userId, ...params });
        return this.cached(key, async () => {
            const { rows } = await query(
                'SELECT * FROM bookmarks WHERE user_id = $1 ORDER BY created_at DESC LIMIT $2 OFFSET $3',
                [userId, params.limit || 20, params.offset || 0]
            );
            return rows;
        }, 60);
    }
}

const bookmarkCache = new CachedRepository('bookmarks', 60);

// Invalidate on write
async function createBookmark(userId, data) {
    const result = await query(
        'INSERT INTO bookmarks (user_id, title, url) VALUES ($1, $2, $3) RETURNING *',
        [userId, data.title, data.url]
    );
    await bookmarkCache.invalidate(`list:${userId}*`);
    return result.rows[0];
}
```

## How It Connects

- Connection pooling follows [06-databases-performance](../../../06-databases-performance/)
- Caching follows [16-caching-redis](../../../16-caching-redis/) patterns
- Migrations follow [06-databases-performance/01-database-integration-patterns/07-migration-schema-management.md](../../../06-databases-performance/01-database-integration-patterns/07-migration-schema-management.md)

## Common Mistakes

- Not setting connection pool limits (exhausts database connections)
- Not using transactions for multi-step operations
- Caching without invalidation strategy
- Not monitoring slow queries

## Try It Yourself

### Exercise 1: Optimize Pool
Set up pool monitoring and test with concurrent requests.

### Exercise 2: Add Caching
Add caching to the bookmarks list endpoint and measure improvement.

### Exercise 3: Create Migration
Write a migration to add a `favorites` table.

## Next Steps

Continue to [14-advanced-auth/01-mfa-implementation.md](../14-advanced-auth/01-mfa-implementation.md).
