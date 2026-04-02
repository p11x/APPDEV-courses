# CI/CD with GitHub Actions

## Overview

GitHub Actions automates testing, building, and deploying FastAPI applications. This guide covers complete CI/CD pipeline setup.

## Basic Workflow

### Simple CI Pipeline

```yaml
# Example 1: .github/workflows/ci.yml
name: CI

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v4

    - name: Set up Python
      uses: actions/setup-python@v5
      with:
        python-version: '3.11'

    - name: Install dependencies
      run: |
        python -m pip install --upgrade pip
        pip install -r requirements.txt
        pip install pytest pytest-cov

    - name: Run tests
      run: |
        pytest --cov=app --cov-report=xml

    - name: Upload coverage
      uses: codecov/codecov-action@v3
      with:
        file: ./coverage.xml
```

## Complete CI/CD Pipeline

### Testing, Building, Deploying

```yaml
# Example 2: Complete CI/CD pipeline
name: CI/CD Pipeline

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

env:
  REGISTRY: ghcr.io
  IMAGE_NAME: ${{ github.repository }}

jobs:
  test:
    runs-on: ubuntu-latest

    services:
      postgres:
        image: postgres:15
        env:
          POSTGRES_USER: test
          POSTGRES_PASSWORD: test
          POSTGRES_DB: test_db
        ports:
          - 5432:5432
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5

      redis:
        image: redis:7
        ports:
          - 6379:6379

    steps:
    - uses: actions/checkout@v4

    - name: Set up Python
      uses: actions/setup-python@v5
      with:
        python-version: '3.11'

    - name: Cache pip
      uses: actions/cache@v3
      with:
        path: ~/.cache/pip
        key: ${{ runner.os }}-pip-${{ hashFiles('requirements.txt') }}

    - name: Install dependencies
      run: |
        pip install -r requirements.txt
        pip install pytest pytest-cov pytest-asyncio httpx

    - name: Run linting
      run: |
        pip install ruff
        ruff check app/

    - name: Run type checking
      run: |
        pip install mypy
        mypy app/ --ignore-missing-imports

    - name: Run tests
      env:
        DATABASE_URL: postgresql://test:test@localhost:5432/test_db
        REDIS_URL: redis://localhost:6379
      run: |
        pytest --cov=app --cov-report=xml -v

    - name: Upload coverage
      uses: codecov/codecov-action@v3
      with:
        file: ./coverage.xml

  build:
    needs: test
    runs-on: ubuntu-latest
    if: github.event_name == 'push' && github.ref == 'refs/heads/main'

    permissions:
      contents: read
      packages: write

    steps:
    - uses: actions/checkout@v4

    - name: Log in to Container Registry
      uses: docker/login-action@v3
      with:
        registry: ${{ env.REGISTRY }}
        username: ${{ github.actor }}
        password: ${{ secrets.GITHUB_TOKEN }}

    - name: Extract metadata
      id: meta
      uses: docker/metadata-action@v5
      with:
        images: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}
        tags: |
          type=sha
          type=ref,event=branch
          latest

    - name: Build and push Docker image
      uses: docker/build-push-action@v5
      with:
        context: .
        push: true
        tags: ${{ steps.meta.outputs.tags }}
        labels: ${{ steps.meta.outputs.labels }}

  deploy:
    needs: build
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    environment: production

    steps:
    - name: Deploy to production
      run: |
        echo "Deploying to production..."
        # Add deployment commands here
```

## Database Migrations

### CI with Migrations

```yaml
# Example 3: Pipeline with database migrations
name: Deploy with Migrations

on:
  push:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    # ... test steps from Example 2

  deploy:
    needs: test
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v4

    - name: Set up Python
      uses: actions/setup-python@v5
      with:
        python-version: '3.11'

    - name: Install dependencies
      run: |
        pip install alembic psycopg2-binary

    - name: Run database migrations
      env:
        DATABASE_URL: ${{ secrets.DATABASE_URL }}
      run: |
        alembic upgrade head

    - name: Deploy application
      run: |
        # Deployment commands
        kubectl apply -f k8s/
```

## Security Scanning

### Security in CI

