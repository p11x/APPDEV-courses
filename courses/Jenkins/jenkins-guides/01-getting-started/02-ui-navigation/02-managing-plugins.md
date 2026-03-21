# Managing Plugins in Jenkins

## What this covers

This guide explains how to install, update, and remove plugins via the Jenkins Plugin Manager. You'll learn about the different tabs (Available, Installed, Updates, Advanced), how to install essential plugins like Pipeline and Git, and understand plugin dependencies.

## Prerequisites

- Jenkins installed and accessible
- Admin access to Jenkins (Manage Jenkins link visible)
- Internet connectivity to download plugins

## What Are Plugins?

Jenkins plugins extend Jenkins functionality. Think of plugins like apps on your phone:

> **Base Jenkins is like a smartphone out of the box** — it has basic features (phone, contacts, messages).
> 
> **Plugins are like apps you install** — they add capabilities like:
> - Git integration (GitHub, GitLab, Bitbucket)
> - Building Docker images
> - Running Maven or Gradle builds
> - Sending Slack notifications
> - Security scanning
> - And 1,800+ more!

## Accessing the Plugin Manager

1. From the Jenkins dashboard, click **Manage Jenkins**
2. Click **Plugin Manager**

```
┌─────────────────────────────────────────────────────────────────────┐
│  Plugin Manager                                                    │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  ┌──────────┬──────────┬─────────┬────────────┐                    │
│  │ Available│ Installed│ Updates │ Advanced  │                    │
│  └──────────┴──────────┴─────────┴────────────┘                    │
│                                                                     │
│  [Tab content appears here]                                        │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Understanding the Tabs

### Available Tab

Shows all plugins available for installation from the Jenkins Update Center:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Available ┌────────────────────────────────────────────────────────┐│
│  ┌─────────┐                                                        ││
│  │ 🔍 Filter│                                                        ││
│  └─────────┘                                                        ││
│                                                                     │
│  Categories: [All] [Category] [Release] [Installed]                │
│                                                                     │
│  ─────────────────────────────────────────────────────────────────  │
│                                                                     │
│  □ Pipeline                     600.v0.0_a_866.#49c                 │
│    Pipeline as Code support                                     │
│                                                                     │
│  □ Git                          5.2.0                               │
│    Git client support for Jenkins                                 │
│                                                                     │
│  □ Docker Pipeline              1.30                               │
│    Build and use Docker containers from pipelines                 │
│                                                                     │
│  □ Blue Ocean                   1.27.12                             │
│    New UX for Jenkins                                                           │
│                                                                     │
│  □ Credentials Binding          1.27                               │
│    Allows credentials to be bound to environment variables        │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**Use this tab to**: Install new plugins you don't currently have.

### Installed Tab

Shows plugins already installed on your Jenkins:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Installed ┌───────────────────────────────────────────────────────┐│
│            │                                                        ││
│  Plugin                          │ Version │ Enabled │             ││
│  ───────────────────────────────┼─────────┼─────────┤             ││
│  Jenkins (core)                 │ 2.440.1 │   ✓     │ [×]           ││
│  Pipeline                       │ 600.v0. │   ✓     │ [×]           ││
│  Git                            │ 5.2.0   │   ✓     │ [×]           ││
│  Credentials Binding           │ 1.27    │   ✓     │ [×]           ││
│  Timestamper                    │ 1.11    │   ✓     │ [×]           ││
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**Use this tab to**: See what you have, disable plugins, or uninstall them.

### Updates Tab

Shows plugins with newer versions available:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Updates  ┌────────────────────────────────────────────────────────┐│
│            │                                                        ││
│  Plugin           │ Current │ New    │                              ││
│  ─────────────────┼─────────┼────────┤                              ││
│  Git              │ 5.1.0   │ 5.2.0  │ [Download now]               ││
│  Pipeline         │ 580.v0. │ 600.v0 │ [Download now]               ││
│  SSH Agent        │ 1.18    │ 1.21   │ [Download now]               ││
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**Use this tab to**: Update plugins to get new features and security fixes.

### Advanced Tab

Advanced settings including:
- Update site URL (use custom update centers)
- HTTP proxy settings
- Manual plugin upload (.hpi files)
- Plugin directory location

```
┌─────────────────────────────────────────────────────────────────────┐
│  Advanced ┌────────────────────────────────────────────────────────┐│
│           │                                                        ││
│  Update Site                                                         ││
│  ┌────────────────────────────────────────────────────────────────┐│
│  │ https://updates.jenkins.io/current/                         ││
│  └────────────────────────────────────────────────────────────────┘│
│                                                                     │
│  Proxy Configuration                                                ││
│  ┌────────────────────────────────────────────────────────────────┐│
│  │ (○) No Proxy                    ○ Use HTTP Proxy             ││
│  └────────────────────────────────────────────────────────────────┘│
│                                                                     │
│  Upload Plugin                                                      ││
│  ┌────────────────────────────────────────────────────────────────┐│
│  │ [Choose File]  No file chosen                                 ││
│  └────────────────────────────────────────────────────────────────┘│
│                          [Upload]                                   │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Installing the Pipeline Plugin (Example)

