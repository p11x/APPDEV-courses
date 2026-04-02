# Disaster Recovery and Business Continuity

## What You'll Learn

- Backup and recovery strategies
- High availability patterns
- Failover implementation
- RTO and RPO planning
- Disaster recovery testing

## Backup Strategy

```javascript
// scripts/backup.js
import { exec } from 'node:child_process';
import { promisify } from 'node:util';
import { S3Client, PutObjectCommand } from '@aws-sdk/client-s3';

const execAsync = promisify(exec);
const s3 = new S3Client({ region: 'us-east-1' });

class BackupManager {
    constructor(config) {
        this.config = config;
    }

    async backupPostgres() {
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const filename = `backup-${timestamp}.sql.gz`;
        const tmpPath = `/tmp/${filename}`;

        // Create compressed backup
        await execAsync(
            `pg_dump -h ${this.config.dbHost} -U ${this.config.dbUser} -d ${this.config.dbName} | gzip > ${tmpPath}`
        );

        // Upload to S3
        const fileBuffer = await fs.readFile(tmpPath);
        await s3.send(new PutObjectCommand({
            Bucket: this.config.backupBucket,
            Key: `postgres/${filename}`,
            Body: fileBuffer,
            ServerSideEncryption: 'aws:kms',
        }));

        // Cleanup local file
        await fs.unlink(tmpPath);

        console.log(`Backup uploaded: postgres/${filename}`);
        return filename;
    }

    async backupRedis() {
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');

        await execAsync(
            `redis-cli -h ${this.config.redisHost} --rdb /tmp/redis-${timestamp}.rdb`
        );

        const fileBuffer = await fs.readFile(`/tmp/redis-${timestamp}.rdb`);
        await s3.send(new PutObjectCommand({
            Bucket: this.config.backupBucket,
            Key: `redis/redis-${timestamp}.rdb`,
            Body: fileBuffer,
        }));

        await fs.unlink(`/tmp/redis-${timestamp}.rdb`);
    }

    async rotateBackups(retainDays = 30) {
        const cutoff = new Date(Date.now() - retainDays * 24 * 60 * 60 * 1000);
        // List and delete old backups
    }

    async schedule() {
        // Daily at 2 AM
        setInterval(async () => {
            try {
                await this.backupPostgres();
                await this.backupRedis();
                await this.rotateBackups(30);
            } catch (err) {
                console.error('Backup failed:', err);
                // Alert on failure
            }
        }, 24 * 60 * 60 * 1000);
    }
}
```

## Recovery Procedures

```javascript
class RecoveryManager {
    async restorePostgres(backupKey) {
        // Download from S3
        const { Body } = await s3.send(new GetObjectCommand({
            Bucket: this.config.backupBucket,
            Key: backupKey,
        }));

        // Decompress and restore
        const tmpPath = `/tmp/restore.sql.gz`;
        await fs.writeFile(tmpPath, Body);
        await execAsync(`gunzip -c ${tmpPath} | psql -h ${this.config.dbHost} -U ${this.config.dbUser} -d ${this.config.dbName}`);
        await fs.unlink(tmpPath);

        console.log(`Restored from: ${backupKey}`);
    }

    async restoreRedis(backupKey) {
        const { Body } = await s3.send(new GetObjectCommand({
            Bucket: this.config.backupBucket,
            Key: backupKey,
        }));

        await fs.writeFile('/tmp/dump.rdb', Body);
        await execAsync(`redis-cli -h ${this.config.redisHost} --rdb /tmp/dump.rdb`);
    }
}
```

## High Availability Configuration

```
HA Architecture:
─────────────────────────────────────────────
                    ┌───────────┐
                    │   CDN     │
                    └─────┬─────┘
                          │
                    ┌─────▼─────┐
                    │ Load      │
                    │ Balancer  │
                    └──┬─────┬──┘
                       │     │
              ┌────────▼┐  ┌▼────────┐
              │ App-1   │  │ App-2   │
              └────────┬┘  └┬────────┘
                       │    │
              ┌────────▼────▼────────┐
              │   Redis Sentinel     │
              │   (Primary+Replica)  │
              └────────┬─────┬───────┘
                       │     │
              ┌────────▼┐  ┌▼────────┐
              │ DB-1    │  │ DB-2    │
              │Primary  │  │Replica  │
              └─────────┘  └─────────┘

RTO/RPO Targets:
├── RTO (Recovery Time Objective): < 15 minutes
├── RPO (Recovery Point Objective): < 5 minutes
├── Backup frequency: Every 6 hours
├── Retention: 30 days
└── Cross-region replication: Yes
```

## Disaster Recovery Runbook

```markdown
# Disaster Recovery Runbook

## Scenario 1: Database Failure
1. Check replica status: `pg_stat_replication`
2. Promote replica: `pg_ctl promote`
3. Update application connection strings
4. Monitor for data loss (< RPO)
5. Create new replica from promoted primary

## Scenario 2: Application Outage
1. Check health endpoints
2. Review logs for errors
3. If code issue: rollback deployment
4. If resource issue: scale up/scale out
5. If infrastructure: failover to DR region

## Scenario 3: Complete Region Failure
1. Activate DR region
2. Update DNS to DR load balancer
3. Verify database replication lag
4. Run smoke tests
5. Monitor DR region stability
```

## Best Practices Checklist

- [ ] Automate backups (daily minimum)
- [ ] Store backups in multiple locations
- [ ] Test restore procedures regularly
- [ ] Define RTO and RPO for each service
- [ ] Implement automated failover
- [ ] Maintain a disaster recovery runbook
- [ ] Run DR drills quarterly
- [ ] Monitor replication lag

## Cross-References

- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for HA patterns
- See [Kubernetes](../03-container-orchestration/01-kubernetes-patterns.md) for K8s HA
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for alerting
- See [Security](../09-deployment-security/01-security-scanning.md) for backup security

## Next Steps

Continue to [Best Practices](../12-deployment-best-practices/01-deployment-checklist.md).
