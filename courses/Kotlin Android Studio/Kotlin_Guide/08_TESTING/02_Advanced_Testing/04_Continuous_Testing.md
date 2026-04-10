# Continuous Testing

## Learning Objectives

1. Understanding continuous testing fundamentals
2. Setting up CI/CD pipelines
3. Configuring test automation
4. Implementing test reporting
5. Managing test environments
6. Best practices for test automation

## Prerequisites

- Testing fundamentals
- Build systems (Gradle)
- CI/CD basics (Jenkins/GitHub Actions)

## Core Concepts

### Continuous Testing Overview

Continuous testing integrates automated tests throughout the development pipeline:
- **Build tests**: Run on every build
- **Commit tests**: Run on every commit
- **PR tests**: Run on pull requests
- **Deployment tests**: Run before deployment

### CI/CD Integration

Continuous Integration/Continuous Deployment pipelines:
- **Automated builds**: Automatic compilation
- **Automated tests**: Run tests automatically
- **Automated deployment**: Deploy after tests pass
- **Feedback**: Immediate test results

## Code Examples

### Standard Example: Gradle Test Configuration

```groovy
// build.gradle

android {
    testOptions {
        unitTests {
            includeAndroidResources = true
            returnDefaultValues = true
        }
    }
}

tasks.withType(Test) {
    testLogging {
        events "passed", "skipped", "failed"
        showStandardStreams = false
        showExceptions = true
        showCauses = true
    }
}

jacoco {
    toolVersion = "0.8.7"
}

tasks.withType(JacocoReport) {
    reports {
        xml.enabled = true
        html.enabled = true
    }
}
```

### Real-World Example: GitHub Actions Pipeline

```yaml
# .github/workflows/test.yml

name: Android Tests

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  unit-tests:
    name: Unit Tests
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Set up JDK
      uses: actions/setup-java@v2
      with:
        java-version: '11'
        distribution: 'temurin'
    
    - name: Cache Gradle
      uses: actions/cache@v2
      with:
        path: ~/.gradle/caches
        key: ${{ runner.os }}-gradle-${{ hashFiles('**/*.gradle*') }}
        restore-keys: |
          ${{ runner.os }}-gradle-
    
    - name: Run unit tests
      run: ./gradlew testDebugUnitTest
    
    - name: Upload test results
      uses: actions/upload-artifact@v2
      if: always()
      with:
        name: unit-test-results
        path: app/build/reports/tests/

  instrumentation-tests:
    name: Instrumentation Tests
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Set up JDK
      uses: actions/setup-java@v2
      with:
        java-version: '11'
    
    - name: Run instrumentation tests
      uses: reactivecircus/android-emulator-runner@v2
      with:
        api-level: 30
        profile: Nexus 6
        script: ./gradlew connectedDebugAndroidTest
    
    - name: Upload results
      uses: actions/upload-artifact@v2
      if: always()
      with:
        name: instrumentation-test-results
        path: app/build/reports/

  lint:
    name: Lint
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Run lint
      run: ./gradlew lint
    
    - name: Upload lint results
      uses: actions/upload-artifact@v2
      with:
        name: lint-results
        path: app/build/reports/
```

### Real-World Example: Jenkins Pipeline

```groovy
// Jenkinsfile

pipeline {
    agent any
    
    environment {
        ANDROID_SDK_ROOT = "${env.ANDROID_SDK_ROOT}"
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build') {
            steps {
                sh './gradlew assembleDebug'
            }
        }
        
        stage('Unit Tests') {
            steps {
                sh './gradlew testDebugUnitTest'
            }
            
            post {
                always {
                    junit 'app/build/test-results/**/*.xml'
                    publishHTML([
                        allowMissing: false,
                        alwaysLinkToLastBuild: true,
                        reportDir: 'app/build/reports/tests/',
                        reportFiles: 'index.html',
                        reportName: 'Unit Test Report'
                    ])
                }
            }
        }
        
        stage('Instrumentation Tests') {
            steps {
                sh './gradlew connectedDebugAndroidTest'
            }
            
            post {
                always {
                    junit 'app/build/outputs/android-test-results/**/*.xml'
                }
            }
        }
        
        stage('Code Coverage') {
            steps {
                sh './gradlew jacocoTestReport'
            }
            
            post {
                always {
                    publishHTML([
                        reportDir: 'app/build/reports/jacoco/',
                        reportFiles: 'index.html',
                        reportName: 'Coverage Report'
                    ])
                }
            }
        }
        
        stage('Static Analysis') {
            steps {
                sh './gradlew lintDebug'
                sh './gradlew detekt'
            }
        }
        
        stage('Deploy') {
            when {
                branch 'main'
            }
            steps {
                sh './gradlew publishDebug'
            }
        }
    }
    
    post {
        always {
            cleanWs()
        }
        failure {
            emailext subject: "Build Failed: ${env.JOB_NAME}",
                   body: "Check console output at ${env.BUILD_URL}",
                   to: 'team@example.com'
        }
        success {
            emailext subject: "Build Succeeded: ${env.JOB_NAME}",
                   body: "All tests passed. Check ${env.BUILD_URL}",
                   to: 'team@example.com'
        }
    }
}
```

