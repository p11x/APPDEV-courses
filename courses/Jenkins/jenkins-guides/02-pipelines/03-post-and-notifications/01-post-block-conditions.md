# Post Block Conditions

## What this covers

This guide explains all the `post` block conditions in Jenkins Declarative Pipeline: `always`, `success`, `failure`, `unstable`, `changed`, `fixed`, `regression`, `cleanup`, and `aborted`. You'll learn when each condition runs and how to use them for notifications and cleanup.

## Prerequisites

- Understanding of Declarative Pipeline
- Completed Pipeline basics guides

## The post Block

The `post` block runs after all stages complete, regardless of how the pipeline finished. It's used for:
- Notifications (email, Slack, etc.)
- Cleanup (delete workspace, artifacts)
- Archive results
- Post-processing

---

## All Post Conditions

### always

Runs **in every situation**—success, failure, unstable, or aborted.

```groovy
post {
    always {
        echo 'This always runs - no matter what!'
        // Common use: cleanup
        cleanWs deleteDirs: true
    }
}
```

**Use for**: Cleanup, archiving, notifications that should always send.

---

### success

Runs only when the pipeline **succeeded** (exit code 0).

```groovy
post {
    success {
        echo 'The pipeline succeeded!'
        // Notify team of success
        slackSend channel: '#builds', color: 'good', message: "Build ${env.BUILD_NUMBER} succeeded"
    }
}
```

**Use for**: Success notifications, merging PRs, promoting builds.

---

### failure

Runs only when the pipeline **failed**.

```groovy
post {
    failure {
        echo 'The pipeline failed!'
        // Alert the team
        slackSend channel: '#builds', color: 'danger', message: "Build ${env.BUILD_NUMBER} FAILED!"
    }
}
```

**Use for**: Failure alerts, paging on-call engineers.

---

### unstable

Runs when the pipeline is **unstable** (usually from test failures).

```groovy
post {
    unstable {
        echo 'Pipeline is unstable - tests may have failed'
        // Still notify, but with warning
        emailext subject: "Unstable: ${env.JOB_NAME} #${env.BUILD_NUMBER}",
                 body: "Tests failed. Check console output."
    }
}
```

**Use for**: Different severity notifications.

---

### changed

Runs when the build status is **different from the previous build**.

```groovy
post {
    changed {
        echo 'Build status changed!'
        // Notify about status change
        slackSend channel: '#builds', 
                  message: "Build ${env.BUILD_NUMBER} status changed: ${currentBuild.result}"
    }
}
```

**Use for**: Notifications only when status changes (e.g., was failing, now passing).

---

### fixed

Runs when the current build is **successful** and the previous build **failed or unstable**.

```groovy
post {
    fixed {
        echo 'Build is fixed! Was failing before.'
        slackSend channel: '#builds', color: 'good', 
                  message: "Build ${env.BUILD_NUMBER} FIXED! Back to passing."
    }
}
```

**Use for**: Celebrating fixes, de-escalating alerts.

---

### regression

Runs when the current build **failed or unstable** and the previous build was **successful**.

```groovy
post {
    regression {
        echo 'Regression detected! Was passing before.'
        // Escalate - this is a regression!
        slackSend channel: '#alerts', color: 'danger', 
                  message: "REGRESSION in ${env.BUILD_NUMBER}!"
    }
}
```

**Use for**: Alerting about broken builds that were previously passing.

---

### aborted

Runs when the build was **manually cancelled** or **timed out**.

```groovy
post {
    aborted {
        echo 'Build was aborted'
        // May want to notify
        slackSend channel: '#builds', message: "Build ${env.BUILD_NUMBER} was aborted"
    }
}
```

**Use for**: Notifying about cancelled builds.

---

### cleanup

Runs **last**, after all other post conditions, **regardless of status**.

```groovy
post {
    always {
        echo 'I run first'
    }
    success {
        echo 'I run on success'
    }
    failure {
        echo 'I run on failure'
    }
    cleanup {
        echo 'I run LAST - after everything else'
        // Good for final cleanup
        cleanWs()
    }
}
```

**Use for**: Final cleanup that must run after everything else.

---

