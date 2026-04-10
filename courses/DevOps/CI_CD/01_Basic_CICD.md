---
Category: DevOps
Subcategory: CI/CD
Concept: CI/CD Basics
Purpose: Understanding continuous integration and continuous deployment pipelines
Difficulty: beginner
Prerequisites: Git Basics
RelatedFiles: 02_Advanced_CICD.md
UseCase: Automated software delivery
CertificationExam: AWS DevOps Engineer
LastUpdated: 2025
---

## WHY

CI/CD pipelines enable automated, reliable software delivery to the cloud.

## WHAT

### CI/CD Components

**Source Control**: Code versioning

**Build**: Compile and package

**Test**: Automated testing

**Deploy**: Release to environment

## HOW

### Example: GitHub Actions

```yaml
name: CI/CD Pipeline
on:
  push:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup Node
      uses: actions/setup-node@v3
      with:
        node-version: '18'
    
    - name: Install dependencies
      run: npm install
    
    - name: Run tests
      run: npm test
    
    - name: Deploy to AWS
      run: npm run deploy
```

### Example: CodePipeline

```bash
# Create pipeline
aws codepipeline create-pipeline \
    --pipeline name=my-pipeline \
    --role-arn role-arn \
    --artifact-store s3 \
    --stages '[{
        "name": "Source",
        "actions": [{
            "name": "SourceAction",
            "actionTypeId": {
                "category": "Source",
                "owner": "AWS",
                "provider": "GitHub",
                "version": 1
            }
        }]
    }]'
```

## CROSS-REFERENCES

### Related Tools

- Jenkins: Open-source CI/CD
- GitLab CI: GitLab CI/CD
- CircleCI: Managed CI/CD