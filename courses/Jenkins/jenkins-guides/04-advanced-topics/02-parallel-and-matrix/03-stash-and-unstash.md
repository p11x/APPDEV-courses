# Stash and Unstash in Jenkins

## What this covers

This guide explains how to use `stash` and `unstash` to pass files between stages, especially when running parallel stages or when stages run on different agents. You'll learn why workspace doesn't persist between stages on different agents and how to work around this.

## Prerequisites

- Understanding of Pipeline stages
- Completed parallel stages guide

## The Workspace Problem

When stages run on different agents (or different nodes), the workspace is not shared:

```
Stage 1 (agent-1)          Stage 2 (agent-2)
┌─────────────────┐         ┌─────────────────┐
│ workspace/      │         │ workspace/      │
│   file.txt     │ ╳  →    │   (empty!)     │
│   built.jar    │         │                 │
└─────────────────┘         └─────────────────┘

Files built in Stage 1 are LOST in Stage 2!
```

**Solution**: Use `stash` to save files, `unstash` to restore them.

---

## Using stash

### Stash Files

```groovy
stage('Build') {
    steps {
        sh 'make build'
        
        // Stash files for later stages
        // name: identifier for this stash
        // includes: files to include (glob pattern)
        // excludes: files to exclude (optional)
        stash name: 'build-artifacts',
             includes: 'target/*.jar,dist/**/*',
             allowEmpty: false  // Fail if no files found
    }
}
```

### Unstash Files

```groovy
stage('Deploy') {
    steps {
        // Restore files from previous stash
        unstash name: 'build-artifacts'
        
        // Now files are available!
        sh 'java -jar target/app.jar'
    }
}
```

---

## Complete Example

```groovy
pipeline {
    agent any
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build') {
            steps {
                sh 'mvn clean package'
                
                // Stash build outputs
                stash name: 'maven-artifacts',
                     includes: 'target/*.jar',
                     allowEmpty: false
            }
        }
        
        stage('Test') {
            steps {
                sh 'mvn test'
                junit 'target/surefire-reports/*.xml'
            }
        }
        
        stage('Deploy') {
            agent { label 'deploy-agent' }
            steps {
                // Restore artifacts from different agent
                unstash name: 'maven-artifacts'
                
                sh 'java -jar target/myapp.jar'
            }
        }
    }
}
```

---

## Using with Parallel Stages

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                sh 'make build'
                
                // Stash before parallel
                stash name: 'build-output',
                     includes: 'dist/**/*'
            }
        }
        
        stage('Test in Parallel') {
            parallel {
                stage('Unit Tests') {
                    steps {
                        // Need build output
                        unstash 'build-output'
                        sh 'npm run test:unit'
                    }
                }
                
                stage('Integration Tests') {
                    steps {
                        // Need build output too!
                        unstash 'build-output'
                        sh 'npm run test:integration'
                    }
                }
            }
        }
    }
}
```

---

## Stash Options Reference

### stash

| Option | Type | Description |
|--------|------|-------------|
| `name` | String | Unique identifier (required) |
| `includes` | String | Files to include (glob) |
| `excludes` | String | Files to exclude (glob) |
| `allowEmpty` | boolean | Don't fail if empty (default: false) |
| `useDefaultExcludes` | boolean | Use default excludes (default: true) |

### unstash

| Option | Type | Description |
|--------|------|-------------|
| `name` | String | Name of stash to restore (required) |

---

## Multiple Stashes

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                // Multiple stashes for different purposes
                sh 'mvn clean package'
                stash name: 'java-jar', includes: 'target/*.jar'
                
                sh 'npm run build'
                stash name: 'npm-dist', includes: 'dist/**/*'
            }
        }
        
        stage('Deploy Backend') {
            steps {
                unstash name: 'java-jar'
                sh 'deploy-java.sh'
            }
        }
        
        stage('Deploy Frontend') {
            steps {
                unstash name: 'npm-dist'
                sh 'deploy-frontend.sh'
            }
        }
    }
}
```

---

## Stash in Matrix Builds

```groovy
matrix {
    axes {
        axis { name 'ENV' values 'dev', 'staging', 'prod' }
    }
    
    stages {
        stage('Build') {
            steps {
                sh "make build ENV=${ENV}"
                stash name: "build-${ENV}",
                      includes: "dist-${ENV}/**/*"
            }
        }
    }
}

stage('Deploy All') {
    steps {
        unstash 'build-dev'
        unstash 'build-staging'
        unstash 'build-prod'
    }
}
```

---

## When to Use Stash vs Artifact?

### Use Stash for:

- Temporary file passing between stages
- Files needed within the same build
- Files not needed after build completes

### Use Archive Artifacts for:

- Files to keep after build
- Files to download from Jenkins UI
- Files for future reference

```groovy
// Stash - temporary
stash name: 'temp-files', includes: '*.tmp'

// Archive - permanent
post {
    always {
        archiveArtifacts artifacts: 'target/*.jar', fingerprint: true
    }
}
```

---

## Common Mistakes

### Forgetting to Unstash

```groovy
// ❌ Files won't be available!
stage('Build') {
    steps {
        sh 'make build'
        stash name: 'artifacts'
    }
}

stage('Deploy') {
    steps {
        // Forgot to unstash!
        sh 'deploy.sh'  // Files not found!
    }
}

// ✅ Unstash first
stage('Deploy') {
    steps {
        unstash 'artifacts'  // Restore files
        sh 'deploy.sh'
    }
}
```

### Wrong Path

```groovy
// ❌ Wrong glob pattern
stash name: 'artifacts', includes: 'target/app.jar'

// ✅ Use correct pattern
stash name: 'artifacts', includes: 'target/*.jar'
// or
stash name: 'artifacts', includes: 'target/**/app.jar'
```

### Empty Stash Allowed

```groovy
// ❌ Will fail if no files
stash name: 'artifacts', includes: 'target/*.jar'

// ✅ Allow empty if optional
stash name: 'artifacts', includes: 'target/*.jar', allowEmpty: true
```

---

## Best Practices

1. **Stash Early, Unstash Later**: Stash right after creating files
2. **Use Descriptive Names**: `build-artifacts`, `test-results`
3. **Clean Up**: Don't stash unnecessary files
4. **Use Excludes**: Exclude node_modules, build directories

```groovy
// Good stash with excludes
stash name: 'source',
     includes: 'src/**/*,package.json',
     excludes: '**/node_modules/**,**/dist/**',
     useDefaultExcludes: true
```

---

## Next Steps

- **[Kubernetes Agents](03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Scale with K8s
- **[Docker Agents](.../03-plugins-and-integrations/02-docker-integration/02-docker-agent-in-pipeline.md)** - Use Docker
- **[Security Setup](04-security-and-rbac/01-security-realm-setup.md)** - Secure Jenkins
