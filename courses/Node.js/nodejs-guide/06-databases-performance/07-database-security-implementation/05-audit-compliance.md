# Database Audit, Backup, and Compliance

## What You'll Learn

- Database audit logging setup
- Backup strategies and automation
- Compliance requirements (GDPR, SOC2)
- Data retention policies
- Disaster recovery planning

## Database Audit System

```javascript
class DatabaseAuditSystem {
    constructor(pool) {
        this.pool = pool;
    }

    async init() {
        await this.pool.query(`
            CREATE TABLE IF NOT EXISTS audit_events (
                id BIGSERIAL PRIMARY KEY,
                event_type VARCHAR(50) NOT NULL,
                table_name VARCHAR(255),
                record_id TEXT,
                action VARCHAR(20) NOT NULL,
                old_values JSONB,
                new_values JSONB,
                user_id INTEGER,
                user_role VARCHAR(50),
                ip_address INET,
                user_agent TEXT,
                query_hash VARCHAR(64),
                duration_ms INTEGER,
                success BOOLEAN DEFAULT true,
                error_message TEXT,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX idx_audit_table ON audit_events(table_name, created_at);
            CREATE INDEX idx_audit_user ON audit_events(user_id, created_at);
            CREATE INDEX idx_audit_action ON audit_events(action, created_at);
        `);
    }

    async log(event) {
        await this.pool.query(
            `INSERT INTO audit_events 
             (event_type, table_name, record_id, action, old_values, new_values, 
              user_id, user_role, ip_address, user_agent, duration_ms, success, error_message)
             VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13)`,
            [event.eventType, event.tableName, event.recordId, event.action,
             event.oldValues, event.newValues, event.userId, event.userRole,
             event.ipAddress, event.userAgent, event.durationMs, event.success, event.errorMessage]
        );
    }

    async query(filters = {}) {
        let sql = 'SELECT * FROM audit_events WHERE 1=1';
        const params = [];

        if (filters.tableName) {
            sql += ` AND table_name = $${params.length + 1}`;
            params.push(filters.tableName);
        }
        if (filters.userId) {
            sql += ` AND user_id = $${params.length + 1}`;
            params.push(filters.userId);
        }
        if (filters.from) {
            sql += ` AND created_at >= $${params.length + 1}`;
            params.push(filters.from);
        }
        if (filters.to) {
            sql += ` AND created_at <= $${params.length + 1}`;
            params.push(filters.to);
        }

        sql += ' ORDER BY created_at DESC LIMIT $' + (params.length + 1);
        params.push(filters.limit || 100);

        const { rows } = await this.pool.query(sql, params);
        return rows;
    }

    async getTableActivity(table, days = 30) {
        const { rows } = await this.pool.query(`
            SELECT 
                action,
                DATE(created_at) as date,
                COUNT(*) as count
            FROM audit_events
            WHERE table_name = $1 AND created_at > NOW() - INTERVAL '${days} days'
            GROUP BY action, DATE(created_at)
            ORDER BY date DESC
        `, [table]);
        return rows;
    }
}
```

## Automated Backup System

