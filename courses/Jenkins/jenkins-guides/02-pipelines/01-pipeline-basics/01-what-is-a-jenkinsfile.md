# What is a Jenkinsfile?

## What this covers

This guide explains what a Jenkinsfile is, why you should store your CI/CD pipeline as code in your repository, and the difference between Scripted and Declarative Pipeline syntax. You'll understand why Jenkinsfile is like a recipe card for Jenkins.

## Prerequisites

- Completed all guides in "Getting Started" section
- Basic understanding of CI/CD concepts
- Familiarity with Git version control

## What is a Jenkinsfile?

A **Jenkinsfile** is a text file that defines your CI/CD pipeline. It's stored in your source code repository alongside your application code.

```
your-project/
├── src/
│   └── (your code)
├── package.json
├── Jenkinsfile          ← Your pipeline definition
└── README.md
```

## Why Use a Jenkinsfile?

### The Problem: UI-Driven Jobs

Before Jenkinsfiles, pipelines were defined through the Jenkins web UI:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Job Configuration (in the UI)                                      │
│                                                                     │
│  Step 1: [Checkout code] ← Click to add                           │
│  Step 2: [Build]        ← Click to add                           │
│  Step 3: [Test]         ← Click to add                           │
│  Step 4: [Deploy]       ← Click to add                           │
│                                                                     │
│  Problems:                                                          │
│  ❌ No version control                                             │
│  ❌ Hard to review changes                                         │
│  ❌ Hard to replicate jobs                                          │
│  ❌ Knowledge trapped in Jenkins                                   │
└─────────────────────────────────────────────────────────────────────┘
```

### The Solution: Pipeline as Code

With Jenkinsfile, your pipeline lives in your repo:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Jenkinsfile (in your repository)                                  │
│                                                                     │
│  pipeline {                                                        │
│      stages {                                                      │
│          stage('Build') { ... }                                   │
│          stage('Test') { ... }                                    │
│          stage('Deploy') { ... }                                   │
│      }                                                             │
│  }                                                                 │
│                                                                     │
│  Benefits:                                                         │
│  ✅ Version controlled (git history)                               │
│  ✅ Code review for pipeline changes                               │
│  ✅ Easy to replicate (just clone repo)                           │
│  ✅ Self-documenting                                               │
└─────────────────────────────────────────────────────────────────────┘
```

## Jenkinsfile as a Recipe Analogy

Think of a Jenkinsfile like a recipe card:

> **Recipe Card (Jenkinsfile)**: "How to make this app"
> 
> Like a recipe lists ingredients and steps, a Jenkinsfile lists:
> - **Environment** (ingredients): What tools, credentials, dependencies
> - **Stages** (steps): Build, Test, Deploy
> - **Steps** (instructions): The actual commands to run
> 
> Just as you keep recipes in a cookbook, you keep Jenkinsfiles in your repo!

---

## Scripted vs Declarative Syntax

Jenkins supports two Pipeline syntaxes:

### Scripted Pipeline (Legacy)

```groovy
// Scripted Pipeline - older syntax
node {
    stage('Build') {
        echo 'Building...'
    }
    stage('Test') {
        echo 'Testing...'
    }
    stage('Deploy') {
        echo 'Deploying...'
    }
}
```

### Declarative Pipeline (Recommended)

```groovy
// Declarative Pipeline - modern, recommended syntax
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                echo 'Building...'
            }
        }
        stage('Test') {
            steps {
                echo 'Testing...'
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying...'
            }
        }
    }
}
```

### Comparison Table

| Feature | Scripted | Declarative |
|---------|----------|-------------|
| **Syntax** | Groovy-based | YAML-like |
| **Learning curve** | Steeper | Easier |
| **Validation** | Runtime | Pre-run validation |
| **Structure** | Flexible | Enforced |
| **Documentation** | Less standardized | Well-documented |
| **Recommended** | ❌ Legacy | ✅ Yes |

**Recommendation**: Always use **Declarative Pipeline**. It's the modern standard, easier to learn, and has better tooling.

---

## The Pipeline Block Structure

A Declarative Pipeline has a structured format:

```groovy
pipeline {                              // Required: Top-level block
    agent any                            // Where to run: Any available agent
    
    environment {                       // Environment variables
        APP_NAME = 'my-app'
    }
    
    options {                           // Pipeline options
        timeout(time: 30, unit: 'MINUTES')
    }
    
    parameters {                        // Input parameters
        string(name: 'VERSION', defaultValue: '1.0.0')
    }
    
    triggers {                          // When to run
        cron('H 2 * * *')
    }
    
    stages {                            // The work
        stage('Build') {
            steps {
                echo 'Building...'
            }
        }
    }
    
    post {                              // After stages
        always {
            echo 'Done!'
        }
    }
}
```

---

## Creating a Jenkinsfile

### Step 1: Create the File

In your project root, create a file named `Jenkinsfile`:

```bash
# Using echo (Linux/macOS)
echo 'pipeline { agent any; stages { stage("Build") { steps { echo "Hello" } } } }' > Jenkinsfile

# Using PowerShell (Windows)
"pipeline { agent any; stages { stage('Build') { steps { echo 'Hello' } } } }" | Out-File -FilePath Jenkinsfile
```

### Step 2: Add Basic Pipeline

```groovy
pipeline {
    agent any
    
    stages {
        stage('Hello') {
            steps {
                echo 'Hello from Jenkins Pipeline!'
            }
        }
    }
}
```

### Step 3: Commit to Git

```bash
git add Jenkinsfile
git commit -m "Add Jenkinsfile"
git push origin main
```

### Step 4: Create Pipeline Job in Jenkins

1. In Jenkins, click **New Item**
2. Enter job name (e.g., `my-pipeline`)
3. Select **Pipeline**
4. Click **OK**
5. In Pipeline section, select **Pipeline script from SCM**
6. Configure Git repository URL
7. Click **Save**

---

## Jenkinsfile Benefits Summary

| Benefit | Explanation |
|---------|-------------|
| **Version Control** | Pipeline changes are tracked in Git |
| **Code Review** | Pull requests can review pipeline changes |
| **Reproducibility** | Clone repo = get the pipeline |
| **Branching** | Different branches can have different pipelines |
| **Audit Trail** | Know who changed what and when |
| **Self-Documentation** | Pipeline describes the build process |

---

## Next Steps

- **[Declarative Syntax Overview](02-declarative-syntax-overview.md)** - Detailed breakdown of Declarative Pipeline
- **[Stages and Steps](03-stages-and-steps.md)** - Deep dive into stages and steps
- **[Pipeline Agents](04-pipeline-agents.md)** - Where your pipeline runs
