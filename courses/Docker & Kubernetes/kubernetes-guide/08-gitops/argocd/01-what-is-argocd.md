# What is ArgoCD

## Overview

ArgoCD is a declarative, GitOps continuous delivery tool for Kubernetes. It automatically ensures applications match the desired state in Git.

## Prerequisites

- Kubernetes basics
- GitOps concept understanding

## Core Concepts

### GitOps Model

1. Store desired state in Git
2. ArgoCD monitors Git repository
3. Automatically syncs cluster to match Git

### Key Features

- **Declarative**: Define app as code
- **Automatic sync**: Monitors Git changes
- **Rollback**: Easy reversion
- **Multi-tenancy**: Built-in RBAC

## Quick Reference

| Feature | Description |
|---------|-------------|
| GitOps | Git as source of truth |
| Auto-sync | Automatic reconciliation |
| Rollback | Git-based revert |

## What's Next

Continue to [Installing ArgoCD](./02-installing-argocd.md) for setup.
