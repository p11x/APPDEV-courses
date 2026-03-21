# Jenkins Dashboard Overview

## What this covers

This guide explains the Jenkins dashboard—the first thing you see when you log in. You'll learn what each panel displays, what executors are, and how to navigate the sidebar to access different Jenkins features.

## Prerequisites

- Jenkins installed and accessible via web browser
- Initial setup wizard completed
- Basic understanding of CI/CD concepts

## The Jenkins Dashboard

When you log into Jenkins, you see the main dashboard. Here's a detailed breakdown:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Jenkins     New Item  People  Build History  Manage Jenkins        │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Welcome to Jenkins                                                 │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │  Status                                                      │ │
│  │                                                               │ │
│  │  # of executors: 2                                           │ │
│  │  Mode: Normal                                                │ │
│  │  Labels: (none)                                              │ │
│  │                                                               │ │
│  │  Build Queue (0)                                              │ │
│  │  No builds in queue                                           │ │
│  │                                                               │ │
│  │  Build Executor Status                                        │ │
│  │  ● master (idle)                                              │ │
│  │  ● master (idle)                                              │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │  No builds are currently running                              │ │
│  │  Build history is empty                                       │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Top Navigation Bar

The top bar contains primary navigation:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Jenkins     New Item  People  Build History  Manage Jenkins        │
└─────────────────────────────────────────────────────────────────────┘
```

| Link | Purpose |
|------|---------|
| **Jenkins** (logo) | Returns to the main dashboard |
| **New Item** | Create a new job (freestyle or pipeline) |
| **People** | View all users who have run builds |
| **Build History** | View build history across all jobs |
| **Manage Jenkins** | Configure Jenkins system settings |

## Sidebar Navigation

Click **Manage Jenkins** to see more options:

```
┌─────────────────────────────────────────────────────────────────────┐
│  System Configuration                                               │
│  ├── Configure System                                               │
│  ├── Global Tool Configuration                                     │
│  ├── Plugins                                                       │
│  ─────────────────                                                │
│  Security                                                          │
│  ├── Configure Global Security                                     │
│  ├── Manage Credentials                                            │
│  ├── Manage Users                                                  │
│  ─────────────────                                                │
│  Troubleshooting                                                    │
│  ├── System Information                                            │
│  ├── About Jenkins                                                 │
└─────────────────────────────────────────────────────────────────────┘
```

### Manage Jenkins Options Explained

| Section | What It Does |
|---------|-------------|
| **Configure System** | Set Jenkins URL, email server, global properties |
| **Global Tool Configuration** | Configure JDK, Maven, Gradle, Docker, etc. |
| **Plugins** | Install, update, remove plugins |
| **Configure Global Security** | Security settings, authorization, access control |
| **Manage Credentials** | Store passwords, API keys, SSH keys securely |
| **Manage Users** | Create and manage Jenkins users |
| **System Information** | View system properties for debugging |
| **About Jenkins** | Version info and update check |

## Status Panel Explained

### Executors

An **executor** is a slot where Jenkins can run a build. Think of it like a parking space:

```
┌─────────────────────────────────────────────┐
│         Jenkins Server                      │
│                                             │
│  Executor #1 ──► Running Build #45         │
│  Executor #2 ──► Running Build #46         │
│  Executor #3 ──► (empty - available)       │
│  Executor #4 ──► (empty - available)       │
│                                             │
└─────────────────────────────────────────────┘
```

- **# of executors**: How many builds can run simultaneously
- **Mode: Normal**: All executors can run any job
- **Mode: Exclusive**: Executors are dedicated to specific jobs via labels

### Build Queue

The **build queue** shows jobs waiting for an available executor:

```
Build Queue (2)
├── my-app #47 - Waiting
│   └──reason: "Waiting for available executor"
└── api-service #23 - In queue for 5s
    └──reason: "Waiting for available executor"
```

### Build Executor Status

Shows what's currently running:

```
Build Executor Status
● master #1 (idle)        # Executor 1 is free
● master #2 (busy: 2m)    # Executor 2 is building for 2 minutes
  └─ my-pipeline #12
```

## What Are Executors? An Analogy

Think of Jenkins as a construction foreman:

> **Executors are like workers on a construction site.**
> 
> - If you have 2 executors, you have 2 workers
> - Each worker can do one task at a time
> - If 5 jobs need to run but you only have 2 workers, 3 jobs wait in the queue
> - More executors = more parallel work = faster CI/CD

### Setting Number of Executors

Default is usually 2. To change:
1. Go to **Manage Jenkins**
2. Click **Configure System**
3. Find **# of executors** field
4. Change the number (balance between CPU and workload)

**Recommendation**: Don't set executors higher than CPU cores on your Jenkins server.

## Job List (When You Have Jobs)

Once you create jobs, the dashboard shows them:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Name                               Last Success    Last Failure   │
│  ───────────────────────────────────────────────────────────────── │
│  my-web-app                         3 min ago       2 weeks ago     │
│  api-service                        1 hour ago      Never           │
│  nightly-build                      12 hours ago    5 days ago      │
└─────────────────────────────────────────────────────────────────────┘
```

Each job row shows:
- **Name**: Job name (clickable)
- **Last Success**: How long since last successful build
- **Last Failure**: How long since last failed build

### Build Status Icons

| Icon | Meaning |
|------|---------|
| 🔵 (blue circle) | Last build was SUCCESS |
| 🔴 (red circle) | Last build was FAILURE |
| 🟡 (yellow circle) | Last build was UNSTABLE (tests failed but build succeeded) |
| ⚪ (grey/white circle) | No builds yet or build was ABORTED |
| 🔄 (spinning) | Build currently running |

## Next Steps

- **[Managing Plugins](02-managing-plugins.md)** - Install and configure plugins
- **[Global Configuration](03-global-configuration.md)** - Configure system settings
- **[Create Your First Job](03-first-job/01-create-freestyle-job.md)** - Create a simple job
