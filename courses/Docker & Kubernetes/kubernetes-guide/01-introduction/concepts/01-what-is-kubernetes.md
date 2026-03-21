# What is Kubernetes

## Overview

Kubernetes (also known as K8s) is an open-source container orchestration platform that automates the deployment, scaling, and management of containerized applications. Originally developed by Google, it has become the industry standard for running containers in production. Kubernetes provides powerful abstractions for managing complex distributed systems, enabling features like self-healing, automatic rollouts, and service discovery.

## Prerequisites

- Understanding of containers (Docker)
- Basic command-line knowledge
- Familiarity with deployment concepts

## Core Concepts

### What is Container Orchestration?

Container orchestration handles:
- **Scheduling**: Placing containers on appropriate nodes
- **Scaling**: Adding/removing container instances
- **Health monitoring**: Detecting and replacing failed containers
- **Service discovery**: Finding services automatically
- **Load balancing**: Distributing traffic across replicas
- **Rolling updates**: Deploying new versions without downtime

### Why Kubernetes?

Before Kubernetes, teams managed containers manually or with simpler tools. Kubernetes provides:
- **Declarative configuration**: Define desired state, not steps
- **Self-healing**: Automatically replaces failed containers
- **Auto-scaling**: Scale based on metrics
- **Portability**: Works on any Kubernetes cluster (cloud, on-prem)
- **Ecosystem**: Large ecosystem of tools and integrations

### Kubernetes Architecture

```
┌─────────────────────────────────────────────┐
│                 Control Plane               │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐  │
│  │   API    │ │ Scheduler│ │ Controller│  │
│  │  Server  │ │          │ │  Manager  │  │
│  └──────────┘ └──────────┘ └──────────┘  │
│  ┌──────────┐                                │
│  │   etcd   │                                │
│  └──────────┘                                │
└─────────────────────────────────────────────┘
              │              │
┌─────────────┴─────────────┴─────────────┐
│                  Nodes                    │
│  ┌──────────────────────────────────┐   │
│  │  kubelet  │  kube-proxy │ runtime│   │
│  └──────────────────────────────────┘   │
│  ┌──────┐ ┌──────┐ ┌──────┐           │
│  │ Pod  │ │ Pod  │ │ Pod  │           │
│  └──────┘ └──────┘ └──────┘           │
└──────────────────────────────────────────┘
```

## Common Mistakes

- **Confusing Kubernetes with Docker**: Docker runs containers, Kubernetes orchestrates them.
- **Overcomplicating**: Start simple, add complexity as needed.
- **Ignoring the learning curve**: Kubernetes has significant complexity.

## Quick Reference

| Term | Description |
|------|-------------|
| Pod | Smallest deployable unit |
| Node | Worker machine |
| Cluster | Collection of nodes |
| Control Plane | Management components |

## What's Next

Continue to [Control Plane and Nodes](./02-control-plane-and-nodes.md)
