# Minikube

## Overview

Minikube is a tool that lets you run a single-node Kubernetes cluster locally on your computer. It's perfect for learning Kubernetes and developing applications locally before deploying to production clusters. Minikube creates a virtual machine and runs all Kubernetes components inside it.

## Prerequisites

- Virtualization support (VT-x/AMD-V)
- 2GB+ RAM available
- 20GB+ disk space
- kubectl installed

## Installation

```bash
# Install minikube (Linux)
curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64
sudo install minikube-linux-amd64 /usr/local/bin/minikube

# macOS
brew install minikube

# Windows
choco install minikube
```

## Usage

```bash
# Start cluster
minikube start

# Check status
minikube status

# Access dashboard
minikube dashboard

# Stop cluster
minikube stop

# Delete cluster
minikube delete
```

## Common Mistakes

- **Not enough resources**: Ensure sufficient RAM and CPU.
- **Using default driver**: Choose appropriate driver for your OS.

## What's Next

Continue to [Kind Local Cluster](./02-kind-local-cluster.md)
