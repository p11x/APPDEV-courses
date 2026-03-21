# Secret Masking in Jenkins

## What this covers

This guide explains how Jenkins automatically masks secrets in build logs, what happens when you accidentally print credentials, and best practices for keeping your secrets safe.

## Prerequisites

- Understanding of credentials
- Completed the Credentials Plugin guide

## How Secret Masking Works

Jenkins automatically masks sensitive values in console output to prevent secrets from appearing in logs.

### What Gets Masked?

1. **Credentials defined in Jenkins** - automatically masked
2. **Environment variables with common secret names** - e.g., `PASSWORD`, `SECRET`, `TOKEN`, `API_KEY`
3. **URLs containing credentials** - e.g., `https://user:pass@github.com`

### How It Looks in Practice

```groovy
pipeline {
    agent any
    
    environment {
        // This will be masked
        MY_SECRET = credentials('my-api-token')
    }
    
    stages {
        stage('Build') {
            steps {
                // This command contains the secret
                sh 'curl -H "Authorization: Bearer $MY_SECRET" https://api.example.com'
            }
        }
    }
}
```

**Console Output:**
```
[Pipeline] stage
[Pipeline] { (Build)
[Pipeline] sh
+ curl -H 'Authorization: Bearer ****' https://api.example.com
[Pipeline] }
[Pipeline] // stage
```

Notice how `$MY_SECRET` was replaced with `****`!

---

## What Happens When You Accidentally Print a Secret?

### Example 1: Echoing a Secret Variable

```groovy
steps {
    // ❌ DANGER - This will print the secret!
    echo "Token: ${env.MY_SECRET}"
}
```

**Console Output:**
```
[Pipeline] echo
Token: ****
[Pipeline] echo
```

Jenkins detects this and masks it! But this isn't foolproof.

### Example 2: Secret in a URL

```groovy
steps {
    // This URL contains credentials
    sh 'git clone https://user:password123@github.com/user/repo.git'
}
```

**Console Output:**
```
[Pipeline] sh
+ git clone https://user:****@github.com/user/repo.git
[Pipeline] sh
```

The password is masked.

---

## Automatic Masking Patterns

Jenkins masks variables with these names (case-insensitive):

| Pattern | Example Variables |
|---------|-------------------|
| `*PASSWORD*` | `MY_PASSWORD`, `DB_PASSWORD` |
| `*SECRET*` | `API_SECRET`, `CLIENT_SECRET` |
| `*TOKEN*` | `GITHUB_TOKEN`, `AWS_TOKEN` |
| `*API_KEY*` | `AWS_API_KEY` |
| `*PRIVATE*` | `PRIVATE_KEY` |
| `*CREDENTIAL*` | `CREDENTIALS` |
| `*AUTH*` | `AUTH_TOKEN` |

---

## Credentials Binding Plugin Masking

The Credentials Binding Plugin provides explicit masking:

```groovy
pipeline {
    agent any
    
    stages {
        stage('Deploy') {
            steps {
                withCredentials([
                    string(credentialsId: 'my-token', variable: 'MY_TOKEN')
                ]) {
                    // Inside this block, $MY_TOKEN is masked
                    sh 'some-tool --token=$MY_TOKEN'
                    
                    // Even if you accidentally print it:
                    echo "Using token: $MY_TOKEN"  // Shows ****
                }
            }
        }
    }
}
```

---

## Best Practices for Secret Safety

### 1. Never Print Secrets

```groovy
// ❌ WRONG - Don't echo secrets
steps {
    echo "Password: ${env.MY_PASSWORD}"
}

// ✅ CORRECT - Use in commands only
steps {
    sh 'mysql -u user -p$MY_PASSWORD'
}
```

### 2. Use credentials() in environment Block

```groovy
// ✅ GOOD - Automatic masking
environment {
    MY_TOKEN = credentials('my-token')
}
```

### 3. Use withCredentials for Explicit Control

```groovy
// ✅ GOOD - Explicit, clear masking
withCredentials([string(credentialsId: 'token', variable: 'TOKEN')]) {
    sh 'command --token=$TOKEN'
}
```

### 4. Use URL-Encoded Credentials

```groovy
// When using credentials in URLs, use environment variable
environment {
    GIT_CREDENTIALS = credentials('git-creds')
}

steps {
    // Jenkins masks the URL automatically
    sh 'git clone https://$GIT_CREDENTIALS@github.com/user/repo.git'
}
```

