# Global Configuration in Jenkins

## What this covers

This guide walks through the Configure System page in Jenkins—where you set global settings that affect all jobs. You'll learn about the Jenkins URL, email notifications, number of executors, Git configuration, and other critical system settings.

## Prerequisites

- Jenkins installed and accessible
- Admin access to Jenkins
- Completed initial setup wizard

## Accessing Configure System

1. From the dashboard, click **Manage Jenkins**
2. Click **Configure System**

```
┌─────────────────────────────────────────────────────────────────────┐
│  Configure System                                                  │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Jenkins Location                                                  │
│  ─────────────────                                                │
│                                                                     │
│  Jenkins URL:                                                      │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ http://localhost:8080/                                      │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  System Admin e-mail address:                                      │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ jenkins@example.com                                         │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Configuration Sections Explained

### Jenkins Location

```
┌─────────────────────────────────────────────────────────────────────┐
│  Jenkins Location                                                 │
│  ─────────────────                                                 │
│                                                                     │
│  Jenkins URL:  [ http://localhost:8080/  ]                        │
│       ↓                                                             │
│  This is the public-facing URL. Used in:                          │
│  • Email notifications (links to builds)                          │
│  • GitHub/GitLab webhook callbacks                                 │
│  • Generated links in artifacts                                   │
│                                                                     │
│  System Admin e-mail address:  [ jenkins@example.com ]            │
│       ↓                                                             │
│  The "From" address for email notifications                        │
└─────────────────────────────────────────────────────────────────────┘
```

| Field | What It Controls | Why It Matters |
|-------|------------------|----------------|
| **Jenkins URL** | Public-facing URL for Jenkins | Used in emails, webhooks, generated links. Must be set correctly for external access. |
| **System Admin e-mail address** | Sender email for notifications | Should be a real email so recipients can reply |

**For Production**: Set Jenkins URL to your actual domain (e.g., `https://jenkins.yourcompany.com/`)

### Number of Executors

```
┌─────────────────────────────────────────────────────────────────────┐
│  Executors                                                         │
│  ─────────                                                         │
│                                                                     │
│  Number of executors:  [ 2 ]                                       │
│       ↓                                                             │
│  How many build jobs can run simultaneously                        │
│                                                                     │
│  ✗ Mode: (○) Normal    ( ) Exclusive                              │
│       ↓                                                             │
│  Normal: Any job can use any executor                              │
│  Exclusive: Jobs with matching labels get priority                │
└─────────────────────────────────────────────────────────────────────┘
```

| Field | What It Controls | Recommendation |
|-------|------------------|----------------|
| **Number of executors** | Concurrent builds | Set to CPU cores on master. Too many = slow builds |
| **Mode** | How executors are assigned | Normal for most cases |

**Rule of Thumb**: Start with number of CPU cores. Monitor and adjust.

### Execution

```
┌─────────────────────────────────────────────────────────────────────┐
│  Execution                                                         │
│  ─────────                                                         │
│                                                                     │
│  Labels:  [ linux docker ]                                         │
│       ↓                                                             │
│  Add labels to this Jenkins master (space-separated)              │
│  Use in pipeline: agent { label 'docker' }                         │
└─────────────────────────────────────────────────────────────────────┘
```

**Labels**: Tag your Jenkins agent with labels like "docker", "linux", "windows" to control where jobs run.

### Global Properties

```
┌─────────────────────────────────────────────────────────────────────┐
│  Global Properties                                                 │
│  ─────────────────                                                  │
│                                                                     │
│  ☑ Environment variables                                           │
│       ┌───────────────────────────────────────────────────────────┐ │
│       │ Name                     │ Value                         │ │
│       │ ─────────────────────────┼────────────────────────────── │ │
│       │ DOCKER_REGISTRY          │ https://registry.hub.docker.io│
│       │ APP_ENV                  │ production                    │ │
│       └───────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────┘
```

**Environment Variables**: These are available in ALL jobs. Use for values that don't change often:
- Registry URLs
- Default timezones
- Company-wide settings

### Git Configuration

```
┌─────────────────────────────────────────────────────────────────────┐
│  Git plugin                                                         │
│  ──────────                                                         │
│                                                                     │
│  Global Config user.name:  [ Jenkins Build User ]                  │
│       ↓                                                             │
│  Name used for Git commits made by Jenkins                         │
│                                                                     │
│  Global Config user.email:  [ jenkins@example.com ]                │
│       ↓                                                             │
│  Email used for Git commits made by Jenkins                        │
│                                                                     │
│  ☑ Use Git executable:  [ /usr/bin/git ]                          │
│       ↓                                                             │
│  Path to Git binary (leave auto-detect on most systems)           │
└─────────────────────────────────────────────────────────────────────┘
```

| Field | What It Controls | When to Change |
|-------|------------------|----------------|
| **Global Config user.name** | Name for Git commits | Set to "Jenkins" or your CI username |
| **Global Config user.email** | Email for Git commits | Set to a real email (or noreply@company.com) |
| **Git executable path** | Location of git binary | Usually auto-detected; only change if needed |

### Email Notification

```
┌─────────────────────────────────────────────────────────────────────┐
│  E-mail Notification                                                │
│  ───────────────────                                                │
│                                                                     │
│  SMTP server:  [ smtp.gmail.com ]                                   │
│       ↓                                                             │
│  SMTP server hostname                                               │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ ○ Default (java -jar)                                         │ │
│  │ ○ Advanced...                                                 │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  Default user e-mail suffix:  [ @gmail.com ]                       │
│       ↓                                                             │
│  Appended to usernames without @domain                            │
└─────────────────────────────────────────────────────────────────────┘
```

**Configuring Gmail (Example)**:

```
SMTP server: smtp.gmail.com
Default user e-mail suffix: @gmail.com

[ Advanced ] button reveals:
├── SMTP port: [ 587 ]               ← Use 587 for TLS, 465 for SSL
├── Use SSL: [ ]                     ← Check for SSL
├── Credentials: [ Configure ]       ← Add Gmail app password
└── Charset: [ UTF-8 ]
```

| Field | What It Controls |
|-------|------------------|
| **SMTP server** | Your email provider's SMTP host |
| **SMTP port** | 587 (TLS) or 465 (SSL) |
| **Credentials** | Username/password for authentication |
| **Default user e-email suffix** | Added to usernames without @ |

**Important**: For Gmail, you need an "App Password", not your regular password. Generate at: https://myaccount.google.com/apppasswords

### Extended E-mail Notification (Email Extension Plugin)

If installed, you'll see additional email settings:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Extended E-mail Notification                                       │
│  ──────────────────────────                                         │
│                                                                     │
│  SMTP server:  [ smtp.gmail.com ]                                   │
│  Default subject:  [ BUILD ${BUILD_STATUS} - ${PROJECT_NAME} ]   │
│                                                                     │
│  Default content:                                                   │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ ${BUILD_STATUS} - ${PROJECT_NAME}                            │ │
│  │ Build #: ${BUILD_NUMBER}                                      │ │
│  │ Build URL: ${BUILD_URL}                                       │ │
│  │ Full logs: ${BUILD_LOG, maxLines=100}                        │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  Default recipients:  [ dev-team@company.com ]                     │
│                                                                     │
│  ☑ Attach build log                                                │
│  ☑ Include last build failure                                     │
└─────────────────────────────────────────────────────────────────────┘
```

This plugin allows rich email templates with Jenkins variables.

### Quiet Period

```
┌─────────────────────────────────────────────────────────────────────┐
│  Quiet Period                                                      │
│  ────────────                                                       │
│                                                                     │
│  Default quiet period:  [ 5 ] seconds                              │
│       ↓                                                             │
│  Wait this long after a commit before building                    │
│  Helps batch multiple rapid commits into one build               │
└─────────────────────────────────────────────────────────────────────┘
```

**When to use**: If you expect rapid-fire commits, set quiet period to wait and batch them into one build.

### Retry Count

```
┌─────────────────────────────────────────────────────────────────────┐
│  Retry Count                                                        │
│  ──────────                                                         │
│                                                                     │
│  Connection retry count:  [ 3 ]                                     │
│       ↓                                                             │
│  Number of times to retry failed SCM checkout                     │
└─────────────────────────────────────────────────────────────────────┘
```

**When to use**: For flaky network connections to Git repositories.

## Saving Configuration

After making changes:

1. Click **Save** at the bottom of the page
2. Changes apply immediately
3. No restart required for most changes

```
[ Save ]  [ Apply ]  [ Validate Button ]
```

| Button | What It Does |
|--------|--------------|
| **Save** | Save and return to dashboard |
| **Apply** | Save and stay on page (for testing) |
| **Validate Button** | Test specific settings |

## Common Configuration Mistakes

### Mistake 1: Wrong Jenkins URL

**Problem**: Can't access Jenkins from other machines
**Solution**: Set Jenkins URL to the server's IP or hostname, not localhost

### Mistake 2: Too Many Executors

**Problem**: Builds are slow, system is sluggish
**Solution**: Reduce executors to match CPU cores

### Mistake 3: Wrong Git Path

**Problem**: "git not found" errors in jobs
**Solution**: Install Git on Jenkins server, or configure the correct path

### Mistake 4: No Email Credentials

**Problem**: Email notifications fail
**Solution**: Configure SMTP credentials (especially for Gmail which requires App Passwords)

## Next Steps

- **[Create Your First Job](03-first-job/01-create-freestyle-job.md)** - Create a simple job
- **[Build Triggers](03-first-job/03-build-triggers.md)** - Learn how to trigger builds automatically
- **[Pipeline Basics](../02-pipelines/01-pipeline-basics/01-what-is-a-jenkinsfile.md)** - Learn about Pipelines
