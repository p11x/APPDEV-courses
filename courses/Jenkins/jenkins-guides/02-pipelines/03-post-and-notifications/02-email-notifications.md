# Email Notifications in Jenkins

## What this covers

This guide explains how to configure and use the Email Extension Plugin (`emailext`) for sending rich email notifications from your pipelines. You'll learn about all the parameters, how to use Groovy templates, and configure SMTP settings.

## Prerequisites

- Completed Pipeline basics
- Email Extension Plugin installed (usually included in suggested plugins)
- SMTP configured in Jenkins global settings

## Configuring Email in Jenkins

### Step 1: Configure SMTP Server

Go to **Manage Jenkins** → **Configure System** → **E-mail Notification**:

```
┌─────────────────────────────────────────────────────────────────────┐
│  E-mail Notification                                                │
│                                                                     │
│  SMTP server:  [ smtp.gmail.com ]                                   │
│  SMTP port:   [ 587 ]              ← Use 587 for TLS               │
│  ☐ Use SSL:                                           ← Check for SSL│
│  Default user e-mail suffix:  [ @gmail.com ]                       │
│                                                                     │
│  [ Advanced ] → Configure credentials, charset, etc.               │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: Configure Extended Email Notification

Find **Extended E-mail Notification** section (if plugin installed):

```
┌─────────────────────────────────────────────────────────────────────┐
│  Extended E-mail Notification                                       │
│                                                                     │
│  SMTP server:  [ smtp.gmail.com ]                                  │
│  Default subject:  [ $PROJECT_NAME - Build #$BUILD_NUMBER - ... ] │
│                                                                     │
│  Default content:                                                   │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │ $PROJECT_NAME - $BUILD_STATUS!                               │ │
│  │ Build #: $BUILD_NUMBER                                      │ │
│  │ Build URL: $BUILD_URL                                       │ │
│  │ ----------------------------------------------------------------│
│  │ $CHANGES                                                      │ │
│  └───────────────────────────────────────────────────────────────┘ │
│                                                                     │
│  Default recipients:  [ $DEFAULT_RECIPIENTS ]                     │
│  ☑ Attach build log                                                │
│  ☑ Include last build failure                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Using emailext in Pipeline

### Basic Example

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
    }
    
    post {
        success {
            emailext subject: "SUCCESS: ${env.JOB_NAME}",
                     body: "Build ${env.BUILD_NUMBER} succeeded!",
                     to: 'dev-team@company.com'
        }
        failure {
            emailext subject: "FAILURE: ${env.JOB_NAME}",
                     body: "Build ${env.BUILD_NUMBER} FAILED!",
                     to: 'dev-team@company.com'
        }
    }
}
```

### Full Parameter Reference

```groovy
post {
    always {
        emailext (
            // Subject line of the email
            subject: "BUILD ${currentBuild.result ?: 'STARTED'}: ${env.JOB_NAME}",
            
            // Body of the email (supports HTML)
            body: """
                <h2>Build Status: ${currentBuild.result ?: 'RUNNING'}</h2>
                <p>Project: ${env.JOB_NAME}</p>
                <p>Build #: ${env.BUILD_NUMBER}</p>
                <p>URL: <a href="${env.BUILD_URL}">${env.BUILD_URL}</a></p>
                <p>Branch: ${env.GIT_BRANCH}</p>
            """,
            
            // Recipients (comma-separated)
            to: 'dev-team@company.com,qa-team@company.com',
            
            // CC recipients
            cc: 'manager@company.com',
            
            // BCC recipients  
            bcc: 'archive@company.com',
            
            // Reply-To address
            replyTo: 'jenkins@company.com',
            
            // Attach build log
            attachLog: true,
            
            // Compress log into single file
            compressLog: true,
            
            // Attach pattern (files to attach)
            attachmentsPattern: 'build/reports/*.html',
            
            // Email format (default: html)
            mimeType: 'text/html',
            
            // Recipient providers
            recipientProviders: [
                // Always send to list
                recipients(),
                // Send to developers who made changes
                developers(),
                // Send to requester (who triggered build)
                requester(),
                // Send to list of watchers (if configured)
                watchers()
            ]
        )
    }
}
```

---

## Email Variables Reference

### Built-in Variables

| Variable | Description |
|----------|-------------|
| `${env.BUILD_NUMBER}` | Build number |
| `${env.BUILD_URL}` | URL to build |
| `${env.JOB_NAME}` | Job name |
| `${env.WORKSPACE}` | Workspace path |
| `${env.BUILD_STATUS}` | Current build status |
| `${env.CHANGES}` | Changes since last build |
| `${env.GIT_BRANCH}` | Git branch |
| `${env.GIT_COMMIT}` | Git commit |

### CurrentBuild Variables

| Variable | Description |
|----------|-------------|
| `${currentBuild.result}` | SUCCESS, FAILURE, UNSTABLE, etc. |
| `${currentBuild.duration}` | Build duration in ms |
| `${currentBuild.number}` | Build number |
| `${currentBuild.displayName}` | Display name |

---

## Groovy Templates for Email Body

### Basic Template

```groovy
def emailTemplate() {
    return """
        <h2>${env.JOB_NAME} - Build #${env.BUILD_NUMBER}</h2>
        
        <p><b>Status:</b> ${currentBuild.result ?: 'SUCCESS'}</p>
        <p><b>Duration:</b> ${currentBuild.durationString}</p>
        <p><b>Branch:</b> ${env.GIT_BRANCH}</p>
        
        <h3>Changes:</h3>
        ${env.CHANGES ? env.CHANGES : 'No changes'}
        
        <h3>Build Log:</h3>
        <p><a href="${env.BUILD_URL}console">View full log</a></p>
    """
}