```javascript
import { exec } from 'node:child_process';
import { promisify } from 'node:util';
import fs from 'node:fs/promises';

const execAsync = promisify(exec);

class DatabaseBackup {
    constructor(config) {
        this.config = config;
        this.backupDir = config.backupDir || './backups';
    }

    async backupPostgres() {
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const filename = `${this.backupDir}/backup-${timestamp}.sql.gz`;

        await fs.mkdir(this.backupDir, { recursive: true });

        const command = `pg_dump -h ${this.config.host} -p ${this.config.port} -U ${this.config.user} -d ${this.config.database} -Fc | gzip > ${filename}`;
        
        const { stdout, stderr } = await execAsync(command, {
            env: { ...process.env, PGPASSWORD: this.config.password },
        });

        const stats = await fs.stat(filename);
        console.log(`Backup created: ${filename} (${(stats.size / 1024 / 1024).toFixed(1)}MB)`);

        return { filename, size: stats.size };
    }

    async backupMongoDB() {
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const backupPath = `${this.backupDir}/mongo-${timestamp}`;

        await fs.mkdir(this.backupDir, { recursive: true });

        await execAsync(
            `mongodump --uri="${this.config.uri}" --out="${backupPath}"`
        );

        console.log(`MongoDB backup created: ${backupPath}`);
        return { path: backupPath };
    }

    async rotateBackups(retainDays = 30) {
        const files = await fs.readdir(this.backupDir);
        const cutoff = Date.now() - retainDays * 24 * 60 * 60 * 1000;

        for (const file of files) {
            const filePath = `${this.backupDir}/${file}`;
            const stats = await fs.stat(filePath);
            if (stats.mtimeMs < cutoff) {
                await fs.unlink(filePath);
                console.log(`Deleted old backup: ${file}`);
            }
        }
    }

    scheduleBackup(cronExpression) {
        // Run backup daily at 2 AM
        const scheduleBackup = async () => {
            try {
                await this.backupPostgres();
                await this.rotateBackups();
            } catch (err) {
                console.error('Backup failed:', err.message);
            }
        };

        // Simple daily scheduler
        const now = new Date();
        const target = new Date(now);
        target.setHours(2, 0, 0, 0);
        if (target <= now) target.setDate(target.getDate() + 1);

        const delay = target - now;
        setTimeout(() => {
            scheduleBackup();
            setInterval(scheduleBackup, 24 * 60 * 60 * 1000);
        }, delay);
    }
}
```

## Data Retention Policy

```javascript
class DataRetentionPolicy {
    constructor(pool) {
        this.pool = pool;
        this.policies = new Map();
    }

    addPolicy(table, retentionDays, options = {}) {
        this.policies.set(table, {
            retentionDays,
            dateColumn: options.dateColumn || 'created_at',
            softDelete: options.softDelete !== false,
            archiveTable: options.archiveTable,
        });
    }

    async applyPolicies() {
        for (const [table, policy] of this.policies) {
            await this.applyPolicy(table, policy);
        }
    }

    async applyPolicy(table, policy) {
        const cutoff = new Date(Date.now() - policy.retentionDays * 24 * 60 * 60 * 1000);

        if (policy.archiveTable) {
            // Archive before deletion
            await this.pool.query(`
                INSERT INTO ${policy.archiveTable}
                SELECT * FROM ${table}
                WHERE ${policy.dateColumn} < $1
            `, [cutoff]);
        }

        if (policy.softDelete) {
            await this.pool.query(`
                UPDATE ${table} SET deleted_at = NOW()
                WHERE ${policy.dateColumn} < $1 AND deleted_at IS NULL
            `, [cutoff]);
        } else {
            const { rowCount } = await this.pool.query(`
                DELETE FROM ${table} WHERE ${policy.dateColumn} < $1
            `, [cutoff]);
            console.log(`Deleted ${rowCount} old records from ${table}`);
        }
    }
}

// Usage
const retention = new DataRetentionPolicy(pool);
retention.addPolicy('audit_events', 365, { softDelete: false });
retention.addPolicy('sessions', 30, { softDelete: false });
retention.addPolicy('logs', 90, { archiveTable: 'logs_archive' });
await retention.applyPolicies();
```

## Best Practices Checklist

- [ ] Implement audit logging for all sensitive operations
- [ ] Automate database backups with retention
- [ ] Test backup restoration regularly
- [ ] Implement data retention policies
- [ ] Comply with GDPR right-to-deletion
- [ ] Encrypt backup files
- [ ] Store backups in multiple locations
- [ ] Document disaster recovery procedures

## Cross-References

- See [Access Control](./02-access-control.md) for permissions
- See [Encryption](./04-encryption.md) for backup encryption
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for alerting

## Next Steps

Continue to [Performance Testing](../08-performance-testing-benchmarking/01-load-testing.md) for testing.
