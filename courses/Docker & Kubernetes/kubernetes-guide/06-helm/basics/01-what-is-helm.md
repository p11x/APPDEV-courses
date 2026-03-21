# What is Helm

## Overview

Helm is Kubernetes' package manager. It helps define, install, and upgrade complex Kubernetes applications using charts - packages of pre-configured Kubernetes resources.

## Prerequisites

- Kubernetes cluster access
- kubectl basics

## Core Concepts

### Why Helm

Managing Kubernetes applications manually is complex:

- Multiple YAML files to manage
- Configuration across environments
- Upgrades and rollbacks

### Helm Concepts

| Concept | Description |
|---------|-------------|
| Chart | Package of Kubernetes manifests |
| Repository | Chart storage location |
| Release | Deployed chart instance |
| Values | Configuration overrides |

## Quick Reference

| Command | Description |
|---------|-------------|
| helm search | Find charts |
| helm install | Deploy chart |
| helm upgrade | Update release |
| helm rollback | Revert changes |

## What's Next

Continue to [Installing Helm](./02-installing-helm.md) for setup.