### 5. Check Log Patterns

If you see secrets in logs:

1. **Rotate the secret immediately** - assume it's compromised
2. **Add the variable name to mask list** if not already
3. **Never use the secret again**

---

## Custom Secret Masking

### Adding Custom Patterns

Install the "Mask Passwords" plugin for custom patterns:

```groovy
pipeline {
    agent any
    
    options {
        // Add custom mask pattern
        maskPasswords()
    }
    
    environment {
        // Custom variable (might not be auto-masked)
        CUSTOM_SECRET = credentials('custom-secret')
    }
    
    stages { ... }
}
```

### Using maskPasswords Step

```groovy
steps {
    maskPasswords(
        varPasswordPairs: [
            [var: 'MY_CUSTOM_VAR', password: 'secret-value']
        ]
    )
}
```

---

## Sandbox Security

Jenkins Pipeline runs in a **Groovy Sandbox** for security:

### What is the Sandbox?

The sandbox restricts which methods can be called. Dangerous operations require approval.

```groovy
// In sandbox, this might require approval:
// Example: Reading files
def content = readFile('secrets.txt')  // May trigger approval
```

### Disabling Sandbox (Not Recommended!)

```groovy
// ⚠️ WARNING - Disabling sandbox is dangerous
pipeline {
    agent any
    
    options {
        // Disable sandbox - allows ALL Groovy operations
        // NOT RECOMMENDED FOR PRODUCTION
        disableSandbox()
    }
}
```

### Script Approval

When a method isn't whitelisted, Jenkins prompts an administrator:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Script Approval                                                    │
│                                                                     │
│  Approve these scripts:                                             │
│                                                                     │
│  Method: org.jenkinsci.plugins.scriptsecurity.sandbox.             │
│          groovy.sandbox.Interceptor.intercept                       │
│                                                                     │
│  [ Approve ] [ Deny ]                                               │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Troubleshooting Secret Leaks

### If You See a Leaked Secret

1. **Assume the secret is compromised**
2. **Immediately rotate the secret** - change the password/token
3. **Revoke the old credential** in Jenkins
4. **Check git history** - if committed, rotate immediately
5. **Audit access logs** - who might have seen it?

### Checking for Leaks

1. Go to **Manage Jenkins** → **Script Console**
2. Run this to search build logs:

```groovy
// Search for secrets in build logs
def secretPatterns = [
    'password=', 
    'token=', 
    'secret=', 
    'api_key='
]

Jenkins.instance.allItems(Job).each { job ->
    job.builds.each { build ->
        if (build.isComplete()) {
            def log = build.log
            secretPatterns.each { pattern ->
                if (log.contains(pattern)) {
                    println "Found '${pattern}' in ${job.name} #${build.number}"
                }
            }
        }
    }
}
```

---

## Common Mistakes

### Mistake 1: Logging Too Much

```groovy
// ❌ WRONG - Print everything for debugging
steps {
    sh 'set'  // Prints all environment variables!
    sh 'env'  // Prints all environment variables!
}

// ✅ CORRECT - Be careful what you print
steps {
    sh 'printenv | grep -v SECRET'  // Filter secrets
}
```

### Mistake 2: Using Wrong Variable Name

```groovy
// ❌ WRONG - Variable isn't set
withCredentials([string(credentialsId: 'token', variable: 'MY_TOKEN')]) {
    echo "$MYTOKEN"  // Wrong! Variable name is MY_TOKEN
}

// ✅ CORRECT
echo "$MY_TOKEN"
```

### Mistake 3: Not Using Credentials at All

```groovy
// ❌ WRONG - Hardcoded secrets in Jenkinsfile
steps {
    sh 'npm publish --token=abc123def456'
}

// ✅ CORRECT - Use credentials
withCredentials([string(credentialsId: 'npm-token', variable: 'NPM_TOKEN')]) {
    sh 'npm publish --token=$NPM_TOKEN'
}
```

---

## Next Steps

- **[Post Block Conditions](../03-post-and-notifications/01-post-block-conditions.md)** - Run notifications after build
- **[Email Notifications](../03-post-and-notifications/02-email-notifications.md)** - Send emails securely
- **[Slack Notifications](../03-post-and-notifications/03-slack-notifications.md)** - Send Slack messages
