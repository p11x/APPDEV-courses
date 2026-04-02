# GitLab CI/CD, Jenkins, Azure DevOps & Other CI/CD Platforms

## What You'll Learn

- GitLab CI/CD: full pipeline configuration, Docker-in-Docker, Kubernetes deployment
- Jenkins: declarative pipelines, shared libraries, Docker agents, Kubernetes plugin
- Azure DevOps: YAML pipelines, templates, environments, approvals
- CircleCI & Travis CI configuration
- Platform comparison matrix

## Table of Contents

- [GitLab CI/CD](#gitlab-cicd)
- [Jenkins](#jenkins)
- [Azure DevOps Pipelines](#azure-devops-pipelines)
- [CircleCI](#circleci)
- [Travis CI](#travis-ci)
- [Platform Comparison](#platform-comparison)
- [Cross-References](#cross-references)

---

## GitLab CI/CD

GitLab CI/CD uses a `.gitlab-ci.yml` file at the repository root. It supports stages, jobs, variables, caching, artifacts, environments, and review apps.

### Full Pipeline Configuration

```yaml
# .gitlab-ci.yml
stages:
  - install
  - quality
  - test
  - build
  - security
  - deploy-review
  - deploy-staging
  - deploy-production

variables:
  NODE_VERSION: "20"
  DOCKER_HOST: tcp://docker:2376
  DOCKER_TLS_CERTDIR: "/certs"
  DOCKER_DRIVER: overlay2
  npm_config_cache: "$CI_PROJECT_DIR/.npm"

default:
  image: node:${NODE_VERSION}-alpine
  before_script:
    - npm ci --cache .npm --prefer-offline
  cache:
    key:
      files:
        - package-lock.json
    paths:
      - .npm/
      - node_modules/

# ──────────── Install Dependencies ────────────
install:
  stage: install
  script:
    - npm ci
  artifacts:
    paths:
      - node_modules/
    expire_in: 1 hour

# ──────────── Code Quality ────────────
lint:
  stage: quality
  needs: [install]
  script:
    - npm run lint
    - npm run typecheck
  allow_failure: false

prettier:
  stage: quality
  needs: [install]
  script:
    - npm run format:check
  allow_failure: true

# ──────────── Test Matrix ────────────
unit-tests:
  stage: test
  needs: [install]
  script:
    - npm run test:unit -- --coverage --ci
  coverage: '/All files\s*\|\s*([\d.]+)/'
  artifacts:
    reports:
      coverage_report:
        coverage_format: cobertura
        path: coverage/cobertura-coverage.xml
    paths:
      - coverage/
    expire_in: 1 week

integration-tests:
  stage: test
  needs: [install]
  services:
    - name: postgres:16-alpine
      alias: postgres
    - name: redis:7-alpine
      alias: redis
  variables:
    POSTGRES_DB: test_db
    POSTGRES_USER: test_user
    POSTGRES_PASSWORD: test_pass
    DATABASE_URL: "postgresql://test_user:test_pass@postgres:5432/test_db"
    REDIS_URL: "redis://redis:6379"
  script:
    - npm run test:integration -- --ci

e2e-tests:
  stage: test
  needs: [install]
  image: mcr.microsoft.com/playwright:v1.40.0
  script:
    - npm run test:e2e -- --ci
  artifacts:
    when: on_failure
    paths:
      - test-results/
    expire_in: 3 days

# ──────────── Build Docker Image ────────────
build:
  stage: build
  image: docker:24-dind
  services:
    - docker:24-dind
  variables:
    IMAGE_TAG: "$CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA"
  script:
    - docker login -u $CI_REGISTRY_USER -p $CI_REGISTRY_PASSWORD $CI_REGISTRY
    - docker build --cache-from $CI_REGISTRY_IMAGE:latest -t $IMAGE_TAG -t $CI_REGISTRY_IMAGE:latest .
    - docker push $IMAGE_TAG
    - docker push $CI_REGISTRY_IMAGE:latest
  only:
    - main
    - tags

# ──────────── Security Scanning ────────────
sast:
  stage: security
  needs: [install]
  script:
    - npx audit-ci --critical
    - npx snyk test --severity-threshold=high
  variables:
    SNYK_TOKEN: $SNYK_TOKEN
  allow_failure: false

dependency-scan:
  stage: security
  needs: [install]
  script:
    - npm audit --audit-level=high
    - npx better-npm-audit audit
  artifacts:
    reports:
      dependency_scanning: gl-dependency-scanning-report.json
  allow_failure: true

container-scan:
  stage: security
  image: docker:24-dind
  services:
    - docker:24-dind
  needs: [build]
  script:
    - docker pull $CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA
    - docker run --rm -v /var/run/docker.sock:/var/run/docker.sock
      aquasec/trivy image $CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA
        --severity HIGH,CRITICAL --exit-code 1

# ──────────── Deploy to Review App ────────────
deploy-review:
  stage: deploy-review
  image: bitnami/kubectl:latest
  environment:
    name: review/$CI_COMMIT_REF_SLUG
    url: https://$CI_COMMIT_REF_SLUG.review.example.com
    on_stop: stop-review
  script:
    - kubectl create namespace $CI_COMMIT_REF_SLUG || true
    - kubectl set image deployment/app app=$CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA
      -n $CI_COMMIT_REF_SLUG
    - kubectl rollout status deployment/app -n $CI_COMMIT_REF_SLUG --timeout=300s
  rules:
    - if: $CI_MERGE_REQUEST_IID

stop-review:
  stage: deploy-review
  image: bitnami/kubectl:latest
  environment:
    name: review/$CI_COMMIT_REF_SLUG
    action: stop
  script:
    - kubectl delete namespace $CI_COMMIT_REF_SLUG || true
  rules:
    - if: $CI_MERGE_REQUEST_IID
      when: manual

# ──────────── Deploy to Staging ────────────
deploy-staging:
  stage: deploy-staging
  image: bitnami/kubectl:latest
  environment:
    name: staging
    url: https://staging.example.com
  script:
    - kubectl set image deployment/app-staging app=$CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA
      -n staging
    - kubectl rollout status deployment/app-staging -n staging --timeout=300s
  only:
    - main

# ──────────── Deploy to Production ────────────
deploy-production:
  stage: deploy-production
  image: bitnami/kubectl:latest
  environment:
    name: production
    url: https://app.example.com
  script:
    - kubectl set image deployment/app-production app=$CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA
      -n production
    - kubectl rollout status deployment/app-production -n production --timeout=600s
  when: manual
  only:
    - tags
  allow_failure: false
```

### GitLab CI/CD Key Features

| Feature | Description |
|---------|-------------|
| **Services** | Sidecar containers (databases, caches) per job |
| **Cache** | Shared between pipeline runs (keyed by lock file) |
| **Artifacts** | Files passed between stages, test reports |
| **Environments** | Track deployments, review apps per MR |
| **Rules** | Fine-grained job triggering with `if`, `when`, `exists` |
| **Coverage** | Regex-based coverage extraction from output |
| **Reports** | JUnit, SAST, dependency scanning native integration |

---

## Jenkins

Jenkins uses a `Jenkinsfile` (declarative or scripted pipeline). It supports shared libraries, Docker agents, Blue Ocean UI, and the Kubernetes plugin.

### Declarative Pipeline

```groovy
// Jenkinsfile
pipeline {
    agent {
        kubernetes {
            yaml '''
                apiVersion: v1
                kind: Pod
                spec:
                  containers:
                  - name: node
                    image: node:20-alpine
                    command: ["sleep"]
                    args: ["infinity"]
                    resources:
                      requests:
                        memory: "512Mi"
                        cpu: "500m"
                  - name: docker
                    image: docker:24-dind
                    securityContext:
                      privileged: true
                    volumeMounts:
                    - name: docker-storage
                      mountPath: /var/lib/docker
                  volumes:
                  - name: docker-storage
                    emptyDir: {}
            '''
        }
    }

    environment {
        NODE_VERSION    = '20'
        DOCKER_REGISTRY = 'registry.example.com'
        IMAGE_NAME      = 'myapp'
        SNYK_TOKEN      = credentials('snyk-token')
        NPM_TOKEN       = credentials('npm-token')
    }

    options {
        timeout(time: 30, unit: 'MINUTES')
        disableConcurrentBuilds()
        buildDiscarder(logRotator(numToKeepStr: '20'))
        timestamps()
        ansiColor('xterm')
    }

    stages {
        stage('Install') {
            steps {
                container('node') {
                    sh 'npm ci --prefer-offline'
                }
            }
        }

        stage('Quality') {
            parallel {
                stage('Lint') {
                    steps {
                        container('node') {
                            sh 'npm run lint'
                            sh 'npm run typecheck'
                        }
                    }
                }
                stage('Format') {
                    steps {
                        container('node') {
                            sh 'npm run format:check'
                        }
                    }
                }
            }
        }

        stage('Test') {
            parallel {
                stage('Unit Tests') {
                    steps {
                        container('node') {
                            sh 'npm run test:unit -- --coverage --ci'
                        }
                    }
                    post {
                        always {
                            junit 'junit.xml'
                            publishHTML(target: [
                                reportDir: 'coverage/lcov-report',
                                reportFiles: 'index.html',
                                reportName: 'Coverage Report'
                            ])
                        }
                    }
                }
                stage('Integration Tests') {
                    steps {
                        container('node') {
                            sh 'npm run test:integration -- --ci'
                        }
                    }
                }
            }
        }

        stage('Security') {
            steps {
                container('node') {
                    sh 'npm audit --audit-level=high'
                    sh "npx snyk test --severity-threshold=high"
                }
            }
        }

        stage('Build & Push') {
            when { branch 'main' }
            steps {
                container('docker') {
                    sh """
                        docker login -u \$DOCKER_USER -p \$DOCKER_PASS ${DOCKER_REGISTRY}
                        docker build -t ${DOCKER_REGISTRY}/${IMAGE_NAME}:${GIT_COMMIT} .
                        docker tag ${DOCKER_REGISTRY}/${IMAGE_NAME}:${GIT_COMMIT} ${DOCKER_REGISTRY}/${IMAGE_NAME}:latest
                        docker push ${DOCKER_REGISTRY}/${IMAGE_NAME}:${GIT_COMMIT}
                        docker push ${DOCKER_REGISTRY}/${IMAGE_NAME}:latest
                    """
                }
            }
        }

        stage('Deploy Staging') {
            when { branch 'main' }
            steps {
                container('node') {
                    sh """
                        kubectl set image deployment/${IMAGE_NAME} ${IMAGE_NAME}=${DOCKER_REGISTRY}/${IMAGE_NAME}:${GIT_COMMIT} -n staging
                        kubectl rollout status deployment/${IMAGE_NAME} -n staging --timeout=300s
                    """
                }
            }
        }

        stage('Deploy Production') {
            when { buildingTag() }
            input {
                message "Deploy to production?"
                ok "Deploy"
                parameters {
                    string(name: 'CONFIRM', defaultValue: '', description: 'Type DEPLOY to confirm')
                }
            }
            steps {
                script {
                    if (CONFIRM != 'DEPLOY') error('Deployment cancelled')
                }
                container('node') {
                    sh """
                        kubectl set image deployment/${IMAGE_NAME} ${IMAGE_NAME}=${DOCKER_REGISTRY}/${IMAGE_NAME}:${GIT_COMMIT} -n production
                        kubectl rollout status deployment/${IMAGE_NAME} -n production --timeout=600s
                    """
                }
            }
        }
    }

    post {
        success {
            slackSend(color: 'good', message: "Pipeline SUCCESS: ${env.JOB_NAME} #${env.BUILD_NUMBER}")
        }
        failure {
            slackSend(color: 'danger', message: "Pipeline FAILED: ${env.JOB_NAME} #${env.BUILD_NUMBER}")
        }
    }
}
```

### Jenkins Shared Library Example

```groovy
// vars/nodePipeline.groovy
def call(Map config) {
    pipeline {
        agent { label config.agent ?: 'docker' }
        environment {
            NODE_VERSION = config.nodeVersion ?: '20'
        }
        stages {
            stage('Install') {
                steps {
                    sh 'npm ci'
                }
            }
            stage('Test') {
                steps {
                    sh "npm run ${config.testCommand ?: 'test'}"
                }
            }
            stage('Deploy') {
                when { branch config.deployBranch ?: 'main' }
                steps {
                    sh "kubectl set image deployment/${config.app} ${config.app}=${config.registry}/${config.app}:${GIT_COMMIT}"
                }
            }
        }
    }
}

// Jenkinsfile (consumer)
@Library('my-shared-lib') _
nodePipeline(
    app: 'myapp',
    registry: 'registry.example.com',
    testCommand: 'test:ci',
    deployBranch: 'main'
)
```

### Jenkins Key Features

| Feature | Description |
|---------|-------------|
| **Shared Libraries** | Reusable pipeline code across projects |
| **Blue Ocean** | Modern UI for pipeline visualization |
| **Kubernetes Plugin** | Dynamic pod agents per pipeline |
| **Parallel Stages** | Run independent stages concurrently |
| **Input Steps** | Manual approval gates |
| **Credentials** | Secret management with credential binding |
| **Distributed Builds** | Multiple agents across infrastructure |

---

## Azure DevOps Pipelines

Azure DevOps uses YAML pipelines with stages, jobs, templates, variable groups, environments, and service connections.

### Multi-Stage YAML Pipeline

```yaml
# azure-pipelines.yml
trigger:
  branches:
    include: [main, develop, 'release/*']
  paths:
    exclude: ['docs/**', '*.md']

pr:
  branches:
    include: [main]

variables:
  - group: 'node-app-variables'
  - name: NODE_VERSION
    value: '20'
  - name: IMAGE_NAME
    value: '$(Build.Repository.Name)'
  - ${{ if eq(variables['Build.SourceBranch'], 'refs/heads/main') }}:
    - name: ENVIRONMENT
      value: production

pool:
  vmImage: 'ubuntu-latest'

stages:
  - stage: Quality
    displayName: 'Code Quality'
    jobs:
      - job: Lint
        displayName: 'Lint & Type Check'
        steps:
          - task: NodeTool@0
            inputs:
              versionSpec: $(NODE_VERSION)
            displayName: 'Install Node.js'

          - task: Cache@2
            inputs:
              key: 'npm | "$(Agent.OS)" | package-lock.json'
              restoreKeys: npm | "$(Agent.OS)"
              path: $(npm_config_cache)
            displayName: 'Cache npm'

          - script: npm ci
            displayName: 'Install dependencies'

          - script: npm run lint
            displayName: 'ESLint'

          - script: npm run typecheck
            displayName: 'TypeScript check'

  - stage: Test
    displayName: 'Test'
    dependsOn: Quality
    jobs:
      - job: UnitTests
        displayName: 'Unit Tests'
        steps:
          - task: NodeTool@0
            inputs:
              versionSpec: $(NODE_VERSION)

          - script: npm ci && npm run test:unit -- --ci --coverage
            displayName: 'Run unit tests'

          - task: PublishTestResults@2
            inputs:
              testResultsFormat: 'JUnit'
              testResultsFiles: 'junit.xml'
            condition: succeededOrFailed()

          - task: PublishCodeCoverageResults@2
            inputs:
              summaryFileLocation: 'coverage/cobertura-coverage.xml'

      - job: IntegrationTests
        displayName: 'Integration Tests'
        services:
          postgres:
            image: postgres:16-alpine
            env:
              POSTGRES_DB: test_db
              POSTGRES_USER: test_user
              POSTGRES_PASSWORD: test_pass
          redis:
            image: redis:7-alpine
        steps:
          - task: NodeTool@0
            inputs:
              versionSpec: $(NODE_VERSION)

          - script: npm ci && npm run test:integration -- --ci
            displayName: 'Run integration tests'
            env:
              DATABASE_URL: 'postgresql://test_user:test_pass@localhost:5432/test_db'

  - stage: Security
    displayName: 'Security Scan'
    dependsOn: Quality
    jobs:
      - job: SAST
        displayName: 'SAST & Dependency Scan'
        steps:
          - task: NodeTool@0
            inputs:
              versionSpec: $(NODE_VERSION)

          - script: npm ci && npm audit --audit-level=high
            displayName: 'npm audit'

          - script: npx snyk test --severity-threshold=high
            displayName: 'Snyk security scan'
            env:
              SNYK_TOKEN: $(snyk-token)

  - stage: Build
    displayName: 'Build & Push'
    dependsOn: [Test, Security]
    jobs:
      - job: DockerBuild
        displayName: 'Build Docker Image'
        steps:
          - task: Docker@2
            inputs:
              containerRegistry: 'azure-container-registry'
              repository: $(IMAGE_NAME)
              command: 'buildAndPush'
              Dockerfile: '**/Dockerfile'
              tags: |
                $(Build.BuildId)
                latest

  - stage: DeployStaging
    displayName: 'Deploy to Staging'
    dependsOn: Build
    jobs:
      - deployment: DeployStaging
        displayName: 'Deploy to Staging'
        environment: 'staging'
        strategy:
          runOnce:
            deploy:
              steps:
                - task: KubernetesManifest@0
                  inputs:
                    action: 'deploy'
                    kubernetesServiceConnection: 'k8s-staging'
                    namespace: 'staging'
                    manifests: 'k8s/staging/*.yml'
                    containers: '$(AcrUrl)/$(IMAGE_NAME):$(Build.BuildId)'

  - stage: DeployProduction
    displayName: 'Deploy to Production'
    dependsOn: DeployStaging
    condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
    jobs:
      - deployment: DeployProduction
        displayName: 'Deploy to Production'
        environment: 'production'
        strategy:
          runOnce:
            deploy:
              steps:
                - task: KubernetesManifest@0
                  inputs:
                    action: 'deploy'
                    kubernetesServiceConnection: 'k8s-production'
                    namespace: 'production'
                    manifests: 'k8s/production/*.yml'
                    containers: '$(AcrUrl)/$(IMAGE_NAME):$(Build.BuildId)'
```

### Reusable Templates

```yaml
# templates/node-job.yml
parameters:
  - name: nodeVersion
    type: string
    default: '20'
  - name: steps
    type: stepList

jobs:
  - job: NodeJob
    steps:
      - task: NodeTool@0
        inputs:
          versionSpec: ${{ parameters.nodeVersion }}

      - script: npm ci
        displayName: 'Install dependencies'

      - ${{ parameters.steps }}

# azure-pipelines.yml (usage)
stages:
  - stage: Test
    jobs:
      - template: templates/node-job.yml
        parameters:
          steps:
            - script: npm run test:ci
              displayName: 'Run tests'
```

### Azure DevOps Key Features

| Feature | Description |
|---------|-------------|
| **Templates** | Reusable YAML for stages, jobs, steps |
| **Variable Groups** | Shared variables across pipelines |
| **Environments** | Deployment targets with approvals/gates |
| **Service Connections** | Authenticated links to external services |
| **Deployment Jobs** | Specialized jobs with strategies (runOnce, rolling, canary) |
| **Approvals** | Manual approval before deployment stages |
| **Agent Pools** | Microsoft-hosted or self-hosted agents |

---

## CircleCI

CircleCI uses `.circleci/config.yml` with orbs (reusable configuration packages), workflows, executors, and test splitting.

### CircleCI Configuration

```yaml
# .circleci/config.yml
version: 2.1

orbs:
  node: circleci/node@6.0
  docker: circleci/docker@2.4
  kubernetes: circleci/kubernetes@1.3

executors:
  node-executor:
    docker:
      - image: cimg/node:20.0
    resource_class: medium
    working_directory: ~/project

jobs:
  install:
    executor: node-executor
    steps:
      - checkout
      - node/install-packages:
          pkg-manager: npm
          cache-version: v2
      - persist_to_workspace:
          root: ~/project
          paths: [./*]

  lint-and-typecheck:
    executor: node-executor
    steps:
      - attach_workspace:
          at: ~/project
      - run:
          name: Lint
          command: npm run lint
      - run:
          name: Type Check
          command: npm run typecheck

  unit-tests:
    executor: node-executor
    parallelism: 4
    steps:
      - attach_workspace:
          at: ~/project
      - run:
          name: Split test files
          command: |
            TESTFILES=$(circleci tests glob "src/**/*.test.ts" | circleci tests split --split-by=timings)
            echo $TESTFILES | tr ' ' '\n' > /tmp/test-files.txt
      - run:
          name: Run tests
          command: npm run test -- --ci --passWithNoTests $(cat /tmp/test-files.txt | tr '\n' ' ')
      - store_test_results:
          path: test-results

  integration-tests:
    executor:
      docker:
        - image: cimg/node:20.0
        - image: cimg/postgres:16.0
          environment:
            POSTGRES_DB: test_db
            POSTGRES_USER: test_user
            POSTGRES_PASSWORD: test_pass
        - image: cimg/redis:7.0
    steps:
      - attach_workspace:
          at: ~/project
      - run:
          name: Wait for services
          command: dockerize -wait tcp://localhost:5432 -wait tcp://localhost:6379 -timeout 30s
      - run:
          name: Run integration tests
          command: npm run test:integration -- --ci
          environment:
            DATABASE_URL: postgresql://test_user:test_pass@localhost:5432/test_db

  build-and-push:
    executor: node-executor
    steps:
      - attach_workspace:
          at: ~/project
      - setup_remote_docker:
          version: default
      - docker/build:
          image: $CIRCLE_PROJECT_REPONAME
          tag: $CIRCLE_SHA1,latest
      - docker/push:
          image: $CIRCLE_PROJECT_REPONAME
          tag: $CIRCLE_SHA1,latest

  deploy:
    executor: node-executor
    parameters:
      environment:
        type: string
    steps:
      - attach_workspace:
          at: ~/project
      - kubernetes/install-kubectl
      - run:
          name: Deploy to << parameters.environment >>
          command: |
            kubectl set image deployment/app app=$CIRCLE_PROJECT_REPONAME:$CIRCLE_SHA1 \
              -n << parameters.environment >>
            kubectl rollout status deployment/app -n << parameters.environment >> --timeout=300s

workflows:
  build-test-deploy:
    jobs:
      - install
      - lint-and-typecheck:
          requires: [install]
      - unit-tests:
          requires: [install]
      - integration-tests:
          requires: [install]
      - build-and-push:
          requires: [lint-and-typecheck, unit-tests, integration-tests]
          filters:
            branches:
              only: main
      - deploy:
          name: deploy-staging
          environment: staging
          requires: [build-and-push]
          filters:
            branches:
              only: main
      - deploy:
          name: deploy-production
          environment: production
          requires: [deploy-staging]
          filters:
            branches:
              only: main
          type: approval
```

---

## Travis CI

Travis CI uses `.travis.yml` with build stages, deployment providers, and conditional execution.

### Travis CI Configuration

```yaml
# .travis.yml
language: node_js
node_js:
  - '20'

os: linux
dist: jammy

cache:
  npm: true
  directories:
    - ~/.cache/ms-playwright

stages:
  - name: quality
  - name: test
  - name: security
  - name: build
    if: branch = main OR tag IS present
  - name: deploy
    if: branch = main OR tag IS present

jobs:
  include:
    - stage: quality
      name: Lint & Type Check
      script:
        - npm run lint
        - npm run typecheck

    - stage: test
      name: Unit Tests
      script:
        - npm run test:unit -- --coverage --ci
      after_success:
        - npx codecov

    - stage: test
      name: Integration Tests
      services:
        - postgresql
        - redis-server
      env:
        - DATABASE_URL=postgresql://postgres@localhost/test_db
      before_script:
        - psql -c 'create database test_db;' -U postgres
      script:
        - npm run test:integration -- --ci

    - stage: test
      name: E2E Tests
      before_script:
        - npx playwright install --with-deps
      script:
        - npm run test:e2e -- --ci

    - stage: security
      name: Security Scan
      script:
        - npm audit --audit-level=high
        - npx snyk test

    - stage: build
      name: Docker Build & Push
      services:
        - docker
      script:
        - docker build -t $DOCKER_REPO:$TRAVIS_COMMIT .
        - docker tag $DOCKER_REPO:$TRAVIS_COMMIT $DOCKER_REPO:latest
      deploy:
        provider: script
        script: bash scripts/docker-push.sh
        on:
          branch: main

    - stage: deploy
      name: Deploy to Staging
      if: branch = main AND type != pull_request
      deploy:
        provider: script
        script: bash scripts/deploy.sh staging
        on:
          branch: main

    - stage: deploy
      name: Deploy to Production
      if: tag IS present
      deploy:
        provider: script
        script: bash scripts/deploy.sh production
        on:
          tags: true

notifications:
  email:
    on_success: never
    on_failure: always
  slack:
    rooms:
      - secure: "encrypted-token"
    on_success: change
    on_failure: always
```

---

## Platform Comparison

| Feature | GitHub Actions | GitLab CI/CD | Jenkins | Azure DevOps | CircleCI | Travis CI |
|---------|---------------|-------------|---------|-------------|---------|-----------|
| **Config Format** | YAML | YAML | Groovy | YAML | YAML | YAML |
| **Self-Hosted** | Yes (runners) | Yes (runners) | Yes (agents) | Yes (agents) | Yes (runners) | Limited |
| **Free Tier** | 2000 min/mo | 400 min/mo | Free (OSS) | 1800 min/mo | 6000 credits/mo | Free (OSS) |
| **Docker Support** | Native | Native | Plugin | Native | Native | Service |
| **Kubernetes** | Actions | Native | Plugin | Native | Orb | Manual |
| **Matrix Builds** | Native | Parallel | Parallel | Strategy | Parallelism | Matrix |
| **Secret Management** | Secrets | Variables | Credentials | Variable Groups | Contexts | Env Vars |
| **Marketplace** | Actions | Registry | Plugins | Extensions | Orbs | N/A |
| **Approval Gates** | Environments | Manual jobs | Input step | Environments | Approval jobs | N/A |
| **Caching** | Actions cache | Cache directive | Plugins | Cache task | Save/restore | Built-in |
| **Best For** | GitHub repos | GitLab repos | Complex/legacy | Enterprise | Fast builds | Open source |
| **Learning Curve** | Low | Low-Medium | High | Medium | Low | Low |

### When to Use Each Platform

| Scenario | Recommended Platform |
|----------|---------------------|
| GitHub-hosted repositories | **GitHub Actions** |
| Self-hosted GitLab | **GitLab CI/CD** |
| Enterprise with complex requirements | **Jenkins** or **Azure DevOps** |
| Microsoft/Azure ecosystem | **Azure DevOps** |
| Fast parallelized builds | **CircleCI** |
| Simple open-source projects | **Travis CI** |
| Need Kubernetes-native builds | **Jenkins** (K8s plugin) or **GitLab** |

---

## Cross-References

- [GitHub Actions](./01-github-actions.md) — detailed GitHub Actions pipeline configuration
- [Pipeline Security & Optimization](./03-pipeline-security-optimization.md) — security scanning, secret management, performance tuning
- [Docker & Containers](../05-express-framework/) — containerization for CI/CD
- [Kubernetes Deployment](../10-deployment/) — container orchestration
- [Testing Strategies](../09-testing/) — test frameworks and coverage integration
