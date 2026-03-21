# Multibranch Pipeline in Jenkins

## What this covers

This guide explains what a Multibranch Pipeline is, why it's the modern standard for CI/CD, how Jenkins automatically scans repositories and creates branch-specific jobs, and how to configure branch filtering and Jenkinsfile discovery.

## Prerequisites

- Git plugin installed
- Pipeline plugin installed
- Understanding of basic Pipeline syntax
- GitHub Branch Source Plugin (for GitHub) or GitLab Branch Source Plugin (for GitLab)

## What is a Multibranch Pipeline?

A **Multibranch Pipeline** automatically discovers and builds pipelines for multiple branches in a Git repository.

```
┌─────────────────────────────────────────────────────────────────────┐
│  My Repository                                                      │
│  ├── main        ──► Creates job: my-repo/main                    │
│  ├── develop     ──► Creates job: my-repo/develop                 │
│  ├── feature/login  ──► Creates job: my-repo/feature/login       │
│  ├── bugfix/123  ──► Creates job: my-repo/bugfix/123              │
│  └── release/2.0 ──► Creates job: my-repo/release/2.0              │
└─────────────────────────────────────────────────────────────────────┘
```

### Traditional vs Multibranch

| Approach | Pros | Cons |
|----------|------|------|
| **Single Branch** | Simple | Only builds one branch |
| **Multiple Jobs** | Full control | Manual setup, hard to maintain |
| **Multibranch** | Auto-discovery, per-branch config | Requires Jenkinsfile in each branch |

---

## Setting Up Multibranch Pipeline

### Step 1: Install Required Plugins

1. Go to **Manage Jenkins** → **Plugin Manager**
2. Install:
   - **Pipeline** (usually installed)
   - **Git** (usually installed)
   - **GitHub Branch Source** (for GitHub)
   - **GitLab Branch Source** (for GitLab)
   - **Organization Folders** (for GitHub Enterprise/GitLab)

### Step 2: Create Multibranch Pipeline Job

1. From Jenkins dashboard, click **New Item**
2. Enter name: `my-app`
3. Select **Multibranch Pipeline**
4. Click **OK**

### Step 3: Configure Branch Sources

