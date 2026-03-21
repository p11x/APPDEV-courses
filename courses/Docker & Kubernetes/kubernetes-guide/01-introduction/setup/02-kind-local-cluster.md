# Kind Local Cluster

## Overview

Kind (Kubernetes in Docker) creates Kubernetes clusters using Docker containers as nodes. It's faster than Minikube and excellent for CI/CD pipelines and local development. Kind uses a simple configuration file to define cluster topology.

## Prerequisites

- Docker installed and running
- kubectl installed
- Kind installed

## Installation

```bash
# Install kind (Linux)
curl -Lo kind https://kind.sigs.k8s.io/dl/v0.20.0/kind-linux-amd64
chmod +x kind
sudo mv kind /usr/local/bin/

# macOS
brew install kind

# Windows
choco install kind
```

## Creating a Cluster

```bash
# Create simple cluster
kind create cluster

# Create with custom name
kind create cluster --name my-cluster

# Create from config
kind create cluster --config kind-config.yaml
```

## Config File Example

```yaml
# kind-config.yaml
kind: Cluster
apiVersion: kind.x-k8s.io/v1alpha4
nodes:
- role: control-plane
- role: worker
- role: worker
```

## Common Mistakes

- **Docker not running**: Kind requires Docker to be running.
- **Port conflicts**: Ensure ports aren't in use.

## What's Next

Continue to [Kubectl Install and Config](./03-kubectl-install-and-config.md)
