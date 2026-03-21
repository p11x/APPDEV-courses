# Create Your First Jenkins Job (Freestyle)

## What this covers

This guide walks through creating a simple Freestyle job in Jenkins—a basic job type that lets you run shell commands, scripts, and automate tasks. You'll create a job that simply prints "Hello Jenkins" to understand the job configuration interface.

## Prerequisites

- Jenkins installed and accessible
- Completed initial setup wizard
- Basic familiarity with the Jenkins dashboard

## What is a Freestyle Job?

A **Freestyle job** is the traditional Jenkins job type. It provides a graphical interface to configure:

- What to build (source code management)
- When to build (build triggers)
- How to build (build steps)
- What to do after (post-build actions)

Think of it as a checklist in the Jenkins UI.

> **Note**: While Freestyle jobs are great for learning, modern Jenkins recommends using **Pipeline jobs** for production because Pipelines are stored as code (Jenkinsfile) and can be version-controlled. We'll cover Pipelines in the next section.

## Step-by-Step: Create Your First Job

### Step 1: Click "New Item"

From the Jenkins dashboard, click **New Item**:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Jenkins     New Item  People  Build History  Manage Jenkins        │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: Enter Job Name

You'll see the "New Item" page:

```
┌─────────────────────────────────────────────────────────────────────┐
│  New Item                                                          │
│                                                                     │
│  Item name:  [ my-first-job ]                                      │
│       ↓                                                             │
│  Enter a unique name for your job                                  │
│                                                                     │
│  └─ An item with that name already exists                          │
│       (You'll see this warning if name is taken)                  │
│                                                                     │
│  └─ This name is reserved                   ← Can't use these     │
│                                                                     │
│  What to copy from:  [ - None - ▼ ]                                │
│       ↓                                                             │
│  Optionally copy from existing job                                │
│                                                                     │
│  ┌─────────────────────────────────────────────────────────────────┐│
│  │ ○ Freestyle project                                           ││
│  │   Build a basic software project                               ││
│  │                                                                 ││
│  │ ○ Pipeline                                                     ││
│  │   Orchestates long-running activities                          ││
│  │                                                                 ││
│  │ ○ Multi-configuration project                                  ││
│  │   For projects that need different configurations               ││
│  │                                                                 ││
│  │ ○ Folder                                                       ││
│  │   Create nested folders                                         ││
│  └─────────────────────────────────────────────────────────────────┘│
│                                                                     │
│                              [ OK ]  [ Cancel ]                     │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 3: Select Job Type

Select **Freestyle project** and enter a name like `my-first-job`.

### Step 4: Configure the Job

After clicking OK, you'll see the job configuration page. Let's walk through each section:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Configure [my-first-job]                                           │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  General ┌─────────────────────────────────────────────────────────┐│
│  Source Code Management                                            │
│  Build Triggers                                                    │
│  Build Environment                                                 │
│  Build Steps                                                       │
│  Post-build Actions                                                │
└─────────────────────────────────────────────────────────────────────┘
```

---

## General Section

```
┌─────────────────────────────────────────────────────────────────────┐
│  General                                                           │
│  ──────                                                           │
│                                                                     │
│  ☑ Discard old builds                                              │
│     Strategy:  [Days to keep builds: 7 ▼]                          │
│     Max # of builds to keep:  [ 10 ]                               │
│       ↓                                                             │
│     Automatically delete old builds to save disk space            │
│                                                                     │
│  ☑ This project is parameterized   ← Add parameters later         │
│     Add Parameter ▼                                               │
│                                                                     │
│  Description:                                                      │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ My first Jenkins job - prints Hello Jenkins                  │ │
│  └───────────────────────────────────────────────────────────────┘ │
│       ↓                                                             │
│     Optional description of what this job does                    │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

| Field | What It Does | Recommendation |
|-------|--------------|----------------|
| **Discard old builds** | Delete old builds after X days or builds | Keep enabled to save disk space |
| **This project is parameterized** | Allow passing parameters to the build | Enable for flexible builds |
| **Description** | Job description | Add helpful description |

---

## Source Code Management (SCM)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Source Code Management                                             │
│  ─────────────────────                                              │
│                                                                     │
│  ☑ None        ← No source code (we're just running a command)     │
│      ↓                                                               │
│    Git                                                              │
│    Subversion                                                       │
│                                                                     │
│  If using Git:                                                      │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ Repository URL:  [ https://github.com/user/repo.git ]        │ │
│  │                                                                   │ │
│  │ Credentials:  [ - None - ▼]                                    │ │
│  │                                                                   │ │
│  │ Branches to build:                                              │ │
│  │ Branch Specifier (blank for default):  [ */main ▼]            │ │
│  │                                                                   │ │
│  └───────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────┘
```

