# Falco Runtime Security

## Overview

Falco is a cloud-native runtime security project that detects anomalous activity in containers and Kubernetes. It monitors system calls and Kubernetes audit events for security threats.

## Prerequisites

- Kubernetes cluster
- Helm knowledge (for installation)

## Core Concepts

### How Falco Works

1. **Kernel module/eBPF**: Captures system calls
2. **Rules engine**: Evaluates events against rules
3. **Alerting**: Sends alerts to various outputs

### Rule Types

| Type | Description |
|------|-------------|
| File rules | File operations (open, read, write) |
| Network rules | Network connections |
| Process rules | Process creation/execution |
| Kubernetes rules | K8s API activity |

## Step-by-Step Examples

### Install Falco with Helm

```bash
# Add Falco repository
helm repo add falcosecurity https://falcosecurity.github.io/charts

# Install Falco
helm install falco falcosecurity/falco \
  --namespace falco \
  --create-namespace \
  --set driver.kind=ebpf
```

### Basic Falco Rule

```yaml
# falco-rules.yaml
- rule: Detect shell in container
  desc: A shell was spawned in a container
  condition: >
    container.id != host and
    proc.name = bash
  output: "Shell detected in container (user=%user.name container=%container.name)"
  priority: WARNING
  tags: [container, shell]
```

### Falco Configuration

```yaml
# falco.yaml
rules:
  - falco_rules.yaml
  - falco_rules.local.yaml
  - k8s_audit_rules.yaml

syscall_event_drops:
  actions:
    - log
    - alert
  threshold: 1

outputs:
  syslog:
    enabled: true
  stdout:
    enabled: true
```

### Falco with FalcoSidekick

```bash
# Install Falco with FalcoSidekick
helm install falco falcosecurity/falco \
  --set falcosidekick.enabled=true \
  --set falcosidekick.webui.enabled=true
```

## Gotchas for Docker Users

- **No Docker equivalent**: Runtime security monitoring is Kubernetes-native
- **Kernel access**: Requires kernel module or eBPF
- **Performance**: Can impact host performance

## Common Mistakes

- **Too many rules**: Alert fatigue
- **Missing rules**: Gaps in coverage
- **Resource usage**: High CPU in production

## Quick Reference

| Alert Priority | Description |
|---------------|-------------|
| EMERGENCY | Critical security issue |
| ALERT | Immediate action needed |
| CRITICAL | High severity |
| ERROR | Error condition |
| WARNING | Warning condition |
| NOTICE | Notable event |
| INFO | Informational |

## What's Next

Continue to [Multi-Tenancy](./../../10-platform-engineering/multi-tenancy/01-namespace-isolation.md) for platform engineering.
