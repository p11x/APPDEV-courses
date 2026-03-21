# Jenkins Credentials Plugin

## What this covers

This guide explains how to securely store and use secrets in Jenkins using the Credentials Plugin. You'll learn about different credential types (Username/Password, Secret Text, SSH Key, Certificate), how to configure credentials in the UI, and how to use them in pipelines.

## Prerequisites

- Understanding of environment variables
- Completed the Environment Variables guide
- Credentials Binding plugin installed

## What is the Credentials Plugin?

The Credentials Plugin provides a centralized way to store secrets in Jenkins:

- **API tokens** for accessing GitHub, AWS, etc.
- **SSH keys** for Git authentication
- **Passwords** for databases or services
- **Certificates** for TLS/SSL

> Think of the Credentials store as a **password manager** for Jenkins—secure, centralized, and accessible to pipelines.

---

## Accessing the Credentials Store

### Step 1: Navigate to Credentials

From Jenkins dashboard:
1. Click **Manage Jenkins**
2. Click **Credentials**

```
┌─────────────────────────────────────────────────────────────────────┐
│  Credentials                                                       │
│                                                                     │
│  System                                                            │
│  └─ Global credentials (unrestricted)                             │
│       └─ Domain: Global                                           │
│            ├── github-token (Secret text)                        │
│            ├── deploy-user (Username/password)                    │
│            └── ssh-key (SSH Username with private key)           │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: Add New Credentials

Click **Add Credentials**:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Add Credentials                                                   │
│                                                                     │
│  Kind:        [ Secret text ▼ ]                                    │
│       ↓                                                             │
│  Scope:       [ Global (Jenkins, nodes, items, child items) ▼ ]   │
│       ↓                                                             │
│  ID:          [ _______________ ]                                  │
│       ↓                                                             │
│  Description: [ _______________ ]                                  │
│       ↓                                                             │
│  ──────────────────────────────────────────────────────────────── │
│  (Fields change based on Kind selection)                          │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Credential Types

### 1. Secret Text

Stores a single secret value (API key, token, etc.):

```
┌─────────────────────────────────────────────────────────────────────┐
│  Kind: Secret text                                                │
│                                                                     │
│  Secret:    [ •••••••••••••••••••••• ]                            │
│       ↓                                                             │
│  ID:        [ github-api-token ]                                   │
│       ↓                                                             │
│  Description: [ GitHub Personal Access Token ]                   │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**Use for**:
- GitHub/GitLab API tokens
- AWS access keys
- NPM tokens
- Docker Hub tokens
- Any single secret value

### 2. Username and Password

Stores both username and password:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Kind: Username with password                                     │
│                                                                     │
│  Username: [ deploy-user ]                                         │
│  Password:  [ •••••••••••••••• ]                                  │
│       ↓                                                             │
│  ID:        [ deployment-credentials ]                            │
│       ↓                                                             │
│  Description: [ Deploy user for production ]                      │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**Use for**:
- Database credentials
- Deploy user accounts
- Service accounts

**Available as**:
- `${ID}` → `username:password`
- `${ID}_USR` → `username`
- `${ID_PSW}` → `password`

### 3. SSH Username with Private Key

Stores SSH key authentication:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Kind: SSH Username with private key                              │
│                                                                     │
│  Username: [ git ]                                                 │
│  Private Key:                                                       │
│    ○ Enter directly                                                │
│    ● From Jenkins master ~/.ssh                                    │
│    ○ From a file on Jenkins master                                 │
│       ↓                                                             │
│  Passphrase: [ ••••••••••••••• ] (if key has passphrase)         │
│       ↓                                                             │
│  ID:        [ github-ssh-key ]                                    │
│       ↓                                                             │
│  Description: [ SSH key for GitHub ]                              │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**Use for**:
- Git SSH authentication
- Remote server access
- SCP/SFTP transfers

### 4. Certificate

Stores PKCS#12 or JKS certificate files:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Kind: Certificate                                                │
│                                                                     │
│  Certificate: [ Upload certificate file ]                        │
│  Password:    [ •••••••••••••••• ]                                │
│       ↓                                                             │
│  ID:          [ client-cert ]                                     │
│       ↓                                                             │
│  Description: [ Client certificate ]                              │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Using Credentials in Pipeline

### Method 1: credentials() Helper (Environment Block)

```groovy
pipeline {
    agent any
    
    environment {
        // Secret text - available as $GITHUB_TOKEN
        GITHUB_TOKEN = credentials('github-api-token')
        
        // Username/Password - creates three variables
        DB_CREDS = credentials('database-credentials')
        
        // SSH key - available as $SSH_KEY
        SSH_KEY = credentials('github-ssh-key')
    }
    
    stages {
        stage('Use Secret Text') {
            steps {
                // Use the token (masked in logs)
                sh 'curl -H "Authorization: token $GITHUB_TOKEN" ...'
            }
        }
        
        stage('Use Username/Password') {
            steps {
                // Full credential: username:password
                echo "Connecting as: ${env.DB_CREDS}"
                
                // Individual parts
                echo "User: ${env.DB_CREDS_USR}"
                // Password: ${env.DB_CREDS_PSW} - don't echo!
            }
        }
        
        stage('Use SSH Key') {
            steps {
                // SSH key file path available
                sh 'git clone git@github.com:user/repo.git'
                // SSH uses key from ~/.ssh by ID
            }
        }
    }
}
```