```yaml
# Example 4: Security scanning pipeline
name: Security Scan

on:
  push:
    branches: [main]
  schedule:
    - cron: '0 0 * * 0'  # Weekly

jobs:
  security:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v4

    - name: Run Bandit security linter
      run: |
        pip install bandit
        bandit -r app/ -f json -o bandit-report.json

    - name: Run Safety check
      run: |
        pip install safety
        safety check --json --output safety-report.json

    - name: Run Trivy vulnerability scanner
      uses: aquasecurity/trivy-action@master
      with:
        scan-type: 'fs'
        scan-ref: '.'
        format: 'sarif'
        output: 'trivy-results.sarif'

    - name: Upload Trivy scan results
      uses: github/codeql-action/upload-sarif@v2
      with:
        sarif_file: 'trivy-results.sarif'
```

## Environment Secrets

### Managing Secrets

```yaml
# Example 5: Using secrets in workflow
name: Deploy

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    environment: production

    steps:
    - uses: actions/checkout@v4

    - name: Configure AWS credentials
      uses: aws-actions/configure-aws-credentials@v4
      with:
        aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
        aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
        aws-region: us-east-1

    - name: Deploy to ECS
      run: |
        aws ecs update-service \
          --cluster my-cluster \
          --service my-service \
          --force-new-deployment
```

## Matrix Testing

### Testing Multiple Versions

```yaml
# Example 6: Matrix testing
name: Test Matrix

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        python-version: ['3.9', '3.10', '3.11', '3.12']
        database: ['postgresql', 'sqlite']

    steps:
    - uses: actions/checkout@v4

    - name: Set up Python ${{ matrix.python-version }}
      uses: actions/setup-python@v5
      with:
        python-version: ${{ matrix.python-version }}

    - name: Install dependencies
      run: |
        pip install -r requirements.txt
        pip install pytest

    - name: Run tests
      env:
        DATABASE_TYPE: ${{ matrix.database }}
      run: |
        pytest -v
```

## Caching

### Speeding Up Pipelines

```yaml
# Example 7: Pipeline caching
name: CI with Caching

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v4

    - name: Set up Python
      uses: actions/setup-python@v5
      with:
        python-version: '3.11'

    - name: Cache pip packages
      uses: actions/cache@v3
      with:
        path: ~/.cache/pip
        key: ${{ runner.os }}-pip-${{ hashFiles('requirements.txt') }}
        restore-keys: |
          ${{ runner.os }}-pip-

    - name: Cache pre-commit
      uses: actions/cache@v3
      with:
        path: ~/.cache/pre-commit
        key: ${{ runner.os }}-pre-commit-${{ hashFiles('.pre-commit-config.yaml') }}

    - name: Install dependencies
      run: |
        pip install -r requirements.txt
```

## Best Practices

### CI/CD Guidelines

```yaml
# Example 8: Best practices workflow
name: Production CI/CD

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true

jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    - uses: actions/setup-python@v5
      with:
        python-version: '3.11'
    - run: |
        pip install ruff mypy
        ruff check app/
        mypy app/ --ignore-missing-imports

  test:
    needs: lint
    runs-on: ubuntu-latest
    strategy:
      matrix:
        python-version: ['3.10', '3.11']
    steps:
    - uses: actions/checkout@v4
    - uses: actions/setup-python@v5
      with:
        python-version: ${{ matrix.python-version }}
    - run: |
        pip install -r requirements.txt
        pip install pytest pytest-cov
        pytest --cov=app -v

  build:
    needs: test
    if: github.event_name == 'push'
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    - uses: docker/build-push-action@v5
      with:
        push: true
        tags: myapp:${{ github.sha }}

  deploy:
    needs: build
    if: github.ref == 'refs/heads/main'
    runs-on: ubuntu-latest
    environment: production
    steps:
    - name: Deploy
      run: echo "Deploying..."
```

## Summary

| Stage | Purpose | Tools |
|-------|---------|-------|
| Lint | Code quality | ruff, mypy |
| Test | Verify functionality | pytest |
| Build | Create artifacts | Docker |
| Deploy | Release | kubectl, AWS CLI |

## Next Steps

Continue learning about:
- [GitLab CI](./03_gitlab_ci.md) - GitLab pipelines
- [Cloud Deployment](../03_cloud_deployment/01_aws_deployment.md) - AWS deployment
- [Monitoring](../05_monitoring_and_observability/01_monitoring_overview.md)
