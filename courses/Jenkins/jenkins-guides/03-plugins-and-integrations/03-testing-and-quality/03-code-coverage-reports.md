# Code Coverage Reports in Jenkins

## What this covers

This guide explains how to publish code coverage reports using the Coverage Plugin (the modern replacement for deprecated Cobertura). You'll learn about the `recordCoverage` step with tools, sourceCodeRetention, and qualityGates parameters, and the difference between line, branch, and method coverage.

## Prerequisites

- Coverage Plugin installed
- Tests that generate coverage data
- Understanding of Pipeline basics

## Installing Coverage Plugin

1. Go to **Manage Jenkins** → **Plugin Manager**
2. Search for "Coverage"
3. Install **Coverage Plugin**

---

## Understanding Coverage Types

### Line Coverage

Percentage of code lines executed during tests.

```java
// 3 of 4 lines covered = 75% line coverage
int add(int a, int b) {
    return a + b;         // ✓ covered
}

int divide(int a, int b) {
    if (b == 0) {         // ✓ covered
        return 0;          // ✓ covered
    }
    return a / b;         // ✗ NOT covered
}
```

### Branch Coverage

Percentage of code branches (if/else, switch) executed.

```java
// 2 of 4 branches covered = 50% branch coverage
if (a > 0 || b < 0) {     // || has 4 branches:
                          // true || true
                          // true || false  ← covered
    doSomething();         // false || true ← covered
                          // false || false
}
```

### Method Coverage

Percentage of methods/functions called.

```java
class Calculator {
    int add() {}           // ✓ covered
    int subtract() {}      // ✗ NOT covered  
    int multiply() {}      // ✓ covered
}
// 2 of 3 methods = 67% method coverage
```

---

## Using recordCoverage Step

### Basic Java with JaCoCo

```groovy
pipeline {
    agent any
    
    stages {
        stage('Build & Test') {
            steps {
                sh 'mvn clean test'
            }
            post {
                always {
                    // Publish JaCoCo coverage
                    recordCoverage(
                        // Tool configuration
                        tools: [
                            jacoco(
                                pattern: '**/target/site/jacoco/jacoco.xml',
                                // Or for XML:
                                // pattern: '**/jacoco.xml'
                            )
                        ],
                        // Source code retention
                        sourceCodeRetention: 'AFTER_BUILD'
                    )
                }
            }
        }
    }
}
```

### With Full Options

```groovy
recordCoverage(
    // Coverage tools to use
    tools: [
        jacoco(pattern: '**/target/site/jacoco/jacoco.xml'),
        // Or for Cobertura (legacy):
        // cobertura(pattern: '**/coverage.xml')
    ],
    
    // Source code handling
    // 'EVERY_BUILD' - Save with each build (default)
    // 'LAST_BUILD' - Only keep most recent
    // 'AFTER_BUILD' - Keep during build, delete after
    // 'NEVER' - Don't store source
    sourceCodeRetention: 'EVERY_BUILD',
    
    // Quality gates - fail build if coverage too low
    qualityGates: [
        [threshold: 80, type: 'LINE', unstable: false],
        [threshold: 70, type: 'BRANCH', unstable: false],
        [threshold: 90, type: 'METHOD', unstable: true]
    ]
)
```

---

## Complete Coverage Pipeline

```groovy
pipeline {
    agent any
    
    environment {
        // Coverage thresholds
        MIN_LINE_COVERAGE = 75
        MIN_BRANCH_COVERAGE = 60
    }
    
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Install Dependencies') {
            steps {
                sh 'npm ci'
            }
        }
        
        stage('Build & Test with Coverage') {
            steps {
                // Run tests with coverage
                sh 'npm run test:coverage'
            }
            post {
                always {
                    // Publish coverage reports
                    recordCoverage(
                        tools: [
                            jacoco(
                                pattern: '**/target/site/jacoco/jacoco.xml'
                            )
                        ],
                        sourceCodeRetention: 'EVERY_BUILD'
                    )
                }
            }
        }
        
        stage('Quality Gate') {
            steps {
                // Fail build if coverage too low
                recordCoverage(
                    tools: [
                        jacoco(pattern: '**/jacoco.xml')
                    ],
                    qualityGates: [
                        [threshold: env.MIN_LINE_COVERAGE as Integer, type: 'LINE'],
                        [threshold: env.MIN_BRANCH_COVERAGE as Integer, type: 'BRANCH']
                    ]
                )
            }
        }
    }
}
```

