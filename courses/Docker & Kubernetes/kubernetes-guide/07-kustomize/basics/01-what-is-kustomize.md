# What is Kustomize

## Overview

Kustomize is a Kubernetes-native configuration management tool that uses overlays instead of templates. Built into kubectl, it provides a declarative approach to customizing manifests.

## Prerequisites

- kubectl basics

## Core Concepts

### Kustomize vs Helm

| Aspect | Helm | Kustomize |
|--------|------|-----------|
| Templating | Go templates | Patch-based |
| Values | values.yaml | Patches |
| Built-in | No | Yes (kubectl -k) |

## Quick Reference

| Command | Description |
|---------|-------------|
| kubectl kustomize | Build manifests |
| kubectl apply -k | Apply with kustomize |

## What's Next

Continue to [Kustomization File](./02-kustomization-file.md) for configuration.
