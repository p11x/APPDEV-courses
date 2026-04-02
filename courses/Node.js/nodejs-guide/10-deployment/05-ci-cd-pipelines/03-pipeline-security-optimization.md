# CI/CD Pipeline Security, Optimization & Advanced Patterns

## What You'll Learn

- Pipeline optimization: caching, parallel execution, matrix builds, path filtering
- Pipeline security: secret management, OIDC, least-privilege accounts
- Security scanning integration: SAST, DAST, SCA, container scanning
- Compliance: SOC2, PCI-DSS pipeline requirements
- Error handling: retries, notifications, automatic rollback
- GitOps with ArgoCD and Flux
- Progressive delivery: feature flags, canary analysis with Flagger

## Table of Contents

- [Pipeline Optimization](#pipeline-optimization)
- [Pipeline Security](#pipeline-security)
- [Security Scanning Integration](#security-scanning-integration)
- [Compliance](#compliance)
- [Error Handling](#error-handling)
- [Pipeline Testing](#pipeline-testing)
- [GitOps Workflow](#gitops-workflow)
- [Progressive Delivery](#progressive-delivery)
- [Pipeline Visualization & Debugging](#pipeline-visualization--debugging)
- [Cross-References](#cross-ferences)

---

## Pipeline Optimization

### Caching Strategies

```yaml
# GitHub Actions - Multi-layer caching
jobs:
  build:
    steps:
      - uses: actions/checkout@v4

      # npm cache (built-in with setup-node)
      - uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: npm

      # Custom cache for build artifacts
      - uses: actions/cache@v4
        id: build-cache
        with:
          path: |
            dist/
            .turbo/
            node_modules/.cache/
          key: build-${{ runner.os }}-${{ hashFiles('src/**', 'package-lock.json') }}
          restore-keys: |
            build-${{ runner.os }}-

      # Docker layer caching
      - uses: docker/build-push-action@v5
        with:
          context: .
          push: true
          tags: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}:${{ github.sha }}
          cache-from: type=gha
          cache-to: type=gha,mode=max

      - run: npm ci
      - run: npm run build
```

### Parallel Execution & Matrix Builds

```yaml
# GitHub Actions - Matrix strategy with fail-fast
jobs:
  test:
    strategy:
      fail-fast: false
      matrix:
        node-version: [18, 20, 22]
        os: [ubuntu-latest, windows-latest, macos-latest]
        shard: [1, 2, 3, 4]
        exclude:
          - os: macos-latest
            node-version: 18
    runs-on: ${{ matrix.os }}
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ matrix.node-version }}
          cache: npm
      - run: npm ci
      - run: npm run test -- --shard=${{ matrix.shard }}/4
```

### Path Filtering

```yaml
# GitHub Actions - Path-based triggering
on:
  push:
    paths:
      - 'src/**'
      - 'package*.json'
      - '.github/workflows/**'
    paths-ignore:
      - 'docs/**'
      - '*.md'
      - 'assets/**'

jobs:
  # Only run docs job when docs change
  docs:
    if: contains(github.event.head_commit.modified, 'docs/')
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: npm run docs:build

  # Skip build if only markdown changed
  build:
    if: |
      !contains(github.event.head_commit.message, '[skip ci]') &&
      !startsWith(github.event.head_commit.message, 'docs:')
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - run: npm ci && npm run build
```

### Incremental Builds with Turborepo

```json
// turbo.json
{
  "$schema": "https://turbo.build/schema.json",
  "tasks": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**"],
      "inputs": ["src/**", "package.json", "tsconfig.json"]
    },
    "test": {
      "dependsOn": ["build"],
      "outputs": ["coverage/**"],
      "inputs": ["src/**", "test/**"]
    },
    "lint": {
      "outputs": [],
      "inputs": ["src/**", ".eslintrc.*"]
    }
  }
}
```

```yaml
# GitHub Actions with Turborepo remote caching
env:
  TURBO_TOKEN: ${{ secrets.TURBO_TOKEN }}
  TURBO_TEAM: ${{ vars.TURBO_TEAM }}

steps:
  - run: npm run build -- --cache-dir=.turbo/cache
  - run: npm run test -- --cache-dir=.turbo/cache
```

### Pipeline Performance Monitoring

```yaml
# Track pipeline duration across runs
jobs:
  metrics:
    runs-on: ubuntu-latest
    steps:
      - name: Record pipeline start
        id: start
        run: echo "time=$(date +%s)" >> $GITHUB_OUTPUT

      # ... actual pipeline steps ...

      - name: Record pipeline end
        if: always()
        run: |
          END=$(date +%s)
          DURATION=$((END - ${{ steps.start.outputs.time }}))
          echo "Pipeline duration: ${DURATION}s"

          # Send metrics to monitoring system
          curl -X POST "$METRICS_ENDPOINT" \
            -H "Authorization: Bearer ${{ secrets.METRICS_TOKEN }}" \
            -d "pipeline_duration_seconds{repo=\"${{ github.repository }}\"} ${DURATION}"
```

---

## Pipeline Security

### Secret Management Best Practices

```yaml
# GitHub Actions - Secret handling
jobs:
  deploy:
    runs-on: ubuntu-latest
    # Restrict permissions to minimum required
    permissions:
      contents: read
      id-token: write  # Required for OIDC
      deployments: write

    steps:
      # OIDC authentication (no long-lived credentials)
      - uses: aws-actions/configure-aws-credentials@v4
        with:
          role-to-assume: arn:aws:iam::123456789:role/github-actions
          aws-region: us-east-1

      # Azure OIDC
      - uses: azure/login@v2
        with:
          client-id: ${{ secrets.AZURE_CLIENT_ID }}
          tenant-id: ${{ secrets.AZURE_TENANT_ID }}
          subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}

      # GCP Workload Identity Federation
      - uses: google-github-actions/auth@v2
        with:
          workload_identity_provider: projects/123/locations/global/workloadIdentityPools/github/providers/github
          service_account: github-actions@project.iam.gserviceaccount.com

      # Never echo secrets
      - name: Deploy with secret
        env:
          API_KEY: ${{ secrets.API_KEY }}
          DATABASE_URL: ${{ secrets.DATABASE_URL }}
        run: |
          # ✅ Pass via environment variable
          node scripts/deploy.js

          # ❌ NEVER do this
          # echo $API_KEY
          # echo "$DATABASE_URL" | base64
```

### OIDC Authentication Setup

```yaml
# GitHub Actions OIDC with AWS - Full example
name: Deploy to AWS with OIDC

on:
  push:
    branches: [main]

permissions:
  id-token: write
  contents: read

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Configure AWS credentials via OIDC
        uses: aws-actions/configure-aws-credentials@v4
        with:
          role-to-assume: arn:aws:iam::123456789:role/github-actions-deploy
          role-session-name: github-actions-${{ github.run_id }}
          aws-region: us-east-1
          role-duration-seconds: 900  # 15 min, minimum for task

      - name: Deploy to ECS
        run: |
          aws ecs update-service \
            --cluster production \
            --service myapp \
            --force-new-deployment
```

```yaml
# GitLab CI/CD - HashiCorp Vault integration
variables:
  VAULT_ADDR: https://vault.example.com

secrets:
  database_url:
    vault:
      engine:
        name: kv-v2
        path: secret
      path: myapp/database
      field: url
```

### Least-Privilege Service Accounts

```yaml
# Kubernetes - Least-privilege service account for CI/CD
apiVersion: v1
kind: ServiceAccount
metadata:
  name: cicd-deployer
  namespace: production
---
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: deployer-role
  namespace: production
rules:
  - apiGroups: ["apps"]
    resources: ["deployments"]
    verbs: ["get", "list", "patch", "update"]
  - apiGroups: [""]
    resources: ["services", "configmaps"]
    verbs: ["get", "list"]
  - apiGroups: [""]
    resources: ["pods"]
    verbs: ["get", "list", "watch"]
  # No secrets access, no delete permissions
---
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: deployer-binding
  namespace: production
subjects:
  - kind: ServiceAccount
    name: cicd-deployer
    namespace: production
roleRef:
  kind: Role
  name: deployer-role
  apiGroup: rbac.authorization.k8s.io
```

---

## Security Scanning Integration

### SAST, SCA, DAST, and Container Scanning Pipeline

```yaml
# .github/workflows/security.yml
name: Security Pipeline

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
  schedule:
    - cron: '0 6 * * 1'  # Weekly Monday scan

permissions:
  contents: read
  security-events: write

jobs:
  # ──── Static Application Security Testing ────
  sast:
    name: SAST
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      # CodeQL (GitHub native)
      - uses: github/codeql-action/init@v3
        with:
          languages: javascript

      - uses: github/codeql-action/analyze@v3
        with:
          category: '/language:javascript'

      # Semgrep
      - uses: returntocorp/semgrep-action@v1
        with:
          config: p/ci
        env:
          SEMGREP_APP_TOKEN: ${{ secrets.SEMGREP_APP_TOKEN }}

  # ──── Software Composition Analysis ────
  sca:
    name: Dependency Scan
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: npm
      - run: npm ci

      # npm audit
      - name: npm audit
        run: npm audit --audit-level=high --json > npm-audit.json
        continue-on-error: true

      # Snyk
      - uses: snyk/actions/node@master
        with:
          args: --severity-threshold=high --json-file-output=snyk.json
        env:
          SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}

      # OWASP Dependency-Check
      - uses: dependency-check/Dependency-Check_Action@main
        with:
          project: 'myapp'
          path: '.'
          format: 'JSON'

      # Upload SARIF results
      - uses: github/codeql-action/upload-sarif@v3
        with:
          sarif_file: snyk.sarif
        if: always()

  # ──── Container Scanning ────
  container-scan:
    name: Container Scan
    runs-on: ubuntu-latest
    needs: [sast, sca]
    steps:
      - uses: actions/checkout@v4

      - name: Build image for scanning
        run: docker build -t myapp:scan .

      # Trivy
      - name: Trivy scan
        uses: aquasecurity/trivy-action@master
        with:
          image-ref: myapp:scan
          format: sarif
          output: trivy-results.sarif
          severity: CRITICAL,HIGH
          exit-code: '1'

      # Grype
      - name: Grype scan
        uses: anchore/scan-action@v4
        with:
          image: myapp:scan
          fail-build: true
          severity-cutoff: high

      - uses: github/codeql-action/upload-sarif@v3
        with:
          sarif_file: trivy-results.sarif
        if: always()

  # ──── Dynamic Application Security Testing ────
  dast:
    name: DAST
    runs-on: ubuntu-latest
    needs: [container-scan]
    if: github.ref == 'refs/heads/main'
    services:
      app:
        image: myapp:${{ github.sha }}
        ports:
          - 3000:3000
    steps:
      # ZAP Baseline Scan
      - name: ZAP scan
        uses: zaproxy/action-baseline@v0.12.0
        with:
          target: http://localhost:3000
          rules_file_name: .zap/rules.tsv
          allow_issue_writing: true

      # Nuclei
      - name: Nuclei scan
        uses: projectdiscovery/nuclei-action@main
        with:
          target: http://localhost:3000
          severity: high,critical
```

### License Compliance Check

```yaml
  license-check:
    name: License Compliance
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: npm
      - run: npm ci

      - name: Check licenses
        run: |
          npx license-checker --summary --failOn 'GPL-3.0;AGPL-3.0;SSPL-1.0'
```

---

## Compliance

### SOC2 & PCI-DSS Pipeline Requirements

```yaml
# Compliance-enforced pipeline
name: Compliance Pipeline

on:
  push:
    branches: [main]

jobs:
  compliance:
    runs-on: ubuntu-latest
    environment: production  # Requires approval

    steps:
      # Audit trail: every action logged
      - name: Audit - Pipeline start
        run: |
          echo "Pipeline started: $(date -u +%Y-%m-%dT%H:%M:%SZ)" >> audit.log
          echo "Triggered by: ${{ github.actor }}" >> audit.log
          echo "Commit: ${{ github.sha }}" >> audit.log
          echo "Repository: ${{ github.repository }}" >> audit.log

      - uses: actions/checkout@v4
        with:
          # Verify commit signatures (SOC2 CC7.1)
          persist-credentials: false

      - name: Verify commit signatures
        run: |
          git verify-commit HEAD || exit 1
          echo "Commit signature verified" >> audit.log

      # Dependency integrity (PCI-DSS 6.3.2)
      - name: Audit dependencies
        run: |
          npm ci --ignore-scripts
          npm audit --audit-level=high
          echo "Dependency audit completed" >> audit.log

      # SAST scanning (PCI-DSS 6.5)
      - name: SAST scan
        run: |
          npx semgrep --config=p/ci --json > semgrep.json
          CRITICAL=$(jq '[.results[] | select(.extra.severity == "ERROR")] | length' semgrep.json)
          echo "Critical SAST findings: $CRITICAL" >> audit.log
          if [ "$CRITICAL" -gt 0 ]; then exit 1; fi

      # Secrets detection (SOC2 CC6.1)
      - name: Secrets scan
        uses: trufflesecurity/trufflehog@main
        with:
          extra_args: --only-verified --fail

      # Signed artifact (SOC2 CC7.2)
      - name: Build and sign artifact
        env:
          COSIGN_KEY: ${{ secrets.COSIGN_PRIVATE_KEY }}
        run: |
          docker build -t myapp:${{ github.sha }} .
          cosign sign --key env://COSIGN_KEY myapp:${{ github.sha }}

      # Compliance report
      - name: Generate compliance report
        if: always()
        run: |
          echo "=== Compliance Report ===" > compliance-report.txt
          echo "Date: $(date -u +%Y-%m-%dT%H:%M:%SZ)" >> compliance-report.txt
          echo "Pipeline Run: ${{ github.run_id }}" >> compliance-report.txt
          cat audit.log >> compliance-report.txt

      - name: Archive compliance report
        uses: actions/upload-artifact@v4
        with:
          name: compliance-report
          path: compliance-report.txt
          retention-days: 365  # SOC2: retain for 1 year
```

### Branch Protection for Compliance

```yaml
# .github/branch-protection.yml (applied via GitHub API or settings)
# main branch protection rules:
# - Require PR reviews (min 2 for SOC2)
# - Require status checks (lint, test, security)
# - Require signed commits
# - Require linear history
# - Include administrators
# - Restrict force pushes
```

---

## Error Handling

### Retry Strategies

```yaml
# GitHub Actions - Retry flaky jobs
jobs:
  test:
    runs-on: ubuntu-latest
    timeout-minutes: 15
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-node@v4
        with:
          node-version: 20
          cache: npm
      - run: npm ci

      # Retry with nick-fields/retry
      - uses: nick-fields/retry@v3
        with:
          timeout_minutes: 10
          max_attempts: 3
          retry_wait_seconds: 30
          command: npm run test:integration -- --ci
          on_retry_command: echo "Retrying integration tests..."

      # Retry flaky E2E tests
      - name: E2E Tests with retry
        run: |
          for i in 1 2 3; do
            npm run test:e2e -- --ci && break
            if [ $i -eq 3 ]; then
              echo "E2E tests failed after 3 attempts"
              exit 1
            fi
            echo "Attempt $i failed, retrying in 30s..."
            sleep 30
          done
```

### Failure Notifications

```yaml
jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      # ... deployment steps ...

  notify:
    runs-on: ubuntu-latest
    needs: [deploy]
    if: always()
    steps:
      # Slack notification on failure
      - name: Notify failure
        if: needs.deploy.result == 'failure'
        uses: slackapi/slack-github-action@v1
        with:
          payload: |
            {
              "text": "Pipeline Failed",
              "blocks": [
                {
                  "type": "section",
                  "text": {
                    "type": "mrkdwn",
                    "text": ":x: *Pipeline Failed*\n*Repo:* ${{ github.repository }}\n*Branch:* ${{ github.ref_name }}\n*Actor:* ${{ github.actor }}\n*Commit:* `${{ github.sha }}`\n<${{ github.server_url }}/${{ github.repository }}/actions/runs/${{ github.run_id }}|View Run>"
                  }
                }
              ]
            }
        env:
          SLACK_WEBHOOK_URL: ${{ secrets.SLACK_WEBHOOK_URL }}

      # PagerDuty on production failure
      - name: PagerDuty alert
        if: needs.deploy.result == 'failure' && github.ref == 'refs/heads/main'
        run: |
          curl -X POST https://events.pagerduty.com/v2/enqueue \
            -H 'Content-Type: application/json' \
            -d '{
              "routing_key": "${{ secrets.PAGERDUTY_KEY }}",
              "event_action": "trigger",
              "payload": {
                "summary": "Production deployment failed for ${{ github.repository }}",
                "severity": "critical",
                "source": "github-actions"
              }
            }'

      # Email notification (GitHub native)
      - name: Email on failure
        if: needs.deploy.result == 'failure'
        uses: dawidd6/action-send-mail@v3
        with:
          server_address: smtp.gmail.com
          server_port: 587
          username: ${{ secrets.EMAIL_USER }}
          password: ${{ secrets.EMAIL_PASS }}
          subject: "Pipeline Failed: ${{ github.repository }}"
          to: team@example.com
          body: "Pipeline failed for ${{ github.ref_name }}. Run: ${{ github.run_id }}"
```

### Automatic Rollback

```yaml
  deploy-with-rollback:
    runs-on: ubuntu-latest
    steps:
      - name: Deploy new version
        id: deploy
        run: |
          CURRENT_IMAGE=$(kubectl get deployment app -n production -o jsonpath='{.spec.template.spec.containers[0].image}')
          echo "current_image=$CURRENT_IMAGE" >> $GITHUB_OUTPUT

          kubectl set image deployment/app app=$REGISTRY/$IMAGE:${{ github.sha }} -n production
          kubectl rollout status deployment/app -n production --timeout=300s

      - name: Health check
        id: health
        run: |
          for i in $(seq 1 10); do
            STATUS=$(curl -s -o /dev/null -w "%{http_code}" https://app.example.com/health)
            if [ "$STATUS" = "200" ]; then
              echo "Health check passed"
              exit 0
            fi
            echo "Health check attempt $i failed (HTTP $STATUS)"
            sleep 10
          done
          echo "Health check failed"
          exit 1

      - name: Automatic rollback
        if: failure() && steps.deploy.outcome == 'success'
        run: |
          echo "Rolling back to ${{ steps.deploy.outputs.current_image }}"
          kubectl set image deployment/app app=${{ steps.deploy.outputs.current_image }} -n production
          kubectl rollout status deployment/app -n production --timeout=300s
          echo "Rollback complete"
```

---

## Pipeline Testing

### Testing Pipeline Code Locally

```yaml
# .github/workflows/test-pipeline.yml
name: Test Pipeline Code

on:
  pull_request:
    paths:
      - '.github/workflows/**'
      - 'Dockerfile'
      - 'Jenkinsfile'
      - '.gitlab-ci.yml'

jobs:
  validate-workflows:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      # Validate GitHub Actions syntax
      - name: Validate workflow files
        uses: docker://rhysd/actionlint:latest
        with:
          args: -color

      # YAML linting
      - name: YAML lint
        uses: ibiqlik/action-yamllint@v3
        with:
          file_or_dir: .github/workflows/
          config_file: .yamllint.yml

      # Validate Dockerfile
      - name: Lint Dockerfile
        uses: hadolint/hadolint-action@v3.1.0
        with:
          dockerfile: Dockerfile
          failure-threshold: warning
```

### Shell-based Pipeline Validation

```bash
#!/bin/bash
# scripts/validate-pipeline.sh

set -euo pipefail

echo "=== Validating CI/CD Configuration ==="

# Validate GitHub Actions workflows
if [ -d ".github/workflows" ]; then
  echo "Validating GitHub Actions workflows..."
  for file in .github/workflows/*.yml; do
    echo "  Checking: $file"
    python3 -c "import yaml; yaml.safe_load(open('$file'))" || {
      echo "  ERROR: Invalid YAML in $file"
      exit 1
    }
  done
fi

# Validate GitLab CI
if [ -f ".gitlab-ci.yml" ]; then
  echo "Validating GitLab CI config..."
  docker run --rm -v "$(pwd):/project" \
    gitlab/gitlab-runner:latest \
    verify --executor docker || echo "Warning: validation requires GitLab connection"
fi

# Validate Jenkinsfile
if [ -f "Jenkinsfile" ]; then
  echo "Validating Jenkinsfile..."
  curl -X POST -F "jenkinsfile=<Jenkinsfile" \
    https://pipeline-syntax.dev.jenkins.io/declarative-validator/validate
fi

# Validate CircleCI
if [ -f ".circleci/config.yml" ]; then
  echo "Validating CircleCI config..."
  curl -X POST \
    -H "Content-Type: application/json" \
    -d "{\"config\": $(jq -Rs . .circleci/config.yml)}" \
    https://circleci.com/api/v2/pipeline/validate
fi

echo "=== Validation Complete ==="
```

---

## GitOps Workflow

### ArgoCD Configuration

```yaml
# argocd/application.yml
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: myapp
  namespace: argocd
  finalizers:
    - resources-finalizer.argocd.argoproj.io
spec:
  project: default
  source:
    repoURL: https://github.com/org/myapp.git
    targetRevision: main
    path: k8s/overlays/production
    kustomize:
      images:
        - myapp=registry.example.com/myapp
  destination:
    server: https://kubernetes.default.svc
    namespace: production
  syncPolicy:
    automated:
      prune: true
      selfHeal: true
      allowEmpty: false
    syncOptions:
      - CreateNamespace=true
      - PrunePropagationPolicy=foreground
      - PruneLast=true
    retry:
      limit: 3
      backoff:
        duration: 5s
        factor: 2
        maxDuration: 3m
```

### Flux CD Configuration

```yaml
# flux/git-repository.yml
apiVersion: source.toolkit.fluxcd.io/v1
kind: GitRepository
metadata:
  name: myapp
  namespace: flux-system
spec:
  interval: 1m
  url: https://github.com/org/myapp.git
  ref:
    branch: main
  secretRef:
    name: github-credentials
---
# flux/kustomization.yml
apiVersion: kustomize.toolkit.fluxcd.io/v1
kind: Kustomization
metadata:
  name: myapp
  namespace: flux-system
spec:
  interval: 5m
  path: ./k8s/production
  prune: true
  sourceRef:
    kind: GitRepository
    name: myapp
  healthChecks:
    - apiVersion: apps/v1
      kind: Deployment
      name: myapp
      namespace: production
  timeout: 3m
---
# flux/image-automation.yml
apiVersion: image.toolkit.fluxcd.io/v1beta2
kind: ImageRepository
metadata:
  name: myapp
  namespace: flux-system
spec:
  image: registry.example.com/myapp
  interval: 1m
  secretRef:
    name: registry-credentials
---
apiVersion: image.toolkit.fluxcd.io/v1beta2
kind: ImagePolicy
metadata:
  name: myapp
  namespace: flux-system
spec:
  imageRepositoryRef:
    name: myapp
  policy:
    semver:
      range: '>=1.0.0'
```

---

## Progressive Delivery

### Canary Deployment with Flagger

```yaml
# flagger/canary.yml
apiVersion: flagger.app/v1beta1
kind: Canary
metadata:
  name: myapp
  namespace: production
spec:
  targetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: myapp
  service:
    port: 3000
    targetPort: 3000
  analysis:
    interval: 1m
    threshold: 5
    maxWeight: 50
    stepWeight: 10
    metrics:
      - name: request-success-rate
        thresholdRange:
          min: 99
        interval: 1m
      - name: request-duration
        thresholdRange:
          max: 500
        interval: 1m
      - name: error-rate
        thresholdRange:
          max: 1
        interval: 1m
    alerts:
      - name: slack
        severity: warn
        providerRef:
          name: slack
          namespace: flagger
    webhooks:
      - name: load-test
        type: rollout
        url: http://flagger-loadtester.test/
        metadata:
          cmd: "hey -z 2m -q 10 -c 2 http://myapp.production:3000/"
```

### Feature Flags in CI/CD

```yaml
# Feature flag controlled deployment
jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Deploy with feature flags
        env:
          LAUNCHDARKLY_SDK_KEY: ${{ secrets.LD_SDK_KEY }}
          FEATURE_NEW_UI: ${{ vars.FEATURE_NEW_UI }}
          FEATURE_ENHANCED_API: ${{ vars.FEATURE_ENHANCED_API }}
        run: |
          kubectl create configmap feature-flags \
            --from-literal=FEATURE_NEW_UI=$FEATURE_NEW_UI \
            --from-literal=FEATURE_ENHANCED_API=$FEATURE_ENHANCED_API \
            -n production --dry-run=client -o yaml | kubectl apply -f -

          kubectl rollout restart deployment/myapp -n production
```

---

## Pipeline Visualization & Debugging

### Debug Mode Configuration

```yaml
# GitHub Actions - Debug mode
on:
  workflow_dispatch:
    inputs:
      debug_enabled:
        description: 'Enable debug mode'
        type: boolean
        default: false

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      # Enable tmate session for debugging
      - name: Setup tmate session
        if: ${{ github.event_name == 'workflow_dispatch' && inputs.debug_enabled }}
        uses: mxschmitt/action-tmate@v3
        with:
          limit-access-to-actor: true

      # Debug step output
      - name: Debug information
        if: inputs.debug_enabled
        run: |
          echo "=== Environment ==="
          env | sort
          echo "=== Disk ==="
          df -h
          echo "=== Memory ==="
          free -h
          echo "=== Git info ==="
          git log --oneline -5
```

### Pipeline Dependency Graph

```yaml
# GitHub Actions - visualize job dependencies
jobs:
  # ── Layer 0: Independent ──
  install:
    runs-on: ubuntu-latest

  # ── Layer 1: Depends on install ──
  lint:
    needs: [install]
  typecheck:
    needs: [install]
  unit-test:
    needs: [install]

  # ── Layer 2: Depends on layer 1 ──
  integration-test:
    needs: [lint, typecheck]
  build:
    needs: [lint, typecheck, unit-test]

  # ── Layer 3: Depends on layer 2 ──
  security-scan:
    needs: [build]
  deploy-staging:
    needs: [build, integration-test]

  # ── Layer 4: Depends on layer 3 ──
  deploy-production:
    needs: [security-scan, deploy-staging]
```

---

## Cross-References

- [GitHub Actions](./01-github-actions.md) — GitHub Actions detailed configuration
- [GitLab, Jenkins, Azure DevOps & Others](./02-gitlab-jenkins-azure-devops.md) — alternative CI/CD platforms
- [Testing Strategies](../09-testing/) — test frameworks and patterns
- [Security & Rate Limiting](../19-security-rate-limiting/) — application security
- [Docker & Kubernetes](../10-deployment/) — container deployment
- [Logging & Monitoring](../21-logging-monitoring/) — observability integration
- [Email & Notifications](../24-email-sending/) — notification channels
