# Kubectl Install and Config

## Overview

Kubectl is the command-line tool for interacting with Kubernetes clusters. It allows you to deploy applications, inspect resources, manage cluster settings, and view logs. This guide covers installing kubectl and configuring it to work with your clusters.

## Prerequisites

- Access to a Kubernetes cluster
- Command-line terminal

## Installation

```bash
# Linux
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl"
chmod +x kubectl
sudo mv kubectl /usr/local/bin/

# macOS
brew install kubectl

# Windows
choco install kubernetes-cli
```

## Configuration

```bash
# Check configuration
kubectl config view

# List clusters
kubectl config get-contexts

# Switch context
kubectl config use-context context-name

# Create cluster role binding (for admin access)
kubectl create clusterrolebinding cluster-admin-binding \
  --clusterrole=cluster-admin \
  --user=your-email@example.com
```

## Common Commands

```bash
# Get nodes
kubectl get nodes

# Get pods
kubectl get pods -A

# Get services
kubectl get svc -A

# Apply configuration
kubectl apply -f deployment.yaml

# Delete resources
kubectl delete -f deployment.yaml
```

## What's Next

Continue to [Pod Basics](../workloads/pods/01-pod-basics.md)
