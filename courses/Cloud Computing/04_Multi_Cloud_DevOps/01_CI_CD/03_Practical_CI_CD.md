---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud DevOps
Concept: CI/CD
Difficulty: advanced
Prerequisites: Basic Cloud Computing, Basic CI/CD Concepts, Advanced CI/CD
RelatedFiles: 01_Basic_CI_CD.md, 02_Advanced_CI_CD.md
UseCase: Implementing production CI/CD solutions for multi-cloud environments
CertificationExam: AWS Solutions Architect Professional, Azure Architect Expert
LastUpdated: 2025
---

## WHY

Practical CI/CD implementation requires production-ready configurations, automation, and operational procedures for multi-cloud deployments.

### Implementation Value

- Production-ready pipelines
- Automation and monitoring
- Compliance procedures
- Cost optimization

### Success Metrics

| Metric | Target | Measurement |
|--------|--------|-------------|
| Deployment Time | < 10 min | Pipeline metrics |
| Success Rate | > 99% | Pipeline metrics |
| MTTR | < 15 min | Deployment logs |
| Rollback Time | < 5 min | Incident response |

## WHAT

### Production CI/CD Patterns

**Pattern 1: Centralized Pipeline**
- Single pipeline for all clouds
- Cloud-specific stages
- Unified reporting

**Pattern 2: GitOps Pipeline**
- Git as source of truth
- Automated sync
- Rollback to Git

**Pattern 3: Progressive Delivery**
- Canary deployments
- Feature flags
- Observability gates

### Implementation Architecture

```
PRODUCTION CI/CD
=================

┌─────────────────────────────────────────────────────────────┐
│                    SOURCE CONTROL                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │    GitHub   │  │   GitLab    │  │   Bitbucket │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────┼───────────────────────────────────┐
│                    BUILD LAYER                              │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │   Build     │  │   Test       │  │   Security  │       │
│  │             │  │             │  │   Scan      │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────┼───────────────────────────────────┐
│                    DEPLOY LAYER                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │ Deploy AWS  │  │ Deploy Azure │  │ Deploy GCP   │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────┼───────────────────────────────────┐
│                    VALIDATION LAYER                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │  Smoke Test │  │  Monitor     │  │   Alert      │       │
│  └──────────────┘  └──────────────┘  └──────────────┘       │
└─────────────────────────────────────────────────────────────┘
```

## HOW

### Example 1: Terraform Multi-Cloud CI/CD

```hcl
# Production CI/CD with Terraform
terraform {
  required_version = ">= 1.0"
}

# AWS CodePipeline
resource "aws_codepipeline" "main" {
  name     = "multi-cloud-pipeline"
  role_arn = aws_iam_role.pipeline.arn
  
  artifact_store {
    type     = "S3"
    location = aws_s3_bucket.artifact_store.bucket
  }
  
  stage {
    name = "Source"
    action {
      name             = "Source"
      category         = "Source"
      owner            = "ThirdParty"
      provider         = "GitHub"
      version          = "1"
      output_artifacts = ["source"]
      
      configuration = {
        Owner                = var.github_owner
        Repo                 = var.github_repo
        Branch               = "main"
        OAuthToken           = var.github_token
      }
    }
  }
  
  stage {
    name = "Build"
    action {
      name             = "Build"
      category         = "Build"
      owner            = "AWS"
      provider         = "CodeBuild"
      input_artifacts  = ["source"]
      output_artifacts = ["build"]
      
      configuration = {
        ProjectName = aws_codebuild_project.main.name
      }
    }
  }
  
  stage {
    name = "Deploy"
    action {
      name             = "Deploy"
      category         = "Deploy"
      owner            = "AWS"
      provider         = "CloudFormation"
      input_artifacts  = ["build"]
      
      configuration = {
        StackName = "MultiCloudStack"
        TemplatePath = "build::template.yaml"
      }
    }
  }
}

# CodeBuild Project
resource "aws_codebuild_project" "main" {
  name         = "multi-cloud-build"
  service_role = aws_iam_role.build.arn
  
  artifacts {
    type = "no_artifacts"
  }
  
  environment {
    type          = "LINUX_CONTAINER"
    compute_type = "BUILD_GENERAL1_SMALL"
    image        = "aws/codebuild/standard:5.0"
    
    environment_variable {
      name  = "AWS_REGION"
      value = "us-east-1"
    }
  }
  
  source {
    type     = "GITHUB"
    location = "https://github.com/${var.github_owner}/${var.github_repo}.git"
  }
}

# S3 Bucket for artifacts
resource "aws_s3_bucket" "artifact_store" {
  bucket = "multi-cloud-artifacts"
}
```

