# Control Plane and Nodes

## Overview

Understanding the Kubernetes architecture is essential for debugging and managing clusters. Kubernetes consists of a control plane (master) that manages the cluster and worker nodes that run your applications. This guide explains each component and how they work together.

## Prerequisites

- Basic Kubernetes knowledge
- Understanding of distributed systems

## Core Concepts

### Control Plane Components

The control plane manages the cluster:

1. **kube-apiserver**: REST API for all cluster operations
2. **etcd**: Distributed database storing cluster state
3. **kube-scheduler**: Places pods on nodes
4. **kube-controller-manager**: Runs controllers
5. **cloud-controller-manager**: Integrates with cloud providers

### Node Components

Nodes run your workloads:

1. **kubelet**: Agent communicating with API server
2. **kube-proxy**: Network proxy for services
3. **container runtime**: Docker, containerd, or CRI-O

## Quick Reference

| Component | Type | Purpose |
|-----------|------|---------|
| API Server | Control | REST API |
| etcd | Control | Data store |
| Scheduler | Control | Pod placement |
| kubelet | Node | Pod execution |
| kube-proxy | Node | Networking |

## What's Next

Continue to [Kubernetes vs Docker Swarm](./03-kubernetes-vs-docker-swarm.md)
