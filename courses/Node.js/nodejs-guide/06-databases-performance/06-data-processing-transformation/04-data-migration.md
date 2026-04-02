# Data Migration and Synchronization

## What You'll Learn

- Database-to-database migration
- Incremental data synchronization
- Data transformation during migration
- Migration verification and rollback
- Cross-database sync patterns

## Database Migration Runner

```javascript
class DataMigration {
    constructor(sourcePool, targetPool, options = {}) {
        this.source = sourcePool;
        this.target = targetPool;
        this.batchSize = options.batchSize || 1000;
        this.maxConcurrency = options.maxConcurrency || 3;
    }

    async migrateTable(tableName, options = {}) {
        const { columns, transform, where } = options;
        const columnList = columns ? columns.join(', ') : '*';
        const whereClause = where ? `WHERE ${where}` : '';

        let lastId = 0;
        let totalMigrated = 0;

        while (true) {
            const { rows } = await this.source.query(
                `SELECT ${columnList} FROM ${tableName} WHERE id > $1 ${whereClause ? 'AND ' + whereClause.replace('WHERE ', '') : ''} ORDER BY id LIMIT $2`,
                [lastId, this.batchSize]
            );

            if (rows.length === 0) break;

            const data = transform ? rows.map(transform) : rows;

            const columnsToInsert = Object.keys(data[0]);
            const placeholders = data.map((_, rowIdx) => {
                const start = rowIdx * columnsToInsert.length + 1;
                return `(${columnsToInsert.map((_, ci) => `$${start + ci}`).join(', ')})`;
            }).join(', ');

            const values = data.flatMap(row => columnsToInsert.map(col => row[col]));

            await this.target.query(
                `INSERT INTO ${tableName} (${columnsToInsert.join(', ')}) VALUES ${placeholders} ON CONFLICT DO NOTHING`,
                values
            );

            lastId = rows[rows.length - 1].id;
            totalMigrated += rows.length;
            console.log(`${tableName}: migrated ${totalMigrated} rows`);
        }

        return totalMigrated;
    }

    async migrateAll(tables) {
        const results = {};
        for (const table of tables) {
            results[table.name] = await this.migrateTable(table.name, table.options);
        }
        return results;
    }

    async verify(tableName) {
        const [sourceCount, targetCount] = await Promise.all([
            this.source.query(`SELECT COUNT(*) as count FROM ${tableName}`),
            this.target.query(`SELECT COUNT(*) as count FROM ${tableName}`),
        ]);

        return {
            table: tableName,
            sourceCount: parseInt(sourceCount.rows[0].count),
            targetCount: parseInt(targetCount.rows[0].count),
            match: sourceCount.rows[0].count === targetCount.rows[0].count,
        };
    }
}
```

## Change Data Capture (CDC)

```javascript
class CDCSync {
    constructor(sourcePool, targetPool) {
        this.source = sourcePool;
        this.target = targetPool;
    }

    async setupCDC(tableName) {
        // Create tracking table
        await this.source.query(`
            CREATE TABLE IF NOT EXISTS cdc_${tableName} (
                id SERIAL PRIMARY KEY,
                operation VARCHAR(10),
                record_id INTEGER,
                data JSONB,
                captured_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                synced BOOLEAN DEFAULT false
            )
        `);

        // Create trigger
        await this.source.query(`
            CREATE OR REPLACE FUNCTION cdc_${tableName}_trigger()
            RETURNS TRIGGER AS $$
            BEGIN
                IF TG_OP = 'DELETE' THEN
                    INSERT INTO cdc_${tableName} (operation, record_id, data)
                    VALUES ('DELETE', OLD.id, row_to_json(OLD)::jsonb);
                    RETURN OLD;
                ELSIF TG_OP = 'UPDATE' THEN
                    INSERT INTO cdc_${tableName} (operation, record_id, data)
                    VALUES ('UPDATE', NEW.id, row_to_json(NEW)::jsonb);
                    RETURN NEW;
                ELSIF TG_OP = 'INSERT' THEN
                    INSERT INTO cdc_${tableName} (operation, record_id, data)
                    VALUES ('INSERT', NEW.id, row_to_json(NEW)::jsonb);
                    RETURN NEW;
                END IF;
            END;
            $$ LANGUAGE plpgsql;
        `);

        await this.source.query(`
            DROP TRIGGER IF EXISTS cdc_trigger_${tableName} ON ${tableName};
            CREATE TRIGGER cdc_trigger_${tableName}
            AFTER INSERT OR UPDATE OR DELETE ON ${tableName}
            FOR EACH ROW EXECUTE FUNCTION cdc_${tableName}_trigger();
        `);
    }

    async syncChanges(tableName) {
        const { rows: changes } = await this.source.query(
            `SELECT * FROM cdc_${tableName} WHERE synced = false ORDER BY id LIMIT 1000`
        );

        if (changes.length === 0) return { synced: 0 };

        for (const change of changes) {
            switch (change.operation) {
                case 'INSERT':
                    await this.target.query(
                        `INSERT INTO ${tableName} SELECT * FROM json_populate_record(null::${tableName}, $1)`,
                        [JSON.stringify(change.data)]
                    );
                    break;
                case 'UPDATE':
                    const data = change.data;
                    const fields = Object.keys(data).filter(k => k !== 'id');
                    const setClause = fields.map((f, i) => `${f} = $${i + 2}`).join(', ');
                    await this.target.query(
                        `UPDATE ${tableName} SET ${setClause} WHERE id = $1`,
                        [data.id, ...fields.map(f => data[f])]
                    );
                    break;
                case 'DELETE':
                    await this.target.query(`DELETE FROM ${tableName} WHERE id = $1`, [change.record_id]);
                    break;
            }
        }

        const ids = changes.map(c => c.id);
        await this.source.query(
            `UPDATE cdc_${tableName} SET synced = true WHERE id = ANY($1)`,
            [ids]
        );

        return { synced: changes.length };
    }
}
```

## Best Practices Checklist

- [ ] Use incremental migration for large tables
- [ ] Verify row counts after migration
- [ ] Implement CDC for real-time sync
- [ ] Use batch operations to avoid overwhelming target
- [ ] Handle schema differences between source and target
- [ ] Test migration with production-like data
- [ ] Maintain rollback capability

## Cross-References

- See [ETL Pipelines](./02-etl-pipelines.md) for data pipelines
- See [Bulk Operations](../02-database-performance-optimization/04-batch-operations-bulk.md) for batch ops
- See [Streaming Data](./01-streaming-data.md) for stream processing

## Next Steps

Continue to [Data Compression](./05-data-compression.md) for storage optimization.