---

## JavaScript Coverage with Istanbul/Jest

```groovy
pipeline {
    agent any
    
    stages {
        stage('Install') {
            steps {
                sh 'npm ci'
            }
        }
        
        stage('Test with Coverage') {
            steps {
                sh 'npm run test:coverage -- --coverageReporters=cobertura'
            }
            post {
                always {
                    recordCoverage(
                        tools: [
                            jacoco(
                                pattern: '**/coverage/cobertura-coverage.xml'
                            )
                        ]
                    )
                }
            }
        }
    }
}
```

---

## Coverage Quality Gates

| Type | Description | Common Threshold |
|------|-------------|------------------|
| `LINE` | Line coverage | 70-80% |
| `BRANCH` | Branch coverage | 60-70% |
| `METHOD` | Method coverage | 80-90% |

### Quality Gate Options

```groovy
qualityGates: [
    // Fail build if line coverage < 80%
    [threshold: 80, type: 'LINE', unstable: false],
    
    // Mark unstable (yellow) if branch < 60%
    [threshold: 60, type: 'BRANCH', unstable: true],
    
    // Warning only if method < 70%
    [threshold: 70, type: 'METHOD', unstable: true]
]
```

---

## Coverage Trends in Jenkins

After publishing coverage, Jenkins shows:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Coverage Trend                                                     │
│                                                                     │
│  Line Coverage:  75% ████████████████░░░░░░░                       │
│  Branch Coverage: 60% █████████████░░░░░░░░░░░                       │
│  Method Coverage: 85% █████████████████░░░░░░                         │
│                                                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │ Coverage Chart                                              │  │
│  │    ●                                                       │  │
│  │   ╱ ╲   ●                                                  │  │
│  │  ╱   ╲       ●                                            │  │
│  │ ╱     ╲                                                   │  │
│  │└───────┴─────────────────────────────────────────────────  │  │
│  │  Build 1  2   3   4   5   6   7   8                     │  │
│  └──────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Common Coverage Formats

| Tool | Format | Pattern |
|------|--------|---------|
| JaCoCo | XML | `**/jacoco.xml` |
| JaCoCo | XML | `**/target/site/jacoco/jacoco.xml` |
| Cobertura | XML | `**/coverage.xml` |
| Istanbul | JSON | `**/coverage/coverage-summary.json` |
| Istanbul | LCOV | `**/coverage/lcov.info` |

---

## Common Mistakes

### Wrong Pattern

```groovy
// ❌ Pattern doesn't match actual file location
recordCoverage tools: [jacoco(pattern: '**/coverage.xml')]

// ✅ Use correct pattern
recordCoverage tools: [jacoco(pattern: '**/target/site/jacoco/jacoco.xml')]
```

### Not Publishing in Post

```groovy
// ❌ Coverage might not run on failure
stage('Test') {
    steps {
        sh 'npm test'
        recordCoverage ...  // If test fails, this might not run
    }
}

// ✅ Use post always to ensure it runs
stage('Test') {
    steps {
        sh 'npm test'
    }
    post {
        always {
            recordCoverage ...
        }
    }
}
```

### Wrong Tool Name

```groovy
// ❌ Case-sensitive!
tools: [JaCoCo(pattern: '**/jacoco.xml')]

// ✅ Correct
tools: [jacoco(pattern: '**/jacoco.xml')]
```

---

## Next Steps

- **[Parallel Stages](04-advanced-topics/02-parallel-and-matrix/01-parallel-stages.md)** - Run tests in parallel
- **[Shared Libraries](04-advanced-topics/01-shared-libraries/01-what-are-shared-libraries.md)** - Reusable coverage steps
- **[Kubernetes Agents](04-advanced-topics/03-kubernetes-agents/01-kubernetes-plugin-setup.md)** - Scale with Kubernetes