### Method 2: withCredentials Step (Explicit Binding)

```groovy
pipeline {
    agent any
    
    stages {
        stage('Deploy with Credentials') {
            steps {
                // Explicitly bind credentials to variables
                withCredentials([
                    // Secret text binding
                    string(
                        credentialsId: 'aws-access-key',
                        variable: 'AWS_ACCESS_KEY'
                    ),
                    string(
                        credentialsId: 'aws-secret-key',
                        variable: 'AWS_SECRET_KEY'
                    ),
                    // Username/Password binding
                    usernamePassword(
                        credentialsId: 'docker-hub',
                        usernameVariable: 'DOCKER_USER',
                        passwordVariable: 'DOCKER_PASS'
                    ),
                    // SSH key binding
                    sshUserPrivateKey(
                        credentialsId: 'deploy-ssh',
                        usernameVariable: 'SSH_USER',
                        keyFileVariable: 'SSH_KEY'
                    )
                ]) {
                    // All variables available here
                    sh '''
                        aws configure set aws_access_key_id $AWS_ACCESS_KEY
                        aws configure set aws_secret_access_key $AWS_SECRET_KEY
                        docker login -u $DOCKER_USER -p $DOCKER_PASS
                    '''
                }
            }
        }
    }
}
```

---

## Credential Binding Types Reference

### usernamePassword

```groovy
usernamePassword(
    credentialsId: 'my-creds',
    usernameVariable: 'MY_USER',
    passwordVariable: 'MY_PASS'
)
```

Creates:
- `$MY_USER` → username
- `$MY_PASS` → password
- `${env.MY_CREDS}` → `username:password` (combined)

### string

```groovy
string(
    credentialsId: 'my-token',
    variable: 'MY_TOKEN'
)
```

Creates:
- `$MY_TOKEN` → the secret value

### sshUserPrivateKey

```groovy
sshUserPrivateKey(
    credentialsId: 'my-ssh',
    usernameVariable: 'SSH_USER',
    keyFileVariable: 'SSH_KEY_FILE'
)
```

Creates:
- `$SSH_USER` → username
- `$SSH_KEY_FILE` → path to private key file

### certificate

```groovy
certificate(
    credentialsId: 'my-cert',
    keystoreVariable: 'CERT_KEYSTORE',
    passwordVariable: 'CERT_PASSWORD'
)
```

Creates:
- `$CERT_KEYSTORE` → path to keystore file
- `$CERT_PASSWORD` → keystore password

---

## Best Practices

### 1. Use Descriptive IDs

```groovy
// ❌ BAD - cryptic IDs
credentials('abc123')

// ✅ GOOD - descriptive IDs
credentials('github-personal-access-token')
credentials('aws-production-access-key')
credentials('docker-hub-credentials')
```

### 2. Don't Hardcode Credentials

```groovy
// ❌ BAD - secrets in code
sh 'npm publish --token=abc123def456'

// ✅ GOOD - use credentials
withCredentials([string(credentialsId: 'npm-token', variable: 'NPM_TOKEN')]) {
    sh 'npm publish --token=$NPM_TOKEN'
}
```

### 3. Use Separate Credentials Per Environment

```groovy
// ❌ BAD - same credentials for all envs
credentials('deploy-creds')

// ✅ GOOD - separate credentials
credentials('deploy-staging')
credentials('deploy-production')
```

### 4. Rotate Credentials Regularly

1. Create new credential with new value
2. Update pipelines to use new credential ID
3. Delete old credential
4. Document the change

---

## Security Notes

### Credential Masking

Jenkins automatically masks known credential patterns in logs:

```
[Pipeline] withCredentials
 Masking passwords in output
[Pipeline] sh
+ echo ****
```

### Who Can Use Credentials

By default, anyone with:
- Overall/Administer
- Or Credentials/Use permission

Configure in **Manage Jenkins** → **Configure Global Security** → **Authorization**

---

## Common Mistakes

### Mistake 1: Echoing Passwords

```groovy
// ❌ WRONG - Don't print secrets!
steps {
    echo "Password: ${env.DB_PASS}"
}

// ✅ CORRECT - Use in commands, don't echo
steps {
    sh 'mysql -u $DB_USER -p$DB_PASS'
}
```

### Mistake 2: Wrong Scope

```groovy
// ⚠️ System credentials aren't available to pipelines
// Make sure to add to "Global" scope for pipeline access
```

### Mistake 3: Wrong Variable Name

```groovy
// ❌ WRONG - Wrong variable reference
withCredentials([string(credentialsId: 'my-token', variable: 'MY_TOKEN')]) {
    sh 'echo $MYTOKEN'  // Wrong! Missing underscore
}

// ✅ CORRECT
sh 'echo $MY_TOKEN'
```

---

## Next Steps

- **[Secret Masking](03-secret-masking.md)** - How Jenkins protects secrets in logs
- **[Post Block Conditions](../03-post-and-notifications/01-post-block-conditions.md)** - Run actions after build
- **[Slack Notifications](../03-post-and-notifications/03-slack-notifications.md)** - Send notifications with credentials