### Example 2: Azure DevOps Production Pipeline

```yaml
# Production Azure Pipeline with all features
name: Production Multi-Cloud Pipeline

trigger:
  branches:
    include:
    - main
    - release/*

variables:
  version: '1.0.0'
  isMain: $[eq(variables['Build.SourceBranch'], 'refs/heads/main')]

stages:
- stage: CI
  jobs:
  - job: Build
    pool:
      vmImage: 'ubuntu-latest'
    steps:
    - task: NodeTool@0
      inputs:
        versionSpec: '18.x'
    
    - script: |
        npm ci
        npm run lint
        npm run test
        npm run build
      displayName: 'Build and Test'
    
    - task: PublishTestResults@2
      inputs:
        testResultsFormat: 'JUnit'
        testResultsFiles: '**/test-results.xml'
    
    - task: PublishCodeCoverageResults@1
      inputs:
        codeCoverageTool: 'Cobertura'
        summaryFilesLocation: '$(Agent.TempDirectory)/**/coverage.xml'

- stage: Security
  jobs:
  - job: SecurityScan
    pool:
      vmImage: 'ubuntu-latest'
    steps:
    - task: Semgrep@1
      inputs:
        scanType: 'scan'
        repoPath: '$(System.DefaultWorkingDirectory)'
    
    - task: ContainerScan@0
      inputs:
        containerRegistry: 'AzureContainerRegistry'
        imageName: 'app'
        imageTag: '$(version)'

- stage: Staging
  dependsOn: ['CI', 'Security']
  jobs:
  - deployment: DeployStaging
    environment: 'staging'
    pool:
      vmImage: 'ubuntu-latest'
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureCLI@2
            inputs:
              azureSubscription: 'Azure-Staging'
              scriptType: 'bash'
              inlineScript: |
                az aks get-credentials --name staging-cluster
                kubectl apply -f manifests/staging/
          - script: |
              # Smoke tests
              for i in {1..10}; do
                curl -f https://staging.example.com/health && break
                sleep 5
              done

- stage: Production
  dependsOn: ['Staging']
  condition: and(succeeded(), eq(variables['isMain'], true))
  jobs:
  - deployment: DeployProduction
    environment: 'production'
    pool:
      vmImage: 'ubuntu-latest'
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureCLI@2
            inputs:
              azureSubscription: 'Azure-Production'
              scriptType: 'bash'
              inlineScript: |
                az aks get-credentials --name production-cluster
                kubectl apply -f manifests/production/
          
          - script: |
              # Health check
              curl -f https://production.example.com/health
              curl -f https://production.example.com/api/health
          
          - task: InvokeAzureFunction@1
            inputs:
              functionApp: 'alerts-function'
              method: 'POST'
              body: '{"status": "deployed", "version": "$(version)"}'
```

### Example 3: Multi-Cloud Deployment Script

