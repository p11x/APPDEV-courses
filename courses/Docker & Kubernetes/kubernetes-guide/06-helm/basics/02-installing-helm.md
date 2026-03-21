# Installing Helm

## Overview

This guide covers installing Helm 3 on various platforms and verifying the installation.

## Prerequisites

- Kubernetes cluster
- kubectl configured

## Step-by-Step Examples

### Install on Linux

```bash
# Using script
curl -fsSL https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 | bash

# Or using package manager
# apt: apt-get install helm
# yum: yum install helm
```

### Install on macOS

```bash
# Using Homebrew
brew install helm
```

### Install on Windows

```bash
# Using winget
winget install Helm.Helm

# Or Chocolatey
choco install kubernetes-helm
```

### Verify Installation

```bash
# Check version
helm version
# Output: version.BuildInfo{Version:"v3.15.0"}

# Add repository
helm repo add stable https://charts.helm.sh/stable
helm repo update
```

## Quick Reference

| Command | Description |
|---------|-------------|
| helm version | Check version |
| helm repo add | Add repo |
| helm repo update | Update repos |

## What's Next

Continue to [Helm Concepts](./03-helm-concepts.md) for understanding Helm.
