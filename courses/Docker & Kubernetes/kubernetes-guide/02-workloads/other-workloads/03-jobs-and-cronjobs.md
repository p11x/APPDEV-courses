# Jobs and CronJobs

## Overview

Jobs create one or more pods that run to completion, ensuring successful termination. Unlike Deployments or StatefulSets where pods run indefinitely, Jobs are designed for finite tasks like batch processing, database migrations, or one-time data processing. CronJobs extend this with scheduled execution at specific times.

## Prerequisites

- Understanding of Kubernetes Pods
- Basic knowledge of container lifecycle
- Familiarity with container image building

## Core Concepts

### Job vs Deployment

| Aspect | Deployment | Job |
|--------|-----------|-----|
| Pod lifecycle | Long-running | Run to completion |
| Success criteria | Container keeps running | Exit code 0 |
| Restart policy | Always (default) | Never or OnFailure |
| Scaling | Replicas > 1 | completions/parallelism |

### Job Fields

- **completions**: Number of successful pod completions required (default: 1)
- **parallelism**: Number of pods to run in parallel (default: 1)
- **backoffLimit**: Number of retries before marking job as failed (default: 6)
- **activeDeadlineSeconds**: Maximum seconds job is allowed to run
- **ttlSecondsAfterFinished**: Seconds before completed job is cleaned up

### CronJob Fields

- **schedule**: Cron format (minute, hour, day, month, weekday)
- **successfulJobsHistoryLimit**: Number of successful jobs to keep (default: 3)
- **failedJobsHistoryLimit**: Number of failed jobs to keep (default: 1)
- **startingDeadlineSeconds**: Deadline for starting missed jobs
- **concurrencyPolicy**: Allow, Forbid, Replace (default: Allow)
- **timeZone**: Timezone for schedule (new in k8s 1.27)

## Step-by-Step Examples

### Creating a Job

```yaml
apiVersion: batch/v1
kind: Job
metadata:
  name: data-processor                     # Unique name within namespace
spec:
  completions: 3                          # Wait for 3 successful completions
  parallelism: 2                          # Run 2 pods in parallel
  backoffLimit: 4                         # Retry up to 4 times on failure
  activeDeadlineSeconds: 300              # Job times out after 5 minutes
  template:
    metadata:
      labels:
        app: data-processor
    spec:
      restartPolicy: OnFailure            # Only OnFailure or Never, not Always
      containers:
      - name: processor
        image: data-processor:v1.0
        command: ["/app/process", "--input", "/data/input.csv"]
        volumeMounts:
        - name: data
          mountPath: /data
      volumes:
      - name: data
        persistentVolumeClaim:
          claimName: input-pvc
```

### Creating a CronJob

```yaml
apiVersion: batch/v1
kind: CronJob
metadata:
  name: daily-backup
spec:
  schedule: "0 2 * * *"                   # Run at 2:00 AM daily
  timeZone: "America/New_York"            # Timezone (k8s 1.27+)
  successfulJobsHistoryLimit: 5            # Keep 5 successful jobs
  failedJobsHistoryLimit: 3                # Keep 3 failed jobs
  concurrencyPolicy: Forbid               # Don't run if previous still running
  startingDeadlineSeconds: 300             # Start within 5 min if missed
  jobTemplate:
    spec:
      template:
        metadata:
          labels:
            app: backup
        spec:
          restartPolicy: OnFailure
          containers:
          - name: backup
            image: backup-tool:v2.1
            command: ["/app/backup", "--database", "production"]
            env:
            - name: BACKUP_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: db-secrets
                  key: password
```

### Managing Jobs

```bash
# Create Job
kubectl apply -f job.yaml

# List jobs
# Shows active, successful, and failed counts
kubectl get jobs -o wide

# View job details
# Includes: completions, parallelism, backoffLimit status
kubectl describe job data-processor

# Check pods created by job
kubectl get pods -l job-name=data-processor

# View logs from job pod
kubectl logs job/data-processor

# Manually create job from CronJob
kubectl create job daily-backup-manual --from=cronjob/daily-backup

# Delete job
kubectl delete job data-processor
```

### Managing CronJobs

```bash
# Create CronJob
kubectl apply -f cronjob.yaml

# List CronJobs
kubectl get cronjob

# Describe for schedule info
kubectl describe cronjob daily-backup

# Trigger job immediately (without waiting for schedule)
kubectl create job --from=cronjob/daily-backup daily-backup-now

# Pause CronJob (no new jobs created)
kubectl pause cronjob daily-backup

# Unpause CronJob
kubectl unpause cronjob daily-backup

# Delete CronJob (existing jobs remain)
kubectl delete cronjob daily-backup
```

## Cron Schedule Format

```
┌───────────── minute (0 - 59)
│ ┌───────────── hour (0 - 23)
│ │ ┌───────────── day of month (1 - 31)
│ │ │ ┌───────────── month (1 - 12)
│ │ │ │ ┌───────────── day of week (0 - 6) (Sunday to Saturday)
│ │ │ │ │
* * * * *
```

Examples:
- `0 * * * *` - Every hour at minute 0
- `*/15 * * * *` - Every 15 minutes
- `0 0 * * *` - Daily at midnight
- `0 2 * * 0` - Weekly on Sunday at 2 AM
- `0 0 1 * *` - First day of every month at midnight

## Gotchas for Docker Users

- **Run-to-completion**: Unlike Docker containers which run indefinitely, Job containers must exit successfully
- **Restart policy**: Must be OnFailure or Never, not Always (default in Pod spec)
- **No automatic cleanup**: Jobs remain after completion unless TTL is configured or manually deleted
- **Cron schedule timezone**: New in k8s 1.27 - must specify timeZone explicitly
- **Concurrent jobs**: Default allows concurrent runs - may need concurrencyPolicy: Forbid

## Common Mistakes

- **Wrong restart policy**: Using restartPolicy: Always will cause infinite restarts
- **Not setting backoffLimit**: Failed jobs retry indefinitely without this limit
- **Forgetting timezone**: CronJob schedule may be interpreted in UTC without timeZone
- **No cleanup**: Completed jobs accumulate - set ttlSecondsAfterFinished
- **Concurrent execution**: Not setting concurrencyPolicy can cause overlapping runs

## Quick Reference

| Job Field | Purpose |
|-----------|---------|
| completions | Successful runs needed |
| parallelism | Parallel pods |
| backoffLimit | Max retries |
| activeDeadlineSeconds | Max runtime |
| ttlSecondsAfterFinished | Auto cleanup |

| CronJob Field | Purpose |
|---------------|---------|
| schedule | Cron expression |
| timeZone | TZ for schedule (1.27+) |
| concurrencyPolicy | Allow/Forbid/Replace |
| successfulJobsHistoryLimit | Keep N successes |
| failedJobsHistoryLimit | Keep N failures |

## What's Next

Continue to [Ingress Basics](../networking/ingress/01-ingress-basics.md) to learn about HTTP/S routing.
