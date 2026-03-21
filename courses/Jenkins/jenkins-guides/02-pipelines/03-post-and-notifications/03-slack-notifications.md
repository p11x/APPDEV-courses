# Slack Notifications in Jenkins

## What this covers

This guide explains how to configure the Slack Notification Plugin to send build notifications to Slack channels. You'll learn how to set up the Slack app, configure Jenkins, and use the slackSend step with different options.

## Prerequisites

- Completed Pipeline basics
- Slack Notification Plugin installed
- Access to create apps in Slack

---

## Setting Up Slack Integration

### Step 1: Create a Slack App

1. Go to https://api.slack.com/apps
2. Click **Create New App**
3. Choose **From scratch**
4. Enter app name (e.g., "Jenkins CI")
5. Select your workspace
6. Click **Create App**

### Step 2: Add Bot User

1. In your app settings, go to **Bot Users**
2. Click **Add a Bot User**
3. Set **Always Online** to Yes
4. Click **Save Changes**

### Step 3: Install App to Workspace

1. Go to **Install Apps** (in sidebar)
2. Click **Install to Workspace**
3. Review and click **Allow**

### Step 4: Get OAuth Token

1. After installing, you'll see **OAuth Tokens for Your Workspace**
2. Copy the **Bot User OAuth Access Token** (starts with `xoxb-`)
3. **This is your token** - keep it secret!

### Step 5: Add to Channel

1. In Slack, go to your desired channel
2. Type `/invite @your-bot-name`
3. Or go to channel settings → Apps → Add app

---

## Configuring Slack in Jenkins

### Step 1: Add Slack Credentials

1. Go to **Manage Jenkins** → **Credentials**
2. Click **Add Credentials**
3. Kind: **Secret text**
4. Secret: Your Slack Bot OAuth Token (`xoxb-...`)
5. ID: `slack-token` (or your preferred ID)
6. Click **OK**

### Step 2: Configure Slack in Jenkins

