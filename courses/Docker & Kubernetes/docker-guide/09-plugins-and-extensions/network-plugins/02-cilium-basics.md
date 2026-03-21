# Cilium Basics

## Overview

Cilium is a networking and security plugin that uses eBPF (extended Berkeley Packet Filter) for high-performance networking and security. This guide covers Cilium concepts and capabilities.

## Prerequisites

- Docker Engine 20.10+
- Understanding of eBPF

## Core Concepts

### eBPF Revolution

eBPF allows running code in the Linux kernel without modifying kernel code:

- Faster than iptables
- Dynamic network policies
- Transparent observability
- No overlay overhead

### Cilium Features

- Layer 7 policy enforcement
- Service mesh capabilities (Hubble)
- Encryption (WireGuard/IPsec)
- Multi-cluster support

## Gotchas for Docker Users

- **Kubernetes-first**: Primarily designed for Kubernetes
- **Standalone Docker**: Limited support
- **eBPF requirement**: Requires recent kernel

## Quick Reference

| Feature | Description |
|---------|-------------|
| eBPF | Kernel-level packet filtering |
| Hubble | Observability UI |
| Tetragon | Runtime security |

## What's Next

Continue to [Plugin Troubleshooting](./03-plugin-troubleshooting.md) for debugging.