Let's install the Pipeline plugin as an example:

### Step 1: Go to Available Tab

From Plugin Manager, click the **Available** tab.

### Step 2: Find the Plugin

Either:
- Scroll through the list, or
- Use the **Filter** box and type "Pipeline"

### Step 3: Check the Plugin Box

Click the checkbox next to "Pipeline":

```
☑ Pipeline                       600.v0.0_a_866.#49c
    Pipeline as Code support
```

### Step 4: Click Install

Click the **Install** button at the bottom:

```
[Install without restart]  [Download now and install after restart]
```

### Step 5: Wait for Installation

```
Installing Pipeline (600.v0.0_a_866.#49c)
▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ 100%

Success
```

### Two Install Options Explained

| Option | When to Use |
|--------|-------------|
| **Install without restart** | Install and use immediately (recommended for testing) |
| **Download now and install after restart** | Install when Jenkins is less busy; Jenkins restarts after |

## Installing the Git Plugin (Example)

Same process for Git:

1. Go to **Available** tab
2. Filter: "Git"
3. Check **Git** plugin
4. Click **Install**

The Git plugin is essential because:
- It allows Jenkins to clone your source code
- It supports GitHub, GitLab, Bitbucket, and self-hosted Git
- It's required for most CI/CD workflows

## Understanding Plugin Dependencies

Jenkins plugins often depend on other plugins. When you install a plugin, dependencies are installed automatically:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Dependencies Required                                              │
│                                                                     │
│  Installing: MyPlugin                                              │
│  ✓ dependency1 - 1.0        ← automatically included              │
│  ✓ dependency2 - 2.0        ← automatically included              │
│  ✓ dependency3 - 1.5        ← automatically included              │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### Dependency Warning

**Never disable or uninstall core dependencies** unless you know what you're doing. For example:
- **Credentials Plugin** is used by many plugins
- **Structs Plugin** is required by Pipeline
- Disabling these can break other plugins

## Updating Plugins

### Method 1: Individual Updates

1. Go to **Updates** tab
2. Check plugins you want to update
3. Click **Download now and install after restart**

### Method 2: Update All

Some plugins support bulk updates, but it's often safer to update one at a time in production.

### Best Practices for Updates

1. **Test first**: Don't update production Jenkins without testing
2. **Check changelogs**: See what's new before updating
3. **Keep backups**: Back up Jenkins home before major updates
4. **Update during low usage**: Schedule updates during maintenance windows

## Uninstalling Plugins

1. Go to **Installed** tab
2. Find the plugin
3. Click **Uninstall** under the Actions column

```
Plugin              │ Version │ Enabled │ Action
────────────────────┼─────────┼─────────┼────────────
Old Plugin          │ 1.0     │   ✓     │ [Uninstall]
```

**Warning**: Some plugins cannot be uninstalled because they're part of Jenkins core.

## Recommended Plugins for Beginners

These plugins cover 90% of common CI/CD use cases:

| Plugin | Purpose |
|--------|---------|
| **Pipeline** | Write CI/CD as code (Jenkinsfile) |
| **Git** | Clone repositories, trigger on commits |
| **Credentials Binding** | Use secrets securely in pipelines |
| **Docker Pipeline** | Build and run Docker containers |
| **Blue Ocean** | Modern pipeline visualization |
| **Timestamper** | Add timestamps to build logs |
| **Mailer** | Email notifications |

## Next Steps

- **[Global Configuration](03-global-configuration.md)** - Configure system settings
- **[Create Your First Job](03-first-job/01-create-freestyle-job.md)** - Create a simple job
