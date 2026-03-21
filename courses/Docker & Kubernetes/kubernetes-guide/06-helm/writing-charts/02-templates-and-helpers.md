# Templates and Helpers

## Overview

Helm templates use Go templating to generate Kubernetes manifests. This guide covers template syntax and helper functions.

## Prerequisites

- Go template basics
- Chart structure knowledge

## Core Concepts

### Template Syntax

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: {{ .Release.Name }}
spec:
  containers:
  - name: {{ .Chart.Name }}
    image: {{ .Values.image.repository }}:{{ .Values.image.tag }}
```

### Helper Functions

```yaml
{{- define "myhelper" }}
name: {{ .Chart.Name }}
{{- end }}
```

## Quick Reference

| Function | Description |
|----------|-------------|
| {{ .Values.x }} | Access values |
| {{ .Release.Name }} | Release name |
| {{- include }} | Include template |

## What's Next

Continue to [Chart Testing](./03-chart-testing.md) for quality assurance.
