# Git Plugin Setup in Jenkins

## What this covers

This guide explains how to install and configure the Git plugin in Jenkins. You'll learn how to set up global Git credentials, configure SSH keys vs HTTPS tokens for repository access, and understand the difference between `checkout scm` and explicit `git` steps.

## Prerequisites

- Jenkins installed and accessible
- Git installed on the Jenkins server
- Git plugin installed (usually included in suggested plugins)

## Installing the Git Plugin

### If Not Already Installed

1. Go to **Manage Jenkins** → **Plugin Manager**
2. Click **Available** tab
3. Search for "Git"
4. Check **Git** plugin
5. Click **Install**

---

## Configuring Git Globally

### Step 1: Verify Git Installation

Git should be installed on the Jenkins server. Check via:

```bash
# On Jenkins server
git --version
# Output: git version 2.40.0
```

### Step 2: Configure Git in Jenkins

1. Go to **Manage Jenkins** → **Global Tool Configuration**
2. Find **Git** section:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Git                                                               │
│                                                                     │
│  Git installations                                                │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ Name:  [ Default ]                                            │ │
│  │ Path to Git executable:  [ /usr/bin/git ]                    │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  [+ Add Git]                                                        │
└─────────────────────────────────────────────────────────────────────┘
```

- **Name**: Give it a descriptive name (e.g., "Default")
- **Path to Git executable**: Usually auto-detected, or set to `/usr/bin/git` (Linux) or `C:\Program Files\Git\bin\git.exe` (Windows)

---

## Configuring Git Credentials

### Option 1: HTTPS Username/Password or Token

For HTTPS Git access:

1. Go to **Manage Jenkins** → **Credentials**
2. Click **Add Credentials**
3. Kind: **Username with password** (for tokens, put token as password)
4. Fill in:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Kind: Username with password                                      │
│                                                                     │
│  Username:  [ your-github-username ]                                │
│  Password:  [ your-github-token ]     ← Use token, not password   │
│       ↓                                                             │
│  ID:        [ github-credentials ]                                 │
│       ↓                                                             │
│  Description:  [ GitHub credentials ]                             │
└─────────────────────────────────────────────────────────────────────┘
```

### Option 2: SSH Key

For SSH Git access:

1. Generate SSH key pair on Jenkins server (if not exists):
   ```bash
   ssh-keygen -t ed25519 -C "jenkins@company.com"
   ```

2. Add public key to GitHub/GitLab account

3. Add private key to Jenkins:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Kind: SSH Username with private key                               │
│                                                                     │
│  Username:  [ git ]                                                 │
│  Private Key:                                                      │
│    ○ Enter directly                                                │
│    ● From Jenkins master ~/.ssh                                    │
│       ↓                                                             │
│  ID:        [ github-ssh-key ]                                    │
│       ↓                                                             │
│  Description:  [ SSH key for GitHub ]                              │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Using Git in Pipeline

### Method 1: checkout scm (Recommended)

`checkout scm` automatically uses the repository configured in the job:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                // checkout scm automatically checks out the repo
                // that this Jenkinsfile belongs to
                checkout scm
                
                // Or with additional options
                checkout([
                    $class: 'GitSCM',
                    branches: [[name: '*/main']],
                    userRemoteConfigs: [[
                        url: 'https://github.com/user/repo.git',
                        credentialsId: 'github-credentials'
                    ]]
                ])
            }
        }
    }
}
```

### Method 2: Explicit git Step

```groovy
pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                // Explicit checkout with all options
                git (
                    // Repository URL
                    url: 'https://github.com/user/repo.git',
                    
                    // Branch to checkout
                    branch: 'main',
                    
                    // Credentials ID (from Jenkins credentials store)
                    credentialsId: 'github-credentials',
                    
                    // Behavior options
                    poll: false,
                    changelog: true
                )
            }
        }
    }
}
```

---

## checkout scm vs git

### checkout scm

```groovy
checkout scm
```

| Aspect | Description |
|--------|-------------|
| **Auto-detects** | Uses repository from job configuration |
| **Simple** | No URL or credentials needed |
| **Works with** | Multibranch Pipeline, GitHub Branch Source |
| **Branching** | Automatically checks out the branch being built |

**Use when**: You're using GitHub Branch Source or Multibranch Pipeline

### git Step

```groovy
git url: 'https://github.com/user/repo.git',
    branch: 'main',
    credentialsId: 'my-creds'