post {
    always {
        emailext subject: "${env.JOB_NAME} - Build #${env.BUILD_NUMBER} - ${currentBuild.result}",
                 body: emailTemplate(),
                 to: 'team@company.com'
    }
}
```

### Detailed Template with Changes

```groovy
def createEmailBody() {
    def changeLog = ''
    
    // Get changes from current build
    def changes = currentBuild.changeSets
    if (changes) {
        changes.each { changeSet ->
            changeSet.items.each { item ->
                changeLog += "<li>${item.msg} (${item.author})</li>"
            }
        }
    }
    
    return """
        <h2>Build ${currentBuild.result}</h2>
        
        <table>
            <tr><td><b>Project:</b></td><td>${env.JOB_NAME}</td></tr>
            <tr><td><b>Build:</b></td><td>#${env.BUILD_NUMBER}</td></tr>
            <tr><td><b>URL:</b></td><td><a href="${env.BUILD_URL}">${env.BUILD_URL}</a></td></tr>
            <tr><td><b>Branch:</b></td><td>${env.GIT_BRANCH}</td></tr>
        </table>
        
        <h3>Changes:</h3>
        <ul>${changeLog ?: 'No changes'}</ul>
        
        <h3>Build Log:</h3>
        <p><a href="${env.BUILD_URL}console">View Console Output</a></p>
    """
}
```

---

## Conditional Email Recipients

Send to different people based on results:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                sh 'make build'
            }
        }
    }
    
    post {
        always {
            // Determine recipients based on branch
            def recipients = 'dev-team@company.com'
            def subject = "${env.JOB_NAME} - Build #${env.BUILD_NUMBER}"
            
            if (env.GIT_BRANCH == 'main') {
                recipients += ',release-team@company.com'
                subject = '[PROD] ' + subject
            }
            
            emailext subject: subject,
                     body: "Build ${env.BUILD_NUMBER} finished: ${currentBuild.result}",
                     to: recipients
        }
    }
}
```

---

## Sending HTML Emails

```groovy
post {
    success {
        emailext mimeType: 'text/html',
                 subject: "✅ SUCCESS: ${env.JOB_NAME}",
                 body: """
                     <html>
                     <body>
                         <h2 style="color: green;">✅ Build Succeeded</h2>
                         
                         <table style="border: 1px solid #ddd;">
                             <tr>
                                 <td><b>Project:</b></td>
                                 <td>${env.JOB_NAME}</td>
                             </tr>
                             <tr>
                                 <td><b>Build #:</b></td>
                                 <td>${env.BUILD_NUMBER}</td>
                             </tr>
                             <tr>
                                 <td><b>Branch:</b></td>
                                 <td>${env.GIT_BRANCH}</td>
                             </tr>
                             <tr>
                                 <td><b>Duration:</b></td>
                                 <td>${currentBuild.durationString}</td>
                             </tr>
                         </table>
                         
                         <p><a href="${env.BUILD_URL}">View Build</a></p>
                     </body>
                     </html>
                 """,
                 to: 'team@company.com'
    }
}
```

---

## Common Mistakes

### Mistake 1: No SMTP Configured

```groovy
// ❌ WON'T WORK - No SMTP server configured
emailext subject: 'Test', body: 'Body', to: 'test@example.com'
```

**Solution**: Configure SMTP in Manage Jenkins → Configure System

### Mistake 2: Wrong Variable Syntax

```groovy
// ❌ WRONG - Using Groovy string without proper escaping
subject: "Build $BUILD_NUMBER"  // $BUILD_NUMBER won't expand

// ✅ CORRECT
subject: "Build ${env.BUILD_NUMBER}"
```

### Mistake 3: Not Using Always Block

```groovy
// ❌ WRONG - Email won't send on failure if only in success block
post {
    success {
        emailext ...  // Only runs on success
    }
}

// ✅ CORRECT
post {
    always {
        emailext ...  // Runs always
    }
}
```

---

## Next Steps

- **[Slack Notifications](03-slack-notifications.md)** - Send notifications to Slack
- **[JUnit Plugin](../03-plugins-and-integrations/03-testing-and-quality/01-junit-plugin.md)** - Publish test results
- **[Secret Masking](02-environment-and-credentials/03-secret-masking.md)** - Secure your credentials
