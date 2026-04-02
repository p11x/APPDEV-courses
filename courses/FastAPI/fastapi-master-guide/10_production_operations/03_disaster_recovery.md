# Disaster Recovery

## Overview

Disaster recovery planning ensures FastAPI applications can recover from catastrophic failures.

## DR Strategy

### Backup and Recovery

```python
# Example 1: Automated backup system
from fastapi import FastAPI, BackgroundTasks
from datetime import datetime
import boto3
import subprocess

app = FastAPI()

class BackupManager:
    """Manage database backups"""

    def __init__(self):
        self.s3 = boto3.client('s3')
        self.bucket = "app-backups"

    async def create_backup(self, database: str) -> str:
        """Create database backup"""
        timestamp = datetime.utcnow().strftime("%Y%m%d_%H%M%S")
        filename = f"{database}_{timestamp}.sql.gz"

        # Create backup
        subprocess.run([
            "pg_dump",
            database,
            "|", "gzip",
            ">", f"/tmp/{filename}"
        ])

        # Upload to S3
        self.s3.upload_file(
            f"/tmp/{filename}",
            self.bucket,
            f"backups/{filename}"
        )

        return filename

    async def restore_backup(self, backup_file: str, database: str):
        """Restore from backup"""
        # Download from S3
        self.s3.download_file(
            self.bucket,
            f"backups/{backup_file}",
            f"/tmp/{backup_file}"
        )

        # Restore database
        subprocess.run([
            "gunzip", f"/tmp/{backup_file}",
            "|", "psql", database
        ])

backup_manager = BackupManager()

@app.post("/backup/")
async def create_backup(background_tasks: BackgroundTasks):
    """Trigger backup"""
    background_tasks.add_task(
        backup_manager.create_backup,
        "production_db"
    )
    return {"status": "backup_started"}

@app.post("/restore/")
async def restore_backup(backup_file: str):
    """Restore from backup"""
    await backup_manager.restore_restore(backup_file, "production_db")
    return {"status": "restored"}
```

### Failover Configuration

```python
# Example 2: Automatic failover
class FailoverManager:
    """Manage database failover"""

    def __init__(self):
        self.primary = create_engine(PRIMARY_DB_URL)
        self.replica = create_engine(REPLICA_DB_URL)
        self.current = "primary"

    def get_engine(self):
        """Get current active engine"""
        if self.current == "primary":
            return self.primary
        return self.replica

    async def check_health(self):
        """Check primary health"""
        try:
            with self.primary.connect() as conn:
                conn.execute("SELECT 1")
            return True
        except:
            return False

    async def failover(self):
        """Switch to replica"""
        if await self.check_health():
            return  # Primary is healthy

        self.current = "replica"
        # Promote replica to primary
        await promote_replica()

failover_manager = FailoverManager()

# Health check with failover
@app.get("/health")
async def health():
    """Health check with failover status"""
    primary_healthy = await failover_manager.check_health()

    return {
        "status": "healthy" if primary_healthy else "degraded",
        "current_db": failover_manager.current,
        "primary_healthy": primary_healthy
    }
```

## DR Testing

```python
# Example 3: DR testing procedures
class DRTester:
    """Disaster recovery testing"""

    async def test_backup_restore(self):
        """Test backup and restore"""
        # Create backup
        backup = await backup_manager.create_backup("test_db")

        # Simulate failure
        await simulate_database_failure()

        # Restore
        await backup_manager.restore_backup(backup, "test_db")

        # Verify
        assert await verify_data_integrity()

    async def test_failover(self):
        """Test failover procedure"""
        # Record initial state
        initial_data = await get_sample_data()

        # Simulate primary failure
        await failover_manager.failover()

        # Verify replica has data
        replica_data = await get_sample_data()
        assert replica_data == initial_data

dr_tester = DRTester()
```

## Summary

Disaster recovery ensures business continuity during failures.

## Next Steps

Continue learning about:
- [Business Continuity](./04_business_continuity.md)
- [Compliance Automation](./05_compliance_automation.md)
