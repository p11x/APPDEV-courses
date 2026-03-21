# Jenkins Initial Setup Wizard

## What this covers

This guide walks through the Jenkins setup wizard—the process you complete the first time you access Jenkins. You'll learn what each step does, why it matters, and how to configure Jenkins for your needs.

## Prerequisites

- Jenkins installed (either via apt/Docker or other methods)
- Access to the Jenkins web interface
- The initial admin password (from installation logs)

## The Setup Wizard Explained

When you first access Jenkins at `http://localhost:8080`, you'll see a multi-step setup wizard. Here's what each step does:

---

## Step 1: Unlock Jenkins

### What You See

```
┌─────────────────────────────────────────────────────────────┐
│                     🔒 Unlock Jenkins                         │
│                                                              │
│  Jenkins is starting up. To ensure Jenkins is safely        │
│  secured, you must create an administrator password.       │
│                                                              │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │ Administrator password:                                 │ │
│  │ [••••••••••••••••••••••]                                │ │
│  └─────────────────────────────────────────────────────────┘ │
│                                                              │
│  The password is located at:                                │
│  /var/jenkins_home/secrets/initialAdminPassword            │
│                                                              │
│                          [ Continue ]                        │
└─────────────────────────────────────────────────────────────┘
```

### What This Means

This is a security measure to prevent unauthorized access during initial setup. The password proves you have access to the server where Jenkins is running.

### How to Proceed

1. If running in Docker, run:
   ```bash
   docker exec jenkins cat /var/jenkins_home/secrets/initialAdminPassword
   ```

2. If installed via apt, run:
   ```bash
   sudo cat /var/lib/jenkins/secrets/initialAdminPassword
   ```

3. Enter the password in the web form
4. Click **Continue**

### Why This Matters

This password is randomly generated during installation. Only someone with server access can retrieve it, ensuring no one else can configure your Jenkins instance.

---

## Step 2: Install Plugins

### What You See

```
┌─────────────────────────────────────────────────────────────┐
│                  Customize Jenkins                           │
│                                                              │
│  Which plugins would you like to install?                  │
│                                                              │
│  ┌─────────────────────────────────────────────────────────┐│
│  │ ○ Install suggested plugins (recommended)              ││
│  │   Pipeline, Git, Credentials, etc.                     ││
│  └─────────────────────────────────────────────────────────┘│
│  ┌─────────────────────────────────────────────────────────┐│
│  │ ○ Select plugins to install                            ││
│  │   Choose from a list of available plugins              ││
│  └─────────────────────────────────────────────────────────┘│
│                                                              │
│  ┌─────────────────────────────────────────────────────────┐│
│  │  [ ] Pipeline                                           ││
│  │  [ ] Git                                                ││
│  │  [ ] Configuration as Code                               ││
│  │  [ ] Blue Ocean                                         ││
│  │  [ ] Docker Pipeline                                    ││
│  │  ... and 1,800+ more plugins                            ││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
```

### What This Means

Jenkins has a plugin ecosystem. Plugins add features like:
- **Git**: Integration with Git version control
- **Pipeline**: The Pipeline job type
- **Blue Ocean**: Modern pipeline visualization UI
- **Credentials**: Secure storage for passwords, API keys, SSH keys
- **Docker Pipeline**: Building Docker images in pipelines

### Recommended: Install Suggested Plugins

For beginners, choose **"Install suggested plugins"**. This installs the most commonly used plugins:

- Git plugin
- Pipeline plugin
- Credentials Binding plugin
- Timestamper (adds timestamps to logs)
- Matrix Authorization Strategy
- And 50+ others

### What Happens During Installation

```
┌─────────────────────────────────────────────────────────────┐
│                  Installing Plugins                          │
│                                                              │
│  ████████████████████████████████░░░░░░░  80%              │
│                                                              │
│  Installing:                                                 │
│  ✓ git - 5.x.x                                               │
│  ✓ pipeline - 600.x                                          │
│  ✓ credentials - 1.34                                        │
│  → installing: docker-workflow...                           │
└─────────────────────────────────────────────────────────────┘
```

This typically takes 5-10 minutes depending on your internet connection.

### Why Install Plugins Now?

Installing plugins during setup gives you a working Jenkins with common features. You can always install more plugins later via **Manage Jenkins → Plugin Manager**.

