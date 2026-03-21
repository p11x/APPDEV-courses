# Run and Inspect Your Build

## What this covers

This guide shows you how to manually trigger a Jenkins build, view the build log (Console Output), and understand the different build statuses (SUCCESS, FAILURE, UNSTABLE, ABORTED). You'll learn about build numbers, build URLs, and the workspace concept.

## Prerequisites

- Completed the "Create Your First Job" guide
- A job configured with a build step
- Access to Jenkins dashboard

## Triggering a Build Manually

### Step 1: Go to Your Job Page

From the dashboard, click on your job name (`my-first-job`):

```
┌─────────────────────────────────────────────────────────────────────┐
│  Dashboard > my-first-job                                          │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  [ Build Now ]  [ Configure ]  [ Delete Project]                    │
│                                                                     │
│  Build History                                                      │
│  No builds yet.                                                     │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: Click "Build Now"

Click the **Build Now** button on the left sidebar:

```
┌─────────────────────────────────────────────────────────────────────┐
│  my-first-job                                              [Builds]│
│                                                                     │
│  [ Build Now ]  [ Configure ]  [ Delete Project]                  │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ ⚡ Build History                                                │ │
│  │                                                                │ │
│  │ #1  -  [Pending] ← Build is waiting for an executor          │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

You should see **#1** appear under Build History with status [Pending] or [Running].

---

## Understanding Build Statuses

Jenkins builds can have several statuses:

| Status | Icon | Meaning | Color |
|--------|------|---------|-------|
| **SUCCESS** | 🔵 Blue circle | Build completed without errors | Blue |
| **FAILURE** | 🔴 Red circle | Build failed (command error, test failure) | Red |
| **UNSTABLE** | 🟡 Yellow circle | Build succeeded but tests had failures | Yellow |
| **ABORTED** | ⚪ Grey circle | Build was manually cancelled or timed out | Grey |
| **IN PROGRESS** | 🔄 Spinning | Build is currently running | Blue/Grey |

### Visual Flow

```
                    ┌─────────────────┐
                    │ Build Starts    │
                    └────────┬────────┘
                             │
                    ┌────────▼────────┐
                    │ Running Build   │
                    └────────┬────────┘
                             │
              ┌──────────────┼──────────────┐
              │              │              │
       ┌──────▼──────┐ ┌─────▼─────┐ ┌─────▼──────┐
       │   SUCCESS   │ │  FAILURE  │ │  ABORTED   │
       │  (Blue 🔵)  │ │ (Red 🔴)  │ │ (Grey ⚪)   │
       └─────────────┘ └───────────┘ └────────────┘
              │
              │ (if tests fail)
       ┌──────▼──────┐
       │  UNSTABLE   │
       │ (Yellow 🟡) │
       └─────────────┘
```

---

## Viewing the Build Log

### Step 1: Click on the Build Number

In the Build History, click on **#1**:

```
┌─────────────────────────────────────────────────────────────────────┐
│  my-first-job > #1                                                 │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Console Output  │  Changes  │  Workspace  │  Delete ✓            │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: View Console Output

Click **Console Output** to see what happened:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Console Output                                                    │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Started by user admin                                             │
│  Running as: SYSTEM                                                │
│  Building in workspace /var/lib/jenkins/workspace/my-first-job    │
│  [my-first-job] $ /bin/sh -xe /tmp/jenkins123456789.sh            │
│  + echo Hello Jenkins                                              │
│  Hello Jenkins                                                      │
│  Finished: SUCCESS                                                  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### Full Console Output Breakdown

Here's what each line means:

```
Started by user admin                          ← Who triggered the build
Running as: SYSTEM                             ← Which user Jenkins ran as
Building in workspace /var/lib/jenkins/workspace/my-first-job  ← Where files are
[my-first-job] $ /bin/sh -xe /tmp/jenkins... ← Shell command being executed
+ echo Hello Jenkins                           ← The actual command (echo)
Hello Jenkins                                  ← The output from the command
Finished: SUCCESS                              ← Final build status
```

---

## Understanding Build Numbers

Each build gets a unique sequential number:

```
Build History:
├── #5  -  SUCCESS    ← Latest
├── #4  -  SUCCESS
├── #3  -  FAILURE
├── #2  -  SUCCESS
└── #1  -  SUCCESS    ← First build ever
```

The build number:
- Starts at #1
- Increments by 1 for each build
- Never resets (unless you delete the job)
- Used in environment variable `${BUILD_NUMBER}`

---

## Understanding the Build URL

Every build has a unique URL:

```
http://localhost:8080/job/my-first-job/1/
                         │    │       │
                         │    │       └─ Build number
                         │    └───────── Job name
                         └────────────── Jenkins server
```

This URL is useful for:
- Linking in emails
- Including in Slack notifications
- Sharing with team members

---

## Understanding the Workspace

The **workspace** is a directory on the Jenkins server where the build runs:

```
/var/lib/jenkins/workspace/
├── my-first-job/          ← Your job's workspace
│   └── (files here)
├── another-job/
│   └── (files here)
└── yet-another-job/
    └── (files here)
```

### Workspace Contents

When you clone a Git repository, files go here:

```
workspace/my-first-job/
├── src/
│   └── (source code)
├── package.json
├── Jenkinsfile
└── node_modules/
```

### Key Points About Workspace

1. **Per-job**: Each job gets its own workspace
2. **Persistent**: Files remain between builds (unless you enable "Delete workspace before build")
3. **Build-specific**: For Pipeline, each build stage might use a different workspace
4. **On agents**: When using distributed builds, workspace is on the agent, not master

---

## Inspecting Build Details

Click on a build number, then explore the tabs:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Build #1                                                           │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Console Output  │  Changes  │  Workspace  │  ✓ Delete            │
│                                                                     │
│  Status: SUCCESS          |  Built on: master                    │
│  Start Time: Jan 15 10:30 |  Duration: 2 sec                      │
│  Changelog: (none)        |  No artifacts                        │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

| Tab | What It Shows |
|-----|---------------|
| **Console Output** | Full build log |
| **Changes** | What code changed since last build (for Git jobs) |
| **Workspace** | Files in the workspace |
| **Delete** | Delete this specific build |

---

## Build Statuses in the Job Page

The main job page shows status at a glance:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Name                               Last Success    Last Failure   │
│  ───────────────────────────────────────────────────────────────── │
│  my-first-job                         1 min ago       4 days ago    │
│                        ▲                ▲              ▲
│                        │                │              │
│                    Blue 🔵          1 min ago       4 days ago
│                  (SUCCESS)                              
└─────────────────────────────────────────────────────────────────────┘
```

---

## Common Issues and Solutions

### Issue: Build Stuck in Queue

**Problem**: Build stays "Pending" forever

**Solution**: Check if you have available executors. Go to Dashboard → Check "Build Executor Status"

### Issue: Permission Denied

**Problem**: "Permission denied" in console output

**Solution**: Check file permissions in workspace, or run Jenkins with appropriate user

### Issue: Workspace Full

**Problem**: Disk space issues

**Solution**: Enable "Discard old builds" in job configuration, or manually clean workspace

---

## Next Steps

- **[Build Triggers](03-build-triggers.md)** - Learn how to trigger builds automatically
- **[Pipeline Basics](../02-pipelines/01-pipeline-basics/01-what-is-a-jenkinsfile.md)** - Learn about Pipelines (recommended over Freestyle)
