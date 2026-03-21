# Script Approval and Sandbox in Jenkins

## What this covers

This guide explains the Groovy Sandbox in Jenkins Pipelines, what triggers script approval requests, how to use the Script Approval page, and when `@NonCPS` is needed.

## Prerequisites

- Understanding of Pipeline syntax
- Jenkins admin access

## What is the Sandbox?

The **Groovy Sandbox** is a security feature that restricts which methods can be called in Pipeline scripts. It prevents dangerous operations by only allowing pre-approved methods.

```
┌─────────────────────────────────────────────────────────────────────┐
│  Pipeline Execution With Sandbox                                   │
│                                                                     │
│  ┌─────────────┐      ┌──────────────┐      ┌──────────────┐      │
│  │ Pipeline    │ ──►  │   Sandbox    │ ──►  │   Execute    │      │
│  │ Script      │      │   Whitelist  │      │   (or block) │      │
│  └─────────────┘      └──────────────┘      └──────────────┘      │
│                              │                                     │
│                              ▼                                     │
│                     Allowed Methods Only!                         │
└─────────────────────────────────────────────────────────────────────┘
```

---

## How the Sandbox Works

### Approved Methods

Most common methods are pre-approved:

- `echo`, `sh`, `bat` - Shell commands
- `checkout` - Source control
- `readFile`, `writeFile` - File operations
- `env.*` - Environment variables

### Methods Requiring Approval

When a script uses a non-whitelisted method, Jenkins prompts for approval:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Script Approval                                                    │
│                                                                     │
│  Pending Approvals:                                                │
│                                                                     │
│  Method: org.jenkinsci.plugins.scriptsecurity.sandbox.              │
│          groovy.sandbox.Interceptor.intercept                      │
│                                                                     │
│  [ Approve ] [ Deny ]                                              │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Common Methods Requiring Approval

### File Operations Beyond Workspace

```groovy
// These may require approval:
readFile '/etc/passwd'           // Reading outside workspace
writeFile '/tmp/file', 'data'    // Writing outside workspace
fileExists '/etc/shadow'         // Checking outside workspace
```

### System Operations

```groovy
// These require approval:
'rm -rf /'.execute()             // Running system commands
new File('/etc/passwd').text     // Direct file access
HttpURLConnection.new(...)       // Network calls
```

### Jenkins Internal APIs

```groovy
// These require approval:
Jenkins.instance.getItemById(...)
hudson.model.User.all()
```

---

## Script Approval Page

### Access

Go to **Manage Jenkins** → **In-process Script Approval**

```
┌─────────────────────────────────────────────────────────────────────┐
│  In-process Script Approval                                         │
│                                                                     │
│  Approved Scripts:                                                  │
│  ├─ method java.io.File getText            ← Approved            │
│  ├─ staticMethod org.apache.utils MyClass   ← Approved            │
│  └─ ...                                                            │
│                                                                     │
│  Pending Approvals:                                                │
│  ├─ method java.lang.System getProperty     ← Pending            │
│  └─ ...                                                            │
│                                                                     │
│  [ Approve] [ Approve (signature)] [ Deny]                        │
└─────────────────────────────────────────────────────────────────────┘
```

### Approving Methods

1. Review the pending method
2. Click **Approve** if safe
3. Or **Deny** to block

---

## Using @NonCPS

### What is @NonCPS?

`@NonCPS` marks a method to **skip** the sandbox. Use for:
- Operations that need full Groovy capabilities
- Performance-critical code
- Complex data processing

### Example

```groovy
pipeline {
    agent any
    stages {
        stage('Process') {
            steps {
                script {
                    // This method needs @NonCPS
                    def result = parseJson(input)
                }
            }
        }
    }
}

// This method processes JSON - needs full Groovy
@NonCPS
def parseJson(String jsonText) {
    return new groovy.json.JsonSlurper().parseText(jsonText)
}
```

### When to Use @NonCPS

| Use @NonCPS | Don't Use @NonCPS |
|-------------|-------------------|
| JSON parsing | Simple echo/sh |
| Complex loops | Checkout |
| System calls | Environment variables |

### Warning

`@NonCPS` methods:
- Run outside sandbox protection
- Can access anything on the system
- Should be used carefully

---

## Disabling Sandbox (Not Recommended!)

### Method 1: Pipeline Option

```groovy
pipeline {
    options {
        // ⚠️ DISABLES ALL SECURITY
        disableSandbox()
    }
    // ...
}
```

### Method 2: System Configuration

Go to **Manage Jenkins** → **Configure Global Security**:
- Uncheck "Enable Script Security"

**⚠️ WARNING**: Never disable sandbox in production!

---

## Security Best Practices

### 1. Review All Approvals

```
┌─────────────────────────────────────────────────────────────────────┐
│  Best Practice:                                                    │
│  - Review every approval request                                    │
│  - Only approve what you need                                       │
│  - Deny risky operations                                            │
└─────────────────────────────────────────────────────────────────────┘
```

### 2. Use Libraries for Approved Code

```groovy
// Instead of inline complex code
@Library('my-library') _

// Use pre-approved library functions
buildApp()  // Already approved in library
```

### 3. Keep Pipeline Simple

```groovy
// Simple, sandbox-safe code
stage('Build') {
    sh 'mvn clean package'
}

// Complex code that needs approval → Put in shared library
```

---

## Troubleshooting

### Approval Required Error

```
org.jenkinsci.plugins.scriptsecurity.sandbox.RejectedAccessException:
Scripts not permitted to use method java.lang.System getProperty
```

**Solution**: Go to Script Approval and approve the method

### Timeout Issues

```groovy
// CPS method hitting timeout
@NonCPS
def processLargeFile() { ... }  // Won't timeout
```

---

## Common Mistakes

### Approving Too Much

```groovy
// ❌ Don't approve everything
// "Allow all scripts" = security risk!

// ✅ Only approve what's needed
// Review each approval request
```

### Forgetting @NonCPS

```groovy
// ❌ May not work in sandbox
def result = new JsonSlurper().parseText(json)

// ✅ Add @NonCPS if needed
@NonCPS
def parseJson(text) {
    return new JsonSlurper().parseText(text)
}
```

---

## Summary

| Topic | Key Point |
|-------|-----------|
| Sandbox | Restricts dangerous operations |
| Approval | Admin reviews pending methods |
| @NonCPS | Skip sandbox for complex code |
| Disable | Never disable in production! |

---

## Next Steps

- **[Security Best Practices](https://www.jenkins.io/doc/book/security/)** - Official docs
- Complete your Jenkins journey with real-world projects!
