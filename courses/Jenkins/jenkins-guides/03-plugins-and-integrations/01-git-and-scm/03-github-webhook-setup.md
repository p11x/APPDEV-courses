# GitHub Webhook Setup

## What this covers

This guide explains how to create a GitHub webhook that triggers Jenkins builds on push. You'll learn the payload URL format, content type, secret token configuration, and how Jenkins processes webhook payloads.

## Prerequisites

- GitHub account with repository
- Jenkins with GitHub Branch Source Plugin installed
- Multibranch Pipeline or regular Pipeline job configured
- GitHub repository admin access (to add webhooks)

---

## How Webhooks Work

```
┌─────────┐          ┌─────────┐          ┌─────────┐
│ Developer│          │ GitHub   │          │ Jenkins │
│   Push   │─────────►│Receives  │─────────►│Triggers │
│   Code   │          │  Push    │          │  Build  │
└─────────┘          └─────────┘          └─────────┘
                           │
                    Webhook Payload
                    (JSON with
                    commit info)
```

1. Developer pushes code to GitHub
2. GitHub sends webhook HTTP POST to Jenkins
3. Jenkins processes payload and triggers builds
4. Build runs with new code

---

## Step 1: Get Jenkins Webhook URL

### For Multibranch Pipeline

The webhook URL is:
```
http://JENKINS_URL/job/REPO_NAME/build
```

Example:
```
http://jenkins.example.com/job/my-app/build
```

### For GitHub Branch Source Plugin

Just add the repository in Jenkins — webhooks are configured automatically!

If using GitHub Branch Source Plugin, Jenkins automatically registers webhooks when you add the repository.

---

## Step 2: Configure GitHub Webhook

### Option A: Automatic (GitHub Branch Source)

When you add a GitHub repository using GitHub Branch Source:
1. Jenkins automatically configures webhooks
2. No manual setup needed
3. Works immediately

### Option B: Manual Webhook

1. Go to your GitHub repository
2. Click **Settings** → **Webhooks** → **Add webhook**

```
┌─────────────────────────────────────────────────────────────────────┐
│  Add webhook                                                       │
│                                                                     │
│  Payload URL:  [ http://jenkins.example.com/github-webhook/ ]    │
│       ↓                                                             │
│  Content type:  [ application/json ]                               │
│       ↓                                                             │
│  Secret:        [ __________________ ]  ← Optional                 │
│       ↓                                                             │
│  Which events would you like to trigger this webhook?               │
│    ○ Just the push event                                            │
│    ● Let me select individual events                               │
│       ✓ Pushes                                                      │
│       ✓ Pull requests                                               │
│                                                                     │
│  [ ✓ Active ]                                                       │
│       ↓                                                             │
│  [ Add webhook ]                                                   │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 3: Configure Events

Select which events trigger builds:

| Event | When It Triggers |
|-------|------------------|
| **Pushes** | Any push to any branch |
| **Pull requests** | PR created/updated |
| **Branch or tag creation** | New branch/tag created |
| **Branch or tag deletion** | Branch/tag deleted |

Recommended: **Pushes** and **Pull requests**

---

## Configuring Secret Token (Optional but Recommended)

### Step 1: Create Secret Token

Generate a random string:
```bash
# Generate random token
openssl rand -hex 20
# Output: a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6
```

### Step 2: Add to Jenkins

In Jenkins job configuration, enable webhook authentication:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Build Triggers                                                    │
│  ───────────────                                                   │
│                                                                     │
│  ✓ GitHub Hook Trigger (pollings)                                 │
│       ↓                                                             │
│  ────────────────────────────────────────────────────────────────  │
│  Or when using GitHub Branch Source:                               │
│  ✓ GitHub hook trigger for GITScm polling                         │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 3: Add Secret to GitHub Webhook

In GitHub webhook settings:
- Secret: `a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4e5f6`

### Step 4: Configure in Jenkins Credentials

1. Go to **Manage Jenkins** → **Credentials**
2. Add a "Secret text" credential with the webhook secret
3. In job configuration, select the credential

---

## Testing the Webhook

### Method 1: GitHub UI

In GitHub webhook settings:
1. Click **Edit** on your webhook
2. Scroll to "Recent Deliveries"
3. Click a delivery to see the request/response
4. Look for green checkmark (success) or red X (failure)

### Method 2: Trigger Test

In GitHub webhook settings:
1. Click **Test** → **Push events**
2. Should trigger a Jenkins build

### Method 3: Manual curl

```bash
# Test webhook manually
curl -X POST http://jenkins.example.com/github-webhook/ \
  -H "Content-Type: application/json" \
  -d '{"ref":"refs/heads/main","repository":{"full_name":"user/repo"}}'
