# CI/CD Pipeline Setup

## Concept Title and Overview

In this lesson, you'll learn how to set up Continuous Integration and Continuous Deployment pipelines for automated software delivery.

## Real-World Importance and Context

CI/CD pipelines automate the build, test, and deployment process, enabling:
- Faster releases
- Fewer bugs in production
- Consistent deployments
- Team productivity

## Detailed Step-by-Step Explanation

### CI/CD Workflow

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    CI/CD PIPELINE                                        │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌──────────┐    ┌──────────┐    ┌──────────┐    ┌──────────┐        │
│  │   CODE   │───►│  BUILD   │───►│  TEST   │───►│ DEPLOY  │        │
│  │  COMMIT  │    │  COMPILE │    │ UNIT/E2E │    │ STAGING │        │
│  └──────────┘    └──────────┘    └──────────┘    └──────────┘        │
│                                                              │        │
│                                                              ▼        │
│                                                         ┌──────────┐ │
│                                                         │ PRODUCTION│ │
│                                                         └──────────┘ │
│                                                                         │
│  Tools: GitHub Actions, Jenkins, GitLab CI, CircleCI                  │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

### GitHub Actions Workflow

```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Set up JDK 21
      uses: actions/setup-java@v3
      with:
        java-version: '21'
        distribution: 'temurin'
        
    - name: Build with Maven
      run: mvn clean package -DskipTests
      
    - name: Run Tests
      run: mvn test
      
    - name: Build Docker Image
      run: docker build -t myapp:${{ github.sha }} .
      
    - name: Deploy to Staging
      if: github.ref == 'refs/heads/main'
      run: |
        echo "Deploying to staging..."
```

---

## Summary

You've learned CI/CD pipeline fundamentals and GitHub Actions setup.
