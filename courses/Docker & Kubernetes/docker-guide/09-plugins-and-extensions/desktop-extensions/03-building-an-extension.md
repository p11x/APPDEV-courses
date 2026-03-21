# Building an Extension

## Overview

Docker Desktop extensions allow you to add custom functionality. This guide covers the basics of creating a simple extension.

## Prerequisites

- Docker Desktop 4.0+
- Node.js for UI
- Docker for building

## Core Concepts

### Extension Structure

```
my-extension/
├── metadata.json
├── ui/          # React/Vue frontend
├── backend/     # Docker Compose services
└── Dockerfile
```

### metadata.json

```json
{
  "name": "my-extension",
  "description": "My custom extension",
  "icon": "icon.svg",
  "vm": "docker",
  "api": {
    "Docker": {
      "DesktopACL": ["*"]
    }
  }
}
```

## Quick Reference

This is a conceptual overview. Production extensions require significant development.

## What's Next

Continue to [Microservices Architecture](../../10-real-world-projects/microservices/01-microservices-architecture.md) for production patterns.