```

| Aspect | Description |
|--------|-------------|
| **Explicit** | You specify everything |
| **Flexible** | Can checkout any repo/branch |
| **Manual** | You must configure URL and credentials |
| **Static** | Doesn't auto-detect branch from job |

**Use when**: You need to checkout a different repo or specific commit

---

## Git Branch Variables

When using Git, Jenkins automatically sets these variables:

| Variable | Description | Example |
|----------|-------------|---------|
| `GIT_BRANCH` | Branch being built | `main` |
| `GIT_COMMIT` | Full commit SHA | `abc123def...` |
| `GIT_URL` | Repository URL | `https://github.com/...` |
| `GIT_AUTHOR_NAME` | Author name | `John Doe` |
| `GIT_COMMITTER_NAME` | Committer name | `John Doe` |
| `GIT_MESSAGE` | Commit message | "Fix bug" |

```groovy
pipeline {
    agent any
    
    stages {
        stage('Show Git Info') {
            steps {
                echo "Branch: ${env.GIT_BRANCH}"
                echo "Commit: ${env.GIT_COMMIT}"
                echo "Author: ${env.GIT_AUTHOR_NAME}"
            }
        }
    }
}
```

---

## Complete Example with Git

```groovy
pipeline {
    agent any
    
    // Use credentials for Git access
    environment {
        GIT_CREDENTIALS = credentials('github-credentials')
    }
    
    stages {
        stage('Checkout') {
            steps {
                // Checkout using scm (for job-configured repo)
                checkout scm
                
                // Or explicit checkout
                git branch: 'main',
                    credentialsId: 'github-credentials',
                    url: 'https://github.com/user/myapp.git'
            }
        }
        
        stage('Build') {
            steps {
                sh 'make build'
            }
        }
        
        stage('Test') {
            steps {
                sh 'make test'
            }
        }
    }
    
    post {
        always {
            echo "Built from branch: ${env.GIT_BRANCH}"
            echo "Commit: ${env.GIT_COMMIT}"
        }
    }
}
```

---

## Common Git Configurations

### Checkout Specific Branch

```groovy
git branch: 'develop',
    url: 'https://github.com/user/repo.git'
```

### Checkout Tag

```groovy
git branch: 'refs/tags/v1.0.0',
    url: 'https://github.com/user/repo.git'
```

### Shallow Clone (Faster)

```groovy
git branch: 'main',
    url: 'https://github.com/user/repo.git',
    depth: 1  // Only last 1 commit
```

### With Submodules

```groovy
checkout([
    $class: 'GitSCM',
    branches: [[name: 'main']],
    userRemoteConfigs: [[
        url: 'https://github.com/user/repo.git',
        credentialsId: 'github-credentials'
    ]],
    submoduleCfg: [
        [type: 'null'],
        [name: 'submodule1'],
        [name: 'submodule2']
    ]
])
```

---

## Common Mistakes

### Mistake 1: Wrong Credentials

```groovy
// ❌ WRONG - Credentials don't exist or are wrong
git url: 'https://github.com/user/repo.git',
    credentialsId: 'wrong-id'

// ✅ CORRECT - Use correct credentials ID
git url: 'https://github.com/user/repo.git',
    credentialsId: 'github-credentials'
```

### Mistake 2: Forgetting Credentials

```groovy
// ❌ WRONG - Private repo needs credentials
git url: 'https://github.com/user/private-repo.git'
// No credentialsId!

// ✅ CORRECT
git url: 'https://github.com/user/private-repo.git',
    credentialsId: 'github-credentials'
```

### Mistake 3: Using checkout scm Without Job Config

```groovy
// ❌ WRONG - checkout scm won't work if job has no SCM configured
checkout scm  // Job doesn't have Git configured!

// ✅ CORRECT - Use explicit git step
git url: 'https://github.com/user/repo.git'
```

---

## Next Steps

- **[Multibranch Pipeline](02-multibranch-pipeline.md)** - Auto-create jobs for branches
- **[GitHub Webhooks](03-github-webhook-setup.md)** - Trigger builds on push
- **[GitLab Integration](https://plugins.jenkins.io/gitlab-plugin/)** - For GitLab users