For our simple "Hello Jenkins" job, select **None** (no source code needed).

---

## Build Triggers

```
┌─────────────────────────────────────────────────────────────────────┐
│  Build Triggers                                                    │
│  ───────────────                                                   │
│                                                                     │
│  ☐ Trigger builds remotely (e.g., from scripts)                   │
│     Authentication Token:  [ __________ ]                          │
│       ↓                                                             │
│     Allow other systems to trigger this job via URL                │
│                                                                     │
│  ☑ Build after other projects are built                           │
│     Projects to watch:  [ __________ ]                            │
│       ↓                                                             │
│     Trigger when another job completes                            │
│                                                                     │
│  ☐ Build periodically                                             │
│     Schedule:  [ H/5 * * * * ]                                   │
│       ↓                                                             │
│     Cron-like schedule (see cron section)                        │
│                                                                     │
│  ☐ Poll SCM                                                        │
│     Schedule:  [ H/5 * * * * ]                                   │
│       ↓                                                             │
│     Check for code changes every 5 minutes                        │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

For our first job, don't select any triggers. We'll manually trigger the build.

---

## Build Environment

```
┌─────────────────────────────────────────────────────────────────────┐
│  Build Environment                                                  │
│  ────────────────                                                  │
│                                                                     │
│  ☐ Delete workspace before build starts                           │
│       ↓                                                             │
│     Clean workspace before each build                             │
│                                                                     │
│  ☐ Abort the build if it's stuck                                  │
│     Timeout:  [ 3 ] minutes                                       │
│       ↓                                                             │
│     Kill build if it takes too long                               │
│                                                                     │
│  ☐ timestamps in console output                                    │
│       ↓                                                             │
│     Add timestamps to build log (requires Timestamper plugin)    │
│                                                                     │
│  ☑ Use secret text(s) or file(s)                                  │
│     Bindings:  [ Add ▼]                                           │
│       ↓                                                             │
│     Inject credentials into build (covered in advanced topics)    │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

Enable **timestamps in console output** - it helps when reading logs.

---

## Build Steps (The Most Important Section!)

This is where we define what the job actually does:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Build Steps                                                        │
│  ──────────                                                         │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ Add build step ▼                                            │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  ────────────────────────────────────────────────────────────────  │
│                                                                     │
│  Common build steps:                                               │
│  ├── Execute Windows batch command                                │
│  ├── Execute shell                                                  │
│  ├── Invoke Ant                                                    │
│  ├── Invoke Gradle script                                          │
│  ├── Run Maven build                                               │
│  ├── Execute Python script                                         │
│  └── ... (many more depending on plugins)                         │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### Adding a Shell Build Step

Click **Add build step** → **Execute shell** (or "Execute Windows batch command" on Windows):

```
┌─────────────────────────────────────────────────────────────────────┐
│  Execute shell                                                     │
│  Command:                                                          │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ #!/bin/bash                                                    │ │
│  │ echo "Hello Jenkins"                                           │ │
│  └───────────────────────────────────────────────────────────────┘ │
│       ↓                                                             │
│  Write the commands to run                                         │
│  Each line runs in order                                           │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**Enter this command**:
```bash
echo "Hello Jenkins"
```

---

## Post-build Actions

```
┌─────────────────────────────────────────────────────────────────────┐
│  Post-build Actions                                                 │
│  ───────────────────                                               │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ Add post-build action ▼                                      │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  ────────────────────────────────────────────────────────────────  │
│                                                                     │
│  Common post-build actions:                                        │
│  ├── E-mail Notification                                           │
│  ├── Archive the artifacts                                         │
│  ├── Build other projects                                          │
│  ├── Git Publisher                                                 │
│  ├── Fingerprint published files                                   │
│  └── ... (many more depending on plugins)                         │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

For our simple job, we don't need any post-build actions.

---

## Save the Job

Click **Save** at the bottom:

```
┌─────────────────────────────────────────────────────────────────────┐
│                              [ Save ]  [ Apply ]                    │
└─────────────────────────────────────────────────────────────────────┘
```

---

## What Happens Next

After saving, you'll see the job page:

```
┌─────────────────────────────────────────────────────────────────────┐
│  my-first-job                                              [Builds]│
│                                                                     │
│  [ Build Now ]  [ Configure ]  [ Delete Project]                  │
│                                                                     │
│  Build History                                                      │
│  #1  -  #2  -  #3 (latest)                                         │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Next Steps

- **[Run and Inspect Build](02-run-and-inspect-build.md)** - Trigger the build and see the output