#### For GitHub:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Branch Sources                                                     │
│  ───────────────                                                   │
│                                                                     │
│  [+ Add Source] ▼                                                 │
│    └── GitHub                                                      │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ Credentials:  [ GitHub App ▼ ] or [ Username/Password ]     │ │
│  │       ↓                                                        │ │
│  │ Owner:  [ your-username ]                                     │ │
│  │       ↓                                                        │ │
│  │ Repository:  [ your-repo ]                                   │ │
│  │       ↓                                                        │ │
│  │  [ ✓ ] Build origin/main only                                  │ │
│  │  [ ✓ ] Build origin/feature/* only                            │ │
│  └───────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────┘
```

#### For Git (Generic):

```
┌─────────────────────────────────────────────────────────────────────┐
│  Branch Sources                                                     │
│  [+ Add Source] ▼                                                 │
│    └── Git                                                         │
│                                                                     │
│  Project Repository:  [ https://github.com/user/repo.git ]        │
│                                                                     │
│  Credentials:  [ github-credentials ▼ ]                            │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ Behaviors:                                                   │ │
│  │  ├── Discover branches                                       │ │
│  │     ○ All branches                                           │ │
│  │     ○ Origin branches only  ← Recommended                   │ │
│  │  ├── Discover pull requests                                  │ │
│  │  ├── Discover tags                                           │ │
│  └───────────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 4: Configure Build Configuration

```
┌─────────────────────────────────────────────────────────────────────┐
│  Build Configuration                                                │
│  ───────────────────                                               │
│                                                                     │
│  Method:  [ ○ ] by Jenkinsfile                                      │
│           by default, Jenkins looks for 'Jenkinsfile' in the      │
│           root of each branch                                      │
│           (○) by Scan with                 │                        │
│                  Groovy script: [ ________ ]                       │
│                                                                     │
│  Script Path:  [ Jenkinsfile ]                                      │
│       ↓                                                             │
│  ✓ Lightweight checkout                                           │
│       (If checked, Jenkinsfile is loaded from the scm without     │
│       checking out the entire repository)                         │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 5: Configure Branch Filtering

Filter which branches to build:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Properties (optional)                                             │
│  ───────────────────                                               │
│                                                                     │
│  ✓ Filter by name (with wildcard)                                  │
│      Include:  [ */main */develop */feature/* ]                   │
│      Exclude:  [ */experimental/* ]                                │
│                                                                     │
│  ✓ Filter by age                                                   │
│      Build only branches older than:  [ 1 week ]                   │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 6: Save and Scan

Click **Save** — Jenkins will automatically scan the repository and create jobs for each branch containing a Jenkinsfile.

---

## Branch-Specific Conditions in Jenkinsfile

You can have different behavior per branch using `when` conditions:

```groovy
pipeline {
    agent any
    
    environment {
        // Different config per branch
        APP_ENV = env.GIT_BRANCH == 'main' ? 'production' : 'staging'
    }
    
    stages {
        stage('Build') {
            steps {
                echo "Building ${env.GIT_BRANCH}"
                sh 'make build'
            }
        }
        
        // Only run tests on feature branches and main
        stage('Test') {
            when {
                anyOf {
                    branch 'main'
                    branch 'develop'
                    branch 'feature/*'
                }
            }
            steps {
                sh 'make test'
            }
            post {
                always {
                    junit '**/test-results/*.xml'
                }
            }
        }
        
        // Only deploy from main branch
        stage('Deploy Production') {
            when {
                branch 'main'
            }
            steps {
                echo 'Deploying to production!'
                sh 'make deploy-prod'
            }
        }
        
        // Deploy develop to staging
        stage('Deploy Staging') {
            when {
                branch 'develop'
            }
            steps {
                echo 'Deploying to staging!'
                sh 'make deploy-staging'
            }
        }
    }
    
    post {
        always {
            echo "Branch: ${env.GIT_BRANCH}"
        }
        success {
            slackSend channel: '#builds',
                      color: 'good',
                      message: "Build ${env.BUILD_NUMBER} succeeded on ${env.GIT_BRANCH}"
        }
    }
}
```

---

## Understanding the Multibranch Dashboard

After setup, you'll see:

```
┌─────────────────────────────────────────────────────────────────────┐
│  my-app (Multibranch Pipeline)                            [ Scan ] │
│                                                                     │
│  [ Scan Repository Now ]  [ Branch Indexing ]                       │
│                                                                     │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ Branch Jobs                                                    │ │
│  │                                                                │ │
│  │ 🔵 main          - Last build: #45 - 5 min ago                │ │
│  │ 🔵 develop       - Last build: #102 - 1 hour ago               │ │
│  │ 🔵 feature/login - Last build: #3 - 2 days ago                 │ │
│  │ ⚪ bugfix/auth   - Last build: #1 - 5 days ago                │ │
│  │                                                                │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Jenkinsfile Discovery

Jenkins looks for Jenkinsfiles in:

| Location | Description |
|----------|-------------|
| `Jenkinsfile` | Root of repository (most common) |
| `jenkins/Jenkinsfile` | In subfolder |
| Custom path | Configure in job settings |

---

## Best Practices

### 1. Use a Common Jenkinsfile

Keep the same Jenkinsfile across all branches with conditional logic:

```groovy
pipeline {
    stages {
        // Common stages for all branches
        stage('Build') { ... }
        
        // Branch-specific via when
        stage('Deploy') { ... }
    }
}
```

### 2. Use Branch Variables

Use `env.GIT_BRANCH` to differentiate:

```groovy
environment {
    DEPLOY_TARGET = env.GIT_BRANCH == 'main' ? 'production' : 'staging'
}
```

### 3. Enable Webhooks

Configure GitHub/GitLab webhooks for instant triggers (covered next).

### 4. Protect Main Branch

Use branch protection in GitHub/GitLab to require reviews before merging to main.

---

## Common Mistakes

### Mistake 1: No Jenkinsfile in Branch

```groovy
// If a branch doesn't have a Jenkinsfile, it won't build!
```

**Solution**: Ensure all branches have a Jenkinsfile (even a minimal one)

### Mistake 2: Wrong Branch Filter

```groovy
// ❌ Build will never run - filter excludes all
when {
    branch 'main'  // Only main!
    branch 'develop'  // This is OR logic!
}

// ✅ Use anyOf for multiple
when {
    anyOf {
        branch 'main'
        branch 'develop'
    }
}
```

### Mistake 3: Forgetting to Scan

```
// If you add a new branch, you need to scan!
Click "Scan Repository Now" to discover new branches
```

---

## Next Steps

- **[GitHub Webhook Setup](03-github-webhook-setup.md)** - Trigger builds on push
- **[Docker Pipeline](../02-docker-integration/01-docker-pipeline-plugin.md)** - Build Docker images
- **[JUnit Plugin](../03-testing-and-quality/01-junit-plugin.md)** - Publish test results