```python
# Multi-cloud deployment automation
import boto3
import subprocess
from dataclasses import dataclass
from typing import List, Dict

@dataclass
class DeploymentConfig:
    version: str
    environment: str
    regions: List[str]
    timeout: int = 300

class MultiCloudDeployer:
    def __init__(self, config: DeploymentConfig):
        self.config = config
        self.clients = {
            'aws': boto3.client('ecs'),
            'azure': None,
            'gcp': None
        }
        
    def deploy_aws(self, region: str):
        """Deploy to AWS"""
        print(f"Deploying to AWS region {region}")
        
        # Update ECS service
        response = self.clients['aws'].update_service(
            cluster='production',
            service='app-service',
            forceNewDeployment=True,
            region=region
        )
        
        # Wait for deployment
        waiter = self.clients['aws'].get_waiter('services_stable')
        waiter.wait(
            cluster='production',
            services=['app-service']
        )
        
        return response
        
    def deploy_azure(self, resource_group: str):
        """Deploy to Azure"""
        print(f"Deploying to Azure resource group {resource_group}")
        
        # Use Azure CLI
        result = subprocess.run([
            'az', 'aks', 'get-credentials',
            '--name', 'production-cluster',
            '--resource-group', resource_group
        ], capture_output=True)
        
        result = subprocess.run([
            'kubectl', 'set', 'image', 'deployment/app',
            'app=myregistry.azurecr.io/app:{}'.format(self.config.version)
        ], capture_output=True)
        
        return result.returncode == 0
        
    def deploy_gcp(self, project: str, zone: str):
        """Deploy to GCP"""
        print(f"Deploying to GCP project {project}")
        
        # Update GKE deployment
        result = subprocess.run([
            'gcloud', 'container', 'clusters', 'get-credentials',
            'production-cluster', '--zone', zone
        ], capture_output=True)
        
        result = subprocess.run([
            'kubectl', 'set', 'image', 'deployment/app',
            'app=gcr.io/{}/app:{}'.format(project, self.config.version)
        ], capture_output=True)
        
        return result.returncode == 0
        
    def deploy_all(self):
        """Deploy to all clouds"""
        results = {}
        
        # Deploy to each cloud
        for region in self.config.regions:
            results[f'aws-{region}'] = self.deploy_aws(region)
            
        results['azure'] = self.deploy_azure('production-rg')
        results['gcp'] = self.deploy_gcp('my-project', 'us-central1')
        
        # Validate all deployments
        all_success = all(results.values())
        
        if all_success:
            print("All deployments successful!")
        else:
            print("Deployment failures:", results)
            
        return results
        
    def rollback(self, cloud: str):
        """Rollback deployment"""
        print(f"Rolling back {cloud}")
        
        if cloud == 'aws':
            # Get previous task definition
            pass
        elif cloud == 'azure':
            # Rollback deployment
            subprocess.run([
                'kubectl', ' rollout undo', 'deployment/app'
            ])
        elif cloud == 'gcp':
            # Rollback deployment
            subprocess.run([
                'kubectl', ' rollout undo', 'deployment/app'
            ])
```

## COMMON ISSUES

### 1. Deployment Failures

- Incomplete rollbacks
- Solution: Automated rollback

### 2. Secrets Management

- Hardcoded secrets
- Solution: Use secrets manager

### 3. Configuration Drift

- Environment differences
- Solution: IaC with versioning

## PERFORMANCE

### Performance Optimization

| Optimization | Technique | Impact |
|--------------|-----------|--------|
| Parallel Deploy | Deploy all clouds at once | 70% faster |
| Caching | Cache build artifacts | 50% faster |
| Incremental | Deploy only changed | 60% faster |

## COMPATIBILITY

### Deployment Tools

| Tool | Multi-Cloud | Kubernetes | Serverless |
|------|-------------|-------------|------------|
| ArgoCD | Yes | Yes | Yes |
| Flux | Yes | Yes | Yes |
| Spinnaker | Yes | Yes | Yes |
| Jenkins X | Yes | Yes | Yes |

## CROSS-REFERENCES

### Prerequisites

- Basic CI/CD concepts
- Advanced CI/CD
- Terraform knowledge

### Related Topics

1. Terraform IaC
2. GitOps
3. Kubernetes Multi-Cloud

## EXAM TIPS

- Know production patterns
- Understand automation requirements
- Be able to design operational excellence