```

---

## For Local Development: ngrok

If Jenkins is running locally, use ngrok to expose it to the internet:

### Step 1: Install ngrok

```bash
# Download from https://ngrok.com/download
# Or via brew
brew install ngrok
```

### Step 2: Start ngrok

```bash
ngrok http 8080
# Output: Forwarding https://abc123.ngrok.io -> http://localhost:8080
```

### Step 3: Use ngrok URL

In GitHub webhook:
```
Payload URL: https://abc123.ngrok.io/github-webhook/
```

**⚠️ Warning**: ngrok URLs change each time you restart ngrok. For development, consider using a tunnel service or deploying Jenkins to a public server.

---

## How Jenkins Processes Webhooks

### Without GitHub Branch Source

```groovy
// Add to your pipeline
pipeline {
    agent any
    
    triggers {
        // This enables webhook trigger
        githubPush()  // Requires "GitHub Hook Trigger"
    }
    
    stages {
        stage('Build') {
            steps {
                checkout scm  // Automatically gets latest code
                sh 'make build'
            }
        }
    }
}
```

### With GitHub Branch Source

No code needed! Just add the repository in Jenkins.

---

## Webhook Payload Example

GitHub sends this JSON payload:

```json
{
  "ref": "refs/heads/main",
  "before": "0000000000000000000000000000000000000000",
  "after": "abc123def456",
  "repository": {
    "id": 123456789,
    "name": "my-repo",
    "full_name": "username/my-repo",
    "html_url": "https://github.com/username/my-repo"
  },
  "pusher": {
    "name": "developer",
    "email": "dev@example.com"
  },
  "commits": [
    {
      "id": "abc123def456",
      "message": "Fix login bug",
      "timestamp": "2024-01-15T10:30:00Z",
      "author": {
        "name": "Developer",
        "email": "dev@example.com"
      }
    }
  ]
}
```

Jenkins parses this to:
- Know which branch was pushed
- Get the commit SHA
- Trigger appropriate builds

---

## Troubleshooting

### Webhook Not Triggering

1. **Check Jenkins URL** - Must be publicly accessible
2. **Check webhook delivery** - In GitHub, look at webhook deliveries
3. **Check Jenkins logs** - Manage Jenkins → System Log → All Jenkins Logs

### 404 Error

- Ensure webhook URL is correct
- Ensure Jenkins has GitHub Hook Trigger enabled

### 403 Error

- Check GitHub credentials in Jenkins
- Ensure webhook has correct permissions

---

## Common Mistakes

### Mistake 1: Using localhost

```groovy
// ❌ WON'T WORK - GitHub can't reach localhost
Payload URL: http://localhost:8080/github-webhook/

// ✅ Use public URL
Payload URL: http://jenkins.example.com/github-webhook/
```

### Mistake 2: Wrong Content Type

```
// ❌ Must be application/json
Content type: application/x-www-form-urlencoded

// ✅ Correct
Content type: application/json
```

### Mistake 3: Forgetting to Enable in Jenkins

```
// In job configuration, must enable:
// ✓ GitHub Hook Trigger (pollings)
```

---

## Security Best Practices

1. **Use HTTPS** - Always use HTTPS for webhook URLs
2. **Set secret token** - Prevents fake webhook calls
3. **Limit events** - Only enable events you need
4. **Monitor deliveries** - Check webhook logs regularly

---

## Next Steps

- **[Docker Pipeline Plugin](../02-docker-integration/01-docker-pipeline-plugin.md)** - Build Docker images
- **[JUnit Plugin](../03-testing-and-quality/01-junit-plugin.md)** - Publish test results
- **[Slack Notifications](../02-pipelines/03-post-and-notifications/03-slack-notifications.md)** - Send build notifications
