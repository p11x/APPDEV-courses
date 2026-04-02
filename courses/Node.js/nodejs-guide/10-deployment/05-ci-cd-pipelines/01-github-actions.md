# GitHub Actions and CI/CD Pipeline Implementation

## What You'll Learn

- GitHub Actions comprehensive setup
- Multi-stage CI/CD pipelines
- GitLab CI/CD patterns
- Pipeline optimization strategies
- Pipeline security and compliance

## GitHub Actions Complete Pipeline

```yaml
# .github/workflows/ci-cd.yml
name: CI/CD Pipeline

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]
  release:
    types: [published]

env:
  NODE_VERSION: '20'
  REGISTRY: ghcr.io
  IMAGE_NAME: ${{ github.repository }}

jobs:
  # ──────────── Lint & Type Check ────────────
  quality:
    name: Code Quality
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ env.NODE_VERSION }}
          cache: npm

      - run: npm ci
      - run: npm run lint
      - run: npm run typecheck

  # ──────────── Unit Tests ────────────
  unit-tests:
    name: Unit Tests
    runs-on: ubuntu-latest
    needs: quality
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ env.NODE_VERSION }}
          cache: npm

      - run: npm ci
      - run: npm run test:unit -- --coverage

      - name: Upload coverage
        uses: actions/upload-artifact@v4
        with:
          name: coverage
          path: coverage/

      - name: Coverage check
        uses: codecov/codecov-action@v3
        with:
          fail_ci_if_error: true

  # ──────────── Integration Tests ────────────
  integration-tests:
    name: Integration Tests
    runs-on: ubuntu-latest
    needs: unit-tests
    services:
      postgres:
        image: postgres:15
        env:
          POSTGRES_DB: test_db
          POSTGRES_USER: test_user
          POSTGRES_PASSWORD: test_pass
        ports:
          - 5432:5432
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5

      redis:
        image: redis:7-alpine
        ports:
          - 6379:6379

    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ env.NODE_VERSION }}
          cache: npm

      - run: npm ci
      - run: npm run test:integration
        env:
          DATABASE_URL: postgresql://test_user:test_pass@localhost:5432/test_db
          REDIS_URL: redis://localhost:6379

  # ──────────── Build & Push ────────────
  build:
    name: Build Docker Image
    runs-on: ubuntu-latest
    needs: integration-tests
    if: github.ref == 'refs/heads/main' || github.event_name == 'release'
    permissions:
      contents: read
      packages: write

    outputs:
      image-tag: ${{ steps.meta.outputs.tags }}

    steps:
      - uses: actions/checkout@v4

      - name: Login to GHCR
        uses: docker/login-action@v3
        with:
          registry: ${{ env.REGISTRY }}
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}

      - name: Docker meta
        id: meta
        uses: docker/metadata-action@v5
        with:
          images: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}
          tags: |
            type=sha
            type=semver,pattern={{version}}
            type=ref,event=branch

      - name: Build and push
        uses: docker/build-push-action@v5
        with:
          context: .
          push: true
          tags: ${{ steps.meta.outputs.tags }}
          cache-from: type=gha
          cache-to: type=gha,mode=max

  # ──────────── Security Scan ────────────
  security:
    name: Security Scan
    runs-on: ubuntu-latest
    needs: build
    steps:
      - uses: actions/checkout@v4

      - name: Run Trivy vulnerability scanner
        uses: aquasecurity/trivy-action@master
        with:
          image-ref: ${{ needs.build.outputs.image-tag }}
          format: sarif
          output: trivy-results.sarif

      - name: Upload Trivy results
        uses: github/codeql-action/upload-sarif@v2
        with:
          sarif_file: trivy-results.sarif

      - name: npm audit
        run: npm audit --audit-level=high

  # ──────────── Deploy to Staging ────────────
  deploy-staging:
    name: Deploy Staging
    runs-on: ubuntu-latest
    needs: [build, security]
    if: github.ref == 'refs/heads/main'
    environment:
      name: staging
      url: https://staging.example.com

    steps:
      - uses: actions/checkout@v4
      - uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}

      - name: Deploy to Azure Container Apps
        uses: azure/container-apps-deploy-action@v1
        with:
          containerAppName: my-app-staging
          resourceGroup: my-rg
          imageToDeploy: ${{ needs.build.outputs.image-tag }}

  # ──────────── E2E Tests (Staging) ────────────
  e2e-tests:
    name: E2E Tests
    runs-on: ubuntu-latest
    needs: deploy-staging
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ env.NODE_VERSION }}

      - run: npm ci
      - run: npx playwright install --with-deps
      - run: npm run test:e2e
        env:
          BASE_URL: https://staging.example.com

  # ──────────── Deploy to Production ────────────
  deploy-production:
    name: Deploy Production
    runs-on: ubuntu-latest
    needs: [e2e-tests]
    if: github.event_name == 'release'
    environment:
      name: production
      url: https://example.com

    steps:
      - uses: actions/checkout@v4

      - name: Deploy to production
        run: |
          echo "Deploying ${{ needs.build.outputs.image-tag }} to production"
          # kubectl set image deployment/my-app app=${{ needs.build.outputs.image-tag }}
```

