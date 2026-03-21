# Build Triggers in Jenkins

## What this covers

This guide explains all the different ways to trigger Jenkins builds automatically. You'll learn about manual triggers, SCM polling, webhooks (GitHub/GitLab), upstream job triggers, and scheduled builds. The cron syntax is explained in detail.

## Prerequisites

- Completed previous guides in this section
- A job configured in Jenkins
- Understanding of build basics

## What Are Build Triggers?

Build triggers define **when** Jenkins should start a build. Without triggers, you must manually click "Build Now".

> Think of triggers as **alarms** — they tell Jenkins when to wake up and do work.

---

## Types of Build Triggers

### 1. Trigger Builds Remotely (Webhooks)

This allows external systems to trigger your build via a URL.

**Configuration**:
```
┌─────────────────────────────────────────────────────────────────────┐
│  Build Triggers                                                    │
│  ───────────────                                                   │
│                                                                     │
│  ☑ Trigger builds remotely (e.g., from scripts)                   │
│     Authentication Token:  [ my-secret-token ]                      │
│       ↓                                                             │
│     Any URL can trigger:                                           │
│     http://localhost:8080/job/my-job/build?token=my-secret-token  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**How to use**:
```bash
# Trigger via curl
curl http://localhost:8080/job/my-job/build?token=my-secret-token
```

**Use cases**:
- GitHub/GitLab webhooks
- Custom scripts
- CI/CD pipeline orchestration
- Cron jobs

**⚠️ Security Note**: Anyone with this URL can trigger your job. Use a long, random token.

---

### 2. Build After Other Projects Are Built

Trigger this job when another job completes.

**Configuration**:
```
┌─────────────────────────────────────────────────────────────────────┐
│  Build after other projects are built                             │
│     Projects to watch:  [ my-upstream-job ]                       │
│     ☑ Trigger only if build is stable                             │
│     ☐ Trigger even if unstable                                     │
│     ☐ Trigger even if build fails                                 │
└─────────────────────────────────────────────────────────────────────┘
```

**Use cases**:
- **Build chain**: Build → Test → Deploy
- Run tests only after successful build
- Deploy only after tests pass

**Example workflow**:
```
my-api (builds first) ──────────────► my-api-tests (runs after)
     │                                        │
     │                                        │
     └────────────────┬───────────────────────┘
                      ▼
               my-api-deploy (runs if tests pass)
```

---

### 3. Build Periodically

Run the job on a schedule (like cron).

**Configuration**:
```
┌─────────────────────────────────────────────────────────────────────┐
│  Build periodically                                                │
│     Schedule:  [ H/5 * * * * ]                                    │
└─────────────────────────────────────────────────────────────────────┘
```

**Example schedules**:
| Cron Expression | Meaning |
|----------------|---------|
| `H/5 * * * *` | Every 5 minutes (with hash) |
| `H * * * *` | Every hour |
| `H 0 * * *` | Every day at midnight |
| `H 2 * * 1-5` | Weekdays at 2 AM |
| `H * * * 1,3,5` | Monday, Wednesday, Friday |

---

### 4. Poll SCM

Check your Git repository for changes on a schedule.

**Configuration**:
```
┌─────────────────────────────────────────────────────────────────────┐
│  Poll SCM                                                          │
│     Schedule:  [ H/5 * * * * ]                                    │
│       ↓                                                             │
│     Check for new commits every 5 minutes                         │
└─────────────────────────────────────────────────────────────────────┘
```

**How it works**:
1. Jenkins checks Git repository at scheduled times
2. If new commits found since last build → trigger build
3. If no changes → do nothing

**⚠️ Note**: Webhooks are preferred over polling. Polling wastes resources checking for changes.

---

### 5. GitHub/GitLab Push Triggers (Recommended!)

This is the **most common** trigger for modern CI/CD.

When you push code to GitHub/GitLab, they send a webhook to Jenkins, triggering a build.

**No configuration needed in Jenkins** - it works automatically when:
1. Git plugin is installed
2. GitHub Branch Source Plugin or GitLab Plugin is installed
3. You configure a webhook in GitHub/GitLab

We'll cover this in detail in: **[GitHub Webhook Setup](../03-plugins-and-integrations/01-git-and-scm/03-github-webhook-setup.md)**

---

## Understanding Cron Syntax

Jenkins uses cron-like scheduling with a twist: the **H** (hash) symbol.

### Cron Expression Format

```
┌───────────── minute (0-59)
│ ┌─────────── hour (0-23)
│ │ ┌───────── day of month (1-31)
│ │ │ ┌─────── month (1-12)
│ │ │ │ ┌───── day of week (0-7, 0 and 7 are Sunday)
│ │ │ │ │
* * * * *
```

### Special Characters

| Character | Meaning | Example |
|-----------|---------|---------|
| `*` | Any value | `* * * * *` = every minute |
| `,` | List of values | `0 9,17 * * *` = 9 AM and 5 PM |
| `-` | Range | `0 9-17 * * *` = every hour 9 AM-5 PM |
| `/` | Step | `*/15 * * * *` = every 15 minutes |

### The H Symbol (IMPORTANT!)

The **H** (hash) is unique to Jenkins:

```
# Without H - all builds start at exact same time
# Could overload the system!
*/5 * * * * = 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 minutes

