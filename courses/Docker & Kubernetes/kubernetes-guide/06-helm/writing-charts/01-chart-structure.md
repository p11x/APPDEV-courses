# Chart Structure

## Overview

This guide covers the structure of a Helm chart and its required files.

## Prerequisites

- Helm basics

## Core Concepts

### Chart Structure

```
mychart/
├── Chart.yaml          # Metadata
├── values.yaml         # Default values
├── templates/          # Kubernetes manifests
├── templates/NOTES.txt
└── charts/             # Dependencies
```

### Chart.yaml

```yaml
apiVersion: v2
name: mychart
description: My Kubernetes application
version: 1.0.0
appVersion: "1.0"
```

## Quick Reference

| File | Purpose |
|------|---------|
| Chart.yaml | Chart metadata |
| values.yaml | Default config |
| templates/ | K8s manifests |

## What's Next

Continue to [Templates and Helpers](./02-templates-and-helpers.md) for templating.