### Real-World Example: Test Parallelization

```groovy
// Parallel execution configuration

android {
    testOptions {
        executionEnum = 'ANDROID_TEST_SHARDING'
        
        unitTests {
            sharding {
                enabled = true
                
                // Split tests across 4 shards
                shardCount = 4
            }
        }
    }
}

// Alternative: Manual parallelization
task unitTestParallel(type: Test) {
    use junitPlatform('shard-$shardIndex')
    
    maxParallelForks = 4
    
    testClassesDirs = fileTree('app/build/classes/')
    
    filter {
        includeTestsMatching "*Test"
    }
}
```

### Output Results

```
Continuous Test Pipeline Results:

Build: #123 - SUCCESS
├── Checkout: 5s
├── Build: 45s
├── Unit Tests: 120s (312 tests)
│   └── PASSED: 312
├── Instrumentation Tests: 240s (45 tests)
│   └── PASSED: 45
├── Lint: 30s
│   └── WARNINGS: 3
├── Code Coverage: 75%
│   ├── Unit: 85%
│   └── Integration: 65%
└── Total Time: ~7 minutes

Deployed to staging: YES
```

## Best Practices

1. **Run tests on every commit**: Don't skip tests
2. **Keep CI fast**: Use parallel execution
3. **Fail fast**: Run unit tests first
4. **Use test containers**: Isolate tests
5. **Stage-based testing**: Progressive pipelines
6. **Automated retry**: Handle flaky tests
7. **Report clearly**: Actionable feedback
8. **Monitor trends**: Track over time

## Common Pitfalls

**Pitfall 1: Slow CI builds**
- **Problem**: Tests take too long
- **Solution**: Use parallelization, skip unnecessary tests

**Pitfall 2: Flaky tests**
- **Problem**: Tests fail randomly
- **Solution**: Fix root causes, not retry

**Pitfall 3: No test caching**
- **Problem**: Repeated test runs
- **Solution**: Use Gradle build cache

**Pitfall 4: Missing coverage**
- **Problem**: Low coverage
- **Solution**: Increase test coverage

**Pitfall 5: No test maintenance**
- **Problem**: Test debt accumulates
- **Solution**: Regular cleanup

## Troubleshooting Guide

**Issue: "Tests timeout"**
1. Increase timeout values
2. Improve test isolation
3. Use test doubles

**Issue: "Memory errors"**
1. Increase Gradle daemon memory
2. Limit parallel test runs
3. Use sharding

**Issue: "Emulator issues"**
1. Use better emulator configuration
2. Consider device farms
3. Use Robolectric

## Advanced Tips

**Tip 1: Flaky test detection**
```kotlin
// Mark flaky tests
@Flaky(rerunCount = 3)
fun testFlaky() { }
```

**Tip 2: Test distribution**
```groovy
// Use Firebase Test Lab
android { testBuildType("cloud") }
```

**Tip 3: Test analysis**
```groovy
tasks.withType<JacocoTestReport> {
    reports {
        xml.required.set(true)
        html.required.set(true)
    }
}
```

**Tip 4: Performance thresholds**
```groovy
testOptions {
    unitTests.all {
        extensions.configure<JacocoTaskExtension> {
            setMinimumInstructionCoverage(70)
        }
    }
}
```

## Cross-References

See: 08_TESTING/01_Testing_Fundamentals/01_Unit_Testing_Basics.md
See: 08_TESTING/01_Testing_Fundamentals/04_Test_Utilities.md
See: 08_TESTING/01_Testing_Fundamentals/05_Test_Architecture.md
See: 08_TESTING/02_Advanced_Testing/02_Performance_Testing.md
See: 08_TESTING/02_Advanced_Testing/05_Test_Reporting.md
See: 01_SETUP_ENVIRONMENT/01_IDE_Installation_and_Configuration/04_Gradle_Configuration.md