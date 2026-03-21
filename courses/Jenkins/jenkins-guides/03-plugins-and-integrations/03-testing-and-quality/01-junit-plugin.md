# JUnit Plugin in Jenkins

## What this covers

This guide explains how to publish JUnit test results using the `junit` step. You'll learn about XML report path globs, how to mark builds as UNSTABLE vs FAILURE based on test results, and how to use the test trend charts in Jenkins.

## Prerequisites

- JUnit Plugin installed (usually in suggested plugins)
- Tests that produce JUnit XML output
- Understanding of Pipeline basics

## Installing JUnit Plugin

1. Go to **Manage Jenkins** → **Plugin Manager**
2. Search for "JUnit"
3. Install **JUnit Plugin**

---

## Understanding JUnit XML Format

Jenkins needs test results in JUnit XML format. Your tests should produce XML like:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<testsuite name="com.example.AppTest" tests="5" failures="1" skipped="0" errors="0" time="0.123">
    <testcase name="testSum" classname="com.example.AppTest" time="0.01"/>
    <testcase name="testMultiply" classname="com.example.AppTest" time="0.005"/>
    <testcase name="testDivide" classname="com.example.AppTest" time="0.008">
        <failure message="Expected: 2.0, Actual: 2.5" type="AssertionError"/>
    </testcase>
    <testcase name="testSubtract" classname="com.example.AppTest" time="0.004"/>
    <testcase name="testPower" classname="com.example.AppTest" time="0.095"/>
</testsuite>
```

---

## Using junit Step in Pipeline

### Basic Usage

```groovy
pipeline {
    agent any
    
    stages {
        stage('Test') {
            steps {
                // Run tests
                sh 'npm test'  // Produces test-results/*.xml
                
                // Publish test results
                junit '**/test-results/*.xml'
            }
        }
    }
}
```

### With Full Options

```groovy
junit (
    // Test report XML file pattern (required)
    testDataPublishers: [
        // Publish test results
        publishJUnitReports: '**/test-results/*.xml'
    ],
    
    // Allow empty results (when no tests run)
    allowEmptyResults: true,
    
    // Test report location
    testResults: '**/test-results/*.xml',
    
    // Build health multiplier (0-1)
    healthScaleFactor: 1.0
)
```

---

## Setting Build Status Based on Tests

### Mark UNSTABLE Instead of FAILURE

```groovy
pipeline {
    agent any
    
    stages {
        stage('Test') {
            steps {
                sh 'npm test || true'  // Don't fail build
                
                // Publish and allow empty results
                junit testResults: '**/test-results/*.xml',
                      allowEmptyResults: true
            }
        }
    }
}
```

### Real Example with Maven

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                sh 'mvn clean package -DskipTests'
            }
        }
        
        stage('Test') {
            steps {
                // Run tests with JUnit output
                sh 'mvn test'
            }
            post {
                always {
                    // Publish test results
                    // Maven outputs to target/surefire-reports/*.xml
                    junit 'target/surefire-reports/*.xml'
                }
            }
        }
    }
}
```

---

## Complete Test Reporting Pipeline

```groovy
pipeline {
    agent any
    
    environment {
        // Test report paths vary by framework
        TEST_REPORT_PATTERN = '**/test-results/*.xml'
    }
    
    stages {
        stage('Install Dependencies') {
            steps {
                sh 'npm ci'
            }
        }
        
        stage('Unit Tests') {
            steps {
                sh 'npm run test:unit'
                
                // Publish unit test results
                junit testResults: '**/test-results/unit/*.xml',
                      allowEmptyResults: true,
                      healthScaleFactor: 1.0
            }
        }
        
        stage('Integration Tests') {
            steps {
                sh 'npm run test:integration'
                
                // Publish integration test results
                junit testResults: '**/test-results/integration/*.xml',
                      allowEmptyResults: true
            }
        }
    }
    
    post {
        always {
            // Archive test reports
            archiveArtifacts artifacts: '**/test-results/**/*',
                             fingerprint: true,
                             allowEmptyArchive: true
        }
    }
}
```

---

## Understanding Test Results in Jenkins

### Test Trend Chart

After publishing tests, Jenkins shows:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Test Result Trend                                                  │
│                                                                     │
│  10 ┤           ●                                                   │
│     │         ●   ●                                                 │
│   8 ┤       ●       ●                                               │
│     │     ●           ●                                           │
│   6 ┤   ●                                                       │
│     │ ●                                                             │
│     └────────────────────────────────────────────────────────────── │
│       Build 1  2   3   4   5   6   7   8   9  10                   │
│                                                                     │
│  ──────── Passed: 8    Failed: 0   Skipped: 2                      │
└─────────────────────────────────────────────────────────────────────┘
```

### Test Details Page

Click on test results to see:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Test Results                                                       │
│                                                                     │
│  Package: com.example.myapp                                         │
│  Tests: 25    Failures: 1    Skipped: 2    Duration: 5.2s          │
│                                                                     │
│  ─────────────────────────────────────────────────────────────────  │
│  ☐ testLogin        ✓ PASSED    0.5s                               │
│  ☐ testLogout       ✓ PASSED    0.3s                               │
│  ☐ testPayment      ✗ FAILED    1.2s                               │
│      └─ AssertionError: Expected 200, got 404                     │
│  ☐ testProfile      ✓ PASSED    0.8s                               │
│  ...                                                             │
└─────────────────────────────────────────────────────────────────────┘
```

---

## junit Step Options

| Option | Type | Description |
|--------|------|-------------|
| `testResults` | String | Path to XML files (required) |
| `allowEmptyResults` | boolean | Don't fail if no tests |
| `healthScaleFactor` | double | Affects build health (0-1) |
| `testDataPublishers` | List | Additional publishers |

---

## Handling Different Test Frameworks

### Java/Maven

```groovy
// Maven Surefire reports
junit 'target/surefire-reports/*.xml'

// Maven Failsafe reports (integration tests)
junit 'target/failsafe-reports/*.xml'
```

### Java/Gradle

```groovy
// Gradle test reports
junit 'build/test-results/test/*.xml'
```

### JavaScript/Jest

```groovy
// Jest JUnit output
junit 'test-results/junit.xml'
```

### Python/pytest

```groovy
// pytest JUnit XML
junit 'test-results/junit.xml'
```

### .NET/NUnit

```groovy
// NUnit XML results
junit 'TestResults/*.xml'
```

---

## Common Mistakes

### Mistake 1: Wrong Path

```groovy
// ❌ Path doesn't match actual location
junit '**/tests.xml'

// ✅ Use correct pattern
junit '**/test-results/*.xml'
```

### Missing Test Reports

```groovy
// ❌ No tests run = build fails by default
junit '**/test-results/*.xml'

// ✅ Allow empty results
junit testResults: '**/test-results/*.xml',
      allowEmptyResults: true
```

### Not Publishing After Each Stage

```groovy
// ❌ All in one stage - can't see which tests failed
stage('All Tests') {
    sh 'npm test'
    junit '**/*.xml'
}

// ✅ Separate stages for clarity
stage('Unit Tests') {
    sh 'npm run test:unit'
    junit '**/unit/*.xml'
}
```

---

## Next Steps

- **[SonarQube Integration](02-sonarqube-integration.md)** - Code quality analysis
- **[Code Coverage Reports](03-code-coverage-reports.md)** - Publish coverage
- **[Slack Notifications](02-pipelines/03-post-and-notifications/03-slack-notifications.md)** - Alert on failures