## Complete Example: Real-World Post Block

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                echo 'Building...'
                sh 'make build'
            }
        }
        
        stage('Test') {
            steps {
                echo 'Testing...'
                sh 'make test'
            }
            post {
                always {
                    // Always publish test results
                    junit '**/test-results/*.xml'
                }
            }
        }
    }
    
    // Complete post block with all conditions
    post {
        // 1. Always cleanup workspace
        always {
            echo 'Cleaning up...'
            cleanWs deleteDirs: true, notFailBuild: true
        }
        
        // 2. Success notifications
        success {
            echo 'Build succeeded!'
            slackSend channel: '#ci-cd',
                      color: 'good',
                      message: "✅ Build ${env.BUILD_NUMBER} succeeded in ${env.JOB_NAME}"
            emailext subject: "SUCCESS: ${env.JOB_NAME}",
                     body: "Build ${env.BUILD_NUMBER} succeeded.\nCheck console: ${env.BUILD_URL}",
                     to: 'dev-team@company.com'
        }
        
        // 3. Failure notifications
        failure {
            echo 'Build failed!'
            slackSend channel: '#ci-cd',
                      color: 'danger',
                      message: "❌ Build ${env.BUILD_NUMBER} FAILED in ${env.JOB_NAME}"
            // More urgent email
            emailext subject: "🚨 FAILURE: ${env.JOB_NAME}",
                     body: "Build ${env.BUILD_NUMBER} FAILED!\nCheck console: ${env.BUILD_URL}",
                     to: 'dev-team@company.com,oncall@company.com'
        }
        
        // 4. Handle regressions
        regression {
            echo 'Regression detected!'
            slackSend channel: '#alerts',
                      color: 'warning',
                      message: "⚠️ REGRESSION: ${env.JOB_NAME} was passing before!"
        }
        
        // 5. Handle fixes
        fixed {
            echo 'Fixed!'
            slackSend channel: '#ci-cd',
                      color: 'good',
                      message: "🎉 Build ${env.BUILD_NUMBER} is fixed!"
        }
        
        // 6. Final cleanup (runs last)
        cleanup {
            echo 'Final cleanup - always runs last'
        }
    }
}
```

---

## Condition Execution Order

When multiple conditions match, they execute in this order:

```
1. always        - Always runs first (after stages)
2. changed      - If status changed
3. fixed        - If fixed (before regression)
4. regression   - If regressed (after fixed)
5. failure      - If failed
6. unstable     - If unstable
7. aborted      - If aborted
8. success      - If succeeded
9. cleanup      - Always runs LAST
```

---

## Common Post Patterns

### Pattern 1: Simple Success/Failure

```groovy
post {
    success {
        slackSend channel: '#builds', color: 'good', message: 'Build succeeded'
    }
    failure {
        slackSend channel: '#builds', color: 'danger', message: 'Build failed'
    }
}
```

### Pattern 2: Always Cleanup

```groovy
post {
    always {
        cleanWs()
    }
}
```

### Pattern 3: Archive Everything

```groovy
post {
    always {
        archiveArtifacts artifacts: 'build/**/*', fingerprint: true
    }
}
```

### Pattern 4: Status Change Detection

```groovy
post {
    changed {
        echo "Build status changed from ${previousBuild.result} to ${currentBuild.result}"
    }
}
```

---

## Common Mistakes

### Mistake 1: Wrong Condition Order

```groovy
// ❌ WRONG - Using 'success' after 'always'
post {
    success {
        // This might not run because always already ran
    }
    always {
        // This should be last!
    }
}

// ✅ CORRECT - 'always' first, then specific conditions
post {
    always {
        // Cleanup first
    }
    success {
        // Then success-specific
    }
}
```

### Mistake 2: Missing Always Block

```groovy
// ❌ WRONG - No cleanup on failure!
post {
    success {
        echo 'Success'
    }
    // No always = no cleanup on failure!
}

// ✅ CORRECT - Always cleanup
post {
    always {
        cleanWs()  // Always cleanup
    }
    success {
        // Notify success
    }
}
```

### Mistake 3: Too Many Notifications

```groovy
// ⚠️ WARNING - Don't spam notifications!
post {
    always {
        slackSend ...  // Too many!
    }
    success {
        slackSend ...  // And more!
    }
    failure {
        slackSend ...  // Keep it simple
    }
}
```

---

## Next Steps

- **[Email Notifications](02-email-notifications.md)** - Configure detailed emails
- **[Slack Notifications](03-slack-notifications.md)** - Configure Slack integration
- **[JUnit Plugin](../03-plugins-and-integrations/03-testing-and-quality/01-junit-plugin.md)** - Publish test results