## GitLab CI/CD

```yaml
# .gitlab-ci.yml
stages:
  - quality
  - test
  - build
  - security
  - deploy

variables:
  NODE_IMAGE: node:20-alpine
  DOCKER_IMAGE: docker:24

quality:
  stage: quality
  image: $NODE_IMAGE
  cache:
    key: ${CI_COMMIT_REF_SLUG}
    paths:
      - node_modules/
  script:
    - npm ci
    - npm run lint
    - npm run typecheck
  rules:
    - if: $CI_PIPELINE_SOURCE == "merge_request_event"
    - if: $CI_COMMIT_BRANCH == "main"

unit-tests:
  stage: test
  image: $NODE_IMAGE
  needs: [quality]
  script:
    - npm ci
    - npm run test:unit -- --coverage --reporters=jest-junit
  artifacts:
    reports:
      junit: junit.xml
    coverage_lines:
      - Statements\s*:\s*([\d.]+)%

integration-tests:
  stage: test
  image: $NODE_IMAGE
  services:
    - postgres:15
    - redis:7-alpine
  variables:
    POSTGRES_DB: test_db
    POSTGRES_USER: test_user
    POSTGRES_PASSWORD: test_pass
    DATABASE_URL: postgresql://test_user:test_pass@postgres:5432/test_db
    REDIS_URL: redis://redis:6379
  script:
    - npm ci
    - npm run test:integration

build:
  stage: build
  image: $DOCKER_IMAGE
  services:
    - docker:24-dind
  variables:
    DOCKER_TLS_CERTDIR: "/certs"
  script:
    - docker login -u $CI_REGISTRY_USER -p $CI_REGISTRY_PASSWORD $CI_REGISTRY
    - docker build -t $CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA .
    - docker push $CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA
  rules:
    - if: $CI_COMMIT_BRANCH == "main"

deploy-production:
  stage: deploy
  image: bitnami/kubectl:latest
  script:
    - kubectl set image deployment/my-app app=$CI_REGISTRY_IMAGE:$CI_COMMIT_SHORT_SHA
    - kubectl rollout status deployment/my-app
  environment:
    name: production
    url: https://example.com
  rules:
    - if: $CI_COMMIT_TAG
```

## Pipeline Optimization

```
CI/CD Optimization Strategies:
─────────────────────────────────────────────
Speed:
├── Cache node_modules between runs
├── Run tests in parallel (jest --shard)
├── Use incremental builds
├── Skip unchanged jobs (path filters)
└── Use self-hosted runners for heavy workloads

Cost:
├── Use spot/preemptible runners
├── Cancel outdated pipeline runs
├── Optimize Docker layer caching
├── Use matrix builds for parallelism
└── Schedule non-critical jobs off-peak

Reliability:
├── Retry flaky tests automatically
├── Use timeouts for all jobs
├── Implement rollback on failure
├── Monitor pipeline success rates
└── Use environment protection rules
```

## Best Practices Checklist

- [ ] Cache dependencies between pipeline runs
- [ ] Run unit tests before integration tests
- [ ] Run security scans in every pipeline
- [ ] Use environment protection rules for production
- [ ] Implement automatic rollback on deployment failure
- [ ] Use matrix builds for multi-version testing
- [ ] Set pipeline timeouts to prevent runaway jobs

## Cross-References

- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for deployment patterns
- See [IaC](../06-infrastructure-as-code/01-terraform.md) for infrastructure
- See [Security](../09-deployment-security/01-security-scanning.md) for security
- See [Monitoring](../08-deployment-monitoring/01-apm-metrics.md) for observability

## Next Steps

Continue to [Infrastructure as Code](../06-infrastructure-as-code/01-terraform.md).