1. Go to **Manage Jenkins** → **Configure System**
2. Find **Slack**
3. Fill in:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Slack                                                           │
│                                                                     │
│  Workspace:  [ your-workspace-name ]                               │
│       ↓                                                             │
│  Default channel name:  [ #builds ]                                │
│       ↓                                                             │
│  Credential:  [ slack-token ▼ ]                                  │
│       ↓                                                             │
│  Team domain:  [ your-company ]                                   │
│       ↓                                                             │
│  ☑ Add default recipients?                                       │
│       ↓                                                             │
│  Access Token:  [ slack-token ] (from credentials)              │
└─────────────────────────────────────────────────────────────────────┘
```

4. Click **Test Connection**
5. You should see a success message in Slack!

---

## Using slackSend in Pipeline

### Basic Example

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                echo 'Building...'
            }
        }
    }
    
    post {
        success {
            slackSend channel: '#builds',
                      color: 'good',
                      message: "Build ${env.BUILD_NUMBER} succeeded in ${env.JOB_NAME}"
        }
        failure {
            slackSend channel: '#builds',
                      color: 'danger',
                      message: "Build ${env.BUILD_NUMBER} FAILED in ${env.JOB_NAME}"
        }
    }
}
```

### Full Parameter Reference

```groovy
post {
    always {
        slackSend (
            // Channel name (or use default from config)
            channel: '#ci-cd',
            
            // Color for the message
            // 'good' (green), 'warning' (yellow), 'danger' (red), or hex color
            color: 'good',
            
            // Message text (supports Slack formatting)
            message: """
                *Build ${env.BUILD_NUMBER}*
                Job: ${env.JOB_NAME}
                Status: ${currentBuild.result}
                URL: ${env.BUILD_URL}
            """,
            
            // Team domain (workspace name)
            teamDomain: 'mycompany',
            
            // Bot user to send as
            botUser: true,
            
            // Attach build log
            attachLog: true,
            
            // Include changes
            includeChanges: true,
            
            // Custom token (if not using global config)
            token: 'xoxb-...'
        )
    }
}
```

---

## Color Codes

| Color | Meaning | When to Use |
|-------|---------|-------------|
| `good` | Green | Success |
| `warning` | Yellow | Unstable |
| `danger` | Red | Failure |
| `#FF0000` | Custom | Any |

---

## Rich Slack Messages

### With Blocks (Modern Format)

```groovy
post {
    success {
        slackSend channel: '#builds',
                  color: 'good',
                  message: "Build Succeeded",
                  teamDomain: 'mycompany',
                  tokenCredentialId: 'slack-token'
        
        // Alternative: using blocks for rich formatting
        slackSend channel: '#builds',
                  color: 'good',
                  blocks: [
                    [
                        type: "section",
                        text: [
                            type: "mrkdwn",
                            text: "*Build Succeeded!* ✅"
                        ]
                    ],
                    [
                        type: "section",
                        fields: [
                            [
                                type: "mrkdwn",
                                text: "*Job:*\n${env.JOB_NAME}"
                            ],
                            [
                                type: "mrkdwn",
                                text: "*Build #:*\n${env.BUILD_NUMBER}"
                            ],
                            [
                                type: "mrkdwn",
                                text: "*Branch:*\n${env.GIT_BRANCH}"
                            ]
                        ]
                    ],
                    [
                        type: "actions",
                        elements: [
                            [
                                type: "button",
                                text: [
                                    type: "plain_text",
                                    text: "View Build"
                                ],
                                url: "${env.BUILD_URL}"
                            ]
                        ]
                    ]
                ]
    }
    
    failure {
        slackSend channel: '#builds',
                  color: 'danger',
                  message: "Build FAILED! ❌",
                  blocks: [
                    [
                        type: "section",
                        text: [
                            type: "mrkdwn",
                            text: "*Build Failed!* ❌\n\n*Job:* ${env.JOB_NAME}\n*Build:* #${env.BUILD_NUMBER}"
                        ]
                    ]
                ]
    }
}
```

---

## Conditional Notifications

### Only Notify on Failure

```groovy
post {
    failure {
        slackSend channel: '#oncall',
                  color: 'danger',
                  message: "🚨 *CRITICAL*: Build ${env.BUILD_NUMBER} FAILED in ${env.JOB_NAME}!"
    }
}
```

### Notify on Status Change

```groovy
post {
    changed {
        def color = currentBuild.result == 'SUCCESS' ? 'good' : 'danger'
        def emoji = currentBuild.result == 'SUCCESS' ? '✅' : '❌'
        
        slackSend channel: '#builds',
                  color: color,
                  message: "${emoji} Build ${env.BUILD_NUMBER} status changed: ${currentBuild.result}"
    }
}
```

### Different Channels Per Branch

```groovy
post {
    always {
        def channel = env.GIT_BRANCH == 'main' ? '#production-builds' : '#dev-builds'
        
        slackSend channel: channel,
                  color: currentBuild.result == 'SUCCESS' ? 'good' : 'danger',
                  message: "Build ${env.BUILD_NUMBER} - ${currentBuild.result}"
    }
}
```

---

## Complete Example

```groovy
pipeline {
    agent any
    
    environment {
        SLACK_CHANNEL = '#ci-cd'
    }
    
    stages {
        stage('Checkout') {
            steps {
                echo 'Checking out code...'
                checkout scm
            }
        }
        
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
                    junit '**/test-results/*.xml'
                }
            }
        }
        
        stage('Deploy') {
            when {
                branch 'main'
            }
            steps {
                echo 'Deploying...'
            }
        }
    }
    
    post {
        always {
            // Always notify
            slackSend channel: env.SLACK_CHANNEL,
                      color: currentBuild.result == 'SUCCESS' ? 'good' : 
                             currentBuild.result == 'UNSTABLE' ? 'warning' : 'danger',
                      message: """
                          *${currentBuild.result ?: 'SUCCESS'}* - ${env.JOB_NAME} #${env.BUILD_NUMBER}
                          Branch: ${env.GIT_BRANCH}
                          Duration: ${currentBuild.durationString}
                      """.stripIndent()
        }
        
        success {
            // Extra notification for production
            when {
                branch 'main'
            }
            slackSend channel: '#production-deployments',
                      color: 'good',
                      message: "🎉 Production deployment #${env.BUILD_NUMBER} succeeded!"
        }
        
        failure {
            // Alert on-call for main branch failures
            when {
                branch 'main'
            }
            slackSend channel: '#oncall',
                      color: 'danger',
                      message: "🚨 PRODUCTION BUILD FAILED! Immediate attention required!"
        }
    }
}
```

---

## Common Mistakes

### Mistake 1: Wrong Token

```groovy
// ❌ WRONG - Using Slack API token instead of Bot OAuth token
token: 'xoxa2-...'

// ✅ CORRECT - Use Bot User OAuth token (starts with xoxb-)
token: 'xoxb-1234567890123-...'
```

### Mistake 2: Wrong Channel Name

```groovy
// ❌ WRONG - Wrong format
channel: 'builds'  // Missing #

// ✅ CORRECT
channel: '#builds'
```

### Mistake 3: No Credentials Configured

```groovy
// ❌ WON'T WORK - No global config
slackSend message: 'Hello'

// ✅ CORRECT - Either configure globally OR pass token
slackSend message: 'Hello', tokenCredentialId: 'slack-token'
```

---

## Next Steps

- **[Git Plugin Setup](../03-plugins-and-integrations/01-git-and-scm/01-git-plugin-setup.md)** - Configure Git integration
- **[GitHub Webhooks](../03-plugins-and-integrations/01-git-and-scm/03-github-webhook-setup.md)** - Auto-trigger builds
- **[Docker Pipeline](../03-plugins-and-integrations/02-docker-integration/01-docker-pipeline-plugin.md)** - Build Docker images
