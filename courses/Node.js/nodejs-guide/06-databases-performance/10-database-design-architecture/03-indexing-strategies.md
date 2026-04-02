# Database Indexing Strategies and Best Practices

## What You'll Learn

- Index types and their use cases
- Composite index design
- Index optimization techniques
- Index monitoring and maintenance
- Common indexing mistakes

## PostgreSQL Index Types

```sql
-- B-tree (default, most common)
-- Good for: equality, range, sorting, LIKE 'prefix%'
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_created_at ON users(created_at DESC);

-- Composite index (column order matters!)
-- Rule: equality columns first, then range, then sort
CREATE INDEX idx_users_role_active_created 
    ON users(role, is_active, created_at DESC);

-- Partial index (filtered)
-- Only index rows that match the condition
CREATE INDEX idx_users_active 
    ON users(email) WHERE is_active = true;

-- Expression index
CREATE INDEX idx_users_lower_email 
    ON users(LOWER(email));

-- GIN index (for arrays, JSONB, full-text)
CREATE INDEX idx_posts_tags ON posts USING GIN(tags);
CREATE INDEX idx_posts_metadata ON posts USING GIN(metadata jsonb_path_ops);

-- GiST index (for geometric, range, full-text)
CREATE INDEX idx_events_time_range ON events USING GIST(time_range);

-- Hash index (equality only, faster than B-tree for equality)
CREATE INDEX idx_sessions_token ON sessions USING HASH(token);

-- Covering index (INCLUDE - index-only scans)
CREATE INDEX idx_orders_user_date 
    ON orders(user_id, created_at DESC) 
    INCLUDE (total, status);
```

## Index Design Patterns

```javascript
// Analyze query patterns to design indexes
class IndexAnalyzer {
    constructor(pool) {
        this.pool = pool;
    }

    async findMissingIndexes() {
        const { rows } = await this.pool.query(`
            SELECT 
                schemaname, tablename, attname, n_distinct, correlation,
                most_common_vals, most_common_freqs
            FROM pg_stats
            WHERE schemaname = 'public'
            ORDER BY n_distinct DESC
        `);
        return rows;
    }

    async findUnusedIndexes() {
        const { rows } = await this.pool.query(`
            SELECT 
                schemaname, tablename, indexname,
                idx_scan as scans,
                pg_size_pretty(pg_relation_size(indexrelid)) as size
            FROM pg_stat_user_indexes
            WHERE idx_scan = 0 
                AND indexrelname NOT LIKE '%pkey%'
            ORDER BY pg_relation_size(indexrelid) DESC
        `);
        return rows;
    }

    async findDuplicateIndexes() {
        const { rows } = await this.pool.query(`
            SELECT 
                pg_size_pretty(SUM(pg_relation_size(idx))::bigint) as size,
                array_agg(indexname::text) as indexes,
                array_agg(idx::text) as index_ids
            FROM pg_index i
            JOIN pg_class c ON c.oid = i.indexrelid
            JOIN pg_namespace n ON n.oid = c.relnamespace
            WHERE n.nspname = 'public'
            GROUP BY indrelid, indkey
            HAVING COUNT(*) > 1
        `);
        return rows;
    }

    async getIndexUsageStats() {
        const { rows } = await this.pool.query(`
            SELECT 
                schemaname, tablename, indexname,
                idx_scan as scans,
                idx_tup_read as tuples_read,
                idx_tup_fetch as tuples_fetched,
                pg_size_pretty(pg_relation_size(indexrelid)) as size
            FROM pg_stat_user_indexes
            ORDER BY idx_scan DESC
        `);
        return rows;
    }

    async suggestIndexes() {
        // Find sequential scans on large tables
        const { rows } = await this.pool.query(`
            SELECT 
                relname as table_name,
                seq_scan,
                seq_tup_read,
                idx_scan,
                n_live_tup as row_count,
                CASE WHEN seq_scan > 0 
                    THEN seq_tup_read / seq_scan 
                    ELSE 0 
                END as avg_rows_per_scan
            FROM pg_stat_user_tables
            WHERE seq_scan > 100
                AND n_live_tup > 10000
            ORDER BY seq_tup_read DESC
            LIMIT 20
        `);
        return rows.map(r => ({
            table: r.table_name,
            suggestion: `Consider adding indexes to ${r.table_name} (${r.seq_scan} seq scans, avg ${r.avg_rows_per_scan} rows/scan)`,
        }));
    }
}
```

## MongoDB Indexing

```javascript
import mongoose from 'mongoose';

const userSchema = new mongoose.Schema({
    name: String,
    email: String,
    age: Number,
    role: String,
    isActive: Boolean,
    createdAt: Date,
    tags: [String],
    profile: {
        city: String,
        country: String,
    },
});

// Single field index
userSchema.index({ email: 1 }, { unique: true });

// Compound index
userSchema.index({ role: 1, isActive: 1, createdAt: -1 });

// Text index for search
userSchema.index({ name: 'text', 'profile.city': 'text' });

// Multikey index (arrays)
userSchema.index({ tags: 1 });

// TTL index (auto-delete)
const sessionSchema = new mongoose.Schema({
    token: String,
    createdAt: { type: Date, default: Date.now, expires: 3600 }, // TTL
});

// Partial index
userSchema.index(
    { email: 1 },
    { partialFilterExpression: { isActive: true } }
);

// Index with collation
userSchema.index(
    { name: 1 },
    { collation: { locale: 'en', strength: 2 } } // Case-insensitive
);
```

## Index Performance Analysis

```
Index Performance Guidelines:
─────────────────────────────────────────────
Index Type        Read Speed  Write Impact  Size    Use Case
─────────────────────────────────────────────
B-tree            Fast        Low           Medium  General purpose
Hash              Fast        Low           Small   Equality only
GIN               Fast        High          Large   Arrays, JSONB
GiST              Medium      High          Large   Geometric, ranges
Partial           Fast        Very Low      Small   Filtered data
Expression        Fast        Low           Medium  Computed values
Covering          Fastest     Low           Medium  Index-only scans

Index maintenance:
├── REINDEX periodically (or use CONCURRENTLY)
├── ANALYZE after bulk data changes
├── Monitor index bloat
├── Remove unused indexes
└── Keep index count per table reasonable (5-10 max)
```

## Best Practices Checklist

- [ ] Index columns used in WHERE, JOIN, ORDER BY
- [ ] Use composite indexes with correct column order
- [ ] Create partial indexes for filtered queries
- [ ] Use covering indexes for hot queries
- [ ] Remove unused indexes (they slow down writes)
- [ ] Monitor index usage with pg_stat_user_indexes
- [ ] Run ANALYZE after bulk data changes
- [ ] Keep composite indexes under 4-5 columns

## Cross-References

- See [Query Optimization](../02-database-performance-optimization/01-query-optimization.md) for queries
- See [Normalization](./02-normalization-denormalization.md) for schema patterns
- See [Schema Design](./01-schema-design.md) for design basics

## Next Steps

Continue to [Database Evolution](./04-database-evolution.md) for schema change management.
