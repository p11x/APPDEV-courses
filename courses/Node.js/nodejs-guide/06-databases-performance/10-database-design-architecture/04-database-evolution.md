# Database Evolution and Schema Change Management

## What You'll Learn

- Schema evolution strategies
- Backward-compatible schema changes
- Feature flag driven schema changes
- Schema documentation standards
- Database architecture patterns

## Schema Evolution Patterns

```javascript
class SchemaEvolution {
    constructor(pool) {
        this.pool = pool;
    }

    // Add column safely (backward-compatible)
    async addColumn(table, column, type, defaultValue) {
        // Step 1: Add nullable column
        await this.pool.query(
            `ALTER TABLE ${table} ADD COLUMN ${column} ${type} DEFAULT ${defaultValue}`
        );

        // Step 2: Backfill existing rows (in batches)
        let updated = 0;
        while (true) {
            const { rowCount } = await this.pool.query(`
                UPDATE ${table} SET ${column} = ${defaultValue}
                WHERE ${column} IS NULL AND id IN (
                    SELECT id FROM ${table} WHERE ${column} IS NULL LIMIT 1000
                )
            `);
            if (rowCount === 0) break;
            updated += rowCount;
        }

        console.log(`Added column ${column} to ${table}, backfilled ${updated} rows`);
    }

    // Rename column safely (zero-downtime)
    async renameColumn(table, oldName, newName) {
        // Phase 1: Add new column
        const { rows } = await this.pool.query(
            `SELECT data_type, column_default FROM information_schema.columns 
             WHERE table_name = $1 AND column_name = $2`,
            [table, oldName]
        );

        await this.pool.query(
            `ALTER TABLE ${table} ADD COLUMN ${newName} ${rows[0].data_type} DEFAULT ${rows[0].column_default}`
        );

        // Phase 2: Copy data
        await this.pool.query(
            `UPDATE ${table} SET ${newName} = ${oldName}`
        );

        // Phase 3: Application code now uses both columns
        // Phase 4: After deployment, drop old column
        // await this.pool.query(`ALTER TABLE ${table} DROP COLUMN ${oldName}`);
    }

    // Split table safely
    async splitTable(sourceTable, newTable, columns) {
        // Create new table
        const columnDefs = columns.map(col => {
            const { rows } = this.pool.query(
                `SELECT data_type, is_nullable, column_default 
                 FROM information_schema.columns 
                 WHERE table_name = $1 AND column_name = $2`,
                [sourceTable, col]
            );
            return `${col} ${rows[0].data_type}`;
        });

        await this.pool.query(`CREATE TABLE ${newTable} (id SERIAL PRIMARY KEY, ${columnDefs.join(', ')})`);

        // Migrate data
        await this.pool.query(`
            INSERT INTO ${newTable} (${columns.join(', ')})
            SELECT ${columns.join(', ')} FROM ${sourceTable}
        `);
    }
}
```

## Feature Flag Driven Schema Changes

```javascript
class FeatureFlagSchemaManager {
    constructor(pool, featureFlags) {
        this.pool = pool;
        this.flags = featureFlags;
    }

    async migrateWithFlag(flagName, migration) {
        if (!this.flags.isEnabled(flagName)) {
            console.log(`Feature ${flagName} disabled, skipping migration`);
            return;
        }

        console.log(`Running migration for feature: ${flagName}`);
        await migration(this.pool);
    }

    async applyAllPending() {
        const migrations = [
            {
                flag: 'enable_user_profiles',
                migration: async (pool) => {
                    await pool.query(`
                        CREATE TABLE IF NOT EXISTS user_profiles (
                            user_id INT PRIMARY KEY REFERENCES users(id),
                            bio TEXT,
                            avatar_url TEXT,
                            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                        )
                    `);
                },
            },
            {
                flag: 'enable_order_analytics',
                migration: async (pool) => {
                    await pool.query(`
                        ALTER TABLE orders ADD COLUMN IF NOT EXISTS analytics_data JSONB DEFAULT '{}'
                    `);
                },
            },
        ];

        for (const { flag, migration } of migrations) {
            await this.migrateWithFlag(flag, migration);
        }
    }
}
```

## Database Architecture Patterns

```
Database Architecture Patterns:
─────────────────────────────────────────────
Pattern          Pros                    Cons
─────────────────────────────────────────────
Monolith DB      Simple, ACID            Hard to scale
Shared DB        Easy to start           Coupling, conflicts
DB per Service   Independent scaling     Data consistency
Event Sourcing   Full audit trail        Complex queries
CQRS             Optimized reads/writes  Complexity
Polyglot         Right tool per task     Operational overhead

Architecture evolution path:
Phase 1: Monolith DB (start here)
Phase 2: Read replicas
Phase 3: Caching layer
Phase 4: Database per service (if needed)
Phase 5: Event sourcing (if needed)
```

## Schema Documentation Standard

```sql
-- Table documentation
COMMENT ON TABLE orders IS 'Customer orders with line items and payment info';

-- Column documentation
COMMENT ON COLUMN orders.status IS 'Order status: pending, confirmed, shipped, delivered, cancelled';
COMMENT ON COLUMN orders.total IS 'Order total in cents (integer) to avoid floating point issues';

-- Index documentation
COMMENT ON INDEX idx_orders_user_date IS 'Covers user order history queries (user_id + date ordering)';

-- Constraint documentation
COMMENT ON CONSTRAINT orders_total_positive ON orders IS 'Ensures order total is never negative';
```

## Best Practices Checklist

- [ ] Make all schema changes backward-compatible
- [ ] Use feature flags for gradual rollout
- [ ] Document schema decisions and tradeoffs
- [ ] Version control all migration scripts
- [ ] Test migrations against production-like data
- [ ] Plan for rollback of every migration
- [ ] Keep transactional migrations short
- [ ] Use advisory locks for concurrent migration safety

## Cross-References

- See [Migration Management](../01-database-integration-patterns/07-migration-schema-management.md) for tools
- See [Schema Design](./01-schema-design.md) for design principles
- See [Microservices DB](../05-scalability-patterns/03-microservices-database.md) for service patterns

## Next Steps

This completes the comprehensive Chapter 6 database and performance guide. Review all sections and apply patterns relevant to your application's specific requirements.