---

## Step 3: Create First Admin User

### What You See

```
┌─────────────────────────────────────────────────────────────┐
│                  Create First Admin User                     │
│                                                              │
│  ┌─────────────────────────────────────────────────────────┐│
│  │ Username:         [admin________________]               ││
│  │ Password:        [••••••••••••••]                       ││
│  │ Confirm:         [••••••••••••••••]                       ││
│  │ Full Name:       [Jenkins Admin___]                     ││
│  │ Email:           [admin@example.com__]                  ││
│  └─────────────────────────────────────────────────────────┘│
│                                                              │
│                          [ Save and Continue ]              │
└─────────────────────────────────────────────────────────────┘
```

### What This Means

Create your administrator account. This user has full access to Jenkins.

### How to Proceed

Fill in the details:
- **Username**: `admin` (or your preferred username)
- **Password**: Choose a strong password
- **Full Name**: Your name
- **Email**: Your email address

Click **Save and Continue**

### ⚠️ Important Notes

- This account has **full administrative privileges**
- Remember these credentials—you'll need them to log in
- For production, consider setting up LDAP or GitHub OAuth (covered in advanced topics)

---

## Step 4: Configure Jenkins URL

### What You See

```
┌─────────────────────────────────────────────────────────────┐
│                  Jenkins URL Configuration                   │
│                                                              │
│  ┌─────────────────────────────────────────────────────────┐│
│  │ Jenkins URL:                                            ││
│  │ http://localhost:8080/                              ││
│  └─────────────────────────────────────────────────────────┘│
│                                                              │
│  This is the address where users can access Jenkins.        │
│  This address will be used in email notifications and       │
│  generated links.                                           │
│                                                              │
│                          [ Save and Finish ]                 │
└─────────────────────────────────────────────────────────────┘
```

### What This Means

The **Jenkins URL** is used for:
- Generating links in emails and notifications
- Building URLs for build artifacts
- Webhook callbacks from GitHub/GitLab
- Accessing Jenkins from agents

### For Local Development

Keep `http://localhost:8080/` — Jenkins auto-detects this.

### For Production/Servers

Change to your actual domain:
- `http://jenkins.yourcompany.com/`
- `https://jenkins.yourcompany.com/` (if using SSL/HTTPS)

### Click "Save and Finish"

---

## Step 5: Welcome to Jenkins!

### What You See

```
┌─────────────────────────────────────────────────────────────┐
│                    Welcome to Jenkins!                       │
│                                                              │
│  ┌─────────────────────────────────────────────────────────┐│
│  │                    🎉                                    ││
│  │         Jenkins is fully up and running!                ││
│  │                                                            ││
│  │                  [ Start using Jenkins ]                ││
│  └─────────────────────────────────────────────────────────┘│
└─────────────────────────────────────────────────────────────┘
```

### Congratulations!

Jenkins is now ready to use. Click **Start using Jenkins** to access the dashboard.

---

## What You See: The Jenkins Dashboard

```
┌─────────────────────────────────────────────────────────────┐
│  Jenkins     New Item  People  Build History  Manage Jenkins│
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  Welcome to Jenkins                                         │
│                                                             │
│  ┌─────────────────────────────────────────────────────────┐│
│  │  Status                                                  ││
│  │                                                          ││
│  │  # of executors: 2                                       ││
│  │  Mode: Normal                                           ││
│  │  Labels: (none)                                         ││
│  │                                                          ││
│  │  Build Queue (0)                                         ││
│  │  No builds in queue                                      ││
│  │                                                          ││
│  │  Build Executor Status                                   ││
│  │  ● master (idle)                                         ││
│  │  ● master (idle)                                         ││
│  └─────────────────────────────────────────────────────────┘│
│                                                             │
│  ┌─────────────────────────────────────────────────────────┐│
│  │  No builds are currently running                        ││
│  │  Build history is empty                                 ││
│  └─────────────────────────────────────────────────────────┘│
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## Next Steps

- **[Dashboard Overview](02-ui-navigation/01-dashboard-overview.md)** - Understand the Jenkins interface
- **[Create Your First Job](03-first-job/01-create-freestyle-job.md)** - Create a simple job to verify Jenkins works
