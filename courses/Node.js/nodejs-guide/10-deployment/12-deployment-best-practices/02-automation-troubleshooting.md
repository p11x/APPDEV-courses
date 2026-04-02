# Deployment Automation & Troubleshooting

> **Chapter:** 10 - Deployment | **Section:** 12 - Deployment Best Practices
>
> **Previous:** [01 - Deployment Checklist](./01-deployment-checklist.md)
>
> **Next:** [03 - Capacity Planning & Cost Improvement](./03-capacity-cost-improvement.md)

---

## Table of Contents

1. [Deployment Automation](#deployment-automation)
2. [Deployment Scripting Patterns](#deployment-scripting-patterns)
3. [Automated Rollback Scripts](#automated-rollback-scripts)
4. [Database Migration Automation](#database-migration-automation)
5. [Deployment Documentation Standards](#deployment-documentation-standards)
6. [Deployment Notification Systems](#deployment-notification-systems)
7. [Deployment Approval Workflows](#deployment-approval-workflows)
8. [Troubleshooting Guide](#troubleshooting-guide)
9. [Debugging Production Issues](#debugging-production-issues)
10. [Log Analysis for Deployment Issues](#log-analysis-for-deployment-issues)
11. [Troubleshooting Decision Trees](#troubleshooting-decision-trees)
12. [Cross-References](#cross-references)

---

## Deployment Automation

### Shell Scripts for Common Deployment Tasks

```bash
#!/bin/bash
# deploy.sh - Main deployment orchestrator
set -euo pipefail

# Configuration
APP_NAME="nodejs-app"
DEPLOY_ENV="${1:-staging}"
VERSION="${2:-latest}"
DEPLOY_DIR="/opt/apps/${APP_NAME}"
LOG_FILE="/var/log/deploy/${APP_NAME}-$(date +%Y%m%d-%H%M%S).log"
HEALTH_CHECK_URL="http://localhost:3000/health"
HEALTH_CHECK_RETRIES=30
HEALTH_CHECK_INTERVAL=2

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

log() { echo -e "[$(date +'%Y-%m-%d %H:%M:%S')] $*" | tee -a "$LOG_FILE"; }
log_success() { log "${GREEN}[SUCCESS]${NC} $*"; }
log_error() { log "${RED}[ERROR]${NC} $*"; }
log_warn() { log "${YELLOW}[WARNING]${NC} $*"; }

# Pre-deployment checks
pre_deploy_checks() {
    log "Running pre-deployment checks..."

    # Check disk space (require at least 2GB free)
    local available_space=$(df -BG "$DEPLOY_DIR" | awk 'NR==2 {print $4}' | tr -d 'G')
    if [ "$available_space" -lt 2 ]; then
        log_error "Insufficient disk space. Available: ${available_space}GB, Required: 2GB"
        return 1
    fi

    # Check if the target version exists
    if [ "$VERSION" != "latest" ]; then
        if ! docker manifest inspect "${APP_NAME}:${VERSION}" &>/dev/null; then
            log_error "Image version ${VERSION} not found in registry"
            return 1
        fi
    fi

    # Verify environment configuration
    if [ ! -f "${DEPLOY_DIR}/.env.${DEPLOY_ENV}" ]; then
        log_error "Environment file .env.${DEPLOY_ENV} not found"
        return 1
    fi

    log_success "Pre-deployment checks passed"
}

# Pull and verify the new image
pull_image() {
    log "Pulling image ${APP_NAME}:${VERSION}..."
    if ! docker pull "${APP_NAME}:${VERSION}"; then
        log_error "Failed to pull image"
        return 1
    fi

    # Verify image integrity
    local image_id=$(docker inspect --format='{{.Id}}' "${APP_NAME}:${VERSION}")
    log "Image ID: ${image_id}"
    log_success "Image pulled successfully"
}

# Deploy with zero-downtime
deploy() {
    log "Deploying ${APP_NAME}:${VERSION} to ${DEPLOY_ENV}..."

    # Get current running container
    local current_container=$(docker ps -q -f "name=${APP_NAME}-blue")
    local target_slot="green"

    if [ -n "$current_container" ]; then
        local current_image=$(docker inspect --format='{{.Config.Image}}' "$current_container")
        if [[ "$current_image" == *"blue"* ]]; then
            target_slot="green"
        else
            target_slot="blue"
        fi
    fi

    log "Target deployment slot: ${target_slot}"

    # Start new container on alternate port
    docker run -d \
        --name "${APP_NAME}-${target_slot}" \
        --env-file "${DEPLOY_DIR}/.env.${DEPLOY_ENV}" \
        -p "${target_slot}":3000 \
        "${APP_NAME}:${VERSION}"

    # Health check on new container
    if ! health_check "http://localhost:${target_slot}"; then
        log_error "Health check failed on new container"
        docker stop "${APP_NAME}-${target_slot}"
        docker rm "${APP_NAME}-${target_slot}"
        return 1
    fi

    # Switch traffic
    update_load_balancer "$target_slot"

    # Stop old container (keep for rollback)
    if [ -n "$current_container" ]; then
        local old_slot=$( [ "$target_slot" = "blue" ] && echo "green" || echo "blue" )
        docker stop "${APP_NAME}-${old_slot}" || true
        log "Old container stopped: ${APP_NAME}-${old_slot}"
    fi

    log_success "Deployment completed successfully"
}

# Health check function
health_check() {
    local url="${1:-$HEALTH_CHECK_URL}"
    local retry=0

    log "Running health checks against ${url}..."

    while [ $retry -lt $HEALTH_CHECK_RETRIES ]; do
        local response=$(curl -s -o /dev/null -w "%{http_code}" "$url" 2>/dev/null || echo "000")

        if [ "$response" = "200" ]; then
            log_success "Health check passed (attempt $((retry + 1)))"
            return 0
        fi

        retry=$((retry + 1))
        log "Health check attempt $retry/$HEALTH_CHECK_RETRIES - HTTP status: $response"
        sleep $HEALTH_CHECK_INTERVAL
    done

    log_error "Health check failed after $HEALTH_CHECK_RETRIES attempts"
    return 1
}

# Update load balancer configuration
update_load_balancer() {
    local active_slot="$1"
    log "Updating load balancer to route traffic to ${active_slot}..."

    # Update nginx upstream configuration
    cat > /etc/nginx/conf.d/upstream.conf << EOF
upstream app_backend {
    server 127.0.0.1:${active_slot};
}
EOF

    nginx -t && nginx -s reload
    log_success "Load balancer updated"
}

# Main execution flow
main() {
    log "========== Deployment Started =========="
    log "Environment: ${DEPLOY_ENV}"
    log "Version: ${VERSION}"
    log "Timestamp: $(date -u +%Y-%m-%dT%H:%M:%SZ)"

    pre_deploy_checks
    pull_image
    deploy

    log "========== Deployment Completed =========="
}

main "$@"
```

### Makefile for Deployment Commands

```makefile
# Makefile - Deployment automation commands
.PHONY: help deploy deploy-staging deploy-prod rollback health-check logs clean

SHELL := /bin/bash
APP_NAME := nodejs-app
DOCKER_REGISTRY := registry.example.com
VERSION ?= $(shell git describe --tags --always)
DEPLOY_ENV ?= staging

# Default target
help: ## Show this help message
	@echo "Usage: make [target]"
	@echo ""
	@echo "Targets:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | \
		awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-20s\033[0m %s\n", $$1, $$2}'

# Build targets
build: ## Build Docker image
	docker build -t $(APP_NAME):$(VERSION) .
	docker tag $(APP_NAME):$(VERSION) $(DOCKER_REGISTRY)/$(APP_NAME):$(VERSION)

push: build ## Push image to registry
	docker push $(DOCKER_REGISTRY)/$(APP_NAME):$(VERSION)

# Deployment targets
deploy-staging: ## Deploy to staging environment
	./scripts/deploy.sh staging $(VERSION)

deploy-prod: approval-required ## Deploy to production environment
	./scripts/deploy.sh production $(VERSION)

deploy-canary: ## Deploy canary release (10% traffic)
	./scripts/canary-deploy.sh $(VERSION) 10

promote-canary: ## Promote canary to 100% traffic
	./scripts/canary-promote.sh

# Rollback targets
rollback: ## Rollback to previous version
	./scripts/rollback.sh $(DEPLOY_ENV)

rollback-to: ## Rollback to specific version (VERSION=x.y.z)
	./scripts/rollback.sh $(DEPLOY_ENV) $(VERSION)

# Monitoring targets
health-check: ## Run health checks
	@curl -s http://localhost:3000/health | jq .

logs: ## Tail application logs
	docker logs -f --tail=100 $(APP_NAME)-blue

metrics: ## Show deployment metrics
	./scripts/deployment-metrics.sh

# Database targets
db-migrate: ## Run database migrations
	npx prisma migrate deploy

db-migrate-status: ## Check migration status
	npx prisma migrate status

db-rollback: ## Rollback last migration
	npx prisma migrate resolve --rolled-back

# Cleanup targets
clean: ## Remove old Docker images and containers
	docker system prune -f
	docker image prune -af --filter "until=72h"

clean-deep: ## Deep clean including volumes
	docker system prune -af --volumes

# Quality gates
approval-required:
	@if [ "$(DEPLOY_ENV)" = "production" ] && [ -z "$$SKIP_APPROVAL" ]; then \
		echo "Production deployment requires approval."; \
		read -p "Type 'DEPLOY' to confirm: " confirm; \
		if [ "$$confirm" != "DEPLOY" ]; then \
			echo "Deployment cancelled."; \
			exit 1; \
		fi; \
	fi

# Composite targets
full-deploy: push deploy-staging smoke-test deploy-prod ## Full deployment pipeline

smoke-test: ## Run smoke tests against staging
	./scripts/smoke-test.sh http://staging.example.com
```

---

## Deployment Scripting Patterns

### Idempotent Deployment Script

```bash
#!/bin/bash
# idempotent-deploy.sh - Safe to run multiple times
set -euo pipefail

APP_DIR="/opt/apps/myapp"
LOCK_FILE="/tmp/deploy-myapp.lock"
STATE_FILE="${APP_DIR}/.deploy-state"

# Ensure single execution
acquire_lock() {
    exec 200>"$LOCK_FILE"
    if ! flock -n 200; then
        echo "Another deployment is in progress. PID: $(cat "$LOCK_FILE")"
        exit 1
    fi
    echo $$ > "$LOCK_FILE"
    trap 'release_lock' EXIT
}

release_lock() {
    rm -f "$LOCK_FILE"
    exec 200>&-
}

# State management for resumable deployments
get_state() {
    if [ -f "$STATE_FILE" ]; then
        cat "$STATE_FILE"
    else
        echo "init"
    fi
}

set_state() {
    echo "$1" > "$STATE_FILE"
}

# Idempotent service management
ensure_service_running() {
    local service="$1"

    if systemctl is-active --quiet "$service"; then
        echo "Service ${service} already running - skipping start"
        return 0
    fi

    echo "Starting service ${service}..."
    systemctl start "$service"
    systemctl enable "$service"
}

# Idempotent directory creation
ensure_directory() {
    local dir="$1"
    local owner="${2:-root:root}"
    local mode="${3:-755}"

    if [ -d "$dir" ]; then
        echo "Directory ${dir} exists - ensuring permissions"
    else
        echo "Creating directory ${dir}"
    fi

    mkdir -p "$dir"
    chown "$owner" "$dir"
    chmod "$mode" "$dir"
}

# Idempotent file download
ensure_file() {
    local url="$1"
    local dest="$2"
    local checksum="$3"

    if [ -f "$dest" ]; then
        local current_checksum=$(sha256sum "$dest" | cut -d' ' -f1)
        if [ "$current_checksum" = "$checksum" ]; then
            echo "File ${dest} already up to date"
            return 0
        fi
    fi

    echo "Downloading ${url} to ${dest}..."
    curl -fsSL "$url" -o "$dest"

    local new_checksum=$(sha256sum "$dest" | cut -d' ' -f1)
    if [ "$new_checksum" != "$checksum" ]; then
        echo "Checksum verification failed"
        return 1
    fi
}

# Idempotent npm install
ensure_dependencies() {
    cd "$APP_DIR"

    local lock_checksum=$(sha256sum package-lock.json | cut -d' ' -f1)
    local stored_checksum=""

    if [ -f ".deps-checksum" ]; then
        stored_checksum=$(cat ".deps-checksum")
    fi

    if [ "$lock_checksum" = "$stored_checksum" ] && [ -d "node_modules" ]; then
        echo "Dependencies up to date - skipping install"
        return 0
    fi

    echo "Installing dependencies..."
    npm ci --production
    echo "$lock_checksum" > ".deps-checksum"
}

# Resumable deployment flow
resume_deploy() {
    local current_state=$(get_state)
    echo "Resuming deployment from state: ${current_state}"

    case "$current_state" in
        init)
            set_state "backing-up"
            ;&  # Fall through intentionally
        backing-up)
            backup_current_version
            set_state "pulling-image"
            ;&
        pulling-image)
            pull_new_image
            set_state "installing-deps"
            ;&
        installing-deps)
            ensure_dependencies
            set_state "migrating-db"
            ;&
        migrating-db)
            run_migrations
            set_state "updating-config"
            ;&
        updating-config)
            update_configuration
            set_state "restarting-service"
            ;&
        restarting-service)
            ensure_service_running "myapp"
            set_state "verifying"
            ;&
        verifying)
            verify_deployment
            set_state "complete"
            ;&
        complete)
            echo "Deployment complete"
            rm -f "$STATE_FILE"
            ;;
        *)
            echo "Unknown state: ${current_state}"
            exit 1
            ;;
    esac
}

main() {
    acquire_lock
    resume_deploy
}

main "$@"
```

### Error Handling Pattern

```bash
#!/bin/bash
# error-handling-pattern.sh - Robust error handling for deployments

set -euo pipefail
IFS=$'\n\t'

# Global error tracking
ERRORS=()
WARNINGS=()

# Error trap with context
trap 'error_handler ${LINENO} $?' ERR

error_handler() {
    local line=$1
    local code=$2
    local command="${BASH_COMMAND}"
    local script="${BASH_SOURCE[1]}"

    ERRORS+=("Line ${line}: Command '${command}' exited with code ${code}")

    log_error "Error at ${script}:${line}"
    log_error "Command: ${command}"
    log_error "Exit code: ${code}"

    # Capture context (10 lines around error)
    if [ -f "$script" ]; then
        log_error "Context:"
        awk -v line="$line" 'NR>=line-5 && NR<=line+5 {
            printf "  %s%d: %s\n", (NR==line ? ">>" : "  "), NR, $0
        }' "$script"
    fi

    # Trigger cleanup
    cleanup_on_error
}

cleanup_on_error() {
    log_warn "Running error cleanup..."

    # Stop partial deployments
    if [ -n "${NEW_CONTAINER:-}" ]; then
        docker stop "$NEW_CONTAINER" 2>/dev/null || true
        docker rm "$NEW_CONTAINER" 2>/dev/null || true
    fi

    # Restore from backup if available
    if [ -f "/tmp/app-backup-${DEPLOY_VERSION}.tar.gz" ]; then
        log_warn "Restoring from backup..."
        tar -xzf "/tmp/app-backup-${DEPLOY_VERSION}.tar.gz" -C "$APP_DIR"
    fi

    # Notify about failure
    send_notification "FAILURE" "Deployment failed at line ${1}: ${BASH_COMMAND}"
}

# Retry with exponential backoff
retry_with_backoff() {
    local max_attempts=${1:-5}
    local initial_delay=${2:-1}
    local max_delay=${3:-60}
    shift 3

    local attempt=1
    local delay=$initial_delay

    while [ $attempt -le $max_attempts ]; do
        if "$@"; then
            return 0
        fi

        if [ $attempt -eq $max_attempts ]; then
            log_error "Command failed after $max_attempts attempts: $*"
            return 1
        fi

        log_warn "Attempt $attempt/$max_attempts failed. Retrying in ${delay}s..."
        sleep $delay

        attempt=$((attempt + 1))
        delay=$((delay * 2))
        if [ $delay -gt $max_delay ]; then
            delay=$max_delay
        fi
    done
}

# Validate prerequisites before deployment
validate_prerequisites() {
    local missing=()

    command -v docker >/dev/null 2>&1 || missing+=("docker")
    command -v jq >/dev/null 2>&1 || missing+=("jq")
    command -v curl >/dev/null 2>&1 || missing+=("curl")

    if [ ${#missing[@]} -gt 0 ]; then
        log_error "Missing required tools: ${missing[*]}"
        return 1
    fi

    # Check Docker daemon
    if ! docker info >/dev/null 2>&1; then
        log_error "Docker daemon is not running"
        return 1
    fi

    log_success "All prerequisites validated"
}

# Structured logging
log_json() {
    local level="$1"
    local message="$2"
    local context="${3:-{}}"

    jq -cn \
        --arg level "$level" \
        --arg message "$message" \
        --arg timestamp "$(date -u +%Y-%m-%dT%H:%M:%SZ)" \
        --argjson context "$context" \
        '{timestamp: $timestamp, level: $level, message: $message, context: $context}'
}

log_error() { echo "[ERROR] $(date +'%Y-%m-%d %H:%M:%S') $*" >&2; }
log_warn() { echo "[WARN] $(date +'%Y-%m-%d %H:%M:%S') $*" ; WARNINGS+=("$*"); }
log_info() { echo "[INFO] $(date +'%Y-%m-%d %H:%M:%S') $*"; }
log_success() { echo "[SUCCESS] $(date +'%Y-%m-%d %H:%M:%S') $*"; }

# Final report
deployment_report() {
    echo ""
    echo "========== Deployment Report =========="
    echo "Duration: ${SECONDS}s"
    echo "Warnings: ${#WARNINGS[@]}"
    echo "Errors: ${#ERRORS[@]}"

    if [ ${#WARNINGS[@]} -gt 0 ]; then
        echo ""
        echo "Warnings:"
        printf '  - %s\n' "${WARNINGS[@]}"
    fi

    if [ ${#ERRORS[@]} -gt 0 ]; then
        echo ""
        echo "Errors:"
        printf '  - %s\n' "${ERRORS[@]}"
    fi

    echo "========================================"

    if [ ${#ERRORS[@]} -gt 0 ]; then
        return 1
    fi
    return 0
}
```

---

## Automated Rollback Scripts

```bash
#!/bin/bash
# rollback.sh - Automated rollback with health verification
set -euo pipefail

DEPLOY_ENV="${1:-production}"
TARGET_VERSION="${2:-}"
APP_NAME="node-app"
APP_DIR="/opt/apps/${APP_NAME}"
DEPLOY_HISTORY="${APP_DIR}/.deploy-history"
HEALTH_CHECK_URL="http://localhost:3000/health"
MAX_HEALTH_RETRIES=20

# Deployment history format: timestamp|version|status|image_id
get_previous_version() {
    if [ -n "$TARGET_VERSION" ]; then
        echo "$TARGET_VERSION"
        return
    fi

    local last_success=$(grep "|success|" "$DEPLOY_HISTORY" | tail -2 | head -1)
    if [ -z "$last_success" ]; then
        echo "No previous successful deployment found"
        return 1
    fi

    echo "$last_success" | cut -d'|' -f2
}

# Verify rollback target exists and is valid
verify_rollback_target() {
    local version="$1"

    if ! docker image inspect "${APP_NAME}:${version}" &>/dev/null; then
        echo "Image ${APP_NAME}:${version} not found locally. Pulling..."
        docker pull "${APP_NAME}:${version}" || {
            echo "Failed to pull image ${APP_NAME}:${version}"
            return 1
        }
    fi

    echo "Rollback target verified: ${version}"
}

# Perform rollback
execute_rollback() {
    local target_version="$1"
    local current_version=$(get_current_version)

    echo "Rolling back from ${current_version} to ${target_version}..."

    # Snapshot current state for forensics
    create_failure_snapshot "$current_version"

    # Stop current container gracefully
    docker stop --time 30 "${APP_NAME}-current" || true

    # Start previous version
    docker run -d \
        --name "${APP_NAME}-rollback" \
        --env-file "${APP_DIR}/.env.${DEPLOY_ENV}" \
        -p 3000:3000 \
        "${APP_NAME}:${target_version}"

    # Health verification
    if ! verify_health; then
        echo "CRITICAL: Rollback health check failed!"
        escalate_to_oncall
        return 1
    else
        echo "Rollback health check passed"
    fi

    # Rename for consistency
    docker rename "${APP_NAME}-rollback" "${APP_NAME}-current"
    docker rm -f "${APP_NAME}-old" 2>/dev/null || true

    # Record rollback
    echo "$(date -u +%Y-%m-%dT%H:%M:%SZ)|${target_version}|rollback-success|$(docker inspect --format='{{.Id}}' "${APP_NAME}-current")" >> "$DEPLOY_HISTORY"

    echo "Rollback to ${target_version} completed successfully"
}

# Health verification
verify_health() {
    local attempts=0
    local ready=false

    echo "Verifying service health..."

    while [ $attempts -lt $MAX_HEALTH_RETRIES ] && [ "$ready" = false ]; do
        attempts=$((attempts + 1))

        local http_code=$(curl -s -o /dev/null -w "%{http_code}" "$HEALTH_CHECK_URL" 2>/dev/null || echo "000")
        local response=$(curl -s "$HEALTH_CHECK_URL" 2>/dev/null || echo "{}")

        if [ "$http_code" = "200" ]; then
            local db_status=$(echo "$response" | jq -r '.database // "unknown"')
            local cache_status=$(echo "$response" | jq -r '.cache // "unknown"')

            if [ "$db_status" = "connected" ] && [ "$cache_status" = "connected" ]; then
                ready=true
                echo "All health checks passed (attempt ${attempts})"
            else
                echo "Health endpoint up but dependencies not ready (db: ${db_status}, cache: ${cache_status})"
            fi
        else
            echo "Health check attempt ${attempts}/${MAX_HEALTH_RETRIES} - HTTP ${http_code}"
        fi

        [ "$ready" = false ] && sleep 3
    done

    $ready
}

# Create snapshot for post-mortem analysis
create_failure_snapshot() {
    local version="$1"
    local snapshot_dir="/tmp/failure-snapshot-$(date +%Y%m%d-%H%M%S)"

    mkdir -p "$snapshot_dir"

    # Capture container logs
    docker logs "${APP_NAME}-current" > "${snapshot_dir}/container.log" 2>&1 || true

    # Capture system state
    docker stats --no-stream > "${snapshot_dir}/docker-stats.txt" 2>/dev/null || true
    free -h > "${snapshot_dir}/memory.txt" 2>/dev/null || true
    df -h > "${snapshot_dir}/disk.txt" 2>/dev/null || true

    # Capture Node.js metrics if available
    curl -s http://localhost:3000/metrics > "${snapshot_dir}/metrics.json" 2>/dev/null || true

    # Save metadata
    cat > "${snapshot_dir}/metadata.json" << EOF
{
    "failedVersion": "${version}",
    "timestamp": "$(date -u +%Y-%m-%dT%H:%M:%SZ)",
    "environment": "${DEPLOY_ENV}",
    "hostname": "$(hostname)",
    "uptime": "$(uptime)"
}
EOF

    echo "Failure snapshot saved to ${snapshot_dir}"
}

# Escalation
escalate_to_oncall() {
    echo "CRITICAL: Rollback failed - escalating to on-call"
    # Integration with PagerDuty, OpsGenie, etc.
    curl -s -X POST "https://events.pagerduty.com/v2/enqueue" \
        -H "Content-Type: application/json" \
        -d "{
            \"routing_key\": \"${PAGERDUTY_KEY}\",
            \"event_action\": \"trigger\",
            \"payload\": {
                \"summary\": \"Rollback failed for ${APP_NAME} in ${DEPLOY_ENV}\",
                \"severity\": \"critical\",
                \"source\": \"deployment-system\"
            }
        }" || echo "Failed to send PagerDuty alert"
}

get_current_version() {
    docker inspect --format='{{.Config.Image}}' "${APP_NAME}-current" 2>/dev/null | cut -d':' -f2 || echo "unknown"
}

main() {
    echo "========== Rollback Initiated =========="
    echo "Environment: ${DEPLOY_ENV}"

    local target_version=$(get_previous_version) || exit 1
    echo "Target version: ${target_version}"

    verify_rollback_target "$target_version"
    execute_rollback "$target_version"

    echo "========== Rollback Complete =========="
}

main "$@"
```

---

## Database Migration Automation

### Zero-Downtime Migration with Expand-Contract Pattern

```bash
#!/bin/bash
# migrate.sh - Zero-downtime database migrations
set -euo pipefail

DB_URL="${DATABASE_URL}"
MIGRATION_DIR="./migrations"
LOCK_TABLE="_migration_lock"
MAX_LOCK_WAIT=30

# Acquire distributed lock for migration
acquire_migration_lock() {
    echo "Acquiring migration lock..."

    psql "$DB_URL" << EOF
    CREATE TABLE IF NOT EXISTS ${LOCK_TABLE} (
        id SERIAL PRIMARY KEY,
        locked_by TEXT,
        locked_at TIMESTAMPTZ
    );

    INSERT INTO ${LOCK_TABLE} (locked_by, locked_at)
    SELECT '${HOSTNAME}-$$', NOW()
    WHERE NOT EXISTS (
        SELECT 1 FROM ${LOCK_TABLE} WHERE locked_at > NOW() - INTERVAL '${MAX_LOCK_WAIT} seconds'
    );
EOF

    local has_lock=$(psql "$DB_URL" -t -c "SELECT COUNT(*) FROM ${LOCK_TABLE} WHERE locked_by = '${HOSTNAME}-$$';")

    if [ "$has_lock" -ne 1 ]; then
        echo "Could not acquire migration lock - another migration is running"
        return 1
    fi

    trap release_migration_lock EXIT
}

release_migration_lock() {
    psql "$DB_URL" -c "DELETE FROM ${LOCK_TABLE} WHERE locked_by = '${HOSTNAME}-$$';" || true
}

# Run expand phase migrations (additive only)
expand_phase() {
    echo "Running EXPAND phase migrations..."

    local expand_dir="${MIGRATION_DIR}/expand"
    if [ ! -d "$expand_dir" ]; then
        echo "No expand migrations to run"
        return 0
    fi

    for migration in $(ls -1 "$expand_dir"/*.sql 2>/dev/null | sort); do
        local migration_name=$(basename "$migration")

        if is_migration_applied "$migration_name"; then
            echo "Skipping already applied: $migration_name"
            continue
        fi

        echo "Applying expand migration: $migration_name"

        # Wrap in transaction with statement timeout
        psql "$DB_URL" << EOF
        SET statement_timeout = '30s';
        BEGIN;
        $(cat "$migration")
        INSERT INTO _migrations (name, phase, applied_at) VALUES ('${migration_name}', 'expand', NOW());
        COMMIT;
EOF

        if [ $? -eq 0 ]; then
            echo "Expand migration applied: $migration_name"
        else
            echo "Expand migration failed: $migration_name"
            return 1
        fi
    done
}

# Run data migration phase
data_phase() {
    echo "Running DATA migration phase..."

    local data_dir="${MIGRATION_DIR}/data"
    if [ ! -d "$data_dir" ]; then
        echo "No data migrations to run"
        return 0
    fi

    for migration in $(ls -1 "$data_dir"/*.sql 2>/dev/null | sort); do
        local migration_name=$(basename "$migration")

        if is_migration_applied "$migration_name"; then
            continue
        fi

        echo "Applying data migration: $migration_name"

        # Data migrations run in batches to avoid locks
        psql "$DB_URL" << EOF
        SET statement_timeout = '5min';
        BEGIN;
        $(cat "$migration")
        INSERT INTO _migrations (name, phase, applied_at) VALUES ('${migration_name}', 'data', NOW());
        COMMIT;
EOF
    done
}

# Run contract phase (removals - only after full rollout)
contract_phase() {
    echo "Running CONTRACT phase migrations..."

    local contract_dir="${MIGRATION_DIR}/contract"
    if [ ! -d "$contract_dir" ]; then
        echo "No contract migrations to run"
        return 0
    fi

    for migration in $(ls -1 "$contract_dir"/*.sql 2>/dev/null | sort); do
        local migration_name=$(basename "$migration")

        if is_migration_applied "$migration_name"; then
            continue
        fi

        echo "Applying contract migration: $migration_name"

        psql "$DB_URL" << EOF
        SET statement_timeout = '60s';
        BEGIN;
        $(cat "$migration")
        INSERT INTO _migrations (name, phase, applied_at) VALUES ('${migration_name}', 'contract', NOW());
        COMMIT;
EOF
    done
}

is_migration_applied() {
    local name="$1"
    local count=$(psql "$DB_URL" -t -c "SELECT COUNT(*) FROM _migrations WHERE name = '${name}';")
    [ "$count" -gt 0 ]
}

# Verify migration safety
verify_migration_safety() {
    echo "Verifying migration safety..."

    # Check for long-running transactions
    local long_tx=$(psql "$DB_URL" -t -c "
        SELECT COUNT(*) FROM pg_stat_activity
        WHERE state = 'active'
        AND query_start < NOW() - INTERVAL '5 minutes';
    ")

    if [ "$long_tx" -gt 0 ]; then
        echo "WARNING: ${long_tx} long-running transactions detected"
        read -p "Continue with migration? (y/N): " confirm
        [ "$confirm" = "y" ] || return 1
    fi

    # Check replication lag
    local lag=$(psql "$DB_URL" -t -c "
        SELECT COALESCE(MAX(EXTRACT(EPOCH FROM replay_lag)), 0)
        FROM pg_stat_replication;
    " 2>/dev/null || echo "0")

    if (( $(echo "$lag > 10" | bc -l) )); then
        echo "WARNING: Replication lag is ${lag}s - migrations may cause issues"
        return 1
    fi

    echo "Migration safety verified"
}

main() {
    echo "========== Database Migration =========="
    local phase="${1:-all}"

    acquire_migration_lock
    verify_migration_safety || exit 1

    case "$phase" in
        expand)  expand_phase ;;
        data)    data_phase ;;
        contract) contract_phase ;;
        all)
            expand_phase
            data_phase
            echo "NOTE: Contract phase must be run separately after full deployment"
            ;;
        *)
            echo "Unknown phase: $phase"
            exit 1
            ;;
    esac

    echo "========== Migration Complete =========="
}

main "$@"
```

---

## Deployment Documentation Standards

### Runbook Template

```markdown
# Runbook: [Service Name] Production Deployment

## Overview
- **Service:** node-api-service
- **Owner:** Platform Team
- **Last Updated:** 2026-03-15
- **Deployment Frequency:** Weekly

## Prerequisites
- [ ] All staging tests passing
- [ ] Change request approved in ServiceNow (CHGxxxxxx)
- [ ] On-call engineer notified in #deploys channel
- [ ] Database migrations reviewed and tested

## Deployment Steps

### Step 1: Pre-deployment Verification
```bash
# Verify staging is healthy
curl -s https://staging.example.com/health | jq .

# Check current production version
curl -s https://api.example.com/version
```

### Step 2: Database Migrations
```bash
# Run expand phase only
./scripts/migrate.sh expand
# Verify schema changes
./scripts/migrate.sh verify
```

### Step 3: Deploy Application
```bash
make deploy-prod VERSION=v2.5.0
```

### Step 4: Post-deployment Verification
```bash
# Health check
curl -s https://api.example.com/health | jq .

# Verify metrics
./scripts/check-metrics.sh --threshold error_rate=0.01

# Run smoke tests
./scripts/smoke-test.sh production
```

## Rollback Procedure
```bash
# Automated rollback
make rollback

# Manual rollback (if automated fails)
./scripts/rollback.sh production v2.4.9
./scripts/migrate.sh rollback
```

## Failure Scenarios

### Scenario 1: Health Check Fails
1. Check container logs: `docker logs node-app-current`
2. Verify database connectivity: `psql $DATABASE_URL -c "SELECT 1"`
3. If issue persists, initiate rollback

### Scenario 2: High Error Rate Post-deploy
1. Check error logs for patterns
2. If error rate > 5%, initiate automatic rollback
3. If 1-5%, monitor for 10 minutes, then decide

### Scenario 3: Database Migration Failure
1. Check migration lock table: `SELECT * FROM _migration_lock`
2. If stuck, manually release lock
3. Restore from pre-migration backup if needed

## Contacts
- **Primary:** platform-team@company.com
- **Escalation:** oncall@company.com
- **Slack:** #platform-oncall
```

### Architecture Decision Record (ADR) Template

```markdown
# ADR-001: Blue-Green Deployment Strategy

## Status
Accepted

## Date
2026-03-01

## Context
We need a zero-downtime deployment strategy for our Node.js API serving 10K requests/second.
Current rolling deployments cause brief periods of mixed version responses.

## Decision
We will implement blue-green deployments using Docker and nginx upstream switching.

## Alternatives Considered

### 1. Rolling Deployment
- **Pros:** Simple, resource efficient
- **Cons:** Mixed versions during rollout, no instant rollback

### 2. Canary Releases
- **Pros:** Gradual rollout, risk mitigation
- **Cons:** Complex traffic splitting, longer deployment time

### 3. Blue-Green (Chosen)
- **Pros:** Instant rollback, zero downtime, full environment testing
- **Cons:** 2x resource requirement during deployment

## Consequences
- Infrastructure cost increases ~40% during deployments
- Rollback time reduced from ~5 minutes to ~30 seconds
- Deployment confidence increased with full environment validation

## Implementation
See deployment scripts in `scripts/deploy.sh`

## Review Date
2026-06-01
```

---

## Deployment Notification Systems

### Slack Integration

```javascript
// notifications/slack.js
const https = require('https');

class SlackNotifier {
  constructor(webhookUrl, channel = '#deployments') {
    this.webhookUrl = webhookUrl;
    this.channel = channel;
  }

  async sendDeploymentStart({ version, environment, deployer }) {
    return this.send({
      blocks: [
        {
          type: 'header',
          text: {
            type: 'plain_text',
            text: `:rocket: Deployment Started`,
            emoji: true,
          },
        },
        {
          type: 'section',
          fields: [
            { type: 'mrkdwn', text: `*Version:*\n${version}` },
            { type: 'mrkdwn', text: `*Environment:*\n${environment}` },
            { type: 'mrkdwn', text: `*Deployer:*\n${deployer}` },
            {
              type: 'mrkdwn',
              text: `*Time:*\n${new Date().toISOString()}`,
            },
          ],
        },
      ],
    });
  }

  async sendDeploymentComplete({ version, environment, duration, url }) {
    return this.send({
      blocks: [
        {
          type: 'header',
          text: {
            type: 'plain_text',
            text: `:white_check_mark: Deployment Complete`,
            emoji: true,
          },
        },
        {
          type: 'section',
          fields: [
            { type: 'mrkdwn', text: `*Version:*\n${version}` },
            { type: 'mrkdwn', text: `*Environment:*\n${environment}` },
            {
              type: 'mrkdwn',
              text: `*Duration:*\n${(duration / 1000).toFixed(1)}s`,
            },
            { type: 'mrkdwn', text: `*URL:*\n${url}` },
          ],
        },
      ],
    });
  }

  async sendDeploymentFailed({ version, environment, error, logs }) {
    return this.send({
      blocks: [
        {
          type: 'header',
          text: {
            type: 'plain_text',
            text: `:x: Deployment Failed`,
            emoji: true,
          },
        },
        {
          type: 'section',
          fields: [
            { type: 'mrkdwn', text: `*Version:*\n${version}` },
            { type: 'mrkdwn', text: `*Environment:*\n${environment}` },
          ],
        },
        {
          type: 'section',
          text: {
            type: 'mrkdwn',
            text: `*Error:*\n\`\`\`${error}\`\`\``,
          },
        },
        {
          type: 'actions',
          elements: [
            {
              type: 'button',
              text: { type: 'plain_text', text: 'View Logs' },
              url: logs,
              style: 'danger',
            },
            {
              type: 'button',
              text: { type: 'plain_text', text: 'Rollback' },
              action_id: 'rollback_deploy',
            },
          ],
        },
      ],
    });
  }

  async send(payload) {
    const data = JSON.stringify({
      channel: this.channel,
      ...payload,
    });

    return new Promise((resolve, reject) => {
      const url = new URL(this.webhookUrl);
      const options = {
        hostname: url.hostname,
        path: url.pathname,
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Content-Length': Buffer.byteLength(data),
        },
      };

      const req = https.request(options, (res) => {
        let body = '';
        res.on('data', (chunk) => (body += chunk));
        res.on('end', () => resolve(body));
      });

      req.on('error', reject);
      req.write(data);
      req.end();
    });
  }
}

module.exports = { SlackNotifier };
```

### Microsoft Teams Integration

```javascript
// notifications/teams.js
const https = require('https');

class TeamsNotifier {
  constructor(webhookUrl) {
    this.webhookUrl = webhookUrl;
  }

  async sendDeploymentNotification({
    title,
    version,
    environment,
    status,
    details,
  }) {
    const color =
      status === 'success'
        ? '00FF00'
        : status === 'failed'
          ? 'FF0000'
          : 'FFAA00';

    const payload = {
      '@type': 'MessageCard',
      '@context': 'https://schema.org/extensions',
      summary: title,
      themeColor: color,
      sections: [
        {
          activityTitle: title,
          facts: [
            { name: 'Version', value: version },
            { name: 'Environment', value: environment },
            { name: 'Status', value: status.toUpperCase() },
            { name: 'Time', value: new Date().toISOString() },
            ...(details || []),
          ],
        },
      ],
      potentialAction: [
        {
          '@type': 'OpenUri',
          name: 'View Dashboard',
          targets: [
            {
              os: 'default',
              uri: 'https://grafana.example.com/deployments',
            },
          ],
        },
      ],
    };

    return this.send(payload);
  }

  async send(payload) {
    const data = JSON.stringify(payload);
    const url = new URL(this.webhookUrl);

    return new Promise((resolve, reject) => {
      const options = {
        hostname: url.hostname,
        path: url.pathname + url.search,
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Content-Length': Buffer.byteLength(data),
        },
      };

      const req = https.request(options, (res) => {
        let body = '';
        res.on('data', (chunk) => (body += chunk));
        res.on('end', () => resolve(body));
      });

      req.on('error', reject);
      req.write(data);
      req.end();
    });
  }
}

module.exports = { TeamsNotifier };
```

### Deployment Orchestrator with Notifications

```javascript
// deploy-orchestrator.js
const { SlackNotifier } = require('./notifications/slack');
const { TeamsNotifier } = require('./notifications/teams');

class DeployOrchestrator {
  constructor(config) {
    this.notifiers = [];
    if (config.slack?.webhookUrl) {
      this.notifiers.push(new SlackNotifier(config.slack.webhookUrl));
    }
    if (config.teams?.webhookUrl) {
      this.notifiers.push(new TeamsNotifier(config.teams.webhookUrl));
    }
    this.config = config;
  }

  async deploy(version, environment) {
    const startTime = Date.now();

    // Notify start
    await this.notifyAll('start', { version, environment });

    try {
      await this.preDeployChecks();
      await this.pullImage(version);
      await this.runMigrations();
      await this.deployVersion(version, environment);
      await this.healthCheck();
      await this.smokeTests();

      // Notify success
      await this.notifyAll('complete', {
        version,
        environment,
        duration: Date.now() - startTime,
      });
    } catch (error) {
      // Notify failure
      await this.notifyAll('failed', {
        version,
        environment,
        error: error.message,
      });

      await this.rollback();
      throw error;
    }
  }

  async notifyAll(event, data) {
    for (const notifier of this.notifiers) {
      try {
        if (event === 'start') await notifier.sendDeploymentStart(data);
        if (event === 'complete')
          await notifier.sendDeploymentComplete(data);
        if (event === 'failed')
          await notifier.sendDeploymentFailed(data);
      } catch (err) {
        console.error('Notification failed:', err.message);
      }
    }
  }
}

module.exports = { DeployOrchestrator };
```

---

## Deployment Approval Workflows

```javascript
// approval-workflow.js
const { EventEmitter } = require('events');

class DeploymentApprovalWorkflow extends EventEmitter {
  constructor(config) {
    super();
    this.approvalGateUrl = config.approvalGateUrl;
    this.requiredApprovals = config.requiredApprovals || 1;
    this.timeoutMs = config.timeoutMs || 3600000; // 1 hour
    this.pendingApprovals = new Map();
  }

  async requestApproval(deployment) {
    const approvalId = `deploy-${deployment.version}-${Date.now()}`;

    const approvalRequest = {
      id: approvalId,
      deployment,
      requestedAt: new Date().toISOString(),
      status: 'pending',
      approvals: [],
      requiredApprovals: this.getRequiredApprovers(deployment),
    };

    this.pendingApprovals.set(approvalId, approvalRequest);

    // Send approval requests
    await this.sendApprovalNotifications(approvalRequest);

    // Wait for approvals with timeout
    return this.waitForApproval(approvalId);
  }

  getRequiredApprovers(deployment) {
    if (deployment.environment === 'production') {
      return {
        count: 2,
        approvers: ['team-lead', 'sre-oncall'],
        rules: [
          'At least one SRE approval required',
          'Cannot be self-approved',
          'Valid for 1 hour after request',
        ],
      };
    }
    return { count: 1, approvers: ['team-member'], rules: [] };
  }

  async waitForApproval(approvalId) {
    return new Promise((resolve, reject) => {
      const timeout = setTimeout(() => {
        this.pendingApprovals.delete(approvalId);
        reject(new Error('Approval request timed out'));
      }, this.timeoutMs);

      const checkInterval = setInterval(() => {
        const request = this.pendingApprovals.get(approvalId);
        if (!request) {
          clearInterval(checkInterval);
          clearTimeout(timeout);
          reject(new Error('Approval request not found'));
          return;
        }

        if (request.approvals.length >= this.requiredApprovals) {
          clearInterval(checkInterval);
          clearTimeout(timeout);
          request.status = 'approved';
          this.emit('approved', request);
          resolve(request);
        }
      }, 5000);

      this.on('approval-received', (id) => {
        if (id === approvalId) {
          const request = this.pendingApprovals.get(approvalId);
          if (
            request.approvals.length >=
            request.requiredApprovals.count
          ) {
            clearInterval(checkInterval);
            clearTimeout(timeout);
            request.status = 'approved';
            resolve(request);
          }
        }
      });

      this.on('approval-rejected', (id) => {
        if (id === approvalId) {
          clearInterval(checkInterval);
          clearTimeout(timeout);
          request.status = 'rejected';
          reject(new Error('Deployment rejected'));
        }
      });
    });
  }

  async recordApproval(approvalId, approver, decision) {
    const request = this.pendingApprovals.get(approvalId);
    if (!request) throw new Error('Approval request not found');

    if (decision === 'approve') {
      request.approvals.push({
        approver,
        timestamp: new Date().toISOString(),
      });
      this.emit('approval-received', approvalId);
    } else {
      this.emit('approval-rejected', approvalId);
    }
  }
}

module.exports = { DeploymentApprovalWorkflow };
```

---

## Troubleshooting Guide

### Common Deployment Failures

| Failure Pattern | Symptom | Root Cause | Solution |
|---|---|---|---|
| **Container won't start** | Exit code 1 immediately | Missing env vars, bad config | Check `docker logs`, verify `.env` file |
| **Health check timeout** | 502/503 after deploy | App slow to initialize | Increase health check timeout, check DB connections |
| **OOM Killed** | Exit code 137 | Memory limit too low | Increase container memory limit, check for leaks |
| **Port conflict** | Bind error on startup | Port already in use | Check `lsof -i :3000`, kill conflicting process |
| **Permission denied** | EACCES errors | Wrong file permissions | Fix with `chown` and `chmod` |
| **DNS resolution failure** | Cannot connect to DB | Network misconfiguration | Check DNS settings, verify security groups |
| **Disk space full** | Write failures | Log rotation missing | Clean up, add log rotation |
| **Migration deadlock** | Deploy hangs | Concurrent migrations | Kill lock, run migrations serially |

### Quick Diagnostic Commands

```bash
#!/bin/bash
# diagnose.sh - Quick deployment diagnostics

echo "========== System Diagnostics =========="
echo "Memory:"
free -h
echo ""
echo "Disk:"
df -h /
echo ""
echo "CPU Load:"
uptime

echo ""
echo "========== Docker Diagnostics =========="
echo "Running containers:"
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
echo ""
echo "Recent container events:"
docker events --since "1h" --filter "type=container" --format "{{.Time}} {{.Action}} {{.Actor.Attributes.name}}" 2>/dev/null | tail -20

echo ""
echo "========== Application Diagnostics =========="
echo "Health endpoint:"
curl -s http://localhost:3000/health | jq . 2>/dev/null || echo "Health endpoint unavailable"

echo ""
echo "Node.js process info:"
pgrep -a node | head -5

echo ""
echo "Recent logs:"
docker logs --tail=50 node-app-current 2>&1 | tail -30

echo ""
echo "========== Network Diagnostics =========="
echo "Listening ports:"
ss -tlnp | grep -E '(3000|3306|5432|6379)'

echo ""
echo "DNS resolution:"
nslookup database.internal 2>/dev/null || echo "DNS lookup failed"
```

---

## Debugging Production Issues

### Node.js Inspector for Production Debugging

```javascript
// debug-helper.js - Production debugging utilities
const inspector = require('inspector');
const fs = require('fs');
const v8 = require('v8');

class ProductionDebugger {
  constructor() {
    this.session = null;
    this.isAttached = false;
  }

  // Attach inspector on-demand (requires --inspect flag at startup)
  attach(port = 9229) {
    if (this.isAttached) return;

    this.session = new inspector.Session();
    this.session.connect();

    this.session.post('Debugger.enable', () => {
      console.log(`Inspector attached on port ${port}`);
      this.isAttached = true;
    });
  }

  // Take heap snapshot for memory leak analysis
  async takeHeapSnapshot(filename) {
    const snapshotPath = filename || `heap-${Date.now()}.heapsnapshot`;

    return new Promise((resolve, reject) => {
      this.session.post('HeapProfiler.enable', () => {
        const ws = fs.createWriteStream(snapshotPath);

        this.session.on('HeapProfiler.addHeapSnapshotChunk', (message) => {
          ws.write(message.params.chunk);
        });

        this.session.post(
          'HeapProfiler.takeHeapSnapshot',
          { reportProgress: false },
          (err) => {
            ws.end();
            if (err) reject(err);
            else resolve(snapshotPath);
          }
        );
      });
    });
  }

  // CPU profiling
  async startCpuProfile() {
    return new Promise((resolve, reject) => {
      this.session.post('Profiler.enable', () => {
        this.session.post('Profiler.start', (err) => {
          if (err) reject(err);
          else resolve();
        });
      });
    });
  }

  async stopCpuProfile(filename) {
    const profilePath = filename || `cpu-${Date.now()}.cpuprofile`;

    return new Promise((resolve, reject) => {
      this.session.post('Profiler.stop', (err, { profile }) => {
        if (err) reject(err);
        else {
          fs.writeFileSync(profilePath, JSON.stringify(profile));
          resolve(profilePath);
        }
      });
    });
  }

  // Analyze event loop delay
  monitorEventLoop(intervalMs = 1000) {
    let lastTime = process.hrtime.bigint();

    setInterval(() => {
      const now = process.hrtime.bigint();
      const delay = Number(now - lastTime) / 1e6 - intervalMs;

      if (delay > 100) {
        console.warn(`Event loop delay: ${delay.toFixed(2)}ms`);
      }

      lastTime = process.hrtime.bigint();
    }, intervalMs);
  }

  // Get GC statistics
  monitorGC() {
    const obs = new PerformanceObserver((list) => {
      for (const entry of list.getEntries()) {
        console.log(`GC: ${entry.kind} - ${entry.duration.toFixed(2)}ms`);
      }
    });
    obs.observe({ entryTypes: ['gc'], buffered: false });
  }

  disconnect() {
    if (this.session) {
      this.session.disconnect();
      this.isAttached = false;
    }
  }
}

module.exports = { ProductionDebugger };
```

### Heap Snapshot Analysis Script

```javascript
// analyze-heap.js - Analyze heap snapshots for memory leaks
const fs = require('fs');
const v8 = require('v8');

function analyzeHeapSnapshot(snapshotPath) {
  console.log(`Analyzing heap snapshot: ${snapshotPath}`);

  // Parse snapshot (simplified analysis)
  const snapshot = JSON.parse(fs.readFileSync(snapshotPath, 'utf8'));

  const nodes = snapshot.nodes;
  const strings = snapshot.strings;
  const edges = snapshot.edges;

  // Count objects by type
  const typeCounts = {};
  const retainedSize = {};

  for (let i = 0; i < nodes.length; i += snapshot.snapshot.meta.node_fields.length) {
    const type = nodes[i];
    const name = strings[nodes[i + 1]] || 'unknown';
    const size = nodes[i + 2];

    typeCounts[name] = (typeCounts[name] || 0) + 1;
    retainedSize[name] = (retainedSize[name] || 0) + size;
  }

  // Sort by retained size
  const sortedBySize = Object.entries(retainedSize)
    .sort((a, b) => b[1] - a[1])
    .slice(0, 20);

  console.log('\nTop 20 objects by retained size:');
  console.log('Type'.padEnd(40), 'Count'.padStart(10), 'Size (MB)'.padStart(12));
  console.log('-'.repeat(62));

  for (const [type, size] of sortedBySize) {
    console.log(
      type.padEnd(40),
      String(typeCounts[type]).padStart(10),
      (size / 1024 / 1024).toFixed(2).padStart(12)
    );
  }

  // Detect common leak patterns
  console.log('\nPotential leak indicators:');

  if (typeCounts['(string)'] > 100000) {
    console.log(`WARNING: High string count (${typeCounts['(string)']}) - possible string leak`);
  }
  if (typeCounts['(array)'] > 50000) {
    console.log(`WARNING: High array count (${typeCounts['(array)']}) - possible array leak`);
  }

  const totalHeap = snapshot.snapshot.meta.node_fields.reduce((sum, _, i) => {
    return i % snapshot.snapshot.meta.node_fields.length === 2 ? sum + nodes[i] : sum;
  }, 0);

  console.log(`\nTotal heap size: ${(totalHeap / 1024 / 1024).toFixed(2)} MB`);
}

// Run if called directly
if (require.main === module) {
  const snapshotPath = process.argv[2];
  if (!snapshotPath) {
    console.error('Usage: node analyze-heap.js <snapshot.heapsnapshot>');
    process.exit(1);
  }
  analyzeHeapSnapshot(snapshotPath);
}
```

---

## Log Analysis for Deployment Issues

```bash
#!/bin/bash
# log-analysis.sh - Analyze deployment logs for issues
set -euo pipefail

LOG_DIR="/var/log/app"
DEPLOY_TIME="${1:-$(date -d '1 hour ago' +%Y-%m-%dT%H:%M:%S)}"

echo "========== Deployment Log Analysis =========="
echo "Analyzing logs since: ${DEPLOY_TIME}"

# Error frequency analysis
echo ""
echo "--- Error Frequency (last hour) ---"
for log in "${LOG_DIR}"/*.log; do
    if [ -f "$log" ]; then
        errors=$(grep -c '"level":"error"' "$log" 2>/dev/null || echo "0")
        echo "$(basename "$log"): ${errors} errors"
    fi
done

# Common error patterns
echo ""
echo "--- Top Error Patterns ---"
grep -h '"level":"error"' "${LOG_DIR}"/*.log 2>/dev/null | \
    jq -r '.message' 2>/dev/null | \
    sort | uniq -c | sort -rn | head -10

# Slow requests
echo ""
echo "--- Slow Requests (>1000ms) ---"
grep -h '"responseTime"' "${LOG_DIR}"/*.log 2>/dev/null | \
    jq -r 'select(.responseTime > 1000) | "\(.responseTime)ms \(.method) \(.url)"' 2>/dev/null | \
    sort -rn | head -10

# Memory usage over time
echo ""
echo "--- Memory Usage Trend ---"
grep -h '"heapUsed"' "${LOG_DIR}"/*.log 2>/dev/null | \
    jq -r '"\(.timestamp): \(.heapUsed / 1024 / 1024 | floor)MB"' 2>/dev/null | \
    tail -20

# Connection errors
echo ""
echo "--- Database/External Connection Errors ---"
grep -hE '"(ECONNREFUSED|ETIMEDOUT|ENOTFOUND)"' "${LOG_DIR}"/*.log 2>/dev/null | \
    jq -r '"\(.timestamp): \(.error)"' 2>/dev/null | tail -10

# Deployment timeline
echo ""
echo "--- Deployment Timeline ---"
grep -hE '"(deploy|migration|health_check)"' "${LOG_DIR}"/*.log 2>/dev/null | \
    jq -r '"\(.timestamp): [\(.event)] \(.message // "")"' 2>/dev/null

echo ""
echo "========== Analysis Complete =========="
```

### JavaScript Log Analyzer

```javascript
// log-analyzer.js - Structured log analysis for deployment issues
const fs = require('fs');
const readline = require('readline');

class DeploymentLogAnalyzer {
  constructor(logPath) {
    this.logPath = logPath;
    this.entries = [];
    this.errors = [];
    this.deployEvents = [];
  }

  async analyze() {
    const rl = readline.createInterface({
      input: fs.createReadStream(this.logPath),
      crlfDelay: Infinity,
    });

    for await (const line of rl) {
      try {
        const entry = JSON.parse(line);
        this.entries.push(entry);

        if (entry.level === 'error') {
          this.errors.push(entry);
        }

        if (entry.event?.includes('deploy')) {
          this.deployEvents.push(entry);
        }
      } catch {
        // Skip non-JSON lines
      }
    }

    return this.generateReport();
  }

  generateReport() {
    const report = {
      totalEntries: this.entries.length,
      timeRange: this.getTimeRange(),
      errors: this.analyzeErrors(),
      performance: this.analyzePerformance(),
      deployment: this.analyzeDeploymentEvents(),
      anomalies: this.detectAnomalies(),
    };

    return report;
  }

  analyzeErrors() {
    const errorGroups = {};

    for (const error of this.errors) {
      const key = error.message?.substring(0, 100) || 'unknown';
      errorGroups[key] = errorGroups[key] || { count: 0, firstSeen: null, lastSeen: null };
      errorGroups[key].count++;
      errorGroups[key].lastSeen = error.timestamp;
      if (!errorGroups[key].firstSeen) {
        errorGroups[key].firstSeen = error.timestamp;
      }
    }

    return Object.entries(errorGroups)
      .sort((a, b) => b[1].count - a[1].count)
      .slice(0, 10)
      .map(([message, data]) => ({ message, ...data }));
  }

  analyzePerformance() {
    const responseTimes = this.entries
      .filter((e) => e.responseTime)
      .map((e) => e.responseTime)
      .sort((a, b) => a - b);

    if (responseTimes.length === 0) return null;

    return {
      p50: responseTimes[Math.floor(responseTimes.length * 0.5)],
      p95: responseTimes[Math.floor(responseTimes.length * 0.95)],
      p99: responseTimes[Math.floor(responseTimes.length * 0.99)],
      max: responseTimes[responseTimes.length - 1],
      avg: responseTimes.reduce((a, b) => a + b, 0) / responseTimes.length,
    };
  }

  detectAnomalies() {
    const anomalies = [];

    // Memory spike detection
    const memoryEntries = this.entries.filter((e) => e.heapUsed);
    for (let i = 1; i < memoryEntries.length; i++) {
      const increase = memoryEntries[i].heapUsed - memoryEntries[i - 1].heapUsed;
      if (increase > 100 * 1024 * 1024) {
        // 100MB spike
        anomalies.push({
          type: 'memory_spike',
          timestamp: memoryEntries[i].timestamp,
          details: `Memory increased by ${(increase / 1024 / 1024).toFixed(2)}MB`,
        });
      }
    }

    // Error rate spike
    const errorWindow = 60000; // 1 minute windows
    let windowStart = 0;
    let windowErrors = 0;

    for (const entry of this.entries) {
      const time = new Date(entry.timestamp).getTime();
      if (time - windowStart > errorWindow) {
        if (windowErrors > 10) {
          anomalies.push({
            type: 'error_spike',
            timestamp: new Date(windowStart).toISOString(),
            details: `${windowErrors} errors in 1 minute window`,
          });
        }
        windowStart = time;
        windowErrors = 0;
      }
      if (entry.level === 'error') windowErrors++;
    }

    return anomalies;
  }

  getTimeRange() {
    if (this.entries.length === 0) return null;
    return {
      start: this.entries[0].timestamp,
      end: this.entries[this.entries.length - 1].timestamp,
    };
  }

  analyzeDeploymentEvents() {
    return {
      totalEvents: this.deployEvents.length,
      events: this.deployEvents.map((e) => ({
        timestamp: e.timestamp,
        event: e.event,
        message: e.message,
      })),
    };
  }
}

module.exports = { DeploymentLogAnalyzer };
```

---

## Troubleshooting Decision Trees

### Deployment Failure Decision Tree

```
Deployment Failed
│
├─ Container won't start?
│  ├─ Exit code 137 → OOM killed
│  │  └─ Solution: Increase memory limit, check for memory leaks
│  ├─ Exit code 1 → Application error
│  │  ├─ Check docker logs for stack trace
│  │  ├─ Missing env vars → Verify .env file
│  │  └─ Module not found → Check node_modules
│  └─ Exit code 0 but unhealthy
│     ├─ Health endpoint missing → Add /health route
│     ├─ Slow startup → Increase health check timeout
│     └─ DB connection failed → Verify DB_URL, check network
│
├─ Health check failing?
│  ├─ HTTP 502/503 → App not listening
│  │  ├─ Port mismatch → Verify PORT env var
│  │  └─ Crash loop → Check container restart count
│  ├─ HTTP 500 → App error
│  │  ├─ Check application logs
│  │  └─ Check dependency health (DB, cache, queue)
│  └─ Timeout → App overloaded
│     ├─ Check CPU/memory usage
│     └─ Check event loop delay
│
├─ Deployment timeout?
│  ├─ Image pull slow → Use local registry mirror
│  ├─ Migration stuck → Check DB locks
│  └─ Health check timeout → See above
│
└─ Rollback failed?
   ├─ Previous version unavailable → Keep N versions locally
   ├─ DB incompatible → Manual migration rollback needed
   └─ Cascading failure → Full incident response required
```

### Performance Degradation Decision Tree

```
Performance Issue Post-Deploy
│
├─ Response time increased?
│  ├─ P95 > threshold?
│  │  ├─ Check slow query logs
│  │  ├─ Check connection pool exhaustion
│  │  └─ Profile CPU usage
│  └─ All requests slow?
│     ├─ Event loop blocked → Check sync operations
│     ├─ Memory pressure → Check heap usage
│     └─ GC thrashing → Check GC frequency
│
├─ Memory usage increasing?
│  ├─ Linear growth → Memory leak
│  │  ├─ Take heap snapshot
│  │  ├─ Compare snapshots over time
│  │  └─ Check for unclosed resources
│  └─ Sudden spike → Large allocation
│     ├─ Check recent data loads
│     └─ Check cache size limits
│
├─ CPU usage high?
│  ├─ Single core maxed → Check event loop
│  ├─ All cores busy → Check worker threads
│  └─ Spiky → Check request patterns
│
└─ Error rate increased?
   ├─ 5xx errors → Application errors
   │  ├─ Check error logs for patterns
   │  └─ Check dependency availability
   ├─ 4xx errors → Client errors
   │  ├─ Check API contract changes
   │  └─ Check authentication/authorization
   └─ Connection errors → Network issues
      ├─ Check DNS resolution
      ├─ Check firewall rules
      └─ Check load balancer config
```

---

## Cross-References

### Related Sections in This Chapter

| Section | Topic | Relationship |
|---|---|---|
| [01 - Deployment Checklist](./01-deployment-checklist.md) | Pre-deployment verification | Use checklist before running automation scripts |
| [03 - Capacity & Cost](./03-capacity-cost-improvement.md) | Capacity planning | Troubleshoot capacity-related failures |

### Related Chapters

| Chapter | Topic | Relevance |
|---|---|---|
| [06 - Databases & Performance](../../06-databases-performance/) | Database optimization | Database migration patterns |
| [09 - Testing](../../09-testing/) | Testing strategies | Pre-deployment test automation |
| [21 - Logging & Monitoring](../../21-logging-monitoring/) | Observability | Log analysis and monitoring |
| [26 - CI/CD GitHub Actions](../../26-cicd-github-actions/) | CI/CD pipelines | Integrating automation into pipelines |
| [33 - Observability](../../33-observability-monitoring/) | Production monitoring | Debugging production issues |

### External Resources

- [Node.js Debugging Guide](https://nodejs.org/en/docs/guides/debugging-getting-started)
- [Docker Deployment Best Practices](https://docs.docker.com/develop/develop-images/dockerfile_best-practices)
- [Database Migration Patterns](https://martinfowler.com/articles/evodb.html)
- [Incident Response Best Practices](https://response.pagerduty.com/)

---

*Last updated: 2026-04-01*