# With H - builds distributed across the minute
# Better for system health
H/5 * * * * = Random minute (e.g., 2, 7, 12, 18, 23, 29, 33, 41, 46, 52)
```

**Why H matters**:
- Prevents all jobs from running at the same time
- Distributes load across the time window
- Use H for any field except day-of-month (some say use H there too)

### Cron Expression Examples

| Expression | Meaning | When it Runs |
|------------|---------|--------------|
| `H/15 * * * *` | Every 15 minutes | Minutes: 3, 18, 33, 48 |
| `H * * * *` | Every hour | Random minute each hour |
| `H 0 * * *` | Once a day | Random minute at midnight |
| `H 2 * * 1-5` | Weekdays at 2 AM | Monday-Friday at ~2 AM |
| `H H * * *` | Once a day (hour also hashed) | Random time daily |
| `H H(0-8) * * *` | Night hours | 12 AM - 8 AM |
| `H H * * 0` | Once a week | Random time on Sunday |

---

## Cron Expression Breakdown Table

Here's a detailed breakdown:

```
Field        Allowed Values       Special Characters    Example
─────────────────────────────────────────────────────────────────────
Minute       0-59                 , - * /                H/10 (every 10 mins)
Hour         0-23                 , - * /                H(0-6) (midnight-6am)
Day of Month 1-31                 , - * /                H(1-15) (1st-15th)
Month        1-12 or JAN-DEC     , - * /                H (any month)
Day of Week  0-7 (0,7=SUN)       , - * /                1-5 (Mon-Fri)
```

---

## Practical Trigger Examples

### Example 1: CI/CD Pipeline with Staging and Production

```
Job: build-app
├── Trigger: GitHub webhook
└── Runs: On every push

Job: test-app
├── Trigger: Build after build-app (stable only)
└── Runs: After build-app succeeds

Job: deploy-staging
├── Trigger: Build after test-app (stable only)
└── Runs: After tests pass

Job: deploy-production
├── Trigger: Build periodically
└── Runs: Daily at 2 AM
```

### Example 2: Nightly Build

```
Job: nightly-build
├── Trigger: Build periodically: H 2 * * *
└── Runs: Around 2 AM every day
```

### Example 3: Code Every 10 Minutes

```
Job: check-for-updates
├── Trigger: Poll SCM: H/10 * * * *
└── Runs: Every 10 minutes, only if code changed
```

---

## Common Mistakes

### Mistake 1: No H Symbol

**Problem**: `*/5 * * * *` - All jobs trigger at same time
**Solution**: Use `H/5 * * * *` to distribute load

### Mistake 2: Polling Instead of Webhooks

**Problem**: Poll SCM every minute, checking Git for changes
**Solution**: Use GitHub/GitLab webhooks instead - they're instant and efficient

### Mistake 3: Too Frequent Builds

**Problem**: Build on every commit without batching
**Solution**: Use quiet period or configure webhook to batch commits

### Mistake 4: Forgetting the H

**Problem**: Jobs all run at :00, :15, :30, :45
**Solution**: Always use H in your cron expressions

---

## Recommended Trigger Setup

For most projects, use this combination:

1. **Primary**: GitHub/GitLab webhook (instant, efficient)
2. **Backup**: Poll SCM with `H/5 * * * *` (in case webhook fails)
3. **Scheduled**: Nightly `H 2 * * *` for full test suite

---

## Next Steps

- **[GitHub Webhook Setup](../03-plugins-and-integrations/01-git-and-scm/03-github-webhook-setup.md)** - Configure automatic builds on push
- **[Pipeline Basics](../02-pipelines/01-pipeline-basics/01-what-is-a-jenkinsfile.md)** - Learn about modern Pipelines
- **[Multibranch Pipelines](../03-plugins-and-integrations/01-git-and-scm/02-multibranch-pipeline.md)** - Auto-create jobs for each branch
